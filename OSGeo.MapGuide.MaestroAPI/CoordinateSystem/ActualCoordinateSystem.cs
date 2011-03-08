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
using Topology.CoordinateSystems;
using Topology.CoordinateSystems.Transformations;

namespace OSGeo.MapGuide.MaestroAPI.CoordinateSystem
{
    internal class ActualCoordinateSystem : MeterBasedCoordinateSystem
    {
        private Topology.CoordinateSystems.Transformations.ICoordinateTransformation m_transform;
        private const string XY_M = "LOCAL_CS[\"Non-Earth (Meter)\",LOCAL_DATUM[\"Local Datum\",0],UNIT[\"Meter\", 1],AXIS[\"X\",EAST],AXIS[\"Y\",NORTH]]";

        internal ActualCoordinateSystem(Topology.CoordinateSystems.ICoordinateSystem coordinateSystem)
        {
            if (coordinateSystem == null)
                throw new ArgumentNullException("coordinateSystem");

            CoordinateTransformationFactory f = new CoordinateTransformationFactory();
            CoordinateSystemFactory cf = new CoordinateSystemFactory();

            /*Topology.CoordinateSystems.ICoordinateSystem local = cf.CreateLocalCoordinateSystem(
                "Non-Earth (Meter)",
                cf.CreateLocalDatum("Local Datum", Topology.CoordinateSystems.DatumType.VD_Normal),
                new Topology.CoordinateSystems.LinearUnit(1.0, "Meter", "", 0, "", "", ""),
                new List<Topology.CoordinateSystems.AxisInfo>(new Topology.CoordinateSystems.AxisInfo[] {
                    new Topology.CoordinateSystems.AxisInfo("X", Topology.CoordinateSystems.AxisOrientationEnum.East),
                    new Topology.CoordinateSystems.AxisInfo("Y", Topology.CoordinateSystems.AxisOrientationEnum.North)
                })
            );

            string s = cf.ToString();*/

            m_transform = f.CreateFromCoordinateSystems(coordinateSystem, cf.CreateFromWkt(XY_M));
        }

        protected override double CalculateScale(Topology.Geometries.IEnvelope bbox, System.Drawing.Size size)
        {
            double[] points = m_transform.MathTransform.Transform(new double[] { bbox.MinX, bbox.MinY, bbox.MaxX, bbox.MaxY });
            Topology.Geometries.IEnvelope localEnv = new Topology.Geometries.Envelope(points[0], points[2], points[1], points[3]);
            return base.CalculateScale(localEnv, size);
        }

        protected override Topology.Geometries.IEnvelope AdjustBoundingBox(Topology.Geometries.IEnvelope bbox, double scale, System.Drawing.Size size)
        {
            double[] points = m_transform.MathTransform.Transform(new double[] { bbox.MinX, bbox.MinY, bbox.MaxX, bbox.MaxY });
            Topology.Geometries.IEnvelope localEnv = new Topology.Geometries.Envelope(points[0], points[2], points[1], points[3]);
            localEnv = base.AdjustBoundingBox(localEnv, scale, size);
            points = m_transform.MathTransform.Inverse().Transform(new double[] { localEnv.MinX, localEnv.MinY, localEnv.MaxX, localEnv.MaxY });
            return new Topology.Geometries.Envelope(points[0], points[2], points[1], points[3]);
        }

        protected override double DistanceInMeters(Topology.Geometries.IPoint p1, Topology.Geometries.IPoint p2)
        {
            double[] points = m_transform.MathTransform.Transform(new double[] { p1.X, p1.Y, p2.X, p2.Y });
            return base.DistanceInMeters(new Topology.Geometries.Point(points[0], points[1]), new Topology.Geometries.Point(points[2], points[3]));
        }

        //public override Topology.CoordinateSystems.ICoordinateSystem CoordinateSystem { get { return m_transform.SourceCS; } }
    }
}
