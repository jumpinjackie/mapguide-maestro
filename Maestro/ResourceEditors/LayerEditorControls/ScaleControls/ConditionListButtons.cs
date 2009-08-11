#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
                if (m_owner != null && m_owner.Globalizor != null)
                {
                    label1.Text = m_owner.Globalizor.Translate("Rule");
                    ShowInLegend.Text = m_owner.Globalizor.Translate("Legendlabel");
                    label3.Text = m_owner.Globalizor.Translate("Featurestyle");
                    label4.Text = m_owner.Globalizor.Translate("Labelstyle");
                }
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

        public void AddRule()
        {
            AddRuleButton_Click(null, null);
        }

        private void AddRuleButton_Click(object sender, EventArgs e)
        {
            if (m_point != null)
            {
                PointRuleType prt = DefaultItemGenerator.CreatePointRuleType();
                if (m_point.PointRule == null)
                    m_point.PointRule = new PointRuleTypeCollection();
                m_point.PointRule.Add(prt);
                conditionList.AddRuleControl(prt).Focus();
            }
            else if (m_line != null)
            {
                LineRuleType lrt = DefaultItemGenerator.CreateLineRuleType();
                if (m_line.LineRule == null)
                    m_line.LineRule = new LineRuleTypeCollection();
                m_line.LineRule.Add(lrt);
                conditionList.AddRuleControl(lrt).Focus(); ;
            }
            else if (m_area != null)
            {
                AreaRuleType art = DefaultItemGenerator.CreateAreaRuleType();
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
            try
            {
                ThemeCreator dlg = new ThemeCreator(m_owner.Editor, m_owner.Resource as LayerDefinition, m_owner.Schema);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    m_owner.UpdateDisplay();
            }
            catch (Exception ex)
            {
                m_owner.Editor.SetLastException(ex);
                MessageBox.Show(this, string.Format("An error occured: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void CopyRuleButton_Click(object sender, EventArgs e)
        {
            if (m_currentSelection == null)
                conditionList.SelectedItem = m_lastSelection;

            if (conditionList.SelectedItem == null)
                return;

            object rule = Utility.XmlDeepCopy(conditionList.SelectedItem);


            if (m_point != null)
            {
                if (m_point.PointRule == null)
                    m_point.PointRule = new PointRuleTypeCollection();
                m_point.PointRule.Add((PointRuleType)rule);
            }
            else if (m_line != null)
            {
                if (m_line.LineRule == null)
                    m_line.LineRule = new LineRuleTypeCollection();
                m_line.LineRule.Add((LineRuleType)rule);
            }
            else if (m_area != null)
            {
                if (m_area.AreaRule == null)
                    m_area.AreaRule = new AreaRuleTypeCollection();
                m_area.AreaRule.Add((AreaRuleType)rule);
            }
            else
                return;

            conditionList.AddRuleControl(rule).Focus();

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


        public void ResizeAuto()
        {
            this.Height = this.GetPreferedHeight();
        }

        public int GetPreferedHeight()
        {
            return panel1.Height + conditionList.GetPreferedHeight() + 24;
        }

        private void ShowInLegend_CheckedChanged(object sender, EventArgs e)
        {
            LayerDefinition layer = (LayerDefinition)m_owner.Resource;
            Version layerVersion = new Version((layer).version);
            bool supported = layerVersion >= new Version(1, 3, 0);

            if (!supported)
                layer.ConvertLayerDefinitionToVersion(new Version(1, 3, 0));

            if (m_point != null)
            {
                m_point.ShowInLegend = ShowInLegend.Checked;
            }
            else if (m_line != null)
            {
                m_line.ShowInLegend = ShowInLegend.Checked;
            }
            else if (m_area != null)
            {
                m_area.ShowInLegend = ShowInLegend.Checked;
            }

            if (!supported)
                layer.ConvertLayerDefinitionToVersion(layerVersion); 
            
            if (ItemChanged != null)
                ItemChanged(this, null);

        }

    }
}
