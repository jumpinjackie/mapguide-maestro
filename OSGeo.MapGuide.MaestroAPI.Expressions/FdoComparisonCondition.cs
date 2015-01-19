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
    public enum ComparisonOperations
    {
        EqualsTo,
        NotEqualsTo,
        GreaterThan,
        GreaterThanOrEqualTo,
        LessThan,
        LessThanOrEqualTo,
        Like
    }

    public class FdoComparisonCondition : FdoSearchCondition
    {
        public FdoExpression Left { get; private set; }

        public ComparisonOperations Operator { get; private set; }

        public FdoExpression Right { get; private set; }

        public FdoComparisonCondition(ParseTreeNode node)
        {
            this.Left = FdoExpression.ParseNode(node.ChildNodes[0]);
            var opName = node.ChildNodes[1].ChildNodes[0].Token.ValueString;
            switch (opName.ToUpper())
            {
                case "=":
                    this.Operator = ComparisonOperations.EqualsTo;
                    break;
                case "<>":
                    this.Operator = ComparisonOperations.NotEqualsTo;
                    break;
                case ">":
                    this.Operator = ComparisonOperations.GreaterThan;
                    break;
                case ">=":
                    this.Operator = ComparisonOperations.GreaterThanOrEqualTo;
                    break;
                case "<":
                    this.Operator = ComparisonOperations.LessThan;
                    break;
                case "<=":
                    this.Operator = ComparisonOperations.LessThanOrEqualTo;
                    break;
                case "LIKE":
                    this.Operator = ComparisonOperations.Like;
                    break;
                default:
                    throw new FdoParseException("Unknown operator: " + opName);
            }
            this.Right = FdoExpression.ParseNode(node.ChildNodes[2]);
        }
    }
}
