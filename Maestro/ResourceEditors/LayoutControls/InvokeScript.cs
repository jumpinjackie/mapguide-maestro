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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls
{
	/// <summary>
	/// Summary description for InvokeScript.
	/// </summary>
	public class InvokeScript : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox Script;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.InvokeScriptCommandType m_command;
		private OSGeo.MapGuide.MaestroAPI.WebLayout m_layout;
		private EditorInterface m_editor;
		private bool m_isUpdating = false;
		private LayoutEditor m_layoutEditor = null;

		public InvokeScript()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public void SetItem(OSGeo.MapGuide.MaestroAPI.InvokeScriptCommandType command, OSGeo.MapGuide.MaestroAPI.WebLayout layout, EditorInterface editor, LayoutEditor layoutEditor)
		{
			m_command = command;
			m_layout = layout;
			m_layoutEditor = layoutEditor;
			m_editor = editor;
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				if (m_command == null)
					return;

				Script.Text = m_command.Script;
			}
			finally
			{
				m_isUpdating = false;
			}
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvokeScript));
            this.label1 = new System.Windows.Forms.Label();
            this.Script = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Script
            // 
            resources.ApplyResources(this.Script, "Script");
            this.Script.Name = "Script";
            this.Script.TextChanged += new System.EventHandler(this.Script_TextChanged);
            // 
            // InvokeScript
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.Script);
            this.Controls.Add(this.label1);
            this.Name = "InvokeScript";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void Script_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_command == null)
				return;

			m_command.Script = Script.Text;

			m_editor.HasChanged();
		
		}
	}
}
