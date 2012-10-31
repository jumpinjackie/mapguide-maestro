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
using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System.Net;
using Maestro.Shared.UI;

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
                _baseUrl = service.GetCustomProperty("BaseUrl").ToString(); //NOXLATE

                if (!_baseUrl.EndsWith("/")) //NOXLATE
                    _baseUrl += "/"; //NOXLATE
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

        void OnSaved(object sender, EventArgs e)
        {
            GeneratePreviewUrl();
        }

        private void GeneratePreviewUrl()
        {
            btnShowInBrowser.Enabled = false;
            txtPublicUrl.Text = string.Empty;

            try
            {
                var conn = _flexLayout.CurrentConnection;
                string baseUrl = conn.GetCustomProperty("BaseUrl").ToString(); //NOXLATE
                if (!baseUrl.EndsWith("/")) //NOXLATE
                    baseUrl += "/"; //NOXLATE

                if (!_edsvc.IsNew)
                {
                    txtPublicUrl.Text = baseUrl + txtTemplateUrl.Text + "?ApplicationDefinition=" + _edsvc.ResourceID + "&locale=" + _edsvc.PreviewLocale; //NOXLATE
                    btnShowInBrowser.Enabled = true;
                }
                else
                {
                    txtPublicUrl.Text = Strings.PreviewUrlNotAvailable;
                }
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
                var handler = this.TemplateChanged;
                if (handler != null)
                {
                    handler(template);
                }
            }
        }

        private void btnShowInBrowser_Click(object sender, EventArgs e)
        {
            _edsvc.OpenUrl(txtPublicUrl.Text);
        }
    }
}
