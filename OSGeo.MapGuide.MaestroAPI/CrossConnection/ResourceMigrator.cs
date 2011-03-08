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

namespace OSGeo.MapGuide.MaestroAPI.CrossConnection
{
    /// <summary>
    /// A helper class that copies/moves resources from one
    /// connection to another
    /// </summary>
    public class ResourceMigrator
    {
        private IServerConnection _source;
        private IServerConnection _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceMigrator"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public ResourceMigrator(IServerConnection source, IServerConnection target)
        {
            Check.NotNull(source, "source");
            Check.NotNull(target, "target");
            _source = source;
            _target = target;
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
            Check.NotNull(resourceIds, "resourceIds");
            Check.NotEmpty(folderId, "folderId");

            var cb = callback;
            if (cb == null)
            {
                cb = new LengthyOperationProgressCallBack((s, e) =>
                {
                    //Do nothing
                });
            }

            int copied = 0;
            int unit = 100 / resourceIds.Length;
            int progress = 0;
            foreach (var resId in resourceIds)
            {
                string targetId = folderId + ResourceIdentifier.GetName(resId) + "." + ResourceIdentifier.GetResourceType(resId);
                string message = "";
                IResource res = _source.ResourceService.GetResource(resId);

                //Skip if target exists and overwrite is not specified
                if (!overwrite && _target.ResourceService.ResourceExists(targetId))
                {
                    continue;
                }
                else
                {
                    //Save resource
                    _target.ResourceService.SaveResourceAs(res, targetId);
                    //Copy resource data
                    foreach (var data in res.EnumerateResourceData())
                    {
                        using (var stream = res.GetResourceData(data.Name))
                        {
                            stream.Position = 0L;
                            _target.ResourceService.SetResourceData(targetId, data.Name, data.Type, stream);
                        }
                    }

                    copied++;
                    message = string.Format(Properties.Resources.CopiedResource, resId);
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
            Check.NotNull(resourceIds, "resourceIds");
            Check.NotEmpty(folderId, "folderId");

            var cb = callback;
            if (cb == null)
            {
                cb = new LengthyOperationProgressCallBack((s, e) =>
                {
                    //Do nothing
                });
            }

            int moved = 0;
            int unit = 100 / resourceIds.Length;
            int progress = 0;
            foreach (var resId in resourceIds)
            {
                string targetId = folderId + ResourceIdentifier.GetName(resId) + "." + ResourceIdentifier.GetResourceType(resId);
                string message = "";
                IResource res = _source.ResourceService.GetResource(resId);

                //Skip if target exists and overwrite is not specified
                if (!overwrite && _target.ResourceService.ResourceExists(targetId))
                {
                    continue;
                }
                else
                {
                    //Save resource
                    _target.ResourceService.SaveResourceAs(res, targetId);
                    //Copy resource data
                    foreach (var data in res.EnumerateResourceData())
                    {
                        using (var stream = res.GetResourceData(data.Name))
                        {
                            stream.Position = 0L;
                            _target.ResourceService.SetResourceData(targetId, data.Name, data.Type, stream);
                        }
                    }

                    moved++;
                    _source.ResourceService.DeleteResource(resId);
                    message = string.Format(Properties.Resources.CopiedResource, resId);
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
            Check.NotEmpty(resourceId, "resourceId");
            Check.NotNull(dependentResourceIds, "dependentResourceIds");

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

            int total = dependentResourceIds.Length + 1;
            int unit = 100 / total;
            int progress = 0;

            try
            {
                //Copy the specified resource
                IResource res = _source.ResourceService.GetResource(resourceId);
                _target.ResourceService.SaveResource(res);

                //Copy its resource data
                foreach (var data in res.EnumerateResourceData())
                {
                    using (var stream = res.GetResourceData(data.Name))
                    {
                        stream.Position = 0L;
                        _target.ResourceService.SetResourceData(resourceId, data.Name, data.Type, stream);
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
                cb(this, new LengthyOperationProgressArgs(string.Format(Properties.Resources.CopiedResource, resourceId), progress));

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
                                    stream.Position = 0L;
                                    _target.ResourceService.SetResourceData(resId, data.Name, data.Type, stream);
                                }
                            }

                            migrated.Add(resId);
                        }
                        catch //This happens if we're saving a resource to an older version where this resource version does not exist
                        {

                        }

                        progress += unit;
                        cb(this, new LengthyOperationProgressArgs(string.Format(Properties.Resources.CopiedResource, resId), progress));
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
            Check.NotEmpty(resourceId, "resourceId");
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
