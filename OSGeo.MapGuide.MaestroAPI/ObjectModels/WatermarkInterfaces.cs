#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.ComponentModel;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.ObjectModels.Common;

namespace OSGeo.MapGuide.ObjectModels.WatermarkDefinition
{
    /// <summary>
    /// The allowed length units for a watermark position
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")] //NOXLATE
    [System.SerializableAttribute()]
    public enum UnitType {
        
        /// <remarks/>
        Inches,
        
        /// <remarks/>
        Centimeters,
        
        /// <remarks/>
        Millimeters,
        
        /// <remarks/>
        Pixels,
        
        /// <remarks/>
        Points,
    }
    
    /// <summary>
    /// The allowed horizontal alignment values for a watermark position
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")] //NOXLATE
    [System.SerializableAttribute()]
    public enum HorizontalAlignmentType {
        
        /// <remarks/>
        Left,
        
        /// <remarks/>
        Center,
        
        /// <remarks/>
        Right,
    }

    /// <summary>
    /// The context in which the watermark is displayed
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")] //NOXLATE
    [System.SerializableAttribute()]
    public enum UsageType {
        
        /// <remarks/>
        WMS,
        
        /// <remarks/>
        Viewer,
        
        /// <remarks/>
        All,
    }
    
    /// <summary>
    /// The allowed vertical alignments for a watermark position
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.4.0.38968")] //NOXLATE
    [System.SerializableAttribute()]
    public enum VerticalAlignmentType {
        
        /// <remarks/>
        Top,
        
        /// <remarks/>
        Center,
        
        /// <remarks/>
        Bottom,
    }
    
    /// <summary>
    /// A watermark definition containing content, appearance and position information
    /// </summary>
    public interface IWatermarkDefinition : IResource, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a symbol definition defining the content of the watermark
        /// </summary>
        ISymbolDefinitionBase Content { get; set; }

        /// <summary>
        /// Gets or sets the appearance of the watermark
        /// </summary>
        IWatermarkAppearance Appearance { get; set; }

        /// <summary>
        /// Gets or sets the position of the watermark
        /// </summary>
        IPosition Position { get; set; }

        /// <summary>
        /// Creates the XY position.
        /// </summary>
        /// <returns></returns>
        IXYPosition CreateXYPosition();

        /// <summary>
        /// Creates the tile position.
        /// </summary>
        /// <returns></returns>
        ITilePosition CreateTilePosition();

        /// <summary>
        /// Gets the version of the Map Definition that can take this watermark
        /// </summary>
        Version SupportedMapDefinitionVersion { get; }

        /// <summary>
        /// Gets the version of the Layer Definition that can take this watermark
        /// </summary>
        Version SupportedLayerDefinitionVersion { get; }
    }

    /// <summary>
    /// Defines the appearance of a watermark.
    /// </summary>
    public interface IWatermarkAppearance : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the transparency of the watermark in the range 0-100.  The default value is 0 (opaque)
        /// </summary>
        double Transparency { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the watermark, in degrees, in the range 0-360.  The default value is 0
        /// </summary>
        double Rotation { get; set; }
    }

    /// <summary>
    /// Defines the type of watermark position
    /// </summary>
    public enum PositionType
    {
        /// <summary>
        /// X/Y based position
        /// </summary>
        XY,
        /// <summary>
        /// Tile-based position
        /// </summary>
        Tile
    }

    /// <summary>
    /// Abstract base type used with all watermark positions
    /// </summary>
    public interface IPosition : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the type of watermark position
        /// </summary>
        PositionType Type { get; }
    }

    /// <summary>
    /// Positions a watermark at a single X/Y location
    /// </summary>
    public interface IXYPosition : IPosition
    {
        /// <summary>
        /// Gets or sets the position along the X-axis
        /// </summary>
        IHorizontalPosition XPosition { get; set; }

        /// <summary>
        /// Gets or sets the position along the Y-axis
        /// </summary>
        IVerticalPosition YPosition { get; set; }
    }

    /// <summary>
    /// Positions a watermark according to a regular grid
    /// </summary>
    public interface ITilePosition : IPosition
    {
        /// <summary>
        /// Gets or sets the width of each tile in the grid
        /// </summary>
        double TileWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of each tile in the grid
        /// </summary>
        double TileHeight { get; set; }

        /// <summary>
        /// Gets or sets the horizontal position of the watermark within a tile
        /// </summary>
        IHorizontalPosition HorizontalPosition { get; set; }

        /// <summary>
        /// Gets or sets the vertical position of the watermark within a tile
        /// </summary>
        IVerticalPosition VerticalPosition { get; set; }
    }

    /// <summary>
    /// Represents the horizontal position of a watermark
    /// </summary>
    public interface IHorizontalPosition : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the horizontal offset for the position
        /// </summary>
        double Offset { get; set; }

        /// <summary>
        /// Gets or sets the unit of the offset
        /// </summary>
        UnitType Unit { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment for the position
        /// </summary>
        HorizontalAlignmentType Alignment { get; set; }
    }

    /// <summary>
    /// Defines the vertical position of a watermark
    /// </summary>
    public interface IVerticalPosition : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the vertical offset for the position
        /// </summary>
        double Offset { get; set; }

        /// <summary>
        /// Gets or sets the unit of the offset
        /// </summary>
        UnitType Unit { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment for the position
        /// </summary>
        VerticalAlignmentType Alignment { get; set; }
    }

    /// <summary>
    /// Defines a collection of <see cref="IWatermark"/> instances
    /// </summary>
    public interface IWatermarkCollection
    {
        /// <summary>
        /// Gets the watermarks used by this map definition
        /// </summary>
        IEnumerable<IWatermark> Watermarks { get; }

        /// <summary>
        /// Adds a watermark
        /// </summary>
        /// <param name="watermarkDef">The watermark definition to add</param>
        /// <returns>The added watermark instance.</returns>
        IWatermark AddWatermark(IWatermarkDefinition watermarkDef);

        /// <summary>
        /// Removes the specified watermark
        /// </summary>
        /// <param name="watermark"></param>
        void RemoveWatermark(IWatermark watermark);

        /// <summary>
        /// Gets the number of watermarks used by this map definition
        /// </summary>
        int WatermarkCount { get; }
    }

    /// <summary>
    /// A watermark instance used in a map definition or layer definition
    /// </summary>
    public interface IWatermark
    {
        /// <summary>
        /// Gets or sets the name of the watermark
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a library reference to an existing WatermarkDefinition
        /// </summary>
        string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the context in which the watermark is displayed.
        /// </summary>
        UsageType Usage { get; set; }

        /// <summary>
        /// If specified, overrides the appearance of the watermark definition
        /// </summary>
        IWatermarkAppearance AppearanceOverride { get; set; }

        /// <summary>
        /// If specified, overrides the position of the watermark definition
        /// </summary>
        IPosition PositionOverride { get; set; }

        /// <summary>
        /// Creates the default appearance.
        /// </summary>
        /// <returns></returns>
        IWatermarkAppearance CreateDefaultAppearance();

        /// <summary>
        /// Creates the default XY position.
        /// </summary>
        /// <returns></returns>
        IXYPosition CreateDefaultXYPosition();

        /// <summary>
        /// Creates the default tile position.
        /// </summary>
        /// <returns></returns>
        ITilePosition CreateDefaultTilePosition();
    }
}
