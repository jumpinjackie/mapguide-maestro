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
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;

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
            Check.NotNull(context, "context"); //NOXLATE

            if (context.IsAlreadyValidated(resource.ResourceID))
                return null;

            var ldef = resource as ILayerDefinition;
            var vldef = ldef.SubLayer as IVectorLayerDefinition;
            var gldef = ldef.SubLayer as IRasterLayerDefinition;
            var dldef = ldef.SubLayer as IDrawingLayerDefinition;

            List<ValidationIssue> issues = new List<ValidationIssue>();

            if (ldef.SubLayer == null)
                issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_LayerNull, Strings.LDF_LayerNullError));

            ClassDefinition cls = null;
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
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_FeatureSourceLoadError, string.Format(Strings.LDF_FeatureSourceLoadError)));
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

                        cls = fs.GetClass(qualClassName);
                        if (cls != null)
                        {
                            foundSchema = true;
                            foreach (PropertyDefinition col in cls.Properties)
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
                                    foreach (PropertyDefinition col in cls.Properties)
                                    {
                                        if (col.Name == s.Name)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (!found)
                                        issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_ClassNotFound, string.Format(Strings.LDF_SchemaMissingError, qualClassName, fs.ResourceID)));
                                }
                            }
                        }

                        if (!foundSchema)
                            issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_ClassNotFound, string.Format(Strings.LDF_SchemaMissingError, qualClassName, fs.ResourceID)));
                        else if (!foundGeometry)
                            issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_GeometryNotFound, string.Format(Strings.LDF_GeometryMissingError, geometry, qualClassName, fs.ResourceID)));
                    }
                    catch (Exception ex)
                    {
                        issues.Add(new ValidationIssue(fs, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_Generic, string.Format(Strings.LDF_SchemaAndColumnReadFailedError, ex.ToString())));
                    }

                    if (recurse)
                    {
                        issues.AddRange(ResourceValidatorSet.Validate(context, fs, recurse));
                    }
                }
            }

            if (vldef != null)
            {
                if (string.IsNullOrEmpty(vldef.FeatureName))
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_MissingFeatureSource, Strings.LDF_MissingFeatureSourceError));
                if (string.IsNullOrEmpty(vldef.Geometry))
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_MissingGeometry, Strings.LDF_MissingGeometryError));

                if (vldef.VectorScaleRange == null || !vldef.HasVectorScaleRanges())
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_MissingScaleRanges, Strings.LDF_MissingScaleRangesError));
                else
                {
                    //Test for overlapping scale ranges
                    List<KeyValuePair<double, double>> ranges = new List<KeyValuePair<double, double>>();
                    foreach (IVectorScaleRange vsr in vldef.VectorScaleRange)
                    {
                        IVectorScaleRange2 vsr2 = vsr as IVectorScaleRange2;
                        if (vsr2 != null)
                        {
                            var area = vsr2.AreaStyle;
                            var line = vsr2.LineStyle;
                            var point = vsr2.PointStyle;

                            if (vsr2.CompositeStyleCount > 0 && (area != null || line != null || point != null))
                            {
                                issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_LayerDefinition_CompositeStyleDefinedAlongsideBasicStyle, string.Format(
                                    Strings.LDF_CompositeStyleDefinedAlongsideBasicStyle,
                                    vsr2.MinScale.HasValue ? vsr2.MinScale.Value : 0,
                                    vsr2.MaxScale.HasValue ? vsr2.MaxScale.Value.ToString() : Strings.Infinity))); 
                            }

                            if (vsr2.CompositeStyleCount > 0)
                            {
                                foreach (var cs in vsr2.CompositeStyle)
                                {
                                    foreach (var cr in cs.CompositeRule)
                                    {
                                        if (cr.CompositeSymbolization != null)
                                        {
                                            foreach (var si in cr.CompositeSymbolization.SymbolInstance)
                                            {
                                                //verify that symbol references point to valid symbol definitions
                                                if (si.Reference.Type == SymbolInstanceType.Reference)
                                                {
                                                    var symRef = (ISymbolInstanceReferenceLibrary)si.Reference;
                                                    if (!context.ResourceExists(symRef.ResourceId))
                                                        issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_SymbolDefintionReferenceNotFound, string.Format(
                                                            Strings.LDF_SymbolDefintionReferenceNotFound, symRef.ResourceId)));


                                                    if (cls != null)
                                                    {
                                                        //warn if the symbol definition has usage contexts that are irrelevant against the layer's feature source
                                                        var prop = cls.FindProperty(vldef.Geometry) as GeometricPropertyDefinition;
                                                        if (prop != null)
                                                        {
                                                            var gtypes = new List<FeatureGeometricType>(prop.GetIndividualGeometricTypes());
                                                            var sym = context.GetResource(symRef.ResourceId) as ISimpleSymbolDefinition;
                                                            if (sym != null)
                                                            {
                                                                if (sym.LineUsage != null && !gtypes.Contains(FeatureGeometricType.Curve))
                                                                {
                                                                    issues.Add(new ValidationIssue(resource, ValidationStatus.Information, ValidationStatusCode.Info_LayerDefinition_IrrelevantUsageContext, string.Format(
                                                                        Strings.LDF_IrrelevantUsageContext, symRef.ResourceId, FeatureGeometricType.Curve.ToString())));
                                                                }

                                                                if (sym.AreaUsage != null && !gtypes.Contains(FeatureGeometricType.Surface))
                                                                {
                                                                    issues.Add(new ValidationIssue(resource, ValidationStatus.Information, ValidationStatusCode.Info_LayerDefinition_IrrelevantUsageContext, string.Format(
                                                                        Strings.LDF_IrrelevantUsageContext, symRef.ResourceId, FeatureGeometricType.Surface.ToString())));
                                                                }

                                                                if (sym.PointUsage != null && !gtypes.Contains(FeatureGeometricType.Point))
                                                                {
                                                                    issues.Add(new ValidationIssue(resource, ValidationStatus.Information, ValidationStatusCode.Info_LayerDefinition_IrrelevantUsageContext, string.Format(
                                                                        Strings.LDF_IrrelevantUsageContext, symRef.ResourceId, FeatureGeometricType.Point.ToString())));
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                //verify that the overrides specified are for symbol parameters that actually exist
                                                if (si.ParameterOverrides.Count > 0)
                                                {
                                                    if (si.Reference.Type == SymbolInstanceType.Reference)
                                                    {
                                                        var symRef = (ISymbolInstanceReferenceLibrary)si.Reference;
                                                        var sym = (ISymbolDefinitionBase)context.GetResource(symRef.ResourceId);
                                                        var paramList = new Dictionary<string, IParameter>();
                                                        foreach (var p in sym.GetParameters())
                                                        {
                                                            paramList[p.Identifier] = p;
                                                        }

                                                        foreach (var ov in si.ParameterOverrides.Override)
                                                        {
                                                            if (!paramList.ContainsKey(ov.ParameterIdentifier))
                                                            {
                                                                issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_LayerDefinition_SymbolParameterOverrideToNonExistentParameter, string.Format(
                                                                    Strings.LDF_SymbolParameterOverrideToNonExistentParameter, symRef.ResourceId, ov.ParameterIdentifier)));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

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
                            issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_MinMaxScaleSwapped, string.Format(Strings.LDF_MinAndMaxScaleSwappedError, sr.Value, sr.Key)));
                    }

                    ranges.Sort(CompareScales);

                    //TODO: Detect gaps in scale ranges
                    for (int i = 0; i < ranges.Count; i++)
                        for (int j = i + 1; j < ranges.Count; j++)
                            if (ranges[i].Key > ranges[j].Value || ranges[i].Value > ranges[j].Value)
                                issues.Add(new ValidationIssue(resource, ValidationStatus.Information, ValidationStatusCode.Info_LayerDefinition_ScaleRangeOverlap, string.Format(Strings.LDF_ScaleRangesOverlapInformation, ranges[i].Value, ranges[i].Key, ranges[j].Value, ranges[j].Key)));

                }
            }
            else if (gldef != null)
            {
                if (gldef.GridScaleRange == null || gldef.GridScaleRangeCount == 0)
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_NoGridScaleRanges, Strings.LDF_MissingScaleRangesError));
                else if (gldef.GridScaleRangeCount != 1)
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_LayerDefinition_MultipleGridScaleRanges, Strings.LDF_MultipleScaleRangesWarning));
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
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_DrawingSourceError, Strings.LDF_DrawingSourceError));
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
                            issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_DrawingSourceSheetNotFound, string.Format(Strings.LDF_SheetNotFound, dldef.Sheet)));
                        }
                        else
                        {
                            //null or empty filter means all layers, in that case do nothing
                            if (!string.IsNullOrEmpty(dldef.LayerFilter))
                            {
                                //Check if specified layers all exist in specified section
                                var specifiedLayers = dldef.LayerFilter.Split(','); //NOXLATE
                                var dwLayers = new Dictionary<string, string>();

                                var shtLayers = dwSvc.EnumerateDrawingLayers(dws.ResourceID, sheet.Name);
                                foreach (var sl in shtLayers)
                                {
                                    dwLayers.Add(sl, sl);
                                }

                                foreach (var sl in specifiedLayers)
                                {
                                    if (!dwLayers.ContainsKey(sl.Trim()))
                                        issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_LayerDefinition_DrawingSourceSheetLayerNotFound, string.Format(Strings.LDF_SheetLayerNotFound, sl.Trim(), sheet.Name)));
                                }
                            }
                        }
                    }

                    if (recurse)
                    {
                        issues.AddRange(ResourceValidatorSet.Validate(context, dws, recurse));
                    }
                }
            }
            else
            {
                issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, ValidationStatusCode.Warning_LayerDefinition_UnsupportedLayerType, Strings.LDF_UnsupportedLayerTypeWarning));
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

        private static int CompareScales(KeyValuePair<double, double> rangeA, KeyValuePair<double, double> rangeB)
        {
            //We are comparing from scales of both ranges
            return rangeA.Key.CompareTo(rangeB.Key);
        }
    }
}
