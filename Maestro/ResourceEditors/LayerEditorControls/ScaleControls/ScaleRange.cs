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
    public partial class ScaleRange : UserControl
    {
        private VectorScaleRangeType m_vsc;
        private bool m_isUpdating = false;

        public event EventHandler ItemChanged;
        
        private VectorLayer m_owner;

        public VectorLayer Owner
        {
            get { return m_owner; }
            set
            {
                m_owner = value;
                scaleRangeConditions.Owner = m_owner;
            }
        }


        public ScaleRange()
        {
            InitializeComponent();
        }

        public VectorScaleRangeType Item { get { return m_vsc; } }

        public void SetItem(VectorScaleRangeType vsc)
        {
            try
            {
                m_isUpdating = true;
                m_vsc = vsc;
                MaxScale.Text = m_vsc.MaxScaleSpecified ? m_vsc.MaxScale.ToString(Globalizator.Globalizator.CurrentCulture) : "infinite";
                MinScale.Text = m_vsc.MinScaleSpecified ? m_vsc.MinScale.ToString(Globalizator.Globalizator.CurrentCulture) : "infinite";

                scaleRangeConditions.SetItem(m_vsc);
            }
            finally
            {
                m_isUpdating = false;
            }
        }

        private void MinScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_vsc == null || m_isUpdating)
                return;

            if (MinScale.SelectedIndex == 0)
                m_vsc.MinScaleSpecified = false;
        }

        private void MaxScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_vsc == null || m_isUpdating)
                return;

            if (MaxScale.SelectedIndex == 0)
                m_vsc.MaxScaleSpecified = false;

        }

        private void MinScale_TextChanged(object sender, EventArgs e)
        {
            if (m_vsc == null || m_isUpdating)
                return;

            double o;

            if (double.TryParse(MinScale.Text, System.Globalization.NumberStyles.Number, Globalizator.Globalizator.CurrentCulture, out o))
            {
                m_vsc.MinScaleSpecified = true;
                m_vsc.MinScale = o;
                errorProvider.SetError(MinScale, null);
            }
            else
                errorProvider.SetError(MinScale, "Invalid value");


        }

        private void MaxScale_TextChanged(object sender, EventArgs e)
        {
            if (m_vsc == null || m_isUpdating)
                return;

            double o;

            if (double.TryParse(MaxScale.Text, System.Globalization.NumberStyles.Number, Globalizator.Globalizator.CurrentCulture, out o))
            {
                m_vsc.MaxScaleSpecified = true;
                m_vsc.MaxScale = o;
                errorProvider.SetError(MaxScale, null);
            }
            else
                errorProvider.SetError(MaxScale, "Invalid value");
        }

        private void scaleRangeConditions_ItemChanged(object sender, EventArgs e)
        {
            if (m_vsc == null || m_isUpdating)
                return;

            if (ItemChanged != null)
                ItemChanged(m_vsc, null);
        }
    }
}
