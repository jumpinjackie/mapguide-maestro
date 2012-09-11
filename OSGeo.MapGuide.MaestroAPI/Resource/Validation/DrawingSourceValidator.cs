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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.DrawingSource;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// Resource validator for Drawing Sources
    /// </summary>
    public class DrawingSourceValidator : IResourceValidator
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
            Check.NotNull(context, "context"); //NOXLATE

            if (context.IsAlreadyValidated(resource.ResourceID))
                return null;

            if (resource.ResourceType != ResourceTypes.DrawingSource)
                return null;

            var issues = new List<ValidationIssue>();

            IDrawingSource dws = (IDrawingSource)resource;
            if (string.IsNullOrEmpty(dws.SourceName))
                issues.Add(new ValidationIssue(resource, ValidationStatus.Error, ValidationStatusCode.Error_DrawingSource_NoSourceDwf, Strings.DS_NoSourceSpecified));

            if (string.IsNullOrEmpty(dws.CoordinateSpace))
                issues.Add(new ValidationIssue(resource, ValidationStatus.Information, ValidationStatusCode.Info_DrawingSource_NoCoordinateSpace, Strings.DS_NoCoordinateSpace));

            context.MarkValidated(resource.ResourceID);

            return issues.ToArray();
        }

        /// <summary>
        /// Gets the resource type and version this validator supports
        /// </summary>
        /// <value></value>
        public ResourceTypeDescriptor SupportedResourceAndVersion
        {
            get { return new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.DrawingSource, "1.0.0"); } //NOXLATE
        }
    }
}
