#region Disclaimer / License

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

using Maestro.Editors.Common;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace Maestro.Editors.Fusion
{
    internal delegate void TemplateChangeEventHandler(IApplicationDefinitionTemplateInfo selectedTemplate);

    [ToolboxItem(false)]
    //[ToolboxItem(true)]
    internal partial class FlexLayoutSettingsCtrl : EditorBindableCollapsiblePanel
    {
        public FlexLayoutSettingsCtrl()
        {
            InitializeComponent();
        }

        private IFusionService _fsvc;
        private IApplicationDefinition _flexLayout;
        private string _baseUrl;
        private IEditorService _edsvc;

        public override void Bind(IEditorService service)
        {
            _edsvc = service;
            _edsvc.RegisterCustomNotifier(this);

            try
            {
                _fsvc = (IFusionService)_edsvc.GetService((int)ServiceType.Fusion);
                if (service.CurrentConnection.ProviderName.ToUpper() == "MAESTRO.HTTP")
                {
                    _baseUrl = service.GetCustomProperty("BaseUrl").ToString(); //NOXLATE

                    if (!_baseUrl.EndsWith("/")) //NOXLATE
                        _baseUrl += "/"; //NOXLATE
                }
            }
            catch
            {
                throw new NotSupportedException(Strings.IncompatibleConnection);
            }

            _edsvc.Saved += OnSaved;
            _flexLayout = (IApplicationDefinition)service.GetEditedResource();

            TextBoxBinder.BindText(txtTemplateUrl, _flexLayout, "TemplateUrl"); //NOXLATE
            TextBoxBinder.BindText(txtTitle, _flexLayout, "Title"); //NOXLATE
            var templates = _fsvc.GetApplicationTemplates();
            InitializeTemplateList(templates);

            GeneratePreviewUrl();
        }

        protected override void UnsubscribeEventHandlers()
        {
            _edsvc.Saved -= OnSaved;
            base.UnsubscribeEventHandlers();
        }

        private void OnSaved(object sender, EventArgs e)
        {
            GeneratePreviewUrl();
        }

        class UnavailablePreviewUrl : IPreviewUrl
        {
            public string Url => "about:blank";

            public string Display => Strings.PreviewUrlNotAvailable;
        }

        private void GeneratePreviewUrl()
        {
            btnShowInBrowser.Enabled = false;

            try
            {
                var conn = _edsvc.CurrentConnection;
                string baseUrl = conn.GetCustomProperty("BaseUrl").ToString(); //NOXLATE
                if (!baseUrl.EndsWith("/")) //NOXLATE
                    baseUrl += "/"; //NOXLATE

                var list = new List<IPreviewUrl>();
                if (!_edsvc.IsNew)
                {
                    list.Add(new PreviewUrl { Name = "Selected Fusion Template", Url = $"{baseUrl + txtTemplateUrl.Text}?ApplicationDefinition={_edsvc.ResourceID}&locale={_edsvc.PreviewLocale}" }); //NOXLATE
                    list.AddRange(_edsvc.GetAlternateFlexibleLayoutPreviewUrls(_edsvc.ResourceID, _edsvc.PreviewLocale));
                    btnShowInBrowser.Enabled = true;
                }
                else
                {
                    list.Add(new UnavailablePreviewUrl());
                }

                cmbPublicUrl.DataSource = list;
                cmbPublicUrl.SelectedIndex = 0;
            }
            catch { }
        }

        private void InitializeTemplateList(IApplicationDefinitionTemplateInfoSet templates)
        {
            lstTemplates.Clear();
            tplImageList.Images.Clear();

            foreach (var tpl in templates.TemplateInfo)
            {
                Image img = null;
                string prevUrl = _baseUrl + tpl.PreviewImageUrl;
                try
                {
                    var req = (HttpWebRequest)HttpWebRequest.Create(prevUrl);
                    using (var resp = (HttpWebResponse)req.GetResponse())
                    {
                        using (var stream = resp.GetResponseStream())
                        {
                            img = Image.FromStream(stream);
                        }
                    }
                }
                catch
                {
                    img = Properties.Resources.question;
                }
                tplImageList.Images.Add(tpl.PreviewImageUrl, img);
            }

            lstTemplates.Focus(); //Item doesn't get selected when ListView doesn't have focus

            foreach (var tpl in templates.TemplateInfo)
            {
                var item = new ListViewItem();
                item.Tag = tpl;
                item.Name = tpl.Name;
                item.Text = tpl.Name;
                item.ImageKey = tpl.PreviewImageUrl;

                lstTemplates.Items.Add(item);

                if (tpl.LocationUrl == _flexLayout.TemplateUrl)
                    item.Selected = true;
            }
        }

        internal event TemplateChangeEventHandler TemplateChanged;

        private void lstTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTemplates.SelectedItems.Count == 1)
            {
                var item = lstTemplates.SelectedItems[0];
                var template = (IApplicationDefinitionTemplateInfo)item.Tag;
                txtTemplateUrl.Text = template.LocationUrl;
                GeneratePreviewUrl();
                this.TemplateChanged?.Invoke(template);
            }
        }

        private void btnShowInBrowser_Click(object sender, EventArgs e)
        {
            _edsvc.OpenUrl(((IPreviewUrl)cmbPublicUrl.SelectedItem).Url);
        }

        private void btnCopyClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(((IPreviewUrl)cmbPublicUrl.SelectedItem).Url);
            MessageBox.Show(Strings.CopiedUrlToClipboard);
        }
    }
}