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
using System.Collections.Generic;
using OSGeo.MapGuide.MaestroAPI;

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
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox Filter;
        private System.Windows.Forms.Button BuildFilter;
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
        private NumericUpDown ResultLimit;
        private ComboBox Layer;
		private bool m_hasChanged = false;
        private List<MaestroAPI.MapLayerType> m_layerList;

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

            try
            {
                m_isUpdating = true;
                Layer.Items.Clear();
                m_layerList = new List<MaestroAPI.MapLayerType>();
                MaestroAPI.MapDefinition mdef = m_layout.CurrentConnection.GetMapDefinition(m_layout.Map.ResourceId);
                foreach (MaestroAPI.MapLayerType l in mdef.Layers)
                    m_layerList.Add(l);
                if (mdef.BaseMapDefinition != null && mdef.BaseMapDefinition.BaseMapLayerGroup != null)
                    foreach (MaestroAPI.BaseMapLayerGroupCommonType g in mdef.BaseMapDefinition.BaseMapLayerGroup)
                        foreach (MaestroAPI.MapLayerType l in g.BaseMapLayer)
                            m_layerList.Add(l);
            }
            catch { }
            finally
            {
                foreach (MaestroAPI.MapLayerType l in m_layerList)
                    Layer.Items.Add(l.Name);

                m_isUpdating = false;
            }

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
                Layer.SelectedIndex = Layer.FindString(m_command.Layer);
				Filter.Text = m_command.Filter;
				ResultLimit.Value = Math.Min(Math.Max(m_command.MatchLimit, ResultLimit.Minimum), ResultLimit.Maximum);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchCommand));
            this.TargetFrame = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Target = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Prompt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Filter = new System.Windows.Forms.TextBox();
            this.BuildFilter = new System.Windows.Forms.Button();
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
            this.ResultLimit = new System.Windows.Forms.NumericUpDown();
            this.Layer = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.resultColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultLimit)).BeginInit();
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
            this.Target.SelectedIndexChanged += new System.EventHandler(this.SomeProperty_Changed);
            this.Target.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // Prompt
            // 
            resources.ApplyResources(this.Prompt, "Prompt");
            this.Prompt.Name = "Prompt";
            this.Prompt.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // Filter
            // 
            resources.ApplyResources(this.Filter, "Filter");
            this.Filter.Name = "Filter";
            this.Filter.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // BuildFilter
            // 
            resources.ApplyResources(this.BuildFilter, "BuildFilter");
            this.BuildFilter.Name = "BuildFilter";
            this.BuildFilter.Click += new System.EventHandler(this.BuildFilter_Click);
            // 
            // label6
            // 
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // resultColumns
            // 
            resources.ApplyResources(this.resultColumns, "resultColumns");
            this.resultColumns.CaptionVisible = false;
            this.resultColumns.DataMember = "";
            this.resultColumns.DataSource = this.ResultTable;
            this.resultColumns.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.resultColumns.Name = "resultColumns";
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
            resources.ApplyResources(this.CaptionColumnStyle, "CaptionColumnStyle");
            // 
            // PropertyColumnStyle
            // 
            this.PropertyColumnStyle.Format = "";
            this.PropertyColumnStyle.FormatInfo = null;
            resources.ApplyResources(this.PropertyColumnStyle, "PropertyColumnStyle");
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // dataSet
            // 
            this.dataSet.DataSetName = "NewDataSet";
            this.dataSet.Locale = new System.Globalization.CultureInfo("da-DK");
            this.dataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.ResultTable});
            // 
            // ResultLimit
            // 
            resources.ApplyResources(this.ResultLimit, "ResultLimit");
            this.ResultLimit.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ResultLimit.Name = "ResultLimit";
            this.ResultLimit.ValueChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // Layer
            // 
            resources.ApplyResources(this.Layer, "Layer");
            this.Layer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Layer.FormattingEnabled = true;
            this.Layer.Name = "Layer";
            this.Layer.SelectedIndexChanged += new System.EventHandler(this.SomeProperty_Changed);
            this.Layer.TextChanged += new System.EventHandler(this.SomeProperty_Changed);
            // 
            // SearchCommand
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.Layer);
            this.Controls.Add(this.ResultLimit);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.resultColumns);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BuildFilter);
            this.Controls.Add(this.Filter);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Prompt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TargetFrame);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Target);
            this.Controls.Add(this.label1);
            this.Name = "SearchCommand";
            ((System.ComponentModel.ISupportInitialize)(this.resultColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultLimit)).EndInit();
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
			m_command.ResultColumns = new OSGeo.MapGuide.MaestroAPI.ResultColumnTypeCollection();
            m_command.Prompt = Prompt.Text;
            m_command.Layer = Layer.Text;
            m_command.Filter = Filter.Text;
            int i;
            if (int.TryParse(ResultLimit.Text, out i))
                m_command.MatchLimit = i;

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

		private void BuildFilter_Click(object sender, System.EventArgs e)
		{
            try
            {
                if (Layer.SelectedIndex < 0)
                {
                    MessageBox.Show(this, Strings.SearchCommand.NoLayerSelectedError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MaestroAPI.MapLayerType l = m_layerList[Layer.SelectedIndex];
                MaestroAPI.LayerDefinition ldef = m_editor.CurrentConnection.GetLayerDefinition(l.ResourceId);
                MaestroAPI.VectorLayerDefinitionType vldef = ldef.Item as MaestroAPI.VectorLayerDefinitionType;
                if (vldef == null)
                {
                    MessageBox.Show(this, Strings.SearchCommand.NoVectorLayerSelectedError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                MaestroAPI.FeatureSource fs =m_editor.CurrentConnection.GetFeatureSource(vldef.ResourceId);
                MaestroAPI.FeatureSourceDescription.FeatureSourceSchema schema = m_editor.CurrentConnection.GetFeatureSourceSchema(vldef.ResourceId, vldef.FeatureName);


                string exp = m_editor.EditExpression(Filter.Text, schema, fs.Provider, fs.ResourceId);
                if (exp != null)
                    Filter.Text = exp;
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                m_editor.SetLastException(ex);
                MessageBox.Show(this, string.Format(OSGeo.MapGuide.Maestro.ResourceEditors.Strings.Common.GenericError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
		}

		private void resultColumns_Leave(object sender, System.EventArgs e)
		{
			if (m_hasChanged)
				SomeProperty_Changed(sender, e);
			m_hasChanged = false;
		}
	}
}
