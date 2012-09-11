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
using OSGeo.MapGuide.MaestroAPI.Resource.Conversion;
using System.IO;

namespace OSGeo.MapGuide.MaestroAPI.CrossConnection
{
    /// <summary>
    /// A helper class that copies/moves resources from one
    /// connection to another
    /// </summary>
    /// <example>
    /// This example shows how to copy resources from one connection to another using
    /// the <see cref="T:OSGeo.MapGuide.MaestroAPI.CrossConnection.ResourceMigrator"/> class
    /// <code>
    /// <![CDATA[
    /// IServerConnection sourceConn;
    /// IServerConnection targetConn;
    /// ...
    /// ResourceMigrator migrator = new ResourceMigrator(sourceConn, targetConn);
    /// //These are the source resource ids to copy
    /// string [] sourceIds = new string[] {
    ///     "Library://Samples/Sheboyan/Data/Parcels.FeatureSource",
    ///     "Library://Samples/Sheboyan/Data/Rail.FeatureSource",
    ///     "Library://Samples/Sheboyan/Data/Islands.FeatureSource",
    ///     "Library://Samples/Sheboyan/Data/Buildings.FeatureSource"
    /// };
    /// //These the the target ids we are copying to. The number of source and
    /// //target ids must be the same
    /// string [] targetIds = new string[] {
    ///     "Library://Production/Data/Parcels.FeatureSource",
    ///     "Library://Production/Data/Rail.FeatureSource",
    ///     "Library://Production/Data/Islands.FeatureSource",
    ///     "Library://Production/Data/Buildings.FeatureSource"
    /// };
    /// //Set up the re-base options. This is generally the common parent of the source and target ids.
    /// //This is to ensure that any resource ids references are updated as they are copied across
    /// RebaseOptions options = new RebaseOptions("Library://Samples/Sheboygan/", "Library://Production/");
    /// bool bOverwrite = true;
    /// //Execute the migration
    /// string [] copied = migrator.CopyResources(sourceIds, targetIds, bOverwrite, options, null);
    /// ]]>
    /// </code>
    /// </example>
    public class ResourceMigrator
    {
        private IServerConnection _source;
        private IServerConnection _target;

        private IResourceConverter _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceMigrator"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public ResourceMigrator(IServerConnection source, IServerConnection target)
        {
            Check.NotNull(source, "source"); //NOXLATE
            Check.NotNull(target, "target"); //NOXLATE
            _source = source;
            _target = target;
            _converter = new ResourceObjectConverter();
        }

        /// <summary>
        /// Copies resource from the source connection to another connection. 
        /// </summary>
        /// <param name="sourceResourceIds">The array of source resource ids</param>
        /// <param name="targetResourceIds">The array of target resource ids to copy to. Each resource id in the source array will be copied to the corresponding resource id in the target array</param>
        /// <param name="overwrite">Indicates whether to overwrite </param>
        /// <param name="options">Re-base options</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public string[] CopyResources(string[] sourceResourceIds, string[] targetResourceIds, bool overwrite, RebaseOptions options, LengthyOperationProgressCallBack callback)
        {
            Check.NotNull(sourceResourceIds, "sourceResourceIds"); //NOXLATE
            Check.NotNull(targetResourceIds, "targetResourceIds"); //NOXLATE
            Check.Precondition(sourceResourceIds.Length == targetResourceIds.Length, "resourceIds.Length == targetResourceIds.Length"); //NOXLATE

            var copiedItems = new List<string>();
            var cb = callback;
            if (cb == null)
            {
                cb = new LengthyOperationProgressCallBack((s, e) =>
                {
                    //Do nothing
                });
            }

            var targetCaps = _target.Capabilities;

            int copied = 0;
            int unit = 100 / sourceResourceIds.Length;
            int progress = 0;

            string message = string.Empty;
            for (int i = 0; i < sourceResourceIds.Length; i++)
            {
                var srcResId = sourceResourceIds[i];
                var dstResId = targetResourceIds[i];
                
                //Get the source resource object
                IResource res = _source.ResourceService.GetResource(srcResId);

                //Skip if target exists and overwrite is not specified
                if (!overwrite && _target.ResourceService.ResourceExists(dstResId))
                {
                    progress += unit;
                    continue;
                }
                else
                {
                    //Check if downgrading is required
                    var maxVer = targetCaps.GetMaxSupportedResourceVersion(res.ResourceType);
                    if (res.ResourceVersion > maxVer)
                    {
                        res = _converter.Convert(res, maxVer);
                        cb(this, new LengthyOperationProgressArgs(string.Format(Strings.DowngradedResource, srcResId, maxVer), progress));
                    }

                    //Now rebase if rebase options supplied
                    if (options != null)
                    {
                        var rebaser = new ResourceRebaser(res);
                        res = rebaser.Rebase(options.SourceFolder, options.TargetFolder);
                    }

                    //Save resource
                    _target.ResourceService.SaveResourceAs(res, dstResId);
                    //Copy resource data
                    foreach (var data in res.EnumerateResourceData())
                    {
                        using (var stream = res.GetResourceData(data.Name))
                        {
                            if (!stream.CanSeek)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    Utility.CopyStream(stream, ms, false);
                                    ms.Position = 0L;
                                    _target.ResourceService.SetResourceData(dstResId, data.Name, data.Type, ms);
                                }
                            }
                            else
                            {
                                stream.Position = 0L;
                                _target.ResourceService.SetResourceData(dstResId, data.Name, data.Type, stream);
                            }
                        }
                    }

                    copied++;
                    message = string.Format(Strings.CopiedResourceToTarget, srcResId, dstResId);
                }
                copiedItems.Add(srcResId);
                progress += unit;
                cb(this, new LengthyOperationProgressArgs(message, progress));
            }

            return copiedItems.ToArray();
        }

        /// <summary>
        /// Copies resources from the source connection to another connection. The resources in question will
        /// be copied to the specified folder. Folder structure of the source is discarded
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <param name="folderId"></param>
        /// <param name="overwrite"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public int CopyResources(string[] resourceIds, string folderId, bool overwrite, LengthyOperationProgressCallBack callback)
        {
            Check.NotNull(resourceIds, "resourceIds"); //NOXLATE
            Check.NotEmpty(folderId, "folderId"); //NOXLATE

            var cb = callback;
            if (cb == null)
            {
                cb = new LengthyOperationProgressCallBack((s, e) =>
                {
                    //Do nothing
                });
            }

            var targetCaps = _target.Capabilities;

            int copied = 0;
            int unit = 100 / resourceIds.Length;
            int progress = 0;
            foreach (var resId in resourceIds)
            {
                string targetId = folderId + ResourceIdentifier.GetName(resId) + "." + ResourceIdentifier.GetResourceType(resId); //NOXLATE
                string message = string.Empty;
                IResource res = _source.ResourceService.GetResource(resId);

                //Skip if target exists and overwrite is not specified
                if (!overwrite && _target.ResourceService.ResourceExists(targetId))
                {
                    progress += unit;
                    continue;
                }
                else
                {
                    //Check if downgrading is required
                    var maxVer = targetCaps.GetMaxSupportedResourceVersion(res.ResourceType);
                    if (res.ResourceVersion > maxVer)
                    {
                        res = _converter.Convert(res, maxVer);
                        cb(this, new LengthyOperationProgressArgs(string.Format(Strings.DowngradedResource, resId, maxVer), progress));
                    }

                    //Save resource
                    _target.ResourceService.SaveResourceAs(res, targetId);
                    //Copy resource data
                    foreach (var data in res.EnumerateResourceData())
                    {
                        using (var stream = res.GetResourceData(data.Name))
                        {
                            if (!stream.CanSeek)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    Utility.CopyStream(stream, ms, false);
                                    ms.Position = 0L;
                                    _target.ResourceService.SetResourceData(targetId, data.Name, data.Type, ms);
                                }
                            }
                            else
                            {
                                stream.Position = 0L;
                                _target.ResourceService.SetResourceData(targetId, data.Name, data.Type, stream);
                            }
                        }
                    }

                    copied++;
                    message = string.Format(Strings.CopiedResource, resId);
                }
                progress += unit;
                cb(this, new LengthyOperationProgressArgs(message, progress));
            }
            return copied;
        }

        /// <summary>
        /// Moves resources from the source connection to the specified folder on the target connection. Folder structure of the source is discarded
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <param name="folderId"></param>
        /// <param name="overwrite"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public int MoveResources(string[] resourceIds, string folderId, bool overwrite, LengthyOperationProgressCallBack callback)
        {
            Check.NotNull(resourceIds, "resourceIds"); //NOXLATE
            Check.NotEmpty(folderId, "folderId"); //NOXLATE

            var cb = callback;
            if (cb == null)
            {
                cb = new LengthyOperationProgressCallBack((s, e) =>
                {
                    //Do nothing
                });
            }

            var targetCaps = _target.Capabilities;

            int moved = 0;
            int unit = 100 / resourceIds.Length;
            int progress = 0;
            foreach (var resId in resourceIds)
            {
                string targetId = folderId + ResourceIdentifier.GetName(resId) + "." + ResourceIdentifier.GetResourceType(resId); //NOXLATE
                string message = string.Empty;
                IResource res = _source.ResourceService.GetResource(resId);

                //Skip if target exists and overwrite is not specified
                if (!overwrite && _target.ResourceService.ResourceExists(targetId))
                {
                    progress += unit;
                    continue;
                }
                else
                {
                    //Check if downgrading is required
                    var maxVer = targetCaps.GetMaxSupportedResourceVersion(res.ResourceType);
                    if (res.ResourceVersion > maxVer)
                    {
                        res = _converter.Convert(res, maxVer);
                        cb(this, new LengthyOperationProgressArgs(string.Format(Strings.DowngradedResource, resId, maxVer), progress));
                    }

                    //Save resource
                    _target.ResourceService.SaveResourceAs(res, targetId);
                    //Copy resource data
                    foreach (var data in res.EnumerateResourceData())
                    {
                        using (var stream = res.GetResourceData(data.Name))
                        {
                            if (!stream.CanSeek)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    Utility.CopyStream(stream, ms, false);
                                    ms.Position = 0L;
                                    _target.ResourceService.SetResourceData(targetId, data.Name, data.Type, ms);
                                }
                            }
                            else
                            {
                                stream.Position = 0L;
                                _target.ResourceService.SetResourceData(targetId, data.Name, data.Type, stream);
                            }
                        }
                    }

                    moved++;
                    _source.ResourceService.DeleteResource(resId);
                    message = string.Format(Strings.CopiedResource, resId);
                }
                progress += unit;
                cb(this, new LengthyOperationProgressArgs(message, progress));
            }
            return moved;
        }

        /// <summary>
        /// Migrates a specific resource (and its dependent resources) to the target connection
        /// </summary>
        /// <param name="resourceId">The id of the resource to migrate</param>
        /// <param name="dependentResourceIds">The array of dependent resource ids</param>
        /// <param name="overwrite">If true, all dependent resources that already exist in the target connection are overwritten, otherwise these are not copied over</param>
        /// <param name="callback">A callback method to indicate progress</param>
        /// <returns>An array of resource ids that were succesfully migrated</returns>
        public string[] MigrateResource(string resourceId, string[] dependentResourceIds, bool overwrite, LengthyOperationProgressCallBack callback)
        {
            Check.NotEmpty(resourceId, "resourceId"); //NOXLATE
            Check.NotNull(dependentResourceIds, "dependentResourceIds"); //NOXLATE

            //TODO: Figure out a more elegant strategy of handling saving resources
            //to older versions (downgrading?)

            //TODO: This should not return a string array, it should return an array
            //of migration results. This requires a new API (Capability?) to test whether a resource
            //can be saved to this connection

            List<string> migrated = new List<string>();

            LengthyOperationProgressCallBack cb = callback;
            if (cb == null)
            {
                cb = new LengthyOperationProgressCallBack((o, a) => { });
            }

            var targetCaps = _target.Capabilities;
            int total = dependentResourceIds.Length + 1;
            int unit = 100 / total;
            int progress = 0;

            try
            {
                //Copy the specified resource
                IResource res = _source.ResourceService.GetResource(resourceId);

                //Check if downgrading is required
                var maxVer = targetCaps.GetMaxSupportedResourceVersion(res.ResourceType);
                if (res.ResourceVersion > maxVer)
                {
                    res = _converter.Convert(res, maxVer);
                    cb(this, new LengthyOperationProgressArgs(string.Format(Strings.DowngradedResource, resourceId, maxVer), progress));
                }
                _target.ResourceService.SaveResource(res);

                //Copy its resource data
                foreach (var data in res.EnumerateResourceData())
                {
                    using (var stream = res.GetResourceData(data.Name))
                    {
                        if (!stream.CanSeek)
                        {
                            using (var ms = new MemoryStream())
                            {
                                Utility.CopyStream(stream, ms, false);
                                ms.Position = 0L;
                                _target.ResourceService.SetResourceData(resourceId, data.Name, data.Type, ms);
                            }
                        }
                        else
                        {
                            stream.Position = 0L;
                            _target.ResourceService.SetResourceData(resourceId, data.Name, data.Type, stream);
                        }
                    }
                }

                migrated.Add(resourceId);
            }
            catch //This happens if we're saving a resource to an older version where this resource version does not exist
            {
            }

            //If the first one failed, abort early. Don't bother with the rest
            if (migrated.Count == 1)
            {
                progress += unit;
                cb(this, new LengthyOperationProgressArgs(string.Format(Strings.CopiedResource, resourceId), progress));

                //Now copy dependents
                foreach (var resId in dependentResourceIds)
                {
                    bool existsOnTarget = _target.ResourceService.ResourceExists(resId);
                    if ((existsOnTarget && overwrite) || !existsOnTarget)
                    {
                        try
                        {
                            //Copy the specified resource
                            IResource res = _source.ResourceService.GetResource(resId);
                            _target.ResourceService.SaveResource(res);

                            //Copy its resource data
                            foreach (var data in res.EnumerateResourceData())
                            {
                                using (var stream = res.GetResourceData(data.Name))
                                {
                                    if (!stream.CanSeek)
                                    {
                                        using (var ms = new MemoryStream())
                                        {
                                            Utility.CopyStream(stream, ms, false);
                                            ms.Position = 0L;
                                            _target.ResourceService.SetResourceData(resId, data.Name, data.Type, ms);
                                        }
                                    }
                                    else
                                    {
                                        stream.Position = 0L;
                                        _target.ResourceService.SetResourceData(resId, data.Name, data.Type, stream);
                                    }
                                }
                            }

                            migrated.Add(resId);
                        }
                        catch //This happens if we're saving a resource to an older version where this resource version does not exist
                        {

                        }

                        progress += unit;
                        cb(this, new LengthyOperationProgressArgs(string.Format(Strings.CopiedResource, resId), progress));
                    }
                }
            }
            return migrated.ToArray();
        }

        /// <summary>
        /// Shortcut API to migrate a specific resource to the target connection. Dependent resources are automatically
        /// migrated as well. This copies all dependent resources of the specified resource. 
        /// </summary>
        /// <param name="resourceId">The id of the resource to migrate</param>
        /// <param name="overwrite">If true, all dependent resources that already exist in the target connection are overwritten, otherwise these are not copied over</param>
        /// <param name="callback">A callback method to indicate progress</param>
        /// <returns>An array of resource ids that were succesfully migrated</returns>
        public string[] MigrateResource(string resourceId, bool overwrite, LengthyOperationProgressCallBack callback)
        {
            Check.NotEmpty(resourceId, "resourceId"); //NOXLATE
            Dictionary<string, string> resIds = new Dictionary<string, string>();
            var refList = GetReverseReferences(resourceId);
            BuildFullDependencyList(resIds, refList);

            return MigrateResource(resourceId, new List<string>(resIds.Keys).ToArray(), overwrite, callback);
        }

        private List<string> GetReverseReferences(string id)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            using (var ms = _source.ResourceService.GetResourceXmlData(id))
            {
                doc.Load(ms);
            }

            List<KeyValuePair<System.Xml.XmlNode, string>> refs = Utility.GetResourceIdPointers(doc);
            List<string> dependentIds = new List<string>();
            foreach (KeyValuePair<System.Xml.XmlNode, string> s in refs)
                if (!dependentIds.Contains(s.Value))
                    dependentIds.Add(s.Value);
            return dependentIds;
        }

        private void BuildFullDependencyList(Dictionary<string, string> resIds, IEnumerable<string> resourceIds)
        {
            foreach (var id in resourceIds)
            {
                resIds[id] = id;
                var refList = GetReverseReferences(id);
                BuildFullDependencyList(resIds, refList);
            }
        }

        /// <summary>
        /// Gets the source connection
        /// </summary>
        public IServerConnection Source
        {
            get { return _source; }
        }

        /// <summary>
        /// Gets the target connection
        /// </summary>
        public IServerConnection Target
        {
            get { return _target; }
        }
    }
}
