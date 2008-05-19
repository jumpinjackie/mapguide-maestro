#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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

			string keycol = m_layoutEditor.Globalizor.Translate("OSGeo.MapGuide.Maestro.ResourceEditors.LayoutEditor.commandEditor.invokeURL.KeyColumnStyle.HeaderText");
			string valuecol = m_layoutEditor.Globalizor.Translate("OSGeo.MapGuide.Maestro.ResourceEditors.LayoutEditor.commandEditor.invokeURL.ValueColumnStyle.HeaderText");
			if (keycol != null)
				KeyColumnStyle.HeaderText = keycol;
			if (valuecol != null)
				ValueColumnStyle.HeaderText = valuecol;

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
					LayerSet.Items.Add("All layers");
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
			this.TargetFrame = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.Target = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.URL = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.DisableIfEmpty = new System.Windows.Forms.CheckBox();
			this.dataGrid = new System.Windows.Forms.DataGrid();
			this.LayerSet = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.BrowseLayers = new System.Windows.Forms.Button();
			this.ParameterDataSet = new System.Data.DataSet();
			this.ParamTable = new System.Data.DataTable();
			this.dataColumn1 = new System.Data.DataColumn();
			this.dataColumn2 = new System.Data.DataColumn();
			this.dataGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
			this.KeyColumnStyle = new System.Windows.Forms.DataGridTextBoxColumn();
			this.ValueColumnStyle = new System.Windows.Forms.DataGridTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ParameterDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ParamTable)).BeginInit();
			this.SuspendLayout();
			// 
			// TargetFrame
			// 
			this.TargetFrame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.TargetFrame.Location = new System.Drawing.Point(96, 40);
			this.TargetFrame.Name = "TargetFrame";
			this.TargetFrame.Size = new System.Drawing.Size(88, 20);
			this.TargetFrame.TabIndex = 32;
			this.TargetFrame.Text = "";
			this.TargetFrame.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 16);
			this.label2.TabIndex = 31;
			this.label2.Text = "Frame";
			// 
			// Target
			// 
			this.Target.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Target.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Target.Location = new System.Drawing.Point(96, 8);
			this.Target.Name = "Target";
			this.Target.Size = new System.Drawing.Size(88, 21);
			this.Target.TabIndex = 30;
			this.Target.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 29;
			this.label1.Text = "Target";
			// 
			// URL
			// 
			this.URL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.URL.Location = new System.Drawing.Point(96, 72);
			this.URL.Name = "URL";
			this.URL.Size = new System.Drawing.Size(88, 20);
			this.URL.TabIndex = 34;
			this.URL.Text = "";
			this.URL.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 16);
			this.label3.TabIndex = 33;
			this.label3.Text = "URL";
			// 
			// DisableIfEmpty
			// 
			this.DisableIfEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.DisableIfEmpty.Location = new System.Drawing.Point(8, 104);
			this.DisableIfEmpty.Name = "DisableIfEmpty";
			this.DisableIfEmpty.Size = new System.Drawing.Size(176, 16);
			this.DisableIfEmpty.TabIndex = 37;
			this.DisableIfEmpty.Text = "Disable if selection is empty";
			this.DisableIfEmpty.CheckedChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// dataGrid
			// 
			this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGrid.CaptionVisible = false;
			this.dataGrid.DataMember = "";
			this.dataGrid.DataSource = this.ParamTable;
			this.dataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid.Location = new System.Drawing.Point(8, 152);
			this.dataGrid.Name = "dataGrid";
			this.dataGrid.Size = new System.Drawing.Size(176, 72);
			this.dataGrid.TabIndex = 38;
			this.dataGrid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																								 this.dataGridTableStyle});
			this.dataGrid.Leave += new System.EventHandler(this.dataGrid_Leave);
			// 
			// LayerSet
			// 
			this.LayerSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.LayerSet.Location = new System.Drawing.Point(8, 256);
			this.LayerSet.Name = "LayerSet";
			this.LayerSet.Size = new System.Drawing.Size(176, 69);
			this.LayerSet.TabIndex = 39;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 136);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(160, 16);
			this.label4.TabIndex = 40;
			this.label4.Text = "Additional URL parameters";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 240);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(144, 16);
			this.label5.TabIndex = 41;
			this.label5.Text = "Valid layers";
			// 
			// BrowseLayers
			// 
			this.BrowseLayers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseLayers.Location = new System.Drawing.Point(160, 236);
			this.BrowseLayers.Name = "BrowseLayers";
			this.BrowseLayers.Size = new System.Drawing.Size(24, 20);
			this.BrowseLayers.TabIndex = 42;
			this.BrowseLayers.Text = "...";
			this.BrowseLayers.Click += new System.EventHandler(this.BrowseLayers_Click);
			// 
			// ParameterDataSet
			// 
			this.ParameterDataSet.DataSetName = "NewDataSet";
			this.ParameterDataSet.Locale = new System.Globalization.CultureInfo("da-DK");
			this.ParameterDataSet.Tables.AddRange(new System.Data.DataTable[] {
																				  this.ParamTable});
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
			this.KeyColumnStyle.HeaderText = "Key";
			this.KeyColumnStyle.MappingName = "KeyColumn";
			// 
			// ValueColumnStyle
			// 
			this.ValueColumnStyle.Format = "";
			this.ValueColumnStyle.FormatInfo = null;
			this.ValueColumnStyle.HeaderText = "Value";
			this.ValueColumnStyle.MappingName = "ValueColumn";
			this.ValueColumnStyle.Width = 150;
			// 
			// InvokeURL
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(192, 336);
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
			this.Size = new System.Drawing.Size(192, 336);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ParameterDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ParamTable)).EndInit();
			this.ResumeLayout(false);

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
				MessageBox.Show(this, string.Format(m_layoutEditor.Globalizor.Translate("Failed to read layer list from {0}.\nError message: {1}"), mapname, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			BrowseLayers dlg = new BrowseLayers();
			dlg.LayerList.Items.AddRange(layers.ToArray());
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				m_command.LayerSet = new System.Collections.Specialized.StringCollection();
				foreach(string s in dlg.LayerList.CheckedItems)
					m_command.LayerSet.Add(s);
			

				if (m_command.LayerSet == null || m_command.LayerSet.Count == 0)
				{
					LayerSet.Items.Add("All layers");
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
