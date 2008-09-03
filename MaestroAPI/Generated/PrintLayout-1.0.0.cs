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
    public class PrintLayout {

		public static readonly string SchemaName = "PrintLayout-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}


		private string m_resourceId;
		[System.Xml.Serialization.XmlIgnore()]
		public string ResourceId 
		{ 
			get { return m_resourceId; } 
			set { m_resourceId = value; }
		}

        
        private PrintLayoutPageProperties m_pageProperties;
        
        private PrintLayoutLayoutProperties m_layoutProperties;
        
        private PrintLayoutLogoCollection m_customLogos;
        
        private PrintLayoutTextCollection m_customText;
        
        /// <remarks/>
        public PrintLayoutPageProperties PageProperties {
            get {
                return this.m_pageProperties;
            }
            set {
                this.m_pageProperties = value;
            }
        }
        
        /// <remarks/>
        public PrintLayoutLayoutProperties LayoutProperties {
            get {
                return this.m_layoutProperties;
            }
            set {
                this.m_layoutProperties = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Logo", IsNullable=false)]
        public PrintLayoutLogoCollection CustomLogos {
            get {
                return this.m_customLogos;
            }
            set {
                this.m_customLogos = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Text", IsNullable=false)]
        public PrintLayoutTextCollection CustomText {
            get {
                return this.m_customText;
            }
            set {
                this.m_customText = value;
            }
        }
    }
    
    /// <remarks/>
    public class PrintLayoutPageProperties {
        
        private PrintLayoutPagePropertiesBackgroundColor m_backgroundColor;
        
        /// <remarks/>
        public PrintLayoutPagePropertiesBackgroundColor BackgroundColor {
            get {
                return this.m_backgroundColor;
            }
            set {
                this.m_backgroundColor = value;
            }
        }
    }
    
    /// <remarks/>
    public class PrintLayoutPagePropertiesBackgroundColor {
        
        private string m_red;
        
        private string m_blue;
        
        private string m_green;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string Red {
            get {
                return this.m_red;
            }
            set {
                this.m_red = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string Blue {
            get {
                return this.m_blue;
            }
            set {
                this.m_blue = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string Green {
            get {
                return this.m_green;
            }
            set {
                this.m_green = value;
            }
        }
    }
    
    /// <remarks/>
    public class PrintLayoutTextFont {
        
        private string m_name;
        
        private System.Single m_height;
        
        private string m_units;
        
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
        public System.Single Height {
            get {
                return this.m_height;
            }
            set {
                this.m_height = value;
            }
        }
        
        /// <remarks/>
        public string Units {
            get {
                return this.m_units;
            }
            set {
                this.m_units = value;
            }
        }
    }
    
    /// <remarks/>
    public class PrintLayoutTextPosition {
        
        private System.Single m_left;
        
        private System.Single m_bottom;
        
        private string m_units;
        
        /// <remarks/>
        public System.Single Left {
            get {
                return this.m_left;
            }
            set {
                this.m_left = value;
            }
        }
        
        /// <remarks/>
        public System.Single Bottom {
            get {
                return this.m_bottom;
            }
            set {
                this.m_bottom = value;
            }
        }
        
        /// <remarks/>
        public string Units {
            get {
                return this.m_units;
            }
            set {
                this.m_units = value;
            }
        }
    }
    
    /// <remarks/>
    public class PrintLayoutText {
        
        private PrintLayoutTextPosition m_position;
        
        private PrintLayoutTextFont m_font;
        
        private string m_value;
        
        /// <remarks/>
        public PrintLayoutTextPosition Position {
            get {
                return this.m_position;
            }
            set {
                this.m_position = value;
            }
        }
        
        /// <remarks/>
        public PrintLayoutTextFont Font {
            get {
                return this.m_font;
            }
            set {
                this.m_font = value;
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
    public class PrintLayoutLogoSize {
        
        private System.Single m_width;
        
        private System.Single m_height;
        
        private string m_units;
        
        /// <remarks/>
        public System.Single Width {
            get {
                return this.m_width;
            }
            set {
                this.m_width = value;
            }
        }
        
        /// <remarks/>
        public System.Single Height {
            get {
                return this.m_height;
            }
            set {
                this.m_height = value;
            }
        }
        
        /// <remarks/>
        public string Units {
            get {
                return this.m_units;
            }
            set {
                this.m_units = value;
            }
        }
    }
    
    /// <remarks/>
    public class PrintLayoutLogoPosition {
        
        private System.Single m_left;
        
        private System.Single m_bottom;
        
        private string m_units;
        
        /// <remarks/>
        public System.Single Left {
            get {
                return this.m_left;
            }
            set {
                this.m_left = value;
            }
        }
        
        /// <remarks/>
        public System.Single Bottom {
            get {
                return this.m_bottom;
            }
            set {
                this.m_bottom = value;
            }
        }
        
        /// <remarks/>
        public string Units {
            get {
                return this.m_units;
            }
            set {
                this.m_units = value;
            }
        }
    }
    
    /// <remarks/>
    public class PrintLayoutLogo {
        
        private PrintLayoutLogoPosition m_position;
        
        private string m_resourceId;
        
        private string m_name;
        
        private PrintLayoutLogoSize m_size;
        
        private System.Single m_rotation;
        
        private bool m_rotationSpecified;
        
        /// <remarks/>
        public PrintLayoutLogoPosition Position {
            get {
                return this.m_position;
            }
            set {
                this.m_position = value;
            }
        }
        
        /// <remarks/>
        public string ResourceId {
            get {
                return this.m_resourceId;
            }
            set {
                this.m_resourceId = value;
            }
        }
        
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
        public PrintLayoutLogoSize Size {
            get {
                return this.m_size;
            }
            set {
                this.m_size = value;
            }
        }
        
        /// <remarks/>
        public System.Single Rotation {
            get {
                return this.m_rotation;
            }
            set {
                this.m_rotation = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RotationSpecified {
            get {
                return this.m_rotationSpecified;
            }
            set {
                this.m_rotationSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    public class PrintLayoutLayoutProperties {
        
        private bool m_showTitle;
        
        private bool m_showTitleSpecified;
        
        private bool m_showLegend;
        
        private bool m_showLegendSpecified;
        
        private bool m_showScaleBar;
        
        private bool m_showScaleBarSpecified;
        
        private bool m_showNorthArrow;
        
        private bool m_showNorthArrowSpecified;
        
        private bool m_showURL;
        
        private bool m_showURLSpecified;
        
        private bool m_showDateTime;
        
        private bool m_showDateTimeSpecified;
        
        private bool m_showCustomLogos;
        
        private bool m_showCustomLogosSpecified;
        
        private bool m_showCustomText;
        
        private bool m_showCustomTextSpecified;
        
        /// <remarks/>
        public bool ShowTitle {
            get {
                return this.m_showTitle;
            }
            set {
                this.m_showTitle = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowTitleSpecified {
            get {
                return this.m_showTitleSpecified;
            }
            set {
                this.m_showTitleSpecified = value;
            }
        }
        
        /// <remarks/>
        public bool ShowLegend {
            get {
                return this.m_showLegend;
            }
            set {
                this.m_showLegend = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowLegendSpecified {
            get {
                return this.m_showLegendSpecified;
            }
            set {
                this.m_showLegendSpecified = value;
            }
        }
        
        /// <remarks/>
        public bool ShowScaleBar {
            get {
                return this.m_showScaleBar;
            }
            set {
                this.m_showScaleBar = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowScaleBarSpecified {
            get {
                return this.m_showScaleBarSpecified;
            }
            set {
                this.m_showScaleBarSpecified = value;
            }
        }
        
        /// <remarks/>
        public bool ShowNorthArrow {
            get {
                return this.m_showNorthArrow;
            }
            set {
                this.m_showNorthArrow = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowNorthArrowSpecified {
            get {
                return this.m_showNorthArrowSpecified;
            }
            set {
                this.m_showNorthArrowSpecified = value;
            }
        }
        
        /// <remarks/>
        public bool ShowURL {
            get {
                return this.m_showURL;
            }
            set {
                this.m_showURL = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowURLSpecified {
            get {
                return this.m_showURLSpecified;
            }
            set {
                this.m_showURLSpecified = value;
            }
        }
        
        /// <remarks/>
        public bool ShowDateTime {
            get {
                return this.m_showDateTime;
            }
            set {
                this.m_showDateTime = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowDateTimeSpecified {
            get {
                return this.m_showDateTimeSpecified;
            }
            set {
                this.m_showDateTimeSpecified = value;
            }
        }
        
        /// <remarks/>
        public bool ShowCustomLogos {
            get {
                return this.m_showCustomLogos;
            }
            set {
                this.m_showCustomLogos = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowCustomLogosSpecified {
            get {
                return this.m_showCustomLogosSpecified;
            }
            set {
                this.m_showCustomLogosSpecified = value;
            }
        }
        
        /// <remarks/>
        public bool ShowCustomText {
            get {
                return this.m_showCustomText;
            }
            set {
                this.m_showCustomText = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ShowCustomTextSpecified {
            get {
                return this.m_showCustomTextSpecified;
            }
            set {
                this.m_showCustomTextSpecified = value;
            }
        }
    }
    
    public class PrintLayoutLogoCollection : System.Collections.CollectionBase {
        
        public PrintLayoutLogo this[int idx] {
            get {
                return ((PrintLayoutLogo)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(PrintLayoutLogo value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class PrintLayoutTextCollection : System.Collections.CollectionBase {
        
        public PrintLayoutText this[int idx] {
            get {
                return ((PrintLayoutText)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(PrintLayoutText value) {
            return base.InnerList.Add(value);
        }
    }
}
