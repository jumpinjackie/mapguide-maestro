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
	public class SaveMap : FusionEditor.BasisWidgetEditor
	{
		private new System.Windows.Forms.TextBox Scale;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox Format;
		private System.ComponentModel.IContainer components = null;

		public SaveMap()
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

				Scale.Text = GetSettingValue("Scale"); 
				Format.Text = GetSettingValue("Format"); 
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
			this.Scale = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.Format = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// Scale
			// 
			this.Scale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Scale.Location = new System.Drawing.Point(144, 32);
			this.Scale.Name = "Scale";
			this.Scale.Size = new System.Drawing.Size(464, 20);
			this.Scale.TabIndex = 15;
			this.Scale.Text = "";
			this.Scale.TextChanged += new System.EventHandler(this.Scale_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.TabIndex = 14;
			this.label2.Text = "Scale";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 16);
			this.label1.TabIndex = 12;
			this.label1.Text = "Format";
			// 
			// Format
			// 
			this.Format.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Format.Items.AddRange(new object[] {
														"png",
														"jpg",
														"gif"});
			this.Format.Location = new System.Drawing.Point(144, 8);
			this.Format.Name = "Format";
			this.Format.Size = new System.Drawing.Size(464, 21);
			this.Format.TabIndex = 30;
			this.Format.TextChanged += new System.EventHandler(this.Format_TextChanged);
			// 
			// SaveMap
			// 
			this.Controls.Add(this.Format);
			this.Controls.Add(this.Scale);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "SaveMap";
			this.Size = new System.Drawing.Size(616, 56);
			this.ResumeLayout(false);

		}
		#endregion

		private void Scale_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Scale", Scale.Text);
		}

		private void Format_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Format", Format.Text);
		}
	}
}

