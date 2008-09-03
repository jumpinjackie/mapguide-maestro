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
using System.Collections.Specialized;
namespace OSGeo.MapGuide.MaestroAPI 
{
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class ResourceReferenceList {
        
		public static readonly string SchemaName = "ResourceReferenceList-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
		
		private StringCollection m_resourceId;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ResourceId")]
        public StringCollection ResourceId {
            get {
                return this.m_resourceId;
            }
            set {
                this.m_resourceId = value;
            }
        }
    }
    
}
