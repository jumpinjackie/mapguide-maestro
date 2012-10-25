#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.Linq;
using System.Text;
using System.Drawing;

namespace Maestro.MapViewer
{
    /// <summary>
    /// A callback for circle digitization
    /// </summary>
    /// <param name="x">The X coordinate of the circle's center</param>
    /// <param name="y">The Y coordinate of the circle's center</param>
    /// <param name="radius">The circle's radius</param>
    public delegate void CircleDigitizationCallback(double x, double y, double radius);

    /// <summary>
    /// A callback for line digitization
    /// </summary>
    /// <param name="x1">The X coordinate of the line's first point</param>
    /// <param name="y1">The Y coordinate of the line's first point</param>
    /// <param name="x2">The X coordinate of the line's second point</param>
    /// <param name="y2">The Y coordinate of the line's second point</param>
    public delegate void LineDigitizationCallback(double x1, double y1, double x2, double y2);

    /// <summary>
    /// A callback for point digitization
    /// </summary>
    /// <param name="x">The X coordinate of the point</param>
    /// <param name="y">The Y coordinate of the point</param>
    public delegate void PointDigitizationCallback(double x, double y);

    /// <summary>
    /// A callback for polygon digitization
    /// </summary>
    /// <param name="coordinates">A n by 2 array of polygon coordinates, where n is the number of vertices</param>
    public delegate void PolygonDigitizationCallback(double[,] coordinates);

    /// <summary>
    /// A callback for line string digitization
    /// </summary>
    /// <param name="coordinates">A n by 2 array of line string coordinates, where n is the number of vertices</param>
    public delegate void LineStringDigitizationCallback(double[,] coordinates);

    /// <summary>
    /// A callback for rectangle digitization
    /// </summary>
    /// <param name="llx">The X coordinate of the rectangle's lower left point</param>
    /// <param name="lly">The Y coordinate of the rectangle's lower left point</param>
    /// <param name="urx">The X coordinate of the rectangle's upper right point</param>
    /// <param name="ury">The Y coordinate of the rectangle's upper right point</param>
    public delegate void RectangleDigitizationCallback(double llx, double lly, double urx, double ury);

    /// <summary>
    /// Represents an entry in the view history stack
    /// </summary>
    public class MapViewHistoryEntry
    {
        internal MapViewHistoryEntry(double x, double y, double scale)
        {
            this.X = x;
            this.Y = y;
            this.Scale = scale;
        }

        /// <summary>
        /// Gets the X coordinate
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Gets the Y coordinate
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Gets the view scale
        /// </summary>
        public double Scale { get; private set; }
    }

    /// <summary>
    /// Contains data of a MouseMapPositionChanged event
    /// </summary>
    public class MapPointEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the X coordinate 
        /// </summary>
        public readonly double X;

        /// <summary>
        /// Gets the Y coordinate
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public MapPointEventArgs(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    /// <summary>
    /// The type of digitization in progress
    /// </summary>
    public enum MapDigitizationType
    {
        /// <summary>
        /// No digitization in progress
        /// </summary>
        None,
        /// <summary>
        /// A point digitization is in progress
        /// </summary>
        Point,
        /// <summary>
        /// A line digitization is in progress
        /// </summary>
        Line,
        /// <summary>
        /// A line string digitization is in progress
        /// </summary>
        LineString,
        /// <summary>
        /// A rectangle digitization is in progress
        /// </summary>
        Rectangle,
        /// <summary>
        /// A polygon digitization is in progress
        /// </summary>
        Polygon,
        /// <summary>
        /// A circle digitization is in progress
        /// </summary>
        Circle
    }

    /// <summary>
    /// The active viewer tool
    /// </summary>
    public enum MapActiveTool
    {
        /// <summary>
        /// Zoom In command
        /// </summary>
        ZoomIn,
        /// <summary>
        /// Zoom Out command
        /// </summary>
        ZoomOut,
        /// <summary>
        /// Pan command
        /// </summary>
        Pan,
        /// <summary>
        /// Select command
        /// </summary>
        Select,
        /// <summary>
        /// No active command
        /// </summary>
        None
    }

    /// <summary>
    /// Viewer rendering options
    /// </summary>
    public class ViewerRenderingOptions
    {
        internal ViewerRenderingOptions(string format, int behavior, Color color)
        {
            this.Format = format;
            this.Behavior = behavior;
            this.Color = color;
        }

        /// <summary>
        /// Gets the format.
        /// </summary>
        public string Format { get; private set; }

        /// <summary>
        /// Gets the behavior.
        /// </summary>
        public int Behavior { get; private set; }

        /// <summary>
        /// Gets the color.
        /// </summary>
        public Color Color { get; private set; }
    }
}
