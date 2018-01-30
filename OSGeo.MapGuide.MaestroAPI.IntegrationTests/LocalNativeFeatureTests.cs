#region Disclaimer / License

// Copyright (C) 2012, Jackie Ng
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
using Xunit;

namespace MaestroAPITests
{
    public class LocalNativeFeatureFixture : ConnectionTestBaseFixture
    {
        protected override bool ShouldIgnore(out string reason)
        {
            reason = string.Empty;
            if (TestControl.IgnoreLocalNativeFeatureTests)
                reason = "Skipping LocalNativeFeatureTests because TestControl.IgnoreLocalNativeFeatureTests = true";

            return TestControl.IgnoreLocalNativeFeatureTests;
        }

        protected override string Provider => "LocalNative";

        public override IServerConnection CreateTestConnection()
        {
            return ConnectionUtil.CreateTestLocalNativeConnection();
        }
    }

    public class LocalNativeFeatureTests : ConnectionTestBase<LocalNativeFeatureFixture>
    {
        public LocalNativeFeatureTests(LocalNativeFeatureFixture fixture) 
            : base(fixture)
        {
        }

        protected override string GetTestPrefix()
        {
            return "LocalNative";
        }

        [SkippableFact]
        public override void TestTouch()
        {
            base.TestTouch();
        }

        [SkippableFact]
        public override void TestAnyStreamInput()
        {
            base.TestAnyStreamInput();
        }

        [SkippableFact]
        public override void TestCreateFromExistingSession()
        {
            base.TestCreateFromExistingSession();
        }

        [SkippableFact]
        public override void TestResourceExists()
        {
            base.TestResourceExists();
        }

        [SkippableFact]
        public void TestFeatureSourceCachingImpl()
        {
            base.TestFeatureSourceCaching("LocalNativeFeatureSourceCaching");
        }

        [SkippableFact]
        public void TestClassDefinitionCachingImpl()
        {
            base.TestClassDefinitionCaching("LocalNativeClassCaching");
        }

        [SkippableFact]
        public override void TestApplySchema()
        {
            base.TestApplySchema();
        }

        [SkippableFact]
        public override void TestQueryLimits()
        {
            base.TestQueryLimits();
        }

        [SkippableFact]
        public override void TestCreateDataStore()
        {
            base.TestCreateDataStore();
        }

        [SkippableFact]
        public override void TestDeleteFeatures()
        {
            base.TestDeleteFeatures();
        }

        [SkippableFact]
        public override void TestInsertFeatures()
        {
            base.TestInsertFeatures();
        }

        [SkippableFact]
        public override void TestUpdateFeatures()
        {
            base.TestUpdateFeatures();
        }

        [SkippableFact]
        public override void TestSchemaMapping()
        {
            base.TestSchemaMapping();
        }

        public override IServerConnection CreateFromExistingSession(IServerConnection orig)
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.LocalNative",
                "SessionId", orig.SessionID);
        }
    }
}