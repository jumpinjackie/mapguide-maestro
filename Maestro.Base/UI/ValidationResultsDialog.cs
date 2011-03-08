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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Exceptions;

namespace Maestro.Base
{
    public partial class ValidationResultsDialog : Form
    {
        private List<KeyValuePair<string, ValidationIssue[]>> m_issues = new List<KeyValuePair<string, ValidationIssue[]>>();

        public ValidationResultsDialog(string resourceId, ValidationIssue[] issues)
            : this(
                new List<KeyValuePair<string, ValidationIssue[]>>(
                    new KeyValuePair<string, ValidationIssue[]>[] { 
                        new KeyValuePair<string, ValidationIssue[]>(resourceId, issues) 
                    }
                )
            )
        { }

        public ValidationResultsDialog(List<KeyValuePair<string, ValidationIssue[]>> issues)
            : this()
        {
            listView1.Items.Clear();

            m_issues = issues;

            foreach (KeyValuePair<string, ValidationIssue[]> e in issues)
            {
                foreach (ValidationIssue issue in e.Value)
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
                    lvi.SubItems.Add(issue.Message);
                    listView1.Items.Add(lvi);
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
                                sw.WriteLine(new string('*', 80));
                                sw.WriteLine(string.Format(Properties.Resources.ValidationProgressMessage, p.Key));
                                foreach (ValidationIssue i in p.Value)
                                    sw.WriteLine(string.Format(Properties.Resources.ValidationResultFormat, i.Status, i.Resource, i.Message));

                                sw.WriteLine();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                MessageBox.Show(this, string.Format(Properties.Resources.ValidationFailedError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}