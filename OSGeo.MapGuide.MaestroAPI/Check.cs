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

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// An exception thrown as a result of a failed precondition
    /// </summary>
    [global::System.Serializable]
    public class PreconditionException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// Initializes a new instance of the <see cref="PreconditionException"/> class.
        /// </summary>
        public PreconditionException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PreconditionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public PreconditionException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PreconditionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public PreconditionException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PreconditionException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected PreconditionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

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
                throw new PreconditionException(msg);
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
                throw new PreconditionException(Strings.PrecondValueNull + arg);
        }

        /// <summary>
        /// Check that string value is not null or empty
        /// </summary>
        /// <param name="value"></param>
        /// <param name="arg"></param>
        public static void NotEmpty(string value, string arg) 
        {
            if (string.IsNullOrEmpty(value))
                throw new PreconditionException(Strings.PrecondStringEmpty + arg);
        }

        /// <summary>
        /// Check that the specified condition is true
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="msg"></param>
        public static void Precondition(bool condition, string msg)
        {
            if (!condition)
                throw new PreconditionException(Strings.PrecondFailure + msg);
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
                throw new PreconditionException(Strings.PrecondFailure + msg);
        }
    }
}
