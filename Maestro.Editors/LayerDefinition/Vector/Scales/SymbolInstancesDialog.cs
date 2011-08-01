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
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using Maestro.Editors.LayerDefinition.Vector.Scales.SymbolInstanceEditors;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    internal partial class SymbolInstancesDialog : Form
    {
        private IEditorService _edSvc;
        private ICompositeSymbolization _comp;

        private ClassDefinition _cls;
        private string _provider;
        private string _featureSourceId;

        public SymbolInstancesDialog(IEditorService edSvc, ICompositeSymbolization comp, ClassDefinition cls, string provider, string featureSourceId)
        {
            InitializeComponent();
            _edSvc = edSvc;
            _comp = comp;

            _cls = cls;
            _provider = provider;
            _featureSourceId = featureSourceId;

            foreach (var inst in _comp.SymbolInstance)
                AddInstance(inst, false);
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
                if (LastSelectedFolder.IsSet)
                    picker.SetStartingPoint(LastSelectedFolder.FolderId);

                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    var symRef = _comp.CreateSymbolReference(picker.ResourceID);
                    AddInstance(symRef, true);
                }
            }
        }

        private void AddInstance(ISymbolInstance symRef, bool add)
        {
            var li = new ListViewItem();
            li.ImageIndex = (symRef.Reference.Type == SymbolInstanceType.Reference) ? 0 : 1;
            li.Tag = symRef;
            if (li.ImageIndex == 0)
                li.Text = ((ISymbolInstanceReferenceLibrary)symRef.Reference).ResourceId;
            else
                li.Text = Properties.Resources.InlineSymbolDefinition;

            lstInstances.Items.Add(li);
            if (add)
                _comp.AddSymbolInstance(symRef);
            li.Selected = (lstInstances.Items.Count == 1);
        }

        private void inlineSimpleSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void inlineCompoundSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstInstances.SelectedItems.Count == 1)
            {
                var it = lstInstances.SelectedItems[0];
                ISymbolInstance symRef = (ISymbolInstance)it.Tag;
                _comp.RemoveSymbolInstance(symRef);
                lstInstances.Items.Remove(it);
            }
        }

        private void lstInstances_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstInstances.SelectedItems.Count == 1)
            {
                var it = lstInstances.SelectedItems[0];
                ISymbolInstance symRef = (ISymbolInstance)it.Tag;
                var c = new SymbolInstanceSettingsCtrl();
                c.SetContent(symRef, _edSvc, _cls, _provider, _featureSourceId);
                c.Dock = DockStyle.Fill;
                splitContainer1.Panel2.Controls.Clear();
                splitContainer1.Panel2.Controls.Add(c);
            }
        }
    }
}
