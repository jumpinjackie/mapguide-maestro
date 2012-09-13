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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Maestro.Shared.UI
{
    /// <summary>
    /// A flexible error dialog for displaying error messages and exceptions
    /// </summary>
    public partial class ErrorDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDialog"/> class.
        /// </summary>
        public ErrorDialog()
        {
            InitializeComponent();
            saveFileDialog.Filter = string.Format("{0} (*.{1})|*.{1}", Strings.PickTxt, "txt"); //NOXLATE
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName,
                    string.Format(Properties.Resources.ErrorLogTemplate,
                        Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                        DateTime.Now.ToString(),
                        txtMessage.Text,
                        txtDetails.Text));

                MessageBox.Show(Strings.FileSaved);
            }
        }

        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="detail">The detail.</param>
        public static void Show(string message, string detail)
        {
            var diag = new ErrorDialog();
            diag.txtMessage.Text = message;
            diag.txtDetails.Text = detail;
            diag.ShowDialog();    
        }

        /// <summary>
        /// Shows the specified exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public static void Show(Exception ex)
        {
            Show(ex.Message, ex.ToString());
        }
    }
}
