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

namespace OSGeo.MapGuide.MaestroAPI.Exceptions
{
    /// <summary>
    /// Helper class to process exception messages for exceptions that may contain one or more nested inner exceptions
    /// </summary>
    public static class NestedExceptionMessageProcessor
    {
        /// <summary>
        /// Returns a formatted string containing the main exception message and all messages within the <see cref="P:System.Exception.InnerException"/> properties
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public static string GetFullMessage(Exception error)
        {
            if (error == null)
                return string.Empty;

            Exception ex = error;
            string innerPrefix = Environment.NewLine + "\t"; //NOXLATE
            StringBuilder sb = new StringBuilder();
            while (ex.InnerException != null)
            {
                sb.Append(innerPrefix + ex.Message);
                ex = ex.InnerException;
            }
            sb.Append(innerPrefix + ex.Message);
            return sb.ToString();
        }
    }
}
