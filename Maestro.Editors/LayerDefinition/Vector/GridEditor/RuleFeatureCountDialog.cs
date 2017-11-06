#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maestro.Editors.LayerDefinition.Vector.GridEditor
{
    internal struct RuleCountTotal
    {
        public RuleCountTotal(string filter, string label, int total)
        {
            this.Filter = filter;
            this.Label = label;
            this.Total = total;
        }

        public string Filter { get; }

        public string Label { get; }

        public int Total { get; }
    }

    internal partial class RuleFeatureCountDialog : Form
    {
        private RuleFeatureCountDialog()
        {
            InitializeComponent();
        }

        readonly List<RuleCountTotal> _totals;

        public RuleFeatureCountDialog(IEnumerable<RuleCountTotal> totals)
            : this()
        {
            _totals = new List<RuleCountTotal>(totals.Where(t => !string.IsNullOrEmpty(t.Filter)));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            grdTotals.DataSource = _totals;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
