#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Base.UI
{
    internal partial class ResourceDependencyListDialog : Form
    {
        private ResourceDependencyListDialog()
        {
            InitializeComponent();
        }

        private IList<string> _items;

        public ResourceDependencyListDialog(IList<string> items)
            : this()
        {
            _items = items;
            lstDependencies.DataSource = _items;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var save = DialogFactory.SaveFile())
            {
                save.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickTxt, "txt"); //NOXLATE
                if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.File.WriteAllLines(save.FileName, _items);
                }
            }
        }
    }
}
