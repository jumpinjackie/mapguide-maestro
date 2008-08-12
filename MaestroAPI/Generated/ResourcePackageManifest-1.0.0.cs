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
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class ResourcePackageManifest {
        
		public static readonly string SchemaName = "ResourcePackageManifest-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
		
		private string m_description;
        
        private ResourcePackageManifestOperations m_operations;
        
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
        public ResourcePackageManifestOperations Operations {
            get {
                return this.m_operations;
            }
            set {
                this.m_operations = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourcePackageManifestOperations {
        
        private ResourcePackageManifestOperationsOperationCollection m_operation;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Operation")]
        public ResourcePackageManifestOperationsOperationCollection Operation {
            get {
                return this.m_operation;
            }
            set {
                this.m_operation = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourcePackageManifestOperationsOperation {
        
        private string m_name;
        
        private string m_version;
        
        private ResourcePackageManifestOperationsOperationParameters m_parameters;
        
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
        public string Version {
            get {
                return this.m_version;
            }
            set {
                this.m_version = value;
            }
        }
        
        /// <remarks/>
        public ResourcePackageManifestOperationsOperationParameters Parameters {
            get {
                return this.m_parameters;
            }
            set {
                this.m_parameters = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourcePackageManifestOperationsOperationParameters {
        
        private ResourcePackageManifestOperationsOperationParametersParameterCollection m_parameter;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Parameter")]
        public ResourcePackageManifestOperationsOperationParametersParameterCollection Parameter {
            get {
                return this.m_parameter;
            }
            set {
                this.m_parameter = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResourcePackageManifestOperationsOperationParametersParameter {
        
        private string m_name;
        
        private string m_value;
        
        private string m_contentType;
        
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
        
        /// <remarks/>
        public string ContentType {
            get {
                return this.m_contentType;
            }
            set {
                this.m_contentType = value;
            }
        }
    }
    
    public class ResourcePackageManifestOperationsOperationCollection : System.Collections.CollectionBase {
        
        public ResourcePackageManifestOperationsOperation this[int idx] {
            get {
                return ((ResourcePackageManifestOperationsOperation)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(ResourcePackageManifestOperationsOperation value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class ResourcePackageManifestOperationsOperationParametersParameterCollection : System.Collections.CollectionBase {
        
        public ResourcePackageManifestOperationsOperationParametersParameter this[int idx] {
            get {
                return ((ResourcePackageManifestOperationsOperationParametersParameter)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }

        public ResourcePackageManifestOperationsOperationParametersParameter this[string idx]
        {
            get
            {
                ResourcePackageManifestOperationsOperationParametersParameter secondBest = null;
                foreach (ResourcePackageManifestOperationsOperationParametersParameter op in base.InnerList)
                    if (op.Name == idx)
                        return op;
                    else if (secondBest == null && op.Name.ToLower().Equals(idx.ToLower()))
                        secondBest = op;

                return secondBest;
            }
            set
            {
                ResourcePackageManifestOperationsOperationParametersParameter op = this[idx];
                if (op == null)
                    this.Add(value);
                else
                    this[base.InnerList.IndexOf(op)] = value;
            }
        }
        
        public int Add(ResourcePackageManifestOperationsOperationParametersParameter value) {
            return base.InnerList.Add(value);
        }
    }
}
