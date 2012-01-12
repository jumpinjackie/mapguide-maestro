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
using OSGeo.MapGuide.MaestroAPI;

namespace MaestroAPITests
{
    //Use this to toggle individual test suites

    public class TestControl
    {
        public const bool IgnoreConfigurationTests = true;
        public const bool IgnoreCapabilityTests = true;
        public const bool IgnoreExpressionTests = true;
        public const bool IgnoreFeatureReaderTests = true;
        public const bool IgnoreHttpConnectionTests = true;
        public const bool IgnoreHttpSiteTests = true;
        public const bool IgnoreObjectTests = true;
        public const bool IgnoreResourceTests = true;
        public const bool IgnoreHttpRuntimeMapTests = true;
        public const bool IgnoreLocalRuntimeMapTests = true;
        public const bool IgnoreLocalFeatureTests = true;
        public const bool IgnoreLocalNativeFeatureTests = true;
        public const bool IgnoreSchemaTests = true;
        public const bool IgnoreSerializationTests = true;
        public const bool IgnoreValidationTests = false;
    }

    public class ConnectionUtil
    {
        public static string Port { get { return "8008"; } }

        public static IServerConnection CreateTestHttpConnection()
        {
            if (!string.IsNullOrEmpty(Port))
            {
                return ConnectionProviderRegistry.CreateConnection("Maestro.Http",
                    "Url", "http://" + Environment.MachineName + ":" + Port + "/mapguide/mapagent/mapagent.fcgi",
                    "Username", "Administrator",
                    "Password", "admin");
            }
            else
            {
                return ConnectionProviderRegistry.CreateConnection("Maestro.Http",
                    "Url", "http://" + Environment.MachineName + "/mapguide/mapagent/mapagent.fcgi",
                    "Username", "Administrator",
                    "Password", "admin");
            }
        }
    }
}
