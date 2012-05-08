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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    public partial class PointStylePanel : UserControl
    {
        public PointStylePanel()
        {
            InitializeComponent();
        }

        public bool DisplayEnabled
        {
            get { return chkEnable.Checked; }
            set 
            {
                if (value != chkEnable.Checked)
                {
                    chkEnable.Checked = value;
                    var h = this.DisplayEnabledChanged;
                    if (h != null)
                        h(this, EventArgs.Empty);
                }
                chkDisplayAsText.Enabled = chkAllowOverpost.Enabled = value;
            }
        }

        public event EventHandler DisplayEnabledChanged;

        public bool DisplayAsText
        {
            get { return chkDisplayAsText.Checked; }
            set
            {
                if (value != chkDisplayAsText.Checked)
                {
                    chkDisplayAsText.Checked = value;
                    var h = this.DisplayAsTextChanged;
                    if (h != null)
                        h(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler DisplayAsTextChanged;

        public bool AllowOverpost
        {
            get { return chkAllowOverpost.Checked; }
            set
            {
                if (value != chkAllowOverpost.Checked)
                {
                    chkAllowOverpost.Checked = value;
                    var h = this.AllowOverpostChanged;
                    if (h != null)
                        h(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler AllowOverpostChanged;

        private void chkEnable_CheckedChanged(object sender, EventArgs e)
        {
            chkDisplayAsText.Enabled = chkAllowOverpost.Enabled = chkEnable.Checked;
            var h = this.DisplayEnabledChanged;
            if (h != null)
                h(this, EventArgs.Empty);
        }

        private void chkDisplayAsText_CheckedChanged(object sender, EventArgs e)
        {
            var h = this.DisplayAsTextChanged;
            if (h != null)
                h(this, EventArgs.Empty);
        }

        private void chkAllowOverpost_CheckedChanged(object sender, EventArgs e)
        {
            var h = this.AllowOverpostChanged;
            if (h != null)
                h(this, EventArgs.Empty);
        }
    }
}
