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
	public class Measure : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox DistancePrecision;
		private System.Windows.Forms.TextBox AreaPrecision;
		private System.Windows.Forms.TextBox LineStyleWidth;
		private System.Windows.Forms.TextBox MeasureTooltipContainer;
		private System.Windows.Forms.TextBox MeasureTipPositionTop;
		private System.Windows.Forms.TextBox MeasureTipPositionLeft;
		private System.Windows.Forms.ComboBox FillStyle;
		private System.Windows.Forms.ComboBox LineStyleColor;
		private System.Windows.Forms.ComboBox Type;
		private System.Windows.Forms.ComboBox MeasureTooltipType;
		private System.Windows.Forms.ComboBox Units;
		private System.ComponentModel.IContainer components = null;

		public Measure()
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

				DistancePrecision.Text = GetSettingValue("DistancePrecision");
				AreaPrecision.Text = GetSettingValue("AreaPrecision");
				FillStyle.Text = GetSettingValue("FillStyle");
				LineStyleColor.Text = GetSettingValue("LineStyleColor");
				LineStyleWidth.Text = GetSettingValue("LineStyleWidth");
				MeasureTipPositionLeft.Text = GetSettingValue("MeasureTipPositionLeft");
				MeasureTipPositionTop.Text = GetSettingValue("MeasureTipPositionTop");
				MeasureTooltipContainer.Text = GetSettingValue("MeasureTooltipContainer");
				MeasureTooltipType.Text = GetSettingValue("MeasureTooltipType");
				Type.Text = GetSettingValue("Type");
				Units.Text = GetSettingValue("Units");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Measure));
            this.DistancePrecision = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AreaPrecision = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LineStyleWidth = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.MeasureTooltipContainer = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.MeasureTipPositionTop = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.MeasureTipPositionLeft = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.FillStyle = new System.Windows.Forms.ComboBox();
            this.LineStyleColor = new System.Windows.Forms.ComboBox();
            this.Type = new System.Windows.Forms.ComboBox();
            this.MeasureTooltipType = new System.Windows.Forms.ComboBox();
            this.Units = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // DistancePrecision
            // 
            resources.ApplyResources(this.DistancePrecision, "DistancePrecision");
            this.DistancePrecision.Name = "DistancePrecision";
            this.DistancePrecision.TextChanged += new System.EventHandler(this.DistancePrecision_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // AreaPrecision
            // 
            resources.ApplyResources(this.AreaPrecision, "AreaPrecision");
            this.AreaPrecision.Name = "AreaPrecision";
            this.AreaPrecision.TextChanged += new System.EventHandler(this.AreaPrecision_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // LineStyleWidth
            // 
            resources.ApplyResources(this.LineStyleWidth, "LineStyleWidth");
            this.LineStyleWidth.Name = "LineStyleWidth";
            this.LineStyleWidth.TextChanged += new System.EventHandler(this.LineStyleWidth_TextChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // MeasureTooltipContainer
            // 
            resources.ApplyResources(this.MeasureTooltipContainer, "MeasureTooltipContainer");
            this.MeasureTooltipContainer.Name = "MeasureTooltipContainer";
            this.MeasureTooltipContainer.TextChanged += new System.EventHandler(this.MeasureTooltipContainer_TextChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // MeasureTipPositionTop
            // 
            resources.ApplyResources(this.MeasureTipPositionTop, "MeasureTipPositionTop");
            this.MeasureTipPositionTop.Name = "MeasureTipPositionTop";
            this.MeasureTipPositionTop.TextChanged += new System.EventHandler(this.MeasureTipPositionTop_TextChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // MeasureTipPositionLeft
            // 
            resources.ApplyResources(this.MeasureTipPositionLeft, "MeasureTipPositionLeft");
            this.MeasureTipPositionLeft.Name = "MeasureTipPositionLeft";
            this.MeasureTipPositionLeft.TextChanged += new System.EventHandler(this.MeasureTipPositionLeft_TextChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // FillStyle
            // 
            resources.ApplyResources(this.FillStyle, "FillStyle");
            this.FillStyle.Name = "FillStyle";
            this.FillStyle.TextChanged += new System.EventHandler(this.FillStyle_TextChanged);
            // 
            // LineStyleColor
            // 
            resources.ApplyResources(this.LineStyleColor, "LineStyleColor");
            this.LineStyleColor.Name = "LineStyleColor";
            this.LineStyleColor.TextChanged += new System.EventHandler(this.LineStyleColor_TextChanged);
            // 
            // Type
            // 
            resources.ApplyResources(this.Type, "Type");
            this.Type.Items.AddRange(new object[] {
            resources.GetString("Type.Items")});
            this.Type.Name = "Type";
            this.Type.TextChanged += new System.EventHandler(this.Type_TextChanged);
            // 
            // MeasureTooltipType
            // 
            resources.ApplyResources(this.MeasureTooltipType, "MeasureTooltipType");
            this.MeasureTooltipType.Items.AddRange(new object[] {
            resources.GetString("MeasureTooltipType.Items"),
            resources.GetString("MeasureTooltipType.Items1")});
            this.MeasureTooltipType.Name = "MeasureTooltipType";
            this.MeasureTooltipType.TextChanged += new System.EventHandler(this.MeasureTooltipType_TextChanged);
            // 
            // Units
            // 
            resources.ApplyResources(this.Units, "Units");
            this.Units.Items.AddRange(new object[] {
            resources.GetString("Units.Items"),
            resources.GetString("Units.Items1"),
            resources.GetString("Units.Items2")});
            this.Units.Name = "Units";
            this.Units.TextChanged += new System.EventHandler(this.Units_TextChanged);
            // 
            // Measure
            // 
            this.Controls.Add(this.Units);
            this.Controls.Add(this.Type);
            this.Controls.Add(this.MeasureTooltipType);
            this.Controls.Add(this.LineStyleColor);
            this.Controls.Add(this.FillStyle);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.MeasureTooltipContainer);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.MeasureTipPositionTop);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.MeasureTipPositionLeft);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.LineStyleWidth);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AreaPrecision);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DistancePrecision);
            this.Controls.Add(this.label1);
            this.Name = "Measure";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void DistancePrecision_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("DistancePrecision", DistancePrecision.Text);
		}

		private void AreaPrecision_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("AreaPrecision", AreaPrecision.Text);
		}

		private void FillStyle_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("FillStyle", FillStyle.Text);
		}

		private void LineStyleColor_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("LineStyleColor", LineStyleColor.Text);
		}

		private void LineStyleWidth_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("LineStyleWidth", LineStyleWidth.Text);
		}

		private void MeasureTipPositionLeft_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("MeasureTipPositionLeft", MeasureTipPositionLeft.Text);
		}

		private void MeasureTipPositionTop_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("MeasureTipPositionTop", MeasureTipPositionTop.Text);
		}

		private void MeasureTooltipContainer_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("MeasureTooltipContainer", MeasureTooltipContainer.Text);
		}

		private void MeasureTooltipType_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("MeasureTooltipType", MeasureTooltipType.Text);
		}

		private void Type_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Type", Type.Text);
		}

		private void Units_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Units", Units.Text);
		}
	}
}

