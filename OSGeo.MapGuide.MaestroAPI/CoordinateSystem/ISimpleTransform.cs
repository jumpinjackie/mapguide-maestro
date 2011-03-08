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

namespace OSGeo.MapGuide.MaestroAPI.CoordinateSystem
{
    /// <summary>
    /// A simple coordinate transformation interface
    /// </summary>
    public interface ISimpleTransform
    {
        /// <summary>
        /// Transforms the specified point
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="tx">The transformed X coordinate</param>
        /// <param name="ty">The transformed Y coordinate</param>
        void Transform(double x, double y, out double tx, out double ty);
    }

    /// <summary>
    /// A simple transform that wraps the NTS coordinate system transformation APIs
    /// 
    /// This does not handle some of the more complex transformations like CS-Map can,
    /// such as WGS84.PseudoMercator (required for Google/Yahoo/Bing underlays in fusion)
    /// </summary>
    public class DefaultSimpleTransform : ISimpleTransform
    {
        private Topology.CoordinateSystems.ICoordinateSystem _source;
        private Topology.CoordinateSystems.ICoordinateSystem _target;
        private Topology.CoordinateSystems.Transformations.ICoordinateTransformation _trans;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSimpleTransform"/> class.
        /// </summary>
        /// <param name="sourceCsWkt">The source cs WKT.</param>
        /// <param name="targetCsWkt">The target cs WKT.</param>
        public DefaultSimpleTransform(string sourceCsWkt, string targetCsWkt)
        {
            var fact = new Topology.CoordinateSystems.CoordinateSystemFactory();
            _source = fact.CreateFromWkt(sourceCsWkt);
            _target = fact.CreateFromWkt(targetCsWkt);
            var tfact = new Topology.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            _trans = tfact.CreateFromCoordinateSystems(_source, _target);
        }

        /// <summary>
        /// Transforms the specified point
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="tx">The transformed X coordinate</param>
        /// <param name="ty">The transformed Y coordinate</param>
        public void Transform(double x, double y, out double tx, out double ty)
        {
            tx = Double.NaN;
            ty = Double.NaN;

            double [] pts = _trans.MathTransform.Transform(new double[] { x, y });
            tx = pts[0];
            ty = pts[1];
        }
    }
}
