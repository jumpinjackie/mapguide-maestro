#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
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

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace MaestroAPITests
{
    public class HttpSiteTests : IDisposable
    {
        private bool _skip;
        private string _skipReason;

        public HttpSiteTests()
        {
            _skip = TestControl.IgnoreHttpSiteTests;
            _skipReason = _skip ? "Skipping HttpSiteTests because TestControl.IgnoreHttpSiteTests = true" : string.Empty;
        }

        public void Dispose() { }

        private IServerConnection CreateTestConnection()
        {
            return ConnectionUtil.CreateTestHttpConnection();
        }

        [SkippableFact]
        public void TestMapDefinitionValidation()
        {
            Skip.If(_skip, _skipReason);

            var conn = CreateTestConnection();
            var mdf = Utility.CreateMapDefinition(conn, "Test");
            mdf.ResourceID = "Library://UnitTests/Test.MapDefinition";

            var context = new ResourceValidationContext(conn);
            var issues = ResourceValidatorSet.Validate(context, mdf, false);
            Assert.Contains(issues, x => x.StatusCode == ValidationStatusCode.Warning_MapDefinition_MissingCoordinateSystem);

            conn.ResourceService.SetResourceXmlData("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.ldf"));

            var layer = mdf.AddLayer(null, "bar", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            layer.Group = "foo"; //AddLayer no longer lets us put in bogus group names
            context = new ResourceValidationContext(conn);
            issues = ResourceValidatorSet.Validate(context, mdf, false);
            Assert.Contains(issues, x => x.StatusCode == ValidationStatusCode.Error_MapDefinition_LayerWithNonExistentGroup);

            mdf.AddLayer(null, "bar", "Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition");
            context = new ResourceValidationContext(conn);
            issues = ResourceValidatorSet.Validate(context, mdf, false);
            Assert.Contains(issues, x => x.StatusCode == ValidationStatusCode.Error_MapDefinition_DuplicateLayerName);

            var group = mdf.AddGroup("foo");
            group.Group = "bar";

            context = new ResourceValidationContext(conn);
            issues = ResourceValidatorSet.Validate(context, mdf, false);
            Assert.Contains(issues, x => x.StatusCode == ValidationStatusCode.Error_MapDefinition_GroupWithNonExistentGroup);
        }

        [SkippableFact]
        public void TestSymbolDefinitionValidation()
        {
            Skip.If(_skip, _skipReason);

            var conn = CreateTestConnection();
            var ssym = Utility.CreateSimpleSymbol(conn, "Test", "Test");
            ssym.ResourceID = "Library://UnitTests/Test.SymbolDefinition";

            var context = new ResourceValidationContext(conn);
            var issues = ResourceValidatorSet.Validate(context, ssym, true);
            Assert.Contains(issues, x => x.StatusCode == ValidationStatusCode.Error_SymbolDefinition_NoGeometryUsageContexts);

            var param = ssym.CreateParameter();
            param.Identifier = "TEST";
            ssym.ParameterDefinition.AddParameter(param);

            context = new ResourceValidationContext(conn);
            issues = ResourceValidatorSet.Validate(context, ssym, true);
            Assert.Contains(issues, x => x.StatusCode == ValidationStatusCode.Warning_SymbolDefinition_SymbolParameterNotUsed);

            var imgRef = ssym.CreateImageReference("Library://UnitTest/IDontExist.LayerDefinition", "Foo.png");
            var img = ssym.CreateImageGraphics();
            img.Item = imgRef;

            ssym.AddGraphics(img);

            context = new ResourceValidationContext(conn);
            issues = ResourceValidatorSet.Validate(context, ssym, true);
            Assert.Contains(issues, x => x.StatusCode == ValidationStatusCode.Error_SymbolDefinition_ImageGraphicReferenceResourceIdNotFound);

            conn.ResourceService.SetResourceXmlData("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.ldf"));

            imgRef = ssym.CreateImageReference("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition", "Foo.png");
            img = ssym.CreateImageGraphics();
            img.Item = imgRef;

            ssym.AddGraphics(img);

            context = new ResourceValidationContext(conn);
            issues = ResourceValidatorSet.Validate(context, ssym, true);
            Assert.Contains(issues, x => x.StatusCode == ValidationStatusCode.Error_SymbolDefinition_ImageGraphicReferenceResourceDataNotFound);
        }
    }
}