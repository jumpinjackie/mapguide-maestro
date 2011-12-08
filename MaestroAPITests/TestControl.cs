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
using System.Collections.Generic;
using System.Text;

namespace MaestroAPITests
{
    //Use this to toggle individual test suites

    public class TestControl
    {
        public const bool IgnoreConfigurationTests = false;
        public const bool IgnoreCapabilityTests = false;
        public const bool IgnoreExpressionTests = false;
        public const bool IgnoreFeatureReaderTests = false;
        public const bool IgnoreHttpConnectionTests = false;
        public const bool IgnoreHttpSiteTests = false;
        public const bool IgnoreObjectTests = false;
        public const bool IgnoreResourceTests = false;
        public const bool IgnoreHttpRuntimeMapTests = false;
        public const bool IgnoreLocalRuntimeMapTests = true;
        public const bool IgnoreSchemaTests = false;
        public const bool IgnoreSerializationTests = false;
        public const bool IgnoreValidationTests = false;
    }
}
