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
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.OGR
{
	/// <summary>
	/// Summary description for ODBC.
	/// </summary>
	public class ODBC : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox DSN;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.DataGrid TableGrid;
		private System.Data.DataSet TableDataSet;
		private System.Data.DataTable TableNames;
		private System.Data.DataColumn TablenameColumn;
		private System.Data.DataColumn GeometryColumn;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
		private System.ComponentModel.IContainer components;

		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_item;
		private bool m_isUpdating = false;
		private System.Windows.Forms.TextBox SpatialIDColumn;
		private System.Windows.Forms.TextBox SpatialTablename;
		private System.Windows.Forms.TextBox SpatialTextColumn;
		private EditorInterface m_editor;
		private ResourceEditors.FeatureSourceEditors.ODBC.Credentials credentials;
		private bool m_hasChanged = false;

		private string m_username;
		private string m_password;

		public void SetItem(ResourceEditors.EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource item)
		{
			m_editor = editor;
			m_item = item;
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			if (m_item == null)
				return;

			try
			{
				m_isUpdating = true;
				string connectionstring = m_item.Parameter["DataSource"];
				if (connectionstring == null)
					connectionstring = "";

				if (!connectionstring.StartsWith("ODBC:"))
					connectionstring = "";
				else
					connectionstring = connectionstring.Substring("ODBC:".Length);

				if (connectionstring.IndexOf("@") > 0)
				{
					string usernamepassword = connectionstring.Substring(0, connectionstring.IndexOf("@"));
					connectionstring = connectionstring.Substring(usernamepassword.Length + 1);
					string[] parts = usernamepassword.Split('/');
					if (parts.Length > 1)
					{
						m_username = parts[0];
						m_password = parts[1];
					}
					else
					{
						m_username = "";
						m_password = "";
					}
				}

				credentials.SetCredentials(m_username, m_password);

				if (connectionstring.IndexOf(":") > 0)
				{
					string tmp = connectionstring.Substring(0, connectionstring.IndexOf(":"));
					string spt = connectionstring.Substring(tmp.Length + 1);
					connectionstring = tmp;

					int i1 = spt.IndexOf('(');
					int i2 = spt.LastIndexOf(')');

					if (i2 > i1 && i1 >= 0)
					{
						SpatialTablename.Text = spt.Substring(0, i1);
						string columnnames = spt.Substring(i1 + 1, i2 - i1 - 1);
						string[] parts = columnnames.Split(',');
						if (parts.Length > 1)
						{
							SpatialIDColumn.Text = parts[0];
							SpatialTextColumn.Text = parts[1];
						}
						else
						{
							SpatialIDColumn.Text = columnnames;
							SpatialTextColumn.Text = "";
						}
					}
					else
					{
						SpatialTablename.Text = spt;
						SpatialIDColumn.Text = "";
						SpatialTextColumn.Text = "";
					}
				}
				else
				{
					SpatialTablename.Text = "";
					SpatialIDColumn.Text = "";
					SpatialTextColumn.Text = "";
				}


				if (connectionstring.IndexOf(",") > 0)
				{
					DSN.Text = connectionstring.Substring(0, connectionstring.IndexOf(","));
					connectionstring = connectionstring.Substring(DSN.Text.Length + 1);
				}
				else 
				{
					DSN.Text = connectionstring;
					connectionstring = "";
				}

               
				string[] tables = connectionstring.Split(',');
				TableNames.Rows.Clear();

				for(int i = 0; i < tables.Length; i++)
				{
					if (tables[i].Trim().Length == 0)
						continue;

					int i1 = tables[i].IndexOf('(');
					int i2 = tables[i].LastIndexOf(')');

					if (i2 > i1 && i1 >= 0)
					{
						string tablename = tables[i].Substring(0, i1);
						string columnname = tables[i].Substring(i1 + 1, i2 - i1 - 1);
						TableNames.Rows.Add(new object[] {tablename, columnname});
					}
					else
					{
						TableNames.Rows.Add(new object[] {tables[i], ""});
					}
				}

			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private void UpdateConnectionString()
		{
			if (m_item == null) 
				return;

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("ODBC:");
			sb.Append(m_username);
			sb.Append("/");
			sb.Append(m_password);
			sb.Append("@");
			sb.Append(DSN.Text);
			
			if (TableNames.Rows.Count > 0)
				sb.Append(",");

			foreach(DataRow dr in TableNames.Rows)
			{
				sb.Append((string)dr[0]);
				sb.Append("(");
				sb.Append((string)dr[1]);
				sb.Append("),");
			}

			sb.Length--;
			sb.Append(":");
			sb.Append(SpatialTablename.Text);
			sb.Append("(");
			sb.Append(SpatialIDColumn.Text);
			sb.Append(",");
			sb.Append(SpatialTextColumn.Text);
			sb.Append(")");

			m_item.Parameter["DataSource"] = sb.ToString();
		}

		private string GetDefaultValue(string item, string defaultValue)
		{
			if (item != null && item.Trim().Length > 0)
				return item;
			else
				return defaultValue;
		}

		public ODBC()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			TableNames.RowChanged += new DataRowChangeEventHandler(TableNames_RowChanged);
			TableNames.RowDeleted += new DataRowChangeEventHandler(TableNames_RowDeleted);
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ODBC));
            this.DSN = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SpatialTextColumn = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SpatialIDColumn = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SpatialTablename = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TableGrid = new System.Windows.Forms.DataGrid();
            this.TableDataSet = new System.Data.DataSet();
            this.TableNames = new System.Data.DataTable();
            this.TablenameColumn = new System.Data.DataColumn();
            this.GeometryColumn = new System.Data.DataColumn();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.credentials = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Credentials();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TableGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TableDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TableNames)).BeginInit();
            this.SuspendLayout();
            // 
            // DSN
            // 
            resources.ApplyResources(this.DSN, "DSN");
            this.DSN.Name = "DSN";
            this.DSN.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.SpatialTextColumn);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.SpatialIDColumn);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.SpatialTablename);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // SpatialTextColumn
            // 
            resources.ApplyResources(this.SpatialTextColumn, "SpatialTextColumn");
            this.SpatialTextColumn.Name = "SpatialTextColumn";
            this.SpatialTextColumn.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // SpatialIDColumn
            // 
            resources.ApplyResources(this.SpatialIDColumn, "SpatialIDColumn");
            this.SpatialIDColumn.Name = "SpatialIDColumn";
            this.SpatialIDColumn.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // SpatialTablename
            // 
            resources.ApplyResources(this.SpatialTablename, "SpatialTablename");
            this.SpatialTablename.Name = "SpatialTablename";
            this.SpatialTablename.TextChanged += new System.EventHandler(this.SomeProperty_Change);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.TableGrid);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // TableGrid
            // 
            this.TableGrid.CaptionVisible = false;
            this.TableGrid.DataMember = "Names";
            this.TableGrid.DataSource = this.TableDataSet;
            resources.ApplyResources(this.TableGrid, "TableGrid");
            this.TableGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.TableGrid.Name = "TableGrid";
            this.TableGrid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            this.TableGrid.Enter += new System.EventHandler(this.TableGrid_Enter);
            this.TableGrid.Leave += new System.EventHandler(this.TableGrid_Leave);
            // 
            // TableDataSet
            // 
            this.TableDataSet.DataSetName = "TableNames";
            this.TableDataSet.Locale = new System.Globalization.CultureInfo("da-DK");
            this.TableDataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.TableNames});
            // 
            // TableNames
            // 
            this.TableNames.Columns.AddRange(new System.Data.DataColumn[] {
            this.TablenameColumn,
            this.GeometryColumn});
            this.TableNames.TableName = "Names";
            // 
            // TablenameColumn
            // 
            this.TablenameColumn.Caption = "Tablename";
            this.TablenameColumn.ColumnName = "Table";
            // 
            // GeometryColumn
            // 
            this.GeometryColumn.Caption = "Geometry column";
            this.GeometryColumn.ColumnName = "Geometry";
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.DataGrid = this.TableGrid;
            this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dataGridTextBoxColumn1,
            this.dataGridTextBoxColumn2});
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle1.MappingName = "Names";
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            resources.ApplyResources(this.dataGridTextBoxColumn1, "dataGridTextBoxColumn1");
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            resources.ApplyResources(this.dataGridTextBoxColumn2, "dataGridTextBoxColumn2");
            // 
            // credentials
            // 
            resources.ApplyResources(this.credentials, "credentials");
            this.credentials.Name = "credentials";
            this.credentials.CredentialsChanged += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceEditors.ODBC.Credentials.CredentialsChangedDelegate(this.credentials_CredentialsChanged);
            // 
            // ODBC
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.credentials);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.DSN);
            this.Controls.Add(this.label1);
            this.Name = "ODBC";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TableGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TableDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TableNames)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void SomeProperty_Change(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_item == null)
				return;

			if (m_item.Parameter == null)
				m_item.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			UpdateConnectionString();
			m_editor.HasChanged();
		
		}

		private void TableNames_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (m_isUpdating || m_item == null)
				return;

			SomeProperty_Change(sender, e);
			m_hasChanged = true;
		}

		private void TableNames_RowDeleted(object sender, DataRowChangeEventArgs e)
		{
			if (m_isUpdating || m_item == null)
				return;

			SomeProperty_Change(sender, e);
			m_hasChanged = true;
		}

		private void TableGrid_Leave(object sender, System.EventArgs e)
		{
			if (m_hasChanged)
				SomeProperty_Change(sender, e);
			m_hasChanged = false;
		}

		private void TableGrid_Enter(object sender, System.EventArgs e)
		{
			m_hasChanged = false;
		}

		private void credentials_CredentialsChanged(string username, string password)
		{
			if (m_isUpdating || m_item == null)
				return;

			m_username = username;
			m_password = password;

			SomeProperty_Change(null, null);
		}
	}
}
