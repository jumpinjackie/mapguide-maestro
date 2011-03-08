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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.LayerDefinition.Vector.Scales
{
    internal partial class EditorTemplateForm : Form
    {
        public EditorTemplateForm()
        {
            InitializeComponent();
        }

        public void RefreshSize()
        {
            if (ItemPanel.Controls.Count > 0 && ItemPanel.Controls[0] as UserControl != null)
            {
                this.Height = ButtonPanel.Height + ItemPanel.Top + (ItemPanel.Controls[0] as UserControl).AutoScrollMinSize.Height + (8 * 6);

                this.Width = Math.Max(this.Width, (ItemPanel.Controls[0] as UserControl).AutoScrollMinSize.Width + 2 * ItemPanel.Left + (8 * 4));
            }
        }

        private void EditorTemplateForm_Load(object sender, EventArgs e)
        {
            RefreshSize();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}