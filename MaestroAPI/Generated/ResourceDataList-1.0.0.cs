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
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class ResourceDataList {
        
		public static readonly string SchemaName = "ResourceDataList-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
		
		private ResourceDataListResourceDataCollection m_resourceData;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ResourceData")]
        public ResourceDataListResourceDataCollection ResourceData {
            get {
                return this.m_resourceData;
            }
            set {
                this.m_resourceData = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourceDataListResourceData {
        
        private string m_name;
        
        private ResourceDataType m_type;
        
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
        public ResourceDataType Type {
            get {
                return this.m_type;
            }
            set {
                this.m_type = value;
            }
        }
    }
    
    /// <remarks/>
    public enum ResourceDataType {
        
        /// <remarks/>
        File,
        
        /// <remarks/>
        Stream,
        
        /// <remarks/>
        String,
    }
    
    public class ResourceDataListResourceDataCollection : System.Collections.CollectionBase {
        
        public ResourceDataListResourceData this[int idx] {
            get {
                return ((ResourceDataListResourceData)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(ResourceDataListResourceData value) {
            return base.InnerList.Add(value);
        }
    }
}
