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
using Maestro.Shared.UI;
using Maestro.Base.UI.Preferences;
using Maestro.Editors.Preview;
using Maestro.Editors.LayerDefinition;

namespace Maestro.Base.Events
{
    internal static class EventWatcher
    {
        private static Timer _keepAliveTimer;

        internal static void Initialize()
        {
            _keepAliveTimer = new Timer();
            _keepAliveTimer.Interval = 60000;
            _keepAliveTimer.Tick += OnKeepAliveTimerElapsed;
            var svc = ServiceRegistry.GetService<ServerConnectionManager>();
            Debug.Assert(svc != null);
            PropertyService.PropertyChanged += OnAppPropertyChanged;

            svc.ConnectionAdded += new ServerConnectionEventHandler(OnConnectionAdded);
            svc.ConnectionRemoved += new ServerConnectionEventHandler(OnConnectionRemoved);

            LoggingService.Info("Starting session keep-alive timer"); //NOXLATE
            _keepAliveTimer.Start();
        }

        private static void OnAppPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.Key)
            {
                case ConfigProperties.UseLocalPreview:
                    PreviewSettings.UseLocalPreview = Convert.ToBoolean(e.NewValue);
                    LoggingService.Info("Use Local Preview setting is now: " + PreviewSettings.UseLocalPreview); //NOXLATE
                    break;
                case ConfigProperties.AddDebugWatermark:
                    PreviewSettings.AddDebugWatermark = Convert.ToBoolean(e.NewValue);
                    LoggingService.Info("Add Debug Watermark setting is now: " + PreviewSettings.AddDebugWatermark); //NOXLATE
                    break;
                case ConfigProperties.PreviewViewerType:
                    PreviewSettings.UseAjaxViewer = (e.NewValue.ToString() == "AJAX"); //NOXLATE
                    LoggingService.Info("Use AJAX Viewer setting is now: " + PreviewSettings.UseAjaxViewer);
                    break;
                case ConfigProperties.UseGridStyleEditor:
                    LayerEditorSettings.UseGridEditor = Convert.ToBoolean(e.NewValue);
                    LoggingService.Info("Use Grid Style Editor is now: " + LayerEditorSettings.UseGridEditor);
                    break;
            }
        }

        static bool smShowingError = false;

        static void OnKeepAliveTimerElapsed(object sender, EventArgs e)
        {
            var svc = ServiceRegistry.GetService<ServerConnectionManager>();
            foreach (var name in svc.GetConnectionNames())
            {
                var conn = svc.GetConnection(name);
                if (conn.ProviderName.ToUpper().Equals("MAESTRO.LOCAL")) //NOXLATE
                    continue;

                string sessionId = conn.SessionID;
                if (!string.IsNullOrEmpty(sessionId))
                {
                    try
                    {
                        conn.FeatureService.GetProviderCapabilities("OSGeo.SDF"); //NOXLATE
                        LoggingService.Info("Session kept alive: " + sessionId); //NOXLATE
                    }
                    catch (Exception)
                    {
                        if (smShowingError)
                            return;

                        try
                        {
                            smShowingError = true;
                            MessageService.ShowError(string.Format(Strings.KeepAliveFailed, sessionId));
                        }
                        finally
                        {
                            smShowingError = false;
                        }
                    }
                }
            }
        }

        static void OnConnectionRemoved(object sender, string name)
        {
            Workbench wb = Workbench.Instance;
            Debug.Assert(wb.ActiveSiteExplorer != null);
            var svc = ServiceRegistry.GetService<ServerConnectionManager>();
            LoggingService.Info("There are now " + svc.GetConnectionNames().Count + " active connections");  //NOXLATE
            //Debug.Assert(wb.ActiveSiteExplorer.ConnectionName == name);
        }

        static void OnConnectionAdded(object sender, string name)
        {
            var wb = Workbench.Instance;
            if (wb.ActiveSiteExplorer == null)
            {
                var viewMgr = ServiceRegistry.GetService<ViewContentManager>();

                wb.ActiveSiteExplorer = viewMgr.OpenContent(Strings.Content_SiteExplorer, 
                                        Strings.Content_SiteExplorer, 
                                        ViewRegion.Left, 
                                        () => { return new SiteExplorer(); });

                wb.ActiveSiteExplorer.ActiveConnectionChanged += new EventHandler(OnActiveConnectionChanged);

                var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
                var openMgr = ServiceRegistry.GetService<OpenResourceManager>();
                var siteExp = wb.ActiveSiteExplorer;

                var nav = new ResourceIdNavigator(connMgr, openMgr, viewMgr, siteExp);
                wb.AddToolbar("Resource ID Bar", nav.NavigatorToolStrip, ToolbarRegion.Top, true);
            }

            wb.ActiveSiteExplorer.RefreshModel(name);
            

            var svc = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = svc.GetConnection(name);

            LoggingService.Info("There are now " + svc.GetConnectionNames().Count + " active connections"); //NOXLATE
        }

        static void OnActiveConnectionChanged(object sender, EventArgs e)
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            if (exp != null)
            {
                wb.SetStatusLabel(string.Format(Strings.StatusActiveConnection, exp.ConnectionName));
            }
            else
            {
                wb.SetStatusLabel(string.Empty);
            }
        }
    }
}
