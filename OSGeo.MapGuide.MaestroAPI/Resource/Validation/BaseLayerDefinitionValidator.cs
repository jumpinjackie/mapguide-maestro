#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// A base layer definition validator class the provides the common validation logic. Because this is working against
    /// the layer definition interfaces, this common logic can apply to all versions of the layer definition schema (that
    /// implement these interfaces)
    /// </summary>
    public abstract class BaseLayerDefinitionValidator : IResourceValidator
    {
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
        /// Perform base validation logic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resource"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        protected static ValidationIssue[] ValidateBase(ResourceValidationContext context, IResource resource, bool recurse)
        {
            Check.NotNull(context, "context");

            if (context.IsAlreadyValidated(resource.ResourceID))
                return null;

            var ldef = resource as ILayerDefinition;
            var vldef = ldef.SubLayer as IVectorLayerDefinition;
            var gldef = ldef.SubLayer as IRasterLayerDefinition;
            var dldef = ldef.SubLayer as IDrawingLayerDefinition;

            List<ValidationIssue> issues = new List<ValidationIssue>();

            if (ldef.SubLayer == null)
                issues.Add(new ValidationIssue(resource, ValidationStatus.Error, Properties.Resources.LDF_LayerNullError));
            
            if (vldef != null)
            {
                if (string.IsNullOrEmpty(vldef.FeatureName))
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, Properties.Resources.LDF_MissingFeatureSourceError));
                if (string.IsNullOrEmpty(vldef.Geometry))
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, Properties.Resources.LDF_MissingGeometryError));

                if (vldef.VectorScaleRange == null || !vldef.HasVectorScaleRanges())
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, Properties.Resources.LDF_MissingScaleRangesError));
                else
                {
                    //Test for overlapping scale ranges
                    List<KeyValuePair<double, double>> ranges = new List<KeyValuePair<double, double>>();
                    foreach (IVectorScaleRange vsr in vldef.VectorScaleRange)
                    {
                        ranges.Add(new KeyValuePair<double, double>(
                            vsr.MaxScale.HasValue ? vsr.MaxScale.Value : double.PositiveInfinity,
                            vsr.MinScale.HasValue ? vsr.MinScale.Value : double.NegativeInfinity));
                    }

                    double min = double.PositiveInfinity;
                    double max = double.NegativeInfinity;
                    foreach (KeyValuePair<double, double> sr in ranges)
                    {
                        min = Math.Min(min, sr.Value);
                        max = Math.Max(max, sr.Key);
                        if (sr.Key < sr.Value)
                            issues.Add(new ValidationIssue(resource, ValidationStatus.Error, string.Format(Properties.Resources.LDF_MinAndMaxScaleSwappedError, sr.Value, sr.Key)));
                    }

                    //TODO: Detect gaps in scale ranges
                    for (int i = 0; i < ranges.Count; i++)
                        for (int j = i + 1; j < ranges.Count; j++)
                            if (ranges[i].Key > ranges[j].Value || ranges[i].Value > ranges[j].Value)
                                issues.Add(new ValidationIssue(resource, ValidationStatus.Information, string.Format(Properties.Resources.LDF_ScaleRangesOverlapInformation, ranges[i].Value, ranges[i].Key, ranges[j].Value, ranges[j].Key)));

                }
            }
            else if (gldef != null)
            {
                if (gldef.GridScaleRange == null || gldef.GridScaleRangeCount == 0)
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, Properties.Resources.LDF_MissingScaleRangesError));
                else if (gldef.GridScaleRangeCount != 1)
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, Properties.Resources.LDF_MultipleScaleRangesWarning));
            }
            else if (dldef != null)
            {
                IDrawingSource dws = null;
                try
                {
                    dws = (IDrawingSource)context.GetResource(dldef.ResourceId);
                }
                catch (Exception)
                {
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, Properties.Resources.LDF_DrawingSourceError));
                }

                if (dws != null)
                {
                    if (Array.IndexOf(ldef.CurrentConnection.Capabilities.SupportedServices, (int)ServiceType.Drawing) >= 0)
                    {
                        var dwSvc = (IDrawingService)ldef.CurrentConnection.GetService((int)ServiceType.Drawing);

                        //Check if specified section exists
                        var shtList = dwSvc.EnumerateDrawingSections(dws.ResourceID);
                        DrawingSectionListSection sheet = null;
                        foreach (var sht in shtList.Section)
                        {
                            if (sht.Name.Equals(dldef.Sheet))
                            {
                                sheet = sht;
                                break;
                            }
                        }

                        if (sheet == null)
                        {
                            issues.Add(new ValidationIssue(resource, ValidationStatus.Error, string.Format(Properties.Resources.LDF_SheetNotFound, dldef.Sheet)));
                        }
                        else
                        {
                            //null or empty filter means all layers, in that case do nothing
                            if (!string.IsNullOrEmpty(dldef.LayerFilter))
                            {
                                //Check if specified layers all exist in specified section
                                var specifiedLayers = dldef.LayerFilter.Split(',');
                                var dwLayers = new Dictionary<string, string>();

                                var shtLayers = dwSvc.EnumerateDrawingLayers(dws.ResourceID, sheet.Name);
                                foreach (var sl in shtLayers)
                                {
                                    dwLayers.Add(sl, sl);
                                }

                                foreach (var sl in specifiedLayers)
                                {
                                    if (!dwLayers.ContainsKey(sl.Trim()))
                                        issues.Add(new ValidationIssue(resource, ValidationStatus.Error, string.Format(Properties.Resources.LDF_SheetLayerNotFound, sl.Trim(), sheet.Name)));
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, Properties.Resources.LDF_UnsupportedLayerTypeWarning));
            }

            if (recurse)
            {
                if (vldef != null || gldef != null)
                {
                    //Load referenced feature source
                    IFeatureSource fs = null;

                    try
                    {
                        fs = (IFeatureSource)context.GetResource(ldef.SubLayer.ResourceId);
                        issues.AddRange(ResourceValidatorSet.Validate(context, fs, recurse));
                    }
                    catch (Exception)
                    {
                        issues.Add(new ValidationIssue(resource, ValidationStatus.Error, string.Format(Properties.Resources.LDF_FeatureSourceLoadError)));
                    }

                    if (fs != null)
                    {
                        //Verify specified feature class and geometry check out
                        try
                        {
                            string qualClassName = vldef == null ? gldef.FeatureName : vldef.FeatureName;
                            string geometry = vldef == null ? gldef.Geometry : vldef.Geometry;

                            bool foundSchema = false;
                            bool foundGeometry = false;

                            FeatureSourceDescription desc = context.DescribeFeatureSource(ldef.SubLayer.ResourceId);
                            foreach (FeatureSchema fsc in desc.Schemas)
                            {
                                foreach (ClassDefinition scm in fsc.Classes)
                                {
                                    if (scm.QualifiedName == qualClassName)
                                    {
                                        foundSchema = true;

                                        foreach (PropertyDefinition col in scm.Properties)
                                        {
                                            if (col.Name == geometry)
                                            {
                                                foundGeometry = true;
                                                break;
                                            }
                                        }

                                        if (vldef != null && vldef.PropertyMapping != null)
                                        {
                                            foreach (INameStringPair s in vldef.PropertyMapping)
                                            {
                                                bool found = false;
                                                foreach (PropertyDefinition col in scm.Properties)
                                                {
                                                    if (col.Name == s.Name)
                                                    {
                                                        found = true;
                                                        break;
                                                    }
                                                }
                                                if (!found)
                                                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, string.Format(Properties.Resources.LDF_SchemaMissingError, qualClassName, fs.ResourceID)));
                                            }
                                        }

                                        break;
                                    }
                                }
                            }

                            if (!foundSchema)
                                issues.Add(new ValidationIssue(resource, ValidationStatus.Error, string.Format(Properties.Resources.LDF_SchemaMissingError, qualClassName, fs.ResourceID)));
                            else if (!foundGeometry)
                                issues.Add(new ValidationIssue(resource, ValidationStatus.Error, string.Format(Properties.Resources.LDF_GeometryMissingError, geometry, qualClassName, fs.ResourceID)));
                        }
                        catch (Exception)
                        {
                            issues.Add(new ValidationIssue(fs, ValidationStatus.Error, string.Format(Properties.Resources.LDF_SchemaAndColumnReadFailedError)));
                        }
                    }
                }
            }

            context.MarkValidated(resource.ResourceID);

            return issues.ToArray();
        }

        /// <summary>
        /// Gets the resource type and version this validator supports
        /// </summary>
        /// <value></value>
        public abstract ResourceTypeDescriptor SupportedResourceAndVersion
        {
            get;
        }
    }
}
