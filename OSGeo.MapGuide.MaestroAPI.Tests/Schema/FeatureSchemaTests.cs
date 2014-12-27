#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSGeo.MapGuide.MaestroAPI.Schema;
using NUnit.Framework;
using System.Xml;
namespace OSGeo.MapGuide.MaestroAPI.Schema.Tests
{
    [TestFixture()]
    public class FeatureSchemaTests
    {
        [Test()]
        public void FeatureSchemaTest()
        {
            var fs = new FeatureSchema();
            Assert.IsNullOrEmpty(fs.Name);
            Assert.IsNullOrEmpty(fs.Description);
            Assert.AreEqual(0, fs.Classes.Count);

            fs = new FeatureSchema("Foo", "");
            Assert.AreEqual("Foo", fs.Name);
            Assert.IsNullOrEmpty(fs.Description);
            Assert.AreEqual(0, fs.Classes.Count);

            fs = new FeatureSchema("Foo", "Bar");
            Assert.AreEqual("Foo", fs.Name);
            Assert.AreEqual("Bar", fs.Description);
            Assert.AreEqual(0, fs.Classes.Count);
        }

        [Test()]
        public void AddClassTest()
        {
            var fs = new FeatureSchema("Foo", "Bar");
            Assert.AreEqual("Foo", fs.Name);
            Assert.AreEqual("Bar", fs.Description);
            Assert.AreEqual(0, fs.Classes.Count);

            var cls = new ClassDefinition("Class1", "Test Class");
            fs.AddClass(cls);
            Assert.AreEqual(1, fs.Classes.Count);
        }

        [Test()]
        public void RemoveClassTest()
        {
            var fs = new FeatureSchema("Foo", "Bar");
            Assert.AreEqual("Foo", fs.Name);
            Assert.AreEqual("Bar", fs.Description);
            Assert.AreEqual(0, fs.Classes.Count);

            var cls = new ClassDefinition("Class1", "Test Class");
            fs.AddClass(cls);
            Assert.AreEqual(1, fs.Classes.Count);

            fs.RemoveClass("asdgsdf");
            Assert.AreEqual(1, fs.Classes.Count);

            Assert.True(fs.RemoveClass(cls));
            Assert.AreEqual(0, fs.Classes.Count);

            fs.AddClass(cls);
            Assert.AreEqual(1, fs.Classes.Count);
            fs.RemoveClass("Class1");
            Assert.AreEqual(0, fs.Classes.Count);
        }

        [Test()]
        public void GetClassTest()
        {
            var fs = new FeatureSchema("Foo", "Bar");
            Assert.AreEqual("Foo", fs.Name);
            Assert.AreEqual("Bar", fs.Description);
            Assert.AreEqual(0, fs.Classes.Count);

            var cls = new ClassDefinition("Class1", "Test Class");
            fs.AddClass(cls);

            Assert.NotNull(fs.GetClass("Class1"));
        }

        [Test()]
        public void IndexOfTest()
        {
            var fs = new FeatureSchema("Foo", "Bar");
            Assert.AreEqual("Foo", fs.Name);
            Assert.AreEqual("Bar", fs.Description);
            Assert.AreEqual(0, fs.Classes.Count);

            var cls = new ClassDefinition("Class1", "Test Class");
            fs.AddClass(cls);

            Assert.GreaterOrEqual(fs.IndexOf(cls), 0);
            Assert.True(fs.RemoveClass(cls));
            Assert.Less(fs.IndexOf(cls), 0);
        }

        [Test()]
        public void GetItemTest()
        {
            var fs = new FeatureSchema("Foo", "Bar");
            Assert.AreEqual("Foo", fs.Name);
            Assert.AreEqual("Bar", fs.Description);
            Assert.AreEqual(0, fs.Classes.Count);

            var cls = new ClassDefinition("Class1", "Test Class");
            fs.AddClass(cls);

            Assert.Throws<ArgumentOutOfRangeException>(() => fs.GetItem(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => fs.GetItem(1));
            Assert.NotNull(fs.GetItem(0));
            Assert.True(fs.RemoveClass(cls));
            Assert.Throws<ArgumentOutOfRangeException>(() => fs.GetItem(0));
        }

        [Test()]
        public void WriteXmlTest()
        {
            var fs = new FeatureSchema("Foo", "Bar");
            Assert.AreEqual("Foo", fs.Name);
            Assert.AreEqual("Bar", fs.Description);
            Assert.AreEqual(0, fs.Classes.Count);

            var cls = new ClassDefinition("Class1", "Test Class");
            var id = new DataPropertyDefinition("ID", "");
            id.DataType = DataPropertyType.Int32;
            id.IsAutoGenerated = true;
            var name = new DataPropertyDefinition("Name", "");
            cls.AddProperty(id, true);
            cls.AddProperty(name);
            fs.AddClass(cls);

            var doc = new XmlDocument();
            fs.WriteXml(doc, doc);

            string xml = doc.ToXmlString();
            Assert.IsNotNullOrEmpty(xml);
        }

        [Test()]
        public void CloneTest()
        {
            var fs = new FeatureSchema("Foo", "Bar");
            Assert.AreEqual("Foo", fs.Name);
            Assert.AreEqual("Bar", fs.Description);
            Assert.AreEqual(0, fs.Classes.Count);

            var cls = new ClassDefinition("Class1", "Test Class");
            var id = new DataPropertyDefinition("ID", "");
            id.DataType = DataPropertyType.Int32;
            id.IsAutoGenerated = true;
            var name = new DataPropertyDefinition("Name", "");
            cls.AddProperty(id, true);
            cls.AddProperty(name);
            fs.AddClass(cls);

            var fs2 = FeatureSchema.Clone(fs);
            Assert.AreEqual(fs.Name, fs2.Name);
            Assert.AreEqual(fs.Description, fs2.Description);
            Assert.AreEqual(fs.Classes.Count, fs2.Classes.Count);
            Assert.AreNotSame(fs, fs2);
        }
    }
}
