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
using System;
using Xunit;

namespace MaestroAPITests
{
    public class LocalConnectionFixture : ConnectionTestBaseFixture
    {
        protected override bool ShouldIgnore(out string reason)
        {
            reason = string.Empty;
            if (TestControl.IgnoreLocalFeatureTests)
                reason = "Skipping LocalConnectionTests because TestControl.IgnoreLocalFeatureTests = true";

            return TestControl.IgnoreLocalFeatureTests;
        }

        public override IServerConnection CreateTestConnection()
        {
            return ConnectionUtil.CreateTestLocalConnection();
        }
    }

    public class LocalConnectionTests : ConnectionTestBase<LocalConnectionFixture>
    {
        public LocalConnectionTests(LocalConnectionFixture fixture) 
            : base(fixture)
        {
        }

        protected override string GetTestPrefix()
        {
            return "Local";
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
        public override void TestResourceExists()
        {
            base.TestResourceExists();
        }

        [SkippableFact]
        public void TestFeatureSourceCaching()
        {
            base.TestFeatureSourceCaching("LocalFeatureSourceCaching");
        }

        [SkippableFact]
        public void TestClassDefinitionCaching()
        {
            base.TestClassDefinitionCaching("LocalClassCaching");
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

        [SkippableFact]
        public override void TestCreateRuntimeMapWithInvalidLayersErrorsDisabled()
        {
            base.TestCreateRuntimeMapWithInvalidLayersErrorsDisabled();
        }

        [SkippableFact]
        public override void TestCreateRuntimeMapWithInvalidLayersErrorsEnabled()
        {
            base.TestCreateRuntimeMapWithInvalidLayersErrorsEnabled();
        }

        public override IServerConnection CreateFromExistingSession(IServerConnection orig)
        {
            throw new NotImplementedException();
        }
    }
}