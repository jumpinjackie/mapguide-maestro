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
using System.Collections;

namespace OSGeo.MapGuide.MaestroAPI 
{
    
        
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class ResourceList {
        
		public static readonly string SchemaName = "ResourceList-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
		
		private ArrayList m_items;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ResourceFolder", typeof(ResourceListResourceFolder))]
        [System.Xml.Serialization.XmlElementAttribute("ResourceDocument", typeof(ResourceListResourceDocument))]
        public ArrayList Items {
            get {
                return this.m_items;
            }
            set {
                this.m_items = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourceListResourceFolder {
        
        private string m_resourceId;
        
        private string m_depth;
        
        private string m_owner;
        
        private System.DateTime m_createdDate;
        
        private System.DateTime m_modifiedDate;
        
        private string m_numberOfFolders;
        
        private string m_numberOfDocuments;
        
        private ResourceFolderHeaderType m_resourceFolderHeader;
        
        /// <remarks/>
        public string ResourceId {
            get {
                return this.m_resourceId;
            }
            set {
                this.m_resourceId = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string Depth {
            get {
                return this.m_depth;
            }
            set {
                this.m_depth = value;
            }
        }
        
        /// <remarks/>
        public string Owner {
            get {
                return this.m_owner;
            }
            set {
                this.m_owner = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime CreatedDate {
            get {
                return this.m_createdDate;
            }
            set {
                this.m_createdDate = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime ModifiedDate {
            get {
                return this.m_modifiedDate;
            }
            set {
                this.m_modifiedDate = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string NumberOfFolders {
            get {
                return this.m_numberOfFolders;
            }
            set {
                this.m_numberOfFolders = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string NumberOfDocuments {
            get {
                return this.m_numberOfDocuments;
            }
            set {
                this.m_numberOfDocuments = value;
            }
        }
        
        /// <remarks/>
        public ResourceFolderHeaderType ResourceFolderHeader {
            get {
                return this.m_resourceFolderHeader;
            }
            set {
                this.m_resourceFolderHeader = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourceListResourceDocument {
        
        private string m_resourceId;
        
        private string m_depth;
        
        private string m_owner;
        
        private System.DateTime m_createdDate;
        
        private System.DateTime m_modifiedDate;
        
        private ResourceDocumentHeaderType m_resourceDocumentHeader;
        
        /// <remarks/>
        public string ResourceId {
            get {
                return this.m_resourceId;
            }
            set {
                this.m_resourceId = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string Depth {
            get {
                return this.m_depth;
            }
            set {
                this.m_depth = value;
            }
        }
        
        /// <remarks/>
        public string Owner {
            get {
                return this.m_owner;
            }
            set {
                this.m_owner = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime CreatedDate {
            get {
                return this.m_createdDate;
            }
            set {
                this.m_createdDate = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime ModifiedDate {
            get {
                return this.m_modifiedDate;
            }
            set {
                this.m_modifiedDate = value;
            }
        }
        
        /// <remarks/>
        public ResourceDocumentHeaderType ResourceDocumentHeader {
            get {
                return this.m_resourceDocumentHeader;
            }
            set {
                this.m_resourceDocumentHeader = value;
            }
        }
    }
    
}
