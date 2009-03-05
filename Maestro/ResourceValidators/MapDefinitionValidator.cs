using System;
using System.Collections.Generic;
using System.Text;

namespace OSGeo.MapGuide.Maestro.ResourceValidators
{
    public class MapDefinitionValidator : IValidator
    {
        #region IValidator Members

        public ValidationIssue[] Validate(object resource, bool recurse)
        {
            if (resource as MaestroAPI.MapDefinition == null)
                return null;

            List<ValidationIssue> issues = new List<ValidationIssue>();

            MaestroAPI.MapDefinition mdef = resource as MaestroAPI.MapDefinition;

            foreach(MaestroAPI.MapLayerGroupType g in mdef.LayerGroups)
                if (g.ShowInLegend && g.LegendLabel.Trim().Length == 0)
                    issues.Add(new ValidationIssue(mdef, ValidationStatus.Information, string.Format("Group {0} does not have a legend label", g.Name)));
                else if (g.ShowInLegend && g.LegendLabel.Trim().ToLower() == "layer group")
                    issues.Add(new ValidationIssue(mdef, ValidationStatus.Information, string.Format("Group {0} has the default legend label", g.Name)));

            List<MaestroAPI.BaseMapLayerType> layers = new List<OSGeo.MapGuide.MaestroAPI.BaseMapLayerType>();
            foreach(MaestroAPI.BaseMapLayerType l in mdef.Layers)
                layers.Add(l);

            if (mdef.BaseMapDefinition != null && mdef.BaseMapDefinition.BaseMapLayerGroup != null)
                foreach(MaestroAPI.BaseMapLayerGroupCommonType g in mdef.BaseMapDefinition.BaseMapLayerGroup)
                    foreach(MaestroAPI.BaseMapLayerType l in g.BaseMapLayer)
                        layers.Add(l);

            foreach (MaestroAPI.BaseMapLayerType l in layers)
            {
                if (l.ShowInLegend && l.LegendLabel.Trim().Length == 0)
                    issues.Add(new ValidationIssue(mdef, ValidationStatus.Information, string.Format("Layer {0} does not have a legend label", l.Name)));
                
                if (recurse)
                {
                    Topology.Geometries.Envelope mapEnv = new Topology.Geometries.Envelope(mdef.Extents.MinX, mdef.Extents.MaxX, mdef.Extents.MinY, mdef.Extents.MaxY);

                    try
                    {
                        MaestroAPI.LayerDefinition ldef = mdef.CurrentConnection.GetLayerDefinition(l.ResourceId);

                        issues.AddRange(Validation.Validate(ldef, true));

                        string schema = null;
                        string geom = null;
                        if (ldef.Item is MaestroAPI.VectorLayerDefinitionType)
                        {
                            schema = (ldef.Item as MaestroAPI.VectorLayerDefinitionType).FeatureName;
                            geom = (ldef.Item as MaestroAPI.VectorLayerDefinitionType).Geometry;

                            if (string.IsNullOrEmpty(schema))
                                issues.Add(new ValidationIssue(ldef, ValidationStatus.Error, string.Format("Layer {0} has an invalid Schema", l.ResourceId)));

                            if (string.IsNullOrEmpty(geom))
                                issues.Add(new ValidationIssue(ldef, ValidationStatus.Error, string.Format("Layer {0} has no geometry column", l.ResourceId)));
                        }
                        else if (ldef.Item is MaestroAPI.GridLayerDefinitionType)
                        {
                            schema = (ldef.Item as MaestroAPI.GridLayerDefinitionType).FeatureName;
                        }
                        else
                            issues.Add(new ValidationIssue(ldef, ValidationStatus.Warning, string.Format("Layer {0} is a type that is unsupported by Maestro", l.ResourceId)));


                        if (schema != null)
                        {
                            try
                            {
                                MaestroAPI.FeatureSource fs = mdef.CurrentConnection.GetFeatureSource(ldef.Item.ResourceId);
                                //The layer recurses on the FeatureSource
                                //issues.AddRange(Validation.Validate(fs, true));

                                try
                                {
                                    MaestroAPI.FdoSpatialContextList context = fs.GetSpatialInfo();
                                    if (context.SpatialContext == null || context.SpatialContext.Count == 0)
                                        issues.Add(new ValidationIssue(fs, ValidationStatus.Warning, string.Format("{0} has no spatial context (eg. no coordinate system)", fs.ResourceId)));
                                    else
                                    {
                                        if (context.SpatialContext.Count > 1)
                                            issues.Add(new ValidationIssue(fs, ValidationStatus.Information, string.Format("{0} has more than one spatial context, the following test results may be inacurate", fs.ResourceId)));


                                        bool skipGeomCheck = false;

                                        //TODO: Switch to the correct version (2.1), once released
                                        if (context.SpatialContext[0].CoordinateSystemWkt != mdef.CoordinateSystem)
                                        {
                                            if (ldef.Item is MaestroAPI.GridLayerDefinitionType && mdef.CurrentConnection.SiteVersion <= MaestroAPI.SiteVersions.GetVersion(OSGeo.MapGuide.MaestroAPI.KnownSiteVersions.MapGuideOS2_0_2))
                                                issues.Add(new ValidationIssue(fs, ValidationStatus.Error, string.Format("{0} is a raster layer, and in another coordinate system than the map. No data will be displayed by the layer", fs.ResourceId)));
                                            else
                                                issues.Add(new ValidationIssue(fs, ValidationStatus.Warning, string.Format("{0} has a different coordinate system than the map, this will impact performance as the coordinates are transformed while rendering the map. Maestro cannot validate the extent of the data.", fs.ResourceId)));

                                            skipGeomCheck = true;
                                        }


                                        if (geom != null && !skipGeomCheck)
                                        {
                                            Topology.Geometries.IEnvelope env = fs.GetSpatialExtent(schema, geom);
                                            if (!env.Intersects(mapEnv))
                                                issues.Add(new ValidationIssue(fs, ValidationStatus.Warning, string.Format("Data from {0} is not visible in the map's start extent", fs.ResourceId)));
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    issues.Add(new ValidationIssue(ldef, ValidationStatus.Error, string.Format("{0} could not be processed for spatial info: {1}", fs.ResourceId, ex.Message)));
                                }
                            }
                            catch (Exception ex)
                            {
                                issues.Add(new ValidationIssue(mdef, ValidationStatus.Error, string.Format("Layer {0}'s FeatureSource could not be read: {1}", l.ResourceId, ex.Message)));
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        issues.Add(new ValidationIssue(mdef, ValidationStatus.Error, string.Format("Layer {0} could not be read: {1}", l.ResourceId, ex.Message)));
                    }
                }
            }

            return issues.ToArray();
        }

        #endregion
    }
}
