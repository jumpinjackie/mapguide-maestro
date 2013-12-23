#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.IO;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.SchemaOverrides;

namespace MaestroAPITests
{
    [TestFixture]
    public class HttpConnectionTests : ConnectionTestBase
    {
        protected override bool ShouldIgnore(out string reason)
        {
            reason = string.Empty;
            if (TestControl.IgnoreHttpConnectionTests)
                reason = "Skipping HttpConnectionTests because TestControl.IgnoreHttpConnectionTests = true";

            return TestControl.IgnoreLocalNativeFeatureTests;
        }

        protected override string GetTestPrefix()
        {
            return "Http";
        }

        //[Test]
        public override void TestEncryptedFeatureSourceCredentials()
        {
            base.TestEncryptedFeatureSourceCredentials();
        }

        [Test]
        public void TestResourceExists()
        {
            base.TestResourceExists();
        }

        [Test]
        public void TestFeatureSourceCaching()
        {
            base.TestFeatureSourceCaching("HttpCaching");
        }

        [Test]
        public void TestClassDefinitionCaching()
        {
            base.TestClassDefinitionCaching("HttpCaching");
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
        public override void TestCreateFromExistingSession()
        {
            base.TestCreateFromExistingSession();
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

        protected override IServerConnection CreateTestConnection()
        {
            return ConnectionUtil.CreateTestHttpConnection();
        }

        public override IServerConnection CreateFromExistingSession(IServerConnection orig)
        {
            return ConnectionProviderRegistry.CreateConnection("Maestro.Http",
                HttpServerConnection.PARAM_URL, orig.GetCustomProperty(HttpServerConnection.PROP_BASE_URL).ToString(),
                HttpServerConnection.PARAM_SESSION, orig.SessionID);
        }

        [Test]
        public void TestSheboyganCsToPseudoMercator()
        {
            //Purpose: Unit test to guard against regression as a result of updating/replacing NTS

            var conn = CreateTestConnection();
            var srcWkt = "GEOGCS[\"WGS84 Lat/Long's, Degrees, -180 ==> +180\",DATUM[\"D_WGS_1984\",SPHEROID[\"World_Geodetic_System_of_1984\",6378137,298.257222932867]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]]";
            var dstCs = conn.CoordinateSystemCatalog.FindCoordSys("WGS84.PseudoMercator");
            var dstWkt = dstCs.WKT;

            var trans = conn.CoordinateSystemCatalog.CreateTransform(srcWkt, dstWkt);

            double tx1;
            double ty1;
            trans.Transform(-87.7649869909628, 43.6913981287878, out tx1, out ty1);
            Assert.AreEqual(-9769953.66131227, tx1, 0.0000001);
            Assert.AreEqual(5417808.88017179, ty1, 0.0000001);

            double tx2;
            double ty2;

            trans.Transform(-87.6955215108997, 43.7975200004803, out tx2, out ty2);
            Assert.AreEqual(-9762220.79944393, tx2, 0.0000001);
            Assert.AreEqual(5434161.22418638, ty2, 0.0000001);
        }

        [Test]
        public void TestCustomProperties()
        {
            var builder = new RequestBuilder(new Uri("http://tempuri.org"), "en");
            var conn = new HttpServerConnection(builder);

            //Work through the interface 
            IServerConnection isvc = (IServerConnection)conn;

            //UserAgent is exposed as a custom property
            var props = isvc.GetCustomPropertyNames();

            Assert.IsNotNull(props);
            Assert.AreEqual(props.Length, 2);
            Assert.IsTrue(Array.IndexOf<string>(props, HttpServerConnection.PROP_USER_AGENT) >= 0);
            Assert.IsTrue(Array.IndexOf<string>(props, HttpServerConnection.PROP_BASE_URL) >= 0);

            //It is of type string
            var type = isvc.GetCustomPropertyType(HttpServerConnection.PROP_USER_AGENT);
            Assert.AreEqual(type, typeof(string));
            type = isvc.GetCustomPropertyType(HttpServerConnection.PROP_BASE_URL);
            Assert.AreEqual(type, typeof(string));

            //We can set and get it
            isvc.SetCustomProperty(HttpServerConnection.PROP_USER_AGENT, "MapGuide Maestro API Unit Test Fixture");
            var agent = (string)isvc.GetCustomProperty(HttpServerConnection.PROP_USER_AGENT);
            Assert.AreEqual(agent, "MapGuide Maestro API Unit Test Fixture");

            //BaseUrl is read-only
            try
            {
                isvc.SetCustomProperty(HttpServerConnection.PROP_BASE_URL, "http://mylocalhost/mapguide");
                Assert.Fail("Should've thrown exception");
            }
            catch { }
        }

        [Test]
        public void TestServiceCapabilities()
        {
            var builder = new RequestBuilder(new Uri("http://tempuri.org"), "en");
            var conn = new HttpServerConnection(builder);
            conn.SetSiteVersion(new Version(1, 2, 0));

            //Work through the interface 
            IServerConnection isvc = (IServerConnection)conn;
            int[] stypes = isvc.Capabilities.SupportedServices;
            foreach (int st in stypes)
            {
                try
                {
                    IService svc = isvc.GetService(st);
                }
                catch
                {
                    Assert.Fail("Supported service type mismatch");
                }
            }

            conn.SetSiteVersion(new Version(2, 0, 0));

            stypes = isvc.Capabilities.SupportedServices;
            foreach (int st in stypes)
            {
                try
                {
                    IService svc = isvc.GetService(st);
                }
                catch
                {
                    Assert.Fail("Supported service type mismatch");
                }
            }
        }
    }
}
