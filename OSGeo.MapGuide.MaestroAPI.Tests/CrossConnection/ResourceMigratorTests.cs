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
using OSGeo.MapGuide.MaestroAPI.CrossConnection;
using NUnit.Framework;
using Moq;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Common;
namespace OSGeo.MapGuide.MaestroAPI.CrossConnection.Tests
{
    [TestFixture()]
    public class ResourceMigratorTests
    {
        [Test()]
        public void ResourceMigratorTest()
        {
            var source = new Mock<IServerConnection>();
            var target = new Mock<IServerConnection>();

            Assert.Throws<ArgumentNullException>(() => new ResourceMigrator(null, null));
            Assert.Throws<ArgumentNullException>(() => new ResourceMigrator(source.Object, null));
            Assert.Throws<ArgumentNullException>(() => new ResourceMigrator(null, target.Object));

            var mig = new ResourceMigrator(source.Object, target.Object);
            Assert.NotNull(mig.Source);
            Assert.NotNull(mig.Target);
        }

        [Test()]
        public void CopyResourcesWithOverwriteTest()
        {
            var sr = new Mock<IResourceService>();
            var tr = new Mock<IResourceService>();

            var tc = new Mock<IConnectionCapabilities>();
            tc.Setup(c => c.GetMaxSupportedResourceVersion(It.IsAny<string>())).Returns(new Version(1, 0, 0));

            var source = new Mock<IServerConnection>();
            var target = new Mock<IServerConnection>();

            var emptyDataList = new ResourceDataList() { ResourceData = new System.ComponentModel.BindingList<ResourceDataListResourceData>() };

            string [] resources = new string[]
            {
                "Library://Test/Data.FeatureSource",
                "Library://Test/Data1.FeatureSource",
                "Library://Test/Data2.FeatureSource",
                "Library://Test/Data3.FeatureSource",
                "Library://Test/Data4.FeatureSource",
            };

            var res1 = new Mock<IResource>();
            var res2 = new Mock<IResource>();
            var res3 = new Mock<IResource>();
            var res4 = new Mock<IResource>();
            var res5 = new Mock<IResource>();

            res1.Setup(r => r.ResourceID).Returns(resources[0]);
            res2.Setup(r => r.ResourceID).Returns(resources[1]);
            res3.Setup(r => r.ResourceID).Returns(resources[2]);
            res4.Setup(r => r.ResourceID).Returns(resources[3]);
            res5.Setup(r => r.ResourceID).Returns(resources[4]);

            sr.Setup(r => r.EnumerateResourceData(It.IsAny<string>())).Returns(emptyDataList);
            sr.Setup(r => r.GetResource(resources[0])).Returns(res1.Object);
            sr.Setup(r => r.GetResource(resources[1])).Returns(res2.Object);
            sr.Setup(r => r.GetResource(resources[2])).Returns(res3.Object);
            sr.Setup(r => r.GetResource(resources[3])).Returns(res4.Object);
            sr.Setup(r => r.GetResource(resources[4])).Returns(res5.Object);
            //tr.Setup(r => r.ResourceExists(It.IsAny<string>())).Returns(false);

            source.Setup(c => c.ResourceService).Returns(sr.Object);
            target.Setup(c => c.ResourceService).Returns(tr.Object);
            target.Setup(c => c.Capabilities).Returns(tc.Object);

            var mig = new ResourceMigrator(source.Object, target.Object);
            Assert.NotNull(mig.Source);
            Assert.NotNull(mig.Target);

            int migrated = mig.CopyResources(resources, "Library://Migrated/", true, null);
            Assert.AreEqual(5, migrated);

            sr.Verify(c => c.GetResource(resources[0]), Times.Once);
            sr.Verify(c => c.GetResource(resources[1]), Times.Once);
            sr.Verify(c => c.GetResource(resources[2]), Times.Once);
            sr.Verify(c => c.GetResource(resources[3]), Times.Once);
            sr.Verify(c => c.GetResource(resources[4]), Times.Once);

            sr.Verify(c => c.EnumerateResourceData(resources[0]), Times.Once);
            sr.Verify(c => c.EnumerateResourceData(resources[1]), Times.Once);
            sr.Verify(c => c.EnumerateResourceData(resources[2]), Times.Once);
            sr.Verify(c => c.EnumerateResourceData(resources[3]), Times.Once);
            sr.Verify(c => c.EnumerateResourceData(resources[4]), Times.Once);

            tr.Verify(c => c.SaveResourceAs(It.IsAny<IResource>(), It.IsAny<string>()), Times.Exactly(5));
        }

        [Test()]
        public void CopyResourcesWithoutOverwriteTest()
        {
            var sr = new Mock<IResourceService>();
            var tr = new Mock<IResourceService>();

            var tc = new Mock<IConnectionCapabilities>();
            tc.Setup(c => c.GetMaxSupportedResourceVersion(It.IsAny<string>())).Returns(new Version(1, 0, 0));

            var source = new Mock<IServerConnection>();
            var target = new Mock<IServerConnection>();

            var emptyDataList = new ResourceDataList() { ResourceData = new System.ComponentModel.BindingList<ResourceDataListResourceData>() };

            string[] resources = new string[]
            {
                "Library://Test/Data.FeatureSource",
                "Library://Test/Data1.FeatureSource",
                "Library://Test/Data2.FeatureSource",
                "Library://Test/Data3.FeatureSource",
                "Library://Test/Data4.FeatureSource",
            };

            var res1 = new Mock<IResource>();
            var res2 = new Mock<IResource>();
            var res3 = new Mock<IResource>();
            var res4 = new Mock<IResource>();
            var res5 = new Mock<IResource>();

            res1.Setup(r => r.ResourceID).Returns(resources[0]);
            res2.Setup(r => r.ResourceID).Returns(resources[1]);
            res3.Setup(r => r.ResourceID).Returns(resources[2]);
            res4.Setup(r => r.ResourceID).Returns(resources[3]);
            res5.Setup(r => r.ResourceID).Returns(resources[4]);

            sr.Setup(r => r.EnumerateResourceData(It.IsAny<string>())).Returns(emptyDataList);
            sr.Setup(r => r.GetResource(resources[0])).Returns(res1.Object);
            sr.Setup(r => r.GetResource(resources[1])).Returns(res2.Object);
            sr.Setup(r => r.GetResource(resources[2])).Returns(res3.Object);
            sr.Setup(r => r.GetResource(resources[3])).Returns(res4.Object);
            sr.Setup(r => r.GetResource(resources[4])).Returns(res5.Object);

            tr.Setup(r => r.ResourceExists("Library://Migrated/Data.FeatureSource")).Returns(false);
            tr.Setup(r => r.ResourceExists("Library://Migrated/Data1.FeatureSource")).Returns(true);
            tr.Setup(r => r.ResourceExists("Library://Migrated/Data2.FeatureSource")).Returns(true);
            tr.Setup(r => r.ResourceExists("Library://Migrated/Data3.FeatureSource")).Returns(false);
            tr.Setup(r => r.ResourceExists("Library://Migrated/Data4.FeatureSource")).Returns(false);

            source.Setup(c => c.ResourceService).Returns(sr.Object);
            target.Setup(c => c.ResourceService).Returns(tr.Object);
            target.Setup(c => c.Capabilities).Returns(tc.Object);

            var mig = new ResourceMigrator(source.Object, target.Object);
            Assert.NotNull(mig.Source);
            Assert.NotNull(mig.Target);

            int migrated = mig.CopyResources(resources, "Library://Migrated/", false, null);
            Assert.AreEqual(3, migrated);

            sr.Verify(c => c.GetResource(resources[0]), Times.Once);
            sr.Verify(c => c.GetResource(resources[1]), Times.Never);
            sr.Verify(c => c.GetResource(resources[2]), Times.Never);
            sr.Verify(c => c.GetResource(resources[3]), Times.Once);
            sr.Verify(c => c.GetResource(resources[4]), Times.Once);

            sr.Verify(c => c.EnumerateResourceData(resources[0]), Times.Once);
            sr.Verify(c => c.EnumerateResourceData(resources[1]), Times.Never);
            sr.Verify(c => c.EnumerateResourceData(resources[2]), Times.Never);
            sr.Verify(c => c.EnumerateResourceData(resources[3]), Times.Once);
            sr.Verify(c => c.EnumerateResourceData(resources[4]), Times.Once);

            tr.Verify(c => c.SaveResourceAs(It.IsAny<IResource>(), "Library://Migrated/Data.FeatureSource"), Times.Once);
            tr.Verify(c => c.SaveResourceAs(It.IsAny<IResource>(), "Library://Migrated/Data1.FeatureSource"), Times.Never);
            tr.Verify(c => c.SaveResourceAs(It.IsAny<IResource>(), "Library://Migrated/Data2.FeatureSource"), Times.Never);
            tr.Verify(c => c.SaveResourceAs(It.IsAny<IResource>(), "Library://Migrated/Data3.FeatureSource"), Times.Once);
            tr.Verify(c => c.SaveResourceAs(It.IsAny<IResource>(), "Library://Migrated/Data4.FeatureSource"), Times.Once);
        }

        [Test()]
        public void MoveResourcesWithOverwriteTest()
        {
            var sr = new Mock<IResourceService>();
            var tr = new Mock<IResourceService>();

            var tc = new Mock<IConnectionCapabilities>();
            tc.Setup(c => c.GetMaxSupportedResourceVersion(It.IsAny<string>())).Returns(new Version(1, 0, 0));

            var source = new Mock<IServerConnection>();
            var target = new Mock<IServerConnection>();

            var emptyDataList = new ResourceDataList() { ResourceData = new System.ComponentModel.BindingList<ResourceDataListResourceData>() };

            string[] resources = new string[]
            {
                "Library://Test/Data.FeatureSource",
                "Library://Test/Data1.FeatureSource",
                "Library://Test/Data2.FeatureSource",
                "Library://Test/Data3.FeatureSource",
                "Library://Test/Data4.FeatureSource",
            };

            var res1 = new Mock<IResource>();
            var res2 = new Mock<IResource>();
            var res3 = new Mock<IResource>();
            var res4 = new Mock<IResource>();
            var res5 = new Mock<IResource>();

            res1.Setup(r => r.ResourceID).Returns(resources[0]);
            res2.Setup(r => r.ResourceID).Returns(resources[1]);
            res3.Setup(r => r.ResourceID).Returns(resources[2]);
            res4.Setup(r => r.ResourceID).Returns(resources[3]);
            res5.Setup(r => r.ResourceID).Returns(resources[4]);

            sr.Setup(r => r.EnumerateResourceData(It.IsAny<string>())).Returns(emptyDataList);
            sr.Setup(r => r.GetResource(resources[0])).Returns(res1.Object);
            sr.Setup(r => r.GetResource(resources[1])).Returns(res2.Object);
            sr.Setup(r => r.GetResource(resources[2])).Returns(res3.Object);
            sr.Setup(r => r.GetResource(resources[3])).Returns(res4.Object);
            sr.Setup(r => r.GetResource(resources[4])).Returns(res5.Object);
            //tr.Setup(r => r.ResourceExists(It.IsAny<string>())).Returns(false);

            source.Setup(c => c.ResourceService).Returns(sr.Object);
            target.Setup(c => c.ResourceService).Returns(tr.Object);
            target.Setup(c => c.Capabilities).Returns(tc.Object);

            var mig = new ResourceMigrator(source.Object, target.Object);
            Assert.NotNull(mig.Source);
            Assert.NotNull(mig.Target);

            int migrated = mig.MoveResources(resources, "Library://Migrated/", true, null);
            Assert.AreEqual(5, migrated);

            sr.Verify(c => c.GetResource(resources[0]), Times.Once);
            sr.Verify(c => c.GetResource(resources[1]), Times.Once);
            sr.Verify(c => c.GetResource(resources[2]), Times.Once);
            sr.Verify(c => c.GetResource(resources[3]), Times.Once);
            sr.Verify(c => c.GetResource(resources[4]), Times.Once);

            sr.Verify(c => c.EnumerateResourceData(resources[0]), Times.Once);
            sr.Verify(c => c.EnumerateResourceData(resources[1]), Times.Once);
            sr.Verify(c => c.EnumerateResourceData(resources[2]), Times.Once);
            sr.Verify(c => c.EnumerateResourceData(resources[3]), Times.Once);
            sr.Verify(c => c.EnumerateResourceData(resources[4]), Times.Once);

            tr.Verify(c => c.SaveResourceAs(It.IsAny<IResource>(), It.IsAny<string>()), Times.Exactly(5));

            sr.Verify(c => c.DeleteResource(resources[0]), Times.Once);
            sr.Verify(c => c.DeleteResource(resources[1]), Times.Once);
            sr.Verify(c => c.DeleteResource(resources[2]), Times.Once);
            sr.Verify(c => c.DeleteResource(resources[3]), Times.Once);
            sr.Verify(c => c.DeleteResource(resources[4]), Times.Once);
        }

        [Test()]
        public void MoveResourcesWithoutOverwriteTest()
        {
            var sr = new Mock<IResourceService>();
            var tr = new Mock<IResourceService>();

            var tc = new Mock<IConnectionCapabilities>();
            tc.Setup(c => c.GetMaxSupportedResourceVersion(It.IsAny<string>())).Returns(new Version(1, 0, 0));

            var source = new Mock<IServerConnection>();
            var target = new Mock<IServerConnection>();

            var emptyDataList = new ResourceDataList() { ResourceData = new System.ComponentModel.BindingList<ResourceDataListResourceData>() };

            string[] resources = new string[]
            {
                "Library://Test/Data.FeatureSource",
                "Library://Test/Data1.FeatureSource",
                "Library://Test/Data2.FeatureSource",
                "Library://Test/Data3.FeatureSource",
                "Library://Test/Data4.FeatureSource",
            };

            var res1 = new Mock<IResource>();
            var res2 = new Mock<IResource>();
            var res3 = new Mock<IResource>();
            var res4 = new Mock<IResource>();
            var res5 = new Mock<IResource>();

            res1.Setup(r => r.ResourceID).Returns(resources[0]);
            res2.Setup(r => r.ResourceID).Returns(resources[1]);
            res3.Setup(r => r.ResourceID).Returns(resources[2]);
            res4.Setup(r => r.ResourceID).Returns(resources[3]);
            res5.Setup(r => r.ResourceID).Returns(resources[4]);

            sr.Setup(r => r.EnumerateResourceData(It.IsAny<string>())).Returns(emptyDataList);
            sr.Setup(r => r.GetResource(resources[0])).Returns(res1.Object);
            sr.Setup(r => r.GetResource(resources[1])).Returns(res2.Object);
            sr.Setup(r => r.GetResource(resources[2])).Returns(res3.Object);
            sr.Setup(r => r.GetResource(resources[3])).Returns(res4.Object);
            sr.Setup(r => r.GetResource(resources[4])).Returns(res5.Object);

            tr.Setup(r => r.ResourceExists("Library://Migrated/Data.FeatureSource")).Returns(false);
            tr.Setup(r => r.ResourceExists("Library://Migrated/Data1.FeatureSource")).Returns(true);
            tr.Setup(r => r.ResourceExists("Library://Migrated/Data2.FeatureSource")).Returns(true);
            tr.Setup(r => r.ResourceExists("Library://Migrated/Data3.FeatureSource")).Returns(false);
            tr.Setup(r => r.ResourceExists("Library://Migrated/Data4.FeatureSource")).Returns(false);

            source.Setup(c => c.ResourceService).Returns(sr.Object);
            target.Setup(c => c.ResourceService).Returns(tr.Object);
            target.Setup(c => c.Capabilities).Returns(tc.Object);

            var mig = new ResourceMigrator(source.Object, target.Object);
            Assert.NotNull(mig.Source);
            Assert.NotNull(mig.Target);

            int migrated = mig.MoveResources(resources, "Library://Migrated/", false, null);
            Assert.AreEqual(3, migrated);

            sr.Verify(c => c.GetResource(resources[0]), Times.Once);
            sr.Verify(c => c.GetResource(resources[1]), Times.Never);
            sr.Verify(c => c.GetResource(resources[2]), Times.Never);
            sr.Verify(c => c.GetResource(resources[3]), Times.Once);
            sr.Verify(c => c.GetResource(resources[4]), Times.Once);

            sr.Verify(c => c.EnumerateResourceData(resources[0]), Times.Once);
            sr.Verify(c => c.EnumerateResourceData(resources[1]), Times.Never);
            sr.Verify(c => c.EnumerateResourceData(resources[2]), Times.Never);
            sr.Verify(c => c.EnumerateResourceData(resources[3]), Times.Once);
            sr.Verify(c => c.EnumerateResourceData(resources[4]), Times.Once);

            tr.Verify(c => c.SaveResourceAs(It.IsAny<IResource>(), "Library://Migrated/Data.FeatureSource"), Times.Once);
            tr.Verify(c => c.SaveResourceAs(It.IsAny<IResource>(), "Library://Migrated/Data1.FeatureSource"), Times.Never);
            tr.Verify(c => c.SaveResourceAs(It.IsAny<IResource>(), "Library://Migrated/Data2.FeatureSource"), Times.Never);
            tr.Verify(c => c.SaveResourceAs(It.IsAny<IResource>(), "Library://Migrated/Data3.FeatureSource"), Times.Once);
            tr.Verify(c => c.SaveResourceAs(It.IsAny<IResource>(), "Library://Migrated/Data4.FeatureSource"), Times.Once);

            sr.Verify(c => c.DeleteResource(resources[0]), Times.Once);
            sr.Verify(c => c.DeleteResource(resources[1]), Times.Never);
            sr.Verify(c => c.DeleteResource(resources[2]), Times.Never);
            sr.Verify(c => c.DeleteResource(resources[3]), Times.Once);
            sr.Verify(c => c.DeleteResource(resources[4]), Times.Once);
        }

        //[Test()]
        //public void MigrateResourceTest()
        //{
        //    Assert.Fail();
        //}
    }
}
