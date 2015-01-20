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
using OSGeo.FDO.Expressions;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestro.Editors.Common.Expression
{
    [Serializable]
    public class FdoExpressionValidationException : FdoParseException
    {
        private FdoExpressionValidationException(string message) : base(message) { }
        public FdoExpressionValidationException(string message, Exception inner) : base(message, inner) { }
        protected FdoExpressionValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// Gets the contextual token that is the cause of the validation error
        /// </summary>
        public string Token { get; private set; }

        public FdoExpressionValidationException(string message, string token) 
            : this(message)
        {
            this.Token = token;
        }
    }

    public class FdoExpressionValidator
    {
        public void ValidateExpression(FdoExpression expr, ClassDefinition cls, IFdoProviderCapabilities caps)
        {
            switch (expr.ExpressionType)
            {
                case ExpressionType.BinaryExpression:
                    {
                        var binExpr = (FdoBinaryExpression)expr;
                        ValidateExpression(binExpr.Left, cls, caps);
                        ValidateExpression(binExpr.Right, cls, caps);
                    }
                    break;
                case ExpressionType.UnaryExpression:
                    {
                        var unExpr = (FdoUnaryExpression)expr;
                        ValidateExpression(unExpr.Expression, cls, caps);
                    }
                    break;
                case ExpressionType.Identifier:
                    {
                        ValidateIdentifier(((FdoIdentifier)expr), cls);
                    }
                    break;
                case ExpressionType.Function:
                    {
                        var func = ((FdoFunction)expr);
                        string name = func.Identifier.Name;
                        if (!caps.Expression.SupportedFunctions.Any(f => f.Name.ToUpper() == name.ToUpper()))
                            throw new FdoExpressionValidationException(string.Format(Strings.InvalidExpressionUnsupportedFunction, name), name);

                        foreach (var arg in func.Arguments)
                        {
                            ValidateExpression(arg, cls, caps);
                        }
                    }
                    break;
            }
        }

        private static void ValidateIdentifier(FdoIdentifier expr, ClassDefinition cls)
        {
            string name = expr.Name;
            if (cls.FindProperty(name) == null)
                throw new FdoExpressionValidationException(string.Format(Strings.InvalidExpressionPropertyNotFound, name), name);
        }

        public void ValidateFilter(FdoFilter filter, ClassDefinition cls, IFdoProviderCapabilities caps)
        {
            switch (filter.FilterType)
            {
                case FilterType.BinaryLogicalOperator:
                    {
                        var binOp = (FdoBinaryLogicalOperator)filter;
                        ValidateFilter(binOp.Left, cls, caps);
                        ValidateFilter(binOp.Right, cls, caps);
                    }
                    break;
                case FilterType.ComparisonCondition:
                    {
                        var compCond = (FdoComparisonCondition)filter;
                        if (compCond.Operator != ComparisonOperations.Like)
                        {
                            if (!caps.Filter.ConditionTypes.Select(o => o.ToUpper()).Contains("COMPARISON")) //NOXLATE
                                throw new FdoExpressionValidationException(string.Format(Strings.InvalidFilterUnsupportedConditionType, compCond.Operator), compCond.Operator.ToString());
                        }
                        else //Like
                        {
                            if (!caps.Filter.ConditionTypes.Select(o => o.ToUpper()).Any(o => o == compCond.Operator.ToString().ToUpper()))
                                throw new FdoExpressionValidationException(string.Format(Strings.InvalidFilterUnsupportedConditionType, compCond.Operator), compCond.Operator.ToString());
                        }
                        ValidateExpression(compCond.Left, cls, caps);
                        ValidateExpression(compCond.Right, cls, caps);
                    }
                    break;
                case FilterType.DistanceCondition:
                    {
                        var distCond = (FdoDistanceCondition)filter;
                        if (!caps.Filter.DistanceOperations.Select(o => o.ToUpper()).Any(o => o == distCond.Operator.ToString().ToUpper()))
                            throw new FdoExpressionValidationException(string.Format(Strings.InvalidFilterUnsupportedDistanceOperator, distCond.Operator), distCond.Operator.ToString());
                        ValidateIdentifier(distCond.Identifier, cls);
                        ValidateExpression(distCond.Expression, cls, caps);
                    }
                    break;
                case FilterType.InCondition:
                    {
                        if (!caps.Filter.ConditionTypes.Select(o => o.ToUpper()).Contains("IN")) //NOXLATE
                            throw new FdoExpressionValidationException(string.Format(Strings.InvalidFilterUnsupportedConditionType, "IN"), "IN"); //NOXLATE
                        var inCond = (FdoInCondition)filter;
                        ValidateIdentifier(inCond.Identifier, cls);
                        foreach (var val in inCond.ValueList)
                        {
                            ValidateExpression(val, cls, caps);
                        }
                    }
                    break;
                case FilterType.NullCondition:
                    {
                        if (!caps.Filter.ConditionTypes.Select(o => o.ToUpper()).Contains("NULL")) //NOXLATE
                            throw new FdoExpressionValidationException(string.Format(Strings.InvalidFilterUnsupportedConditionType, "NULL"), "NULL"); //NOXLATE
                        var nullCond = (FdoNullCondition)filter;
                        ValidateIdentifier(nullCond.Identifier, cls);
                    }
                    break;
                case FilterType.SpatialCondition:
                    {
                        var spFilter = (FdoSpatialCondition)filter;
                        if (!caps.Filter.ConditionTypes.Select(o => o.ToUpper()).Contains("SPATIAL")) //NOXLATE
                            throw new FdoExpressionValidationException(string.Format(Strings.InvalidFilterUnsupportedConditionType, "SPATIAL"), spFilter.Operator.ToString()); //NOXLATE
                        if (!caps.Filter.SpatialOperations.Select(o => o.ToUpper()).Any(o => o == spFilter.Operator.ToString().ToUpper()))
                            throw new FdoExpressionValidationException(string.Format(Strings.InvalidFilterUnsupportedSpatialOperator, spFilter.Operator), spFilter.Operator.ToString());

                        ValidateIdentifier(spFilter.Identifier, cls);
                        ValidateExpression(spFilter.Expression, cls, caps);
                    }
                    break;
                case FilterType.UnaryLogicalOperator:
                    {
                        var negatedFilter = ((FdoUnaryLogicalOperator)filter).NegatedFilter;
                        ValidateFilter(negatedFilter, cls, caps);
                    }
                    break;
            }
        }
    }
}
