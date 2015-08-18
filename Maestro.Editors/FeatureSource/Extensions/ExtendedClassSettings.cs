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

#endregion Disclaimer / License

using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Maestro.Editors.FeatureSource.Extensions
{
    [ToolboxItem(false)]
    internal partial class ExtendedClassSettings : UserControl, IEditorBindable
    {
        private ExtendedClassSettings()
        {
            InitializeComponent();
        }

        private readonly IFeatureSourceExtension _ext;
        private readonly IFeatureSource _fs;

        public ExtendedClassSettings(IFeatureSource fs, IEnumerable<string> qualifiedClassNames, IFeatureSourceExtension ext)
            : this()
        {
            _fs = fs;
            _ext = ext;
            var names = new List<string>(qualifiedClassNames);
            cmbBaseClass.DataSource = names;
            ext.PropertyChanged += WeakEventHandler.Wrap<PropertyChangedEventHandler>(OnExtensionPropertyChanged, (eh) => ext.PropertyChanged -= eh);

            //HACK
            if (string.IsNullOrEmpty(_ext.FeatureClass))
                _ext.FeatureClass = names[0];

            ComboBoxBinder.BindSelectedIndexChanged(cmbBaseClass, nameof(cmbBaseClass.SelectedItem), ext, nameof(ext.FeatureClass));
        }

        private void OnExtensionPropertyChanged(object sender, PropertyChangedEventArgs e) => OnResourceChanged();

        public void Bind(IEditorService service) => service.RegisterCustomNotifier(this);

        private void OnResourceChanged()
        {
            this.ResourceChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ResourceChanged;
        
        private void txtExtendedName_TextChanged(object sender, EventArgs e)
        {
            //Before we apply, check if the new name matches any existing feature classes
            string newName = txtExtendedName.Text;
            if (_fs.Extension.Count(ext => ext.Name == newName) > 0)
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