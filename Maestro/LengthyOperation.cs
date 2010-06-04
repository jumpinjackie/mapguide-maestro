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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Form that displays progress for a lengthy operation.
    /// This implementation is really messy, and should be rewritten,
    /// using a BackgroundWorker.
	/// </summary>
	public class LengthyOperation : System.Windows.Forms.Form
	{
		public enum OperationType : int
		{
			MoveResource,
			MoveFolder,
			CopyFolder,
			Other,
		}

		private enum ProgressState
		{
			InitialOperation,
			ReferenceOperation,
			UpdateOperation
		}

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button OKBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel ProgressPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ImageList progressImages;
		private System.Windows.Forms.Timer animationTimer;
		private System.ComponentModel.IContainer components;

		private System.Threading.Thread m_runner = null;
		private OSGeo.MapGuide.MaestroAPI.ServerConnectionI m_connection;
		private System.Reflection.MethodInfo m_callbackEnabledMethod;
		private object[] m_arguments;

		private ListViewItem m_currentItem = null;
		private System.Windows.Forms.ListView workList;
		private System.Windows.Forms.Label OperationDescription;

		private System.Threading.ManualResetEvent m_event;
		private System.Windows.Forms.ColumnHeader columnHeader1;
        
		private System.Windows.Forms.ProgressBar ProgressBar;
		private bool m_cancel = false;
		private System.Windows.Forms.Timer MarqueeTimer;
		private OperationType m_operation;

		private ProgressState m_state = ProgressState.InitialOperation;

		private bool m_cancelRequested = false;
		private bool m_waitingForUser = true;
		private object m_callbackobject = null;
		private bool m_noCancelConfirmation = false;
        private bool m_waitForAccept = true;

		public LengthyOperation()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            this.Icon = FormMain.MaestroIcon;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LengthyOperation));
            this.workList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.progressImages = new System.Windows.Forms.ImageList(this.components);
            this.OperationDescription = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ProgressPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.animationTimer = new System.Windows.Forms.Timer(this.components);
            this.MarqueeTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.ProgressPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // workList
            // 
            this.workList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            resources.ApplyResources(this.workList, "workList");
            this.workList.FullRowSelect = true;
            this.workList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.workList.Name = "workList";
            this.workList.SmallImageList = this.progressImages;
            this.workList.UseCompatibleStateImageBehavior = false;
            this.workList.View = System.Windows.Forms.View.Details;
            this.workList.Resize += new System.EventHandler(this.workList_Resize);
            // 
            // progressImages
            // 
            this.progressImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("progressImages.ImageStream")));
            this.progressImages.TransparentColor = System.Drawing.Color.Transparent;
            this.progressImages.Images.SetKeyName(0, "");
            this.progressImages.Images.SetKeyName(1, "");
            this.progressImages.Images.SetKeyName(2, "");
            this.progressImages.Images.SetKeyName(3, "");
            this.progressImages.Images.SetKeyName(4, "");
            this.progressImages.Images.SetKeyName(5, "");
            this.progressImages.Images.SetKeyName(6, "");
            this.progressImages.Images.SetKeyName(7, "");
            this.progressImages.Images.SetKeyName(8, "");
            this.progressImages.Images.SetKeyName(9, "");
            this.progressImages.Images.SetKeyName(10, "");
            // 
            // OperationDescription
            // 
            resources.ApplyResources(this.OperationDescription, "OperationDescription");
            this.OperationDescription.Name = "OperationDescription";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // OKBtn
            // 
            resources.ApplyResources(this.OKBtn, "OKBtn");
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.OperationDescription);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // ProgressPanel
            // 
            this.ProgressPanel.Controls.Add(this.label2);
            this.ProgressPanel.Controls.Add(this.ProgressBar);
            resources.ApplyResources(this.ProgressPanel, "ProgressPanel");
            this.ProgressPanel.Name = "ProgressPanel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ProgressBar
            // 
            resources.ApplyResources(this.ProgressBar, "ProgressBar");
            this.ProgressBar.Name = "ProgressBar";
            // 
            // animationTimer
            // 
            this.animationTimer.Interval = 500;
            this.animationTimer.Tick += new System.EventHandler(this.animationTimer_Tick);
            // 
            // MarqueeTimer
            // 
            this.MarqueeTimer.Interval = 500;
            this.MarqueeTimer.Tick += new System.EventHandler(this.MarqueeTimer_Tick);
            // 
            // LengthyOperation
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.workList);
            this.Controls.Add(this.ProgressPanel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "LengthyOperation";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Load += new System.EventHandler(this.LengthyOperation_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ProgressPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void animationTimer_Tick(object sender, System.EventArgs e)
		{
			if (m_currentItem == null)
				return;

			if (m_currentItem.ImageIndex < 8 && m_currentItem.ImageIndex >= 0)
				m_currentItem.ImageIndex = (m_currentItem.ImageIndex + 1) % 8;
		}

		public System.Reflection.MethodInfo CallbackEnabledMethod
		{
			get { return m_callbackEnabledMethod; }
			set { m_callbackEnabledMethod = value; }
		}

		public object CallbackObject
		{
			get { return m_callbackobject; }
			set { m_callbackobject = value; }
		}

		public void InitializeDialog(OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection, string argument1, string argument2, OperationType operation, bool waitForAccept)
		{
			m_connection = connection;
			OSGeo.MapGuide.MaestroAPI.LengthyOperationCallBack callback = new OSGeo.MapGuide.MaestroAPI.LengthyOperationCallBack(CallbackMethod);
			OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressCallBack pgcallback = new OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressCallBack(WaitOperationCallBack);
			m_event = new System.Threading.ManualResetEvent(false);
			m_operation = operation;
            m_waitForAccept = waitForAccept;

			switch(operation)
			{
				case OperationType.CopyFolder:
					m_callbackEnabledMethod = typeof(OSGeo.MapGuide.MaestroAPI.ServerConnectionI).GetMethod("CopyFolderWithReferences");
					m_arguments = new object[] { argument1, argument2, callback, pgcallback };
					OperationDescription.Text = Strings.LengthyOperation.CopyFolderCopyConfirmation;
					m_callbackobject = connection;
					break;
				case OperationType.MoveFolder:
					m_callbackEnabledMethod = typeof(OSGeo.MapGuide.MaestroAPI.ServerConnectionI).GetMethod("MoveFolderWithReferences");
					m_arguments = new object[] { argument1, argument2, callback, pgcallback };
					OperationDescription.Text = Strings.LengthyOperation.MoveFolderConfirmation;
					m_callbackobject = connection;
					break;
				case OperationType.MoveResource:
					m_callbackEnabledMethod = typeof(OSGeo.MapGuide.MaestroAPI.ServerConnectionI).GetMethod("MoveResourceWithReferences");
					m_arguments = new object[] { argument1, argument2, callback, pgcallback };
					OperationDescription.Text = Strings.LengthyOperation.MoveResourceConfirmation;
					m_callbackobject = connection;
					break;
				case OperationType.Other:
					this.Text = Strings.LengthyOperation.OtherOperationConfirmation;
					m_arguments = new object[] { argument1, argument2, callback, pgcallback };
					m_noCancelConfirmation = true;
					break;
				default:
					throw new Exception(string.Format(Strings.LengthyOperation.UnknownOperationInternalError, operation.ToString()));
			}

			if (m_callbackEnabledMethod == null)
				throw new MissingMethodException(Strings.LengthyOperation.MissingMethodInternalError);

		}

		private void ThreadRunner()
		{
			bool retry = true;
			while(retry)
			{
				retry = false;
				try
				{
					try
					{
						m_callbackEnabledMethod.Invoke(m_callbackobject, m_arguments);
					}
					catch (System.Reflection.TargetInvocationException tex)
					{
						Exception ex = tex.InnerException;
                        string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
						switch (MessageBox.Show(string.Format(Strings.LengthyOperation.ErrorOccuredRetryQuestion, msg), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1))
						{
							case DialogResult.No:
								throw;
							case DialogResult.Yes:
								retry = true;
								break;
						}
					}
				}
				catch (Exception ex)
				{
					if (ex.GetType() == typeof(System.Reflection.TargetInvocationException))
						ex = ex.InnerException;

                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
					MessageBox.Show(string.Format(Strings.LengthyOperation.OperationFailedError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			this.Invoke(new System.Threading.ThreadStart(CloseDialog));
		}

		private void WaitOperationCallBack(object sender, OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressArgs args)
		{
			if (this.InvokeRequired)
				this.Invoke(new OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressCallBack(WaitOperationCallBack), new object[] { sender, args } );
			else
			{
				OperationDescription.Text = args.StatusMessage;
				if (args.Progress == -1)
				{
					MarqueeTimer.Enabled = true;
				}
				else
				{
					AskCancel();
					args.Cancel |= m_cancel;

					ProgressBar.Value = args.Progress;
					MarqueeTimer.Enabled = false;

					if (args.Progress == 100)
					{
						m_state = (ProgressState)((int)m_state) + 1;
						ProgressBar.Value = 0;

						CancelBtn.Enabled = true;
					}

				}
			}
		}

		private void AskCancel()
		{
			if (m_cancelRequested && !m_cancel) 
				if (m_noCancelConfirmation || MessageBox.Show(this, Strings.LengthyOperation.CancelNotRecomendedConfirmation, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
					m_cancel = true;
			m_cancelRequested = false;
		}


		private void CloseDialog()
		{
			if (m_cancel)
				this.DialogResult = DialogResult.Cancel;
			else
				this.DialogResult = DialogResult.OK;

			this.Close();
		}

		private void CallbackMethod(object sender, OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs args)
		{
			this.Invoke(new UpdateDisplayDelegate(UpdateDisplay), new object[] { args });
			m_event.WaitOne();
			m_waitingForUser = false;

			AskCancel();
			args.Cancel |= m_cancel;
		}

		private delegate void UpdateDisplayDelegate(OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs args);
		private void UpdateDisplay(OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs args)
		{
			if (!m_event.WaitOne(1, true))
			{
				m_waitingForUser = true;

				workList.Items.Clear();
				foreach(OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs.LengthyOperationItem item in args.Items)
					workList.Items.Add(new ListViewItem(item.Itempath, 10));

				switch(m_operation)
				{
					case OperationType.CopyFolder:
						OperationDescription.Text = Strings.LengthyOperation.CopyFolderUpdateConfirmation;
						break;
					case OperationType.MoveFolder:
						OperationDescription.Text = Strings.LengthyOperation.MoveFolderConfirmation;
						break;
					case OperationType.MoveResource:
						OperationDescription.Text = Strings.LengthyOperation.MoveResourceConfirmation;
						break;
				}

				ProgressBar.Value = 0;
				this.Height = 432;
				ProgressPanel.Visible = false;
				OKBtn.Enabled = true;

                if (workList.Items.Count == 0)
                    m_event.Set();
                else
                {
                    if (m_waitForAccept)
                        this.Focus(); //Flash it
                    else
                        OKBtn_Click(OKBtn, null);
                }
			}
			else
			{
				animationTimer.Enabled = false;
				m_currentItem = workList.Items[args.Index];
				ProgressBar.Value = Math.Min(100, (int)((args.Index / (double)args.Items.Length) * 100));
				switch(args.Items[args.Index].Status)
				{
					case OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs.LengthyOperationItem.OperationStatus.Pending:
						m_currentItem.ImageIndex = 0;
						break;
					case OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs.LengthyOperationItem.OperationStatus.Success:
						m_currentItem.ImageIndex = 8;
						break;
					case OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs.LengthyOperationItem.OperationStatus.Failure:
						m_currentItem.ImageIndex = 9;
						break;
					default: /*OSGeo.MapGuide.MaestroAPI.LengthyOperationCallbackArgs.LengthyOperationItem.OperationStatus.None:*/
						m_currentItem.ImageIndex = 10;
						break;
				}

				if (m_currentItem.ImageIndex == 0)
					animationTimer.Enabled = true;

			}
		}

		private void LengthyOperation_Load(object sender, System.EventArgs e)
		{
			if (m_callbackEnabledMethod == null)
				throw new Exception(Strings.LengthyOperation.NotInitializedInternalError);

			m_runner = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadRunner));
			m_runner.IsBackground = true;
			m_runner.Start();
		}

		private void OKBtn_Click(object sender, System.EventArgs e)
		{
			OKBtn.Enabled = false;
			ProgressPanel.Visible = true;
			m_event.Set();
		}

		private void CancelBtn_Click(object sender, System.EventArgs e)
		{
			m_cancelRequested = true;
			if (m_waitingForUser)
			{
				AskCancel();
				if (m_cancel)
					m_event.Set();
			}
		}

		private void MarqueeTimer_Tick(object sender, System.EventArgs e)
		{
			ProgressBar.Value = ProgressBar.Value % (ProgressBar.Maximum + 1);
		}

		private void workList_Resize(object sender, System.EventArgs e)
		{
			workList.Columns[0].Width = workList.Width - 40;
	}
}
}
