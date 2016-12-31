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
using Moq;
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    [TestFixture]
    public class ValidationTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            ResourceValidatorLoader.LoadStockValidators();
        }

        [Test]
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
            Assert.AreEqual(totalIssues.Length, 2);
            Assert.AreEqual(set.ResourceIDs.Length, 1);
            Assert.AreEqual(set.GetIssuesForResource("Library://Test.FeatureSource", ValidationStatus.Error).Count, 2);
        }

        [Test]
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
            Assert.AreEqual(4, set.GetAllIssues().Length);

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
            Assert.AreEqual(4, set.GetAllIssues().Length);

            lp = new OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.LoadProcedure()
            {
                Item = new OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.DwgLoadProcedureType()
            };
            lp.ResourceID = id;
            set = new ValidationResultSet();
            context.Reset();
            set.AddIssues(ResourceValidatorSet.Validate(context, lp, false));

            //Not supported
            Assert.AreEqual(1, set.GetAllIssues().Length);

            lp = new OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.LoadProcedure()
            {
                Item = new OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0.RasterLoadProcedureType()
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

            var validator = new LayerDefinitionValidator();

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

        [Test]
        public void TestCase1896()
        {
            var fs = new FeatureSchema();
            var doc = new XmlDocument();

            doc.Load(Utils.ResolvePath("UserTestData\\1896.xml"));

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
