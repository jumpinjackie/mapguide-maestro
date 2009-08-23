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
	public class SelectWithin : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox Target;
		private System.Windows.Forms.TextBox AdditionalParameter;
		private System.Windows.Forms.CheckBox DisableIfSelectionEmpty;
		private System.ComponentModel.IContainer components = null;

		public SelectWithin()
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

				try { DisableIfSelectionEmpty.Checked = bool.Parse(GetSettingValue("DisableIfSelectionEmpty")); }
				catch {}
				AdditionalParameter.Text = GetSettingValue("AdditionalParameter"); 
				Target.Text = GetSettingValue("Target"); 
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
			this.Target = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.AdditionalParameter = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.DisableIfSelectionEmpty = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// Target
			// 
			this.Target.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Target.Location = new System.Drawing.Point(144, 56);
			this.Target.Name = "Target";
			this.Target.Size = new System.Drawing.Size(464, 20);
			this.Target.TabIndex = 46;
			this.Target.Text = "";
			this.Target.TextChanged += new System.EventHandler(this.Target_TextChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 56);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(136, 16);
			this.label9.TabIndex = 45;
			this.label9.Text = "Target";
			// 
			// AdditionalParameter
			// 
			this.AdditionalParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.AdditionalParameter.Location = new System.Drawing.Point(144, 32);
			this.AdditionalParameter.Name = "AdditionalParameter";
			this.AdditionalParameter.Size = new System.Drawing.Size(464, 20);
			this.AdditionalParameter.TabIndex = 44;
			this.AdditionalParameter.Text = "";
			this.AdditionalParameter.TextChanged += new System.EventHandler(this.AdditionalParameter_TextChanged);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 32);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(136, 16);
			this.label10.TabIndex = 43;
			this.label10.Text = "Additional parameter";
			// 
			// DisableIfSelectionEmpty
			// 
			this.DisableIfSelectionEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.DisableIfSelectionEmpty.Location = new System.Drawing.Point(8, 8);
			this.DisableIfSelectionEmpty.Name = "DisableIfSelectionEmpty";
			this.DisableIfSelectionEmpty.Size = new System.Drawing.Size(600, 16);
			this.DisableIfSelectionEmpty.TabIndex = 47;
			this.DisableIfSelectionEmpty.Text = "Disable if the selection is empty";
			this.DisableIfSelectionEmpty.CheckedChanged += new System.EventHandler(this.DisableIfSelectionEmpty_CheckedChanged);
			// 
			// SelectWithin
			// 
			this.Controls.Add(this.DisableIfSelectionEmpty);
			this.Controls.Add(this.Target);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.AdditionalParameter);
			this.Controls.Add(this.label10);
			this.Name = "SelectWithin";
			this.Size = new System.Drawing.Size(616, 80);
			this.ResumeLayout(false);

		}
		#endregion

		private void DisableIfSelectionEmpty_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("DisableIfSelectionEmpty", DisableIfSelectionEmpty.Checked.ToString().ToLower());
		}

		private void AdditionalParameter_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("AdditionalParameter", AdditionalParameter.Text);
		}

		private void Target_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Target", Target.Text);
		}
	}
}

