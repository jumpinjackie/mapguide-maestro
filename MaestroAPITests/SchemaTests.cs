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
using OSGeo.MapGuide.MaestroAPI.Schema;
using System.IO;
using System.Xml;
using OSGeo.MapGuide.MaestroAPI;

namespace MaestroAPITests
{
    [TestFixture]
    public class SchemaTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            if (TestControl.IgnoreSchemaTests)
                Assert.Ignore("Skipping SchemaTests because TestControl.IgnoreSchemaTests = true");
        }

        //These tests are to verify that we can read FDO XML configuration and schema documents without problems
        
        [Test]
        public void TestMySqlSchema()
        {
            var fds = new FeatureSourceDescription(File.OpenRead("UserTestData\\gen_default1_MySql_master.xml"));
            Assert.AreEqual(1, fds.Schemas.Length);

            var fs = fds.GetSchema("AutoGen");
            Assert.IsNotNull(fs);

            Assert.AreEqual(12, fs.Classes.Count);

            var c1 = fds.GetClass("AutoGen:rtable1");
            var c2 = fds.GetClass("AutoGen:rtable2");
            var c3 = fds.GetClass("AutoGen:rtable5");
            var c4 = fds.GetClass("AutoGen:rtable6");
            var c5 = fds.GetClass("AutoGen:rtable7");
            var c6 = fds.GetClass("AutoGen:table1");
            var c7 = fds.GetClass("AutoGen:table3");
            var c8 = fds.GetClass("AutoGen:table4");
            var c9 = fds.GetClass("AutoGen:table5");
            var c10 = fds.GetClass("AutoGen:table6");
            var c11 = fds.GetClass("AutoGen:table7");
            var c12 = fds.GetClass("AutoGen:view1");

            Assert.NotNull(c1);
            Assert.NotNull(c2);
            Assert.NotNull(c3);
            Assert.NotNull(c4);
            Assert.NotNull(c5);
            Assert.NotNull(c6);
            Assert.NotNull(c7);
            Assert.NotNull(c8);
            Assert.NotNull(c9);
            Assert.NotNull(c10);
            Assert.NotNull(c11);
            Assert.NotNull(c12);

            Assert.AreEqual(1, c1.IdentityProperties.Count);
            Assert.AreEqual(1, c2.IdentityProperties.Count);
            Assert.AreEqual(1, c3.IdentityProperties.Count);
            Assert.AreEqual(1, c4.IdentityProperties.Count);
            Assert.AreEqual(1, c5.IdentityProperties.Count);
            Assert.AreEqual(1, c6.IdentityProperties.Count);
            Assert.AreEqual(2, c7.IdentityProperties.Count);
            Assert.AreEqual(1, c8.IdentityProperties.Count);
            Assert.AreEqual(1, c9.IdentityProperties.Count);
            Assert.AreEqual(2, c10.IdentityProperties.Count);
            Assert.AreEqual(1, c11.IdentityProperties.Count);
            Assert.AreEqual(0, c12.IdentityProperties.Count);

            Assert.AreEqual(c1.Properties.Count, 3);
            Assert.AreEqual(c2.Properties.Count, 5);
            Assert.AreEqual(c3.Properties.Count, 3);
            Assert.AreEqual(c4.Properties.Count, 4);
            Assert.AreEqual(c5.Properties.Count, 3);
            Assert.AreEqual(c6.Properties.Count, 47);
            Assert.AreEqual(c7.Properties.Count, 3);
            Assert.AreEqual(c8.Properties.Count, 4);
            Assert.AreEqual(c9.Properties.Count, 2);
            Assert.AreEqual(c10.Properties.Count, 3);
            Assert.AreEqual(c11.Properties.Count, 2);
            Assert.AreEqual(c12.Properties.Count, 3);

            Assert.AreEqual(c1, fds.GetClass("AutoGen", "rtable1"));
            Assert.AreEqual(c2, fds.GetClass("AutoGen", "rtable2"));
            Assert.AreEqual(c3, fds.GetClass("AutoGen", "rtable5"));
            Assert.AreEqual(c4, fds.GetClass("AutoGen", "rtable6"));
            Assert.AreEqual(c5, fds.GetClass("AutoGen", "rtable7"));
            Assert.AreEqual(c6, fds.GetClass("AutoGen", "table1"));
            Assert.AreEqual(c7, fds.GetClass("AutoGen", "table3"));
            Assert.AreEqual(c8, fds.GetClass("AutoGen", "table4"));
            Assert.AreEqual(c9, fds.GetClass("AutoGen", "table5"));
            Assert.AreEqual(c10, fds.GetClass("AutoGen", "table6"));
            Assert.AreEqual(c11, fds.GetClass("AutoGen", "table7"));
            Assert.AreEqual(c12, fds.GetClass("AutoGen", "view1"));

            Assert.IsTrue(string.IsNullOrEmpty(c1.DefaultGeometryPropertyName));
            Assert.IsTrue(string.IsNullOrEmpty(c2.DefaultGeometryPropertyName));
            Assert.IsTrue(string.IsNullOrEmpty(c3.DefaultGeometryPropertyName));
            Assert.IsTrue(string.IsNullOrEmpty(c4.DefaultGeometryPropertyName));
            Assert.IsTrue(string.IsNullOrEmpty(c5.DefaultGeometryPropertyName));
            //Though this feature class has geometries, the XML schema says none
            //are designated
            Assert.IsTrue(string.IsNullOrEmpty(c6.DefaultGeometryPropertyName));
            Assert.IsTrue(string.IsNullOrEmpty(c7.DefaultGeometryPropertyName));
            Assert.IsFalse(string.IsNullOrEmpty(c8.DefaultGeometryPropertyName));
            Assert.IsTrue(string.IsNullOrEmpty(c9.DefaultGeometryPropertyName));
            Assert.IsTrue(string.IsNullOrEmpty(c10.DefaultGeometryPropertyName));
            Assert.IsTrue(string.IsNullOrEmpty(c11.DefaultGeometryPropertyName));
            Assert.IsTrue(string.IsNullOrEmpty(c12.DefaultGeometryPropertyName));
        }

        [Test]
        public void TestBidirectional()
        {
            FeatureSchema schema = new FeatureSchema("Default", "Default Schema");
            ClassDefinition cls = new ClassDefinition("Class1", "Test Class");

            cls.AddProperty(new DataPropertyDefinition("ID", "ID Property")
            {
                IsAutoGenerated = true,
                DataType = DataPropertyType.Int64,
                IsNullable = false,
            }, true);

            var prop = cls.FindProperty("ID") as DataPropertyDefinition;

            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(1, cls.Properties.Count);
            Assert.NotNull(prop);
            Assert.AreEqual(DataPropertyType.Int64, prop.DataType);
            Assert.IsTrue(prop.IsAutoGenerated);
            Assert.IsFalse(prop.IsReadOnly);
            Assert.IsFalse(prop.IsNullable);
           
            cls.AddProperty(new DataPropertyDefinition("Name", "The name")
            {
                DataType = DataPropertyType.String,
                Length = 255
            });

            prop = cls.FindProperty("Name") as DataPropertyDefinition;

            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(2, cls.Properties.Count);
            Assert.NotNull(prop);
            Assert.AreEqual(DataPropertyType.String, prop.DataType);
            Assert.IsFalse(prop.IsAutoGenerated);
            Assert.IsFalse(prop.IsReadOnly);
            Assert.IsFalse(prop.IsNullable);

            cls.AddProperty(new DataPropertyDefinition("Date", "The date")
            {
                DataType = DataPropertyType.DateTime,
                IsNullable = true
            });

            prop = cls.FindProperty("Date") as DataPropertyDefinition;

            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(3, cls.Properties.Count);
            Assert.NotNull(prop);
            Assert.AreEqual(DataPropertyType.DateTime, prop.DataType);
            Assert.IsFalse(prop.IsAutoGenerated);
            Assert.IsFalse(prop.IsReadOnly);
            Assert.IsTrue(prop.IsNullable);

            schema.AddClass(cls);
            Assert.AreEqual(1, schema.Classes.Count);

            XmlDocument doc = new XmlDocument();
            schema.WriteXml(doc, doc);

            string path = Path.GetTempFileName();
            doc.Save(path);

            FeatureSourceDescription fsd = new FeatureSourceDescription(File.OpenRead(path));
            Assert.AreEqual(1, fsd.Schemas.Length);

            schema = fsd.Schemas[0];
            Assert.NotNull(schema);
            
            cls = schema.GetClass("Class1");
            Assert.NotNull(cls);

            prop = cls.FindProperty("ID") as DataPropertyDefinition;

            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(3, cls.Properties.Count);
            Assert.NotNull(prop);
            Assert.AreEqual(DataPropertyType.Int64, prop.DataType);
            Assert.IsTrue(prop.IsAutoGenerated);
            Assert.IsFalse(prop.IsReadOnly);
            Assert.IsFalse(prop.IsNullable);
           

            prop = cls.FindProperty("Name") as DataPropertyDefinition;

            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(3, cls.Properties.Count);
            Assert.NotNull(prop);
            Assert.AreEqual(DataPropertyType.String, prop.DataType);
            Assert.IsFalse(prop.IsAutoGenerated);
            Assert.IsFalse(prop.IsReadOnly);
            Assert.IsFalse(prop.IsNullable);

            prop = cls.FindProperty("Date") as DataPropertyDefinition;

            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(3, cls.Properties.Count);
            Assert.NotNull(prop);
            Assert.AreEqual(DataPropertyType.DateTime, prop.DataType);
            Assert.IsFalse(prop.IsAutoGenerated);
            Assert.IsFalse(prop.IsReadOnly);
            Assert.IsTrue(prop.IsNullable);
        }

        [Test]
        public void TestCreate()
        {
            var schema = new FeatureSchema("Default", "Default Schema");
            Assert.AreEqual("Default", schema.Name);
            Assert.AreEqual("Default Schema", schema.Description);

            var cls = new ClassDefinition("Class1", "My Class");
            Assert.AreEqual("Class1", cls.Name);
            Assert.AreEqual("My Class", cls.Description);
            Assert.IsTrue(string.IsNullOrEmpty(cls.DefaultGeometryPropertyName));
            Assert.AreEqual(0, cls.Properties.Count);
            Assert.AreEqual(0, cls.IdentityProperties.Count);

            var prop = new DataPropertyDefinition("ID", "identity");
            Assert.AreEqual("ID", prop.Name);
            Assert.AreEqual("identity", prop.Description);
            Assert.AreEqual(false, prop.IsAutoGenerated);
            Assert.AreEqual(false, prop.IsReadOnly);
            Assert.IsTrue(string.IsNullOrEmpty(prop.DefaultValue));

            prop.DataType = DataPropertyType.Int32;
            Assert.AreEqual(DataPropertyType.Int32, prop.DataType);

            prop.IsAutoGenerated = true;
            Assert.IsTrue(prop.IsAutoGenerated);

            prop.IsReadOnly = true;
            Assert.IsTrue(prop.IsReadOnly);

            cls.AddProperty(prop, true);
            Assert.AreEqual(1, cls.Properties.Count);
            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(cls, prop.Parent);
            Assert.NotNull(cls.FindProperty(prop.Name));

            cls.RemoveProperty(prop);
            Assert.AreEqual(0, cls.Properties.Count);
            Assert.AreEqual(0, cls.Properties.Count);
            Assert.IsNull(prop.Parent);
            Assert.IsNull(cls.FindProperty(prop.Name));

            cls.AddProperty(prop, true);
            Assert.AreEqual(1, cls.Properties.Count);
            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(cls, prop.Parent);
            Assert.NotNull(cls.FindProperty(prop.Name));

            cls.AddProperty(new DataPropertyDefinition("Name", "")
            {
                DataType = DataPropertyType.String,
                Length = 255,
                IsNullable = true
            });

            Assert.AreEqual(2, cls.Properties.Count);
            Assert.AreEqual(1, cls.IdentityProperties.Count);

            cls.AddProperty(new GeometricPropertyDefinition("Geom", "")
            {
                HasMeasure = false,
                HasElevation = false,
                GeometricTypes = FeatureGeometricType.All,
                SpecificGeometryTypes = (SpecificGeometryType[])Enum.GetValues(typeof(SpecificGeometryType))
            });
            Assert.AreEqual(3, cls.Properties.Count);
            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.IsTrue(string.IsNullOrEmpty(cls.DefaultGeometryPropertyName));

            var geom = cls.FindProperty("Geom");
            Assert.NotNull(geom);

            cls.DefaultGeometryPropertyName = geom.Name;
            Assert.IsNotNullOrEmpty(cls.DefaultGeometryPropertyName);

            schema.AddClass(cls);
            Assert.AreEqual(schema, cls.Parent);
        }
        
        [Test]
        public void TestClassNameEncoding()
        {
            // Round-trip various invalid XML names. Copied from FDO test suite
            string name1 = "Abc def";
            string name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "Abc-x20-def" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );
 
            name1 = " Abc#defg$$";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x20-Abc-x23-defg-x24--x24-" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );
        
            name1 = " Abc#defg hij";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x20-Abc-x23-defg-x20-hij" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );
        
            name1 = "--abc-def---ghi--";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x2d--abc-def---ghi--" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );
        
            name1 = "--abc-x20-def-x23--x24-ghi--";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x2d--abc-x2d-x20-def-x2d-x23--x2d-x24-ghi--" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "-xab";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x2d-xab" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "&Entity";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x26-Entity" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "11ab";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x31-1ab" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "2_Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x32-_Class" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "2%Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x32--x25-Class" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "2-Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x32--Class" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "2-x2f-Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x32--x2d-x2f-Class" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "_x2d-";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x00-_x2d-" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "-x3d-";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x2d-x3d-" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "_x2d-_x3f-";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x00-_x2d-_x3f-" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "__x2d-_x3f-";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "__x2d-_x3f-" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "_Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_Class" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "_5Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_5Class" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "_-5Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_-5Class" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "-_x2f-Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual( name2, "_x2d-_x2f-Class" );
            Assert.AreEqual( name1, Utility.DecodeFDOName(name2) );

            name1 = "Foo/Bar - snafu";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "Foo-x2f-Bar-x20---x20-snafu");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            // Backward compatibility check. Make sure old-style 1st character encodings get decoded.
            name2 = "-x40-A";
            Assert.AreEqual( Utility.DecodeFDOName(name2), "@A" );
        }
    }
}
