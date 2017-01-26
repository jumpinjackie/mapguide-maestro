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
using NUnit.Framework;
using OSGeo.FDO.Expressions;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    [TestFixture]
    public class FilterTests
    {
        [Test]
        public void TestFilterParse()
        {
            FdoFilter filter = null;
            FdoFilter left = null;
            FdoFilter right = null;

            filter = FdoFilter.Parse("A = 'B'");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A = 'B')");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("A <> 'B'");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.NotEqualsTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A <> 'B')");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.NotEqualsTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("A > 2.3");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.GreaterThan, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A > 2.3)");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.GreaterThan, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("A >= 2.3");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.GreaterThanOrEqualTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A >= 2.3)");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.GreaterThanOrEqualTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("A < 2.3");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.LessThan, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A < 2.3)");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.LessThan, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("A <= 2.3");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.LessThanOrEqualTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A <= 2.3)");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.LessThanOrEqualTo, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("A LIKE 'B%'");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.Like, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("(A LIKE 'B%')");
            Assert.IsInstanceOf<FdoComparisonCondition>(filter);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)filter).Left);
            Assert.AreEqual(ComparisonOperations.Like, ((FdoComparisonCondition)filter).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)filter).Right);

            filter = FdoFilter.Parse("NOT (A LIKE 'B%')");
            Assert.IsInstanceOf<FdoUnaryLogicalOperator>(filter);
            var subExpr = ((FdoUnaryLogicalOperator)filter).NegatedFilter;
            Assert.IsInstanceOf<FdoComparisonCondition>(subExpr);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)subExpr).Left);
            Assert.AreEqual(ComparisonOperations.Like, ((FdoComparisonCondition)subExpr).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)subExpr).Right);

            filter = FdoFilter.Parse("NAME NULL");
            Assert.IsInstanceOf<FdoNullCondition>(filter);
            Assert.AreEqual("NAME", ((FdoNullCondition)filter).Identifier.Name);

            filter = FdoFilter.Parse("NOT NAME NULL");
            Assert.IsInstanceOf<FdoUnaryLogicalOperator>(filter);
            subExpr = ((FdoUnaryLogicalOperator)filter).NegatedFilter;
            Assert.AreEqual("NAME", ((FdoNullCondition)subExpr).Identifier.Name);

            filter = FdoFilter.Parse("A = 'B' OR B = 'C'");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsInstanceOf<FdoBinaryLogicalOperator>(filter);
            Assert.IsInstanceOf<FdoComparisonCondition>(left);
            Assert.AreEqual(BinaryLogicalOperations.Or, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsInstanceOf<FdoComparisonCondition>(right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.AreEqual(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)left).Right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.AreEqual(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)right).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("A = 'B' AND B = 'C'");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsInstanceOf<FdoBinaryLogicalOperator>(filter);
            Assert.IsInstanceOf<FdoComparisonCondition>(left);
            Assert.AreEqual(BinaryLogicalOperations.And, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsInstanceOf<FdoComparisonCondition>(right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.AreEqual(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)left).Right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.AreEqual(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)right).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A = 'B') OR (B = 'C')");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsInstanceOf<FdoBinaryLogicalOperator>(filter);
            Assert.IsInstanceOf<FdoComparisonCondition>(left);
            Assert.AreEqual(BinaryLogicalOperations.Or, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsInstanceOf<FdoComparisonCondition>(right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.AreEqual(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)left).Right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.AreEqual(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)right).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A = 'B') AND (B = 'C')");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsInstanceOf<FdoBinaryLogicalOperator>(filter);
            Assert.IsInstanceOf<FdoComparisonCondition>(left);
            Assert.AreEqual(BinaryLogicalOperations.And, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsInstanceOf<FdoComparisonCondition>(right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.AreEqual(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)left).Right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.AreEqual(ComparisonOperations.EqualsTo, ((FdoComparisonCondition)right).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A <> 'B') OR (B LIKE 'C%')");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsInstanceOf<FdoBinaryLogicalOperator>(filter);
            Assert.IsInstanceOf<FdoComparisonCondition>(left);
            Assert.AreEqual(BinaryLogicalOperations.Or, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsInstanceOf<FdoComparisonCondition>(right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.AreEqual(ComparisonOperations.NotEqualsTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)left).Right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.AreEqual(ComparisonOperations.Like, ((FdoComparisonCondition)right).Operator);
            Assert.IsInstanceOf<FdoStringValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A < 2.2) OR (B > 1.4)");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsInstanceOf<FdoBinaryLogicalOperator>(filter);
            Assert.IsInstanceOf<FdoComparisonCondition>(left);
            Assert.AreEqual(BinaryLogicalOperations.Or, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsInstanceOf<FdoComparisonCondition>(right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.AreEqual(ComparisonOperations.LessThan, ((FdoComparisonCondition)left).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)left).Right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.AreEqual(ComparisonOperations.GreaterThan, ((FdoComparisonCondition)right).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A < 2.2) AND (B > 1.4)");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsInstanceOf<FdoBinaryLogicalOperator>(filter);
            Assert.IsInstanceOf<FdoComparisonCondition>(left);
            Assert.AreEqual(BinaryLogicalOperations.And, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsInstanceOf<FdoComparisonCondition>(right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.AreEqual(ComparisonOperations.LessThan, ((FdoComparisonCondition)left).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)left).Right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.AreEqual(ComparisonOperations.GreaterThan, ((FdoComparisonCondition)right).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A <= 2.2) OR (B >= 1.4)");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsInstanceOf<FdoBinaryLogicalOperator>(filter);
            Assert.IsInstanceOf<FdoComparisonCondition>(left);
            Assert.AreEqual(BinaryLogicalOperations.Or, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsInstanceOf<FdoComparisonCondition>(right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.AreEqual(ComparisonOperations.LessThanOrEqualTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)left).Right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.AreEqual(ComparisonOperations.GreaterThanOrEqualTo, ((FdoComparisonCondition)right).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("(A <= 2.2) AND (B >= 1.4)");
            left = ((FdoBinaryLogicalOperator)filter).Left;
            right = ((FdoBinaryLogicalOperator)filter).Right;
            Assert.IsInstanceOf<FdoBinaryLogicalOperator>(filter);
            Assert.IsInstanceOf<FdoComparisonCondition>(left);
            Assert.AreEqual(BinaryLogicalOperations.And, ((FdoBinaryLogicalOperator)filter).Operator);
            Assert.IsInstanceOf<FdoComparisonCondition>(right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)left).Left);
            Assert.AreEqual(ComparisonOperations.LessThanOrEqualTo, ((FdoComparisonCondition)left).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)left).Right);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoComparisonCondition)right).Left);
            Assert.AreEqual(ComparisonOperations.GreaterThanOrEqualTo, ((FdoComparisonCondition)right).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoComparisonCondition)right).Right);

            filter = FdoFilter.Parse("A IN ('A', 'B', 'C')");
            Assert.IsInstanceOf<FdoInCondition>(filter);
            Assert.AreEqual("A", ((FdoInCondition)filter).Identifier.Name);
            Assert.AreEqual(3, ((FdoInCondition)filter).ValueList.Count);
            Assert.IsInstanceOf<FdoStringValue>(((FdoInCondition)filter).ValueList[0]);
            Assert.IsInstanceOf<FdoStringValue>(((FdoInCondition)filter).ValueList[1]);
            Assert.IsInstanceOf<FdoStringValue>(((FdoInCondition)filter).ValueList[2]);

            filter = FdoFilter.Parse("A IN (1, 2.0, '3', TRUE, FALSE)");
            Assert.IsInstanceOf<FdoInCondition>(filter);
            Assert.AreEqual("A", ((FdoInCondition)filter).Identifier.Name);
            Assert.AreEqual(5, ((FdoInCondition)filter).ValueList.Count);
            Assert.IsInstanceOf<FdoInt32Value>(((FdoInCondition)filter).ValueList[0]);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoInCondition)filter).ValueList[1]);
            Assert.IsInstanceOf<FdoStringValue>(((FdoInCondition)filter).ValueList[2]);
            Assert.IsInstanceOf<FdoBooleanValue>(((FdoInCondition)filter).ValueList[3]);
            Assert.IsInstanceOf<FdoBooleanValue>(((FdoInCondition)filter).ValueList[4]);

            filter = FdoFilter.Parse("Geometry CONTAINS GeomFromText('POINT (1 1)')");
            Assert.IsInstanceOf<FdoSpatialCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.AreEqual(SpatialOperations.Contains, ((FdoSpatialCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry CROSSES GeomFromText('POINT (1 1)')");
            Assert.IsInstanceOf<FdoSpatialCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.AreEqual(SpatialOperations.Crosses, ((FdoSpatialCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry DISJOINT GeomFromText('POINT (1 1)')");
            Assert.IsInstanceOf<FdoSpatialCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.AreEqual(SpatialOperations.Disjoint, ((FdoSpatialCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry EQUALS GeomFromText('POINT (1 1)')");
            Assert.IsInstanceOf<FdoSpatialCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.AreEqual(SpatialOperations.Equals, ((FdoSpatialCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry INTERSECTS GeomFromText('POINT (1 1)')");
            Assert.IsInstanceOf<FdoSpatialCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.AreEqual(SpatialOperations.Intersects, ((FdoSpatialCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry OVERLAPS GeomFromText('POINT (1 1)')");
            Assert.IsInstanceOf<FdoSpatialCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.AreEqual(SpatialOperations.Overlaps, ((FdoSpatialCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry TOUCHES GeomFromText('POINT (1 1)')");
            Assert.IsInstanceOf<FdoSpatialCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.AreEqual(SpatialOperations.Touches, ((FdoSpatialCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry WITHIN GeomFromText('POINT (1 1)')");
            Assert.IsInstanceOf<FdoSpatialCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.AreEqual(SpatialOperations.Within, ((FdoSpatialCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry COVEREDBY GeomFromText('POINT (1 1)')");
            Assert.IsInstanceOf<FdoSpatialCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.AreEqual(SpatialOperations.CoveredBy, ((FdoSpatialCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry INSIDE GeomFromText('POINT (1 1)')");
            Assert.IsInstanceOf<FdoSpatialCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoSpatialCondition)filter).Identifier.Name);
            Assert.AreEqual(SpatialOperations.Inside, ((FdoSpatialCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoSpatialCondition)filter).Expression);

            filter = FdoFilter.Parse("Geometry BEYOND GeomFromText('POINT (1 1)') 2.0");
            Assert.IsInstanceOf<FdoDistanceCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoDistanceCondition)filter).Identifier.Name);
            Assert.AreEqual(DistanceOperations.Beyond, ((FdoDistanceCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoDistanceCondition)filter).Expression);
            Assert.AreEqual(DataType.Double, ((FdoDistanceCondition)filter).Distance.DataType);
            Assert.AreEqual(2.0, ((FdoDoubleValue)((FdoDistanceCondition)filter).Distance).Value);

            filter = FdoFilter.Parse("Geometry BEYOND GeomFromText('POINT (1 1)') 5");
            Assert.IsInstanceOf<FdoDistanceCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoDistanceCondition)filter).Identifier.Name);
            Assert.AreEqual(DistanceOperations.Beyond, ((FdoDistanceCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoDistanceCondition)filter).Expression);
            Assert.AreEqual(DataType.Int32, ((FdoDistanceCondition)filter).Distance.DataType);
            Assert.AreEqual(5, ((FdoInt32Value)((FdoDistanceCondition)filter).Distance).Value);

            filter = FdoFilter.Parse("Geometry WITHINDISTANCE GeomFromText('POINT (1 1)') 2.0");
            Assert.IsInstanceOf<FdoDistanceCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoDistanceCondition)filter).Identifier.Name);
            Assert.AreEqual(DistanceOperations.WithinDistance, ((FdoDistanceCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoDistanceCondition)filter).Expression);
            Assert.AreEqual(DataType.Double, ((FdoDistanceCondition)filter).Distance.DataType);
            Assert.AreEqual(2.0, ((FdoDoubleValue)((FdoDistanceCondition)filter).Distance).Value);

            filter = FdoFilter.Parse("Geometry WITHINDISTANCE GeomFromText('POINT (1 1)') 5");
            Assert.IsInstanceOf<FdoDistanceCondition>(filter);
            Assert.AreEqual("Geometry", ((FdoDistanceCondition)filter).Identifier.Name);
            Assert.AreEqual(DistanceOperations.WithinDistance, ((FdoDistanceCondition)filter).Operator);
            Assert.IsInstanceOf<FdoGeometryValue>(((FdoDistanceCondition)filter).Expression);
            Assert.AreEqual(DataType.Int32, ((FdoDistanceCondition)filter).Distance.DataType);
            Assert.AreEqual(5, ((FdoInt32Value)((FdoDistanceCondition)filter).Distance).Value);
        }
    }
}
