#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSGeo.MapGuide.MaestroAPI.Tile
{
    /// <summary>
    /// The default tile walking strategy
    /// </summary>
    public class DefaultTileWalker : ITileWalker
    {
        const double INCH_TO_METER = 0.0254;

        readonly TileWalkOptions _options;

        public DefaultTileWalker(TileWalkOptions options)
        {
            _options = options;
        }

        public string ResourceID => _options.ResourceID;

        struct TileGroupSet
        {
            public TileGroupSet(string groupName, int rows, int cols, int scaleIndex, int rowTileOffset, int colTileOffset)
            {
                this.GroupName = groupName;
                this.Rows = rows;
                this.Cols = cols;
                this.ScaleIndex = scaleIndex;
                this.RowTileOffset = rowTileOffset;
                this.ColTileOffset = colTileOffset;
            }

            public string GroupName { get; }

            public int ScaleIndex { get; }

            public int Rows { get; }

            public int Cols { get; }

            public int RowTileOffset { get; }

            public int ColTileOffset { get; }

            public int Total => Rows * Cols;
        }

        public TileRef[] GetTileList()
        {
            var extents = _options.Extents;
            var tileSet = _options.TileSet;
            var maxScale = tileSet.GetMaxScale();

            var metersPerUnit = _options.MetersPerUnit;

            var widthM = Math.Abs(metersPerUnit * (extents.MaxX - extents.MinX));
            var heightM = Math.Abs(metersPerUnit * (extents.MaxY - extents.MinY));

            var scaleCount = tileSet.ScaleCount;
            var groups = new List<TileGroupSet>();
            for (var scaleIndex = scaleCount - 1; scaleIndex >= 0; scaleIndex--)
            {
                var rowTileOffset = 0;
                var colTileOffset = 0;
                var scale = tileSet.GetScaleAt(scaleIndex);

                // This is the official method, and the only one MgCooker will ever use

                // This is the algorithm proposed by the MapGuide team:
                // http://www.nabble.com/Pre-Genererate--tiles-for-the-entire-map-at-all-pre-defined-zoom-scales-to6074037.html#a6078663
                //
                // Method description inline (in case nabble link disappears):
                //
                // The upper left corner of the extents of the map corresponds to tile (0,0). Then tile (1,0) is to the right of that and tile (0,1) is under tile (0,0).
                // So assuming you know the extents of your map, you can calculate how many tiles it spans at the given scale, using the following
                //
                // number of tiles x = map width in meters  / ( 0.079375 * map_scale)
                // number of tiles y = map height in meters / ( 0.079375 * map_scale)
                //
                // where 0.079375 = [inch to meter] / image DPI * tile size = 0.0254 / 96 * 300.
                //
                // This assumes you know the scale factor that converts your map width and height to meters. You can get this from the coordinate system of the map if you don't know it, but it's much easier to just plug in the number into this equation.
                //
                // Also have in mind that you can also request tiles beyond the map extent (for example tile (-1, -1), however, there is probably no point to cache them unless you have valid data outside your initial map extents.

                //The tile extent in meters
                var tileMapWidth = ((INCH_TO_METER / _options.DPI * _options.TileWidth) * (scale));
                var tileMapHeight = ((INCH_TO_METER / _options.DPI * _options.TileHeight) * (scale));

                //Using this algorithm, yields a negative number of columns/rows, if the max scale is larger than the max extent of the map.
                var rows = Math.Max(1, (int)Math.Ceiling((heightM / tileMapHeight)));
                var cols = Math.Max(1, (int)Math.Ceiling((widthM / tileMapWidth)));

                var ovExtents = _options.OverrideExtents;
                if (ovExtents != null)
                {
                    //The extent is overridden, so we need to adjust the start offsets
                    //and re-compute row/col span against the overridden extents
                    var offsetMapX = Math.Abs(metersPerUnit * (ovExtents.MinX - extents.MinX));
                    var offsetMapY = Math.Abs(metersPerUnit * (extents.MaxY - ovExtents.MaxY));
                    rowTileOffset = (int)Math.Floor(offsetMapY / tileMapHeight);
                    colTileOffset = (int)Math.Floor(offsetMapX / tileMapWidth);

                    //Re-compute rows/cols against override extent
                    widthM = Math.Abs(metersPerUnit * (ovExtents.MaxX - ovExtents.MinX));
                    heightM = Math.Abs(metersPerUnit * (ovExtents.MaxY - ovExtents.MinY));

                    rows = Math.Max(1, (int)Math.Ceiling((heightM / tileMapHeight)));
                    cols = Math.Max(1, (int)Math.Ceiling((widthM / tileMapWidth)));
                }

                //Collect the sub-totals for each group, so that we can then do one big array allocation with 
                //the grand total number of tile requests we are expecting to make at the end
                foreach (var groupName in _options.GroupNames)
                {
                    groups.Add(new TileGroupSet(groupName, rows, cols, scaleIndex, rowTileOffset, colTileOffset));
                }
            }

            var capacity = groups.Sum(g => g.Total);
            var tiles = new TileRef[capacity];
            var current = 0;

            foreach (var g in groups)
            {
                for (int r = 0; r < g.Rows; r++)
                {
                    for (int c = 0; c < g.Cols; c++)
                    {
                        var row = r + g.RowTileOffset;
                        var col = c + g.ColTileOffset;
                        tiles[current] = new TileRef(g.GroupName, row, col, g.ScaleIndex);
                        current++;
                    }
                }
            }

            return tiles;
        }
    }
}
