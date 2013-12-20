#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI;
using System.IO;
using OSGeo.MapGuide.MaestroAPI.Expression;
using System.Threading;
using System.Diagnostics;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Http;

namespace MaestroAPITests
{
    // Test data schema (for reference in these unit tests)
    //
    // ID (int32) | Name(string) | URL(string)
    //
    // 1 | Foobar | http://foobar.com
    // 2 | snafu  | (null)
    // 2 | (null) | (null)

    [TestFixture]
    public class ExpressionTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            if (TestControl.IgnoreExpressionTests)
                Assert.Ignore("Skipping ExpressionTests because TestControl.IgnoreExpressionTests = true");
        }

        [Test]
        public void TestMismatch()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.Catch<ExpressionException>(() => exprReader.Evaluate<bool>("CurrentDate()"));
            Assert.Catch<ExpressionException>(() => exprReader.Evaluate<bool>("'Foobar'"));
            Assert.Catch<ExpressionException>(() => exprReader.Evaluate<bool>("Power(2, 4)"));
            Assert.Catch<ExpressionException>(() => exprReader.Evaluate<string>("Foobar"));

            var dt = exprReader.Evaluate<DateTime>("CurrentDate()");
            var str = exprReader.Evaluate<string>("'Foobar'");
            var num = exprReader.Evaluate<int>("Power(2, 4)");
            var num2 = exprReader.Evaluate<float>("Power(2, 4)");

            exprReader.Close();
        }

        [Test]
        public void TestCurrentDateAndAddMonths()
        {
            //TODO: Know how to calculate diffs between 2 DateTimes in months?

            var dtNow = DateTime.Now;

            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());

            Thread.Sleep(50);

            var dt1 = exprReader.Evaluate("CurrentDate()");
            Assert.NotNull(dt1);
            Trace.WriteLine("dt1: " + dt1.ToString());
            Assert.AreEqual(typeof(DateTime), dt1.GetType());
            Assert.IsTrue((DateTime)dt1 > dtNow);

            var fut1 = exprReader.Evaluate("AddMonths(CurrentDate(), 3)");
            Assert.NotNull(fut1);
            Trace.WriteLine("fut1: " + fut1.ToString());

            var past1 = exprReader.Evaluate("AddMonths(CurrentDate(), -3)");
            Assert.NotNull(past1);
            Trace.WriteLine("past1: " + past1.ToString());
            
            Thread.Sleep(50);

            Assert.IsTrue(exprReader.ReadNext());

            var dt2 = exprReader.Evaluate("CurrentDate()");
            Assert.NotNull(dt2);
            Trace.WriteLine("dt2: " + dt2.ToString());
            Assert.AreEqual(typeof(DateTime), dt2.GetType());
            Assert.IsTrue((DateTime)dt2 > (DateTime)dt1);

            var fut2 = exprReader.Evaluate("AddMonths(CurrentDate(), 3)");
            Assert.NotNull(fut2);
            Trace.WriteLine("fut2: " + fut2.ToString());

            var past2 = exprReader.Evaluate("AddMonths(CurrentDate(), -3)");
            Assert.NotNull(past2);
            Trace.WriteLine("past2: " + past2.ToString());

            Thread.Sleep(50);

            Assert.IsTrue(exprReader.ReadNext());

            var dt3 = exprReader.Evaluate("CurrentDate()");
            Assert.NotNull(dt3);
            Trace.WriteLine("dt2: " + dt3.ToString());
            Assert.AreEqual(typeof(DateTime), dt3.GetType());
            Assert.IsTrue((DateTime)dt3 > (DateTime)dt2);

            var fut3 = exprReader.Evaluate("AddMonths(CurrentDate(), 3)");
            Assert.NotNull(fut3);
            Trace.WriteLine("fut3: " + fut3.ToString());

            var past3 = exprReader.Evaluate("AddMonths(CurrentDate(), -3)");
            Assert.NotNull(past3);
            Trace.WriteLine("past3: " + past3.ToString());

            Thread.Sleep(50);

            Assert.IsFalse(exprReader.ReadNext()); //end of stream

            exprReader.Close();
        }

        [Test]
        public void TestNullValue()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());

            Assert.IsTrue(!exprReader.IsNull("URL"));

            var url = exprReader["URL"];
            var value = exprReader.Evaluate("NullValue(URL, 'http://www.snafu.com')");
            Assert.NotNull(url);
            Assert.NotNull(value);
            Assert.AreEqual(url.ToString(), value.ToString());

            Assert.IsTrue(exprReader.ReadNext());

            var value2 = exprReader.Evaluate("NullValue(URL, 'http://www.foo.com')");
            Assert.NotNull(value2);
            Assert.AreEqual("http://www.foo.com", value2);

            Assert.IsTrue(exprReader.ReadNext());

            var value3 = exprReader.Evaluate("NullValue(URL, 'http://www.bar.com')");
            Assert.NotNull(value3);
            Assert.AreEqual("http://www.bar.com", value3);

            Assert.IsFalse(exprReader.ReadNext());
            exprReader.Close();
        }

        [Test]
        public void TestMathAbs()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());

            var orig = (int)exprReader["ID"];
            var value = Convert.ToInt32(exprReader.Evaluate("Abs(-ID)"));

            Assert.AreEqual(orig, value);

            Assert.IsTrue(exprReader.ReadNext());

            var orig2 = (int)exprReader["ID"];
            var value2 = Convert.ToInt32(exprReader.Evaluate("Abs(-ID)"));

            Assert.AreEqual(orig2, value2);

            Assert.IsTrue(exprReader.ReadNext());

            var orig3 = (int)exprReader["ID"];
            var value3 = Convert.ToInt32(exprReader.Evaluate("Abs(-ID)"));

            Assert.AreEqual(orig3, value3);

            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        [Test]
        public void TestMathAcos()
        {
            //The numbers in this sample are way too large for Acos() so trim them down to size

            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());

            var orig = (int)exprReader["ID"];
            var value = Convert.ToDouble(exprReader.Evaluate("Acos(ID * 0.1)"));

            Assert.AreEqual(Math.Acos(orig * 0.1), value);

            Assert.IsTrue(exprReader.ReadNext());

            var orig2 = (int)exprReader["ID"];
            var value2 = Convert.ToDouble(exprReader.Evaluate("Acos(ID * 0.1)"));

            Assert.AreEqual(Math.Acos(orig2 * 0.1), value2);

            Assert.IsTrue(exprReader.ReadNext());

            var orig3 = (int)exprReader["ID"];
            var value3 = Convert.ToDouble(exprReader.Evaluate("Acos(ID * 0.1)"));

            Assert.AreEqual(Math.Acos(orig3 * 0.1), value3);

            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        [Test]
        public void TestMathAsin()
        {
            //The numbers in this sample are way too large for Asin() so trim them down to size

            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());

            var orig = (int)exprReader["ID"];
            var value = Convert.ToDouble(exprReader.Evaluate("Asin(ID * 0.1)"));

            Assert.AreEqual(Math.Asin(orig * 0.1), value);

            Assert.IsTrue(exprReader.ReadNext());

            var orig2 = (int)exprReader["ID"];
            var value2 = Convert.ToDouble(exprReader.Evaluate("Asin(ID * 0.1)"));

            Assert.AreEqual(Math.Asin(orig2 * 0.1), value2);

            Assert.IsTrue(exprReader.ReadNext());

            var orig3 = (int)exprReader["ID"];
            var value3 = Convert.ToDouble(exprReader.Evaluate("Asin(ID * 0.1)"));

            Assert.AreEqual(Math.Asin(orig3 * 0.1), value3);

            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestMathAtan2()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        [Test]
        public void TestMathCos()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());

            var orig = (int)exprReader["ID"];
            var value = Convert.ToDouble(exprReader.Evaluate("Cos(ID)"));

            Assert.AreEqual(Math.Cos(orig), value);

            Assert.IsTrue(exprReader.ReadNext());

            var orig2 = (int)exprReader["ID"];
            var value2 = Convert.ToDouble(exprReader.Evaluate("Cos(ID)"));

            Assert.AreEqual(Math.Cos(orig2), value2);

            Assert.IsTrue(exprReader.ReadNext());

            var orig3 = (int)exprReader["ID"];
            var value3 = Convert.ToDouble(exprReader.Evaluate("Cos(ID)"));

            Assert.AreEqual(Math.Cos(orig3), value3);

            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        [Test]
        public void TestMathExp()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());

            var orig = (int)exprReader["ID"];
            var value = Convert.ToDouble(exprReader.Evaluate("Exp(ID)"));

            Assert.AreEqual(Math.Exp(orig), value);

            Assert.IsTrue(exprReader.ReadNext());

            var orig2 = (int)exprReader["ID"];
            var value2 = Convert.ToDouble(exprReader.Evaluate("Exp(ID)"));

            Assert.AreEqual(Math.Exp(orig2), value2);

            Assert.IsTrue(exprReader.ReadNext());

            var orig3 = (int)exprReader["ID"];
            var value3 = Convert.ToDouble(exprReader.Evaluate("Exp(ID)"));

            Assert.AreEqual(Math.Exp(orig3), value3);

            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        [Test]
        public void TestMathLn()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());

            var orig = (int)exprReader["ID"];
            var value = Convert.ToDouble(exprReader.Evaluate("Ln(ID)"));

            Assert.AreEqual(Math.Log(orig), value);

            Assert.IsTrue(exprReader.ReadNext());

            var orig2 = (int)exprReader["ID"];
            var value2 = Convert.ToDouble(exprReader.Evaluate("Ln(ID)"));

            Assert.AreEqual(Math.Log(orig2), value2);

            Assert.IsTrue(exprReader.ReadNext());

            var orig3 = (int)exprReader["ID"];
            var value3 = Convert.ToDouble(exprReader.Evaluate("Ln(ID)"));

            Assert.AreEqual(Math.Log(orig3), value3);

            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        [Test]
        public void TestMathLog()
        {
            
        }

        public void TestMathMod()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        [Test]
        public void TestMathPower()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());

            var orig = (int)exprReader["ID"];
            var value = Convert.ToDouble(exprReader.Evaluate("Power(ID, 2)"));

            Assert.AreEqual(Math.Pow(orig, 2), value);

            Assert.IsTrue(exprReader.ReadNext());

            var orig2 = (int)exprReader["ID"];
            var value2 = Convert.ToDouble(exprReader.Evaluate("Power(ID, 4)"));

            Assert.AreEqual(Math.Pow(orig2, 4), value2);

            Assert.IsTrue(exprReader.ReadNext());

            var orig3 = (int)exprReader["ID"];
            var value3 = Convert.ToDouble(exprReader.Evaluate("Power(ID, 8)"));

            Assert.AreEqual(Math.Pow(orig3, 8), value3);

            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestMathRemainder()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestMathSin()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestMathSqrt()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestMathTan()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestMathCeil()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestMathFloor()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestMathRound()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestMathSign()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestMathTrunc()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        [Test]
        public void TestConcat()
        {
            //Test nested and variadic forms of Concat()

            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());

            var orig = exprReader["Name"].ToString();
            var value = exprReader.Evaluate("Concat(Concat(Name, 'Foo'), 'Bar')");
            var valueV = exprReader.Evaluate("Concat(Name, 'Foo', 'Bar', 'Snafu')");
            Assert.AreEqual(orig + "FooBar", value);
            Assert.AreEqual(orig + "FooBarSnafu", valueV);

            Assert.IsTrue(exprReader.ReadNext());

            var orig2 = exprReader["Name"].ToString();
            var value2 = exprReader.Evaluate("Concat(Concat(Name, 'Foo'), 'Bar')");
            var valueV2 = exprReader.Evaluate("Concat(Name, 'Foo', 'Bar', 'Snafu')");
            Assert.AreEqual(orig2 + "FooBar", value2);
            Assert.AreEqual(orig2 + "FooBarSnafu", valueV2);

            Assert.IsTrue(exprReader.ReadNext());

            var value3 = exprReader.Evaluate("Concat(Concat(Name, 'Foo'), 'Bar')");
            var valueV3 = exprReader.Evaluate("Concat(Name, 'Foo', 'Bar', 'Snafu')");
            Assert.AreEqual("FooBar", value3);
            Assert.AreEqual("FooBarSnafu", valueV3);

            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestInstr()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestLength()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestLower()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestLpad()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestLtrim()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestRpad()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestRtrim()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestSoundex()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestSubstr()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestTranslate()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestTrim()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }

        public void TestUpper()
        {
            //Simulate post-#708 SELECTFEATURES output and test our expression engine reader
            var bytes = Encoding.UTF8.GetBytes(Properties.Resources.SelectFeatureSample);
            var reader = new XmlFeatureReader(new MemoryStream(bytes));
            Assert.NotNull(reader.ClassDefinition);
            Assert.AreEqual(3, reader.FieldCount);
            Assert.AreEqual(3, reader.ClassDefinition.Properties.Count);
            Assert.NotNull(reader.ClassDefinition.FindProperty("ID"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("Name"));
            Assert.NotNull(reader.ClassDefinition.FindProperty("URL"));

            var exprReader = new ExpressionFeatureReader(reader);

            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsTrue(exprReader.ReadNext());
            Assert.IsFalse(exprReader.ReadNext());

            exprReader.Close();
        }
    }
}
