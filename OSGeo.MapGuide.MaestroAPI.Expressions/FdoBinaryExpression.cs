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
    public enum BinaryOperator
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }

    public class FdoBinaryExpression : FdoExpression
    {
        public FdoExpression Left { get; private set; }

        public BinaryOperator Operator { get; private set; }

        public FdoExpression Right { get; private set; }

        public FdoBinaryExpression(ParseTreeNode node)
        {
            this.Left = FdoExpression.ParseNode(node.ChildNodes[0]);
            //HACK: Workaround bug in parser or grammar specification where (a - b) produces a: Expression, b: Expression
            if (node.ChildNodes.Count == 2)
            {
                var expr = FdoExpression.ParseNode(node.ChildNodes[1]);
                this.Operator = BinaryOperator.Subtract;
                this.Right = expr;
                /*
                if (expr is FdoUnaryExpression)
                {
                    this.Operator = BinaryOperator.Subtract;
                    this.Right = ((FdoUnaryExpression)expr).Expression;
                }
                else if (expr is FdoDataValue)
                {
                    var dt = ((FdoDataValue)expr).DataType;
                    switch (dt)
                    {
                        case DataType.Double:
                            {
                                FdoDoubleValue dbl = (FdoDoubleValue)expr;
                                if (dbl.Value < 0.0)
                                {
                                    this.Operator = BinaryOperator.Subtract;
                                    this.Right = dbl.Negate();
                                }
                                else
                                {
                                    this.Operator = BinaryOperator.Add;
                                    this.Right = expr;
                                }
                            }
                            break;
                        case DataType.Int32:
                            {
                                FdoInt32Value iv = (FdoInt32Value)expr;
                                if (iv.Value < 0)
                                {
                                    this.Operator = BinaryOperator.Subtract;
                                    this.Right = iv.Negate();
                                }
                                else
                                {
                                    this.Operator = BinaryOperator.Add;
                                    this.Right = expr;
                                }
                            }
                            break;
                        default:
                            throw new FdoParseException("Could not parse as Binary Expression");
                    }
                }
                else
                {
                    throw new FdoParseException("Could not parse as Binary Expression");
                }
                 */
            }
            else
            {
                var opStr = node.ChildNodes[1].Token.ValueString;
                switch (opStr)
                {
                    case "+":
                        this.Operator = BinaryOperator.Add;
                        break;
                    case "-":
                        this.Operator = BinaryOperator.Subtract;
                        break;
                    case "/":
                        this.Operator = BinaryOperator.Divide;
                        break;
                    case "*":
                        this.Operator = BinaryOperator.Multiply;
                        break;
                    default:
                        throw new FdoParseException("Unknown binary operator: " + opStr);
                }
                this.Right = FdoExpression.ParseNode(node.ChildNodes[2]);
            }
        }
    }
}
