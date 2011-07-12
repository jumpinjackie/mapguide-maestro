#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using Maestro.Shared.UI;
using System.IO;
using Maestro.Editors.Common;
using Maestro.Editors.Generic;

namespace Maestro.Editors.SymbolDefinition.GraphicsEditors
{
    internal partial class ImageDialog : Form
    {
        private ISimpleSymbolDefinition _ssd;
        private IImageGraphic _image;
        private IImageReference _imageRef;
        private IInlineImage _imageInline;
        private EditorBindableCollapsiblePanel _ed;

        private bool _init = false;

        public ImageDialog(EditorBindableCollapsiblePanel parent, ISimpleSymbolDefinition ssd, IImageGraphic image)
        {
            InitializeComponent();
            _ed = parent;
            _ssd = ssd;
            _image = image;
            try
            {
                _init = true;
                if (_image.Item.Type == ImageType.Reference)
                {
                    _imageRef = (IImageReference)_image.Item;
                    rdResourceRef.Checked = true;
                }
                else
                {
                    _imageInline = (IInlineImage)_image.Item;
                    rdInline.Checked = true;
                }

                if (_imageRef == null)
                    _imageRef = ssd.CreateImageReference("", "");
                else
                    rdResourceRef.Checked = true;

                if (_imageInline == null)
                    _imageInline = ssd.CreateInlineImage(new byte[0]);
                else
                    rdInline.Checked = true;

                txtResourceId.Text = _imageRef.ResourceId;
                txtResData.Text = _imageRef.LibraryItemName;

                txtImageBase64.Text = Convert.ToBase64String(_imageInline.Content);
                txtImageBase64.Tag = _imageInline.Content;

                imageType_CheckedChanged(this, null);
            }
            finally
            {
                _init = false;
            }
        }

        private void imageType_CheckedChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            txtImageBase64.Enabled = false;
            txtResData.Enabled = false;
            txtResourceId.Enabled = false;

            if (rdResourceRef.Checked)
            {
                txtResourceId.Enabled = true;
                txtResData.Enabled = true;

                _image.Item = _imageRef;
            }
            else if (rdInline.Checked)
            {
                txtImageBase64.Enabled = true;

                _image.Item = _imageInline;
            }
        }

        private void txtResourceId_TextChanged(object sender, EventArgs e)
        {
            _imageRef.ResourceId = txtImageBase64.Text;
        }

        private void txtResData_TextChanged(object sender, EventArgs e)
        {
            _imageRef.LibraryItemName = txtResData.Text;
        }

        private void txtImageBase64_TextChanged(object sender, EventArgs e)
        {
            _imageInline.Content = Convert.FromBase64String(txtImageBase64.Text);
        }

        private void lnkLoadImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var open = DialogFactory.OpenFile())
            {
                open.Filter = Properties.Resources.FilterPng;
                if (open.ShowDialog() == DialogResult.OK)
                {
                    byte[] content = File.ReadAllBytes(open.FileName);
                    _imageInline.Content = content;
                    txtImageBase64.Text = Convert.ToBase64String(content);
                    txtImageBase64.Tag = content;
                }
            }
        }

        private void lnkPreview_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtImageBase64.Tag != null)
            {
                byte[] content = txtImageBase64.Tag as byte[];
                if (content != null)
                {
                    using (var ms = new MemoryStream(content))
                    {
                        picPreview.Image = Image.FromStream(ms);
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_ssd.CurrentConnection.ResourceService, ResourcePickerMode.OpenResource))
            {
                txtResourceId.Text = picker.ResourceID;
            }
        }

        private void btnResData_Click(object sender, EventArgs e)
        {
            string resourceId = txtResourceId.Text;
            if (string.IsNullOrEmpty(resourceId))
                resourceId = _ssd.ResourceID;

            if (!_ssd.CurrentConnection.ResourceService.ResourceExists(resourceId))
            {
                MessageBox.Show(Properties.Resources.ResourceDoesntExist);
                return;
            }

            var resData = _ssd.CurrentConnection.ResourceService.EnumerateResourceData(resourceId);
            var items = new List<string>();
            foreach (var rd in resData.ResourceData)
            {
                items.Add(rd.Name);
            }

            var result = GenericItemSelectionDialog.SelectItem(null, null, items.ToArray());
            if (result != null)
            {
                txtResData.Text = "'" + result + "'"; //To avoid Expression Engine invocation
            }
        }
    }
}
