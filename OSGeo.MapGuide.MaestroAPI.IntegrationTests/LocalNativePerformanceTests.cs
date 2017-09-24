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

using System;
using System.Diagnostics;
using Xunit;

namespace MaestroAPITests
{
    public class LocalNativePerformanceTests : IDisposable
    {
        private bool _skip;
        private string _skipReason;

        public LocalNativePerformanceTests()
        {
            _skip = TestControl.IgnoreLocalNativePerformanceTests;
            _skipReason = _skip ? "Skipping LocalNativePerformanceTests because TestControl.IgnoreLocalNativePerformanceTests = true" : string.Empty;
        }

        public void Dispose()
        {
            
        }

        [SkippableFact]
        public void TestCase1914()
        {
            Skip.If(_skip, _skipReason);

            var conn = ConnectionUtil.CreateTestLocalNativeConnection();
            var sw = new Stopwatch();
            sw.Start();
            conn.ResourceService.ResourceExists("Library://UnitTests/Data/Parcels.FeatureSource");
            sw.Stop();
            Trace.TraceInformation("ResourceExists() executed in {0}ms", sw.ElapsedMilliseconds);
        }
    }
}