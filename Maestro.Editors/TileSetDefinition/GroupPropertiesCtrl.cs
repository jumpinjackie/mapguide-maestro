#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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

using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Maestro.Editors.TileSetDefinition
{
    [ToolboxItem(false)]
    internal partial class GroupPropertiesCtrl : UserControl
    {
        private GroupPropertiesCtrl()
        {
            InitializeComponent();
        }

        public event EventHandler GroupChanged;

        private readonly ITileSetDefinition _mdf;
        private readonly IMapLegendElementBase _el;

        private bool _init = false;

        public GroupPropertiesCtrl(ITileSetDefinition map, IMapLayerGroup group)
            : this()
        {
            _init = true;
            try
            {
                _mdf = map;
                _el = group;
                group.PropertyChanged += WeakEventHandler.Wrap<PropertyChangedEventHandler>(OnGroupChanged, (eh) => group.PropertyChanged -= eh);
                string currentName = group.Name;
                txtName.Text = currentName;
                TextBoxBinder.BindText(txtLegendLabel, group, nameof(group.LegendLabel));
            }
            finally
            {
                _init = false;
            }
        }

        public GroupPropertiesCtrl(ITileSetDefinition map, IBaseMapGroup group)
            : this()
        {
            _init = true;
            try
            {
                _mdf = map;
                _el = group;
                group.PropertyChanged += WeakEventHandler.Wrap<PropertyChangedEventHandler>(OnGroupChanged, (eh) => group.PropertyChanged -= eh);

                txtName.Text = group.Name;
                TextBoxBinder.BindText(txtLegendLabel, group, nameof(group.LegendLabel));
            }
            finally
            {
                _init = false;
            }
        }

        private void OnGroupChanged(object sender, PropertyChangedEventArgs e) => this.GroupChanged?.Invoke(this, EventArgs.Empty);

        private static int GetGroupCount(ITileSetDefinition map, string name)
        {
            int count = 0;
            foreach (var grp in map.BaseMapLayerGroups)
            {
                if (grp.Name == name)
                    count++;
            }
            System.Diagnostics.Debug.WriteLine($"{count} groups with the name: {name}"); //NOXLATE
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
                System.Diagnostics.Debug.WriteLine($"Updated group name {currentName} -> {newName}"); //NOXLATE
            }
            else
            {
                errorProvider.SetError(txtName, string.Format(Strings.GroupAlreadyExists, newName));
            }
        }
    }
}