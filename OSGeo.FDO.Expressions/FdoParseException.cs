#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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

namespace OSGeo.FDO.Expressions
{
    /// <summary>
    /// Thrown when an exception occurs parsing an FDO expression or filter
    /// </summary>
    [Serializable]
    public class FdoParseException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FdoParseException() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public FdoParseException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public FdoParseException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected FdoParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
    
    /// <summary>
    /// A parse error message
    /// </summary>
    public class FdoParseErrorMessage
    {
        /// <summary>
        /// The line number of the source of the offending parse error
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// The column number of the source of the offending parse error
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// The parse error message
        /// </summary>
        public string Message { get; }

        internal FdoParseErrorMessage(string message, int line, int column)
        {
            this.Message = message;
            this.LineNumber = line;
            this.Column = column;
        }
    }

    /// <summary>
    /// Thrown when a malformed FDO expression is encountered
    /// </summary>
    [Serializable]
    public class FdoMalformedExpressionException : FdoParseException
    {
        /// <summary>
        /// The list of parser errors
        /// </summary>
        public List<FdoParseErrorMessage> Messages { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parserErrorMessages"></param>
        public FdoMalformedExpressionException(string message, List<FdoParseErrorMessage> parserErrorMessages)
            : this(message)
        {
            this.Messages = parserErrorMessages;
        }

        private FdoMalformedExpressionException(string message) : base(message) { }
        private FdoMalformedExpressionException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected FdoMalformedExpressionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
