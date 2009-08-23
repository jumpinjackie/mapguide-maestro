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
    public class DrawingSource {
        
		public static readonly string SchemaName = "DrawingSource-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

        private string m_sourceName;
        
        private string m_coordinateSpace;
        
        private DrawingSourceSheetCollection m_sheet;
        
        /// <remarks/>
        public string SourceName {
            get {
                return this.m_sourceName;
            }
            set {
                this.m_sourceName = value;
            }
        }
        
        /// <remarks/>
        public string CoordinateSpace {
            get {
                return this.m_coordinateSpace;
            }
            set {
                this.m_coordinateSpace = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Sheet")]
        public DrawingSourceSheetCollection Sheet {
            get {
                return this.m_sheet;
            }
            set {
                this.m_sheet = value;
            }
        }
    }
    
    /// <remarks/>
    public class DrawingSourceSheet {
        
        private string m_name;
        
        private DrawingSourceSheetExtent m_extent;
        
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
        public DrawingSourceSheetExtent Extent {
            get {
                return this.m_extent;
            }
            set {
                this.m_extent = value;
            }
        }
    }
    
    /// <remarks/>
    public class DrawingSourceSheetExtent {
        
        private System.Double m_minX;
        
        private System.Double m_minY;
        
        private System.Double m_maxX;
        
        private System.Double m_maxY;
        
        /// <remarks/>
        public System.Double MinX {
            get {
                return this.m_minX;
            }
            set {
                this.m_minX = value;
            }
        }
        
        /// <remarks/>
        public System.Double MinY {
            get {
                return this.m_minY;
            }
            set {
                this.m_minY = value;
            }
        }
        
        /// <remarks/>
        public System.Double MaxX {
            get {
                return this.m_maxX;
            }
            set {
                this.m_maxX = value;
            }
        }
        
        /// <remarks/>
        public System.Double MaxY {
            get {
                return this.m_maxY;
            }
            set {
                this.m_maxY = value;
            }
        }
    }
    
    public class DrawingSourceSheetCollection : System.Collections.CollectionBase {
        
        public DrawingSourceSheet this[int idx] {
            get {
                return ((DrawingSourceSheet)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(DrawingSourceSheet value) {
            return base.InnerList.Add(value);
        }
    }
}
