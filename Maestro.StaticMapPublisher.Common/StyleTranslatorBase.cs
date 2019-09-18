#region Disclaimer / License

// Copyright (C) 2019, Jackie Ng
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

namespace Maestro.StaticMapPublisher.Common
{
    public abstract class StyleTranslatorBase
    {
        protected const string INDENT = "    ";

        protected static string ToHtmlColor(string color)
        {
            return color.Substring(2);
        }

        protected abstract string TryTranslateIdentifier(FdoIdentifier ident, string featureVar);

        protected string TryTranslateExpression(FdoExpression expr, string featureVar)
        {
            switch (expr)
            {
                case FdoIdentifier ident:
                    return TryTranslateIdentifier(ident, featureVar);
                case FdoStringValue strVal:
                    return $"('{strVal.Value}')";
                case FdoInt32Value ival:
                    return $"{ival.Value}";
                case FdoDoubleValue dval:
                    return $"{dval.Value}";
                case FdoBooleanValue bval:
                    return bval.Value ? "true" : "false";
            }
            return null;
        }

        protected static string TryTranslateComparsionOperator(ComparisonOperations op)
        {
            switch (op)
            {
                case ComparisonOperations.EqualsTo:
                    return " == ";
                case ComparisonOperations.GreaterThan:
                    return " > ";
                case ComparisonOperations.GreaterThanOrEqualTo:
                    return " >= ";
                case ComparisonOperations.LessThan:
                    return " < ";
                case ComparisonOperations.LessThanOrEqualTo:
                    return " <= ";
                case ComparisonOperations.Like:
                    return null;
                case ComparisonOperations.NotEqualsTo:
                    return " != ";
            }

            return null;
        }

        protected string TryTranslateFdoFilter(FdoFilter filter, string featureVar)
        {
            switch (filter)
            {
                case FdoComparisonCondition cmpCond:
                    {
                        var lhs = TryTranslateExpression(cmpCond.Left, featureVar);
                        var rhs = TryTranslateExpression(cmpCond.Right, featureVar);
                        var op = TryTranslateComparsionOperator(cmpCond.Operator);
                        if (lhs != null && rhs != null && op != null)
                        {
                            return lhs + op + rhs;
                        }
                        return null;
                    }
                default:
                    return null;
            }
        }
    }
}
