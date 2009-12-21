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
	public class ExtentHistory : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox Direction;
        private System.Data.DataSet DirectionTypesDataset;
        private System.Data.DataTable DirectionUnitTable;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
		private System.ComponentModel.IContainer components = null;

		public ExtentHistory()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

            //Fill dataset
            using (System.IO.StringReader sr = new System.IO.StringReader(Properties.Resources.DirectionTypeDataset))
                DirectionTypesDataset.ReadXml(sr);
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

                Direction.SelectedIndex = -1;
                Direction.SelectedValue = GetSettingValue("Direction");

                if (Direction.SelectedIndex == -1)
                    Direction.Text = GetSettingValue("Direction");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtentHistory));
            this.label1 = new System.Windows.Forms.Label();
            this.Direction = new System.Windows.Forms.ComboBox();
            this.DirectionUnitTable = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.DirectionTypesDataset = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.DirectionUnitTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DirectionTypesDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Direction
            // 
            resources.ApplyResources(this.Direction, "Direction");
            this.Direction.DataSource = this.DirectionUnitTable;
            this.Direction.DisplayMember = "DisplayName";
            this.Direction.Name = "Direction";
            this.Direction.ValueMember = "Value";
            this.Direction.SelectedValueChanged += new System.EventHandler(this.Direction_TextChanged);
            this.Direction.TextChanged += new System.EventHandler(this.Direction_TextChanged);
            // 
            // DirectionUnitTable
            // 
            this.DirectionUnitTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2});
            this.DirectionUnitTable.TableName = "DirectionUnit";
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
            // DirectionTypesDataset
            // 
            this.DirectionTypesDataset.DataSetName = "NewDataSet";
            this.DirectionTypesDataset.Locale = new System.Globalization.CultureInfo("da-DK");
            this.DirectionTypesDataset.Tables.AddRange(new System.Data.DataTable[] {
            this.DirectionUnitTable});
            // 
            // ExtentHistory
            // 
            this.Controls.Add(this.Direction);
            this.Controls.Add(this.label1);
            this.Name = "ExtentHistory";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.DirectionUnitTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DirectionTypesDataset)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


		private void Direction_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

            if (Direction.SelectedValue != null)
                SetSettingValue("Direction", (string)Direction.SelectedValue);
            else
            {
                foreach(System.Data.DataRow r in DirectionUnitTable.Rows)
                    if (string.Equals((string)r["Displayname"], Direction.Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        SetSettingValue("Direction", (string)r["Value"]);
                        return;
                    }

                SetSettingValue("Direction", Direction.Text);
            }
        }
	}
}

