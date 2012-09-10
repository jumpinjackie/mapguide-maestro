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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Maestro.Editors.Common;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    /// <summary>
    /// Summary description for FillStyleEditor.
    /// </summary>
    [ToolboxItem(false)]
    internal partial class FillStyleEditor : System.Windows.Forms.UserControl
    {
        public FillStyleEditor()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        
            fillCombo.Items.Clear();
            fillCombo.Items.AddRange(FeaturePreviewRender.FillImages);
        }

        private void FillStyleEditor_Load(object sender, System.EventArgs e)
        {
        }

        private void displayFill_CheckedChanged(object sender, System.EventArgs e)
        {
            lblFill.Enabled = 
            lblForeground.Enabled = 
            lblBackground.Enabled = 
            fillCombo.Enabled =
            foregroundColor.Enabled =
            backgroundColor.Enabled = 
                displayFill.Checked;
        }

        public event EventHandler ForegroundRequiresExpression;
        public event EventHandler BackgroundRequiresExpression;

        private void foregroundColor_RequestExpressionEditor(object sender, EventArgs e)
        {
            var handler = this.ForegroundRequiresExpression;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void backgroundColor_RequestExpressionEditor(object sender, EventArgs e)
        {
            var handler = this.BackgroundRequiresExpression;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
