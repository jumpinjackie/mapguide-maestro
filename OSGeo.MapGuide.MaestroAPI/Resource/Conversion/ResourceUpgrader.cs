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
    /// Base class of all resource converter classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ResourceUpgrader<T> : IResourceConverter where T : IResource
    {
        /// <summary>
        /// Gets the version this converter will convert to
        /// </summary>
        /// <value></value>
        public abstract Version TargetVersion
        {
            get;
        }

        /// <summary>
        /// Converts the specified resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        public T Convert(T resource)
        {
            if (resource.ResourceVersion > this.TargetVersion)
                throw new InvalidOperationException(Properties.Resources.ERR_CANNOT_UPGRADE_NEWER_RESOURCE);

            if (resource.ResourceVersion == this.TargetVersion)
                return resource;

            return Upgrade(resource);
        }

        /// <summary>
        /// Upgrades the resource to the target version
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        protected abstract T Upgrade(T resource);

        /// <summary>
        /// Converts the resource
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public IResource Convert(IResource resource)
        {
            return Convert((T)resource);
        }

        /// <summary>
        /// Gets the version this converter can convert from
        /// </summary>
        /// <value></value>
        public abstract Version SourceVersion
        {
            get;
        }

        /// <summary>
        /// Gets the type of resource this can convert
        /// </summary>
        /// <value></value>
        public abstract ResourceTypes ResourceType
        {
            get;
        }
    }
}
