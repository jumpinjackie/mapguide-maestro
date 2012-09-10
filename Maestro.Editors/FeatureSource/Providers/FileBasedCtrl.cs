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
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.FeatureSource;

namespace Maestro.Editors.FeatureSource.Providers
{
    [ToolboxItem(false)]
    internal partial class FileBasedCtrl : EditorBindableCollapsiblePanel
    {
        public FileBasedCtrl()
        {
            InitializeComponent();
        }

        private void rdManaged_CheckedChanged(object sender, EventArgs e)
        {
            unmanagedPanel.Enabled = false;
            resDataCtrl.Enabled = true;
        }

        private void rdUnmanaged_CheckedChanged(object sender, EventArgs e)
        {
            unmanagedPanel.Enabled = true;
            resDataCtrl.Enabled = false;
        }

        private IEditorService _service;

        public override void Bind(IEditorService service)
        {
            _service = service;
            _service.RegisterCustomNotifier(this);
            btnBrowseAliasFile.Enabled = true;
            btnBrowseAliasFolder.Enabled = CanSelectFolders();
            resDataCtrl.Init(service);
            resDataCtrl.DataListChanged += (sender, e) => { OnResourceChanged(); };
            resDataCtrl.ResourceDataMarked += (sender, e) => { OnResourceMarked(e); };
        }

        protected virtual void OnResourceMarked(string dataName)
        {
            
        }

        protected virtual string[] GetUnmanagedFileExtensions() { return new string[0]; }

        private void btnBrowseAlias_Click(object sender, EventArgs e)
        {
            using (var picker = new UnmanagedFileBrowser(_service.ResourceService))
            {
                picker.Extensions = GetUnmanagedFileExtensions();
                picker.SelectFoldersOnly = false;
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtAlias.Text = picker.SelectedItem;
                }
            }
        }

        private void btnBrowseAliasFolder_Click(object sender, EventArgs e)
        {
            using (var picker = new UnmanagedFileBrowser(_service.ResourceService))
            {
                picker.SelectFoldersOnly = true;
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtAlias.Text = picker.SelectedItem;
                }
            }
        }

        protected virtual string FileFdoProperty { get { return "File"; } } //NOXLATE

        protected virtual bool CanSelectFolders()
        {
            return false;
        }

        private void txtAlias_TextChanged(object sender, EventArgs e)
        {
            if (rdUnmanaged.Checked)
            {
                var fs = (IFeatureSource)_service.GetEditedResource();
                fs.SetConnectionProperty(this.FileFdoProperty, txtAlias.Text);
                OnResourceChanged();
            }
        }
    }
}
