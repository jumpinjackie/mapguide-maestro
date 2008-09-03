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
namespace OSGeo.MapGuide.MaestroAPI {
    
    
    /// <summary>
    /// Represents a DrawingSectionResourceList
    /// </summary>
    [System.Xml.Serialization.XmlRootAttribute("DrawingSectionResourceList", Namespace="", IsNullable=false)]
    public class DrawingSectionResourceList {
        
		private ServerConnectionI m_serverConnection;

		/// <summary>
		/// Gets or sets the connection used in various operations performed on this object
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public ServerConnectionI CurrentConnection
		{
			get { return m_serverConnection; }
			set { m_serverConnection = value; }
		}

		/// <summary>
		/// The name of the xsd document that will be used to validate this class before and after serialization
		/// </summary>
		public static readonly string SchemaName = "DrawingSectionResourceList-1.0.0.xsd";
        
		/// <summary>
		/// Gets the name of the xsd document that will be used to validate this class before and after serialization
		/// </summary>
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

        private DrawingSectionResourceCollection m_sectionResource;
        
        /// <summary>
        /// Gets or sets the Section Resources
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SectionResource")]
        public DrawingSectionResourceCollection SectionResources {
            get {
                return this.m_sectionResource;
            }
            set {
                this.m_sectionResource = value;
            }
        }
    }
    
    /// <summary>
    /// Represents a single DrawingSectionResource
    /// </summary>
    public class DrawingSectionResource {
        
        private string m_href;
        private string m_role;
        private string m_mime;
        private string m_title;
        
        /// <summary>
        /// Gets or sets the resource link
        /// </summary>
        public string Href {
            get { return this.m_href; }
            set { this.m_href = value; }
        }
        
        /// <summary>
        /// Gets or sets the resource role
        /// </summary>
        public string Role {
            get { return this.m_role; }
            set { this.m_role = value; }
        }
        
        /// <summary>
        /// Gets or sets the resource mime type
        /// </summary>
        public string Mime {
            get { return this.m_mime; }
            set { this.m_mime = value; }
        }
        
        /// <summary>
        /// Gets or sets the resource title
        /// </summary>
        public string Title {
            get { return this.m_title; }
            set { this.m_title = value; }
        }
    }
    
	/// <summary>
	/// Represents a list of DrawingSectionResources
	/// </summary>
    public class DrawingSectionResourceCollection : System.Collections.CollectionBase {
        
		/// <summary>
		/// Gets or sets an item, using the collection index
		/// </summary>
        public DrawingSectionResource this[int idx] {
            get { return ((DrawingSectionResource)(base.InnerList[idx])); }
            set { base.InnerList[idx] = value; }
        }
        
		/// <summary>
		/// Adds a resource to the collection
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
        public int Add(DrawingSectionResource value) {
            return base.InnerList.Add(value);
        }
    }
}
