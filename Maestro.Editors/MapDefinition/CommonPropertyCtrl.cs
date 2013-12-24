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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.MapDefinition
{
    internal partial class CommonPropertyCtrl : UserControl
    {
        public CommonPropertyCtrl()
        {
            InitializeComponent();
        }

        public object SelectedObject
        {
            get { return propGrid.SelectedObject; }
            set 
            { 
                propGrid.SelectedObject = value;
                SetCount(1);
            }
        }

        private void SetCount(int count)
        {
            if (count > 1)
            {
                lnkCount.Text = "(" + string.Format(Strings.SelectedItemCount, count) + ")";
            }
            else
            {
                lnkCount.Text = "";
            }
        }

        public object[] SelectedObjects
        {
            get { return propGrid.SelectedObjects; }
            set 
            { 
                propGrid.SelectedObjects = value;
                SetCount(value.Length);
            }
        }

        private void lnkCount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (propGrid.SelectedObjects != null && propGrid.SelectedObjects.Length > 0)
            {
                new SelectedItemsDialog(propGrid.SelectedObjects).ShowDialog();
            }
        }
    }
}
