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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// Pre-condition verifier utility class
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Check that condition evaluates to true
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="msg"></param>
        public static void That(bool condition, string msg)
        {
            if (!condition)
                throw new ArgumentException(msg);
        }

        /// <summary>
        /// Check that the argument value is not null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="arg"></param>
        public static void ArgumentNotNull<T>(T obj, string arg) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(arg);
        }

        /// <summary>
        /// Check that value is not null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="arg"></param>
        public static void NotNull<T>(T obj, string arg) where T : class
        {
            if (obj == null)
                throw new ArgumentException(Strings.PrecondValueNull + arg);
        }

        /// <summary>
        /// Check that string value is not null or empty
        /// </summary>
        /// <param name="value"></param>
        /// <param name="arg"></param>
        public static void NotEmpty(string value, string arg)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(Strings.PrecondStringEmpty + arg);
        }

        /// <summary>
        /// Check that the specified condition is true
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="msg"></param>
        public static void Precondition(bool condition, string msg)
        {
            if (!condition)
                throw new ArgumentException(Strings.PrecondFailure + msg);
        }

        /// <summary>
        /// Check that the given integer is between the specified range
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <param name="min">The lower bound</param>
        /// <param name="max">The upper bound</param>
        /// <param name="bInclusive">Determines whether the range is inclusive. If false, the range is exclusive</param>
        /// <param name="msg">The message to include for precondition failure</param>
        public static void IntBetween(int value, int min, int max, bool bInclusive, string msg)
        {
            bool bInRange = false;
            if (bInclusive)
                bInRange = (value >= min && value <= max);
            else
                bInRange = (value > min && value < max);

            if (!bInRange)
                throw new ArgumentException(Strings.PrecondFailure + msg);
        }

        /// <summary>
        /// Check that the given argument is a folder resource id
        /// </summary>
        /// <param name="folderid">The folder resource id to check</param>
        /// <param name="name">The argument name</param>
        public static void IsFolderArgument(string folderid, string name)
        {
            if (!ResourceIdentifier.IsFolderResource(folderid))
                throw new ArgumentException(string.Format(Strings.NotAFolder, folderid), name);
        }
    }
}