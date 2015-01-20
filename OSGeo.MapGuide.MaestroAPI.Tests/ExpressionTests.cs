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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSGeo.FDO.Expressions;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    [TestFixture]
    public class ExpressionTests
    {
        [Test]
        public void TestExpressionParse()
        {
            FdoExpression expr = null;

            expr = FdoExpression.Parse("RNAME");
            Assert.IsInstanceOf<FdoIdentifier>(expr);
            Assert.AreEqual("RNAME", ((FdoIdentifier)expr).Name);

            expr = FdoExpression.Parse("TRUE");
            Assert.IsInstanceOf<FdoBooleanValue>(expr);

            expr = FdoExpression.Parse("FALSE");
            Assert.IsInstanceOf<FdoBooleanValue>(expr);
            
            expr = FdoExpression.Parse(":Foo");
            Assert.IsInstanceOf<FdoParameter>(expr);
            Assert.AreEqual("Foo", ((FdoParameter)expr).Name);
            
            expr = FdoExpression.Parse(":Foo_bar");
            Assert.AreEqual("Foo_bar", ((FdoParameter)expr).Name);
            Assert.IsInstanceOf<FdoParameter>(expr);

            expr = FdoExpression.Parse("1");
            Assert.IsInstanceOf<FdoInt32Value>(expr);
            Assert.AreEqual(1, ((FdoInt32Value)expr).Value);

            expr = FdoExpression.Parse("-1");
            Assert.IsInstanceOf<FdoInt32Value>(expr);
            Assert.AreEqual(-1, ((FdoInt32Value)expr).Value);
            
            expr = FdoExpression.Parse("1 + 2.2");
            Assert.IsInstanceOf<FdoBinaryExpression>(expr);
            Assert.IsInstanceOf<FdoInt32Value>(((FdoBinaryExpression)expr).Left);
            Assert.AreEqual(BinaryOperator.Add, ((FdoBinaryExpression)expr).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoBinaryExpression)expr).Right);

            expr = FdoExpression.Parse("1 * 2.2");
            Assert.IsInstanceOf<FdoBinaryExpression>(expr);
            Assert.IsInstanceOf<FdoInt32Value>(((FdoBinaryExpression)expr).Left);
            Assert.AreEqual(BinaryOperator.Multiply, ((FdoBinaryExpression)expr).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoBinaryExpression)expr).Right);

            expr = FdoExpression.Parse("1 / 2.2");
            Assert.IsInstanceOf<FdoBinaryExpression>(expr);
            Assert.IsInstanceOf<FdoInt32Value>(((FdoBinaryExpression)expr).Left);
            Assert.AreEqual(BinaryOperator.Divide, ((FdoBinaryExpression)expr).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoBinaryExpression)expr).Right);

            expr = FdoExpression.Parse("1 - 2.2");
            Assert.IsInstanceOf<FdoBinaryExpression>(expr);
            Assert.IsInstanceOf<FdoInt32Value>(((FdoBinaryExpression)expr).Left);
            Assert.AreEqual(BinaryOperator.Subtract, ((FdoBinaryExpression)expr).Operator);
            Assert.IsInstanceOf<FdoDoubleValue>(((FdoBinaryExpression)expr).Right);

            expr = FdoExpression.Parse("GeomFromText('POINT (1 1)')");
            Assert.IsInstanceOf<FdoGeometryValue>(expr);
            Assert.AreEqual("POINT (1 1)", ((FdoGeometryValue)expr).GeometryWkt);

            expr = FdoExpression.Parse("DATE '1971-12-24'");
            Assert.IsInstanceOf<FdoDateTimeValue>(expr);
            Assert.True(((FdoDateTimeValue)expr).DateTime.HasValue);
            Assert.False(((FdoDateTimeValue)expr).Time.HasValue);
            Assert.AreEqual(1971, ((FdoDateTimeValue)expr).DateTime.Value.Year);
            Assert.AreEqual(12, ((FdoDateTimeValue)expr).DateTime.Value.Month);
            Assert.AreEqual(24, ((FdoDateTimeValue)expr).DateTime.Value.Day);

            expr = FdoExpression.Parse("TIME '11:23:43'");
            Assert.IsInstanceOf<FdoDateTimeValue>(expr);
            Assert.False(((FdoDateTimeValue)expr).DateTime.HasValue);
            Assert.True(((FdoDateTimeValue)expr).Time.HasValue);
            Assert.AreEqual(11, ((FdoDateTimeValue)expr).Time.Value.Hours);
            Assert.AreEqual(23, ((FdoDateTimeValue)expr).Time.Value.Minutes);
            Assert.AreEqual(43, ((FdoDateTimeValue)expr).Time.Value.Seconds);

            expr = FdoExpression.Parse("TIME '11:23:43.123'");
            Assert.IsInstanceOf<FdoDateTimeValue>(expr);
            Assert.False(((FdoDateTimeValue)expr).DateTime.HasValue);
            Assert.True(((FdoDateTimeValue)expr).Time.HasValue);
            Assert.AreEqual(11, ((FdoDateTimeValue)expr).Time.Value.Hours);
            Assert.AreEqual(23, ((FdoDateTimeValue)expr).Time.Value.Minutes);
            Assert.AreEqual(43, ((FdoDateTimeValue)expr).Time.Value.Seconds);
            Assert.AreEqual(123, ((FdoDateTimeValue)expr).Time.Value.Milliseconds);
            
            expr = FdoExpression.Parse("TIMESTAMP '2003-10-23 11:00:02'");
            Assert.IsInstanceOf<FdoDateTimeValue>(expr);
            Assert.True(((FdoDateTimeValue)expr).DateTime.HasValue);
            Assert.False(((FdoDateTimeValue)expr).Time.HasValue);
            Assert.AreEqual(2003, ((FdoDateTimeValue)expr).DateTime.Value.Year);
            Assert.AreEqual(10, ((FdoDateTimeValue)expr).DateTime.Value.Month);
            Assert.AreEqual(23, ((FdoDateTimeValue)expr).DateTime.Value.Day);
            Assert.AreEqual(11, ((FdoDateTimeValue)expr).DateTime.Value.Hour);
            Assert.AreEqual(00, ((FdoDateTimeValue)expr).DateTime.Value.Minute);
            Assert.AreEqual(02, ((FdoDateTimeValue)expr).DateTime.Value.Second);

            expr = FdoExpression.Parse("TIMESTAMP '2003-10-23 11:00:02.123'");
            Assert.IsInstanceOf<FdoDateTimeValue>(expr);
            Assert.True(((FdoDateTimeValue)expr).DateTime.HasValue);
            Assert.False(((FdoDateTimeValue)expr).Time.HasValue);
            Assert.AreEqual(2003, ((FdoDateTimeValue)expr).DateTime.Value.Year);
            Assert.AreEqual(10, ((FdoDateTimeValue)expr).DateTime.Value.Month);
            Assert.AreEqual(23, ((FdoDateTimeValue)expr).DateTime.Value.Day);
            Assert.AreEqual(11, ((FdoDateTimeValue)expr).DateTime.Value.Hour);
            Assert.AreEqual(00, ((FdoDateTimeValue)expr).DateTime.Value.Minute);
            Assert.AreEqual(02, ((FdoDateTimeValue)expr).DateTime.Value.Second);
            Assert.AreEqual(123, ((FdoDateTimeValue)expr).DateTime.Value.Millisecond);

            expr = FdoExpression.Parse("CurrentDate()");
            Assert.IsInstanceOf<FdoFunction>(expr);
            Assert.AreEqual("CurrentDate", ((FdoFunction)expr).Identifier.Name);
            Assert.AreEqual(0, ((FdoFunction)expr).Arguments.Count);

            expr = FdoExpression.Parse("Concat(RNAME, ' ', RBILAD)");
            Assert.IsInstanceOf<FdoFunction>(expr);
            Assert.AreEqual("Concat", ((FdoFunction)expr).Identifier.Name);
            Assert.AreEqual(3, ((FdoFunction)expr).Arguments.Count);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoFunction)expr).Arguments[0]);
            Assert.IsInstanceOf<FdoStringValue>(((FdoFunction)expr).Arguments[1]);
            Assert.IsInstanceOf<FdoIdentifier>(((FdoFunction)expr).Arguments[2]);

            expr = FdoExpression.Parse("-UnixTime()");
            Assert.IsInstanceOf<FdoUnaryExpression>(expr);
        }
    }
}
