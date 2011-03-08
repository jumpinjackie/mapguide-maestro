using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ICSharpCode.Core;
using System.Reflection;
using System.IO;
using NMock2;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Http;
using Maestro.Base.Editor;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.ObjectModels.PrintLayout;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;

namespace MaestroBaseTests
{
    [TestFixture]
    public class EditorTests
    {
        [TestFixtureSetUp]
        public void Init()
        {

        }

        private Mockery _mocks;

        #region Hard-coded mocks because I can't figure out how to handle generics in NMock
        class MockResourceServiceForXmlEditor : IResourceService
        {
            public OSGeo.MapGuide.ObjectModels.Common.ResourceList RepositoryResources
            {
                get { throw new NotImplementedException(); }
            }

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

            public bool HasFolder(string folderpath)
            {
                throw new NotImplementedException();
            }

            public void CreateFolder(string folderpath)
            {
                throw new NotImplementedException();
            }

            public object DeserializeObject(Type type, Stream data)
            {
                throw new NotImplementedException();
            }

            public T DeserializeObject<T>(Stream data)
            {
                throw new NotImplementedException();
            }

            public MemoryStream SerializeObject(object o)
            {
                return new MemoryStream();
            }

            public void SerializeObject(object o, Stream stream)
            {
                throw new NotImplementedException();
            }

            public Type GetResourceType(string resourceID)
            {
                throw new NotImplementedException();
            }

            public Type TryGetResourceType(string resourceID)
            {
                throw new NotImplementedException();
            }

            public MemoryStream GetResourceData(string resourceID, string dataname)
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

            public byte[] GetResourceXmlData(string resourceID)
            {
                throw new NotImplementedException();
            }

            public IResource GetResource(string resourceID)
            {
                throw new NotImplementedException();
            }

            public void SetResourceData(string resourceid, string dataname, OSGeo.MapGuide.ObjectModels.Common.ResourceDataType datatype, Stream stream)
            {
                throw new NotImplementedException();
            }

            public void SetResourceData(string resourceid, string dataname, OSGeo.MapGuide.ObjectModels.Common.ResourceDataType datatype, Stream stream, Utility.StreamCopyProgressDelegate callback)
            {
                throw new NotImplementedException();
            }

            public void SetResourceXmlData(string resourceid, Stream stream)
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

            public void DeleteFolder(string folderPath)
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

            public void CopyFolder(string oldpath, string newpath, bool overwrite)
            {
                throw new NotImplementedException();
            }

            public void MoveResource(string oldpath, string newpath, bool overwrite)
            {
                throw new NotImplementedException();
            }

            public void MoveFolder(string oldpath, string newpath, bool overwrite)
            {
                throw new NotImplementedException();
            }

            public bool MoveResourceWithReferences(string oldpath, string newpath, LengthyOperationCallBack callback, LengthyOperationProgressCallBack progress)
            {
                throw new NotImplementedException();
            }

            public bool MoveFolderWithReferences(string oldpath, string newpath, LengthyOperationCallBack callback, LengthyOperationProgressCallBack progress)
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

            public OSGeo.MapGuide.ObjectModels.Common.UserList EnumerateUsers()
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.UserList EnumerateUsers(string group)
            {
                throw new NotImplementedException();
            }

            public OSGeo.MapGuide.ObjectModels.Common.GroupList EnumerateGroups()
            {
                throw new NotImplementedException();
            }

            public void UploadPackage(string filename, Utility.StreamCopyProgressDelegate callback)
            {
                throw new NotImplementedException();
            }

            public int ServiceType
            {
                get { throw new NotImplementedException(); }
            }

            public T CreateResourceObject<T>() where T : IResource
            {
                throw new NotImplementedException();
            }

            Stream IResourceService.GetResourceXmlData(string resourceID)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        [Test]
        public void TestUpgradeableXmlEditor100()
        {
            //All resource objects are based on 1.0.0 xsd schemas

            _mocks = new Mockery();

            var resSvc = new MockResourceServiceForXmlEditor();

            var conn = _mocks.NewMock<IServerConnection>();
            var caps = new HttpCapabilities(conn);

            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(1, 0)));
            Stub.On(conn).GetProperty("ResourceService").Will(Return.Value(resSvc));
            Stub.On(conn).GetProperty("Capabilities").Will(Return.Value(caps));
            

            IEditorViewContent ed = new XmlEditor();

            var resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.FeatureSource"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.FeatureSource));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;
            
            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.LayerDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.MapDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.MapDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.WebLayout"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.WebLayout));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.PrintLayout"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.PrintLayout));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.LoadProcedure"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LoadProcedure));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            //MGOS 1.0.0 doesn't do fusion
            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.ApplicationDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.ApplicationDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            try
            {
                ed.Resource = resource;
            }
            catch (UnsupportedResourceTypeException ex)
            {
                Assert.AreEqual(ex.ResourceType, ResourceTypes.ApplicationDefinition);
            }

            //MGOS 1.0.0 doesn't do advanced symbology either
            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolDefinition ));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            try
            {
                ed.Resource = resource;
            }
            catch (UnsupportedResourceTypeException ex)
            {
                Assert.AreEqual(ex.ResourceType, ResourceTypes.SymbolDefinition);
            }

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolLibrary"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolLibrary));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

        }

        [Test]
        public void TestUpgradeableXmlEditor110()
        {
            //All resource objects are based on 1.0.0 xsd schemas
            //MGOS 1.1.0 has the same capability matrix as 1.0.0 so the code is 99% the same

            _mocks = new Mockery();

            var resSvc = new MockResourceServiceForXmlEditor();

            var conn = _mocks.NewMock<IServerConnection>();
            var caps = new HttpCapabilities(conn);

            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(1, 1)));
            Stub.On(conn).GetProperty("ResourceService").Will(Return.Value(resSvc));
            Stub.On(conn).GetProperty("Capabilities").Will(Return.Value(caps));


            IEditorViewContent ed = new XmlEditor();

            var resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.FeatureSource"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.FeatureSource));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.LayerDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.MapDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.MapDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.WebLayout"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.WebLayout));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.PrintLayout"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.PrintLayout));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.LoadProcedure"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LoadProcedure));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolLibrary"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolLibrary));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);


            //MGOS 1.0.0 doesn't do fusion
            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.ApplicationDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.ApplicationDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            try
            {
                ed.Resource = resource;
            }
            catch (UnsupportedResourceTypeException ex)
            {
                Assert.AreEqual(ex.ResourceType, ResourceTypes.ApplicationDefinition);
            }

            //MGOS 1.0.0 doesn't do advanced symbology either
            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            try
            {
                ed.Resource = resource;
            }
            catch (UnsupportedResourceTypeException ex)
            {
                Assert.AreEqual(ex.ResourceType, ResourceTypes.SymbolDefinition);
            }

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolLibrary"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolLibrary));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

        }

        [Test]
        public void TestUpgradeableXmlEditor120()
        {
            //All resource objects are based on 1.0.0 xsd schemas

            _mocks = new Mockery();

            var resSvc = new MockResourceServiceForXmlEditor();

            var conn = _mocks.NewMock<IServerConnection>();
            var caps = new HttpCapabilities(conn);

            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(1, 2)));
            Stub.On(conn).GetProperty("ResourceService").Will(Return.Value(resSvc));
            Stub.On(conn).GetProperty("Capabilities").Will(Return.Value(caps));


            IEditorViewContent ed = new XmlEditor();

            IResource resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.FeatureSource"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.FeatureSource));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.LayerDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));


            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsTrue(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.MapDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.MapDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));


            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.WebLayout"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.WebLayout));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));


            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.PrintLayout"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.PrintLayout));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.LoadProcedure"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LoadProcedure));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            //MGOS 1.2.0 doesn't do fusion
            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.ApplicationDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.ApplicationDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            try
            {
                ed.Resource = resource;
            }
            catch (UnsupportedResourceTypeException ex)
            {
                Assert.AreEqual(ex.ResourceType, ResourceTypes.ApplicationDefinition);
            }

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            Assert.IsFalse(ed.CanBePreviewed);
            Assert.IsFalse(ed.CanUpgrade);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolLibrary"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolLibrary));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

        }

        [Test]
        public void TestUpgradeableXmlEditor200()
        {
            //All resource objects are based on 1.0.0 xsd schemas

            _mocks = new Mockery();

            var resSvc = new MockResourceServiceForXmlEditor();

            var conn = _mocks.NewMock<IServerConnection>();
            var caps = new HttpCapabilities(conn);

            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(2, 0)));
            Stub.On(conn).GetProperty("ResourceService").Will(Return.Value(resSvc));
            Stub.On(conn).GetProperty("Capabilities").Will(Return.Value(caps));


            IEditorViewContent ed = new XmlEditor();

            IResource resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.FeatureSource"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.FeatureSource));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.LayerDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsTrue(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.MapDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.MapDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.WebLayout"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.WebLayout));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.PrintLayout"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.PrintLayout));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.LoadProcedure"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LoadProcedure));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsTrue(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.ApplicationDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.ApplicationDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            Assert.IsTrue(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolLibrary"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolLibrary));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);
        }

        [Test]
        public void TestUpgradeableXmlEditor210()
        {
            //All resource objects are based on 1.0.0 xsd schemas

            _mocks = new Mockery();

            var resSvc = new MockResourceServiceForXmlEditor();

            var conn = _mocks.NewMock<IServerConnection>();
            var caps = new HttpCapabilities(conn);

            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(2, 1)));
            Stub.On(conn).GetProperty("ResourceService").Will(Return.Value(resSvc));
            Stub.On(conn).GetProperty("Capabilities").Will(Return.Value(caps));


            IEditorViewContent ed = new XmlEditor();

            IResource resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.FeatureSource"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.FeatureSource));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.LayerDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsTrue(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.MapDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.MapDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.WebLayout"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.WebLayout));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.PrintLayout"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.PrintLayout));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.LoadProcedure"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LoadProcedure));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsTrue(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.ApplicationDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.ApplicationDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            Assert.IsTrue(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolLibrary"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolLibrary));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);
        }

        [Test]
        public void TestUpgradeableXmlEditor220()
        {
            //All resource objects are based on 1.0.0 xsd schemas

            _mocks = new Mockery();

            var resSvc = new MockResourceServiceForXmlEditor();

            var conn = _mocks.NewMock<IServerConnection>();
            var caps = new HttpCapabilities(conn);

            Stub.On(conn).GetProperty("SiteVersion").Will(Return.Value(new Version(2, 2)));
            Stub.On(conn).GetProperty("ResourceService").Will(Return.Value(resSvc));
            Stub.On(conn).GetProperty("Capabilities").Will(Return.Value(caps));


            IEditorViewContent ed = new XmlEditor();

            IResource resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.FeatureSource"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.FeatureSource));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.LayerDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LayerDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsTrue(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.MapDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.MapDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.WebLayout"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.WebLayout));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsTrue(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.PrintLayout"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.PrintLayout));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.LoadProcedure"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.LoadProcedure));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsTrue(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.ApplicationDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.ApplicationDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsTrue(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolDefinition"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolDefinition));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            Assert.IsTrue(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);

            resource = _mocks.NewMock<IResource>();
            Stub.On(resource).Method("Serialize").Will(Return.Value(""));
            Stub.On(resource).GetProperty("ResourceID").Will(Return.Value("Library://UnitTest/Test.SymbolLibrary"));
            Stub.On(resource).GetProperty("ResourceType").Will(Return.Value(ResourceTypes.SymbolLibrary));
            Stub.On(resource).GetProperty("ResourceVersion").Will(Return.Value(new Version(1, 0, 0)));
            Stub.On(resource).GetProperty("CurrentConnection").Will(Return.Value(conn));

            //The bit that actually kicks everything into action
            ed.Resource = resource;

            //The values we're interested in
            Assert.IsFalse(ed.CanUpgrade);
            Assert.IsFalse(ed.CanBePreviewed);
        }
    }
}
