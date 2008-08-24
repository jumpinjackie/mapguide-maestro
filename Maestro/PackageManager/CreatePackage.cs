using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.PackageManager
{
    public partial class CreatePackage : Form
    {
        private OSGeo.MapGuide.Maestro.ResourceEditors.EditorInterface m_editor;
        private bool m_isUpdating = false;

        public CreatePackage()
        {
            InitializeComponent();
        }

        public void Setup(OSGeo.MapGuide.Maestro.ResourceEditors.EditorInterface editor, string startpath)
        {
            try
            {
                m_isUpdating = true;
                m_editor = editor;
                AllowedTypes.Items.Clear();
                AllowedTypes.Items.Add("All types", true);
                foreach (string s in ((ServerConnectionBase)m_editor.CurrentConnection).ResourceTypeLookup.Keys)
                    AllowedTypes.Items.Add(s, true);
                AllowedTypes.Items.Add("Unknown types", true);
                if (!string.IsNullOrEmpty(startpath))
                    ResourcePath.Text = startpath.IndexOf('.') > 0 ? startpath.Substring(0, startpath.LastIndexOf('/')) : startpath;
            }
            finally
            {
                m_isUpdating = false;
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (AllowedTypes.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "You must select at least one type", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                if (PackageFilename.Text.Trim().Length == 0 || !System.IO.Path.IsPathRooted(PackageFilename.Text))
                {
                    MessageBox.Show(this, "You must enter a full path to the output file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("An error occured while validating the output file path: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string restorePath = null;

            try
            {
                if (EnableRestorePath.Checked)
                {
                    ResourceIdentifier.Validate("Library://" + RestorePath.Text, ResourceTypes.Folder);
                    if (string.IsNullOrEmpty(RestorePath.Text))
                        if (MessageBox.Show(this, "You have selected to restore the package at another location, but not entered one\r\nThis will cause the package to be restored a the root of the resource tree.\r\nAre you sure this is what you want?", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes)
                            return;

                    restorePath = "Library://" + RestorePath.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("An error occured while validating the restore path: {0}\nIt should have the format: \"Libray://folder/folder/\".", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<string> extensions = new List<string>();
            for (int i = 1; i < AllowedTypes.Items.Count - 1; i++)
                if (AllowedTypes.GetItemChecked(i))
                    extensions.Add(AllowedTypes.Items[i].ToString());
            if (AllowedTypes.GetItemChecked(AllowedTypes.Items.Count - 1))
                extensions.Add("*");

            if (RemoveTargeOnRestore.Checked && ((restorePath != null && restorePath == "Library://") || (restorePath == null && ResourcePath.Text == "Library://")))
                if (MessageBox.Show(this, "You have selected to restore the package at the root.\r\nYou have also selected to delete the target before restoring.\r\nThis will result in the entire repository being deleted and replaced with this package.\r\nAre you absolutely sure that is what you want?", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes)
                    return;



            PackageBuilder pkb = new PackageBuilder(this, ResourcePath.Text, PackageFilename.Text, restorePath, extensions.ToArray(), RemoveTargeOnRestore.Checked, m_editor.CurrentConnection);
            this.DialogResult = pkb.Start();
            this.Close();
        }

        private void AllowedTypes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (m_isUpdating)
                return;

            try
            {
                m_isUpdating = true;
                if (e.Index == 0)
                {
                    for (int i = 1; i < AllowedTypes.Items.Count; i++)
                        AllowedTypes.SetItemChecked(i, e.NewValue == CheckState.Checked);
                }
                else
                {
                    int checkCount = 0;

                    for (int i = 1; i < AllowedTypes.Items.Count; i++)
                        if (i != e.Index)
                            checkCount += AllowedTypes.GetItemChecked(i) ? 1 : 0;
                        else
                            checkCount += e.NewValue == CheckState.Checked ? 1 : 0;

                    if (checkCount == 0)
                        AllowedTypes.SetItemChecked(0, false);
                    else if (checkCount == AllowedTypes.Items.Count - 1)
                        AllowedTypes.SetItemChecked(0, true);
                    else
                        AllowedTypes.SetItemCheckState(0, CheckState.Indeterminate);
                }
            }
            finally
            {
                m_isUpdating = false;
            }

        }

        private void BrowseResourcePath_Click(object sender, EventArgs e)
        {
            //TODO: can't select folder
            string path = m_editor.BrowseResource("Folder");
            if (path != null)
                ResourcePath.Text = path;
        }

        private void EnableRestorePath_CheckedChanged(object sender, EventArgs e)
        {
            RestorePath.Enabled = LibraryLabel.Enabled = EnableRestorePath.Checked;
        }

        private void BrowseTargetFilename_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                PackageFilename.Text = saveFileDialog.FileName;
        }

    }
}