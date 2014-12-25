#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using OSGeo.MapGuide.ObjectModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModels
{
    public class ObjectFactory
    {
        /// <summary>
        /// Creates an envelope (bounding box)
        /// </summary>
        /// <param name="minx"></param>
        /// <param name="miny"></param>
        /// <param name="maxx"></param>
        /// <param name="maxy"></param>
        /// <returns></returns>
        public static IEnvelope CreateEnvelope(double minx, double miny, double maxx, double maxy)
        {
            if (minx > maxx)
                throw new ArgumentException("minx > maxx", "minx"); //NOXLATE

            if (miny > maxy)
                throw new ArgumentException("miny > maxy", "miny"); //NOXLATE

            return new Envelope()
            {
                LowerLeftCoordinate = new EnvelopeLowerLeftCoordinate()
                {
                    X = minx,
                    Y = miny
                },
                UpperRightCoordinate = new EnvelopeUpperRightCoordinate()
                {
                    X = maxx,
                    Y = maxy
                }
            };
        }

        /// <summary>
        /// Creates a 2d point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IPoint2D CreatePoint2D(double x, double y)
        {
            return new Point2DImpl() { X = x, Y = y };
        }

        /// <summary>
        /// Creates a 3d point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static IPoint3D CreatePoint3D(double x, double y, double z)
        {
            return new Point3DImpl() { X = x, Y = y, Z = z };
        }
    }
}