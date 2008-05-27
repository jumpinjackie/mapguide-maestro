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
    public partial class ConditionList : UserControl
    {
        private PointTypeStyleType m_point;
        private LineTypeStyleType m_line;
        private AreaTypeStyleType m_area;

        private VectorScaleRangeType m_parent;

        public event EventHandler ItemChanged;

        public ConditionList()
        {
            InitializeComponent();
        }

        public void SetItem(VectorScaleRangeType parent, PointTypeStyleType point)
        {
            SetItemInternal(parent, point);
        }

        public void SetItem(VectorScaleRangeType parent, LineTypeStyleType line)
        {
            SetItemInternal(parent, line);
        }

        public void SetItem(VectorScaleRangeType parent, AreaTypeStyleType area)
        {
            SetItemInternal(parent, area);
        }

        private void SetItemInternal(VectorScaleRangeType parent, object item)
        {
            m_parent = parent;
            m_point = item as PointTypeStyleType;
            m_line = item as LineTypeStyleType;
            m_area = item as AreaTypeStyleType;

            ControlList.Controls.Clear();

            if (m_point != null)
            {
                if (m_point.PointRule != null)
                    foreach (PointRuleType prt in m_point.PointRule)
                        AddRuleControl(prt);
            }
            else if (m_line != null)
            {
                if (m_line.LineRule != null)
                    foreach (LineRuleType lrt in m_line.LineRule)
                        AddRuleControl(lrt);
            }
            else if (m_area != null)
            {
                if (m_area != null)
                    foreach (AreaRuleType art in m_area.AreaRule)
                        AddRuleControl(art);
            }
        }

        private Condition AddRuleControl(object rule)
        {
            if (rule == null)
                return null;

            Condition c = new Condition();

            if (rule as PointRuleType != null)
                c.SetItem(rule as PointRuleType);
            else if (rule as LineRuleType != null)
                c.SetItem(rule as LineRuleType);
            else if (rule as AreaRuleType != null)
                c.SetItem(rule as AreaRuleType);

            c.Dock = DockStyle.Top;
            ControlList.Controls.Add(c);
            c.BringToFront();
            c.ItemChanged += new EventHandler(Rule_ItemChanged);
            c.ItemDeleted += new EventHandler(Rule_ItemDeleted);
            return c;
        }

        private void SignalItemChanged()
        {
            if (ItemChanged != null)
                if (m_point != null)
                    ItemChanged(m_point, null);
                else if (m_line != null)
                    ItemChanged(m_line, null);
                else if (m_area != null)
                    ItemChanged(m_area, null);
        }

        void Rule_ItemChanged(object sender, EventArgs e)
        {
            SignalItemChanged();
        }

        void Rule_ItemDeleted(object sender, EventArgs e)
        {
            foreach (Condition c in ControlList.Controls)
                if (c.Item == sender)
                {
                    ControlList.Controls.Remove(c);
                    break;
                }

            if (m_point != null)
            {
                for (int i = 0; i < m_point.PointRule.Count; i++)
                    if (m_point.PointRule[i] == sender)
                    {
                        m_point.PointRule.RemoveAt(i);
                        break;
                    }
            }
            else if (m_line != null)
            {
                for (int i = 0; i < m_line.LineRule.Count; i++)
                    if (m_line.LineRule[i] == sender)
                    {
                        m_line.LineRule.RemoveAt(i);
                        break;
                    }
            }
            else if (m_area != null)
            {
                for (int i = 0; i < m_area.AreaRule.Count; i++)
                    if (m_area.AreaRule[i] == sender)
                    {
                        m_area.AreaRule.RemoveAt(i);
                        break;
                    }
            }

            SignalItemChanged();
            
        }

        private void AddRuleButton_Click(object sender, EventArgs e)
        {
            if (m_point != null)
            {
                PointRuleType prt = new PointRuleType();
                prt.Item = new PointSymbolization2DType();
                prt.Item.Item = new MarkSymbolType();
                prt.Item.Item.SizeContext = SizeContextType.DeviceUnits;
                if (m_point.PointRule == null)
                    m_point.PointRule = new PointRuleTypeCollection();
                m_point.PointRule.Add(prt);
                AddRuleControl(prt).Focus();
            }
            else if (m_line != null)
            {
                LineRuleType lrt = new LineRuleType();
                lrt.Items = new StrokeTypeCollection();
                lrt.Items.Add(new StrokeType());
                lrt.Items[0].LineStyle = "Solid";
                lrt.Items[0].SizeContext = SizeContextType.DeviceUnits;
                if (m_line.LineRule == null)
                    m_line.LineRule = new LineRuleTypeCollection();
                m_line.LineRule.Add(lrt);
                AddRuleControl(lrt).Focus();;
            }
            else if (m_area != null)
            {
                AreaRuleType art = new AreaRuleType();
                art.Item = new AreaSymbolizationFillType();
                art.Item.Fill = new FillType();
                art.Item.Stroke = new StrokeType();
                art.Item.Fill.BackgroundColor = Color.White;
                art.Item.Fill.ForegroundColor = Color.White;
                art.Item.Fill.FillPattern = "Solid";
                art.Item.Stroke.Color = Color.Black;
                art.Item.Stroke.LineStyle = "Solid";
                if (m_area.AreaRule == null)
                    m_area.AreaRule = new AreaRuleTypeCollection();
                m_area.AreaRule.Add(art);
                AddRuleControl(art).Focus();
               
            }

        }
    }
}
