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

namespace Maestro.Base.UI
{
    public partial class NewResourceDialog : Form
    {
        private static string _lastSelectedCategory;

        private NewResourceDialog()
        {
            InitializeComponent();
        }

        private Version _siteVersion;
        private IResourceService _resSvc;
        private NewItemTemplateService _nits;

        public NewResourceDialog(IResourceService resSvc, Version siteVersion, NewItemTemplateService nits)
            : this()
        {
            _siteVersion = siteVersion;
            _resSvc = resSvc;
            _nits = nits;
        }

        protected override void OnLoad(EventArgs e)
        {
            lstCategories.DataSource = _nits.GetCategories();

            if (!string.IsNullOrEmpty(_lastSelectedCategory))
            {
                var idx = lstCategories.Items.IndexOf(_lastSelectedCategory);
                if (idx >= 0)
                    lstCategories.SelectedIndex = idx;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void lstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCategories.SelectedItem != null)
            {
                string cat = lstCategories.SelectedItem.ToString();
                LoadTemplates(_nits.GetItemTemplates(cat, _siteVersion));

                txtDescription.Text = string.Empty;
                btnOK.Enabled = false;
            }
        }

        private ImageList tplImgList = new ImageList();

        private void LoadTemplates(ItemTemplate[] templates)
        {
            lstTemplates.Clear();
            tplImgList.Images.Clear();
            tplImgList.Images.Add(Properties.Resources.document);
            foreach (var tpl in templates)
            {
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

                lstTemplates.Items.Add(li);
            }
            lstTemplates.SmallImageList = tplImgList;
            lstTemplates.LargeImageList = tplImgList;
        }

        private void lstTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDescription.Text = "";
            if (lstTemplates.SelectedItems.Count == 1)
            {
                var li = lstTemplates.SelectedItems[0];
                var tpl = (ItemTemplate)li.Tag;

                txtDescription.Text = tpl.Description;
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
            RememberSelectedCategory();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Gets the selected template
        /// </summary>
        public ItemTemplate SelectedTemplate
        {
            get
            {
                if (lstTemplates.SelectedItems.Count == 1)
                {
                    return (ItemTemplate)lstTemplates.SelectedItems[0].Tag;
                }
                return null;
            }
        }

        private void RememberSelectedCategory()
        {
            _lastSelectedCategory = lstCategories.SelectedItem.ToString();
        }

        private void lstTemplates_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var item = lstTemplates.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                lstTemplates.SelectedItems.Clear();
                item.Selected = true;

                RememberSelectedCategory();
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
