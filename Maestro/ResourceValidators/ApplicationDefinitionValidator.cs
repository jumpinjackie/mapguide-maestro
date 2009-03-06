using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;

namespace OSGeo.MapGuide.Maestro.ResourceValidators
{
    public class ApplicationDefinitionValidator : IValidator
    {
        #region IValidator Members

        public ValidationIssue[] Validate(object resource, bool recurse)
        {
            if (resource as ApplicationDefinitionType == null)
                return null;

            List<ValidationIssue> issues = new List<ValidationIssue>();

            ApplicationDefinitionType fusionApp = resource as ApplicationDefinitionType;
            if (fusionApp.MapSet == null || fusionApp.MapSet.Count == 0)
                issues.Add(new ValidationIssue(fusionApp, ValidationStatus.Error, string.Format("Fusion application does not specify a map")));
            else
            {
                if (recurse)
                {
                    foreach (MapGroupType mapGroup in fusionApp.MapSet)
                        foreach(OSGeo.MapGuide.MaestroAPI.ApplicationDefinition.MapType map in mapGroup.Map)
                        {
                            try
                            {

                                if (map.Extension == null || map.Extension["ResourceId"] == null)
                                {
                                    issues.Add(new ValidationIssue(fusionApp, ValidationStatus.Error, string.Format("Map with ID {0} does not point to a valid map", mapGroup.id)));
                                }
                                else
                                {
                                    MapDefinition mdef = fusionApp.CurrentConnection.GetMapDefinition(map.Extension["ResourceId"]);

                                    issues.AddRange(Validation.Validate(mdef, true));

                                    Topology.Geometries.Envelope mapEnv = new Topology.Geometries.Envelope(mdef.Extents.MinX, mdef.Extents.MaxX, mdef.Extents.MinY, mdef.Extents.MaxY);

                                    if (mapGroup.InitialView != null)
                                    {
                                        if (!mapEnv.Contains(mapGroup.InitialView.CenterX, mapGroup.InitialView.CenterY))
                                            issues.Add(new ValidationIssue(mdef, ValidationStatus.Warning, string.Format("Fusion application specifies a start view that is outside the map's initial extents")));
                                    }
                                }
                                

                            }
                            catch (Exception ex)
                            {
                                issues.Add(new ValidationIssue(fusionApp, ValidationStatus.Error, string.Format("Error validating MapDefinition {0}, message: {1}", mapGroup.id, ex.Message)));
                            }
                        }
                }
            }

            return issues.ToArray();

        }

        #endregion
    }
}
