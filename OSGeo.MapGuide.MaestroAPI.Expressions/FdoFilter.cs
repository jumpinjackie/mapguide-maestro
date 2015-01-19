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
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Expressions
{
    public enum FilterType
    {
        UnaryLogicalOperator,
        NullCondition,
        InCondition,
        BinaryLogicalOperator,
        DistanceCondition,
        ComparisonCondition,
        SpatialCondition
    }

    public abstract class FdoFilter : FdoParseable
    {
        public abstract FilterType FilterType { get; }

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
                List<string> errors = new List<string>();
                foreach (var msg in tree.ParserMessages)
                {
                    if (msg.Level == Irony.ErrorLevel.Error)
                    {
                        errors.Add(string.Format(Strings.ParserErrorMessage, msg.Location.ToUiString(), msg.Message));
                    }
                }
                throw new FdoParseException(string.Format(Strings.FilterParseError, Environment.NewLine, string.Join(Environment.NewLine, errors.ToArray())));
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
