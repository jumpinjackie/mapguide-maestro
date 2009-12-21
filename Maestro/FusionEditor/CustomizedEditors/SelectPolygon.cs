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
	public class SelectPolygon : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.ComboBox SelectionType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox Tolerance;
		private System.Windows.Forms.Label label1;
        private System.Data.DataSet GeometryOperationTypeDataset;
        private System.Data.DataTable GeometryOperationTypeTable;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
		private System.ComponentModel.IContainer components = null;

		public SelectPolygon()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

            //Fill dataset
            using (System.IO.StringReader sr = new System.IO.StringReader(Properties.Resources.GeometryOperationTypeDataset))
                GeometryOperationTypeDataset.ReadXml(sr);
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

                SelectionType.SelectedIndex = -1;
                SelectionType.SelectedValue = GetSettingValue("SelectionType");

                if (SelectionType.SelectedIndex == -1)
                    SelectionType.Text = GetSettingValue("SelectionType");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectPolygon));
            this.SelectionType = new System.Windows.Forms.ComboBox();
            this.GeometryOperationTypeTable = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.Tolerance = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GeometryOperationTypeDataset = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.GeometryOperationTypeTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GeometryOperationTypeDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // SelectionType
            // 
            resources.ApplyResources(this.SelectionType, "SelectionType");
            this.SelectionType.DataSource = this.GeometryOperationTypeTable;
            this.SelectionType.DisplayMember = "Displayname";
            this.SelectionType.Name = "SelectionType";
            this.SelectionType.ValueMember = "Value";
            this.SelectionType.SelectedIndexChanged += new System.EventHandler(this.SelectionType_TextChanged);
            this.SelectionType.TextChanged += new System.EventHandler(this.SelectionType_TextChanged);
            // 
            // GeometryOperationTypeTable
            // 
            this.GeometryOperationTypeTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2});
            this.GeometryOperationTypeTable.TableName = "GeometryOperationType";
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
            // GeometryOperationTypeDataset
            // 
            this.GeometryOperationTypeDataset.DataSetName = "NewDataSet";
            this.GeometryOperationTypeDataset.Locale = new System.Globalization.CultureInfo("da-DK");
            this.GeometryOperationTypeDataset.Tables.AddRange(new System.Data.DataTable[] {
            this.GeometryOperationTypeTable});
            // 
            // SelectPolygon
            // 
            this.Controls.Add(this.SelectionType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Tolerance);
            this.Controls.Add(this.label1);
            this.Name = "SelectPolygon";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.GeometryOperationTypeTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GeometryOperationTypeDataset)).EndInit();
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

            if (SelectionType.SelectedValue != null)
                SetSettingValue("SelectionType", (string)SelectionType.SelectedValue);
            else
            {
                foreach (System.Data.DataRow r in GeometryOperationTypeTable.Rows)
                    if (string.Equals((string)r["Displayname"], SelectionType.Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        SetSettingValue("SelectionType", (string)r["Value"]);
                        return;
                    }

                SetSettingValue("SelectionType", SelectionType.Text);
            }
        }
	}
}

