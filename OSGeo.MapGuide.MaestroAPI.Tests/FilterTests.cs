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
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class FilterTests
    {
        [Fact]
        public void TestFilterParse()
        {
            FdoFilter filter = null;
            FdoFilter left = null;
            FdoFilter right = null;

            filter = FdoFilter.Parse("A = 'B'");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A = 'B')");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("A <> 'B'");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.NotEqualsTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A <> 'B')");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.NotEqualsTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("A > 2.3");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.GreaterThan, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A > 2.3)");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.GreaterThan, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("A >= 2.3");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.GreaterThanOrEqualTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A >= 2.3)");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.GreaterThanOrEqualTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("A < 2.3");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.LessThan, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A < 2.3)");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.LessThan, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("A <= 2.3");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.LessThanOrEqualTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A <= 2.3)");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.LessThanOrEqualTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("A LIKE 'B%'");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.Like, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A LIKE 'B%')");
            Assert.IsAssignableFrom<FdoComparisonCondition>(filter);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.Equal(ComparisonOperations.Like, ((FdoComparisonCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("NOT (A LIKE 'B%')");
            Assert.IsAssignableFrom<FdoUnaryLogicalOperator>(filter);
            var subExpr = ((FdoUnaryLogicalOperator)filter).NegatedFilter;
            Assert.IsAssignableFrom<FdoComparisonCondition>(subExpr);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)subExpr).Left);
            Assert.Equal(ComparisonOperations.Like, ((FdoComparisonCondition)subExpr).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)subExpr).Right);

            filter = FdoFilter.Parse("NAME NULL");
            Assert.IsAssignableFrom<FdoNullCondition>(filter);
            Assert.Equal("NAME", ((FdoNullCondition)filter).Identifier.Name);

            filter = FdoFilter.Parse("NOT NAME NULL");
            Assert.IsAssignableFrom<FdoUnaryLogicalOperator>(filter);
            subExpr = ((FdoUnaryLogicalOperator)filter).NegatedFilter;
            Assert.Equal("NAME", ((FdoNullCondition)subExpr).Identifier.Name);

            filter = FdoFilter.Parse("A = 'B' OR B = 'C'");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsAssignableFrom<FdoBinaryLogicalOperator>(filter);
            Assert.IsAssignableFrom<FdoComparisonCondition>(left);
            Assert.Equal(BinaryLogicalOperations.Or, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsAssignableFrom<FdoComparisonCondition>(right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.Equal(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)left).Right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.Equal(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)right).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("A = 'B' AND B = 'C'");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsAssignableFrom<FdoBinaryLogicalOperator>(filter);
            Assert.IsAssignableFrom<FdoComparisonCondition>(left);
            Assert.Equal(BinaryLogicalOperations.And, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsAssignableFrom<FdoComparisonCondition>(right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.Equal(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)left).Right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.Equal(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)right).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A = 'B') OR (B = 'C')");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsAssignableFrom<FdoBinaryLogicalOperator>(filter);
            Assert.IsAssignableFrom<FdoComparisonCondition>(left);
            Assert.Equal(BinaryLogicalOperations.Or, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsAssignableFrom<FdoComparisonCondition>(right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.Equal(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)left).Right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.Equal(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)right).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A = 'B') AND (B = 'C')");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsAssignableFrom<FdoBinaryLogicalOperator>(filter);
            Assert.IsAssignableFrom<FdoComparisonCondition>(left);
            Assert.Equal(BinaryLogicalOperations.And, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsAssignableFrom<FdoComparisonCondition>(right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.Equal(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)left).Right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.Equal(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)right).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A <> 'B') OR (B LIKE 'C%')");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsAssignableFrom<FdoBinaryLogicalOperator>(filter);
            Assert.IsAssignableFrom<FdoComparisonCondition>(left);
            Assert.Equal(BinaryLogicalOperations.Or, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsAssignableFrom<FdoComparisonCondition>(right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.Equal(ComparisonOperations.NotEqualsTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)left).Right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.Equal(ComparisonOperations.Like, ((FdoComparisonCondition)right).Operator);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A < 2.2) OR (B > 1.4)");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsAssignableFrom<FdoBinaryLogicalOperator>(filter);
            Assert.IsAssignableFrom<FdoComparisonCondition>(left);
            Assert.Equal(BinaryLogicalOperations.Or, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsAssignableFrom<FdoComparisonCondition>(right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.Equal(ComparisonOperations.LessThan, ((FdoComparisonCondition)left).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)left).Right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.Equal(ComparisonOperations.GreaterThan, ((FdoComparisonCondition)right).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A < 2.2) AND (B > 1.4)");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsAssignableFrom<FdoBinaryLogicalOperator>(filter);
            Assert.IsAssignableFrom<FdoComparisonCondition>(left);
            Assert.Equal(BinaryLogicalOperations.And, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsAssignableFrom<FdoComparisonCondition>(right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.Equal(ComparisonOperations.LessThan, ((FdoComparisonCondition)left).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)left).Right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.Equal(ComparisonOperations.GreaterThan, ((FdoComparisonCondition)right).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A <= 2.2) OR (B >= 1.4)");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsAssignableFrom<FdoBinaryLogicalOperator>(filter);
            Assert.IsAssignableFrom<FdoComparisonCondition>(left);
            Assert.Equal(BinaryLogicalOperations.Or, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsAssignableFrom<FdoComparisonCondition>(right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.Equal(ComparisonOperations.LessThanOrEqualTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)left).Right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.Equal(ComparisonOperations.GreaterThanOrEqualTo, ((FdoComparisonCondition)right).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A <= 2.2) AND (B >= 1.4)");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsAssignableFrom<FdoBinaryLogicalOperator>(filter);
            Assert.IsAssignableFrom<FdoComparisonCondition>(left);
            Assert.Equal(BinaryLogicalOperations.And, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsAssignableFrom<FdoComparisonCondition>(right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.Equal(ComparisonOperations.LessThanOrEqualTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)left).Right);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.Equal(ComparisonOperations.GreaterThanOrEqualTo, ((FdoComparisonCondition)right).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("A IN ('A', 'B', 'C')");
            Assert.IsAssignableFrom<FdoInCondition>(filter);
            Assert.Equal("A", ((FdoInCondition)filter).Identifier.Name);
            Assert.Equal(3, ((FdoInCondition)filter).ValueList.Count);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoInCondition)filter).ValueList[0]);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoInCondition)filter).ValueList[1]);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoInCondition)filter).ValueList[2]);

            filter = FdoFilter.Parse("A IN (1, 2.0, '3', TRUE, FALSE)");
            Assert.IsAssignableFrom<FdoInCondition>(filter);
            Assert.Equal("A", ((FdoInCondition)filter).Identifier.Name);
            Assert.Equal(5, ((FdoInCondition)filter).ValueList.Count);
            Assert.IsAssignableFrom<FdoInt32Value>(((FdoInCondition)filter).ValueList[0]);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoInCondition)filter).ValueList[1]);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoInCondition)filter).ValueList[2]);
            Assert.IsAssignableFrom<FdoBooleanValue>(((FdoInCondition)filter).ValueList[3]);
            Assert.IsAssignableFrom<FdoBooleanValue>(((FdoInCondition)filter).ValueList[4]);

            filter = FdoFilter.Parse("Geometry CONTAINS GeomFromText('POINT (1 1)')");
            Assert.IsAssignableFrom<FdoSpatialCondition>(filter);
            Assert.Equal("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.Equal(SpatialOperations.Contains, ((FdoSpatialCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry CROSSES GeomFromText('POINT (1 1)')");
            Assert.IsAssignableFrom<FdoSpatialCondition>(filter);
            Assert.Equal("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.Equal(SpatialOperations.Crosses, ((FdoSpatialCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry DISJOINT GeomFromText('POINT (1 1)')");
            Assert.IsAssignableFrom<FdoSpatialCondition>(filter);
            Assert.Equal("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.Equal(SpatialOperations.Disjoint, ((FdoSpatialCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry EQUALS GeomFromText('POINT (1 1)')");
            Assert.IsAssignableFrom<FdoSpatialCondition>(filter);
            Assert.Equal("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.Equal(SpatialOperations.Equals, ((FdoSpatialCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry INTERSECTS GeomFromText('POINT (1 1)')");
            Assert.IsAssignableFrom<FdoSpatialCondition>(filter);
            Assert.Equal("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.Equal(SpatialOperations.Intersects, ((FdoSpatialCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry OVERLAPS GeomFromText('POINT (1 1)')");
            Assert.IsAssignableFrom<FdoSpatialCondition>(filter);
            Assert.Equal("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.Equal(SpatialOperations.Overlaps, ((FdoSpatialCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry TOUCHES GeomFromText('POINT (1 1)')");
            Assert.IsAssignableFrom<FdoSpatialCondition>(filter);
            Assert.Equal("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.Equal(SpatialOperations.Touches, ((FdoSpatialCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry WITHIN GeomFromText('POINT (1 1)')");
            Assert.IsAssignableFrom<FdoSpatialCondition>(filter);
            Assert.Equal("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.Equal(SpatialOperations.Within, ((FdoSpatialCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry COVEREDBY GeomFromText('POINT (1 1)')");
            Assert.IsAssignableFrom<FdoSpatialCondition>(filter);
            Assert.Equal("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.Equal(SpatialOperations.CoveredBy, ((FdoSpatialCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry INSIDE GeomFromText('POINT (1 1)')");
            Assert.IsAssignableFrom<FdoSpatialCondition>(filter);
            Assert.Equal("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.Equal(SpatialOperations.Inside, ((FdoSpatialCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry BEYOND GeomFromText('POINT (1 1)') 2.0");
            Assert.IsAssignableFrom<FdoDistanceCondition>(filter);
            Assert.Equal("Geometry", ((FdoDistanceCondition)filter).Identifier.Name);
            Assert.Equal(DistanceOperations.Beyond, ((FdoDistanceCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoDistanceCondition)filter).Expression);
            Assert.Equal(DataType.Double, ((FdoDistanceCondition)filter).Distance.DataType);
            Assert.Equal(2.0, ((FdoDoubleValue)((FdoDistanceCondition)filter).Distance).Value);

            filter = FdoFilter.Parse("Geometry BEYOND GeomFromText('POINT (1 1)') 5");
            Assert.IsAssignableFrom<FdoDistanceCondition>(filter);
            Assert.Equal("Geometry", ((FdoDistanceCondition)filter).Identifier.Name);
            Assert.Equal(DistanceOperations.Beyond, ((FdoDistanceCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoDistanceCondition)filter).Expression);
            Assert.Equal(DataType.Int32, ((FdoDistanceCondition)filter).Distance.DataType);
            Assert.Equal(5, ((FdoInt32Value)((FdoDistanceCondition)filter).Distance).Value);

            filter = FdoFilter.Parse("Geometry WITHINDISTANCE GeomFromText('POINT (1 1)') 2.0");
            Assert.IsAssignableFrom<FdoDistanceCondition>(filter);
            Assert.Equal("Geometry", ((FdoDistanceCondition)filter).Identifier.Name);
            Assert.Equal(DistanceOperations.WithinDistance, ((FdoDistanceCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoDistanceCondition)filter).Expression);
            Assert.Equal(DataType.Double, ((FdoDistanceCondition)filter).Distance.DataType);
            Assert.Equal(2.0, ((FdoDoubleValue)((FdoDistanceCondition)filter).Distance).Value);

            filter = FdoFilter.Parse("Geometry WITHINDISTANCE GeomFromText('POINT (1 1)') 5");
            Assert.IsAssignableFrom<FdoDistanceCondition>(filter);
            Assert.Equal("Geometry", ((FdoDistanceCondition)filter).Identifier.Name);
            Assert.Equal(DistanceOperations.WithinDistance, ((FdoDistanceCondition)filter).Operator);
            Assert.IsAssignableFrom<FdoGeometryValue>(((FdoDistanceCondition)filter).Expression);
            Assert.Equal(DataType.Int32, ((FdoDistanceCondition)filter).Distance.DataType);
            Assert.Equal(5, ((FdoInt32Value)((FdoDistanceCondition)filter).Distance).Value);
        }
    }
}
