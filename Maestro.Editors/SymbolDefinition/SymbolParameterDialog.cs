#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;

namespace Maestro.Editors.SymbolDefinition
{
    internal partial class SymbolParameterDialog : Form
    {
        private IParameter _p;
        private bool _init = false;

        public SymbolParameterDialog(Version ver, IParameter p)
        {
            InitializeComponent();
            _p = p;
            _init = true;
            try
            {
                txtIdentifier.Text = _p.Identifier;
                txtDisplayName.Text = _p.DisplayName;
                txtDescription.Text = _p.Description;
                txtDefaultValue.Text = _p.DefaultValue;

                if (ver >= new Version(1, 1, 0))
                {
                    cmbDataType.DataSource = Enum.GetValues(typeof(DataType2));
                }
                else
                {
                    cmbDataType.DataSource = Enum.GetValues(typeof(DataType));
                }

                int idx = cmbDataType.FindString(_p.DataType);
                if (idx >= 0)
                    cmbDataType.SelectedIndex = idx;
            }
            finally
            {
                _init = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtIdentifier_TextChanged(object sender, EventArgs e)
        {
            if (_init) return;
            _p.Identifier = txtIdentifier.Text;
        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            if (_init) return;
            _p.DisplayName = txtDisplayName.Text;
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (_init) return;
            _p.Description = txtDescription.Text;
        }

        private void txtDefaultValue_TextChanged(object sender, EventArgs e)
        {
            if (_init) return;
            _p.DefaultValue = txtDefaultValue.Text;
        }

        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_init) return;
            _p.DataType = cmbDataType.SelectedItem.ToString();
        }
    }
}
