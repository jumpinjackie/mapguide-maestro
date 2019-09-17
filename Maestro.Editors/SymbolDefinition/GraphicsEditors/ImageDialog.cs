#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
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
using Maestro.Editors.Generic;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Maestro.Editors.SymbolDefinition.GraphicsEditors
{
    internal partial class ImageDialog : Form
    {
        private readonly ISimpleSymbolDefinition _ssd;
        private IImageGraphic _image;
        private IImageReference _imageRef;
        private IInlineImage _imageInline;
        private EditorBindableCollapsiblePanel _ed;
        private readonly IServerConnection _conn;

        private bool _init = false;

        public ImageDialog(EditorBindableCollapsiblePanel parent, IServerConnection conn, ISimpleSymbolDefinition ssd, IImageGraphic image)
        {
            InitializeComponent();
            _ed = parent;
            _ssd = ssd;
            _image = image;
            _conn = conn;
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
                    _imageRef = ssd.CreateImageReference(string.Empty, string.Empty);
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

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                _init = true;
                symAngle.Bind(_image, nameof(_image.Angle));
                symPositionX.Bind(_image, nameof(_image.PositionX));
                symPositionY.Bind(_image, nameof(_image.PositionY));
                symSizeScalable.Bind(_image, nameof(_image.SizeScalable));
                symSizeX.Bind(_image, nameof(_image.SizeX));
                symSizeY.Bind(_image, nameof(_image.SizeY));
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

        private void txtResourceId_TextChanged(object sender, EventArgs e) => _imageRef.ResourceId = txtResourceId.Text;

        private void txtResData_TextChanged(object sender, EventArgs e) => _imageRef.LibraryItemName = txtResData.Text;

        private void txtImageBase64_TextChanged(object sender, EventArgs e)
        {
            //HACK: Leaky Abstraction. The impl of IImageGraphic.Item returns a new instance
            //So these are not the same reference
            _imageInline.Content = Convert.FromBase64String(txtImageBase64.Text);
            if (_image.Item.Type == ImageType.Inline)
                _image.Item = _imageInline;
        }

        private void lnkLoadImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var open = DialogFactory.OpenFile())
            {
                open.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickPng, "png"); //NOXLATE
                if (open.ShowDialog() == DialogResult.OK)
                {
                    byte[] content = File.ReadAllBytes(open.FileName);
                    //HACK: Leaky Abstraction. The impl of IImageGraphic.Item returns a new instance
                    //So these are not the same reference
                    _imageInline.Content = content;
                    if (_image.Item.Type == ImageType.Inline)
                        _image.Item = _imageInline;
                    txtImageBase64.Text = Convert.ToBase64String(content);
                    txtImageBase64.Tag = content;
                    using (var ms = new MemoryStream(content))
                    {
                        Image img = Image.FromStream(ms);
                        symSizeX.Content = $"'{PxToMM(img.Width, 96).ToString(System.Globalization.CultureInfo.InvariantCulture)}'"; //NOXLATE
                        symSizeY.Content = $"'{PxToMM(img.Height, 96).ToString(System.Globalization.CultureInfo.InvariantCulture)}'"; //NOXLATE
                    }
                }
            }
        }

        private static double PxToMM(int px, int dpi) => (px * 25.4) / dpi;

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

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_conn, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    txtResourceId.Text = picker.ResourceID;
                }
            }
        }

        private void btnResData_Click(object sender, EventArgs e)
        {
            string resourceId = txtResourceId.Text;
            if (string.IsNullOrEmpty(resourceId))
                resourceId = _ssd.ResourceID;

            if (!_conn.ResourceService.ResourceExists(resourceId))
            {
                MessageBox.Show(Strings.ResourceDoesntExist);
                return;
            }

            var resData = _conn.ResourceService.EnumerateResourceData(resourceId);
            var items = new List<string>();
            foreach (var rd in resData.ResourceData)
            {
                items.Add(rd.Name);
            }

            var result = GenericItemSelectionDialog.SelectItem(null, null, items.ToArray());
            if (result != null)
            {
                txtResData.Text = $"'{result}'"; //To avoid Expression Engine invocation
            }
        }

        private void OnContentChanged(object sender, EventArgs e) => _ed.RaiseResourceChanged();

        private void OnRequestBrowse(SymbolField sender) => ParameterSelector.ShowParameterSelector(_ssd.ParameterDefinition.Parameter, sender);
    }
}