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
    
    /// <summary>
    /// Represents a set of features
    /// </summary>
    [System.Xml.Serialization.XmlRootAttribute("FeatureSet", Namespace="", IsNullable=false)]
    public class FeatureSet {
        
		public static readonly string SchemaName = "FeatureSet-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
		
		private FeatureSetLayerCollection m_layer;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Layer")]
        public FeatureSetLayerCollection Layer {
            get {
                return this.m_layer;
            }
            set {
                this.m_layer = value;
            }
        }
    }
    
    /// <remarks/>
    public class FeatureSetLayer {
        
        private FeatureSetLayerClassCollection m_class;
        
        private string m_id;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Class")]
        public FeatureSetLayerClassCollection Class {
            get {
                return this.m_class;
            }
            set {
                this.m_class = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id {
            get {
                return this.m_id;
            }
            set {
                this.m_id = value;
            }
        }
    }
    
    /// <remarks/>
    public class FeatureSetLayerClass {
        
        private StringCollection m_iD;
        
        private string m_id;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ID")]
        public StringCollection ID {
            get {
                return this.m_iD;
            }
            set {
                this.m_iD = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id {
            get {
                return this.m_id;
            }
            set {
                this.m_id = value;
            }
        }
    }
    
    public class FeatureSetLayerCollection : System.Collections.CollectionBase {
        
        public FeatureSetLayer this[int idx] {
            get {
                return ((FeatureSetLayer)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FeatureSetLayer value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class FeatureSetLayerClassCollection : System.Collections.CollectionBase {
        
        public FeatureSetLayerClass this[int idx] {
            get {
                return ((FeatureSetLayerClass)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FeatureSetLayerClass value) {
            return base.InnerList.Add(value);
        }
    }
    
}
