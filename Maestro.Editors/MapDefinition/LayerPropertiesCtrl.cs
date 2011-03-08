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
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using Maestro.Shared.UI;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.MapDefinition
{
    internal partial class LayerPropertiesCtrl : UserControl
    {
        private LayerPropertiesCtrl()
        {
            InitializeComponent();
        }

        public event EventHandler LayerChanged;
        private IResourceService _resSvc;

        public LayerPropertiesCtrl(IMapLayer layer, IResourceService resSvc) : this()
        {
            layer.PropertyChanged += new PropertyChangedEventHandler(OnLayerChanged);
            _resSvc = resSvc;

            TextBoxBinder.BindText(txtResourceId, layer, "ResourceId");
            TextBoxBinder.BindText(txtName, layer, "Name");
            TextBoxBinder.BindText(txtLegendLabel, layer, "LegendLabel");

            CheckBoxBinder.BindChecked(chkExpanded, layer, "ExpandInLegend");
            CheckBoxBinder.BindChecked(chkLegendVisible, layer, "ShowInLegend");
            CheckBoxBinder.BindChecked(chkVisible, layer, "Visible");
            CheckBoxBinder.BindChecked(chkSelectable, layer, "Selectable");
        }

        public LayerPropertiesCtrl(IBaseMapLayer layer, IResourceService resSvc)
            : this()
        {
            layer.PropertyChanged += new PropertyChangedEventHandler(OnLayerChanged);
            _resSvc = resSvc;

            TextBoxBinder.BindText(txtResourceId, layer, "ResourceId");
            TextBoxBinder.BindText(txtName, layer, "Name");
            TextBoxBinder.BindText(txtLegendLabel, layer, "LegendLabel");

            CheckBoxBinder.BindChecked(chkExpanded, layer, "ExpandInLegend");
            CheckBoxBinder.BindChecked(chkLegendVisible, layer, "ShowInLegend");
            //CheckBoxBinder.BindChecked(chkVisible, layer, "Visible");
            chkVisible.Visible = false;
            CheckBoxBinder.BindChecked(chkSelectable, layer, "Selectable");
        }

        void OnLayerChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = this.LayerChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_resSvc, ResourceTypes.LayerDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtResourceId.Text = picker.ResourceID;
                }
            }
        }
    }
}
