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
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Services;
using Maestro.Shared.UI;
using Maestro.Editors.Preview;
using System.IO;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    internal partial class SymbolInstancesDialog : Form
    {
        private IEditorService _edSvc;
        private ICompositeSymbolization _comp;

        private ClassDefinition _cls;
        private string _provider;
        private string _featureSourceId;

        private IMappingService _mappingSvc;
        private ILayerStylePreviewable _preview;
        private BindingList<ParameterModel> _params = new BindingList<ParameterModel>();

        class ParameterModel : INotifyPropertyChanged
        {
            private IParameterOverride _ov;
            private IParameter _pdef;

            public ParameterModel(IParameterOverride ov, IParameter pdef)
            {
                _ov = ov;
                _pdef = pdef;
            }

            [Browsable(false)]
            public IParameterOverride Override { get { return _ov; } }

            [Browsable(false)]
            public IParameter Definition { get { return _pdef; } }

            public string Name { get { return _pdef.DisplayName; } }

            public string Type { get { return _pdef.DataType; } }

            public string Value
            {
                get
                {
                    return _ov.ParameterValue;
                }
                set
                {
                    if (value != _ov.ParameterValue)
                    {
                        _ov.ParameterValue = value;
                        var h = this.PropertyChanged;
                        if (h != null)
                            h(this, new PropertyChangedEventArgs("Value"));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public SymbolInstancesDialog(IEditorService edSvc, ICompositeSymbolization comp, ClassDefinition cls, string provider, string featureSourceId, ILayerStylePreviewable prev)
        {
            InitializeComponent();
            grdOverrides.DataSource = _params;
            _edSvc = edSvc;
            _comp = comp;

            _cls = cls;
            _provider = provider;
            _featureSourceId = featureSourceId;

            _preview = prev;
            var conn = edSvc.GetEditedResource().CurrentConnection;
            if (Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Mapping) >= 0)
            {
                _mappingSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            }

            foreach (var inst in _comp.SymbolInstance)
                AddInstance(inst, false);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdatePreviewImage();
        }

        public bool UseLayerIconPreview
        {
            get { return _mappingSvc != null && _preview != null; }
        }

        private void UpdatePreviewImage()
        {
            using (new WaitCursor(this))
            {
                _edSvc.SyncSessionCopy();
                _previewImg = _mappingSvc.GetLegendImage(_preview.Scale, _preview.LayerDefinition, _preview.ThemeCategory, 4, symPreview.Width, symPreview.Height, _preview.ImageFormat);
                symPreview.Invalidate();
            }
        }

        private Image _previewImg = null;

        private void previewPicture_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (UseLayerIconPreview)
            {
                if (_previewImg != null)
                {
                    e.Graphics.DrawImage(_previewImg, new Point(0, 0));
                }
            }
            else 
            {
                e.Graphics.DrawString(Strings.TextRenderPreviewNotAvailable, Control.DefaultFont, Brushes.Black, new PointF(0, 0));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RenderPreview(ISymbolInstance symInst, ListViewItem item)
        {
            string previewKey = null;
            if (!string.IsNullOrEmpty(item.ImageKey))
                previewKey = item.ImageKey;
            else
                previewKey = Guid.NewGuid().ToString();

            if (imgPreviews.Images.ContainsKey(previewKey))
            {
                Image img = imgPreviews.Images[previewKey];
                imgPreviews.Images.RemoveByKey(previewKey);
                img.Dispose();
            }

            var conn = _edSvc.GetEditedResource().CurrentConnection;
            ISymbolDefinitionBase previewSymbol = null;
            string resId = "Session:" + conn.SessionID + "//" + Guid.NewGuid().ToString() + ".SymbolDefinition";
            switch (symInst.Reference.Type)
            {
                case SymbolInstanceType.Inline:
                    previewSymbol = (ISymbolDefinitionBase)((ISymbolInstanceReferenceInline)symInst.Reference).SymbolDefinition.Clone();
                    //Inline symbol definitions will have schema and version stripped. We need to add them back
                    previewSymbol.SetSchemaAttributes();
                    previewSymbol.ResourceID = resId;
                    using (var stream = ApplyOverrides(previewSymbol, symInst.ParameterOverrides))
                    {
                        conn.ResourceService.SetResourceXmlData(resId, stream);
                    }
                    previewSymbol = (ISymbolDefinitionBase)conn.ResourceService.GetResource(resId);
                    break;
                case SymbolInstanceType.Reference:
                    previewSymbol = (ISymbolDefinitionBase)conn.ResourceService.GetResource(((ISymbolInstanceReferenceLibrary)symInst.Reference).ResourceId);
                    previewSymbol = (ISymbolDefinitionBase)previewSymbol.Clone();
                    previewSymbol.ResourceID = resId;
                    using (var stream = ApplyOverrides(previewSymbol, symInst.ParameterOverrides))
                    {
                        conn.ResourceService.SetResourceXmlData(resId, stream);
                    }
                    previewSymbol = (ISymbolDefinitionBase)conn.ResourceService.GetResource(resId);
                    break;
            }
            var res = DefaultResourcePreviewer.GenerateSymbolDefinitionPreview(conn, previewSymbol, imgPreviews.ImageSize.Width, imgPreviews.ImageSize.Height);
            imgPreviews.Images.Add(previewKey, res.ImagePreview);
            item.ImageKey = previewKey;
        }

        private static Stream ApplyOverrides(ISymbolDefinitionBase previewSymbol, IParameterOverrideCollection parameterOverrideCollection)
        {
            foreach (var ov in parameterOverrideCollection.Override)
            {
                foreach (var pdef in previewSymbol.GetParameters())
                {
                    if (pdef.Name == ov.ParameterIdentifier)
                    {
                        pdef.DefaultValue = ov.ParameterValue;
                        break;
                    }
                }
            }
            return ResourceTypeRegistry.Serialize(previewSymbol);
        }

        private void AddInstance(ISymbolInstance symRef, bool add)
        {
            var li = new ListViewItem();
            li.Tag = symRef;

            var sym = GetSymbolDefinition(symRef);
            if (li.ImageIndex == 0)
                li.Text = sym.Name + " (" + ((ISymbolInstanceReferenceLibrary)symRef.Reference).ResourceId + ")";
            else
                li.Text = sym.Name + " (" + Strings.InlineSymbolDefinition + ")";

            lstInstances.Items.Add(li);
            if (add)
                _comp.AddSymbolInstance(symRef);
            li.Selected = (lstInstances.Items.Count == 1);
            RenderPreview(symRef, li);
        }

        private void referenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.ResourceService, ResourceTypes.SymbolDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    var symRef = _comp.CreateSymbolReference(picker.ResourceID);
                    AddInstance(symRef, true);
                }
            }
        }

        private void inlineSimpleSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = (ILayerDefinition)_edSvc.GetEditedResource();
            var vl = (IVectorLayerDefinition)res.SubLayer;
            if (vl.SymbolDefinitionVersion == null)
                throw new InvalidOperationException(Strings.ErrorLayerDefnitionDoesNotSupportCompositeSymbolization);
            var ssym = ObjectFactory.CreateSimpleSymbol(_edSvc.GetEditedResource().CurrentConnection, 
                                                        vl.SymbolDefinitionVersion, 
                                                        "InlineSimpleSymbol", //NOXLATE
                                                        Strings.TextInlineSimpleSymbol);

            var instance = _comp.CreateInlineSimpleSymbol(ssym);
            AddInstance(instance, true);
        }

        private void inlineCompoundSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = (ILayerDefinition)_edSvc.GetEditedResource();
            var vl = (IVectorLayerDefinition)res.SubLayer;
            if (vl.SymbolDefinitionVersion == null)
                throw new InvalidOperationException(Strings.ErrorLayerDefnitionDoesNotSupportCompositeSymbolization);
            var csym = ObjectFactory.CreateCompoundSymbol(_edSvc.GetEditedResource().CurrentConnection,
                                                          vl.SymbolDefinitionVersion,
                                                          "InlineCompoundSymbol",
                                                          Strings.TextInlineCompoundSymbol); //NOXLATE

            var instance = _comp.CreateInlineCompoundSymbol(csym);
            AddInstance(instance, true);
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
                _params.Clear();

                ISymbolDefinitionBase sym = GetSymbolDefinition(symRef);

                //Add existing overrides
                foreach (var p in symRef.ParameterOverrides.Override)
                {
                    IParameter pdef = sym.GetParameter(p.ParameterIdentifier);
                    var model = new ParameterModel(p, pdef);
                    _params.Add(model);
                }
                //Now add available parameters
                PopulateAvailableParameters(symRef, sym);

                btnEditInstanceProperties.Enabled = true;
                btnEditComponent.Enabled = true;
            }
            else
            {
                btnEditInstanceProperties.Enabled = false;
                btnEditComponent.Enabled = false;
            }
        }

        private ISymbolDefinitionBase GetSymbolDefinition(ISymbolInstance symRef)
        {
            ISymbolDefinitionBase sym = null;
            if (symRef.Reference.Type == SymbolInstanceType.Reference)
            {
                sym = (ISymbolDefinitionBase)_edSvc.ResourceService.GetResource(((ISymbolInstanceReferenceLibrary)symRef.Reference).ResourceId);
            }
            else if (symRef.Reference.Type == SymbolInstanceType.Inline)
            {
                var inline = (ISymbolInstanceReferenceInline)symRef.Reference;
                sym = inline.SymbolDefinition;
            }
            return sym;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshPreviews(true, true);
        }

        private void RefreshPreviews(bool refreshMain, bool refreshComponents)
        {
            if (refreshMain)
            {
                UpdatePreviewImage();
            }

            if (refreshComponents)
            {
                foreach (ListViewItem li in lstInstances.Items)
                {
                    var symRef = (ISymbolInstance)li.Tag;
                    RenderPreview(symRef, li);
                }
                lstInstances.Invalidate();
            }
        }

        private void btnEditInstanceProperties_Click(object sender, EventArgs e)
        {
            if (lstInstances.SelectedItems.Count == 1)
            {
                var it = lstInstances.SelectedItems[0];
                ISymbolInstance symRef = (ISymbolInstance)it.Tag;

                using (var diag = new SymbolInstancePropertiesDialog(symRef, _edSvc, _cls, _featureSourceId, _provider))
                {
                    diag.ShowDialog();
                }
            }
        }

        private void btnEditAsXml_Click(object sender, EventArgs e)
        {
            string xml = _comp.ToXml();
            XmlEditorDialog diag = new XmlEditorDialog();
            diag.XmlContent = xml;
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _comp.UpdateFromXml(diag.XmlContent);
                RefreshPreviews(true, false);
                lstInstances.Clear();
                imgPreviews.Images.Clear();
                foreach (var sym in _comp.SymbolInstance)
                {
                    AddInstance(sym, false);
                }
            }
        }

        private void lnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RefreshPreviews(true, true);
        }

        private void btnEditComponent_Click(object sender, EventArgs e)
        {
            var it = lstInstances.SelectedItems[0];
            ISymbolInstance symRef = (ISymbolInstance)it.Tag;
            var c = CreateSymbolDefinitionEditor(symRef, _edSvc.ResourceService);
            c.Dock = DockStyle.Fill;
            using (var ed = new EditorTemplateForm())
            {
                ed.Text = Strings.EditSymbolDefinition;
                ed.ItemPanel.Controls.Add(c);
                ed.ManualSizeManagement = true;
                ed.Width = 800;
                ed.Height = 600;
                if (ed.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _edSvc.HasChanged();
                }
            }
        }

        private void PopulateAvailableParameters(ISymbolInstance symRef, ISymbolDefinitionBase symbol)
        {
            btnAddProperty.DropDown.Items.Clear();
            PopulateParameterList(symRef, symbol);
        }

        private void PopulateParameterList(ISymbolInstance symRef, ISymbolDefinitionBase sym)
        {
            foreach (var p in sym.GetParameters())
            {
                var param = p;
                var btn = new ToolStripButton(p.Name, null, (s, e) =>
                {
                    AddParameterOverride(symRef, sym, param);
                });
                btn.ToolTipText = p.Description;
                btnAddProperty.DropDown.Items.Insert(0, btn);
            }
            btnAddProperty.DropDown.Items.Add(toolStripSeparator1);
            btnAddProperty.DropDown.Items.Add(refreshToolStripMenuItem);
        }

        private void AddParameterOverride(ISymbolInstance symRef, ISymbolDefinitionBase sym, IParameter param)
        {
            foreach (var p in _params)
            {
                if (p.Override.ParameterIdentifier == param.Name)
                {
                    MessageBox.Show(Strings.ParameterOverrideExists);
                    return;
                }
            }

            var ov = symRef.ParameterOverrides.CreateParameterOverride(sym.Name, param.Name);
            ov.SymbolName = sym.Name;
            ov.ParameterValue = param.DefaultValue;

            var model = new ParameterModel(ov, param);
            _params.Add(model);
            symRef.ParameterOverrides.AddOverride(ov);
            _edSvc.HasChanged();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grdOverrides.SelectedRows.Count == 1)
            {
                var ov = (ParameterModel)grdOverrides.SelectedRows[0].DataBoundItem;
                string expr = _edSvc.EditExpression(ov.Value, _cls, _provider, _featureSourceId, true);
                if (expr != null)
                {
                    ov.Value = expr;
                    _edSvc.HasChanged();
                }
            }
        }

        private void btnDeleteProperty_Click(object sender, EventArgs e)
        {
            if (grdOverrides.SelectedRows.Count == 1 && lstInstances.SelectedItems.Count == 1)
            {
                var it = lstInstances.SelectedItems[0];
                ISymbolInstance symRef = (ISymbolInstance)it.Tag;
                var ov = (ParameterModel)grdOverrides.SelectedRows[0].DataBoundItem;
                _params.Remove(ov);
                symRef.ParameterOverrides.RemoveOverride(ov.Override);
                _edSvc.HasChanged();
            }
        }

        private void btnPropertyInfo_Click(object sender, EventArgs e)
        {
            if (grdOverrides.SelectedRows.Count == 1)
            {
                var ov = (ParameterModel)grdOverrides.SelectedRows[0].DataBoundItem;
                string text = string.Format(Strings.PropertyInfo,
                    Environment.NewLine,
                    ov.Definition.Name,
                    ov.Definition.Identifier,
                    ov.Definition.Description,
                    ov.Definition.DataType,
                    ov.Definition.DefaultValue);
                MessageBox.Show(text);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstInstances.SelectedItems.Count == 1)
            {
                var it = lstInstances.SelectedItems[0];
                ISymbolInstance symRef = (ISymbolInstance)it.Tag;
                PopulateAvailableParameters(symRef, GetSymbolDefinition(symRef));
            }
        }

        private void grdOverrides_SelectionChanged(object sender, EventArgs e)
        {
            btnPropertyInfo.Enabled = btnEditProperty.Enabled = btnDeleteProperty.Enabled = (grdOverrides.SelectedRows.Count == 1);
        }

        private Control CreateSymbolDefinitionEditor(ISymbolInstance symRef, IResourceService resSvc)
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

        private void solidFillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = (ILayerDefinition)_edSvc.GetEditedResource();
            var vl = (IVectorLayerDefinition)res.SubLayer;
            if (vl.SymbolDefinitionVersion == null)
                throw new InvalidOperationException(Strings.ErrorLayerDefnitionDoesNotSupportCompositeSymbolization);
            var ssym = ObjectFactory.CreateSimpleSolidFill(_edSvc.GetEditedResource().CurrentConnection,
                                                           vl.SymbolDefinitionVersion);

            var instance = _comp.CreateInlineSimpleSymbol(ssym);
            AddInstance(instance, true);
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = (ILayerDefinition)_edSvc.GetEditedResource();
            var vl = (IVectorLayerDefinition)res.SubLayer;
            if (vl.SymbolDefinitionVersion == null)
                throw new InvalidOperationException(Strings.ErrorLayerDefnitionDoesNotSupportCompositeSymbolization);
            var ssym = ObjectFactory.CreateSimpleSolidLine(_edSvc.GetEditedResource().CurrentConnection,
                                                           vl.SymbolDefinitionVersion);

            var instance = _comp.CreateInlineSimpleSymbol(ssym);
            AddInstance(instance, true);
        }

        private void textLabelPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = (ILayerDefinition)_edSvc.GetEditedResource();
            var vl = (IVectorLayerDefinition)res.SubLayer;
            if (vl.SymbolDefinitionVersion == null)
                throw new InvalidOperationException(Strings.ErrorLayerDefnitionDoesNotSupportCompositeSymbolization);
            var ssym = ObjectFactory.CreateSimpleLabel(_edSvc.GetEditedResource().CurrentConnection,
                                                       vl.SymbolDefinitionVersion, 
                                                       GeometryContextType.Point);

            var instance = _comp.CreateInlineSimpleSymbol(ssym);
            AddInstance(instance, true);
        }

        private void textLabelLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = (ILayerDefinition)_edSvc.GetEditedResource();
            var vl = (IVectorLayerDefinition)res.SubLayer;
            if (vl.SymbolDefinitionVersion == null)
                throw new InvalidOperationException(Strings.ErrorLayerDefnitionDoesNotSupportCompositeSymbolization);
            var ssym = ObjectFactory.CreateSimpleLabel(_edSvc.GetEditedResource().CurrentConnection,
                                                       vl.SymbolDefinitionVersion,
                                                       GeometryContextType.LineString);

            var instance = _comp.CreateInlineSimpleSymbol(ssym);
            AddInstance(instance, true);
        }

        private void textLabelPolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = (ILayerDefinition)_edSvc.GetEditedResource();
            var vl = (IVectorLayerDefinition)res.SubLayer;
            if (vl.SymbolDefinitionVersion == null)
                throw new InvalidOperationException(Strings.ErrorLayerDefnitionDoesNotSupportCompositeSymbolization);
            var ssym = ObjectFactory.CreateSimpleLabel(_edSvc.GetEditedResource().CurrentConnection,
                                                       vl.SymbolDefinitionVersion,
                                                       GeometryContextType.Polygon);

            var instance = _comp.CreateInlineSimpleSymbol(ssym);
            AddInstance(instance, true);
        }

        private void pointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = (ILayerDefinition)_edSvc.GetEditedResource();
            var vl = (IVectorLayerDefinition)res.SubLayer;
            if (vl.SymbolDefinitionVersion == null)
                throw new InvalidOperationException(Strings.ErrorLayerDefnitionDoesNotSupportCompositeSymbolization);
            var ssym = ObjectFactory.CreateSimplePoint(_edSvc.GetEditedResource().CurrentConnection,
                                                       vl.SymbolDefinitionVersion);

            var instance = _comp.CreateInlineSimpleSymbol(ssym);
            AddInstance(instance, true);
        }
    }
}
