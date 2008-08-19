using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.PackageManager
{

    public enum EntryTypeEnum
    {
        Regular,
        Deleted,
        Added
    }

    public class ResourceDataItem
    {
        private string m_resourceName;
        private string m_contentType;
        private string m_filename;
        private string m_dataType;
        private EntryTypeEnum m_entryType;

        public ResourceDataItem(string resourceName, string contentType, string filename, string dataType)
        {
            m_resourceName = resourceName;
            m_contentType = contentType;
            m_filename = filename;
            m_dataType = dataType;
            m_entryType = EntryTypeEnum.Regular;
        }

        public ResourceDataItem(ResourceDataItem rdi)
        {
            m_resourceName = rdi.m_resourceName;
            m_contentType = rdi.m_contentType;
            m_filename = rdi.m_filename;
            m_dataType = rdi.m_dataType;
            m_entryType = rdi.m_entryType;
        }

        public string ResourceName
        {
            get { return m_resourceName; }
            set { m_resourceName = value; }
        }

        public string ContentType
        {
            get { return m_contentType; }
            set { m_contentType = value; }
        }

        public string Filename
        {
            get { return m_filename; }
            set { m_filename = value; }
        }

        public EntryTypeEnum EntryType
        {
            get { return m_entryType; }
            set { m_entryType = value; }
        }

        public string DataType
        {
            get { return m_dataType; }
            set { m_dataType = value; }
        }
    }

    public class ResourceItem
    {

        public ResourceItem(string resourcePath, string headerPath, string contentPath)
        {
            m_originalResourcePath = m_resourcePath = resourcePath;
            m_headerpath = headerPath;
            m_contentpath = contentPath;
            m_entryType = EntryTypeEnum.Regular;
            m_items = new List<ResourceDataItem>();
            m_isFolder = m_originalResourcePath.EndsWith("/");
        }

        public ResourceItem(ResourceItem ri)
        {
            m_originalResourcePath = ri.m_originalResourcePath;
            m_headerpath = ri.m_headerpath;
            m_contentpath = ri.m_contentpath;
            m_resourcePath = ri.m_resourcePath;
            m_entryType = ri.m_entryType;
            m_isFolder = ri.m_isFolder;
            m_items = new List<ResourceDataItem>();
            foreach (ResourceDataItem rdi in ri.m_items)
                m_items.Add(new ResourceDataItem(rdi));
        }

        private string m_originalResourcePath;
        private string m_headerpath;
        private string m_contentpath;
        private string m_resourcePath;
        private EntryTypeEnum m_entryType;
        private List<ResourceDataItem> m_items;
        private bool m_isFolder;

        public bool IsFolder
        {
            get { return m_isFolder; }
            set { m_isFolder = true; }
        }

        public List<ResourceDataItem> Items
        {
            get { return m_items; }
            set { m_items = value; }
        }

        public EntryTypeEnum EntryType
        {
            get { return m_entryType; }
            set { m_entryType = value; }
        }


        public string OriginalResourcePath
        {
            get { return m_originalResourcePath; }
            set { m_originalResourcePath = value; }
        }

        public string ResourcePath
        {
            get { return m_resourcePath; }
            set { m_resourcePath = value; }
        }

        public string Contentpath
        {
            get { return m_contentpath; }
            set { m_contentpath = value; }
        }

        public string Headerpath
        {
            get { return m_headerpath; }
            set { m_headerpath = value; }
        }
    }

    class PackageRebuilder
    {
        public const string DEFAULT_HEADER =
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
            "<ResourceFolderHeader xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"ResourceFolderHeader-1.0.0.xsd\">\n" +
            "	<Security xsi:noNamespaceSchemaLocation=\"ResourceSecurity-1.0.0.xsd\">\n" +
            "		<Inherited>true</Inherited>\n" +
            "	</Security>\n" +
            "</ResourceFolderHeader>";

        private System.Threading.Thread m_thread;
        private List<ResourceItem> m_items;
        private string m_tempfolder;
        private ServerConnectionI m_connection;
        private string m_targetfile;
        private Exception m_ex;
        private string m_zipfilename;

        public PackageRebuilder(ServerConnectionI connection, string zipfile, List<ResourceItem> items, string targetfile)
        {
            m_tempfolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            m_items = items;
            m_targetfile = targetfile;
            m_ex = null;
            m_connection = connection;
            m_zipfilename = zipfile;

            m_thread = new System.Threading.Thread(new System.Threading.ThreadStart(RunThread));
            m_thread.IsBackground = true;
            m_thread.Start();
        }

        public bool IsComplete { get { return !m_thread.IsAlive; } }

        public Exception Result { get { return m_ex; } }

        private string MapResourcePathToFolder(string tempfolder, string resourcename)
        {
            return PackageBuilder.CreateFolderForResource(m_connection, resourcename, tempfolder);
        }

        private void RunThread()
        {
            try
            {
                //Step 1: Create the file system layout
                if (!System.IO.Directory.Exists(m_tempfolder))
                    System.IO.Directory.CreateDirectory(m_tempfolder);

                string zipfilecomment;

                using (ICSharpCode.SharpZipLib.Zip.ZipFile zipfile = new ICSharpCode.SharpZipLib.Zip.ZipFile(m_zipfilename))
                {
                    zipfilecomment = zipfile.ZipFileComment;

                    foreach (ResourceItem ri in m_items)
                    {
                        string filebase;
                        if (ri.IsFolder)
                        {
                            filebase = System.IO.Path.GetDirectoryName(MapResourcePathToFolder(m_tempfolder, ri.ResourcePath + "dummy.xml"));
                            if (!filebase.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                                filebase += System.IO.Path.DirectorySeparatorChar;
                        }
                        else
                            filebase = MapResourcePathToFolder(m_tempfolder, ri.ResourcePath);

                        string headerpath = filebase + "_HEADER.xml";
                        string contentpath = filebase + "_CONTENT.xml";

                        if (ri.EntryType == EntryTypeEnum.Added)
                        {
                            if (string.IsNullOrEmpty(ri.Headerpath))
                                using (System.IO.FileStream fs = new System.IO.FileStream(ri.Headerpath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                {
                                    byte[] data = System.Text.Encoding.UTF8.GetBytes(DEFAULT_HEADER);
                                    fs.Write(data, 0, data.Length);
                                }
                            else if (!ri.IsFolder)
                                System.IO.File.Copy(ri.Headerpath, headerpath);

                            System.IO.File.Copy(ri.Contentpath, contentpath);
                        }
                        else if (ri.EntryType == EntryTypeEnum.Regular)
                        {
                            int index = FindZipEntry(zipfile, ri.Headerpath);
                            if (index < 0)
                                throw new Exception(string.Format("Failed to find file {0} in archive", ri.Headerpath));

                            using (System.IO.FileStream fs = new System.IO.FileStream(headerpath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                Utility.CopyStream(zipfile.GetInputStream(index), fs);

                            if (!ri.IsFolder)
                            {
                                index = FindZipEntry(zipfile, ri.Contentpath);
                                if (index < 0)
                                    throw new Exception(string.Format("Failed to find file {0} in archive", ri.Contentpath));

                                using (System.IO.FileStream fs = new System.IO.FileStream(contentpath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                    Utility.CopyStream(zipfile.GetInputStream(index), fs);
                            }

                        }

                        ri.Headerpath = headerpath;
                        ri.Contentpath = contentpath;

                        foreach (ResourceDataItem rdi in ri.Items)
                        {
                            string targetpath = filebase + "_DATA_" + rdi.ResourceName;
                            if (rdi.EntryType == EntryTypeEnum.Added)
                                System.IO.File.Copy(rdi.Filename, targetpath);
                            else
                            {
                                int index = FindZipEntry(zipfile, rdi.Filename);
                                if (index < 0)
                                    throw new Exception(string.Format("Failed to find file {0} in archive", ri.Contentpath));

                                using (System.IO.FileStream fs = new System.IO.FileStream(targetpath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                    Utility.CopyStream(zipfile.GetInputStream(index), fs);
                            }
                            rdi.Filename = targetpath;
                        }
                    }
                }

                //Step 2: Repoint all resources with respect to the update
                foreach (ResourceItem ri in m_items)
                    if (ri.OriginalResourcePath != ri.ResourcePath)
                    {
                        foreach (ResourceItem rix in m_items)
                        {
                            if (!rix.IsFolder)
                            {
                                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                                doc.Load(rix.Contentpath);
                                ((ServerConnectionBase)m_connection).UpdateResourceReferences(doc, ri.OriginalResourcePath, ri.ResourcePath, ri.IsFolder);
                                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                doc.Save(ms);
                                System.IO.MemoryStream ms2 = Utility.RemoveUTF8BOM(ms);
                                if (ms2 != ms)
                                    ms.Dispose();

                                ms2.Position = 0;
                                using (System.IO.FileStream fs = new System.IO.FileStream(rix.Contentpath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                                    ms2.WriteTo(fs);
                                ms2.Dispose();
                            }
                        }
                    }

                //Step 3: Create an updated definition file
                ResourcePackageManifest manifest = new ResourcePackageManifest();
                manifest.Description = "MapGuide Package created by Maestro";
                manifest.Operations = new ResourcePackageManifestOperations();
                manifest.Operations.Operation = new ResourcePackageManifestOperationsOperationCollection();

                bool eraseFirst = true;

                foreach (ResourceItem ri in m_items)
                    if (ri.IsFolder)
                    {
                        PackageBuilder.AddFolderResource(
                            manifest,
                            ri.ResourcePath,
                            PackageBuilder.RelativeName(ri.Headerpath, m_tempfolder).Replace('\\', '/'),
                            eraseFirst);
                    }
                    else
                    {
                        PackageBuilder.AddFileResource(
                            manifest,
                            ri.ResourcePath,
                            PackageBuilder.RelativeName(ri.Headerpath, m_tempfolder).Replace('\\', '/'),
                            PackageBuilder.RelativeName(ri.Contentpath, m_tempfolder).Replace('\\', '/'),
                            eraseFirst);

                        foreach (ResourceDataItem rdi in ri.Items)
                            PackageBuilder.AddResourceData(
                                manifest,
                                ri.ResourcePath,
                                rdi.ContentType,
                                rdi.DataType,
                                rdi.ResourceName,
                                PackageBuilder.RelativeName(rdi.Filename, m_tempfolder).Replace('\\', '/'),
                                new System.IO.FileInfo(rdi.Filename).Length);
                    }

                using (System.IO.FileStream fs = new System.IO.FileStream(System.IO.Path.Combine(m_tempfolder, "MgResourcePackageManifest.xml"), System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    m_connection.SerializeObject(manifest, fs);

                //Step 4: Create the zip file
                PackageBuilder.ZipDirectory(m_targetfile, m_tempfolder, zipfilecomment, null);
            }
            catch (Exception ex)
            {
                m_ex = ex;
            }
            finally
            {
                try { System.IO.Directory.Delete(m_tempfolder, true); }
                catch { }
            }
        }

        public static int FindZipEntry(ICSharpCode.SharpZipLib.Zip.ZipFile file, string path)
        {
            string p = path.Replace('\\', '/');
            foreach (ICSharpCode.SharpZipLib.Zip.ZipEntry ze in file)
                if (ze.Name.Replace('\\', '/').Equals(p))
                    return (int)ze.ZipFileIndex;

            return -1;
        }
    }
}
