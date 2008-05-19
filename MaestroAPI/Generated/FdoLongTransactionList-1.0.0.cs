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
    public class FdoLongTransactionList {
        
		public static readonly string SchemaName = "FdoLongTransactionList-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

        private string m_providerName;
        
        private FdoLongTransactionListLongTransactionCollection m_longTransaction;
        
        /// <remarks/>
        public string ProviderName {
            get {
                return this.m_providerName;
            }
            set {
                this.m_providerName = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("LongTransaction")]
        public FdoLongTransactionListLongTransactionCollection LongTransaction {
            get {
                return this.m_longTransaction;
            }
            set {
                this.m_longTransaction = value;
            }
        }
    }
    
    /// <remarks/>
    public class FdoLongTransactionListLongTransaction {
        
        private string m_name;
        
        private string m_description;
        
        private string m_owner;
        
        private string m_creationDate;
        
        private bool m_isActive;
        
        private bool m_isFrozen;
        
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
        
        /// <remarks/>
        public string Owner {
            get {
                return this.m_owner;
            }
            set {
                this.m_owner = value;
            }
        }
        
        /// <remarks/>
        public string CreationDate {
            get {
                return this.m_creationDate;
            }
            set {
                this.m_creationDate = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsActive {
            get {
                return this.m_isActive;
            }
            set {
                this.m_isActive = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsFrozen {
            get {
                return this.m_isFrozen;
            }
            set {
                this.m_isFrozen = value;
            }
        }
    }
    
    public class FdoLongTransactionListLongTransactionCollection : System.Collections.CollectionBase {
        
        public FdoLongTransactionListLongTransaction this[int idx] {
            get {
                return ((FdoLongTransactionListLongTransaction)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoLongTransactionListLongTransaction value) {
            return base.InnerList.Add(value);
        }
    }
}
