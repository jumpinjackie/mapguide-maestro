#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace Maestro.Shared.UI
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public delegate object BusyWaitDelegate();
    
    /// <summary>
    /// A generic dialog for some potentially long task that cannot be measured
    /// </summary>
    public partial class BusyWaitDialog : Form
    {
        private BusyWaitDelegate _action;
        private CultureInfo _culture;
        
        internal BusyWaitDialog(BusyWaitDelegate action, CultureInfo culture)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            _action = action;
            _culture = culture;
        }
        
        /// <summary>
        /// Raises the System.Windows.Forms.Form.Load event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            bgWorker.RunWorkerAsync();
        }
        
        /// <summary>
        /// Gets the return value of the completed background worker (if any)
        /// </summary>
        public object ReturnValue { get; private set; }

        /// <summary>
        /// Gets the error thrown by the completed background worker (if any)
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Opens a modal dialog to execute the given delegate in a background worker
        /// </summary>
        /// <param name="message"></param>
        /// <param name="action"></param>
        /// <param name="onComplete"></param>
        public static void Run(string message, BusyWaitDelegate action, Action<object, Exception> onComplete)
        {
            Run(message, action, onComplete, true);
        }

        /// <summary>
        /// Opens a modal dialog to execute the given delegate in a background worker
        /// </summary>
        /// <param name="message"></param>
        /// <param name="action"></param>
        /// <param name="onComplete"></param>
        /// <param name="bPreserveThreadCulture"></param>
        public static void Run(string message, BusyWaitDelegate action, Action<object, Exception> onComplete, bool bPreserveThreadCulture)
        {
            if (action == null)
                throw new ArgumentNullException("action"); //NOXLATE
            if (onComplete == null)
                throw new ArgumentNullException("onComplete"); //NOXLATE
            
            var frm = new BusyWaitDialog(action, bPreserveThreadCulture ? Thread.CurrentThread.CurrentCulture : null);
            frm.lblBusy.Text = message;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                onComplete.Invoke(frm.ReturnValue, frm.Error);
            }
        }
        
        void BgWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (_culture != null)
            {
                Thread.CurrentThread.CurrentCulture =
                    Thread.CurrentThread.CurrentUICulture =
                        _culture;
            }
            e.Result = _action.Invoke();
        }
        
        void BgWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.Error = e.Error;
            }
            else
            {
                this.ReturnValue = e.Result;
            }
            
            this.DialogResult = DialogResult.OK;
        }
    }
}
