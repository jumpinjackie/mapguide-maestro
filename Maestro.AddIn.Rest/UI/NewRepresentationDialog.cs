#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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

#endregion Disclaimer / License
using Maestro.AddIn.Rest.Model;
using Maestro.AddIn.Rest.UI.Representation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maestro.AddIn.Rest.UI
{
    public partial class NewRepresentationDialog : Form
    {
        private NewRepresentationDialog()
        {
            InitializeComponent();
        }

        private string _rep;
        private IRepresentationCtrl _ctrl;
        private dynamic _config;
        private RestSourceContext _context;

        public NewRepresentationDialog(string rep, dynamic config, RestSourceContext context)
            : this()
        {
            _context = context;
            _config = config;
            _rep = rep;
            switch(rep)
            {
                case "xml":
                case "geojson":
                    _ctrl = new CruddableRepresentationCtrl(rep, context);
                    break;
                case "csv":
                    _ctrl = new CsvRepresentationCtrl(context);
                    break;
                case "image":
                    _ctrl = new ImageRepresentationCtrl(context);
                    break;
                case "template":
                    _ctrl = new TemplateRepresentationCtrl(context);
                    break;
            }

            if (_ctrl != null)
            {
                _ctrl.Dock = DockStyle.Fill;
                panelSettings.Controls.Add((Control)_ctrl);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dynamic repr = null;
            if (!((IDictionary<string, object>)_config).ContainsKey("Representations")) 
            {
                _config.Representation = new ExpandoObject();   
            }
            repr = _config.Representation;

            dynamic conf = _ctrl.GetOptions();
            ((IDictionary<string, object>)repr)[_rep] = conf;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
