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
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI.Services;
using Maestro.Base.Services;
using Maestro.Base.Templates;
using OSGeo.MapGuide.MaestroAPI;
using System.Threading;

namespace Maestro.Base.UI
{
    //FIXME: Exceptions thrown in Mono centered around a non-existent image

    internal partial class NewResourceDialog : Form
    {
        private static bool _subsequentRun = false;
        private static List<string> _lastSelectedCategoies = new List<string>();

        private NewResourceDialog()
        {
            InitializeComponent();
        }

        private IServerConnection _conn;
        private NewItemTemplateService _nits;

        public NewResourceDialog(IServerConnection conn, NewItemTemplateService nits)
            : this()
        {
            _conn = conn;
            _nits = nits;
        }

        protected override void OnLoad(EventArgs e)
        {
            lstCategories.DataSource = _nits.GetCategories();

            if (Platform.IsRunningOnMono)
            {
                //Sorry, you mono users will have to manually select
                //the other categories due to dodgy ImageList/ListView
                //implementation
                lstCategories.SetSelected(0, true);
            }
            else
            {
                if (_subsequentRun)
                {
                    if (_lastSelectedCategoies.Count > 0)
                    {
                        foreach (var cat in _lastSelectedCategoies)
                        {
                            var idx = lstCategories.Items.IndexOf(cat);
                            if (idx >= 0)
                            {
                                lstCategories.SetSelected(idx, true);
                            }
                        }
                    }
                }
                else
                {
                    //First run, select all categories
                    foreach (var cat in _nits.GetCategories())
                    {
                        var idx = lstCategories.Items.IndexOf(cat);
                        if (idx >= 0)
                        {
                            lstCategories.SetSelected(idx, true);
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private string[] GetSelectedCategories()
        {
            List<string> categories = new List<string>();
            foreach (var item in lstCategories.SelectedItems)
            {
                categories.Add(item.ToString());
            }
            return categories.ToArray();
        }

        private void lstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCategories.SelectedItems.Count > 0)
            {
                string cat = lstCategories.SelectedItem.ToString();
                LoadTemplates(_nits.GetItemTemplates(GetSelectedCategories(), _conn.SiteVersion));

                txtDescription.Text = string.Empty;
                btnOK.Enabled = false;
            }
            else
            {
                lstTemplates.Clear();
                lstTemplates.Groups.Clear();
                tplImgList.Images.Clear();
            }
        }

        private ImageList tplImgList = new ImageList();

        private void LoadTemplates(Maestro.Base.Services.NewItemTemplateService.TemplateSet templSet)
        {
            lstTemplates.Clear();
            tplImgList.Images.Clear();
            lstTemplates.Groups.Clear();

            lstTemplates.SmallImageList = tplImgList;
            lstTemplates.LargeImageList = tplImgList;

            Dictionary<string, ListViewGroup> groups = new Dictionary<string, ListViewGroup>();
            foreach (var cat in templSet.GetCategories())
            {
                var grp = new ListViewGroup();
                grp.Name = cat;
                grp.Header = cat;

                lstTemplates.Groups.Add(grp);

                groups.Add(cat, grp);
            }
            foreach (var cat in templSet.GetCategories())
            {
                tplImgList.Images.Add(Properties.Resources.document);
                foreach (var tpl in templSet.GetTemplatesForCategory(cat))
                {
                    //This is to weed out resource types not supported by the current connection
                    //we're working with
                    if (!_conn.Capabilities.IsSupportedResourceType(tpl.ResourceType))
                        continue;

                    var li = new ListViewItem();
                    li.Name = tpl.Name;
                    li.Text = tpl.Name;
                    li.ToolTipText = tpl.Description;

                    if (tpl.Icon == null)
                    {
                        li.ImageIndex = 0;
                    }
                    else
                    {
                        tplImgList.Images.Add(tpl.Icon);
                        li.ImageIndex = tplImgList.Images.Count - 1;
                    }

                    li.Tag = tpl;

                    li.Group = groups[cat];

                    lstTemplates.Items.Add(li);
                }
                
            }
        }

        private void lstTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDescription.Text = string.Empty;
            if (lstTemplates.SelectedItems.Count == 1)
            {
                var li = lstTemplates.SelectedItems[0];
                var tpl = (ItemTemplate)li.Tag;

                txtDescription.Text = tpl.Description;
                this.SelectedTemplate = tpl;
                CheckButtonState();
            }
        }

        private void CheckButtonState()
        {
            btnOK.Enabled = this.SelectedTemplate != null;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            CheckButtonState();
        }

        private void txtParentFolder_TextChanged(object sender, EventArgs e)
        {
            CheckButtonState();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RememberSelectedCategories();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Gets the selected template
        /// </summary>
        public ItemTemplate SelectedTemplate
        {
            get;
            private set;
        }

        private void RememberSelectedCategories()
        {
            _lastSelectedCategoies.Clear();
            foreach (var item in lstCategories.SelectedItems)
            {
                _lastSelectedCategoies.Add(item.ToString());
            }

            if (!_subsequentRun)
                _subsequentRun = true;
        }

        private void lstTemplates_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var item = lstTemplates.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                lstTemplates.SelectedItems.Clear();
                item.Selected = true;

                RememberSelectedCategories();
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
