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
    public partial class ConditionList : UserControl
    {
        private PointTypeStyleType m_point;
        private LineTypeStyleType m_line;
        private AreaTypeStyleType m_area;

        private VectorScaleRangeType m_parent;
        private object m_selectedItem;

        public event EventHandler SelectionChanged;
        public event EventHandler ItemChanged;

        private VectorLayer m_owner;

        public VectorLayer Owner
        {
            get { return m_owner; }
            set
            {
                m_owner = value;
                foreach (Condition c in this.Controls)
                    c.Owner = m_owner; ;
            }
        }

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

        public object SelectedItem 
        { 
            get { return m_selectedItem; }
            set
            {
                m_selectedItem = value;
                if (m_selectedItem != null)
                {
                    foreach (Condition c in this.Controls)
                        if (c.Item == m_selectedItem)
                        {
                            c.Focus();
                            return;
                        }
                }
            }
        }

        private void SetItemInternal(VectorScaleRangeType parent, object item)
        {
            m_parent = parent;
            m_point = item as PointTypeStyleType;
            m_line = item as LineTypeStyleType;
            m_area = item as AreaTypeStyleType;

            this.Controls.Clear();

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
                    if (m_area.AreaRule != null)
                        foreach (AreaRuleType art in m_area.AreaRule)
                            AddRuleControl(art);
            }
        }

        public Condition AddRuleControl(object rule)
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

            c.Owner = m_owner;
            c.Dock = DockStyle.Top;
            this.Controls.Add(c);
            c.BringToFront();
            c.ItemChanged += new EventHandler(Rule_ItemChanged);
            c.ItemDeleted += new EventHandler(Rule_ItemDeleted);
            c.Enter += new EventHandler(Rule_Enter);
            c.Leave += new EventHandler(Rule_Leave);
            return c;
        }

        private void Rule_Leave(object sender, EventArgs e)
        {
            Condition c = sender as Condition;
            if (c == null)
                return;

            c.BorderStyle = BorderStyle.None;
           
            m_selectedItem = null;
            if (SelectionChanged != null)
                SelectionChanged(this, null);
        }

        private void Rule_Enter(object sender, EventArgs e)
        {
            Condition c = sender as Condition;
            if (c == null)
                return;

            c.BorderStyle = BorderStyle.FixedSingle;
            m_selectedItem = c.Item;
            if (SelectionChanged != null)
                SelectionChanged(this, null);
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
            foreach (Condition c in this.Controls)
                if (c.Item == sender)
                {
                    this.Controls.Remove(c);
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

        public void MoveSelectedRule(bool down)
        {
            if (this.SelectedItem == null)
                return;

            Condition selectedControl = null;
            object selectedRule = this.SelectedItem;

            foreach (Condition c in this.Controls)
                if (c.Item == this.SelectedItem)
                {
                    selectedControl = c;
                    break;
                }

            if (selectedControl == null)
                return;

            int pos = this.Controls.GetChildIndex(selectedControl);
            if ((down && pos > 0) || (!down && pos < this.Controls.Count - 1))
            {
                this.Controls.SetChildIndex(selectedControl, pos + (down ? -1 : 1));

                System.Collections.IList ic = null;
                if (m_point != null)
                    ic = m_point.PointRule;
                else if (m_line != null)
                    ic = m_line.LineRule;
                else if (m_area != null)
                    ic = m_area.AreaRule;

                pos = ic.IndexOf(selectedRule);
                if ((!down && pos > 0) || (down && pos < ic.Count - 1 && pos > 0))
                {
                    ic.RemoveAt(pos);
                    ic.Insert(pos + (down ? 1 : -1), selectedRule);
                }

            }
        }

        public int GetPreferedHeight()
        {
            int minHeight = 0;

            for (int i = 0; i < Math.Min(10, this.Controls.Count); i++)
                minHeight += this.Controls[i].Height;

            return minHeight;
        }

    }
}
