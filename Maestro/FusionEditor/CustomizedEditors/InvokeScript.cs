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
            this.ScriptPicker = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Target = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Script = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ScriptPicker
            // 
            this.ScriptPicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ScriptPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScriptPicker.Items.AddRange(new object[] {
            "< Select a built in script >",
            "showOverviewMapDialog()",
            "showTaskDialog()",
            "showLegendDialog()",
            "showSelectionDialog()"});
            this.ScriptPicker.Location = new System.Drawing.Point(136, 32);
            this.ScriptPicker.Name = "ScriptPicker";
            this.ScriptPicker.Size = new System.Drawing.Size(472, 21);
            this.ScriptPicker.TabIndex = 32;
            this.ScriptPicker.SelectedIndexChanged += new System.EventHandler(this.ScriptPicker_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 16);
            this.label3.TabIndex = 31;
            this.label3.Text = "Script";
            // 
            // Target
            // 
            this.Target.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Target.Location = new System.Drawing.Point(136, 8);
            this.Target.Name = "Target";
            this.Target.Size = new System.Drawing.Size(472, 20);
            this.Target.TabIndex = 30;
            this.Target.TextChanged += new System.EventHandler(this.Target_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 16);
            this.label2.TabIndex = 29;
            this.label2.Text = "Target";
            // 
            // Script
            // 
            this.Script.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Script.Location = new System.Drawing.Point(8, 56);
            this.Script.Multiline = true;
            this.Script.Name = "Script";
            this.Script.Size = new System.Drawing.Size(600, 136);
            this.Script.TabIndex = 33;
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
            this.Size = new System.Drawing.Size(616, 198);
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

