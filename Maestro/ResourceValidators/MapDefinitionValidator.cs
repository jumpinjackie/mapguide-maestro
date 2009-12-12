#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
#endregion
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
                if (g.ShowInLegend && (g.LegendLabel == null || g.LegendLabel.Trim().Length == 0))
                    issues.Add(new ValidationIssue(mdef, ValidationStatus.Information, string.Format(Strings.MapDefinitionValidator.GroupMissingLabelInformation, g.Name)));
                else if (g.ShowInLegend && g.LegendLabel.Trim().ToLower() == "layer group")
                    issues.Add(new ValidationIssue(mdef, ValidationStatus.Information, string.Format(Strings.MapDefinitionValidator.GroupHasDefaultLabelInformation, g.Name)));

            List<MaestroAPI.BaseMapLayerType> layers = new List<OSGeo.MapGuide.MaestroAPI.BaseMapLayerType>();
            foreach(MaestroAPI.BaseMapLayerType l in mdef.Layers)
                layers.Add(l);

            if (mdef.BaseMapDefinition != null && mdef.BaseMapDefinition.BaseMapLayerGroup != null)
                foreach(MaestroAPI.BaseMapLayerGroupCommonType g in mdef.BaseMapDefinition.BaseMapLayerGroup)
                    foreach(MaestroAPI.BaseMapLayerType l in g.BaseMapLayer)
                        layers.Add(l);

            Dictionary<string, MaestroAPI.BaseMapLayerType> nameCounter = new Dictionary<string, OSGeo.MapGuide.MaestroAPI.BaseMapLayerType>();

            foreach (MaestroAPI.BaseMapLayerType l in layers)
            {
                if (nameCounter.ContainsKey(l.Name))
                    issues.Add(new ValidationIssue(mdef, ValidationStatus.Warning, string.Format(Strings.MapDefinitionValidator.LayerNameDuplicateWarning, l.Name, l.ResourceId, nameCounter[l.Name].ResourceId)));
                else
                    nameCounter.Add(l.Name, l);
 
                if (l.ShowInLegend && (string.IsNullOrEmpty(l.LegendLabel) || l.LegendLabel.Trim().Length == 0))
                    issues.Add(new ValidationIssue(mdef, ValidationStatus.Information, string.Format(Strings.MapDefinitionValidator.LayerMissingLabelInformation, l.Name)));
                
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
                                issues.Add(new ValidationIssue(ldef, ValidationStatus.Error, string.Format(Strings.MapDefinitionValidator.MissingLayerSchemaError, l.ResourceId)));

                            if (string.IsNullOrEmpty(geom))
                                issues.Add(new ValidationIssue(ldef, ValidationStatus.Error, string.Format(Strings.MapDefinitionValidator.MissingLayerGeometryColumnError, l.ResourceId)));
                        }
                        else if (ldef.Item is MaestroAPI.GridLayerDefinitionType)
                        {
                            schema = (ldef.Item as MaestroAPI.GridLayerDefinitionType).FeatureName;
                            geom = (ldef.Item as MaestroAPI.GridLayerDefinitionType).Geometry;

                            if (string.IsNullOrEmpty(schema))
                                issues.Add(new ValidationIssue(ldef, ValidationStatus.Error, string.Format(Strings.MapDefinitionValidator.MissingLayerSchemaError, l.ResourceId)));

                            if (string.IsNullOrEmpty(geom))
                                issues.Add(new ValidationIssue(ldef, ValidationStatus.Error, string.Format(Strings.MapDefinitionValidator.MissingLayerGeometryColumnError, l.ResourceId)));
                        }
                        else
                            issues.Add(new ValidationIssue(ldef, ValidationStatus.Warning, string.Format(Strings.MapDefinitionValidator.UnsupportedLayerTypeWarning, l.ResourceId)));


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
                                        issues.Add(new ValidationIssue(fs, ValidationStatus.Warning, string.Format(Strings.MapDefinitionValidator.MissingSpatialContextWarning, fs.ResourceId)));
                                    else
                                    {
                                        if (context.SpatialContext.Count > 1)
                                            issues.Add(new ValidationIssue(fs, ValidationStatus.Information, string.Format(Strings.MapDefinitionValidator.MultipleSpatialContextsInformation, fs.ResourceId)));


                                        bool skipGeomCheck = false;

                                        //TODO: Switch to the correct version (2.1), once released
                                        if (context.SpatialContext[0].CoordinateSystemWkt != mdef.CoordinateSystem)
                                        {
                                            if (ldef.Item is MaestroAPI.GridLayerDefinitionType && mdef.CurrentConnection.SiteVersion <= MaestroAPI.SiteVersions.GetVersion(OSGeo.MapGuide.MaestroAPI.KnownSiteVersions.MapGuideOS2_0_2))
                                                issues.Add(new ValidationIssue(fs, ValidationStatus.Error, string.Format(Strings.MapDefinitionValidator.RasterReprojectionError, fs.ResourceId)));
                                            else
                                                issues.Add(new ValidationIssue(fs, ValidationStatus.Warning, string.Format(Strings.MapDefinitionValidator.DataReprojectionWarning, fs.ResourceId)));

                                            skipGeomCheck = true;
                                        }


                                        if (geom != null && !skipGeomCheck)
                                        {
                                            Topology.Geometries.IEnvelope env = fs.GetSpatialExtent(schema, geom);
                                            if (!env.Intersects(mapEnv))
                                                issues.Add(new ValidationIssue(fs, ValidationStatus.Warning, string.Format(Strings.MapDefinitionValidator.DataOutsideMapWarning, fs.ResourceId)));
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    issues.Add(new ValidationIssue(ldef, ValidationStatus.Error, string.Format(Strings.MapDefinitionValidator.ResourceReadError, fs.ResourceId, ex.Message)));
                                }
                            }
                            catch (Exception ex)
                            {
                                issues.Add(new ValidationIssue(mdef, ValidationStatus.Error, string.Format(Strings.MapDefinitionValidator.FeatureSourceReadError, l.ResourceId, ex.Message)));
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        issues.Add(new ValidationIssue(mdef, ValidationStatus.Error, string.Format(Strings.MapDefinitionValidator.LayerReadError, l.ResourceId, ex.Message)));
                    }
                }
            }

            return issues.ToArray();
        }

        #endregion
    }
}
