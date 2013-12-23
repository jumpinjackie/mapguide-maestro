#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Internal;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System;
using System.IO;

namespace MaestroAPITests
{
    public abstract class ConnectionTestBase
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            string reason;
            if (ShouldIgnore(out reason))
            {
                Assert.Ignore(reason);
            }
            else
            {
                SetupTestData();
            }
        }

        protected virtual bool ShouldIgnore(out string reason)
        {
            reason = string.Empty;
            return false;
        }

        protected abstract string GetTestPrefix();

        private void SetupTestData()
        {
            var conn = CreateTestConnection();
            var resSvc = conn.ResourceService;

            resSvc.DeleteResource("Library://UnitTests/");

            resSvc.SetResourceXmlData("Library://UnitTests/Maps/Sheboygan.MapDefinition", File.OpenRead("TestData/MappingService/UT_Sheboygan.mdf"));
            resSvc.SetResourceXmlData("Library://UnitTests/Maps/SheboyganTiled.MapDefinition", File.OpenRead("UserTestData/TestTiledMap.xml"));

            resSvc.SetResourceXmlData("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.ldf"));
            resSvc.SetResourceXmlData("Library://UnitTests/Layers/Rail.LayerDefinition", File.OpenRead("TestData/MappingService/UT_Rail.ldf"));
            resSvc.SetResourceXmlData("Library://UnitTests/Layers/Parcels.LayerDefinition", File.OpenRead("TestData/TileService/UT_Parcels.ldf"));

            resSvc.SetResourceXmlData("Library://UnitTests/Data/HydrographicPolygons.FeatureSource", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.fs"));
            resSvc.SetResourceXmlData("Library://UnitTests/Data/Rail.FeatureSource", File.OpenRead("TestData/MappingService/UT_Rail.fs"));
            resSvc.SetResourceXmlData("Library://UnitTests/Data/Parcels.FeatureSource", File.OpenRead("TestData/TileService/UT_Parcels.fs"));

            resSvc.SetResourceData("Library://UnitTests/Data/HydrographicPolygons.FeatureSource", "UT_HydrographicPolygons.sdf", ResourceDataType.File, File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.sdf"));
            resSvc.SetResourceData("Library://UnitTests/Data/Rail.FeatureSource", "UT_Rail.sdf", ResourceDataType.File, File.OpenRead("TestData/MappingService/UT_Rail.sdf"));
            resSvc.SetResourceData("Library://UnitTests/Data/Parcels.FeatureSource", "UT_Parcels.sdf", ResourceDataType.File, File.OpenRead("TestData/TileService/UT_Parcels.sdf"));

            resSvc.SetResourceXmlData("Library://UnitTests/Data/SpaceShip.DrawingSource", File.OpenRead("TestData/DrawingService/SpaceShipDrawingSource.xml"));
            resSvc.SetResourceData("Library://UnitTests/Data/SpaceShip.DrawingSource", "SpaceShip.dwf", ResourceDataType.File, File.OpenRead("TestData/DrawingService/SpaceShip.dwf"));
        }

        protected abstract IServerConnection CreateTestConnection();

        protected void TestFeatureSourceCaching(string fsName)
        {
            var conn = CreateTestConnection();
            string fsId = "Library://UnitTests/" + fsName + ".FeatureSource";
            if (!conn.ResourceService.ResourceExists(fsId))
            {
                var fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SDF");
                fs.SetConnectionProperty("File", "%MG_DATA_FILE_PATH%Sheboygan_Parcels.sdf");
                fs.ResourceID = fsId;
                conn.ResourceService.SaveResourceAs(fs, fsId);
                using (var stream = File.OpenRead("TestData/FeatureService/SDF/Sheboygan_Parcels.sdf"))
                {
                    fs.SetResourceData("Sheboygan_Parcels.sdf", ResourceDataType.File, stream);
                }
                Assert.True(Convert.ToBoolean(conn.FeatureService.TestConnection(fsId)));
            }
            var pc = (PlatformConnectionBase)conn;
            pc.ResetFeatureSourceSchemaCache();

            Assert.AreEqual(0, pc.CachedClassDefinitions);
            Assert.AreEqual(0, pc.CachedFeatureSources);

            var fsd = conn.FeatureService.DescribeFeatureSource(fsId);

            Assert.AreEqual(1, pc.CachedFeatureSources);
            Assert.AreEqual(1, pc.CachedClassDefinitions);

            var fsd2 = conn.FeatureService.DescribeFeatureSource(fsId);

            Assert.AreEqual(1, pc.CachedFeatureSources);
            Assert.AreEqual(1, pc.CachedClassDefinitions);
            //Each cached instance returned is a clone
            Assert.False(object.ReferenceEquals(fsd, fsd2));
        }

        protected void TestClassDefinitionCaching(string fsName)
        {
            var conn = ConnectionUtil.CreateTestHttpConnection();
            string fsId = "Library://UnitTests/" + fsName + ".FeatureSource";
            if (!conn.ResourceService.ResourceExists(fsId))
            {
                var fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SDF");
                fs.SetConnectionProperty("File", "%MG_DATA_FILE_PATH%Sheboygan_Parcels.sdf");
                conn.ResourceService.SaveResourceAs(fs, fsId);
                fs.ResourceID = fsId;
                using (var stream = File.OpenRead("TestData/FeatureService/SDF/Sheboygan_Parcels.sdf"))
                {
                    fs.SetResourceData("Sheboygan_Parcels.sdf", ResourceDataType.File, stream);
                }
                Assert.True(Convert.ToBoolean(conn.FeatureService.TestConnection(fsId)));
            }
            var pc = (PlatformConnectionBase)conn;
            pc.ResetFeatureSourceSchemaCache();

            Assert.AreEqual(0, pc.CachedClassDefinitions);
            Assert.AreEqual(0, pc.CachedFeatureSources);

            var cls = conn.FeatureService.GetClassDefinition(fsId, "SHP_Schema:Parcels");

            Assert.AreEqual(0, pc.CachedFeatureSources);
            Assert.AreEqual(1, pc.CachedClassDefinitions);

            var cls2 = conn.FeatureService.GetClassDefinition(fsId, "SHP_Schema:Parcels");

            Assert.AreEqual(0, pc.CachedFeatureSources);
            Assert.AreEqual(1, pc.CachedClassDefinitions);
            //Each cached instance returned is a clone
            Assert.False(object.ReferenceEquals(cls, cls2));
        }

        protected void CreateTestDataStore(IServerConnection conn, string fsId, ref FeatureSchema schema, ref ClassDefinition cls)
        {
            schema = new FeatureSchema("Default", "");
            cls = new ClassDefinition("Class1", "");

            try
            {
                if (conn.ResourceService.ResourceExists(fsId))
                    conn.ResourceService.DeleteResource(fsId);

                cls.DefaultGeometryPropertyName = "GEOM";
                cls.AddProperty(new DataPropertyDefinition("KEY", "")
                {
                    DataType = DataPropertyType.Int32,
                    IsAutoGenerated = true,
                    IsReadOnly = true,
                    IsNullable = false
                }, true);

                cls.AddProperty(new DataPropertyDefinition("NAME", "")
                {
                    DataType = DataPropertyType.String,
                    Length = 255,
                    IsNullable = true,
                    IsReadOnly = false
                });

                cls.AddProperty(new GeometricPropertyDefinition("GEOM", "")
                {
                    GeometricTypes = FeatureGeometricType.Point,
                    SpatialContextAssociation = "Default"
                });

                schema.AddClass(cls);

                ICreateDataStore create = (ICreateDataStore)conn.CreateCommand((int)CommandType.CreateDataStore);
                CoordinateSystemDefinitionBase coordSys = conn.CoordinateSystemCatalog.FindCoordSys("LL84");
                create.FeatureSourceId = fsId;
                create.CoordinateSystemWkt = coordSys.WKT;
                create.Name = "Default";
                create.ExtentType = OSGeo.MapGuide.ObjectModels.Common.FdoSpatialContextListSpatialContextExtentType.Dynamic;
                create.FileName = "Test.sdf";
                create.Provider = "OSGeo.SDF";
                create.Schema = schema;
                create.XYTolerance = 0.001;
                create.ZTolerance = 0.001;

                create.Execute();
            }
            catch
            {
                schema = null;
                cls = null;
                throw;
            }
        }

        protected void PopulateTestDataStore(IServerConnection conn, string fsId, ref FeatureSchema schema, ref ClassDefinition cls)
        {
            CreateTestDataStore(conn, fsId, ref schema, ref cls);

            IInsertFeatures insert = (IInsertFeatures)conn.CreateCommand((int)CommandType.InsertFeature);
            insert.ClassName = cls.Name;
            insert.FeatureSourceId = fsId;
            var feat = new MutableRecord();

            var reader = new FixedWKTReader();

            //Initialize this record
            feat.PutValue("GEOM", new GeometryValue(reader.Read("POINT (0 0)")));
            feat.PutValue("NAME", new StringValue("Test1"));

            //Attach to command.
            insert.RecordToInsert = feat;

            var res = insert.Execute();

            feat.SetGeometry("GEOM", reader.Read("POINT (0 1)"));
            feat.SetString("NAME", "Test2");

            res = insert.Execute();

            feat.SetGeometry("GEOM", reader.Read("POINT (1 1)"));
            feat.SetString("NAME", "Test3");

            res = insert.Execute();

            feat.SetGeometry("GEOM", reader.Read("POINT (1 0)"));
            feat.SetString("NAME", "Test4");

            res = insert.Execute();
        }

        public virtual void TestInsertFeatures()
        {
            var conn = CreateTestConnection();
            var fsId = "Library://UnitTests/Data/Test" + GetTestPrefix() + "InsertFeatures.FeatureSource";
            ClassDefinition cls = null;
            FeatureSchema schema = null;
            CreateTestDataStore(conn, fsId, ref schema, ref cls);

            IInsertFeatures insert = (IInsertFeatures)conn.CreateCommand((int)CommandType.InsertFeature);
            insert.ClassName = cls.Name;
            insert.FeatureSourceId = fsId;
            var feat = new MutableRecord();

            var reader = new FixedWKTReader();

            //Initialize this record
            feat.PutValue("GEOM", new GeometryValue(reader.Read("POINT (0 0)")));
            feat.PutValue("NAME", new StringValue("Test1"));

            Assert.True(feat.PropertyExists("GEOM"));
            Assert.True(feat.PropertyExists("NAME"));

            //Attach to command.
            insert.RecordToInsert = feat;

            var res = insert.Execute();
            Assert.Null(res.Error);

            feat.SetGeometry("GEOM", reader.Read("POINT (0 1)"));
            feat.SetString("NAME", "Test2");

            res = insert.Execute();
            Assert.Null(res.Error);

            feat.SetGeometry("GEOM", reader.Read("POINT (1 1)"));
            feat.SetString("NAME", "Test3");

            res = insert.Execute();
            Assert.Null(res.Error);

            feat.SetGeometry("GEOM", reader.Read("POINT (1 0)"));
            feat.SetString("NAME", "Test4");

            res = insert.Execute();
            Assert.Null(res.Error);

            int count = 0;
            using (var rdr = conn.FeatureService.QueryFeatureSource(fsId, cls.Name))
            {
                while (rdr.ReadNext())
                {
                    count++;
                }
                rdr.Close();
            }

            Assert.AreEqual(4, count);
        }

        public virtual void TestUpdateFeatures()
        {
            var conn = CreateTestConnection();
            var fsId = "Library://UnitTests/Data/Test" + GetTestPrefix() + "UpdateFeatures.FeatureSource";
            ClassDefinition cls = null;
            FeatureSchema schema = null;
            PopulateTestDataStore(conn, fsId, ref schema, ref cls);

            IUpdateFeatures update = (IUpdateFeatures)conn.CreateCommand((int)CommandType.UpdateFeatures);
            update.ClassName = cls.Name;
            update.FeatureSourceId = fsId;
            update.Filter = "NAME = 'Test4'";

            update.ValuesToUpdate = new MutableRecord();
            update.ValuesToUpdate.PutValue("NAME", new StringValue("Test4Modified"));

            Assert.AreEqual(1, update.Execute());
        }

        public virtual void TestDeleteFeatures()
        {
            var conn = CreateTestConnection();
            var fsId = "Library://UnitTests/Data/Test" + GetTestPrefix() + "DeleteFeatures.FeatureSource";
            ClassDefinition cls = null;
            FeatureSchema schema = null;
            PopulateTestDataStore(conn, fsId, ref schema, ref cls);

            IDeleteFeatures delete = (IDeleteFeatures)conn.CreateCommand((int)CommandType.DeleteFeatures);
            delete.ClassName = cls.Name;
            delete.FeatureSourceId = fsId;
            delete.Filter = "NAME = 'Test4'";

            Assert.AreEqual(1, delete.Execute());

            int count = 0;
            using (var rdr = conn.FeatureService.QueryFeatureSource(fsId, cls.Name))
            {
                while (rdr.ReadNext()) { count++; }
            }

            Assert.AreEqual(3, count);
        }

        public virtual void TestCreateDataStore()
        {
            var conn = CreateTestConnection();
            var fsId = "Library://UnitTests/Data/Test" + GetTestPrefix() + "CreateDataStore.FeatureSource";
            ClassDefinition cls = null;
            FeatureSchema schema = null;
            CreateTestDataStore(conn, fsId, ref schema, ref cls);

            ClassDefinition cls2 = conn.FeatureService.GetClassDefinition(fsId, "Class1");
            Assert.NotNull(cls2);
            Assert.False(ClassDefinition.ReferenceEquals(cls, cls2));

            Assert.AreEqual(cls.Name, cls2.Name);
            Assert.AreEqual(cls.DefaultGeometryPropertyName, cls2.DefaultGeometryPropertyName);
            Assert.AreEqual(cls.Properties.Count, cls2.Properties.Count);
            Assert.AreEqual(cls.IdentityProperties.Count, cls2.IdentityProperties.Count);
            foreach (var prop in cls.Properties)
            {
                var prop2 = cls2.FindProperty(prop.Name);
                Assert.AreEqual(prop.Name, prop2.Name);
                Assert.AreEqual(prop.Type, prop2.Type);
            }
        }

        public virtual void TestApplySchema()
        {
            var fsId = "Library://UnitTests/Data/TestMaestro" + GetTestPrefix() + "ApplySchema.FeatureSource";
            var conn = CreateTestConnection();
            if (conn.ResourceService.ResourceExists(fsId))
                conn.ResourceService.DeleteResource(fsId);

            ClassDefinition cls = null;
            FeatureSchema schema = null;
            CreateTestDataStore(conn, fsId, ref schema, ref cls);

            cls.AddProperty(new DataPropertyDefinition("ExtraProp", "")
            {
                DataType = DataPropertyType.String,
                IsNullable = false,
                Length = 255
            });

            //Apply changes
            IApplySchema cmd = (IApplySchema)conn.CreateCommand((int)CommandType.ApplySchema);
            cmd.Schema = schema;
            cmd.FeatureSourceId = fsId;
            cmd.Execute();

            ClassDefinition cls2 = conn.FeatureService.GetClassDefinition(cmd.FeatureSourceId, "Class1");
            Assert.NotNull(cls2);
            Assert.False(ClassDefinition.ReferenceEquals(cls, cls2));

            Assert.AreEqual(cls.Name, cls2.Name);
            Assert.AreEqual(cls.DefaultGeometryPropertyName, cls2.DefaultGeometryPropertyName);
            Assert.AreEqual(cls.Properties.Count, cls2.Properties.Count);
            Assert.AreEqual(cls.IdentityProperties.Count, cls2.IdentityProperties.Count);
            foreach (var prop in cls.Properties)
            {
                var prop2 = cls2.FindProperty(prop.Name);
                Assert.AreEqual(prop.Name, prop2.Name);
                Assert.AreEqual(prop.Type, prop2.Type);
            }
        }

        //[Test]
        public virtual void TestEncryptedFeatureSourceCredentials()
        {
            //Sensitive data redacted, nevertheless you can test and verify this by filling in the
            //blanks here and uncommenting the above [Test] attribute. If the test passes, credential encryption is working
            string server = "";
            string database = "";
            string actualUser = "";
            string actualPass = "";
            string bogusUser = "foo";
            string bogusPass = "bar";

            var conn = ConnectionUtil.CreateTestHttpConnection();
            string fsId = "Library://UnitTests/" + GetTestPrefix() + "EncryptedCredentials.FeatureSource";
            var fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SQLServerSpatial");
            fs.SetConnectionProperty("Username", "%MG_USERNAME%");
            fs.SetConnectionProperty("Password", "%MG_PASSWORD%");
            fs.SetConnectionProperty("Service", server);
            fs.SetConnectionProperty("DataStore", database);
            fs.ResourceID = fsId;
            conn.ResourceService.SaveResource(fs);

            using (var ms = CredentialWriter.Write(actualUser, actualPass))
            {
                conn.ResourceService.SetResourceData(fsId, "MG_USER_CREDENTIALS", ResourceDataType.String, ms);
            }

            string result = conn.FeatureService.TestConnection(fsId);
            Assert.AreEqual("TRUE", result.ToUpper());

            //Test convenience method
            fsId = "Library://UnitTests/" + GetTestPrefix() + "EncryptedCredentials2.FeatureSource";
            fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SQLServerSpatial");
            fs.SetConnectionProperty("Username", "%MG_USERNAME%");
            fs.SetConnectionProperty("Password", "%MG_PASSWORD%");
            fs.SetConnectionProperty("Service", server);
            fs.SetConnectionProperty("DataStore", database);
            fs.ResourceID = fsId;
            conn.ResourceService.SaveResource(fs);
            fs.SetEncryptedCredentials(actualUser, actualPass);

            result = conn.FeatureService.TestConnection(fsId);
            Assert.AreEqual("TRUE", result.ToUpper());
            Assert.AreEqual(actualUser, fs.GetEncryptedUsername());

            //Do not set encrypted credentials
            fsId = "Library://UnitTests/" + GetTestPrefix() + "EncryptedCredentials3.FeatureSource";
            fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SQLServerSpatial");
            fs.SetConnectionProperty("Username", "%MG_USERNAME%");
            fs.SetConnectionProperty("Password", "%MG_PASSWORD%");
            fs.SetConnectionProperty("Service", server);
            fs.SetConnectionProperty("DataStore", database);
            fs.ResourceID = fsId;
            conn.ResourceService.SaveResource(fs);

            try
            {
                result = conn.FeatureService.TestConnection(fsId);
                Assert.AreEqual("FALSE", result.ToUpper());
            }
            catch //Exception or false I can't remember, as long as the result is not "true"
            {

            }

            //Encrypt credentials, but use bogus username/password
            fsId = "Library://UnitTests/" + GetTestPrefix() + "EncryptedCredentials4.FeatureSource";
            fs = ObjectFactory.CreateFeatureSource(conn, "OSGeo.SQLServerSpatial");
            fs.SetConnectionProperty("Username", "%MG_USERNAME%");
            fs.SetConnectionProperty("Password", "%MG_PASSWORD%");
            fs.SetConnectionProperty("Service", server);
            fs.SetConnectionProperty("DataStore", database);
            fs.ResourceID = fsId;
            conn.ResourceService.SaveResource(fs);
            fs.SetEncryptedCredentials(bogusUser, bogusPass);

            try
            {
                result = conn.FeatureService.TestConnection(fsId);
                Assert.AreEqual("FALSE", result.ToUpper());
            }
            catch
            {
                //Exception or false I can't remember, as long as the result is not "true"
            }
            Assert.AreEqual(bogusUser, fs.GetEncryptedUsername());
        }

        public virtual void TestTouch()
        {
            var conn = CreateTestConnection();
            var resSvc = conn.ResourceService;
            if (!resSvc.ResourceExists("Library://UnitTests/Data/HydrographicPolygons.FeatureSource"))
                resSvc.SetResourceXmlData("Library://UnitTests/Data/HydrographicPolygons.FeatureSource", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.fs"));

            resSvc.Touch("Library://UnitTests/Data/HydrographicPolygons.FeatureSource");
        }

        public virtual void TestAnyStreamInput()
        {
            string source = "Library://UnitTests/Data/HydrographicPolygons.FeatureSource";
            string target = "Library://UnitTests/Data/TestAnyStreamInput.FeatureSource";

            var conn = CreateTestConnection();
            var resSvc = conn.ResourceService;
            if (!resSvc.ResourceExists(source))
                resSvc.SetResourceXmlData(source, File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.fs"));

            resSvc.SetResourceXmlData(target, resSvc.GetResourceXmlData(source));

            string dataName = "UT_HydrographicPolygons.sdf";
            var resDataList = resSvc.EnumerateResourceData(source);
            if (resDataList.ResourceData.Count == 1)
                dataName = resDataList.ResourceData[0].Name;
            else
                resSvc.SetResourceData(source, dataName, ResourceDataType.File, File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.sdf"));

            resSvc.SetResourceData(target,
                                   dataName,
                                   ResourceDataType.File,
                                   resSvc.GetResourceData(source, dataName));
        }

        public abstract IServerConnection CreateFromExistingSession(IServerConnection orig);

        public virtual void TestCreateFromExistingSession()
        {
            var conn = CreateTestConnection();
            var conn2 = CreateFromExistingSession(conn);

            //This connection cannot restart sessions, and cannot be set to restart sessions
            Assert.False(conn2.AutoRestartSession);
            Assert.Throws<InvalidOperationException>(() => { conn2.AutoRestartSession = true; });

            //Exercise an API to check the minimum parameters are met
            try
            {
                var result = conn2.ResourceService.GetRepositoryResources();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        public virtual void TestResourceExists()
        {
            var conn = CreateTestConnection();
            Assert.True(conn.ResourceService.ResourceExists("Library://UnitTests/Maps/Sheboygan.MapDefinition"));
            Assert.False(conn.ResourceService.ResourceExists("Library://UnitTests/IDontExist.MapDefinition"));
        }

        public virtual void TestSchemaMapping()
        {
            var conn = CreateTestConnection();
            var doc1 = conn.FeatureService.GetSchemaMapping("OSGeo.WMS", "FeatureServer=http://wms.jpl.nasa.gov/wms.cgi");
            Assert.NotNull(doc1);
            Assert.True(doc1 is WmsConfigurationDocument);
        }

        public virtual void TestCreateRuntimeMapWithInvalidLayersErrorsEnabled()
        {
            var conn = CreateTestConnection();
            var resSvc = conn.ResourceService;
            resSvc.SetResourceXmlData("Library://UnitTests/Maps/SheboyganWithInvalidLayers.MapDefinition", File.OpenRead("UserTestData/TestMapWithInvalidLayers.xml"));
            resSvc.SetResourceXmlData("Library://UnitTests/Layers/InvalidLayer.LayerDefinition", File.OpenRead("UserTestData/InvalidLayer.xml"));
            
            if (Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Mapping) < 0)
            {
                Assert.Ignore("Connection does not support the Mapping Service");
            }
            else
            {
                var mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
                var mdf = (IMapDefinition)resSvc.GetResource("Library://UnitTests/Maps/SheboyganWithInvalidLayers.MapDefinition");
                try
                {
                    mapSvc.CreateMap(mdf, false);
                    Assert.Fail("CreateMap should've thrown an exception with suppressErrors = false");
                }
                catch (Exception ex)
                {
                    Assert.True(true, ex.ToString());
                }
            }
        }

        public virtual void TestCreateRuntimeMapWithInvalidLayersErrorsDisabled()
        {
            var conn = CreateTestConnection();
            var resSvc = conn.ResourceService;
            resSvc.SetResourceXmlData("Library://UnitTests/Maps/SheboyganWithInvalidLayers.MapDefinition", File.OpenRead("UserTestData/TestMapWithInvalidLayers.xml"));
            resSvc.SetResourceXmlData("Library://UnitTests/Layers/InvalidLayer.LayerDefinition", File.OpenRead("UserTestData/InvalidLayer.xml"));

            if (Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Mapping) < 0)
            {
                Assert.Ignore("Connection does not support the Mapping Service");
            }
            else
            {
                var mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
                var mdf = (IMapDefinition)resSvc.GetResource("Library://UnitTests/Maps/SheboyganWithInvalidLayers.MapDefinition");
                try
                {
                    mapSvc.CreateMap(mdf, true);
                    Assert.True(true);
                }
                catch (Exception ex)
                {
                    Assert.Fail("CreateMap with suppressErrors = true should not have thrown an exception: " + ex.ToString());
                }
            }
        }
    }
}
