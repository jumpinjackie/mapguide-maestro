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
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels;

namespace OSGeo.MapGuide.MaestroAPI.Resource
{
    /// <summary>
    /// Represents an editable MapGuide Resource
    /// </summary>
    public interface IResource : IVersionedEntity, ICloneable, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the current connection.
        /// </summary>
        /// <value>The current connection.</value>
        IServerConnection CurrentConnection { get; set; }

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
        ResourceTypes ResourceType { get; }

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

        /// <summary>
        /// Copies the resource data to the specified resource
        /// </summary>
        /// <remarks>
        /// Avoid using this method if you are copying a IFeatureSource with MG_USER_CREDENTIALS resource data, as MapGuide will automatically return
        /// the decrypted username for MG_USER_CREDENTIALS, rendering the resource data invalid for the target resource. Instead use the
        /// <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IResourceService.CopyResource"/> method, which will copy the resource and its resource
        /// data and keep any MG_USER_CREDENTIALS items intact
        /// </remarks>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public static void CopyResourceDataTo(this IResource source, IResource target)
        {
            Check.NotNull(source, "source"); //NOXLATE
            Check.NotNull(target, "target"); //NOXLATE

            foreach (var res in source.EnumerateResourceData())
            {
                var data = source.GetResourceData(res.Name);
                if (!data.CanSeek)
                {
                    var ms = new MemoryStream();
                    Utility.CopyStream(data, ms);
                    data = ms;
                }
                target.SetResourceData(res.Name, res.Type, data);
            }
        }

        /// <summary>
        /// Copies the resource data to the specified resource
        /// </summary>
        /// <remarks>
        /// Avoid using this method if you are copying a IFeatureSource with MG_USER_CREDENTIALS resource data, as MapGuide will automatically return
        /// the decrypted username for MG_USER_CREDENTIALS, rendering the resource data invalid for the target resource. Instead use the
        /// <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IResourceService.CopyResource"/> method, which will copy the resource and its resource
        /// data and keep any MG_USER_CREDENTIALS items intact
        /// </remarks>
        /// <param name="source">The source.</param>
        /// <param name="targetID">The target ID.</param>
        public static void CopyResourceDataTo(this IResource source, string targetID)
        {
            Check.NotNull(source, "source"); //NOXLATE
            Check.NotEmpty(targetID, "targetID"); //NOXLATE

            foreach (var res in source.EnumerateResourceData())
            {
                var data = source.GetResourceData(res.Name);
                if (!data.CanSeek)
                {
                    var ms = new MemoryStream();
                    Utility.CopyStream(data, ms);
                    data = ms;
                }
                source.CurrentConnection.ResourceService.SetResourceData(targetID, res.Name, res.Type, data);
            }
        }

        /// <summary>
        /// Convenience method for enumerating resource data of this resource
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static ResourceDataListResourceData[] EnumerateResourceData(this IResource res)
        {
            if (res.CurrentConnection == null)
                throw new ArgumentException(Strings.ERR_RESOURCE_NOT_ATTACHED);

            return res.CurrentConnection.ResourceService.EnumerateResourceData(res.ResourceID).ResourceData.ToArray();
        }

        /// <summary>
        /// Convenience method for getting an associated resource data stream of this resource
        /// </summary>
        /// <param name="res"></param>
        /// <param name="dataName"></param>
        /// <returns></returns>
        public static Stream GetResourceData(this IResource res, string dataName)
        {
            if (res.CurrentConnection == null)
                throw new ArgumentException(Strings.ERR_RESOURCE_NOT_ATTACHED);

            return res.CurrentConnection.ResourceService.GetResourceData(res.ResourceID, dataName);
        }

        /// <summary>
        /// Convenience method for setting an associated resource data stream of this resource
        /// </summary>
        /// <param name="res"></param>
        /// <param name="dataName"></param>
        /// <param name="dataType"></param>
        /// <param name="inputStream"></param>
        public static void SetResourceData(this IResource res, string dataName, ResourceDataType dataType, Stream inputStream)
        {
            if (res.CurrentConnection == null)
                throw new ArgumentException(Strings.ERR_RESOURCE_NOT_ATTACHED);

            res.CurrentConnection.ResourceService.SetResourceData(res.ResourceID, dataName, dataType, inputStream);
        }

        /// <summary>
        /// Convenience method for deleting an associated resource data stream of this resource
        /// </summary>
        /// <param name="res"></param>
        /// <param name="dataName"></param>
        public static void DeleteResourceData(this IResource res, string dataName)
        {
            if (res.CurrentConnection == null)
                throw new ArgumentException(Strings.ERR_RESOURCE_NOT_ATTACHED);

            res.CurrentConnection.ResourceService.DeleteResourceData(res.ResourceID, dataName);
        }
    }
}
