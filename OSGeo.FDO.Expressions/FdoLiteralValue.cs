#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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
using Irony.Parsing;

namespace OSGeo.FDO.Expressions
{
    /// <summary>
    /// The literal value type
    /// </summary>
    public enum LiteralValueType
    {
        /// <summary>
        /// A data value
        /// </summary>
        Data,
        /// <summary>
        /// A geometry value
        /// </summary>
        Geometry
    }

    /// <summary>
    /// An FDO literal value
    /// </summary>
    public abstract class FdoLiteralValue : FdoValueExpression
    {
        /// <summary>
        /// The literal value type
        /// </summary>
        public LiteralValueType LiteralValueType { get; protected set; }

        internal static FdoLiteralValue ParseLiteralNode(ParseTreeNode node)
        {
            if (node.Term.Name == FdoTerminalNames.LiteralValue)
            {
                return ParseLiteralNode(node.ChildNodes[0]);
            }
            else
            {
                switch (node.Term.Name)
                {
                    case FdoTerminalNames.DataValue:
                        return FdoDataValue.ParseDataNode(node);
                    case FdoTerminalNames.GeometryValue:
                        return new FdoGeometryValue(node);
                    default:
                        throw new FdoParseException("Unknown terminal: " + node.Term.Name);
                }
            }
        }
    }
}
