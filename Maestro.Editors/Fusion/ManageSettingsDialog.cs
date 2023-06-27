#region Disclaimer / License

// Copyright (C) 2023, Jackie Ng
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

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Maestro.Editors.Fusion
{
    public partial class ManageSettingsDialog : Form
    {
        private ManageSettingsDialog()
        {
            InitializeComponent();
        }

        readonly IServerConnection _conn;
        readonly IApplicationDefinition _appDef;

        public ManageSettingsDialog(IServerConnection conn, IApplicationDefinition appDef)
            : this()
        {
            _conn = conn;
            _appDef = appDef;
        }

        public ViewerSettings Settings { get; set; }

        private BindingList<Setting> _items = new BindingList<Setting>();


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var ser = new XmlSerializer(typeof(ViewerSettings));
            var el = _appDef.GetValue("ViewerSettings", ExtensionElementContentKind.OuterXml);
            try
            {
                if (!string.IsNullOrEmpty(el))
                {
                    using var sr = new StringReader(el);
                    this.Settings = (ViewerSettings)ser.Deserialize(sr);
                }
            }
            catch { }
            finally
            {
                if (this.Settings == null)
                {
                    this.Settings = new ViewerSettings()
                    {
                        Setting = new List<Setting>()
                    };
                }
            }

            foreach (var it in this.Settings.Setting)
            {
                _items.Add(it);
            }
            grdSettings.DataSource = _items;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnApplyAndClose_Click(object sender, EventArgs e)
        {
            this.Settings.Setting.Clear();
            this.Settings.Setting.AddRange(_items.Where(ent => !string.IsNullOrEmpty(ent.Value)));
            this.DialogResult = DialogResult.OK;
        }
    }
}
