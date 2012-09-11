#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI.Exceptions;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// Resource validator for Map Definitions
    /// </summary>
    public abstract class BaseMapDefinitionValidator : IResourceValidator
    {
        /// <summary>
        /// Gets the resource type and version this validator supports
        /// </summary>
        /// <value></value>
        public abstract ResourceTypeDescriptor SupportedResourceAndVersion { get; }

        /// <summary>
        /// Validats the specified resources for common issues associated with this
        /// resource type
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resource"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public virtual ValidationIssue[] Validate(ResourceValidationContext context, IResource resource, bool recurse)
        {
            if (!resource.GetResourceTypeDescriptor().Equals(this.SupportedResourceAndVersion))
                return null;

            return ValidateBase(context, resource, recurse);
        }

        /// <summary>
        /// Performs base resource validation
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resource"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        protected static ValidationIssue[] ValidateBase(ResourceValidationContext context, IResource resource, bool recurse)
        {
            Check.NotNull(context, "context"); //NOXLATE

            if (context.IsAlreadyValidated(resource.ResourceID))
                return null;

            if (resource.ResourceType != ResourceTypes.MapDefinition)
                return null;

            List<ValidationIssue> issues = new List<ValidationIssue>();

            IMapDefinition mdef = resource as IMapDefinition;

            if (string.IsNullOrEmpty(mdef.CoordinateSystem))
            {
                issues.Add(new ValidationIssue(mdef, ValidationStatus.Warning, ValidationStatusCode.Warning_MapDefinition_MissingCoordinateSystem, Strings.MDF_NoCoordinateSystem));
            }

            foreach (IMapLayerGroup g in mdef.MapLayerGroup)
            {
                if (g.ShowInLegend && (g.LegendLabel == null || g.LegendLabel.Trim().Length == 0))
                    issues.Add(new ValidationIssue(mdef, ValidationStatus.Information, ValidationStatusCode.Info_MapDefinition_GroupMissingLabelInformation, string.Format(Strings.MDF_GroupMissingLabelInformation, g.Name)));
                else if (g.ShowInLegend && g.LegendLabel.Trim().ToLower() == "layer group") //NOXLATE
                    issues.Add(new ValidationIssue(mdef, ValidationStatus.Information, ValidationStatusCode.Info_MapDefinition_GroupHasDefaultLabel, string.Format(Strings.MDF_GroupHasDefaultLabelInformation, g.Name)));

                if (!string.IsNullOrEmpty(g.Group))
                {
                    var grp = mdef.GetGroupByName(g.Group);
                    if (grp == null)
                        issues.Add(new ValidationIssue(mdef, ValidationStatus.Error, ValidationStatusCode.Error_MapDefinition_GroupWithNonExistentGroup, string.Format(Strings.MDF_GroupWithNonExistentGroup, g.Name, g.Group)));
                }
            }

            List<IBaseMapLayer> layers = new List<IBaseMapLayer>();
            foreach (IBaseMapLayer l in mdef.MapLayer)
                layers.Add(l);

            if (mdef.BaseMap != null && mdef.BaseMap.HasGroups())
            {
                if (mdef.BaseMap.ScaleCount == 0)
                    issues.Add(new ValidationIssue(mdef, ValidationStatus.Error, ValidationStatusCode.Error_MapDefinition_NoFiniteDisplayScales, Strings.MDF_NoFiniteDisplayScalesSpecified));

                foreach (IBaseMapGroup g in mdef.BaseMap.BaseMapLayerGroup)
                {
                    foreach (IBaseMapLayer l in g.BaseMapLayer)
                        layers.Add(l);
                }
            }
            Dictionary<string, IBaseMapLayer> nameCounter = new Dictionary<string, IBaseMapLayer>();

            foreach (IBaseMapLayer l in layers)
            {
                if (nameCounter.ContainsKey(l.Name))
                    issues.Add(new ValidationIssue(mdef, ValidationStatus.Warning, ValidationStatusCode.Error_MapDefinition_DuplicateLayerName, string.Format(Strings.MDF_LayerNameDuplicateWarning, l.Name, l.ResourceId, nameCounter[l.Name].ResourceId)));
                else
                    nameCounter.Add(l.Name, l);

                var ml = l as IMapLayer;
                if (ml != null && !string.IsNullOrEmpty(ml.Group))
                {
                    var grp = mdef.GetGroupByName(ml.Group);
                    if (grp == null)
                        issues.Add(new ValidationIssue(mdef, ValidationStatus.Error, ValidationStatusCode.Error_MapDefinition_LayerWithNonExistentGroup, string.Format(Strings.MDF_LayerWithNonExistentGroup, ml.Name, ml.Group)));
                }

                if (l.ShowInLegend && (string.IsNullOrEmpty(l.LegendLabel) || l.LegendLabel.Trim().Length == 0))
                    issues.Add(new ValidationIssue(mdef, ValidationStatus.Information, ValidationStatusCode.Warning_MapDefinition_LayerMissingLegendLabel, string.Format(Strings.MDF_LayerMissingLabelInformation, l.Name)));

                var mapEnv = ObjectFactory.CreateEnvelope(mdef.Extents.MinX, mdef.Extents.MinY, mdef.Extents.MaxX, mdef.Extents.MaxY);

                try
                {
                    ILayerDefinition layer = null;
                    IResource res = context.GetResource(l.ResourceId);
                    if (!ResourceValidatorSet.HasValidator(res.ResourceType, res.ResourceVersion))
                    {
                        //Need to trap the no registered validator message
                        issues.AddRange(ResourceValidatorSet.Validate(context, res, true));
                        continue;
                    }

                    layer = (ILayerDefinition)res;
                    if (recurse)
                    {
                        issues.AddRange(ResourceValidatorSet.Validate(context, layer, recurse));
                    }

                    IVectorLayerDefinition vl = null;
                    if (layer.SubLayer.LayerType == LayerType.Vector)
                        vl = (IVectorLayerDefinition)layer.SubLayer;

                    if (vl != null)
                    {
                        try
                        {
                            IFeatureSource fs = (IFeatureSource)context.GetResource(vl.ResourceId);
                            //The layer recurses on the FeatureSource
                            //issues.AddRange(Validation.Validate(fs, true));

                            try
                            {
                                FdoSpatialContextList scList = context.GetSpatialContexts(fs.ResourceID);

                                if (scList.SpatialContext == null || scList.SpatialContext.Count == 0)
                                    issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_MapDefinition_MissingSpatialContext, string.Format(Strings.MDF_MissingSpatialContextWarning, fs.ResourceID)));
                                else
                                {
                                    if (scList.SpatialContext.Count > 1)
                                        issues.Add(new ValidationIssue(resource, ValidationStatus.Information, ValidationStatusCode.Info_MapDefinition_MultipleSpatialContexts, string.Format(Strings.MDF_MultipleSpatialContextsInformation, fs.ResourceID)));


                                    bool skipGeomCheck = false;

                                    //TODO: Switch to the correct version (2.1), once released
                                    if (scList.SpatialContext[0].CoordinateSystemWkt != mdef.CoordinateSystem)
                                    {
                                        if (layer.SubLayer.LayerType == LayerType.Raster && mdef.CurrentConnection.SiteVersion <= SiteVersions.GetVersion(OSGeo.MapGuide.MaestroAPI.KnownSiteVersions.MapGuideOS2_0_2))
                                            issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_MapDefinition_RasterReprojection, string.Format(Strings.MDF_RasterReprojectionError, fs.ResourceID)));
                                        else
                                            issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_MapDefinition_LayerReprojection, string.Format(Strings.MDF_DataReprojectionWarning, fs.ResourceID)));

                                        skipGeomCheck = true;
                                    }

                                    if (vl.Geometry != null && !skipGeomCheck)
                                    {
                                        var env = fs.GetSpatialExtent(vl.FeatureName, vl.Geometry);
                                        if (!env.Intersects(mapEnv))
                                            issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_MapDefinition_DataOutsideMapBounds, string.Format(Strings.MDF_DataOutsideMapWarning, fs.ResourceID)));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                var nex = ex as NullExtentException;
                                if (nex != null)
                                {
                                    issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_MapDefinition_FeatureSourceWithNullExtent, string.Format(Strings.MDF_LayerWithNullExtent, fs.ResourceID)));
                                }
                                else
                                {
                                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_MapDefinition_ResourceRead, string.Format(Strings.MDF_ResourceReadError, fs.ResourceID, msg)));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                            issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_MapDefinition_FeatureSourceRead, string.Format(Strings.MDF_FeatureSourceReadError, l.ResourceId, msg)));
                        }
                    }

                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_MapDefinition_LayerRead, string.Format(Strings.MDF_LayerReadError, l.ResourceId, msg)));
                }
            }

            context.MarkValidated(resource.ResourceID);

            return issues.ToArray();
        }
    }
}
