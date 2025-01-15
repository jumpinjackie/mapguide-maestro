#region Disclaimer / License

// Copyright (C) 2025, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using System.Windows.Forms;

namespace Maestro.Editors.Common
{
    /// <summary>
    /// A generic prompt dialog for capturing a string-based value
    /// </summary>
    public partial class PromptDialog : Form
    {
        private PromptDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Display a prompt dialog to capture a string-based value
        /// </summary>
        /// <param name="title"></param>
        /// <param name="prompt"></param>
        /// <param name="initialAnswer"></param>
        /// <returns>The prompt response</returns>
        public static string Show(string title, string prompt, string initialAnswer = "")
        {
            var diag = new PromptDialog();
            diag.Text = title;
            diag.lblPrompt.Text = prompt;
            diag.txtAnswer.Text = initialAnswer;
            if (diag.ShowDialog() == DialogResult.OK)
                return diag.txtAnswer.Text;
            return null;
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
