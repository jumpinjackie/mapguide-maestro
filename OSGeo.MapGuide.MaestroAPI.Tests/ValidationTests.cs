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
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class ValidationTests : IDisposable
    {
        public ValidationTests()
        {
            ResourceValidatorLoader.LoadStockValidators();
        }

        public void Dispose() { }

        [Fact]
        public void TestUniqueIssues()
        {
            //Test for the purposes of equality and hashing that any two issues with
            //the same set of resource id, status and message are identical.

            var res1 = new Mock<IResource>();
            res1.Setup(r => r.ResourceID).Returns("Library://Test.FeatureSource");
            var res2 = new Mock<IResource>();
            res2.Setup(r => r.ResourceID).Returns("Library://Test.FeatureSource");
            var res3 = new Mock<IResource>();
            res3.Setup(r => r.ResourceID).Returns("Library://Test.FeatureSource");

            var msg1 = new ValidationIssue(res1.Object, ValidationStatus.Error, ValidationStatusCode.Dummy, "Epic Fail");
            var msg2 = new ValidationIssue(res2.Object, ValidationStatus.Error, ValidationStatusCode.Dummy, "Epic Fail");
            var msg3 = new ValidationIssue(res3.Object, ValidationStatus.Error, ValidationStatusCode.Dummy, "Not so epic fail");

            Assert.Equal(msg1.GetHashCode(), msg2.GetHashCode());
            Assert.True(msg1.Equals(msg2));

            Assert.NotEqual(msg1.GetHashCode(), msg3.GetHashCode());
            Assert.NotEqual(msg2.GetHashCode(), msg3.GetHashCode());
            Assert.False(msg1.Equals(msg3));
            Assert.False(msg2.Equals(msg3));
        }

        [Fact]
        public void TestValidationResultSet()
        {
            var res1 = new Mock<IResource>();
            res1.Setup(r => r.ResourceID).Returns("Library://Test.FeatureSource");
            var res2 = new Mock<IResource>();
            res2.Setup(r => r.ResourceID).Returns("Library://Test.FeatureSource");
            var res3 = new Mock<IResource>();
            res3.Setup(r => r.ResourceID).Returns("Library://Test.FeatureSource");

            var msg1 = new ValidationIssue(res1.Object, ValidationStatus.Error, ValidationStatusCode.Dummy, "Epic Fail");
            var msg2 = new ValidationIssue(res2.Object, ValidationStatus.Error, ValidationStatusCode.Dummy, "Epic Fail");
            var msg3 = new ValidationIssue(res3.Object, ValidationStatus.Error, ValidationStatusCode.Dummy, "Not so epic fail");

            var set = new ValidationResultSet();
            set.AddIssues(new ValidationIssue[] { msg1, msg2, msg3 });

            var totalIssues = set.GetAllIssues();
            Assert.Equal(2, totalIssues.Length);
            Assert.Single(set.ResourceIDs);
            Assert.Equal(2, set.GetIssuesForResource("Library://Test.FeatureSource", ValidationStatus.Error).Count);
        }

        [Fact]
        public void TestLoadProcedureValidation()
        {
            //TODO: Update these test to use pre-defined XML fragments
            //since ObjectFactory should correctly forbid creation of
            //unsupported types

            var id = "Library://Test.LoadProcedure";
            var conn = new Mock<IServerConnection>();
            var featSvc = new Mock<IFeatureService>();
            var resSvc = new Mock<IResourceService>();
            conn.Setup(c => c.FeatureService).Returns(featSvc.Object);
            conn.Setup(c => c.ResourceService).Returns(resSvc.Object);
            var lp = ObjectFactory.CreateLoadProcedure(LoadType.Sdf, new string[]
            {
                "C:\\foo.sdf",
                "C:\\bar.sdf"
            });
            lp.ResourceID = id;

            var context = new ResourceValidationContext(conn.Object);
            var set = new ValidationResultSet();
            context.Reset();
            set.AddIssues(ResourceValidatorSet.Validate(context, lp, false));

            //SDF2, generalization and 2 missing files
            Assert.Equal(4, set.GetAllIssues().Length);

            lp = ObjectFactory.CreateLoadProcedure(LoadType.Shp, new string[]
            {
                "C:\\foo.shp",
                "C:\\bar.shp"
            });
            lp.ResourceID = id;

            set = new ValidationResultSet();
            context.Reset();
            set.AddIssues(ResourceValidatorSet.Validate(context, lp, false));

            //SDF3 conversion, generalization and 2 missing files
            Assert.Equal(4, set.GetAllIssues().Length);

            lp = new OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.LoadProcedure()
            {
                Item = new OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.DwgLoadProcedureType()
            };
            lp.ResourceID = id;
            set = new ValidationResultSet();
            context.Reset();
            set.AddIssues(ResourceValidatorSet.Validate(context, lp, false));

            //Not supported
            Assert.Single(set.GetAllIssues());

            lp = new OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.LoadProcedure()
            {
                Item = new OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.RasterLoadProcedureType()
            };
            lp.ResourceID = id;
            set = new ValidationResultSet();
            context.Reset();
            set.AddIssues(ResourceValidatorSet.Validate(context, lp, false));

            //Not supported
            Assert.Single(set.GetAllIssues());
        }

        static T CreateInstance<T>() where T : class
        {
            var bf = BindingFlags.OptionalParamBinding | BindingFlags.Instance | BindingFlags.NonPublic;
            var ctor = typeof(T).GetConstructor(bf, null, new[] { typeof(string) }, null);

            //var ctor = typeof(T).GetInternalConstructor(new Type[0]);
            //return ctor.Invoke(new object[0]) as T;
            return ctor.Invoke(BindingFlags.OptionalParamBinding | BindingFlags.InvokeMethod | BindingFlags.CreateInstance, null, new[] { Type.Missing }, null) as T;
        }

        [Fact]
        public void TestLayerScaleRangeOverlap()
        {
            var conn = new Mock<IServerConnection>();
            var ldf1 = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
            ldf1.ResourceID = "Library://Test/Foo.LayerDefinition";

            var vl1 = (IVectorLayerDefinition)ldf1.SubLayer;
            vl1.ResourceId = "Library://Test/Foo.FeatureSource";
            vl1.FeatureName = "Foo:Bar";
            vl1.Geometry = "Geometry";

            var vsr1 = ldf1.CreateVectorScaleRange();
            var vsr2 = ldf1.CreateVectorScaleRange();

            vsr1.MaxScale = 9999;
            vsr2.MinScale = 9384;
            vsr2.MaxScale = 11000;

            vl1.RemoveAllScaleRanges();
            vl1.AddVectorScaleRange(vsr1);
            vl1.AddVectorScaleRange(vsr2);

            var featSvc = new Mock<IFeatureService>();
            var resSvc = new Mock<IResourceService>();

            var validator = CreateInstance<LayerDefinitionValidator>();

            var context = new ResourceValidationContext(conn.Object);
            var issues = validator.Validate(context, ldf1, false);

            bool hasIssue = false;
            foreach (var issue in issues)
            {
                if (issue.StatusCode == ValidationStatusCode.Info_LayerDefinition_ScaleRangeOverlap)
                {
                    hasIssue = true;
                    break;
                }
            }

            Assert.True(hasIssue);

            //Case described in trac #1472
            var ldf2 = ObjectFactory.CreateDefaultLayer(OSGeo.MapGuide.ObjectModels.LayerDefinition.LayerType.Vector, new Version(1, 0, 0));
            ldf2.ResourceID = "Library://Test/Foo.LayerDefinition";

            var vl2 = (IVectorLayerDefinition)ldf2.SubLayer;
            vl2.ResourceId = "Library://Test/Foo.FeatureSource";
            vl2.FeatureName = "Foo:Bar";
            vl2.Geometry = "Geometry";

            vsr1 = ldf2.CreateVectorScaleRange();
            vsr2 = ldf2.CreateVectorScaleRange();

            vsr1.MinScale = 10000;
            vsr2.MinScale = 9384;
            vsr2.MaxScale = 9999;

            vl2.RemoveAllScaleRanges();
            vl2.AddVectorScaleRange(vsr1);
            vl2.AddVectorScaleRange(vsr2);

            context = new ResourceValidationContext(conn.Object);
            issues = validator.Validate(context, ldf2, false);

            hasIssue = false;
            foreach (var issue in issues)
            {
                if (issue.StatusCode == ValidationStatusCode.Info_LayerDefinition_ScaleRangeOverlap)
                {
                    hasIssue = true;
                    break;
                }
            }

            Assert.False(hasIssue);
        }

        [Fact]
        public void TestCase1896()
        {
            //This test case is for ticket 1896: Maestro layer validation incorrectly report missing primary key

            var fs = new FeatureSchema("Test", "");
            var doc = new XmlDocument();

            doc.Load(Utils.OpenFile($"UserTestData{System.IO.Path.DirectorySeparatorChar}1896.xml"));

            var mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("xs", XmlNamespaces.XS);
            mgr.AddNamespace("xsi", XmlNamespaces.XSI);
            mgr.AddNamespace("fdo", XmlNamespaces.FDO);
            mgr.AddNamespace("gml", XmlNamespaces.GML);
            mgr.AddNamespace("xlink", XmlNamespaces.XLINK);
            mgr.AddNamespace("fds", XmlNamespaces.FDS);

            fs.ReadXml(doc.SelectSingleNode("xs:schema", mgr), mgr);

            foreach (var cls in fs.Classes)
            {
                Assert.True(cls.IdentityProperties.Count > 0, "Expected identity properties in: " + cls.QualifiedName);
            }
        }

        [Fact]
        public void TestCase_FsValidator_ValidatesOdbcConfigurationIssues()
        {
            var conf = new OdbcConfigurationDocument();
            var sc = CreateTestSpatialContext("Default");

            var schema = new FeatureSchema("Default", "Default Schema");
            ClassDefinition klass = CreateTestClass(sc.Name);
            GeometricPropertyDefinition geom = klass.Properties.OfType<GeometricPropertyDefinition>().FirstOrDefault(p => p.Name == klass.DefaultGeometryPropertyName);
            
            schema.AddClass(klass);

            conf.AddSchema(schema);
            conf.AddSpatialContext(sc);

            var odbcTable = new OdbcTableItem()
            {
                ClassName = klass.Name,
                SchemaName = schema.Name,
                SpatialContextName = sc.Name,
                XColumn = "X",
                YColumn = null
            };
            conf.AddOverride(odbcTable);

            Func<Stream> xmlStreamFunc = () => new MemoryStream(Encoding.UTF8.GetBytes(conf.ToXml()));

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            var resId = "Library://Test.FeatureSource";
            var dataName = "config.xml";

            mockFeatSvc.Setup(fs => fs.TestConnection(It.Is<string>(arg => arg == resId))).Returns("true");
            mockFeatSvc.Setup(fs => fs.GetSpatialContextInfo(It.Is<string>(arg => arg == resId), It.IsAny<bool>())).Returns(new FdoSpatialContextList
            {
                SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>()
                {
                    sc
                }
            });
            mockFeatSvc.Setup(fs => fs.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schema.Name });

            mockResSvc.Setup(rs => rs.GetResourceData(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == dataName))).Returns(xmlStreamFunc);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var mockFs = new Mock<IFeatureSource>();
            mockFs.Setup(fs => fs.ConfigurationDocument).Returns(dataName);
            mockFs.Setup(fs => fs.ResourceID).Returns(resId);
            mockFs.Setup(fs => fs.Extension).Returns(Enumerable.Empty<IFeatureSourceExtension>());
            mockFs.Setup(fs => fs.ResourceType).Returns(ResourceTypes.FeatureSource.ToString());
            mockFs.Setup(fs => fs.Provider).Returns("OSGeo.SDF");
            mockFs.Setup(fs => fs.Serialize()).Returns(string.Empty);

            //XYZ column misconfigurations

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, mockFs.Object, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_OdbcConfig_IncompleteXYZColumnMapping, issues.First().StatusCode);

            odbcTable.XColumn = null;
            odbcTable.YColumn = "Y";

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mockFs.Object, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_OdbcConfig_IncompleteXYZColumnMapping, issues.First().StatusCode);

            odbcTable.XColumn = null;
            odbcTable.YColumn = "Y";
            odbcTable.ZColumn = "Z";

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mockFs.Object, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_OdbcConfig_IncompleteXYZColumnMapping, issues.First().StatusCode);

            odbcTable.XColumn = "X";
            odbcTable.YColumn = null;
            odbcTable.ZColumn = "Z";

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mockFs.Object, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_OdbcConfig_IncompleteXYZColumnMapping, issues.First().StatusCode);

            odbcTable.XColumn = "X";
            odbcTable.YColumn = "Y";

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mockFs.Object, false);
            Assert.Empty(issues);

            //Bogus mapping class target
            odbcTable.ClassName = "IDontExist";

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mockFs.Object, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_OdbcConfig_NoTableOverrideForFeatureClass, issues.First().StatusCode);

            //Bogus logical geometry property
            odbcTable.ClassName = klass.Name;
            geom.GeometricTypes = FeatureGeometricType.Surface;

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mockFs.Object, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_OdbcConfig_InvalidLogicalGeometryProperty, issues.First().StatusCode);

            //All good
            geom.GeometricTypes = FeatureGeometricType.Point;
            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mockFs.Object, false);
            Assert.Empty(issues);
        }

        private ClassDefinition CreateTestClass(string scName, string className = "Test", string idName = "ID", string geomName = "Geometry")
        {
            var klass = new ClassDefinition(className, "Test Class");

            var id = new DataPropertyDefinition(idName, "Identity");
            var geom = new GeometricPropertyDefinition(geomName, "geometry")
            {
                GeometricTypes = FeatureGeometricType.Point
            };
            geom.SpatialContextAssociation = scName;
            klass.AddProperty(id, true);
            klass.AddProperty(geom);
            klass.DefaultGeometryPropertyName = geom.Name;

            return klass;
        }

        private static FdoSpatialContextListSpatialContext CreateTestSpatialContext(string name, string wkt = "")
        {
            var sc = new FdoSpatialContextListSpatialContext();
            sc.CoordinateSystemName = "LL84";
            sc.CoordinateSystemWkt = wkt;
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
            sc.Name = name;
            sc.XYTolerance = 0.0001;
            sc.ZTolerance = 0.0001;
            return sc;
        }

        [Fact]
        public void TestCase_FsValidator_FailedTestConnection()
        {
            var resId = "Library://Test.FeatureSource";
            var dataName = "config.xml";
            var schemaName = "Default";

            var sc = CreateTestSpatialContext("Default");
            var scList = new FdoSpatialContextList
            {
                SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>() { sc }
            };

            var mockExt = new Mock<IFeatureSourceExtension>();
            var mockRelate = new Mock<IAttributeRelation>();
            mockRelate.Setup(r => r.Name).Returns("abcd");

            mockExt.Setup(ext => ext.AttributeRelate).Returns(new[] { mockRelate.Object });

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(rs => rs.GetResourceData(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == dataName))).Returns((Stream)null);
            mockFeatSvc.Setup(fs => fs.TestConnection(It.Is<string>(arg => arg == resId))).Returns("false");
            mockFeatSvc.Setup(fs => fs.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fs => fs.GetSpatialContextInfo(It.Is<string>(arg => arg == resId), It.IsAny<bool>())).Returns(scList);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var mockFs = new Mock<IFeatureSource>();
            mockFs.Setup(fs => fs.ConfigurationDocument).Returns(dataName);
            mockFs.Setup(fs => fs.ResourceID).Returns(resId);
            mockFs.Setup(fs => fs.Extension).Returns(new[] { mockExt.Object });
            mockFs.Setup(fs => fs.ResourceType).Returns(ResourceTypes.FeatureSource.ToString());
            mockFs.Setup(fs => fs.Provider).Returns("OSGeo.SDF");
            mockFs.Setup(fs => fs.Serialize()).Returns(string.Empty);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, mockFs.Object, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_FeatureSource_ConnectionTestFailed, issues.First().StatusCode);
        }

        [Fact]
        public void TestCase_FsValidator_Credentials()
        {
            var resId = "Library://Test.FeatureSource";
            var dataName = "config.xml";
            var schemaName = "Default";

            var sc = CreateTestSpatialContext("Default");
            var scList = new FdoSpatialContextList
            {
                SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>() { sc }
            };

            var mockExt = new Mock<IFeatureSourceExtension>();
            var mockRelate = new Mock<IAttributeRelation>();
            mockRelate.Setup(r => r.Name).Returns("abcd");

            mockExt.Setup(ext => ext.AttributeRelate).Returns(new[] { mockRelate.Object });

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(rs => rs.GetResourceData(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == dataName))).Returns((Stream)null);
            mockResSvc.Setup(rs => rs.EnumerateResourceData(It.IsAny<string>())).Returns(new ResourceDataList { ResourceData = new System.ComponentModel.BindingList<ResourceDataListResourceData>() });
            mockFeatSvc.Setup(fs => fs.TestConnection(It.Is<string>(arg => arg == resId))).Returns("true");
            mockFeatSvc.Setup(fs => fs.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fs => fs.GetSpatialContextInfo(It.Is<string>(arg => arg == resId), It.IsAny<bool>())).Returns(scList);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var mockFs = ObjectFactory.CreateFeatureSource("OSGeo.ODBC");
            mockFs.ResourceID = resId;
            mockFs.SetConnectionProperty("Username", "admin");
            mockFs.SetConnectionProperty("Password", "password");

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, mockFs, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Warning_FeatureSource_Plaintext_Credentials, issues.First().StatusCode);

            mockFs.SetConnectionProperty("Username", StringConstants.MgUsernamePlaceholder);
            mockFs.SetConnectionProperty("Password", StringConstants.MgPasswordPlaceholder);

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mockFs, false);
            Assert.Equal(2, issues.Count());
            Assert.Equal(ValidationStatusCode.Warning_FeatureSource_Cannot_Package_Secured_Credentials, issues.First().StatusCode);
            Assert.Equal(ValidationStatusCode.Error_FeatureSource_SecuredCredentialTokensWithoutSecuredCredentialData, issues.Last().StatusCode);
        }

        [Fact]
        public void TestCase_FsValidator_SchemaRead()
        {
            var resId = "Library://Test.FeatureSource";
            var dataName = "config.xml";

            var sc = CreateTestSpatialContext("Default");
            var scList = new FdoSpatialContextList
            {
                SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>() { sc }
            };

            var mockExt = new Mock<IFeatureSourceExtension>();
            var mockRelate = new Mock<IAttributeRelation>();
            mockRelate.Setup(r => r.Name).Returns("abcd");

            mockExt.Setup(ext => ext.AttributeRelate).Returns(new[] { mockRelate.Object });

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(rs => rs.GetResourceData(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == dataName))).Returns((Stream)null);
            mockFeatSvc.Setup(fs => fs.TestConnection(It.Is<string>(arg => arg == resId))).Returns("true");
            mockFeatSvc.Setup(fs => fs.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new string[0]);
            mockFeatSvc.Setup(fs => fs.GetSpatialContextInfo(It.Is<string>(arg => arg == resId), It.IsAny<bool>())).Returns(scList);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var mockFs = new Mock<IFeatureSource>();
            mockFs.Setup(fs => fs.ConfigurationDocument).Returns(dataName);
            mockFs.Setup(fs => fs.ResourceID).Returns(resId);
            mockFs.Setup(fs => fs.Extension).Returns(new[] { mockExt.Object });
            mockFs.Setup(fs => fs.ResourceType).Returns(ResourceTypes.FeatureSource.ToString());
            mockFs.Setup(fs => fs.Provider).Returns("OSGeo.SDF");
            mockFs.Setup(fs => fs.Serialize()).Returns(string.Empty);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, mockFs.Object, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Warning_FeatureSource_NoSchemasFound, issues.First().StatusCode);
        }

        [Fact]
        public void TestCase_FsValidator_SpatialContexts()
        {
            var resId = "Library://Test.FeatureSource";
            var resId2 = "Library://Test2.FeatureSource";
            var resId3 = "Library://Test3.FeatureSource";
            var resId4 = "Library://Test4.FeatureSource";
            var resId5 = "Library://Test5.FeatureSource";
            var dataName = "config.xml";
            var schemaName = "Default";

            var sc = CreateTestSpatialContext("Default");
            var scBogus = CreateTestSpatialContext("Bogus");
            scBogus.Extent = null;
            var scOOB = CreateTestSpatialContext("OOB");
            scOOB.Extent = new FdoSpatialContextListSpatialContextExtent()
            {
                LowerLeftCoordinate = new FdoSpatialContextListSpatialContextExtentLowerLeftCoordinate()
                {
                    X = "-1000001",
                    Y = "-1000001"
                },
                UpperRightCoordinate = new FdoSpatialContextListSpatialContextExtentUpperRightCoordinate()
                {
                    X = "1000001",
                    Y = "1000001"
                }
            };
            var badScList = new FdoSpatialContextList();
            var scList = new FdoSpatialContextList
            {
                SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>() { sc }
            };
            var scListBogus = new FdoSpatialContextList
            {
                SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>() { scBogus }
            };
            var scListOOB = new FdoSpatialContextList
            {
                SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>() { scOOB }
            };

            var mockExt = new Mock<IFeatureSourceExtension>();
            var mockRelate = new Mock<IAttributeRelation>();
            mockRelate.Setup(r => r.Name).Returns("abcd");

            mockExt.Setup(ext => ext.AttributeRelate).Returns(new[] { mockRelate.Object });

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(rs => rs.GetResourceData(It.IsAny<string>(), It.IsAny<string>())).Returns((Stream)null);
            mockFeatSvc.Setup(fs => fs.TestConnection(It.IsAny<string>())).Returns("true");
            mockFeatSvc.Setup(fs => fs.GetSchemas(It.IsAny<string>())).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fs => fs.GetSpatialContextInfo(It.Is<string>(arg => arg == resId), It.IsAny<bool>())).Returns((FdoSpatialContextList)null);
            mockFeatSvc.Setup(fs => fs.GetSpatialContextInfo(It.Is<string>(arg => arg == resId2), It.IsAny<bool>())).Returns(badScList);
            mockFeatSvc.Setup(fs => fs.GetSpatialContextInfo(It.Is<string>(arg => arg == resId3), It.IsAny<bool>())).Returns(scListBogus);
            mockFeatSvc.Setup(fs => fs.GetSpatialContextInfo(It.Is<string>(arg => arg == resId4), It.IsAny<bool>())).Returns(scListOOB);
            mockFeatSvc.Setup(fs => fs.GetSpatialContextInfo(It.Is<string>(arg => arg == resId5), It.IsAny<bool>())).Returns(scList);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var mockFs = new Mock<IFeatureSource>();
            mockFs.Setup(fs => fs.ConfigurationDocument).Returns(dataName);
            mockFs.Setup(fs => fs.ResourceID).Returns(resId);
            mockFs.Setup(fs => fs.Extension).Returns(new[] { mockExt.Object });
            mockFs.Setup(fs => fs.ResourceType).Returns(ResourceTypes.FeatureSource.ToString());
            mockFs.Setup(fs => fs.Provider).Returns("OSGeo.SDF");
            mockFs.Setup(fs => fs.Serialize()).Returns(string.Empty);

            var mockFs2 = new Mock<IFeatureSource>();
            mockFs2.Setup(fs => fs.ConfigurationDocument).Returns(dataName);
            mockFs2.Setup(fs => fs.ResourceID).Returns(resId2);
            mockFs2.Setup(fs => fs.Extension).Returns(new[] { mockExt.Object });
            mockFs2.Setup(fs => fs.ResourceType).Returns(ResourceTypes.FeatureSource.ToString());
            mockFs2.Setup(fs => fs.Provider).Returns("OSGeo.SDF");
            mockFs2.Setup(fs => fs.Serialize()).Returns(string.Empty);

            var mockFs3 = new Mock<IFeatureSource>();
            mockFs3.Setup(fs => fs.ConfigurationDocument).Returns(dataName);
            mockFs3.Setup(fs => fs.ResourceID).Returns(resId3);
            mockFs3.Setup(fs => fs.Extension).Returns(new[] { mockExt.Object });
            mockFs3.Setup(fs => fs.ResourceType).Returns(ResourceTypes.FeatureSource.ToString());
            mockFs3.Setup(fs => fs.Provider).Returns("OSGeo.SDF");
            mockFs3.Setup(fs => fs.Serialize()).Returns(string.Empty);

            var mockFs4 = new Mock<IFeatureSource>();
            mockFs4.Setup(fs => fs.ConfigurationDocument).Returns(dataName);
            mockFs4.Setup(fs => fs.ResourceID).Returns(resId4);
            mockFs4.Setup(fs => fs.Extension).Returns(new[] { mockExt.Object });
            mockFs4.Setup(fs => fs.ResourceType).Returns(ResourceTypes.FeatureSource.ToString());
            mockFs4.Setup(fs => fs.Provider).Returns("OSGeo.SDF");
            mockFs4.Setup(fs => fs.Serialize()).Returns(string.Empty);

            var mockFs5 = new Mock<IFeatureSource>();
            mockFs5.Setup(fs => fs.ConfigurationDocument).Returns(dataName);
            mockFs5.Setup(fs => fs.ResourceID).Returns(resId5);
            mockFs5.Setup(fs => fs.Extension).Returns(new[] { mockExt.Object });
            mockFs5.Setup(fs => fs.ResourceType).Returns(ResourceTypes.FeatureSource.ToString());
            mockFs5.Setup(fs => fs.Provider).Returns("OSGeo.SDF");
            mockFs5.Setup(fs => fs.Serialize()).Returns(string.Empty);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, mockFs2.Object, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Warning_FeatureSource_NoSpatialContext, issues.First().StatusCode);

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mockFs3.Object, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Warning_FeatureSource_EmptySpatialContext, issues.First().StatusCode);

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mockFs4.Object, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Warning_FeatureSource_DefaultSpatialContext, issues.First().StatusCode);

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mockFs5.Object, false);
            Assert.Empty(issues);
        }

        [Fact]
        public void TestCase_FsValidator_EmptyJoinPrefix()
        {
            var resId = "Library://Test.FeatureSource";
            var dataName = "config.xml";
            var schemaName = "Default";

            var sc = CreateTestSpatialContext("Default");
            var scList = new FdoSpatialContextList
            {
                SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>() { sc }
            };

            var mockExt = new Mock<IFeatureSourceExtension>();
            var mockRelate = new Mock<IAttributeRelation>();
            mockRelate.Setup(r => r.Name).Returns(string.Empty);

            mockExt.Setup(ext => ext.AttributeRelate).Returns(new[] { mockRelate.Object });

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(rs => rs.GetResourceData(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == dataName))).Returns((Stream)null);
            mockFeatSvc.Setup(fs => fs.TestConnection(It.Is<string>(arg => arg == resId))).Returns("true");
            mockFeatSvc.Setup(fs => fs.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fs => fs.GetSpatialContextInfo(It.Is<string>(arg => arg == resId), It.IsAny<bool>())).Returns(scList);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var mockFs = new Mock<IFeatureSource>();
            mockFs.Setup(fs => fs.ConfigurationDocument).Returns(dataName);
            mockFs.Setup(fs => fs.ResourceID).Returns(resId);
            mockFs.Setup(fs => fs.Extension).Returns(new[] { mockExt.Object });
            mockFs.Setup(fs => fs.ResourceType).Returns(ResourceTypes.FeatureSource.ToString());
            mockFs.Setup(fs => fs.Provider).Returns("OSGeo.SDF");
            mockFs.Setup(fs => fs.Serialize()).Returns(string.Empty);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<FeatureSourceValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, mockFs.Object, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Warning_FeatureSource_EmptyJoinPrefix, issues.First().StatusCode);
        }

        [Fact]
        public void TestCase_LayerValidator_ClassNotFound()
        {
            var resId = "Library://Test.FeatureSource";
            var ldfId = "Library://Test.LayerDefinition";
            var schemaName = "Default";
            var className = "Test";
            var idName = "ID";
            var geomName = "Geometry";
            var klass = CreateTestClass("Default", className, idName, geomName);

            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");

            var ldf = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
            ldf.SubLayer.ResourceId = resId;
            ldf.ResourceID = ldfId;
            var vl = (IVectorLayerDefinition)ldf.SubLayer;
            vl.FeatureName = $"{schemaName}:{klass.Name}";
            vl.Geometry = geomName;

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == resId))).Returns(fs);

            mockFeatSvc.Setup(fsvc => fsvc.TestConnection(It.Is<string>(arg => arg == resId))).Returns("false");
            mockFeatSvc.Setup(fsvc => fsvc.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fsvc => fsvc.GetClassDefinition(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == $"{schemaName}:{className}"))).Returns((ClassDefinition)null);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<LayerDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, ldf, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_ClassNotFound, issues.First().StatusCode);
        }

        [Fact]
        public void TestCase_LayerValidator_GeometryNotFound()
        {
            var resId = "Library://Test.FeatureSource";
            var ldfId = "Library://Test.LayerDefinition";
            var schemaName = "Default";
            var className = "Test";
            var idName = "ID";
            var geomName = "Geometry";
            var klass = CreateTestClass("Default", className, idName, "Geom");

            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            fs.ResourceID = resId;

            var ldf = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
            ldf.SubLayer.ResourceId = resId;
            ldf.ResourceID = ldfId;
            var vl = (IVectorLayerDefinition)ldf.SubLayer;
            vl.FeatureName = $"{schemaName}:{klass.Name}";
            vl.Geometry = geomName;

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == resId))).Returns(fs);

            mockFeatSvc.Setup(fsvc => fsvc.TestConnection(It.Is<string>(arg => arg == resId))).Returns("false");
            mockFeatSvc.Setup(fsvc => fsvc.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fsvc => fsvc.GetClassDefinition(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == $"{schemaName}:{className}"))).Returns(klass);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<LayerDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, ldf, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_GeometryNotFound, issues.First().StatusCode);
        }

        [Fact]
        public void TestCase_LayerValidator_BadPropertyMapping()
        {
            var resId = "Library://Test.FeatureSource";
            var ldfId = "Library://Test.LayerDefinition";
            var schemaName = "Default";
            var className = "Test";
            var idName = "ID";
            var geomName = "Geometry";
            var klass = CreateTestClass("Default", className, idName, geomName);

            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            fs.ResourceID = resId;

            var ldf = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
            ldf.SubLayer.ResourceId = resId;
            ldf.ResourceID = ldfId;
            var vl = (IVectorLayerDefinition)ldf.SubLayer;
            vl.FeatureName = $"{schemaName}:{klass.Name}";
            vl.Geometry = geomName;

            vl.AddPropertyMapping(ldf.CreatePair("Foo", "Bar"));

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == resId))).Returns(fs);

            mockFeatSvc.Setup(fsvc => fsvc.TestConnection(It.Is<string>(arg => arg == resId))).Returns("false");
            mockFeatSvc.Setup(fsvc => fsvc.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fsvc => fsvc.GetClassDefinition(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == $"{schemaName}:{className}"))).Returns(klass);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<LayerDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, ldf, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_InvalidPropertyMapping, issues.First().StatusCode);
        }

        [Fact]
        public void TestCase_LayerValidator_MissingFeatureName()
        {
            var resId = "Library://Test.FeatureSource";
            var resId2 = "Library://TestRaster.FeatureSource";
            var ldfId = "Library://Test.LayerDefinition";
            var schemaName = "Default";
            var className = "Test";
            var idName = "ID";
            var geomName = "Geometry";
            var klass = CreateTestClass("Default", className, idName, geomName);

            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            fs.ResourceID = resId;

            var gfs = ObjectFactory.CreateFeatureSource("OSGeo.GDAL");
            gfs.ResourceID = resId2;

            var ldf = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
            ldf.SubLayer.ResourceId = resId;
            ldf.ResourceID = ldfId;
            var vl = (IVectorLayerDefinition)ldf.SubLayer;
            vl.Geometry = geomName;

            var gldf = ObjectFactory.CreateDefaultLayer(LayerType.Raster, new Version(1, 0, 0));
            gldf.SubLayer.ResourceId = resId2;
            gldf.ResourceID = ldfId;
            var gl = (IRasterLayerDefinition)gldf.SubLayer;
            gl.Geometry = geomName;

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == resId))).Returns(fs);
            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == resId2))).Returns(gfs);

            mockFeatSvc.Setup(fsvc => fsvc.TestConnection(It.Is<string>(arg => arg == resId))).Returns("true");
            mockFeatSvc.Setup(fsvc => fsvc.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fsvc => fsvc.GetClassDefinition(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == $"{schemaName}:{className}"))).Returns(klass);

            mockFeatSvc.Setup(fsvc => fsvc.TestConnection(It.Is<string>(arg => arg == resId))).Returns("true");
            mockFeatSvc.Setup(fsvc => fsvc.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fsvc => fsvc.GetClassDefinition(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == $"{schemaName}:{className}"))).Returns(klass);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<LayerDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, ldf, false);
            Assert.Equal(2, issues.Count());
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_ClassNotFound, issues.First().StatusCode);
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_MissingFeatureName, issues.Last().StatusCode);

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<LayerDefinitionValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, gldf, false);
            Assert.Equal(2, issues.Count());
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_ClassNotFound, issues.First().StatusCode);
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_MissingFeatureName, issues.Last().StatusCode);
        }

        [Fact]
        public void TestCase_LayerValidator_MissingGeometry()
        {
            var resId = "Library://Test.FeatureSource";
            var resId2 = "Library://TestRaster.FeatureSource";
            var ldfId = "Library://Test.LayerDefinition";
            var schemaName = "Default";
            var className = "Test";
            var idName = "ID";
            var geomName = "Geometry";
            var klass = CreateTestClass("Default", className, idName, geomName);

            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            fs.ResourceID = resId;

            var gfs = ObjectFactory.CreateFeatureSource("OSGeo.GDAL");
            gfs.ResourceID = resId;

            var ldf = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
            ldf.SubLayer.ResourceId = resId;
            ldf.ResourceID = ldfId;
            var vl = (IVectorLayerDefinition)ldf.SubLayer;
            vl.FeatureName = $"{schemaName}:{klass.Name}";

            var gldf = ObjectFactory.CreateDefaultLayer(LayerType.Raster, new Version(1, 0, 0));
            gldf.SubLayer.ResourceId = resId2;
            gldf.ResourceID = ldfId;
            var gl = (IRasterLayerDefinition)gldf.SubLayer;
            gl.FeatureName = $"{schemaName}:{klass.Name}";

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == resId))).Returns(fs);
            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == resId2))).Returns(gfs);

            mockFeatSvc.Setup(fsvc => fsvc.TestConnection(It.Is<string>(arg => arg == resId))).Returns("false");
            mockFeatSvc.Setup(fsvc => fsvc.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fsvc => fsvc.GetClassDefinition(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == $"{schemaName}:{className}"))).Returns(klass);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<LayerDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, ldf, false);
            Assert.Equal(2, issues.Count());
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_GeometryNotFound, issues.First().StatusCode);
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_MissingGeometry, issues.Last().StatusCode);

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<LayerDefinitionValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, gldf, false);
            Assert.Equal(2, issues.Count());
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_GeometryNotFound, issues.First().StatusCode);
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_MissingGeometry, issues.Last().StatusCode);
        }

        [Fact]
        public void TestCase_LayerValidator_ScaleRangeMinMaxSwapped()
        {
            var resId = "Library://Test.FeatureSource";
            var ldfId = "Library://Test.LayerDefinition";
            var schemaName = "Default";
            var className = "Test";
            var idName = "ID";
            var geomName = "Geometry";
            var klass = CreateTestClass("Default", className, idName, geomName);

            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            fs.ResourceID = resId;

            var ldf = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
            ldf.SubLayer.ResourceId = resId;
            ldf.ResourceID = ldfId;
            var vl = (IVectorLayerDefinition)ldf.SubLayer;
            vl.FeatureName = $"{schemaName}:{klass.Name}";
            vl.Geometry = geomName;

            vl.GetScaleRangeAt(0).MinScale = 1000;
            vl.GetScaleRangeAt(0).MaxScale = 999;

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == resId))).Returns(fs);

            mockFeatSvc.Setup(fsvc => fsvc.TestConnection(It.Is<string>(arg => arg == resId))).Returns("false");
            mockFeatSvc.Setup(fsvc => fsvc.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fsvc => fsvc.GetClassDefinition(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == $"{schemaName}:{className}"))).Returns(klass);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<LayerDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, ldf, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_MinMaxScaleSwapped, issues.First().StatusCode);
        }

        [Fact]
        public void TestCase_LayerValidator_OverlappingScaleRanges()
        {
            var resId = "Library://Test.FeatureSource";
            var ldfId = "Library://Test.LayerDefinition";
            var schemaName = "Default";
            var className = "Test";
            var idName = "ID";
            var geomName = "Geometry";
            var klass = CreateTestClass("Default", className, idName, geomName);

            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            fs.ResourceID = resId;

            var ldf = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
            ldf.SubLayer.ResourceId = resId;
            ldf.ResourceID = ldfId;
            var vl = (IVectorLayerDefinition)ldf.SubLayer;
            vl.FeatureName = $"{schemaName}:{klass.Name}";
            vl.Geometry = geomName;

            vl.GetScaleRangeAt(0).MinScale = 100;
            vl.GetScaleRangeAt(0).MaxScale = 200;

            var vsr = ldf.CreateVectorScaleRange();
            vsr.MinScale = 199;
            vsr.MaxScale = 300;
            vl.AddVectorScaleRange(vsr);

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == resId))).Returns(fs);

            mockFeatSvc.Setup(fsvc => fsvc.TestConnection(It.Is<string>(arg => arg == resId))).Returns("false");
            mockFeatSvc.Setup(fsvc => fsvc.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fsvc => fsvc.GetClassDefinition(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == $"{schemaName}:{className}"))).Returns(klass);

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<LayerDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, ldf, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Info_LayerDefinition_ScaleRangeOverlap, issues.First().StatusCode);
        }

        [Fact]
        public void TestCase_LayerValidator_MissingDrawingSheet()
        {
            var resId = "Library://Test.FeatureSource";
            var dsId = "Library://Test.DrawingSource";
            var ldfId = "Library://Test.LayerDefinition";
            var schemaName = "Default";
            var className = "Test";
            var idName = "ID";
            var geomName = "Geometry";
            var klass = CreateTestClass("Default", className, idName, geomName);

            var ds = ObjectFactory.CreateDrawingSource();
            ds.ResourceID = dsId;

            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            fs.ResourceID = resId;

            var ldf = ObjectFactory.CreateDefaultLayer(LayerType.Drawing, new Version(1, 0, 0));
            ldf.SubLayer.ResourceId = dsId;
            ldf.ResourceID = ldfId;
            var dl = (IDrawingLayerDefinition)ldf.SubLayer;
            dl.Sheet = "Foo";

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();
            var mockDrawSvc = new Mock<IDrawingService>();

            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == resId))).Returns(fs);
            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == dsId))).Returns(ds);

            mockFeatSvc.Setup(fsvc => fsvc.TestConnection(It.Is<string>(arg => arg == resId))).Returns("false");
            mockFeatSvc.Setup(fsvc => fsvc.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fsvc => fsvc.GetClassDefinition(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == $"{schemaName}:{className}"))).Returns(klass);

            var sheetList = new DrawingSectionList
            {
                Section = new System.ComponentModel.BindingList<DrawingSectionListSection>()
            };
            mockDrawSvc.Setup(dsvc => dsvc.EnumerateDrawingSections(It.IsAny<string>())).Returns(sheetList);

            var mockCaps = new Mock<IConnectionCapabilities>();
            mockCaps.Setup(c => c.SupportedServices).Returns(new[] { (int)ServiceType.Drawing });

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);
            mockConn.Setup(c => c.Capabilities).Returns(mockCaps.Object);
            mockConn.Setup(c => c.GetService(It.Is<int>(arg => arg == (int)ServiceType.Drawing))).Returns(mockDrawSvc.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<LayerDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, ldf, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_DrawingSourceSheetNotFound, issues.First().StatusCode);
        }

        [Fact]
        public void TestCase_LayerValidator_MissingDrawingSheetLayer()
        {
            var resId = "Library://Test.FeatureSource";
            var dsId = "Library://Test.DrawingSource";
            var ldfId = "Library://Test.LayerDefinition";
            var schemaName = "Default";
            var className = "Test";
            var idName = "ID";
            var geomName = "Geometry";
            var klass = CreateTestClass("Default", className, idName, geomName);

            var ds = ObjectFactory.CreateDrawingSource();
            ds.ResourceID = dsId;

            var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
            fs.ResourceID = resId;

            var ldf = ObjectFactory.CreateDefaultLayer(LayerType.Drawing, new Version(1, 0, 0));
            ldf.SubLayer.ResourceId = dsId;
            ldf.ResourceID = ldfId;
            var dl = (IDrawingLayerDefinition)ldf.SubLayer;
            dl.Sheet = "Foo";
            dl.LayerFilter = "Baz";

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();
            var mockDrawSvc = new Mock<IDrawingService>();

            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == resId))).Returns(fs);
            mockResSvc.Setup(rsvc => rsvc.GetResource(It.Is<string>(arg => arg == dsId))).Returns(ds);

            mockFeatSvc.Setup(fsvc => fsvc.TestConnection(It.Is<string>(arg => arg == resId))).Returns("false");
            mockFeatSvc.Setup(fsvc => fsvc.GetSchemas(It.Is<string>(arg => arg == resId))).Returns(new[] { schemaName });
            mockFeatSvc.Setup(fsvc => fsvc.GetClassDefinition(It.Is<string>(arg => arg == resId), It.Is<string>(arg => arg == $"{schemaName}:{className}"))).Returns(klass);

            var sheetList = new DrawingSectionList
            {
                Section = new System.ComponentModel.BindingList<DrawingSectionListSection>()
                {
                    new DrawingSectionListSection { Name = "Foo" }
                }
            };
            mockDrawSvc.Setup(dsvc => dsvc.EnumerateDrawingSections(It.Is<string>(arg => arg == dsId))).Returns(sheetList);
            mockDrawSvc.Setup(dsvc => dsvc.EnumerateDrawingLayers(It.Is<string>(arg => arg == dsId), It.Is<string>(arg => arg == "Foo"))).Returns(new[] { "Bar" });

            var mockCaps = new Mock<IConnectionCapabilities>();
            mockCaps.Setup(c => c.SupportedServices).Returns(new[] { (int)ServiceType.Drawing });

            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);
            mockConn.Setup(c => c.Capabilities).Returns(mockCaps.Object);
            mockConn.Setup(c => c.GetService(It.Is<int>(arg => arg == (int)ServiceType.Drawing))).Returns(mockDrawSvc.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<LayerDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, ldf, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_LayerDefinition_DrawingSourceSheetLayerNotFound, issues.First().StatusCode);
        }

        [Fact]
        public void TestCase_MapDefinitionValidator_EmptyCS()
        {
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Sheboygan");
            mdf.ResourceID = "Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition";
            var mockConn = new Mock<IServerConnection>();

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<MapDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, mdf, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Warning_MapDefinition_MissingCoordinateSystem, issues.First().StatusCode);
        }

        const string LL84_WKT = "GEOGCS[\"LL84\",DATUM[\"WGS84\",SPHEROID[\"WGS84\",6378137.000,298.25722293]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.01745329251994]]";

        [Fact]
        public void TestCase_MapDefinitionValidator_GroupSettings()
        {
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Sheboygan");
            mdf.CoordinateSystem = LL84_WKT;
            mdf.ResourceID = "Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition";

            var grp = mdf.AddGroup("Test");
            grp.LegendLabel = "";
            grp.ShowInLegend = true;

            var mockConn = new Mock<IServerConnection>();

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<MapDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, mdf, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Info_MapDefinition_GroupMissingLabelInformation, issues.First().StatusCode);

            grp.LegendLabel = "layer group";

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<MapDefinitionValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mdf, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Info_MapDefinition_GroupHasDefaultLabel, issues.First().StatusCode);

            grp.LegendLabel = "Test Layer Group";

            var grp2 = mdf.AddGroup("Test2");
            grp2.LegendLabel = "Test Layer Group 2";
            grp2.Group = "IDontExist";

            context = new ResourceValidationContext(mockConn.Object);
            validator = CreateInstance<MapDefinitionValidator>();
            validator.Connection = mockConn.Object;
            issues = validator.Validate(context, mdf, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_MapDefinition_GroupWithNonExistentGroup, issues.First().StatusCode);
        }

        [Fact]
        public void TestCase_MapDefinitionValidator_NoFiniteScales()
        {
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Sheboygan");
            mdf.CoordinateSystem = LL84_WKT;
            mdf.ResourceID = "Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition";

            mdf.InitBaseMap();
            mdf.BaseMap.AddBaseLayerGroup("Test");

            var mockConn = new Mock<IServerConnection>();

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<MapDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, mdf, false);
            Assert.Single(issues);
            Assert.Equal(ValidationStatusCode.Error_MapDefinition_NoFiniteDisplayScales, issues.First().StatusCode);
        }

        [Fact]
        public void TestCase_MapDefinitionValidator_BaseLayers()
        {
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Sheboygan");
            mdf.CoordinateSystem = LL84_WKT;
            mdf.ResourceID = "Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition";

            mdf.InitBaseMap();
            mdf.BaseMap.AddFiniteDisplayScale(10);
            var grp = mdf.BaseMap.AddBaseLayerGroup("Test");
            grp.AddLayer("Layer1", "Library://Samples/Sheboygan/Layers/Parcels.LayerDefinition").Selectable = true;
            grp.AddLayer("Layer1", "Library://Samples/Sheboygan/Layers/Roads.LayerDefinition").Selectable = false;

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();
            mockFeatSvc.Setup(f => f.GetIdentityProperties(It.IsAny<string>(), It.IsAny<string>())).Returns(new string[0]);
            mockFeatSvc.Setup(f => f.GetSpatialContextInfo(It.IsAny<string>(), It.IsAny<bool>())).Returns(() => new FdoSpatialContextList { SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>() });
            mockResSvc.Setup(r => r.GetResource(It.IsAny<string>())).Returns<string>((resId) =>
            {
                IResource res = null;
                if (resId.EndsWith(ResourceTypes.FeatureSource.ToString()))
                {
                    var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
                    fs.ResourceID = resId;
                    res = fs;
                }
                else if (resId.EndsWith(ResourceTypes.LayerDefinition.ToString()))
                {
                    var layer = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
                    layer.ResourceID = resId;
                    layer.SubLayer.ResourceId = resId.Replace(ResourceTypes.LayerDefinition.ToString(), ResourceTypes.FeatureSource.ToString());
                    var vl = (IVectorLayerDefinition)layer.SubLayer;
                    vl.FeatureName = "Test:Class";
                    res = layer;
                }
                return res;
            });
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);
            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<MapDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, mdf, false);
            Assert.Equal(4, issues.Count());
            Assert.Equal(1, issues.Count(i => i.StatusCode == ValidationStatusCode.Error_MapDefinition_DuplicateLayerName));
            Assert.Equal(2, issues.Count(i => i.StatusCode == ValidationStatusCode.Warning_MapDefinition_MissingSpatialContext));
            Assert.Equal(1, issues.Count(i => i.StatusCode == ValidationStatusCode.Warning_MapDefinition_UnselectableLayer));
        }

        [Fact]
        public void TestCase_MapDefinitionValidator_BaseLayersSC()
        {
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Sheboygan");
            mdf.CoordinateSystem = LL84_WKT;
            mdf.ResourceID = "Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition";

            mdf.InitBaseMap();
            mdf.BaseMap.AddFiniteDisplayScale(10);
            var grp = mdf.BaseMap.AddBaseLayerGroup("Test");
            grp.AddLayer("Layer1", "Library://Samples/Sheboygan/Layers/Roads.LayerDefinition").Selectable = false;
            grp.AddLayer("Layer2", "Library://Samples/Sheboygan/Layers/Parcels.LayerDefinition").Selectable = false;
            grp.AddLayer("Layer3", "Library://Samples/Sheboygan/Layers/Districts.LayerDefinition").Selectable = false;

            var mockConn = new Mock<IServerConnection>();
            var mockFeatSvc = new Mock<IFeatureService>();
            var mockResSvc = new Mock<IResourceService>();
            mockFeatSvc.Setup(f => f.GetSpatialExtent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns<string, string, string>((a, b, c) =>
            {
                if (a == "Library://Samples/Sheboygan/Layers/Parcels.FeatureSource" || a == "Library://Samples/Sheboygan/Layers/Districts.FeaturSource")
                {
                    return ObjectFactory.CreateEnvelope(90, 0, 270, 180); //Trigger data OOB warning
                }
                return ObjectFactory.CreateEnvelope(-10, -10, 10, 10);
            });
            mockFeatSvc.Setup(f => f.GetSpatialContextInfo(It.IsAny<string>(), It.IsAny<bool>())).Returns<string, bool>((Func<string, bool, FdoSpatialContextList>)((resId, active) => 
            {
                if (resId == "Library://Samples/Sheboygan/Layers/Roads.FeatureSource")
                {
                    return new FdoSpatialContextList
                    {
                        SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>
                        {
                            CreateTestSpatialContext("Default")
                        }
                    };
                }
                else if (resId == "Library://Samples/Sheboygan/Layers/Parcels.FeatureSource")
                {
                    return new FdoSpatialContextList //Trigger multiple SC warning
                    {
                        SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>
                        {
                            CreateTestSpatialContext("SC1", LL84_WKT),
                            CreateTestSpatialContext("SC2", LL84_WKT)
                        }
                    };
                }
                else if (resId == "Library://Samples/Sheboygan/Layers/Districts.FeatureSource")
                {
                    return new FdoSpatialContextList
                    {
                        SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>
                        {
                            CreateTestSpatialContext("Default") //Trigger raster reprojection warning
                        }
                    };
                }
                return new FdoSpatialContextList
                {
                    SpatialContext = new System.ComponentModel.BindingList<FdoSpatialContextListSpatialContext>()
                };
            }));
            mockResSvc.Setup(r => r.GetResource(It.IsAny<string>())).Returns<string>((resId) =>
            {
                IResource res = null;
                if (resId.EndsWith(ResourceTypes.FeatureSource.ToString()))
                {
                    var fs = ObjectFactory.CreateFeatureSource("OSGeo.SDF");
                    fs.ResourceID = resId;
                    res = fs;
                }
                else if (resId.EndsWith(ResourceTypes.LayerDefinition.ToString()))
                {
                    if (resId == "Library://Samples/Sheboygan/Layers/Roads.LayerDefinition")
                    {
                        var layer = ObjectFactory.CreateDefaultLayer(LayerType.Raster, new Version(1, 0, 0));
                        layer.ResourceID = resId;
                        layer.SubLayer.ResourceId = resId.Replace(ResourceTypes.LayerDefinition.ToString(), ResourceTypes.FeatureSource.ToString());
                        var gl = (IRasterLayerDefinition)layer.SubLayer;
                        gl.FeatureName = "Test:Class";
                        res = layer;
                    }
                    else
                    {
                        var layer = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));
                        layer.ResourceID = resId;
                        layer.SubLayer.ResourceId = resId.Replace(ResourceTypes.LayerDefinition.ToString(), ResourceTypes.FeatureSource.ToString());
                        var vl = (IVectorLayerDefinition)layer.SubLayer;
                        vl.FeatureName = "Test:Class";
                        vl.Geometry = "Geometry";
                        res = layer;
                    }
                }
                return res;
            });
            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);
            mockConn.Setup(c => c.FeatureService).Returns(mockFeatSvc.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = CreateInstance<MapDefinitionValidator>();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, mdf, false);
            Assert.Equal(3, issues.Count());
            Assert.Equal(1, issues.Count(i => i.StatusCode == ValidationStatusCode.Info_MapDefinition_MultipleSpatialContexts));
            Assert.Equal(1, issues.Count(i => i.StatusCode == ValidationStatusCode.Warning_MapDefinition_LayerReprojection));
            Assert.Equal(1, issues.Count(i => i.StatusCode == ValidationStatusCode.Warning_MapDefinition_DataOutsideMapBounds));
        }
        
        [Fact]
        public void TestCase_AppDefValidator_MissingMap()
        {
            var appDef = CreateTestFlexLayout();

            var mockConn = new Mock<IServerConnection>();

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = new ApplicationDefinitionValidator();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, appDef, false);
            Assert.Single(issues);
            Assert.Equal(1, issues.Count(i => i.StatusCode == ValidationStatusCode.Error_Fusion_MissingMap));
        }

        [Fact]
        public void TestCase_AppDefValidator_NonWebMercatorMdfWithExternalBaseLayer()
        {
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Test");
            mdf.CoordinateSystem = LL84_WKT;
            mdf.ResourceID = "Library://Test.MapDefinition";
            mdf.Extents = ObjectFactory.CreateEnvelope(-10, -10, 10, 10);

            var appDef = CreateTestFlexLayout();
            var mgroup = appDef.AddMapGroup("mapguide", true, "Library://Test.MapDefinition");
            var goog = mgroup.CreateCmsMapEntry("Google", false, "Google", "Google");
            mgroup.AddMap(goog);

            var mockConn = new Mock<IServerConnection>();

            var mockCatalog = new Mock<ICoordinateSystemCatalog>();
            mockCatalog.Setup(cat => cat.ConvertWktToCoordinateSystemCode(It.IsAny<string>())).Returns("LL84");

            var mockResSvc = new Mock<IResourceService>();
            mockResSvc.Setup(r => r.GetResource(It.Is<string>(arg => arg.EndsWith(".MapDefinition")))).Returns(mdf);
            mockResSvc.Setup(r => r.ResourceExists(It.IsAny<string>())).Returns(true);

            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);
            mockConn.Setup(c => c.CoordinateSystemCatalog).Returns(mockCatalog.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = new ApplicationDefinitionValidator();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, appDef, false);
            Assert.Single(issues);
            Assert.Equal(1, issues.Count(i => i.StatusCode == ValidationStatusCode.Warning_Fusion_MapCoordSysIncompatibleWithCommericalLayers));
        }

        [Fact]
        public void TestCase_AppDefValidator_MGMapWithNoMdfId()
        {
            var appDef = CreateTestFlexLayout();
            appDef.AddMapGroup("mapguide", true, "");

            var mockConn = new Mock<IServerConnection>();

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = new ApplicationDefinitionValidator();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, appDef, false);
            Assert.Single(issues);
            Assert.Equal(1, issues.Count(i => i.StatusCode == ValidationStatusCode.Error_Fusion_InvalidMap));
        }

        [Fact]
        public void TestCase_AppDefValidator_InitialViewOutsideMdfExtents()
        {
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Test");
            mdf.ResourceID = "Library://Test.MapDefinition";
            mdf.Extents = ObjectFactory.CreateEnvelope(-10, -10, 10, 10);

            var appDef = CreateTestFlexLayout();
            var mgroup = appDef.AddMapGroup("mapguide", true, "Library://Test.MapDefinition");
            var view = mgroup.CreateInitialView(40, 40, 4000);
            mgroup.InitialView = view;

            var mockConn = new Mock<IServerConnection>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(r => r.GetResource(It.Is<string>(arg => arg.EndsWith(".MapDefinition")))).Returns(mdf);
            mockResSvc.Setup(r => r.ResourceExists(It.IsAny<string>())).Returns(true);

            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = new ApplicationDefinitionValidator();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, appDef, false);
            Assert.Single(issues);
            Assert.Equal(1, issues.Count(i => i.StatusCode == ValidationStatusCode.Warning_Fusion_InitialViewOutsideMapExtents));
        }

        [Fact]
        public void TestCase_AppDefValidator_BingMapsChanges1July2017()
        {
            var mdf = ObjectFactory.CreateMapDefinition(new Version(1, 0, 0), "Test");
            mdf.ResourceID = "Library://Test.MapDefinition";
            mdf.Extents = ObjectFactory.CreateEnvelope(-10, -10, 10, 10);

            var appDef = CreateTestFlexLayout();
            var mgroup = appDef.AddMapGroup("mapguide", true, "Library://Test.MapDefinition");
            var view = mgroup.CreateInitialView(0, 0, 4000);

            var bingHybrid = mgroup.CreateCmsMapEntry("VirtualEarth", false, "Bing Hybrid", "Hybrid");
            mgroup.AddMap(bingHybrid);

            mgroup.InitialView = view;

            var mockConn = new Mock<IServerConnection>();
            var mockResSvc = new Mock<IResourceService>();

            mockResSvc.Setup(r => r.GetResource(It.Is<string>(arg => arg.EndsWith(".MapDefinition")))).Returns(mdf);
            mockResSvc.Setup(r => r.ResourceExists(It.IsAny<string>())).Returns(true);

            var mockCatalog = new Mock<ICoordinateSystemCatalog>();
            mockCatalog.Setup(cat => cat.ConvertWktToCoordinateSystemCode(It.IsAny<string>())).Returns("WGS84.PseudoMercator");

            mockConn.Setup(c => c.ResourceService).Returns(mockResSvc.Object);
            mockConn.Setup(c => c.CoordinateSystemCatalog).Returns(mockCatalog.Object);

            var context = new ResourceValidationContext(mockConn.Object);
            var validator = new ApplicationDefinitionValidator();
            validator.Connection = mockConn.Object;
            var issues = validator.Validate(context, appDef, false);
            Assert.Equal(2, issues.Count());
            Assert.Equal(1, issues.Count(i => i.StatusCode == ValidationStatusCode.Error_Fusion_BingMapsHybridBaseLayerNoLongerAvailable));
            Assert.Equal(1, issues.Count(i => i.StatusCode == ValidationStatusCode.Error_Fusion_BingMapsMissingApiKey));
        }

        private static IApplicationDefinition CreateTestFlexLayout()
        {
            var appDef = ObjectFactory.DeserializeEmbeddedFlexLayout(SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS2_5));
            appDef.ResourceID = "Library://Test.ApplicationDefinition";
            var removeMaps = new List<IMapGroup>(appDef.MapSet.MapGroups);
            foreach (var rem in removeMaps)
            {
                appDef.MapSet.RemoveGroup(rem);
            }
            var removeW = appDef.WidgetSets.ToArray();
            foreach (var rem in removeW)
            {
                appDef.RemoveWidgetSet(rem);
            }

            return appDef;
        }
    }
}
