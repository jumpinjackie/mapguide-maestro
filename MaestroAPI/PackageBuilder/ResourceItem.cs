#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
    /// Represents a resource item from a package
    /// </summary>
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
}
