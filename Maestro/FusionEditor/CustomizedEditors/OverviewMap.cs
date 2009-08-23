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
	public class OverviewMap : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.TextBox MaxRatio;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox MinRatio;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox MapId;
		private System.Windows.Forms.Label label5;
		private System.ComponentModel.IContainer components = null;

		public OverviewMap()
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

				MapId.Text = GetSettingValue("MapId");
				MinRatio.Text = GetSettingValue("MinRatio");
				MaxRatio.Text = GetSettingValue("MaxRatio");
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
			this.MaxRatio = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.MinRatio = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.MapId = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// MaxRatio
			// 
			this.MaxRatio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.MaxRatio.Location = new System.Drawing.Point(136, 56);
			this.MaxRatio.Name = "MaxRatio";
			this.MaxRatio.Size = new System.Drawing.Size(472, 20);
			this.MaxRatio.TabIndex = 25;
			this.MaxRatio.Text = "";
			this.MaxRatio.TextChanged += new System.EventHandler(this.MaxRatio_TextChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 56);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(128, 16);
			this.label9.TabIndex = 24;
			this.label9.Text = "Max ratio";
			// 
			// MinRatio
			// 
			this.MinRatio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.MinRatio.Location = new System.Drawing.Point(136, 32);
			this.MinRatio.Name = "MinRatio";
			this.MinRatio.Size = new System.Drawing.Size(472, 20);
			this.MinRatio.TabIndex = 23;
			this.MinRatio.Text = "";
			this.MinRatio.TextChanged += new System.EventHandler(this.MinRatio_TextChanged);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 32);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(128, 16);
			this.label10.TabIndex = 22;
			this.label10.Text = "Min ratio";
			// 
			// MapId
			// 
			this.MapId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.MapId.Location = new System.Drawing.Point(136, 8);
			this.MapId.Name = "MapId";
			this.MapId.Size = new System.Drawing.Size(472, 20);
			this.MapId.TabIndex = 21;
			this.MapId.Text = "";
			this.MapId.TextChanged += new System.EventHandler(this.MapId_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(128, 16);
			this.label5.TabIndex = 20;
			this.label5.Text = "Map ID";
			// 
			// OverviewMap
			// 
			this.Controls.Add(this.MaxRatio);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.MinRatio);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.MapId);
			this.Controls.Add(this.label5);
			this.Name = "OverviewMap";
			this.Size = new System.Drawing.Size(616, 88);
			this.ResumeLayout(false);

		}
		#endregion

		private void MapId_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("MapId", MapId.Text);
		}

		private void MinRatio_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("MinRatio", MinRatio.Text);
		}

		private void MaxRatio_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("MaxRatio", MaxRatio.Text);
		}
	}
}

