#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using Maestro.Editors.LayerDefinition.Vector.StyleEditors;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    [ToolboxItem(false)]
    internal partial class ItemStyle : UserControl
    {
        private ITextSymbol m_label;
        private IPointSymbolization2D m_point;
        private IList<IStroke> m_line;
        private IAreaSymbolizationFill m_area;
        private ICompositeSymbolization m_comp;
        private Image m_w2dsymbol;

        private object m_parent;

        private bool isLabel = false;
        private bool isPoint = false;
        private bool isLine = false;
        private bool isArea = false;
        private bool isComp = false;
        private bool isW2dSymbol = false;

        public event EventHandler ItemChanged;

        private VectorLayerEditorCtrl m_owner;

        public VectorLayerEditorCtrl Owner
        {
            get { return m_owner; }
            set { m_owner = value; }
        }

        internal ItemStyle()
        {
            InitializeComponent();
        }

        public void SetItem(object parent, ITextSymbol label, double previewScale, int themeCategory)
        {
            isLabel = true;
            SetItemInternal(parent, label, previewScale, themeCategory);
        }

        public void SetItem(IPointRule parent, IPointSymbolization2D point, Image img, double previewScale, int themeCategory)
        {
            if (point == null)
            {
                isPoint = false;
                isW2dSymbol = false;
            }
            else
            {
                isPoint = (point.Symbol.Type != PointSymbolType.W2D);
                isW2dSymbol = (point.Symbol.Type == PointSymbolType.W2D);
            }
            SetItemInternal(parent, point, previewScale, themeCategory);
            m_w2dsymbol = img;
        }

        public void SetItem(ILineRule parent, IEnumerable<IStroke> line, double previewScale, int themeCategory)
        {
            isLine = true;
            SetItemInternal(parent, line, previewScale, themeCategory);
        }

        public void SetItem(IAreaRule parent, IAreaSymbolizationFill area, double previewScale, int themeCategory)
        {
            isArea = true;
            SetItemInternal(parent, area, previewScale, themeCategory);
        }

        public void SetItem(ICompositeRule parent, ICompositeSymbolization comp, double previewScale, int themeCategory)
        {
            isComp = true;
            SetItemInternal(parent, comp, previewScale, themeCategory);
        }

        private void SetItemInternal(object parent, object item, double previewScale, int themeCategory)
        {
            m_parent = parent;
            m_label = item as ITextSymbol;
            m_point = item as IPointSymbolization2D;
            m_area = item as IAreaSymbolizationFill;
            m_comp = item as ICompositeSymbolization;
            m_w2dsymbol = item as Image;
            this.PreviewScale = previewScale;
            this.ThemeCategory = themeCategory;

            if (item is IEnumerable<IStroke>)
                m_line = new List<IStroke>((IEnumerable<IStroke>)item);
            else
                m_line = null;
        }

        private void previewPicture_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, previewPicture.Width, previewPicture.Height);
            if (m_label != null)
            {
                FeaturePreviewRender.RenderPreviewFont(e.Graphics, rect, m_label);
            }
            else if (m_point != null)
            {
                if (m_point.Symbol.Type == PointSymbolType.Mark)
                    FeaturePreviewRender.RenderPreviewPoint(e.Graphics, rect, m_point.Symbol as IMarkSymbol);
                else if (m_point.Symbol.Type == PointSymbolType.Font)
                    FeaturePreviewRender.RenderPreviewFontSymbol(e.Graphics, rect, m_point.Symbol as IFontSymbol);
                else if (m_point.Symbol.Type == PointSymbolType.W2D)
                    FeaturePreviewRender.RenderW2DImage(e.Graphics, rect, m_point.Symbol as IW2DSymbol, m_w2dsymbol);
            }
            else if (m_line != null)
            {
                FeaturePreviewRender.RenderPreviewLine(e.Graphics, rect, m_line);
            }
            else if (m_area != null)
            {
                FeaturePreviewRender.RenderPreviewArea(e.Graphics, rect, m_area);
            }
            else if (m_comp != null)
            {
                FeaturePreviewRender.RenderNoPreview(e.Graphics, rect);
            }
            else
                FeaturePreviewRender.RenderPreviewFont(e.Graphics, rect, null);
        }

        public double PreviewScale { get; private set; }

        public int ThemeCategory { get; private set; }

        private ITextSymbol m_editLabel;
        private IPointSymbolization2D m_editPoint;
        private IList<IStroke> m_editLine;
        private IAreaSymbolizationFill m_editArea;

        private ITextSymbol m_origLabel;
        private IPointSymbolization2D m_origPoint;
        private IList<IStroke> m_origLine;
        private IAreaSymbolizationFill m_origArea;

        private void EditButton_Click(object sender, EventArgs e)
        {
            UserControl uc = null;
            if (m_owner.SelectedClass == null)
            {
                MessageBox.Show(Strings.NoFeatureClassAssigned);
                return;
            }
            ILayerStylePreviewable prev = new LayerStylePreviewable(m_owner.EditorService.EditedResourceID,
                                                                    this.PreviewScale,
                                                                    80,
                                                                    40,
                                                                    "PNG", //NOXLATE
                                                                    this.ThemeCategory);

            //TODO: This is obviously a mess and could do with some future cleanup, but the idea here should be
            //easy to understand. Each primitive basic style (that's not a label) has 3 actions.
            // - Commit (When user clicks OK on dialog)
            // - Rollback (When user clicks Cancel on dialog)
            // - Edit Commit (When user invokes refresh)
            //Those that support GETLEGENDIMAGE-based previews will be passed an edit commit action. Invoking the
            //edit commit action will update the session-based layer with this edit-copy rule, allowing for the changes
            //to be reflected when we do the GETLEGENDIMAGE call
            //
            //Labels are exempt as those previews can be sufficiently simulated with System.Drawing API
            var factory = (ILayerElementFactory)m_owner.Editor.GetEditedResource();
            Action commit = null;
            Action rollback = null;
            if (isLabel)
            {
                m_origLabel = m_label;
                m_editLabel = (m_label == null) ? null : (ITextSymbol)m_label.Clone();

                uc = new FontStyleEditor(m_owner.Editor, m_owner.SelectedClass, m_owner.FeatureSourceId);
                ((FontStyleEditor)uc).Item = m_editLabel;
            }
            else if (isW2dSymbol)
            {
                m_origPoint = m_point;
                m_editPoint = (m_point == null) ? null : (IPointSymbolization2D)m_point.Clone();

                var pfse = new PointFeatureStyleEditor(m_owner.Editor, m_owner.SelectedClass, m_owner.FeatureSourceId, m_w2dsymbol, prev);
                uc = pfse;
                pfse.Item = m_editPoint;

                Action editCommit = () =>
                {
                    //We need to update this boolean state
                    var w2d = pfse.W2DSymbolPreviewImage;
                    this.isPoint = (w2d == null);
                    this.isW2dSymbol = (w2d != null);

                    m_editPoint = pfse.Item;
                    ((IPointRule)m_parent).PointSymbolization2D = m_editPoint;

                    m_w2dsymbol = w2d;
                };
                pfse.SetEditCommit(editCommit);
                commit = () =>
                {
                    //We need to update this boolean state
                    var w2d = pfse.W2DSymbolPreviewImage;
                    this.isPoint = (w2d == null);
                    this.isW2dSymbol = (w2d != null);

                    m_point = pfse.Item;
                    ((IPointRule)m_parent).PointSymbolization2D = m_point;

                    m_w2dsymbol = w2d;
                };
                rollback = () =>
                {
                    ((IPointRule)m_parent).PointSymbolization2D = m_origPoint;
                };
            }
            else if (isPoint)
            {
                m_origPoint = m_point;
                m_editPoint = (m_point == null) ? null : (IPointSymbolization2D)m_point.Clone();

                var pfse = new PointFeatureStyleEditor(m_owner.Editor, m_owner.SelectedClass, m_owner.FeatureSourceId, prev);
                uc = pfse;
                pfse.Item = m_editPoint;

                Action editCommit = () =>
                {
                    //We need to update this boolean state
                    var w2d = pfse.W2DSymbolPreviewImage;
                    this.isPoint = (w2d == null);
                    this.isW2dSymbol = (w2d != null);

                    m_editPoint = pfse.Item;
                    ((IPointRule)m_parent).PointSymbolization2D = m_editPoint;

                    m_w2dsymbol = w2d;
                };
                pfse.SetEditCommit(editCommit);
                commit = () =>
                {
                    //We need to update this boolean state
                    var w2d = pfse.W2DSymbolPreviewImage;
                    this.isPoint = (w2d == null);
                    this.isW2dSymbol = (w2d != null);

                    m_point = pfse.Item;
                    ((IPointRule)m_parent).PointSymbolization2D = m_point;

                    m_w2dsymbol = w2d;
                };
                rollback = () => 
                {
                    ((IPointRule)m_parent).PointSymbolization2D = m_origPoint;
                };
            }
            else if (isLine)
            {
                m_origLine = m_line;
                m_editLine = (m_line == null) ? new List<IStroke>() : LayerElementCloningUtil.CloneStrokes(m_line);

                var lfse = new LineFeatureStyleEditor(m_owner.Editor, m_owner.SelectedClass, m_owner.FeatureSourceId, factory, prev);
                uc = lfse;
                lfse.Item = m_editLine;

                Action editCommit = () => 
                {
                    m_editLine = lfse.Item;
                    ((ILineRule)m_parent).SetStrokes(m_editLine);
                };
                lfse.SetEditCommit(editCommit);
                commit = () =>
                {
                    m_line = lfse.Item;
                    ((ILineRule)m_parent).SetStrokes(m_line);
                };
                rollback = () =>
                {
                    ((ILineRule)m_parent).SetStrokes(m_origLine);
                };
            }
            else if (isArea)
            {
                m_origArea = m_area;
                m_editArea = (m_area == null) ? null : (IAreaSymbolizationFill)m_area.Clone();

                var afse = new AreaFeatureStyleEditor(m_owner.Editor, m_owner.SelectedClass, m_owner.FeatureSourceId, prev);
                uc = afse;
                afse.Item = m_editArea;

                Action editCommit = () =>
                {
                    m_editArea = afse.Item;
                    ((IAreaRule)m_parent).AreaSymbolization2D = m_editArea;
                };
                commit = () =>
                {
                    m_area = afse.Item;
                    ((IAreaRule)m_parent).AreaSymbolization2D = m_area;
                };
                rollback = () =>
                {
                    ((IAreaRule)m_parent).AreaSymbolization2D = m_origArea;
                };
                afse.SetEditCommit(editCommit);
            }
            else if (isComp)
            {
                var diag = new SymbolInstancesDialog(m_owner.Editor, m_comp, m_owner.SelectedClass, m_owner.GetFdoProvider(), m_owner.FeatureSourceId, prev);
                diag.ShowDialog();
                //HACK: Assume edits made
                Owner.RaiseResourceChanged();
                return;
            }

            if (uc != null)
            {
                EditorTemplateForm dlg = new EditorTemplateForm();
                dlg.ItemPanel.Controls.Add(uc);
                uc.Dock = DockStyle.Fill;
                dlg.RefreshSize();
                var res = dlg.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    if (commit != null)
                    {
                        commit.Invoke();
                    }

                    if (isLabel)
                    {
                        m_label = ((FontStyleEditor)uc).Item;
                        if (m_parent as IPointRule != null)
                            ((IPointRule)m_parent).Label = m_label;
                        else if (m_parent as ILineRule != null)
                            ((ILineRule)m_parent).Label = m_label;
                        else if (m_parent as IAreaRule != null)
                            ((IAreaRule)m_parent).Label = m_label;

                        if (ItemChanged != null)
                            ItemChanged(m_label, null);
                    }
                    else if (isPoint || isW2dSymbol)
                    {
                        if (ItemChanged != null)
                            ItemChanged(m_point, null);
                    }
                    else if (isLine)
                    {
                           
                        if (ItemChanged != null)
                            ItemChanged(m_line, null);
                    }
                    else if (isArea)
                    {
                        if (ItemChanged != null)
                            ItemChanged(m_area, null);
                    }

                    this.Refresh();
                }
                else if (res == DialogResult.Cancel)
                {
                    if (rollback != null)
                        rollback.Invoke();
                }
            }

        }

        private void previewPicture_Click(object sender, EventArgs e)
        {
            try { Parent.Focus(); }
            catch {}
        }

        private void previewPicture_DoubleClick(object sender, EventArgs e)
        {
            try { Parent.Focus(); }
            catch { }

            EditButton_Click(sender, e);
        }
    }
}
