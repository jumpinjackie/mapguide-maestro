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
    public class Role {
        
		public static readonly string SchemaName = "Role-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
		
		private string m_description;
        
        private RoleUsers m_users;
        
        private RoleGroups m_groups;
        
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
        public RoleUsers Users {
            get {
                return this.m_users;
            }
            set {
                this.m_users = value;
            }
        }
        
        /// <remarks/>
        public RoleGroups Groups {
            get {
                return this.m_groups;
            }
            set {
                this.m_groups = value;
            }
        }
    }
    
    /// <remarks/>
    public class RoleUsers {
        
        private RoleUsersUserCollection m_user;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("User")]
        public RoleUsersUserCollection User {
            get {
                return this.m_user;
            }
            set {
                this.m_user = value;
            }
        }
    }
    
    /// <remarks/>
    public class RoleUsersUser {
        
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
    
    /// <remarks/>
    public class RoleGroupsGroup {
        
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
    
    /// <remarks/>
    public class RoleGroups {
        
        private RoleGroupsGroupCollection m_group;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Group")]
        public RoleGroupsGroupCollection Group {
            get {
                return this.m_group;
            }
            set {
                this.m_group = value;
            }
        }
    }
    
    public class RoleUsersUserCollection : System.Collections.CollectionBase {
        
        public RoleUsersUser this[int idx] {
            get {
                return ((RoleUsersUser)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(RoleUsersUser value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class RoleGroupsGroupCollection : System.Collections.CollectionBase {
        
        public RoleGroupsGroup this[int idx] {
            get {
                return ((RoleGroupsGroup)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(RoleGroupsGroup value) {
            return base.InnerList.Add(value);
        }
    }
}
