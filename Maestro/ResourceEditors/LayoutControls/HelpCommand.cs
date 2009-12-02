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
	/// Summary description for HelpCommand.
	/// </summary>
	public class HelpCommand : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox TargetFrame;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox Target;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox URL;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.HelpCommandType m_command;
		private OSGeo.MapGuide.MaestroAPI.WebLayout m_layout;
		private EditorInterface m_editor;
		private bool m_isUpdating = false;
		private LayoutEditor m_layoutEditor = null;

		public HelpCommand()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			Target.Items.Clear();
			foreach(object o in Enum.GetValues(typeof(OSGeo.MapGuide.MaestroAPI.TargetType)))
				Target.Items.Add(o.ToString());
		}

		public void SetItem(OSGeo.MapGuide.MaestroAPI.HelpCommandType command, OSGeo.MapGuide.MaestroAPI.WebLayout layout, EditorInterface editor, LayoutEditor layoutEditor)
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

				Target.SelectedIndex = Target.FindString(m_command.Target.ToString());
				TargetFrame.Text = m_command.TargetFrame;
				URL.Text = m_command.URL;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpCommand));
            this.TargetFrame = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Target = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.URL = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TargetFrame
            // 
            resources.ApplyResources(this.TargetFrame, "TargetFrame");
            this.TargetFrame.Name = "TargetFrame";
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Target
            // 
            resources.ApplyResources(this.Target, "Target");
            this.Target.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Target.Name = "Target";
            this.Target.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // URL
            // 
            resources.ApplyResources(this.URL, "URL");
            this.URL.Name = "URL";
            // 
            // HelpCommand
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.URL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TargetFrame);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Target);
            this.Controls.Add(this.label1);
            this.Name = "HelpCommand";
            this.Load += new System.EventHandler(this.HelpCommand_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void HelpCommand_Load(object sender, System.EventArgs e)
		{
		
		}

		private void SomeProperty_Changed(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_command == null)
				return;

			m_command.Target = (OSGeo.MapGuide.MaestroAPI.TargetType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.TargetType), Target.Text, true);
			m_command.TargetFrame = TargetFrame.Text;
			m_command.URL = URL.Text;

			m_editor.HasChanged();
		}
	}
}
