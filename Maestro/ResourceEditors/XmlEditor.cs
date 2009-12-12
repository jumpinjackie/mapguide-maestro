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
	public class XmlEditor : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Panel panel1;

		private bool m_allowClose = false;
		private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
		private System.ComponentModel.IContainer components;
        private Panel panel2;

        private XmlEditorControl m_xmlEditorControl;

        public XmlEditor(string item, MaestroAPI.ServerConnectionI con)
            : this()
        {
            m_xmlEditorControl = new XmlEditorControl(item, con);
            m_xmlEditorControl.Dock = DockStyle.Fill;
            panel2.Controls.Add(m_xmlEditorControl);
        }

		public XmlEditor(object item, string resourceId, EditorInterface editor)
			: this()
		{
            m_xmlEditorControl = new XmlEditorControl(editor, item, resourceId);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XmlEditor));
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
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // XmlEditor
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "XmlEditor";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
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
                m_xmlEditorControl.SaveResourceData();
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void CancelBtn_Click(object sender, System.EventArgs e)
		{
			if (m_xmlEditorControl.Modified && MessageBox.Show(this, Strings.XmlEditor.CloseWithoutSavingWarning, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
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
