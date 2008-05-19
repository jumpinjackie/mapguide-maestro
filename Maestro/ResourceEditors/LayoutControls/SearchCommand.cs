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
	/// Summary description for SearchCommand.
	/// </summary>
	public class SearchCommand : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TextBox TargetFrame;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox Target;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox Prompt;
		private System.Windows.Forms.Button BrowseLayers;
		private System.Windows.Forms.TextBox Layer;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox Filter;
		private System.Windows.Forms.Button BuildFilter;
		private System.Windows.Forms.TextBox ResultLimit;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.DataGrid resultColumns;
		private System.Windows.Forms.Label label7;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private OSGeo.MapGuide.MaestroAPI.SearchCommandType m_command;
		private OSGeo.MapGuide.MaestroAPI.WebLayout m_layout;
		private EditorInterface m_editor;
		private bool m_isUpdating = false;
		private System.Data.DataSet dataSet;
		private System.Data.DataTable ResultTable;
		private System.Data.DataColumn dataColumn1;
		private System.Data.DataColumn dataColumn2;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private LayoutEditor m_layoutEditor = null;
		private System.Windows.Forms.DataGridTextBoxColumn CaptionColumnStyle;
		private System.Windows.Forms.DataGridTextBoxColumn PropertyColumnStyle;
		private bool m_hasChanged = false;

		public SearchCommand()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			Target.Items.Clear();
			foreach(object o in Enum.GetValues(typeof(OSGeo.MapGuide.MaestroAPI.TargetType)))
				Target.Items.Add(o.ToString());
			ResultTable.RowChanged += new DataRowChangeEventHandler(ResultTable_RowChanged);
			ResultTable.RowDeleted +=new DataRowChangeEventHandler(ResultTable_RowDeleted);
		}

		public void SetItem(OSGeo.MapGuide.MaestroAPI.SearchCommandType command, OSGeo.MapGuide.MaestroAPI.WebLayout layout, EditorInterface editor, LayoutEditor layoutEditor)
		{
			m_command = command;
			m_layout = layout;
			m_layoutEditor = layoutEditor;
			m_editor = editor;

			string capcol = m_layoutEditor.Globalizor.Translate("OSGeo.MapGuide.Maestro.ResourceEditors.LayoutEditor.commandEditor.searchCommand.CaptionColumnStyle.HeaderText");
			string propcol = m_layoutEditor.Globalizor.Translate("OSGeo.MapGuide.Maestro.ResourceEditors.LayoutEditor.commandEditor.searchCommand.PropertyColumnStyle.HeaderText");

			if (capcol != null)
				CaptionColumnStyle.HeaderText = capcol;
			if (propcol != null)
				PropertyColumnStyle.HeaderText = propcol;

			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				if (m_command == null)
					return;
			
				m_isUpdating = true;
				if (m_command == null)
					return;

				Target.SelectedIndex = Target.FindString(m_command.Target.ToString());
				TargetFrame.Text = m_command.TargetFrame;
				Prompt.Text = m_command.Prompt;
				Layer.Text = m_command.Layer;
				Filter.Text = m_command.Filter;
				ResultLimit.Text = m_command.MatchLimit;
				ResultTable.Rows.Clear();
				if (m_command.ResultColumns != null)
					foreach(OSGeo.MapGuide.MaestroAPI.ResultColumnType rs in m_command.ResultColumns)
						ResultTable.Rows.Add(new object[] { rs.Name, rs.Property });
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
			this.label3 = new System.Windows.Forms.Label();
			this.Prompt = new System.Windows.Forms.TextBox();
			this.BrowseLayers = new System.Windows.Forms.Button();
			this.Layer = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.Filter = new System.Windows.Forms.TextBox();
			this.BuildFilter = new System.Windows.Forms.Button();
			this.ResultLimit = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.resultColumns = new System.Windows.Forms.DataGrid();
			this.ResultTable = new System.Data.DataTable();
			this.dataColumn1 = new System.Data.DataColumn();
			this.dataColumn2 = new System.Data.DataColumn();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.CaptionColumnStyle = new System.Windows.Forms.DataGridTextBoxColumn();
			this.PropertyColumnStyle = new System.Windows.Forms.DataGridTextBoxColumn();
			this.label7 = new System.Windows.Forms.Label();
			this.dataSet = new System.Data.DataSet();
			((System.ComponentModel.ISupportInitialize)(this.resultColumns)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ResultTable)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSet)).BeginInit();
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
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 16);
			this.label3.TabIndex = 33;
			this.label3.Text = "Prompt";
			// 
			// Prompt
			// 
			this.Prompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Prompt.Location = new System.Drawing.Point(8, 88);
			this.Prompt.Multiline = true;
			this.Prompt.Name = "Prompt";
			this.Prompt.Size = new System.Drawing.Size(176, 88);
			this.Prompt.TabIndex = 34;
			this.Prompt.Text = "";
			this.Prompt.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// BrowseLayers
			// 
			this.BrowseLayers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseLayers.Location = new System.Drawing.Point(160, 184);
			this.BrowseLayers.Name = "BrowseLayers";
			this.BrowseLayers.Size = new System.Drawing.Size(24, 20);
			this.BrowseLayers.TabIndex = 43;
			this.BrowseLayers.Text = "...";
			this.BrowseLayers.Click += new System.EventHandler(this.BrowseLayers_Click);
			// 
			// Layer
			// 
			this.Layer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Layer.Location = new System.Drawing.Point(96, 184);
			this.Layer.Name = "Layer";
			this.Layer.Size = new System.Drawing.Size(56, 20);
			this.Layer.TabIndex = 45;
			this.Layer.Text = "";
			this.Layer.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(8, 184);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 16);
			this.label4.TabIndex = 44;
			this.label4.Text = "Layer";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 208);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 16);
			this.label5.TabIndex = 46;
			this.label5.Text = "Filter";
			// 
			// Filter
			// 
			this.Filter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Filter.Location = new System.Drawing.Point(96, 208);
			this.Filter.Name = "Filter";
			this.Filter.Size = new System.Drawing.Size(56, 20);
			this.Filter.TabIndex = 47;
			this.Filter.Text = "";
			this.Filter.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// BuildFilter
			// 
			this.BuildFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BuildFilter.Location = new System.Drawing.Point(160, 208);
			this.BuildFilter.Name = "BuildFilter";
			this.BuildFilter.Size = new System.Drawing.Size(24, 20);
			this.BuildFilter.TabIndex = 48;
			this.BuildFilter.Text = "...";
			this.BuildFilter.Click += new System.EventHandler(this.BuildFilter_Click);
			// 
			// ResultLimit
			// 
			this.ResultLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ResultLimit.Location = new System.Drawing.Point(96, 232);
			this.ResultLimit.Name = "ResultLimit";
			this.ResultLimit.Size = new System.Drawing.Size(88, 20);
			this.ResultLimit.TabIndex = 50;
			this.ResultLimit.Text = "";
			this.ResultLimit.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
			// 
			// label6
			// 
			this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label6.Location = new System.Drawing.Point(8, 232);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 16);
			this.label6.TabIndex = 49;
			this.label6.Text = "Result limit";
			// 
			// resultColumns
			// 
			this.resultColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.resultColumns.CaptionVisible = false;
			this.resultColumns.DataMember = "";
			this.resultColumns.DataSource = this.ResultTable;
			this.resultColumns.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.resultColumns.Location = new System.Drawing.Point(8, 288);
			this.resultColumns.Name = "resultColumns";
			this.resultColumns.Size = new System.Drawing.Size(176, 112);
			this.resultColumns.TabIndex = 51;
			this.resultColumns.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																									  this.dataGridTableStyle1});
			this.resultColumns.Leave += new System.EventHandler(this.resultColumns_Leave);
			// 
			// ResultTable
			// 
			this.ResultTable.Columns.AddRange(new System.Data.DataColumn[] {
																			   this.dataColumn1,
																			   this.dataColumn2});
			this.ResultTable.TableName = "ResultTable";
			// 
			// dataColumn1
			// 
			this.dataColumn1.ColumnName = "CaptionColumn";
			// 
			// dataColumn2
			// 
			this.dataColumn2.ColumnName = "PropertyColumn";
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.resultColumns;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																												  this.CaptionColumnStyle,
																												  this.PropertyColumnStyle});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "ResultTable";
			// 
			// CaptionColumnStyle
			// 
			this.CaptionColumnStyle.Format = "";
			this.CaptionColumnStyle.FormatInfo = null;
			this.CaptionColumnStyle.HeaderText = "Caption";
			this.CaptionColumnStyle.MappingName = "CaptionColumn";
			this.CaptionColumnStyle.Width = 75;
			// 
			// PropertyColumnStyle
			// 
			this.PropertyColumnStyle.Format = "";
			this.PropertyColumnStyle.FormatInfo = null;
			this.PropertyColumnStyle.HeaderText = "Property";
			this.PropertyColumnStyle.MappingName = "PropertyColumn";
			this.PropertyColumnStyle.Width = 150;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 272);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(176, 16);
			this.label7.TabIndex = 52;
			this.label7.Text = "Result columns";
			// 
			// dataSet
			// 
			this.dataSet.DataSetName = "NewDataSet";
			this.dataSet.Locale = new System.Globalization.CultureInfo("da-DK");
			this.dataSet.Tables.AddRange(new System.Data.DataTable[] {
																		 this.ResultTable});
			// 
			// SearchCommand
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(192, 408);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.resultColumns);
			this.Controls.Add(this.ResultLimit);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.BuildFilter);
			this.Controls.Add(this.Filter);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.Layer);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.BrowseLayers);
			this.Controls.Add(this.Prompt);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.TargetFrame);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Target);
			this.Controls.Add(this.label1);
			this.Name = "SearchCommand";
			this.Size = new System.Drawing.Size(192, 408);
			((System.ComponentModel.ISupportInitialize)(this.resultColumns)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ResultTable)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSet)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void SomeProperty_Changed(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_command == null)
				return;

			m_command.Target = (OSGeo.MapGuide.MaestroAPI.TargetType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.TargetType), Target.Text, true);
			m_command.TargetFrame = TargetFrame.Text;
			m_command.ResultColumns = new OSGeo.MapGuide.MaestroAPI.ResultColumnTypeCollection();
			foreach(DataRow dr in ResultTable.Rows)
			{
				OSGeo.MapGuide.MaestroAPI.ResultColumnType rs = new OSGeo.MapGuide.MaestroAPI.ResultColumnType();
				rs.Name = dr["CaptionColumn"].ToString();
				rs.Property = dr["PropertyColumn"].ToString();
				m_command.ResultColumns.Add(rs);
			}

			m_editor.HasChanged();
		}

		private void ResultTable_RowDeleted(object sender, DataRowChangeEventArgs e)
		{
			if (m_isUpdating || m_command == null)
				return;

			SomeProperty_Changed(sender, e);
			m_hasChanged = true;
		}

		private void ResultTable_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (m_isUpdating || m_command == null)
				return;

			SomeProperty_Changed(sender, e);
			m_hasChanged = true;
		}

		private void BrowseLayers_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this, "This function is not yet implemented", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void BuildFilter_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this, "This function is not yet implemented", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void resultColumns_Leave(object sender, System.EventArgs e)
		{
			if (m_hasChanged)
				SomeProperty_Changed(sender, e);
			m_hasChanged = false;
		}
	}
}
