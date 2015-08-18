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

using Maestro.Editors.Generic;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Maestro.Editors.MapDefinition
{
    [ToolboxItem(false)]
    internal partial class LayerPropertiesCtrl : UserControl
    {
        private LayerPropertiesCtrl()
        {
            InitializeComponent();
        }

        public event EventHandler LayerChanged;

        private readonly IEditorService _edSvc;

        public LayerPropertiesCtrl(IMapLayer layer, IEditorService edSvc)
            : this()
        {
            layer.PropertyChanged += WeakEventHandler.Wrap<PropertyChangedEventHandler>(OnLayerChanged, (eh) => layer.PropertyChanged -= eh);
            _edSvc = edSvc;

            TextBoxBinder.BindText(txtResourceId, layer, nameof(layer.ResourceId));
            TextBoxBinder.BindText(txtName, layer, nameof(layer.Name));
            TextBoxBinder.BindText(txtLegendLabel, layer, nameof(layer.LegendLabel));
        }

        public LayerPropertiesCtrl(IBaseMapLayer layer, IEditorService edSvc)
            : this()
        {
            layer.PropertyChanged += WeakEventHandler.Wrap<PropertyChangedEventHandler>(OnLayerChanged, (eh) => layer.PropertyChanged -= eh);
            _edSvc = edSvc;

            TextBoxBinder.BindText(txtResourceId, layer, nameof(layer.ResourceId));
            TextBoxBinder.BindText(txtName, layer, nameof(layer.Name));
            TextBoxBinder.BindText(txtLegendLabel, layer, nameof(layer.LegendLabel));
        }

        private void OnLayerChanged(object sender, PropertyChangedEventArgs e) => this.LayerChanged?.Invoke(this, EventArgs.Empty);

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.CurrentConnection, ResourceTypes.LayerDefinition.ToString(), ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    txtResourceId.Text = picker.ResourceID;
                }
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (ResourceIdentifier.Validate(txtResourceId.Text))
                _edSvc.OpenResource(txtResourceId.Text);
        }
    }
}