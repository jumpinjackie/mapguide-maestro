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
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using OSGeo.MapGuide.ObjectModels.Common;
using System.Drawing;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    [TestFixture]
    public class ConfigurationDocumentTests
    {
        //These tests are to verify that we can read FDO XML configuration and schema documents without problems

        [Test]
        public void TestMySqlSchema()
        {
            var fds = new FeatureSourceDescription(Utils.OpenFile("UserTestData\\gen_default1_MySql_master.xml"));
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
        public void TestOdbcLoad()
        {
            var conf = ConfigurationDocument.LoadXml(Utils.ReadAllText("UserTestData\\odbc_example_config.xml")) as OdbcConfigurationDocument;
            Assert.NotNull(conf);
        }

        [Test]
        public void TestOdbcLoad2()
        {
            var conf = ConfigurationDocument.LoadXml(Utils.ReadAllText("UserTestData\\odbc_example_config2.xml")) as OdbcConfigurationDocument;
            Assert.NotNull(conf);
        }

        [Test]
        public void TestOdbcSaveLoad()
        {
            var schema = new FeatureSchema("Default", "Test schema");
            var cls = new ClassDefinition("Cities", "Cities class");

            cls.AddProperty(new DataPropertyDefinition("ID", "Primary Key")
            {
                DataType = DataPropertyType.Int64,
                IsNullable = false,
                IsAutoGenerated = true
            }, true);

            cls.AddProperty(new DataPropertyDefinition("Name", "City Name")
            {
                DataType = DataPropertyType.String,
                IsNullable = true,
                IsAutoGenerated = false,
                Length = 255
            });

            cls.AddProperty(new GeometricPropertyDefinition("Geometry", "Geometry property")
            {
                GeometricTypes = FeatureGeometricType.Point,
                SpecificGeometryTypes = new SpecificGeometryType[] { SpecificGeometryType.Point },
                HasElevation = false,
                HasMeasure = false,
                SpatialContextAssociation = "Default"
            });

            cls.AddProperty(new DataPropertyDefinition("Population", "Population")
            {
                DataType = DataPropertyType.Int32,
                IsNullable = true,
                IsAutoGenerated = false
            });

            cls.DefaultGeometryPropertyName = "Geometry";

            schema.AddClass(cls);

            var sc = new FdoSpatialContextListSpatialContext();
            sc.CoordinateSystemName = "LL84";
            sc.CoordinateSystemWkt = "";
            sc.Description = "Default Spatial Context";
            sc.Extent = new FdoSpatialContextListSpatialContextExtent()
            {
                LowerLeftCoordinate = new FdoSpatialContextListSpatialContextExtentLowerLeftCoordinate()
                {
                    X = "-180.0",
                    Y = "-180.0"
                },
                UpperRightCoordinate = new FdoSpatialContextListSpatialContextExtentUpperRightCoordinate()
                {
                    X = "180.0",
                    Y = "180.0"
                }
            };
            sc.ExtentType = FdoSpatialContextListSpatialContextExtentType.Static;
            sc.Name = "Default";
            sc.XYTolerance = 0.0001;
            sc.ZTolerance = 0.0001;

            var conf = new OdbcConfigurationDocument();
            conf.AddSchema(schema);
            conf.AddSpatialContext(sc);

            var ov = new OdbcTableItem();
            ov.SchemaName = schema.Name;
            ov.ClassName = cls.Name;
            ov.SpatialContextName = sc.Name;
            ov.XColumn = "Lon";
            ov.YColumn = "Lat";

            conf.AddOverride(ov);

            string path = "OdbcConfigTest.xml";
            Utils.WriteAllText(path, conf.ToXml());

            conf = null;
            string xml = Utils.ReadAllText(path);
            conf = ConfigurationDocument.LoadXml(xml) as OdbcConfigurationDocument;
            Assert.NotNull(conf);

            ov = conf.GetOverride("Default", "Cities");
            Assert.NotNull(ov);
            Assert.AreEqual("Default", ov.SchemaName);
            Assert.AreEqual("Cities", ov.ClassName);
            Assert.AreEqual(sc.Name, ov.SpatialContextName);
            Assert.AreEqual("Lon", ov.XColumn);
            Assert.AreEqual("Lat", ov.YColumn);
        }

        [Test]
        public void TestWmsLoad()
        {
            var conf = ConfigurationDocument.LoadXml(Utils.ReadAllText("UserTestData\\NASA_WMS_config_doc.xml")) as WmsConfigurationDocument;
            Assert.NotNull(conf);
        }

        [Test]
        public void TestWmsLoad2()
        {
            var conf = ConfigurationDocument.LoadXml(Utils.ReadAllText("UserTestData\\wms_config_example1.xml")) as WmsConfigurationDocument;
            Assert.NotNull(conf);
        }

        [Test]
        public void TestWmsLoad3()
        {
            var conf = ConfigurationDocument.LoadXml(Utils.ReadAllText("UserTestData\\wms_config_example2.xml")) as WmsConfigurationDocument;
            Assert.NotNull(conf);
        }

        [Test]
        public void TestWmsSaveLoad()
        {
            var conf = new WmsConfigurationDocument();

            var schema = new FeatureSchema("WMS", "WMS Test Schema");
            var cls = new ClassDefinition("NASAWMSGlobalPan", "WMS Test Class");
            cls.AddProperty(new DataPropertyDefinition("Id", "ID Property")
            {
                DataType = DataPropertyType.String,
                Length = 256,
                IsNullable = false
            }, true);
            cls.AddProperty(new RasterPropertyDefinition("Image", "Raster Property")
            {
                DefaultImageXSize = 800,
                DefaultImageYSize = 800
            });

            schema.AddClass(cls);
            conf.AddSchema(schema);

            var item = new RasterWmsItem(schema.Name, cls.Name, "Image");
            item.ImageFormat = RasterWmsItem.WmsImageFormat.PNG;
            item.IsTransparent = true;
            item.BackgroundColor = ColorTranslator.FromHtml("#FFFFFF");
            item.Time = "current";
            item.ElevationDimension = "0";
            item.SpatialContextName = "EPSG:4326";

            for (int i = 0; i < 5; i++)
            {
                item.AddLayer(new WmsLayerDefinition("Layer" + i));
            }

            conf.AddRasterItem(item);

            string path = "WmsConfigTest.xml";
            Utils.WriteAllText(path, conf.ToXml());

            conf = null;
            string xml = Utils.ReadAllText(path);
            conf = ConfigurationDocument.LoadXml(xml) as WmsConfigurationDocument;
            Assert.NotNull(conf);

            Assert.AreEqual(1, conf.RasterOverrides.Length);

            var ritem = conf.RasterOverrides[0];
            cls = conf.GetClass("WMS", "NASAWMSGlobalPan");

            Assert.NotNull(cls);
            Assert.NotNull(cls.Parent);
            Assert.AreEqual("WMS", cls.Parent.Name);
            Assert.AreEqual("WMS Test Schema", cls.Parent.Description);
            Assert.AreEqual("NASAWMSGlobalPan", cls.Name);
            Assert.AreEqual("WMS Test Class", cls.Description);
            var prop = cls.FindProperty("Id");
            Assert.NotNull(prop);
            Assert.AreEqual("Id", prop.Name);
            Assert.AreEqual("ID Property", prop.Description);
            prop = cls.FindProperty("Image");
            Assert.NotNull(prop);
            Assert.AreEqual("Image", prop.Name);
            Assert.AreEqual("Raster Property", prop.Description);

            Assert.AreEqual(item.ImageFormat, ritem.ImageFormat);
            Assert.AreEqual(item.IsTransparent, ritem.IsTransparent);
            Assert.AreEqual(item.BackgroundColor, ritem.BackgroundColor);
            Assert.AreEqual(item.Time, ritem.Time);
            Assert.AreEqual(item.ElevationDimension, ritem.ElevationDimension);
            Assert.AreEqual(item.SpatialContextName, ritem.SpatialContextName);

            Assert.AreEqual(5, item.Layers.Length);
        }
    }
}
