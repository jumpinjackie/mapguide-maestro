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
            {
                issues.Add(new ValidationIssue(layout, ValidationStatus.Error, string.Format(Strings.WebLayoutValidator.MissingMapError)));
            }
            else
            {
                //Check for duplicate command names
                var cmdSet = layout.CommandSet;
                Dictionary<string, CommandType> cmds = new Dictionary<string, CommandType>();
                foreach (CommandType cmd in cmdSet)
                {
                    if (cmds.ContainsKey(cmd.Name))
                        issues.Add(new ValidationIssue(layout, ValidationStatus.Error, string.Format(Strings.WebLayoutValidator.DuplicateCommandName, cmd.Name)));
                    else
                        cmds[cmd.Name] = cmd;
                }

                //Check for duplicate property references in search commands
                foreach (CommandType cmd in cmdSet)
                {
                    if (cmd is SearchCommandType)
                    {
                        SearchCommandType search = (SearchCommandType)cmd;
                        Dictionary<string, string> resColProps = new Dictionary<string, string>();
                        foreach (ResultColumnType resCol in search.ResultColumns)
                        {
                            if (resColProps.ContainsKey(resCol.Property))
                                issues.Add(new ValidationIssue(layout, ValidationStatus.Error, string.Format(Strings.WebLayoutValidator.DuplicateSearchResultColumn, search.Name, resCol.Property)));
                            else
                                resColProps.Add(resCol.Property, resCol.Property);
                        }
                    }
                }

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
                                issues.Add(new ValidationIssue(mdef, ValidationStatus.Warning, string.Format(Strings.WebLayoutValidator.StartViewOutsideExtentsWarning)));
                        }

                    }
                    catch (Exception ex)
                    {
                        string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                        issues.Add(new ValidationIssue(layout, ValidationStatus.Error, string.Format(Strings.WebLayoutValidator.MapValidationError, layout.Map.ResourceId, msg)));
                    }
                }
            }

            return issues.ToArray();
                
        }

        #endregion
    }
}
