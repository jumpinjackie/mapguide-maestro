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
    public partial class Condition : UserControl
    {
        private PointRuleType m_prt;
        private LineRuleType m_lrt;
        private AreaRuleType m_art;
        private bool m_isUpdating = false;

        public event EventHandler ItemDeleted;
        public event EventHandler ItemChanged;

        public Condition()
        {
            InitializeComponent();
        }

        public void SetItem(PointRuleType prt)
        {
            SetItemInternal(prt);
        }

        public void SetItem(LineRuleType lrt)
        {
            SetItemInternal(lrt);
        }

        public void SetItem(AreaRuleType art)
        {
            SetItemInternal(art);
        }

        private void SetItemInternal(object item)
        {
            m_prt = item as PointRuleType;
            m_lrt = item as LineRuleType;
            m_art = item as AreaRuleType;

            try
            {
                m_isUpdating = true;
                if (m_prt != null)
                {
                    RuleCondition.Text = m_prt.Filter;
                    LegendLabel.Text = m_prt.LegendLabel;
                    FeatureStyle.SetItem(m_prt, m_prt.Item);
                    LabelStyle.SetItem(m_prt, m_prt.Label);
                }
                else if (m_lrt != null)
                {
                    RuleCondition.Text = m_lrt.Filter;
                    LegendLabel.Text = m_lrt.LegendLabel;
                    FeatureStyle.SetItem(m_lrt, m_lrt.Items);
                    LabelStyle.SetItem(m_lrt, m_lrt.Label);
                }
                else if (m_art != null)
                {
                    RuleCondition.Text = m_art.Filter;
                    LegendLabel.Text = m_art.LegendLabel;
                    FeatureStyle.SetItem(m_art, m_art.Item);
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
            MessageBox.Show(this, "This feature is not yet implemented", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    }
}
