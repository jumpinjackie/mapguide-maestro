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
    /// Filter types
    /// </summary>
    public enum FilterType
    {
        /// <summary>
        /// Unary logical
        /// </summary>
        UnaryLogicalOperator,
        /// <summary>
        /// Null condition
        /// </summary>
        NullCondition,
        /// <summary>
        /// In condition
        /// </summary>
        InCondition,
        /// <summary>
        /// Binary logical
        /// </summary>
        BinaryLogicalOperator,
        /// <summary>
        /// Distance condition
        /// </summary>
        DistanceCondition,
        /// <summary>
        /// Comparison condition
        /// </summary>
        ComparisonCondition,
        /// <summary>
        /// Spatial condition
        /// </summary>
        SpatialCondition
    }

    /// <summary>
    /// The base class of all FDO filter expressions
    /// </summary>
    public abstract class FdoFilter : FdoParseable
    {
        /// <summary>
        /// The parseable type
        /// </summary>
        public override FdoParseableType ParseableType => FdoParseableType.Filter;

        /// <summary>
        /// The filter type
        /// </summary>
        public abstract FilterType FilterType { get; }

        /// <summary>
        /// Parses the given FDO filter string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static FdoFilter Parse(string str)
        {
            Parser p = new Parser(new FdoFilterGrammar());
            var tree = p.Parse(str);
            CheckParserErrors(tree);
            if (tree.Root.Term.Name == FdoTerminalNames.Filter)
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

        internal static FdoFilter ParseNode(ParseTreeNode child)
        {
            if (child.Term.Name == FdoTerminalNames.Filter)
            {
                return ParseNode(child.ChildNodes[0]);
            }
            else
            {
                switch (child.Term.Name)
                {
                    case FdoTerminalNames.LogicalOperator:
                        return FdoLogicalOperator.ParseLogicalOperatorNode(child);
                    case FdoTerminalNames.SearchCondition:
                        return FdoSearchCondition.ParseSearchNode(child);
                    default:
                        throw new FdoParseException("Unknown terminal: " + child.Term.Name);
                }
            }
        }
    }
}
