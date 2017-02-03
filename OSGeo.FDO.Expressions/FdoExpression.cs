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
using System.Collections.Generic;

namespace OSGeo.FDO.Expressions
{
    /// <summary>
    /// FDO expression types
    /// </summary>
    public enum ExpressionType
    {
        /// <summary>
        /// Unary expression
        /// </summary>
        UnaryExpression,
        /// <summary>
        /// Binary expression
        /// </summary>
        BinaryExpression,
        /// <summary>
        /// Function
        /// </summary>
        Function,
        /// <summary>
        /// Identifier
        /// </summary>
        Identifier,
        /// <summary>
        /// Parameter
        /// </summary>
        Parameter,
        /// <summary>
        /// Geometry value
        /// </summary>
        GeometryValue,
        /// <summary>
        /// Boolean value
        /// </summary>
        BooleanValue,
        /// <summary>
        /// String value
        /// </summary>
        StringValue,
        /// <summary>
        /// Int32 value
        /// </summary>
        Int32Value,
        /// <summary>
        /// Double value
        /// </summary>
        DoubleValue,
        /// <summary>
        /// DateTime value
        /// </summary>
        DateTimeValue
    }

    /// <summary>
    /// The base class of all FDO expressions
    /// </summary>
    public abstract class FdoExpression : FdoParseable
    {
        /// <summary>
        /// The parseable type
        /// </summary>
        public override FdoParseableType ParseableType => FdoParseableType.Expression;

        /// <summary>
        /// The expression type
        /// </summary>
        public abstract ExpressionType ExpressionType { get; }

        /// <summary>
        /// Parses the given FDO expression string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static FdoExpression Parse(string str)
        {
            Parser p = new Parser(new FdoExpressionGrammar());
            var tree = p.Parse(str);
            CheckParserErrors(tree);
            if (tree.Root.Term.Name == FdoTerminalNames.Expression)
            {
                var child = tree.Root.ChildNodes[0];
                return ParseNode(child);
            }
            else
            {
                throw new FdoParseException();
            }
        }

        private static void CheckParserErrors(ParseTree tree)
        {
            if (tree.HasErrors())
            {
                List<FdoParseErrorMessage> errors = new List<FdoParseErrorMessage>();
                foreach (var msg in tree.ParserMessages)
                {
                    if (msg.Level == Irony.ErrorLevel.Error)
                    {
                        errors.Add(new FdoParseErrorMessage(msg.Message, msg.Location.Line, msg.Location.Column));
                    }
                }
                throw new FdoMalformedExpressionException(Strings.ParserErrorMessage, errors);
            }
        }

        internal static FdoExpression ParseNode(ParseTreeNode child)
        {
            if (child.Term.Name == FdoTerminalNames.Expression)
            {
                return ParseNode(child.ChildNodes[0]);
            }
            else
            {
                switch (child.Term.Name)
                {
                    case FdoTerminalNames.UnaryExpression:
                        return new FdoUnaryExpression(child);
                    case FdoTerminalNames.BinaryExpression:
                        return new FdoBinaryExpression(child);
                    case FdoTerminalNames.Function:
                        return new FdoFunction(child);
                    case FdoTerminalNames.Identifier:
                        return new FdoIdentifier(child);
                    case FdoTerminalNames.ValueExpression:
                        return FdoValueExpression.ParseValueNode(child);
                    default:
                        throw new FdoParseException("Unknown terminal: " + child.Term.Name);
                }
            }
        }
    }
}
