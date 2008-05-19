#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute("SymbolLibrary", Namespace="", IsNullable=false)]
    public class SymbolLibraryType {
        
		public static readonly string SchemaName = "SymbolLibrary-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

		private string m_resourceId;
		[System.Xml.Serialization.XmlIgnore()]
		public string ResourceId 
		{ 
			get { return m_resourceId; } 
			set { m_resourceId = value; } 
		}
		
		private string m_description;
        
        private SymbolTypeLibraryCollection m_symbol;
        
        /// <remarks/>
        public string Description {
            get {
                return this.m_description;
            }
            set {
                this.m_description = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Symbol")]
        public SymbolTypeLibraryCollection Symbol {
            get {
                return this.m_symbol;
            }
            set {
                this.m_symbol = value;
            }
        }
    }
    
    /// <remarks/>
    public class SymbolTypeLibrary {
        
        private string m_name;
        
        private string m_resource;
        
        /// <remarks/>
        public string Name {
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }
        
        /// <remarks/>
        public string Resource {
            get {
                return this.m_resource;
            }
            set {
                this.m_resource = value;
            }
        }
    }
    
    public class SymbolTypeLibraryCollection : System.Collections.CollectionBase {
        
        public SymbolTypeLibrary this[int idx] {
            get {
                return ((SymbolTypeLibrary)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(SymbolTypeLibrary value) {
            return base.InnerList.Add(value);
        }
    }
}
