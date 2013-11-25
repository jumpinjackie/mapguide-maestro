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
    [ToolboxItem(false)]
    internal partial class GroupPropertiesCtrl : UserControl
    {
        private GroupPropertiesCtrl()
        {
            InitializeComponent();
        }

        public event EventHandler GroupChanged;
        private IMapDefinition _mdf;
        private IMapLegendElementBase _el;

        private bool _init = false;

        public GroupPropertiesCtrl(IMapDefinition map, IMapLayerGroup group)
            : this()
        {
            _init = true;
            try
            {
                _mdf = map;
                _el = group;
                group.PropertyChanged += new PropertyChangedEventHandler(OnGroupChanged);
                string currentName = group.Name;
                txtName.Text = currentName;
                //TextBoxBinder.BindText(txtName, group, "Name");
                /*
                IMapDefinition mdf = group.Parent;
                string currentName = group.Name;
                txtName.Text = currentName;
                txtName.TextChanged += (s, e) =>
                {
                    string newName = txtName.Text;
                    group.Name = newName;
                    mdf.UpdateDynamicGroupName(currentName, newName);
                    System.Diagnostics.Debug.WriteLine(string.Format("Updated group name {0} -> {1}", currentName, newName));
                    currentName = newName;
                };*/

                TextBoxBinder.BindText(txtLegendLabel, group, "LegendLabel");
            }
            finally
            {
                _init = false;
            }
        }

        public GroupPropertiesCtrl(IMapDefinition map, IBaseMapGroup group)
            : this()
        {
            _init = true;
            try
            {
                _mdf = map;
                _el = group;
                group.PropertyChanged += new PropertyChangedEventHandler(OnGroupChanged);

                txtName.Text = group.Name;
                TextBoxBinder.BindText(txtLegendLabel, group, "LegendLabel");
            }
            finally
            {
                _init = false;
            }
        }

        void OnGroupChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = this.GroupChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        static int GetGroupCount(IMapDefinition map, string name)
        {
            int count = 0;
            foreach (var grp in map.MapLayerGroup)
            {
                if (grp.Name == name)
                    count++;
            }
            if (map.BaseMap != null)
            {
                foreach (var grp in map.BaseMap.BaseMapLayerGroup)
                {
                    if (grp.Name == name)
                        count++;
                }
            }
            System.Diagnostics.Debug.WriteLine("{0} groups with the name: {1}", count, name);
            return count;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            string newName = txtName.Text;
            //Before we apply to model, check if another one of the same name exists
            if (GetGroupCount(_mdf, newName) == 0)
            {
                errorProvider.Clear();
                string currentName = _el.Name;
                _el.Name = newName;
                _mdf.UpdateDynamicGroupName(currentName, newName);
                System.Diagnostics.Debug.WriteLine(string.Format("Updated group name {0} -> {1}", currentName, newName));
            }
            else
            {
                errorProvider.SetError(txtName, string.Format(Strings.GroupAlreadyExists, newName));
            }
        }
    }
}
