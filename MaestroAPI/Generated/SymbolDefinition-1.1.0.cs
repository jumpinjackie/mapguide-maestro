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
    
    /*
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="CompoundSymbolDefinition")]
    [System.Xml.Serialization.XmlRootAttribute("CompoundSymbolDefinition", Namespace="", IsNullable=false)]
    public class CompoundSymbolDefinition1 : CompoundSymbolDefinition {
        
        private string m_version = "1.1.0";
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1.1.0")]
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CompoundSymbolDefinition1))]
    public class CompoundSymbolDefinition : SymbolDefinitionBase {
        
        private SimpleSymbolCollection m_simpleSymbol;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SimpleSymbol")]
        public SimpleSymbolCollection SimpleSymbol {
            get {
                return this.m_simpleSymbol;
            }
            set {
                this.m_simpleSymbol = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public class SimpleSymbol {
        
        private object m_item;
        
        private string m_renderingPass = "0";
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SimpleSymbolDefinition", typeof(SimpleSymbolDefinition))]
        [System.Xml.Serialization.XmlElementAttribute("ResourceId", typeof(string))]
        public object Item {
            get {
                return this.m_item;
            }
            set {
                this.m_item = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0")]
        public string RenderingPass {
            get {
                return this.m_renderingPass;
            }
            set {
                this.m_renderingPass = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public class SimpleSymbolDefinition : SymbolDefinitionBase {
        
        private GraphicBaseCollection m_graphics;
        
        private ResizeBox m_resizeBox;
        
        private PointUsage m_pointUsage;
        
        private LineUsage m_lineUsage;
        
        private AreaUsage m_areaUsage;
        
        private ParameterDefinition m_parameterDefinition;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(typeof(Text), IsNullable=false)]
        [System.Xml.Serialization.XmlArrayItemAttribute(typeof(Image), IsNullable=false)]
        [System.Xml.Serialization.XmlArrayItemAttribute(typeof(Path), IsNullable=false)]
        public GraphicBaseCollection Graphics {
            get {
                return this.m_graphics;
            }
            set {
                this.m_graphics = value;
            }
        }
        
        /// <remarks/>
        public ResizeBox ResizeBox {
            get {
                return this.m_resizeBox;
            }
            set {
                this.m_resizeBox = value;
            }
        }
        
        /// <remarks/>
        public PointUsage PointUsage {
            get {
                return this.m_pointUsage;
            }
            set {
                this.m_pointUsage = value;
            }
        }
        
        /// <remarks/>
        public LineUsage LineUsage {
            get {
                return this.m_lineUsage;
            }
            set {
                this.m_lineUsage = value;
            }
        }
        
        /// <remarks/>
        public AreaUsage AreaUsage {
            get {
                return this.m_areaUsage;
            }
            set {
                this.m_areaUsage = value;
            }
        }
        
        /// <remarks/>
        public ParameterDefinition ParameterDefinition {
            get {
                return this.m_parameterDefinition;
            }
            set {
                this.m_parameterDefinition = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public class Text : GraphicBase {
        
        private string m_content;
        
        private string m_fontName = "\'Arial\'";
        
        private string m_bold = "false";
        
        private string m_italic = "false";
        
        private string m_underlined = "false";
        
        private string m_overlined = "false";
        
        private string m_obliqueAngle = "0.0";
        
        private string m_trackSpacing = "1.0";
        
        private string m_height = "4.0";
        
        private string m_heightScalable = "true";
        
        private string m_angle = "0.0";
        
        private string m_positionX = "0.0";
        
        private string m_positionY = "0.0";
        
        private string m_horizontalAlignment = "\'Center\'";
        
        private string m_verticalAlignment = "\'Halfline\'";
        
        private string m_justification = "\'FromAlignment\'";
        
        private string m_lineSpacing = "1.05";
        
        private string m_textColor = "ff000000";
        
        private string m_ghostColor;
        
        private TextFrame m_frame;
        
        private string m_markup = "\'Plain\'";
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        public string Content {
            get {
                return this.m_content;
            }
            set {
                this.m_content = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'Arial\'")]
        public string FontName {
            get {
                return this.m_fontName;
            }
            set {
                this.m_fontName = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("false")]
        public string Bold {
            get {
                return this.m_bold;
            }
            set {
                this.m_bold = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("false")]
        public string Italic {
            get {
                return this.m_italic;
            }
            set {
                this.m_italic = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("false")]
        public string Underlined {
            get {
                return this.m_underlined;
            }
            set {
                this.m_underlined = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("false")]
        public string Overlined {
            get {
                return this.m_overlined;
            }
            set {
                this.m_overlined = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string ObliqueAngle {
            get {
                return this.m_obliqueAngle;
            }
            set {
                this.m_obliqueAngle = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("1.0")]
        public string TrackSpacing {
            get {
                return this.m_trackSpacing;
            }
            set {
                this.m_trackSpacing = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("4.0")]
        public string Height {
            get {
                return this.m_height;
            }
            set {
                this.m_height = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("true")]
        public string HeightScalable {
            get {
                return this.m_heightScalable;
            }
            set {
                this.m_heightScalable = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string Angle {
            get {
                return this.m_angle;
            }
            set {
                this.m_angle = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string PositionX {
            get {
                return this.m_positionX;
            }
            set {
                this.m_positionX = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string PositionY {
            get {
                return this.m_positionY;
            }
            set {
                this.m_positionY = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'Center\'")]
        public string HorizontalAlignment {
            get {
                return this.m_horizontalAlignment;
            }
            set {
                this.m_horizontalAlignment = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'Halfline\'")]
        public string VerticalAlignment {
            get {
                return this.m_verticalAlignment;
            }
            set {
                this.m_verticalAlignment = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'FromAlignment\'")]
        public string Justification {
            get {
                return this.m_justification;
            }
            set {
                this.m_justification = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("1.05")]
        public string LineSpacing {
            get {
                return this.m_lineSpacing;
            }
            set {
                this.m_lineSpacing = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("ff000000")]
        public string TextColor {
            get {
                return this.m_textColor;
            }
            set {
                this.m_textColor = value;
            }
        }
        
        /// <remarks/>
        public string GhostColor {
            get {
                return this.m_ghostColor;
            }
            set {
                this.m_ghostColor = value;
            }
        }
        
        /// <remarks/>
        public TextFrame Frame {
            get {
                return this.m_frame;
            }
            set {
                this.m_frame = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'Plain\'")]
        public string Markup {
            get {
                return this.m_markup;
            }
            set {
                this.m_markup = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public class TextFrame {
        
        private string m_lineColor;
        
        private string m_fillColor;
        
        private string m_offsetX = "0.0";
        
        private string m_offsetY = "0.0";
        
        private ExtendedDataType m_extendedData1;
        
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
        public string FillColor {
            get {
                return this.m_fillColor;
            }
            set {
                this.m_fillColor = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string OffsetX {
            get {
                return this.m_offsetX;
            }
            set {
                this.m_offsetX = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string OffsetY {
            get {
                return this.m_offsetY;
            }
            set {
                this.m_offsetY = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public class ExtendedDataType {
        
        private System.Xml.XmlElementCollection m_any;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlElementCollection Any {
            get {
                return this.m_any;
            }
            set {
                this.m_any = value;
            }
        }
    }
    
    /// <remarks/>
    public class Parameter {
        
        private string m_identifier;
        
        private string m_defaultValue;
        
        private string m_displayName;
        
        private string m_description;
        
        private DataType m_dataType = DataType.String;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        public string Identifier {
            get {
                return this.m_identifier;
            }
            set {
                this.m_identifier = value;
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
        [System.ComponentModel.DefaultValueAttribute(DataType.String)]
        public DataType DataType {
            get {
                return this.m_dataType;
            }
            set {
                this.m_dataType = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public enum DataType {
        
        /// <remarks/>
        String,
        
        /// <remarks/>
        Boolean,
        
        /// <remarks/>
        Integer,
        
        /// <remarks/>
        Real,
        
        /// <remarks/>
        Color,
        
        /// <remarks/>
        Angle,
        
        /// <remarks/>
        FillColor,
        
        /// <remarks/>
        LineColor,
        
        /// <remarks/>
        LineWeight,
        
        /// <remarks/>
        Content,
        
        /// <remarks/>
        Markup,
        
        /// <remarks/>
        FontName,
        
        /// <remarks/>
        Bold,
        
        /// <remarks/>
        Italic,
        
        /// <remarks/>
        Underlined,
        
        /// <remarks/>
        Overlined,
        
        /// <remarks/>
        ObliqueAngle,
        
        /// <remarks/>
        TrackSpacing,
        
        /// <remarks/>
        FontHeight,
        
        /// <remarks/>
        HorizontalAlignment,
        
        /// <remarks/>
        VerticalAlignment,
        
        /// <remarks/>
        Justification,
        
        /// <remarks/>
        LineSpacing,
        
        /// <remarks/>
        TextColor,
        
        /// <remarks/>
        GhostColor,
        
        /// <remarks/>
        FrameLineColor,
        
        /// <remarks/>
        FrameFillColor,
        
        /// <remarks/>
        StartOffset,
        
        /// <remarks/>
        EndOffset,
        
        /// <remarks/>
        RepeatX,
        
        /// <remarks/>
        RepeatY,
    }
    
    /// <remarks/>
    public class ParameterDefinition {
        
        private ParameterCollection m_parameter;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Parameter")]
        public ParameterCollection Parameter {
            get {
                return this.m_parameter;
            }
            set {
                this.m_parameter = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public class AreaUsage {
        
        private string m_angleControl = "\'FromAngle\'";
        
        private string m_originControl = "\'Global\'";
        
        private string m_clippingControl = "\'Clip\'";
        
        private string m_angle = "0.0";
        
        private string m_originX = "0.0";
        
        private string m_originY = "0.0";
        
        private string m_repeatX = "0.0";
        
        private string m_repeatY = "0.0";
        
        private string m_bufferWidth = "0.0";
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'FromAngle\'")]
        public string AngleControl {
            get {
                return this.m_angleControl;
            }
            set {
                this.m_angleControl = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'Global\'")]
        public string OriginControl {
            get {
                return this.m_originControl;
            }
            set {
                this.m_originControl = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'Clip\'")]
        public string ClippingControl {
            get {
                return this.m_clippingControl;
            }
            set {
                this.m_clippingControl = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string Angle {
            get {
                return this.m_angle;
            }
            set {
                this.m_angle = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string OriginX {
            get {
                return this.m_originX;
            }
            set {
                this.m_originX = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string OriginY {
            get {
                return this.m_originY;
            }
            set {
                this.m_originY = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string RepeatX {
            get {
                return this.m_repeatX;
            }
            set {
                this.m_repeatX = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string RepeatY {
            get {
                return this.m_repeatY;
            }
            set {
                this.m_repeatY = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string BufferWidth {
            get {
                return this.m_bufferWidth;
            }
            set {
                this.m_bufferWidth = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public class LineUsage {
        
        private string m_angleControl = "\'FromGeometry\'";
        
        private string m_unitsControl = "\'Absolute\'";
        
        private string m_vertexControl = "\'OverlapNone\'";
        
        private string m_angle = "0.0";
        
        private string m_startOffset;
        
        private string m_endOffset;
        
        private string m_repeat = "0.0";
        
        private string m_vertexAngleLimit = "0.0";
        
        private string m_vertexJoin = "\'Round\'";
        
        private string m_vertexMiterLimit = "5.0";
        
        private Path m_defaultPath;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'FromGeometry\'")]
        public string AngleControl {
            get {
                return this.m_angleControl;
            }
            set {
                this.m_angleControl = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'Absolute\'")]
        public string UnitsControl {
            get {
                return this.m_unitsControl;
            }
            set {
                this.m_unitsControl = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'OverlapNone\'")]
        public string VertexControl {
            get {
                return this.m_vertexControl;
            }
            set {
                this.m_vertexControl = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string Angle {
            get {
                return this.m_angle;
            }
            set {
                this.m_angle = value;
            }
        }
        
        /// <remarks/>
        public string StartOffset {
            get {
                return this.m_startOffset;
            }
            set {
                this.m_startOffset = value;
            }
        }
        
        /// <remarks/>
        public string EndOffset {
            get {
                return this.m_endOffset;
            }
            set {
                this.m_endOffset = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string Repeat {
            get {
                return this.m_repeat;
            }
            set {
                this.m_repeat = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string VertexAngleLimit {
            get {
                return this.m_vertexAngleLimit;
            }
            set {
                this.m_vertexAngleLimit = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'Round\'")]
        public string VertexJoin {
            get {
                return this.m_vertexJoin;
            }
            set {
                this.m_vertexJoin = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("5.0")]
        public string VertexMiterLimit {
            get {
                return this.m_vertexMiterLimit;
            }
            set {
                this.m_vertexMiterLimit = value;
            }
        }
        
        /// <remarks/>
        public Path DefaultPath {
            get {
                return this.m_defaultPath;
            }
            set {
                this.m_defaultPath = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public class Path : GraphicBase {
        
        private string m_geometry;
        
        private string m_fillColor;
        
        private string m_lineColor;
        
        private string m_lineWeight = "0.0";
        
        private string m_lineWeightScalable = "true";
        
        private string m_lineCap = "\'Round\'";
        
        private string m_lineJoin = "\'Round\'";
        
        private string m_lineMiterLimit = "5.0";
        
        private ExtendedDataType m_extendedData1;
        
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
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string LineWeight {
            get {
                return this.m_lineWeight;
            }
            set {
                this.m_lineWeight = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("true")]
        public string LineWeightScalable {
            get {
                return this.m_lineWeightScalable;
            }
            set {
                this.m_lineWeightScalable = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'Round\'")]
        public string LineCap {
            get {
                return this.m_lineCap;
            }
            set {
                this.m_lineCap = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'Round\'")]
        public string LineJoin {
            get {
                return this.m_lineJoin;
            }
            set {
                this.m_lineJoin = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("5.0")]
        public string LineMiterLimit {
            get {
                return this.m_lineMiterLimit;
            }
            set {
                this.m_lineMiterLimit = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Image))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Text))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Path))]
    public class GraphicBase {
        
        private string m_resizeControl = "\'ResizeNone\'";
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'ResizeNone\'")]
        public string ResizeControl {
            get {
                return this.m_resizeControl;
            }
            set {
                this.m_resizeControl = value;
            }
        }
    }
    
    /// <remarks/>
    public class Image : GraphicBase {
        
        private object m_item;
        
        private string m_sizeX = "1.0";
        
        private string m_sizeY = "1.0";
        
        private string m_sizeScalable = "true";
        
        private string m_angle = "0.0";
        
        private string m_positionX = "0.0";
        
        private string m_positionY = "0.0";
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Content", typeof(System.Byte[]), DataType="base64Binary")]
        [System.Xml.Serialization.XmlElementAttribute("Reference", typeof(ImageReference))]
        public object Item {
            get {
                return this.m_item;
            }
            set {
                this.m_item = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("1.0")]
        public string SizeX {
            get {
                return this.m_sizeX;
            }
            set {
                this.m_sizeX = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("1.0")]
        public string SizeY {
            get {
                return this.m_sizeY;
            }
            set {
                this.m_sizeY = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("true")]
        public string SizeScalable {
            get {
                return this.m_sizeScalable;
            }
            set {
                this.m_sizeScalable = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string Angle {
            get {
                return this.m_angle;
            }
            set {
                this.m_angle = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string PositionX {
            get {
                return this.m_positionX;
            }
            set {
                this.m_positionX = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string PositionY {
            get {
                return this.m_positionY;
            }
            set {
                this.m_positionY = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public class ImageReference {
        
        private string m_resourceId;
        
        private string m_libraryItemName;
        
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
        public string LibraryItemName {
            get {
                return this.m_libraryItemName;
            }
            set {
                this.m_libraryItemName = value;
            }
        }
    }
    
    /// <remarks/>
    public class PointUsage {
        
        private string m_angleControl = "\'FromAngle\'";
        
        private string m_angle = "0.0";
        
        private string m_originOffsetX = "0.0";
        
        private string m_originOffsetY = "0.0";
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'FromAngle\'")]
        public string AngleControl {
            get {
                return this.m_angleControl;
            }
            set {
                this.m_angleControl = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string Angle {
            get {
                return this.m_angle;
            }
            set {
                this.m_angle = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string OriginOffsetX {
            get {
                return this.m_originOffsetX;
            }
            set {
                this.m_originOffsetX = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string OriginOffsetY {
            get {
                return this.m_originOffsetY;
            }
            set {
                this.m_originOffsetY = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public class ResizeBox {
        
        private string m_sizeX = "1.0";
        
        private string m_sizeY = "1.0";
        
        private string m_positionX = "0.0";
        
        private string m_positionY = "0.0";
        
        private string m_growControl = "\'GrowInXYMaintainAspect\'";
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("1.0")]
        public string SizeX {
            get {
                return this.m_sizeX;
            }
            set {
                this.m_sizeX = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("1.0")]
        public string SizeY {
            get {
                return this.m_sizeY;
            }
            set {
                this.m_sizeY = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string PositionX {
            get {
                return this.m_positionX;
            }
            set {
                this.m_positionX = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string PositionY {
            get {
                return this.m_positionY;
            }
            set {
                this.m_positionY = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("\'GrowInXYMaintainAspect\'")]
        public string GrowControl {
            get {
                return this.m_growControl;
            }
            set {
                this.m_growControl = value;
            }
        }
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 {
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SimpleSymbolDefinition))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CompoundSymbolDefinition))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CompoundSymbolDefinition1))]
    public class SymbolDefinitionBase {
        
        private string m_name;
        
        private string m_description;
        
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
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="SimpleSymbolDefinition")]
    [System.Xml.Serialization.XmlRootAttribute("SimpleSymbolDefinition", Namespace="", IsNullable=false)]
    public class SimpleSymbolDefinition1 : SimpleSymbolDefinition {
        
        private string m_version = "1.1.0";
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1.1.0")]
        public string version {
            get {
                return this.m_version;
            }
            set {
                this.m_version = value;
            }
        }
    }
    
    public class SimpleSymbolCollection : System.Collections.CollectionBase {
        
        public SimpleSymbol this[int idx] {
            get {
                return ((SimpleSymbol)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(SimpleSymbol value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class GraphicBaseCollection : System.Collections.CollectionBase {
        
        public GraphicBase this[int idx] {
            get {
                return ((GraphicBase)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(GraphicBase value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class System.Xml.XmlElementCollection : System.Collections.CollectionBase {
        
        public System.Xml.XmlElement this[int idx] {
            get {
                return ((System.Xml.XmlElement)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(System.Xml.XmlElement value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class ParameterCollection : System.Collections.CollectionBase {
        
        public Parameter this[int idx] {
            get {
                return ((Parameter)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(Parameter value) {
            return base.InnerList.Add(value);
        }
    }
	*/
}
