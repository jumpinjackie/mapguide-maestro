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
using GeoAPI.Geometries;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using System;

namespace OSGeo.MapGuide.MaestroAPI.Geometry
{
    internal class GeoAPIGeometryRef : IGeometryRef
    {
        readonly IGeometry _geom;

        internal GeoAPIGeometryRef(IGeometry geom)
        {
            _geom = geom;
        }

        public IPoint2D GetCentroid(bool envelope)
        {
            if (envelope)
            {
                return GetEnvelope().Center();
            }
            else
            {
                var cnt = _geom.Centroid;
                return ObjectFactory.CreatePoint2D(cnt.X, cnt.Y);
            }
        }

        public double Area => _geom.Area;

        public double Length => _geom.Length;

        public bool IsEmpty => _geom.IsEmpty;

        public bool IsSimple => _geom.IsSimple;

        public bool IsValid => _geom.IsValid;

        public string AsText() => _geom.AsText();

        public IGeometryRef GetBoundary() => new GeoAPIGeometryRef(_geom.Boundary);

        public ObjectModels.Common.IEnvelope GetEnvelope()
        {
            var env = _geom.EnvelopeInternal;
            return ObjectFactory.CreateEnvelope(env.MinX, env.MinY, env.MaxX, env.MaxY);
        }

        static IGeometryRef BinaryOperator(GeoAPIGeometryRef thisRef, IGeometryRef otherRef, Func<IGeometry, IGeometry, IGeometry> operation)
        {
            var impl = otherRef as GeoAPIGeometryRef;
            if (impl != null)
            {
                var result = operation(thisRef._geom, impl._geom);
                return new GeoAPIGeometryRef(result);
            }
            throw new ArgumentException("Incorrect IGeometryRef implementation");
        }

        static bool BinaryPredicate(GeoAPIGeometryRef thisRef, IGeometryRef otherRef, Func<IGeometry, IGeometry, bool> operation)
        {
            var impl = otherRef as GeoAPIGeometryRef;
            if (impl != null)
            {
                return operation(thisRef._geom, impl._geom);
            }
            throw new ArgumentException("Incorrect IGeometryRef implementation");
        }

        public IGeometryRef ConvexHull() => new GeoAPIGeometryRef(_geom.ConvexHull());

        public bool Contains(IGeometryRef other) => BinaryPredicate(this, other, (a, b) => a.Contains(b));

        public bool Crosses(IGeometryRef other) => BinaryPredicate(this, other, (a, b) => a.Crosses(b));

        public IGeometryRef Difference(IGeometryRef other) => BinaryOperator(this, other, (a, b) => a.Difference(b));

        public bool Disjoint(IGeometryRef other) => BinaryPredicate(this, other, (a, b) => a.Disjoint(b));

        public bool Equals(IGeometryRef other) => BinaryPredicate(this, other, (a, b) => a.Equals(b));

        public bool Intersects(IGeometryRef other) => BinaryPredicate(this, other, (a, b) => a.Intersects(b));

        public IGeometryRef Intersection(IGeometryRef other) => BinaryOperator(this, other, (a, b) => a.Intersection(b));

        public bool Overlaps(IGeometryRef other) => BinaryPredicate(this, other, (a, b) => a.Overlaps(b));

        public IGeometryRef SymmetricDifference(IGeometryRef other) => BinaryOperator(this, other, (a, b) => a.SymmetricDifference(b));

        public bool Touches(IGeometryRef other) => BinaryPredicate(this, other, (a, b) => a.Touches(b));

        public bool Within(IGeometryRef other) => BinaryPredicate(this, other, (a, b) => a.Within(b));

        public IGeometryRef Union(IGeometryRef other) => BinaryOperator(this, other, (a, b) => a.Union(b));
    }
}
