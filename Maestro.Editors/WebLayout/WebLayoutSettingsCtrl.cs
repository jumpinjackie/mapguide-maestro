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
using Maestro.Editors.Common;
using Maestro.Editors.Generic;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.WebLayout;

namespace Maestro.Editors.WebLayout
{
    //[ToolboxItem(true)]
    [ToolboxItem(false)]
    internal partial class WebLayoutSettingsCtrl : EditorBindableCollapsiblePanel
    {
        public WebLayoutSettingsCtrl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            cmbHyperlinkTarget.DataSource = Enum.GetValues(typeof(TargetType));
        }

        private IEditorService _edsvc;
        private IWebLayout _wl;

        private IMapView _view;

        public override void Bind(IEditorService service)
        {
            _edsvc = service;
            _edsvc.RegisterCustomNotifier(this);
            _edsvc.Saved += OnSaved;

            _wl = (IWebLayout)_edsvc.GetEditedResource();
            GeneratePreviewUrl();
            
            _view = _wl.Map.InitialView;
            if (_view == null)
            {
                _view = _wl.CreateDefaultView();
                chkCustomView.Checked = false;
            }
            else 
            {
                chkCustomView.Checked = true;
            }
            CheckInitialView();

            var wl2 = _wl as IWebLayout2;
            if (wl2 != null)
                CheckBoxBinder.BindChecked(chkPingServer, wl2, "EnablePingServer");
            else
                chkPingServer.Visible = false;

            TextBoxBinder.BindText(numX, _view, "CenterX");
            TextBoxBinder.BindText(numY, _view, "CenterY");
            TextBoxBinder.BindText(numScale, _view, "Scale");
            
            TextBoxBinder.BindText(txtBrowserTitle, _wl, "Title");
            TextBoxBinder.BindText(txtMapDefinition, _wl.Map, "ResourceId");
            TextBoxBinder.BindText(txtInitialTaskPaneUrl, _wl.TaskPane, "InitialTask");

            CheckBoxBinder.BindChecked(chkContextMenu, _wl.ContextMenu, "Visible");
            CheckBoxBinder.BindChecked(chkLegend, _wl.InformationPane, "LegendVisible");
            CheckBoxBinder.BindChecked(chkProperties, _wl.InformationPane, "PropertiesVisible");
            CheckBoxBinder.BindChecked(chkStatusBar, _wl.StatusBar, "Visible");

            CheckBoxBinder.BindChecked(chkTaskPane, _wl.TaskPane, "Visible");
            CheckBoxBinder.BindChecked(chkTaskBar, _wl.TaskPane.TaskBar, "Visible");
            CheckBoxBinder.BindChecked(chkToolbar, _wl.ToolBar, "Visible");
            CheckBoxBinder.BindChecked(chkZoomControl, _wl.ZoomControl, "Visible");

            ComboBoxBinder.BindSelectedIndexChanged(cmbHyperlinkTarget, "SelectedItem", _wl.Map, "HyperlinkTarget");
            TextBoxBinder.BindText(txtHyperlinkFrame, _wl.Map, "HyperlinkTargetFrame");

            TextBoxBinder.BindText(numInfoPaneWidth, _wl.InformationPane, "Width");
            TextBoxBinder.BindText(numTaskPaneWidth, _wl.TaskPane, "Width");

            _wl.PropertyChanged += OnWebLayoutPropertyChanged;
            _view.PropertyChanged += OnWebLayoutPropertyChanged;
            _wl.Map.PropertyChanged += OnWebLayoutPropertyChanged;
            _wl.ContextMenu.PropertyChanged += OnWebLayoutPropertyChanged;
            _wl.InformationPane.PropertyChanged += OnWebLayoutPropertyChanged;
            _wl.StatusBar.PropertyChanged += OnWebLayoutPropertyChanged;
            _wl.TaskPane.PropertyChanged += OnWebLayoutPropertyChanged;
            _wl.TaskPane.TaskBar.PropertyChanged += OnWebLayoutPropertyChanged;
            _wl.ToolBar.PropertyChanged += OnWebLayoutPropertyChanged;
            _wl.ZoomControl.PropertyChanged += OnWebLayoutPropertyChanged;
        }

        private void GeneratePreviewUrl()
        {
            btnShowInBrowser.Enabled = false;
            txtAjaxViewerUrl.Text = string.Empty;
            try
            {
                var conn = _wl.CurrentConnection;
                string baseUrl = conn.GetCustomProperty("BaseUrl").ToString();
                if (!baseUrl.EndsWith("/"))
                    baseUrl += "/";

                if (!_edsvc.IsNew)
                {
                    txtAjaxViewerUrl.Text = baseUrl + "mapviewerajax/?WEBLAYOUT=" + _edsvc.ResourceID + "&LOCALE=" + _edsvc.PreviewLocale;
                    btnShowInBrowser.Enabled = true;
                }
                else
                {
                    txtAjaxViewerUrl.Text = Strings.PreviewUrlNotAvailable;
                }
            }
            catch { }
        }

        void OnSaved(object sender, EventArgs e)
        {
            GeneratePreviewUrl();
        }

        void OnWebLayoutPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnResourceChanged();
        }

        protected override void UnsubscribeEventHandlers()
        {
            try
            {
                _edsvc.Saved -= OnSaved;

                if (_wl != null)
                {
                    _wl.PropertyChanged -= OnWebLayoutPropertyChanged;
                    _wl.Map.PropertyChanged -= OnWebLayoutPropertyChanged;
                    _wl.ContextMenu.PropertyChanged -= OnWebLayoutPropertyChanged;
                    _wl.InformationPane.PropertyChanged -= OnWebLayoutPropertyChanged;
                    _wl.StatusBar.PropertyChanged -= OnWebLayoutPropertyChanged;
                    _wl.TaskPane.PropertyChanged -= OnWebLayoutPropertyChanged;
                    _wl.TaskPane.TaskBar.PropertyChanged -= OnWebLayoutPropertyChanged;
                    _wl.ToolBar.PropertyChanged -= OnWebLayoutPropertyChanged;
                    _wl.ZoomControl.PropertyChanged -= OnWebLayoutPropertyChanged;
                }

                if (_view != null)
                    _view.PropertyChanged -= OnWebLayoutPropertyChanged;
            }
            catch { }

            base.UnsubscribeEventHandlers();
        }

        private void chkCustomView_CheckedChanged(object sender, EventArgs e)
        {
            CheckInitialView();
        }

        private void CheckInitialView()
        {
            numX.Enabled = numY.Enabled = numScale.Enabled = chkCustomView.Checked;
            if (chkCustomView.Checked)
                _wl.Map.InitialView = _view;
            else
                _wl.Map.InitialView = null;
        }

        private void chkTaskPane_CheckedChanged(object sender, EventArgs e)
        {
            numTaskPaneWidth.Enabled = chkTaskPane.Checked;
        }

        private void CheckLeftPaneVisibility()
        {
            numInfoPaneWidth.Enabled = chkLegend.Checked || chkProperties.Checked;
        }

        private void chkLegend_CheckedChanged(object sender, EventArgs e)
        {
            CheckLeftPaneVisibility();
        }

        private void chkProperties_CheckedChanged(object sender, EventArgs e)
        {
            CheckLeftPaneVisibility();
        }

        private void cmbHyperlinkTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHyperlinkTarget.SelectedItem == null)
                txtHyperlinkFrame.Enabled = false;

            txtHyperlinkFrame.Enabled = (((TargetType)cmbHyperlinkTarget.SelectedItem) == TargetType.SpecifiedFrame);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edsvc.ResourceService, ResourceTypes.MapDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    _wl.Map.ResourceId = picker.ResourceID;
                }
            }
        }

        private void btnShowInBrowser_Click(object sender, EventArgs e)
        {
            _edsvc.OpenUrl(txtAjaxViewerUrl.Text);
        }
    }
}
