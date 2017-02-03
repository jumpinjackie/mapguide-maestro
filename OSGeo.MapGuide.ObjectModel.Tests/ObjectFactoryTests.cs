#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

using Moq;
using NUnit.Framework;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    [TestFixture]
    public class ObjectFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            ObjectFactory.Reset();
        }

        [TearDown]
        public void Teardown()
        {
            ObjectFactory.Reset();
        }

        [Test]
        public void RegisterResourceTest()
        {
            var ser = new Mock<ResourceSerializer>();
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterResource(null, null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterResource(null, ser.Object));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterResource(new ResourceTypeDescriptor("MyNewResourceType", "1.0.0"), null));
            //Already exists
            Assert.Throws<ArgumentException>(() => ObjectFactory.RegisterResource(new ResourceTypeDescriptor("FeatureSource", "1.0.0"), ser.Object));
            Assert.DoesNotThrow(() => ObjectFactory.RegisterResource(new ResourceTypeDescriptor("MyNewResourceType", "1.0.0"), ser.Object));
        }

        [Test]
        public void RegisterResourceSerializerTest()
        {
            Func<IResource, Stream> serFunc = (res) => null;
            Func<string, IResource> deserFunc = (stream) => null;

            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterResourceSerializer(new ResourceTypeDescriptor("FeatureSource", "1.0.0"), null, null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterResourceSerializer(new ResourceTypeDescriptor("FeatureSource", "1.0.0"), serFunc, null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterResourceSerializer(null, serFunc, null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterResourceSerializer(null, serFunc, deserFunc));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterResourceSerializer(new ResourceTypeDescriptor("FeatureSource", "1.0.0"), null, deserFunc));
            //Already exists
            Assert.Throws<ArgumentException>(() => ObjectFactory.RegisterResourceSerializer(new ResourceTypeDescriptor("FeatureSource", "1.0.0"), serFunc, deserFunc));
            Assert.DoesNotThrow(() => ObjectFactory.RegisterResourceSerializer(new ResourceTypeDescriptor("MyNewResourceType", "1.0.0"), serFunc, deserFunc));
        }

        [Test]
        public void RegisterCompoundSymbolFactoryMethodTest()
        {
            Func<ICompoundSymbolDefinition> func = () => null;
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterCompoundSymbolFactoryMethod(null, null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterCompoundSymbolFactoryMethod(new Version(1, 0, 0), null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterCompoundSymbolFactoryMethod(null, func));
            //Already exists
            Assert.Throws<ArgumentException>(() => ObjectFactory.RegisterCompoundSymbolFactoryMethod(new Version(1, 0, 0), func));
            Assert.DoesNotThrow(() => ObjectFactory.RegisterCompoundSymbolFactoryMethod(new Version(3, 0, 0), func));
        }

        [Test]
        public void RegisterSimpleSymbolFactoryMethodTest()
        {
            Func<ISimpleSymbolDefinition> func = () => null;
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterSimpleSymbolFactoryMethod(null, null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterSimpleSymbolFactoryMethod(new Version(1, 0, 0), null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterSimpleSymbolFactoryMethod(null, func));
            //Already exists
            Assert.Throws<ArgumentException>(() => ObjectFactory.RegisterSimpleSymbolFactoryMethod(new Version(1, 0, 0), func));
            Assert.DoesNotThrow(() => ObjectFactory.RegisterSimpleSymbolFactoryMethod(new Version(3, 0, 0), func));
        }

        [Test]
        public void RegisterLayerFactoryMethodTest()
        {
            Func<LayerType, ILayerDefinition> func = (lt) => null;
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterLayerFactoryMethod(null, null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 0, 0), null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterLayerFactoryMethod(null, func));
            //Already exists
            Assert.Throws<ArgumentException>(() => ObjectFactory.RegisterLayerFactoryMethod(new Version(1, 0, 0), func));
            Assert.DoesNotThrow(() => ObjectFactory.RegisterLayerFactoryMethod(new Version(3, 0, 0), func));
        }

        [Test]
        public void RegisterLoadProcedureFactoryMethodTest()
        {
            Func<ILoadProcedure> func = () => null;
            foreach (LoadType lt in Enum.GetValues(typeof(LoadType)))
            {
                Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterLoadProcedureFactoryMethod(LoadType.Dwf, null));
            }
        }

        [Test]
        public void RegisterWebLayoutFactoryMethodTest()
        {
            Func<string, IWebLayout> func = (mdfId) => null;
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterWebLayoutFactoryMethod(null, null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterWebLayoutFactoryMethod(null, func));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterWebLayoutFactoryMethod(new Version(1, 0, 0), null));
            //Already exists
            Assert.Throws<ArgumentException>(() => ObjectFactory.RegisterWebLayoutFactoryMethod(new Version(1, 0, 0), func));
            Assert.DoesNotThrow(() => ObjectFactory.RegisterWebLayoutFactoryMethod(new Version(3, 0, 0), func));
        }

        [Test]
        public void RegisterMapDefinitionFactoryMethodTest()
        {
            Func<IMapDefinition> func = () => null;
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterMapDefinitionFactoryMethod(null, null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterMapDefinitionFactoryMethod(null, func));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterMapDefinitionFactoryMethod(new Version(1, 0, 0), null));
            //Already exists
            Assert.Throws<ArgumentException>(() => ObjectFactory.RegisterMapDefinitionFactoryMethod(new Version(1, 0, 0), func));
            Assert.DoesNotThrow(() => ObjectFactory.RegisterMapDefinitionFactoryMethod(new Version(4, 0, 0), func));
        }

        [Test]
        public void RegisterWatermarkDefinitionFactoryMethodTest()
        {
            Func<SymbolDefinitionType, IWatermarkDefinition> func = (st) => null;
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterWatermarkDefinitionFactoryMethod(null, null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterWatermarkDefinitionFactoryMethod(null, func));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterWatermarkDefinitionFactoryMethod(new Version(1, 0, 0), null));
            //Already exists
            Assert.Throws<ArgumentException>(() => ObjectFactory.RegisterWatermarkDefinitionFactoryMethod(new Version(2, 3, 0), func));
            Assert.DoesNotThrow(() => ObjectFactory.RegisterWatermarkDefinitionFactoryMethod(new Version(4, 0, 0), func));
        }

        [Test]
        public void RegisterTileSetDefinitionFactoryMethodTest()
        {
            Func<ITileSetDefinition> func = () => null;
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterTileSetDefinitionFactoryMethod(null, null));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterTileSetDefinitionFactoryMethod(null, func));
            Assert.Throws<ArgumentNullException>(() => ObjectFactory.RegisterTileSetDefinitionFactoryMethod(new Version(1, 0, 0), null));
            //Already exists
            Assert.Throws<ArgumentException>(() => ObjectFactory.RegisterTileSetDefinitionFactoryMethod(new Version(3, 0, 0), func));
            Assert.DoesNotThrow(() => ObjectFactory.RegisterTileSetDefinitionFactoryMethod(new Version(4, 0, 0), func));
        }

        [Test]
        public void CreateMetadataTest()
        {
            var meta = ObjectFactory.CreateMetadata();
            Assert.NotNull(meta);
            Assert.NotNull(meta.Simple);
            Assert.AreEqual(0, meta.Simple.Property.Count);
        }

        [Test]
        public void CreateSecurityUserTest()
        {
            var user = ObjectFactory.CreateSecurityUser();
            Assert.NotNull(user);
            Assert.AreEqual(0, user.User.Count);
        }

        [Test]
        public void CreateSecurityGroupTest()
        {
            var group = ObjectFactory.CreateSecurityGroup();
            Assert.NotNull(group);
            Assert.NotNull(group.Group);
        }

        [Test]
        public void CreateFeatureSourceExtensionTest()
        {
            var ext = ObjectFactory.CreateFeatureSourceExtension("Foo", "Bar");
            Assert.AreEqual("Bar", ext.FeatureClass);
            Assert.AreEqual("Foo", ext.Name);
            Assert.NotNull(ext.AttributeRelate);
            Assert.NotNull(ext.CalculatedProperty);
            Assert.AreEqual(0, ext.AttributeRelate.Count());
            Assert.AreEqual(0, ext.CalculatedProperty.Count());
        }

        [Test]
        public void CreateCalculatedPropertyTest()
        {
            var calc = ObjectFactory.CreateCalculatedProperty("Foo", "Bar");
            Assert.AreEqual("Foo", calc.Name);
            Assert.AreEqual("Bar", calc.Expression);
        }

        [Test]
        public void CreateAttributeRelationTest()
        {
            var rel = ObjectFactory.CreateAttributeRelation();
            Assert.AreEqual(RelateTypeEnum.LeftOuter, rel.RelateType);
            Assert.False(rel.ForceOneToOne);
            Assert.NotNull(rel.RelateProperty);
            Assert.AreEqual(0, rel.RelatePropertyCount);
        }

        [Test]
        public void CreateEnvelopeTest()
        {
            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateEnvelope(.1, -.1, -.1, .1));
            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateEnvelope(-.1, .1, .1, -.1));
            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateEnvelope(.1, .1, -.1, -.1));
            var env = ObjectFactory.CreateEnvelope(-1, -1, 1, 1);
            Assert.NotNull(env);
            Assert.AreEqual(-1, env.MinX);
            Assert.AreEqual(-1, env.MinY);
            Assert.AreEqual(1, env.MaxX);
            Assert.AreEqual(1, env.MaxY);
        }

        [Test]
        public void CreateDefaultLayerTest()
        {
            var versions = new Version[]
            {
                new Version(1, 0, 0),
                new Version(1, 1, 0),
                new Version(1, 2, 0),
                new Version(1, 3, 0),
                new Version(2, 3, 0),
                new Version(2, 4, 0)
            };

            foreach (var version in versions)
            {
                var dl = ObjectFactory.CreateDefaultLayer(LayerDefinition.LayerType.Drawing, version);
                var gl = ObjectFactory.CreateDefaultLayer(LayerDefinition.LayerType.Raster, version);
                var vl = ObjectFactory.CreateDefaultLayer(LayerDefinition.LayerType.Vector, version);

                Assert.NotNull(dl);
                Assert.NotNull(gl);
                Assert.NotNull(vl);

                Assert.AreEqual(version, dl.ResourceVersion);
                Assert.AreEqual(version, gl.ResourceVersion);
                Assert.AreEqual(version, vl.ResourceVersion);

                Assert.AreEqual(LayerDefinition.LayerType.Drawing, dl.SubLayer.LayerType);
                Assert.AreEqual(LayerDefinition.LayerType.Raster, gl.SubLayer.LayerType);
                Assert.AreEqual(LayerDefinition.LayerType.Vector, vl.SubLayer.LayerType);

                //TODO: Verify content model satisfaction when saving back to XML
            }
        }

        [Test]
        public void CreateDrawingSourceTest()
        {
            var ds = ObjectFactory.CreateDrawingSource();
            Assert.NotNull(ds);
            Assert.NotNull(ds.Sheet);
            Assert.AreEqual(0, ds.Sheet.Count());
            Assert.True(String.IsNullOrEmpty(ds.SourceName));
            Assert.True(String.IsNullOrEmpty(ds.CoordinateSpace));
        }

        [Test]
        public void CreateVectorLayerDefinitionTest()
        {
            var ldf100 = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
            var ldf110 = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 1, 0));
            var ldf120 = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 2, 0));
            var ldf130 = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 3, 0));
            var ldf230 = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(2, 3, 0));
            var ldf240 = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(2, 4, 0));

            var vl100 = (IVectorLayerDefinition)ldf100.SubLayer;
            var vl110 = (IVectorLayerDefinition)ldf110.SubLayer;
            var vl120 = (IVectorLayerDefinition)ldf120.SubLayer;
            var vl130 = (IVectorLayerDefinition)ldf130.SubLayer;
            var vl230 = (IVectorLayerDefinition)ldf230.SubLayer;
            var vl240 = (IVectorLayerDefinition)ldf240.SubLayer;

            Assert.AreEqual(0, vl100.PropertyMapping.Count());
            Assert.AreEqual(0, vl110.PropertyMapping.Count());
            Assert.AreEqual(0, vl120.PropertyMapping.Count());
            Assert.AreEqual(0, vl130.PropertyMapping.Count());
            Assert.AreEqual(0, vl230.PropertyMapping.Count());
            Assert.AreEqual(0, vl240.PropertyMapping.Count());

            Assert.AreEqual(1, vl100.VectorScaleRange.Count());
            Assert.AreEqual(1, vl110.VectorScaleRange.Count());
            Assert.AreEqual(1, vl120.VectorScaleRange.Count());
            Assert.AreEqual(1, vl130.VectorScaleRange.Count());
            Assert.AreEqual(1, vl230.VectorScaleRange.Count());
            Assert.AreEqual(1, vl240.VectorScaleRange.Count());
        }

        [Test]
        public void CreateFeatureSourceTest()
        {
            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            Assert.NotNull(fs);
            Assert.AreEqual("OSGeo.SDF", fs.Provider);
            Assert.False(fs.UsesAliasedDataFiles);
            Assert.False(fs.UsesEmbeddedDataFiles);
            var sc = fs.SupplementalSpatialContextInfo;
            Assert.NotNull(sc);
            Assert.AreEqual(0, sc.Count());
            var ext = fs.Extension;
            Assert.NotNull(ext);
            Assert.AreEqual(0, ext.Count());
            Assert.True(String.IsNullOrEmpty(fs.ConfigurationDocument));
            Assert.AreEqual(0, fs.ConnectionPropertyNames.Length);
        }

        [Test]
        public void CreateFeatureSourceTestWithParameters()
        {
            var param = new NameValueCollection();
            param["File"] = "C:\\Test.sdf";
            param["ReadOnly"] = "TRUE";

            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF", param);
            Assert.NotNull(fs);
            Assert.AreEqual("OSGeo.SDF", fs.Provider);
            Assert.False(fs.UsesAliasedDataFiles);
            Assert.False(fs.UsesEmbeddedDataFiles);
            var sc = fs.SupplementalSpatialContextInfo;
            Assert.NotNull(sc);
            Assert.AreEqual(0, sc.Count());
            var ext = fs.Extension;
            Assert.NotNull(ext);
            Assert.AreEqual(0, ext.Count());
            Assert.True(String.IsNullOrEmpty(fs.ConfigurationDocument));
            Assert.AreEqual(2, fs.ConnectionPropertyNames.Length);
            Assert.False(String.IsNullOrEmpty(fs.GetConnectionProperty("File")));
            Assert.False(String.IsNullOrEmpty(fs.GetConnectionProperty("ReadOnly")));
            var param2 = fs.GetConnectionProperties();
            Assert.AreEqual(2, param2.Count);
            Assert.NotNull(param2["File"]);
            Assert.NotNull(param2["ReadOnly"]);
        }

        [Test]
        public void CreateWatermarkTest()
        {
            var versions = new Version[]
            {
                new Version(2, 3, 0),
                new Version(2, 4, 0)
            };

            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateWatermark(SymbolDefinition.SymbolDefinitionType.Simple, new Version(1, 0, 0)));
            foreach (var version in versions)
            {
                var simpWmd = ObjectFactory.CreateWatermark(SymbolDefinition.SymbolDefinitionType.Simple, version);
                var compWmd = ObjectFactory.CreateWatermark(SymbolDefinition.SymbolDefinitionType.Compound, version);
                Assert.NotNull(simpWmd);
                Assert.NotNull(compWmd);

                Assert.AreEqual(version, simpWmd.ResourceVersion);
                Assert.AreEqual(version, compWmd.ResourceVersion);
            }
        }

        [Test]
        public void CreateSymbolLibraryTest()
        {
            var lib = ObjectFactory.CreateSymbolLibrary();
            Assert.NotNull(lib);
            Assert.NotNull(lib.Symbol);
            Assert.AreEqual(0, lib.Symbol.Count());
        }

        [Test]
        public void CreateLoadProcedureTest()
        {
            foreach (LoadType lt in Enum.GetValues(typeof(LoadType)))
            {
                if (lt == LoadType.Dwg || lt == LoadType.Raster)
                    continue;

                var lp = ObjectFactory.CreateLoadProcedure(lt);
                Assert.NotNull(lp);
                Assert.NotNull(lp.SubType);
                Assert.AreEqual(0, lp.SubType.SourceFile.Count);
            }
        }

        [Test]
        public void CreateLoadProcedureTestWithFileNames()
        {
            foreach (LoadType lt in Enum.GetValues(typeof(LoadType)))
            {
                if (lt == LoadType.Dwg || lt == LoadType.Raster)
                    continue;

                var files = new string[]
                {
                    "C:\\Temp\\Foo.bin",
                    "C:\\Temp\\Bar.bin",
                };
                var lp = ObjectFactory.CreateLoadProcedure(lt, files);
                Assert.NotNull(lp);
                Assert.NotNull(lp.SubType);
                Assert.AreEqual(2, lp.SubType.SourceFile.Count);
                Assert.True(lp.SubType.SourceFile.Contains("C:\\Temp\\Foo.bin"));
                Assert.True(lp.SubType.SourceFile.Contains("C:\\Temp\\Bar.bin"));
            }
        }

        [Test]
        public void CreateMapDefinitionTestWithName()
        {
            var versions = new Version[]
            {
                new Version(1, 0, 0),
                new Version(2, 3, 0),
                new Version(2, 4, 0)
            };
            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateMapDefinition(new Version(1, 2, 0), "Test"));
            foreach (var version in versions)
            {
                var mdf = ObjectFactory.CreateMapDefinition(version, "Test");
                Assert.AreEqual("Test", mdf.Name);
                Assert.True(String.IsNullOrEmpty(mdf.CoordinateSystem));
                Assert.Null(mdf.ExtentCalculator);
                Assert.Null(mdf.BaseMap);
                Assert.NotNull(mdf.MapLayer);
                Assert.AreEqual(0, mdf.MapLayer.Count());
                Assert.NotNull(mdf.MapLayerGroup);
                Assert.AreEqual(0, mdf.MapLayerGroup.Count());
            }
        }

        [Test]
        public void CreateMapDefinitionTestWithNameAndCoordSys()
        {
            var versions = new Version[]
            {
                new Version(1, 0, 0),
                new Version(2, 3, 0),
                new Version(2, 4, 0)
            };
            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateMapDefinition(new Version(1, 2, 0), "Test", "CoordSys"));
            foreach (var version in versions)
            {
                var mdf = ObjectFactory.CreateMapDefinition(version, "Test", "CoordSys");
                Assert.AreEqual("Test", mdf.Name);
                Assert.AreEqual("CoordSys", mdf.CoordinateSystem);
                Assert.Null(mdf.ExtentCalculator);
                Assert.Null(mdf.BaseMap);
                Assert.NotNull(mdf.MapLayer);
                Assert.AreEqual(0, mdf.MapLayer.Count());
                Assert.NotNull(mdf.MapLayerGroup);
                Assert.AreEqual(0, mdf.MapLayerGroup.Count());
            }
        }

        [Test]
        public void CreateMapDefinitionTestWithNameAndCoordSysAndExtent()
        {
            var versions = new Version[]
            {
                new Version(1, 0, 0),
                new Version(2, 3, 0),
                new Version(2, 4, 0)
            };
            var extent = ObjectFactory.CreateEnvelope(-180, -90, 180, 90);
            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateMapDefinition(new Version(1, 2, 0), "Test", "CoordSys", extent));
            foreach (var version in versions)
            {
                var mdf = ObjectFactory.CreateMapDefinition(version, "Test", "CoordSys", extent);
                Assert.AreEqual("Test", mdf.Name);
                Assert.AreEqual("CoordSys", mdf.CoordinateSystem);
                Assert.AreEqual(extent.MinX, mdf.Extents.MinX);
                Assert.AreEqual(extent.MinY, mdf.Extents.MinY);
                Assert.AreEqual(extent.MaxX, mdf.Extents.MaxX);
                Assert.AreEqual(extent.MaxY, mdf.Extents.MaxY);
                Assert.Null(mdf.ExtentCalculator);
                Assert.Null(mdf.BaseMap);
                Assert.NotNull(mdf.MapLayer);
                Assert.AreEqual(0, mdf.MapLayer.Count());
                Assert.NotNull(mdf.MapLayerGroup);
                Assert.AreEqual(0, mdf.MapLayerGroup.Count());
            }
        }

        [Test]
        public void CreateSimpleLabelTest()
        {
            var versions = new Version[]
            {
                new Version(1, 0, 0),
                new Version(1, 1, 0),
                new Version(2, 4, 0)
            };

            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateSimpleLabel(new Version(1, 2, 0), LayerDefinition.GeometryContextType.LineString));
            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateSimpleLabel(new Version(1, 2, 0), LayerDefinition.GeometryContextType.Point));
            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateSimpleLabel(new Version(1, 2, 0), LayerDefinition.GeometryContextType.Polygon));
            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateSimpleLabel(new Version(1, 2, 0), LayerDefinition.GeometryContextType.Unspecified));

            foreach (var version in versions)
            {
                var lnLabel = ObjectFactory.CreateSimpleLabel(version, LayerDefinition.GeometryContextType.LineString);
                var ptLabel = ObjectFactory.CreateSimpleLabel(version, LayerDefinition.GeometryContextType.Point);
                var plLabel = ObjectFactory.CreateSimpleLabel(version, LayerDefinition.GeometryContextType.Polygon);

                Assert.NotNull(lnLabel);
                Assert.NotNull(ptLabel);
                Assert.NotNull(plLabel);
            }
        }

        [Test]
        public void CreateSimplePointTest()
        {
            var versions = new Version[]
            {
                new Version(1, 0, 0),
                new Version(1, 1, 0),
                new Version(2, 4, 0)
            };

            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateSimplePoint(new Version(1, 2, 0)));

            foreach (var version in versions)
            {
                var pt = ObjectFactory.CreateSimplePoint(version);

                Assert.NotNull(pt);
            }
        }

        [Test]
        public void CreateSimpleSolidLineTest()
        {
            var versions = new Version[]
            {
                new Version(1, 0, 0),
                new Version(1, 1, 0),
                new Version(2, 4, 0)
            };

            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateSimpleSolidLine(new Version(1, 2, 0)));

            foreach (var version in versions)
            {
                var pt = ObjectFactory.CreateSimpleSolidLine(version);

                Assert.NotNull(pt);
            }
        }

        [Test]
        public void CreateSimpleSolidFillTest()
        {
            var versions = new Version[]
            {
                new Version(1, 0, 0),
                new Version(1, 1, 0),
                new Version(2, 4, 0)
            };

            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateSimpleSolidFill(new Version(1, 2, 0)));

            foreach (var version in versions)
            {
                var pt = ObjectFactory.CreateSimpleSolidFill(version);

                Assert.NotNull(pt);
            }
        }

        [Test]
        public void CreateSimpleSymbolTest()
        {
            var versions = new Version[]
            {
                new Version(1, 0, 0),
                new Version(1, 1, 0),
                new Version(2, 4, 0)
            };

            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateSimpleSymbol(new Version(1, 2, 0), "Foo", "Bar"));

            foreach (var version in versions)
            {
                var simp = ObjectFactory.CreateSimpleSymbol(version, "Foo", "Bar");

                Assert.NotNull(simp);
                Assert.AreEqual("Foo", simp.Name);
                Assert.AreEqual("Bar", simp.Description);
            }
        }

        [Test]
        public void CreateCompoundSymbolTest()
        {
            var versions = new Version[]
            {
                new Version(1, 0, 0),
                new Version(1, 1, 0),
                new Version(2, 4, 0)
            };

            Assert.Throws<ArgumentException>(() => ObjectFactory.CreateCompoundSymbol(new Version(1, 2, 0), "Foo", "Bar"));

            foreach (var version in versions)
            {
                var comp = ObjectFactory.CreateCompoundSymbol(version, "Foo", "Bar");

                Assert.NotNull(comp);
                Assert.AreEqual("Foo", comp.Name);
                Assert.AreEqual("Bar", comp.Description);
                Assert.NotNull(comp.SimpleSymbol);
                Assert.AreEqual(0, comp.SimpleSymbol.Count());
            }
        }

        [Test]
        public void CreatePrintLayoutTest()
        {
            var pl = ObjectFactory.CreatePrintLayout();
            Assert.NotNull(pl);
            Assert.NotNull(pl.PageProperties);
            Assert.NotNull(pl.LayoutProperties);
            Assert.NotNull(pl.CustomLogos);
            Assert.NotNull(pl.CustomText);
        }

        [Test]
        public void CreatePoint2DTest()
        {
            var pt = ObjectFactory.CreatePoint2D(1, 2);
            Assert.AreEqual(1, pt.X);
            Assert.AreEqual(2, pt.Y);
        }

        [Test]
        public void CreatePoint3DTest()
        {
            var pt = ObjectFactory.CreatePoint3D(1, 2, 3);
            Assert.AreEqual(1, pt.X);
            Assert.AreEqual(2, pt.Y);
            Assert.AreEqual(3, pt.Z);
        }
    }
}