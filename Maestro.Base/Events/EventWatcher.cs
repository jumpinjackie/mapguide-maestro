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
using Maestro.Base.Services;
using System.Diagnostics;
using Maestro.Base.UI;
using Maestro.Editors.Diagnostics;
using OSGeo.MapGuide.MaestroAPI.Services;
using ICSharpCode.Core;
using System.Windows.Forms;

namespace Maestro.Base.Events
{
    public static class EventWatcher
    {
        private static Timer _keepAliveTimer;

        internal static void Initialize()
        {
            _keepAliveTimer = new Timer();
            _keepAliveTimer.Interval = 60000;
            _keepAliveTimer.Tick += OnKeepAliveTimerElapsed;
            var svc = ServiceRegistry.GetService<ServerConnectionManager>();
            Debug.Assert(svc != null);

            svc.ConnectionAdded += new ServerConnectionEventHandler(OnConnectionAdded);
            svc.ConnectionRemoved += new ServerConnectionEventHandler(OnConnectionRemoved);

            LoggingService.Info("Starting session keep-alive timer");
            _keepAliveTimer.Start();
        }

        static void OnKeepAliveTimerElapsed(object sender, EventArgs e)
        {
            var svc = ServiceRegistry.GetService<ServerConnectionManager>();
            foreach (var name in svc.GetConnectionNames())
            {
                var conn = svc.GetConnection(name);
                string sessionId = conn.SessionID;
                if (!string.IsNullOrEmpty(sessionId))
                {
                    try
                    {
                        conn.FeatureService.GetProviderCapabilities("OSGeo.SDF");
                        LoggingService.Info("Session kept alive: " + sessionId);
                    }
                    catch (Exception ex)
                    {
                        MessageService.ShowError(Properties.Resources.KeepAliveFailed);
                    }
                }
            }
        }

        static void OnConnectionRemoved(object sender, string name)
        {
            Workbench wb = Workbench.Instance;
            Debug.Assert(wb.ActiveSiteExplorer != null);
            Debug.Assert(wb.ActiveSiteExplorer.ConnectionName == name);

            wb.ActiveSiteExplorer = null;

            //TODO: Review this API design when we do decide to support multiple
            //site connections
            ServerStatusMonitor.Init(null);
        }

        static void OnConnectionAdded(object sender, string name)
        {
            var exp = new SiteExplorer(name);
            var wb = Workbench.Instance;
            wb.ShowContent(exp);

            var svc = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = svc.GetConnection(name);
            
            ISiteService siteSvc = null;
            var svcTypes = conn.Capabilities.SupportedServices;
            if (Array.IndexOf(svcTypes, (int)ServiceType.Site) >= 0)
            {
                siteSvc = (ISiteService)conn.GetService((int)ServiceType.Site);
            }
            //TODO: Review this API design when we do decide to support multiple
            //site connections
            ServerStatusMonitor.Init(siteSvc);
        }
    }
}
