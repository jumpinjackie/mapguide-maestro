#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// Represents a MapGuide Resource
    /// </summary>
    public interface IResource : IVersionedEntity, ICloneable, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the validating schema.
        /// </summary>
        /// <value>The validating schema.</value>
        string ValidatingSchema { get; }

        /// <summary>
        /// Gets or sets the resource ID.
        /// </summary>
        /// <value>The resource ID.</value>
        string ResourceID { get; set; }

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <value>The type of the resource.</value>
        string ResourceType { get; }

        /// <summary>
        /// Serializes this instance to XML and returns the XML content. It is not recommended to call this method directly
        /// instead use <see cref="M:OSGeo.MapGuide.MaestroAPI.ResourceTypeRegistry.Serialize"/> as that will invoke any pre-serialization
        /// hooks that may have been set up for this particular resource.
        /// </summary>
        /// <returns></returns>
        string Serialize();

        /// <summary>
        /// Indicates whether this resource is strongly typed. If false it means the implementer
        /// is a <see cref="T:OSGeo.MapGuide.ObjectModels.UntypedResource"/> object. This usually means that the matching serializer
        /// could not be found because the resource version is unrecognised.
        /// </summary>
        bool IsStronglyTyped { get; }
    }

    /// <summary>
    /// Extension method class
    /// </summary>
    public static class ResourceExtensions
    {
        /// <summary>
        /// Serializes to stream.
        /// </summary>
        /// <param name="res">The res.</param>
        /// <returns></returns>
        public static Stream SerializeToStream(this IResource res)
        {
            string str = res.Serialize();
            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Gets the resource type descriptor.
        /// </summary>
        /// <param name="res">The res.</param>
        /// <returns></returns>
        public static ResourceTypeDescriptor GetResourceTypeDescriptor(this IResource res)
        {
            return new ResourceTypeDescriptor(res.ResourceType, res.ResourceVersion.ToString());
        }
    }
}