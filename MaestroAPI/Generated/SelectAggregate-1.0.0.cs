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
    [System.Xml.Serialization.XmlRootAttribute("PropertySet", Namespace="", IsNullable=false)]
    public class PropertySetType {

		public static readonly string SchemaName = "SelectAggregate-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

        
        private PropertySetTypePropertyDefinitions m_propertyDefinitions;
        
        private PropertySetTypeProperties m_properties;
        
        /// <remarks/>
        public PropertySetTypePropertyDefinitions PropertyDefinitions {
            get {
                return this.m_propertyDefinitions;
            }
            set {
                this.m_propertyDefinitions = value;
            }
        }
        
        /// <remarks/>
        public PropertySetTypeProperties Properties {
            get {
                return this.m_properties;
            }
            set {
                this.m_properties = value;
            }
        }
    }
    
    /// <remarks/>
    public class PropertySetTypePropertyDefinitions : PropertyDefinitionsType {
        
        private PropertyDefinitionTypeCollection m_propertyDefinition;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PropertyDefinition")]
        public PropertyDefinitionTypeCollection PropertyDefinition {
            get {
                return this.m_propertyDefinition;
            }
            set {
                this.m_propertyDefinition = value;
            }
        }
    }
    
    /// <remarks/>
    public class PropertyDefinitionType {
        
        private string m_name;
        
        private PropertyDefinitionTypeType m_type;
        
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
        public PropertyDefinitionTypeType Type {
            get {
                return this.m_type;
            }
            set {
                this.m_type = value;
            }
        }
    }
    
    /// <remarks/>
    public enum PropertyDefinitionTypeType {
        
        /// <remarks/>
        boolean,
        
        /// <remarks/>
        @byte,
        
        /// <remarks/>
        datetime,
        
        /// <remarks/>
        @decimal,
        
        /// <remarks/>
        @double,
        
        /// <remarks/>
        int16,
        
        /// <remarks/>
        int32,
        
        /// <remarks/>
        int64,
        
        /// <remarks/>
        single,
        
        /// <remarks/>
        @string,
        
        /// <remarks/>
        blob,
        
        /// <remarks/>
        clob,
    }
    
    /// <remarks/>
    public class PropertyType {
        
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PropertySetTypePropertiesPropertyCollection))]
    public class PropertyCollectionType {
    }
    
    /// <remarks/>
    public class PropertySetTypePropertiesPropertyCollection : PropertyCollectionType {
        
        private PropertyTypeCollection m_property;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Property")]
        public PropertyTypeCollection Property {
            get {
                return this.m_property;
            }
            set {
                this.m_property = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PropertySetTypeProperties))]
    public class PropertiesType {
    }
    
    /// <remarks/>
    public class PropertySetTypeProperties : PropertiesType {
        
        private PropertySetTypePropertiesPropertyCollectionCollection m_propertyCollection;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PropertyCollection")]
        public PropertySetTypePropertiesPropertyCollectionCollection PropertyCollection {
            get {
                return this.m_propertyCollection;
            }
            set {
                this.m_propertyCollection = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PropertySetTypePropertyDefinitions))]
    public class PropertyDefinitionsType {
    }
    
    public class PropertyDefinitionTypeCollection : System.Collections.CollectionBase {
        
        public PropertyDefinitionType this[int idx] {
            get {
                return ((PropertyDefinitionType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(PropertyDefinitionType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class PropertyTypeCollection : System.Collections.CollectionBase {
        
        public PropertyType this[int idx] {
            get {
                return ((PropertyType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(PropertyType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class PropertySetTypePropertiesPropertyCollectionCollection : System.Collections.CollectionBase {
        
        public PropertySetTypePropertiesPropertyCollection this[int idx] {
            get {
                return ((PropertySetTypePropertiesPropertyCollection)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(PropertySetTypePropertiesPropertyCollection value) {
            return base.InnerList.Add(value);
        }
    }
}
