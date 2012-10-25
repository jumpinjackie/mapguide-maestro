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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Maestro.Shared.UI;
using System.IO;
using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Base.UI.Preferences;

namespace Maestro.Base.UI
{
    internal partial class OutboundRequestViewer : SingletonViewContent
    {
        public OutboundRequestViewer()
        {
            InitializeComponent();
            this.Title = this.Description = Strings.Content_OutboundRequests;

            //This is okay because changing the value requires a restart, so it will either be listening
            //or not and that will remain in effect for the duration of the application running.
            if (PropertyService.Get(ConfigProperties.ShowOutboundRequests, ConfigProperties.DefaultShowOutboundRequests))
            {
                var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
                connMgr.ConnectionAdded += (s, args) =>
                {
                    var conn = connMgr.GetConnection(args);
                    conn.RequestDispatched += OnRequestDispatched;
                };
                connMgr.ConnectionRemoving += (s, ce) =>
                {
                    var conn = connMgr.GetConnection(ce.ConnectionName);
                    conn.RequestDispatched -= OnRequestDispatched;
                };
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        void OnRequestDispatched(object sender, OSGeo.MapGuide.MaestroAPI.RequestEventArgs e)
        {
            string msg = string.Format("[{0}]: {1}", DateTime.Now.ToString("dd MMM yyyy hh:mm:ss"), e.Data); //NOXLATE

            if (!txtMessages.IsDisposed)
            {
                if (txtMessages.InvokeRequired)
                {
                    txtMessages.Invoke(new MethodInvoker(() =>
                    {
                        txtMessages.AppendText(msg + Environment.NewLine);
                        txtMessages.ScrollToCaret();
                    }));
                }
                else
                {
                    txtMessages.AppendText(msg + Environment.NewLine);
                    txtMessages.ScrollToCaret();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMessages.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var save = DialogFactory.SaveFile())
            {
                save.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickLog, "log"); //NOXLATE
                if (save.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(save.FileName, txtMessages.Text);
                    MessageService.ShowMessage(string.Format(Strings.Log_Saved, save.FileName));
                }
            }
        }

        public override ViewRegion DefaultRegion
        {
            get
            {
                return ViewRegion.Bottom;
            }
        }
    }
}
