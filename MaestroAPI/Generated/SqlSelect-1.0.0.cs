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
    [System.Xml.Serialization.XmlRootAttribute("RowSet", Namespace="", IsNullable=false)]
    public class RowCollectionType {

		public static readonly string SchemaName = "SqlSelect-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
        
        private RowCollectionTypeColumnDefinitionsColumnCollection m_columnDefinitions;
        
        private RowCollectionTypeRowCollection m_rows;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Column", typeof(RowCollectionTypeColumnDefinitionsColumn), IsNullable=false)]
        public RowCollectionTypeColumnDefinitionsColumnCollection ColumnDefinitions {
            get {
                return this.m_columnDefinitions;
            }
            set {
                this.m_columnDefinitions = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Row", IsNullable=false)]
        public RowCollectionTypeRowCollection Rows {
            get {
                return this.m_rows;
            }
            set {
                this.m_rows = value;
            }
        }
    }
    
    /// <remarks/>
    public class RowCollectionTypeColumnDefinitionsColumn {
        
        private string m_name;
        
        private RowCollectionTypeColumnDefinitionsColumnType m_type;
        
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
        public RowCollectionTypeColumnDefinitionsColumnType Type {
            get {
                return this.m_type;
            }
            set {
                this.m_type = value;
            }
        }
    }
    
    /// <remarks/>
    public enum RowCollectionTypeColumnDefinitionsColumnType {
        
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
        
        /// <remarks/>
        uniqueId,
    }
    
    /// <remarks/>
    public class RowCollectionTypeRowColumnName {
        
        private string m_value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
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
    public class RowCollectionTypeRowColumn {
        
        private RowCollectionTypeRowColumnName m_name;
        
        private string m_value;
        
        /// <remarks/>
        public RowCollectionTypeRowColumnName Name {
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RowCollectionTypeRow))]
    public class RowType {
    }
    
    /// <remarks/>
    public class RowCollectionTypeRow : RowType {
        
        private RowCollectionTypeRowColumnCollection m_column;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Column")]
        public RowCollectionTypeRowColumnCollection Column {
            get {
                return this.m_column;
            }
            set {
                this.m_column = value;
            }
        }
    }
    
    public class RowCollectionTypeColumnDefinitionsColumnCollection : System.Collections.CollectionBase {
        
        public RowCollectionTypeColumnDefinitionsColumnCollection this[int idx] {
            get {
                return ((RowCollectionTypeColumnDefinitionsColumnCollection)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(RowCollectionTypeColumnDefinitionsColumnCollection value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class RowCollectionTypeRowCollection : System.Collections.CollectionBase {
        
        public RowCollectionTypeRow this[int idx] {
            get {
                return ((RowCollectionTypeRow)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(RowCollectionTypeRow value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class RowCollectionTypeRowColumnCollection : System.Collections.CollectionBase {
        
        public RowCollectionTypeRowColumn this[int idx] {
            get {
                return ((RowCollectionTypeRowColumn)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(RowCollectionTypeRowColumn value) {
            return base.InnerList.Add(value);
        }
    }
}
