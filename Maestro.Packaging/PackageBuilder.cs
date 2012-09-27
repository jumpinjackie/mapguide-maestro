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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Services;
using ICSharpCode.SharpZipLib.Zip;
using System.Xml;
using System.Collections.Specialized;
using System.IO;

namespace Maestro.Packaging
{
    /// <summary>
    /// Enumeration used to signal the type of operation currently running
    /// </summary>
    public enum ProgressType
    {
        /// <summary>
        /// The file list is being fetched from MapGuide
        /// </summary>
        ReadingFileList,
        /// <summary>
        /// Files are downloaded in temporary folder
        /// </summary>
        PreparingFolder,
        /// <summary>
        /// Resource references are updated to use the new folder
        /// </summary>
        MovingResources,
        /// <summary>
        /// The files are being compressed
        /// </summary>
        Compressing,
        /// <summary>
        /// The package opertion has completed
        /// </summary>
        Done,
        /// <summary>
        /// The package is being uploaded
        /// </summary>
        Uploading,
        /// <summary>
        /// Extracting filenames from package
        /// </summary>
        ListingFiles,
        /// <summary>
        /// Setting resource content
        /// </summary>
        SetResource,
        /// <summary>
        /// Setting resource data
        /// </summary>
        SetResourceData
    }

    /// <summary>
    /// Defines the type of entry
    /// </summary>
    public enum EntryTypeEnum
    {
        /// <summary>
        /// The item already exists in the package
        /// </summary>
        Regular,
        /// <summary>
        /// The item is deleted from the package
        /// </summary>
        Deleted,
        /// <summary>
        /// The item is added to the package
        /// </summary>
        Added
    }

    /// <summary>
    /// A delegate for reporting package creation progress
    /// </summary>
    /// <param name="type">The progress type that is currently running</param>
    /// <param name="maxValue">The max value, meaning that when value equals maxValue, progress is equal to 100%</param>
    /// <param name="value">The current item being progressed</param>
    /// <param name="resourceId">The name of the resource being processed, if any</param>
    public delegate void ProgressDelegate(ProgressType type, string resourceId, int maxValue, double value);

    /// <summary>
    /// A class to create MapGuide data packages
    /// </summary>
    public class PackageBuilder
    {
        /// <summary>
        /// The connection object
        /// </summary>
        private IServerConnection m_connection;

        /// <summary>
        /// Constructs a new package builder instance
        /// </summary>
        /// <param name="connection">The connection used to serialize and fetch items</param>
        public PackageBuilder(IServerConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection"); //NOXLATE
            m_connection = connection;
        }

        /// <summary>
        /// This event is invoked to report progress to the caller
        /// </summary>
        public event ProgressDelegate Progress;

        /// <summary>
        /// Keep track of the last pg sent, to avoid excessive events
        /// </summary>
        private long m_lastPg = -1;

        /// <summary>
        /// Uploads a package to the server
        /// </summary>
        /// <param name="sourceFile"></param>
        public void UploadPackage(string sourceFile)
        {
            if (Progress != null)
                Progress(ProgressType.Uploading, sourceFile, 100, 0);
            
            m_lastPg = -1;
            m_connection.ResourceService.UploadPackage(sourceFile, new Utility.StreamCopyProgressDelegate(ProgressCallback_Upload));

            if (Progress != null)
                Progress(ProgressType.Uploading, sourceFile, 100, 100);
        }

        /// <summary>
        /// Uploads a package to the server in a non-transactional fashion. Resources which fail to load are added to the specified list of 
        /// failed resources. The upload is non-transactional in the sense that it can partially fail. Failed operations are logged.
        /// </summary>
        /// <param name="sourceFile">The source package file</param>
        /// <param name="result">An <see cref="T:Maestro.Packaging.UploadPackageResult"/> object containing an optional list of operations to skip. It will be populated with the list of operations that passed and failed as the process executes</param>
        public void UploadPackageNonTransactional(string sourceFile, UploadPackageResult result)
        {
            Dictionary<PackageOperation, PackageOperation> skipOps = new Dictionary<PackageOperation, PackageOperation>();
            if (result.SkipOperations.Count > 0)
            {
                foreach (var op in result.SkipOperations)
                {
                    skipOps[op] = op;
                }
            }

            ProgressDelegate progress = this.Progress;
            if (progress == null)
                progress = (type, file, a, b) => { };

            double step = 0.0;
            progress(ProgressType.ListingFiles, sourceFile, 100, step);
            
            //Process overview:
            //
            // 1. Extract the package to a temp directory
            // 2. Read the package manifest
            // 3. For each resource id in the manifest, if it is in the list of resource ids to skip 
            //    then skip it. Otherwise process the directive that uses this id.

            ZipFile package = new ZipFile(sourceFile);
            ZipEntry manifestEntry = package.GetEntry("MgResourcePackageManifest.xml"); //NOXLATE
            XmlDocument doc = new XmlDocument();
            using (var s = package.GetInputStream(manifestEntry))
            {
                doc.Load(s);
            }
            XmlNodeList opNodes = doc.GetElementsByTagName("Operation"); //NOXLATE
            double unit = (100.0 / (double)opNodes.Count);
            foreach (XmlNode opNode in opNodes)
            {
                step += unit;
                string name = opNode["Name"].InnerText.ToUpper(); //NOXLATE

                PackageOperation op = ParseOperation(opNode);
                //TODO: A DELETERESOURCE would cause a null operation. Should we bother to support it?
                if (op == null)
                    continue;

                //Is a skipped operation?
                if (skipOps.ContainsKey(op))
                {
                    System.Diagnostics.Trace.TraceInformation("Skipping " + op.OperationName + " on " + op.ResourceId); //NOXLATE
                    continue;
                }

                switch (name)
                {
                    case "SETRESOURCE": //NOXLATE
                        {
                            SetResourcePackageOperation sop = (SetResourcePackageOperation)op;
                            if (sop.Content == null)
                            {
                                skipOps[sop] = sop;
                            }
                            else
                            {
                                ZipEntry contentEntry = package.GetEntry(sop.Content);
                                ZipEntry headerEntry = null;

                                if (!string.IsNullOrEmpty(sop.Header))
                                    headerEntry = package.GetEntry(sop.Header);

                                try
                                {
                                    using (var s = package.GetInputStream(contentEntry))
                                    {
                                        m_connection.ResourceService.SetResourceXmlData(op.ResourceId, s);
                                        progress(ProgressType.SetResource, op.ResourceId, 100, step);
                                    }

                                    if (headerEntry != null)
                                    {
                                        using (var s = package.GetInputStream(headerEntry))
                                        {
                                            using (var sr = new StreamReader(s))
                                            {
                                                ResourceDocumentHeaderType header = ResourceDocumentHeaderType.Deserialize(sr.ReadToEnd());
                                                m_connection.ResourceService.SetResourceHeader(op.ResourceId, header);
                                                progress(ProgressType.SetResource, op.ResourceId, 100, step);
                                            }
                                        }
                                    }

                                    result.Successful.Add(op);
                                }
                                catch (Exception ex)
                                {
                                    //We don't really care about the header. We consider failure if the
                                    //content upload did not succeed
                                    if (!m_connection.ResourceService.ResourceExists(op.ResourceId))
                                        result.Failed.Add(op, ex);
                                }
                            }
                        }
                        break;
                    case "SETRESOURCEDATA": //NOXLATE
                        {
                            SetResourceDataPackageOperation sop = (SetResourceDataPackageOperation)op;
                            ZipEntry dataEntry = package.GetEntry(sop.Data);

                            try
                            {
                                using (var s = package.GetInputStream(dataEntry))
                                {
                                    m_connection.ResourceService.SetResourceData(sop.ResourceId, sop.DataName, sop.DataType, s);
                                    progress(ProgressType.SetResourceData, sop.ResourceId, 100, step);
                                }

                                result.Successful.Add(op);
                            }
                            catch (Exception ex)
                            {
                                var resData = m_connection.ResourceService.EnumerateResourceData(sop.ResourceId);
                                bool found = false;

                                foreach (var data in resData.ResourceData)
                                {
                                    if (data.Name == sop.DataName)
                                    {
                                        found = true;
                                        break;
                                    }
                                }

                                if (!found)
                                    result.Failed.Add(sop, ex);
                            }
                        }
                        break;
                }
            }
        }

        private static PackageOperation ParseOperation(XmlNode opNode)
        {
            PackageOperation op = null;
            NameValueCollection p = new NameValueCollection();
            foreach (XmlNode paramNode in opNode["Parameters"].ChildNodes) //NOXLATE
            {
                p[paramNode["Name"].InnerText] = paramNode["Value"].InnerText; //NOXLATE
            }
            string resourceId = p["RESOURCEID"]; //NOXLATE
            switch (opNode["Name"].InnerText) //NOXLATE
            {
                case "SETRESOURCE": //NOXLATE
                    {
                        op = new SetResourcePackageOperation(resourceId, p["CONTENT"], p["HEADER"]); //NOXLATE
                    }
                    break;
                case "SETRESOURCEDATA": //NOXLATE
                    {
                        ResourceDataType rdt;
                        try
                        {
                            rdt = (ResourceDataType)Enum.Parse(typeof(ResourceDataType), p["DATATYPE"], true); //NOXLATE
                        }
                        catch { rdt = ResourceDataType.File; }
                        op = new SetResourceDataPackageOperation(resourceId, p["DATA"], p["DATANAME"], rdt); //NOXLATE
                    }
                    break;
            }
            return op;
        }

        private void ProgressCallback_Upload(long copied, long remain, long total)
        {
            if (Progress != null)
            {
                if (m_lastPg < 0 || remain == 0 || copied - m_lastPg > 1024 * 50)
                {
                    Progress(ProgressType.Uploading, string.Empty, (int)(total / 1024), (int)(copied / 1024));
                    m_lastPg = copied;
                }
            }
        }

        /// <summary>
        /// Creates a package
        /// </summary>
        /// <param name="folderResourceId">The folder to create the package from</param>
        /// <param name="zipfilename">The name of the output file to create</param>
        /// <param name="allowedExtensions">A list of allowed extensions without leading dot, or null to include all file types. The special item &quot;*&quot; matches all unknown types.</param>
        /// <param name="removeExistingFiles">A value indicating if a delete operation is included in the package to remove existing files before restoring the package</param>
        /// <param name="alternateTargetResourceId">An optional target folder resourceId, use null or an empty string to restore the files at the original locations</param>
        public void CreatePackage(string folderResourceId, string zipfilename, IEnumerable<ResourceTypes> allowedExtensions, bool removeExistingFiles, string alternateTargetResourceId)
        {
            if (Progress != null)
                Progress(ProgressType.ReadingFileList, folderResourceId, 100, 0);

            ResourceList items = m_connection.ResourceService.GetRepositoryResources(folderResourceId);

            CreatePackageInternal(folderResourceId, zipfilename, allowedExtensions, removeExistingFiles, alternateTargetResourceId, items.Children.Select(x => x.ResourceId));
        }

        /// <summary>
        /// Creates a package
        /// </summary>
        /// <param name="resourceIdsToPack">The list of resource ids to include into the package</param>
        /// <param name="zipfilename">The name of the output file to create</param>
        /// <param name="allowedExtensions">A list of allowed extensions without leading dot, or null to include all file types. The special item &quot;*&quot; matches all unknown types.</param>
        /// <param name="removeExistingFiles">A value indicating if a delete operation is included in the package to remove existing files before restoring the package</param>
        /// <param name="alternateTargetResourceId">An optional target folder resourceId, use null or an empty string to restore the files at the original locations</param>
        public void CreatePackage(IEnumerable<string> resourceIdsToPack, string zipfilename, IEnumerable<ResourceTypes> allowedExtensions, bool removeExistingFiles, string alternateTargetResourceId)
        {
            if (Progress != null)
                Progress(ProgressType.ReadingFileList, string.Empty, 100, 0);

            var resourceIds = new List<string>(resourceIdsToPack);
            string folderId = GetCommonParent(resourceIds);

            CreatePackageInternal(folderId, zipfilename, allowedExtensions, removeExistingFiles, alternateTargetResourceId, resourceIds);
        }

        private static string GetCommonParent(ICollection<string> data)
        {
            if (data.Count > 0)
            {
                var firstResId = new ResourceIdentifier(data.ElementAt(0));
                if (data.Count == 1)
                {
                    if (firstResId.IsFolder)
                        return firstResId.ResourceId.ToString();
                    else
                        return firstResId.ParentFolder;
                }
                else
                {
                    int matches = 0;
                    string[] parts = firstResId.ResourceId.ToString()
                                               .Substring(StringConstants.RootIdentifier.Length)
                                               .Split('/'); //NOXLATE
                    string test = StringConstants.RootIdentifier;
                    string parent = test;
                    int partIndex = 0;
                    //Use first one as a sample to see how far we can go. Keep going until we have
                    //a parent that doesn't match all of them. The one we recorded before then will
                    //be the common parent
                    while (matches == data.Count)
                    {
                        parent = test;
                        partIndex++;
                        if (partIndex < parts.Length) //Shouldn't happen, but just in case
                            break;

                        test = test + parts[partIndex];
                        matches = data.Where(x => x.StartsWith(test)).Count();
                    }
                    return parent;
                }
            }
            else
            {
                return StringConstants.RootIdentifier;
            }
        }

        private void CreatePackageInternal(string folderResourceId, string zipfilename, IEnumerable<ResourceTypes> allowedExtensions, bool removeExistingFiles, string alternateTargetResourceId, IEnumerable<string> resourceIds)
        {
            ResourcePackageManifest manifest = new ResourcePackageManifest();
            manifest.Description = "MapGuide Package created with Maestro"; //NOXLATE
            manifest.Operations = new ResourcePackageManifestOperations();
            manifest.Operations.Operation = new System.ComponentModel.BindingList<ResourcePackageManifestOperationsOperation>();

            var allowed = new List<ResourceTypes>(allowedExtensions);
            var files = new List<string>();
            var folders = new List<string>();
            var resourceData = new Dictionary<string, List<ResourceDataListResourceData>>();

            foreach (var resId in resourceIds)
            {
                if (!ResourceIdentifier.Validate(resId))
                    continue;

                var r = new ResourceIdentifier(resId);
                if (r.IsFolder)
                {
                    folders.Add(resId);
                }
                else
                {
                    var extension = r.ResourceType;
                    if (allowedExtensions == null || allowed.Count == 0)
                        files.Add(resId);
                    else if (m_connection.Capabilities.IsSupportedResourceType(extension) && allowed.Contains(extension))
                        files.Add(resId);
                }
            }

            if (Progress != null)
            {
                Progress(ProgressType.ReadingFileList, folderResourceId, 100, 100);
                Progress(ProgressType.PreparingFolder, string.Empty, files.Count + folders.Count + 1, 0);
            }

            string temppath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());

            //All files have random names on disk, but a full path in the zip file
            List<KeyValuePair<string, string>> filemap = new List<KeyValuePair<string, string>>();

            try
            {
                System.IO.Directory.CreateDirectory(temppath);
                int opno = 1;

                foreach (var folder in folders)
                {

                    if (Progress != null)
                        Progress(ProgressType.PreparingFolder, folder, files.Count + folders.Count + 1, opno);
                    AddFolderResource(manifest, temppath, folder, removeExistingFiles, m_connection, filemap);
                    if (Progress != null)
                        Progress(ProgressType.PreparingFolder, folder, files.Count + folders.Count + 1, opno++);
                }

                foreach (var doc in files)
                {
                    if (Progress != null)
                        Progress(ProgressType.PreparingFolder, doc, files.Count + folders.Count + 1, opno);
                    string filebase = CreateFolderForResource(doc, temppath);

                    resourceData[doc] = new List<ResourceDataListResourceData>();
                    ResourceDataList rdl = m_connection.ResourceService.EnumerateResourceData(doc);
                    foreach (ResourceDataListResourceData rd in rdl.ResourceData)
                        resourceData[doc].Add(rd);

                    int itemCount = resourceData[doc].Count + 1;

                    filemap.Add(new KeyValuePair<string, string>(filebase + "_CONTENT.xml", System.IO.Path.Combine(temppath, Guid.NewGuid().ToString()))); //NOXLATE
                    using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    {
                        using (var s = m_connection.ResourceService.GetResourceXmlData(doc))
                        {
                            var data = Utility.StreamAsArray(s);
                            fs.Write(data, 0, data.Length);
                        }
                    }

                    AddFileResource(manifest, temppath, doc, filemap[filemap.Count - 1].Key, removeExistingFiles, m_connection, filemap);

                    foreach (ResourceDataListResourceData rd in rdl.ResourceData)
                    {
                        filemap.Add(new KeyValuePair<string, string>(filebase + "_DATA_" + EncodeFilename(rd.Name), System.IO.Path.Combine(temppath, Guid.NewGuid().ToString())));
                        System.IO.FileInfo fi = new System.IO.FileInfo(filemap[filemap.Count - 1].Value);
                        using (System.IO.FileStream fs = new System.IO.FileStream(fi.FullName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                        {
                            Utility.CopyStream(m_connection.ResourceService.GetResourceData(doc, rd.Name), fs);
                        }

                        AddResourceData(manifest, temppath, doc, fi, filemap[filemap.Count - 1].Key, rd, m_connection);
                    }

                    if (Progress != null)
                        Progress(ProgressType.PreparingFolder, doc, files.Count + folders.Count + 1, opno++);
                }

                if (Progress != null)
                    Progress(ProgressType.PreparingFolder, Strings.ProgressDone, files.Count + folders.Count + 1, files.Count + folders.Count + 1);

                if (!string.IsNullOrEmpty(alternateTargetResourceId))
                {
                    if (Progress != null)
                        Progress(ProgressType.MovingResources, Strings.ProgressUpdatingReferences, 100, 0);
                    RemapFiles(m_connection, manifest, temppath, folderResourceId, alternateTargetResourceId, filemap);
                    if (Progress != null)
                        Progress(ProgressType.MovingResources, Strings.ProgressUpdatedReferences, 100, 100);
                }

                filemap.Add(new KeyValuePair<string, string>(System.IO.Path.Combine(temppath, "MgResourcePackageManifest.xml"), System.IO.Path.Combine(temppath, Guid.NewGuid().ToString()))); //NOXLATE
                using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    m_connection.ResourceService.SerializeObject(manifest, fs);


                if (Progress != null)
                    Progress(ProgressType.MovingResources, zipfilename, filemap.Count, 0);

                ZipDirectory(zipfilename, temppath, "MapGuide Package created by Maestro", filemap); //NOXLATE

                if (Progress != null)
                {
                    Progress(ProgressType.MovingResources, zipfilename, filemap.Count, filemap.Count);
                    Progress(ProgressType.Done, "", filemap.Count, filemap.Count);
                }

            }
            finally
            {
                try
                {
                    if (System.IO.Directory.Exists(temppath))
                        System.IO.Directory.Delete(temppath, true);
                }
                catch
                { }
            }
        }

        private void AddResourceData(ResourcePackageManifest manifest, string temppath, string docResourceId, System.IO.FileInfo fi, string resourcePath, ResourceDataListResourceData rd, IServerConnection connection)
        {
            string contentType = "application/octet-stream"; //NOXLATE

            string name = rd.Name;
            string type = rd.Type.ToString();
            string resourceId = docResourceId;
            string filename = RelativeName(resourcePath, temppath).Replace('\\', '/'); //NOXLATE
            long size = fi.Length;

            AddResourceData(manifest, resourceId, contentType, type, name, filename, size);
        }

        private void AddResourceData(ResourcePackageManifest manifest, string resourceId, string contentType, string type, string name, string filename, long size)
        {
            ResourcePackageManifestOperationsOperation op = new ResourcePackageManifestOperationsOperation();
            op.Name = "SETRESOURCEDATA"; //NOXLATE
            op.Version = "1.0.0"; //NOXLATE
            op.Parameters = new ResourcePackageManifestOperationsOperationParameters();
            op.Parameters.Parameter = new System.ComponentModel.BindingList<ResourcePackageManifestOperationsOperationParametersParameter>();

            ResourcePackageManifestOperationsOperationParametersParameter param = new ResourcePackageManifestOperationsOperationParametersParameter();

            param.Name = "DATA"; //NOXLATE
            param.Value = filename;
            param.ContentType = contentType;
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "DATALENGTH"; //NOXLATE
            param.Value = size.ToString();
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "DATANAME"; //NOXLATE
            param.Value = name;
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "DATATYPE";
            param.Value = type;
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "RESOURCEID"; //NOXLATE
            param.Value = resourceId;
            op.Parameters.Parameter.Add(param);

            manifest.Operations.Operation.Add(op);
        }

        private void AddFileResource(ResourcePackageManifest manifest, string temppath, string docResourceId, string contentfilename, bool eraseFirst, IServerConnection connection, List<KeyValuePair<string, string>> filemap)
        {
            string filebase = CreateFolderForResource(docResourceId, temppath);

            filemap.Add(new KeyValuePair<string, string>(filebase + "_HEADER.xml", System.IO.Path.Combine(temppath, Guid.NewGuid().ToString()))); //NOXLATE
            using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                connection.ResourceService.SerializeObject(connection.ResourceService.GetResourceHeader(docResourceId), fs);

            string headerpath = RelativeName(filemap[filemap.Count - 1].Key, temppath).Replace('\\', '/'); //NOXLATE
            string contentpath = RelativeName(contentfilename, temppath).Replace('\\', '/'); //NOXLATE
            AddFileResource(manifest, docResourceId, headerpath, contentpath, eraseFirst);
        }

        private void AddFileResource(ResourcePackageManifest manifest, string resourceId, string headerpath, string contentpath, bool eraseFirst)
        {
            if (eraseFirst)
            {
                ResourcePackageManifestOperationsOperation delop = new ResourcePackageManifestOperationsOperation();
                delop.Name = "DELETERESOURCE"; //NOXLATE
                delop.Version = "1.0.0"; //NOXLATE
                delop.Parameters = new ResourcePackageManifestOperationsOperationParameters();
                delop.Parameters.Parameter = new System.ComponentModel.BindingList<ResourcePackageManifestOperationsOperationParametersParameter>();

                ResourcePackageManifestOperationsOperationParametersParameter delparam = new ResourcePackageManifestOperationsOperationParametersParameter();

                delparam.Name = "RESOURCEID"; //NOXLATE
                delparam.Value = resourceId;
                delop.Parameters.Parameter.Add(delparam);
                manifest.Operations.Operation.Add(delop);
            }

            ResourcePackageManifestOperationsOperation op = new ResourcePackageManifestOperationsOperation();
            op.Name = "SETRESOURCE"; //NOXLATE
            op.Version = "1.0.0"; //NOXLATE
            op.Parameters = new ResourcePackageManifestOperationsOperationParameters();
            op.Parameters.Parameter = new System.ComponentModel.BindingList<ResourcePackageManifestOperationsOperationParametersParameter>();

            ResourcePackageManifestOperationsOperationParametersParameter param = new ResourcePackageManifestOperationsOperationParametersParameter();

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "CONTENT"; //NOXLATE
            param.Value = contentpath;
            param.ContentType = "text/xml"; //NOXLATE
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "HEADER"; //NOXLATE
            param.Value = headerpath;
            param.ContentType = "text/xml"; //NOXLATE
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "RESOURCEID"; //NOXLATE
            param.Value = resourceId;
            op.Parameters.Parameter.Add(param);

            manifest.Operations.Operation.Add(op);
        }

        private void AddFolderResource(ResourcePackageManifest manifest, string temppath, string folderResId, bool eraseFirst, IServerConnection connection, List<KeyValuePair<string, string>> filemap)
        {
            string filebase = System.IO.Path.GetDirectoryName(CreateFolderForResource(folderResId + "dummy.xml", temppath)); //NOXLATE

            filemap.Add(new KeyValuePair<string, string>(System.IO.Path.Combine(filebase, "_HEADER.xml"), System.IO.Path.Combine(temppath, Guid.NewGuid().ToString()))); //NOXLATE
            using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                connection.ResourceService.SerializeObject(connection.ResourceService.GetFolderHeader(folderResId), fs);

            if (!filebase.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                filebase += System.IO.Path.DirectorySeparatorChar;

            string headerpath = RelativeName(filebase + "_HEADER.xml", temppath).Replace('\\', '/'); //NOXLATE

            AddFolderResource(manifest, folderResId, headerpath, eraseFirst);
        }


        private void AddFolderResource(ResourcePackageManifest manifest, string resourceId, string headerpath, bool eraseFirst)
        {
            if (eraseFirst)
            {
                ResourcePackageManifestOperationsOperation delop = new ResourcePackageManifestOperationsOperation();
                delop.Name = "DELETERESOURCE"; //NOXLATE
                delop.Version = "1.0.0"; //NOXLATE
                delop.Parameters = new ResourcePackageManifestOperationsOperationParameters();
                delop.Parameters.Parameter = new System.ComponentModel.BindingList<ResourcePackageManifestOperationsOperationParametersParameter>();

                ResourcePackageManifestOperationsOperationParametersParameter delparam = new ResourcePackageManifestOperationsOperationParametersParameter();

                delparam.Name = "RESOURCEID"; //NOXLATE
                delparam.Value = resourceId;
                delop.Parameters.Parameter.Add(delparam);
                manifest.Operations.Operation.Add(delop);
            }

            ResourcePackageManifestOperationsOperation op = new ResourcePackageManifestOperationsOperation();
            if (resourceId.EndsWith("//")) //NOXLATE
                op.Name = "UPDATEREPOSITORY"; //NOXLATE
            else
                op.Name = "SETRESOURCE"; //NOXLATE
            op.Version = "1.0.0"; //NOXLATE
            op.Parameters = new ResourcePackageManifestOperationsOperationParameters();
            op.Parameters.Parameter = new System.ComponentModel.BindingList<ResourcePackageManifestOperationsOperationParametersParameter>();

            ResourcePackageManifestOperationsOperationParametersParameter param = new ResourcePackageManifestOperationsOperationParametersParameter();

            param.Name = "HEADER"; //NOXLATE
            param.Value = headerpath;
            param.ContentType = "text/xml"; //NOXLATE
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "RESOURCEID"; //NOXLATE
            param.Value = resourceId;
            op.Parameters.Parameter.Add(param);

            manifest.Operations.Operation.Add(op);
        }

        private string RelativeName(string filebase, string temppath)
        {
            if (!filebase.StartsWith(temppath))
                throw new Exception(string.Format(Strings.FilenameRelationInternalError, filebase, temppath));
            if (!temppath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                temppath += System.IO.Path.DirectorySeparatorChar;
            return filebase.Substring(temppath.Length);
        }

        private System.Text.RegularExpressions.Regex m_filenameTransformer = new System.Text.RegularExpressions.Regex(@"[^A-Za-z0-9\.-\/]", System.Text.RegularExpressions.RegexOptions.Compiled); //NOXLATE

        //There are some problems with the Zip reader in MapGuide and international characters :(
        private string EncodeFilename(string filename)
        {
            System.Text.RegularExpressions.Match m = m_filenameTransformer.Match(filename);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int previndex = 0;

            while (m != null && m.Success)
            {
                string replaceval = string.Format("-x{0:x2}-", (int)m.Value[0]); //NOXLATE

                sb.Append(filename.Substring(previndex, m.Index - previndex));
                sb.Append(replaceval);
                previndex = m.Index + m.Value.Length;

                m = m.NextMatch();
            }

            if (sb.Length == 0)
                return filename;
            else
            {
                sb.Append(filename.Substring(previndex));
                return sb.ToString();
            }
        }

        private string CreateFolderForResource(string resourceId, string temppath)
        {
            var rid = new ResourceIdentifier(resourceId);
            string filebase = EncodeFilename(rid.Name);
            string folder = "Library/" + EncodeFilename(rid.Path); //NOXLATE
            folder = folder.Substring(0, folder.Length - filebase.Length);
            filebase += resourceId.Substring(resourceId.LastIndexOf('.')); //NOXLATE

            folder = folder.Replace('/', System.IO.Path.DirectorySeparatorChar); //NOXLATE
            folder = System.IO.Path.Combine(temppath, folder);

            return System.IO.Path.Combine(folder, filebase);
        }

        private void RemapFiles(IServerConnection connection, ResourcePackageManifest manifest, string tempdir, string origpath, string newpath, List<KeyValuePair<string, string>> filemap)
        {
            if (!newpath.EndsWith("/")) //NOXLATE
                newpath += "/"; //NOXLATE
            if (!origpath.EndsWith("/")) //NOXLATE
                origpath += "/"; //NOXLATE

            Dictionary<string, string> lookup = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> p in filemap)
                lookup.Add(p.Key, p.Value);

            foreach (ResourcePackageManifestOperationsOperation op in manifest.Operations.Operation)
            {
                op.Parameters.SetParameterValue("RESOURCEID", newpath + op.Parameters.GetParameterValue("RESOURCEID").Substring(origpath.Length)); //NOXLATE
                if (op.Parameters.GetParameterValue("CONTENT") != null) //NOXLATE
                {
                    string path = System.IO.Path.Combine(tempdir, op.Parameters.GetParameterValue("CONTENT").Replace('/', System.IO.Path.DirectorySeparatorChar)); //NOXLATE
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.Load(lookup[path]);
                    ((PlatformConnectionBase)connection).UpdateResourceReferences(doc, origpath, newpath, true);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    doc.Save(ms);
                    System.IO.MemoryStream ms2 = Utility.RemoveUTF8BOM(ms);
                    if (ms2 != ms)
                        ms.Dispose();

                    ms2.Position = 0;
                    using (System.IO.FileStream fs = new System.IO.FileStream(lookup[path], System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    {
                        Utility.CopyStream(ms2, fs);
                    }
                    ms2.Dispose();
                }
            }
        }

        private void ZipDirectory(string zipfile, string folder, string comment, List<KeyValuePair<string, string>> filemap)
        {
            ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = System.Text.Encoding.UTF8.CodePage;
            ICSharpCode.SharpZipLib.Checksums.Crc32 crc = new ICSharpCode.SharpZipLib.Checksums.Crc32();
            using (System.IO.FileStream ofs = new System.IO.FileStream(zipfile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            using (ICSharpCode.SharpZipLib.Zip.ZipOutputStream zip = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(ofs))
            {
                try
                {
                    zip.SetLevel(9);
                    if (!string.IsNullOrEmpty(comment))
                        zip.SetComment(comment);

                    int i = 0;

                    foreach (KeyValuePair<string, string> f in filemap)
                    {
                        if (Progress != null)
                            Progress(ProgressType.Compressing, f.Key, filemap.Count, i);

                        System.IO.FileInfo fi = new System.IO.FileInfo(f.Value);
                        ICSharpCode.SharpZipLib.Zip.ZipEntry ze = new ICSharpCode.SharpZipLib.Zip.ZipEntry(RelativeName(f.Key, folder).Replace('\\', '/'));
                        ze.DateTime = fi.LastWriteTime;
                        ze.Size = fi.Length;
                        zip.PutNextEntry(ze);
                        using (System.IO.FileStream fs = new System.IO.FileStream(fi.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                            Utility.CopyStream(fs, zip);

                        if (Progress != null)
                            Progress(ProgressType.Compressing, f.Key, filemap.Count, i++);
                    }

                    zip.Finish();
                }
                finally
                {
                    try { zip.Close(); }
                    catch { }
                }
            }
        }


        private const string DEFAULT_HEADER =
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
            "<ResourceFolderHeader xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"ResourceFolderHeader-1.0.0.xsd\">\n" +
            "	<Security xsi:noNamespaceSchemaLocation=\"ResourceSecurity-1.0.0.xsd\">\n" +
            "		<Inherited>true</Inherited>\n" +
            "	</Security>\n" +
            "</ResourceFolderHeader>"; //NOXLATE


        private string MapResourcePathToFolder(string tempfolder, string resourcename)
        {
            return CreateFolderForResource(resourcename, tempfolder);
        }

        /// <summary>
        /// Builds a package with the specified content
        /// </summary>
        /// <param name="sourcePackageFile">The MGP file to read existing items from</param>
        /// <param name="items">The list of items that should be present in the new package</param>
        /// <param name="insertEraseCommands">True if each resource should have a delete operation inserted before the actual operation, false otherwise</param>
        /// <param name="targetfile">The output package filename</param>
        public void RebuildPackage(string sourcePackageFile, List<ResourceItem> items, string targetfile, bool insertEraseCommands)
        {
            string tempfolder = System.IO.Path.GetTempPath();
            int opno = 1;
            try
            {
                if (Progress != null)
                    Progress(ProgressType.ReadingFileList, sourcePackageFile, 100, 0);

                //Step 1: Create the file system layout
                if (!System.IO.Directory.Exists(tempfolder))
                    System.IO.Directory.CreateDirectory(tempfolder);

                string zipfilecomment;

                List<KeyValuePair<string, string>> filemap = new List<KeyValuePair<string, string>>();

                ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = System.Text.Encoding.UTF8.CodePage;
                using (ICSharpCode.SharpZipLib.Zip.ZipFile zipfile = new ICSharpCode.SharpZipLib.Zip.ZipFile(sourcePackageFile))
                {
                    zipfilecomment = zipfile.ZipFileComment;

                    if (Progress != null)
                        Progress(ProgressType.ReadingFileList, sourcePackageFile, 100, 100);

                    if (Progress != null)
                        Progress(ProgressType.PreparingFolder, string.Empty, items.Count, 0);

                    foreach (ResourceItem ri in items)
                    {
                        if (Progress != null)
                            Progress(ProgressType.PreparingFolder, ri.ResourcePath, items.Count, opno);

                        string filebase;
                        if (ri.IsFolder)
                        {
                            filebase = System.IO.Path.GetDirectoryName(MapResourcePathToFolder(tempfolder, ri.ResourcePath + "dummy.xml")); //NOXLATE
                            if (!filebase.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                                filebase += System.IO.Path.DirectorySeparatorChar;
                        }
                        else
                            filebase = MapResourcePathToFolder(tempfolder, ri.ResourcePath);

                        string headerpath = filebase + "_HEADER.xml"; //NOXLATE
                        string contentpath = filebase + "_CONTENT.xml"; //NOXLATE

                        if (ri.EntryType == EntryTypeEnum.Added)
                        {
                            if (string.IsNullOrEmpty(ri.Headerpath))
                            {
                                filemap.Add(new KeyValuePair<string, string>(headerpath, System.IO.Path.Combine(tempfolder, ri.GenerateUniqueName())));
                                using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                {
                                    byte[] data = System.Text.Encoding.UTF8.GetBytes(DEFAULT_HEADER);
                                    fs.Write(data, 0, data.Length);
                                }
                            }
                            else if (!ri.IsFolder)
                            {
                                filemap.Add(new KeyValuePair<string, string>(headerpath, ri.Headerpath));
                                System.IO.File.Copy(ri.Headerpath, headerpath);
                            }

                            if (!string.IsNullOrEmpty(ri.Contentpath))
                                filemap.Add(new KeyValuePair<string, string>(contentpath, ri.Contentpath));
                        }
                        else if (ri.EntryType == EntryTypeEnum.Regular)
                        {
                            if (string.IsNullOrEmpty(ri.Headerpath))
                            {
                                filemap.Add(new KeyValuePair<string, string>(headerpath, System.IO.Path.Combine(tempfolder, ri.GenerateUniqueName())));
                                using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                {
                                    byte[] data = System.Text.Encoding.UTF8.GetBytes(DEFAULT_HEADER);
                                    fs.Write(data, 0, data.Length);
                                }
                            }
                            else
                            {
                                int index = FindZipEntry(zipfile, ri.Headerpath);
                                if (index < 0)
                                    throw new Exception(string.Format(Strings.FileMissingError, ri.Headerpath));

                                filemap.Add(new KeyValuePair<string, string>(headerpath, System.IO.Path.Combine(tempfolder, ri.GenerateUniqueName())));
                                using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                    Utility.CopyStream(zipfile.GetInputStream(index), fs);
                            }

                            if (!ri.IsFolder)
                            {
                                int index = FindZipEntry(zipfile, ri.Contentpath);
                                if (index < 0)
                                    throw new Exception(string.Format(Strings.FileMissingError, ri.Contentpath));

                                filemap.Add(new KeyValuePair<string, string>(contentpath, System.IO.Path.Combine(tempfolder, ri.GenerateUniqueName())));
                                using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                    Utility.CopyStream(zipfile.GetInputStream(index), fs);
                            }

                        }

                        ri.Headerpath = headerpath;
                        ri.Contentpath = contentpath;

                        foreach (ResourceDataItem rdi in ri.Items)
                        {
                            string targetpath = filebase + "_DATA_" + EncodeFilename(rdi.ResourceName); //NOXLATE
                            if (rdi.EntryType == EntryTypeEnum.Added)
                            {
                                var tempFilePath = System.IO.Path.Combine(tempfolder, ri.GenerateUniqueName());
                                filemap.Add(new KeyValuePair<string, string>(targetpath, tempFilePath));
                                if (File.Exists(rdi.Filename)) 
                                    File.Copy(rdi.Filename, tempFilePath);
                            }
                            else
                            {
                                int index = FindZipEntry(zipfile, rdi.Filename);
                                if (index < 0)
                                    throw new Exception(string.Format(Strings.FileMissingError, rdi.Filename));

                                filemap.Add(new KeyValuePair<string, string>(targetpath, System.IO.Path.Combine(tempfolder, ri.GenerateUniqueName())));
                                using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                    Utility.CopyStream(zipfile.GetInputStream(index), fs);
                            }
                            rdi.Filename = targetpath;
                        }

                        if (Progress != null)
                            Progress(ProgressType.PreparingFolder, ri.ResourcePath, items.Count, opno++);
                    }
                }

                int i = 0;

                Dictionary<string, string> filemap_lookup = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> kv in filemap)
                    filemap_lookup[kv.Key] = kv.Value;

                //Step 2: Repoint all resources with respect to the update
                foreach (ResourceItem ri in items)
                {
                    if (Progress != null)
                        Progress(ProgressType.MovingResources, Strings.ProgressUpdatingResources, items.Count, i);

                    if (ri.OriginalResourcePath != ri.ResourcePath)
                    {
                        foreach (ResourceItem rix in items)
                        {
                            if (!rix.IsFolder)
                            {
                                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                                doc.Load(filemap_lookup[rix.Contentpath]);
                                ((PlatformConnectionBase)m_connection).UpdateResourceReferences(doc, ri.OriginalResourcePath, ri.ResourcePath, ri.IsFolder);
                                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                doc.Save(ms);
                                System.IO.MemoryStream ms2 = Utility.RemoveUTF8BOM(ms);
                                if (ms2 != ms)
                                    ms.Dispose();

                                ms2.Position = 0;
                                using (System.IO.FileStream fs = new System.IO.FileStream(filemap_lookup[rix.Contentpath], System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                {
                                    Utility.CopyStream(ms2, fs);
                                }
                                ms2.Dispose();
                            }
                        }
                    }
                    if (Progress != null)
                        Progress(ProgressType.MovingResources, Strings.ProgressUpdatingResources, items.Count, i++);
                }

                if (Progress != null)
                    Progress(ProgressType.MovingResources, Strings.ProgressUpdatedResources, items.Count, items.Count);

                //Step 3: Create an updated definition file
                ResourcePackageManifest manifest = new ResourcePackageManifest();
                manifest.Description = "MapGuide Package created by Maestro"; //NOXLATE
                manifest.Operations = new ResourcePackageManifestOperations();
                manifest.Operations.Operation = new System.ComponentModel.BindingList<ResourcePackageManifestOperationsOperation>();

                foreach (ResourceItem ri in items)
                {
                    if (ri.IsFolder)
                    {
                        AddFolderResource(
                            manifest,
                            ri.ResourcePath,
                            RelativeName(ri.Headerpath, tempfolder).Replace('\\', '/'), //NOXLATE
                            insertEraseCommands);
                    }
                    else
                    {
                        AddFileResource(
                            manifest,
                            ri.ResourcePath,
                            RelativeName(ri.Headerpath, tempfolder).Replace('\\', '/'), //NOXLATE
                            RelativeName(ri.Contentpath, tempfolder).Replace('\\', '/'), //NOXLATE
                            insertEraseCommands);

                        foreach (ResourceDataItem rdi in ri.Items)
                        {
                            AddResourceData(
                                manifest,
                                ri.ResourcePath,
                                rdi.ContentType,
                                rdi.DataType,
                                rdi.ResourceName,
                                RelativeName(rdi.Filename, tempfolder).Replace('\\', '/'), //NOXLATE
                                new System.IO.FileInfo(filemap_lookup[rdi.Filename]).Length);
                        }
                    }
                }

                filemap.Add(new KeyValuePair<string, string>(System.IO.Path.Combine(tempfolder, "MgResourcePackageManifest.xml"), System.IO.Path.Combine(tempfolder, Guid.NewGuid().ToString()))); //NOXLATE
                using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    m_connection.ResourceService.SerializeObject(manifest, fs);

                if (Progress != null)
                    Progress(ProgressType.Compressing, Strings.ProgressCompressing, 100, 0);

                //Step 4: Create the zip file
                ZipDirectory(targetfile, tempfolder, zipfilecomment, filemap);

                if (Progress != null)
                    Progress(ProgressType.Compressing, Strings.ProgressCompressed, 100, 100);
            }
            finally
            {
                try { System.IO.Directory.Delete(tempfolder, true); }
                catch { }
            }
        }

        private int FindZipEntry(ICSharpCode.SharpZipLib.Zip.ZipFile file, string path)
        {
            string p = path.Replace('\\', '/'); //NOXLATE
            foreach (ICSharpCode.SharpZipLib.Zip.ZipEntry ze in file)
                if (ze.Name.Replace('\\', '/').Equals(p)) //NOXLATE
                    return (int)ze.ZipFileIndex;

            return -1;
        }

        /// <summary>
        /// Reads the contents of a package file
        /// </summary>
        /// <param name="packageFile">The file to read</param>
        /// <returns>A dictionary of items, the key is the resourceId</returns>
        public Dictionary<string, ResourceItem> ListPackageContents(string packageFile)
        {
            if (Progress != null)
                Progress(ProgressType.ListingFiles, packageFile, 100, 0);

            Dictionary<string, ResourceItem> resourceList = new Dictionary<string, ResourceItem>();

            ResourcePackageManifest manifest;
            ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = System.Text.Encoding.UTF8.CodePage;
            using (ICSharpCode.SharpZipLib.Zip.ZipFile zipfile = new ICSharpCode.SharpZipLib.Zip.ZipFile(packageFile))
            {
                int index = FindZipEntry(zipfile, "MgResourcePackageManifest.xml"); //NOXLATE
                if (index < 0)
                    throw new Exception(Strings.InvalidPackageFileError);

                manifest = m_connection.ResourceService.DeserializeObject<ResourcePackageManifest>(zipfile.GetInputStream(index));
            }

            int i = 0;
            if (Progress != null)
                Progress(ProgressType.ListingFiles, packageFile, manifest.Operations.Operation.Count, i);

            //TODO: Much of this assumes that the package is correctly constructed, ea.: no SETRESOURCEDATA, before a SETRESOURCE and so on.
            foreach (ResourcePackageManifestOperationsOperation op in manifest.Operations.Operation)
            {
                if (Progress != null)
                    Progress(ProgressType.ListingFiles, packageFile, manifest.Operations.Operation.Count, i++);

                if (op.Name.ToLower().Equals("setresource")) //NOXLATE
                {
                    string id = op.Parameters.GetParameterValue("RESOURCEID"); //NOXLATE
                    string header;
                    if (op.Parameters.GetParameterValue("HEADER") != null) //NOXLATE
                        header = op.Parameters.GetParameterValue("HEADER"); //NOXLATE
                    else
                        header = null;
                    string content = op.Parameters.GetParameterValue("CONTENT") == null ? null : op.Parameters.GetParameterValue("CONTENT"); //NOXLATE

                    resourceList.Add(id, new ResourceItem(id, header, content));
                }
                else if (op.Name.ToLower().Equals("setresourcedata")) //NOXLATE
                {
                    string id = op.Parameters.GetParameterValue("RESOURCEID"); //NOXLATE
                    ResourceItem ri = resourceList[id];
                    string name = op.Parameters.GetParameterValue("DATANAME"); //NOXLATE
                    string file = op.Parameters.GetParameterValue("DATA"); //NOXLATE
                    string contentType = op.Parameters.GetParameterValue("DATA"); //NOXLATE
                    string dataType = op.Parameters.GetParameterValue("DATATYPE"); //NOXLATE

                    ri.Items.Add(new ResourceDataItem(name, contentType, file, dataType));
                }

                //TODO: What to do with "DELETERESOURCE" ?
            }

            return resourceList;
        }
    }

    /// <summary>
    /// Base class of all package operations
    /// </summary>
    public abstract class PackageOperation
    {
        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        /// <value>
        /// The resource id.
        /// </value>
        public string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the operation.
        /// </summary>
        /// <value>
        /// The name of the operation.
        /// </value>
        public string OperationName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageOperation"/> class.
        /// </summary>
        /// <param name="resId">The res id.</param>
        protected PackageOperation(string resId) { this.ResourceId = resId; }
    }

    /// <summary>
    /// A SETRESOURCE package operation
    /// </summary>
    public class SetResourcePackageOperation : PackageOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetResourcePackageOperation"/> class.
        /// </summary>
        /// <param name="resId">The res id.</param>
        /// <param name="content">The content.</param>
        /// <param name="header">The header.</param>
        public SetResourcePackageOperation(string resId, string content, string header)
            : base(resId)
        {
            this.OperationName = "SETRESOURCE"; //NOXLATE
            this.Content = content;
            this.Header = header;
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public string Header { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!typeof(SetResourcePackageOperation).IsAssignableFrom(obj.GetType()))
                return false;

            SetResourcePackageOperation vi = (SetResourcePackageOperation)obj;
            return string.Compare(this.Content, vi.Content) == 0 &&
                   string.Compare(this.Header, vi.Header) == 0 &&
                   string.Compare(this.OperationName, vi.OperationName) == 0 &&
                   string.Compare(this.ResourceId, vi.ResourceId) == 0;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            //http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-systemobjectgethashcode
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (this.Content ?? string.Empty).GetHashCode();
                if (this.Header != null)
                    hash = hash * 23 + this.Header.GetHashCode();
                hash = hash * 23 + this.OperationName.GetHashCode();
                hash = hash * 23 + this.ResourceId.GetHashCode();

                return hash;
            }
        }
    }

    /// <summary>
    /// A SETRESOURCEDATA package operation
    /// </summary>
    public class SetResourceDataPackageOperation : PackageOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetResourceDataPackageOperation"/> class.
        /// </summary>
        /// <param name="resId">The res id.</param>
        /// <param name="data">The data.</param>
        /// <param name="dataName">Name of the data.</param>
        /// <param name="dataType">Type of the data.</param>
        public SetResourceDataPackageOperation(string resId, string data, string dataName, ResourceDataType dataType)
            : base(resId)
        {
            this.OperationName = "SETRESOURCEDATA"; //NOXLATE
            this.Data = data;
            this.DataName = dataName;
            this.DataType = dataType;
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the name of the data.
        /// </summary>
        /// <value>
        /// The name of the data.
        /// </value>
        public string DataName { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public ResourceDataType DataType { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!typeof(SetResourceDataPackageOperation).IsAssignableFrom(obj.GetType()))
                return false;

            SetResourceDataPackageOperation vi = (SetResourceDataPackageOperation)obj;
            return this.Data.Equals(vi.Data) &&
                   this.DataName.Equals(vi.DataName) &&
                   this.OperationName.Equals(vi.OperationName) &&
                   this.ResourceId.Equals(vi.ResourceId) &&
                   this.DataType == vi.DataType;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            //http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-systemobjectgethashcode
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.Data.GetHashCode();
                hash = hash * 23 + this.DataName.GetHashCode();
                hash = hash * 23 + this.OperationName.GetHashCode();
                hash = hash * 23 + this.ResourceId.GetHashCode();
                hash = hash * 23 + this.DataType.GetHashCode();

                return hash;
            }
        }
    }
}
