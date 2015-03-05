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

#endregion Disclaimer / License

using Maestro.Editors.Common;
using Maestro.Editors.Generic;
using Maestro.Editors.LayerDefinition.Vector.Scales.SymbolInstanceEditors;
using Maestro.Editors.LayerDefinition.Vector.Scales.SymbolParamEditors;
using Maestro.Editors.Preview;
using Maestro.Editors.SymbolDefinition;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

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

        private class ParameterModel : INotifyPropertyChanged
        {
            private IParameterOverride _ov;
            private IParameter _pdef;

            private string _name;

            public ParameterModel(IParameterOverride ov, IParameter pdef)
            {
                _ov = ov;
                _pdef = pdef;
                _name = pdef.DisplayName.Replace("&amp;", "");
            }

            [Browsable(false)]
            public IParameterOverride Override { get { return _ov; } }

            [Browsable(false)]
            public IParameter Definition { get { return _pdef; } }

            public string Name { get { return _name; } }

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
            var conn = edSvc.CurrentConnection;
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

            var conn = _edSvc.CurrentConnection;
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
            return ObjectFactory.Serialize(previewSymbol);
        }

        private void AddInstance(ISymbolInstance symRef, bool add)
        {
            var li = new ListViewItem();
            li.Tag = symRef;

            SetListViewItemLabel(symRef, li);

            lstInstances.Items.Add(li);
            btnUp.Enabled = btnDown.Enabled = (lstInstances.Items.Count > 1);

            if (add)
                _comp.AddSymbolInstance(symRef);
            li.Selected = (lstInstances.Items.Count == 1);
            RenderPreview(symRef, li);
        }

        private void SetListViewItemLabel(ISymbolInstance symRef, ListViewItem li)
        {
            var sym = GetSymbolDefinition(symRef);
            if (symRef.Reference.Type == SymbolInstanceType.Reference)
            {
                ISymbolInstance2 symRef2 = symRef as ISymbolInstance2;
                if (symRef2 != null)
                    li.Text = sym.Name + " (" + symRef2.GeometryContext + " - " + ((ISymbolInstanceReferenceLibrary)symRef.Reference).ResourceId + ")";
                else
                    li.Text = sym.Name + " (" + ((ISymbolInstanceReferenceLibrary)symRef.Reference).ResourceId + ")";
            }
            else
            {
                ISymbolInstance2 symRef2 = symRef as ISymbolInstance2;
                if (symRef2 != null)
                    li.Text = sym.Name + " (" + symRef2.GeometryContext + " - " + Strings.InlineSymbolDefinition + ")";
                else
                    li.Text = sym.Name + " (" + Strings.InlineSymbolDefinition + ")";
            }
        }

        private void referenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.CurrentConnection, ResourceTypes.SymbolDefinition.ToString(), ResourcePickerMode.OpenResource))
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
            var ssym = ObjectFactory.CreateSimpleSymbol(vl.SymbolDefinitionVersion,
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
            var csym = ObjectFactory.CreateCompoundSymbol(vl.SymbolDefinitionVersion,
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

                btnSaveExternal.Enabled = (symRef.Reference.Type == SymbolInstanceType.Inline);
                btnEditInstanceProperties.Enabled = true;
                btnEditComponent.Enabled = true;
            }
            else
            {
                btnSaveExternal.Enabled = false;
                btnEditInstanceProperties.Enabled = false;
                btnEditComponent.Enabled = false;
            }
        }

        private ISymbolDefinitionBase GetSymbolDefinition(ISymbolInstance symRef)
        {
            ISymbolDefinitionBase sym = null;
            if (symRef.Reference.Type == SymbolInstanceType.Reference)
            {
                sym = (ISymbolDefinitionBase)_edSvc.CurrentConnection.ResourceService.GetResource(((ISymbolInstanceReferenceLibrary)symRef.Reference).ResourceId);
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
                    SetListViewItemLabel(symRef, it);
                }
            }
        }

        private void btnEditAsXml_Click(object sender, EventArgs e)
        {
            string xml = _comp.ToXml();
            XmlEditorDialog diag = new XmlEditorDialog();
            diag.OnlyValidateWellFormedness = true;
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
            var c = CreateSymbolDefinitionEditor(symRef);
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
                var btn = new ToolStripButton(p.DisplayName.Replace("&amp;", "&"), null, (s, e) =>
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

        private void viaExpressionEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdOverrides.SelectedRows.Count == 1)
            {
                var ov = (ParameterModel)grdOverrides.SelectedRows[0].DataBoundItem;
                string expr = _edSvc.EditExpression(ov.Value, _cls, _provider, _featureSourceId, ExpressionEditorMode.Expression, true);
                if (expr != null)
                {
                    ov.Value = expr;
                    _edSvc.HasChanged();
                }
            }
        }

        private void viaEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdOverrides.SelectedRows.Count == 1 && lstInstances.SelectedItems.Count == 1)
            {
                var it = lstInstances.SelectedItems[0];
                ISymbolInstance symRef = (ISymbolInstance)it.Tag;
                var ov = (ParameterModel)grdOverrides.SelectedRows[0].DataBoundItem;
                Func<string> editAction = GetParameterEditorAction(ov.Type, ov.Value);
                if (editAction != null)
                {
                    string expr = editAction();
                    if (expr != null)
                    {
                        ov.Value = expr;
                        _edSvc.HasChanged();
                    }
                }
                else //Fallback to Expression Editor
                {
                    string expr = _edSvc.EditExpression(ov.Value, _cls, _provider, _featureSourceId, ExpressionEditorMode.Expression, true);
                    if (expr != null)
                    {
                        ov.Value = expr;
                        _edSvc.HasChanged();
                    }
                }
            }
        }

        private Func<string> GetParameterEditorAction(string paramType, string currentValue)
        {
            DataType2 dt2;
            if (Enum.TryParse(paramType, out dt2))
            {
                switch (dt2)
                {
                    case DataType2.Boolean:
                    case DataType2.Bold:
                    case DataType2.Italic:
                    case DataType2.Overlined:
                    case DataType2.Underlined:
                        return () =>
                        {
                            using (var ed = new BooleanEditor())
                            {
                                bool b = default(bool);
                                bool.TryParse(currentValue, out b);
                                ed.SetDataType(dt2, b);
                                if (ed.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    return ed.Result ? "true" : "false"; //NOXLATE
                                }
                            }
                            return null;
                        };
                    case DataType2.Integer:
                    case DataType2.Real:
                    case DataType2.Angle:
                    case DataType2.EndOffset:
                    case DataType2.FontHeight:
                    case DataType2.LineSpacing:
                    case DataType2.LineWeight:
                    case DataType2.ObliqueAngle:
                    case DataType2.RepeatX:
                    case DataType2.RepeatY:
                    case DataType2.TrackSpacing:
                    case DataType2.StartOffset:
                        return () =>
                        {
                            using (var ed = new NumberEditor())
                            {
                                decimal d = default(decimal);
                                decimal.TryParse(currentValue, out d);
                                ed.SetDataType(dt2, d);
                                if (ed.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    decimal result = ed.Value;
                                    if (dt2 == DataType2.Integer)
                                        return Convert.ToInt32(result).ToString(CultureInfo.InvariantCulture);
                                    else
                                        return Convert.ToDouble(result).ToString(CultureInfo.InvariantCulture);
                                }
                            }
                            return null;
                        };
                    case DataType2.Color:
                    case DataType2.FillColor:
                    case DataType2.FrameFillColor:
                    case DataType2.FrameLineColor:
                    case DataType2.GhostColor:
                    case DataType2.LineColor:
                    case DataType2.TextColor:
                        {
                            return () =>
                            {
                                using (var picker = new ColorPickerDialog())
                                {
                                    try
                                    {
                                        picker.SelectedColor = Utility.ParseHTMLColorARGB((currentValue ?? "").Replace("0x", ""));
                                    }
                                    catch { }
                                    if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        return "0x" + Utility.SerializeHTMLColorARGB(picker.SelectedColor, true);
                                    }
                                }
                                return null;
                            };
                        }
                    case DataType2.Justification:
                        {
                            var values = Enum.GetValues(typeof(OSGeo.MapGuide.ObjectModels.SymbolDefinition.Justification));
                            return GetEnumPicker(dt2, values);
                        }
                    case DataType2.HorizontalAlignment:
                        {
                            var values = Enum.GetValues(typeof(OSGeo.MapGuide.ObjectModels.SymbolDefinition.HorizontalAlignment));
                            return GetEnumPicker(dt2, values);
                        }
                    case DataType2.VerticalAlignment:
                        {
                            var values = Enum.GetValues(typeof(OSGeo.MapGuide.ObjectModels.SymbolDefinition.VerticalAlignment));
                            return GetEnumPicker(dt2, values);
                        }
                }
            }
            return null;
        }

        private Func<string> GetEnumPicker(DataType2 dt2, Array values)
        {
            return () =>
            {
                var list = new List<string>();
                foreach (object val in values)
                {
                    list.Add(val.ToString());
                }
                var item = Maestro.Editors.Common.GenericItemSelectionDialog.SelectItem(null, dt2.ToString(), list.ToArray());
                if (item != null)
                {
                    return "'" + item + "'";
                }
                return null;
            };
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

        private Control CreateSymbolDefinitionEditor(ISymbolInstance symRef)
        {
            Check.ArgumentNotNull(symRef, "symRef"); //NOXLATE
            if (symRef.Reference.Type == SymbolInstanceType.Reference)
            {
                return new ReferenceCtrl((ISymbolInstanceReferenceLibrary)symRef.Reference, _edSvc);
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
            var ssym = ObjectFactory.CreateSimpleSolidFill(vl.SymbolDefinitionVersion);

            var instance = _comp.CreateInlineSimpleSymbol(ssym);
            AddInstance(instance, true);
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = (ILayerDefinition)_edSvc.GetEditedResource();
            var vl = (IVectorLayerDefinition)res.SubLayer;
            if (vl.SymbolDefinitionVersion == null)
                throw new InvalidOperationException(Strings.ErrorLayerDefnitionDoesNotSupportCompositeSymbolization);
            var ssym = ObjectFactory.CreateSimpleSolidLine(vl.SymbolDefinitionVersion);

            var instance = _comp.CreateInlineSimpleSymbol(ssym);
            AddInstance(instance, true);
        }

        private void textLabelPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = (ILayerDefinition)_edSvc.GetEditedResource();
            var vl = (IVectorLayerDefinition)res.SubLayer;
            if (vl.SymbolDefinitionVersion == null)
                throw new InvalidOperationException(Strings.ErrorLayerDefnitionDoesNotSupportCompositeSymbolization);
            var ssym = ObjectFactory.CreateSimpleLabel(vl.SymbolDefinitionVersion,
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
            var ssym = ObjectFactory.CreateSimpleLabel(vl.SymbolDefinitionVersion,
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
            var ssym = ObjectFactory.CreateSimpleLabel(vl.SymbolDefinitionVersion,
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
            var ssym = ObjectFactory.CreateSimplePoint(vl.SymbolDefinitionVersion);

            var instance = _comp.CreateInlineSimpleSymbol(ssym);
            AddInstance(instance, true);
        }

        private void btnSaveExternal_Click(object sender, EventArgs e)
        {
            if (lstInstances.SelectedItems.Count == 1)
            {
                var it = lstInstances.SelectedItems[0];
                ISymbolInstance symRef = (ISymbolInstance)it.Tag;

                if (symRef.Reference.Type == SymbolInstanceType.Inline)
                {
                    var sym = ((ISymbolInstanceReferenceInline)symRef.Reference).SymbolDefinition;
                    using (var picker = new ResourcePicker(_edSvc.CurrentConnection, ResourceTypes.SymbolDefinition.ToString(), ResourcePickerMode.SaveResource))
                    {
                        if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            sym.SetSchemaAttributes();
                            _edSvc.CurrentConnection.ResourceService.SaveResourceAs(sym, picker.ResourceID);
                            MessageBox.Show(string.Format(Maestro.Editors.Strings.SymbolExported, picker.ResourceID));
                        }
                    }
                }
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lstInstances.SelectedItems.Count == 1)
            {
                var it = lstInstances.SelectedItems[0];
                var oldIdx = lstInstances.Items.IndexOf(it);
                ISymbolInstance symRef = (ISymbolInstance)it.Tag;
                int idx = _comp.MoveSymbolInstanceUp(symRef);
                if (idx >= 0)
                {
                    var item = lstInstances.Items[idx];
                    lstInstances.Items.RemoveAt(oldIdx);
                    lstInstances.Items.RemoveAt(idx);
                    lstInstances.Items.Insert(idx, it);
                    lstInstances.Items.Insert(oldIdx, item);
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lstInstances.SelectedItems.Count == 1)
            {
                var it = lstInstances.SelectedItems[0];
                var oldIdx = lstInstances.Items.IndexOf(it);
                ISymbolInstance symRef = (ISymbolInstance)it.Tag;
                int idx = _comp.MoveSymbolInstanceDown(symRef);
                if (idx <= _comp.SymbolInstance.Count() - 1 && idx >= 0)
                {
                    var item = lstInstances.Items[idx];
                    lstInstances.Items.RemoveAt(idx);
                    lstInstances.Items.RemoveAt(oldIdx);
                    lstInstances.Items.Insert(oldIdx, item);
                    lstInstances.Items.Insert(idx, it);
                }
            }
        }
    }
}