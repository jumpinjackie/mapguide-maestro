#region Disclaimer / License

// Copyright (C) 2021, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class LoadProcedureTests
    {
        class MockConnection : PlatformConnectionBase
        {
            readonly IServerConnection _conn;

            public MockConnection(IServerConnection conn)
            {
                _conn = conn;
            }

            public override string ProviderName => throw new NotImplementedException();

            public override NameValueCollection CloneParameters => throw new NotImplementedException();

            public override string SessionID => throw new NotImplementedException();

            public override FeatureProviderRegistryFeatureProvider[] FeatureProviders => throw new NotImplementedException();

            public override Version SiteVersion => throw new NotImplementedException();

            public override IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter, string[] columns)
            {
                throw new NotImplementedException();
            }

            public override IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter, NameValueCollection aggregateFunctions)
            {
                throw new NotImplementedException();
            }

            public override IServerConnection Clone()
            {
                throw new NotImplementedException();
            }

            public override void CopyFolder(string oldpath, string newpath, bool overwrite)
            {
                throw new NotImplementedException();
            }

            public override void CopyResource(string oldpath, string newpath, bool overwrite)
            {
                throw new NotImplementedException();
            }

            public override void DeleteResource(string resourceID)
            {
                throw new NotImplementedException();
            }

            public override FeatureSchema DescribeFeatureSource(string resourceID, string schema)
            {
                throw new NotImplementedException();
            }

            public override FeatureSchema DescribeFeatureSourcePartial(string resourceID, string schema, string[] classNames)
            {
                throw new NotImplementedException();
            }

            public override DataStoreList EnumerateDataStores(string providerName, string partialConnString)
            {
                throw new NotImplementedException();
            }

            public override ResourceReferenceList EnumerateResourceReferences(string resourceid)
            {
                throw new NotImplementedException();
            }

            public override UnmanagedDataList EnumerateUnmanagedData(string startpath, string filter, bool recursive, UnmanagedDataTypes type)
            {
                throw new NotImplementedException();
            }

            public override string[] GetClassNames(string resourceId, string schemaName)
            {
                throw new NotImplementedException();
            }

            public override object GetCustomProperty(string name)
            {
                throw new NotImplementedException();
            }

            public override string[] GetCustomPropertyNames()
            {
                throw new NotImplementedException();
            }

            public override Type GetCustomPropertyType(string name)
            {
                throw new NotImplementedException();
            }

            public override object GetFolderOrResourceHeader(string resourceId)
            {
                throw new NotImplementedException();
            }

            public override string[] GetIdentityProperties(string resourceID, string classname)
            {
                throw new NotImplementedException();
            }

            public override ILongTransactionList GetLongTransactions(string resourceId, bool activeOnly)
            {
                throw new NotImplementedException();
            }

            public override ResourceList GetRepositoryResources(string startingpoint, string type, int depth, bool computeChildren)
            {
                throw new NotImplementedException();
            }

            public override Stream GetResourceData(string resourceID, string dataname)
            {
                throw new NotImplementedException();
            }

            public override Stream GetResourceXmlData(string resourceID)
            {
                throw new NotImplementedException();
            }

            public override ConfigurationDocument GetSchemaMapping(string provider, string partialConnString)
            {
                throw new NotImplementedException();
            }

            public override string[] GetSchemas(string resourceId)
            {
                throw new NotImplementedException();
            }

            public override FdoSpatialContextList GetSpatialContextInfo(string resourceID, bool activeOnly)
            {
                throw new NotImplementedException();
            }

            public override void MoveFolder(string oldpath, string newpath, bool overwrite)
            {
                throw new NotImplementedException();
            }

            public override void MoveResource(string oldpath, string newpath, bool overwrite)
            {
                throw new NotImplementedException();
            }

            public override IFeatureReader QueryFeatureSource(string resourceID, string className, string filter, string[] propertyNames, NameValueCollection computedProperties)
            {
                throw new NotImplementedException();
            }

            public override void SetCustomProperty(string name, object value)
            {
                throw new NotImplementedException();
            }

            public override void SetResourceData(string resourceid, string dataname, ResourceDataType datatype, Stream stream, Utility.StreamCopyProgressDelegate callback)
            {
                throw new NotImplementedException();
            }

            public override void SetResourceXmlData(string resourceId, Stream content, Stream header)
            {
                throw new NotImplementedException();
            }

            public override string TestConnection(string featuresource)
            {
                throw new NotImplementedException();
            }

            public override void UpdateRepository(string resourceId, ResourceFolderHeaderType header)
            {
                throw new NotImplementedException();
            }

            public override void UploadPackage(string filename, Utility.StreamCopyProgressDelegate callback)
            {
                throw new NotImplementedException();
            }

            protected override FeatureSourceDescription DescribeFeatureSourceInternal(string resourceId)
            {
                throw new NotImplementedException();
            }

            protected override ClassDefinition GetClassDefinitionInternal(string resourceId, string schemaName, string className)
            {
                throw new NotImplementedException();
            }

            protected override IServerConnection GetInterface() => _conn;
        }

        private (Mock<IServerConnection> conn, string resIdRoot, ILoadProcedure loadProc, string[] files) MockSetup(LoadType ltype, params string[] fileNames)
        {
            var resSvc = new Mock<IResourceService>();
            var featSvc = new Mock<IFeatureService>();

            // Have all feature source connection tests return true
            featSvc.Setup(f => f.TestConnection(It.IsAny<string>())).Returns("true");

            // Have the spatial context report LL84 for all spatial context queries
            var scList = new FdoSpatialContextList
            {
                SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>
                {
                    new FdoSpatialContextListSpatialContext
                    {
                        Name = "Default",
                        CoordinateSystemName = "LL84",
                        CoordinateSystemWkt = "GEOGCS[\"LL84\",DATUM[\"WGS84\",SPHEROID[\"WGS84\",6378137.000,298.25722356]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.01745329251994]]"
                    }
                }
            };
            featSvc.Setup(f => f.GetSpatialContextInfo(It.IsAny<string>(), It.IsAny<bool>())).Returns(scList);

            var resIdRoot = "Library://Loaded/";

            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            var lproc = ObjectFactory.CreateLoadProcedure(ltype);
            string[] files;
            if (ltype == LoadType.Shp)
            {
                //files = fileNames.SelectMany(fn => YieldShpFile(fn)).ToArray();
                files = fileNames.Select(fn => Path.Combine(tempDir, fn)).ToArray();
            }
            else
            {
                files = fileNames.Select(fn => Path.Combine(tempDir, fn)).ToArray();
            }
            // Touch all the files referenced
            foreach (var f in files)
            {
                if (!File.Exists(f))
                    File.Create(f).Close();   // close immediately 
                File.SetLastWriteTimeUtc(f, DateTime.UtcNow);

                if (ltype == LoadType.Shp && f.EndsWith(".shp"))
                {
                    var fn = Path.GetFileNameWithoutExtension(f);
                    var expFsId = resIdRoot + "Data/" + fn + ".FeatureSource";

                    var resDataList = new ResourceDataList
                    {
                        ResourceData = new System.ComponentModel.BindingList<ResourceDataListResourceData> ()
                    };
                    resDataList.ResourceData.Add(new ResourceDataListResourceData { Name = fn + ".shp", Type = ResourceDataType.File });
                    resDataList.ResourceData.Add(new ResourceDataListResourceData { Name = fn + ".shx", Type = ResourceDataType.File });
                    resDataList.ResourceData.Add(new ResourceDataListResourceData { Name = fn + ".prj", Type = ResourceDataType.File });
                    resDataList.ResourceData.Add(new ResourceDataListResourceData { Name = fn + ".idx", Type = ResourceDataType.File });
                    resDataList.ResourceData.Add(new ResourceDataListResourceData { Name = fn + ".dbf", Type = ResourceDataType.File });
                    resDataList.ResourceData.Add(new ResourceDataListResourceData { Name = fn + ".cpg", Type = ResourceDataType.File });

                    resSvc.Setup(r => r.EnumerateResourceData(It.Is<string>(s => s == expFsId)))
                        .Returns(resDataList);
                }

                if ((ltype == LoadType.Shp && f.EndsWith(".shp")) || ltype != LoadType.Shp)
                {
                    // Setup walked schema
                    var expFsId = resIdRoot + "Data/" + Path.GetFileNameWithoutExtension(f) + ".FeatureSource";

                    var klass = new ClassDefinition(Path.GetFileNameWithoutExtension(f), "Default Feature Class");
                    klass.AddProperty(new DataPropertyDefinition("Id", "Identity property"), true);
                    klass.AddProperty(new GeometricPropertyDefinition("Geometry", "geometry property"));
                    klass.DefaultGeometryPropertyName = "Geometry";

                    var sc = new FeatureSchema("Default", "Default Schema");
                    sc.AddClass(klass);
                    var fsd = new FeatureSourceDescription(new[] { sc });
                    featSvc.Setup(f => f.DescribeFeatureSource(It.Is<string>(s => s == expFsId))).Returns(fsd);
                }
            }

            var conn = new Mock<IServerConnection>();
            conn.Setup(c => c.ResourceService).Returns(resSvc.Object);
            conn.Setup(c => c.FeatureService).Returns(featSvc.Object);

            return (conn, resIdRoot, lproc, files);
        }

        [Theory]
        [InlineData(LoadType.Sdf, "sdf")]
        [InlineData(LoadType.Shp, "shp")]
        [InlineData(LoadType.Sqlite, "sqlite")]
        public void TestLoadProc(LoadType lt, string ext)
        {
            var (conn, resIdRoot, lproc, files) = MockSetup(lt,
                "Parcels." + ext,
                "Roads." + ext,
                "Districts." + ext,
                "Rivers." + ext);
            var pconn = new MockConnection(conn.Object);

            lproc.SubType.AddFiles(files);

            lproc.SubType.GenerateLayers = true;
            lproc.SubType.GenerateSpatialDataSources = true;
            lproc.SubType.RootPath = resIdRoot;
            lproc.SubType.LayersFolder = "Layers";
            lproc.SubType.SpatialDataSourcesFolder = "Data";

            var loaded = pconn.ExecuteLoadProcedure(lproc, (s, e) => { }, true);
            Assert.Equal(8, loaded.Length);

            foreach (var fn in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(fn);
                var expectFs = lproc.SubType.RootPath + lproc.SubType.SpatialDataSourcesFolder + "/" + fileName + ".FeatureSource";
                var expectLdf = lproc.SubType.RootPath + lproc.SubType.LayersFolder + "/" + fileName + ".LayerDefinition";
                Assert.Contains(expectFs, loaded);
                Assert.Contains(expectLdf, loaded);
            }
        }

        /*
        [Theory]
        [InlineData(LoadType.Raster, "tif")]
        [InlineData(LoadType.Dwg, "dwg")]
        public void TestUnsupported(LoadType lt, string ext)
        {
            var (conn, resIdRoot, lproc, files) = MockSetup(lt,
                "Parcels." + ext,
                "Roads." + ext,
                "Districts." + ext,
                "Rivers." + ext);
            var pconn = new MockConnection(conn.Object);

            lproc.SubType.AddFiles(files);

            lproc.SubType.GenerateLayers = true;
            lproc.SubType.GenerateSpatialDataSources = true;
            lproc.SubType.RootPath = resIdRoot;
            lproc.SubType.LayersFolder = "Layers";
            lproc.SubType.SpatialDataSourcesFolder = "Data";

            Assert.Throws<NotSupportedException>(() => pconn.ExecuteLoadProcedure(lproc, (s, e) => { }, true));
        }
        */
    }
}
