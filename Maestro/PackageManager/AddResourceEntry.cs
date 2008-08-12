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
                    MessageBox.Show(this, "You must enter a alternate name, or remove the checkmark", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AlternateName.Focus();
                    return;
                }

                if (UseHeader.Checked && !System.IO.File.Exists(HeaderPath.Text))
                {
                    MessageBox.Show(this, "The header file does not exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    HeaderPath.Focus();
                    return;
                }

                if (!System.IO.File.Exists(ContentPath.Text))
                {
                    MessageBox.Show(this, "The content file does not exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ContentPath.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("Failed to validate the filenames: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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