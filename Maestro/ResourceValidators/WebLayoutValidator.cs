using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceValidators
{
    public class WebLayoutValidator : IValidator 
    {
        #region IValidator Members

        public ValidationIssue[] Validate(object resource, bool recurse)
        {
            if (resource as WebLayout == null)
                return null;

            List<ValidationIssue> issues = new List<ValidationIssue>();

            WebLayout layout = resource as WebLayout;
            if (layout.Map == null || layout.Map.ResourceId == null)
                issues.Add(new ValidationIssue(layout, ValidationStatus.Error, string.Format("Layout does not specify a map")));
            else
            {
                if (recurse)
                {
                    try
                    {
                        MapDefinition mdef = layout.CurrentConnection.GetMapDefinition(layout.Map.ResourceId);

                        issues.AddRange(Validation.Validate(mdef, true));

                        if (layout.Map.InitialView != null)
                        {
                            Topology.Geometries.Envelope mapEnv = new Topology.Geometries.Envelope(mdef.Extents.MinX, mdef.Extents.MaxX, mdef.Extents.MinY, mdef.Extents.MaxY);
                            if (!mapEnv.Contains(layout.Map.InitialView.CenterX, layout.Map.InitialView.CenterY))
                                issues.Add(new ValidationIssue(mdef, ValidationStatus.Warning, string.Format("Layout specifies a start view that is outside the map's initial extents")));
                        }

                    }
                    catch (Exception ex)
                    {
                        issues.Add(new ValidationIssue(layout, ValidationStatus.Error, string.Format("Error validating MapDefinition {0}, message: {1}", layout.Map.ResourceId, ex.Message)));
                    }
                }
            }

            return issues.ToArray();
                
        }

        #endregion
    }
}
