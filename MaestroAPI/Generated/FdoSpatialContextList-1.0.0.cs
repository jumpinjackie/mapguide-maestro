#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
    public class FdoSpatialContextList {
        
		public static readonly string SchemaName = "FdoSpatialContextList-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}
		
		private string m_providerName;
        
        private FdoSpatialContextListSpatialContextCollection m_spatialContext;
        
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
        [System.Xml.Serialization.XmlElementAttribute("SpatialContext")]
        public FdoSpatialContextListSpatialContextCollection SpatialContext {
            get {
                return this.m_spatialContext;
            }
            set {
                this.m_spatialContext = value;
            }
        }
    }
    
    /// <remarks/>
    public class FdoSpatialContextListSpatialContext {
        
        private string m_name;
        
        private string m_description;
        
        private string m_coordinateSystemName;
        
        private string m_coordinateSystemWkt;
        
        private FdoSpatialContextListSpatialContextExtentType m_extentType;
        
        private FdoSpatialContextListSpatialContextExtent m_extent;
        
        private System.Double m_xYTolerance;
        
        private System.Double m_zTolerance;
        
        private bool m_isActive = false;
        
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
        public string CoordinateSystemName {
            get {
                return this.m_coordinateSystemName;
            }
            set {
                this.m_coordinateSystemName = value;
            }
        }
        
        /// <remarks/>
        public string CoordinateSystemWkt {
            get {
                return this.m_coordinateSystemWkt;
            }
            set {
                this.m_coordinateSystemWkt = value;
            }
        }
        
        /// <remarks/>
        public FdoSpatialContextListSpatialContextExtentType ExtentType {
            get {
                return this.m_extentType;
            }
            set {
                this.m_extentType = value;
            }
        }
        
        /// <remarks/>
        public FdoSpatialContextListSpatialContextExtent Extent {
            get {
                return this.m_extent;
            }
            set {
                this.m_extent = value;
            }
        }
        
        /// <remarks/>
        public System.Double XYTolerance {
            get {
                return this.m_xYTolerance;
            }
            set {
                this.m_xYTolerance = value;
            }
        }
        
        /// <remarks/>
        public System.Double ZTolerance {
            get {
                return this.m_zTolerance;
            }
            set {
                this.m_zTolerance = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsActive {
            get {
                return this.m_isActive;
            }
            set {
                this.m_isActive = value;
            }
        }
    }
    
    /// <remarks/>
    public enum FdoSpatialContextListSpatialContextExtentType {
        
        /// <remarks/>
        Static,
        
        /// <remarks/>
        Dynamic,
    }
    
    /// <remarks/>
    public class FdoSpatialContextListSpatialContextExtent {
        
        private FdoSpatialContextListSpatialContextExtentLowerLeftCoordinate m_lowerLeftCoordinate;
        
        private FdoSpatialContextListSpatialContextExtentUpperRightCoordinate m_upperRightCoordinate;
        
        /// <remarks/>
        public FdoSpatialContextListSpatialContextExtentLowerLeftCoordinate LowerLeftCoordinate {
            get {
                return this.m_lowerLeftCoordinate;
            }
            set {
                this.m_lowerLeftCoordinate = value;
            }
        }
        
        /// <remarks/>
        public FdoSpatialContextListSpatialContextExtentUpperRightCoordinate UpperRightCoordinate {
            get {
                return this.m_upperRightCoordinate;
            }
            set {
                this.m_upperRightCoordinate = value;
            }
        }
    }
    
    /// <remarks/>
    public class FdoSpatialContextListSpatialContextExtentLowerLeftCoordinate {
        
        private string m_x;
        
        private string m_y;
        
        private string m_z;
        
        private string m_m;
        
        /// <remarks/>
        public string X {
            get {
                return this.m_x;
            }
            set {
                this.m_x = value;
            }
        }
        
        /// <remarks/>
        public string Y {
            get {
                return this.m_y;
            }
            set {
                this.m_y = value;
            }
        }
        
        /// <remarks/>
        public string Z {
            get {
                return this.m_z;
            }
            set {
                this.m_z = value;
            }
        }
        
        /// <remarks/>
        public string M {
            get {
                return this.m_m;
            }
            set {
                this.m_m = value;
            }
        }
    }
    
    /// <remarks/>
    public class FdoSpatialContextListSpatialContextExtentUpperRightCoordinate {
        
        private string m_x;
        
        private string m_y;
        
        private string m_z;
        
        private string m_m;
        
        /// <remarks/>
        public string X {
            get {
                return this.m_x;
            }
            set {
                this.m_x = value;
            }
        }
        
        /// <remarks/>
        public string Y {
            get {
                return this.m_y;
            }
            set {
                this.m_y = value;
            }
        }
        
        /// <remarks/>
        public string Z {
            get {
                return this.m_z;
            }
            set {
                this.m_z = value;
            }
        }
        
        /// <remarks/>
        public string M {
            get {
                return this.m_m;
            }
            set {
                this.m_m = value;
            }
        }
    }
    
    public class FdoSpatialContextListSpatialContextCollection : System.Collections.CollectionBase {
        
        public FdoSpatialContextListSpatialContext this[int idx] {
            get {
                return ((FdoSpatialContextListSpatialContext)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(FdoSpatialContextListSpatialContext value) {
            return base.InnerList.Add(value);
        }
    }
}
