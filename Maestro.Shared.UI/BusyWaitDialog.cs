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
using System.Windows.Forms;

namespace Maestro.Shared.UI
{
    public delegate object BusyWaitDelegate();
    
    /// <summary>
    /// A generic dialog for some potentially long task that cannot be measured
    /// </summary>
    public partial class BusyWaitDialog : Form
    {
        private BusyWaitDelegate _action;
        
        internal BusyWaitDialog(BusyWaitDelegate action)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            _action = action;
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            bgWorker.RunWorkerAsync();
        }
        
        public object ReturnValue { get; private set; }
        
        public static void Run(string message, BusyWaitDelegate action, Action<object> onComplete)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (onComplete == null)
                throw new ArgumentNullException("onComplete");
            
            var frm = new BusyWaitDialog(action);
            frm.lblBusy.Text = message;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                onComplete.Invoke(frm.ReturnValue);
            }
        }
        
        void BgWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            e.Result = _action.Invoke();
        }
        
        void BgWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ErrorDialog.Show(e.Error);
            }
            else
            {
                this.ReturnValue = e.Result;
            }
            
            this.DialogResult = DialogResult.OK;
        }
    }
}
