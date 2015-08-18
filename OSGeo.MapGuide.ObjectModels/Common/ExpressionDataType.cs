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


namespace OSGeo.MapGuide.ObjectModels.Common
{
    /// <summary>
    /// The types of valid FDO expressions for use in Symbol Definitions
    /// </summary>
    public enum ExpressionDataType
    {
        /// <summary>
        /// BLOB data property
        /// </summary>
        Data_Blob,

        /// <summary>
        /// Boolean data property
        /// </summary>
        Data_Boolean,

        /// <summary>
        /// Byte data property
        /// </summary>
        Data_Byte,

        /// <summary>
        /// CLOB data property
        /// </summary>
        Data_Clob,

        /// <summary>
        /// DateTime data property
        /// </summary>
        Data_DateTime,

        /// <summary>
        /// Double data property
        /// </summary>
        Data_Double,

        /// <summary>
        /// Int16 data property
        /// </summary>
        Data_Int16,

        /// <summary>
        /// Int32 data property
        /// </summary>
        Data_Int32,

        /// <summary>
        /// Int64 data property
        /// </summary>
        Data_Int64,

        /// <summary>
        /// Single data property
        /// </summary>
        Data_Single,

        /// <summary>
        /// String data property
        /// </summary>
        Data_String,

        /// <summary>
        /// Geometry property
        /// </summary>
        Geometry,

        /// <summary>
        /// Raster property
        /// </summary>
        Raster,

        /// <summary>
        /// Association property
        /// </summary>
        Association,

        /// <summary>
        /// String symbol parameter
        /// </summary>
        Sym_String,

        /// <summary>
        /// Boolean symbol parameter
        /// </summary>
        Sym_Boolean,

        /// <summary>
        /// Integer symbol parameter
        /// </summary>
        Sym_Integer,

        /// <summary>
        /// Real symbol parameter
        /// </summary>
        Sym_Real,

        /// <summary>
        /// Color symbol parameter
        /// </summary>
        Sym_Color,

        /// <summary>
        /// Angle symbol parameter
        /// </summary>
        Sym_Angle,

        /// <summary>
        /// Fill color symbol parameter
        /// </summary>
        Sym_FillColor,

        /// <summary>
        /// Line color symbol parameter
        /// </summary>
        Sym_LineColor,

        /// <summary>
        /// Line weight symbol parameter
        /// </summary>
        Sym_LineWeight,

        /// <summary>
        /// Content symbol parameter
        /// </summary>
        Sym_Content,

        /// <summary>
        /// Markup symbol parameter
        /// </summary>
        Sym_Markup,

        /// <summary>
        /// Font name symbol parameter
        /// </summary>
        Sym_FontName,

        /// <summary>
        /// Bold symbol parameter
        /// </summary>
        Sym_Bold,

        /// <summary>
        /// Italic symbol parameter
        /// </summary>
        Sym_Italic,

        /// <summary>
        /// Underlined symbol parameter
        /// </summary>
        Sym_Underlined,

        /// <summary>
        /// Overlined symbol parameter
        /// </summary>
        Sym_Overlined,

        /// <summary>
        /// Oblique angle symbol parameter
        /// </summary>
        Sym_ObliqueAngle,

        /// <summary>
        /// Track spacing symbol parameter
        /// </summary>
        Sym_TrackSpacing,

        /// <summary>
        /// Font height symbol parameter
        /// </summary>
        Sym_FontHeight,

        /// <summary>
        /// Horizontal alignment symbol parameter
        /// </summary>
        Sym_HorizontalAlignment,

        /// <summary>
        /// Vertical alignment symbol parameter
        /// </summary>
        Sym_VerticalAlignment,

        /// <summary>
        /// Justification symbol parameter
        /// </summary>
        Sym_Justification,

        /// <summary>
        /// Line spacing symbol parameter
        /// </summary>
        Sym_LineSpacing,

        /// <summary>
        /// Text color symbol parameter
        /// </summary>
        Sym_TextColor,

        /// <summary>
        /// Ghost color symbol parameter
        /// </summary>
        Sym_GhostColor,

        /// <summary>
        /// Frame line color symbol parameter
        /// </summary>
        Sym_FrameLineColor,

        /// <summary>
        /// Frame fill color symbol parameter
        /// </summary>
        Sym_FrameFillColor,

        /// <summary>
        /// Start offset symbol parameter
        /// </summary>
        Sym_StartOffset,

        /// <summary>
        /// End offset symbol parameter
        /// </summary>
        Sym_EndOffset,

        /// <summary>
        /// Repeat X symbol parameter
        /// </summary>
        Sym_RepeatX,

        /// <summary>
        /// Repeat Y symbol parameter
        /// </summary>
        Sym_RepeatY
    }
}