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

namespace OSGeo.MapGuide.Maestro.PackageManager
{
    public partial class AddResourceEntry : Form
    {
        public AddResourceEntry()
        {
            InitializeComponent();
        }

        private void UseHeader_CheckedChanged(object sender, EventArgs e)
        {
            HeaderPath.Enabled = BrowseHeaderButton.Enabled = UseHeader.Checked;
        }

        private void BrowseHeaderButton_Click(object sender, EventArgs e)
        {
            if (BrowseFileDialog.ShowDialog(this) == DialogResult.OK)
                HeaderPath.Text = BrowseFileDialog.FileName;
        }

        private void BrowseContentButton_Click(object sender, EventArgs e)
        {
            if (BrowseFileDialog.ShowDialog(this) == DialogResult.OK)
                ContentPath.Text = BrowseFileDialog.FileName;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (UseAlternateName.Checked && AlternateName.Text.Trim().Length == 0)
                {
                    MessageBox.Show(this, Strings.AddResourceEntry.AlternateNameMissing, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AlternateName.Focus();
                    return;
                }

                if (UseHeader.Checked && !System.IO.File.Exists(HeaderPath.Text))
                {
                    MessageBox.Show(this, Strings.AddResourceEntry.HeaderFileMissing, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    HeaderPath.Focus();
                    return;
                }

                if (!System.IO.File.Exists(ContentPath.Text))
                {
                    MessageBox.Show(this, Strings.AddResourceEntry.ContentFileMissing, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ContentPath.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(Strings.AddResourceEntry.FilenameValidationError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string ContentFilepath { get { return ContentPath.Text; } }
        public string HeaderFilepath { get { return UseHeader.Checked ? HeaderPath.Text : null; } }
        public string ResourceName { get { return UseAlternateName.Checked ? AlternateName.Text : System.IO.Path.GetFileName(ContentPath.Text); } }

        private void UseAlternateName_CheckedChanged(object sender, EventArgs e)
        {
            AlternateName.Enabled = UseAlternateName.Checked;
        }

        private void ContentPath_TextChanged(object sender, EventArgs e)
        {
            try { AlternateName.Text = System.IO.Path.GetFileName(ContentPath.Text); }
            catch { }
        }

        private void AddResourceEntry_Load(object sender, EventArgs e)
        {
            this.Show();
            ContentPath.Focus();
        }
    }
}