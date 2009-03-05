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
        public ValidationResults(ResourceValidators.ValidationIssue[] issues)
            : this()
        {
            listView1.Items.Clear();

            foreach (ResourceValidators.ValidationIssue issue in issues)
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
    }
}