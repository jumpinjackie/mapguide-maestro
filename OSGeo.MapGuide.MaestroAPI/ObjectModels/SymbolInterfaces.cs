#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace OSGeo.MapGuide.ObjectModels.SymbolDefinition
{
    /// <summary>
    /// Defines the type of symbol instances
    /// </summary>
    public enum SymbolInstanceType
    {
        /// <summary>
        /// A library reference to an existing symbol definition
        /// </summary>
        Reference,
        /// <summary>
        /// An inline simple symbol definition
        /// </summary>
        Simple,
        /// <summary>
        /// An inline compound symbol definition
        /// </summary>
        Compound
    }

    /// <summary>
    /// Represents a symbol instance used for advanced cartographic stylization
    /// </summary>
    public interface ISymbolInstance
    {
        /// <summary>
        /// Gets or sets the symbol reference
        /// </summary>
        ISymbolInstanceReference Reference { get; set; }

        /// <summary>
        /// Gets the parameter overrides for this symbol
        /// </summary>
        IParameterOverrideCollection ParameterOverrides { get; }

        /// <summary>
        /// Gets or sets the X scale
        /// </summary>
        double? ScaleX { get; set; }

        /// <summary>
        /// Gets or sets the Y scale
        /// </summary>
        double? ScaleY { get; set; }

        /// <summary>
        /// Gets or sets the X insertion offset
        /// </summary>
        double? InsertionOffsetX { get; set; }

        /// <summary>
        /// Gets or sets the Y insertion offset
        /// </summary>
        double? InsertionOffsetY { get; set; }

        /// <summary>
        /// Gets or sets the size context
        /// </summary>
        SizeContextType SizeContext { get; set; }

        /// <summary>
        /// Gets or sets whether to draw this instance last
        /// </summary>
        bool? DrawLast { get; set; }

        /// <summary>
        /// Gets or sets whether to check the exclusion region
        /// </summary>
        bool? CheckExclusionRegion { get; set; }

        /// <summary>
        /// Gets or sets whether to add this instance to the exclusion region
        /// </summary>
        bool? AddToExclusionRegion { get; set; }

        /// <summary>
        /// Gets or sets the position algorithm
        /// </summary>
        string PositionAlgorithm { get; set; }
    }

    /// <summary>
    /// Represents a symbol instance reference
    /// </summary>
    public interface ISymbolInstanceReference
    {
        /// <summary>
        /// Gets the type
        /// </summary>
        SymbolInstanceType Type { get; }
    }

    /// <summary>
    /// Represents a symbol instance reference by a resource id
    /// </summary>
    public interface ISymbolLibraryReference : ISymbolInstanceReference
    {
        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        /// <value>The resource id.</value>
        string ResourceId { get; set; }
    }

    /// <summary>
    /// Represents a symbol instance reference by a inline definition
    /// </summary>
    public interface IInlineSimpleSymbolReference : ISymbolInstanceReference
    {
        /// <summary>
        /// Gets or sets the inline definition
        /// </summary>
        ISimpleSymbolDefinition SimpleSymbolDefinition { get; set; } 
    }

    /// <summary>
    /// Represents a symbol instance reference by a inline definition
    /// </summary>
    public interface IInlineCompoundSymbolReference : ISymbolInstanceReference
    {
        /// <summary>
        /// Gets or sets the inline definition
        /// </summary>
        ICompoundSymbolDefinition CompoundSymbolDefinition { get; set; }
    }

    #region Symbol Definition 1.0.0 interfaces

    /// <summary>
    /// Base interface of all symbol definitions
    /// </summary>
    public interface ISymbolDefinitionBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }
    }

    /// <summary>
    /// Represents a simple symbol definition
    /// </summary>
    public interface ISimpleSymbolDefinition : ISymbolDefinitionBase
    {
        /// <summary>
        /// Gets the graphics.
        /// </summary>
        /// <value>The graphics.</value>
        IEnumerable<IGraphics> Graphics { get; }

        /// <summary>
        /// Adds the graphics.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        void AddGraphics(IGraphics graphics);

        /// <summary>
        /// Removes the graphics.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        void RemoveGraphics(IGraphics graphics);

        /// <summary>
        /// Gets or sets the resize box.
        /// </summary>
        /// <value>The resize box.</value>
        IResizeBox ResizeBox { get; set; }

        /// <summary>
        /// Gets or sets the point usage.
        /// </summary>
        /// <value>The point usage.</value>
        IPointUsage PointUsage { get; set; }

        /// <summary>
        /// Gets or sets the line usage.
        /// </summary>
        /// <value>The line usage.</value>
        ILineUsage LineUsage { get; set; }

        /// <summary>
        /// Gets or sets the area usage.
        /// </summary>
        /// <value>The area usage.</value>
        IAreaUsage AreaUsage { get; set; }

        /// <summary>
        /// Gets the parameter definition.
        /// </summary>
        /// <value>The parameter definition.</value>
        IParameterDefinition ParameterDefinition { get; }
    }

    /// <summary>
    /// Represents a compound symbol definition
    /// </summary>
    public interface ICompoundSymbolDefinition : ISymbolDefinitionBase
    {
        /// <summary>
        /// Gets the simple symbols.
        /// </summary>
        /// <value>The simple symbols.</value>
        IEnumerable<ISimpleSymbolReferenceBase> SimpleSymbol { get; }

        /// <summary>
        /// Adds the simple symbol.
        /// </summary>
        /// <param name="sym">The sym.</param>
        void AddSimpleSymbol(ISimpleSymbolReferenceBase sym);

        /// <summary>
        /// Removes the simple symbol.
        /// </summary>
        /// <param name="sym">The sym.</param>
        void RemoveSimpleSymbol(ISimpleSymbolReferenceBase sym);
    }

    /// <summary>
    /// A collection of graphic elements
    /// </summary>
    public interface IGraphics
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <value>The elements.</value>
        IEnumerable<IGraphicBase> Elements { get; }

        /// <summary>
        /// Adds the graphic element.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        void AddGraphicElement(IGraphicBase graphics);

        /// <summary>
        /// Removes the graphic element.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        void RemoveGraphicElement(IGraphicBase graphics);
    }

    /// <summary>
    /// Defines a resize box used with SimpleSymbolDefinitions
    /// </summary>
    public interface IResizeBox
    {
        /// <summary>
        /// Gets or sets initial width of the resize box, in mm.  This must be greater than or equal to zero
        /// </summary>
        double? SizeX { get; set; }

        /// <summary>
        /// Gets or sets initial height of the resize box, in mm.  This must be greater than or equal to zero
        /// </summary>
        double? SizeY { get; set; }

        /// <summary>
        /// Gets or sets initial x-coordinate of the resize box center, in mm
        /// </summary>
        double? PositionX { get; set; }

        /// <summary>
        /// Gets or sets initial y-coordinate of the resize box center, in mm
        /// </summary>
        double? PositionY { get; set; }

        /// <summary>
        /// Gets or sets how the resize box grows in size.  This must evaluate to one of: GrowInX, GrowInY, GrowInXY, or GrowInXYMaintainAspect (default).
        /// </summary>
        string GrowControl { get; set; }
    }

    /// <summary>
    /// Base usage interface
    /// </summary>
    public interface IUsageBase
    {
        /// <summary>
        /// Specifies how the symbol angle is defined.  This must evaluate to one of: FromAngle (default) or FromGeometry
        /// </summary>
        string AngleControl { get; set; }

        /// <summary>
        /// Specifies the symbol angle, in degrees.  Only applies if AngleControl evaluates to FromAngle.  Defaults to 0
        /// </summary>
        double? Angle { get; set; }
    }

    /// <summary>
    /// Specifies how a symbol is used in the context of point features
    /// </summary>
    public interface IPointUsage : IUsageBase
    {
        /// <summary>
        /// Specifies the horizontal offset to apply to the symbol origin, in mm.  This offset is applied before the symbol is scaled and rotated.  Defaults to 0
        /// </summary>
        double OriginOffsetX { get; set; }

        /// <summary>
        /// Specifies the vertical offset to apply to the symbol origin, in mm.  This offset is applied before the symbol is scaled and rotated.  Defaults to 0
        /// </summary>
        double OriginOffsetY { get; set; }
    }

    /// <summary>
    /// Represents line usage
    /// </summary>
    public interface ILineUsage : IUsageBase
    {
        /// <summary>
        /// Gets or sets the units control.
        /// </summary>
        /// <value>The units control.</value>
        string UnitsControl { get; set; }

        /// <summary>
        /// Gets or sets the vertex control.
        /// </summary>
        /// <value>The vertex control.</value>
        string VertexControl { get; set; }

        /// <summary>
        /// Gets or sets the start offset.
        /// </summary>
        /// <value>The start offset.</value>
        double? StartOffset { get; set; }

        /// <summary>
        /// Gets or sets the end offset.
        /// </summary>
        /// <value>The end offset.</value>
        double? EndOffset { get; set; }

        /// <summary>
        /// Gets or sets the repeat value
        /// </summary>
        /// <value>The repeat value.</value>
        double? Repeat { get; set; }

        /// <summary>
        /// Gets or sets the vertex angle limit.
        /// </summary>
        /// <value>The vertex angle limit.</value>
        double? VertexAngleLimit { get; set; }

        /// <summary>
        /// Gets or sets the vertex join.
        /// </summary>
        /// <value>The vertex join.</value>
        string VertexJoin { get; set; }

        /// <summary>
        /// Gets or sets the vertex miter limit.
        /// </summary>
        /// <value>The vertex miter limit.</value>
        string VertexMiterLimit { get; set; }

        /// <summary>
        /// Gets or sets the default path.
        /// </summary>
        /// <value>The default path.</value>
        IPath DefaultPath { get; set; }
    }

    /// <summary>
    /// Defines area usage
    /// </summary>
    public interface IAreaUsage : IUsageBase
    {
        /// <summary>
        /// Gets or sets the origin control.
        /// </summary>
        /// <value>The origin control.</value>
        string OriginControl { get; set; }

        /// <summary>
        /// Gets or sets the clipping control.
        /// </summary>
        /// <value>The clipping control.</value>
        string ClippingControl { get; set; }

        /// <summary>
        /// Gets or sets the X origin.
        /// </summary>
        /// <value>The X origin.</value>
        double? OriginX { get; set; }

        /// <summary>
        /// Gets or sets the Y origin.
        /// </summary>
        /// <value>The Y origin.</value>
        double? OriginY { get; set; }

        /// <summary>
        /// Gets or sets the X repeat value.
        /// </summary>
        /// <value>The X repeat value.</value>
        double? RepeatX { get; set; }

        /// <summary>
        /// Gets or sets the Y repeat value
        /// </summary>
        /// <value>The Y repeat value.</value>
        double? RepeatY { get; set; }

        /// <summary>
        /// Gets or sets the width of the buffer.
        /// </summary>
        /// <value>The width of the buffer.</value>
        double? BufferWidth { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.3.0.33572")]
    [System.SerializableAttribute()]
    public enum DataType
    {

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

    /// <summary>
    /// 
    /// </summary>
    [System.SerializableAttribute()]
    public enum DataType2
    {

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

    /// <summary>
    /// Defines a parameter
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        DataType DataType { get; set; }
    }

    /// <summary>
    /// A parameter definition
    /// </summary>
    public interface IParameterDefinition
    {
        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        IEnumerable<IParameter> Parameter { get; }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="param">The param.</param>
        void AddParameter(IParameter param);

        /// <summary>
        /// Removes the parameter.
        /// </summary>
        /// <param name="param">The param.</param>
        void RemoveParameter(IParameter param);
    }

    /// <summary>
    /// Represents a simple symbol reference
    /// </summary>
    public interface ISimpleSymbolReferenceBase
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        SimpleSymbolReferenceType Type { get; }

        /// <summary>
        /// Gets or sets the rendering pass.
        /// </summary>
        /// <value>The rendering pass.</value>
        string RenderingPass { get; set; }
    }

    /// <summary>
    /// Represents a simple symbol reference by resource id
    /// </summary>
    public interface ISimpleSymbolLibraryReference : ISimpleSymbolReferenceBase
    {
        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        /// <value>The resource id.</value>
        string ResourceId { get; set; }
    }

    /// <summary>
    /// Represents a simple symbol reference by inline definition
    /// </summary>
    public interface ISimpleSymbolInlineReference : ISimpleSymbolReferenceBase
    {
        /// <summary>
        /// Gets or sets the simple symbol definition.
        /// </summary>
        /// <value>The simple symbol definition.</value>
        ISimpleSymbolDefinition SimpleSymbolDefinition { get; set; }
    }

    /// <summary>
    /// The type of simple symbol reference
    /// </summary>
    public enum SimpleSymbolReferenceType
    {
        /// <summary>
        /// External resource id reference
        /// </summary>
        Library,
        /// <summary>
        /// Inlined definition
        /// </summary>
        Inline
    }

    /// <summary>
    /// Represents the base interface of all graphics
    /// </summary>
    public interface IGraphicBase
    {
        /// <summary>
        /// Gets or sets the resize control.
        /// </summary>
        /// <value>The resize control.</value>
        string ResizeControl { get; set; }
    }

    /// <summary>
    /// A path
    /// </summary>
    public interface IPath : IGraphicBase
    {
        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        /// <value>The geometry.</value>
        string Geometry { get; set; }

        /// <summary>
        /// Gets or sets the color of the fill.
        /// </summary>
        /// <value>The color of the fill.</value>
        string FillColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the line.
        /// </summary>
        /// <value>The color of the line.</value>
        string LineColor { get; set; }

        /// <summary>
        /// Gets or sets the line weight.
        /// </summary>
        /// <value>The line weight.</value>
        double? LineWeight { get; set; }

        /// <summary>
        /// Gets or sets the line weight scalable.
        /// </summary>
        /// <value>The line weight scalable.</value>
        bool? LineWeightScalable { get; set; }

        /// <summary>
        /// Gets or sets the line cap.
        /// </summary>
        /// <value>The line cap.</value>
        string LineCap { get; set; }

        /// <summary>
        /// Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        string LineJoin { get; set; }

        /// <summary>
        /// Gets or sets the line miter limit.
        /// </summary>
        /// <value>The line miter limit.</value>
        double? LineMiterLimit { get; set; }
    }

    /// <summary>
    /// An image reference
    /// </summary>
    public interface IImageReference
    {
        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        /// <value>The resource id.</value>
        string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the library item.
        /// </summary>
        /// <value>The name of the library item.</value>
        string LibraryItemName { get; set; }
    }

    /// <summary>
    /// The type of image
    /// </summary>
    public enum ImageType
    {
        /// <summary>
        /// 
        /// </summary>
        Reference,
        /// <summary>
        /// 
        /// </summary>
        Inline
    }

    /// <summary>
    /// An image graphic
    /// </summary>
    public interface IImageBase : IGraphicBase
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        ImageType Type { get; }

        /// <summary>
        /// Gets or sets the size X.
        /// </summary>
        /// <value>The size X.</value>
        double? SizeX { get; set; }

        /// <summary>
        /// Gets or sets the size Y.
        /// </summary>
        /// <value>The size Y.</value>
        double? SizeY { get; set; }

        /// <summary>
        /// Gets or sets the size scalable.
        /// </summary>
        /// <value>The size scalable.</value>
        bool? SizeScalable { get; set; }

        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        /// <value>The angle.</value>
        double? Angle { get; set; }

        /// <summary>
        /// Gets or sets the position X.
        /// </summary>
        /// <value>The position X.</value>
        double? PositionX { get; set; }

        /// <summary>
        /// Gets or sets the position Y.
        /// </summary>
        /// <value>The position Y.</value>
        double? PositionY { get; set; }
    }

    /// <summary>
    /// Represents an inline image
    /// </summary>
    public interface IInlineImage : IImageBase
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        byte[] Content { get; set; }
    }

    /// <summary>
    /// Represents an image refrence
    /// </summary>
    public interface IImageReferenceImage : IImageBase
    {
        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>The reference.</value>
        IImageReference Reference { get; set; }
    }

    /// <summary>
    /// A text frame
    /// </summary>
    public interface ITextFrame
    {
        /// <summary>
        /// Gets or sets the color of the line.
        /// </summary>
        /// <value>The color of the line.</value>
        string LineColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the fill.
        /// </summary>
        /// <value>The color of the fill.</value>
        string FillColor { get; set; }

        /// <summary>
        /// Gets or sets the offset X.
        /// </summary>
        /// <value>The offset X.</value>
        double? OffsetX { get; set; }

        /// <summary>
        /// Gets or sets the offset Y.
        /// </summary>
        /// <value>The offset Y.</value>
        double? OffsetY { get; set; }
    }

    /// <summary>
    /// Text graphics
    /// </summary>
    public interface IText : IGraphicBase
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        string Content { get; set; }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        string FontName { get; set; }

        /// <summary>
        /// Gets or sets the bold.
        /// </summary>
        /// <value>The bold.</value>
        bool? Bold { get; set; }

        /// <summary>
        /// Gets or sets the italic.
        /// </summary>
        /// <value>The italic.</value>
        bool? Italic { get; set; }

        /// <summary>
        /// Gets or sets the underlined.
        /// </summary>
        /// <value>The underlined.</value>
        bool? Underlined { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        double? Height { get; set; }

        /// <summary>
        /// Gets or sets the height scalable.
        /// </summary>
        /// <value>The height scalable.</value>
        bool? HeightScalable { get; set; }

        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        /// <value>The angle.</value>
        double? Angle { get; set; }

        /// <summary>
        /// Gets or sets the position X.
        /// </summary>
        /// <value>The position X.</value>
        double? PositionX { get; set; }

        /// <summary>
        /// Gets or sets the position Y.
        /// </summary>
        /// <value>The position Y.</value>
        double? PositionY { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment.
        /// </summary>
        /// <value>The horizontal alignment.</value>
        string HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment.
        /// </summary>
        /// <value>The vertical alignment.</value>
        string VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the justification.
        /// </summary>
        /// <value>The justification.</value>
        string Justification { get; set; }

        /// <summary>
        /// Gets or sets the line spacing.
        /// </summary>
        /// <value>The line spacing.</value>
        double? LineSpacing { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        string TextColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the ghost.
        /// </summary>
        /// <value>The color of the ghost.</value>
        string GhostColor { get; set; }

        /// <summary>
        /// Gets or sets the frame.
        /// </summary>
        /// <value>The frame.</value>
        ITextFrame Frame { get; set; }
    }

    /// <summary>
    /// The possible values for grow control
    /// </summary>
    public enum GrowControl
    {
        /// <summary>
        /// 
        /// </summary>
        GrowInX,
        /// <summary>
        /// 
        /// </summary>
        GrowInY,
        /// <summary>
        /// 
        /// </summary>
        GrowInXY,
        /// <summary>
        /// 
        /// </summary>
        GrowInXYMaintainAspect,
    }

    /// <summary>
    /// The types of angle control
    /// </summary>
    public enum AngleControl
    {
        /// <summary>
        /// 
        /// </summary>
        FromAngle,
        /// <summary>
        /// 
        /// </summary>
        FromGeometry,
    }

    /// <summary>
    /// The types of units control
    /// </summary>
    public enum UnitsControl
    {
        /// <summary>
        /// 
        /// </summary>
        Absolute,
        /// <summary>
        /// 
        /// </summary>
        Parametric,
    }

    /// <summary>
    /// The types of vertex control
    /// </summary>
    public enum VertexControl
    {
        /// <summary>
        /// 
        /// </summary>
        OverlapNone,
        /// <summary>
        /// 
        /// </summary>
        OverlapDirect,
        /// <summary>
        /// 
        /// </summary>
        OverlapNoWrap,
        /// <summary>
        /// 
        /// </summary>
        OverlapWrap,
    }

    /// <summary>
    /// The types of vertex join
    /// </summary>
    public enum VertexJoin
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Bevel,
        /// <summary>
        /// 
        /// </summary>
        Round,
        /// <summary>
        /// 
        /// </summary>
        Miter,
    }

    /// <summary>
    /// The types of origin control
    /// </summary>
    public enum OriginControl
    {
        /// <summary>
        /// 
        /// </summary>
        Global,
        /// <summary>
        /// 
        /// </summary>
        Local,
        /// <summary>
        /// 
        /// </summary>
        Centroid,
    }

    /// <summary>
    /// The types of clipping control
    /// </summary>
    public enum ClippingControl
    {
        /// <summary>
        /// 
        /// </summary>
        Clip,
        /// <summary>
        /// 
        /// </summary>
        Inside,
        /// <summary>
        /// 
        /// </summary>
        Overlap
    }

    /// <summary>
    /// The types of resize control
    /// </summary>
    public enum ResizeControl
    {
        /// <summary>
        /// 
        /// </summary>
        ResizeNone,
        /// <summary>
        /// 
        /// </summary>
        AddToResizeBox,
        /// <summary>
        /// 
        /// </summary>
        AdjustToResizeBox,
    }

    /// <summary>
    /// The types of line cap
    /// </summary>
    public enum LineCap
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Round,
        /// <summary>
        /// 
        /// </summary>
        Triangle,
        /// <summary>
        /// 
        /// </summary>
        Square,
    }

    /// <summary>
    /// The types of line join
    /// </summary>
    public enum LineJoin
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Bevel,
        /// <summary>
        /// 
        /// </summary>
        Round,
        /// <summary>
        /// 
        /// </summary>
        Miter,
    }

    /// <summary>
    /// The types of horizontal alignment
    /// </summary>
    public enum HorizontalAlignment
    {
        /// <summary>
        /// 
        /// </summary>
        Left,
        /// <summary>
        /// 
        /// </summary>
        Center,
        /// <summary>
        /// 
        /// </summary>
        Right,
    }

    /// <summary>
    /// The types of vertical alignment
    /// </summary>
    public enum VerticalAlignment
    {
        /// <summary>
        /// 
        /// </summary>
        Bottom,
        /// <summary>
        /// 
        /// </summary>
        Baseline,
        /// <summary>
        /// 
        /// </summary>
        Halfline,
        /// <summary>
        /// 
        /// </summary>
        Capline,
        /// <summary>
        /// 
        /// </summary>
        Top,
    }

    /// <summary>
    /// The types of justification
    /// </summary>
    public enum Justification
    {
        /// <summary>
        /// 
        /// </summary>
        Left,
        /// <summary>
        /// 
        /// </summary>
        Center,
        /// <summary>
        /// 
        /// </summary>
        Right,
        /// <summary>
        /// 
        /// </summary>
        Justified,
        /// <summary>
        /// 
        /// </summary>
        FromAlignment,
    }

    #endregion

    #region Symbol Definition 1.1.0 interfaces
    #endregion
}
