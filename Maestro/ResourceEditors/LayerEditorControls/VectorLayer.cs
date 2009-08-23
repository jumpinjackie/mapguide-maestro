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
using System.Collections.Generic;

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
		private System.Windows.Forms.Button EditTooltipBtn;
		private System.Windows.Forms.Button EditLinkBtn;
		private System.Windows.Forms.Button EditFilterBtn;
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
		private System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.GroupBox ScaleRangeGroup;
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
        private ToolStripButton InsertCopyButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton OpenInWindowButton;
        private ToolStripButton CloseWindowButton;
		private EditorInterface m_editor;
        private ToolStrip toolStrip2;
        private ToolStripButton SelectAllButton;
        private ToolStripButton SelectNoneButton;
        private ToolStripButton SelectInverseButton;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton MovePropertyUpButton;
        private ToolStripButton MovePropertyDownButton;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton SortScalesByDisplayRangeButton;

        private FeatureSourceDescription.FeatureSourceSchema m_selectedSchema;

        public Globalizator.Globalizator Globalizor { get { return m_globalizor; } }

		public VectorLayer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			ViewerPropertiesTable.ColumnChanged += new DataColumnChangeEventHandler(ViewerPropertiesTable_ColumnChanged);
            scaleRangeList.Owner = this;
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
            this.schemaSelector = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.SchemaSelector();
            this.EditTooltipBtn = new System.Windows.Forms.Button();
            this.EditLinkBtn = new System.Windows.Forms.Button();
            this.EditFilterBtn = new System.Windows.Forms.Button();
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
            this.StylizationColumn = new System.Data.DataColumn();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.PropertyDataset = new System.Data.DataSet();
            this.LayerStyleImages = new System.Windows.Forms.ImageList(this.components);
            this.groupViewerProperties = new System.Windows.Forms.GroupBox();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridBoolColumn1 = new System.Windows.Forms.DataGridBoolColumn();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.SelectAllButton = new System.Windows.Forms.ToolStripButton();
            this.SelectNoneButton = new System.Windows.Forms.ToolStripButton();
            this.SelectInverseButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MovePropertyUpButton = new System.Windows.Forms.ToolStripButton();
            this.MovePropertyDownButton = new System.Windows.Forms.ToolStripButton();
            this.ScaleRangeGroup = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.scaleRangeList = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.ScaleRangeList();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddScaleRangeButton = new System.Windows.Forms.ToolStripButton();
            this.DeleteItemButton = new System.Windows.Forms.ToolStripButton();
            this.InsertCopyButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.SortScalesByDisplayRangeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.OpenInWindowButton = new System.Windows.Forms.ToolStripButton();
            this.CloseWindowButton = new System.Windows.Forms.ToolStripButton();
            this.DisplayRanges = new System.Data.DataSet();
            this.ResourceGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ViewerPropertiesTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayRangesTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyDataset)).BeginInit();
            this.groupViewerProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.ScaleRangeGroup.SuspendLayout();
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
            this.ResourceGroup.Controls.Add(this.schemaSelector);
            this.ResourceGroup.Controls.Add(this.EditTooltipBtn);
            this.ResourceGroup.Controls.Add(this.EditLinkBtn);
            this.ResourceGroup.Controls.Add(this.EditFilterBtn);
            this.ResourceGroup.Controls.Add(this.Tooltip);
            this.ResourceGroup.Controls.Add(this.Link);
            this.ResourceGroup.Controls.Add(this.Filter);
            this.ResourceGroup.Controls.Add(this.label6);
            this.ResourceGroup.Controls.Add(this.label5);
            this.ResourceGroup.Controls.Add(this.label4);
            this.ResourceGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.ResourceGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ResourceGroup.Location = new System.Drawing.Point(0, 0);
            this.ResourceGroup.Name = "ResourceGroup";
            this.ResourceGroup.Size = new System.Drawing.Size(799, 176);
            this.ResourceGroup.TabIndex = 16;
            this.ResourceGroup.TabStop = false;
            this.ResourceGroup.Text = "Resource settings";
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
            // EditTooltipBtn
            // 
            this.EditTooltipBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EditTooltipBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.EditTooltipBtn.Location = new System.Drawing.Point(759, 144);
            this.EditTooltipBtn.Name = "EditTooltipBtn";
            this.EditTooltipBtn.Size = new System.Drawing.Size(24, 20);
            this.EditTooltipBtn.TabIndex = 13;
            this.EditTooltipBtn.Text = "...";
            this.EditTooltipBtn.Click += new System.EventHandler(this.EditTooltipBtn_Click);
            // 
            // EditLinkBtn
            // 
            this.EditLinkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EditLinkBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.EditLinkBtn.Location = new System.Drawing.Point(759, 112);
            this.EditLinkBtn.Name = "EditLinkBtn";
            this.EditLinkBtn.Size = new System.Drawing.Size(24, 20);
            this.EditLinkBtn.TabIndex = 12;
            this.EditLinkBtn.Text = "...";
            this.EditLinkBtn.Click += new System.EventHandler(this.EditLinkBtn_Click);
            // 
            // EditFilterBtn
            // 
            this.EditFilterBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EditFilterBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.EditFilterBtn.Location = new System.Drawing.Point(759, 80);
            this.EditFilterBtn.Name = "EditFilterBtn";
            this.EditFilterBtn.Size = new System.Drawing.Size(24, 20);
            this.EditFilterBtn.TabIndex = 11;
            this.EditFilterBtn.Text = "...";
            this.EditFilterBtn.Click += new System.EventHandler(this.EditFilterBtn_Click);
            // 
            // Tooltip
            // 
            this.Tooltip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Tooltip.Location = new System.Drawing.Point(120, 144);
            this.Tooltip.Name = "Tooltip";
            this.Tooltip.Size = new System.Drawing.Size(639, 20);
            this.Tooltip.TabIndex = 10;
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
            this.groupViewerProperties.Controls.Add(this.dataGrid1);
            this.groupViewerProperties.Controls.Add(this.toolStrip2);
            this.groupViewerProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupViewerProperties.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupViewerProperties.Location = new System.Drawing.Point(0, 176);
            this.groupViewerProperties.Name = "groupViewerProperties";
            this.groupViewerProperties.Size = new System.Drawing.Size(799, 232);
            this.groupViewerProperties.TabIndex = 17;
            this.groupViewerProperties.TabStop = false;
            this.groupViewerProperties.Text = "Properties avalible in viewer";
            // 
            // dataGrid1
            // 
            this.dataGrid1.AllowSorting = false;
            this.dataGrid1.CaptionVisible = false;
            this.dataGrid1.DataMember = "";
            this.dataGrid1.DataSource = this.ViewerPropertiesTable;
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(3, 41);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.RowHeadersVisible = false;
            this.dataGrid1.Size = new System.Drawing.Size(793, 188);
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
            this.dataGridTextBoxColumn2,
            this.dataGridTextBoxColumn1});
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
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "Name";
            this.dataGridTextBoxColumn2.MappingName = "Name";
            this.dataGridTextBoxColumn2.ReadOnly = true;
            this.dataGridTextBoxColumn2.Width = 75;
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Display";
            this.dataGridTextBoxColumn1.MappingName = "Display";
            this.dataGridTextBoxColumn1.Width = 75;
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectAllButton,
            this.SelectNoneButton,
            this.SelectInverseButton,
            this.toolStripSeparator2,
            this.MovePropertyUpButton,
            this.MovePropertyDownButton});
            this.toolStrip2.Location = new System.Drawing.Point(3, 16);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(793, 25);
            this.toolStrip2.TabIndex = 4;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // SelectAllButton
            // 
            this.SelectAllButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SelectAllButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectAllButton.Image")));
            this.SelectAllButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectAllButton.Name = "SelectAllButton";
            this.SelectAllButton.Size = new System.Drawing.Size(23, 22);
            this.SelectAllButton.ToolTipText = "Make all properties visible";
            this.SelectAllButton.Click += new System.EventHandler(this.SelectAllButton_Click);
            // 
            // SelectNoneButton
            // 
            this.SelectNoneButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SelectNoneButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectNoneButton.Image")));
            this.SelectNoneButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectNoneButton.Name = "SelectNoneButton";
            this.SelectNoneButton.Size = new System.Drawing.Size(23, 22);
            this.SelectNoneButton.ToolTipText = "Make no properties visible";
            this.SelectNoneButton.Click += new System.EventHandler(this.SelectNoneButton_Click);
            // 
            // SelectInverseButton
            // 
            this.SelectInverseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SelectInverseButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectInverseButton.Image")));
            this.SelectInverseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectInverseButton.Name = "SelectInverseButton";
            this.SelectInverseButton.Size = new System.Drawing.Size(23, 22);
            this.SelectInverseButton.ToolTipText = "Reverse the visibility of the properties";
            this.SelectInverseButton.Click += new System.EventHandler(this.SelectInverseButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // MovePropertyUpButton
            // 
            this.MovePropertyUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MovePropertyUpButton.Image = ((System.Drawing.Image)(resources.GetObject("MovePropertyUpButton.Image")));
            this.MovePropertyUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MovePropertyUpButton.Name = "MovePropertyUpButton";
            this.MovePropertyUpButton.Size = new System.Drawing.Size(23, 22);
            this.MovePropertyUpButton.ToolTipText = "Move the highlighted property up";
            this.MovePropertyUpButton.Click += new System.EventHandler(this.MovePropertyUpButton_Click);
            // 
            // MovePropertyDownButton
            // 
            this.MovePropertyDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MovePropertyDownButton.Image = ((System.Drawing.Image)(resources.GetObject("MovePropertyDownButton.Image")));
            this.MovePropertyDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MovePropertyDownButton.Name = "MovePropertyDownButton";
            this.MovePropertyDownButton.Size = new System.Drawing.Size(23, 22);
            this.MovePropertyDownButton.ToolTipText = "Move the highlighted property down";
            this.MovePropertyDownButton.Click += new System.EventHandler(this.MovePropertyDownButton_Click);
            // 
            // ScaleRangeGroup
            // 
            this.ScaleRangeGroup.Controls.Add(this.panel1);
            this.ScaleRangeGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScaleRangeGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ScaleRangeGroup.Location = new System.Drawing.Point(0, 408);
            this.ScaleRangeGroup.Name = "ScaleRangeGroup";
            this.ScaleRangeGroup.Size = new System.Drawing.Size(799, 446);
            this.ScaleRangeGroup.TabIndex = 18;
            this.ScaleRangeGroup.TabStop = false;
            this.ScaleRangeGroup.Text = "Layer style";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.scaleRangeList);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(793, 427);
            this.panel1.TabIndex = 2;
            // 
            // scaleRangeList
            // 
            this.scaleRangeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scaleRangeList.Location = new System.Drawing.Point(0, 25);
            this.scaleRangeList.Name = "scaleRangeList";
            this.scaleRangeList.Owner = null;
            this.scaleRangeList.Size = new System.Drawing.Size(793, 402);
            this.scaleRangeList.TabIndex = 3;
            this.scaleRangeList.Load += new System.EventHandler(this.scaleRangeList_Load);
            this.scaleRangeList.SelectionChanged += new System.EventHandler(this.scaleRangeList_SelectionChanged);
            this.scaleRangeList.ItemChanged += new System.EventHandler(this.scaleRangeList_ItemChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddScaleRangeButton,
            this.DeleteItemButton,
            this.InsertCopyButton,
            this.toolStripSeparator3,
            this.SortScalesByDisplayRangeButton,
            this.toolStripSeparator1,
            this.OpenInWindowButton,
            this.CloseWindowButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(793, 25);
            this.toolStrip1.TabIndex = 2;
            // 
            // AddScaleRangeButton
            // 
            this.AddScaleRangeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddScaleRangeButton.Image = ((System.Drawing.Image)(resources.GetObject("AddScaleRangeButton.Image")));
            this.AddScaleRangeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddScaleRangeButton.Name = "AddScaleRangeButton";
            this.AddScaleRangeButton.Size = new System.Drawing.Size(23, 22);
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
            this.DeleteItemButton.Text = "Delete the selected scalerange";
            this.DeleteItemButton.Click += new System.EventHandler(this.DeleteItemButton_Click);
            // 
            // InsertCopyButton
            // 
            this.InsertCopyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.InsertCopyButton.Enabled = false;
            this.InsertCopyButton.Image = ((System.Drawing.Image)(resources.GetObject("InsertCopyButton.Image")));
            this.InsertCopyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.InsertCopyButton.Name = "InsertCopyButton";
            this.InsertCopyButton.Size = new System.Drawing.Size(23, 22);
            this.InsertCopyButton.Text = "Append a copy of the selected scale";
            this.InsertCopyButton.Click += new System.EventHandler(this.InsertCopyButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // SortScalesByDisplayRangeButton
            // 
            this.SortScalesByDisplayRangeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SortScalesByDisplayRangeButton.Image = ((System.Drawing.Image)(resources.GetObject("SortScalesByDisplayRangeButton.Image")));
            this.SortScalesByDisplayRangeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SortScalesByDisplayRangeButton.Name = "SortScalesByDisplayRangeButton";
            this.SortScalesByDisplayRangeButton.Size = new System.Drawing.Size(23, 22);
            this.SortScalesByDisplayRangeButton.ToolTipText = "Sort the scale ranges by display range";
            this.SortScalesByDisplayRangeButton.Click += new System.EventHandler(this.SortScalesByDisplayRangeButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // OpenInWindowButton
            // 
            this.OpenInWindowButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenInWindowButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenInWindowButton.Image")));
            this.OpenInWindowButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenInWindowButton.Name = "OpenInWindowButton";
            this.OpenInWindowButton.Size = new System.Drawing.Size(23, 22);
            this.OpenInWindowButton.ToolTipText = "Show the scale setup in a seperate window";
            this.OpenInWindowButton.Click += new System.EventHandler(this.OpenInWindowButton_Click);
            // 
            // CloseWindowButton
            // 
            this.CloseWindowButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CloseWindowButton.Image = ((System.Drawing.Image)(resources.GetObject("CloseWindowButton.Image")));
            this.CloseWindowButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CloseWindowButton.Name = "CloseWindowButton";
            this.CloseWindowButton.Size = new System.Drawing.Size(23, 22);
            this.CloseWindowButton.ToolTipText = "Click here to restore the control in the editor";
            this.CloseWindowButton.Visible = false;
            this.CloseWindowButton.Click += new System.EventHandler(this.CloseWindowButton_Click);
            // 
            // DisplayRanges
            // 
            this.DisplayRanges.DataSetName = "DisplayRangesDataSet";
            this.DisplayRanges.Locale = new System.Globalization.CultureInfo("da-DK");
            this.DisplayRanges.Tables.AddRange(new System.Data.DataTable[] {
            this.DisplayRangesTable});
            // 
            // VectorLayer
            // 
            this.Controls.Add(this.ScaleRangeGroup);
            this.Controls.Add(this.groupViewerProperties);
            this.Controls.Add(this.ResourceGroup);
            this.Name = "VectorLayer";
            this.Size = new System.Drawing.Size(799, 854);
            this.ResourceGroup.ResumeLayout(false);
            this.ResourceGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ViewerPropertiesTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayRangesTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyDataset)).EndInit();
            this.groupViewerProperties.ResumeLayout(false);
            this.groupViewerProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ScaleRangeGroup.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayRanges)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

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
                    scaleRangeList.ResizeAuto();
				}
			} 
			finally
			{
				inUpdate = false;
			}

		}

		private void UpdateAfterSchema(bool setChanged, OSGeo.MapGuide.MaestroAPI.FeatureSourceDescription.FeatureSourceSchema schema)
		{
			bool backset = inUpdate;
			try
			{
				inUpdate = true;
                m_selectedSchema = schema;
				if (m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType != null)
				{
					OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vl = (OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType)m_layer.Item;

					if (schema != null)
					{
						ViewerPropertiesTable.Rows.Clear();

                        //Build list of avalible columns
                        List<string> schemaCols = new List<string>();
                        foreach (OSGeo.MapGuide.MaestroAPI.FeatureSetColumn col in schema.Columns)
                            if (col.Type != OSGeo.MapGuide.MaestroAPI.Utility.GeometryType)
                                schemaCols.Add(col.Name);

                        //Insert mapped properties first, and preserve their order
						if (vl.PropertyMapping != null)
                            foreach (OSGeo.MapGuide.MaestroAPI.NameStringPairType ns in vl.PropertyMapping)
                                if (schemaCols.Contains(ns.Name))
                                {
                                    schemaCols.Remove(ns.Name);
                                    System.Data.DataRow row = ViewerPropertiesTable.NewRow();
                                    row["Visible"] = true;
                                    row["Name"] = ns.Name;
                                    row["Display"] = ns.Value;
                                    ViewerPropertiesTable.Rows.Add(row);
                                }

                        //Insert the remaining columns, and preserver schema order
                        foreach (string s in schemaCols)
                        {
                            System.Data.DataRow row = ViewerPropertiesTable.NewRow();
                            row["Visible"] = false;
                            row["Name"] = s;
                            row["Display"] = s;
                            ViewerPropertiesTable.Rows.Add(row);
                        }
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

        public EditorInterface Editor { get { return m_editor; } }

        public FeatureSourceDescription.FeatureSourceSchema Schema { get { return m_selectedSchema; } }

		public object Resource
		{
			get { return m_layer; }
			set 
			{
				m_layer = (OSGeo.MapGuide.MaestroAPI.LayerDefinition)value;
				UpdateDisplay();
			}
		}


		private void SelectInverseButton_Click(object sender, System.EventArgs e)
		{
			foreach(System.Data.DataRow row in ViewerPropertiesTable.Rows)
				row["Visible"] = !((bool)row["Visible"]);
		}


		public string ResourceId
		{
			get { return m_layer.ResourceId; }
			set { m_layer.ResourceId = value; }
		}

		public bool Preview()
		{
			return false;
		}

        public string[] GetAvalibleColumns()
        {
            if (m_selectedSchema == null)
                return new string[0];

            string[] res = new string[m_selectedSchema.Columns.Length];
            for (int i = 0; i < m_selectedSchema.Columns.Length; i++)
                res[i] = m_selectedSchema.Columns[i].Name;

            return res;
        }

        public string EditExpression(string entry)
        {
            if (m_selectedSchema == null)
            {
                MessageBox.Show(this, "Cannot display editor, because no schema was selected", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            VectorLayerDefinitionType vldef = m_layer.Item as VectorLayerDefinitionType;

            FeatureSource fs = null;
            try
            {
                fs = m_editor.CurrentConnection.GetFeatureSource(vldef.ResourceId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to read featuresource: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            return m_editor.EditExpression(entry, m_selectedSchema, fs.Provider);
        }

		private void EditFilterBtn_Click(object sender, System.EventArgs e)
		{
            string tmp = EditExpression(Filter.Text);
            if (tmp != null)
                Filter.Text = tmp;
		}

		private void EditLinkBtn_Click(object sender, System.EventArgs e)
		{
            string tmp = EditExpression(Link.Text);
            if (tmp != null)
                Link.Text = tmp;
		}

		private void EditTooltipBtn_Click(object sender, System.EventArgs e)
		{
            string tmp = EditExpression(Tooltip.Text);
            if (tmp != null)
                Tooltip.Text = tmp;
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
            if (fromUser)
                m_editor.HasChanged();
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
            AddScaleRange(DefaultItemGenerator.CreateVectorScaleRangeType());
        }

        private void AddScaleRange(VectorScaleRangeType vsc)
        {
            OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vld = m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType;
            if (vld == null)
                return; 
            
            if (vld.VectorScaleRange == null)
            {
                vld.VectorScaleRange = new VectorScaleRangeTypeCollection();
                scaleRangeList.SetItem(vld);
            }

            vld.VectorScaleRange.Add(vsc);
            Control c = scaleRangeList.AddScaleRange(vsc);
            scaleRangeList.ResizeAuto();
            m_editor.HasChanged();

            try { c.Focus(); }
            catch { }
        }

        private void DeleteItemButton_Click(object sender, EventArgs e)
        {
            if (scaleRangeList.SelectedItem == null)
                return;
            VectorScaleRangeType vsc = scaleRangeList.SelectedItem;
            VectorLayerDefinitionType vldef = m_layer.Item as VectorLayerDefinitionType;
            scaleRangeList.RemoveScaleRange(scaleRangeList.SelectedItem);
            for (int i = 0; i < vldef.VectorScaleRange.Count; i++)
                if (vldef.VectorScaleRange[i] == vsc)
                {
                    vldef.VectorScaleRange.RemoveAt(i);
                    break;
                }

            m_editor.HasChanged();
        }

        private void scaleRangeList_ItemChanged(object sender, EventArgs e)
        {
            scaleRangeList.ResizeAuto();
            m_editor.HasChanged();
        }

        private void scaleRangeList_SelectionChanged(object sender, EventArgs e)
        {
            DeleteItemButton.Enabled = InsertCopyButton.Enabled = scaleRangeList.SelectedItem != null;
        }

        private void scaleRangeList_Load(object sender, EventArgs e)
        {

        }

        private void InsertCopyButton_Click(object sender, EventArgs e)
        {
            if (scaleRangeList.SelectedItem == null)
                return;

            AddScaleRange((VectorScaleRangeType) Utility.XmlDeepCopy(scaleRangeList.SelectedItem));

        }

        private void OpenInWindowButton_Click(object sender, EventArgs e)
        {
            ScaleControls.EditorTemplateForm dlg = new OSGeo.MapGuide.Maestro.ResourceEditors.LayerEditorControls.ScaleControls.EditorTemplateForm();

            VectorLayerDefinitionType vldef = (VectorLayerDefinitionType)Utility.XmlDeepCopy(m_layer.Item as VectorLayerDefinitionType);

            Control c = ScaleRangeGroup;
            this.Controls.Remove(c);

            dlg.ItemPanel.Controls.Add(c);
            dlg.FormBorderStyle = FormBorderStyle.Sizable;
            dlg.RefreshSize();
            OpenInWindowButton.Visible = false;
            CloseWindowButton.Visible = true;
            dlg.WindowState = FormWindowState.Maximized;

            if (dlg.ShowDialog() != DialogResult.OK)
            {
                m_layer.Item = vldef;
                UpdateDisplay();
            }

            dlg.ItemPanel.Controls.Remove(c);
            this.Controls.Add(c);
            c.BringToFront();

            OpenInWindowButton.Visible = true;
            CloseWindowButton.Visible = false;

        }

        private void CloseWindowButton_Click(object sender, EventArgs e)
        {
            if (ScaleRangeGroup.Parent.Parent as ScaleControls.EditorTemplateForm != null)
            {
                (ScaleRangeGroup.Parent.Parent as ScaleControls.EditorTemplateForm).DialogResult = DialogResult.OK;
                (ScaleRangeGroup.Parent.Parent as ScaleControls.EditorTemplateForm).Close();
            }
        }

        private void MovePropertyUpButton_Click(object sender, EventArgs e)
        {
            MoveRow(true);
        }

        private void MovePropertyDownButton_Click(object sender, EventArgs e)
        {
            MoveRow(false);
        }

        private void MoveRow(bool up)
        {
            try
            {
                inUpdate = true;
                int row = dataGrid1.CurrentRowIndex;
                if (row < 0 || row >= ViewerPropertiesTable.Rows.Count)
                    return;
                System.Data.DataRow r = ViewerPropertiesTable.Rows[row];
                object[] data = r.ItemArray;
                ViewerPropertiesTable.Rows.Remove(r);

                row += up ? -1 : 1;
                row = Math.Min(Math.Max(0, row), ViewerPropertiesTable.Rows.Count);
                ViewerPropertiesTable.Rows.InsertAt(r, row);
                r.ItemArray = data;
                dataGrid1.CurrentRowIndex = row;
                try { dataGrid1.Focus(); }
                catch { }

                if (m_layer.Item is OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType)
                {
                    OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vl = (OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType)m_layer.Item;
                    vl.PropertyMapping.Clear();
                    foreach (System.Data.DataRow dr in ViewerPropertiesTable.Rows)
                        if ((bool)dr["Visible"] == true)
                        {
                            NameStringPairType pair = new NameStringPairType();
                            pair.Name = (string)dr["Name"];
                            pair.Value = (string)dr["Display"];
                            vl.PropertyMapping.Add(pair);
                        }
                }

                m_editor.HasChanged();
            }
            finally
            {
                inUpdate = false;
            }
        }

        private void SortScalesByDisplayRangeButton_Click(object sender, EventArgs e)
        {
            OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType vld = m_layer.Item as OSGeo.MapGuide.MaestroAPI.VectorLayerDefinitionType;
            if (vld == null)
                return;

            if (vld.VectorScaleRange == null || vld.VectorScaleRange.Count == 0)
                return;

            List<VectorScaleRangeType> ranges = new List<VectorScaleRangeType>();
            foreach(VectorScaleRangeType sc in vld.VectorScaleRange)
                ranges.Add(sc);
            ranges.Sort(new ScaleRangeSorter());

            vld.VectorScaleRange.Clear();
            foreach (VectorScaleRangeType sc in ranges)
                vld.VectorScaleRange.Add(sc);

            //Refresh display
            scaleRangeList.SetItem(vld);
            scaleRangeList.ResizeAuto();

            m_editor.HasChanged();
        }

        /// <summary>
        /// Sort helper used to sort the scale ranges
        /// </summary>
        private class ScaleRangeSorter : IComparer, IComparer<VectorScaleRangeType>
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                if (x is MaestroAPI.VectorScaleRangeType && y is MaestroAPI.VectorScaleRangeType)
                {
                    MaestroAPI.VectorScaleRangeType vx = (MaestroAPI.VectorScaleRangeType)x;
                    MaestroAPI.VectorScaleRangeType vy = (MaestroAPI.VectorScaleRangeType)y;

                    double minX = vx.MinScaleSpecified ? vx.MinScale : 0;
                    double maxX = vx.MaxScaleSpecified ? vx.MaxScale : double.MaxValue;
                    double minY = vy.MinScaleSpecified ? vy.MinScale : 0;
                    double maxY = vy.MaxScaleSpecified ? vy.MaxScale : double.MaxValue;

                    if (minX == minY)
                        if (maxX == maxY)
                            return 0;
                        else
                            return maxX > maxY ? 1 : -1;
                    else
                        return minX > minY ? 1 : -1;
                }
                else
                    return 0;
            }

            #endregion

            #region IComparer<VectorScaleRangeType> Members

            public int Compare(VectorScaleRangeType x, VectorScaleRangeType y)
            {
                return this.Compare((object)x, (object)y);
            }

            #endregion
        }

	}	
	
}
