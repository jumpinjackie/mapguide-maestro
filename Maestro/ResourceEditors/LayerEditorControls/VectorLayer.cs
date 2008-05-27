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
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls
{
	/// <summary>
	/// Summary description for VectorLayer.
	/// </summary>
	public class VectorLayer : System.Windows.Forms.UserControl
	{
		private System.Data.DataColumn PropertyColumnDisplay;
		private System.Data.DataColumn PropertyColumnVisible;
		private System.Data.DataColumn PropertyColumnName;
		private System.Windows.Forms.GroupBox ResourceGroup;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox Tooltip;
		private System.Windows.Forms.TextBox Link;
		private System.Windows.Forms.TextBox Filter;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Data.DataTable ViewerPropertiesTable;
		private System.Data.DataTable DisplayRangesTable;
		private System.Data.DataColumn FromScaleColumn;
		private System.Data.DataColumn ToScaleColumn;
		private System.Data.DataColumn StylizationColumn;
		private System.Data.DataColumn dataColumn1;
		private System.Data.DataColumn dataColumn2;
		private System.Data.DataColumn dataColumn3;
        private System.Data.DataSet PropertyDataset;
		private System.Windows.Forms.ImageList LayerStyleImages;
		private System.Windows.Forms.GroupBox groupViewerProperties;
		private System.Windows.Forms.Button SelectInverseButton;
		private System.Windows.Forms.Button SelectNoneButton;
		private System.Windows.Forms.Button SelectAllButton;
		private System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
		private System.Data.DataSet DisplayRanges;
		private System.ComponentModel.IContainer components;


		private OSGeo.MapGuide.MaestroAPI.LayerDefinition m_layer;
		private bool inUpdate = false;
		private Globalizator.Globalizator m_globalizor = null;
		private OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription m_schemas;
		private ResourceEditors.LayerEditorControls.SchemaSelector schemaSelector;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.DataGridBoolColumn dataGridBoolColumn1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
        private ToolStrip toolStrip1;
        private ToolStripButton AddScaleRangeButton;
        private ToolStripButton DeleteItemButton;
        private OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.ScaleRangeList scaleRangeList;
        private ToolStripButton toolStripButton1;
		private EditorInterface m_editor;

		public VectorLayer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			ViewerPropertiesTable.ColumnChanged += new DataColumnChangeEventHandler(ViewerPropertiesTable_ColumnChanged);
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VectorLayer));
            this.PropertyColumnDisplay = new System.Data.DataColumn();
            this.PropertyColumnVisible = new System.Data.DataColumn();
            this.PropertyColumnName = new System.Data.DataColumn();
            this.ResourceGroup = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.Tooltip = new System.Windows.Forms.TextBox();
            this.Link = new System.Windows.Forms.TextBox();
            this.Filter = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ViewerPropertiesTable = new System.Data.DataTable();
            this.DisplayRangesTable = new System.Data.DataTable();
            this.FromScaleColumn = new System.Data.DataColumn();
            this.ToScaleColumn = new System.Data.DataColumn();
            this.PropertyDataset = new System.Data.DataSet();
            this.LayerStyleImages = new System.Windows.Forms.ImageList(this.components);
            this.groupViewerProperties = new System.Windows.Forms.GroupBox();
            this.SelectInverseButton = new System.Windows.Forms.Button();
            this.SelectNoneButton = new System.Windows.Forms.Button();
            this.SelectAllButton = new System.Windows.Forms.Button();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridBoolColumn1 = new System.Windows.Forms.DataGridBoolColumn();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddScaleRangeButton = new System.Windows.Forms.ToolStripButton();
            this.DeleteItemButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.DisplayRanges = new System.Data.DataSet();
            this.schemaSelector = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.SchemaSelector();
            this.scaleRangeList = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.ScaleRangeList();
            this.StylizationColumn = new System.Data.DataColumn();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.ResourceGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ViewerPropertiesTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayRangesTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyDataset)).BeginInit();
            this.groupViewerProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayRanges)).BeginInit();
            this.SuspendLayout();
            // 
            // PropertyColumnDisplay
            // 
            this.PropertyColumnDisplay.Caption = "Display";
            this.PropertyColumnDisplay.ColumnName = "Display";
            // 
            // PropertyColumnVisible
            // 
            this.PropertyColumnVisible.Caption = "Visible";
            this.PropertyColumnVisible.ColumnName = "Visible";
            this.PropertyColumnVisible.DataType = typeof(bool);
            this.PropertyColumnVisible.DefaultValue = false;
            // 
            // PropertyColumnName
            // 
            this.PropertyColumnName.Caption = "Name";
            this.PropertyColumnName.ColumnName = "Name";
            // 
            // ResourceGroup
            // 
            this.ResourceGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ResourceGroup.Controls.Add(this.schemaSelector);
            this.ResourceGroup.Controls.Add(this.button5);
            this.ResourceGroup.Controls.Add(this.button4);
            this.ResourceGroup.Controls.Add(this.button3);
            this.ResourceGroup.Controls.Add(this.Tooltip);
            this.ResourceGroup.Controls.Add(this.Link);
            this.ResourceGroup.Controls.Add(this.Filter);
            this.ResourceGroup.Controls.Add(this.label6);
            this.ResourceGroup.Controls.Add(this.label5);
            this.ResourceGroup.Controls.Add(this.label4);
            this.ResourceGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ResourceGroup.Location = new System.Drawing.Point(0, 0);
            this.ResourceGroup.Name = "ResourceGroup";
            this.ResourceGroup.Size = new System.Drawing.Size(799, 176);
            this.ResourceGroup.TabIndex = 16;
            this.ResourceGroup.TabStop = false;
            this.ResourceGroup.Text = "Resource settings";
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button5.Location = new System.Drawing.Point(759, 144);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(24, 20);
            this.button5.TabIndex = 13;
            this.button5.Text = "...";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button4.Location = new System.Drawing.Point(759, 112);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(24, 20);
            this.button4.TabIndex = 12;
            this.button4.Text = "...";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button3.Location = new System.Drawing.Point(759, 80);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(24, 20);
            this.button3.TabIndex = 11;
            this.button3.Text = "...";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Tooltip
            // 
            this.Tooltip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Tooltip.Location = new System.Drawing.Point(120, 144);
            this.Tooltip.Name = "Tooltip";
            this.Tooltip.Size = new System.Drawing.Size(639, 20);
            this.Tooltip.TabIndex = 10;
            this.Tooltip.Text = "textBox4";
            this.Tooltip.TextChanged += new System.EventHandler(this.Tooltip_TextChanged);
            // 
            // Link
            // 
            this.Link.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Link.Location = new System.Drawing.Point(120, 112);
            this.Link.Name = "Link";
            this.Link.Size = new System.Drawing.Size(639, 20);
            this.Link.TabIndex = 9;
            this.Link.Text = "textBox3";
            this.Link.TextChanged += new System.EventHandler(this.Link_TextChanged);
            // 
            // Filter
            // 
            this.Filter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Filter.Location = new System.Drawing.Point(120, 80);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(639, 20);
            this.Filter.TabIndex = 8;
            this.Filter.Text = "textBox2";
            this.Filter.TextChanged += new System.EventHandler(this.Filter_TextChanged);
            // 
            // label6
            // 
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label6.Location = new System.Drawing.Point(16, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "Tooltip";
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label5.Location = new System.Drawing.Point(16, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "Link";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Location = new System.Drawing.Point(16, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Filter";
            // 
            // ViewerPropertiesTable
            // 
            this.ViewerPropertiesTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.PropertyColumnVisible,
            this.PropertyColumnDisplay,
            this.PropertyColumnName});
            this.ViewerPropertiesTable.TableName = "ViewerPropertiesTable";
            // 
            // DisplayRangesTable
            // 
            this.DisplayRangesTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.FromScaleColumn,
            this.ToScaleColumn,
            this.StylizationColumn,
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3});
            this.DisplayRangesTable.TableName = "DisplayRangesTable";
            // 
            // FromScaleColumn
            // 
            this.FromScaleColumn.Caption = "From";
            this.FromScaleColumn.ColumnName = "FromScale";
            // 
            // ToScaleColumn
            // 
            this.ToScaleColumn.Caption = "To";
            this.ToScaleColumn.ColumnName = "ToScale";
            // 
            // PropertyDataset
            // 
            this.PropertyDataset.DataSetName = "ViewerProperties";
            this.PropertyDataset.Locale = new System.Globalization.CultureInfo("da-DK");
            this.PropertyDataset.Tables.AddRange(new System.Data.DataTable[] {
            this.ViewerPropertiesTable});
            // 
            // LayerStyleImages
            // 
            this.LayerStyleImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("LayerStyleImages.ImageStream")));
            this.LayerStyleImages.TransparentColor = System.Drawing.Color.Transparent;
            this.LayerStyleImages.Images.SetKeyName(0, "");
            this.LayerStyleImages.Images.SetKeyName(1, "");
            this.LayerStyleImages.Images.SetKeyName(2, "");
            this.LayerStyleImages.Images.SetKeyName(3, "");
            this.LayerStyleImages.Images.SetKeyName(4, "");
            this.LayerStyleImages.Images.SetKeyName(5, "");
            this.LayerStyleImages.Images.SetKeyName(6, "");
            this.LayerStyleImages.Images.SetKeyName(7, "");
            this.LayerStyleImages.Images.SetKeyName(8, "");
            this.LayerStyleImages.Images.SetKeyName(9, "");
            this.LayerStyleImages.Images.SetKeyName(10, "");
            // 
            // groupViewerProperties
            // 
            this.groupViewerProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupViewerProperties.Controls.Add(this.SelectInverseButton);
            this.groupViewerProperties.Controls.Add(this.SelectNoneButton);
            this.groupViewerProperties.Controls.Add(this.SelectAllButton);
            this.groupViewerProperties.Controls.Add(this.dataGrid1);
            this.groupViewerProperties.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupViewerProperties.Location = new System.Drawing.Point(0, 184);
            this.groupViewerProperties.Name = "groupViewerProperties";
            this.groupViewerProperties.Size = new System.Drawing.Size(799, 232);
            this.groupViewerProperties.TabIndex = 17;
            this.groupViewerProperties.TabStop = false;
            this.groupViewerProperties.Text = "Properties avalible in viewer";
            // 
            // SelectInverseButton
            // 
            this.SelectInverseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectInverseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectInverseButton.Location = new System.Drawing.Point(232, 192);
            this.SelectInverseButton.Name = "SelectInverseButton";
            this.SelectInverseButton.Size = new System.Drawing.Size(96, 24);
            this.SelectInverseButton.TabIndex = 3;
            this.SelectInverseButton.Text = "Select inverse";
            this.SelectInverseButton.Click += new System.EventHandler(this.SelectInverseButton_Click);
            // 
            // SelectNoneButton
            // 
            this.SelectNoneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectNoneButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectNoneButton.Location = new System.Drawing.Point(128, 192);
            this.SelectNoneButton.Name = "SelectNoneButton";
            this.SelectNoneButton.Size = new System.Drawing.Size(88, 24);
            this.SelectNoneButton.TabIndex = 2;
            this.SelectNoneButton.Text = "Select none";
            this.SelectNoneButton.Click += new System.EventHandler(this.SelectNoneButton_Click);
            // 
            // SelectAllButton
            // 
            this.SelectAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectAllButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectAllButton.Location = new System.Drawing.Point(16, 192);
            this.SelectAllButton.Name = "SelectAllButton";
            this.SelectAllButton.Size = new System.Drawing.Size(96, 24);
            this.SelectAllButton.TabIndex = 1;
            this.SelectAllButton.Text = "Select all";
            this.SelectAllButton.Click += new System.EventHandler(this.SelectAllButton_Click);
            // 
            // dataGrid1
            // 
            this.dataGrid1.AllowSorting = false;
            this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid1.CaptionVisible = false;
            this.dataGrid1.DataMember = "";
            this.dataGrid1.DataSource = this.ViewerPropertiesTable;
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(16, 24);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.RowHeadersVisible = false;
            this.dataGrid1.Size = new System.Drawing.Size(767, 160);
            this.dataGrid1.TabIndex = 0;
            this.dataGrid1.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.AllowSorting = false;
            this.dataGridTableStyle1.DataGrid = this.dataGrid1;
            this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dataGridBoolColumn1,
            this.dataGridTextBoxColumn1,
            this.dataGridTextBoxColumn2});
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle1.MappingName = "ViewerPropertiesTable";
            this.dataGridTableStyle1.RowHeadersVisible = false;
            // 
            // dataGridBoolColumn1
            // 
            this.dataGridBoolColumn1.AllowNull = false;
            this.dataGridBoolColumn1.HeaderText = "Visible";
            this.dataGridBoolColumn1.MappingName = "Visible";
            this.dataGridBoolColumn1.Width = 75;
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Display";
            this.dataGridTextBoxColumn1.MappingName = "Display";
            this.dataGridTextBoxColumn1.Width = 75;
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "Name";
            this.dataGridTextBoxColumn2.MappingName = "Name";
            this.dataGridTextBoxColumn2.ReadOnly = true;
            this.dataGridTextBoxColumn2.Width = 75;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(0, 424);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(799, 430);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Layer style";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.scaleRangeList);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(793, 411);
            this.panel1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddScaleRangeButton,
            this.DeleteItemButton,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(793, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // AddScaleRangeButton
            // 
            this.AddScaleRangeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddScaleRangeButton.Image = ((System.Drawing.Image)(resources.GetObject("AddScaleRangeButton.Image")));
            this.AddScaleRangeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddScaleRangeButton.Name = "AddScaleRangeButton";
            this.AddScaleRangeButton.Size = new System.Drawing.Size(23, 22);
            this.AddScaleRangeButton.Text = "toolStripButton1";
            this.AddScaleRangeButton.ToolTipText = "Add a scale range";
            this.AddScaleRangeButton.Click += new System.EventHandler(this.AddScaleRangeButton_Click);
            // 
            // DeleteItemButton
            // 
            this.DeleteItemButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteItemButton.Enabled = false;
            this.DeleteItemButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteItemButton.Image")));
            this.DeleteItemButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteItemButton.Name = "DeleteItemButton";
            this.DeleteItemButton.Size = new System.Drawing.Size(23, 22);
            this.DeleteItemButton.Text = "toolStripButton3";
            this.DeleteItemButton.Click += new System.EventHandler(this.DeleteItemButton_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Enabled = false;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // DisplayRanges
            // 
            this.DisplayRanges.DataSetName = "DisplayRangesDataSet";
            this.DisplayRanges.Locale = new System.Globalization.CultureInfo("da-DK");
            this.DisplayRanges.Tables.AddRange(new System.Data.DataTable[] {
            this.DisplayRangesTable});
            // 
            // schemaSelector
            // 
            this.schemaSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaSelector.IsRaster = false;
            this.schemaSelector.Location = new System.Drawing.Point(16, 16);
            this.schemaSelector.Name = "schemaSelector";
            this.schemaSelector.Size = new System.Drawing.Size(767, 56);
            this.schemaSelector.TabIndex = 14;
            this.schemaSelector.GeometryChanged += new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.SchemaSelector.GeometryChangedDelegate(this.schemaSelector_GeometryChanged);
            this.schemaSelector.SchemaChanged += new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.SchemaSelector.SchemaChangedDelegate(this.schemaSelector_SchemaChanged);
            // 
            // scaleRangeList
            // 
            this.scaleRangeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scaleRangeList.Location = new System.Drawing.Point(0, 25);
            this.scaleRangeList.Name = "scaleRangeList";
            this.scaleRangeList.Size = new System.Drawing.Size(793, 386);
            this.scaleRangeList.TabIndex = 3;
            this.scaleRangeList.ItemChanged += new System.EventHandler(this.scaleRangeList_ItemChanged);
            // 
            // StylizationColumn
            // 
            this.StylizationColumn.Caption = "Stylization";
            this.StylizationColumn.ColumnName = "Stylization";
            this.StylizationColumn.DataType = typeof(object);
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "PreviewPoint";
            this.dataColumn1.DataType = typeof(object);
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "PreviewLine";
            this.dataColumn2.DataType = typeof(object);
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnName = "PreviewArea";
            this.dataColumn3.DataType = typeof(object);
            // 
            // VectorLayer
            // 
            this.Controls.Add(this.ResourceGroup);
            this.Controls.Add(this.groupViewerProperties);
            this.Controls.Add(this.groupBox1);
            this.Name = "VectorLayer";
            this.Size = new System.Drawing.Size(799, 854);
            this.ResourceGroup.ResumeLayout(false);
            this.ResourceGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ViewerPropertiesTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayRangesTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyDataset)).EndInit();
            this.groupViewerProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayRanges)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void BuildScaleRange()
		{
			/*if (inUpdate)
				return;
			try
			{
				inUpdate = true;

				LayerStyleTree.Nodes.Clear();

				if (m_layer != null && m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType != null)
				{
					OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vl = (OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType) m_layer.Item;

					if (vl.VectorScaleRange.Count == 0)
						vl.VectorScaleRange.Add(new OSGeo.MapGuide.MaestroAPI.VectorScaleRangeType());

					foreach(OSGeo.MapGuide.MaestroAPI.VectorScaleRangeType vst in vl.VectorScaleRange)
					{
						TreeNode nodeScale = new TreeNode((vst.MinScaleSpecified ? vst.MinScale.ToString() : m_globalizor.Translate("Unlimited") ) + " " + m_globalizor.Translate("to") + " " + (vst.MaxScaleSpecified ? vst.MaxScale.ToString() : m_globalizor.Translate("Unlimited")));
						nodeScale.ImageIndex = nodeScale.SelectedImageIndex = 0;
						nodeScale.Tag = vst;
						LayerStyleTree.Nodes.Add(nodeScale);

						RebuildNode(nodeScale);
						nodeScale.Expand();

					}
				}

			}
			finally
			{
				inUpdate = false;
			}*/

		}

		public void UpdateDisplay()
		{
			if (inUpdate)
				return;
			try
			{
				inUpdate = true;

				if (m_layer == null)
					return;

				if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType != null)
				{
					OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vl = (OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType)m_layer.Item;

					schemaSelector.UpdateDisplay();
					ResourceGroup.Enabled = m_schemas != null;

					Link.Text = vl.Url;
					Tooltip.Text = vl.ToolTip;
					Filter.Text = vl.Filter;

                    scaleRangeList.SetItem(vl);

				}
			} 
			finally
			{
				inUpdate = false;
			}

			BuildScaleRange();
		}

		private void UpdateAfterSchema(bool setChanged, OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema schema)
		{
			bool backset = inUpdate;
			try
			{
				inUpdate = true;
				if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType != null)
				{
					OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vl = (OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType)m_layer.Item;

					if (schema != null)
					{
						ViewerPropertiesTable.Rows.Clear();
						Hashtable props = new Hashtable();
						if (vl.PropertyMapping != null)
							foreach(OSGeo.MapGuide.MaestroAPI.NameStringPairType ns in vl.PropertyMapping)
								props.Add(ns.Name, ns.Value);

						ArrayList avalibleColums = new ArrayList();

						foreach(OSGeo.MapGuide.MaestroAPI.FeatureSetColumn col in schema.Columns)
						{
							if (col.Type != OSGeo.MapGuide.MaestroAPI.Utility.GeometryType)
							{
								System.Data.DataRow row = ViewerPropertiesTable.NewRow();
								row["Visible"] = props.ContainsKey(col.Name);
								row["Display"] = col.Name;
								row["Name"] = props.ContainsKey(col.Name) ? (string)props[col.Name] : col.Name;
								ViewerPropertiesTable.Rows.Add(row);
							}
							avalibleColums.Add(col.Name);
						}
                    
						string[] m_avalibleColumns = (string[])avalibleColums.ToArray(typeof(string));
						//fontStyleEditor.SetAvalibleColumns(m_avalibleColumns);

					}
					else
					{
						ViewerPropertiesTable.Rows.Clear();
					}
				}
			}
			finally
			{
				inUpdate = backset;
			}
		}
	


		private void Filter_TextChanged(object sender, System.EventArgs e)
		{
			if (inUpdate)
				return;

			if (m_layer.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType))
			{
				OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vl = (OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType)m_layer.Item;
				vl.Filter = Filter.Text;
				m_editor.HasChanged();
			}		
		}

		private void Link_TextChanged(object sender, System.EventArgs e)
		{
			if (inUpdate)
				return;

			if (m_layer.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType))
			{
				OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vl = (OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType)m_layer.Item;
				vl.Url = Link.Text;
				m_editor.HasChanged();
			}		
		}

		private void Tooltip_TextChanged(object sender, System.EventArgs e)
		{
			if (inUpdate)
				return;

			if (m_layer.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType))
			{
				OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vl = (OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType)m_layer.Item;
				vl.ToolTip = Tooltip.Text;
				m_editor.HasChanged();
			}		
		}

		private void SelectAllButton_Click(object sender, System.EventArgs e)
		{
			foreach(System.Data.DataRow row in ViewerPropertiesTable.Rows)
				row["Visible"] = true;
		}

		private void SelectNoneButton_Click(object sender, System.EventArgs e)
		{
			foreach(System.Data.DataRow row in ViewerPropertiesTable.Rows)
				row["Visible"] = false;		
		}

		public object Resource
		{
			get { return m_layer; }
			set 
			{
				m_layer = (OSGeo.MapGuide.MaestroAPI.LayerDefinition)value;
				UpdateDisplay();
			}
		}

		private void LayerStyleTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			/*UserControl editor = null;
			if (LayerStyleTree.SelectedNode != null && LayerStyleTree.SelectedNode.Tag != null)
			{
				Type t = LayerStyleTree.SelectedNode.Tag.GetType();

				if (t == typeof(OSGeo.MapGuide.MaestroAPI.VectorScaleRangeType))
				{
					editor = scaleRangeProperties;
					scaleRangeProperties.Item = (OSGeo.MapGuide.MaestroAPI.VectorScaleRangeType)LayerStyleTree.SelectedNode.Tag;
				}
				else if (t == typeof(OSGeo.MapGuide.MaestroAPI.PointRuleType))
				{
					editor = styleRuleProperties;
					styleRuleProperties.Item = LayerStyleTree.SelectedNode.Tag;
				}
				else if (t == typeof(OSGeo.MapGuide.MaestroAPI.LineRuleType))
				{
					editor = styleRuleProperties;
					styleRuleProperties.Item = LayerStyleTree.SelectedNode.Tag;
				}
				else if (t == typeof(OSGeo.MapGuide.MaestroAPI.AreaRuleType))
				{
					editor = styleRuleProperties;
					styleRuleProperties.Item = LayerStyleTree.SelectedNode.Tag;
				}
				else if (t == typeof(OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType))
				{
					editor = pointFeatureStyleEditor;
					pointFeatureStyleEditor.Item = (OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType) LayerStyleTree.SelectedNode.Tag;
				}
				else if (t == typeof(OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection))
				{
					editor = lineFeatureStyleEditor;
					lineFeatureStyleEditor.Item = (OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection) LayerStyleTree.SelectedNode.Tag;
				}
				else if (t == typeof(OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType))
				{
					editor = areaFeatureStyleEditor;
					areaFeatureStyleEditor.Item = (OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType) LayerStyleTree.SelectedNode.Tag;
				}
				else if (t == typeof(OSGeo.MapGuide.MaestroAPI.TextSymbolType))
				{
					editor = fontStyleEditor;
					fontStyleEditor.Item = (OSGeo.MapGuide.MaestroAPI.TextSymbolType) LayerStyleTree.SelectedNode.Tag;
				}

				AddRuleButton.Enabled = 
					t == typeof(OSGeo.MapGuide.MaestroAPI.PointTypeStyleType) ||
					t == typeof(OSGeo.MapGuide.MaestroAPI.LineTypeStyleType) ||
					t == typeof(OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType);

				DeleteItemButton.Enabled =
					t == typeof(OSGeo.MapGuide.MaestroAPI.VectorScaleRangeType) ||
					t == typeof(OSGeo.MapGuide.MaestroAPI.PointRuleType) ||
					t == typeof(OSGeo.MapGuide.MaestroAPI.LineRuleType) ||
					t == typeof(OSGeo.MapGuide.MaestroAPI.AreaRuleType);

			}

			if (editor != null)
				editor.Dock = DockStyle.Fill;

			foreach(Control c in EditControlPanel.Controls)
				c.Visible = c == editor;
            */
		}

		private void SelectInverseButton_Click(object sender, System.EventArgs e)
		{
			foreach(System.Data.DataRow row in ViewerPropertiesTable.Rows)
				row["Visible"] = !((bool)row["Visible"]);
		}

		/*		private void ScaleRangeGrid_CurrentCellChanged(object sender, System.EventArgs e)
				{
					if (inUpdate)
						return;

					if (m_layer.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType))
					{
						OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vl = (OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType)m_layer.Item;
						if (ScaleRangeGrid.CurrentRowIndex < 0 || ScaleRangeGrid.CurrentRowIndex > ((DataTable)ScaleRangeGrid.DataSource).Rows.Count - 1)
							return;

						DataRow r = ((DataTable)ScaleRangeGrid.DataSource).Rows[ScaleRangeGrid.CurrentRowIndex];

						if (!r.IsNull("Stylization") && r["Stylization"] != null)
						{
							ArrayList items = (ArrayList)r["Stylization"];
							bool hasPoint = false;
							bool hasLine = false;
							bool hasArea = false;

							foreach(object o in items)
								if (o.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointTypeStyleType))
								{
									hasPoint = true;
									GeometryStylePoint.CurrentItem = o;
								}
								else if(o.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineTypeStyleType))
								{
									hasLine = true;
									GeometryStyleLine.CurrentItem = o;
								}
								else if(o.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType))
								{
									hasArea = true;
									GeometryStyleArea.CurrentItem = o;
								}

							EnablePointStyle.Checked = hasPoint;
							EnableLineStyle.Checked = hasLine;
							EnableAreaStyle.Checked = hasArea;
						}
					}
				}
				*/


		public string ResourceId
		{
			get { return m_layer.ResourceId; }
			set { m_layer.ResourceId = value; }
		}

		public bool Preview()
		{
			return false;
		}

		private void notify_Changed(object sender, System.EventArgs e)
		{
			m_editor.HasChanged();
			//RebuildNode(LayerStyleTree.SelectedNode);
		}

		private void notify_ChangedTree(object sender, System.EventArgs e)
		{
			//RebuildNode(LayerStyleTree.SelectedNode);
		}

		private void RebuildNode(TreeNode n)
		{
			if (n.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.VectorScaleRangeType))
			{
				OSGeo.MapGuide.MaestroAPI.VectorScaleRangeType vst = (OSGeo.MapGuide.MaestroAPI.VectorScaleRangeType)n.Tag;

				OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType area = null;
				OSGeo.MapGuide.MaestroAPI.LineTypeStyleType line = null;
				OSGeo.MapGuide.MaestroAPI.PointTypeStyleType point = null;

				if (vst.Items != null)
					foreach(object o in vst.Items)
						if (o as OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType != null)
							area = o as OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType;
						else if (o as OSGeo.MapGuide.MaestroAPI.LineTypeStyleType != null)
							line = o as OSGeo.MapGuide.MaestroAPI.LineTypeStyleType;
						else if (o as OSGeo.MapGuide.MaestroAPI.PointTypeStyleType != null)
							point = o as OSGeo.MapGuide.MaestroAPI.PointTypeStyleType;

				n.Nodes.Clear();

				if (point != null)
				{
					TreeNode pointNode = new TreeNode(m_globalizor.Translate("Point"));
					pointNode.ImageIndex = pointNode.SelectedImageIndex = 2;
					pointNode.Tag = point;
					pointNode.Expand();

					RebuildNode(pointNode);
					n.Nodes.Add(pointNode);
				}

				if (line != null)
				{
					TreeNode lineNode = new TreeNode(m_globalizor.Translate("Line"));
					lineNode.ImageIndex = lineNode.SelectedImageIndex = 3;
					lineNode.Tag = line;
					lineNode.Expand();

					RebuildNode(lineNode);
					n.Nodes.Add(lineNode);
				}

				if (area != null)
				{
					TreeNode areaNode = new TreeNode(m_globalizor.Translate("Area"));
					areaNode.ImageIndex = areaNode.SelectedImageIndex = 4;
					areaNode.Tag = area;
					areaNode.Expand();

					RebuildNode(areaNode);
					n.Nodes.Add(areaNode);
				}
			}
			else if (n.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointTypeStyleType))
			{
				OSGeo.MapGuide.MaestroAPI.PointTypeStyleType point = (OSGeo.MapGuide.MaestroAPI.PointTypeStyleType)n.Tag;
				n.Nodes.Clear();

				if (point.PointRule == null)
					point.PointRule = new OSGeo.MapGuide.MaestroAPI.PointRuleTypeCollection();

				foreach(OSGeo.MapGuide.MaestroAPI.PointRuleType pr in point.PointRule)
				{
					TreeNode tn = new TreeNode(pr.Filter == null || pr.Filter == "" ? m_globalizor.Translate("<default>") : pr.Filter );
					tn.Tag = pr;
					tn.ImageIndex = tn.SelectedImageIndex = 1;
					tn.Expand();
		
					RebuildNode(tn);

					n.Nodes.Add(tn);
				}

			}
			else if (n.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineTypeStyleType))
			{
				OSGeo.MapGuide.MaestroAPI.LineTypeStyleType line = (OSGeo.MapGuide.MaestroAPI.LineTypeStyleType)n.Tag;
				n.Nodes.Clear();

				if (line.LineRule == null)
					line.LineRule = new OSGeo.MapGuide.MaestroAPI.LineRuleTypeCollection();

				foreach(OSGeo.MapGuide.MaestroAPI.LineRuleType lr in line.LineRule)
				{
					TreeNode tn = new TreeNode(lr.Filter == null || lr.Filter == "" ? m_globalizor.Translate("<default>") : lr.Filter );
					tn.Tag = lr;
					tn.ImageIndex = tn.SelectedImageIndex = 1;
					tn.Expand();

					RebuildNode(tn);

					n.Nodes.Add(tn);
				}
			}
			else if (n.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType))
			{
				OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType area = (OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType)n.Tag;
				n.Nodes.Clear();

				if (area.AreaRule == null)
					area.AreaRule = new OSGeo.MapGuide.MaestroAPI.AreaRuleTypeCollection();

				foreach(OSGeo.MapGuide.MaestroAPI.AreaRuleType ar in area.AreaRule)
				{
					TreeNode tn = new TreeNode(ar.Filter == null || ar.Filter == "" ? m_globalizor.Translate("<default>") : ar.Filter );
					tn.Tag = ar;
					tn.ImageIndex = tn.SelectedImageIndex = 1;
					tn.Expand();

					RebuildNode(tn);

					n.Nodes.Add(tn);
				}
			}
			else if (n.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.PointRuleType pr = (OSGeo.MapGuide.MaestroAPI.PointRuleType) n.Tag;
				n.Nodes.Clear();

				if (pr.Item != null)
				{
					TreeNode featureNode = new TreeNode(m_globalizor.Translate("Feature"));
					featureNode.ImageIndex = featureNode.SelectedImageIndex = 0;
					featureNode.Tag = pr.Item;

					RebuildNode(featureNode);
					n.Nodes.Add(featureNode);    
				}

				if (pr.Label != null)
				{
					TreeNode labelNode = new TreeNode(m_globalizor.Translate("Label"));
					labelNode.ImageIndex = labelNode.SelectedImageIndex = 0;
					labelNode.Tag = pr.Label;

					RebuildNode(labelNode);
					n.Nodes.Add(labelNode);    
				}
			}
			else if (n.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.LineRuleType lr = (OSGeo.MapGuide.MaestroAPI.LineRuleType)n.Tag;
				n.Nodes.Clear();

				if (lr.Items != null)
				{
					TreeNode featureNode = new TreeNode(m_globalizor.Translate("Feature"));
					featureNode.ImageIndex = featureNode.SelectedImageIndex = 0;
					featureNode.Tag = lr.Items;

					RebuildNode(featureNode);
					n.Nodes.Add(featureNode);    
				}

				if (lr.Label != null)
				{
					TreeNode labelNode = new TreeNode(m_globalizor.Translate("Label"));
					labelNode.ImageIndex = labelNode.SelectedImageIndex = 0;
					labelNode.Tag = lr.Label;

					RebuildNode(labelNode);
					n.Nodes.Add(labelNode);    
				}
			}
			else if (n.Tag.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaRuleType))
			{
				OSGeo.MapGuide.MaestroAPI.AreaRuleType ar = (OSGeo.MapGuide.MaestroAPI.AreaRuleType)n.Tag;
				n.Nodes.Clear();

				if (ar.Item != null)
				{
					TreeNode featureNode = new TreeNode(m_globalizor.Translate("Feature"));
					featureNode.ImageIndex = featureNode.SelectedImageIndex = 0;
					featureNode.Tag = ar.Item;

					RebuildNode(featureNode);
					n.Nodes.Add(featureNode);    
				}

				if (ar.Label != null)
				{
					TreeNode labelNode = new TreeNode(m_globalizor.Translate("Label"));
					labelNode.ImageIndex = labelNode.SelectedImageIndex = 0;
					labelNode.Tag = ar.Label;

					RebuildNode(labelNode);
					n.Nodes.Add(labelNode);    
				}
			}


		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this, "This method is not yet implemented", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
	
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this, "This method is not yet implemented", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this, "This method is not yet implemented", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		
		}

		public void SetItem(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.LayerDefinition layer, OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription schema, Globalizator.Globalizator globalizor)
		{

			OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription oldSchema = m_schemas;
			m_editor = editor;
			m_layer = layer;
			m_schemas = schema;
			m_globalizor = globalizor;

			schemaSelector.SetItem(editor, layer, schema, globalizor);

			UpdateAfterSchema(oldSchema != m_schemas, schemaSelector.CurrentSchema);
			UpdateDisplay();
		}

		private void schemaSelector_SchemaChanged(bool fromUser, OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema schema)
		{
			UpdateAfterSchema(fromUser, schema);
		}

		private void schemaSelector_GeometryChanged(bool fromUser, string geom)
		{
			if (m_layer == null || m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType == null)
				return;

			(m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType).Geometry = geom;
		}

		private void ViewerPropertiesTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
		{
			if (m_layer == null || m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType == null || inUpdate)
				return;

			bool updated = false;
			OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vldef = m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType;

			if (vldef.PropertyMapping == null)
				vldef.PropertyMapping = new NameStringPairTypeCollection();

			foreach(DataRow dr in ViewerPropertiesTable.Rows)
			{
				string n = (string)dr["Name"];
				int i;
				for(i = 0; i < vldef.PropertyMapping.Count; i++)
					if (vldef.PropertyMapping[i].Name == n)
						break;

				if ((bool)dr["Visible"] == true)
				{
					if (i >= vldef.PropertyMapping.Count)
					{
						NameStringPairType ns = new NameStringPairType();
						ns.Name = n;
						ns.Value = (string)dr["Display"];
						vldef.PropertyMapping.Add(ns);
						updated = true;
					}
					else
					{
						if (vldef.PropertyMapping[i].Value != (string)dr["Display"])
						{
							vldef.PropertyMapping[i].Value = (string)dr["Display"];
							updated = true;
						}
					}
				}
				else
				{
					if (i < vldef.PropertyMapping.Count)
					{
						vldef.PropertyMapping.RemoveAt(i);
						updated = true;
					}
				}
			}

			if (vldef.PropertyMapping.Count == 0)
				vldef.PropertyMapping = null;

			if (updated)
				m_editor.HasChanged();
			
		}

        private void AddScaleRangeButton_Click(object sender, EventArgs e)
        {
            OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vld = m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType;
            if (vld == null)
                return;

            OSGeo.MapGuide.MaestroAPI.VectorScaleRangeType vsc = new OSGeo.MapGuide.MaestroAPI.VectorScaleRangeType();
            if (vld.VectorScaleRange == null)
            {
                vld.VectorScaleRange = new VectorScaleRangeTypeCollection();
                scaleRangeList.SetItem(vld);
            }

            vld.VectorScaleRange.Add(vsc);
            scaleRangeList.AddScaleRange(vsc);
        }


        private void DeleteItemButton_Click(object sender, EventArgs e)
        {
        }

        private void scaleRangeList_ItemChanged(object sender, EventArgs e)
        {
            m_editor.HasChanged();
        }
	}	
	
}
