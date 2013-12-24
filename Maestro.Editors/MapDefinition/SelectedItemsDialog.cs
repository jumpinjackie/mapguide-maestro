#region Disclaimer / License
// Copyright (C) 2013, Jackie Ng
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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.MapDefinition
{
    internal partial class SelectedItemsDialog : Form
    {
        private SelectedItemsDialog()
        {
            InitializeComponent();
        }

        public SelectedItemsDialog(object[] items)
            : this()
        {
            foreach (var obj in items)
            {
                var li = obj as Maestro.Editors.MapDefinition.MapLayersSectionCtrl.LayerItemDesigner;
                var gi = obj as Maestro.Editors.MapDefinition.MapLayersSectionCtrl.GroupItemDesigner;
                var bli = obj as Maestro.Editors.MapDefinition.MapLayersSectionCtrl.BaseLayerItemDesigner;
                var bgi = obj as Maestro.Editors.MapDefinition.MapLayersSectionCtrl.BaseGroupItemDesigner;

                ListViewItem lv = null;
                if (li != null)
                {
                    lv = new ListViewItem(li.Item.Text, 0);
                }
                else if (gi != null)
                {
                    lv = new ListViewItem(gi.Item.Text, 1);
                }
                else if (bli != null)
                {
                    lv = new ListViewItem(bli.Item.Text, 0);
                }
                else if (bgi != null)
                {
                    lv = new ListViewItem(bgi.Item.Text, 1);
                }

                if (lv != null)
                    lstItems.Items.Add(lv);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
