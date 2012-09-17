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
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Maestro.Shared.UI
{
    /// <summary>
    /// Generic progress dialog
    /// </summary>
    public partial class ProgressDialog : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public delegate object DoBackgroundWork(BackgroundWorker worker, DoWorkEventArgs e, params object[] args);

        private DoBackgroundWork m_method;
        private object[] m_args;
        private object m_result;
        private bool m_cancelAborts = false;
        private CultureInfo m_culture;

        private System.Threading.Thread m_worker;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog"/> class.
        /// </summary>
        public ProgressDialog()
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

        /// <summary>
        /// Runs the operation async.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="method">The method.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public object RunOperationAsync(Form owner, DoBackgroundWork method, params object[] arguments)
        {
            return RunOperationAsync(owner, method, true, arguments);
        }

        /// <summary>
        /// Runs the operation async.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="method">The method.</param>
        /// <param name="bPreserveThreadCulture">If true, the background thread's culture will be set to the culture of the invoking thread</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public object RunOperationAsync(Form owner, DoBackgroundWork method, bool bPreserveThreadCulture, params object[] arguments)
        {
            m_method = method;
            m_args = arguments;
            if (this.Visible)
                this.Hide();

            if (bPreserveThreadCulture)
                m_culture = Thread.CurrentThread.CurrentCulture;

            if (this.ShowDialog(owner) == DialogResult.OK)
                return m_result;
            else
                throw new CancelException();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (m_culture != null)
                {
                    Thread.CurrentThread.CurrentCulture =
                        Thread.CurrentThread.CurrentUICulture = m_culture;
                }
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
                ErrorDialog.Show(e.Error);
                this.DialogResult = DialogResult.Abort;
                this.Close();
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
            BackgroundWorker.WorkerSupportsCancellation = !m_cancelAborts;
            BackgroundWorker.RunWorkerAsync();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            CancelBtn.Enabled = false;
            if (!BackgroundWorker.CancellationPending && BackgroundWorker.WorkerSupportsCancellation)
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
                CancelBtn_Click(sender, e);
        }

        private void WaitForOperation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape && CancelBtn.Enabled)
                CancelBtn_Click(sender, e);
        }
    }
}