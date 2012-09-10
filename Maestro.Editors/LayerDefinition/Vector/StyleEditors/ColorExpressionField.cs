#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Common;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    /// <summary>
    /// A field for entering color values by color picker selection or FDO expressions
    /// </summary>
    [ToolboxItem(true)]
    public partial class ColorExpressionField : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorExpressionField"/> class.
        /// </summary>
        public ColorExpressionField()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the color expression.
        /// </summary>
        /// <value>
        /// The color expression.
        /// </value>
        [DefaultValue("00000000")] //NOXLATE
        public string ColorExpression
        {
            get { return txtColor.Text; }
            set 
            {
                txtColor.Text = value;
            }
        }

        /// <summary>
        /// Occurs when [current color changed].
        /// </summary>
        public event EventHandler CurrentColorChanged;

        /// <summary>
        /// Occurs when [request expression editor].
        /// </summary>
        public event EventHandler RequestExpressionEditor;

        private void btnExpr_Click(object sender, EventArgs e)
        {
            var handler = this.RequestExpressionEditor;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void txtColor_TextChanged(object sender, EventArgs e)
        {
            var handler = this.CurrentColorChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            using (var picker = new ColorPickerDialog())
            {
                Color? currentColor = null;
                try
                {
                    currentColor = Utility.ParseHTMLColor(this.ColorExpression);
                }
                catch { }

                if (currentColor.HasValue)
                    picker.SelectedColor = currentColor.Value;

                if (picker.ShowDialog() == DialogResult.OK)
                {
                    this.ColorExpression = Utility.SerializeHTMLColor(picker.SelectedColor, true).ToUpper();
                }
            }
        }
    }
}
