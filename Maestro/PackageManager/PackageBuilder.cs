using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.PackageManager
{
    public class PackageBuilder
    {
        private System.Windows.Forms.Form m_owner;
        private string m_startingpoint;
        private string m_targetfile;
        private string m_alternatetarget;
        private bool m_removeExisting;
        private ServerConnectionI m_connection;
        private Dictionary<string, string> m_extensions;

        private PackageProgress m_progress;
        private System.Threading.Thread m_thread;

        public PackageBuilder(System.Windows.Forms.Form owner, string startingpoint, string targetfile, string alternatetarget, string[] supportedExtensions, bool removeExisting, ServerConnectionI connection)
        {
            m_owner = owner;
            m_startingpoint = startingpoint;
            m_targetfile = targetfile;
            m_alternatetarget = alternatetarget;
            m_connection = connection;
            m_removeExisting = removeExisting;

            m_extensions = new Dictionary<string, string>();
            if (supportedExtensions != null)
                foreach (string s in supportedExtensions)
                    m_extensions.Add(s.ToLower(), null);

        }

        public System.Windows.Forms.DialogResult Start()
        {
            m_progress = new PackageProgress();
            m_thread = new System.Threading.Thread(new System.Threading.ThreadStart(Runner));
            m_thread.IsBackground = true;
            m_thread.Start();
            return m_progress.ShowDialog(m_owner);
        }

        private void Runner()
        {
            try
            {
                m_progress.SetCurrentProgress(0, 100);
                m_progress.SetOperation("Getting file list");
                ResourceList items = m_connection.GetRepositoryResources(m_startingpoint);

                List<ResourceListResourceDocument> files = new List<ResourceListResourceDocument>();
                List<ResourceListResourceFolder> folders = new List<ResourceListResourceFolder>();
                Dictionary<string, List<ResourceDataListResourceData>> resourceData = new Dictionary<string, List<ResourceDataListResourceData>>();
                ResourcePackageManifest manifest = new ResourcePackageManifest();
                manifest.Description = "MapGuide Package created with Maestro";
                manifest.Operations = new ResourcePackageManifestOperations();
                manifest.Operations.Operation = new ResourcePackageManifestOperationsOperationCollection();
                System.Collections.Hashtable knownTypes =((MaestroAPI.ServerConnectionBase) m_connection).ResourceTypeLookup;

                foreach (object o in items.Items)
                    if (o as ResourceListResourceDocument != null)
                    {
                        ResourceListResourceDocument doc = o as ResourceListResourceDocument;
                        string extension = doc.ResourceId.Substring(doc.ResourceId.LastIndexOf('.'));
                        if (m_extensions == null || m_extensions.Count == 0)
                            files.Add(doc);
                        else if (knownTypes.ContainsKey(extension) && m_extensions.ContainsKey(extension))
                            files.Add(doc);
                        else if (!knownTypes.ContainsKey(extension) && m_extensions.ContainsKey("*"))
                            files.Add(doc);
                    }
                    else if (o as ResourceListResourceFolder != null)
                    {
                        folders.Add(o as ResourceListResourceFolder);
                    }

                m_progress.SetTotalOperations(files.Count + folders.Count + 1);

                string temppath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());

                try
                {
                    System.IO.Directory.CreateDirectory(temppath);
                    int opno = 1;

                    foreach (ResourceListResourceFolder folder in folders)
                    {

                        m_progress.SetOperation("Creating folder " + folder.ResourceId);
                        m_progress.SetCurrentProgress(0, 100);
                        AddFolderResource(manifest, temppath, folder, m_removeExisting, m_connection);
                        m_progress.SetCurrentProgress(100, 100);
                        m_progress.SetOperationNo(opno++);
                    }

                    foreach (ResourceListResourceDocument doc in files)
                    {
                        m_progress.SetOperation("Downloading " + doc.ResourceId);
                        m_progress.SetCurrentProgress(0, 100);

                        string filebase = CreateFolderForResource(doc.ResourceId, temppath);

                        resourceData[doc.ResourceId] = new List<ResourceDataListResourceData>();
                        ResourceDataList rdl = m_connection.EnumerateResourceData(doc.ResourceId);
                        foreach(ResourceDataListResourceData rd in rdl.ResourceData)
                            resourceData[doc.ResourceId].Add(rd);

                        int i = 0;
                        int itemCount = resourceData[doc.ResourceId].Count + 1;

                        m_progress.SetCurrentProgress(i++, itemCount);

                        string contentname = filebase + "_CONTENT.xml";
                        using (System.IO.FileStream fs = new System.IO.FileStream(contentname, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                        {
                            byte[] data = m_connection.GetResourceXmlData(doc.ResourceId);
                            fs.Write(data, 0, data.Length);
                        }

                        AddFileResource(manifest, temppath, doc, contentname, m_removeExisting, m_connection);

                        m_progress.SetCurrentProgress(i++, itemCount);

                        foreach (ResourceDataListResourceData rd in rdl.ResourceData)
                        {
                            System.IO.FileInfo fi = new System.IO.FileInfo(filebase + "_DATA_" + rd.Name);
                            using (System.IO.FileStream fs = new System.IO.FileStream(fi.FullName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                m_connection.GetResourceData(doc.ResourceId, rd.Name).WriteTo(fs);

                            AddResourceData(manifest, temppath, doc, fi, rd, m_connection);

                            m_progress.SetCurrentProgress(i++, itemCount);
                        }

                        m_progress.SetOperationNo(opno++);
                    }

                    if (!string.IsNullOrEmpty(m_alternatetarget))
                        RemapFiles(manifest, temppath, m_startingpoint, m_alternatetarget);

                    using (System.IO.FileStream fs = new System.IO.FileStream(System.IO.Path.Combine(temppath, "MgResourcePackageManifest.xml"), System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                        m_connection.SerializeObject(manifest, fs);


                    m_progress.SetOperation("Compressing files");
                    m_progress.SetCurrentProgress(0, 100);

                    ZipDirectory(m_targetfile, temppath, "MapGuide Package created by Maestro", m_progress);

                    m_progress.SetOperationNo(opno++);
                    m_progress.SetOperation("Finished");
                    m_progress.Close();

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
            catch (Exception ex)
            {
                string s = ex.Message;
                System.Windows.Forms.MessageBox.Show(string.Format("Failed to create package, error message: {0}", ex.Message), System.Windows.Forms.Application.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                m_progress.Cancel();
            }
        }

        public static void AddResourceData(ResourcePackageManifest manifest, string temppath, ResourceListResourceDocument doc, System.IO.FileInfo fi, ResourceDataListResourceData rd, ServerConnectionI connection)
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
            string filename = RelativeName(fi.FullName, temppath).Replace('\\', '/');
            long size = fi.Length;

            AddResourceData(manifest, resourceId, contentType, type, name, filename, size);
        }

        public static void AddResourceData(ResourcePackageManifest manifest, string resourceId, string contentType, string type, string name, string filename, long size)
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

        public static void AddFileResource(ResourcePackageManifest manifest, string temppath, ResourceListResourceDocument doc, string contentfilename, bool eraseFirst, ServerConnectionI connection)
        {
            string filebase = CreateFolderForResource(connection, doc.ResourceId, temppath);

            using (System.IO.FileStream fs = new System.IO.FileStream(filebase + "_HEADER.xml", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                connection.SerializeObject(connection.GetResourceHeader(doc.ResourceId), fs);

            string headerpath = RelativeName(filebase + "_HEADER.xml", temppath).Replace('\\', '/');
            string contentpath = RelativeName(contentfilename, temppath).Replace('\\', '/');
            AddFileResource(manifest, doc.ResourceId, headerpath, contentpath, eraseFirst);
        }

        public static void AddFileResource(ResourcePackageManifest manifest, string resourceId, string headerpath, string contentpath, bool eraseFirst)
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

        public static void AddFolderResource(ResourcePackageManifest manifest, string temppath, ResourceListResourceFolder folder, bool eraseFirst, ServerConnectionI connection)
        {
            string filebase = System.IO.Path.GetDirectoryName(CreateFolderForResource(connection, folder.ResourceId + "dummy.xml", temppath));

            using (System.IO.FileStream fs = new System.IO.FileStream(System.IO.Path.Combine(filebase, "_HEADER.xml"), System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                connection.SerializeObject(connection.GetFolderHeader(folder.ResourceId), fs);

            if (!filebase.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                filebase += System.IO.Path.DirectorySeparatorChar;

            string headerpath = RelativeName(filebase + "_HEADER.xml", temppath).Replace('\\', '/');

            AddFolderResource(manifest, folder.ResourceId, headerpath, eraseFirst);
        }


        public static void AddFolderResource(ResourcePackageManifest manifest, string resourceId, string headerpath, bool eraseFirst)
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

        public static string RelativeName(string filebase, string temppath)
        {
            if (!filebase.StartsWith(temppath))
                throw new Exception(string.Format("Filename \"{0}\" is not relative to \"{1}\"", filebase, temppath));
            if (!temppath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                temppath += System.IO.Path.DirectorySeparatorChar;
            return filebase.Substring(temppath.Length);
        }

        public static string CreateFolderForResource(ServerConnectionI connection, string resourceId, string temppath)
        {
            string filebase = connection.GetResourceName(resourceId, false);
            string folder = "Library/" + connection.GetResourceName(resourceId, true);
            folder = folder.Substring(0, folder.Length - filebase.Length);
            filebase += resourceId.Substring(resourceId.LastIndexOf('.'));

            folder = folder.Replace('/', System.IO.Path.DirectorySeparatorChar);
            folder = System.IO.Path.Combine(temppath, folder);

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            //filebase = System.IO.Path.GetFileNameWithoutExtension(filebase);
            filebase = System.IO.Path.Combine(folder, filebase);

            return filebase;
        }

        private string CreateFolderForResource(string resourceId, string temppath)
        {
            return CreateFolderForResource(m_connection, resourceId, temppath);
        }

        private void RemapFiles(MaestroAPI.ResourcePackageManifest manifest, string tempdir, string origpath, string newpath)
        {
            if (!newpath.EndsWith("/"))
                newpath += "/";
            if (!origpath.EndsWith("/"))
                origpath += "/";

            foreach (MaestroAPI.ResourcePackageManifestOperationsOperation op in manifest.Operations.Operation)
            {
                op.Parameters.Parameter["RESOURCEID"].Value = newpath + op.Parameters.Parameter["RESOURCEID"].Value.Substring(origpath.Length);
                if (op.Parameters.Parameter["CONTENT"] != null)
                {
                    string path = System.IO.Path.Combine(tempdir, op.Parameters.Parameter["CONTENT"].Value.Replace('/', System.IO.Path.DirectorySeparatorChar));
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.Load(path);
                    ((ServerConnectionBase)m_connection).UpdateResourceReferences(doc, origpath, newpath, true);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    doc.Save(ms);
                    System.IO.MemoryStream ms2 = Utility.RemoveUTF8BOM(ms);
                    if (ms2 != ms)
                        ms.Dispose();

                    ms2.Position = 0;
                    using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                        ms2.WriteTo(fs);
                    ms2.Dispose();
                }
            }
        }

        public static void ZipDirectory(string zipfile, string folder, string comment, PackageProgress progress)
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

                    //Replicate file system inside the zip file
                    Queue<string> folderlist = new Queue<string>();
                    Queue<string> filelist = new Queue<string>();
                    folderlist.Enqueue(folder);

                    while (folderlist.Count != 0)
                    {
                        string fl = folderlist.Dequeue();

                        foreach (string f in System.IO.Directory.GetDirectories(fl))
                            folderlist.Enqueue(f);

                        foreach (string f in System.IO.Directory.GetFiles(fl))
                            filelist.Enqueue(f);
                    }

                    if (progress != null)
                        progress.SetCurrentProgress(0, filelist.Count);
                    int i = 0;

                    foreach (string s in filelist)
                    {
                        //TODO: If the files are 100Mb, this needs to be handled differently
                        byte[] data;
                        using (System.IO.FileStream fs = new System.IO.FileStream(s, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                            data = MaestroAPI.Utility.StreamAsArray(fs);

                        crc.Reset();
                        crc.Update(data);
                        ICSharpCode.SharpZipLib.Zip.ZipEntry ze = new ICSharpCode.SharpZipLib.Zip.ZipEntry(RelativeName(s, folder).Replace('\\', '/'));
                        ze.Crc = crc.Value;
                        ze.DateTime = System.IO.File.GetLastWriteTime(s);
                        ze.Size = data.Length;
                        zip.PutNextEntry(ze);
                        zip.Write(data, 0, data.Length);
                        if (progress != null)
                            progress.SetCurrentProgress(i++, filelist.Count);

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

    }
}
