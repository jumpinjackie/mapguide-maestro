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
    public class ExpressionTests
    {
        [Fact]
        public void TestExpressionParse()
        {
            FdoExpression expr = null;

            expr = FdoExpression.Parse("RNAME");
            Assert.IsAssignableFrom<FdoIdentifier>(expr);
            Assert.Equal("RNAME", ((FdoIdentifier)expr).Name);

            expr = FdoExpression.Parse("TRUE");
            Assert.IsAssignableFrom<FdoBooleanValue>(expr);

            expr = FdoExpression.Parse("FALSE");
            Assert.IsAssignableFrom<FdoBooleanValue>(expr);
            
            expr = FdoExpression.Parse(":Foo");
            Assert.IsAssignableFrom<FdoParameter>(expr);
            Assert.Equal("Foo", ((FdoParameter)expr).Name);
            
            expr = FdoExpression.Parse(":Foo_bar");
            Assert.Equal("Foo_bar", ((FdoParameter)expr).Name);
            Assert.IsAssignableFrom<FdoParameter>(expr);

            expr = FdoExpression.Parse("1");
            Assert.IsAssignableFrom<FdoInt32Value>(expr);
            Assert.Equal(1, ((FdoInt32Value)expr).Value);

            expr = FdoExpression.Parse("-1");
            Assert.IsAssignableFrom<FdoInt32Value>(expr);
            Assert.Equal(-1, ((FdoInt32Value)expr).Value);
            
            expr = FdoExpression.Parse("1 + 2.2");
            Assert.IsAssignableFrom<FdoBinaryExpression>(expr);
            Assert.IsAssignableFrom<FdoInt32Value>(((FdoBinaryExpression)expr).Left);
            Assert.Equal(BinaryOperator.Add, ((FdoBinaryExpression)expr).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoBinaryExpression)expr).Right);

            expr = FdoExpression.Parse("1 * 2.2");
            Assert.IsAssignableFrom<FdoBinaryExpression>(expr);
            Assert.IsAssignableFrom<FdoInt32Value>(((FdoBinaryExpression)expr).Left);
            Assert.Equal(BinaryOperator.Multiply, ((FdoBinaryExpression)expr).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoBinaryExpression)expr).Right);

            expr = FdoExpression.Parse("1 / 2.2");
            Assert.IsAssignableFrom<FdoBinaryExpression>(expr);
            Assert.IsAssignableFrom<FdoInt32Value>(((FdoBinaryExpression)expr).Left);
            Assert.Equal(BinaryOperator.Divide, ((FdoBinaryExpression)expr).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoBinaryExpression)expr).Right);

            expr = FdoExpression.Parse("1 - 2.2");
            Assert.IsAssignableFrom<FdoBinaryExpression>(expr);
            Assert.IsAssignableFrom<FdoInt32Value>(((FdoBinaryExpression)expr).Left);
            Assert.Equal(BinaryOperator.Subtract, ((FdoBinaryExpression)expr).Operator);
            Assert.IsAssignableFrom<FdoDoubleValue>(((FdoBinaryExpression)expr).Right);

            expr = FdoExpression.Parse("GeomFromText('POINT (1 1)')");
            Assert.IsAssignableFrom<FdoGeometryValue>(expr);
            Assert.Equal("POINT (1 1)", ((FdoGeometryValue)expr).GeometryWkt);

            expr = FdoExpression.Parse("DATE '1971-12-24'");
            Assert.IsAssignableFrom<FdoDateTimeValue>(expr);
            Assert.True(((FdoDateTimeValue)expr).DateTime.HasValue);
            Assert.False(((FdoDateTimeValue)expr).Time.HasValue);
            Assert.Equal(1971, ((FdoDateTimeValue)expr).DateTime.Value.Year);
            Assert.Equal(12, ((FdoDateTimeValue)expr).DateTime.Value.Month);
            Assert.Equal(24, ((FdoDateTimeValue)expr).DateTime.Value.Day);

            expr = FdoExpression.Parse("TIME '11:23:43'");
            Assert.IsAssignableFrom<FdoDateTimeValue>(expr);
            Assert.False(((FdoDateTimeValue)expr).DateTime.HasValue);
            Assert.True(((FdoDateTimeValue)expr).Time.HasValue);
            Assert.Equal(11, ((FdoDateTimeValue)expr).Time.Value.Hours);
            Assert.Equal(23, ((FdoDateTimeValue)expr).Time.Value.Minutes);
            Assert.Equal(43, ((FdoDateTimeValue)expr).Time.Value.Seconds);

            expr = FdoExpression.Parse("TIME '11:23:43.123'");
            Assert.IsAssignableFrom<FdoDateTimeValue>(expr);
            Assert.False(((FdoDateTimeValue)expr).DateTime.HasValue);
            Assert.True(((FdoDateTimeValue)expr).Time.HasValue);
            Assert.Equal(11, ((FdoDateTimeValue)expr).Time.Value.Hours);
            Assert.Equal(23, ((FdoDateTimeValue)expr).Time.Value.Minutes);
            Assert.Equal(43, ((FdoDateTimeValue)expr).Time.Value.Seconds);
            Assert.Equal(123, ((FdoDateTimeValue)expr).Time.Value.Milliseconds);
            
            expr = FdoExpression.Parse("TIMESTAMP '2003-10-23 11:00:02'");
            Assert.IsAssignableFrom<FdoDateTimeValue>(expr);
            Assert.True(((FdoDateTimeValue)expr).DateTime.HasValue);
            Assert.False(((FdoDateTimeValue)expr).Time.HasValue);
            Assert.Equal(2003, ((FdoDateTimeValue)expr).DateTime.Value.Year);
            Assert.Equal(10, ((FdoDateTimeValue)expr).DateTime.Value.Month);
            Assert.Equal(23, ((FdoDateTimeValue)expr).DateTime.Value.Day);
            Assert.Equal(11, ((FdoDateTimeValue)expr).DateTime.Value.Hour);
            Assert.Equal(00, ((FdoDateTimeValue)expr).DateTime.Value.Minute);
            Assert.Equal(02, ((FdoDateTimeValue)expr).DateTime.Value.Second);

            expr = FdoExpression.Parse("TIMESTAMP '2003-10-23 11:00:02.123'");
            Assert.IsAssignableFrom<FdoDateTimeValue>(expr);
            Assert.True(((FdoDateTimeValue)expr).DateTime.HasValue);
            Assert.False(((FdoDateTimeValue)expr).Time.HasValue);
            Assert.Equal(2003, ((FdoDateTimeValue)expr).DateTime.Value.Year);
            Assert.Equal(10, ((FdoDateTimeValue)expr).DateTime.Value.Month);
            Assert.Equal(23, ((FdoDateTimeValue)expr).DateTime.Value.Day);
            Assert.Equal(11, ((FdoDateTimeValue)expr).DateTime.Value.Hour);
            Assert.Equal(00, ((FdoDateTimeValue)expr).DateTime.Value.Minute);
            Assert.Equal(02, ((FdoDateTimeValue)expr).DateTime.Value.Second);
            Assert.Equal(123, ((FdoDateTimeValue)expr).DateTime.Value.Millisecond);

            expr = FdoExpression.Parse("CurrentDate()");
            Assert.IsAssignableFrom<FdoFunction>(expr);
            Assert.Equal("CurrentDate", ((FdoFunction)expr).Identifier.Name);
            Assert.Empty(((FdoFunction)expr).Arguments);

            expr = FdoExpression.Parse("Concat(RNAME, ' ', RBILAD)");
            Assert.IsAssignableFrom<FdoFunction>(expr);
            Assert.Equal("Concat", ((FdoFunction)expr).Identifier.Name);
            Assert.Equal(3, ((FdoFunction)expr).Arguments.Count);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoFunction)expr).Arguments[0]);
            Assert.IsAssignableFrom<FdoStringValue>(((FdoFunction)expr).Arguments[1]);
            Assert.IsAssignableFrom<FdoIdentifier>(((FdoFunction)expr).Arguments[2]);

            expr = FdoExpression.Parse("-UnixTime()");
            Assert.IsAssignableFrom<FdoUnaryExpression>(expr);
        }
    }
}
