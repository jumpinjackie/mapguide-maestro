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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    using SymbolInstanceEditors;
    using OSGeo.MapGuide.MaestroAPI.Services;
    using OSGeo.MapGuide.ObjectModels.LayerDefinition;
    using OSGeo.MapGuide.MaestroAPI.Schema;
    using Maestro.Editors.SymbolDefinition;

    [ToolboxItem(false)]
    internal partial class SymbolInstanceSettingsCtrl : UserControl
    {
        public SymbolInstanceSettingsCtrl()
        {
            InitializeComponent();
            grdOverrides.DataSource = _params;
        }

        private BindingList<IParameterOverride> _params = new BindingList<IParameterOverride>();
        private ISymbolInstance _symRef;
        private IEditorService _edSvc;

        private ClassDefinition _cls;
        private string _provider;
        private string _featureSourceId;

        public void SetContent(ISymbolInstance symRef, IEditorService edSvc, ClassDefinition cls, string provider, string featureSourceId)
        {
            _symRef = symRef;
            _edSvc = edSvc;

            _cls = cls;
            _provider = provider;
            _featureSourceId = featureSourceId;

            _params.Clear();
            //Add existing overrides
            foreach (var p in symRef.ParameterOverrides.Override)
            {
                _params.Add(p);
            }
            //Now add available parameters
            PopulateAvailableParameters();
            grpSettings.Controls.Clear();
            Control c = CreateEditor(symRef, edSvc.ResourceService);
            c.Dock = DockStyle.Fill;
            grpSettings.Controls.Add(c);
        }

        private void PopulateAvailableParameters()
        {
            btnAdd.DropDown.Items.Clear();
            if (_symRef.Reference.Type == SymbolInstanceType.Reference)
            {
                var sym = (ISymbolDefinitionBase)_edSvc.ResourceService.GetResource(((ISymbolInstanceReferenceLibrary)_symRef.Reference).ResourceId);
                PopulateParameterList(sym);
            }
            else if (_symRef.Reference.Type == SymbolInstanceType.Inline)
            {
                var inline = (ISymbolInstanceReferenceInline)_symRef.Reference;
                PopulateParameterList(inline.SymbolDefinition);
            }
            else
            {
                throw new Exception();
            }
        }

        private void PopulateParameterList(ISymbolDefinitionBase sym)
        {
            foreach (var p in sym.GetParameters())
            {
                var param = p;
                var btn = new ToolStripButton(p.Name, null, (s, e) =>
                {
                    AddParameterOverride(sym, param);
                });
                btnAdd.DropDown.Items.Insert(0, btn);
            }
            btnAdd.DropDown.Items.Add(toolStripSeparator1);
            btnAdd.DropDown.Items.Add(refreshToolStripMenuItem);
        }

        private void AddParameterOverride(ISymbolDefinitionBase sym, IParameter param)
        {
            foreach (var p in _params)
            {
                if (p.ParameterIdentifier == param.Name)
                {
                    MessageBox.Show(Strings.ParameterOverrideExists);
                    return;
                }
            }

            var ov = _symRef.ParameterOverrides.CreateParameterOverride(sym.Name, param.Name);
            ov.SymbolName = sym.Name;

            _params.Add(ov);
            _symRef.ParameterOverrides.AddOverride(ov);
            this.RaiseDirty();
        }

        private void RaiseDirty()
        {
            var handler = this.Dirty;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler Dirty;

        private Control CreateEditor(ISymbolInstance symRef, IResourceService resSvc)
        {
            Check.NotNull(symRef, "symRef"); //NOXLATE
            if (symRef.Reference.Type == SymbolInstanceType.Reference)
            {
                return new ReferenceCtrl((ISymbolInstanceReferenceLibrary)symRef.Reference, resSvc);
            }
            else
            {
                var inline = (ISymbolInstanceReferenceInline)symRef.Reference;
                var symEditor = new SymbolEditorService(_edSvc, inline.SymbolDefinition);
                if (inline.SymbolDefinition.Type == SymbolDefinitionType.Simple)
                {
                    var sed = new SimpleSymbolDefinitionEditorCtrl();
                    sed.Bind(symEditor);
                    return sed;
                }
                else
                {
                    var sed = new CompoundSymbolDefinitionEditorCtrl();
                    sed.Bind(symEditor);
                    return sed;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grdOverrides.SelectedRows.Count == 1)
            {
                var ov = (IParameterOverride)grdOverrides.SelectedRows[0].DataBoundItem;
                string expr = _edSvc.EditExpression(ov.ParameterValue, _cls, _provider, _featureSourceId, true);
                if (expr != null)
                {
                    ov.ParameterValue = expr;
                    this.RaiseDirty();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdOverrides.SelectedRows.Count == 1)
            {
                var ov = (IParameterOverride)grdOverrides.SelectedRows[0].DataBoundItem;
                _params.Remove(ov);
                _symRef.ParameterOverrides.RemoveOverride(ov);
                this.RaiseDirty();
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopulateAvailableParameters();
        }

        private void grdOverrides_SelectionChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = btnDelete.Enabled = (grdOverrides.SelectedRows.Count == 1);
        }
    }
}
