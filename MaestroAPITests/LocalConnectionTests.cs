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

using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI;
using System;

namespace MaestroAPITests
{
    [TestFixture]
    public class LocalConnectionTests : ConnectionTestBase
    {
        protected override bool ShouldIgnore(out string reason)
        {
            reason = string.Empty;
            if (TestControl.IgnoreLocalFeatureTests)
                reason = "Skipping LocalConnectionTests because TestControl.IgnoreLocalFeatureTests = true";

            return TestControl.IgnoreLocalFeatureTests;
        }

        protected override string GetTestPrefix()
        {
            return "Local";
        }

        protected override IServerConnection CreateTestConnection()
        {
            return ConnectionUtil.CreateTestLocalConnection();
        }

        [Test]
        public override void TestTouch()
        {
            base.TestTouch();
        }

        [Test]
        public override void TestAnyStreamInput()
        {
            base.TestAnyStreamInput();
        }

        [Test]
        public override void TestResourceExists()
        {
            base.TestResourceExists();
        }

        [Test]
        public void TestFeatureSourceCaching()
        {
            base.TestFeatureSourceCaching("LocalFeatureSourceCaching");
        }

        [Test]
        public void TestClassDefinitionCaching()
        {
            base.TestClassDefinitionCaching("LocalClassCaching");
        }

        [Test]
        public override void TestApplySchema()
        {
            base.TestApplySchema();
        }

        [Test]
        public override void TestQueryLimits()
        {
            base.TestQueryLimits();
        }

        [Test]
        public override void TestCreateDataStore()
        {
            base.TestCreateDataStore();
        }

        [Test]
        public override void TestDeleteFeatures()
        {
            base.TestDeleteFeatures();
        }

        [Test]
        public override void TestInsertFeatures()
        {
            base.TestInsertFeatures();
        }

        [Test]
        public override void TestUpdateFeatures()
        {
            base.TestUpdateFeatures();
        }

        [Test]
        public override void TestSchemaMapping()
        {
            base.TestSchemaMapping();
        }

        [Test]
        public override void TestCreateRuntimeMapWithInvalidLayersErrorsDisabled()
        {
            base.TestCreateRuntimeMapWithInvalidLayersErrorsDisabled();
        }

        [Test]
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