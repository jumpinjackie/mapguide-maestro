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
                        AddFolderResource(manifest, temppath, folder, m_removeExisting);
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

                        AddFileResource(manifest, temppath, doc, contentname, m_removeExisting);

                        m_progress.SetCurrentProgress(i++, itemCount);

                        foreach (ResourceDataListResourceData rd in rdl.ResourceData)
                        {
                            System.IO.FileInfo fi = new System.IO.FileInfo(filebase + "_DATA_" + rd.Name);
                            using (System.IO.FileStream fs = new System.IO.FileStream(fi.FullName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                m_connection.GetResourceData(doc.ResourceId, rd.Name).WriteTo(fs);

                            AddResourceData(manifest, temppath, doc, fi, rd);

                            m_progress.SetCurrentProgress(i++, itemCount);
                        }

                        m_progress.SetOperationNo(opno++);
                    }

                    using (System.IO.FileStream fs = new System.IO.FileStream(System.IO.Path.Combine(temppath, "MgResourcePackageManifest.xml"), System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                        m_connection.SerializeObject(manifest, fs);

                    if (!string.IsNullOrEmpty(m_alternatetarget))
                        RemapFiles(temppath, m_startingpoint, m_alternatetarget);

                    m_progress.SetOperation("Compressing files");
                    m_progress.SetCurrentProgress(0, 100);

                    ICSharpCode.SharpZipLib.Checksums.Crc32 crc = new ICSharpCode.SharpZipLib.Checksums.Crc32();
                    using(System.IO.FileStream ofs = new System.IO.FileStream(m_targetfile,  System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    using (ICSharpCode.SharpZipLib.Zip.ZipOutputStream zip = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(ofs))
                    {
                        try
                        {
                            zip.SetLevel(9);
                            zip.SetComment("MapGuide Package created by Maestro");
                            //zip.BeginUpdate();

                            //Replicate file system inside the zip file
                            Queue<string> folderlist = new Queue<string>();
                            Queue<string> filelist = new Queue<string>();
                            folderlist.Enqueue(temppath);

                            while (folderlist.Count != 0)
                            {
                                string fl = folderlist.Dequeue();

                                foreach (string f in System.IO.Directory.GetDirectories(fl))
                                {
                                    //zip.AddDirectory(RelativeName(f, temppath)); 
                                    folderlist.Enqueue(f);
                                }
                                foreach (string f in System.IO.Directory.GetFiles(fl))
                                    filelist.Enqueue(f);
                            }

                            m_progress.SetCurrentProgress(0, filelist.Count);
                            int i = 0;

                            foreach (string s in filelist)
                            {
                                //TODO: If the files are 100Mb, this needs to be handled differently
                                byte[] data;
                                using (System.IO.FileStream fs = new System.IO.FileStream(s, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                                    data = MaestroAPI.Utility.StreamAsArray(fs);

                                crc.Reset();
                                crc.Update(data);
                                ICSharpCode.SharpZipLib.Zip.ZipEntry ze = new ICSharpCode.SharpZipLib.Zip.ZipEntry(RelativeName(s, temppath));
                                ze.Crc = crc.Value;
                                ze.DateTime = System.IO.File.GetLastWriteTime(s);
                                ze.Size = data.Length;
                                zip.PutNextEntry(ze);
                                zip.Write(data, 0, data.Length);
                                m_progress.SetCurrentProgress(i++, filelist.Count);

                            }

                            zip.Finish();
                            m_progress.SetOperationNo(opno++);
                            m_progress.SetOperation("Finished");
                            m_progress.Close();
                        }
                        finally
                        {
                            try { zip.Close(); }
                            catch { }
                        }
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
            catch (Exception ex)
            {
                string s = ex.Message;
                System.Windows.Forms.MessageBox.Show(string.Format("Failed to create package, error message: {0}", ex.Message), System.Windows.Forms.Application.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                m_progress.Cancel();
            }
        }

        private void AddResourceData(ResourcePackageManifest manifest, string temppath, ResourceListResourceDocument doc, System.IO.FileInfo fi, ResourceDataListResourceData rd)
        {
            ResourcePackageManifestOperationsOperation op = new ResourcePackageManifestOperationsOperation();
            op.Name = "SETRESOURCEDATA";
            op.Version = "1.0.0";
            op.Parameters = new ResourcePackageManifestOperationsOperationParameters();
            op.Parameters.Parameter = new ResourcePackageManifestOperationsOperationParametersParameterCollection();

            ResourcePackageManifestOperationsOperationParametersParameter param = new ResourcePackageManifestOperationsOperationParametersParameter();

            string contentType = "application/octet-stream";
            try
            {
                contentType = (m_connection as HttpServerConnection).LastResponseHeaders[System.Net.HttpResponseHeader.ContentType];
            }
            catch
            {
            }

            param.Name = "DATA";
            param.Value = RelativeName(fi.FullName, temppath).Replace('\\', '/');
            param.ContentType = contentType;
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "DATALENGTH";
            param.Value = fi.Length.ToString();
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "DATANAME";
            param.Value = rd.Name;
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "DATATYPE";
            param.Value = rd.Type.ToString();
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "RESOURCEID";
            param.Value = doc.ResourceId;
            op.Parameters.Parameter.Add(param);

            manifest.Operations.Operation.Add(op);
        }

        private void AddFileResource(ResourcePackageManifest manifest, string temppath, ResourceListResourceDocument doc, string contentfilename, bool eraseFirst)
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
                delparam.Value = doc.ResourceId;
                delop.Parameters.Parameter.Add(delparam);
                manifest.Operations.Operation.Add(delop);
            }

            ResourcePackageManifestOperationsOperation op = new ResourcePackageManifestOperationsOperation();
            op.Name = "SETRESOURCE";
            op.Version = "1.0.0";
            op.Parameters = new ResourcePackageManifestOperationsOperationParameters();
            op.Parameters.Parameter = new ResourcePackageManifestOperationsOperationParametersParameterCollection();

            ResourcePackageManifestOperationsOperationParametersParameter param = new ResourcePackageManifestOperationsOperationParametersParameter();

            string filebase = CreateFolderForResource(doc.ResourceId, temppath);

            using (System.IO.FileStream fs = new System.IO.FileStream(filebase + "_HEADER.xml", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                m_connection.SerializeObject(m_connection.GetResourceHeader(doc.ResourceId), fs);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "CONTENT";
            param.Value = RelativeName(contentfilename, temppath).Replace('\\', '/');
            param.ContentType = "text/xml";
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "HEADER";
            param.Value = RelativeName(filebase + "_HEADER.xml", temppath).Replace('\\', '/');
            param.ContentType = "text/xml";
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "RESOURCEID";
            param.Value = doc.ResourceId;
            op.Parameters.Parameter.Add(param);

            manifest.Operations.Operation.Add(op);
        }

        private void AddFolderResource(ResourcePackageManifest manifest, string temppath, ResourceListResourceFolder folder, bool eraseFirst)
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
                delparam.Value = folder.ResourceId;
                delop.Parameters.Parameter.Add(delparam);
                manifest.Operations.Operation.Add(delop);
            }

            ResourcePackageManifestOperationsOperation op = new ResourcePackageManifestOperationsOperation();
            op.Name = "SETRESOURCE";
            op.Version = "1.0.0";
            op.Parameters = new ResourcePackageManifestOperationsOperationParameters();
            op.Parameters.Parameter = new ResourcePackageManifestOperationsOperationParametersParameterCollection();

            ResourcePackageManifestOperationsOperationParametersParameter param = new ResourcePackageManifestOperationsOperationParametersParameter();

            string filebase = System.IO.Path.GetDirectoryName(CreateFolderForResource(folder.ResourceId + "dummy.xml", temppath));

            using (System.IO.FileStream fs = new System.IO.FileStream(System.IO.Path.Combine(filebase, "_HEADER.xml"), System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                m_connection.SerializeObject(m_connection.GetFolderHeader(folder.ResourceId), fs);

            if (!filebase.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                filebase += System.IO.Path.DirectorySeparatorChar;

            param.Name = "HEADER";
            param.Value = RelativeName(filebase + "_HEADER.xml", temppath).Replace('\\', '/');
            param.ContentType = "text/xml";
            op.Parameters.Parameter.Add(param);

            param = new ResourcePackageManifestOperationsOperationParametersParameter();
            param.Name = "RESOURCEID";
            param.Value = folder.ResourceId;
            op.Parameters.Parameter.Add(param);

            manifest.Operations.Operation.Add(op);
        }

        private string RelativeName(string filebase, string temppath)
        {
            if (!filebase.StartsWith(temppath))
                throw new Exception(string.Format("Filename \"{0}\" is not relative to \"{1}\"", filebase, temppath));
            if (!temppath.EndsWith(System.IO.Path.PathSeparator.ToString()))
                temppath += System.IO.Path.PathSeparator;
            return filebase.Substring(temppath.Length);
        }

        private string CreateFolderForResource(string resourceId, string temppath)
        {
            string filebase = m_connection.GetResourceName(resourceId, false);
            string folder = "Library/" + m_connection.GetResourceName(resourceId, true);
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

        private void RemapFiles(string tempdir, string origpath, string newpath)
        {
            //TODO: Implement this
        }

    }
}
