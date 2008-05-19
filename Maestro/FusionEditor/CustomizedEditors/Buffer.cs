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
using System.Data;
using System.Windows.Forms;
using System.Xml;
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors
{
	/// <summary>
	/// Summary description for Buffer.
	/// </summary>
	public class Buffer : BasisWidgetEditor
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox BufferUnits;
		private System.Windows.Forms.Label label2;
		private OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox BorderColor;
		private System.Windows.Forms.Label label3;
		private OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox FillColor;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox LayerName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox FillColorInput;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox BufferUnitsInput;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox BorderColorInput;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox LayerNameInput;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox BufferDistance;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox BufferDistanceInput;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Buffer()
		{
			// This call is required by the Windows.Forms Form Designer.
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
				if(components != null)
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
                
				try { BorderColor.CurrentColor = OSGeo.MapGuide.MaestroAPI.Utility.ParseHTMLColor(GetSettingValue("BorderColor")); }
				catch { }
				try	{ FillColor.CurrentColor = OSGeo.MapGuide.MaestroAPI.Utility.ParseHTMLColor(GetSettingValue("FillColor")); }
				catch { }
				BufferUnits.Text = GetSettingValue("BufferUnits");
				BufferDistanceInput.Text = GetSettingValue("BufferDistanceInput");
				LayerName.Text = GetSettingValue("LayerName");
				FillColorInput.Text = GetSettingValue("FillColorInput");
				BufferUnitsInput.Text = GetSettingValue("BufferUnitsInput");
				BorderColorInput.Text = GetSettingValue("BorderColorInput"); 
				LayerNameInput.Text = GetSettingValue("LayerNameInput");
				BufferDistance.Text = GetSettingValue("BufferDistance");
					
			}
			finally
			{
				m_isUpdating = false;
			}

		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Buffer));
			this.label1 = new System.Windows.Forms.Label();
			this.BufferUnits = new System.Windows.Forms.ComboBox();
			this.BorderColor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.BufferDistanceInput = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.FillColor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.LayerName = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.FillColorInput = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.BufferUnitsInput = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.BorderColorInput = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.LayerNameInput = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.BufferDistance = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Units";
			// 
			// BufferUnits
			// 
			this.BufferUnits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.BufferUnits.Items.AddRange(new object[] {
															 "meters",
															 "miles"});
			this.BufferUnits.Location = new System.Drawing.Point(136, 8);
			this.BufferUnits.Name = "BufferUnits";
			this.BufferUnits.Size = new System.Drawing.Size(424, 21);
			this.BufferUnits.TabIndex = 2;
			this.BufferUnits.TextChanged += new System.EventHandler(this.BufferUnits_TextChanged);
			// 
			// BorderColor
			// 
			this.BorderColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.BorderColor.Items.AddRange(new object[] {
															 System.Drawing.Color.Black,
															 System.Drawing.Color.White,
															 System.Drawing.Color.DarkRed,
															 System.Drawing.Color.DarkGreen,
															 System.Drawing.Color.Goldenrod,
															 System.Drawing.Color.DarkBlue,
															 System.Drawing.Color.DarkMagenta,
															 System.Drawing.Color.DarkCyan,
															 System.Drawing.Color.LightGray,
															 System.Drawing.Color.Gray,
															 System.Drawing.Color.Red,
															 System.Drawing.Color.Green,
															 System.Drawing.Color.Yellow,
															 System.Drawing.Color.Blue,
															 System.Drawing.Color.Magenta,
															 System.Drawing.Color.Cyan,
															 System.Drawing.Color.Black,
															 System.Drawing.Color.White,
															 System.Drawing.Color.DarkRed,
															 System.Drawing.Color.DarkGreen,
															 System.Drawing.Color.Goldenrod,
															 System.Drawing.Color.DarkBlue,
															 System.Drawing.Color.DarkMagenta,
															 System.Drawing.Color.DarkCyan,
															 System.Drawing.Color.LightGray,
															 System.Drawing.Color.Gray,
															 System.Drawing.Color.Red,
															 System.Drawing.Color.Green,
															 System.Drawing.Color.Yellow,
															 System.Drawing.Color.Blue,
															 System.Drawing.Color.Magenta,
															 System.Drawing.Color.Cyan});
			this.BorderColor.Location = new System.Drawing.Point(136, 80);
			this.BorderColor.Name = "BorderColor";
			this.BorderColor.Size = new System.Drawing.Size(424, 21);
			this.BorderColor.TabIndex = 4;
			this.BorderColor.SelectedIndexChanged += new System.EventHandler(this.BorderColor_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Border color";
			// 
			// BufferDistanceInput
			// 
			this.BufferDistanceInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.BufferDistanceInput.Location = new System.Drawing.Point(136, 128);
			this.BufferDistanceInput.Name = "BufferDistanceInput";
			this.BufferDistanceInput.Size = new System.Drawing.Size(424, 20);
			this.BufferDistanceInput.TabIndex = 6;
			this.BufferDistanceInput.Text = "";
			this.BufferDistanceInput.TextChanged += new System.EventHandler(this.BufferDistanceInput_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 128);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Distance Input";
			// 
			// FillColor
			// 
			this.FillColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.FillColor.Items.AddRange(new object[] {
														   System.Drawing.Color.Black,
														   System.Drawing.Color.White,
														   System.Drawing.Color.DarkRed,
														   System.Drawing.Color.DarkGreen,
														   System.Drawing.Color.Goldenrod,
														   System.Drawing.Color.DarkBlue,
														   System.Drawing.Color.DarkMagenta,
														   System.Drawing.Color.DarkCyan,
														   System.Drawing.Color.LightGray,
														   System.Drawing.Color.Gray,
														   System.Drawing.Color.Red,
														   System.Drawing.Color.Green,
														   System.Drawing.Color.Yellow,
														   System.Drawing.Color.Blue,
														   System.Drawing.Color.Magenta,
														   System.Drawing.Color.Cyan,
														   System.Drawing.Color.Black,
														   System.Drawing.Color.White,
														   System.Drawing.Color.DarkRed,
														   System.Drawing.Color.DarkGreen,
														   System.Drawing.Color.Goldenrod,
														   System.Drawing.Color.DarkBlue,
														   System.Drawing.Color.DarkMagenta,
														   System.Drawing.Color.DarkCyan,
														   System.Drawing.Color.LightGray,
														   System.Drawing.Color.Gray,
														   System.Drawing.Color.Red,
														   System.Drawing.Color.Green,
														   System.Drawing.Color.Yellow,
														   System.Drawing.Color.Blue,
														   System.Drawing.Color.Magenta,
														   System.Drawing.Color.Cyan});
			this.FillColor.Location = new System.Drawing.Point(136, 104);
			this.FillColor.Name = "FillColor";
			this.FillColor.Size = new System.Drawing.Size(424, 21);
			this.FillColor.TabIndex = 8;
			this.FillColor.SelectedIndexChanged += new System.EventHandler(this.FillColor_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(128, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "Fill color";
			// 
			// LayerName
			// 
			this.LayerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LayerName.Location = new System.Drawing.Point(136, 56);
			this.LayerName.Name = "LayerName";
			this.LayerName.Size = new System.Drawing.Size(424, 20);
			this.LayerName.TabIndex = 10;
			this.LayerName.Text = "";
			this.LayerName.TextChanged += new System.EventHandler(this.LayerName_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(128, 16);
			this.label5.TabIndex = 9;
			this.label5.Text = "Layer name";
			// 
			// FillColorInput
			// 
			this.FillColorInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.FillColorInput.Location = new System.Drawing.Point(136, 152);
			this.FillColorInput.Name = "FillColorInput";
			this.FillColorInput.Size = new System.Drawing.Size(424, 20);
			this.FillColorInput.TabIndex = 12;
			this.FillColorInput.Text = "";
			this.FillColorInput.TextChanged += new System.EventHandler(this.FillColorInput_TextChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 152);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(128, 16);
			this.label6.TabIndex = 11;
			this.label6.Text = "Fill Color Input";
			// 
			// BufferUnitsInput
			// 
			this.BufferUnitsInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.BufferUnitsInput.Location = new System.Drawing.Point(136, 176);
			this.BufferUnitsInput.Name = "BufferUnitsInput";
			this.BufferUnitsInput.Size = new System.Drawing.Size(424, 20);
			this.BufferUnitsInput.TabIndex = 14;
			this.BufferUnitsInput.Text = "";
			this.BufferUnitsInput.TextChanged += new System.EventHandler(this.BufferUnitsInput_TextChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 176);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(128, 16);
			this.label7.TabIndex = 13;
			this.label7.Text = "Buffer Units Input";
			// 
			// BorderColorInput
			// 
			this.BorderColorInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.BorderColorInput.Location = new System.Drawing.Point(136, 200);
			this.BorderColorInput.Name = "BorderColorInput";
			this.BorderColorInput.Size = new System.Drawing.Size(424, 20);
			this.BorderColorInput.TabIndex = 16;
			this.BorderColorInput.Text = "";
			this.BorderColorInput.TextChanged += new System.EventHandler(this.BorderColorInput_TextChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 200);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(128, 16);
			this.label8.TabIndex = 15;
			this.label8.Text = "Border Color Input";
			// 
			// LayerNameInput
			// 
			this.LayerNameInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LayerNameInput.Location = new System.Drawing.Point(136, 224);
			this.LayerNameInput.Name = "LayerNameInput";
			this.LayerNameInput.Size = new System.Drawing.Size(424, 20);
			this.LayerNameInput.TabIndex = 18;
			this.LayerNameInput.Text = "";
			this.LayerNameInput.TextChanged += new System.EventHandler(this.LayerNameInput_TextChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 224);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(128, 16);
			this.label9.TabIndex = 17;
			this.label9.Text = "Layer Name Input";
			// 
			// BufferDistance
			// 
			this.BufferDistance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.BufferDistance.Location = new System.Drawing.Point(136, 32);
			this.BufferDistance.Name = "BufferDistance";
			this.BufferDistance.Size = new System.Drawing.Size(424, 20);
			this.BufferDistance.TabIndex = 20;
			this.BufferDistance.Text = "";
			this.BufferDistance.TextChanged += new System.EventHandler(this.BufferDistance_TextChanged);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 32);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(128, 16);
			this.label10.TabIndex = 19;
			this.label10.Text = "Buffer Distance";
			// 
			// Buffer
			// 
			this.Controls.Add(this.BufferDistance);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.LayerNameInput);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.BorderColorInput);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.BufferUnitsInput);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.FillColorInput);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.LayerName);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.FillColor);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.BufferDistanceInput);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.BorderColor);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.BufferUnits);
			this.Controls.Add(this.label1);
			this.Name = "Buffer";
			this.Size = new System.Drawing.Size(568, 248);
			this.ResumeLayout(false);

		}
		#endregion

		private void BufferUnits_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("BufferUnits", BufferUnits.Text);
		}

		private void BufferDistance_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("BufferDistanceInput", BufferDistanceInput.Text);
		}

		private void LayerName_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("LayerName", LayerName.Text);
		}


		private void BufferDistanceInput_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("BufferDistanceInput", BufferDistanceInput.Text);
		}

		private void FillColorInput_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("FillColorInput", FillColorInput.Text);
		}

		private void BufferUnitsInput_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("BufferUnitsInput", BufferUnitsInput.Text);
		}

		private void BorderColorInput_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("BorderColorInput", BorderColorInput.Text);
		}

		private void LayerNameInput_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("LayerNameInput", LayerNameInput.Text);
		}

		private void BorderColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("BorderColor", OSGeo.MapGuide.MaestroAPI.Utility.SerializeHTMLColor(BorderColor.CurrentColor, true));
		}

		private void FillColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("FillColor", OSGeo.MapGuide.MaestroAPI.Utility.SerializeHTMLColor(FillColor.CurrentColor, true));
		}
	}
}
