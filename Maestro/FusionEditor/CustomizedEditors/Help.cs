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
	public class Help : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.TextBox Target;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox Url;
		private System.Windows.Forms.Label label2;
		private System.ComponentModel.IContainer components = null;

		public Help()
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
				Url.Text = GetSettingValue("Url");
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
			this.label1 = new System.Windows.Forms.Label();
			this.Url = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Target
			// 
			this.Target.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Target.Location = new System.Drawing.Point(104, 8);
			this.Target.Name = "Target";
			this.Target.Size = new System.Drawing.Size(504, 20);
			this.Target.TabIndex = 5;
			this.Target.Text = "";
			this.Target.TextChanged += new System.EventHandler(this.Target_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "Target";
			// 
			// Url
			// 
			this.Url.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Url.Location = new System.Drawing.Point(104, 32);
			this.Url.Name = "Url";
			this.Url.Size = new System.Drawing.Size(504, 20);
			this.Url.TabIndex = 7;
			this.Url.Text = "";
			this.Url.TextChanged += new System.EventHandler(this.Url_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 6;
			this.label2.Text = "Url";
			// 
			// Help
			// 
			this.Controls.Add(this.Url);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Target);
			this.Controls.Add(this.label1);
			this.Name = "Help";
			this.Size = new System.Drawing.Size(616, 64);
			this.ResumeLayout(false);

		}
		#endregion

		private void Target_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Target", Target.Text);
		}

		private void Url_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Url", Url.Text);
		}
	}
}

