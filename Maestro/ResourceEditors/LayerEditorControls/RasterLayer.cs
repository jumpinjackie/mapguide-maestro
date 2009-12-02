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
using System.Data;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls
{
	/// <summary>
	/// Summary description for RasterLayer.
	/// </summary>
	public class RasterLayer : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.LayerDefinition m_layer;
		private bool inUpdate = false;
		private OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription m_schemas;
		private System.Windows.Forms.GroupBox ResourceGroup;
		private ResourceEditors.LayerEditorControls.SchemaSelector schemaSelector;
		private System.Windows.Forms.GroupBox VisualSettings;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox VisibleFrom;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox VisibleTo;
		private System.Windows.Forms.Label RebuildThreshold;
		private ResourceEditors.GeometryStyleEditors.ColorComboBox ForegroundColor;
		private ResourceEditors.GeometryStyleEditors.ColorComboBox BackgroundColor;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox RebuildFactor;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.CheckBox EnableAdvanced;
		private System.Windows.Forms.GroupBox AdvancedSettings;
		private System.Windows.Forms.TextBox BrightnessFactor;
		private System.Windows.Forms.TextBox ContrastFactor;
		private System.Windows.Forms.TextBox Altitude;
		private System.Windows.Forms.TextBox Azimuth;
		private System.Windows.Forms.TextBox HillshadeScaleFactor;
		private System.Windows.Forms.TextBox ZeroValue;
		private System.Windows.Forms.TextBox SurfaceScaleFactor;
		private System.Windows.Forms.TextBox HillshadeBand;
		private System.Windows.Forms.TextBox SurfaceBand;
		private ResourceEditors.GeometryStyleEditors.ColorComboBox TransparencyColor;
		private ResourceEditors.GeometryStyleEditors.ColorComboBox DefaultColor;
		private System.Windows.Forms.CheckBox EnableSurface;
		private System.Windows.Forms.CheckBox EnableHillshade;
		private System.Windows.Forms.GroupBox HillshadeGroup;
		private System.Windows.Forms.GroupBox SurfaceGroup;
		private EditorInterface m_editor;

		public RasterLayer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			schemaSelector.IsRaster = true;

            ForegroundColor.ResetColors();
            BackgroundColor.ResetColors();
            TransparencyColor.ResetColors();
            DefaultColor.ResetColors();

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

		public void SetItem(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.LayerDefinition layer, OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription schema)
		{
			m_editor = editor;
			m_layer = layer;
			m_schemas = schema;

			schemaSelector.SetItem(editor, layer, schema);
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				inUpdate = true;

				if (m_layer == null || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
					return;

				OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld = FixupLayer();
				if (gld.GridScaleRange[0].MinScaleSpecified)
					VisibleFrom.Text = gld.GridScaleRange[0].MinScale.ToString("0", System.Globalization.CultureInfo.CurrentUICulture);
				else
					VisibleFrom.Text = "0";

				if (gld.GridScaleRange[0].MaxScaleSpecified)
				{
					VisibleTo.SelectedIndex = -1;
					VisibleTo.Text = gld.GridScaleRange[0].MaxScale.ToString("0", System.Globalization.CultureInfo.CurrentUICulture);
				}
				else
					VisibleTo.SelectedIndex = 0;


				if (gld.GridScaleRange[0].RebuildFactor > 0)
					RebuildFactor.Text = gld.GridScaleRange[0].RebuildFactor.ToString("0", System.Globalization.CultureInfo.CurrentUICulture);
				else
				{
                    RebuildFactor.Text = "1";
					gld.GridScaleRange[0].RebuildFactor = 1;
					m_editor.HasChanged();
				}

				if (gld.GridScaleRange[0].ColorStyle.ColorRule[0].Color.Item as string == null || (gld.GridScaleRange[0].ColorStyle.ColorRule[0].Color.Item as string) == "")
				{
					gld.GridScaleRange[0].ColorStyle.ColorRule[0].Color.Item = OSGeo.MapGuide.MaestroAPI.Utility.SerializeHTMLColor(Color.Black, false);
					ForegroundColor.CurrentColor = Color.Black;
					m_editor.HasChanged();
				}
				else
					ForegroundColor.CurrentColor = OSGeo.MapGuide.MaestroAPI.Utility.ParseHTMLColor(gld.GridScaleRange[0].ColorStyle.ColorRule[0].Color.Item as string);

				if (gld.GridScaleRange[0].ColorStyle.ColorRule[1].Color.Item as string == null || (gld.GridScaleRange[0].ColorStyle.ColorRule[1].Color.Item as string) == "")
				{
					gld.GridScaleRange[0].ColorStyle.ColorRule[1].Color.Item = OSGeo.MapGuide.MaestroAPI.Utility.SerializeHTMLColor(Color.White, false);
					BackgroundColor.CurrentColor = Color.White;
					m_editor.HasChanged();
				}
				else
					BackgroundColor.CurrentColor = OSGeo.MapGuide.MaestroAPI.Utility.ParseHTMLColor(gld.GridScaleRange[0].ColorStyle.ColorRule[1].Color.Item as string);


				//Extended stuff

				if (
					gld.GridScaleRange[0].ColorStyle.BrightnessFactorSpecified ||
					gld.GridScaleRange[0].ColorStyle.ContrastFactorSpecified ||
					(gld.GridScaleRange[0].ColorStyle.TransparencyColor as string != null && ((string)gld.GridScaleRange[0].ColorStyle.TransparencyColor).Length > 0))
				{
					EnableAdvanced.Checked = true;

					if (gld.GridScaleRange[0].ColorStyle.BrightnessFactorSpecified)
						BrightnessFactor.Text = gld.GridScaleRange[0].ColorStyle.BrightnessFactor.ToString("0", System.Globalization.CultureInfo.CurrentUICulture);
					else
						BrightnessFactor.Text = "0";

					if (gld.GridScaleRange[0].ColorStyle.ContrastFactorSpecified)
						ContrastFactor.Text = gld.GridScaleRange[0].ColorStyle.ContrastFactor.ToString("0", System.Globalization.CultureInfo.CurrentUICulture);
					else
						ContrastFactor.Text = "0";

					if (gld.GridScaleRange[0].ColorStyle.TransparencyColor as string != null && ((string)gld.GridScaleRange[0].ColorStyle.TransparencyColor).Length > 0)
						TransparencyColor.CurrentColor = OSGeo.MapGuide.MaestroAPI.Utility.ParseHTMLColor(gld.GridScaleRange[0].ColorStyle.TransparencyColor as string);
					else
						TransparencyColor.SelectedIndex = -1;
				}
				else
				{
					EnableAdvanced.Checked = false;
				}

				if (gld.GridScaleRange[0].ColorStyle.HillShade != null)
				{
					EnableHillshade.Checked = true;
					Altitude.Text = gld.GridScaleRange[0].ColorStyle.HillShade.Altitude.ToString("0", System.Globalization.CultureInfo.CurrentUICulture);
					Azimuth.Text = gld.GridScaleRange[0].ColorStyle.HillShade.Azimuth.ToString("0", System.Globalization.CultureInfo.CurrentUICulture);
					if (gld.GridScaleRange[0].ColorStyle.HillShade.ScaleFactorSpecified)
						HillshadeScaleFactor.Text = gld.GridScaleRange[0].ColorStyle.HillShade.ScaleFactor.ToString("0", System.Globalization.CultureInfo.CurrentUICulture);
					else
						HillshadeScaleFactor.Text = "0";

				
					HillshadeBand.Text = gld.GridScaleRange[0].ColorStyle.HillShade.Band;
				}
				else
				{
					EnableHillshade.Checked = false;
				}

				if (gld.GridScaleRange[0].SurfaceStyle != null)
				{
					EnableSurface.Checked = true;
					SurfaceBand.Text = gld.GridScaleRange[0].SurfaceStyle.Band;

					if (gld.GridScaleRange[0].SurfaceStyle.ScaleFactorSpecified)
						SurfaceScaleFactor.Text = gld.GridScaleRange[0].SurfaceStyle.ScaleFactor.ToString("0", System.Globalization.CultureInfo.CurrentUICulture);
					else
						SurfaceScaleFactor.Text = "0";

					if (gld.GridScaleRange[0].SurfaceStyle.ZeroValueSpecified)
						ZeroValue.Text = gld.GridScaleRange[0].SurfaceStyle.ZeroValue.ToString("0", System.Globalization.CultureInfo.CurrentUICulture);
					else
						ZeroValue.Text = "0";

					if (gld.GridScaleRange[0].SurfaceStyle.DefaultColor as string != null && ((string)gld.GridScaleRange[0].SurfaceStyle.DefaultColor).Length > 0)
						DefaultColor.CurrentColor = OSGeo.MapGuide.MaestroAPI.Utility.ParseHTMLColor(gld.GridScaleRange[0].SurfaceStyle.DefaultColor as string);
					else
						DefaultColor.SelectedIndex = -1;
				}
				else
				{
					EnableSurface.Checked = false;
				}
			}
			finally
			{
				inUpdate = false;
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RasterLayer));
            this.ResourceGroup = new System.Windows.Forms.GroupBox();
            this.schemaSelector = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.SchemaSelector();
            this.VisualSettings = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BackgroundColor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox();
            this.ForegroundColor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox();
            this.RebuildFactor = new System.Windows.Forms.TextBox();
            this.RebuildThreshold = new System.Windows.Forms.Label();
            this.VisibleTo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.VisibleFrom = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AdvancedSettings = new System.Windows.Forms.GroupBox();
            this.EnableSurface = new System.Windows.Forms.CheckBox();
            this.EnableHillshade = new System.Windows.Forms.CheckBox();
            this.TransparencyColor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox();
            this.ContrastFactor = new System.Windows.Forms.TextBox();
            this.BrightnessFactor = new System.Windows.Forms.TextBox();
            this.HillshadeGroup = new System.Windows.Forms.GroupBox();
            this.HillshadeBand = new System.Windows.Forms.TextBox();
            this.HillshadeScaleFactor = new System.Windows.Forms.TextBox();
            this.Azimuth = new System.Windows.Forms.TextBox();
            this.Altitude = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SurfaceGroup = new System.Windows.Forms.GroupBox();
            this.DefaultColor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox();
            this.SurfaceBand = new System.Windows.Forms.TextBox();
            this.SurfaceScaleFactor = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.ZeroValue = new System.Windows.Forms.TextBox();
            this.EnableAdvanced = new System.Windows.Forms.CheckBox();
            this.ResourceGroup.SuspendLayout();
            this.VisualSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.AdvancedSettings.SuspendLayout();
            this.HillshadeGroup.SuspendLayout();
            this.SurfaceGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ResourceGroup
            // 
            resources.ApplyResources(this.ResourceGroup, "ResourceGroup");
            this.ResourceGroup.Controls.Add(this.schemaSelector);
            this.ResourceGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ResourceGroup.Name = "ResourceGroup";
            this.ResourceGroup.TabStop = false;
            // 
            // schemaSelector
            // 
            resources.ApplyResources(this.schemaSelector, "schemaSelector");
            this.schemaSelector.IsRaster = false;
            this.schemaSelector.Name = "schemaSelector";
            this.schemaSelector.GeometryChanged += new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.SchemaSelector.GeometryChangedDelegate(this.schemaSelector_GeometryChanged);
            // 
            // VisualSettings
            // 
            resources.ApplyResources(this.VisualSettings, "VisualSettings");
            this.VisualSettings.Controls.Add(this.groupBox1);
            this.VisualSettings.Controls.Add(this.RebuildFactor);
            this.VisualSettings.Controls.Add(this.RebuildThreshold);
            this.VisualSettings.Controls.Add(this.VisibleTo);
            this.VisualSettings.Controls.Add(this.label2);
            this.VisualSettings.Controls.Add(this.VisibleFrom);
            this.VisualSettings.Controls.Add(this.label1);
            this.VisualSettings.Name = "VisualSettings";
            this.VisualSettings.TabStop = false;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.BackgroundColor);
            this.groupBox1.Controls.Add(this.ForegroundColor);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // BackgroundColor
            // 
            resources.ApplyResources(this.BackgroundColor, "BackgroundColor");
            this.BackgroundColor.Name = "BackgroundColor";
            this.BackgroundColor.SelectedIndexChanged += new System.EventHandler(this.BackgroundColor_SelectedIndexChanged);
            // 
            // ForegroundColor
            // 
            resources.ApplyResources(this.ForegroundColor, "ForegroundColor");
            this.ForegroundColor.Name = "ForegroundColor";
            this.ForegroundColor.SelectedIndexChanged += new System.EventHandler(this.ForegroundColor_SelectedIndexChanged);
            // 
            // RebuildFactor
            // 
            resources.ApplyResources(this.RebuildFactor, "RebuildFactor");
            this.RebuildFactor.Name = "RebuildFactor";
            this.RebuildFactor.TextChanged += new System.EventHandler(this.RebuildFactor_TextChanged);
            // 
            // RebuildThreshold
            // 
            resources.ApplyResources(this.RebuildThreshold, "RebuildThreshold");
            this.RebuildThreshold.Name = "RebuildThreshold";
            // 
            // VisibleTo
            // 
            this.VisibleTo.Items.AddRange(new object[] {
            resources.GetString("VisibleTo.Items")});
            resources.ApplyResources(this.VisibleTo, "VisibleTo");
            this.VisibleTo.Name = "VisibleTo";
            this.VisibleTo.SelectedIndexChanged += new System.EventHandler(this.VisibleTo_SelectedIndexChanged);
            this.VisibleTo.TextChanged += new System.EventHandler(this.VisibleTo_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // VisibleFrom
            // 
            resources.ApplyResources(this.VisibleFrom, "VisibleFrom");
            this.VisibleFrom.Name = "VisibleFrom";
            this.VisibleFrom.TextChanged += new System.EventHandler(this.VisibleFrom_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // AdvancedSettings
            // 
            resources.ApplyResources(this.AdvancedSettings, "AdvancedSettings");
            this.AdvancedSettings.Controls.Add(this.EnableSurface);
            this.AdvancedSettings.Controls.Add(this.EnableHillshade);
            this.AdvancedSettings.Controls.Add(this.TransparencyColor);
            this.AdvancedSettings.Controls.Add(this.ContrastFactor);
            this.AdvancedSettings.Controls.Add(this.BrightnessFactor);
            this.AdvancedSettings.Controls.Add(this.HillshadeGroup);
            this.AdvancedSettings.Controls.Add(this.label6);
            this.AdvancedSettings.Controls.Add(this.label5);
            this.AdvancedSettings.Controls.Add(this.label7);
            this.AdvancedSettings.Controls.Add(this.SurfaceGroup);
            this.AdvancedSettings.Name = "AdvancedSettings";
            this.AdvancedSettings.TabStop = false;
            // 
            // EnableSurface
            // 
            this.EnableSurface.Checked = true;
            this.EnableSurface.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.EnableSurface, "EnableSurface");
            this.EnableSurface.Name = "EnableSurface";
            this.EnableSurface.CheckedChanged += new System.EventHandler(this.EnableSurface_CheckedChanged);
            // 
            // EnableHillshade
            // 
            this.EnableHillshade.Checked = true;
            this.EnableHillshade.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.EnableHillshade, "EnableHillshade");
            this.EnableHillshade.Name = "EnableHillshade";
            this.EnableHillshade.CheckedChanged += new System.EventHandler(this.EnableHillshade_CheckedChanged);
            // 
            // TransparencyColor
            // 
            resources.ApplyResources(this.TransparencyColor, "TransparencyColor");
            this.TransparencyColor.Name = "TransparencyColor";
            this.TransparencyColor.SelectedIndexChanged += new System.EventHandler(this.TransparencyColor_SelectedIndexChanged);
            // 
            // ContrastFactor
            // 
            resources.ApplyResources(this.ContrastFactor, "ContrastFactor");
            this.ContrastFactor.Name = "ContrastFactor";
            this.ContrastFactor.TextChanged += new System.EventHandler(this.ContrastFactor_TextChanged);
            // 
            // BrightnessFactor
            // 
            resources.ApplyResources(this.BrightnessFactor, "BrightnessFactor");
            this.BrightnessFactor.Name = "BrightnessFactor";
            this.BrightnessFactor.TextChanged += new System.EventHandler(this.BrightnessFactor_TextChanged);
            // 
            // HillshadeGroup
            // 
            resources.ApplyResources(this.HillshadeGroup, "HillshadeGroup");
            this.HillshadeGroup.Controls.Add(this.HillshadeBand);
            this.HillshadeGroup.Controls.Add(this.HillshadeScaleFactor);
            this.HillshadeGroup.Controls.Add(this.Azimuth);
            this.HillshadeGroup.Controls.Add(this.Altitude);
            this.HillshadeGroup.Controls.Add(this.label10);
            this.HillshadeGroup.Controls.Add(this.label8);
            this.HillshadeGroup.Controls.Add(this.label9);
            this.HillshadeGroup.Controls.Add(this.label11);
            this.HillshadeGroup.Name = "HillshadeGroup";
            this.HillshadeGroup.TabStop = false;
            // 
            // HillshadeBand
            // 
            resources.ApplyResources(this.HillshadeBand, "HillshadeBand");
            this.HillshadeBand.Name = "HillshadeBand";
            this.HillshadeBand.TextChanged += new System.EventHandler(this.HillshadeBand_TextChanged);
            // 
            // HillshadeScaleFactor
            // 
            resources.ApplyResources(this.HillshadeScaleFactor, "HillshadeScaleFactor");
            this.HillshadeScaleFactor.Name = "HillshadeScaleFactor";
            this.HillshadeScaleFactor.TextChanged += new System.EventHandler(this.HillshadeScaleFactor_TextChanged);
            // 
            // Azimuth
            // 
            resources.ApplyResources(this.Azimuth, "Azimuth");
            this.Azimuth.Name = "Azimuth";
            this.Azimuth.TextChanged += new System.EventHandler(this.Azimuth_TextChanged);
            // 
            // Altitude
            // 
            resources.ApplyResources(this.Altitude, "Altitude");
            this.Altitude.Name = "Altitude";
            this.Altitude.TextChanged += new System.EventHandler(this.Altitude_TextChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // SurfaceGroup
            // 
            resources.ApplyResources(this.SurfaceGroup, "SurfaceGroup");
            this.SurfaceGroup.Controls.Add(this.DefaultColor);
            this.SurfaceGroup.Controls.Add(this.SurfaceBand);
            this.SurfaceGroup.Controls.Add(this.SurfaceScaleFactor);
            this.SurfaceGroup.Controls.Add(this.label12);
            this.SurfaceGroup.Controls.Add(this.label13);
            this.SurfaceGroup.Controls.Add(this.label14);
            this.SurfaceGroup.Controls.Add(this.label15);
            this.SurfaceGroup.Controls.Add(this.ZeroValue);
            this.SurfaceGroup.Name = "SurfaceGroup";
            this.SurfaceGroup.TabStop = false;
            // 
            // DefaultColor
            // 
            resources.ApplyResources(this.DefaultColor, "DefaultColor");
            this.DefaultColor.Name = "DefaultColor";
            this.DefaultColor.SelectedIndexChanged += new System.EventHandler(this.DefaultColor_SelectedIndexChanged);
            // 
            // SurfaceBand
            // 
            resources.ApplyResources(this.SurfaceBand, "SurfaceBand");
            this.SurfaceBand.Name = "SurfaceBand";
            this.SurfaceBand.TextChanged += new System.EventHandler(this.SurfaceBand_TextChanged);
            // 
            // SurfaceScaleFactor
            // 
            resources.ApplyResources(this.SurfaceScaleFactor, "SurfaceScaleFactor");
            this.SurfaceScaleFactor.Name = "SurfaceScaleFactor";
            this.SurfaceScaleFactor.TextChanged += new System.EventHandler(this.SurfaceScaleFactor_TextChanged);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // ZeroValue
            // 
            resources.ApplyResources(this.ZeroValue, "ZeroValue");
            this.ZeroValue.Name = "ZeroValue";
            this.ZeroValue.TextChanged += new System.EventHandler(this.ZeroValue_TextChanged);
            // 
            // EnableAdvanced
            // 
            resources.ApplyResources(this.EnableAdvanced, "EnableAdvanced");
            this.EnableAdvanced.Name = "EnableAdvanced";
            this.EnableAdvanced.CheckedChanged += new System.EventHandler(this.EnableAdvanced_CheckedChanged);
            // 
            // RasterLayer
            // 
            this.Controls.Add(this.EnableAdvanced);
            this.Controls.Add(this.AdvancedSettings);
            this.Controls.Add(this.VisualSettings);
            this.Controls.Add(this.ResourceGroup);
            this.Name = "RasterLayer";
            resources.ApplyResources(this, "$this");
            this.ResourceGroup.ResumeLayout(false);
            this.VisualSettings.ResumeLayout(false);
            this.VisualSettings.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.AdvancedSettings.ResumeLayout(false);
            this.AdvancedSettings.PerformLayout();
            this.HillshadeGroup.ResumeLayout(false);
            this.HillshadeGroup.PerformLayout();
            this.SurfaceGroup.ResumeLayout(false);
            this.SurfaceGroup.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void VisibleFrom_TextChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();

			double o;
			if (double.TryParse(VisibleFrom.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentUICulture, out o))
			{
				gld.GridScaleRange[0].MinScale = o;
				gld.GridScaleRange[0].MinScaleSpecified = true;
			}
			else
			{
				gld.GridScaleRange[0].MinScale = 0;
				gld.GridScaleRange[0].MinScaleSpecified = false;
			}
			m_editor.HasChanged();
		}

		private OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType FixupLayer()
		{
			if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return null;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType;
			if (gld.GridScaleRange == null)
				gld.GridScaleRange = new OSGeo.MapGuide.MaestroAPI.GridScaleRangeTypeCollection();
			if (gld.GridScaleRange.Count < 1)
				gld.GridScaleRange.Add(new OSGeo.MapGuide.MaestroAPI.GridScaleRangeType());
			if (gld.GridScaleRange[0].ColorStyle == null)
				gld.GridScaleRange[0].ColorStyle = new OSGeo.MapGuide.MaestroAPI.GridColorStyleType();
			if (gld.GridScaleRange[0].ColorStyle.ColorRule == null)
				gld.GridScaleRange[0].ColorStyle.ColorRule = new OSGeo.MapGuide.MaestroAPI.GridColorRuleTypeCollection();
			if (gld.GridScaleRange[0].ColorStyle.ColorRule.Count != 2)
			{
				gld.GridScaleRange[0].ColorStyle.ColorRule.Clear();
				gld.GridScaleRange[0].ColorStyle.ColorRule.Add(new OSGeo.MapGuide.MaestroAPI.GridColorRuleType());
				gld.GridScaleRange[0].ColorStyle.ColorRule.Add(new OSGeo.MapGuide.MaestroAPI.GridColorRuleType());
			}
			if (gld.GridScaleRange[0].ColorStyle.ColorRule[0].Color == null)
				gld.GridScaleRange[0].ColorStyle.ColorRule[0].Color = new OSGeo.MapGuide.MaestroAPI.GridColorType();
			if (gld.GridScaleRange[0].ColorStyle.ColorRule[1].Color == null)
				gld.GridScaleRange[0].ColorStyle.ColorRule[1].Color = new OSGeo.MapGuide.MaestroAPI.GridColorType();

			gld.GridScaleRange[0].ColorStyle.ColorRule[0].Color.ItemElementName = OSGeo.MapGuide.MaestroAPI.ItemChoiceType.ExplicitColor;
			gld.GridScaleRange[0].ColorStyle.ColorRule[1].Color.ItemElementName = OSGeo.MapGuide.MaestroAPI.ItemChoiceType.ExplicitColor;


			
			return gld;
		}

		private void VisibleTo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			VisibleTo_TextChanged(sender, e);
		}

		private void RebuildFactor_TextChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();
			double o;
			if (double.TryParse(RebuildFactor.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentUICulture, out o))
				gld.GridScaleRange[0].RebuildFactor = o;
			else
				gld.GridScaleRange[0].RebuildFactor = 1;
		
			m_editor.HasChanged();
		}


		private void ForegroundColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();
			gld.GridScaleRange[0].ColorStyle.ColorRule[0].Color.Item = OSGeo.MapGuide.MaestroAPI.Utility.SerializeHTMLColor(ForegroundColor.CurrentColor, false);

			m_editor.HasChanged();
		}

		private void BackgroundColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();
			gld.GridScaleRange[0].ColorStyle.ColorRule[1].Color.Item = OSGeo.MapGuide.MaestroAPI.Utility.SerializeHTMLColor(BackgroundColor.CurrentColor, false);

			m_editor.HasChanged();
		}

		private void VisibleTo_TextChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();

			double o;
			if (double.TryParse(VisibleTo.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentUICulture, out o))
			{
				gld.GridScaleRange[0].MaxScale = o;
				gld.GridScaleRange[0].MaxScaleSpecified = true;
			}
			else
			{
				gld.GridScaleRange[0].MaxScale = 0;
				gld.GridScaleRange[0].MaxScaleSpecified = false;
			}
			m_editor.HasChanged();
		}

		private void EnableAdvanced_CheckedChanged(object sender, System.EventArgs e)
		{
			AdvancedSettings.Enabled = EnableAdvanced.Checked;

			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();

			if (EnableAdvanced.Checked)
			{
				gld.GridScaleRange[0].ColorStyle.BrightnessFactorSpecified = true;
				gld.GridScaleRange[0].ColorStyle.ContrastFactorSpecified = true;
				BrightnessFactor_TextChanged(sender, e);
				ContrastFactor_TextChanged(sender, e);
				TransparencyColor_SelectedIndexChanged(sender, e);

				if (EnableHillshade.Tag != null && ((bool)EnableHillshade.Tag) == true)
					EnableHillshade.Checked = true;
				if (EnableSurface.Tag != null && ((bool)EnableSurface.Tag) == true)
					EnableSurface.Checked = true;
			}
			else
			{
				gld.GridScaleRange[0].ColorStyle.BrightnessFactorSpecified = false;
				gld.GridScaleRange[0].ColorStyle.ContrastFactorSpecified = false;
				gld.GridScaleRange[0].ColorStyle.TransparencyColor = null;

				EnableHillshade.Tag = EnableHillshade.Checked;
				EnableSurface.Tag = EnableSurface.Tag;

				EnableHillshade.Checked = false;
				EnableSurface.Checked = false;
			}
		}

		private void BrightnessFactor_TextChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();

			double o;
			if (double.TryParse(BrightnessFactor.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentUICulture, out o))
			{
				gld.GridScaleRange[0].ColorStyle.BrightnessFactor = o;
				gld.GridScaleRange[0].ColorStyle.BrightnessFactorSpecified = true;
			}
			else
			{
				gld.GridScaleRange[0].ColorStyle.BrightnessFactor = 0;
				gld.GridScaleRange[0].ColorStyle.BrightnessFactorSpecified = false;
			}
			m_editor.HasChanged();
		}

		private void ContrastFactor_TextChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();

			double o;
			if (double.TryParse(ContrastFactor.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentUICulture, out o))
			{
				gld.GridScaleRange[0].ColorStyle.ContrastFactor = o;
				gld.GridScaleRange[0].ColorStyle.ContrastFactorSpecified = true;
			}
			else
			{
				gld.GridScaleRange[0].ColorStyle.ContrastFactor = 0;
				gld.GridScaleRange[0].ColorStyle.ContrastFactorSpecified = false;
			}
			m_editor.HasChanged();
		}

		private void TransparencyColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();

			gld.GridScaleRange[0].ColorStyle.TransparencyColor = OSGeo.MapGuide.MaestroAPI.Utility.SerializeHTMLColor(TransparencyColor.CurrentColor, false);
			m_editor.HasChanged();
		}

		private void Altitude_TextChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();

			if (gld.GridScaleRange[0].ColorStyle.HillShade == null)
				gld.GridScaleRange[0].ColorStyle.HillShade = new OSGeo.MapGuide.MaestroAPI.HillShadeType();

			double o;
			if (double.TryParse(Altitude.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentUICulture, out o))
				gld.GridScaleRange[0].ColorStyle.HillShade.Altitude = o;
			else
				gld.GridScaleRange[0].ColorStyle.HillShade.Altitude = 0;

			m_editor.HasChanged();
		}

		private void Azimuth_TextChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();
			if (gld.GridScaleRange[0].ColorStyle.HillShade == null)
				gld.GridScaleRange[0].ColorStyle.HillShade = new OSGeo.MapGuide.MaestroAPI.HillShadeType();

			double o;
			if (double.TryParse(Azimuth.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentUICulture, out o))
				gld.GridScaleRange[0].ColorStyle.HillShade.Azimuth = o;
			else
				gld.GridScaleRange[0].ColorStyle.HillShade.Azimuth = 0;

			m_editor.HasChanged();
		}

		private void HillshadeScaleFactor_TextChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();
			if (gld.GridScaleRange[0].ColorStyle.HillShade == null)
				gld.GridScaleRange[0].ColorStyle.HillShade = new OSGeo.MapGuide.MaestroAPI.HillShadeType();

			double o;
			if (double.TryParse(HillshadeScaleFactor.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentUICulture, out o))
			{
				gld.GridScaleRange[0].ColorStyle.HillShade.ScaleFactor = o;
				gld.GridScaleRange[0].ColorStyle.HillShade.ScaleFactorSpecified = true;
			}
			else
			{
				gld.GridScaleRange[0].ColorStyle.HillShade.ScaleFactor = 0;
				gld.GridScaleRange[0].ColorStyle.HillShade.ScaleFactorSpecified = false;
			}

			m_editor.HasChanged();
		}

		private void SurfaceBand_TextChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();
			if (gld.GridScaleRange[0].SurfaceStyle == null)
				gld.GridScaleRange[0].SurfaceStyle = new OSGeo.MapGuide.MaestroAPI.GridSurfaceStyleType();

			gld.GridScaleRange[0].SurfaceStyle.Band = SurfaceBand.Text;

			m_editor.HasChanged();
		}

		private void ZeroValue_TextChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();
			if (gld.GridScaleRange[0].SurfaceStyle == null)
				gld.GridScaleRange[0].SurfaceStyle = new OSGeo.MapGuide.MaestroAPI.GridSurfaceStyleType();

			double o;
			if (double.TryParse(ZeroValue.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentUICulture, out o))
			{
				gld.GridScaleRange[0].SurfaceStyle.ZeroValue = o;
				gld.GridScaleRange[0].SurfaceStyle.ZeroValueSpecified = true;
			}
			else
			{
				gld.GridScaleRange[0].SurfaceStyle.ZeroValue = 0;
				gld.GridScaleRange[0].SurfaceStyle.ZeroValueSpecified = false;
			}

			m_editor.HasChanged();
		}

		private void HillshadeBand_TextChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();
			if (gld.GridScaleRange[0].ColorStyle.HillShade == null)
				gld.GridScaleRange[0].ColorStyle.HillShade = new OSGeo.MapGuide.MaestroAPI.HillShadeType();

			gld.GridScaleRange[0].ColorStyle.HillShade.Band = HillshadeBand.Text;
		}

		private void DefaultColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();
			if (gld.GridScaleRange[0].SurfaceStyle == null)
				gld.GridScaleRange[0].SurfaceStyle = new OSGeo.MapGuide.MaestroAPI.GridSurfaceStyleType();

			gld.GridScaleRange[0].SurfaceStyle.DefaultColor = OSGeo.MapGuide.MaestroAPI.Utility.SerializeHTMLColor(DefaultColor.CurrentColor, false);
		}

		private void SurfaceScaleFactor_TextChanged(object sender, System.EventArgs e)
		{
			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();
			if (gld.GridScaleRange[0].SurfaceStyle == null)
				gld.GridScaleRange[0].SurfaceStyle = new OSGeo.MapGuide.MaestroAPI.GridSurfaceStyleType();

			double o;
			if (double.TryParse(SurfaceScaleFactor.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentUICulture, out o))
			{
				gld.GridScaleRange[0].SurfaceStyle.ScaleFactor = o;
				gld.GridScaleRange[0].SurfaceStyle.ScaleFactorSpecified = true;
			}
			else
			{
				gld.GridScaleRange[0].SurfaceStyle.ScaleFactor = 0;
				gld.GridScaleRange[0].SurfaceStyle.ScaleFactorSpecified = false;
			}

			m_editor.HasChanged();
		}

		private void EnableHillshade_CheckedChanged(object sender, System.EventArgs e)
		{
			HillshadeGroup.Enabled = EnableHillshade.Checked;

			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();
			if (HillshadeGroup.Enabled)
			{
				gld.GridScaleRange[0].ColorStyle.HillShade = HillshadeGroup.Tag as OSGeo.MapGuide.MaestroAPI.HillShadeType;
				if (gld.GridScaleRange[0].ColorStyle.HillShade == null)
					gld.GridScaleRange[0].ColorStyle.HillShade = new OSGeo.MapGuide.MaestroAPI.HillShadeType();
				
				gld.GridScaleRange[0].ColorStyle.HillShade.ScaleFactorSpecified = true;
				gld.GridScaleRange[0].ColorStyle.HillShade.Band = "";
			}
			else
			{
				HillshadeGroup.Tag = gld.GridScaleRange[0].ColorStyle.HillShade;
				gld.GridScaleRange[0].ColorStyle.HillShade = null;
			}
				

		}

		private void EnableSurface_CheckedChanged(object sender, System.EventArgs e)
		{
			SurfaceGroup.Enabled = EnableSurface.Checked;

			if (m_layer == null || inUpdate || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType gld =  FixupLayer();
			if (SurfaceGroup.Enabled)
			{
				gld.GridScaleRange[0].SurfaceStyle = SurfaceGroup.Tag as OSGeo.MapGuide.MaestroAPI.GridSurfaceStyleType;
				if (gld.GridScaleRange[0].SurfaceStyle == null)
					gld.GridScaleRange[0].SurfaceStyle = new OSGeo.MapGuide.MaestroAPI.GridSurfaceStyleType();
				
				gld.GridScaleRange[0].SurfaceStyle.ScaleFactorSpecified = true;
				gld.GridScaleRange[0].SurfaceStyle.Band = "";
				gld.GridScaleRange[0].SurfaceStyle.ZeroValueSpecified = true;
				if (DefaultColor.SelectedIndex < 0)
					DefaultColor.CurrentColor = Color.White;
				gld.GridScaleRange[0].SurfaceStyle.DefaultColor = OSGeo.MapGuide.MaestroAPI.Utility.SerializeHTMLColor(DefaultColor.CurrentColor, false);
			}
			else
			{
				SurfaceGroup.Tag = gld.GridScaleRange[0].SurfaceStyle;
				gld.GridScaleRange[0].SurfaceStyle = null;
			}
		
		}

		private void schemaSelector_GeometryChanged(bool fromUser, string geom)
		{
			if (m_layer == null || m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType == null)
				return;

			(m_layer.Item as OSGeo.MapGuide.MaestroAPI.GridLayerDefinitionType).Geometry = geom;
		
		}


	}
}
