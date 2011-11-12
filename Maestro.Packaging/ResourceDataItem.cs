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

namespace Maestro.Packaging
{
    /// <summary>
    /// Represents a resource data item from a package
    /// </summary>
    public class ResourceDataItem
    {
        private string m_resourceName;
        private string m_contentType;
        private string m_filename;
        private string m_dataType;
        private EntryTypeEnum m_entryType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceDataItem"/> class.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="dataType">Type of the data.</param>
        public ResourceDataItem(string resourceName, string contentType, string filename, string dataType)
        {
            m_resourceName = resourceName;
            m_contentType = contentType;
            m_filename = filename;
            m_dataType = dataType;
            m_entryType = EntryTypeEnum.Regular;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceDataItem"/> class.
        /// </summary>
        /// <param name="rdi">The rdi.</param>
        public ResourceDataItem(ResourceDataItem rdi)
        {
            m_resourceName = rdi.m_resourceName;
            m_contentType = rdi.m_contentType;
            m_filename = rdi.m_filename;
            m_dataType = rdi.m_dataType;
            m_entryType = rdi.m_entryType;
        }

        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>The name of the resource.</value>
        public string ResourceName
        {
            get { return m_resourceName; }
            set { m_resourceName = value; }
        }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType
        {
            get { return m_contentType; }
            set { m_contentType = value; }
        }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>The filename.</value>
        public string Filename
        {
            get { return m_filename; }
            set { m_filename = value; }
        }

        /// <summary>
        /// Gets or sets the type of the entry.
        /// </summary>
        /// <value>The type of the entry.</value>
        public EntryTypeEnum EntryType
        {
            get { return m_entryType; }
            set { m_entryType = value; }
        }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public string DataType
        {
            get { return m_dataType; }
            set { m_dataType = value; }
        }
    }
}
