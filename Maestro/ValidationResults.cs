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

namespace OSGeo.MapGuide.Maestro
{
    public partial class ValidationResults : Form
    {
        private List<KeyValuePair<string, ResourceValidators.ValidationIssue[]>> m_issues = new List<KeyValuePair<string,OSGeo.MapGuide.Maestro.ResourceValidators.ValidationIssue[]>>();

        public ValidationResults(string resourceId, ResourceValidators.ValidationIssue[] issues)
            : this(new List<KeyValuePair<string, ResourceValidators.ValidationIssue[]>>(new KeyValuePair<string, ResourceValidators.ValidationIssue[]>[] { new KeyValuePair<string, ResourceValidators.ValidationIssue[]>(resourceId, issues) }))
        { }

        public ValidationResults(List<KeyValuePair<string, ResourceValidators.ValidationIssue[]>> issues)
            : this()
        {
            listView1.Items.Clear();

            m_issues = issues;

            foreach(KeyValuePair<string, ResourceValidators.ValidationIssue[]> e in issues)
                foreach (ResourceValidators.ValidationIssue issue in e.Value )
                {
                    if (issue == null || issue.Resource == null || issue.Resource.GetType().GetProperty("ResourceId") == null)
                        continue;

                    ListViewItem lvi = new ListViewItem((string)issue.Resource.GetType().GetProperty("ResourceId").GetValue(issue.Resource, null));
                    switch (issue.Status)
                    {
                        case OSGeo.MapGuide.Maestro.ResourceValidators.ValidationStatus.Information:
                            lvi.ImageIndex = 0;
                            break;
                        case OSGeo.MapGuide.Maestro.ResourceValidators.ValidationStatus.Warning:
                            lvi.ImageIndex = 1;
                            break;
                        case OSGeo.MapGuide.Maestro.ResourceValidators.ValidationStatus.Error:
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

        private ValidationResults()
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
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.FileName, false))
                    {
                        foreach (KeyValuePair<string, ResourceValidators.ValidationIssue[]> p in m_issues)
                        {
                            if (p.Value.Length > 0)
                            {
                                sw.WriteLine(new string('*', 80));
                                sw.WriteLine(string.Format("Validating file: {0}", p.Key));
                                foreach (ResourceValidators.ValidationIssue i in p.Value)
                                    sw.WriteLine(string.Format("{0} - {1}: {2}", i.Status, i.Resource, i.Message));

                                sw.WriteLine();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to save file: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}