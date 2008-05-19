#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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

namespace OSGeo.MapGuide.Maestro
{
	/// <summary>
	/// Summary description for XmlEditor.
	/// </summary>
	public class XmlEditor : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;

		private bool m_allowClose = false;
		private System.Windows.Forms.Button OKBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TextBox textEditor;
		private System.ComponentModel.IContainer components;

		private OSGeo.MapGuide.MaestroAPI.ServerConnectionI m_connection;
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.ImageList toolbarImages;
		private System.Windows.Forms.ToolBarButton CopyClipboardButton;
		private System.Windows.Forms.ToolBarButton CutClipboardButton;
		private System.Windows.Forms.ToolBarButton PasteClipboardButton;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton UndoClipboardButton;
		private System.Windows.Forms.ToolBarButton RedoClipboardButton;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.ToolBarButton ValidateButton;
		private System.Type m_serializeType;
		private bool m_modified = false;
		private System.Windows.Forms.ToolBarButton toolBarButton3;
		private System.Windows.Forms.ToolBarButton LaunchExternalEditorButton;
		private object m_serializedObject = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel LockedMessagePanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;

		private System.IO.FileSystemWatcher m_fsw = null;
		private string m_tempfile = null;
		private System.Diagnostics.Process m_externalProcess = null;
		private System.Threading.Thread m_externalProcessWatcher = null;
		private  Globalizator.Globalizator m_globalizor = null;


		public XmlEditor(object item, OSGeo.MapGuide.MaestroAPI.ServerConnectionI connection)
			: this()
		{
			textEditor.Text = System.Text.Encoding.UTF8.GetString(connection.SerializeObject(item).ToArray());
			m_connection = connection;
			m_serializeType = item.GetType();
			m_modified = false;
			m_serializedObject = item;
		}

		private XmlEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			m_globalizor = new  Globalizator.Globalizator(this);
		}

		//TODO: Add syntax higlighting for the editor

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(XmlEditor));
			this.panel1 = new System.Windows.Forms.Panel();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.OKBtn = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.textEditor = new System.Windows.Forms.TextBox();
			this.LockedMessagePanel = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.CopyClipboardButton = new System.Windows.Forms.ToolBarButton();
			this.CutClipboardButton = new System.Windows.Forms.ToolBarButton();
			this.PasteClipboardButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.UndoClipboardButton = new System.Windows.Forms.ToolBarButton();
			this.RedoClipboardButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.ValidateButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
			this.LaunchExternalEditorButton = new System.Windows.Forms.ToolBarButton();
			this.toolbarImages = new System.Windows.Forms.ImageList(this.components);
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.LockedMessagePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.CancelBtn);
			this.panel1.Controls.Add(this.OKBtn);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 477);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(656, 48);
			this.panel1.TabIndex = 0;
			// 
			// CancelBtn
			// 
			this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelBtn.Location = new System.Drawing.Point(332, 16);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(96, 24);
			this.CancelBtn.TabIndex = 1;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// OKBtn
			// 
			this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.OKBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OKBtn.Location = new System.Drawing.Point(212, 16);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(96, 24);
			this.OKBtn.TabIndex = 0;
			this.OKBtn.Text = "OK";
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.textEditor);
			this.panel2.Controls.Add(this.LockedMessagePanel);
			this.panel2.Controls.Add(this.toolBar);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(656, 477);
			this.panel2.TabIndex = 1;
			// 
			// textEditor
			// 
			this.textEditor.AcceptsReturn = true;
			this.textEditor.AcceptsTab = true;
			this.textEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textEditor.Location = new System.Drawing.Point(0, 96);
			this.textEditor.Multiline = true;
			this.textEditor.Name = "textEditor";
			this.textEditor.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textEditor.Size = new System.Drawing.Size(656, 381);
			this.textEditor.TabIndex = 0;
			this.textEditor.Text = "";
			this.textEditor.TextChanged += new System.EventHandler(this.textEditor_TextChanged);
			// 
			// LockedMessagePanel
			// 
			this.LockedMessagePanel.Controls.Add(this.label3);
			this.LockedMessagePanel.Controls.Add(this.label2);
			this.LockedMessagePanel.Controls.Add(this.label1);
			this.LockedMessagePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.LockedMessagePanel.Location = new System.Drawing.Point(0, 28);
			this.LockedMessagePanel.Name = "LockedMessagePanel";
			this.LockedMessagePanel.Size = new System.Drawing.Size(656, 68);
			this.LockedMessagePanel.TabIndex = 2;
			this.LockedMessagePanel.Visible = false;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(632, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "The text display will automatically update when the editor saves the file.";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(632, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Close the external program to restore editing functionality in this xml editor.";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(632, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "The application associated with xml editing has been started.";
			// 
			// toolBar
			// 
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					   this.CopyClipboardButton,
																					   this.CutClipboardButton,
																					   this.PasteClipboardButton,
																					   this.toolBarButton1,
																					   this.UndoClipboardButton,
																					   this.RedoClipboardButton,
																					   this.toolBarButton2,
																					   this.ValidateButton,
																					   this.toolBarButton3,
																					   this.LaunchExternalEditorButton});
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.toolbarImages;
			this.toolBar.Location = new System.Drawing.Point(0, 0);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(656, 28);
			this.toolBar.TabIndex = 1;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// CopyClipboardButton
			// 
			this.CopyClipboardButton.ImageIndex = 0;
			this.CopyClipboardButton.ToolTipText = "Copy selected text to the clipboard";
			// 
			// CutClipboardButton
			// 
			this.CutClipboardButton.ImageIndex = 1;
			this.CutClipboardButton.ToolTipText = "Cut the selected text and place it in the clipboard";
			// 
			// PasteClipboardButton
			// 
			this.PasteClipboardButton.ImageIndex = 2;
			this.PasteClipboardButton.ToolTipText = "Paste text from the clipboard";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// UndoClipboardButton
			// 
			this.UndoClipboardButton.ImageIndex = 3;
			this.UndoClipboardButton.ToolTipText = "Undo the last action";
			// 
			// RedoClipboardButton
			// 
			this.RedoClipboardButton.ImageIndex = 4;
			this.RedoClipboardButton.ToolTipText = "Redo the last action";
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// ValidateButton
			// 
			this.ValidateButton.ImageIndex = 5;
			this.ValidateButton.ToolTipText = "Validate the current xml";
			// 
			// toolBarButton3
			// 
			this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// LaunchExternalEditorButton
			// 
			this.LaunchExternalEditorButton.ImageIndex = 6;
			this.LaunchExternalEditorButton.ToolTipText = "Launch the system editor associated with xml files";
			// 
			// toolbarImages
			// 
			this.toolbarImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.toolbarImages.ImageSize = new System.Drawing.Size(16, 16);
			this.toolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImages.ImageStream")));
			this.toolbarImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// XmlEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(656, 525);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "XmlEditor";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "XmlEditor";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.XmlEditor_Closing);
			this.Load += new System.EventHandler(this.XmlEditor_Load);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.LockedMessagePanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void OKBtn_Click(object sender, System.EventArgs e)
		{
			if (ValidateXml())
			{
				EndExternalEditing(false);
				m_allowClose = true;
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void CancelBtn_Click(object sender, System.EventArgs e)
		{
			if (m_modified && MessageBox.Show(this, m_globalizor.Translate("Do you want to close this dialog and discard all changes?"), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
				return;

			EndExternalEditing(false);
			m_allowClose = true;
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void XmlEditor_Load(object sender, System.EventArgs e)
		{
		
		}

		private void XmlEditor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!m_allowClose)
			{
				CancelBtn.PerformClick();
				if (!m_allowClose)
					e.Cancel = true;
			}
		}

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == CopyClipboardButton)
				textEditor.Copy();
			else if (e.Button == CutClipboardButton)
				textEditor.Cut();
			else if (e.Button == PasteClipboardButton)
				textEditor.Paste();
			else if (e.Button == UndoClipboardButton)
				textEditor.Undo();
			else if (e.Button == RedoClipboardButton)
			{}
			else if (e.Button == ValidateButton)
				ValidateXml();
			else if (e.Button == LaunchExternalEditorButton)
			{

				try
				{
					m_tempfile = System.IO.Path.GetTempFileName();
					try { System.IO.File.Delete(m_tempfile); }
					catch { }
					m_tempfile += ".xml";

					using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m_tempfile, false, System.Text.Encoding.UTF8))
						sw.Write(textEditor.Text);

					m_fsw = new System.IO.FileSystemWatcher(System.IO.Path.GetDirectoryName(m_tempfile), System.IO.Path.GetFileName(m_tempfile));
					m_fsw.Changed += new System.IO.FileSystemEventHandler(m_fsw_Changed);
					m_fsw.EnableRaisingEvents = true;

					m_externalProcess = new System.Diagnostics.Process();
					m_externalProcess.StartInfo.UseShellExecute = true;
					m_externalProcess.StartInfo.Verb = "edit";
					m_externalProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(m_tempfile);
					m_externalProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
					m_externalProcess.StartInfo.FileName = m_tempfile;
					m_externalProcess.Start();

					m_externalProcessWatcher = new System.Threading.Thread(new System.Threading.ThreadStart(WaitForExternalProcess));
					m_externalProcessWatcher.IsBackground = true;
					m_externalProcessWatcher.Start();
				
					LockedMessagePanel.Visible = true;
					toolBar.Enabled = false;
					textEditor.ReadOnly = true;
				}
				catch (Exception ex)
				{
					EndExternalEditing(false);
					MessageBox.Show(this, string.Format(m_globalizor.Translate("Failed to start external editor, most likely this means that theres is no external editor associated with xml files.\nError message was: {0}"), ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

			}
		}

		private void WaitForExternalProcess()
		{
			try
			{
				m_externalProcess.WaitForExit();
			}
			catch(System.Threading.ThreadAbortException)
			{
				return;
			}
			catch
			{
			}

			this.Invoke(new EndExternalEditingDelegate(EndExternalEditing), new object[] { true });
		}

		private delegate void EndExternalEditingDelegate(bool fromThread);

		private void EndExternalEditing(bool fromThread)
		{
			if (m_externalProcessWatcher != null)
			{
				if (!fromThread && m_externalProcessWatcher.IsAlive)
				{
					m_externalProcessWatcher.Abort();
					m_externalProcessWatcher.Join(500);
				}
				m_externalProcessWatcher = null;
			}

			if (m_fsw != null)
			{
				m_fsw.EnableRaisingEvents = false;
				m_fsw.Dispose();
				m_fsw = null;
			}

			if (m_externalProcess != null)
			{
				m_externalProcess.Dispose();
				m_externalProcess = null;
			}

			if (m_tempfile != null)
			{
				try { System.IO.File.Delete(m_tempfile); }
				catch { }
				m_tempfile = null;
			}

			LockedMessagePanel.Visible = false;
			toolBar.Enabled = true;
			textEditor.ReadOnly = false;
		}

		private bool ValidateXml()
		{
			try
			{
				System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(textEditor.Text));
				object o = m_connection.DeserializeObject(m_serializeType, ms);
				if (o.GetType() != m_serializeType)
					throw new Exception(string.Format(m_globalizor.Translate("Item did not correspond to the desired type.\nXml gave type: {0}\nExpected type was: {1}"), o.GetType().FullName, m_serializeType.FullName));
				m_serializedObject = o;
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, string.Format(m_globalizor.Translate("The entered xml failed to serialize into an object.\nError message is: {0}"), ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); 
				return false;
			}

		}

		public object SerializedObject { get { return m_serializedObject; } }

		private void textEditor_TextChanged(object sender, System.EventArgs e)
		{
			m_modified = true;
		}

		private void m_fsw_Changed(object sender, System.IO.FileSystemEventArgs e)
		{
			try
			{
				using(System.IO.StreamReader sr = new System.IO.StreamReader(m_tempfile))
					textEditor.Text = sr.ReadToEnd();
			}
			catch
			{
			}
		}
	}
}
