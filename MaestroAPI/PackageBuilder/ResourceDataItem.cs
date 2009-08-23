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
