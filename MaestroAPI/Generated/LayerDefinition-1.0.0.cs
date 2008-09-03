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
using System.Collections;

namespace OSGeo.MapGuide.MaestroAPI {
    /*
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class LayerDefinition : LayerDefinitionType {
        
		public static readonly string SchemaName = "LayerDefinition-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

		private string m_resourceId;
		[System.Xml.Serialization.XmlIgnore()]
		public string ResourceId { get { return m_resourceId; } }
		internal void SetResourceId(string id) { m_resourceId = id; }
		
		private string m_version;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version {
            get {
                return this.m_version;
            }
            set {
                this.m_version = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LayerDefinition))]
    public class LayerDefinitionType {
        
        private BaseLayerDefinitionType m_item;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DrawingLayerDefinition", typeof(DrawingLayerDefinitionType))]
        [System.Xml.Serialization.XmlElementAttribute("VectorLayerDefinition", typeof(VectorLayerDefinitionType))]
        [System.Xml.Serialization.XmlElementAttribute("GridLayerDefinition", typeof(GridLayerDefinitionType))]
        public BaseLayerDefinitionType Item {
            get {
                return this.m_item;
            }
            set {
                this.m_item = value;
            }
        }
    }
    
    /// <remarks/>
    public class DrawingLayerDefinitionType : BaseLayerDefinitionType {
        
        private string m_sheet;
        
        private string m_layerFilter;
        
        private System.Double m_minScale;
        
        private bool m_minScaleSpecified;
        
        private System.Double m_maxScale;
        
        private bool m_maxScaleSpecified;
        
        /// <remarks/>
        public string Sheet {
            get {
                return this.m_sheet;
            }
            set {
                this.m_sheet = value;
            }
        }
        
        /// <remarks/>
        public string LayerFilter {
            get {
                return this.m_layerFilter;
            }
            set {
                this.m_layerFilter = value;
            }
        }
        
        /// <remarks/>
        public System.Double MinScale {
            get {
                return this.m_minScale;
            }
            set {
                this.m_minScale = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinScaleSpecified {
            get {
                return this.m_minScaleSpecified;
            }
            set {
                this.m_minScaleSpecified = value;
            }
        }
        
        /// <remarks/>
        public System.Double MaxScale {
            get {
                return this.m_maxScale;
            }
            set {
                this.m_maxScale = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaxScaleSpecified {
            get {
                return this.m_maxScaleSpecified;
            }
            set {
                this.m_maxScaleSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GridLayerDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DrawingLayerDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VectorLayerDefinitionType))]
    public class BaseLayerDefinitionType {
        
        private string m_resourceId;
        
        private System.Double m_opacity;
        
        private bool m_opacitySpecified;
        
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
        public System.Double Opacity {
            get {
                return this.m_opacity;
            }
            set {
                this.m_opacity = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OpacitySpecified {
            get {
                return this.m_opacitySpecified;
            }
            set {
                this.m_opacitySpecified = value;
            }
        }
    }
    
    /// <remarks/>
    public class GridScaleRangeType {
        
        private System.Double m_minScale;
        
        private bool m_minScaleSpecified;
        
        private System.Double m_maxScale;
        
        private bool m_maxScaleSpecified;
        
        private GridSurfaceStylizationType m_surfaceStyle;
        
        private GridColorStylizationType m_colorStyle;
        
        private System.Double m_rebuildFactor;
        
        /// <remarks/>
        public System.Double MinScale {
            get {
                return this.m_minScale;
            }
            set {
                this.m_minScale = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinScaleSpecified {
            get {
                return this.m_minScaleSpecified;
            }
            set {
                this.m_minScaleSpecified = value;
            }
        }
        
        /// <remarks/>
        public System.Double MaxScale {
            get {
                return this.m_maxScale;
            }
            set {
                this.m_maxScale = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaxScaleSpecified {
            get {
                return this.m_maxScaleSpecified;
            }
            set {
                this.m_maxScaleSpecified = value;
            }
        }
        
        /// <remarks/>
        public GridSurfaceStylizationType SurfaceStyle {
            get {
                return this.m_surfaceStyle;
            }
            set {
                this.m_surfaceStyle = value;
            }
        }
        
        /// <remarks/>
        public GridColorStylizationType ColorStyle {
            get {
                return this.m_colorStyle;
            }
            set {
                this.m_colorStyle = value;
            }
        }
        
        /// <remarks/>
        public System.Double RebuildFactor {
            get {
                return this.m_rebuildFactor;
            }
            set {
                this.m_rebuildFactor = value;
            }
        }
    }
    
    /// <remarks/>
    public class GridSurfaceStylizationType {
        
        private string m_band;
        
        private System.Double m_zeroValue;
        
        private bool m_zeroValueSpecified;
        
        private System.Double m_scale;
        
        private bool m_scaleSpecified;
        
        private string m_defaultColor;
        
        /// <remarks/>
        public string Band {
            get {
                return this.m_band;
            }
            set {
                this.m_band = value;
            }
        }
        
        /// <remarks/>
        public System.Double ZeroValue {
            get {
                return this.m_zeroValue;
            }
            set {
                this.m_zeroValue = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ZeroValueSpecified {
            get {
                return this.m_zeroValueSpecified;
            }
            set {
                this.m_zeroValueSpecified = value;
            }
        }
        
        /// <remarks/>
        public System.Double Scale {
            get {
                return this.m_scale;
            }
            set {
                this.m_scale = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ScaleSpecified {
            get {
                return this.m_scaleSpecified;
            }
            set {
                this.m_scaleSpecified = value;
            }
        }
        
        /// <remarks/>
        public string DefaultColor {
            get {
                return this.m_defaultColor;
            }
            set {
                this.m_defaultColor = value;
            }
        }
    }
    
    /// <remarks/>
    public class GridColorStylizationType {
        
        private HillshadeType m_hillshade;
        
        private object m_transparencyColor;
        
        private System.Double m_brightnessFactor;
        
        private bool m_brightnessFactorSpecified;
        
        private System.Double m_contrastFactor;
        
        private bool m_contrastFactorSpecified;
        
        private GridColorRuleTypeCollection m_colorRule;
        
        /// <remarks/>
        public HillshadeType Hillshade {
            get {
                return this.m_hillshade;
            }
            set {
                this.m_hillshade = value;
            }
        }
        
        /// <remarks/>
        public object TransparencyColor {
            get {
                return this.m_transparencyColor;
            }
            set {
                this.m_transparencyColor = value;
            }
        }
        
        /// <remarks/>
        public System.Double BrightnessFactor {
            get {
                return this.m_brightnessFactor;
            }
            set {
                this.m_brightnessFactor = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BrightnessFactorSpecified {
            get {
                return this.m_brightnessFactorSpecified;
            }
            set {
                this.m_brightnessFactorSpecified = value;
            }
        }
        
        /// <remarks/>
        public System.Double ContrastFactor {
            get {
                return this.m_contrastFactor;
            }
            set {
                this.m_contrastFactor = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ContrastFactorSpecified {
            get {
                return this.m_contrastFactorSpecified;
            }
            set {
                this.m_contrastFactorSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ColorRule")]
        public GridColorRuleTypeCollection ColorRule {
            get {
                return this.m_colorRule;
            }
            set {
                this.m_colorRule = value;
            }
        }
    }
    
    /// <remarks/>
    public class HillshadeType {
        
        private string m_band;
        
        private System.Double m_azimuth;
        
        private System.Double m_altitude;
        
        private System.Double m_scaleFactor;
        
        private bool m_scaleFactorSpecified;
        
        /// <remarks/>
        public string Band {
            get {
                return this.m_band;
            }
            set {
                this.m_band = value;
            }
        }
        
        /// <remarks/>
        public System.Double Azimuth {
            get {
                return this.m_azimuth;
            }
            set {
                this.m_azimuth = value;
            }
        }
        
        /// <remarks/>
        public System.Double Altitude {
            get {
                return this.m_altitude;
            }
            set {
                this.m_altitude = value;
            }
        }
        
        /// <remarks/>
        public System.Double ScaleFactor {
            get {
                return this.m_scaleFactor;
            }
            set {
                this.m_scaleFactor = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ScaleFactorSpecified {
            get {
                return this.m_scaleFactorSpecified;
            }
            set {
                this.m_scaleFactorSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    public class GridColorRuleType {
        
        private string m_legendLabel;
        
        private string m_filter;
        
        private TextSymbolType m_label;
        
        private GridColorType m_color;
        
        /// <remarks/>
        public string LegendLabel {
            get {
                return this.m_legendLabel;
            }
            set {
                this.m_legendLabel = value;
            }
        }
        
        /// <remarks/>
        public string Filter {
            get {
                return this.m_filter;
            }
            set {
                this.m_filter = value;
            }
        }
        
        /// <remarks/>
        public TextSymbolType Label {
            get {
                return this.m_label;
            }
            set {
                this.m_label = value;
            }
        }
        
        /// <remarks/>
        public GridColorType Color {
            get {
                return this.m_color;
            }
            set {
                this.m_color = value;
            }
        }
    }
    
    /// <remarks/>
    public class TextSymbolType : SymbolType {
        
        private string m_text;
        
        private string m_fontName;
        
        private string m_foregroundColor;
        
        private string m_backgroundColor;
        
        private BackgroundStyleType m_backgroundStyle;
        
        private string m_horizontalAlignment;
        
        private string m_verticalAlignment;
        
        private string m_bold;
        
        private string m_italic;
        
        private string m_underlined;
        
        private TextSymbolTypeAdvancedPlacement m_advancedPlacement;
        
        /// <remarks/>
        public string Text {
            get {
                return this.m_text;
            }
            set {
                this.m_text = value;
            }
        }
        
        /// <remarks/>
        public string FontName {
            get {
                return this.m_fontName;
            }
            set {
                this.m_fontName = value;
            }
        }
        
		[System.Xml.Serialization.XmlElementAttribute("ForegroundColor")]
		public string ForegroundColorAsHTML
		{
			get { return this.m_foregroundColor; }
			set { this.m_foregroundColor = value; }
		}


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Drawing.Color ForegroundColor {
            get {
                return Utility.ParseHTMLColor(this.m_foregroundColor);
            }
            set {
                this.m_foregroundColor = Utility.SerializeHTMLColor(value, false);
            }
        }

		[System.Xml.Serialization.XmlElementAttribute("BackgroundColor")]
		public string BackgroundColorAsHTML
		{
			get { return this.m_backgroundColor; }
			set { this.m_backgroundColor = value; }
		}

        
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public System.Drawing.Color BackgroundColor 
		{
            get {
                return Utility.ParseHTMLColor(this.m_backgroundColor);
            }
            set {
                this.m_backgroundColor = Utility.SerializeHTMLColor(value, false);
            }
        }
        
        /// <remarks/>
        public BackgroundStyleType BackgroundStyle {
            get {
                return this.m_backgroundStyle;
            }
            set {
                this.m_backgroundStyle = value;
            }
        }
        
        /// <remarks/>
        public string HorizontalAlignment {
            get {
                return this.m_horizontalAlignment;
            }
            set {
                this.m_horizontalAlignment = value;
            }
        }
        
        /// <remarks/>
        public string VerticalAlignment {
            get {
                return this.m_verticalAlignment;
            }
            set {
                this.m_verticalAlignment = value;
            }
        }
        
        /// <remarks/>
        public string Bold {
            get {
                return this.m_bold;
            }
            set {
                this.m_bold = value;
            }
        }
        
        /// <remarks/>
        public string Italic {
            get {
                return this.m_italic;
            }
            set {
                this.m_italic = value;
            }
        }
        
        /// <remarks/>
        public string Underlined {
            get {
                return this.m_underlined;
            }
            set {
                this.m_underlined = value;
            }
        }
        
        /// <remarks/>
        public TextSymbolTypeAdvancedPlacement AdvancedPlacement {
            get {
                return this.m_advancedPlacement;
            }
            set {
                this.m_advancedPlacement = value;
            }
        }
    }
    
    /// <remarks/>
    public enum BackgroundStyleType {
        
        /// <remarks/>
        Transparent,
        
        /// <remarks/>
        Opaque,
        
        /// <remarks/>
        Ghosted,
    }
    
    /// <remarks/>
    public class TextSymbolTypeAdvancedPlacement {
        
        private System.Double m_scaleLimit;
        
        private bool m_scaleLimitSpecified;
        
        /// <remarks/>
        public System.Double ScaleLimit {
            get {
                return this.m_scaleLimit;
            }
            set {
                this.m_scaleLimit = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ScaleLimitSpecified {
            get {
                return this.m_scaleLimitSpecified;
            }
            set {
                this.m_scaleLimitSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(W2DSymbolType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImageSymbolType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BlockSymbolType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MarkSymbolType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TextSymbolType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FontSymbolType))]
    public class SymbolType {
        
        private LengthUnitType m_unit;
        
        private SizeContextType m_sizeContext;
        
        private string m_sizeX;
        
        private string m_sizeY;
        
        private string m_rotation;
        
        private bool m_maintainAspect;
        
        private bool m_maintainAspectSpecified;
        
        private System.Double m_insertionPointX;
        
        private System.Double m_insertionPointY;
        
        private bool m_insertionPointYSpecified;
        
        /// <remarks/>
        public LengthUnitType Unit {
            get {
                return this.m_unit;
            }
            set {
                this.m_unit = value;
            }
        }
        
        /// <remarks/>
        public SizeContextType SizeContext {
            get {
                return this.m_sizeContext;
            }
            set {
                this.m_sizeContext = value;
            }
        }
        
        /// <remarks/>
        public string SizeX {
            get {
                return this.m_sizeX;
            }
            set {
                this.m_sizeX = value;
            }
        }
        
        /// <remarks/>
        public string SizeY {
            get {
                return this.m_sizeY;
            }
            set {
                this.m_sizeY = value;
            }
        }
        
        /// <remarks/>
        public string Rotation {
            get {
                return this.m_rotation;
            }
            set {
                this.m_rotation = value;
            }
        }
        
        /// <remarks/>
        public bool MaintainAspect {
            get {
                return this.m_maintainAspect;
            }
            set {
                this.m_maintainAspect = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaintainAspectSpecified {
            get {
                return this.m_maintainAspectSpecified;
            }
            set {
                this.m_maintainAspectSpecified = value;
            }
        }
        
        /// <remarks/>
        public System.Double InsertionPointX {
            get {
                return this.m_insertionPointX;
            }
            set {
                this.m_insertionPointX = value;
            }
        }
        
        /// <remarks/>
        public System.Double InsertionPointY {
            get {
                return this.m_insertionPointY;
            }
            set {
                this.m_insertionPointY = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InsertionPointYSpecified {
            get {
                return this.m_insertionPointYSpecified;
            }
            set {
                this.m_insertionPointYSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    public enum LengthUnitType {
        
        /// <remarks/>
        Millimeters,
        
        /// <remarks/>
        Centimeters,
        
        /// <remarks/>
        Meters,
        
        /// <remarks/>
        Kilometers,
        
        /// <remarks/>
        Inches,
        
        /// <remarks/>
        Feet,
        
        /// <remarks/>
        Yards,
        
        /// <remarks/>
        Miles,
        
        /// <remarks/>
        Points,
    }
    
    /// <remarks/>
    public enum SizeContextType {
        
        /// <remarks/>
        MappingUnits,
        
        /// <remarks/>
        DeviceUnits,
    }
    
    /// <remarks/>
    public class W2DSymbolType : SymbolType {
        
        private W2DSymbolTypeW2DSymbol m_w2DSymbol;
        
        private string m_fillColor;
        
        private string m_lineColor;
        
        private string m_textColor;
        
        /// <remarks/>
        public W2DSymbolTypeW2DSymbol W2DSymbol {
            get {
                return this.m_w2DSymbol;
            }
            set {
                this.m_w2DSymbol = value;
            }
        }
        
        /// <remarks/>
        public string FillColor {
            get {
                return this.m_fillColor;
            }
            set {
                this.m_fillColor = value;
            }
        }
        
        /// <remarks/>
        public string LineColor {
            get {
                return this.m_lineColor;
            }
            set {
                this.m_lineColor = value;
            }
        }
        
        /// <remarks/>
        public string TextColor {
            get {
                return this.m_textColor;
            }
            set {
                this.m_textColor = value;
            }
        }
    }
    
    /// <remarks/>
    public class W2DSymbolTypeW2DSymbol {
        
        private object m_resourceId;
        
        private object m_libraryItemName;
        
        /// <remarks/>
        public object ResourceId {
            get {
                return this.m_resourceId;
            }
            set {
                this.m_resourceId = value;
            }
        }
        
        /// <remarks/>
        public object LibraryItemName {
            get {
                return this.m_libraryItemName;
            }
            set {
                this.m_libraryItemName = value;
            }
        }
    }
    
    /// <remarks/>
    public class ImageSymbolType : SymbolType {
        
        private object m_item;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Image", typeof(ImageSymbolTypeImage))]
        [System.Xml.Serialization.XmlElementAttribute("Content", typeof(System.Byte[]), DataType="hexBinary")]
        public object Item {
            get {
                return this.m_item;
            }
            set {
                this.m_item = value;
            }
        }
    }
    
    /// <remarks/>
    public class ImageSymbolTypeImage {
        
        private object m_resourceId;
        
        private object m_libraryItemName;
        
        /// <remarks/>
        public object ResourceId {
            get {
                return this.m_resourceId;
            }
            set {
                this.m_resourceId = value;
            }
        }
        
        /// <remarks/>
        public object LibraryItemName {
            get {
                return this.m_libraryItemName;
            }
            set {
                this.m_libraryItemName = value;
            }
        }
    }
    
    /// <remarks/>
    public class BlockSymbolType : SymbolType {
        
        private string m_drawingName;
        
        private string m_blockName;
        
        private string m_blockColor;
        
        private string m_layerColor;
        
        /// <remarks/>
        public string DrawingName {
            get {
                return this.m_drawingName;
            }
            set {
                this.m_drawingName = value;
            }
        }
        
        /// <remarks/>
        public string BlockName {
            get {
                return this.m_blockName;
            }
            set {
                this.m_blockName = value;
            }
        }
        
        /// <remarks/>
        public string BlockColor {
            get {
                return this.m_blockColor;
            }
            set {
                this.m_blockColor = value;
            }
        }
        
        /// <remarks/>
        public string LayerColor {
            get {
                return this.m_layerColor;
            }
            set {
                this.m_layerColor = value;
            }
        }
    }
    
    /// <remarks/>
    public class MarkSymbolType : SymbolType {
        
        private ShapeType m_shape;
        
        private FillType m_fill;
        
        private StrokeType m_edge;
        
        /// <remarks/>
        public ShapeType Shape {
            get {
                return this.m_shape;
            }
            set {
                this.m_shape = value;
            }
        }
        
        /// <remarks/>
        public FillType Fill {
            get {
                return this.m_fill;
            }
            set {
                this.m_fill = value;
            }
        }
        
        /// <remarks/>
        public StrokeType Edge {
            get {
                return this.m_edge;
            }
            set {
                this.m_edge = value;
            }
        }
    }
    
    /// <remarks/>
    public enum ShapeType {
        
        /// <remarks/>
        Square,
        
        /// <remarks/>
        Circle,
        
        /// <remarks/>
        Triangle,
        
        /// <remarks/>
        Star,
        
        /// <remarks/>
        Cross,
        
        /// <remarks/>
        X,
    }
    
    /// <remarks/>
    public class FillType {
        
        private string m_fillPattern;
        
        private string m_foregroundColor;
        
        private string m_backgroundColor;
        
        /// <remarks/>
        public string FillPattern {
            get {
                return this.m_fillPattern;
            }
            set {
                this.m_fillPattern = value;
            }
        }
        
		[System.Xml.Serialization.XmlElementAttribute("ForegroundColor")]
		public string ForegroundColorAsHTML 
		{
            get {
                return this.m_foregroundColor;
            }
            set {
                this.m_foregroundColor = value;
            }
        }
        
        /// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("BackgroundColor")]
		public string BackgroundColorAsHTML 
		{
            get {
                return this.m_backgroundColor;
            }
            set {
                this.m_backgroundColor = value;
            }
        }

		[System.Xml.Serialization.XmlIgnore()]
		public System.Drawing.Color BackgroundColor 
		{
			get 
			{
				return Utility.ParseHTMLColor(this.m_backgroundColor);
			}
			set 
			{
				this.m_backgroundColor = Utility.SerializeHTMLColor(value, true);
			}
		}

		[System.Xml.Serialization.XmlIgnore()]
		public System.Drawing.Color ForegroundColor 
		{
			get 
			{
				return Utility.ParseHTMLColor(this.m_foregroundColor);
			}
			set 
			{
				this.m_foregroundColor = Utility.SerializeHTMLColor(value, true);
			}
		}

    }
    
    /// <remarks/>
    public class StrokeType {
        
        private string m_lineStyle;
        
        private string m_thickness;
        
        private string m_color;
        
        private LengthUnitType m_unit;
        
        /// <remarks/>
        public string LineStyle {
            get {
                return this.m_lineStyle;
            }
            set {
                this.m_lineStyle = value;
            }
        }
        
        /// <remarks/>
        public string Thickness {
            get {
                return this.m_thickness;
            }
            set {
                this.m_thickness = value;
            }
        }
        
		[System.Xml.Serialization.XmlElementAttribute("Color")]
        public string ColorAsHTML {
            get {
                return this.m_color;
            }
            set {
                this.m_color = value;
            }
        }
		[System.Xml.Serialization.XmlIgnore()]
		public System.Drawing.Color Color 
		{
			get 
			{
				return Utility.ParseHTMLColor(this.m_color);
			}
			set 
			{
				this.m_color = Utility.SerializeHTMLColor(value, true);
			}
		}


        
        /// <remarks/>
        public LengthUnitType Unit 
		{
            get {
                return this.m_unit;
            }
            set {
                this.m_unit = value;
            }
        }
    }
    
    /// <remarks/>
    public class FontSymbolType : SymbolType {
        
        private string m_fontName;
        
        private string m_character;
        
        private bool m_bold;
        
        private bool m_boldSpecified;
        
        private bool m_italic;
        
        private bool m_italicSpecified;
        
        private bool m_underlined;
        
        private bool m_underlinedSpecified;
        
        private string m_foregroundColor;
        
        /// <remarks/>
        public string FontName {
            get {
                return this.m_fontName;
            }
            set {
                this.m_fontName = value;
            }
        }
        
        /// <remarks/>
        public string Character {
            get {
                return this.m_character;
            }
            set {
                this.m_character = value;
            }
        }
        
        /// <remarks/>
        public bool Bold {
            get {
                return this.m_bold;
            }
            set {
                this.m_bold = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BoldSpecified {
            get {
                return this.m_boldSpecified;
            }
            set {
                this.m_boldSpecified = value;
            }
        }
        
        /// <remarks/>
        public bool Italic {
            get {
                return this.m_italic;
            }
            set {
                this.m_italic = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ItalicSpecified {
            get {
                return this.m_italicSpecified;
            }
            set {
                this.m_italicSpecified = value;
            }
        }
        
        /// <remarks/>
        public bool Underlined {
            get {
                return this.m_underlined;
            }
            set {
                this.m_underlined = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UnderlinedSpecified {
            get {
                return this.m_underlinedSpecified;
            }
            set {
                this.m_underlinedSpecified = value;
            }
        }
        
        /// <remarks/>
        public string ForegroundColor {
            get {
                return this.m_foregroundColor;
            }
            set {
                this.m_foregroundColor = value;
            }
        }
    }
    
    /// <remarks/>
    public class GridColorType {
        
        private object m_item;
        
        private ItemChoiceType m_itemElementName;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Bands", typeof(GridColorBandsType))]
        [System.Xml.Serialization.XmlElementAttribute("Band", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("ExplicitColor", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item {
            get {
                return this.m_item;
            }
            set {
                this.m_item = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName {
            get {
                return this.m_itemElementName;
            }
            set {
                this.m_itemElementName = value;
            }
        }
    }
    
    /// <remarks/>
    public class GridColorBandsType {
        
        private ChannelBandType m_redBand;
        
        private ChannelBandType m_greenBand;
        
        private ChannelBandType m_blueBand;
        
        /// <remarks/>
        public ChannelBandType RedBand {
            get {
                return this.m_redBand;
            }
            set {
                this.m_redBand = value;
            }
        }
        
        /// <remarks/>
        public ChannelBandType GreenBand {
            get {
                return this.m_greenBand;
            }
            set {
                this.m_greenBand = value;
            }
        }
        
        /// <remarks/>
        public ChannelBandType BlueBand {
            get {
                return this.m_blueBand;
            }
            set {
                this.m_blueBand = value;
            }
        }
    }
    
    /// <remarks/>
    public class ChannelBandType {
        
        private string m_band;
        
        private System.Double m_lowBand;
        
        private bool m_lowBandSpecified;
        
        private System.Double m_highBand;
        
        private bool m_highBandSpecified;
        
        private System.Byte m_lowChannel;
        
        private bool m_lowChannelSpecified;
        
        private System.Byte m_highChannel;
        
        private bool m_highChannelSpecified;
        
        /// <remarks/>
        public string Band {
            get {
                return this.m_band;
            }
            set {
                this.m_band = value;
            }
        }
        
        /// <remarks/>
        public System.Double LowBand {
            get {
                return this.m_lowBand;
            }
            set {
                this.m_lowBand = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LowBandSpecified {
            get {
                return this.m_lowBandSpecified;
            }
            set {
                this.m_lowBandSpecified = value;
            }
        }
        
        /// <remarks/>
        public System.Double HighBand {
            get {
                return this.m_highBand;
            }
            set {
                this.m_highBand = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HighBandSpecified {
            get {
                return this.m_highBandSpecified;
            }
            set {
                this.m_highBandSpecified = value;
            }
        }
        
        /// <remarks/>
        public System.Byte LowChannel {
            get {
                return this.m_lowChannel;
            }
            set {
                this.m_lowChannel = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LowChannelSpecified {
            get {
                return this.m_lowChannelSpecified;
            }
            set {
                this.m_lowChannelSpecified = value;
            }
        }
        
        /// <remarks/>
        public System.Byte HighChannel {
            get {
                return this.m_highChannel;
            }
            set {
                this.m_highChannel = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HighChannelSpecified {
            get {
                return this.m_highChannelSpecified;
            }
            set {
                this.m_highChannelSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    public enum ItemChoiceType {
        
        /// <remarks/>
        Bands,
        
        /// <remarks/>
        Band,
        
        /// <remarks/>
        ExplicitColor,
    }
    
    /// <remarks/>
    public class PointSymbolizationType {
    }
    
    /// <remarks/>
    public class LineRuleType {
        
        private string m_legendLabel;
        
        private string m_filter;
        
        private TextSymbolType m_label;
        
        private StrokeTypeCollection m_items;
        
        /// <remarks/>
        public string LegendLabel {
            get {
                return this.m_legendLabel;
            }
            set {
                this.m_legendLabel = value;
            }
        }
        
        /// <remarks/>
        public string Filter {
            get {
                return this.m_filter;
            }
            set {
                this.m_filter = value;
            }
        }
        
        /// <remarks/>
        public TextSymbolType Label {
            get {
                return this.m_label;
            }
            set {
                this.m_label = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("LineSymbolization2D")]
        public StrokeTypeCollection Items {
            get {
                return this.m_items;
            }
            set {
                this.m_items = value;
            }
        }
    }
    
    /// <remarks/>
    public class PointSymbolization2DType {
        
        private SymbolType m_item;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Font", typeof(FontSymbolType))]
        [System.Xml.Serialization.XmlElementAttribute("W2D", typeof(W2DSymbolType))]
        [System.Xml.Serialization.XmlElementAttribute("Image", typeof(ImageSymbolType))]
        [System.Xml.Serialization.XmlElementAttribute("Mark", typeof(MarkSymbolType))]
        [System.Xml.Serialization.XmlElementAttribute("Block", typeof(BlockSymbolType))]
        public SymbolType Item {
            get {
                return this.m_item;
            }
            set {
                this.m_item = value;
            }
        }
    }
    
    /// <remarks/>
    public class PointRuleType {
        
        private string m_legendLabel;
        
        private string m_filter;
        
        private TextSymbolType m_label;
        
        private PointSymbolization2DType m_item;
        
        /// <remarks/>
        public string LegendLabel {
            get {
                return this.m_legendLabel;
            }
            set {
                this.m_legendLabel = value;
            }
        }
        
        /// <remarks/>
        public string Filter {
            get {
                return this.m_filter;
            }
            set {
                this.m_filter = value;
            }
        }
        
        /// <remarks/>
        public TextSymbolType Label {
            get {
                return this.m_label;
            }
            set {
                this.m_label = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PointSymbolization2D")]
        public PointSymbolization2DType Item {
            get {
                return this.m_item;
            }
            set {
                this.m_item = value;
            }
        }
    }
    
	public class LineTypeStyleType
	{
		private LineRuleTypeCollection m_lineRule;
		[System.Xml.Serialization.XmlElementAttribute("LineRule")]
		public LineRuleTypeCollection LineRule 
		{
			get 
			{
				return this.m_lineRule;
			}
			set 
			{
				this.m_lineRule = value;
			}
		}
	}

	public class AreaTypeStyleType
	{
		private AreaRuleTypeCollection m_areaRule;
		[System.Xml.Serialization.XmlElementAttribute("AreaRule")]
		public AreaRuleTypeCollection AreaRule 
		{
			get 
			{
				return this.m_areaRule;
			}
			set 
			{
				this.m_areaRule = value;
			}
		}
	}

    /// <remarks/>
    public class PointTypeStyleType 
	{
        
        private bool m_displayAsText;
        
        private bool m_allowOverpost;
        
        private PointRuleTypeCollection m_pointRule;
        
        /// <remarks/>
        public bool DisplayAsText {
            get {
                return this.m_displayAsText;
            }
            set {
                this.m_displayAsText = value;
            }
        }
        
        /// <remarks/>
        public bool AllowOverpost {
            get {
                return this.m_allowOverpost;
            }
            set {
                this.m_allowOverpost = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PointRule")]
        public PointRuleTypeCollection PointRule {
            get {
                return this.m_pointRule;
            }
            set {
                this.m_pointRule = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AreaSymbolizationFillType))]
    public class AreaSymbolizationType {
    }
    
    /// <remarks/>
    public class AreaSymbolizationFillType : AreaSymbolizationType {
        
        private FillType m_fill;
        
        private StrokeType m_stroke;
        
        /// <remarks/>
        public FillType Fill {
            get {
                return this.m_fill;
            }
            set {
                this.m_fill = value;
            }
        }
        
        /// <remarks/>
        public StrokeType Stroke {
            get {
                return this.m_stroke;
            }
            set {
                this.m_stroke = value;
            }
        }
    }
    
    /// <remarks/>
    public class AreaRuleType {
        
        private string m_legendLabel;
        
        private string m_filter;
        
        private TextSymbolType m_label;
        
        private AreaSymbolizationFillType m_item;
        
        /// <remarks/>
        public string LegendLabel {
            get {
                return this.m_legendLabel;
            }
            set {
                this.m_legendLabel = value;
            }
        }
        
        /// <remarks/>
        public string Filter {
            get {
                return this.m_filter;
            }
            set {
                this.m_filter = value;
            }
        }
        
        /// <remarks/>
        public TextSymbolType Label {
            get {
                return this.m_label;
            }
            set {
                this.m_label = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AreaSymbolization2D")]
        public AreaSymbolizationFillType Item {
            get {
                return this.m_item;
            }
            set {
                this.m_item = value;
            }
        }
    }
    
    /// <remarks/>
    public class VectorScaleRangeType {
        
        private System.Double m_minScale;
        
        private bool m_minScaleSpecified;
        
        private System.Double m_maxScale;
        
        private bool m_maxScaleSpecified;
        
        private ArrayList m_items;
        
        /// <remarks/>
        public System.Double MinScale {
            get {
                return this.m_minScale;
            }
            set {
                this.m_minScale = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinScaleSpecified {
            get {
                return this.m_minScaleSpecified;
            }
            set {
                this.m_minScaleSpecified = value;
            }
        }
        
        /// <remarks/>
        public System.Double MaxScale {
            get {
                return this.m_maxScale;
            }
            set {
                this.m_maxScale = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaxScaleSpecified {
            get {
                return this.m_maxScaleSpecified;
            }
            set {
                this.m_maxScaleSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PointTypeStyle", typeof(PointTypeStyleType))]
        [System.Xml.Serialization.XmlElementAttribute("LineTypeStyle", typeof(LineTypeStyleType))]
        [System.Xml.Serialization.XmlElementAttribute("AreaTypeStyle", typeof(AreaTypeStyleType))]
        public ArrayList Items {
            get {
                return this.m_items;
            }
            set {
                this.m_items = value;
            }
        }
    }
    
    /// <remarks/>
    public class NameStringPairType {
        
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
    public class GridLayerDefinitionType : BaseLayerDefinitionType {
        
        private string m_featureName;
        
        private string m_geometry;
        
        private string m_filter;
        
        private GridScaleRangeTypeCollection m_gridScaleRange;
        
        /// <remarks/>
        public string FeatureName {
            get {
                return this.m_featureName;
            }
            set {
                this.m_featureName = value;
            }
        }
        
        /// <remarks/>
        public string Geometry {
            get {
                return this.m_geometry;
            }
            set {
                this.m_geometry = value;
            }
        }
        
        /// <remarks/>
        public string Filter {
            get {
                return this.m_filter;
            }
            set {
                this.m_filter = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GridScaleRange")]
        public GridScaleRangeTypeCollection GridScaleRange {
            get {
                return this.m_gridScaleRange;
            }
            set {
                this.m_gridScaleRange = value;
            }
        }
    }
    
    /// <remarks/>
    public class VectorLayerDefinitionType : BaseLayerDefinitionType {
        
        private string m_featureName;
        
        private FeatureNameType m_featureNameType;
        
        private string m_filter;
        
        private NameStringPairTypeCollection m_propertyMapping;
        
        private string m_geometry;
        
        private string m_url;
        
        private string m_toolTip;
        
        private VectorScaleRangeTypeCollection m_vectorScaleRange;
        
        /// <remarks/>
        public string FeatureName {
            get {
                return this.m_featureName;
            }
            set {
                this.m_featureName = value;
            }
        }
        
        /// <remarks/>
        public FeatureNameType FeatureNameType {
            get {
                return this.m_featureNameType;
            }
            set {
                this.m_featureNameType = value;
            }
        }
        
        /// <remarks/>
        public string Filter {
            get {
                return this.m_filter;
            }
            set {
                this.m_filter = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PropertyMapping")]
        public NameStringPairTypeCollection PropertyMapping {
            get {
                return this.m_propertyMapping;
            }
            set {
                this.m_propertyMapping = value;
            }
        }
        
        /// <remarks/>
        public string Geometry {
            get {
                return this.m_geometry;
            }
            set {
                this.m_geometry = value;
            }
        }
        
        /// <remarks/>
        public string Url {
            get {
                return this.m_url;
            }
            set {
                this.m_url = value;
            }
        }
        
        /// <remarks/>
        public string ToolTip {
            get {
                return this.m_toolTip;
            }
            set {
                this.m_toolTip = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("VectorScaleRange")]
        public VectorScaleRangeTypeCollection VectorScaleRange {
            get {
                return this.m_vectorScaleRange;
            }
            set {
                this.m_vectorScaleRange = value;
            }
        }
    }
    
    /// <remarks/>
    public enum FeatureNameType {
        
        /// <remarks/>
        FeatureClass,
        
        /// <remarks/>
        NamedExtension,
    }
    
    public class GridColorRuleTypeCollection : System.Collections.CollectionBase {
        
        public GridColorRuleType this[int idx] {
            get {
                return ((GridColorRuleType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(GridColorRuleType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class StrokeTypeCollection : System.Collections.CollectionBase {
        
        public StrokeType this[int idx] {
            get {
                return ((StrokeType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(StrokeType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class PointRuleTypeCollection : System.Collections.CollectionBase {
        
        public PointRuleType this[int idx] {
            get {
                return ((PointRuleType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(PointRuleType value) {
            return base.InnerList.Add(value);
        }
    }
  
    public class GridScaleRangeTypeCollection : System.Collections.CollectionBase {
        
        public GridScaleRangeType this[int idx] {
            get {
                return ((GridScaleRangeType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(GridScaleRangeType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class NameStringPairTypeCollection : System.Collections.CollectionBase {
        
        public NameStringPairType this[int idx] {
            get {
                return ((NameStringPairType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(NameStringPairType value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class VectorScaleRangeTypeCollection : System.Collections.CollectionBase {
        
        public VectorScaleRangeType this[int idx] {
            get {
                return ((VectorScaleRangeType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(VectorScaleRangeType value) {
            return base.InnerList.Add(value);
        }
    }

	public class LineRuleTypeCollection : System.Collections.CollectionBase
	{
		public LineRuleType this[int idx] 
		{
			get 
			{
				return ((LineRuleType)(base.InnerList[idx]));
			}
			set 
			{
				base.InnerList[idx] = value;
			}
		}
        
		public int Add(LineRuleType value) 
		{
			return base.InnerList.Add(value);
		}
	}

	public class AreaRuleTypeCollection : System.Collections.CollectionBase 
	{
        
		public AreaRuleType this[int idx] 
		{
			get 
			{
				return ((AreaRuleType)(base.InnerList[idx]));
			}
			set 
			{
				base.InnerList[idx] = value;
			}
		}
        
		public int Add(AreaRuleType value) 
		{
			return base.InnerList.Add(value);
		}
	}
	*/
}
