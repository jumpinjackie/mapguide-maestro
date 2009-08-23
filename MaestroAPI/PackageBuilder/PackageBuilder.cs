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
using System.Collections.Generic;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.PackageBuilder
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
        ListingFiles
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
    public delegate void ProgressDelegate(ProgressType type, string resourceId, int maxValue, int value);

    public class PackageBuilder
    {
        /// <summary>
        /// The connection object
        /// </summary>
        private ServerConnectionI m_connection;

        /// <summary>
        /// Constructs a new package builder instance
        /// </summary>
        /// <param name="connection">The connection used to serialize and fetch items</param>
        public PackageBuilder(ServerConnectionI connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
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
            m_connection.UploadPackage(sourceFile, new Utility.StreamCopyProgressDelegate(ProgressCallback_Upload));

            if (Progress != null)
                Progress(ProgressType.Uploading, sourceFile, 100, 100);
        }

        private void ProgressCallback_Upload(long copied, long remain, long total)
        {
            if (Progress != null)
            {
                if (m_lastPg < 0 || remain == 0 || copied - m_lastPg > 1024 * 50)
                {
                    Progress(ProgressType.Uploading, "", (int)(total / 1024), (int)(copied / 1024));
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
        public void CreatePackage(string folderResourceId, string zipfilename, List<string> allowedExtensions, bool removeExistingFiles, string alternateTargetResourceId)
        {
            if (Progress != null)
                Progress(ProgressType.ReadingFileList, folderResourceId, 100, 0);

            ResourceList items = m_connection.GetRepositoryResources(folderResourceId);

            List<ResourceListResourceDocument> files = new List<ResourceListResourceDocument>();
            List<ResourceListResourceFolder> folders = new List<ResourceListResourceFolder>();
            Dictionary<string, List<ResourceDataListResourceData>> resourceData = new Dictionary<string, List<ResourceDataListResourceData>>();
            ResourcePackageManifest manifest = new ResourcePackageManifest();
            manifest.Description = "MapGuide Package created with Maestro";
            manifest.Operations = new ResourcePackageManifestOperations();
            manifest.Operations.Operation = new ResourcePackageManifestOperationsOperationCollection();
            System.Collections.Hashtable knownTypes = ((MaestroAPI.ServerConnectionBase)m_connection).ResourceTypeLookup;

            foreach (object o in items.Items)
                if (o as ResourceListResourceDocument != null)
                {
                    ResourceListResourceDocument doc = o as ResourceListResourceDocument;
                    string extension = doc.ResourceId.Substring(doc.ResourceId.LastIndexOf('.'));
                    if (allowedExtensions == null || allowedExtensions.Count == 0)
                        files.Add(doc);
                    else if (knownTypes.ContainsKey(extension) && allowedExtensions.Contains(extension))
                        files.Add(doc);
                    else if (!knownTypes.ContainsKey(extension) && allowedExtensions.Contains("*"))
                        files.Add(doc);
                }
                else if (o as ResourceListResourceFolder != null)
                {
                    folders.Add(o as ResourceListResourceFolder);
                }

            if (Progress != null)
            {
                Progress(ProgressType.ReadingFileList, folderResourceId, 100, 100);
                Progress(ProgressType.PreparingFolder, "", files.Count + folders.Count + 1, 0);
            }

            string temppath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());

            //All files have random names on disk, but a full path in the zip file
            List<KeyValuePair<string, string>> filemap = new List<KeyValuePair<string, string>>();

            try
            {
                System.IO.Directory.CreateDirectory(temppath);
                int opno = 1;

                foreach (ResourceListResourceFolder folder in folders)
                {

                    if (Progress != null)
                        Progress(ProgressType.PreparingFolder, folder.ResourceId, files.Count + folders.Count + 1, opno);
                    AddFolderResource(manifest, temppath, folder, removeExistingFiles, m_connection, filemap);
                    if (Progress != null)
                        Progress(ProgressType.PreparingFolder, folder.ResourceId, files.Count + folders.Count + 1, opno++);
                }

                foreach (ResourceListResourceDocument doc in files)
                {
                    if (Progress != null)
                        Progress(ProgressType.PreparingFolder, doc.ResourceId, files.Count + folders.Count + 1, opno);
                    string filebase = CreateFolderForResource(doc.ResourceId, temppath);

                    resourceData[doc.ResourceId] = new List<ResourceDataListResourceData>();
                    ResourceDataList rdl = m_connection.EnumerateResourceData(doc.ResourceId);
                    foreach (ResourceDataListResourceData rd in rdl.ResourceData)
                        resourceData[doc.ResourceId].Add(rd);

                    int i = 0;
                    int itemCount = resourceData[doc.ResourceId].Count + 1;

                    filemap.Add(new KeyValuePair<string, string>(filebase + "_CONTENT.xml", System.IO.Path.Combine(temppath, Guid.NewGuid().ToString())));
                    using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    {
                        byte[] data = m_connection.GetResourceXmlData(doc.ResourceId);
                        fs.Write(data, 0, data.Length);
                    }

                    AddFileResource(manifest, temppath, doc, filemap[filemap.Count - 1].Key, removeExistingFiles, m_connection, filemap);

                    foreach (ResourceDataListResourceData rd in rdl.ResourceData)
                    {
                        filemap.Add(new KeyValuePair<string, string>(filebase + "_DATA_" + rd.Name, System.IO.Path.Combine(temppath, Guid.NewGuid().ToString())));
                        System.IO.FileInfo fi = new System.IO.FileInfo(filemap[filemap.Count - 1].Value);
                        using (System.IO.FileStream fs = new System.IO.FileStream(fi.FullName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                            m_connection.GetResourceData(doc.ResourceId, rd.Name).WriteTo(fs);

                        AddResourceData(manifest, temppath, doc, fi, filemap[filemap.Count - 1].Key, rd, m_connection);
                    }

                    if (Progress != null)
                        Progress(ProgressType.PreparingFolder, doc.ResourceId, files.Count + folders.Count + 1, opno++);
                }

                if (Progress != null)
                    Progress(ProgressType.PreparingFolder, "Done", files.Count + folders.Count + 1, files.Count + folders.Count + 1);

                if (!string.IsNullOrEmpty(alternateTargetResourceId))
                {
                    if (Progress != null)
                        Progress(ProgressType.MovingResources, "Updating references ...", 100, 0);
                    RemapFiles(m_connection, manifest, temppath, folderResourceId, alternateTargetResourceId, filemap);
                    if (Progress != null)
                        Progress(ProgressType.MovingResources, "Updated references", 100, 100);
                }

                filemap.Add(new KeyValuePair<string, string>(System.IO.Path.Combine(temppath, "MgResourcePackageManifest.xml"), System.IO.Path.Combine(temppath, Guid.NewGuid().ToString())));
                using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    m_connection.SerializeObject(manifest, fs);


                if (Progress != null)
                    Progress(ProgressType.MovingResources, zipfilename, filemap.Count, 0);

                ZipDirectory(zipfilename, temppath, "MapGuide Package created by Maestro", filemap);

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

        private void AddResourceData(ResourcePackageManifest manifest, string temppath, ResourceListResourceDocument doc, System.IO.FileInfo fi, string resourcePath, ResourceDataListResourceData rd, ServerConnectionI connection)
        {
            string contentType = "application/octet-stream";
            try
            {
                if (connection as HttpServerConnection != null)
                    contentType = (connection as HttpServerConnection).LastResponseHeaders[System.Net.HttpResponseHeader.ContentType];
            }
            catch
            {
            }
            string name = rd.Name;
            string type = rd.Type.ToString();
            string resourceId = doc.ResourceId;
            string filename = RelativeName(resourcePath, temppath).Replace('\\', '/');
            long size = fi.Length;

            AddResourceData(manifest, resourceId, contentType, type, name, filename, size);
        }

        private void AddResourceData(ResourcePackageManifest manifest, string resourceId, string contentType, string type, string name, string filename, long size)
        {
            ResourcePackageManifestOperationsOperation op = new ResourcePackageManifestOperationsOperation();
            op.Name = "SETRESOURCEDATA";
            op.Version = "1.0.0";
            op.Parameters = new ResourcePackageManifestOperationsOperationParameters();
            op.Parameters.Parameter = new ResourcePackageManifestOperationsOperationParametersParameterCollection();

            ResourcePackageManifestOperationsOperationParametersParameter param = new ResourcePackageManifestOperationsOperationParametersParameter();

            param.Name = "DATA";
            param.Value = filename;
            param.ContentType = contentType;
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "DATALENGTH";
            param.Value = size.ToString();
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "DATANAME";
            param.Value = name;
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "DATATYPE";
            param.Value = type;
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "RESOURCEID";
            param.Value = resourceId;
            op.Parameters.Parameter.Add(param);

            manifest.Operations.Operation.Add(op);
        }

        private void AddFileResource(ResourcePackageManifest manifest, string temppath, ResourceListResourceDocument doc, string contentfilename, bool eraseFirst, ServerConnectionI connection, List<KeyValuePair<string, string>> filemap)
        {
            string filebase = CreateFolderForResource(doc.ResourceId, temppath);

            filemap.Add(new KeyValuePair<string, string>(filebase + "_HEADER.xml", System.IO.Path.Combine(temppath, Guid.NewGuid().ToString())));
            using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                connection.SerializeObject(connection.GetResourceHeader(doc.ResourceId), fs);

            string headerpath = RelativeName(filemap[filemap.Count - 1].Key, temppath).Replace('\\', '/');
            string contentpath = RelativeName(contentfilename, temppath).Replace('\\', '/');
            AddFileResource(manifest, doc.ResourceId, headerpath, contentpath, eraseFirst);
        }

        private void AddFileResource(ResourcePackageManifest manifest, string resourceId, string headerpath, string contentpath, bool eraseFirst)
        {
            if (eraseFirst)
            {
                ResourcePackageManifestOperationsOperation delop = new ResourcePackageManifestOperationsOperation();
                delop.Name = "DELETERESOURCE";
                delop.Version = "1.0.0";
                delop.Parameters = new ResourcePackageManifestOperationsOperationParameters();
                delop.Parameters.Parameter = new ResourcePackageManifestOperationsOperationParametersParameterCollection();

                ResourcePackageManifestOperationsOperationParametersParameter delparam = new ResourcePackageManifestOperationsOperationParametersParameter();

                delparam.Name = "RESOURCEID";
                delparam.Value = resourceId;
                delop.Parameters.Parameter.Add(delparam);
                manifest.Operations.Operation.Add(delop);
            }

            ResourcePackageManifestOperationsOperation op = new ResourcePackageManifestOperationsOperation();
            op.Name = "SETRESOURCE";
            op.Version = "1.0.0";
            op.Parameters = new ResourcePackageManifestOperationsOperationParameters();
            op.Parameters.Parameter = new ResourcePackageManifestOperationsOperationParametersParameterCollection();

            ResourcePackageManifestOperationsOperationParametersParameter param = new ResourcePackageManifestOperationsOperationParametersParameter();

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "CONTENT";
            param.Value = contentpath;
            param.ContentType = "text/xml";
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "HEADER";
            param.Value = headerpath;
            param.ContentType = "text/xml";
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "RESOURCEID";
            param.Value = resourceId;
            op.Parameters.Parameter.Add(param);

            manifest.Operations.Operation.Add(op);
        }

        private void AddFolderResource(ResourcePackageManifest manifest, string temppath, ResourceListResourceFolder folder, bool eraseFirst, ServerConnectionI connection, List<KeyValuePair<string, string>> filemap)
        {
            string filebase = System.IO.Path.GetDirectoryName(CreateFolderForResource(folder.ResourceId + "dummy.xml", temppath));

            filemap.Add(new KeyValuePair<string, string>(System.IO.Path.Combine(filebase, "_HEADER.xml"), System.IO.Path.Combine(temppath, Guid.NewGuid().ToString())));
            using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                connection.SerializeObject(connection.GetFolderHeader(folder.ResourceId), fs);

            if (!filebase.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                filebase += System.IO.Path.DirectorySeparatorChar;

            string headerpath = RelativeName(filebase + "_HEADER.xml", temppath).Replace('\\', '/');

            AddFolderResource(manifest, folder.ResourceId, headerpath, eraseFirst);
        }


        private void AddFolderResource(ResourcePackageManifest manifest, string resourceId, string headerpath, bool eraseFirst)
        {
            if (eraseFirst)
            {
                ResourcePackageManifestOperationsOperation delop = new ResourcePackageManifestOperationsOperation();
                delop.Name = "DELETERESOURCE";
                delop.Version = "1.0.0";
                delop.Parameters = new ResourcePackageManifestOperationsOperationParameters();
                delop.Parameters.Parameter = new ResourcePackageManifestOperationsOperationParametersParameterCollection();

                ResourcePackageManifestOperationsOperationParametersParameter delparam = new ResourcePackageManifestOperationsOperationParametersParameter();

                delparam.Name = "RESOURCEID";
                delparam.Value = resourceId;
                delop.Parameters.Parameter.Add(delparam);
                manifest.Operations.Operation.Add(delop);
            }

            ResourcePackageManifestOperationsOperation op = new ResourcePackageManifestOperationsOperation();
            if (resourceId.EndsWith("//"))
                op.Name = "UPDATEREPOSITORY";
            else
                op.Name = "SETRESOURCE";
            op.Version = "1.0.0";
            op.Parameters = new ResourcePackageManifestOperationsOperationParameters();
            op.Parameters.Parameter = new ResourcePackageManifestOperationsOperationParametersParameterCollection();

            ResourcePackageManifestOperationsOperationParametersParameter param = new ResourcePackageManifestOperationsOperationParametersParameter();

            param.Name = "HEADER";
            param.Value = headerpath;
            param.ContentType = "text/xml";
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "RESOURCEID";
            param.Value = resourceId;
            op.Parameters.Parameter.Add(param);

            manifest.Operations.Operation.Add(op);
        }

        private string RelativeName(string filebase, string temppath)
        {
            if (!filebase.StartsWith(temppath))
                throw new Exception(string.Format("Filename \"{0}\" is not relative to \"{1}\"", filebase, temppath));
            if (!temppath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                temppath += System.IO.Path.DirectorySeparatorChar;
            return filebase.Substring(temppath.Length);
        }

        private System.Text.RegularExpressions.Regex m_filenameTransformer = new System.Text.RegularExpressions.Regex(@"[^A-Za-z0-9\.-\/]", System.Text.RegularExpressions.RegexOptions.Compiled);

        //There are some problems with the Zip reader in MapGuide and international characters :(
        private string EncodeFilename(string filename)
        {
            System.Text.RegularExpressions.Match m = m_filenameTransformer.Match(filename);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int previndex = 0;

            while (m != null && m.Success)
            {
                string replaceval = string.Format("-x{0:x2}-", (int)m.Value[0]);

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
            string filebase = EncodeFilename(new MaestroAPI.ResourceIdentifier(resourceId).Name);
            string folder = "Library/" + EncodeFilename(new MaestroAPI.ResourceIdentifier(resourceId).Path);
            folder = folder.Substring(0, folder.Length - filebase.Length);
            filebase += resourceId.Substring(resourceId.LastIndexOf('.'));

            folder = folder.Replace('/', System.IO.Path.DirectorySeparatorChar);
            folder = System.IO.Path.Combine(temppath, folder);

            return System.IO.Path.Combine(folder, filebase);
        }

        private void RemapFiles(ServerConnectionI connection, MaestroAPI.ResourcePackageManifest manifest, string tempdir, string origpath, string newpath, List<KeyValuePair<string, string>> filemap)
        {
            if (!newpath.EndsWith("/"))
                newpath += "/";
            if (!origpath.EndsWith("/"))
                origpath += "/";

            Dictionary<string, string> lookup = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> p in filemap)
                lookup.Add(p.Key, p.Value);

            foreach (MaestroAPI.ResourcePackageManifestOperationsOperation op in manifest.Operations.Operation)
            {
                op.Parameters.Parameter["RESOURCEID"].Value = newpath + op.Parameters.Parameter["RESOURCEID"].Value.Substring(origpath.Length);
                if (op.Parameters.Parameter["CONTENT"] != null)
                {
                    string path = System.IO.Path.Combine(tempdir, op.Parameters.Parameter["CONTENT"].Value.Replace('/', System.IO.Path.DirectorySeparatorChar));
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.Load(lookup[path]);
                    ((ServerConnectionBase)connection).UpdateResourceReferences(doc, origpath, newpath, true);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    doc.Save(ms);
                    System.IO.MemoryStream ms2 = Utility.RemoveUTF8BOM(ms);
                    if (ms2 != ms)
                        ms.Dispose();

                    ms2.Position = 0;
                    using (System.IO.FileStream fs = new System.IO.FileStream(lookup[path], System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                        ms2.WriteTo(fs);
                    ms2.Dispose();
                }
            }
        }

        private void ZipDirectory(string zipfile, string folder, string comment, List<KeyValuePair<string, string>> filemap)
        {
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
                            MaestroAPI.Utility.CopyStream(fs, zip);

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
            "</ResourceFolderHeader>";


        private string MapResourcePathToFolder(string tempfolder, string resourcename)
        {
            return CreateFolderForResource(resourcename, tempfolder);
        }

        /// <summary>
        /// Builds a package with the specified content
        /// </summary>
        /// <param name="sourcePackageFile">The MGP file to read existing items from</param>
        /// <param name="items">The list of items that should be present in the new package</param>
        /// <param name="targetfile">The output package filename</param>
        public void RebuildPackage(string sourcePackageFile, List<ResourceItem> items, string targetfile)
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

                using (ICSharpCode.SharpZipLib.Zip.ZipFile zipfile = new ICSharpCode.SharpZipLib.Zip.ZipFile(sourcePackageFile))
                {
                    zipfilecomment = zipfile.ZipFileComment;

                    if (Progress != null)
                        Progress(ProgressType.ReadingFileList, sourcePackageFile, 100, 100);

                    if (Progress != null)
                        Progress(ProgressType.PreparingFolder, "", items.Count, 0);

                    foreach (ResourceItem ri in items)
                    {
                        if (Progress != null)
                            Progress(ProgressType.PreparingFolder, ri.ResourcePath, items.Count, opno);

                        string filebase;
                        if (ri.IsFolder)
                        {
                            filebase = System.IO.Path.GetDirectoryName(MapResourcePathToFolder(tempfolder, ri.ResourcePath + "dummy.xml"));
                            if (!filebase.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                                filebase += System.IO.Path.DirectorySeparatorChar;
                        }
                        else
                            filebase = MapResourcePathToFolder(tempfolder, ri.ResourcePath);

                        string headerpath = filebase + "_HEADER.xml";
                        string contentpath = filebase + "_CONTENT.xml";

                        if (ri.EntryType == EntryTypeEnum.Added)
                        {
                            if (string.IsNullOrEmpty(ri.Headerpath))
                            {
                                filemap.Add(new KeyValuePair<string, string>(headerpath, System.IO.Path.Combine(tempfolder, Guid.NewGuid().ToString())));
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
                                filemap.Add(new KeyValuePair<string, string>(headerpath, System.IO.Path.Combine(tempfolder, Guid.NewGuid().ToString())));
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
                                    throw new Exception(string.Format("Failed to find file {0} in archive", ri.Headerpath));

                                filemap.Add(new KeyValuePair<string, string>(headerpath, System.IO.Path.Combine(tempfolder, Guid.NewGuid().ToString())));
                                using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                    Utility.CopyStream(zipfile.GetInputStream(index), fs);
                            }

                            if (!ri.IsFolder)
                            {
                                int index = FindZipEntry(zipfile, ri.Contentpath);
                                if (index < 0)
                                    throw new Exception(string.Format("Failed to find file {0} in archive", ri.Contentpath));

                                filemap.Add(new KeyValuePair<string, string>(contentpath, System.IO.Path.Combine(tempfolder, Guid.NewGuid().ToString())));
                                using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                    Utility.CopyStream(zipfile.GetInputStream(index), fs);
                            }

                        }

                        ri.Headerpath = headerpath;
                        ri.Contentpath = contentpath;

                        foreach (ResourceDataItem rdi in ri.Items)
                        {
                            string targetpath = filebase + "_DATA_" + rdi.ResourceName;
                            if (rdi.EntryType == EntryTypeEnum.Added)
                            {
                                filemap.Add(new KeyValuePair<string, string>(targetpath, rdi.Filename));
                            }
                            else
                            {
                                int index = FindZipEntry(zipfile, rdi.Filename);
                                if (index < 0)
                                    throw new Exception(string.Format("Failed to find file {0} in archive", ri.Contentpath));

                                filemap.Add(new KeyValuePair<string, string>(targetpath, System.IO.Path.Combine(tempfolder, Guid.NewGuid().ToString())));
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
                        Progress(ProgressType.MovingResources, "Updating resources ...", items.Count, i);

                    if (ri.OriginalResourcePath != ri.ResourcePath)
                    {
                        foreach (ResourceItem rix in items)
                        {
                            if (!rix.IsFolder)
                            {
                                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                                doc.Load(filemap_lookup[rix.Contentpath]);
                                ((ServerConnectionBase)m_connection).UpdateResourceReferences(doc, ri.OriginalResourcePath, ri.ResourcePath, ri.IsFolder);
                                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                doc.Save(ms);
                                System.IO.MemoryStream ms2 = Utility.RemoveUTF8BOM(ms);
                                if (ms2 != ms)
                                    ms.Dispose();

                                ms2.Position = 0;
                                using (System.IO.FileStream fs = new System.IO.FileStream(filemap_lookup[rix.Contentpath], System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                    ms2.WriteTo(fs);
                                ms2.Dispose();
                            }
                        }
                    }
                    if (Progress != null)
                        Progress(ProgressType.MovingResources, "Updating resources ...", items.Count, i++);
                }

                if (Progress != null)
                    Progress(ProgressType.MovingResources, "Updated resources", items.Count, items.Count);

                //Step 3: Create an updated definition file
                ResourcePackageManifest manifest = new ResourcePackageManifest();
                manifest.Description = "MapGuide Package created by Maestro";
                manifest.Operations = new ResourcePackageManifestOperations();
                manifest.Operations.Operation = new ResourcePackageManifestOperationsOperationCollection();

                bool eraseFirst = true;

                foreach (ResourceItem ri in items)
                    if (ri.IsFolder)
                    {
                        AddFolderResource(
                            manifest,
                            ri.ResourcePath,
                            RelativeName(ri.Headerpath, tempfolder).Replace('\\', '/'),
                            eraseFirst);
                    }
                    else
                    {
                        AddFileResource(
                            manifest,
                            ri.ResourcePath,
                            RelativeName(ri.Headerpath, tempfolder).Replace('\\', '/'),
                            RelativeName(ri.Contentpath, tempfolder).Replace('\\', '/'),
                            eraseFirst);

                        foreach (ResourceDataItem rdi in ri.Items)
                            AddResourceData(
                                manifest,
                                ri.ResourcePath,
                                rdi.ContentType,
                                rdi.DataType,
                                rdi.ResourceName,
                                RelativeName(rdi.Filename, tempfolder).Replace('\\', '/'),
                                new System.IO.FileInfo(filemap_lookup[rdi.Filename]).Length);
                    }

                filemap.Add(new KeyValuePair<string, string>(System.IO.Path.Combine(tempfolder, "MgResourcePackageManifest.xml"), System.IO.Path.Combine(tempfolder, Guid.NewGuid().ToString())));
                using (System.IO.FileStream fs = new System.IO.FileStream(filemap[filemap.Count - 1].Value, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    m_connection.SerializeObject(manifest, fs);

                if (Progress != null)
                    Progress(ProgressType.Compressing, "Compressing ...", 100, 0);

                //Step 4: Create the zip file
                ZipDirectory(targetfile, tempfolder, zipfilecomment, filemap);

                if (Progress != null)
                    Progress(ProgressType.Compressing, "Compressed", 100, 100);
            }
            finally
            {
                try { System.IO.Directory.Delete(tempfolder, true); }
                catch { }
            }
        }

        private int FindZipEntry(ICSharpCode.SharpZipLib.Zip.ZipFile file, string path)
        {
            string p = path.Replace('\\', '/');
            foreach (ICSharpCode.SharpZipLib.Zip.ZipEntry ze in file)
                if (ze.Name.Replace('\\', '/').Equals(p))
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
            using (ICSharpCode.SharpZipLib.Zip.ZipFile zipfile = new ICSharpCode.SharpZipLib.Zip.ZipFile(packageFile))
            {
                int index = FindZipEntry(zipfile, "MgResourcePackageManifest.xml");
                if (index < 0)
                    throw new Exception("Failed to locate file MgResourcePackageManifest.xml in zip file. Most likely the file is not a MapGuide package.");

                manifest = m_connection.DeserializeObject<ResourcePackageManifest>(zipfile.GetInputStream(index));
            }

            int i = 0;
            if (Progress != null)
                Progress(ProgressType.ListingFiles, packageFile, manifest.Operations.Operation.Count, i);

            //TODO: Much of this assumes that the package is correctly constructed, ea.: no SETRESOURCEDATA, before a SETRESOURCE and so on.
            foreach (ResourcePackageManifestOperationsOperation op in manifest.Operations.Operation)
            {
                if (Progress != null)
                    Progress(ProgressType.ListingFiles, packageFile, manifest.Operations.Operation.Count, i++);

                if (op.Name.ToLower().Equals("setresource"))
                {
                    string id = op.Parameters.Parameter["RESOURCEID"].Value;
                    string header;
                    if (op.Parameters.Parameter["HEADER"] != null)
                        header = op.Parameters.Parameter["HEADER"].Value;
                    else
                        header = null;
                    string content = op.Parameters.Parameter["CONTENT"] == null ? null : op.Parameters.Parameter["CONTENT"].Value;

                    resourceList.Add(id, new ResourceItem(id, header, content));
                }
                else if (op.Name.ToLower().Equals("setresourcedata"))
                {
                    string id = op.Parameters.Parameter["RESOURCEID"].Value;
                    ResourceItem ri = resourceList[id];
                    string name = op.Parameters.Parameter["DATANAME"].Value;
                    string file = op.Parameters.Parameter["DATA"].Value;
                    string contentType = op.Parameters.Parameter["DATA"].ContentType;
                    string dataType = op.Parameters.Parameter["DATATYPE"].Value;

                    ri.Items.Add(new ResourceDataItem(name, contentType, file, dataType));
                }

                //TODO: What to do with "DELETERESOURCE" ?
            }

            return resourceList;
        }
    }
}
