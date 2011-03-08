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

namespace Maestro.Editors.MapDefinition
{
    internal partial class GroupPropertiesCtrl : UserControl
    {
        private GroupPropertiesCtrl()
        {
            InitializeComponent();
        }

        public event EventHandler GroupChanged;

        public GroupPropertiesCtrl(IMapLayerGroup group)
            : this()
        {
            group.PropertyChanged += new PropertyChangedEventHandler(OnGroupChanged);

            TextBoxBinder.BindText(txtName, group, "Name");
            TextBoxBinder.BindText(txtLegendLabel, group, "LegendLabel");

            CheckBoxBinder.BindChecked(chkExpanded, group, "ExpandInLegend");
            CheckBoxBinder.BindChecked(chkLegendVisible, group, "ShowInLegend");
            CheckBoxBinder.BindChecked(chkVisible, group, "Visible");
        }

        public GroupPropertiesCtrl(IBaseMapGroup group)
            : this()
        {
            group.PropertyChanged += new PropertyChangedEventHandler(OnGroupChanged);

            TextBoxBinder.BindText(txtName, group, "Name");
            TextBoxBinder.BindText(txtLegendLabel, group, "LegendLabel");

            CheckBoxBinder.BindChecked(chkExpanded, group, "ExpandInLegend");
            CheckBoxBinder.BindChecked(chkLegendVisible, group, "ShowInLegend");
            //CheckBoxBinder.BindChecked(chkVisible, group, "Visible");
            chkVisible.Visible = false;
        }

        void OnGroupChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = this.GroupChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
