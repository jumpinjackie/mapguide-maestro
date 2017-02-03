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
using OSGeo.FDO.Expressions;
using System;
using System.Windows.Forms;

namespace Maestro.Editors.Common.Expression
{
    internal partial class ExpressionDisplayDialog : Form
    {
        private ExpressionDisplayDialog()
        {
            InitializeComponent();
        }

        public ExpressionDisplayDialog(FdoParseable obj)
            : this()
        {
            try
            {
                trvStructure.BeginUpdate();
                trvStructure.Nodes.Add(LoadTree(obj));
            }
            finally
            {
                trvStructure.EndUpdate();
            }
        }

        private TreeNode LoadTree(FdoParseable obj)
        {
            if (obj.ParseableType == FdoParseableType.Filter)
                return LoadFilterTree((FdoFilter)obj);
            else //Expression
                return LoadExpressionTree((FdoExpression)obj);
        }

        private TreeNode LoadExpressionTree(FdoExpression expr)
        {
            TreeNode node = new TreeNode();
            switch (expr.ExpressionType)
            {
                case ExpressionType.BinaryExpression:
                    break;
                case ExpressionType.BooleanValue:
                    node.Text = string.Format(Strings.ExprDispBoolean, ((FdoBooleanValue)expr).Value);
                    break;
                case ExpressionType.DateTimeValue:
                    var dtVal = (FdoDateTimeValue)expr;
                    node.Text = string.Format(Strings.ExprDispDateTime, ((object)dtVal.DateTime ?? (object)dtVal.Time));
                    break;
                case ExpressionType.DoubleValue:
                    node.Text = string.Format(Strings.ExprDispDouble, ((FdoDoubleValue)expr).Value);
                    break;
                case ExpressionType.Function:
                    {
                        var func = (FdoFunction)expr;
                        node.Text = string.Format(Strings.ExprDispFunction, func.Identifier.Name);
                        foreach (var arg in func.Arguments)
                        {
                            node.Nodes.Add(LoadExpressionTree(arg));
                        }
                    }
                    break;
                case ExpressionType.GeometryValue:
                    node.Text = string.Format(Strings.ExprDispGeometry, ((FdoGeometryValue)expr).GeometryWkt);
                    break;
                case ExpressionType.Identifier:
                    node.Text = string.Format(Strings.ExprDispIdentifier, ((FdoIdentifier)expr).Name);
                    break;
                case ExpressionType.Int32Value:
                    node.Text = string.Format(Strings.ExprDispInt32, ((FdoInt32Value)expr).Value);
                    break;
                case ExpressionType.Parameter:
                    node.Text = string.Format(Strings.ExprDispParameter, ((FdoParameter)expr).Name);
                    break;
                case ExpressionType.StringValue:
                    node.Text = string.Format(Strings.ExprDispString, ((FdoStringValue)expr).Value);
                    break;
                case ExpressionType.UnaryExpression:
                    node.Text = Strings.ExprDispUnaryExpr;
                    node.Nodes.Add(LoadExpressionTree(((FdoUnaryExpression)expr).Expression));
                    break;
            }
            return node;
        }

        private TreeNode LoadFilterTree(FdoFilter filter)
        {
            TreeNode node = new TreeNode();
            switch (filter.FilterType)
            {
                case FilterType.BinaryLogicalOperator:
                    {
                        var binOp = (FdoBinaryLogicalOperator)filter;
                        node.Text = string.Format(Strings.FltrDispBinaryLogicalOperator, binOp.Operator);
                        var left = new TreeNode(Strings.FltrDispLeftNode);
                        var right = new TreeNode(Strings.FltrDispRightNode);
                        left.Nodes.Add(LoadFilterTree(binOp.Left));
                        right.Nodes.Add(LoadFilterTree(binOp.Right));
                        node.Nodes.Add(left);
                        node.Nodes.Add(right);
                    }
                    break;
                case FilterType.ComparisonCondition:
                    {
                        var compCond = (FdoComparisonCondition)filter;
                        node.Text = string.Format(Strings.FltrDispComparisonCondition, compCond.Operator);
                        var left = new TreeNode(Strings.FltrDispLeftNode);
                        var right = new TreeNode(Strings.FltrDispRightNode);
                        left.Nodes.Add(LoadExpressionTree(compCond.Left));
                        right.Nodes.Add(LoadExpressionTree(compCond.Right));
                        node.Nodes.Add(left);
                        node.Nodes.Add(right);
                    }
                    break;
                case FilterType.DistanceCondition:
                    {
                        var distCond = (FdoDistanceCondition)filter;
                        node.Text = string.Format(Strings.FdoDispDistanceCondition, distCond.Operator);
                        var ident = new TreeNode(Strings.FdoDispIdentifierTitle);
                        var expr = new TreeNode(Strings.FdoDispExprTitle);
                        var dist = new TreeNode(Strings.FdoDispDistanceTitle);
                        ident.Nodes.Add(LoadExpressionTree(distCond.Identifier));
                        expr.Nodes.Add(LoadExpressionTree(distCond.Expression));
                        dist.Nodes.Add(LoadExpressionTree(distCond.Distance));
                        node.Nodes.Add(ident);
                        node.Nodes.Add(expr);
                        node.Nodes.Add(dist);
                    }
                    break;
                case FilterType.InCondition:
                    {
                        var inCond = (FdoInCondition)filter;
                        node.Text = string.Format(Strings.FdoDispInCondition, inCond.Identifier.Name);
                        foreach (var arg in inCond.ValueList)
                        {
                            node.Nodes.Add(LoadExpressionTree(arg));
                        }
                    }
                    break;
                case FilterType.NullCondition:
                    {
                        var nullCond = (FdoNullCondition)filter;
                        node.Text = string.Format(Strings.FdoDispNullCondition, nullCond.Identifier.Name);
                    }
                    break;
                case FilterType.SpatialCondition:
                    {
                        var spCond = (FdoSpatialCondition)filter;
                        node.Text = string.Format(Strings.FdoDispSpatialCondition, spCond.Operator);
                        var ident = new TreeNode(Strings.FdoDispIdentifierTitle);
                        var exprNode = new TreeNode(Strings.FdoDispExprTitle);
                        ident.Nodes.Add(LoadExpressionTree(spCond.Identifier));
                        exprNode.Nodes.Add(LoadExpressionTree(spCond.Expression));
                        node.Nodes.Add(ident);
                        node.Nodes.Add(exprNode);
                    }
                    break;
                case FilterType.UnaryLogicalOperator:
                    {
                        var unCond = (FdoUnaryLogicalOperator)filter;
                        node.Text = Strings.FdoDispUnaryOperator;
                        node.Nodes.Add(LoadFilterTree(unCond.NegatedFilter));
                    }
                    break;
            }
            return node;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
