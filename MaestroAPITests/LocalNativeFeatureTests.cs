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
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using OSGeo.MapGuide.MaestroAPI.Internal;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using System.IO;

namespace MaestroAPITests
{
    public abstract class LocalNativeFeatureTestsBase
    {
        protected abstract IServerConnection CreateTestConnection();

        protected void TestFeatureSourceCaching(string fsName)
        {
            var conn = ConnectionUtil.CreateTestHttpConnection();
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
            var fsId = "Library://UnitTests/Data/TestInsertFeatures.FeatureSource";
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
            var fsId = "Library://UnitTests/Data/TestUpdateFeatures.FeatureSource";
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
            var fsId = "Library://UnitTests/Data/TestDeleteFeatures.FeatureSource";
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
            var fsId = "Library://UnitTests/Data/TestCreateDataStore.FeatureSource";
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
            var fsId = "Library://UnitTests/Data/TestMaestroLocalApplySchema.FeatureSource";
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
    }

    [TestFixture(Ignore = TestControl.IgnoreLocalNativeFeatureTests)]
    public class LocalNativeFeatureTests : LocalNativeFeatureTestsBase
    {
        protected override IServerConnection CreateTestConnection()
        {
            return LocalNativeConnectionUtil.CreateTestConnection();
        }

        [Test]
        public void TestFeatureSourceCaching()
        {
            base.TestFeatureSourceCaching("LocalNativeFeatureSourceCaching");
        }

        [Test]
        public void TestClassDefinitionCaching()
        {
            base.TestClassDefinitionCaching("LocalNativeClassCaching");
        }

        [Test]
        public override void TestApplySchema()
        {
            base.TestApplySchema();
        }

        [Test]
        public override void TestCreateDataStore()
        {
            base.TestCreateDataStore();
        }

        [Test]
        public override void TestDeleteFeatures()
        {
            base.TestDeleteFeatures();
        }

        [Test]
        public override void TestInsertFeatures()
        {
            base.TestInsertFeatures();
        }

        [Test]
        public override void TestUpdateFeatures()
        {
            base.TestUpdateFeatures();
        }
    }

    [TestFixture(Ignore = TestControl.IgnoreLocalFeatureTests)]
    public class LocalFeatureTests : LocalNativeFeatureTestsBase
    {
        protected override IServerConnection CreateTestConnection()
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.Local", "ConfigFile", "Platform.ini");
        }

        [Test]
        public void TestFeatureSourceCaching()
        {
            base.TestFeatureSourceCaching("LocalFeatureSourceCaching");
        }

        [Test]
        public void TestClassDefinitionCaching()
        {
            base.TestClassDefinitionCaching("LocalClassCaching");
        }

        [Test]
        public override void TestApplySchema()
        {
            base.TestApplySchema();
        }

        [Test]
        public override void TestCreateDataStore()
        {
            base.TestCreateDataStore();
        }

        [Test]
        public override void TestDeleteFeatures()
        {
            base.TestDeleteFeatures();
        }

        [Test]
        public override void TestInsertFeatures()
        {
            base.TestInsertFeatures();
        }

        [Test]
        public override void TestUpdateFeatures()
        {
            base.TestUpdateFeatures();
        }
    }

}
