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

namespace OSGeo.MapGuide.MaestroAPI.Resource.Conversion
{
    /// <summary>
    /// Performs schematic upgrades of resources
    /// </summary>
    public interface IResourceConversionService
    {
        /// <summary>
        /// Upgrades the resource
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="targetVersion">The target version.</param>
        /// <returns></returns>
        IResource Upgrade(IResource resource, Version targetVersion);
    }

    /// <summary>
    /// Converts a specified resource to a specified version
    /// </summary>
    public interface IResourceConverter
    {
        /// <summary>
        /// Gets the type of resource this can convert
        /// </summary>
        ResourceTypes ResourceType { get; }

        /// <summary>
        /// Gets the version this converter can convert from
        /// </summary>
        Version SourceVersion { get; }

        /// <summary>
        /// Gets the version this converter will convert to
        /// </summary>
        Version TargetVersion { get; }

        /// <summary>
        /// Converts the resource
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        IResource Convert(IResource resource);
    }
}
