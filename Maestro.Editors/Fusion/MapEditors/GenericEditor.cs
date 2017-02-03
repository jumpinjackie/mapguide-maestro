#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using System.Windows.Forms;

namespace Maestro.Editors.Fusion.MapEditors
{
    internal partial class GenericEditor : UserControl
    {
        private IEditorService _edSvc;
        private IMap _map;

        public GenericEditor(IEditorService edSvc, IMap map)
        {
            InitializeComponent();
            txtXml.SetHighlighting("XML"); //NOXLATE
            _edSvc = edSvc;
            _map = map;
            txtXml.Text = map.AsXml();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Checked = false;
            _map.FromXml(txtXml.Text);
            MessageBox.Show(Strings.OptionsSyncedToDocument);
            _edSvc.HasChanged();
        }

        private void txtXml_TextChanged(object sender, EventArgs e)
        {
            btnSave.Checked = true;
        }
    }
}