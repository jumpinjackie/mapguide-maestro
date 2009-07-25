using System;
using System.Collections.Generic;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.PackageBuilder
{
    /// <summary>
    /// Represents a data item on a resource
    /// </summary>
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
}
