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
using Props = ICSharpCode.Core.PropertyService;
using ICSharpCode.Core;
using System.IO;

namespace Maestro.Base.UI.Preferences
{
    public partial class GeneralPreferencesCtrl : UserControl, IPreferenceSheet
    {
        public GeneralPreferencesCtrl()
        {
            InitializeComponent();
            cmbModifiedColor.ResetColors();
            cmbOpenedColor.ResetColors();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var viewer = Props.Get(ConfigProperties.PreviewViewerType, "AJAX");
            if (viewer.Equals("AJAX"))
                rdAjax.Checked = true;
            else
                rdFusion.Checked = true;

            var path = Props.Get(ConfigProperties.UserTemplatesDirectory, Path.Combine(FileUtility.ApplicationRootPath, "UserTemplates"));
            txtTemplatePath.Text = path;
            var msg = Props.Get(ConfigProperties.ShowMessages, true);
            chkMessages.Checked = msg;
            var outb = Props.Get(ConfigProperties.ShowOutboundRequests, true);
            chkOutbound.Checked = outb;

            txtFsPreview.Text = Props.Get(ConfigProperties.LocalFsPreviewPath, "");
            txtMgCooker.Text = Props.Get(ConfigProperties.MgCookerPath, "");

            cmbOpenedColor.CurrentColor = Props.Get(ConfigProperties.OpenColor, Color.LightGreen);
            cmbModifiedColor.CurrentColor = Props.Get(ConfigProperties.DirtyColor, Color.Pink);
        } 

        public string Title
        {
            get { return Properties.Resources.Prefs_General; }
        }

        public Control ContentControl
        {
            get { return this; }
        }

        private bool Apply(string key, object newValue)
        {
            if (Props.Get(key).Equals(newValue))
                return false;

            Props.Set(key, newValue);
            return true;
        }

        public bool ApplyChanges()
        {
            bool restart = false;

            //These changes can be applied without restart
            if (rdFusion.Checked)
                Apply(ConfigProperties.PreviewViewerType, "FUSION");
            else
                Apply(ConfigProperties.PreviewViewerType, "AJAX");

            Apply(ConfigProperties.UserTemplatesDirectory, txtTemplatePath.Text);
            Apply(ConfigProperties.MgCookerPath, txtMgCooker.Text);
            Apply(ConfigProperties.LocalFsPreviewPath, txtFsPreview.Text);
            Apply(ConfigProperties.OpenColor, (Color)cmbOpenedColor.CurrentColor);
            Apply(ConfigProperties.DirtyColor, (Color)cmbModifiedColor.CurrentColor);

            //These changes require restart
            if (Apply(ConfigProperties.ShowMessages, chkMessages.Checked ? "True" : "False"))
                restart = true;

            if (Apply(ConfigProperties.ShowOutboundRequests, chkOutbound.Checked ? "True" : "False"))
                restart = true;

            return restart;
        }

        private void btnBrowseTemplatePath_Click(object sender, EventArgs e)
        {
            using (var open = new FolderBrowserDialog())
            {
                open.ShowNewFolderButton = true;
                if (open.ShowDialog() == DialogResult.OK)
                {
                    txtTemplatePath.Text = open.SelectedPath;
                }
            }
        }

        public void ApplyDefaults()
        {
            ConfigProperties.ApplyDefaults();
        }

        private void btnBrowseMgCooker_Click(object sender, EventArgs e)
        {
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.Title = string.Format(Properties.Resources.LocateExecutable, "MgCooker.exe");
                dlg.Filter = Properties.Resources.FilterExecutables;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtMgCooker.Text = dlg.FileName;
                }
            }
        }

        private void btnBrowseFsPreview_Click(object sender, EventArgs e)
        {
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.Title = string.Format(Properties.Resources.LocateExecutable, "MaestroFsPreview.exe");
                dlg.Filter = Properties.Resources.FilterExecutables;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtFsPreview.Text = dlg.FileName;
                }
            }
        }
    }
}
