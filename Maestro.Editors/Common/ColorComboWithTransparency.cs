#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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

namespace Maestro.Editors.Common
{
    /// <summary>
    /// A color combo box with transparency support
    /// </summary>
    public partial class ColorComboWithTransparency : UserControl
    {
        private bool m_isUpdating = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorComboWithTransparency"/> class.
        /// </summary>
        public ColorComboWithTransparency()
        {
            InitializeComponent();
            colorCombo.AllowTransparent = true;
            colorCombo.ResetColors();
        }

        /// <summary>
        /// Occurs when [current color changed].
        /// </summary>
        public event EventHandler CurrentColorChanged;

        /// <summary>
        /// Gets or sets the color of the current.
        /// </summary>
        /// <value>The color of the current.</value>
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
