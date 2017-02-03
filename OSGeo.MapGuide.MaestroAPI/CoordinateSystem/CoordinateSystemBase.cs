#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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

using GeoAPI.CoordinateSystems;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using OSGeo.MapGuide.ObjectModels;
using ProjNet.CoordinateSystems;
using ObjCommon = OSGeo.MapGuide.ObjectModels.Common;

namespace OSGeo.MapGuide.MaestroAPI.CoordinateSystem
{
    /// <summary>
    /// Represents a coordinate system instance
    /// </summary>
    internal abstract class CoordinateSystemBase
    {
        private static ICoordinateSystemFactory _csFact;

        static CoordinateSystemBase()
        {
            _csFact = new CoordinateSystemFactory();
        }

        public static CoordinateSystemBase Create(CoordinateSystemDefinitionBase csDef)
        {
            Check.ArgumentNotNull(csDef, nameof(csDef));
            return Create(csDef.WKT);
        }

        public static CoordinateSystemBase Create(string csWkt)
        {
            Check.ArgumentNotEmpty(csWkt, nameof(csWkt));
            return Create(_csFact.CreateFromWkt(csWkt));
        }

        internal static CoordinateSystemBase Create(ICoordinateSystem coordSys)
        {
            CoordinateSystemBase csb = null;
            try
            {
                //This fails because the XY-M projection is not supported
                csb = new ActualCoordinateSystem(coordSys);
            }
            catch { }

            if (csb == null && coordSys != null)
            {
                IUnit unit = coordSys.GetUnits(0);
                var au = unit as IAngularUnit;
                var lu = unit as ILinearUnit;
                if (au != null)
                {
                    double radians = au.RadiansPerUnit;
                    csb = new DegreeBasedCoordinateSystem();
                }
                else if (lu != null)
                {
                    csb = new MeterBasedCoordinateSystem(lu.MetersPerUnit, lu.MetersPerUnit);
                }
            }

            if (csb == null)
                csb = new MeterBasedCoordinateSystem();

            return csb;
        }

        public abstract double MetersPerUnitX { get; }

        public abstract double MetersPerUnitY { get; }

        /// <summary>
        /// Calculates the scale of the map, given the bounding box and image size
        /// </summary>
        /// <param name="bbox">The map bounding box</param>
        /// <param name="size">The size of the image</param>
        /// <returns>The scale</returns>
        public double CalculateScale(ObjCommon.IEnvelope bbox, System.Drawing.Size size)
        {
            Check.ArgumentNotNull(bbox, nameof(bbox));
            return CalculateScale(new Envelope(bbox.MinX, bbox.MaxX, bbox.MinY, bbox.MaxY), size);
        }

        /// <summary>
        /// Calculates the scale of the map, given the bounding box and image size
        /// </summary>
        /// <param name="bbox">The map bounding box</param>
        /// <param name="size">The size of the image</param>
        /// <returns>The scale</returns>
        protected abstract double CalculateScale(IEnvelope bbox, System.Drawing.Size size);

        /// <summary>
        /// Adjusts the boundingbox to equal proportions
        /// </summary>
        /// <param name="bbox">The actual bounding box</param>
        /// <param name="scale">The scale to fit</param>
        /// <param name="size">The size to fit to</param>
        /// <returns>A bounding box with the correct ratio</returns>
        public ObjCommon.IEnvelope AdjustBoundingBox(ObjCommon.IEnvelope bbox, double scale, System.Drawing.Size size)
        {
            Check.ArgumentNotNull(bbox, nameof(bbox));
            var env = AdjustBoundingBox(new Envelope(bbox.MinX, bbox.MaxX, bbox.MinY, bbox.MaxY), scale, size);
            return ObjectFactory.CreateEnvelope(env.MinX, env.MinY, env.MaxX, env.MaxY);
        }

        /// <summary>
        /// Adjusts the boundingbox to equal proportions
        /// </summary>
        /// <param name="bbox">The actual bounding box</param>
        /// <param name="scale">The scale to fit</param>
        /// <param name="size">The size to fit to</param>
        /// <returns>A bounding box with the correct ratio</returns>
        protected abstract IEnvelope AdjustBoundingBox(IEnvelope bbox, double scale, System.Drawing.Size size);

        /// <summary>
        /// Calculates the distance from one point to another, in meters
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public double DistanceInMeters(double x1, double y1, double x2, double y2) => DistanceInMeters(new Point(x1, y1), new Point(x2, y2));

        /// <summary>
        /// Calculates the distance from one point to another, in meters
        /// </summary>
        /// <param name="p1">One point</param>
        /// <param name="p2">Another point</param>
        /// <returns>The distance in meters</returns>
        protected abstract double DistanceInMeters(IPoint p1, IPoint p2);
    }
}