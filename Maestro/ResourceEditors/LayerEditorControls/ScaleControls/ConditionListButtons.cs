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
    public partial class ConditionListButtons : UserControl
    {
        private PointTypeStyleType m_point;
        private LineTypeStyleType m_line;
        private AreaTypeStyleType m_area;

        private VectorScaleRangeType m_parent;
        private object m_lastSelection;
        private object m_currentSelection;

        public event EventHandler ItemChanged;

        private VectorLayer m_owner;

        public VectorLayer Owner
        {
            get { return m_owner; }
            set
            {
                m_owner = value;
                conditionList.Owner = m_owner;
            }
        }

        public ConditionListButtons()
        {
            InitializeComponent();
            conditionList.SelectionChanged += new EventHandler(conditionList_SelectionChanged);
        }

        private void conditionList_SelectionChanged(object sender, EventArgs e)
        {
            m_lastSelection = m_currentSelection;
            m_currentSelection = conditionList.SelectedItem;
            CopyRuleButton.Enabled = MoveRuleUpButton.Enabled = MoveRuleDownButton.Enabled = m_currentSelection != null || m_lastSelection != null;        
        }

        public void SetItem(VectorScaleRangeType parent, PointTypeStyleType point)
        {
            SetItemInternal(parent, point);
            conditionList.SetItem(parent, point);
        }

        public void SetItem(VectorScaleRangeType parent, LineTypeStyleType line)
        {
            SetItemInternal(parent, line);
            conditionList.SetItem(parent, line);
        }

        public void SetItem(VectorScaleRangeType parent, AreaTypeStyleType area)
        {
            SetItemInternal(parent, area);
            conditionList.SetItem(parent, area);
        }


        private void SetItemInternal(VectorScaleRangeType parent, object item)
        {
            m_parent = parent;
            m_point = item as PointTypeStyleType;
            m_line = item as LineTypeStyleType;
            m_area = item as AreaTypeStyleType;

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
                conditionList.AddRuleControl(prt).Focus();
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
                conditionList.AddRuleControl(lrt).Focus(); ;
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
                conditionList.AddRuleControl(art).Focus();

            }

            if (ItemChanged != null)
                ItemChanged(this, null);
        }

        private void CreateThemeButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "This function is not yet implemented", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CopyRuleButton_Click(object sender, EventArgs e)
        {
            if (m_currentSelection == null)
                conditionList.SelectedItem = m_lastSelection;

            if (conditionList.SelectedItem == null)
                return;

            conditionList.AddRuleControl(Utility.XmlDeepCopy(conditionList.SelectedItem)).Focus();

            if (ItemChanged != null)
                ItemChanged(this, null);
        }

        private void MoveRuleUpButton_Click(object sender, EventArgs e)
        {
            if (m_currentSelection == null)
                conditionList.SelectedItem = m_lastSelection;
            conditionList.MoveSelectedRule(false);
            if (ItemChanged != null)
                ItemChanged(this, null);
        }

        private void MoveRuleDownButton_Click(object sender, EventArgs e)
        {
            if (m_currentSelection == null)
                conditionList.SelectedItem = m_lastSelection;
            conditionList.MoveSelectedRule(true);
            if (ItemChanged != null)
                ItemChanged(this, null);
        }

        private void conditionList_ItemChanged(object sender, EventArgs e)
        {
            if (ItemChanged != null)
                ItemChanged(sender, null);
        }

    }
}
