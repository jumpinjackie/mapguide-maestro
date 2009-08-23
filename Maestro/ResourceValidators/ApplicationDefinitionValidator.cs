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
