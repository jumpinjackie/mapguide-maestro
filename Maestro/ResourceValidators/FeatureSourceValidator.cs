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
using OSGeo.MapGuide.Maestro;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceValidators
{
	/// <summary>
	/// Validator for validating FeatureSources.
	/// </summary>
	public class FeatureSourceValidator : IValidator
	{
        public ValidationIssue[] Validate(object resource, bool recurse)
        {
            if (resource as OSGeo.MapGuide.MaestroAPI.FeatureSource == null)
                return null;

            List<ValidationIssue> issues = new List<ValidationIssue>();

            OSGeo.MapGuide.MaestroAPI.FeatureSource feature = resource as OSGeo.MapGuide.MaestroAPI.FeatureSource;
            //Note: Must be saved!
            string s = feature.CurrentConnection.TestConnection(feature.ResourceId);
            if (s.Trim().ToUpper() != true.ToString().ToUpper())
                return new ValidationIssue[] { new ValidationIssue(feature, ValidationStatus.Error, s) };

            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InvariantCulture;
                MaestroAPI.FdoSpatialContextList lst = feature.GetSpatialInfo();
                if (lst == null || lst.SpatialContext == null || lst.SpatialContext.Count == 0)
                    issues.Add(new ValidationIssue(feature, ValidationStatus.Warning, Strings.FeatureSourceValidator.NoSpatialContextWarning));
                else
                    foreach (MaestroAPI.FdoSpatialContextListSpatialContext c in lst.SpatialContext)
                        if (c.Extent == null || c.Extent.LowerLeftCoordinate == null || c.Extent.UpperRightCoordinate == null)
                            issues.Add(new ValidationIssue(feature, ValidationStatus.Warning, Strings.FeatureSourceValidator.EmptySpatialContextWarning));
                        else if (double.Parse(c.Extent.LowerLeftCoordinate.X, ci) <= -1000000 && double.Parse(c.Extent.LowerLeftCoordinate.Y, ci) <= -1000000 && double.Parse(c.Extent.UpperRightCoordinate.X, ci) >= 1000000 && double.Parse(c.Extent.UpperRightCoordinate.Y, ci) >= 1000000)
                            issues.Add(new ValidationIssue(feature, ValidationStatus.Warning, Strings.FeatureSourceValidator.DefaultSpatialContextWarning));
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                issues.Add(new ValidationIssue(feature, ValidationStatus.Error, string.Format(Strings.FeatureSourceValidator.SpatialContextReadError, msg)));
            }

            List<string> classes = new List<string>();
            try
            {
                MaestroAPI.FeatureSourceDescription fsd = feature.DescribeSource();
                if (fsd == null || fsd.Schemas == null || fsd.Schemas.Length == 0)
                    issues.Add(new ValidationIssue(feature, ValidationStatus.Warning, Strings.FeatureSourceValidator.ShemasMissingWarning));
                else
                    foreach (MaestroAPI.FeatureSourceDescription.FeatureSourceSchema scm in fsd.Schemas)
                        classes.Add(scm.FullnameDecoded);
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                issues.Add(new ValidationIssue(feature, ValidationStatus.Error, string.Format(Strings.FeatureSourceValidator.SchemaReadError, msg)));
            }


            foreach (string cl in classes)
            {
                try
                {
                    string[] ids = feature.GetIdentityProperties(cl);
                    if (ids == null || ids.Length == 0)
                        issues.Add(new ValidationIssue(feature, ValidationStatus.Information, string.Format(Strings.FeatureSourceValidator.PrimaryKeyMissingInformation, cl)));
                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    issues.Add(new ValidationIssue(feature, ValidationStatus.Error, string.Format(Strings.FeatureSourceValidator.PrimaryKeyReadError, msg)));
                }
            }

            return issues.ToArray();
        }

    }
}
