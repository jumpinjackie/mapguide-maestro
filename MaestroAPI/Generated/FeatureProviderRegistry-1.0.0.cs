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
using System.Collections.Specialized;

namespace OSGeo.MapGuide.MaestroAPI 
{
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class FeatureProviderRegistry {
        
		public static readonly string SchemaName = "FeatureProviderRegistry-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
		
		private FeatureProviderRegistryFeatureProviderCollection m_featureProvider;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FeatureProvider")]
        public FeatureProviderRegistryFeatureProviderCollection FeatureProvider {
            get {
                return this.m_featureProvider;
            }
            set {
                this.m_featureProvider = value;
            }
        }
    }
    
    /// <remarks/>
    public class FeatureProviderRegistryFeatureProvider {
        
        private string m_name;
        
        private string m_displayName;
        
        private string m_description;
        
        private string m_version;
        
        private string m_featureDataObjectsVersion;
        
        private FeatureProviderRegistryFeatureProviderConnectionPropertyCollection m_connectionProperties;
        
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
        public string DisplayName {
            get {
                return this.m_displayName;
            }
            set {
                this.m_displayName = value;
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
        
        /// <remarks/>
        public string Version {
            get {
                return this.m_version;
            }
            set {
                this.m_version = value;
            }
        }
        
        /// <remarks/>
        public string FeatureDataObjectsVersion {
            get {
                return this.m_featureDataObjectsVersion;
            }
            set {
                this.m_featureDataObjectsVersion = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ConnectionProperty", IsNullable=false)]
        public FeatureProviderRegistryFeatureProviderConnectionPropertyCollection ConnectionProperties {
            get {
                return this.m_connectionProperties;
            }
            set {
                this.m_connectionProperties = value;
            }
        }
    }
    
    /// <remarks/>
    public class FeatureProviderRegistryFeatureProviderConnectionProperty {
        
        private string m_name;
        
        private string m_localizedName;
        
        private string m_defaultValue;

		private StringCollection m_value;
        
        private bool m_required;
        
        private bool m_protected;
        
        private bool m_enumerable;
        
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
        public string LocalizedName {
            get {
                return this.m_localizedName;
            }
            set {
                this.m_localizedName = value;
            }
        }
        
        /// <remarks/>
        public string DefaultValue {
            get {
                return this.m_defaultValue;
            }
            set {
                this.m_defaultValue = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Value")]
        public StringCollection Value {
            get {
                return this.m_value;
            }
            set {
                this.m_value = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Required {
            get {
                return this.m_required;
            }
            set {
                this.m_required = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Protected {
            get {
                return this.m_protected;
            }
            set {
                this.m_protected = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Enumerable {
            get {
                return this.m_enumerable;
            }
            set {
                this.m_enumerable = value;
            }
        }
    }
    
    public class FeatureProviderRegistryFeatureProviderCollection : System.Collections.CollectionBase {
        
        public FeatureProviderRegistryFeatureProvider this[int idx] {
            get {
                return ((FeatureProviderRegistryFeatureProvider)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FeatureProviderRegistryFeatureProvider value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FeatureProviderRegistryFeatureProviderConnectionPropertyCollection : System.Collections.CollectionBase {
        
        public FeatureProviderRegistryFeatureProviderConnectionProperty this[int idx] {
            get {
                return ((FeatureProviderRegistryFeatureProviderConnectionProperty)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FeatureProviderRegistryFeatureProviderConnectionProperty value) {
            return base.InnerList.Add(value);
        }
    }
    
}
