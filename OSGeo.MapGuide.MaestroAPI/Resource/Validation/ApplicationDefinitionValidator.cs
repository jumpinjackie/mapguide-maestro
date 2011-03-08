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
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.ObjectModels;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// Resource validator for Fusion Flexible Layouts
    /// </summary>
    public class ApplicationDefinitionValidator : IResourceValidator
    {
        /// <summary>
        /// Validats the specified resources for common issues associated with this
        /// resource type
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resource"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public ValidationIssue[] Validate(ResourceValidationContext context, IResource resource, bool recurse)
        {
            if (resource.ResourceType != OSGeo.MapGuide.MaestroAPI.ResourceTypes.ApplicationDefinition)
                return null;

            //TODO: Other items to check for
            //
            // - References to non-existent widgets
            // - MapWidget checks
            //   - Ensure map id checks out
            //   - Ensure context menu id checks out
            // - Verify containers of template are all referenced within this flexible layout
            // - Check required parameters of widgets are satisfied

            List<ValidationIssue> issues = new List<ValidationIssue>();

            IApplicationDefinition fusionApp = resource as IApplicationDefinition;
            if (fusionApp.MapSet == null || fusionApp.MapSet.MapGroupCount == 0)
                issues.Add(new ValidationIssue(fusionApp, ValidationStatus.Error, string.Format(Properties.Resources.ADF_MapMissingError)));
            else
            {
                if (recurse)
                {
                    foreach (IMapGroup mapGroup in fusionApp.MapSet.MapGroups)
                    {
                        foreach (IMap map in mapGroup.Map)
                        {
                            try
                            {
                                if (map.Extension == null || string.IsNullOrEmpty(map.GetMapDefinition()))
                                {
                                    issues.Add(new ValidationIssue(fusionApp, ValidationStatus.Error, string.Format(Properties.Resources.ADF_MapInvalidError, mapGroup.id)));
                                }
                                else
                                {
                                    IMapDefinition mdef = (IMapDefinition)context.GetResource(map.GetMapDefinition());

                                    issues.AddRange(ResourceValidatorSet.Validate(context, mdef, true));

                                    IEnvelope mapEnv = ObjectFactory.CreateEnvelope(mdef.Extents.MinX, mdef.Extents.MaxX, mdef.Extents.MinY, mdef.Extents.MaxY);

                                    if (mapGroup.InitialView != null)
                                    {
                                        if (!mapEnv.Contains(mapGroup.InitialView.CenterX, mapGroup.InitialView.CenterY))
                                            issues.Add(new ValidationIssue(mdef, ValidationStatus.Warning, string.Format(Properties.Resources.ADF_ViewOutsideMapExtents)));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                                issues.Add(new ValidationIssue(fusionApp, ValidationStatus.Error, string.Format(Properties.Resources.ADF_MapValidationError, mapGroup.id, msg)));
                            }
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
        public ResourceTypeDescriptor SupportedResourceAndVersion
        {
            get { return new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.ApplicationDefinition, "1.0.0"); }
        }
    }
}
