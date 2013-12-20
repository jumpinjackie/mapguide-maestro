#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;
using NUnit.Framework;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
using System.IO;

namespace MaestroAPITests
{
    [TestFixture]
    public class HttpSiteTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            if (TestControl.IgnoreHttpSiteTests)
                Assert.Ignore("Skipping HttpSiteTests because TestControl.IgnoreHttpSiteTests = true");
        }

        private IServerConnection CreateTestConnection()
        {
            return ConnectionUtil.CreateTestHttpConnection();
        }

        [Test]
        public void TestMapDefinitionValidation()
        {
            var conn = CreateTestConnection();
            var mdf = ObjectFactory.CreateMapDefinition(conn, "Test");
            mdf.ResourceID = "Library://UnitTests/Test.MapDefinition";

            var context = new ResourceValidationContext(conn.ResourceService, conn.FeatureService);
            var issues = ResourceValidatorSet.Validate(context, mdf, false);
            Assert.True(issues.Any(x => x.StatusCode == ValidationStatusCode.Warning_MapDefinition_MissingCoordinateSystem));

            conn.ResourceService.SetResourceXmlData("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.ldf"));

            var layer = mdf.AddLayer(null, "bar", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            layer.Group = "foo"; //AddLayer no longer lets us put in bogus group names
            context = new ResourceValidationContext(conn.ResourceService, conn.FeatureService);
            issues = ResourceValidatorSet.Validate(context, mdf, false);
            Assert.True(issues.Any(x => x.StatusCode == ValidationStatusCode.Error_MapDefinition_LayerWithNonExistentGroup));

            mdf.AddLayer(null, "bar", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            context = new ResourceValidationContext(conn.ResourceService, conn.FeatureService);
            issues = ResourceValidatorSet.Validate(context, mdf, false);
            Assert.True(issues.Any(x => x.StatusCode == ValidationStatusCode.Error_MapDefinition_DuplicateLayerName));

            var group = mdf.AddGroup("foo");
            group.Group = "bar";

            context = new ResourceValidationContext(conn.ResourceService, conn.FeatureService);
            issues = ResourceValidatorSet.Validate(context, mdf, false);
            Assert.True(issues.Any(x => x.StatusCode == ValidationStatusCode.Error_MapDefinition_GroupWithNonExistentGroup));
        }

        [Test]
        public void TestSymbolDefinitionValidation()
        {
            var conn = CreateTestConnection();
            var ssym = ObjectFactory.CreateSimpleSymbol(conn, "Test", "Test");
            ssym.ResourceID = "Library://UnitTests/Test.SymbolDefinition";

            var context = new ResourceValidationContext(conn.ResourceService, conn.FeatureService);
            var issues = ResourceValidatorSet.Validate(context, ssym, true);
            Assert.True(issues.Any(x => x.StatusCode == ValidationStatusCode.Error_SymbolDefinition_NoGeometryUsageContexts));

            var param = ssym.CreateParameter();
            param.Identifier = "TEST";
            ssym.ParameterDefinition.AddParameter(param);

            context = new ResourceValidationContext(conn.ResourceService, conn.FeatureService);
            issues = ResourceValidatorSet.Validate(context, ssym, true);
            Assert.True(issues.Any(x => x.StatusCode == ValidationStatusCode.Warning_SymbolDefinition_SymbolParameterNotUsed));

            var imgRef = ssym.CreateImageReference("Library://UnitTest/IDontExist.LayerDefinition", "Foo.png");
            var img = ssym.CreateImageGraphics();
            img.Item = imgRef;

            ssym.AddGraphics(img);

            context = new ResourceValidationContext(conn.ResourceService, conn.FeatureService);
            issues = ResourceValidatorSet.Validate(context, ssym, true);
            Assert.True(issues.Any(x => x.StatusCode == ValidationStatusCode.Error_SymbolDefinition_ImageGraphicReferenceResourceIdNotFound));

            conn.ResourceService.SetResourceXmlData("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.ldf"));

            imgRef = ssym.CreateImageReference("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition", "Foo.png");
            img = ssym.CreateImageGraphics();
            img.Item = imgRef;

            ssym.AddGraphics(img);

            context = new ResourceValidationContext(conn.ResourceService, conn.FeatureService);
            issues = ResourceValidatorSet.Validate(context, ssym, true);
            Assert.True(issues.Any(x => x.StatusCode == ValidationStatusCode.Error_SymbolDefinition_ImageGraphicReferenceResourceDataNotFound));
        }
    }
}
