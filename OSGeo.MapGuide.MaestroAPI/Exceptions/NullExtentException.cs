#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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

namespace OSGeo.MapGuide.MaestroAPI.Exceptions
{
    /// <summary>
    /// Thrown when no result exists for a extent query
    /// </summary>
    [global::System.Serializable]
    public class NullExtentException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public NullExtentException() { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="message"></param>
        public NullExtentException(string message) : base(message) { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public NullExtentException(string message, Exception inner) : base(message, inner) { }
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected NullExtentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
