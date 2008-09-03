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
/*#region Disclaimer / License
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
using System.Collections;

namespace OSGeo.MapGuide.MaestroAPI 
{
            
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CompoundSymbolDefinition))]
    public class CompoundSymbolDefinition : SymbolDefinitionBase {
        
		private string m_version = "1.0.0";
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

		[System.Xml.Serialization.XmlAttributeAttribute()]
		[System.ComponentModel.DefaultValueAttribute("1.0.0")]
		public string version 
		{
			get 
			{
				return this.m_version;
			}
			set 
			{
				this.m_version = value;
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
        
        private XmlElementCollection m_any;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public XmlElementCollection Any {
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
        
        private DataType m_dataType;
        
        private bool m_dataTypeSpecified;
        
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
        public DataType DataType {
            get {
                return this.m_dataType;
            }
            set {
                this.m_dataType = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DataTypeSpecified {
            get {
                return this.m_dataTypeSpecified;
            }
            set {
                this.m_dataTypeSpecified = value;
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
        
        private string m_startOffset = "0.0";
        
        private string m_endOffset = "0.0";
        
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
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string StartOffset {
            get {
                return this.m_startOffset;
            }
            set {
                this.m_startOffset = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Path))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Image))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Text))]
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CompoundSymbolDefinition))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SimpleSymbolDefinition))]
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
        
        private string m_version = "1.0.0";
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1.0.0")]
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
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class LayerDefinition : LayerDefinitionType {
        
		public static readonly string SchemaName = "LayerDefinition-1.2.0.xsd";
		public static readonly string SchemaName1_1 = "LayerDefinition-1.1.0.xsd";
		public static readonly string SchemaName1_0 = "LayerDefinition-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get 
			{ 
				if (m_version == "1.2.0") 
					return SchemaName;
				else if (m_version == "1.1.0") 
					return SchemaName1_1; 
				else
					return SchemaName1_0;
			}
			set { if (value != SchemaName && value != SchemaName1_1 || value != SchemaName1_0) throw new System.Exception("Cannot set the schema name"); }
		}

		private string m_resourceId;
		[System.Xml.Serialization.XmlIgnore()]
		public string ResourceId 
		{ 
			get { return m_resourceId; } 
			set { m_resourceId = value; } 
		}

        private string m_version = "1.1.0";
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
		public string version 
		{
            get {
				return this.m_version;
            }
            set {
                this.m_version = value;
            }
        }

		public LayerDefinition()
			: base()
		{
			m_version = "1.2.0";
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
        
        private ExtendedDataType m_extendedData1;
        
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VectorLayerDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GridLayerDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DrawingLayerDefinitionType))]
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
    public class VectorLayerDefinitionType : BaseLayerDefinitionType {
        
        private string m_featureName;
        
        private FeatureNameType m_featureNameType;
        
        private string m_filter;
        
        private NameStringPairTypeCollection m_propertyMapping;
        
        private string m_geometry;
        
        private string m_url;
        
        private string m_toolTip;
        
        private VectorScaleRangeTypeCollection m_vectorScaleRange;
        
        private ExtendedDataType m_extendedData1;
        
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
    public enum FeatureNameType {
        
        /// <remarks/>
        FeatureClass,
        
        /// <remarks/>
        NamedExtension,
    }
    
    /// <remarks/>
    public class NameStringPairType {
        
        private string m_name;
        
        private string m_value;
        
        private ExtendedDataType m_extendedData1;
        
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
    public class VectorScaleRangeType {
        
        private System.Double m_minScale;
        
        private bool m_minScaleSpecified;
        
        private System.Double m_maxScale;
        
        private bool m_maxScaleSpecified;
        
        private System.Collections.ArrayList m_items;
        
        private ElevationSettingsType m_elevationSettings;
        
        private ExtendedDataType m_extendedData1;
        
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
        [System.Xml.Serialization.XmlElementAttribute("CompositeTypeStyle", typeof(CompositeTypeStyle))]
        [System.Xml.Serialization.XmlElementAttribute("PointTypeStyle", typeof(PointTypeStyleType))]
        [System.Xml.Serialization.XmlElementAttribute("LineTypeStyle", typeof(LineTypeStyleType))]
        [System.Xml.Serialization.XmlElementAttribute("AreaTypeStyle", typeof(AreaTypeStyleType))]
        public System.Collections.ArrayList Items {
            get {
                return this.m_items;
            }
            set {
                this.m_items = value;
            }
        }
        
        /// <remarks/>
        public ElevationSettingsType ElevationSettings {
            get {
                return this.m_elevationSettings;
            }
            set {
                this.m_elevationSettings = value;
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
    public class CompositeTypeStyle {
        
        private CompositeRuleCollection m_compositeRule;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CompositeRule")]
        public CompositeRuleCollection CompositeRule {
            get {
                return this.m_compositeRule;
            }
            set {
                this.m_compositeRule = value;
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
    public class CompositeRule {
        
        private string m_legendLabel;
        
        private string m_filter;
        
        private CompositeSymbolization m_compositeSymbolization;
        
        private ExtendedDataType m_extendedData1;
        
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
        public CompositeSymbolization CompositeSymbolization {
            get {
                return this.m_compositeSymbolization;
            }
            set {
                this.m_compositeSymbolization = value;
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
    public class CompositeSymbolization {
        
        private SymbolInstanceCollection m_symbolInstance;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SymbolInstance")]
        public SymbolInstanceCollection SymbolInstance {
            get {
                return this.m_symbolInstance;
            }
            set {
                this.m_symbolInstance = value;
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
    public class SymbolInstance {
        
        private object m_item;
        
        private ParameterOverrides m_parameterOverrides;
        
        private string m_scaleX = "1.0";
        
        private string m_scaleY = "1.0";
        
        private string m_insertionOffsetX = "0.0";
        
        private string m_insertionOffsetY = "0.0";
        
        private SizeContextType m_sizeContext = SizeContextType.DeviceUnits;
        
        private string m_drawLast = "false";
        
        private string m_checkExclusionRegion = "false";
        
        private string m_addToExclusionRegion = "false";
        
        private string m_positioningAlgorithm;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CompoundSymbolDefinition", typeof(CompoundSymbolDefinition))]
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
        public ParameterOverrides ParameterOverrides {
            get {
                return this.m_parameterOverrides;
            }
            set {
                this.m_parameterOverrides = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("1.0")]
        public string ScaleX {
            get {
                return this.m_scaleX;
            }
            set {
                this.m_scaleX = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("1.0")]
        public string ScaleY {
            get {
                return this.m_scaleY;
            }
            set {
                this.m_scaleY = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string InsertionOffsetX {
            get {
                return this.m_insertionOffsetX;
            }
            set {
                this.m_insertionOffsetX = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("0.0")]
        public string InsertionOffsetY {
            get {
                return this.m_insertionOffsetY;
            }
            set {
                this.m_insertionOffsetY = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(SizeContextType.DeviceUnits)]
        public SizeContextType SizeContext {
            get {
                return this.m_sizeContext;
            }
            set {
                this.m_sizeContext = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("false")]
        public string DrawLast {
            get {
                return this.m_drawLast;
            }
            set {
                this.m_drawLast = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("false")]
        public string CheckExclusionRegion {
            get {
                return this.m_checkExclusionRegion;
            }
            set {
                this.m_checkExclusionRegion = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("false")]
        public string AddToExclusionRegion {
            get {
                return this.m_addToExclusionRegion;
            }
            set {
                this.m_addToExclusionRegion = value;
            }
        }
        
        /// <remarks/>
        public string PositioningAlgorithm {
            get {
                return this.m_positioningAlgorithm;
            }
            set {
                this.m_positioningAlgorithm = value;
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
    public class ParameterOverrides {
        
        private OverrideCollection m_override;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Override")]
        public OverrideCollection Override {
            get {
                return this.m_override;
            }
            set {
                this.m_override = value;
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
    public class Override {
        
        private string m_symbolName;
        
        private string m_parameterIdentifier;
        
        private string m_parameterValue;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        public string SymbolName {
            get {
                return this.m_symbolName;
            }
            set {
                this.m_symbolName = value;
            }
        }
        
        /// <remarks/>
        public string ParameterIdentifier {
            get {
                return this.m_parameterIdentifier;
            }
            set {
                this.m_parameterIdentifier = value;
            }
        }
        
        /// <remarks/>
        public string ParameterValue {
            get {
                return this.m_parameterValue;
            }
            set {
                this.m_parameterValue = value;
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
    public enum SizeContextType {
        
        /// <remarks/>
        MappingUnits,
        
        /// <remarks/>
        DeviceUnits,
    }
    
    /// <remarks/>
    public class PointTypeStyleType {
        
        private bool m_displayAsText;
        
        private bool m_allowOverpost;
        
        private PointRuleTypeCollection m_pointRule;
        
        private ExtendedDataType m_extendedData1;
        
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
    public class PointRuleType {
        
        private string m_legendLabel;
        
        private string m_filter;
        
        private TextSymbolType m_label;
        
        private PointSymbolization2DType m_item;
        
        private ExtendedDataType m_extendedData1;
        
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
        
        private ExtendedDataType m_extendedData1;

		private bool m_backgroundStyleSpecified;
        
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
		public System.Drawing.Color ForegroundColor 
		{
			get 
			{
				return this.m_foregroundColor == null ? System.Drawing.Color.Black : Utility.ParseHTMLColor(this.m_foregroundColor);
			}
			set 
			{
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
			get 
			{
				return this.m_backgroundColor == null ? System.Drawing.Color.Transparent : Utility.ParseHTMLColor(this.m_backgroundColor);
			}
			set 
			{
				this.m_backgroundColor = Utility.SerializeHTMLColor(value, false);
			}
		}
        
        /// <remarks/>
        public BackgroundStyleType BackgroundStyle 
		{
            get {
                return this.m_backgroundStyle;
            }
            set {
                this.m_backgroundStyle = value;
            }
        }
        
		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool BackgroundStyleSpecified 
		{
			get 
			{
				return this.m_backgroundStyleSpecified;
			}
			set 
			{
				this.m_backgroundStyleSpecified = value;
			}
		}

        /// <remarks/>
        public string HorizontalAlignment 
		{
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MarkSymbolType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(W2DSymbolType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BlockSymbolType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TextSymbolType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FontSymbolType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImageSymbolType))]
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

		private bool m_insertionPointXSpecified;

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

		/// <remarks/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool InsertionPointXSpecified 
		{
			get 
			{
				return this.m_insertionPointXSpecified;
			}
			set 
			{
				this.m_insertionPointXSpecified = value;
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
    public class MarkSymbolType : SymbolType {
        
        private ShapeType m_shape;
        
        private FillType m_fill;
        
        private StrokeType m_edge;
        
        private ExtendedDataType m_extendedData1;
        
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
        
        private ExtendedDataType m_extendedData1;

		public FillType()
		{
			m_fillPattern = null;
		}
        
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
			get { return this.m_foregroundColor; }
			set { this.m_foregroundColor = value; }
		}


		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public System.Drawing.Color ForegroundColor 
		{
			get 
			{
				return this.m_foregroundColor == null ? System.Drawing.Color.Black : Utility.ParseHTMLColor(this.m_foregroundColor);
			}
			set 
			{
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
			get 
			{
				return this.m_backgroundColor == null ? System.Drawing.Color.Transparent : Utility.ParseHTMLColor(this.m_backgroundColor);
			}
			set 
			{
				this.m_backgroundColor = Utility.SerializeHTMLColor(value, false);
			}
		}
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 
		{
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public class StrokeType {
        
        private string m_lineStyle;
        
        private string m_thickness;
        
        private string m_color;
        
        private LengthUnitType m_unit;
        
        private SizeContextType m_sizeContext;
        
        private ExtendedDataType m_extendedData1;

		/// <summary>
		/// Default constructor
		/// </summary>
		public StrokeType()
		{
			m_lineStyle = null;
			m_thickness = "10";
			m_color = Utility.SerializeHTMLColor(System.Drawing.Color.FromArgb(0,0,0), false);
			m_unit = LengthUnitType.Points;
			m_sizeContext = SizeContextType.DeviceUnits;
		}
        
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
		public string ColorAsHTML
		{
			get { return this.m_color; }
			set { this.m_color = value; }
		}


		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public System.Drawing.Color Color 
		{
			get 
			{
				return Utility.ParseHTMLColor(this.m_color);
			}
			set 
			{
				this.m_color = Utility.SerializeHTMLColor(value, false);
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
    public class W2DSymbolType : SymbolType {
        
        private W2DSymbolTypeW2DSymbol m_w2DSymbol;
        
        private string m_fillColor;
        
        private string m_lineColor;
        
        private string m_textColor;
        
        private ExtendedDataType m_extendedData1;
        
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
    public class W2DSymbolTypeW2DSymbol {
        
		//TODO: Why were these "object" ?
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
    public class BlockSymbolType : SymbolType {
        
        private string m_drawingName;
        
        private string m_blockName;
        
        private string m_blockColor;
        
        private string m_layerColor;
        
        private ExtendedDataType m_extendedData1;
        
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
        
        private ExtendedDataType m_extendedData1;
        
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
        
		[System.Xml.Serialization.XmlElementAttribute("ForegroundColor")]
		public string ForegroundColorAsHTML
		{
			get { return this.m_foregroundColor; }
			set { this.m_foregroundColor = value; }
		}


		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public System.Drawing.Color ForegroundColor 
		{
			get 
			{
				return this.m_foregroundColor == null ? System.Drawing.Color.Black : Utility.ParseHTMLColor(this.m_foregroundColor);
			}
			set 
			{
				this.m_foregroundColor = Utility.SerializeHTMLColor(value, false);
			}
		}
        
        /// <remarks/>
        public ExtendedDataType ExtendedData1 
		{
            get {
                return this.m_extendedData1;
            }
            set {
                this.m_extendedData1 = value;
            }
        }
    }
    
    /// <remarks/>
    public class ImageSymbolType : SymbolType {
        
        private object m_item;
        
        private ExtendedDataType m_extendedData1;
        
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
    public class PointSymbolization2DType {
        
        private SymbolType m_item;
        
        private ExtendedDataType m_extendedData1;
        
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
    public class LineTypeStyleType {
        
        private LineRuleTypeCollection m_lineRule;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("LineRule")]
        public LineRuleTypeCollection LineRule {
            get {
                return this.m_lineRule;
            }
            set {
                this.m_lineRule = value;
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
    public class LineRuleType {
        
        private string m_legendLabel;
        
        private string m_filter;
        
        private TextSymbolType m_label;
        
        private StrokeTypeCollection m_items;
        
        private ExtendedDataType m_extendedData1;
        
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
    public class AreaTypeStyleType {
        
        private AreaRuleTypeCollection m_areaRule;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AreaRule")]
        public AreaRuleTypeCollection AreaRule {
            get {
                return this.m_areaRule;
            }
            set {
                this.m_areaRule = value;
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
    public class AreaRuleType {
        
        private string m_legendLabel;
        
        private string m_filter;
        
        private TextSymbolType m_label;
        
        private AreaSymbolizationFillType m_item;
        
        private ExtendedDataType m_extendedData1;

		public AreaRuleType()
		{
			m_legendLabel = null;
			m_filter = null;
			m_label = null;
			m_item = null;
			m_extendedData1 = null;
		}
        
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
    public class AreaSymbolizationFillType : AreaSymbolizationType {
        
        private FillType m_fill;
        
        private StrokeType m_stroke;
        
        private ExtendedDataType m_extendedData1;
        
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AreaSymbolizationFillType))]
    public class AreaSymbolizationType {
    }
    
    /// <remarks/>
    public class ElevationSettingsType {
        
        private string m_zOffset;
        
        private string m_zExtrusion;
        
        private ElevationTypeType m_zOffsetType;
        
        private bool m_zOffsetTypeSpecified;
        
        private LengthUnitType m_unit;
        
        private bool m_unitSpecified;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        public string ZOffset {
            get {
                return this.m_zOffset;
            }
            set {
                this.m_zOffset = value;
            }
        }
        
        /// <remarks/>
        public string ZExtrusion {
            get {
                return this.m_zExtrusion;
            }
            set {
                this.m_zExtrusion = value;
            }
        }
        
        /// <remarks/>
        public ElevationTypeType ZOffsetType {
            get {
                return this.m_zOffsetType;
            }
            set {
                this.m_zOffsetType = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ZOffsetTypeSpecified {
            get {
                return this.m_zOffsetTypeSpecified;
            }
            set {
                this.m_zOffsetTypeSpecified = value;
            }
        }
        
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UnitSpecified {
            get {
                return this.m_unitSpecified;
            }
            set {
                this.m_unitSpecified = value;
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
    public enum ElevationTypeType {
        
        /// <remarks/>
        RelativeToGround,
        
        /// <remarks/>
        Absolute,
    }
    
    /// <remarks/>
    public class GridLayerDefinitionType : BaseLayerDefinitionType {
        
        private string m_featureName;
        
        private string m_geometry;
        
        private string m_filter;
        
        private GridScaleRangeTypeCollection m_gridScaleRange;
        
        private ExtendedDataType m_extendedData1;
        
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
    public class GridScaleRangeType {
        
        private System.Double m_minScale;
        
        private bool m_minScaleSpecified;
        
        private System.Double m_maxScale;
        
        private bool m_maxScaleSpecified;
        
        private GridSurfaceStyleType m_surfaceStyle;
        
        private GridColorStyleType m_colorStyle;
        
        private System.Double m_rebuildFactor;
        
        private ExtendedDataType m_extendedData1;
        
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
        public GridSurfaceStyleType SurfaceStyle {
            get {
                return this.m_surfaceStyle;
            }
            set {
                this.m_surfaceStyle = value;
            }
        }
        
        /// <remarks/>
        public GridColorStyleType ColorStyle {
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
    public class GridSurfaceStyleType {
        
        private string m_band;
        
        private System.Double m_zeroValue;
        
        private bool m_zeroValueSpecified;
        
        private System.Double m_scaleFactor;
        
        private bool m_scaleFactorSpecified;
        
        private string m_defaultColor;
        
        private ExtendedDataType m_extendedData1;
        
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
        
        /// <remarks/>
        public string DefaultColor {
            get {
                return this.m_defaultColor;
            }
            set {
                this.m_defaultColor = value;
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
    public class GridColorStyleType {
        
        private HillShadeType m_hillShade;
        
        private object m_transparencyColor;
        
        private System.Double m_brightnessFactor;
        
        private bool m_brightnessFactorSpecified;
        
        private System.Double m_contrastFactor;
        
        private bool m_contrastFactorSpecified;
        
        private GridColorRuleTypeCollection m_colorRule;
        
        private ExtendedDataType m_extendedData1;
        
        /// <remarks/>
        public HillShadeType HillShade {
            get {
                return this.m_hillShade;
            }
            set {
                this.m_hillShade = value;
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
    public class HillShadeType {
        
        private string m_band;
        
        private System.Double m_azimuth;
        
        private System.Double m_altitude;
        
        private System.Double m_scaleFactor;
        
        private bool m_scaleFactorSpecified;
        
        private ExtendedDataType m_extendedData1;
        
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
    public class GridColorRuleType {
        
        private string m_legendLabel;
        
        private string m_filter;
        
        private TextSymbolType m_label;
        
        private GridColorType m_color;
        
        private ExtendedDataType m_extendedData1;
        
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
    public class GridColorType {
        
        private object m_item;
        
        private ItemChoiceType m_itemElementName;
        
        private ExtendedDataType m_extendedData1;
        
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
        
        private ExtendedDataType m_extendedData1;
        
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
    [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    public enum ItemChoiceType {
        
        /// <remarks/>
        Bands,
        
        /// <remarks/>
        Band,
        
        /// <remarks/>
        ExplicitColor,
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
    
    public class XmlElementCollection : System.Collections.CollectionBase {
        
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
    
    
    public class CompositeRuleCollection : System.Collections.CollectionBase {
        
        public CompositeRule this[int idx] {
            get {
                return ((CompositeRule)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(CompositeRule value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class SymbolInstanceCollection : System.Collections.CollectionBase {
        
        public SymbolInstance this[int idx] {
            get {
                return ((SymbolInstance)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(SymbolInstance value) {
            return base.InnerList.Add(value);
        }
    }
    
    public class OverrideCollection : System.Collections.CollectionBase {
        
        public Override this[int idx] {
            get {
                return ((Override)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(Override value) {
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
    
    public class LineRuleTypeCollection : System.Collections.CollectionBase {
        
        public LineRuleType this[int idx] {
            get {
                return ((LineRuleType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(LineRuleType value) {
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
    
    public class AreaRuleTypeCollection : System.Collections.CollectionBase {
        
        public AreaRuleType this[int idx] {
            get {
                return ((AreaRuleType)(base.InnerList[idx]));
            }
            set {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(AreaRuleType value) {
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
}
*/