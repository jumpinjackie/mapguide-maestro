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
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayoutControls
{
	/// <summary>
	/// Summary description for InvokeURL.
	/// </summary>
	public class InvokeURL : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox TargetFrame;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox Target;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox URL;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DataGrid dataGrid;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.InvokeURLCommandType m_command;
		private OSGeo.MapGuide.MaestroAPI.WebLayout m_layout;
		private EditorInterface m_editor;
		private bool m_isUpdating = false;
		private System.Windows.Forms.CheckBox DisableIfEmpty;
		private System.Windows.Forms.ListBox LayerSet;
		private System.Data.DataSet ParameterDataSet;
		private System.Data.DataTable ParamTable;
		private System.Data.DataColumn dataColumn1;
		private System.Data.DataColumn dataColumn2;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle;
		private System.Windows.Forms.DataGridTextBoxColumn KeyColumnStyle;
		private System.Windows.Forms.DataGridTextBoxColumn ValueColumnStyle;
		private System.Windows.Forms.Button BrowseLayers;
		private LayoutEditor m_layoutEditor = null;
		private bool m_hasChanged = false;

		public InvokeURL()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			Target.Items.Clear();
			foreach(object o in Enum.GetValues(typeof(OSGeo.MapGuide.MaestroAPI.TargetType)))
				Target.Items.Add(o.ToString());
			ParamTable.RowChanged += new DataRowChangeEventHandler(ParamTable_RowChanged);
			ParamTable.RowDeleted += new DataRowChangeEventHandler(ParamTable_RowDeleted);
		}

		public void SetItem(OSGeo.MapGuide.MaestroAPI.InvokeURLCommandType command, OSGeo.MapGuide.MaestroAPI.WebLayout layout, EditorInterface editor, LayoutEditor layoutEditor)
		{
			m_command = command;
			m_layout = layout;
			m_layoutEditor = layoutEditor;
			m_editor = editor;

			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				if (m_command == null)
					return;

				Target.SelectedIndex = Target.FindString(m_command.Target.ToString());
				TargetFrame.Text = m_command.TargetFrame;
				URL.Text = m_command.URL;
				DisableIfEmpty.Checked = m_command.DisableIfSelectionEmpty;
				LayerSet.Items.Clear();
				if (m_command.LayerSet == null || m_command.LayerSet.Count == 0)
				{
					LayerSet.Items.Add(Strings.InvokeURL.AllLayersName);
					LayerSet.Enabled = false;
				}
				else
				{
					try
					{
						LayerSet.BeginUpdate();
						foreach(string s in m_command.LayerSet)
							LayerSet.Items.Add(s);
					}
					finally
					{
						LayerSet.EndUpdate();
					}
					LayerSet.Enabled = true;
				}

				ParamTable.Rows.Clear();
				if (m_command.AdditionalParameter != null)
					foreach(OSGeo.MapGuide.MaestroAPI.ParameterPairType pt in m_command.AdditionalParameter)
						ParamTable.Rows.Add(new object[] { pt.Key, pt.Value });
			}
			finally
			{
				m_isUpdating = false;
			}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvokeURL));
            this.TargetFrame = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Target = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.URL = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DisableIfEmpty = new System.Windows.Forms.CheckBox();
            this.dataGrid = new System.Windows.Forms.DataGrid();
            this.ParamTable = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.KeyColumnStyle = new System.Windows.Forms.DataGridTextBoxColumn();
            this.ValueColumnStyle = new System.Windows.Forms.DataGridTextBoxColumn();
            this.LayerSet = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BrowseLayers = new System.Windows.Forms.Button();
            this.ParameterDataSet = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParamTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParameterDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // TargetFrame
            // 
            resources.ApplyResources(this.TargetFrame, "TargetFrame");
            this.TargetFrame.Name = "TargetFrame";
            this.TargetFrame.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Target
            // 
            resources.ApplyResources(this.Target, "Target");
            this.Target.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Target.Name = "Target";
            this.Target.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // URL
            // 
            resources.ApplyResources(this.URL, "URL");
            this.URL.Name = "URL";
            this.URL.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // DisableIfEmpty
            // 
            resources.ApplyResources(this.DisableIfEmpty, "DisableIfEmpty");
            this.DisableIfEmpty.Name = "DisableIfEmpty";
            this.DisableIfEmpty.CheckedChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // dataGrid
            // 
            resources.ApplyResources(this.dataGrid, "dataGrid");
            this.dataGrid.CaptionVisible = false;
            this.dataGrid.DataMember = "";
            this.dataGrid.DataSource = this.ParamTable;
            this.dataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle});
            this.dataGrid.Leave += new System.EventHandler(this.dataGrid_Leave);
            // 
            // ParamTable
            // 
            this.ParamTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2});
            this.ParamTable.TableName = "ParamTable";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "KeyColumn";
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "ValueColumn";
            // 
            // dataGridTableStyle
            // 
            this.dataGridTableStyle.DataGrid = this.dataGrid;
            this.dataGridTableStyle.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.KeyColumnStyle,
            this.ValueColumnStyle});
            this.dataGridTableStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle.MappingName = "ParamTable";
            // 
            // KeyColumnStyle
            // 
            this.KeyColumnStyle.Format = "";
            this.KeyColumnStyle.FormatInfo = null;
            resources.ApplyResources(this.KeyColumnStyle, "KeyColumnStyle");
            // 
            // ValueColumnStyle
            // 
            this.ValueColumnStyle.Format = "";
            this.ValueColumnStyle.FormatInfo = null;
            resources.ApplyResources(this.ValueColumnStyle, "ValueColumnStyle");
            // 
            // LayerSet
            // 
            resources.ApplyResources(this.LayerSet, "LayerSet");
            this.LayerSet.Name = "LayerSet";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // BrowseLayers
            // 
            resources.ApplyResources(this.BrowseLayers, "BrowseLayers");
            this.BrowseLayers.Name = "BrowseLayers";
            this.BrowseLayers.Click += new System.EventHandler(this.BrowseLayers_Click);
            // 
            // ParameterDataSet
            // 
            this.ParameterDataSet.DataSetName = "NewDataSet";
            this.ParameterDataSet.Locale = new System.Globalization.CultureInfo("da-DK");
            this.ParameterDataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.ParamTable});
            // 
            // InvokeURL
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.BrowseLayers);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LayerSet);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.DisableIfEmpty);
            this.Controls.Add(this.URL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TargetFrame);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Target);
            this.Controls.Add(this.label1);
            this.Name = "InvokeURL";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParamTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParameterDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void SomeProperty_Changed(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_command == null)
				return;

			m_command.Target = (OSGeo.MapGuide.MaestroAPI.TargetType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.TargetType), Target.Text, true);
			m_command.TargetFrame = TargetFrame.Text;
			m_command.URL = URL.Text;
			m_command.DisableIfSelectionEmpty = DisableIfEmpty.Checked;

			m_command.AdditionalParameter = new OSGeo.MapGuide.MaestroAPI.ParameterPairTypeCollection();
			foreach(DataRow dr in ParamTable.Rows)
			{
				OSGeo.MapGuide.MaestroAPI.ParameterPairType pt = new OSGeo.MapGuide.MaestroAPI.ParameterPairType();
				pt.Key = dr["KeyColumn"].ToString();
				pt.Value = dr["ValueColumn"].ToString();
				m_command.AdditionalParameter.Add(pt);
			}

			m_editor.HasChanged();
		}

		private void ParamTable_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (m_isUpdating || m_command == null)
				return;

			m_hasChanged = true;
			SomeProperty_Changed(sender, e);
		}

		private void ParamTable_RowDeleted(object sender, DataRowChangeEventArgs e)
		{
			if (m_isUpdating || m_command == null)
				return;

			m_hasChanged = true;
			SomeProperty_Changed(sender, e);
		}

		private void BrowseLayers_Click(object sender, System.EventArgs e)
		{
			ArrayList layers = new ArrayList();
			string mapname = "";
			try
			{
				mapname = m_layout.Map.ResourceId;
				OSGeo.MapGuide.MaestroAPI.MapDefinition mdef = m_editor.CurrentConnection.GetMapDefinition(m_layout.Map.ResourceId);
				foreach(OSGeo.MapGuide.MaestroAPI.MapLayerType mlt in mdef.Layers)
					layers.Add(mlt.Name);
			}
			catch (Exception ex)
			{
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                m_editor.SetLastException(ex);
                MessageBox.Show(this, string.Format(Strings.InvokeURL.LayerListReadError, mapname, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			BrowseLayers dlg = new BrowseLayers();
			dlg.LayerList.Items.AddRange(layers.ToArray());
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				m_command.LayerSet = new System.Collections.Specialized.StringCollection();
				foreach(string s in dlg.LayerList.CheckedItems)
					m_command.LayerSet.Add(s);

                LayerSet.Items.Clear();

				if (m_command.LayerSet == null || m_command.LayerSet.Count == 0)
				{
					LayerSet.Items.Add(Strings.InvokeURL.AllLayersName);
					LayerSet.Enabled = false;
				}
				else
				{
					try
					{
						LayerSet.BeginUpdate();
						foreach(string s in m_command.LayerSet)
							LayerSet.Items.Add(s);
					}
					finally
					{
						LayerSet.EndUpdate();
					}
					LayerSet.Enabled = true;
				}

				m_editor.HasChanged();
			}
			
		}

		private void dataGrid_Leave(object sender, System.EventArgs e)
		{
			if (m_hasChanged)
				SomeProperty_Changed(sender, e);
			m_hasChanged = false;
		}

	}
}
