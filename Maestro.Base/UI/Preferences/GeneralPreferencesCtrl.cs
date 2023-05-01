﻿#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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

using ICSharpCode.Core;
using Maestro.Shared.UI;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Props = ICSharpCode.Core.PropertyService;

namespace Maestro.Base.UI.Preferences
{
    internal partial class GeneralPreferencesCtrl : UserControl, IPreferenceSheet
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

            var viewer = Props.Get(ConfigProperties.PreviewViewerType, "AJAX"); //NOXLATE
            if (viewer.Equals("AJAX")) //NOXLATE
                rdAjax.Checked = true;
            else
                rdFusion.Checked = true;

            cmbTheme.DataSource = new string[] { ConfigProperties.DefaultSelectedTheme }.Concat(Themes.List).ToArray();
            cmbTheme.SelectedItem = Props.Get<string>(ConfigProperties.SelectedTheme, ConfigProperties.DefaultSelectedTheme);

            var path = Props.Get(ConfigProperties.UserTemplatesDirectory, Path.Combine(FileUtility.ApplicationRootPath, "UserTemplates")); //NOXLATE
            txtTemplatePath.Text = path;
            var msg = Props.Get(ConfigProperties.ShowMessages, true);
            chkMessages.Checked = msg;
            var outb = Props.Get(ConfigProperties.ShowOutboundRequests, true);
            chkOutbound.Checked = outb;
            var totd = Props.Get(ConfigProperties.ShowTipOfTheDay, ConfigProperties.DefaultShowTipOfTheDay);
            chkShowTipOfTheDay.Checked = totd;

            txtFsPreview.Text = Props.Get(ConfigProperties.LocalFsPreviewPath, ConfigProperties.DefaultLocalFsPreviewPath);
            txtMgTileSeeder.Text = Props.Get(ConfigProperties.MgTileSeederPath, ConfigProperties.DefaultMgTileSeederPath);
            txtRtMapInspector.Text = Props.Get(ConfigProperties.RtMapInspectorPath, ConfigProperties.DefaultRtMapInspectorPath);
            txtLiveMapEditor.Text = Props.Get(ConfigProperties.LiveMapEditorPath, ConfigProperties.DefaultLiveMapEditorPath);
            txtProviderTemplateTool.Text = Props.Get(ConfigProperties.ProviderToolPath, ConfigProperties.DefaultProviderToolPath);

            cmbOpenedColor.CurrentColor = Props.Get(ConfigProperties.OpenColor, Color.LightGreen);
            cmbModifiedColor.CurrentColor = Props.Get(ConfigProperties.DirtyColor, Color.Pink);
        }

        public string Title
        {
            get { return Strings.Prefs_General; }
        }

        public Control ContentControl
        {
            get { return this; }
        }

        private bool Apply<T>(string key, T newValue)
        {
            if (Props.Get<T>(key, newValue).Equals(newValue))
                return false;

            Props.Set<T>(key, newValue);
            return true;
        }

        public bool ApplyChanges()
        {
            bool restart = false;

            //These changes can be applied without restart
            if (rdFusion.Checked)
                Apply(ConfigProperties.PreviewViewerType, "FUSION"); //NOXLATE
            else
                Apply(ConfigProperties.PreviewViewerType, "AJAX"); //NOXLATE

            var themeName = cmbTheme.SelectedItem?.ToString();
            Apply(ConfigProperties.SelectedTheme, themeName);
            Apply(ConfigProperties.UserTemplatesDirectory, txtTemplatePath.Text);
            Apply(ConfigProperties.MgTileSeederPath, txtMgTileSeeder.Text);
            Apply(ConfigProperties.LocalFsPreviewPath, txtFsPreview.Text);
            Apply(ConfigProperties.RtMapInspectorPath, txtRtMapInspector.Text);
            Apply(ConfigProperties.LiveMapEditorPath, txtLiveMapEditor.Text);
            Apply(ConfigProperties.OpenColor, (Color)cmbOpenedColor.CurrentColor);
            Apply(ConfigProperties.DirtyColor, (Color)cmbModifiedColor.CurrentColor);
            Apply(ConfigProperties.ShowTipOfTheDay, chkShowTipOfTheDay.Checked);
            Apply(ConfigProperties.ProviderToolPath, txtProviderTemplateTool.Text);

            //These changes require restart
            if (themeName != Themes.CurrentTheme)
                restart = true;

            if (Apply(ConfigProperties.ShowMessages, chkMessages.Checked ? "True" : "False")) //NOXLATE
                restart = true;

            if (Apply(ConfigProperties.ShowOutboundRequests, chkOutbound.Checked ? "True" : "False")) //NOXLATE
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
            ConfigProperties.ApplyGeneralDefaults();
        }

        private void btnBrowseMgTileSeeder_Click(object sender, EventArgs e)
        {
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.Title = string.Format(Strings.LocateExecutable, "MgTileSeeder.exe"); //NOXLATE
                dlg.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickExe, "exe"); //NOXLATE
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtMgTileSeeder.Text = dlg.FileName;
                }
            }
        }

        private void btnBrowseFsPreview_Click(object sender, EventArgs e)
        {
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.Title = string.Format(Strings.LocateExecutable, "MaestroFsPreview.exe"); //NOXLATE
                dlg.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickExe, "exe"); //NOXLATE
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtFsPreview.Text = dlg.FileName;
                }
            }
        }

        private void btnBrowseRtMapInspector_Click(object sender, EventArgs e)
        {
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.Title = string.Format(Strings.LocateExecutable, "RtMapInspector.exe"); //NOXLATE
                dlg.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickExe, "exe"); //NOXLATE
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtRtMapInspector.Text = dlg.FileName;
                }
            }
        }

        private void btnBrowseLiveMapEditor_Click(object sender, EventArgs e)
        {
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.Title = string.Format(Strings.LocateExecutable, "Maestro.LiveMapEditor.exe"); //NOXLATE
                dlg.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickExe, "exe"); //NOXLATE
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtLiveMapEditor.Text = dlg.FileName;
                }
            }
        }

        private void btnProviderTemplate_Click(object sender, EventArgs e)
        {
            using (var dlg = DialogFactory.OpenFile())
            {
                dlg.Title = string.Format(Strings.LocateExecutable, "ProviderTemplate.exe"); //NOXLATE
                dlg.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickExe, "exe"); //NOXLATE
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtProviderTemplateTool.Text = dlg.FileName;
                }
            }
        }
    }
}