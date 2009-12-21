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
        private System.Data.DataSet MeasureTypeDataset;
        private System.Data.DataTable MeasureTypeTable;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataSet MeasureUnitsDataset;
        private System.Data.DataTable MeasureUnitTable;
        private System.Data.DataColumn dataColumn3;
        private System.Data.DataColumn dataColumn4;
        private System.Data.DataSet TooltipTypeDataset;
        private System.Data.DataTable TooltipTypeTable;
        private System.Data.DataColumn dataColumn5;
        private System.Data.DataColumn dataColumn6;
		private System.ComponentModel.IContainer components = null;

		public Measure()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

            //Fill datasets
            using (System.IO.StringReader sr = new System.IO.StringReader(Properties.Resources.MeasureTypeDataset))
                MeasureTypeDataset.ReadXml(sr);
            using (System.IO.StringReader sr = new System.IO.StringReader(Properties.Resources.MeasureUnitsDataset))
                MeasureUnitTable.ReadXml(sr);
            using (System.IO.StringReader sr = new System.IO.StringReader(Properties.Resources.TooltipTypeDataset))
                TooltipTypeDataset.ReadXml(sr);
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
				Units.Text = GetSettingValue("Units");

                Type.SelectedIndex = -1;
                Type.SelectedValue = GetSettingValue("Type");

                if (Type.SelectedIndex == -1)
                    Type.Text = GetSettingValue("Type");

                Units.SelectedIndex = -1;
                Units.SelectedValue = GetSettingValue("Units");

                if (Units.SelectedIndex == -1)
                    Units.Text = GetSettingValue("Units");

                MeasureTooltipType.SelectedIndex = -1;
                MeasureTooltipType.SelectedValue = GetSettingValue("MeasureTooltipType");

                if (MeasureTooltipType.SelectedIndex == -1)
                    MeasureTooltipType.Text = GetSettingValue("MeasureTooltipType");
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
            this.MeasureTypeTable = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.MeasureTooltipType = new System.Windows.Forms.ComboBox();
            this.TooltipTypeTable = new System.Data.DataTable();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.Units = new System.Windows.Forms.ComboBox();
            this.MeasureUnitTable = new System.Data.DataTable();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.MeasureTypeDataset = new System.Data.DataSet();
            this.MeasureUnitsDataset = new System.Data.DataSet();
            this.TooltipTypeDataset = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureTypeTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TooltipTypeTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureUnitTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureTypeDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureUnitsDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TooltipTypeDataset)).BeginInit();
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
            this.Type.DataSource = this.MeasureTypeTable;
            this.Type.DisplayMember = "Displayname";
            this.Type.Name = "Type";
            this.Type.ValueMember = "Value";
            this.Type.SelectedIndexChanged += new System.EventHandler(this.Type_TextChanged);
            this.Type.TextChanged += new System.EventHandler(this.Type_TextChanged);
            // 
            // MeasureTypeTable
            // 
            this.MeasureTypeTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2});
            this.MeasureTypeTable.TableName = "MeasureType";
            // 
            // dataColumn1
            // 
            this.dataColumn1.Caption = "Value";
            this.dataColumn1.ColumnName = "Value";
            // 
            // dataColumn2
            // 
            this.dataColumn2.Caption = "DisplayName";
            this.dataColumn2.ColumnName = "Displayname";
            // 
            // MeasureTooltipType
            // 
            resources.ApplyResources(this.MeasureTooltipType, "MeasureTooltipType");
            this.MeasureTooltipType.DataSource = this.TooltipTypeTable;
            this.MeasureTooltipType.DisplayMember = "Displayname";
            this.MeasureTooltipType.Name = "MeasureTooltipType";
            this.MeasureTooltipType.ValueMember = "Value";
            this.MeasureTooltipType.SelectedIndexChanged += new System.EventHandler(this.MeasureTooltipType_TextChanged);
            this.MeasureTooltipType.TextChanged += new System.EventHandler(this.MeasureTooltipType_TextChanged);
            // 
            // TooltipTypeTable
            // 
            this.TooltipTypeTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn5,
            this.dataColumn6});
            this.TooltipTypeTable.TableName = "TooltipType";
            // 
            // dataColumn5
            // 
            this.dataColumn5.Caption = "Value";
            this.dataColumn5.ColumnName = "Value";
            // 
            // dataColumn6
            // 
            this.dataColumn6.Caption = "DisplayName";
            this.dataColumn6.ColumnName = "Displayname";
            // 
            // Units
            // 
            resources.ApplyResources(this.Units, "Units");
            this.Units.DataSource = this.MeasureUnitTable;
            this.Units.DisplayMember = "Displayname";
            this.Units.Name = "Units";
            this.Units.ValueMember = "Value";
            this.Units.SelectedIndexChanged += new System.EventHandler(this.Units_TextChanged);
            this.Units.TextChanged += new System.EventHandler(this.Units_TextChanged);
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
            // MeasureTypeDataset
            // 
            this.MeasureTypeDataset.DataSetName = "NewDataSet";
            this.MeasureTypeDataset.Locale = new System.Globalization.CultureInfo("da-DK");
            this.MeasureTypeDataset.Tables.AddRange(new System.Data.DataTable[] {
            this.MeasureTypeTable});
            // 
            // MeasureUnitsDataset
            // 
            this.MeasureUnitsDataset.DataSetName = "NewDataSet";
            this.MeasureUnitsDataset.Locale = new System.Globalization.CultureInfo("da-DK");
            this.MeasureUnitsDataset.Tables.AddRange(new System.Data.DataTable[] {
            this.MeasureUnitTable});
            // 
            // TooltipTypeDataset
            // 
            this.TooltipTypeDataset.DataSetName = "NewDataSet";
            this.TooltipTypeDataset.Locale = new System.Globalization.CultureInfo("da-DK");
            this.TooltipTypeDataset.Tables.AddRange(new System.Data.DataTable[] {
            this.TooltipTypeTable});
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
            ((System.ComponentModel.ISupportInitialize)(this.MeasureTypeTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TooltipTypeTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureUnitTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureTypeDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureUnitsDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TooltipTypeDataset)).EndInit();
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

            if (MeasureTooltipType.SelectedValue != null)
                SetSettingValue("MeasureTooltipType", (string)MeasureTooltipType.SelectedValue);
            else
            {
                foreach (System.Data.DataRow r in TooltipTypeTable.Rows)
                    if (string.Equals((string)r["Displayname"], MeasureTooltipType.Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        SetSettingValue("MeasureTooltipType", (string)r["Value"]);
                        return;
                    }

                SetSettingValue("MeasureTooltipType", MeasureTooltipType.Text);
            }
		}

		private void Type_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

            if (Type.SelectedValue != null)
                SetSettingValue("Type", (string)Type.SelectedValue);
            else
            {
                foreach (System.Data.DataRow r in MeasureTypeTable.Rows)
                    if (string.Equals((string)r["Displayname"], Type.Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        SetSettingValue("Type", (string)r["Value"]);
                        return;
                    }

                SetSettingValue("Type", Type.Text);
            }
        }

		private void Units_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

            if (Units.SelectedValue != null)
                SetSettingValue("Units", (string)Units.SelectedValue);
            else
            {
                foreach (System.Data.DataRow r in MeasureUnitTable.Rows)
                    if (string.Equals((string)r["Displayname"], Units.Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        SetSettingValue("Units", (string)r["Value"]);
                        return;
                    }

                SetSettingValue("Units", Units.Text);
            }
		}
	}
}

