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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectRadius));
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
            resources.ApplyResources(this.SelectionType, "SelectionType");
            this.SelectionType.Items.AddRange(new object[] {
            resources.GetString("SelectionType.Items"),
            resources.GetString("SelectionType.Items1"),
            resources.GetString("SelectionType.Items2"),
            resources.GetString("SelectionType.Items3"),
            resources.GetString("SelectionType.Items4"),
            resources.GetString("SelectionType.Items5")});
            this.SelectionType.Name = "SelectionType";
            this.SelectionType.TextChanged += new System.EventHandler(this.SelectionType_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Tolerance
            // 
            resources.ApplyResources(this.Tolerance, "Tolerance");
            this.Tolerance.Name = "Tolerance";
            this.Tolerance.TextChanged += new System.EventHandler(this.Tolerance_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // RadiusTooltipType
            // 
            resources.ApplyResources(this.RadiusTooltipType, "RadiusTooltipType");
            this.RadiusTooltipType.Items.AddRange(new object[] {
            resources.GetString("RadiusTooltipType.Items"),
            resources.GetString("RadiusTooltipType.Items1")});
            this.RadiusTooltipType.Name = "RadiusTooltipType";
            this.RadiusTooltipType.TextChanged += new System.EventHandler(this.RadiusTooltipType_TextChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // DefaultRadius
            // 
            resources.ApplyResources(this.DefaultRadius, "DefaultRadius");
            this.DefaultRadius.Name = "DefaultRadius";
            this.DefaultRadius.TextChanged += new System.EventHandler(this.DefaultRadius_TextChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // RadiusTooltipContainer
            // 
            resources.ApplyResources(this.RadiusTooltipContainer, "RadiusTooltipContainer");
            this.RadiusTooltipContainer.Name = "RadiusTooltipContainer";
            this.RadiusTooltipContainer.TextChanged += new System.EventHandler(this.RadiusTooltipContainer_TextChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
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
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

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

