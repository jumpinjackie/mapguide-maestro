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
using System.Collections;

namespace OSGeo.MapGuide.MaestroAPI 
{
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class UserList {
        
		public static readonly string SchemaName = "UserList-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
		
		private ArrayList m_items;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("User", typeof(UserListUser))]
        [System.Xml.Serialization.XmlElementAttribute("Group", typeof(UserListGroup))]
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
    public class UserListUser {
        
        private string m_name;
        
        private string m_fullName;
        
        private string m_password;
        
        private string m_description;
        
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
        public string FullName {
            get {
                return this.m_fullName;
            }
            set {
                this.m_fullName = value;
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                return this.m_password;
            }
            set {
                this.m_password = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.m_description;
            }
            set {
                this.m_description = value;
            }
        }
    }
    
    /// <remarks/>
    public class UserListGroup {
        
        private string m_name;
        
        private string m_description;
        
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
        public string Description {
            get {
                return this.m_description;
            }
            set {
                this.m_description = value;
            }
        }
    }
 }
