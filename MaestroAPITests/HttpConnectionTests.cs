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

namespace MaestroAPITests
{
    [TestFixture]
    public class HttpConnectionTests
    {
        [Test]
        public void TestConnectionString()
        {
            System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
            builder["Foo"] = "sdfjkg";
            builder["Bar"] = "skgjuksdf";
            builder["Snafu"] = "asjdgjh;sdgj"; //Note the ; in the value
            builder["Whatever"] = "asjd=gjh;sdgj"; //Note the ; and = in the value

            var values = ConnectionProviderRegistry.ParseConnectionString(builder.ToString());
            Assert.AreEqual(values.Count, 4);

            Assert.AreEqual(builder["Foo"].ToString(), values["Foo"]);
            Assert.AreEqual(builder["Bar"].ToString(), values["Bar"]);
            Assert.AreEqual(builder["Snafu"].ToString(), values["Snafu"]);
            Assert.AreEqual(builder["Whatever"].ToString(), values["Whatever"]);
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
