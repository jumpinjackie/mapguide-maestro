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
	public class Envelope 
	{

		public static readonly string SchemaName = "Envelope-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

        
        private EnvelopeLowerLeftCoordinate m_lowerLeftCoordinate;
        
        private EnvelopeUpperRightCoordinate m_upperRightCoordinate;
        
        /// <remarks/>
        public EnvelopeLowerLeftCoordinate LowerLeftCoordinate {
            get {
                return this.m_lowerLeftCoordinate;
            }
            set {
                this.m_lowerLeftCoordinate = value;
            }
        }
        
        /// <remarks/>
		public EnvelopeUpperRightCoordinate UpperRightCoordinate 
		{
            get {
                return this.m_upperRightCoordinate;
            }
            set {
                this.m_upperRightCoordinate = value;
            }
        }
    }
    
    /// <remarks/>
    public class EnvelopeLowerLeftCoordinate {
        
        private double m_x;
        
        private double m_y;
        
        private double m_z;
        
        private double m_m;
        
        /// <remarks/>
        public double X {
            get {
                return this.m_x;
            }
            set {
                this.m_x = value;
            }
        }
        
        /// <remarks/>
		public double Y 
		{
            get {
                return this.m_y;
            }
            set {
                this.m_y = value;
            }
        }
        
        /// <remarks/>
		public double Z 
		{
            get {
                return this.m_z;
            }
            set {
                this.m_z = value;
            }
        }
        
        /// <remarks/>
		public double M 
		{
            get {
                return this.m_m;
            }
            set {
                this.m_m = value;
            }
        }
    }
    
    /// <remarks/>
    public class EnvelopeUpperRightCoordinate {
        
        private string m_x;
        
        private string m_y;
        
        private string m_z;
        
        private string m_m;
        
        /// <remarks/>
		public string X 
		{
            get {
                return this.m_x;
            }
            set {
                this.m_x = value;
            }
        }
        
        /// <remarks/>
		public string Y 
		{
            get {
                return this.m_y;
            }
            set {
                this.m_y = value;
            }
        }
        
        /// <remarks/>
		public string Z 
		{
            get {
                return this.m_z;
            }
            set {
                this.m_z = value;
            }
        }
        
        /// <remarks/>
		public string M 
		{
            get {
                return this.m_m;
            }
            set {
                this.m_m = value;
            }
        }
    }
}
