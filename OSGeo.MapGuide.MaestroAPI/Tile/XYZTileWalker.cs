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

using OSGeo.MapGuide.MaestroAPI.Tile;
using System;
using System.Linq;

namespace OSGeo.MapGuide.MaestroAPI.Tile
{
    internal struct Point<TComponent>
    {
        public Point(TComponent x, TComponent y)
        {
            this.X = x;
            this.Y = y;
        }

        public TComponent X { get; }

        public TComponent Y { get; }
    }

    /// <summary>
    /// An implementation of <see cref="ITileWalker"/> that can compute a list of tiles to request
    /// for an XYZ tile set
    /// </summary>
    public class XYZTileWalker : ITileWalker
    {
        readonly Point<double> _ll;
        readonly Point<double> _ur;

        /// <summary>
        /// Constructs a new instance for the given extent
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        public XYZTileWalker(double minX, double minY, double maxX, double maxY)
        {
            _ll = new Point<double>(minX, minY);
            _ur = new Point<double>(maxX, maxY);
        }

        static Point<int> WorldToTilePos(double lon, double lat, int zoom)
        {
            var x = (int)((lon + 180.0) / 360.0 * (1 << zoom));
            var y = (int)((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) +
                1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << zoom));
            return new Point<int>(x, y);
        }

        /// <summary>
        /// Un-used. Returns an empty string
        /// </summary>
        public string ResourceID => string.Empty;

        struct TileSetRange
        {
            public TileSetRange(int x, int y, int zoom, int rows, int cols)
            {
                this.X = x;
                this.Y = y;
                this.Z = zoom;
                this.Rows = rows;
                this.Cols = cols;
            }

            public int X { get; }

            public int Y { get; }

            public int Z { get; }

            public int Rows { get; }

            public int Cols { get; }

            public int Total => Rows * Cols;
        }

        /// <summary>
        /// Computes the list of all possible tiles to request
        /// </summary>
        /// <returns></returns>
        public TileRef[] GetTileList()
        {
            //Z goes from 0 to 19
            var ranges = new TileSetRange[20];
            for (int z = 0; z < ranges.Length; z++)
            {
                //Allow floating point drift
                var ll = WorldToTilePos(_ll.X, _ll.Y, z);
                var ur = WorldToTilePos(_ur.X, _ur.Y, z);

                var rows = Math.Abs(ur.X - ll.X) + 1;
                var cols = Math.Abs(ur.Y - ll.Y) + 1;

                ranges[z] = new TileSetRange(Math.Min(ll.X, ur.X), Math.Min(ll.Y, ur.Y), z, rows, cols);
            }

            var capacity = ranges.Sum(g => g.Total);
            var tiles = new TileRef[capacity];
            var current = 0;

            for (int z = 0; z < ranges.Length; z++)
            {
                var g = ranges[z];
                for (int xi = 0; xi < g.Rows; xi++)
                {
                    for (int yi = 0; yi < g.Cols; yi++)
                    {
                        int x = g.X + xi;
                        int y = g.Y + yi;
                        tiles[current] = new TileRef(string.Empty, x, y, z);
                        current++;
                    }
                }
            }

            return tiles;
        }
    }
}
