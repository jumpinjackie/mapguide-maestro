using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro
{
    public partial class WaitForOperation : Form
    {
        public delegate object DoBackgroundWork(BackgroundWorker worker, DoWorkEventArgs e, params object[] args);

        private DoBackgroundWork m_method;
        private object[] m_args;
        private object m_result;
        private bool m_cancelAborts = false;

        private System.Threading.Thread m_worker;

        public WaitForOperation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// A value indicating if the cancel button attempts to abort the thread, rather than simply flag it for cancellation
        /// </summary>
        public bool CancelAbortsThread 
        {
            get { return m_cancelAborts; }
            set { m_cancelAborts = value; }
        }

        public object RunOperationAsync(Form owner, DoBackgroundWork method, params object[] arguments)
        {
            m_method = method;
            m_args = arguments;
            if (this.Visible)
                this.Hide();

            if (this.ShowDialog(owner) == DialogResult.OK)
                return m_result;
            else
                throw new CancelException();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                m_worker = System.Threading.Thread.CurrentThread;
                e.Result = m_method(BackgroundWorker, e, m_args);
            }
            catch (System.Threading.ThreadAbortException)
            {
                e.Cancel = true;
                e.Result = null;
                //We exit, but hide the abort details from the BackgroundWorker, so it processes events as it should
                System.Threading.Thread.ResetAbort();
            }
            finally
            {
                m_worker = null;
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 0)
            {
                progressBar1.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                if (progressBar1.Style == ProgressBarStyle.Marquee)
                    progressBar1.Style = ProgressBarStyle.Blocks;

                progressBar1.Value = Math.Min(Math.Max(progressBar1.Minimum, e.ProgressPercentage), progressBar1.Maximum);
            }

            if (e.UserState != null)
                label1.Text = e.UserState.ToString();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else if (e.Error != null)
            {
                throw e.Error;
            }
            else
            {
                m_result = e.Result;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void WaitForOperation_Load(object sender, EventArgs e)
        {
            BackgroundWorker.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CancelBtn.Enabled = false;
            if (!BackgroundWorker.CancellationPending)
                BackgroundWorker.CancelAsync();

            if (m_cancelAborts)
            {
                try
                {
                    //Protected, because threading can make it null after the check
                    if (m_worker != null && m_worker.IsAlive)
                        m_worker.Abort();
                }
                catch
                {
                }
            }
        }

        private void WaitForOperation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                button1_Click(sender, e);
        }
    }
}