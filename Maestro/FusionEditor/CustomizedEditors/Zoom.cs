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
	public class Zoom : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.TextBox Tolerance;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox Factor;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox Direction;
		private System.Windows.Forms.Label label1;
        private System.Data.DataSet ZoomDirectionDataset;
        private System.Data.DataTable ZoomDirectionTable;
        private System.Data.DataColumn dataColumn5;
        private System.Data.DataColumn dataColumn6;
		private System.ComponentModel.IContainer components = null;

		public Zoom()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

            //Fill dataset
            using (System.IO.StringReader sr = new System.IO.StringReader(Properties.Resources.ZoomDirectionDataset))
                ZoomDirectionDataset.ReadXml(sr);
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
				Factor.Text = GetSettingValue("Factor");

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Zoom));
            this.Tolerance = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Factor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Direction = new System.Windows.Forms.ComboBox();
            this.ZoomDirectionTable = new System.Data.DataTable();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.ZoomDirectionDataset = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomDirectionTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomDirectionDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // Tolerance
            // 
            resources.ApplyResources(this.Tolerance, "Tolerance");
            this.Tolerance.Name = "Tolerance";
            this.Tolerance.TextChanged += new System.EventHandler(this.Tolerance_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // Factor
            // 
            resources.ApplyResources(this.Factor, "Factor");
            this.Factor.Name = "Factor";
            this.Factor.TextChanged += new System.EventHandler(this.Factor_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Direction
            // 
            resources.ApplyResources(this.Direction, "Direction");
            this.Direction.DataSource = this.ZoomDirectionTable;
            this.Direction.DisplayMember = "Displayname";
            this.Direction.Name = "Direction";
            this.Direction.ValueMember = "Value";
            this.Direction.SelectedIndexChanged += new System.EventHandler(this.Direction_TextChanged);
            this.Direction.TextChanged += new System.EventHandler(this.Direction_TextChanged);
            // 
            // ZoomDirectionTable
            // 
            this.ZoomDirectionTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn5,
            this.dataColumn6});
            this.ZoomDirectionTable.TableName = "ZoomDirection";
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ZoomDirectionDataset
            // 
            this.ZoomDirectionDataset.DataSetName = "NewDataSet";
            this.ZoomDirectionDataset.Locale = new System.Globalization.CultureInfo("da-DK");
            this.ZoomDirectionDataset.Tables.AddRange(new System.Data.DataTable[] {
            this.ZoomDirectionTable});
            // 
            // Zoom
            // 
            this.Controls.Add(this.Tolerance);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Factor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Direction);
            this.Controls.Add(this.label1);
            this.Name = "Zoom";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.ZoomDirectionTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomDirectionDataset)).EndInit();
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

		private void Factor_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("Factor", Factor.Text);
		}

		private void Direction_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

            if (Direction.SelectedValue != null)
                SetSettingValue("Direction", (string)Direction.SelectedValue);
            else
            {
                foreach (System.Data.DataRow r in ZoomDirectionTable.Rows)
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

