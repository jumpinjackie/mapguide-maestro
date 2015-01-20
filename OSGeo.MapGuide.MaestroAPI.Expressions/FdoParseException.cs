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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Expressions
{
    [Serializable]
    public class FdoParseException : Exception
    {
        public FdoParseException() { }
        public FdoParseException(string message) : base(message) { }
        public FdoParseException(string message, Exception inner) : base(message, inner) { }
        protected FdoParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public class FdoParseErrorMessage
    {
        public int LineNumber { get; private set; }

        public int Column { get; private set; }

        public string Message { get; private set; }

        public FdoParseErrorMessage(string message, int line, int column)
        {
            this.Message = message;
            this.LineNumber = line;
            this.Column = column;
        }
    }

    [Serializable]
    public class FdoMalformedExpressionException : FdoParseException
    {
        public List<FdoParseErrorMessage> Messages { get; private set; }

        public FdoMalformedExpressionException(string message, List<FdoParseErrorMessage> parserErrorMessages)
            : this(message)
        {
            this.Messages = parserErrorMessages;
        }

        private FdoMalformedExpressionException(string message) : base(message) { }
        private FdoMalformedExpressionException(string message, Exception inner) : base(message, inner) { }
        protected FdoMalformedExpressionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
