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
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;

namespace OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors
{
	public class InvokeScript : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.ComboBox ScriptPicker;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox Target;
		private System.Windows.Forms.Label label2;
        private TextBox Script;
		private System.ComponentModel.IContainer components = null;

		public InvokeScript()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public override void SetItem(WidgetType w)
		{
			try
			{
				m_isUpdating = true;
				m_w = w;
				this.Enabled = m_w != null;

				Target.Text = GetSettingValue("Target");
				Script.Text = GetSettingValue("Script");
                ScriptPicker.SelectedIndex = 0;
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvokeScript));
            this.ScriptPicker = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Target = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Script = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ScriptPicker
            // 
            resources.ApplyResources(this.ScriptPicker, "ScriptPicker");
            this.ScriptPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScriptPicker.Items.AddRange(new object[] {
            resources.GetString("ScriptPicker.Items"),
            resources.GetString("ScriptPicker.Items1"),
            resources.GetString("ScriptPicker.Items2"),
            resources.GetString("ScriptPicker.Items3"),
            resources.GetString("ScriptPicker.Items4")});
            this.ScriptPicker.Name = "ScriptPicker";
            this.ScriptPicker.SelectedIndexChanged += new System.EventHandler(this.ScriptPicker_SelectedIndexChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // Target
            // 
            resources.ApplyResources(this.Target, "Target");
            this.Target.Name = "Target";
            this.Target.TextChanged += new System.EventHandler(this.Target_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Script
            // 
            resources.ApplyResources(this.Script, "Script");
            this.Script.Name = "Script";
            this.Script.TextChanged += new System.EventHandler(this.Script_TextChanged);
            // 
            // InvokeScript
            // 
            this.Controls.Add(this.Script);
            this.Controls.Add(this.ScriptPicker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Target);
            this.Controls.Add(this.label2);
            this.Name = "InvokeScript";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void Target_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Target", Target.Text);
		}

		private void Script_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Script", Script.Text);
		}

        private void ScriptPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_isUpdating || m_w == null)
                return;

            if (ScriptPicker.SelectedIndex > 0)
                Script.Text = ScriptPicker.Text;
        }
	}
}

