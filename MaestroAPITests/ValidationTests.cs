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
using OSGeo.MapGuide.MaestroAPI.Resource;
using NMock2;

using LoadProc = OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System.Xml;

namespace MaestroAPITests
{
    [TestFixture]
    public class ValidationTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            if (TestControl.IgnoreValidationTests)
                Assert.Ignore("Skipping ValidationTests because TestControl.IgnoreValidationTests = true");
        }

        [Test]
        public void TestUniqueIssues()
        {
            //Test for the purposes of equality and hashing that any two issues with
            //the same set of resource id, status and message are identical.

            var mocks = new Mockery();
            var res1 = mocks.NewMock<IResource>();
            Expect.AtLeastOnce.On(res1).GetProperty("ResourceID").Will(Return.Value("Library://Test.FeatureSource"));
            var res2 = mocks.NewMock<IResource>();
            Expect.AtLeastOnce.On(res2).GetProperty("ResourceID").Will(Return.Value("Library://Test.FeatureSource"));
            var res3 = mocks.NewMock<IResource>();
            Expect.AtLeastOnce.On(res3).GetProperty("ResourceID").Will(Return.Value("Library://Test.FeatureSource"));

            var msg1 = new ValidationIssue(res1, ValidationStatus.Error, ValidationStatusCode.Dummy, "Epic Fail");
            var msg2 = new ValidationIssue(res2, ValidationStatus.Error, ValidationStatusCode.Dummy, "Epic Fail");
            var msg3 = new ValidationIssue(res3, ValidationStatus.Error, ValidationStatusCode.Dummy, "Not so epic fail");

            Assert.AreEqual(msg1.GetHashCode(), msg2.GetHashCode());
            Assert.IsTrue(msg1.Equals(msg2));

            Assert.AreNotEqual(msg1.GetHashCode(), msg3.GetHashCode());
            Assert.AreNotEqual(msg2.GetHashCode(), msg3.GetHashCode());
            Assert.IsFalse(msg1.Equals(msg3));
            Assert.IsFalse(msg2.Equals(msg3));
        }

        [Test]
        public void TestValidationResultSet()
        {
            var mocks = new Mockery();
            var res1 = mocks.NewMock<IResource>();
            Expect.AtLeastOnce.On(res1).GetProperty("ResourceID").Will(Return.Value("Library://Test.FeatureSource"));
            var res2 = mocks.NewMock<IResource>();
            Expect.AtLeastOnce.On(res2).GetProperty("ResourceID").Will(Return.Value("Library://Test.FeatureSource"));
            var res3 = mocks.NewMock<IResource>();
            Expect.AtLeastOnce.On(res3).GetProperty("ResourceID").Will(Return.Value("Library://Test.FeatureSource"));

            var msg1 = new ValidationIssue(res1, ValidationStatus.Error, ValidationStatusCode.Dummy, "Epic Fail");
            var msg2 = new ValidationIssue(res2, ValidationStatus.Error, ValidationStatusCode.Dummy, "Epic Fail");
            var msg3 = new ValidationIssue(res3, ValidationStatus.Error, ValidationStatusCode.Dummy, "Not so epic fail");

            var set = new ValidationResultSet();
            set.AddIssues(new ValidationIssue[] { msg1, msg2, msg3 });

            var totalIssues = set.GetAllIssues();
            Assert.AreEqual(totalIssues.Length, 2);
            Assert.AreEqual(set.ResourceIDs.Length, 1);
            Assert.AreEqual(set.GetIssuesForResource("Library://Test.FeatureSource", ValidationStatus.Error).Count, 2);
        }

        #region Mocks
        class MockResourceService : IResourceService
        {
            public event ResourceEventHandler ResourceAdded;

            public event ResourceEventHandler ResourceDeleted;

            public event ResourceEventHandler ResourceUpdated;

            public OSGeo.MapGuide.ObjectModels.Common.ResourceList GetRepositoryResources()
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.ResourceList GetRepositoryResources(int depth)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.ResourceList GetRepositoryResources(string startingpoint, int depth)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.ResourceList GetRepositoryResources(string startingpoint)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.ResourceList GetRepositoryResources(string startingpoint, string type)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.ResourceList GetRepositoryResources(string startingpoint, string type, int depth)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.ResourceList GetRepositoryResources(string startingpoint, string type, int depth, bool computeChildren)
            {
                throw new NotImplementedException();
            }

            public T DeserializeObject<T>(System.IO.Stream data)
            {
                throw new NotImplementedException();
            }

            public void SerializeObject(object o, System.IO.Stream stream)
            {
                throw new NotImplementedException();
            }

            public System.IO.Stream GetResourceData(string resourceID, string dataname)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.ResourceDocumentHeaderType GetResourceHeader(string resourceID)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.ResourceFolderHeaderType GetFolderHeader(string resourceID)
            {
                throw new NotImplementedException();
            }

            public System.IO.Stream GetResourceXmlData(string resourceID)
            {
                throw new NotImplementedException();
            }

            public IResource GetResource(string resourceID)
            {
                throw new NotImplementedException();
            }

            public void Touch(string resourceID)
            {
                throw new NotImplementedException();
            }

            public void SetResourceData(string resourceid, string dataname, OSGeo.MapGuide.ObjectModels.Common.ResourceDataType datatype, System.IO.Stream stream)
            {
                throw new NotImplementedException();
            }

            public void SetResourceData(string resourceid, string dataname, OSGeo.MapGuide.ObjectModels.Common.ResourceDataType datatype, System.IO.Stream stream, Utility.StreamCopyProgressDelegate callback)
            {
                throw new NotImplementedException();
            }

            public void SetResourceXmlData(string resourceid, System.IO.Stream stream)
            {
                throw new NotImplementedException();
            }

            public void SetFolderHeader(string resourceID, OSGeo.MapGuide.ObjectModels.Common.ResourceFolderHeaderType header)
            {
                throw new NotImplementedException();
            }

            public void SetResourceHeader(string resourceID, OSGeo.MapGuide.ObjectModels.Common.ResourceDocumentHeaderType header)
            {
                throw new NotImplementedException();
            }

            public void UpdateRepository(string resourceId, OSGeo.MapGuide.ObjectModels.Common.ResourceFolderHeaderType header)
            {
                throw new NotImplementedException();
            }

            public void DeleteResourceData(string resourceID, string dataname)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.ResourceDataList EnumerateResourceData(string resourceID)
            {
                throw new NotImplementedException();
            }

            public void DeleteResource(string resourceID)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.ResourceReferenceList EnumerateResourceReferences(string resourceid)
            {
                throw new NotImplementedException();
            }

            public void CopyResource(string oldpath, string newpath, bool overwrite)
            {
                throw new NotImplementedException();
            }

            public void MoveResource(string oldpath, string newpath, bool overwrite)
            {
                throw new NotImplementedException();
            }

            public bool MoveResourceWithReferences(string oldpath, string newpath, LengthyOperationCallBack callback, LengthyOperationProgressCallBack progress)
            {
                throw new NotImplementedException();
            }

            public bool CopyFolderWithReferences(string oldpath, string newpath, LengthyOperationCallBack callback, LengthyOperationProgressCallBack progress)
            {
                throw new NotImplementedException();
            }

            public bool ResourceExists(string resourceid)
            {
                throw new NotImplementedException();
            }

            public void SaveResource(IResource resource)
            {
                throw new NotImplementedException();
            }

            public void SaveResourceAs(IResource resource, string resourceid)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.UnmanagedDataList EnumerateUnmanagedData(string startpath, string filter, bool recursive, UnmanagedDataTypes type)
            {
                throw new NotImplementedException();
            }

            public void UploadPackage(string filename, Utility.StreamCopyProgressDelegate callback)
            {
                throw new NotImplementedException();
            }
        }

        class MockFeatureService : IFeatureService
        {
            public OSGeo.MapGuide.ObjectModels.Capabilities.FdoProviderCapabilities GetProviderCapabilities(string provider)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.FeatureProviderRegistryFeatureProvider[] FeatureProviders
            {
                get { throw new NotImplementedException(); }
            }

            public string TestConnection(string providername, System.Collections.Specialized.NameValueCollection parameters)
            {
                throw new NotImplementedException();
            }

            public string TestConnection(string featureSourceId)
            {
                throw new NotImplementedException();
            }

            public string RemoveVersionFromProviderName(string providername)
            {
                throw new NotImplementedException();
            }

            public string[] GetConnectionPropertyValues(string providerName, string propertyName, string partialConnectionString)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.FeatureProviderRegistryFeatureProvider GetFeatureProvider(string providername)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.MaestroAPI.Feature.IReader ExecuteSqlQuery(string featureSourceID, string sql)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader QueryFeatureSource(string resourceID, string schema, string query)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader QueryFeatureSource(string resourceID, string schema)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader QueryFeatureSource(string resourceID, string schema, string query, string[] columns)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.MaestroAPI.Feature.IFeatureReader QueryFeatureSource(string resourceID, string schema, string query, string[] columns, System.Collections.Specialized.NameValueCollection computedProperties)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.MaestroAPI.Feature.IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.MaestroAPI.Feature.IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter, string[] columns)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.MaestroAPI.Feature.IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter, System.Collections.Specialized.NameValueCollection aggregateFunctions)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.IEnvelope GetSpatialExtent(string resourceID, string schema, string geometry)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.IEnvelope GetSpatialExtent(string resourceID, string schema, string geometry, string filter)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.IEnvelope GetSpatialExtent(string resourceID, string schema, string geometry, bool allowFallbackToContextInformation)
            {
                throw new NotImplementedException();
            }

            public FeatureSourceDescription DescribeFeatureSource(string resourceID)
            {
                throw new NotImplementedException();
            }

            public FeatureSchema DescribeFeatureSource(string resourceID, string schema)
            {
                throw new NotImplementedException();
            }

            public ClassDefinition GetClassDefinition(string resourceID, string schema)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.FdoSpatialContextList GetSpatialContextInfo(string resourceID, bool activeOnly)
            {
                throw new NotImplementedException();
            }

            public string[] GetIdentityProperties(string resourceID, string classname)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.DataStoreList EnumerateDataStores(string providerName, string partialConnString)
            {
                throw new NotImplementedException();
            }

            public string[] GetSchemas(string resourceId)
            {
                throw new NotImplementedException();
            }

            public string[] GetClassNames(string resourceId, string schemaName)
            {
                throw new NotImplementedException();
            }

            public FeatureSchema DescribeFeatureSourcePartial(string resourceID, string schema, string[] classNames)
            {
                throw new NotImplementedException();
            }


            public OSGeo.MapGuide.ObjectModels.Common.ILongTransactionList GetLongTransactions(string resourceId, bool activeOnly)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.MaestroAPI.SchemaOverrides.ConfigurationDocument GetSchemaMapping(string provider, string partialConnString)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        [Test]
        public void TestLoadProcedureValidation()
        {
            //TODO: Update these test to use pre-defined XML fragments
            //since ObjectFactory should correctly forbid creation of
            //unsupported types

            var id = "Library://Test.LoadProcedure";
            var mock = new Mockery();
            var conn = mock.NewMock<IServerConnection>();
            var featSvc = new MockFeatureService();
            var resSvc = new MockResourceService();
            var lp = ObjectFactory.CreateLoadProcedure(conn, LoadType.Sdf, new string[] 
            {
                "C:\\foo.sdf",
                "C:\\bar.sdf"
            });
            lp.ResourceID = id;

            var context = new ResourceValidationContext(resSvc, featSvc);
            var set = new ValidationResultSet();
            context.Reset();
            set.AddIssues(ResourceValidatorSet.Validate(context, lp, false));

            //SDF2, generalization and 2 missing files
            Assert.AreEqual(4, set.GetAllIssues().Length);

            lp = ObjectFactory.CreateLoadProcedure(conn, LoadType.Shp, new string[] 
            {
                "C:\\foo.shp",
                "C:\\bar.shp"
            });
            lp.ResourceID = id;

            set = new ValidationResultSet();
            context.Reset();
            set.AddIssues(ResourceValidatorSet.Validate(context, lp, false));

            //SDF3 conversion, generalization and 2 missing files
            Assert.AreEqual(4, set.GetAllIssues().Length);

            lp = new OSGeo.MapGuide.ObjectModels.LoadProcedure_1_0_0.LoadProcedure()
            {
                Item = new OSGeo.MapGuide.ObjectModels.LoadProcedure_1_0_0.DwgLoadProcedureType()
            };
            lp.ResourceID = id;
            set = new ValidationResultSet();
            context.Reset();
            set.AddIssues(ResourceValidatorSet.Validate(context, lp, false));

            //Not supported
            Assert.AreEqual(1, set.GetAllIssues().Length);

            lp = new OSGeo.MapGuide.ObjectModels.LoadProcedure_1_0_0.LoadProcedure()
            {
                Item = new OSGeo.MapGuide.ObjectModels.LoadProcedure_1_0_0.RasterLoadProcedureType()
            };
            lp.ResourceID = id;
            set = new ValidationResultSet();
            context.Reset();
            set.AddIssues(ResourceValidatorSet.Validate(context, lp, false));

            //Not supported
            Assert.AreEqual(1, set.GetAllIssues().Length);
        }

        [Test]
        public void TestLayerScaleRangeOverlap()
        {
            var mock = new Mockery();
            var conn = mock.NewMock<IServerConnection>();
            var ldf1 = ObjectFactory.CreateDefaultLayer(conn, OSGeo.MapGuide.ObjectModels.LayerDefinition.LayerType.Vector, new Version(1, 0, 0));
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

            var featSvc = new MockFeatureService();
            var resSvc = new MockResourceService();

            var validator = new LayerDefinitionValidator();

            var context = new ResourceValidationContext(resSvc, featSvc);
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
            var ldf2 = ObjectFactory.CreateDefaultLayer(conn, OSGeo.MapGuide.ObjectModels.LayerDefinition.LayerType.Vector, new Version(1, 0, 0));
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

            context = new ResourceValidationContext(resSvc, featSvc);
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

        [Test]
        public void TestCase1896()
        {
            var fs = new FeatureSchema();
            var doc = new XmlDocument();
            
            doc.Load("UserTestData\\1896.xml");

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
    }
}
