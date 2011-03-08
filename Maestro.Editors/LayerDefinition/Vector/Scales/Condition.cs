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
    internal partial class Condition : UserControl
    {
        private IPointRule m_prt;
        private ILineRule m_lrt;
        private IAreaRule m_art;
        private bool m_isUpdating = false;

        public event EventHandler ItemDeleted;
        public event EventHandler ItemChanged;

        private VectorLayerEditorCtrl m_owner;

        public VectorLayerEditorCtrl Owner
        {
            get { return m_owner; }
            set
            {
                m_owner = value;
                FeatureStyle.Owner = m_owner;
                LabelStyle.Owner = m_owner;
            }
        }

        public Condition()
        {
            InitializeComponent();
        }

        public void SetItem(IPointRule prt)
        {
            SetItemInternal(prt);
        }

        public void SetItem(ILineRule lrt)
        {
            SetItemInternal(lrt);
        }

        public void SetItem(IAreaRule art)
        {
            SetItemInternal(art);
        }

        private void SetItemInternal(object item)
        {
            m_prt = item as IPointRule;
            m_lrt = item as ILineRule;
            m_art = item as IAreaRule;

            try
            {
                m_isUpdating = true;
                if (m_prt != null)
                {
                    RuleCondition.Text = m_prt.Filter;
                    LegendLabel.Text = m_prt.LegendLabel;
                    FeatureStyle.SetItem(m_prt, m_prt.PointSymbolization2D);
                    LabelStyle.SetItem(m_prt, m_prt.Label);
                }
                else if (m_lrt != null)
                {
                    RuleCondition.Text = m_lrt.Filter;
                    LegendLabel.Text = m_lrt.LegendLabel;
                    FeatureStyle.SetItem(m_lrt, m_lrt.Strokes);
                    LabelStyle.SetItem(m_lrt, m_lrt.Label);
                }
                else if (m_art != null)
                {
                    RuleCondition.Text = m_art.Filter;
                    LegendLabel.Text = m_art.LegendLabel;
                    FeatureStyle.SetItem(m_art, m_art.AreaSymbolization2D);
                    LabelStyle.SetItem(m_art, m_art.Label);
                }
            }
            finally
            {
                m_isUpdating = false;
            }
        }

        public object Item
        {
            get
            {
                if (m_prt != null)
                    return m_prt;
                else if (m_lrt != null)
                    return m_lrt;
                else if (m_art != null)
                    return m_art;
                else
                    return null;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (ItemDeleted != null)
                if (m_prt != null)
                    ItemDeleted(m_prt, null);
                else if (m_lrt != null)
                    ItemDeleted(m_lrt, null);
                else if (m_art != null)
                    ItemDeleted(m_art, null);
        }

        private void RuleCondition_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;

            if (m_prt != null)
                m_prt.Filter = RuleCondition.Text;
            else if (m_lrt != null)
                m_lrt.Filter = RuleCondition.Text;
            else if (m_art != null)
                m_art.Filter = RuleCondition.Text;

            SignalChanged();
        }

        private void EditFilter_Click(object sender, EventArgs e)
        {
            string tmp = m_owner.EditExpression(RuleCondition.Text);
            if (tmp != null)
                RuleCondition.Text = tmp;
        }

        private void LegendLabel_TextChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;

            if (m_prt != null)
                m_prt.LegendLabel = LegendLabel.Text;
            else if (m_lrt != null)
                m_lrt.LegendLabel = LegendLabel.Text;
            else if (m_art != null)
                m_art.LegendLabel = LegendLabel.Text;

            SignalChanged();
        }

        private void SignalChanged()
        {
            if (m_isUpdating)
                return;

            if (m_prt != null)
            {
                if (ItemChanged != null)
                    ItemChanged(m_prt, null);
            }
            else if (m_lrt != null)
            {
                if (ItemChanged != null)
                    ItemChanged(m_lrt, null);
            }
            else if (m_art != null)
            {
                if (ItemChanged != null)
                    ItemChanged(m_art, null);
            }
        }

        private void LabelStyle_ItemChanged(object sender, EventArgs e)
        {
            SignalChanged();
        }

        private void FeatureStyle_ItemChanged(object sender, EventArgs e)
        {
            SignalChanged();
        }

        private void FeatureStyle_Click(object sender, EventArgs e)
        {
            this.Focus();
        }

    }
}
