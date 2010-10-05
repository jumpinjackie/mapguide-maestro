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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;
using System.Globalization;

namespace OSGeo.MapGuide.Maestro
{
    public partial class ServerStatusMonitor : Form
    {
        private ServerStatusMonitor()
        {
            InitializeComponent();
        }

        private void ServerStatusMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            ServerStatusMonitor.HideWindow();
        }

        private void StartTimer() { pollTimer.Start(); }

        private void StopTimer() { pollTimer.Stop(); }

        public static void Init(ServerConnectionI conn)
        {
            _smConn = conn;

            if (_smMonitor != null)
                _smMonitor.Dispose();

            _smMonitor = new ServerStatusMonitor();
        }

        private static ServerConnectionI _smConn;

        private static ServerStatusMonitor _smMonitor;

        public static void ShowWindow()
        {
            _smMonitor.DoPoll();
            _smMonitor.StartTimer();
            _smMonitor.Show();
        }

        public static void HideWindow()
        {
            _smMonitor.StopTimer();
            _smMonitor.Hide();
        }

        private void pollTimer_Tick(object sender, EventArgs e)
        {
            DoPoll();
        }

        private static string ParseMs(string value)
        {
            return value + " " + Strings.FormMain.UnitsMs;
        }

        private static string ParseSeconds(string value)
        {
            return value + " " + Strings.FormMain.UnitsSeconds;
        }

        private static string ParseKb(string valueBytes)
        {
            double d;
            if (double.TryParse(valueBytes, 
                                NumberStyles.AllowThousands, 
                                System.Threading.Thread.CurrentThread.CurrentUICulture, 
                                out d))
            {
                return (d / 1000.0).ToString(System.Threading.Thread.CurrentThread.CurrentUICulture) + " " + Strings.FormMain.UnitsKb;
            }
            return valueBytes;
        }

        private void DoPoll()
        {
            var info = _smConn.GetSiteInformation();

            lblActiveConnections.Text = info.Statistics.ActiveConnections;
            lblAdminQueueCount.Text = info.Statistics.AdminOperationsQueueCount;
            lblAvailPhysMem.Text = ParseKb(info.SiteServer.OperatingSystem.AvailablePhysicalMemory);
            lblAvgOpTime.Text = info.Statistics.AverageOperationTime;
            lblClientQueueCount.Text = info.Statistics.ClientOperationsQueueCount;
            lblCpuUtil.Text = info.Statistics.CpuUtilization;
            lblOsVersion.Text = info.SiteServer.OperatingSystem.Version;
            lblPhysMemTotal.Text = ParseKb(info.SiteServer.OperatingSystem.TotalPhysicalMemory);
            lblServerDisplayName.Text = info.SiteServer.DisplayName;
            lblServerStatus.Text = info.SiteServer.Status;
            lblServerVersion.Text = info.SiteServer.Version;
            lblSiteQueueCount.Text = info.Statistics.SiteOperationsQueueCount;
            lblTotalConnections.Text = info.Statistics.TotalConnections;
            lblTotalOpsProcessed.Text = info.Statistics.TotalOperationsProcessed;
            lblTotalOpsReceived.Text = info.Statistics.TotalOperationsReceived;
            lblTotalOpTime.Text = ParseSeconds(info.Statistics.TotalOperationTime);
            lblUptime.Text = ParseSeconds(info.Statistics.Uptime);
            lblVirtMemAvail.Text = ParseKb(info.SiteServer.OperatingSystem.AvailableVirtualMemory);
            lblVirtMemTotal.Text = ParseKb(info.SiteServer.OperatingSystem.TotalVirtualMemory);

            lblLastUpdated.Text = Strings.FormMain.LastUpdated + DateTime.Now.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture);
        }
    }
}
