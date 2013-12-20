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
using System.IO;
using System.Xml;

namespace MaestroAPITests
{
    public static class TestEnvironment
    {
        public static string LocalNativeConfigFile = "MGOS22\\webconfig.ini";
        public static string LocalNativeUsername = "Administrator";
        public static string LocalNativePassword = "admin";

        public static string LocalConfigFile = "Platform.ini";

        public static string HttpUrl = "http://localhost/mapguide/mapagent/mapagent.fcgi";
        public static string HttpUsername = "Administrator";
        public static string HttpPassword = "admin";

        public static string GeoRestUrl = "http://localhost:99/";
        public static string GeoRestConfig = "UserTestData\\GeoRestConfig.xml";

        public static void Initialize(string initFile)
        {
            if (File.Exists(initFile))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(initFile);

                var settings = doc.SelectSingleNode("//TestConfiguration/TestSettings");
                if (settings != null)
                {
                    if (settings["LocalNativeConfigFile"] != null)
                        TestEnvironment.LocalNativeConfigFile = settings["LocalNativeConfigFile"].InnerText;
                    if (settings["LocalNativeUsername"] != null)
                        TestEnvironment.LocalNativeUsername = settings["LocalNativeUsername"].InnerText;
                    if (settings["LocalNativePassword"] != null)
                        TestEnvironment.LocalNativePassword = settings["LocalNativePassword"].InnerText;
                    if (settings["LocalConfigFile"] != null)
                        TestEnvironment.LocalConfigFile = settings["LocalConfigFile"].InnerText;
                    if (settings["HttpUrl"] != null)
                        TestEnvironment.HttpUrl = settings["HttpUrl"].InnerText;
                    if (settings["HttpUsername"] != null)
                        TestEnvironment.HttpUsername = settings["HttpUsername"].InnerText;
                    if (settings["HttpPassword"] != null)
                        TestEnvironment.HttpPassword = settings["HttpPassword"].InnerText;
                    if (settings["GeoRestUrl"] != null)
                        TestEnvironment.GeoRestUrl = settings["GeoRestUrl"].InnerText;
                    if (settings["GeoRestConfig"] != null)
                        TestEnvironment.GeoRestConfig = settings["GeoRestConfig"].InnerText;
                }
                var control = doc.SelectSingleNode("//TestConfiguration/TestControl");
                if (control != null)
                {
                    if (control["IgnoreConfigurationTests"] != null)
                        TestControl.IgnoreConfigurationTests = control["IgnoreConfigurationTests"].InnerText == "true";
                    if (control["IgnoreCapabilityTests"] != null)
                        TestControl.IgnoreCapabilityTests = control["IgnoreCapabilityTests"].InnerText == "true";
                    if (control["IgnoreExpressionTests"] != null)
                        TestControl.IgnoreExpressionTests = control["IgnoreExpressionTests"].InnerText == "true";
                    if (control["IgnoreFeatureReaderTests"] != null)
                        TestControl.IgnoreFeatureReaderTests = control["IgnoreFeatureReaderTests"].InnerText == "true";
                    if (control["IgnoreHttpConnectionTests"] != null)
                        TestControl.IgnoreHttpConnectionTests = control["IgnoreHttpConnectionTests"].InnerText == "true";
                    if (control["IgnoreHttpSiteTests"] != null)
                        TestControl.IgnoreHttpSiteTests = control["IgnoreHttpSiteTests"].InnerText == "true";
                    if (control["IgnoreObjectTests"] != null)
                        TestControl.IgnoreObjectTests = control["IgnoreObjectTests"].InnerText == "true";
                    if (control["IgnoreResourceTests"] != null)
                        TestControl.IgnoreResourceTests = control["IgnoreResourceTests"].InnerText == "true";
                    if (control["IgnoreHttpRuntimeMapTests"] != null)
                        TestControl.IgnoreHttpRuntimeMapTests = control["IgnoreHttpRuntimeMapTests"].InnerText == "true";
                    if (control["IgnoreLocalRuntimeMapTests"] != null)
                        TestControl.IgnoreLocalRuntimeMapTests = control["IgnoreLocalRuntimeMapTests"].InnerText == "true";
                    if (control["IgnoreLocalNativeRuntimeMapTests"] != null)
                        TestControl.IgnoreLocalNativeRuntimeMapTests = control["IgnoreLocalNativeRuntimeMapTests"].InnerText == "true";
                    if (control["IgnoreLocalNativePerformanceTests"] != null)
                        TestControl.IgnoreLocalNativePerformanceTests = control["IgnoreLocalNativePerformanceTests"].InnerText == "true";
                    if (control["IgnoreLocalNativeFeatureTests"] != null)
                        TestControl.IgnoreLocalNativeFeatureTests = control["IgnoreLocalNativeFeatureTests"].InnerText == "true";
                    if (control["IgnoreGeoRestTests"] != null)
                        TestControl.IgnoreGeoRestTests = control["IgnoreGeoRestTests"].InnerText == "true";
                    if (control["IgnoreLocalFeatureTests"] != null)
                        TestControl.IgnoreLocalFeatureTests = control["IgnoreLocalFeatureTests"].InnerText == "true";
                    if (control["IgnoreSchemaTests"] != null)
                        TestControl.IgnoreSchemaTests = control["IgnoreSchemaTests"].InnerText == "true";
                    if (control["IgnoreSerializationTests"] != null)
                        TestControl.IgnoreSerializationTests = control["IgnoreSerializationTests"].InnerText == "true";
                    if (control["IgnoreValidationTests"] != null)
                        TestControl.IgnoreValidationTests = control["IgnoreValidationTests"].InnerText == "true";
                    if (control["IgnoreMiscTests"] != null)
                        TestControl.IgnoreMiscTests = control["IgnoreMiscTests"].InnerText == "true";
                }
            }
        }

        public static void PrintSummary()
        {
            Console.WriteLine("********************** Test Settings *************************");
            Console.WriteLine("LocalNativeConfigFile                = {0}", TestEnvironment.LocalNativeConfigFile);
            Console.WriteLine("LocalNativeUsername                  = {0}", TestEnvironment.LocalNativeUsername);
            Console.WriteLine("LocalNativePassword                  = {0}", TestEnvironment.LocalNativePassword);
            Console.WriteLine("LocalConfigFile                      = {0}", TestEnvironment.LocalConfigFile);
            Console.WriteLine("HttpUrl                              = {0}", TestEnvironment.HttpUrl);
            Console.WriteLine("HttpUsername                         = {0}", TestEnvironment.HttpUsername);
            Console.WriteLine("HttpPassword                         = {0}", TestEnvironment.HttpPassword);
            Console.WriteLine("GeoRestUrl                           = {0}", TestEnvironment.GeoRestUrl);
            Console.WriteLine("GeoRestConfig                        = {0}", TestEnvironment.GeoRestConfig);
            Console.WriteLine("********************** Test Control **************************");
            Console.WriteLine("IgnoreConfigurationTests             = {0}", TestControl.IgnoreConfigurationTests);
            Console.WriteLine("IgnoreCapabilityTests                = {0}", TestControl.IgnoreCapabilityTests);
            Console.WriteLine("IgnoreExpressionTests                = {0}", TestControl.IgnoreExpressionTests);
            Console.WriteLine("IgnoreFeatureReaderTests             = {0}", TestControl.IgnoreFeatureReaderTests);
            Console.WriteLine("IgnoreHttpConnectionTests            = {0}", TestControl.IgnoreHttpConnectionTests);
            Console.WriteLine("IgnoreHttpSiteTests                  = {0}", TestControl.IgnoreHttpSiteTests);
            Console.WriteLine("IgnoreHttpRuntimeMapTests            = {0}", TestControl.IgnoreHttpRuntimeMapTests);
            Console.WriteLine("IgnoreObjectTests                    = {0}", TestControl.IgnoreObjectTests);
            Console.WriteLine("IgnoreResourceTests                  = {0}", TestControl.IgnoreResourceTests);
            Console.WriteLine("IgnoreLocalRuntimeMapTests           = {0}", TestControl.IgnoreLocalRuntimeMapTests);
            Console.WriteLine("IgnoreLocalNativeRuntimeMapTests     = {0}", TestControl.IgnoreLocalNativeRuntimeMapTests);
            Console.WriteLine("IgnoreLocalNativePerformanceTests    = {0}", TestControl.IgnoreLocalNativePerformanceTests);
            Console.WriteLine("IgnoreLocalNativeFeatureTests        = {0}", TestControl.IgnoreLocalNativeFeatureTests);
            Console.WriteLine("IgnoreGeoRestTests                   = {0}", TestControl.IgnoreGeoRestTests);
            Console.WriteLine("IgnoreLocalFeatureTests              = {0}", TestControl.IgnoreLocalFeatureTests);
            Console.WriteLine("IgnoreSchemaTests                    = {0}", TestControl.IgnoreSchemaTests);
            Console.WriteLine("IgnoreSerializationTests             = {0}", TestControl.IgnoreSerializationTests);
            Console.WriteLine("IgnoreValidationTests                = {0}", TestControl.IgnoreValidationTests);
            Console.WriteLine("IgnoreMiscTests                      = {0}", TestControl.IgnoreMiscTests);
        }
    }

    internal class TestControl
    {
        public static bool IgnoreConfigurationTests = false;
        public static bool IgnoreCapabilityTests = false;
        public static bool IgnoreExpressionTests = false;
        public static bool IgnoreFeatureReaderTests = false;
        public static bool IgnoreHttpConnectionTests = false;
        public static bool IgnoreHttpSiteTests = false;
        public static bool IgnoreObjectTests = false;
        public static bool IgnoreResourceTests = false;
        public static bool IgnoreHttpRuntimeMapTests = false;
        public static bool IgnoreLocalRuntimeMapTests = false;
        public static bool IgnoreLocalNativeRuntimeMapTests = true;
        public static bool IgnoreLocalNativePerformanceTests = true;
        public static bool IgnoreLocalNativeFeatureTests = true;
        public static bool IgnoreGeoRestTests = true;
        public static bool IgnoreLocalFeatureTests = false;
        public static bool IgnoreSchemaTests = false;
        public static bool IgnoreSerializationTests = false;
        public static bool IgnoreValidationTests = false;
        public static bool IgnoreMiscTests = false;
    }

    internal class ConnectionUtil
    {
        public static IServerConnection CreateTestLocalNativeConnection()
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.LocalNative",
                    "ConfigFile", TestEnvironment.LocalNativeConfigFile,
                    "Username", TestEnvironment.LocalNativeUsername,
                    "Password", TestEnvironment.LocalNativePassword);
        }

        public static IServerConnection CreateTestLocalConnection()
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.Local", "ConfigFile", TestEnvironment.LocalConfigFile);
        }

        public static IServerConnection CreateTestHttpConnectionWithGeoRest()
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.Http",
                "Url", TestEnvironment.HttpUrl,
                "Username", TestEnvironment.HttpUsername,
                "Password", TestEnvironment.HttpPassword,
                "GeoRestUrl", TestEnvironment.GeoRestUrl,
                "GeoRestConfigPath", TestEnvironment.GeoRestConfig);
        }

        public static IServerConnection CreateTestHttpConnection()
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.Http",
                "Url", TestEnvironment.HttpUrl,
                "Username", TestEnvironment.HttpUsername,
                "Password", TestEnvironment.HttpPassword);
        }
    }
}
