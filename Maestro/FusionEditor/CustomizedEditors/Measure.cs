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
			this.DistancePrecision.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.DistancePrecision.Location = new System.Drawing.Point(136, 8);
			this.DistancePrecision.Name = "DistancePrecision";
			this.DistancePrecision.Size = new System.Drawing.Size(472, 20);
			this.DistancePrecision.TabIndex = 7;
			this.DistancePrecision.Text = "";
			this.DistancePrecision.TextChanged += new System.EventHandler(this.DistancePrecision_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Distance precision";
			// 
			// AreaPrecision
			// 
			this.AreaPrecision.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.AreaPrecision.Location = new System.Drawing.Point(136, 32);
			this.AreaPrecision.Name = "AreaPrecision";
			this.AreaPrecision.Size = new System.Drawing.Size(472, 20);
			this.AreaPrecision.TabIndex = 9;
			this.AreaPrecision.Text = "";
			this.AreaPrecision.TextChanged += new System.EventHandler(this.AreaPrecision_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "Area precision";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 16);
			this.label3.TabIndex = 10;
			this.label3.Text = "Fill color";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(128, 16);
			this.label4.TabIndex = 12;
			this.label4.Text = "Line color";
			// 
			// LineStyleWidth
			// 
			this.LineStyleWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LineStyleWidth.Location = new System.Drawing.Point(136, 104);
			this.LineStyleWidth.Name = "LineStyleWidth";
			this.LineStyleWidth.Size = new System.Drawing.Size(472, 20);
			this.LineStyleWidth.TabIndex = 15;
			this.LineStyleWidth.Text = "";
			this.LineStyleWidth.TextChanged += new System.EventHandler(this.LineStyleWidth_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 104);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(128, 16);
			this.label5.TabIndex = 14;
			this.label5.Text = "Line width";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 224);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(128, 16);
			this.label6.TabIndex = 24;
			this.label6.Text = "Type";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 200);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(128, 16);
			this.label7.TabIndex = 22;
			this.label7.Text = "Tooltip type";
			// 
			// MeasureTooltipContainer
			// 
			this.MeasureTooltipContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.MeasureTooltipContainer.Location = new System.Drawing.Point(136, 176);
			this.MeasureTooltipContainer.Name = "MeasureTooltipContainer";
			this.MeasureTooltipContainer.Size = new System.Drawing.Size(472, 20);
			this.MeasureTooltipContainer.TabIndex = 21;
			this.MeasureTooltipContainer.Text = "";
			this.MeasureTooltipContainer.TextChanged += new System.EventHandler(this.MeasureTooltipContainer_TextChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 176);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(128, 16);
			this.label8.TabIndex = 20;
			this.label8.Text = "Tooltip container";
			// 
			// MeasureTipPositionTop
			// 
			this.MeasureTipPositionTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.MeasureTipPositionTop.Location = new System.Drawing.Point(136, 152);
			this.MeasureTipPositionTop.Name = "MeasureTipPositionTop";
			this.MeasureTipPositionTop.Size = new System.Drawing.Size(472, 20);
			this.MeasureTipPositionTop.TabIndex = 19;
			this.MeasureTipPositionTop.Text = "";
			this.MeasureTipPositionTop.TextChanged += new System.EventHandler(this.MeasureTipPositionTop_TextChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 152);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(128, 16);
			this.label9.TabIndex = 18;
			this.label9.Text = "Measure tip top";
			// 
			// MeasureTipPositionLeft
			// 
			this.MeasureTipPositionLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.MeasureTipPositionLeft.Location = new System.Drawing.Point(136, 128);
			this.MeasureTipPositionLeft.Name = "MeasureTipPositionLeft";
			this.MeasureTipPositionLeft.Size = new System.Drawing.Size(472, 20);
			this.MeasureTipPositionLeft.TabIndex = 17;
			this.MeasureTipPositionLeft.Text = "";
			this.MeasureTipPositionLeft.TextChanged += new System.EventHandler(this.MeasureTipPositionLeft_TextChanged);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 128);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(128, 16);
			this.label10.TabIndex = 16;
			this.label10.Text = "Measure tip left";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(8, 248);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(128, 16);
			this.label11.TabIndex = 26;
			this.label11.Text = "Units";
			// 
			// FillStyle
			// 
			this.FillStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.FillStyle.Location = new System.Drawing.Point(136, 56);
			this.FillStyle.Name = "FillStyle";
			this.FillStyle.Size = new System.Drawing.Size(472, 21);
			this.FillStyle.TabIndex = 28;
			this.FillStyle.TextChanged += new System.EventHandler(this.FillStyle_TextChanged);
			// 
			// LineStyleColor
			// 
			this.LineStyleColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LineStyleColor.Location = new System.Drawing.Point(136, 80);
			this.LineStyleColor.Name = "LineStyleColor";
			this.LineStyleColor.Size = new System.Drawing.Size(472, 21);
			this.LineStyleColor.TabIndex = 29;
			this.LineStyleColor.TextChanged += new System.EventHandler(this.LineStyleColor_TextChanged);
			// 
			// Type
			// 
			this.Type.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Type.Items.AddRange(new object[] {
													  "both"});
			this.Type.Location = new System.Drawing.Point(136, 224);
			this.Type.Name = "Type";
			this.Type.Size = new System.Drawing.Size(472, 21);
			this.Type.TabIndex = 31;
			this.Type.TextChanged += new System.EventHandler(this.Type_TextChanged);
			// 
			// MeasureTooltipType
			// 
			this.MeasureTooltipType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.MeasureTooltipType.Items.AddRange(new object[] {
																	"static",
																	"dynamic"});
			this.MeasureTooltipType.Location = new System.Drawing.Point(136, 200);
			this.MeasureTooltipType.Name = "MeasureTooltipType";
			this.MeasureTooltipType.Size = new System.Drawing.Size(472, 21);
			this.MeasureTooltipType.TabIndex = 30;
			this.MeasureTooltipType.TextChanged += new System.EventHandler(this.MeasureTooltipType_TextChanged);
			// 
			// Units
			// 
			this.Units.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Units.Items.AddRange(new object[] {
													   "meters",
													   "miles",
													   "none"});
			this.Units.Location = new System.Drawing.Point(136, 248);
			this.Units.Name = "Units";
			this.Units.Size = new System.Drawing.Size(472, 21);
			this.Units.TabIndex = 32;
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
			this.Size = new System.Drawing.Size(616, 280);
			this.ResumeLayout(false);

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

