using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.PackageManager
{
    public partial class PackageProgress : Form
    {
        private bool m_allowClose = true;

        public PackageProgress()
        {
            InitializeComponent();
        }

        private void PackageProgress_Load(object sender, EventArgs e)
        {
            m_allowClose = false;
        }

        private void PackageProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_allowClose && e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show(this, "Do you want to cancel?", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3) == DialogResult.Yes)
                    m_allowClose = true;
                else
                    e.Cancel = true;
            }
        }

        private delegate void SetOperationDelegate(string operation);
        public void SetOperation(string operation)
        {
            if (this.InvokeRequired)
                this.Invoke(new SetOperationDelegate(SetOperation), new object[] { operation });
            else
            {
                OperationLabel.Text = operation;
            }
        }

        private delegate void SetCurrentProgressDelegate(int pg, int total);
        public void SetCurrentProgress(int pg, int total)
        {
            if (this.InvokeRequired)
                this.Invoke(new SetCurrentProgressDelegate(SetCurrentProgress), new object[] { pg, total });
            else
            {
                int span = this.CurrentProgress.Maximum - this.CurrentProgress.Minimum;
                double v = (((double)pg / total) * span) + this.CurrentProgress.Minimum;
                this.CurrentProgress.Value = (int)Math.Max(Math.Min(v, this.CurrentProgress.Maximum), this.CurrentProgress.Minimum);
                this.Update();
            }
        }

        private delegate void SetTotalOperationsDelegate(int count);
        public void SetTotalOperations(int count)
        {
            if (this.InvokeRequired)
                this.Invoke(new SetTotalOperationsDelegate(SetTotalOperations), new object[] { count });
            else
            {
                double pg = this.TotalProgress.Value / (double)this.TotalProgress.Maximum;
                this.TotalProgress.Maximum = count;
                this.TotalProgress.Value = (int)Math.Max(Math.Min(this.TotalProgress.Maximum * pg, this.TotalProgress.Maximum), this.TotalProgress.Minimum);
            }
        }

        private delegate void SetOperationNoDelegate(int no);
        public void SetOperationNo(int no)
        {
            if (this.InvokeRequired)
                this.Invoke(new SetOperationNoDelegate(SetOperationNo), new object[] { no });
            else
            {
                this.TotalProgress.Value = Math.Max(Math.Min(no, this.TotalProgress.Maximum), this.TotalProgress.Minimum);
            }
        }

        private delegate void CancelDelegate();
        public void Cancel()
        {
            if (this.InvokeRequired)
                this.Invoke(new CancelDelegate(Cancel));
            else
            {
                m_allowClose = true;
                this.DialogResult = DialogResult.Cancel;
                base.Close();
            }
        }

        private delegate void CloseDelegate();
        public new void Close()
        {
            if (this.InvokeRequired)
                this.Invoke(new CloseDelegate(Close));
            else
            {
                m_allowClose = true;
                this.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private delegate void HideTotalDelegate();
        public void HideTotal()
        {
            if (this.InvokeRequired)
                this.Invoke(new HideTotalDelegate(HideTotal));
            else
            {
                if (TotalProgress.Visible)
                {
                    TotalLabel.Visible = TotalProgress.Visible = false;
                    this.Height -=  (TotalProgress.Height + (CurrentProgress.Bottom - TotalProgress.Top));
                }
            }
        }
    }
}