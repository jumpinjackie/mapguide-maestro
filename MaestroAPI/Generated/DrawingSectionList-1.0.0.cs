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
namespace OSGeo.MapGuide.MaestroAPI {
    
    
    /// <summary>
    /// Represents a Drawing Section
    /// </summary>
    [System.Xml.Serialization.XmlRootAttribute("DrawingSectionList", Namespace="", IsNullable=false)]
    public class DrawingSectionList {


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
		public static readonly string SchemaName = "DrawingSectionList-1.0.0.xsd";
        
		/// <summary>
		/// Gets the name of the xsd document that will be used to validate this class before and after serialization
		/// </summary>
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

        
        private DrawingSectionCollection m_section;
        
        /// <summary>
        /// Gets or sets a list of drawing sections
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Section")]
        public DrawingSectionCollection Section {
            get {
                return this.m_section;
            }
            set {
                this.m_section = value;
            }
        }
    }
    
    /// <summary>
    /// Represents a single drawing section
    /// </summary>
    public class DrawingSection {
        
        private string m_name;
        private string m_type;
        private string m_title;

        /// <summary>
        /// Gets or sets the name of this drawing section
        /// </summary>
        public string Name {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        
        /// <summary>
        /// Gets or sets the type of this drawing section
        /// </summary>
        public string Type {
            get { return this.m_type; }
            set { this.m_type = value; }
        }
        
        /// <summary>
        /// Gets or sets the title of this drawing section
        /// </summary>
        public string Title {
            get { return this.m_title; }
			set { this.m_title = value; }        
		}
    }
    
	/// <summary>
	/// Represents a list of drawing sections
	/// </summary>
    public class DrawingSectionCollection : System.Collections.CollectionBase {
        
		/// <summary>
		/// Gets or sets a drawing section, based on the item index
		/// </summary>
        public DrawingSection this[int idx] {
            get { return ((DrawingSection)(base.InnerList[idx])); }
            set { base.InnerList[idx] = value; }
        }
        
		/// <summary>
		/// Appends a drawing section to the end of the list
		/// </summary>
		/// <param name="value">The drawing section to append</param>
		/// <returns>The newly assigned index</returns>
        public int Add(DrawingSection value) {
            return base.InnerList.Add(value);
        }
    }
}
