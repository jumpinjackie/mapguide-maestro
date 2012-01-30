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
using System.Drawing;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace OSGeo.MapGuide.MaestroAPI.CoordinateSystem
{
    internal class MeterBasedCoordinateSystem : CoordinateSystemBase
    {
        //Dots pr inch
        protected const double DPI = 96;

        //Inches pr meter
        protected const double IPM = 39.3700787;

        //Coordsys distance pr unit in meters, X/Y axis
        protected readonly double UDM_X = 1;
        protected readonly double UDM_Y = 1;

        internal MeterBasedCoordinateSystem() { }

        internal MeterBasedCoordinateSystem(double meters_pr_x_unit, double meters_pr_y_unit)
            : this()
        {
            UDM_X = meters_pr_x_unit;
            UDM_Y = meters_pr_y_unit;
        }

        public override double MetersPerUnitX { get { return UDM_X; } }

        public override double MetersPerUnitY { get { return UDM_Y; } }

        protected override double CalculateScale(IEnvelope bbox, Size size)
        {
            double picture_width_in_meters = (size.Width / DPI) / IPM;
            double picture_height_in_meters = (size.Height / DPI) / IPM;

            double map_width_in_meters = bbox.Width * UDM_X;
            double map_height_in_meters = bbox.Height * UDM_Y;

            double width_scale = map_width_in_meters / picture_width_in_meters;
            double height_scale = map_height_in_meters / picture_height_in_meters;

            return Math.Max(width_scale, height_scale);
        }

        protected override IEnvelope AdjustBoundingBox(IEnvelope bbox, double scale, Size size)
        {
            double picture_width_in_meters = ((size.Width / DPI) / IPM) * scale; 
            double picture_height_in_meters = ((size.Height / DPI) / IPM) * scale;

            double width_extent = picture_width_in_meters / UDM_X;
            double height_extent = picture_height_in_meters / UDM_Y;

            return new Envelope(bbox.Centre.X - (width_extent / 2), bbox.Centre.X + (width_extent / 2), bbox.Centre.Y - (height_extent / 2), bbox.Centre.Y + (height_extent / 2));
        }

        protected override double DistanceInMeters(IPoint p1, IPoint p2)
        {
            double xdist = Math.Abs(p1.X - p2.X);
            double ydist = Math.Abs(p1.Y - p2.Y);

            return Math.Sqrt((xdist * xdist) + (ydist * ydist));            
        }
    }
}
