#region Disclaimer / License

// Copyright (C) 2021, Jackie Ng
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

using Moq;
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.MaestroAPI.Http;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class HttpConnectionTests
    {
        [Fact]
        public void HttpConnection_CreateRuntimeMap()
        {
            var mockHttp = new Mock<IHttpRequestor>();

            var baseUrl = "http://localhost:8008/mapguide/mapagent/mapagent.fcgi";
            var mdfId = "Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition";
            var mapName = "Sheboygan";            
            var sessionId = "abcd1234";
            var requestedFeatures = 7;
            var userName = "Administrator";
            var password = "admin";

            var asmVer = typeof(HttpServerConnection).Assembly.GetName().Version.ToString();

            var createSessionUrl = $"{baseUrl}?OPERATION=CREATESESSION&VERSION=1.0.0&FORMAT=text%2Fxml&CLIENTAGENT=MapGuide%20Maestro%20API%20v{asmVer}&USERNAME={userName}&PASSWORD={password}";
            var createRuntimeMapUrl = $"{baseUrl}?OPERATION=CREATERUNTIMEMAP&VERSION=4.0.0&SESSION={sessionId}&MAPDEFINITION={Uri.EscapeDataString(mdfId)}&TARGETMAPNAME={mapName}&REQUESTEDFEATURES={requestedFeatures}&ICONSPERSCALERANGE=25&ICONFORMAT=PNG&ICONWIDTH=16&ICONHEIGHT=16";
            var getSiteInfoUrl = $"{baseUrl}?OPERATION=GETSITEINFO&VERSION=1.0.0&SESSION={sessionId}&FORMAT=text%2Fxml&CLIENTAGENT=MapGuide%20Maestro%20API%20v{asmVer}";

            mockHttp
                .Setup(h => h.Get(It.Is<string>(s => s == createSessionUrl), It.IsAny<IHttpGetRequestOptions>()))
                .Returns(() => new MemoryStream(Encoding.UTF8.GetBytes(sessionId)));

            mockHttp
                .Setup(h => h.Get(It.Is<string>(s => s == getSiteInfoUrl), It.IsAny<IHttpGetRequestOptions>()))
                .Returns(() => Utils.OpenFile($"Resources{System.IO.Path.DirectorySeparatorChar}GetSiteInfo.xml"));

            mockHttp
                .Setup(h => h.Get(It.Is<string>(s => s == createRuntimeMapUrl), It.IsAny<IHttpGetRequestOptions>()))
                .Returns(() => Utils.OpenFile($"Resources{System.IO.Path.DirectorySeparatorChar}CreateRuntimeMap.xml"));

            try
            {
                var conn = new HttpServerConnection(mockHttp.Object, new NameValueCollection
                {
                    { HttpServerConnectionParams.PARAM_URL, baseUrl },
                    { HttpServerConnectionParams.PARAM_USERNAME, userName },
                    { HttpServerConnectionParams.PARAM_PASSWORD, password }
                });

                Assert.Equal(sessionId, conn.SessionID);

                var cmd = conn.CreateCommand((int)CommandType.CreateRuntimeMap) as ICreateRuntimeMap;
                cmd.MapDefinition = mdfId;
                cmd.TargetMapName = mapName;
                cmd.RequestedFeatures = requestedFeatures;

                var rtm = cmd.Execute();
                Assert.NotNull(rtm);

                Assert.NotNull(rtm.Layers);
                Assert.Equal(7, rtm.Layers.Count);
                Assert.Equal(10, rtm.FiniteDisplayScales.Length);
            }
            catch
            {

            }

            mockHttp.Verify(h => h.Get(It.Is<string>(s => s == createSessionUrl), It.IsAny<IHttpGetRequestOptions>()), Times.Once);
            mockHttp.Verify(h => h.Get(It.Is<string>(s => s == getSiteInfoUrl), It.IsAny<IHttpGetRequestOptions>()), Times.Once);
            mockHttp.Verify(h => h.Get(It.Is<string>(s => s == createRuntimeMapUrl), It.IsAny<IHttpGetRequestOptions>()), Times.Once);
        }

        [Fact]
        public void HttpConnection_DescribeRuntimeMap()
        {
            var mockHttp = new Mock<IHttpRequestor>();

            var baseUrl = "http://localhost:8008/mapguide/mapagent/mapagent.fcgi";
            var mdfId = "Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition";
            var mapName = "Sheboygan";
            var sessionId = "abcd1234";
            var requestedFeatures = 7;
            var userName = "Administrator";
            var password = "admin";

            var asmVer = typeof(HttpServerConnection).Assembly.GetName().Version.ToString();

            var createSessionUrl = $"{baseUrl}?OPERATION=CREATESESSION&VERSION=1.0.0&FORMAT=text%2Fxml&CLIENTAGENT=MapGuide%20Maestro%20API%20v{asmVer}&USERNAME={userName}&PASSWORD={password}";
            var describeRuntimeMapUrl = $"{baseUrl}?OPERATION=DESCRIBERUNTIMEMAP&VERSION=4.0.0&SESSION={sessionId}&MAPNAME={mapName}&REQUESTEDFEATURES={requestedFeatures}&ICONSPERSCALERANGE=25&ICONFORMAT=PNG&ICONWIDTH=16&ICONHEIGHT=16";
            var getSiteInfoUrl = $"{baseUrl}?OPERATION=GETSITEINFO&VERSION=1.0.0&SESSION={sessionId}&FORMAT=text%2Fxml&CLIENTAGENT=MapGuide%20Maestro%20API%20v{asmVer}";

            mockHttp
                .Setup(h => h.Get(It.Is<string>(s => s == createSessionUrl), It.IsAny<IHttpGetRequestOptions>()))
                .Returns(() => new MemoryStream(Encoding.UTF8.GetBytes(sessionId)));

            mockHttp
                .Setup(h => h.Get(It.Is<string>(s => s == getSiteInfoUrl), It.IsAny<IHttpGetRequestOptions>()))
                .Returns(() => Utils.OpenFile($"Resources{System.IO.Path.DirectorySeparatorChar}GetSiteInfo.xml"));

            mockHttp
                .Setup(h => h.Get(It.Is<string>(s => s == describeRuntimeMapUrl), It.IsAny<IHttpGetRequestOptions>()))
                .Returns(() => Utils.OpenFile($"Resources{System.IO.Path.DirectorySeparatorChar}DescribeRuntimeMap.xml"));

            try
            {
                var conn = new HttpServerConnection(mockHttp.Object, new NameValueCollection
                {
                    { HttpServerConnectionParams.PARAM_URL, baseUrl },
                    { HttpServerConnectionParams.PARAM_USERNAME, userName },
                    { HttpServerConnectionParams.PARAM_PASSWORD, password }
                });

                Assert.Equal(sessionId, conn.SessionID);

                var cmd = conn.CreateCommand((int)CommandType.DescribeRuntimeMap) as IDescribeRuntimeMap;
                cmd.Name = mapName;
                cmd.RequestedFeatures = requestedFeatures;

                var rtm = cmd.Execute();
                Assert.NotNull(rtm);

                Assert.NotNull(rtm.Layers);
                Assert.Equal(7, rtm.Layers.Count);
                Assert.Equal(10, rtm.FiniteDisplayScales.Length);
            }
            catch
            {

            }

            mockHttp.Verify(h => h.Get(It.Is<string>(s => s == createSessionUrl), It.IsAny<IHttpGetRequestOptions>()), Times.Once);
            mockHttp.Verify(h => h.Get(It.Is<string>(s => s == getSiteInfoUrl), It.IsAny<IHttpGetRequestOptions>()), Times.Once);
            mockHttp.Verify(h => h.Get(It.Is<string>(s => s == describeRuntimeMapUrl), It.IsAny<IHttpGetRequestOptions>()), Times.Once);
        }
    }
}
