#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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
    public class LocalNativeRuntimeMapFixture : RuntimeMapFixture
    {
        protected override bool ShouldIgnore(out string reason)
        {
            reason = string.Empty;
            if (TestControl.IgnoreLocalNativeRuntimeMapTests)
                reason = "Skipping LocalNativeRuntimeMapTests because TestControl.IgnoreLocalNativeRuntimeMapTests = true";

            return TestControl.IgnoreLocalNativeRuntimeMapTests;
        }

        public override string Provider => "LocalNative";

        public override IServerConnection CreateTestConnection()
        {
            return ConnectionUtil.CreateTestLocalNativeConnection();
        }
    }

    public class LocalNativeRuntimeMapTests : RuntimeMapTests<LocalNativeRuntimeMapFixture>
    {
        public LocalNativeRuntimeMapTests(LocalNativeRuntimeMapFixture fixture)
            : base(fixture)
        {
        }

        [SkippableFact]
        public override void TestGroupAssignment()
        {
            base.TestGroupAssignment();
        }

        [SkippableFact]
        public override void TestExtentSerialization()
        {
            base.TestExtentSerialization();
        }

        [SkippableFact]
        public override void TestResourceEvents()
        {
            base.TestResourceEvents();
        }

        [SkippableFact]
        public override void TestCreate()
        {
            base.TestCreate();
        }

        [SkippableFact]
        public override void TestSave()
        {
            base.TestSave();
        }

        [SkippableFact]
        public override void TestRender75k()
        {
            base.TestRender75k();
        }

        [SkippableFact]
        public override void TestRender12k()
        {
            base.TestRender12k();
        }

        [SkippableFact]
        public override void TestLegendIconRendering()
        {
            base.TestLegendIconRendering();
        }

        [SkippableFact]
        public override void TestMapManipulation()
        {
            base.TestMapManipulation();
        }

        [SkippableFact]
        public override void TestMapManipulation2()
        {
            base.TestMapManipulation2();
        }

        [SkippableFact]
        public override void TestMapManipulation3()
        {
            base.TestMapManipulation3();
        }

        [SkippableFact]
        public override void TestLargeMapCreatePerformance()
        {
            base.TestLargeMapCreatePerformance();
        }

        [SkippableFact]
        public override void TestMapAddDwfLayer()
        {
            base.TestMapAddDwfLayer();
        }

        [SkippableFact]
        public override void TestCaseDuplicateLayerIds()
        {
            base.TestCaseDuplicateLayerIds();
        }
    }
}