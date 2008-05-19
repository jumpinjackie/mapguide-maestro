#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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
	public class SelectRadius : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.ComboBox SelectionType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox Tolerance;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox RadiusTooltipType;
		private System.Windows.Forms.TextBox DefaultRadius;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox RadiusTooltipContainer;
		private System.Windows.Forms.Label label10;
		private System.ComponentModel.IContainer components = null;

		public SelectRadius()
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
				SelectionType.Text = GetSettingValue("SelectionType"); 
				RadiusTooltipType.Text = GetSettingValue("RadiusTooltipType"); 
				RadiusTooltipContainer.Text = GetSettingValue("RadiusTooltipContainer"); 
				DefaultRadius.Text = GetSettingValue("DefaultRadius"); 
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
			this.SelectionType = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.Tolerance = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.RadiusTooltipType = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.DefaultRadius = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.RadiusTooltipContainer = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// SelectionType
			// 
			this.SelectionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.SelectionType.Items.AddRange(new object[] {
															   "CONTAINS",
															   "DISJOINT",
															   "INTERSECTS",
															   "WITHIN",
															   "INSIDE",
															   "ENVELOPEINTERSECTS"});
			this.SelectionType.Location = new System.Drawing.Point(144, 32);
			this.SelectionType.Name = "SelectionType";
			this.SelectionType.Size = new System.Drawing.Size(464, 21);
			this.SelectionType.TabIndex = 36;
			this.SelectionType.TextChanged += new System.EventHandler(this.SelectionType_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.TabIndex = 35;
			this.label2.Text = "Selection type";
			// 
			// Tolerance
			// 
			this.Tolerance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Tolerance.Location = new System.Drawing.Point(144, 8);
			this.Tolerance.Name = "Tolerance";
			this.Tolerance.Size = new System.Drawing.Size(464, 20);
			this.Tolerance.TabIndex = 34;
			this.Tolerance.Text = "";
			this.Tolerance.TextChanged += new System.EventHandler(this.Tolerance_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 16);
			this.label1.TabIndex = 33;
			this.label1.Text = "Tolerance";
			// 
			// RadiusTooltipType
			// 
			this.RadiusTooltipType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.RadiusTooltipType.Items.AddRange(new object[] {
																   "static",
																   "dynamic"});
			this.RadiusTooltipType.Location = new System.Drawing.Point(144, 56);
			this.RadiusTooltipType.Name = "RadiusTooltipType";
			this.RadiusTooltipType.Size = new System.Drawing.Size(464, 21);
			this.RadiusTooltipType.TabIndex = 38;
			this.RadiusTooltipType.TextChanged += new System.EventHandler(this.RadiusTooltipType_TextChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 56);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(136, 16);
			this.label7.TabIndex = 37;
			this.label7.Text = "Tooltip type";
			// 
			// DefaultRadius
			// 
			this.DefaultRadius.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.DefaultRadius.Location = new System.Drawing.Point(144, 104);
			this.DefaultRadius.Name = "DefaultRadius";
			this.DefaultRadius.Size = new System.Drawing.Size(464, 20);
			this.DefaultRadius.TabIndex = 42;
			this.DefaultRadius.Text = "";
			this.DefaultRadius.TextChanged += new System.EventHandler(this.DefaultRadius_TextChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 104);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(136, 16);
			this.label9.TabIndex = 41;
			this.label9.Text = "Default radius";
			// 
			// RadiusTooltipContainer
			// 
			this.RadiusTooltipContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.RadiusTooltipContainer.Location = new System.Drawing.Point(144, 80);
			this.RadiusTooltipContainer.Name = "RadiusTooltipContainer";
			this.RadiusTooltipContainer.Size = new System.Drawing.Size(464, 20);
			this.RadiusTooltipContainer.TabIndex = 40;
			this.RadiusTooltipContainer.Text = "";
			this.RadiusTooltipContainer.TextChanged += new System.EventHandler(this.RadiusTooltipContainer_TextChanged);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 80);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(136, 16);
			this.label10.TabIndex = 39;
			this.label10.Text = "Tooltip Container";
			// 
			// SelectRadius
			// 
			this.Controls.Add(this.DefaultRadius);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.RadiusTooltipContainer);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.RadiusTooltipType);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.SelectionType);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Tolerance);
			this.Controls.Add(this.label1);
			this.Name = "SelectRadius";
			this.Size = new System.Drawing.Size(616, 128);
			this.ResumeLayout(false);

		}
		#endregion

		private void Tolerance_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Tolerance", Tolerance.Text);
		}

		private void SelectionType_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("SelectionType", SelectionType.Text);
		}

		private void RadiusTooltipType_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("RadiusTooltipType", RadiusTooltipType.Text);
		}

		private void RadiusTooltipContainer_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("RadiusTooltipContainer", RadiusTooltipContainer.Text);
		}

		private void DefaultRadius_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("DefaultRadius", DefaultRadius.Text);
		}
	}
}

