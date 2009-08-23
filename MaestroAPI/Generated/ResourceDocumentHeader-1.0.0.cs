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
    [System.Xml.Serialization.XmlRootAttribute("ResourceDocumentHeader", Namespace="", IsNullable=false)]
    public class ResourceDocumentHeaderType {
        
		public static readonly string SchemaName = "ResourceDocumentHeader-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
		
		private ResourceDocumentHeaderTypeGeneral m_general;
        
        private ResourceSecurityType m_security;
        
        private ResourceDocumentHeaderTypeMetadata m_metadata;
        
        /// <remarks/>
        public ResourceDocumentHeaderTypeGeneral General {
            get {
                return this.m_general;
            }
            set {
                this.m_general = value;
            }
        }
        
        /// <remarks/>
        public ResourceSecurityType Security {
            get {
                return this.m_security;
            }
            set {
                this.m_security = value;
            }
        }
        
        /// <remarks/>
        public ResourceDocumentHeaderTypeMetadata Metadata {
            get {
                return this.m_metadata;
            }
            set {
                this.m_metadata = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourceDocumentHeaderTypeGeneral {
        
        private string m_iconName;
        
        /// <remarks/>
        public string IconName {
            get {
                return this.m_iconName;
            }
            set {
                this.m_iconName = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourceDocumentHeaderTypeMetadataSimpleProperty {
        
        private string m_name;
        
        private string m_value;
        
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
        public string Value {
            get {
                return this.m_value;
            }
            set {
                this.m_value = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourceDocumentHeaderTypeMetadataSimple {
        
        private ResourceDocumentHeaderTypeMetadataSimplePropertyCollection m_property;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Property")]
        public ResourceDocumentHeaderTypeMetadataSimplePropertyCollection Property {
            get {
                return this.m_property;
            }
            set {
                this.m_property = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourceDocumentHeaderTypeMetadata {
        
        private ResourceDocumentHeaderTypeMetadataSimple m_simple;
        
        /// <remarks/>
        public ResourceDocumentHeaderTypeMetadataSimple Simple {
            get {
                return this.m_simple;
            }
            set {
                this.m_simple = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourceSecurityTypeGroupsGroup {
        
        private string m_name;
        
        private PermissionsType m_permissions;
        
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
        public PermissionsType Permissions {
            get {
                return this.m_permissions;
            }
            set {
                this.m_permissions = value;
            }
        }
    }
    
    /// <remarks/>
    public enum PermissionsType {
        
        /// <remarks/>
        n,
        
        /// <remarks/>
        r,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("r,w")]
        rw,
    }
    
    /// <remarks/>
    public class ResourceSecurityTypeGroups {
        
        private ResourceSecurityTypeGroupsGroupCollection m_group;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Group")]
        public ResourceSecurityTypeGroupsGroupCollection Group {
            get {
                return this.m_group;
            }
            set {
                this.m_group = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourceSecurityTypeUsersUser {
        
        private string m_name;
        
        private PermissionsType m_permissions;
        
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
        public PermissionsType Permissions {
            get {
                return this.m_permissions;
            }
            set {
                this.m_permissions = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourceSecurityTypeUsers {
        
        private ResourceSecurityTypeUsersUserCollection m_user;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("User")]
        public ResourceSecurityTypeUsersUserCollection User {
            get {
                return this.m_user;
            }
            set {
                this.m_user = value;
            }
        }
    }
        
    public class ResourceDocumentHeaderTypeMetadataSimplePropertyCollection : System.Collections.CollectionBase {
        
        public ResourceDocumentHeaderTypeMetadataSimpleProperty this[int idx] {
            get {
                return ((ResourceDocumentHeaderTypeMetadataSimpleProperty)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }

        public string this[string idx]
        {
            get
            {
                int x = Find(idx);
                if (x >= 0)
                    return ((ResourceDocumentHeaderTypeMetadataSimpleProperty)base.InnerList[x]).Value;
                else
                    return null;
            }
            set
            {
                int x = Find(idx);
                if (x >= 0)
                {
                    if (value == null)
                        base.InnerList.RemoveAt(x);
                    else
                        ((ResourceDocumentHeaderTypeMetadataSimpleProperty)base.InnerList[x]).Value = value;
                }
                else
                {
                    if (value != null)
                    {
                        ResourceDocumentHeaderTypeMetadataSimpleProperty p = new ResourceDocumentHeaderTypeMetadataSimpleProperty();
                        p.Name = idx;
                        p.Value = value;
                        base.InnerList.Add(p);
                    }
                }   
            }
        }

        public int Find(string x)
        {
            for (int i = 0; i < base.InnerList.Count; i++)
                if (((ResourceDocumentHeaderTypeMetadataSimpleProperty)base.InnerList[i]).Name.ToLower().Equals(x.ToLower()))
                    return i;
            return -1;
        }

        public int Add(ResourceDocumentHeaderTypeMetadataSimpleProperty value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class ResourceSecurityTypeGroupsGroupCollection : System.Collections.CollectionBase {
        
        public ResourceSecurityTypeGroupsGroup this[int idx] {
            get {
                return ((ResourceSecurityTypeGroupsGroup)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(ResourceSecurityTypeGroupsGroup value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class ResourceSecurityTypeUsersUserCollection : System.Collections.CollectionBase {
        
        public ResourceSecurityTypeUsersUser this[int idx] {
            get {
                return ((ResourceSecurityTypeUsersUser)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(ResourceSecurityTypeUsersUser value) {
            return base.InnerList.Add(value);
        }
    }
}
