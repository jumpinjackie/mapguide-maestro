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

        public SymbolInstancesDialog(IEditorService edSvc, ICompositeSymbolization comp, ClassDefinition cls, string provider, string featureSourceId, ILayerStylePreviewable prev)
        {
            InitializeComponent();
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
            if (li.ImageIndex == 0)
                li.Text = ((ISymbolInstanceReferenceLibrary)symRef.Reference).ResourceId;
            else
                li.Text = Strings.InlineSymbolDefinition;

            lstInstances.Items.Add(li);
            if (add)
                _comp.AddSymbolInstance(symRef);
            li.Selected = (lstInstances.Items.Count == 1);
            RenderPreview(symRef, li);
        }

        private void referenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
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
                var c = new SymbolInstanceSettingsCtrl();
                c.SetContent(symRef, _edSvc, _cls, _provider, _featureSourceId);
                c.Dock = DockStyle.Fill;
                splitContainer1.Panel2.Controls.Clear();
                splitContainer1.Panel2.Controls.Add(c);
                btnEditInstanceProperties.Enabled = true;
            }
            else
            {
                btnEditInstanceProperties.Enabled = false;
            }
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
    }
}
