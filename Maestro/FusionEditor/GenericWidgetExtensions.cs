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
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;
using System.Xml;

namespace OSGeo.MapGuide.Maestro.FusionEditor
{
	/// <summary>
	/// Summary description for GenericWidgetExtensions.
	/// </summary>
	public class GenericWidgetExtensions : System.Windows.Forms.DataGrid 
	{
		private System.Data.DataSet ExtensionDataSet;
		private System.Data.DataTable ExtensionTable;
		private System.Data.DataColumn NameColumn;
		private System.Data.DataColumn ValueColumn;
		private WidgetType m_w;
		private bool m_isUpdating = false;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GenericWidgetExtensions()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.DataSource = ExtensionTable;
			ExtensionTable.RowChanged += new DataRowChangeEventHandler(ExtensionTable_RowChanged);
			ExtensionTable.RowDeleting += new DataRowChangeEventHandler(ExtensionTable_RowChanged);

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

		public void SetItem(WidgetType w)
		{
			try
			{
				m_isUpdating = true;
				m_w = w;
				this.Enabled = m_w != null;
				ExtensionTable.Rows.Clear();
				if (m_w == null || m_w.Extension == null || m_w.Extension.Any == null)
					return;

				foreach(XmlNode n in m_w.Extension.Any)
				{
					System.Data.DataRow r = ExtensionTable.NewRow();
					r["Name"] = n.Name;
					r["Value"] = n.InnerXml;
					ExtensionTable.Rows.Add(r);
				}
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
			this.ExtensionDataSet = new System.Data.DataSet();
			this.ExtensionTable = new System.Data.DataTable();
			this.NameColumn = new System.Data.DataColumn();
			this.ValueColumn = new System.Data.DataColumn();
			((System.ComponentModel.ISupportInitialize)(this.ExtensionDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ExtensionTable)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// ExtensionDataSet
			// 
			this.ExtensionDataSet.DataSetName = "NewDataSet";
			this.ExtensionDataSet.Locale = new System.Globalization.CultureInfo("da-DK");
			this.ExtensionDataSet.Tables.AddRange(new System.Data.DataTable[] {
																				  this.ExtensionTable});
			// 
			// ExtensionTable
			// 
			this.ExtensionTable.Columns.AddRange(new System.Data.DataColumn[] {
																				  this.NameColumn,
																				  this.ValueColumn});
			this.ExtensionTable.TableName = "ExtensionTable";
			// 
			// NameColumn
			// 
			this.NameColumn.ColumnName = "Name";
			// 
			// ValueColumn
			// 
			this.ValueColumn.ColumnName = "Value";
			// 
			// GenericWidgetExtensions
			// 
			this.Size = new System.Drawing.Size(400, 456);
			((System.ComponentModel.ISupportInitialize)(this.ExtensionDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ExtensionTable)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		#endregion

		private void ExtensionTable_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			switch(e.Action)
			{
				case DataRowAction.Add:
					m_w.Extension[e.Row["Name"].ToString()] = e.Row["Value"].ToString();
					break;
				case DataRowAction.Change:
					m_w.Extension[e.Row["Name"].ToString()] = e.Row["Value"].ToString();
					//If the Name column was changed, this will add a new value
					Hashtable ht = new Hashtable();
					foreach(DataRow r in e.Row.Table.Rows)
						ht[r["Name"]] = r;

					foreach(XmlElement n in m_w.Extension.Any)
						if (!ht.ContainsKey(n.Name))
						{
							m_w.Extension[n.Name] = null;
							break;
						}


					break;
				case DataRowAction.Delete:
					m_w.Extension[e.Row["Name"].ToString()] = null;
					break;
			}


		}
	}
}
