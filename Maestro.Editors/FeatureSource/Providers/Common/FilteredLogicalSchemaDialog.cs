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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.FeatureSource.Providers.Common
{
    internal partial class FilteredLogicalSchemaDialog : Form
    {
        public FilteredLogicalSchemaDialog(string [] names)
        {
            InitializeComponent();
            foreach (var n in names)
            {
                chkListClassNames.Items.Add(n, false);
            }
        }

        public string[] ClassNames
        {
            get
            {
                var items = new List<string>();
                foreach (var item in chkListClassNames.CheckedItems)
                {
                    items.Add(item.ToString());
                }
                return items.ToArray();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (chkListClassNames.CheckedItems.Count == 0)
            {
                MessageBox.Show(Strings.TextNoItemSelected);
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListClassNames.Items.Count; i++)
            {
                chkListClassNames.SetItemChecked(i, true);
            }
        }

        private void btnCheckNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListClassNames.Items.Count; i++)
            {
                chkListClassNames.SetItemChecked(i, false);
            }
        }
    }
}
