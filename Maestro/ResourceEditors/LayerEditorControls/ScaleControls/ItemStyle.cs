using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls
{
    public partial class ItemStyle : UserControl
    {
        private TextSymbolType m_label;
        private PointSymbolization2DType m_point;
        private StrokeTypeCollection m_line;
        private AreaSymbolizationFillType m_area;

        private object m_parent;

        private bool isLabel = false;
        private bool isPoint = false;
        private bool isLine = false;
        private bool isArea = false;

        public event EventHandler ItemChanged;

        public ItemStyle()
        {
            InitializeComponent();
        }

        public void SetItem(object parent, TextSymbolType label)
        {
            isLabel = true;
            SetItemInternal(parent, label);
        }

        public void SetItem(PointRuleType parent, PointSymbolization2DType point)
        {
            isPoint = true;
            SetItemInternal(parent, point);
        }

        public void SetItem(LineRuleType parent, StrokeTypeCollection line)
        {
            isLine = true;
            SetItemInternal(parent, line);
        }

        public void SetItem(AreaRuleType parent, AreaSymbolizationFillType area)
        {
            isArea = true;
            SetItemInternal(parent, area);
        }

        private void SetItemInternal(object parent, object item)
        {
            m_parent = parent;
            m_label = item as TextSymbolType;
            m_point = item as PointSymbolization2DType;
            m_line = item as StrokeTypeCollection;
            m_area = item as AreaSymbolizationFillType;

        }

        private void previewPicture_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, previewPicture.Width, previewPicture.Height);
            if (m_label != null)
            {
                FeaturePreviewRender.RenderPreviewFont(e.Graphics, rect, m_label);
            }
            else if (m_point != null)
            {
                FeaturePreviewRender.RenderPreviewPoint(e.Graphics, rect, m_point.Item as MarkSymbolType);
            }
            else if (m_line != null)
            {
                FeaturePreviewRender.RenderPreviewLine(e.Graphics, rect, m_line);
            }
            else if (m_area != null)
            {
                FeaturePreviewRender.RenderPreviewArea(e.Graphics, rect, m_area);
            }
            else
                FeaturePreviewRender.RenderPreviewFont(e.Graphics, rect, null);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            UserControl uc = null;
            if (isLabel)
            {
                uc = new GeometryStyleEditors.FontStyleEditor();
                ((GeometryStyleEditors.FontStyleEditor)uc).Item = (TextSymbolType)Utility.DeepCopy(m_label);
            }
            else if (isPoint)
            {
                uc = new GeometryStyleEditors.PointFeatureStyleEditor();
                ((GeometryStyleEditors.PointFeatureStyleEditor)uc).Item = (PointSymbolization2DType)Utility.XmlDeepCopy(m_point);
            }
            else if (isLine)
            {
                uc = new GeometryStyleEditors.LineFeatureStyleEditor();
                ((GeometryStyleEditors.LineFeatureStyleEditor)uc).Item = (StrokeTypeCollection)Utility.XmlDeepCopy(m_line);
            }
            else if (isArea)
            {
                uc = new GeometryStyleEditors.AreaFeatureStyleEditor();
                ((GeometryStyleEditors.AreaFeatureStyleEditor)uc).Item = (AreaSymbolizationFillType)Utility.XmlDeepCopy(m_area);
            }

            if (uc != null)
            {
                EditorTemplateForm dlg = new EditorTemplateForm();
                dlg.ItemPanel.Controls.Add(uc);
                uc.Dock = DockStyle.Fill;
                dlg.RefreshSize();
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (isLabel)
                    {
                        m_label = ((GeometryStyleEditors.FontStyleEditor)uc).Item;
                        if (m_parent as PointRuleType != null)
                            ((PointRuleType)m_parent).Label = m_label;
                        else if (m_parent as LineRuleType != null)
                            ((LineRuleType)m_parent).Label = m_label;
                        else if (m_parent as AreaRuleType != null)
                            ((AreaRuleType)m_parent).Label = m_label;

                        if (ItemChanged != null)
                            ItemChanged(m_label, null);
                    }
                    else if (isPoint)
                    {
                        m_point = ((GeometryStyleEditors.PointFeatureStyleEditor)uc).Item;
                        ((PointRuleType)m_parent).Item = m_point;
                        if (ItemChanged != null)
                            ItemChanged(m_point, null);
                    }
                    else if (isLine)
                    {
                        m_line = ((GeometryStyleEditors.LineFeatureStyleEditor)uc).Item;
                        ((LineRuleType)m_parent).Items = m_line;
                        if (ItemChanged != null)
                            ItemChanged(m_line, null);
                    }
                    else if (isArea)
                    {
                        m_area = ((GeometryStyleEditors.AreaFeatureStyleEditor)uc).Item;
                        ((AreaRuleType)m_parent).Item = m_area; ;
                        if (ItemChanged != null)
                            ItemChanged(m_area, null);
                    }
                    
                    this.Refresh();

                }
            }

        }
    }
}
