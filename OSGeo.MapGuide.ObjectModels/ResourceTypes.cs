#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License


namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// Represents the common resource types in MapGuide
    /// </summary>
    public enum ResourceTypes
    {
        /// <summary>
        /// Map Definition
        /// </summary>
        MapDefinition,

        /// <summary>
        /// Layer Definition
        /// </summary>
        LayerDefinition,

        /// <summary>
        /// Feature Source
        /// </summary>
        FeatureSource,

        /// <summary>
        /// Web Layout
        /// </summary>
        WebLayout,

        /// <summary>
        /// Runtime Map
        /// </summary>
        Map,

        /// <summary>
        /// Folder
        /// </summary>
        Folder,

        /// <summary>
        /// Fusion Flexible Layout
        /// </summary>
        ApplicationDefinition,

        /// <summary>
        /// Print Layout
        /// </summary>
        PrintLayout,

        /// <summary>
        /// Symbol Definition
        /// </summary>
        SymbolDefinition,

        /// <summary>
        /// Load Procedure
        /// </summary>
        LoadProcedure,

        /// <summary>
        /// Drawing Source
        /// </summary>
        DrawingSource,

        /// <summary>
        /// DWF-based Symbol Library
        /// </summary>
        SymbolLibrary,

        /// <summary>
        /// A watermark
        /// </summary>
        WatermarkDefinition,

        /// <summary>
        /// A selection for a runtime map
        /// </summary>
        Selection,

        /// <summary>
        /// A tile set definition
        /// </summary>
        TileSetDefinition
    }
}