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
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using Maestro.Editors.SymbolDefinition;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    public partial class SymbolInstancesDialog : Form
    {
        private IEditorService _edSvc;
        private ICompositeSymbolization _comp;

        public SymbolInstancesDialog(IEditorService edSvc, ICompositeSymbolization comp)
        {
            InitializeComponent();
            _edSvc = edSvc;
            _comp = comp;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void referenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            using (var picker = new ResourcePicker(_edSvc.ResourceService, ResourceTypes.SymbolDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    var symRef = _comp.CreateSymbolReference(picker.ResourceID);
                    //var ctrl = new SimpleSymbolReferenceCtrl(_edSvc.ResourceService, symRef);
                    
                    //var ctrl2 = new SymbolInstanceSettingsCtrl(
                }
            }
        }

        private void inlineSimpleSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void inlineCompoundSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}
