#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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

namespace OSGeo.MapGuide.Maestro
{
    public partial class ApplicationSettings : Form
    {
        private PreferedSiteList m_settings;
        private bool m_isUpdating = false;

        public ApplicationSettings()
        {
            InitializeComponent();

            this.Icon = FormMain.MaestroIcon;
            m_settings = (PreferedSiteList)Utility.XmlDeepCopy(Program.ApplicationSettings);

            try
            {
                m_isUpdating = true;
                BrowserCommand.Text = m_settings.SystemBrowser;
                RegularViewer.Checked = !m_settings.UseFusionPreview;
                FusionViewer.Checked = m_settings.UseFusionPreview;

                Connections.Items.Clear();
                if (m_settings.Sites != null)
                    foreach (PreferedSite pfs in m_settings.Sites)
                        Connections.Items.Add(pfs.SiteURL);
            }
            finally
            {
                m_isUpdating = false;
            }

        }

        private void BrowseForBrowser_Click(object sender, EventArgs e)
        {
            if (SelectBrowser.ShowDialog(this) == DialogResult.OK)
                BrowserCommand.Text = SelectBrowser.FileName;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Program.ApplicationSettings = m_settings;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BrowserCommand_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;

            m_settings.SystemBrowser = BrowserCommand.Text;
        }

        private void RegularViewer_CheckedChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;

            m_settings.UseFusionPreview = !RegularViewer.Checked;
        }

        private void FusionViewer_CheckedChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;

            m_settings.UseFusionPreview = !RegularViewer.Checked;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;

            List<PreferedSite> lst = new List<PreferedSite>();
            for (int i = 0; i < Connections.Items.Count; i++)
                if (!Connections.SelectedIndices.Contains(i))
                    lst.Add(m_settings.Sites[i]);

            m_settings.Sites = lst.ToArray();

            while (Connections.SelectedIndices.Count > 0)
                Connections.Items.RemoveAt(Connections.SelectedIndices[0]);
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            removeToolStripMenuItem.Enabled = Connections.SelectedIndices.Count != 0;
        }

        private void Connections_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                removeToolStripMenuItem_Click(null, null);
            }
        }
    }
}