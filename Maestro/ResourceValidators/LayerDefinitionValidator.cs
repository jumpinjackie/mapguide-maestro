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

            }



            if (recurse)
                issues.AddRange(Validate(ldef.CurrentConnection.GetFeatureSource(ldef.Item.ResourceId), recurse));

            return issues.ToArray();
        }
    }
}
