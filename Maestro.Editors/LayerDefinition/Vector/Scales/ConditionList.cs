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

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    internal partial class ConditionList : UserControl
    {
        private IPointVectorStyle m_point;
        private ILineVectorStyle m_line;
        private IAreaVectorStyle m_area;

        private IVectorScaleRange m_parent;
        private object m_selectedItem;

        public event EventHandler SelectionChanged;
        public event EventHandler ItemChanged;

        private VectorLayerEditorCtrl m_owner;

        public VectorLayerEditorCtrl Owner
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

        public void SetItem(IVectorScaleRange parent, IPointVectorStyle point)
        {
            SetItemInternal(parent, point);
        }

        public void SetItem(IVectorScaleRange parent, ILineVectorStyle line)
        {
            SetItemInternal(parent, line);
        }

        public void SetItem(IVectorScaleRange parent, IAreaVectorStyle area)
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

        private void SetItemInternal(IVectorScaleRange parent, object item)
        {
            m_parent = parent;
            m_point = item as IPointVectorStyle;
            m_line = item as ILineVectorStyle;
            m_area = item as IAreaVectorStyle;

            this.Controls.Clear();

            try
            {
                this.Visible = false;
                this.SuspendLayout();

                if (m_point != null)
                {
                    foreach (IPointRule prt in m_point.Rules)
                        AddRuleControl(prt);
                }
                else if (m_line != null)
                {
                    foreach (ILineRule lrt in m_line.Rules)
                        AddRuleControl(lrt);
                }
                else if (m_area != null)
                {
                    foreach (IAreaRule art in m_area.Rules)
                        AddRuleControl(art);
                }
            }
            finally
            {
                this.ResumeLayout();
                this.Visible = true;
            }
        }

        public Condition AddRuleControl(object rule)
        {
            if (rule == null)
                return null;

            Condition c = new Condition();

            if (rule as IPointRule != null)
                c.SetItem(rule as IPointRule);
            else if (rule as ILineRule != null)
                c.SetItem(rule as ILineRule);
            else if (rule as IAreaRule != null)
                c.SetItem(rule as IAreaRule);

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
            {
                if (c.Item == sender)
                {
                    this.Controls.Remove(c);
                    break;
                }
            }

            if (m_point != null)
            {
                IPointRule remove = null;
                foreach (var pr in m_point.Rules)
                {
                    if (pr == sender)
                    {
                        remove = pr;
                        break;
                    }
                }

                if (remove != null)
                    m_point.RemoveRule(remove);
            }
            else if (m_line != null)
            {
                ILineRule remove = null;
                foreach (var lr in m_line.Rules)
                {
                    if (lr == sender)
                    {
                        remove = lr;
                        break;
                    }
                }

                if (remove != null)
                    m_line.RemoveRule(remove);
            }
            else if (m_area != null)
            {
                IAreaRule remove = null;
                foreach (var ar in m_area.Rules)
                {
                    if (ar == sender)
                    {
                        remove = ar;
                        break;
                    }
                }

                if (remove != null)
                    m_area.RemoveRule(remove);
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
                    ic = (System.Collections.IList)m_point.Rules;
                else if (m_line != null)
                    ic = (System.Collections.IList)m_line.Rules;
                else if (m_area != null)
                    ic = (System.Collections.IList)m_area.Rules;

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
