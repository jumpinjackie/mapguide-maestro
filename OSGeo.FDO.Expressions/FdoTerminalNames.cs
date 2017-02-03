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

namespace OSGeo.FDO.Expressions
{
    internal static class FdoTerminalNames
    {
        public const string Identifier = nameof(Identifier);
        public const string String = nameof(String);
        public const string Integer = nameof(Integer);
        public const string Double = nameof(Double);

        public const string Expression = nameof(Expression);
        public const string UnaryExpression = nameof(UnaryExpression);
        public const string BinaryExpression = nameof(BinaryExpression);
        public const string ValueExpression = nameof(ValueExpression);
        public const string Function = nameof(Function);

        public const string DataValue = nameof(DataValue);
        public const string LiteralValue = nameof(LiteralValue);
        public const string Parameter = nameof(Parameter);
        public const string ExpressionCollection = nameof(ExpressionCollection);
        public const string GeometryValue = nameof(GeometryValue);

        public const string Boolean = nameof(Boolean);
        public const string DateTime = nameof(DateTime);

        public const string Filter = nameof(Filter);
        public const string LogicalOperator = nameof(LogicalOperator);
        public const string BinaryLogicalOperator = nameof(BinaryLogicalOperator);
        public const string UnaryLogicalOperator = nameof(UnaryLogicalOperator);
        public const string BinaryLogicalOperations = nameof(BinaryLogicalOperations);
        public const string InCondition = nameof(InCondition);
        public const string SearchCondition = nameof(SearchCondition);
        public const string ComparisonCondition = nameof(ComparisonCondition);
        public const string GeometricCondition = nameof(GeometricCondition);
        public const string NullCondition = nameof(NullCondition);
        public const string DistanceCondition = nameof(DistanceCondition);
        public const string SpatialCondition = nameof(SpatialCondition);

        public const string ValueExpressionCollection = nameof(ValueExpressionCollection);
        public const string ComparisonOperations = nameof(ComparisonOperations);
        public const string DistanceOperations = nameof(DistanceOperations);
        public const string Distance = nameof(Distance);
        public const string SpatialOperations = nameof(SpatialOperations);
    }
}
