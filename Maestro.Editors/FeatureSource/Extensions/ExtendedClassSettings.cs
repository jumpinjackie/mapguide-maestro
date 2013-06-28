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
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.FeatureSource.Extensions
{
    [ToolboxItem(false)]
    internal partial class ExtendedClassSettings : UserControl, IEditorBindable
    {
        private ExtendedClassSettings()
        {
            InitializeComponent();
        }

        private IFeatureSourceExtension _ext;
        private IFeatureSource _fs;

        public ExtendedClassSettings(IFeatureSource fs, IEnumerable<string> qualifiedClassNames, IFeatureSourceExtension ext)
            : this()
        {
            _fs = fs;
            _ext = ext;
            var names = new List<string>(qualifiedClassNames);
            cmbBaseClass.DataSource = names;
            ext.PropertyChanged += (sender, e) => { OnResourceChanged(); };

            //HACK
            if (string.IsNullOrEmpty(_ext.FeatureClass))
                _ext.FeatureClass = names[0];

            ComboBoxBinder.BindSelectedIndexChanged(cmbBaseClass, "SelectedItem", ext, "FeatureClass"); //NOXLATE
        }

        public void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
        }

        private void OnResourceChanged()
        {
            var handler = this.ResourceChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler ResourceChanged;

        private int GetExtensionCount(string name)
        {
            int count = 0;
            foreach (var ext in _fs.Extension)
            {
                if (ext.Name == name)
                    count++;
            }
            return count;
        }

        private void txtExtendedName_TextChanged(object sender, EventArgs e)
        {
            //Before we apply, check if the new name matches any existing feature classes
            string newName = txtExtendedName.Text;
            if (GetExtensionCount(newName) > 0)
            {
                errorProvider.SetError(txtExtendedName, string.Format(Strings.ExtendedFeatureClassAlreadyExists, newName));
            }
            else
            {
                errorProvider.Clear();
                _ext.Name = txtExtendedName.Text;
            }
        }
    }
}
