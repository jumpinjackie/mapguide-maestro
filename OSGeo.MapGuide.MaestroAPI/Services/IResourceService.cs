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

using ObjCommon = OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.IO;

namespace OSGeo.MapGuide.MaestroAPI.Services
{
    /// <summary>
    /// Provides services for accessing resources in a repository
    /// </summary>
    /// <remarks>
    /// Note that <see cref="T:OSGeo.MapGuide.MaestroAPI.IServerConnection"/> provides
    /// built-in access to resource and feature services. Using the <see cref="M:OSGeo.MapGuide.MaestroAPI.IServerConnection.GetService"/>
    /// method is not necessary
    /// </remarks>
    public interface IResourceService : IService
    {
        /// <summary>
        /// Raised when a resource is added
        /// </summary>
        event ResourceEventHandler ResourceAdded;
        /// <summary>
        /// Raised when a resource is deleted. Note if a folder is deleted, this will
        /// only be raised for the folder and not its children. Also note that this is
        /// raised on any move operations as the original source is for all intents and
        /// purposes, deleted.
        /// </summary>
        event ResourceEventHandler ResourceDeleted;
        /// <summary>
        /// Raised when a resource is updated
        /// </summary>
        event ResourceEventHandler ResourceUpdated;

        /// <summary>
        /// Gets a listing of resources in this repository. This performs a full listing
        /// </summary>
        /// <returns></returns>
        ObjCommon.ResourceList GetRepositoryResources();

        /// <summary>
        /// Gets a listing of resources in this repository
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        ObjCommon.ResourceList GetRepositoryResources(int depth);

        /// <summary>
        /// Gets a listing of resources in this repository
        /// </summary>
        /// <param name="startingpoint"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        ObjCommon.ResourceList GetRepositoryResources(string startingpoint, int depth);

        /// <summary>
        /// Gets a listing of resources in this repository
        /// </summary>
        /// <param name="startingpoint"></param>
        /// <returns></returns>
        ObjCommon.ResourceList GetRepositoryResources(string startingpoint);

        /// <summary>
        /// Gets a listing of resources in this repository
        /// </summary>
        /// <param name="startingpoint"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        ObjCommon.ResourceList GetRepositoryResources(string startingpoint, string type);

        /// <summary>
        /// Gets a listing of resources in this repository
        /// </summary>
        /// <param name="startingpoint"></param>
        /// <param name="type"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        ObjCommon.ResourceList GetRepositoryResources(string startingpoint, string type, int depth);

        /// <summary>
        /// Gets a listing of resources in this repository
        /// </summary>
        /// <param name="startingpoint"></param>
        /// <param name="type"></param>
        /// <param name="depth"></param>
        /// <param name="computeChildren"></param>
        /// <returns></returns>
        ObjCommon.ResourceList GetRepositoryResources(string startingpoint, string type, int depth, bool computeChildren);

        /// <summary>
        /// Converts the specified XML stream to a strongly typed object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        T DeserializeObject<T>(System.IO.Stream data);
        
        /// <summary>
        /// Serializes the specified object to the specified stream
        /// </summary>
        /// <param name="o"></param>
        /// <param name="stream"></param>
        void SerializeObject(object o, System.IO.Stream stream);

        /// <summary>
        /// Gets the stream of the attached data of the specified resource
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="dataname"></param>
        /// <returns></returns>
        System.IO.Stream GetResourceData(string resourceID, string dataname);

        /// <summary>
        /// Gets the document header of the specified resource
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        ObjCommon.ResourceDocumentHeaderType GetResourceHeader(string resourceID);

        /// <summary>
        /// Gets the folder header of the specified resource
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        ObjCommon.ResourceFolderHeaderType GetFolderHeader(string resourceID);
        
        /// <summary>
        /// Gets the raw XML stream of the specified resource id
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        Stream GetResourceXmlData(string resourceID);

        /// <summary>
        /// Gets a typed resource object from the specified resource id
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        IResource GetResource(string resourceID);

        /// <summary>
        /// Forces a timestamp update of the specified resource. This is akin to 
        /// setting the resource's content using its existing content.
        /// </summary>
        /// <param name="resourceID"></param>
        void Touch(string resourceID);

        /// <summary>
        /// Sets the resource data of a specified resource
        /// </summary>
        /// <remarks>For the HTTP implementation of this API, the input stream must be seekable</remarks>
        /// <param name="resourceid"></param>
        /// <param name="dataname"></param>
        /// <param name="datatype"></param>
        /// <param name="stream"></param>
        void SetResourceData(string resourceid, string dataname, ObjCommon.ResourceDataType datatype, System.IO.Stream stream);
        
        /// <summary>
        /// Sets the resource data of a specified resource
        /// </summary>
        /// <remarks>For the HTTP implementation of this API, the input stream must be seekable</remarks>
        /// <param name="resourceID"></param>
        /// <param name="dataName"></param>
        /// <param name="dataType"></param>
        /// <param name="stream"></param>
        /// <param name="callback"></param>
        void SetResourceData(string resourceID, string dataName, ObjCommon.ResourceDataType dataType, System.IO.Stream stream, Utility.StreamCopyProgressDelegate callback);

        /// <summary>
        /// Sets the raw XML data of the specified resource
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="stream"></param>
        void SetResourceXmlData(string resourceID, System.IO.Stream stream);

        /// <summary>
        /// Sets the header for the specified folder
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="header"></param>
        void SetFolderHeader(string resourceID, ObjCommon.ResourceFolderHeaderType header);
        
        /// <summary>
        /// Sets the header for the specified resource
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="header"></param>
        void SetResourceHeader(string resourceID, ObjCommon.ResourceDocumentHeaderType header);

        /// <summary>
        /// Updates the repository
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="header"></param>
        void UpdateRepository(string resourceID, ObjCommon.ResourceFolderHeaderType header);

        /// <summary>
        /// Deletes the specified attached resource data
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="dataname"></param>
        void DeleteResourceData(string resourceID, string dataname);

        /// <summary>
        /// Gets a listing of all resource data attached to the specified resource
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        ObjCommon.ResourceDataList EnumerateResourceData(string resourceID);

        /// <summary>
        /// Delete the specified resource. For folders, ensure the resource ID has a trailing slash "/"
        /// </summary>
        /// <param name="resourceID"></param>
        void DeleteResource(string resourceID);

        /// <summary>
        /// Gets a listing of all resources dependent on the specified resource
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        ObjCommon.ResourceReferenceList EnumerateResourceReferences(string resourceID);

        /// <summary>
        /// Copies the specified resource to the specified path
        /// </summary>
        /// <param name="oldResourceID"></param>
        /// <param name="newResourceID"></param>
        /// <param name="overwrite"></param>
        void CopyResource(string oldResourceID, string newResourceID, bool overwrite);

        /// <summary>
        /// Moves the specified resources to the specified path
        /// </summary>
        /// <param name="oldResourceID"></param>
        /// <param name="newResourceID"></param>
        /// <param name="overwrite"></param>
        void MoveResource(string oldResourceID, string newResourceID, bool overwrite);
        
        /// <summary>
        /// Moves the specified resources to the specified path. Any resources referencing this resource
        /// are updated to reference the resource's new location
        /// </summary>
        /// <param name="oldResourceID"></param>
        /// <param name="newResourceID"></param>
        /// <param name="callback"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        bool MoveResourceWithReferences(string oldResourceID, string newResourceID, LengthyOperationCallBack callback, LengthyOperationProgressCallBack progress);
        
        /// <summary>
        /// Copies the specified folder to the specified path. Any resources referencing this folder 
        /// are updated to reference the resources's new location
        /// </summary>
        /// <param name="oldResourceID"></param>
        /// <param name="newResourceID"></param>
        /// <param name="callback"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        bool CopyFolderWithReferences(string oldResourceID, string newResourceID, LengthyOperationCallBack callback, LengthyOperationProgressCallBack progress);

        /// <summary>
        /// Gets whether the specified resource id exists
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        bool ResourceExists(string resourceID);
        
        /// <summary>
        /// Saves an object into the repository
        /// </summary>
        /// <param name="resource"></param>
        void SaveResource(IResource resource);
        /// <summary>
        /// Saves an object into the repository using the specified resource id.
        /// </summary>
        /// <remarks>
        /// The <paramref name="resourceID"/> parameter only instructs this method where to save the resource to. It does
        /// not modify the <see cref="P:IResource.ResourceID"/> property of the input resource does not get
        /// updated as a result of this operation.
        /// </remarks>
        /// <param name="resource">The object to save</param>
        /// <param name="resourceID">The resourceId to save the object as</param>
        void SaveResourceAs(IResource resource, string resourceID);

        /// <summary>
        /// Enumerates all unmanaged folders, meaning alias'ed folders
        /// </summary>
        /// <param name="startpath">The path to retrieve the data from</param>
        /// <param name="filter">A filter applied to the items</param>
        /// <param name="recursive">True if the list should contains recursive results</param>
        /// <param name="type">The type of data to return</param>
        /// <returns>A list of unmanaged data</returns>
        ObjCommon.UnmanagedDataList EnumerateUnmanagedData(string startpath, string filter, bool recursive, UnmanagedDataTypes type);

        /// <summary>
        /// Upload a MapGuide Package file to the server
        /// </summary>
        /// <param name="fileName">Name of the file to upload</param>
        /// <param name="callback">A callback argument used to display progress. May be null.</param>
        void UploadPackage(string fileName, Utility.StreamCopyProgressDelegate callback);
    }
}
