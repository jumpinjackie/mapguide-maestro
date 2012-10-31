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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Exceptions;

namespace Maestro.Base
{
    /// <summary>
    /// A dialog for displaying validation results
    /// </summary>
    public partial class ValidationResultsDialog : Form
    {
        private List<KeyValuePair<string, ValidationIssue[]>> m_issues = new List<KeyValuePair<string, ValidationIssue[]>>();

        private Action<IResource> _openAction;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="issues"></param>
        public ValidationResultsDialog(string resourceId, ValidationIssue[] issues)
            : this(resourceId, issues, null)
        { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="issues"></param>
        /// <param name="openAction"></param>
        public ValidationResultsDialog(string resourceId, ValidationIssue[] issues, Action<IResource> openAction)
            : this(
                new List<KeyValuePair<string, ValidationIssue[]>>(
                    new KeyValuePair<string, ValidationIssue[]>[] { 
                        new KeyValuePair<string, ValidationIssue[]>(resourceId, issues) 
                    }
                ),
                openAction
            )
        { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="issues"></param>
        public ValidationResultsDialog(List<KeyValuePair<string, ValidationIssue[]>> issues)
            : this(issues, null)
        {

        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="issues"></param>
        /// <param name="openAction"></param>
        public ValidationResultsDialog(List<KeyValuePair<string, ValidationIssue[]>> issues, Action<IResource> openAction)
            : this()
        {
            m_issues = issues;
            PopulateIssues();
            _openAction = openAction;
            btnOpen.Visible = (_openAction != null);
            btnOpen.Enabled = false;
        }

        private void PopulateIssues()
        {
            lstIssues.Items.Clear();
            foreach (KeyValuePair<string, ValidationIssue[]> e in m_issues)
            {
                IEnumerable<ValidationIssue> items = null;
                if (chkErrors.Checked)
                {
                    if (items == null)
                        items = e.Value.Where(x => x.Status == ValidationStatus.Error);
                    else
                        items = items.Concat(e.Value.Where(x => x.Status == ValidationStatus.Error));
                }
                if (chkWarnings.Checked)
                {
                    if (items == null)
                        items = e.Value.Where(x => x.Status == ValidationStatus.Warning);
                    else
                        items = items.Concat(e.Value.Where(x => x.Status == ValidationStatus.Warning));
                }
                if (chkNotices.Checked)
                {
                    if (items == null)
                        items = e.Value.Where(x => x.Status == ValidationStatus.Information);
                    else
                        items = items.Concat(e.Value.Where(x => x.Status == ValidationStatus.Information));
                }

                if (items == null)
                    continue;

                foreach (ValidationIssue issue in items)
                {
                    if (issue == null || issue.Resource == null || string.IsNullOrEmpty(issue.Message) || string.IsNullOrEmpty(issue.Resource.ResourceID))
                        continue;

                    ListViewItem lvi = new ListViewItem(issue.Resource.ResourceID);
                    switch (issue.Status)
                    {
                        case ValidationStatus.Information:
                            lvi.ImageIndex = 0;
                            break;
                        case ValidationStatus.Warning:
                            lvi.ImageIndex = 1;
                            break;
                        case ValidationStatus.Error:
                            lvi.ImageIndex = 2;
                            break;
                        default:
                            lvi.ImageIndex = -1;
                            break;
                    }
                    lvi.Tag = issue;
                    lvi.SubItems.Add(issue.Message);
                    lvi.SubItems.Add(issue.StatusCode.ToString());
                    lstIssues.Items.Add(lvi);
                }
            }
        }

        private ValidationResultsDialog()
        {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveReportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog.FileName, false))
                    {
                        foreach (KeyValuePair<string, ValidationIssue[]> p in m_issues)
                        {
                            if (p.Value.Length > 0)
                            {
                                sw.WriteLine(new string('*', 80)); //NOXLATE
                                sw.WriteLine(string.Format(Strings.ValidationProgressMessage, p.Key));
                                foreach (ValidationIssue i in p.Value)
                                    sw.WriteLine(string.Format(Strings.ValidationResultFormat, i.Status, i.StatusCode, i.Message));

                                sw.WriteLine();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                MessageBox.Show(this, string.Format(Strings.ValidationFailedError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnResultFilterCheckedChanged(object sender, EventArgs e)
        {
            PopulateIssues();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (_openAction != null && lstIssues.SelectedItems.Count == 1)
            {
                _openAction(((ValidationIssue)lstIssues.SelectedItems[0].Tag).Resource);
            }
        }

        private void lstIssues_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOpen.Enabled = (lstIssues.SelectedItems.Count == 1);
            btnOpen.Visible = (_openAction != null);
        }
    }
}