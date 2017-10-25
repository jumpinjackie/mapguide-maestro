#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using OSGeo.MapGuide.ObjectModels.Common;
using System.IO;
using System.Xml;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests.Schema
{
    public class FeatureSourceDescriptionTests
    {
        [Fact]
        public void TestLoadFromStream()
        {
            var spatialContext = new FdoSpatialContextListSpatialContext()
            {
                Name = "Default",
                Description = "Test Spatial Context",
                CoordinateSystemName = "Default",
                CoordinateSystemWkt = "",
                ExtentType = FdoSpatialContextListSpatialContextExtentType.Static,
                Extent = new FdoSpatialContextListSpatialContextExtent
                {
                    LowerLeftCoordinate = new FdoSpatialContextListSpatialContextExtentLowerLeftCoordinate
                    {
                        X = "-180.0",
                        Y = "-90.0"
                    },
                    UpperRightCoordinate = new FdoSpatialContextListSpatialContextExtentUpperRightCoordinate
                    {
                        X = "180.0",
                        Y = "90"
                    }
                },
                XYTolerance = 0.00001,
                ZTolerance = 0.00001
            };

            var doc = new GenericConfigurationDocument();
            var schema = new FeatureSchema("Default", "");
            var cls = new ClassDefinition("Test", "");

            var id = new DataPropertyDefinition("ID", "")
            {
                DataType = DataPropertyType.Int32
            };
            var name = new DataPropertyDefinition("Name", "")
            {
                IsNullable = true
            };
            var geom = new GeometricPropertyDefinition("Geometry", "")
            {
                SpatialContextAssociation = spatialContext.Name,
                GeometricTypes = FeatureGeometricType.Point
            };
            cls.AddProperty(id, true);
            cls.AddProperty(name);
            cls.AddProperty(geom);
            cls.DefaultGeometryPropertyName = geom.Name;

            schema.AddClass(cls);

            doc.AddSchema(schema);
            doc.AddSpatialContext(spatialContext);

            var xmldoc = new XmlDocument();
            using (var ms = new MemoryStream())
            {
                var xml = doc.ToXml();
                xmldoc.LoadXml(xml);
                using (var xw = XmlWriter.Create(ms))
                {
                    xmldoc.WriteTo(xw);
                }
                //Rewind
                ms.Position = 0L;

                var fsd = new FeatureSourceDescription(ms);
                Assert.Single(fsd.SchemaNames);
                foreach (var sn in fsd.SchemaNames)
                {
                    var sch1 = fsd.GetSchema(sn);
                    Assert.NotNull(sch1);

                    Assert.Equal(schema.Name, sch1.Name);

                    foreach (var klass in schema.Classes)
                    {
                        var c1 = fsd.GetClass($"{schema.Name}:{klass.Name}");
                        var c2 = fsd.GetClass(schema.Name, klass.Name);
                        Assert.Equal(klass.Name, c1.Name);
                        Assert.Equal(c1.Name, c2.Name);
                        Assert.Equal(3, klass.Properties.Count);
                        Assert.Equal(klass.Properties.Count, c1.Properties.Count);
                        Assert.Equal(c1.Properties.Count, c2.Properties.Count);
                    }
                }
            }
        }
    }
}
