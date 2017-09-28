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
using OSGeo.MapGuide.ObjectModels.Common;

namespace OSGeo.MapGuide.MaestroAPI.Geometry
{
    /// <summary>
    /// Defines a geometry instance
    /// </summary>
    public interface IGeometryRef
    {
        /// <summary>
        /// Gets the centroid of the geometry instance
        /// </summary>
        /// <param name="envelope">If true, will return the center of the geometry's envelope instead. This is faster for really complex geometries and you only require an approximate center (eg. A point to zoom to)</param>
        /// <returns></returns>
        IPoint2D GetCentroid(bool envelope);

        /// <summary>
        /// Gets the area of this instance
        /// </summary>
        double Area { get; }

        /// <summary>
        /// Gets the length of this instance
        /// </summary>
        double Length { get; }

        /// <summary>
        /// Gets whether this instance is empty
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets whether this entity is simple or not. Simple entities do not contain any points of self-tangency or self intersection. 
        /// </summary>
        bool IsSimple { get; }

        /// <summary>
        /// Gets whether the coordinates given to construct the entity represent a valid Geometry
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Gets the well-known text representation of this geometry
        /// </summary>
        /// <returns></returns>
        string AsText();

        /// <summary>
        /// Gets the envelope of this geometry
        /// </summary>
        /// <returns></returns>
        IEnvelope GetEnvelope();

        /// <summary>
        /// Returns true if and only if no points of B lie in the exterior of A, and at least one point of the interior of B lies in the interior of A
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool Contains(IGeometryRef other);

        /// <summary>
        /// Gets the minimum convex geometry that encloses all geometries within the set
        /// </summary>
        /// <returns></returns>
        IGeometryRef ConvexHull();

        /// <summary>
        /// Returns true if the supplied geometries have some, but not all, interior points in common
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool Crosses(IGeometryRef other);

        /// <summary>
        /// Returns a geometry that represents that part of geometry A that does not intersect with geometry B
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        IGeometryRef Difference(IGeometryRef other);

        /// <summary>
        /// Returns true if the Geometries do not "spatially intersect" - if they do not share any space together
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool Disjoint(IGeometryRef other);

        /// <summary>
        /// Returns true if the given geometries represent the same geometry. Directionality is ignored
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool Equals(IGeometryRef other);

        /// <summary>
        /// This is a convenience method. Given 2 geometries a and b, a.Intersects(b) is true if and only if a.Disjoint (b) is false.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool Intersects(IGeometryRef other);

        /// <summary>
        /// Returns a geometry that represents the point set intersection of this geometry and another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        IGeometryRef Intersection(IGeometryRef other);

        /// <summary>
        /// Given 2 geometries a and b, a.Overlaps(b) is true if and only if the dimension of the interior of a equals the dimension of the interior of b equals the dimension of the intersection of the interior of a and the interior of b and the intersection of a and b is neither a nor b
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool Overlaps(IGeometryRef other);

        /// <summary>
        /// Returns a geometry that represents the point set symmetric difference of this geometry with another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        IGeometryRef SymmetricDifference(IGeometryRef other);

        /// <summary>
        /// Returns true if the geometries have at least one point in common, but their interiors do not intersect
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool Touches(IGeometryRef other);

        /// <summary>
        /// Returns true if the geometry A is completely inside geometry B
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool Within(IGeometryRef other);

        /// <summary>
        /// Returns a geometry that represents the point set union of the Geometries
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        IGeometryRef Union(IGeometryRef other);
    }
}
