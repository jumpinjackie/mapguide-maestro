using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors
{
    public partial class ColorComboWithTransparency : UserControl
    {
        private bool m_isUpdating = false;

        public ColorComboWithTransparency()
        {
            InitializeComponent();
            colorCombo.AllowTransparent = true;
            colorCombo.ResetColors();
        }

        public event EventHandler CurrentColorChanged;

        public Color CurrentColor
        {
            get { return Color.FromArgb((byte.MaxValue - transparencySlider.Value), colorCombo.CurrentColor); }
            set
            {
                try
                {
                    m_isUpdating = true;

                    if (value.A == 0)
                        colorCombo.CurrentColor = Color.Transparent;
                    else
                        colorCombo.CurrentColor = Color.FromArgb(byte.MaxValue, value);
                    transparencySlider.Value = byte.MaxValue - value.A;

                    if (CurrentColorChanged != null)
                        CurrentColorChanged(this, null);
                    
                    colorCombo.Refresh();
                }
                finally
                {
                    m_isUpdating = false;
                }
            }
        }

        private void colorCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_isUpdating)
                return;

            if (CurrentColorChanged != null)
                CurrentColorChanged(this, null);
        }

        private void transparencySlider_ValueChanged(object sender, EventArgs e)
        {
            percentageLabel.Text = ((int)((transparencySlider.Value / (double)byte.MaxValue) * 100)).ToString() + "%";

            if (m_isUpdating)
                return;

            if (CurrentColorChanged != null)
                CurrentColorChanged(this, null);
        }
    }
}
