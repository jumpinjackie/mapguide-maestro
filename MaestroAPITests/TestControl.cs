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
        public const bool IgnoreConfigurationTests = false;
        public const bool IgnoreCapabilityTests = false;
        public const bool IgnoreExpressionTests = false;
        public const bool IgnoreFeatureReaderTests = false;
        public const bool IgnoreHttpConnectionTests = false;
        public const bool IgnoreHttpSiteTests = false;
        public const bool IgnoreObjectTests = false;
        public const bool IgnoreResourceTests = false;
        public const bool IgnoreHttpRuntimeMapTests = false;
        public const bool IgnoreLocalRuntimeMapTests = false;
        public const bool IgnoreLocalNativeRuntimeMapTests = true;
        public const bool IgnoreLocalNativePerformanceTests = true;
        public const bool IgnoreLocalNativeFeatureTests = true;
        public const bool IgnoreGeoRestTests = true;
        public const bool IgnoreLocalFeatureTests = false;
        public const bool IgnoreSchemaTests = false;
        public const bool IgnoreSerializationTests = false;
        public const bool IgnoreValidationTests = false;
        public const bool IgnoreMiscTests = false;
    }

    public class ConnectionUtil
    {
        public static string Port { get { return "8018"; } }

        public static IServerConnection CreateTestLocalNativeConnection()
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.LocalNative",
                    "ConfigFile", "MGOS22\\webconfig.ini",
                    "Username", "Administrator",
                    "Password", "admin");
        }

        public static IServerConnection CreateTestLocalConnection()
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.Local", "ConfigFile", "Platform.ini");
        }

        public static IServerConnection CreateTestHttpConnectionWithGeoRest()
        {
            if (!string.IsNullOrEmpty(Port))
            {
                return ConnectionProviderRegistry.CreateConnection("Maestro.Http",
                    "Url", "http://" + Environment.MachineName + ":" + Port + "/mapguide/mapagent/mapagent.fcgi",
                    "Username", "Administrator",
                    "Password", "admin",
                    "GeoRestUrl", "http://localhost:99/",
                    "GeoRestConfigPath", "UserTestData\\GeoRestConfig.xml");
            }
            else
            {
                return ConnectionProviderRegistry.CreateConnection("Maestro.Http",
                    "Url", "http://" + Environment.MachineName + "/mapguide/mapagent/mapagent.fcgi",
                    "Username", "Administrator",
                    "Password", "admin",
                    "GeoRestUrl", "http://localhost:99/",
                    "GeoRestConfigPath", "UserTestData\\GeoRestConfig.xml");
            }
        }

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
