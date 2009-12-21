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
        private DataSet MeasureUnitsDataset;
        private DataTable MeasureUnitTable;
        private DataColumn dataColumn3;
        private DataColumn dataColumn4;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Buffer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            BorderColor.ResetColors();
            FillColor.ResetColors();

            //Fill dataset
            using (System.IO.StringReader sr = new System.IO.StringReader(Properties.Resources.MeasureUnitsDataset))
                MeasureUnitTable.ReadXml(sr);
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
				BufferDistanceInput.Text = GetSettingValue("BufferDistanceInput");
				LayerName.Text = GetSettingValue("LayerName");
				FillColorInput.Text = GetSettingValue("FillColorInput");
				BufferUnitsInput.Text = GetSettingValue("BufferUnitsInput");
				BorderColorInput.Text = GetSettingValue("BorderColorInput"); 
				LayerNameInput.Text = GetSettingValue("LayerNameInput");
				BufferDistance.Text = GetSettingValue("BufferDistance");

                BufferUnits.SelectedIndex = -1;
                BufferUnits.SelectedValue = GetSettingValue("BufferUnits");

                if (BufferUnits.SelectedIndex == -1)
                    BufferUnits.Text = GetSettingValue("BufferUnits");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Buffer));
            this.label1 = new System.Windows.Forms.Label();
            this.BufferUnits = new System.Windows.Forms.ComboBox();
            this.MeasureUnitTable = new System.Data.DataTable();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
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
            this.MeasureUnitsDataset = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureUnitTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureUnitsDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // BufferUnits
            // 
            resources.ApplyResources(this.BufferUnits, "BufferUnits");
            this.BufferUnits.DataSource = this.MeasureUnitTable;
            this.BufferUnits.DisplayMember = "Displayname";
            this.BufferUnits.Name = "BufferUnits";
            this.BufferUnits.ValueMember = "Value";
            this.BufferUnits.SelectedIndexChanged += new System.EventHandler(this.BufferUnits_TextChanged);
            this.BufferUnits.TextChanged += new System.EventHandler(this.BufferUnits_TextChanged);
            // 
            // MeasureUnitTable
            // 
            this.MeasureUnitTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn3,
            this.dataColumn4});
            this.MeasureUnitTable.TableName = "MeasureUnit";
            // 
            // dataColumn3
            // 
            this.dataColumn3.Caption = "Value";
            this.dataColumn3.ColumnName = "Value";
            // 
            // dataColumn4
            // 
            this.dataColumn4.Caption = "DisplayName";
            this.dataColumn4.ColumnName = "Displayname";
            // 
            // BorderColor
            // 
            resources.ApplyResources(this.BorderColor, "BorderColor");
            this.BorderColor.Name = "BorderColor";
            this.BorderColor.SelectedIndexChanged += new System.EventHandler(this.BorderColor_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // BufferDistanceInput
            // 
            resources.ApplyResources(this.BufferDistanceInput, "BufferDistanceInput");
            this.BufferDistanceInput.Name = "BufferDistanceInput";
            this.BufferDistanceInput.TextChanged += new System.EventHandler(this.BufferDistanceInput_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // FillColor
            // 
            resources.ApplyResources(this.FillColor, "FillColor");
            this.FillColor.Name = "FillColor";
            this.FillColor.SelectedIndexChanged += new System.EventHandler(this.FillColor_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // LayerName
            // 
            resources.ApplyResources(this.LayerName, "LayerName");
            this.LayerName.Name = "LayerName";
            this.LayerName.TextChanged += new System.EventHandler(this.LayerName_TextChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // FillColorInput
            // 
            resources.ApplyResources(this.FillColorInput, "FillColorInput");
            this.FillColorInput.Name = "FillColorInput";
            this.FillColorInput.TextChanged += new System.EventHandler(this.FillColorInput_TextChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // BufferUnitsInput
            // 
            resources.ApplyResources(this.BufferUnitsInput, "BufferUnitsInput");
            this.BufferUnitsInput.Name = "BufferUnitsInput";
            this.BufferUnitsInput.TextChanged += new System.EventHandler(this.BufferUnitsInput_TextChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // BorderColorInput
            // 
            resources.ApplyResources(this.BorderColorInput, "BorderColorInput");
            this.BorderColorInput.Name = "BorderColorInput";
            this.BorderColorInput.TextChanged += new System.EventHandler(this.BorderColorInput_TextChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // LayerNameInput
            // 
            resources.ApplyResources(this.LayerNameInput, "LayerNameInput");
            this.LayerNameInput.Name = "LayerNameInput";
            this.LayerNameInput.TextChanged += new System.EventHandler(this.LayerNameInput_TextChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // BufferDistance
            // 
            resources.ApplyResources(this.BufferDistance, "BufferDistance");
            this.BufferDistance.Name = "BufferDistance";
            this.BufferDistance.TextChanged += new System.EventHandler(this.BufferDistance_TextChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // MeasureUnitsDataset
            // 
            this.MeasureUnitsDataset.DataSetName = "NewDataSet";
            this.MeasureUnitsDataset.Locale = new System.Globalization.CultureInfo("da-DK");
            this.MeasureUnitsDataset.Tables.AddRange(new System.Data.DataTable[] {
            this.MeasureUnitTable});
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
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.MeasureUnitTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureUnitsDataset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void BufferUnits_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

            if (BufferUnits.SelectedValue != null)
                SetSettingValue("BufferUnits", (string)BufferUnits.SelectedValue);
            else
            {
                foreach (System.Data.DataRow r in MeasureUnitTable.Rows)
                    if (string.Equals((string)r["Displayname"], BufferUnits.Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        SetSettingValue("BufferUnits", (string)r["Value"]);
                        return;
                    }

                SetSettingValue("BufferUnits", BufferUnits.Text);
            }
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
