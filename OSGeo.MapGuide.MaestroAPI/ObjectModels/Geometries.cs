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

namespace OSGeo.MapGuide.ObjectModels.Common
{
    /// <summary>
    /// Represents a point in 2 dimensional space
    /// </summary>
    public interface IPoint2D
    {
        /// <summary>
        /// Gets or sets the X coordinate
        /// </summary>
        double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate
        /// </summary>
        double Y { get; set; }
    }

    /// <summary>
    /// Represents a point in 3 dimensional space
    /// </summary>
    public interface IPoint3D : IPoint2D
    {
        /// <summary>
        /// Gets or sets the Z coordinate
        /// </summary>
        double Z { get; set; }
    }

    internal class Point2DImpl : IPoint2D
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    internal class Point3DImpl : Point2DImpl, IPoint3D
    {
        public double Z { get; set; }
    }
}
