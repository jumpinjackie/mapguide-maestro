#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
	public class XmlEditor : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Panel panel1;

		private bool m_allowClose = false;
		private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
		private System.ComponentModel.IContainer components;
        private Panel panel2;

		private Globalizator.Globalizator m_globalizor = null;
        private XmlEditorControl m_xmlEditorControl;

        public XmlEditor(string item, MaestroAPI.ServerConnectionI con)
            : this()
        {
            m_xmlEditorControl = new XmlEditorControl(item, con);
            m_xmlEditorControl.Dock = DockStyle.Fill;
            panel2.Controls.Add(m_xmlEditorControl);
        }

		public XmlEditor(object item, EditorInterface editor)
			: this()
		{
            m_xmlEditorControl = new XmlEditorControl(editor, item);
            m_xmlEditorControl.Dock = DockStyle.Fill;
            panel2.Controls.Add(m_xmlEditorControl);
        }

        public XmlEditor(string item, EditorInterface editor)
            : this()
        {
            m_xmlEditorControl = new XmlEditorControl(editor, item);
            m_xmlEditorControl.Dock = DockStyle.Fill;
            panel2.Controls.Add(m_xmlEditorControl);
        }

		private XmlEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			m_globalizor = new  Globalizator.Globalizator(this);
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Controls.Add(this.OKBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 447);
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
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(656, 447);
            this.panel2.TabIndex = 1;
            // 
            // XmlEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(656, 495);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "XmlEditor";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "XmlEditor";
            this.Load += new System.EventHandler(this.XmlEditor_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.XmlEditor_Closing);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void OKBtn_Click(object sender, System.EventArgs e)
		{
			if (m_xmlEditorControl.ValidateXml())
			{
                m_allowClose = true;
				m_xmlEditorControl.EndExternalEditing();
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void CancelBtn_Click(object sender, System.EventArgs e)
		{
			if (m_xmlEditorControl.Modified && MessageBox.Show(this, m_globalizor.Translate("Do you want to close this dialog and discard all changes?"), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
				return;

			m_xmlEditorControl.EndExternalEditing();
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

        public string EditorText { get { return m_xmlEditorControl.textEditor.Text; } }

        public object SerializedObject { get { return m_xmlEditorControl.SerializedObject; } }
	}
}
