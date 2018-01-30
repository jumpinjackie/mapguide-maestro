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
using OSGeo.MapGuide.ObjectModels.Common;
using System;
using System.Collections.Generic;
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

        internal static void Initialize(string initFile)
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
                }
                var control = doc.SelectSingleNode("//TestConfiguration/TestControl");
                if (control != null)
                {
                    if (control["IgnoreExpressionTests"] != null)
                        TestControl.IgnoreExpressionTests = control["IgnoreExpressionTests"].InnerText == "true";
                    if (control["IgnoreFeatureReaderTests"] != null)
                        TestControl.IgnoreFeatureReaderTests = control["IgnoreFeatureReaderTests"].InnerText == "true";
                    if (control["IgnoreHttpConnectionTests"] != null)
                        TestControl.IgnoreHttpConnectionTests = control["IgnoreHttpConnectionTests"].InnerText == "true";
                    if (control["IgnoreHttpSiteTests"] != null)
                        TestControl.IgnoreHttpSiteTests = control["IgnoreHttpSiteTests"].InnerText == "true";
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
                    if (control["IgnoreLocalFeatureTests"] != null)
                        TestControl.IgnoreLocalFeatureTests = control["IgnoreLocalFeatureTests"].InnerText == "true";
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
            Console.WriteLine("********************** Test Control **************************");
            Console.WriteLine("IgnoreExpressionTests                = {0}", TestControl.IgnoreExpressionTests);
            Console.WriteLine("IgnoreFeatureReaderTests             = {0}", TestControl.IgnoreFeatureReaderTests);
            Console.WriteLine("IgnoreHttpConnectionTests            = {0}", TestControl.IgnoreHttpConnectionTests);
            Console.WriteLine("IgnoreHttpSiteTests                  = {0}", TestControl.IgnoreHttpSiteTests);
            Console.WriteLine("IgnoreHttpRuntimeMapTests            = {0}", TestControl.IgnoreHttpRuntimeMapTests);
            Console.WriteLine("IgnoreLocalRuntimeMapTests           = {0}", TestControl.IgnoreLocalRuntimeMapTests);
            Console.WriteLine("IgnoreLocalNativeRuntimeMapTests     = {0}", TestControl.IgnoreLocalNativeRuntimeMapTests);
            Console.WriteLine("IgnoreLocalNativePerformanceTests    = {0}", TestControl.IgnoreLocalNativePerformanceTests);
            Console.WriteLine("IgnoreLocalNativeFeatureTests        = {0}", TestControl.IgnoreLocalNativeFeatureTests);
            Console.WriteLine("IgnoreLocalFeatureTests              = {0}", TestControl.IgnoreLocalFeatureTests);
        }

        static readonly object initLock = new object();
        static HashSet<string> initProviders = new HashSet<string>();

        public static void SetupTestData(string provider, Func<IServerConnection> connFactory)
        {
            lock (initLock)
            {
                if (initProviders.Contains(provider))
                    return;

                var conn = connFactory();
                var resSvc = conn.ResourceService;

                resSvc.DeleteResource("Library://UnitTests/");

                resSvc.SetResourceXmlData("Library://UnitTests/Maps/Sheboygan.MapDefinition", File.OpenRead("TestData/MappingService/UT_Sheboygan.mdf"));
                resSvc.SetResourceXmlData("Library://UnitTests/Maps/SheboyganTiled.MapDefinition", File.OpenRead("UserTestData/TestTiledMap.xml"));
                resSvc.SetResourceXmlData("Library://UnitTests/Maps/DuplicateLayerIds.MapDefinition", File.OpenRead("UserTestData/TestDuplicateLayerIds.xml"));

                resSvc.SetResourceXmlData("Library://UnitTests/Layers/HydrographicPolygons.LayerDefinition", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.ldf"));
                resSvc.SetResourceXmlData("Library://UnitTests/Layers/Rail.LayerDefinition", File.OpenRead("TestData/MappingService/UT_Rail.ldf"));
                resSvc.SetResourceXmlData("Library://UnitTests/Layers/Parcels.LayerDefinition", File.OpenRead("TestData/TileService/UT_Parcels.ldf"));

                resSvc.SetResourceXmlData("Library://UnitTests/Data/HydrographicPolygons.FeatureSource", File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.fs"));
                resSvc.SetResourceXmlData("Library://UnitTests/Data/Rail.FeatureSource", File.OpenRead("TestData/MappingService/UT_Rail.fs"));
                resSvc.SetResourceXmlData("Library://UnitTests/Data/Parcels.FeatureSource", File.OpenRead("TestData/TileService/UT_Parcels.fs"));

                resSvc.SetResourceData("Library://UnitTests/Data/HydrographicPolygons.FeatureSource", "UT_HydrographicPolygons.sdf", ResourceDataType.File, File.OpenRead("TestData/MappingService/UT_HydrographicPolygons.sdf"));
                resSvc.SetResourceData("Library://UnitTests/Data/Rail.FeatureSource", "UT_Rail.sdf", ResourceDataType.File, File.OpenRead("TestData/MappingService/UT_Rail.sdf"));
                resSvc.SetResourceData("Library://UnitTests/Data/Parcels.FeatureSource", "UT_Parcels.sdf", ResourceDataType.File, File.OpenRead("TestData/FeatureService/SDF/Sheboygan_Parcels.sdf"));

                resSvc.SetResourceXmlData("Library://UnitTests/Data/SpaceShip.DrawingSource", File.OpenRead("TestData/DrawingService/SpaceShipDrawingSource.xml"));
                resSvc.SetResourceData("Library://UnitTests/Data/SpaceShip.DrawingSource", "SpaceShip.dwf", ResourceDataType.File, File.OpenRead("TestData/DrawingService/SpaceShip.dwf"));

                if (conn.SiteVersion >= new Version(3, 0))
                {
                    resSvc.SetResourceXmlData("Library://UnitTests/Data/RoadCenterLines.FeatureSource", File.OpenRead("TestData/TileService/UT_RoadCenterLines.fs"));
                    resSvc.SetResourceData("Library://UnitTests/Data/RoadCenterLines.FeatureSource", "RoadCenterLines.sdf", ResourceDataType.File, File.OpenRead("TestData/TileService/UT_RoadCenterLines.sdf"));

                    resSvc.SetResourceXmlData("Library://UnitTests/Layers/RoadCenterLines.LayerDefinition", File.OpenRead("TestData/TileService/UT_RoadCenterLines.ldf"));

                    resSvc.SetResourceXmlData("Library://UnitTests/Data/VotingDistricts.FeatureSource", File.OpenRead("TestData/TileService/UT_VotingDistricts.fs"));
                    resSvc.SetResourceData("Library://UnitTests/Data/VotingDistricts.FeatureSource", "VotingDistricts.sdf", ResourceDataType.File, File.OpenRead("TestData/TileService/UT_VotingDistricts.sdf"));

                    resSvc.SetResourceXmlData("Library://UnitTests/Layers/VotingDistricts.LayerDefinition", File.OpenRead("TestData/TileService/UT_VotingDistricts.ldf"));

                    resSvc.SetResourceXmlData("Library://UnitTests/TileSets/Sheboygan.TileSetDefinition", File.OpenRead("TestData/TileService/UT_BaseMap.tsd"));
                    resSvc.SetResourceXmlData("Library://UnitTests/Maps/SheboyganLinked.MapDefinition", File.OpenRead("TestData/TileService/UT_LinkedTileSet.mdf"));
                }

                initProviders.Add(provider);
            }
        }
    }

    internal static class TestControl
    {
        public static bool IgnoreExpressionTests = false;
        public static bool IgnoreFeatureReaderTests = false;
        public static bool IgnoreHttpConnectionTests = false;
        public static bool IgnoreHttpSiteTests = false;
        public static bool IgnoreHttpRuntimeMapTests = false;
        public static bool IgnoreLocalRuntimeMapTests = false;
        public static bool IgnoreLocalNativeRuntimeMapTests = true;
        public static bool IgnoreLocalNativePerformanceTests = true;
        public static bool IgnoreLocalNativeFeatureTests = true;
        public static bool IgnoreLocalFeatureTests = false;

        static TestControl()
        {
            TestEnvironment.Initialize("TestMaestroAPI.xml");
            TestEnvironment.PrintSummary();
            ResourceValidatorLoader.LoadStockValidators();
        }
    }

    internal static class ConnectionUtil
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

        public static IServerConnection CreateTestHttpConnection()
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.Http",
                "Url", TestEnvironment.HttpUrl,
                "Username", TestEnvironment.HttpUsername,
                "Password", TestEnvironment.HttpPassword);
        }
    }
}