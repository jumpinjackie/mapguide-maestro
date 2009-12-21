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
	public class ViewSize : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox Units;
		private System.Windows.Forms.TextBox Precision;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox Template;
		private System.Windows.Forms.Label label2;
        private System.Data.DataSet MeasureUnitsDataset;
        private System.Data.DataTable MeasureUnitTable;
        private System.Data.DataColumn dataColumn3;
        private System.Data.DataColumn dataColumn4;
		private System.ComponentModel.IContainer components = null;

		public ViewSize()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

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

				Precision.Text = GetSettingValue("Precision"); 
				Template.Text = GetSettingValue("Template");
                Units.SelectedIndex = -1;
                Units.SelectedValue = GetSettingValue("Units");

                if (Units.SelectedIndex == -1)
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewSize));
            this.Units = new System.Windows.Forms.ComboBox();
            this.MeasureUnitTable = new System.Data.DataTable();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.Precision = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Template = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MeasureUnitsDataset = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureUnitTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureUnitsDataset)).BeginInit();
            this.SuspendLayout();
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Precision
            // 
            resources.ApplyResources(this.Precision, "Precision");
            this.Precision.Name = "Precision";
            this.Precision.TextChanged += new System.EventHandler(this.Precision_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // Template
            // 
            resources.ApplyResources(this.Template, "Template");
            this.Template.Name = "Template";
            this.Template.TextChanged += new System.EventHandler(this.Template_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // MeasureUnitsDataset
            // 
            this.MeasureUnitsDataset.DataSetName = "NewDataSet";
            this.MeasureUnitsDataset.Locale = new System.Globalization.CultureInfo("da-DK");
            this.MeasureUnitsDataset.Tables.AddRange(new System.Data.DataTable[] {
            this.MeasureUnitTable});
            // 
            // ViewSize
            // 
            this.Controls.Add(this.Precision);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Template);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Units);
            this.Controls.Add(this.label1);
            this.Name = "ViewSize";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.MeasureUnitTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureUnitsDataset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void Precision_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Precision", Precision.Text);
		}

		private void Template_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Template", Template.Text);
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

