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

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for GeometryStyleEditor.
	/// </summary>
	public class GeometryStyleEditor : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel ThemePanel;
		private System.Windows.Forms.Panel BasicPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button StyleFeatureButton;
		private System.Windows.Forms.Button StyleLabelButton;
		private System.Windows.Forms.Button ThemeModeButton;
		private System.Windows.Forms.Button BasicModeButton;
		private System.Windows.Forms.ToolBar ThemeToolBar;
		private System.Windows.Forms.ToolBarButton AddRuleButton;
		private System.Windows.Forms.ToolBarButton ThemeButton;
		private System.Windows.Forms.ToolBarButton CopyButton;
		private System.Windows.Forms.ToolBarButton DeleteButton;
		private System.Windows.Forms.ToolBarButton MoveUpButton;
		private System.Windows.Forms.ToolBarButton MoveDownButton;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.DataGrid StyleGrid;
		private System.Data.DataSet StyleDataset;
		private System.Data.DataTable StyleTable;
		private System.Data.DataColumn ConditionColumn;
		private System.Data.DataColumn StyleColumn;
		private System.Data.DataColumn FeatureLabelColumn;
		private System.Data.DataColumn LegendLabelColumn;
		private System.Windows.Forms.DataGridTableStyle FeatureStyle;
		private System.Windows.Forms.DataGridTextBoxColumn ConditionColumnStyle;
		private PreviewFeatureStyleColumn StyleColumnStyle;
		private PreviewFeatureStyleColumn FeatureLabelColumnStyle;
		private System.ComponentModel.IContainer components;
		
		private object m_item;
		private const int m_themeheight = 200;
		private System.Windows.Forms.PictureBox labelPreview;
		private System.Windows.Forms.PictureBox stylePreview;
		private const int m_basicheight = 48;
		private System.Data.DataColumn dataColumn1;
		private System.Windows.Forms.ImageList toolbarImages;
		private System.Windows.Forms.DataGridTextBoxColumn LegendLabelColumnStyle;
		private string[] m_avalibleColumns;

		public GeometryStyleEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			StyleTable.RowChanged += new DataRowChangeEventHandler(StyleTable_RowChanged);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GeometryStyleEditor));
			this.ThemePanel = new System.Windows.Forms.Panel();
			this.StyleGrid = new System.Windows.Forms.DataGrid();
			this.StyleTable = new System.Data.DataTable();
			this.ConditionColumn = new System.Data.DataColumn();
			this.StyleColumn = new System.Data.DataColumn();
			this.FeatureLabelColumn = new System.Data.DataColumn();
			this.LegendLabelColumn = new System.Data.DataColumn();
			this.dataColumn1 = new System.Data.DataColumn();
			this.FeatureStyle = new System.Windows.Forms.DataGridTableStyle();
			this.ConditionColumnStyle = new System.Windows.Forms.DataGridTextBoxColumn();
			this.StyleColumnStyle = new ResourceEditors.PreviewFeatureStyleColumn();
			this.FeatureLabelColumnStyle = new ResourceEditors.PreviewFeatureStyleColumn();
			this.ThemeToolBar = new System.Windows.Forms.ToolBar();
			this.ThemeButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.AddRuleButton = new System.Windows.Forms.ToolBarButton();
			this.CopyButton = new System.Windows.Forms.ToolBarButton();
			this.DeleteButton = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.MoveUpButton = new System.Windows.Forms.ToolBarButton();
			this.MoveDownButton = new System.Windows.Forms.ToolBarButton();
			this.toolbarImages = new System.Windows.Forms.ImageList(this.components);
			this.BasicModeButton = new System.Windows.Forms.Button();
			this.BasicPanel = new System.Windows.Forms.Panel();
			this.ThemeModeButton = new System.Windows.Forms.Button();
			this.StyleLabelButton = new System.Windows.Forms.Button();
			this.StyleFeatureButton = new System.Windows.Forms.Button();
			this.labelPreview = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.stylePreview = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.StyleDataset = new System.Data.DataSet();
			this.LegendLabelColumnStyle = new System.Windows.Forms.DataGridTextBoxColumn();
			this.ThemePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.StyleGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.StyleTable)).BeginInit();
			this.BasicPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.StyleDataset)).BeginInit();
			this.SuspendLayout();
			// 
			// ThemePanel
			// 
			this.ThemePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ThemePanel.Controls.Add(this.StyleGrid);
			this.ThemePanel.Controls.Add(this.ThemeToolBar);
			this.ThemePanel.Controls.Add(this.BasicModeButton);
			this.ThemePanel.Location = new System.Drawing.Point(0, 0);
			this.ThemePanel.Name = "ThemePanel";
			this.ThemePanel.Size = new System.Drawing.Size(664, 200);
			this.ThemePanel.TabIndex = 0;
			// 
			// StyleGrid
			// 
			this.StyleGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.StyleGrid.CaptionVisible = false;
			this.StyleGrid.DataMember = "";
			this.StyleGrid.DataSource = this.StyleTable;
			this.StyleGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.StyleGrid.Location = new System.Drawing.Point(0, 32);
			this.StyleGrid.Name = "StyleGrid";
			this.StyleGrid.Size = new System.Drawing.Size(664, 128);
			this.StyleGrid.TabIndex = 9;
			this.StyleGrid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																								  this.FeatureStyle});
			this.StyleGrid.Click += new System.EventHandler(this.StyleGrid_Click);
			this.StyleGrid.DoubleClick += new System.EventHandler(this.StyleGrid_DoubleClick);
			// 
			// StyleTable
			// 
			this.StyleTable.Columns.AddRange(new System.Data.DataColumn[] {
																			  this.ConditionColumn,
																			  this.StyleColumn,
																			  this.FeatureLabelColumn,
																			  this.LegendLabelColumn,
																			  this.dataColumn1});
			this.StyleTable.TableName = "StyleTable";
			// 
			// ConditionColumn
			// 
			this.ConditionColumn.Caption = "Condition";
			this.ConditionColumn.ColumnName = "Condition";
			// 
			// StyleColumn
			// 
			this.StyleColumn.Caption = "Style";
			this.StyleColumn.ColumnName = "Style";
			this.StyleColumn.DataType = typeof(object);
			// 
			// FeatureLabelColumn
			// 
			this.FeatureLabelColumn.Caption = "Feature Label";
			this.FeatureLabelColumn.ColumnName = "FeatureLabel";
			this.FeatureLabelColumn.DataType = typeof(object);
			// 
			// LegendLabelColumn
			// 
			this.LegendLabelColumn.Caption = "Legend Label";
			this.LegendLabelColumn.ColumnName = "LegendLabel";
			// 
			// dataColumn1
			// 
			this.dataColumn1.ColumnName = "Item";
			this.dataColumn1.DataType = typeof(object);
			// 
			// FeatureStyle
			// 
			this.FeatureStyle.DataGrid = this.StyleGrid;
			this.FeatureStyle.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																										   this.ConditionColumnStyle,
																										   this.StyleColumnStyle,
																										   this.FeatureLabelColumnStyle,
																										   this.LegendLabelColumnStyle});
			this.FeatureStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.FeatureStyle.MappingName = "StyleTable";
			this.FeatureStyle.PreferredColumnWidth = 100;
			// 
			// ConditionColumnStyle
			// 
			this.ConditionColumnStyle.Format = "";
			this.ConditionColumnStyle.FormatInfo = null;
			this.ConditionColumnStyle.HeaderText = "Condition";
			this.ConditionColumnStyle.MappingName = "Condition";
			this.ConditionColumnStyle.Width = 75;
			// 
			// StyleColumnStyle
			// 
			this.StyleColumnStyle.Format = "";
			this.StyleColumnStyle.FormatInfo = null;
			this.StyleColumnStyle.HeaderText = "Style";
			this.StyleColumnStyle.MappingName = "Style";
			this.StyleColumnStyle.ReadOnly = true;
			this.StyleColumnStyle.Width = 75;
			// 
			// FeatureLabelColumnStyle
			// 
			this.FeatureLabelColumnStyle.Format = "";
			this.FeatureLabelColumnStyle.FormatInfo = null;
			this.FeatureLabelColumnStyle.HeaderText = "Feature Label";
			this.FeatureLabelColumnStyle.MappingName = "FeatureLabel";
			this.FeatureLabelColumnStyle.ReadOnly = true;
			this.FeatureLabelColumnStyle.Width = 75;
			// 
			// ThemeToolBar
			// 
			this.ThemeToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							this.ThemeButton,
																							this.toolBarButton1,
																							this.AddRuleButton,
																							this.CopyButton,
																							this.DeleteButton,
																							this.toolBarButton2,
																							this.MoveUpButton,
																							this.MoveDownButton});
			this.ThemeToolBar.DropDownArrows = true;
			this.ThemeToolBar.ImageList = this.toolbarImages;
			this.ThemeToolBar.Location = new System.Drawing.Point(0, 0);
			this.ThemeToolBar.Name = "ThemeToolBar";
			this.ThemeToolBar.ShowToolTips = true;
			this.ThemeToolBar.Size = new System.Drawing.Size(664, 28);
			this.ThemeToolBar.TabIndex = 8;
			this.ThemeToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.ThemeToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.ThemeToolBar_ButtonClick);
			// 
			// ThemeButton
			// 
			this.ThemeButton.ImageIndex = 5;
			this.ThemeButton.Text = "Theme...";
			this.ThemeButton.ToolTipText = "Creates a number of rules using a wizard";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// AddRuleButton
			// 
			this.AddRuleButton.ImageIndex = 0;
			this.AddRuleButton.Text = "New";
			this.AddRuleButton.ToolTipText = "Add a new rule";
			// 
			// CopyButton
			// 
			this.CopyButton.ImageIndex = 1;
			this.CopyButton.Text = "Copy";
			this.CopyButton.ToolTipText = "Copy an existing rule";
			// 
			// DeleteButton
			// 
			this.DeleteButton.ImageIndex = 2;
			this.DeleteButton.Text = "Delete";
			this.DeleteButton.ToolTipText = "Delete the selected rule";
			// 
			// toolBarButton2
			// 
			this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// MoveUpButton
			// 
			this.MoveUpButton.ImageIndex = 4;
			this.MoveUpButton.Text = "Move up";
			this.MoveUpButton.ToolTipText = "Moves the selected rule up";
			// 
			// MoveDownButton
			// 
			this.MoveDownButton.ImageIndex = 3;
			this.MoveDownButton.Text = "Move down";
			this.MoveDownButton.ToolTipText = "Moves the selected rule down";
			// 
			// toolbarImages
			// 
			this.toolbarImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.toolbarImages.ImageSize = new System.Drawing.Size(16, 16);
			this.toolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImages.ImageStream")));
			this.toolbarImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// BasicModeButton
			// 
			this.BasicModeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BasicModeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.BasicModeButton.Location = new System.Drawing.Point(544, 168);
			this.BasicModeButton.Name = "BasicModeButton";
			this.BasicModeButton.Size = new System.Drawing.Size(116, 24);
			this.BasicModeButton.TabIndex = 7;
			this.BasicModeButton.Text = "<<< Basic";
			this.BasicModeButton.Click += new System.EventHandler(this.BasicModeButton_Click);
			// 
			// BasicPanel
			// 
			this.BasicPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.BasicPanel.Controls.Add(this.ThemeModeButton);
			this.BasicPanel.Controls.Add(this.StyleLabelButton);
			this.BasicPanel.Controls.Add(this.StyleFeatureButton);
			this.BasicPanel.Controls.Add(this.labelPreview);
			this.BasicPanel.Controls.Add(this.label2);
			this.BasicPanel.Controls.Add(this.stylePreview);
			this.BasicPanel.Controls.Add(this.label1);
			this.BasicPanel.Location = new System.Drawing.Point(0, 0);
			this.BasicPanel.Name = "BasicPanel";
			this.BasicPanel.Size = new System.Drawing.Size(672, 48);
			this.BasicPanel.TabIndex = 1;
			// 
			// ThemeModeButton
			// 
			this.ThemeModeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ThemeModeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ThemeModeButton.Location = new System.Drawing.Point(536, 8);
			this.ThemeModeButton.Name = "ThemeModeButton";
			this.ThemeModeButton.Size = new System.Drawing.Size(116, 24);
			this.ThemeModeButton.TabIndex = 6;
			this.ThemeModeButton.Text = "Theme >>>";
			this.ThemeModeButton.Click += new System.EventHandler(this.ThemeModeButton_Click);
			// 
			// StyleLabelButton
			// 
			this.StyleLabelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.StyleLabelButton.Location = new System.Drawing.Point(384, 8);
			this.StyleLabelButton.Name = "StyleLabelButton";
			this.StyleLabelButton.Size = new System.Drawing.Size(24, 24);
			this.StyleLabelButton.TabIndex = 5;
			this.StyleLabelButton.Text = "...";
			this.StyleLabelButton.Click += new System.EventHandler(this.StyleLabelButton_Click);
			// 
			// StyleFeatureButton
			// 
			this.StyleFeatureButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.StyleFeatureButton.Location = new System.Drawing.Point(168, 8);
			this.StyleFeatureButton.Name = "StyleFeatureButton";
			this.StyleFeatureButton.Size = new System.Drawing.Size(24, 24);
			this.StyleFeatureButton.TabIndex = 4;
			this.StyleFeatureButton.Text = "...";
			this.StyleFeatureButton.Click += new System.EventHandler(this.StyleFeatureButton_Click);
			// 
			// labelPreview
			// 
			this.labelPreview.BackColor = System.Drawing.SystemColors.Window;
			this.labelPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelPreview.Location = new System.Drawing.Point(280, 8);
			this.labelPreview.Name = "labelPreview";
			this.labelPreview.Size = new System.Drawing.Size(96, 24);
			this.labelPreview.TabIndex = 3;
			this.labelPreview.TabStop = false;
			this.labelPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.labelPreview_Paint);
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(232, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Labels";
			// 
			// stylePreview
			// 
			this.stylePreview.BackColor = System.Drawing.SystemColors.Window;
			this.stylePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.stylePreview.Location = new System.Drawing.Point(64, 8);
			this.stylePreview.Name = "stylePreview";
			this.stylePreview.Size = new System.Drawing.Size(96, 24);
			this.stylePreview.TabIndex = 1;
			this.stylePreview.TabStop = false;
			this.stylePreview.Paint += new System.Windows.Forms.PaintEventHandler(this.stylePreview_Paint);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Style";
			// 
			// StyleDataset
			// 
			this.StyleDataset.DataSetName = "NewDataSet";
			this.StyleDataset.Locale = new System.Globalization.CultureInfo("da-DK");
			this.StyleDataset.Tables.AddRange(new System.Data.DataTable[] {
																			  this.StyleTable});
			// 
			// LegendLabelColumnStyle
			// 
			this.LegendLabelColumnStyle.Format = "";
			this.LegendLabelColumnStyle.FormatInfo = null;
			this.LegendLabelColumnStyle.HeaderText = "Legend label";
			this.LegendLabelColumnStyle.MappingName = "LegendLabel";
			// 
			// GeometryStyleEditor
			// 
			this.Controls.Add(this.BasicPanel);
			this.Controls.Add(this.ThemePanel);
			this.Name = "GeometryStyleEditor";
			this.Size = new System.Drawing.Size(664, 200);
			this.ThemePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.StyleGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.StyleTable)).EndInit();
			this.BasicPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.StyleDataset)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void ThemeModeButton_Click(object sender, System.EventArgs e)
		{
			BasicPanel.Visible = false;
			ThemePanel.Visible = true;
			ThemePanel.Height = m_themeheight;

			this.AutoScrollMinSize = new Size(this.AutoScrollMinSize.Width, m_themeheight);
			this.Height = m_themeheight;
			if (this.Parent != null)
				this.Parent.Height = m_themeheight + (this.Parent.Height - this.Parent.ClientRectangle.Height) + this.Top + 10;
		}

		private void BasicModeButton_Click(object sender, System.EventArgs e)
		{
			BasicPanel.Visible = true;
			ThemePanel.Visible = false;
		
			this.AutoScrollMinSize = new Size(this.AutoScrollMinSize.Width, m_basicheight);
			this.Height = m_basicheight;
			if (this.Parent != null)
				this.Parent.Height = m_basicheight + (this.Parent.Height - this.Parent.ClientRectangle.Height) + this.Top + 10;
		}

		public string[] AvalibleColumns
		{
			get { return m_avalibleColumns; }
			set { m_avalibleColumns = value; }
		}

		public object CurrentItem
		{
			get { return m_item; }
			set 
			{
				m_item = value;
				if (m_item == null)
					return;
				UpdateData();
				UpdateDisplay();
			}
		}

		private void UpdateData()
		{
			StyleTable.Rows.Clear();
			if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointTypeStyleType))
			{
				OSGeo.MapGuide.MaestroAPI.PointTypeStyleType pr = (OSGeo.MapGuide.MaestroAPI.PointTypeStyleType) m_item;
				foreach(OSGeo.MapGuide.MaestroAPI.PointRuleType prt in pr.PointRule)
					StyleTable.Rows.Add(new object[] {prt.Filter, prt.Item, prt.Label, prt.LegendLabel, prt} );
			}
			else if(m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineTypeStyleType))
			{
				OSGeo.MapGuide.MaestroAPI.LineTypeStyleType lr = (OSGeo.MapGuide.MaestroAPI.LineTypeStyleType) m_item;
				foreach(OSGeo.MapGuide.MaestroAPI.LineRuleType lrt in lr.LineRule)
					StyleTable.Rows.Add(new object[] {lrt.Filter, lrt.Items, lrt.Label, lrt.LegendLabel, lrt} );
			}
			else if(m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType))
			{
				OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType ar = (OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType) m_item;
				foreach(OSGeo.MapGuide.MaestroAPI.AreaRuleType art in ar.AreaRule)
					StyleTable.Rows.Add(new object[] {art.Filter, art.Item, art.Label, art.LegendLabel, art} );
			}
		}

		private void UpdateDisplay()
		{
			if (StyleTable.Rows.Count <= 1)
				BasicModeButton_Click(null, null);
			else
				ThemeModeButton_Click(null, null);
		}

		private void StyleLabelButton_Click(object sender, System.EventArgs e)
		{
			DataRow r = null;
			if (StyleGrid.CurrentRowIndex >= 0 && StyleGrid.CurrentRowIndex < StyleTable.Rows.Count)
				r = StyleTable.Rows[StyleGrid.CurrentRowIndex];
			else if (StyleTable.Rows.Count == 1)
				r = StyleTable.Rows[0];

			if (r != null)
			{
				OSGeo.MapGuide.MaestroAPI.TextSymbolType labelStyle;
				if (r["FeatureLabel"] == DBNull.Value)
					labelStyle = null;
				else
					labelStyle = (OSGeo.MapGuide.MaestroAPI.TextSymbolType) r["FeatureLabel"];

				/*GeometryStyleEditors.FontStyleEditor dlg = new ResourceEditors.GeometryStyleEditors.FontStyleEditor(labelStyle);
				if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointTypeStyleType) || r["Style"].GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType))
				{
					dlg.verticalCombo.Enabled = false;
					dlg.verticalLabel.Enabled = false;
					dlg.horizontalCombo.Enabled = false;
					dlg.horizontalLabel.Enabled = false;
				} 
				else if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineTypeStyleType))
				{
					dlg.verticalCombo.Enabled = false;
					dlg.verticalLabel.Enabled = false;
					dlg.horizontalCombo.Enabled = true;
					dlg.horizontalLabel.Enabled = true;
				}

				dlg.SetAvalibleColumns(m_avalibleColumns);

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					if (dlg.Item == null)
					{
						r["FeatureLabel"] = DBNull.Value;
					}
					else
					{
						if (r["FeatureLabel"] == DBNull.Value)
							r["FeatureLabel"] = new OSGeo.MapGuide.MaestroAPI.TextSymbolType();
						OSGeo.MapGuide.MaestroAPI.Utility.DeepCopy(dlg.Item, (OSGeo.MapGuide.MaestroAPI.TextSymbolType)r["FeatureLabel"]);
					}

					OSGeo.MapGuide.MaestroAPI.TextSymbolType txs = r["FeatureLabel"] == DBNull.Value ? null : (OSGeo.MapGuide.MaestroAPI.TextSymbolType)r["FeatureLabel"];

					if (r["Item"].GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointRuleType))
					{
						OSGeo.MapGuide.MaestroAPI.PointRuleType pr = (OSGeo.MapGuide.MaestroAPI.PointRuleType) r["Item"];
						pr.Label = txs;
					}
					else if(r["Item"].GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineRuleType))
					{
						OSGeo.MapGuide.MaestroAPI.LineRuleType lr = (OSGeo.MapGuide.MaestroAPI.LineRuleType) r["Item"];
						lr.Label = txs;
					}
					else if(r["Item"].GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaRuleType))
					{
						OSGeo.MapGuide.MaestroAPI.AreaRuleType ar = (OSGeo.MapGuide.MaestroAPI.AreaRuleType) r["Item"];
						ar.Label = txs;
					}					

					labelPreview.Refresh();
				}*/
			}
		}

		private void StyleFeatureButton_Click(object sender, System.EventArgs e)
		{
			DataRow r = null;
			if (StyleGrid.CurrentRowIndex >= 0 && StyleGrid.CurrentRowIndex < StyleTable.Rows.Count)
				r = StyleTable.Rows[StyleGrid.CurrentRowIndex];
			else if (StyleTable.Rows.Count == 1)
				r = StyleTable.Rows[0];

			if (r!= null)
			{
				if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointTypeStyleType))
				{
					OSGeo.MapGuide.MaestroAPI.PointTypeStyleType pr = (OSGeo.MapGuide.MaestroAPI.PointTypeStyleType) m_item;
					//OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.PointFeatureStyleEditor dlg = new ResourceEditors.GeometryStyleEditors.PointFeatureStyleEditor((OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType) r["Style"]);
					//if (dlg.ShowDialog(this) == DialogResult.OK)
					//	stylePreview.Refresh();
				}
				else if(m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineTypeStyleType))
				{
					OSGeo.MapGuide.MaestroAPI.LineTypeStyleType lr = (OSGeo.MapGuide.MaestroAPI.LineTypeStyleType) m_item;
					OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection col;
					if (r["Style"] == DBNull.Value)
						col = null;
					else
						col = (OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection)r["Style"];
					/*OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.LineFeatureStyleEditor dlg = new ResourceEditors.GeometryStyleEditors.LineFeatureStyleEditor(col);
					if (dlg.ShowDialog(this) == DialogResult.OK)
					{
						if (dlg.Item == null)
							r["Style"] = DBNull.Value;
						else
						{
							OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection lst = (OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection) r["Style"];
							lst.Clear();
							foreach(object o in dlg.Item)
								lst.Add((OSGeo.MapGuide.MaestroAPI.StrokeType)OSGeo.MapGuide.MaestroAPI.Utility.DeepCopy(o));
						}
						stylePreview.Refresh();
					}*/
				}
				else if(m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType))
				{
					/*OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType ar = (OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType) m_item;
					OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.AreaFeatureStyleEditor dlg = new ResourceEditors.GeometryStyleEditors.AreaFeatureStyleEditor((OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType) r["Style"]);
					if (dlg.ShowDialog(this) == DialogResult.OK)
					{
						if (dlg.Item == null)
							r["Style"] = DBNull.Value;
						else
							OSGeo.MapGuide.MaestroAPI.Utility.DeepCopy(dlg.Item, (OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType) r["Style"]);
						stylePreview.Refresh();
					}*/
				}
			}
			
		}

		private void stylePreview_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			DataRow r = null;
			if (StyleGrid.CurrentRowIndex >= 0 && StyleGrid.CurrentRowIndex < StyleTable.Rows.Count)
				r = StyleTable.Rows[StyleGrid.CurrentRowIndex];
			else if (StyleTable.Rows.Count == 1)
				r = StyleTable.Rows[0];

			if (r!= null)
			{
				if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointTypeStyleType))
				{
					object item = ((OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType) r["Style"]).Item;
					if (item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
						FeaturePreviewRender.RenderPreviewPoint(e.Graphics, new Rectangle(1,1, stylePreview.Width - 2, stylePreview.Height - 2), (OSGeo.MapGuide.MaestroAPI.MarkSymbolType) item);
				}
				else if(m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineTypeStyleType))
				{
					object item = r["Style"];
					if (item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection))
						FeaturePreviewRender.RenderPreviewLine(e.Graphics, new Rectangle(1,1, stylePreview.Width - 2, stylePreview.Height - 2), (OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection) item);
				}
				else if(m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType))
				{
					object item = r["Style"];
					if (item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType))
						FeaturePreviewRender.RenderPreviewArea(e.Graphics, new Rectangle(1,1, stylePreview.Width - 4, stylePreview.Height - 4), (OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType) item);
				}
				
			}
		
		}

		private void labelPreview_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			DataRow r = null;
			if (StyleGrid.CurrentRowIndex >= 0 && StyleGrid.CurrentRowIndex < StyleTable.Rows.Count)
				r = StyleTable.Rows[StyleGrid.CurrentRowIndex];
			else if (StyleTable.Rows.Count == 1)
				r = StyleTable.Rows[0];

			if (r != null)
				if (r["FeatureLabel"] == DBNull.Value)
					FeaturePreviewRender.RenderPreviewFont(e.Graphics, new Rectangle(1, 1, labelPreview.Width - 2, labelPreview.Height - 2), null);
				else
					FeaturePreviewRender.RenderPreviewFont(e.Graphics, new Rectangle(1, 1, labelPreview.Width - 2, labelPreview.Height - 2), (OSGeo.MapGuide.MaestroAPI.TextSymbolType) r["FeatureLabel"]);

		}

		private void ThemeToolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == ThemeButton)
				ThemeButton_Click();
			else if (e.Button == AddRuleButton)
				AddRuleButton_Click();
			else if (e.Button == CopyButton)
				CopyButton_Click();
			else if (e.Button == DeleteButton)
				DeleteButton_Click();
			else if (e.Button == MoveUpButton)
				MoveUpButton_Click();
			else if (e.Button == MoveDownButton)
				MoveDownButton_Click();
		}

		private object GetNewItem()
		{
			if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointTypeStyleType))
				return new OSGeo.MapGuide.MaestroAPI.PointRuleType();
			else if(m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineTypeStyleType))
				return new OSGeo.MapGuide.MaestroAPI.LineRuleType();
			else if(m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType))
				return new OSGeo.MapGuide.MaestroAPI.AreaRuleType();
			else 
				return null;
		}

		private IList GetItemList()
		{
			if (m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointTypeStyleType))
				return ((OSGeo.MapGuide.MaestroAPI.PointTypeStyleType) m_item).PointRule;
			else if(m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineTypeStyleType))
				return ((OSGeo.MapGuide.MaestroAPI.LineTypeStyleType) m_item).LineRule;
			else if(m_item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType))
				return ((OSGeo.MapGuide.MaestroAPI.AreaTypeStyleType) m_item).AreaRule;
			else
				return null;
		}

		private void AddRuleButton_Click()
		{
			IList list = GetItemList();
			if (list != null)
			{
				object item = GetNewItem();
				if (item != null)
				{
					list.Add(item);
			
					int ri = StyleGrid.CurrentRowIndex;
					UpdateData();
					StyleGrid.CurrentRowIndex = StyleTable.Rows.Count - 1;
					StyleGrid.Select(StyleGrid.CurrentRowIndex);
				}
			}
		}

		private void DeleteButton_Click()
		{
			DataRow r = null;
			if (StyleGrid.CurrentRowIndex >= 0 && StyleGrid.CurrentRowIndex < StyleTable.Rows.Count)
				r = StyleTable.Rows[StyleGrid.CurrentRowIndex];
			else if (StyleTable.Rows.Count == 1)
				r = StyleTable.Rows[0];

			if (r!= null)
			{
				System.Collections.IList list = GetItemList();

				if (list != null && list.Contains(r["Item"]))
					list.Remove(r["Item"]);
				int ri = StyleGrid.CurrentRowIndex;
				UpdateData();
				StyleGrid.CurrentRowIndex = Math.Max(0, ri - 1);
				StyleGrid.Select(StyleGrid.CurrentRowIndex);
			}
		}

		private void CopyButton_Click()
		{
			DataRow r = null;
			if (StyleGrid.CurrentRowIndex >= 0 && StyleGrid.CurrentRowIndex < StyleTable.Rows.Count)
				r = StyleTable.Rows[StyleGrid.CurrentRowIndex];
			else if (StyleTable.Rows.Count == 1)
				r = StyleTable.Rows[0];

			if (r!= null)
			{
				System.Collections.IList list = GetItemList();

				if (list != null)
					list.Add(OSGeo.MapGuide.MaestroAPI.Utility.DeepCopy(r["Item"]));

				int ri = StyleGrid.CurrentRowIndex;
				UpdateData();
				StyleGrid.CurrentRowIndex = StyleTable.Rows.Count - 1;
				StyleGrid.Select(StyleGrid.CurrentRowIndex);
			}
		}

		public void MoveUpButton_Click()
		{
			DataRow r = null;
			if (StyleGrid.CurrentRowIndex >= 0 && StyleGrid.CurrentRowIndex < StyleTable.Rows.Count)
				r = StyleTable.Rows[StyleGrid.CurrentRowIndex];
			else if (StyleTable.Rows.Count == 1)
				r = StyleTable.Rows[0];

			if (r!= null)
			{
				System.Collections.IList list = GetItemList();

				if (list != null)
				{
					int index = list.IndexOf(r["Item"]);
					if (index == 0)
						return;
					list.RemoveAt(index);
					list.Insert(index - 1, r["Item"]);

					int ri = StyleGrid.CurrentRowIndex;
					UpdateData();
					StyleGrid.CurrentRowIndex = ri - 1;
					StyleGrid.Select(StyleGrid.CurrentRowIndex);
				}
			}
		}

		public void MoveDownButton_Click()
		{
			DataRow r = null;
			if (StyleGrid.CurrentRowIndex >= 0 && StyleGrid.CurrentRowIndex < StyleTable.Rows.Count)
				r = StyleTable.Rows[StyleGrid.CurrentRowIndex];
			else if (StyleTable.Rows.Count == 1)
				r = StyleTable.Rows[0];

			if (r!= null)
			{
				System.Collections.IList list = GetItemList();

				if (list != null)
				{
					int index = list.IndexOf(r["Item"]);
					if (index >= list.Count)
						return;
					list.RemoveAt(index);
					list.Insert(index + 1, r["Item"]);

					int ri = StyleGrid.CurrentRowIndex ;
					UpdateData();
					StyleGrid.CurrentRowIndex = ri + 1;
					StyleGrid.Select(StyleGrid.CurrentRowIndex);
				}
			}
		}

		private void ThemeButton_Click()
		{
			//TODO: Implement theme dialog
			MessageBox.Show(this, "The theme dialog is not yet implemented", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void StyleGrid_DoubleClick(object sender, System.EventArgs e)
		{
			StyleGrid_Click(sender, e);
		}

		private void StyleGrid_Click(object sender, System.EventArgs e)
		{
			DataGrid.HitTestInfo hti = StyleGrid.HitTest(StyleGrid.PointToClient(System.Windows.Forms.Cursor.Position));
			if (hti.Row >= 0 && hti.Row < StyleTable.Rows.Count)
			{
				StyleGrid.CurrentRowIndex = hti.Row;
				StyleGrid.Select(hti.Row);
				if (hti.Column == 1)
					StyleFeatureButton_Click(sender, e);
				else if (hti.Column == 2)
					StyleLabelButton_Click(sender, e);
			}
		
		}

		private void StyleTable_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (e.Action == System.Data.DataRowAction.Add && e.Row["Item"] == DBNull.Value)
			{
				object newitem = GetNewItem();
				e.Row["Item"] = newitem;
				e.Row["LegendLabel"] = "";

				if (newitem.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.PointRuleType))
				{
					OSGeo.MapGuide.MaestroAPI.PointRuleType pr = (OSGeo.MapGuide.MaestroAPI.PointRuleType) newitem;
					e.Row["Style"] = pr.Item;
					e.Row["FeatureLabel"] = pr.Label;
				}
				else if(newitem.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.LineRuleType))
				{
					OSGeo.MapGuide.MaestroAPI.LineRuleType lr = (OSGeo.MapGuide.MaestroAPI.LineRuleType) newitem;
					e.Row["Style"] = lr.Items;
					e.Row["FeatureLabel"] = lr.Label;
				}
				else if(newitem.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.AreaRuleType))
				{
					OSGeo.MapGuide.MaestroAPI.AreaRuleType ar = (OSGeo.MapGuide.MaestroAPI.AreaRuleType) newitem;
					e.Row["Style"] = ar.Item;
					e.Row["FeatureLabel"] = ar.Label;
				}

			}
			else if (e.Action == System.Data.DataRowAction.Change)
			{
				DataRow r = null;
				if (StyleGrid.CurrentRowIndex >= 0 && StyleGrid.CurrentRowIndex < StyleTable.Rows.Count)
					r = StyleTable.Rows[StyleGrid.CurrentRowIndex];
				else if (StyleTable.Rows.Count == 1)
					r = StyleTable.Rows[0];

				if (r != null)
				{
					string legendLabel = (string)r["LegendLabel"];
					string condition = (string)r["Condition"];

					if (r["Item"] as OSGeo.MapGuide.MaestroAPI.PointRuleType != null)
					{
						(r["Item"] as OSGeo.MapGuide.MaestroAPI.PointRuleType).Filter = condition;
						(r["Item"] as OSGeo.MapGuide.MaestroAPI.PointRuleType).LegendLabel = legendLabel;
					}
					else if (r["Item"] as OSGeo.MapGuide.MaestroAPI.LineRuleType != null)
					{
						(r["Item"] as OSGeo.MapGuide.MaestroAPI.LineRuleType).Filter = condition;
						(r["Item"] as OSGeo.MapGuide.MaestroAPI.LineRuleType).LegendLabel = legendLabel;
					}
					else if (r["Item"] as OSGeo.MapGuide.MaestroAPI.AreaRuleType != null)
					{
						(r["Item"] as OSGeo.MapGuide.MaestroAPI.AreaRuleType).Filter = condition;
						(r["Item"] as OSGeo.MapGuide.MaestroAPI.AreaRuleType).LegendLabel = legendLabel;
					}
				}
			}
		}
	}
}
