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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for XmlEditor.
	/// </summary>
	public class XmlEditorControl : System.Windows.Forms.UserControl, IResourceEditorControl 
	{
        private System.Windows.Forms.Panel panel2;
        public TextBox textEditor;
		private System.ComponentModel.IContainer components;

        private EditorInterface m_editor;
        private System.Windows.Forms.ImageList toolbarImages;
        private System.Type m_serializeType;
		private object m_serializedObject = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel LockedMessagePanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;

		private System.IO.FileSystemWatcher m_fsw = null;
		private string m_tempfile = null;
		private System.Diagnostics.Process m_externalProcess = null;
		private System.Threading.Thread m_externalProcessWatcher = null;
		private bool m_inUpdate = false;
        private ToolStrip toolBar;
        private ToolStripButton CopyClipboardButton;
        private ToolStripButton CutClipboardButton;
        private ToolStripButton PasteClipboardButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton UndoClipboardButton;
        private ToolStripButton RedoClipboardButton;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton ValidateButton;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton LaunchExternalEditorButton;
		private string m_resourceId = null;
        private GroupBox ResourceDataGroup;
        private ResourceDataEditor resourceDataEditor;
        private ToolStripLabel toolStripLabel1;
        private bool m_modified = false;
        private string m_positionTemplate;

		public XmlEditorControl(EditorInterface editor, string item)
			: this(editor, editor.CurrentConnection.TryGetResourceType(item) == null ? editor.CurrentConnection.GetResourceXmlData(item) : editor.CurrentConnection.GetResource(item), item)
		{
            m_resourceId = item;
		}

        public XmlEditorControl(EditorInterface editor, object item, string resourceId)
            : this(editor)
        {
            m_inUpdate = true;
            m_editor = editor;

            m_resourceId = resourceId;
            m_serializeType = null;
            m_serializedObject = null;

            editor.Closing += new EventHandler(editor_Closing);

            if (item is byte[])
            {
                try
                {
                    //Attempt to format this with indents etc.
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.LoadXml(System.Text.Encoding.UTF8.GetString(item as byte[]));
                    using (System.IO.StringWriter sw = new System.IO.StringWriter())
                    {
                        doc.WriteTo(new MaestroAPI.Utf8XmlWriter(sw));
                        textEditor.Text = sw.GetStringBuilder().ToString();
                    }
                }
                catch
                {
                    //Fallback, just display the text
                    textEditor.Text = System.Text.Encoding.UTF8.GetString(item as byte[]);
                }
            }
            else
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(m_editor.CurrentConnection.SerializeObject(item), System.Text.Encoding.UTF8, true))
                    textEditor.Text = sr.ReadToEnd();

                if (item.GetType().GetProperty("ResourceId") != null)
                    m_resourceId = (string)item.GetType().GetProperty("ResourceId").GetValue(item, null);

                if (m_resourceId != null)
                    m_serializeType = m_editor.CurrentConnection.TryGetResourceType(m_resourceId);

                if (m_serializeType == null)
                    m_serializeType = item.GetType();

                if (m_serializeType != null)
                    m_serializedObject = m_editor.CurrentConnection.DeserializeObject(m_serializeType, new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(textEditor.Text)));
            }
            m_inUpdate = false;
            UpdateDisplay();
        }

		public XmlEditorControl(EditorInterface editor)
			: this()
		{
			m_inUpdate = true;
			m_editor = editor;
			textEditor.Text = "";
			editor.Closing += new EventHandler(editor_Closing);
			m_serializeType = null;
			m_serializedObject = null;
			m_inUpdate = false;
			m_resourceId = null;
			UpdateDisplay();
		}

        public XmlEditorControl(string item, MaestroAPI.ServerConnectionI editor)
            : this()
        {
            m_inUpdate = true;
            m_editor = null;
            textEditor.Text = item;
            m_serializeType = null;
            m_serializedObject = null;
            m_inUpdate = false;
            m_resourceId = null;
            UpdateDisplay();
        }

		public void SetItem(string text, Type type)
		{
			try
			{
				m_inUpdate = true;
				m_serializeType = type;
				textEditor.Text = text;
			}
			finally
			{
				m_inUpdate = false;
			}
		}

		private XmlEditorControl()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            m_positionTemplate = toolStripLabel1.Text;
            toolStripLabel1.Text = String.Format(m_positionTemplate, 0, 0);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XmlEditorControl));
            this.panel2 = new System.Windows.Forms.Panel();
            this.textEditor = new System.Windows.Forms.TextBox();
            this.ResourceDataGroup = new System.Windows.Forms.GroupBox();
            this.resourceDataEditor = new OSGeo.MapGuide.Maestro.ResourceEditors.ResourceDataEditor();
            this.LockedMessagePanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.CopyClipboardButton = new System.Windows.Forms.ToolStripButton();
            this.CutClipboardButton = new System.Windows.Forms.ToolStripButton();
            this.PasteClipboardButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.UndoClipboardButton = new System.Windows.Forms.ToolStripButton();
            this.RedoClipboardButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ValidateButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.LaunchExternalEditorButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolbarImages = new System.Windows.Forms.ImageList(this.components);
            this.panel2.SuspendLayout();
            this.ResourceDataGroup.SuspendLayout();
            this.LockedMessagePanel.SuspendLayout();
            this.toolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textEditor);
            this.panel2.Controls.Add(this.ResourceDataGroup);
            this.panel2.Controls.Add(this.LockedMessagePanel);
            this.panel2.Controls.Add(this.toolBar);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // textEditor
            // 
            this.textEditor.AcceptsReturn = true;
            this.textEditor.AcceptsTab = true;
            resources.ApplyResources(this.textEditor, "textEditor");
            this.textEditor.Name = "textEditor";
            this.textEditor.TextChanged += new System.EventHandler(this.textEditor_TextChanged);
            this.textEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.textEditor_MouseMove);
            this.textEditor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textEditor_KeyUp);
            this.textEditor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textEditor_MouseUp);
            // 
            // ResourceDataGroup
            // 
            this.ResourceDataGroup.Controls.Add(this.resourceDataEditor);
            resources.ApplyResources(this.ResourceDataGroup, "ResourceDataGroup");
            this.ResourceDataGroup.Name = "ResourceDataGroup";
            this.ResourceDataGroup.TabStop = false;
            // 
            // resourceDataEditor
            // 
            resources.ApplyResources(this.resourceDataEditor, "resourceDataEditor");
            this.resourceDataEditor.Editor = null;
            this.resourceDataEditor.Name = "resourceDataEditor";
            this.resourceDataEditor.ResourceExists = false;
            this.resourceDataEditor.ResourceID = null;
            this.resourceDataEditor.ResourceDataChanged += new System.EventHandler(this.resourceDataEditor_ResourceDataChanged);
            // 
            // LockedMessagePanel
            // 
            this.LockedMessagePanel.Controls.Add(this.label3);
            this.LockedMessagePanel.Controls.Add(this.label2);
            this.LockedMessagePanel.Controls.Add(this.label1);
            resources.ApplyResources(this.LockedMessagePanel, "LockedMessagePanel");
            this.LockedMessagePanel.Name = "LockedMessagePanel";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // toolBar
            // 
            this.toolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyClipboardButton,
            this.CutClipboardButton,
            this.PasteClipboardButton,
            this.toolStripSeparator1,
            this.UndoClipboardButton,
            this.RedoClipboardButton,
            this.toolStripSeparator2,
            this.ValidateButton,
            this.toolStripSeparator3,
            this.LaunchExternalEditorButton,
            this.toolStripLabel1});
            resources.ApplyResources(this.toolBar, "toolBar");
            this.toolBar.Name = "toolBar";
            this.toolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // CopyClipboardButton
            // 
            this.CopyClipboardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.CopyClipboardButton, "CopyClipboardButton");
            this.CopyClipboardButton.Name = "CopyClipboardButton";
            this.CopyClipboardButton.Click += new System.EventHandler(this.CopyClipboardButton_Click);
            // 
            // CutClipboardButton
            // 
            this.CutClipboardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.CutClipboardButton, "CutClipboardButton");
            this.CutClipboardButton.Name = "CutClipboardButton";
            this.CutClipboardButton.Click += new System.EventHandler(this.CutClipboardButton_Click);
            // 
            // PasteClipboardButton
            // 
            this.PasteClipboardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.PasteClipboardButton, "PasteClipboardButton");
            this.PasteClipboardButton.Name = "PasteClipboardButton";
            this.PasteClipboardButton.Click += new System.EventHandler(this.PasteClipboardButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // UndoClipboardButton
            // 
            this.UndoClipboardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.UndoClipboardButton, "UndoClipboardButton");
            this.UndoClipboardButton.Name = "UndoClipboardButton";
            this.UndoClipboardButton.Click += new System.EventHandler(this.UndoClipboardButton_Click);
            // 
            // RedoClipboardButton
            // 
            this.RedoClipboardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.RedoClipboardButton, "RedoClipboardButton");
            this.RedoClipboardButton.Name = "RedoClipboardButton";
            this.RedoClipboardButton.Click += new System.EventHandler(this.RedoClipboardButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // ValidateButton
            // 
            this.ValidateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ValidateButton, "ValidateButton");
            this.ValidateButton.Name = "ValidateButton";
            this.ValidateButton.Click += new System.EventHandler(this.ValidateButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // LaunchExternalEditorButton
            // 
            this.LaunchExternalEditorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.LaunchExternalEditorButton, "LaunchExternalEditorButton");
            this.LaunchExternalEditorButton.Name = "LaunchExternalEditorButton";
            this.LaunchExternalEditorButton.Click += new System.EventHandler(this.LaunchExternalEditorButton_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // toolbarImages
            // 
            this.toolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImages.ImageStream")));
            this.toolbarImages.TransparentColor = System.Drawing.Color.Transparent;
            this.toolbarImages.Images.SetKeyName(0, "");
            this.toolbarImages.Images.SetKeyName(1, "");
            this.toolbarImages.Images.SetKeyName(2, "");
            this.toolbarImages.Images.SetKeyName(3, "");
            this.toolbarImages.Images.SetKeyName(4, "");
            this.toolbarImages.Images.SetKeyName(5, "");
            this.toolbarImages.Images.SetKeyName(6, "");
            // 
            // XmlEditorControl
            // 
            this.Controls.Add(this.panel2);
            this.Name = "XmlEditorControl";
            resources.ApplyResources(this, "$this");
            this.Load += new System.EventHandler(this.XmlEditor_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResourceDataGroup.ResumeLayout(false);
            this.LockedMessagePanel.ResumeLayout(false);
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void XmlEditor_Load(object sender, System.EventArgs e)
		{
			if (m_serializeType == null)
				ValidateButton.Enabled = false;
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

        public void EndExternalEditing()
        {
            EndExternalEditing(false);
        }

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

		public bool ValidateXml()
		{
			try
			{
				if (m_serializeType != null)
				{
					System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(textEditor.Text));
					object o = m_editor.CurrentConnection.DeserializeObject(m_serializeType, ms);
					if (o.GetType() != m_serializeType)
						throw new Exception(string.Format(Strings.XmlEditorControl.UnexpectedTypeError, o.GetType().FullName, m_serializeType.FullName));
					m_serializedObject = o;
				}
				return true;
			}
			catch (Exception ex)
			{
                if (m_editor != null)
                    m_editor.SetLastException(ex);
                MessageBox.Show(this, string.Format(Strings.XmlEditorControl.SerializeError, ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); 
				return false;
			}

		}

		private void textEditor_TextChanged(object sender, System.EventArgs e)
		{
            if (!m_inUpdate)
            {
                m_modified = true;
                if (m_editor != null && !(this.ParentForm is XmlEditor))
                    m_editor.HasChanged();
            }
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

		private void editor_Closing(object sender, EventArgs e)
		{
			EndExternalEditing(false);
		}

		public object Resource
		{
			get { return m_serializedObject; }
			set 
			{
				m_serializedObject = value;
				UpdateDisplay();
			}
		}
		public string ResourceId
		{
			get { return m_resourceId; }
			set { m_resourceId = value; }
		}

		public bool Preview()
		{
			return false;
		}

		public bool Save(string savename)
		{
            resourceDataEditor.SaveChanges();

			if (m_serializeType != null)
			{
                m_serializedObject = m_editor.CurrentConnection.DeserializeObject(m_serializeType, new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(textEditor.Text)));
                m_modified = false;
				return false;
			}
			else
			{
                m_editor.CurrentConnection.SetResourceXmlData(savename, new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(textEditor.Text)));
                m_modified = false;
                return true;
			}
		}

		public void UpdateDisplay()
		{
			try
			{
				m_inUpdate = true;
				if (m_serializedObject != null && m_serializeType != null)
					textEditor.Text = System.Text.Encoding.UTF8.GetString(m_editor.CurrentConnection.SerializeObject(m_serializedObject).ToArray());

                ResourceDataGroup.Visible = m_resourceId != null;
                if (m_resourceId != null)
                {
                    resourceDataEditor.Editor = m_editor;
                    resourceDataEditor.ResourceExists = true;
                    resourceDataEditor.ResourceID = m_resourceId;
                }
			}
			finally
			{
				m_inUpdate = false;
			}
		}

        private void CopyClipboardButton_Click(object sender, EventArgs e)
        {
            textEditor.Copy();
        }

        private void CutClipboardButton_Click(object sender, EventArgs e)
        {
            textEditor.Cut();
        }

        private void PasteClipboardButton_Click(object sender, EventArgs e)
        {
            textEditor.Paste();
        }

        private void UndoClipboardButton_Click(object sender, EventArgs e)
        {
            textEditor.Undo();
        }

        private void RedoClipboardButton_Click(object sender, EventArgs e)
        {
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            ValidateXml();
        }

        private void LaunchExternalEditorButton_Click(object sender, EventArgs e)
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
                m_fsw.SynchronizingObject = this;

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
                if (m_editor != null)
                    m_editor.SetLastException(ex);
                EndExternalEditing(false);
                MessageBox.Show(this, string.Format(Strings.XmlEditorControl.ExternalEditorError, ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool Profile() { return true; }
        public bool ValidateResource(bool recurse) { return true; }
        public bool SupportsPreview { get { return false; } }
        public bool SupportsValidate { get { return false; } }
        public bool SupportsProfiling { get { return false; } }

        private void textEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                textEditor.SelectAll();

            UpdateTextPosition();
        }

        private void UpdateTextPosition()
        {
            int line = textEditor.GetLineFromCharIndex(textEditor.SelectionStart + textEditor.SelectionLength);
            int col = (textEditor.SelectionStart + textEditor.SelectionLength) - textEditor.GetFirstCharIndexFromLine(line);

            toolStripLabel1.Text = String.Format(m_positionTemplate, line + 1, col + 1);
        }

        public bool Modified { get { return m_modified; } }
        public object SerializedObject { get { return m_serializedObject; } }

        private void resourceDataEditor_ResourceDataChanged(object sender, EventArgs e)
        {
            m_modified = true;
            if (m_editor != null && !(this.ParentForm is XmlEditor))
                m_editor.HasChanged();
        }

        internal void SaveResourceData()
        {
            resourceDataEditor.SaveChanges();
        }

        private void textEditor_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateTextPosition();
        }

        private void textEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None)
                UpdateTextPosition();
        }
    }
}
