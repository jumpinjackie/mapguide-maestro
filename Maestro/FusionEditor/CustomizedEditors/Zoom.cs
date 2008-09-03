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
	public class Zoom : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.TextBox Tolerance;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox Factor;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox Direction;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components = null;

		public Zoom()
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

				Tolerance.Text = GetSettingValue("Tolerance"); 
				Factor.Text = GetSettingValue("Factor"); 
				Direction.Text = GetSettingValue("Direction"); 
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
			this.Tolerance = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.Factor = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.Direction = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Tolerance
			// 
			this.Tolerance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Tolerance.Location = new System.Drawing.Point(104, 8);
			this.Tolerance.Name = "Tolerance";
			this.Tolerance.Size = new System.Drawing.Size(504, 20);
			this.Tolerance.TabIndex = 19;
			this.Tolerance.Text = "";
			this.Tolerance.TextChanged += new System.EventHandler(this.Tolerance_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 18;
			this.label3.Text = "Tolerance";
			// 
			// Factor
			// 
			this.Factor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Factor.Location = new System.Drawing.Point(104, 32);
			this.Factor.Name = "Factor";
			this.Factor.Size = new System.Drawing.Size(504, 20);
			this.Factor.TabIndex = 17;
			this.Factor.Text = "";
			this.Factor.TextChanged += new System.EventHandler(this.Factor_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 16;
			this.label2.Text = "Factor";
			// 
			// Direction
			// 
			this.Direction.Items.AddRange(new object[] {
														   "in",
														   "out"});
			this.Direction.Location = new System.Drawing.Point(104, 56);
			this.Direction.Name = "Direction";
			this.Direction.Size = new System.Drawing.Size(504, 21);
			this.Direction.TabIndex = 15;
			this.Direction.TextChanged += new System.EventHandler(this.Direction_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 14;
			this.label1.Text = "Direction";
			// 
			// Zoom
			// 
			this.Controls.Add(this.Tolerance);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.Factor);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Direction);
			this.Controls.Add(this.label1);
			this.Name = "Zoom";
			this.Size = new System.Drawing.Size(616, 88);
			this.ResumeLayout(false);

		}
		#endregion

		private void Tolerance_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Tolerance", Tolerance.Text);
		}

		private void Factor_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Factor", Factor.Text);
		}

		private void Direction_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Direction", Direction.Text);
		}
	}
}

