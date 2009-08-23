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
    public class Group {
        
		public static readonly string SchemaName = "Group-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
		
		private string m_description;
        
        private GroupUsers m_users;
        
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
        public GroupUsers Users {
            get {
                return this.m_users;
            }
            set {
                this.m_users = value;
            }
        }
    }
    
    /// <remarks/>
    public class GroupUsers {
        
        private GroupUsersUserCollection m_user;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("User")]
        public GroupUsersUserCollection User {
            get {
                return this.m_user;
            }
            set {
                this.m_user = value;
            }
        }
    }
    
    /// <remarks/>
    public class GroupUsersUser {
        
        private string m_name;
        
        /// <remarks/>
        public string Name {
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }
    }
    
    public class GroupUsersUserCollection : System.Collections.CollectionBase {
        
        public GroupUsersUser this[int idx] {
            get {
                return ((GroupUsersUser)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(GroupUsersUser value) {
            return base.InnerList.Add(value);
        }
    }
}
