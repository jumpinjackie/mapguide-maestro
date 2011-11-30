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
using OSGeo.MapGuide.MaestroAPI.Services;

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

        private ILayerElementFactory _factory;
        private IVectorScaleRange _parentRange;

        public ItemStyle(ILayerElementFactory factory)
        {
            _factory = factory;
        }

        public void SetItem(IVectorScaleRange parentRange, object parent, ITextSymbol label, int themeCategory)
        {
            isLabel = true;
            SetItemInternal(parentRange, parent, label, themeCategory);
        }

        public void SetItem(IVectorScaleRange parentRange, IPointRule parent, IPointSymbolization2D point, Image img, int themeCategory)
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
            SetItemInternal(parentRange, parent, point, themeCategory);
            m_w2dsymbol = img;
        }

        public void SetItem(IVectorScaleRange parentRange, ILineRule parent, IEnumerable<IStroke> line, int themeCategory)
        {
            isLine = true;
            SetItemInternal(parentRange, parent, line, themeCategory);
        }

        public void SetItem(IVectorScaleRange parentRange, IAreaRule parent, IAreaSymbolizationFill area, int themeCategory)
        {
            isArea = true;
            SetItemInternal(parentRange, parent, area, themeCategory);
        }

        public void SetItem(IVectorScaleRange parentRange, ICompositeRule parent, ICompositeSymbolization comp, int themeCategory)
        {
            isComp = true;
            SetItemInternal(parentRange, parent, comp, themeCategory);
        }

        private int _themeCategory;

        private void SetItemInternal(IVectorScaleRange parentRange, object parent, object item, int themeCategory)
        {
            _parentRange = parentRange;
            _themeCategory = themeCategory;
            m_parent = parent;
            m_label = item as ITextSymbol;
            m_point = item as IPointSymbolization2D;
            m_area = item as IAreaSymbolizationFill;
            m_comp = item as ICompositeSymbolization;
            m_w2dsymbol = item as Image;

            if (item is IEnumerable<IStroke>)
                m_line = new List<IStroke>((IEnumerable<IStroke>)item);
            else
                m_line = null;
        }

        private IMappingService _mapSvc = null;

        IMappingService GetMappingService()
        {
            if (null == _mapSvc)
            {
                var conn = Owner.EditorService.GetEditedResource().CurrentConnection;
                if (Array.IndexOf<int>(conn.Capabilities.SupportedServices, (int)ServiceType.Mapping) >= 0)
                    _mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            }
            return _mapSvc;
        }

        int GetGeomType()
        {
            int gt = -1;
            if (isPoint || isLabel || isW2dSymbol)
                return 1;
            else if (isLine)
                return 2;
            else if (isArea)
                return 3;
            else if (isComp)
                return 4;
            return gt;
        }

        private void previewPicture_Paint(object sender, PaintEventArgs e)
        {
            var mapSvc = GetMappingService();
            if (mapSvc != null && !StylePreview.UseClientSideStylePreview)
            {
                double scale = 10000.0;
                if (_parentRange.MaxScale.HasValue)
                    scale = _parentRange.MaxScale.Value - 1.0;
                else if (_parentRange.MinScale.HasValue)
                    scale = _parentRange.MinScale.Value + 1.0;

                //Ensure the layer def we're rendering from is latest
                Owner.EditorService.SyncSessionCopy();
                using (var img = mapSvc.GetLegendImage(scale, Owner.EditorService.GetEditedResource().ResourceID, _themeCategory, GetGeomType(), previewPicture.Width, previewPicture.Height, "PNG"))
                {
                    e.Graphics.DrawImage(img, new Point(0, 0));
                }
            }
            else
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
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            IFeatureStyleEditor uc = null;
            if (isLabel)
            {
                uc = new FontStyleEditor(m_owner.Editor, m_owner.SelectedClass, m_owner.FeatureSourceId, m_label);
                //((FontStyleEditor)uc).Item = m_label == null ? null : (ITextSymbol)m_label.Clone(); //(ITextSymbol)Utility.DeepCopy(m_label);
            }
            else if (isW2dSymbol)
            {
                uc = new PointFeatureStyleEditor(m_owner.Editor, m_owner.SelectedClass, m_owner.FeatureSourceId, m_w2dsymbol, _parentRange, _themeCategory, m_point);
                //((PointFeatureStyleEditor)uc).Item = m_point == null ? null : (IPointSymbolization2D)m_point.Clone(); //(IPointSymbolization2D)Utility.XmlDeepCopy(m_point);
            }
            else if (isPoint)
            {
                uc = new PointFeatureStyleEditor(m_owner.Editor, m_owner.SelectedClass, m_owner.FeatureSourceId, _parentRange, _themeCategory, m_point);
                //((PointFeatureStyleEditor)uc).Item = m_point == null ? null : (IPointSymbolization2D)m_point.Clone(); //(IPointSymbolization2D)Utility.XmlDeepCopy(m_point);
            }
            else if (isLine)
            {
                uc = new LineFeatureStyleEditor(m_owner.Editor, m_owner.SelectedClass, m_owner.FeatureSourceId, _factory, _parentRange, _themeCategory, m_line);
                //((LineFeatureStyleEditor)uc).Item = m_line == null ? null : LayerElementCloningUtil.CloneStrokes(m_line);//(IList<IStroke>)Utility.XmlDeepCopy(m_line);
            }
            else if (isArea)
            {
                uc = new AreaFeatureStyleEditor(m_owner.Editor, m_owner.SelectedClass, m_owner.FeatureSourceId, _parentRange, _themeCategory, m_area);
                //((AreaFeatureStyleEditor)uc).Item = m_area == null ? null : (IAreaSymbolizationFill)m_area.Clone(); //(IAreaSymbolizationFill)Utility.XmlDeepCopy(m_area);
            }
            else if (isComp)
            {
                var diag = new SymbolInstancesDialog(m_owner.Editor, m_comp, m_owner.SelectedClass, m_owner.GetFdoProvider(), m_owner.FeatureSourceId);
                diag.ShowDialog();
                //HACK: Assume edits made
                Owner.RaiseResourceChanged();
                return;
            }

            if (uc != null)
            {
                uc.CreateSnapshot();
                
                EditorTemplateForm dlg = new EditorTemplateForm();
                dlg.ItemPanel.Controls.Add(uc.Content);
                uc.Content.Dock = DockStyle.Fill;
                dlg.RefreshSize();
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    /*
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
                        //We need to update this boolean state
                        var w2d = ((PointFeatureStyleEditor)uc).W2DSymbolPreviewImage;
                        isPoint = (w2d == null);
                        isW2dSymbol = (w2d != null);

                        m_point = ((PointFeatureStyleEditor)uc).Item;
                        ((IPointRule)m_parent).PointSymbolization2D = m_point;

                        m_w2dsymbol = w2d;

                        if (ItemChanged != null)
                            ItemChanged(m_point, null);
                    }
                    else if (isLine)
                    {
                        m_line = ((LineFeatureStyleEditor)uc).Item;
                        ((ILineRule)m_parent).SetStrokes(m_line);
                        if (ItemChanged != null)
                            ItemChanged(m_line, null);
                    }
                    else if (isArea)
                    {
                        m_area = ((AreaFeatureStyleEditor)uc).Item;
                        ((IAreaRule)m_parent).AreaSymbolization2D = m_area;
                        if (ItemChanged != null)
                            ItemChanged(m_area, null);
                    }
                    */
                    this.Refresh();
                    Owner.FlagDirty();
                }
                else
                {
                    uc.RejectChanges();
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

    internal interface IFeatureStyleEditor
    {
        void RejectChanges();
        void CreateSnapshot();
        Control Content { get; }
        void Detach();
        void Attach();
        bool IsAttached { get; }
        event EventHandler StyleDetached;
        event EventHandler StyleAttached;
    }

    public class StylePreview
    {
        /// <summary>
        /// Indicates whether to use client-side rendering to generate style previews. Client-side rendering is more efficient, but does
        /// not produce fully accurate preview styles. Setting it to false will use the GETLEGENDIMAGE API to produce the style preview which
        /// will produce the exact preview style.
        /// 
        /// For style-intensive layers, the GETLEGENDIMAGE approach can momentarily grind the MapGuide Server
        /// </summary>
        public static bool UseClientSideStylePreview;

        static StylePreview()
        {
            UseClientSideStylePreview = true;
        }
    }
}
