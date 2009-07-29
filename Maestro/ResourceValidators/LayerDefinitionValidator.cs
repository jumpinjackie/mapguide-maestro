#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
    public class LayerDefinitionValidator : IValidator
    {
        public ValidationIssue[] Validate(object resource, bool recurse)
        {
            if (resource as MaestroAPI.LayerDefinition == null)
                return null;

            MaestroAPI.LayerDefinition ldef = resource as MaestroAPI.LayerDefinition;
            MaestroAPI.VectorLayerDefinitionType vldef = ldef.Item as MaestroAPI.VectorLayerDefinitionType;
            MaestroAPI.GridLayerDefinitionType gldef = ldef.Item as MaestroAPI.GridLayerDefinitionType;
            MaestroAPI.DrawingLayerDefinitionType dldef = ldef.Item as MaestroAPI.DrawingLayerDefinitionType;

            List<ValidationIssue> issues = new List<ValidationIssue>();

            if (ldef.Item == null)
                issues.Add(new ValidationIssue(resource, ValidationStatus.Error, "Layer is missing core information"));
            else if (vldef == null && gldef == null)
                issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, "Only vector layers and raster layers are currently validated"));

            if (vldef != null)
            {
                if (string.IsNullOrEmpty(vldef.FeatureName))
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, "No FeatureSource is assigned to the layer"));
                if (string.IsNullOrEmpty(vldef.Geometry))
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, "No Geometry is assigned to the layer"));

                if (vldef.VectorScaleRange == null || vldef.VectorScaleRange.Count == 0)
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, "No scale ranges are defined, no data can be displayed"));
                else
                {
                    //Test for overlapping scale ranges
                    List<KeyValuePair<double, double>> ranges = new List<KeyValuePair<double, double>>();
                    foreach (MaestroAPI.VectorScaleRangeType vsr in vldef.VectorScaleRange)
                        ranges.Add(new KeyValuePair<double, double>(
                            vsr.MaxScaleSpecified ? vsr.MaxScale : double.PositiveInfinity,
                            vsr.MinScaleSpecified ? vsr.MinScale : double.NegativeInfinity));

                    double min = double.PositiveInfinity;
                    double max = double.NegativeInfinity;
                    foreach (KeyValuePair<double, double> sr in ranges)
                    {
                        min = Math.Min(min, sr.Value);
                        max = Math.Max(max, sr.Key);
                        if (sr.Key < sr.Value)
                            issues.Add(new ValidationIssue(resource, ValidationStatus.Error, string.Format("The minimum scale ({0}) is larger than the maximum scale ({1}).", sr.Value, sr.Key)));
                    }

                    //TODO: Detect gaps in scale ranges
                    for (int i = 0; i < ranges.Count; i++)
                        for (int j = i + 1; j < ranges.Count; j++)
                            if (ranges[i].Key > ranges[j].Value || ranges[i].Value > ranges[j].Value)
                                issues.Add(new ValidationIssue(resource, ValidationStatus.Information, string.Format("The scale range {0}-{1} overlaps the range {2}-{3}", ranges[i].Value, ranges[i].Key, ranges[j].Value, ranges[j].Key)));

                }
            }
            else if (gldef != null)
            {
                if (gldef.GridScaleRange == null || gldef.GridScaleRange.Count == 0)
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, "No scale ranges are defined, no data can be displayed"));
                else if (gldef.GridScaleRange.Count != 1)
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, "More than one scale ranges is defined, this is valid, but unsupported by Maestro"));
            }
            else if (dldef != null)
                issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, "Maestro does not support DrawingLayers"));
            else
                issues.Add(new ValidationIssue(resource, ValidationStatus.Warning, "The layer has no type, or the type is unsupported by Maestro"));

            if (recurse)
            {
                try
                {
                    MaestroAPI.FeatureSource fs = ldef.CurrentConnection.GetFeatureSource(ldef.Item.ResourceId);
                    issues.AddRange(Validation.Validate(fs, recurse));

                    try
                    {
                        if (vldef != null || gldef != null)
                        {
                            string schema = vldef == null ? gldef.FeatureName : vldef.FeatureName;
                            string geometry = vldef == null ? gldef.Geometry : vldef.Geometry;

                            bool foundSchema = false;
                            bool foundGeometry = false;

                            MaestroAPI.FeatureSourceDescription desc = fs.DescribeSource();
                            foreach (MaestroAPI.FeatureSourceDescription.FeatureSourceSchema scm in desc.Schemas)
                                if (scm.FullnameDecoded == schema)
                                {
                                    foundSchema = true;

                                    foreach(MaestroAPI.FeatureSetColumn col in scm.Columns)
                                        if (col.Name == geometry)
                                        {
                                            foundGeometry = true;
                                            break;
                                        }


                                    //TODO: Validate property mapping, if any
                                    break;
                                }

                            if (!foundSchema)
                                issues.Add(new ValidationIssue(resource, ValidationStatus.Error, string.Format("Failed to find schema {0} in featuresource {1}", schema, fs.ResourceId)));
                            else if (!foundGeometry)
                                issues.Add(new ValidationIssue(resource, ValidationStatus.Error, string.Format("Failed to find geometry column {0} in schema {1} on featuresource {2}", geometry, schema, fs.ResourceId)));
                        }
                    }
                    catch (Exception ex)
                    {
                        issues.Add(new ValidationIssue(fs, ValidationStatus.Error, string.Format("Failed to validate column and schema")));
                    }
                }
                catch (Exception ex)
                {
                    issues.Add(new ValidationIssue(resource, ValidationStatus.Error, string.Format("Failed to load featuresource")));
                }
            }

            return issues.ToArray();
        }
    }
}
