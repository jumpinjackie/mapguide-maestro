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
using System.ComponentModel;
using OSGeo.MapGuide.MaestroAPI;

#pragma warning disable 1591, 0114, 0108

namespace OSGeo.MapGuide.ObjectModels.Common
{
    /// <summary>
    /// Represents a rectangular bounding box
    /// </summary>
    public interface IEnvelope : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the min X.
        /// </summary>
        /// <value>The min X.</value>
        double MinX { get; set; }

        /// <summary>
        /// Gets or sets the min Y.
        /// </summary>
        /// <value>The min Y.</value>
        double MinY { get; set; }

        /// <summary>
        /// Gets or sets the max X.
        /// </summary>
        /// <value>The max X.</value>
        double MaxX { get; set; }

        /// <summary>
        /// Gets or sets the max Y.
        /// </summary>
        /// <value>The max Y.</value>
        double MaxY { get; set; }
    }

    /// <summary>
    /// Extension method class
    /// </summary>
    public static class EnvelopeExtensions
    {
        /// <summary>
        /// Gets the center of the specified envelope
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IPoint2D Center(this IEnvelope env)
        {
            Check.NotNull(env, "env"); //NOXLATE

            return ObjectFactory.CreatePoint2D(
                (env.MinX + env.MaxX) / 2,
                (env.MinY + env.MaxY) / 2);
        }

        /// <summary>
        /// Clones this instance
        /// </summary>
        /// <param name="env">The envelope.</param>
        /// <returns></returns>
        public static IEnvelope Clone(this IEnvelope env)
        {
            Check.NotNull(env, "env"); //NOXLATE
            return ObjectFactory.CreateEnvelope(env.MinX, env.MinY, env.MaxX, env.MaxY);
        }

        /// <summary>
        /// Expands this envelope to accomodate the given envelope
        /// </summary>
        /// <param name="env"></param>
        /// <param name="e1"></param>
        public static void ExpandToInclude(this IEnvelope env, IEnvelope e1)
        {
            Check.NotNull(env, "env"); //NOXLATE
            if (e1 == null)
                return;

            if (e1.MinX < env.MinX)
                env.MinX = e1.MinX;

            if (e1.MinY < env.MinY)
                env.MinY = e1.MinY;

            if (e1.MaxX > env.MaxX)
                env.MaxX = e1.MaxX;

            if (e1.MaxY > env.MaxY)
                env.MaxY = e1.MaxY;
        }

        /// <summary>
        /// Indicates whether the specified coordinates are within this instance
        /// </summary>
        /// <param name="env">The env.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified env]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(this IEnvelope env, double x, double y)
        {
            Check.NotNull(env, "env"); //NOXLATE

            return env.MinX <= x &&
                   env.MaxX >= x &&
                   env.MinY <= y &&
                   env.MaxY >= y;
        }

        /// <summary>
        /// Indicates whether the specified envelope intersects this instance
        /// </summary>
        /// <param name="env">The env.</param>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public static bool Intersects(this IEnvelope env, IEnvelope other)
        {
            Check.NotNull(env, "env"); //NOXLATE

            if (other == null)
                return false;

            return !(other.MinX > env.MaxX || other.MaxX < env.MinX || other.MinY > env.MaxY || other.MaxY < env.MinY);
        }
    }

    partial class Envelope : IEnvelope
    {
        internal Envelope() { }

        public Envelope(double minx, double miny, double maxx, double maxy)
        {
            this.lowerLeftCoordinateField = new EnvelopeLowerLeftCoordinate() { X = minx, Y = miny };
            this.upperRightCoordinateField = new EnvelopeUpperRightCoordinate() { X = maxx, Y = maxy };
        }

        /// <summary>
        /// Gets or sets the lower X value
        /// </summary>
        public double MinX
        {
            get { return this.lowerLeftCoordinateField.X; }
            set 
            { 
                this.lowerLeftCoordinateField.X = value;
                OnPropertyChanged("MinX"); //NOXLATE
            }
        }

        /// <summary>
        /// Gets or set the lower Y value
        /// </summary>
        public double MinY
        {
            get { return this.lowerLeftCoordinateField.Y; }
            set 
            { 
                this.lowerLeftCoordinateField.Y = value;
                OnPropertyChanged("MinY"); //NOXLATE
            }
        }

        /// <summary>
        /// Gets or sets the upper X value
        /// </summary>
        public double MaxX
        {
            get { return this.upperRightCoordinateField.X; }
            set 
            { 
                this.upperRightCoordinateField.X = value;
                OnPropertyChanged("MaxX");
            }
        }

        /// <summary>
        /// Gets or sets the upper Y value
        /// </summary>
        public double MaxY
        {
            get { return this.upperRightCoordinateField.Y; }
            set 
            { 
                this.upperRightCoordinateField.Y = value;
                OnPropertyChanged("MaxY"); //NOXLATE
            }
        }
    }
}
