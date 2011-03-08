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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
	/// <summary>
	/// Summary description for FontStyleEditor.
	/// </summary>
	internal class FontStyleEditor : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.GroupBox fontGroup;
		private System.Windows.Forms.GroupBox colorGroup;
		private System.Windows.Forms.GroupBox alignmentGroup;
		private System.Windows.Forms.PictureBox previewPicture;
		private System.Windows.Forms.ComboBox sizeCombo;
		private System.Windows.Forms.ComboBox unitsCombo;
		private System.Windows.Forms.ComboBox sizeContextCombo;
		private System.Windows.Forms.ComboBox fontCombo;
		private System.Windows.Forms.ComboBox propertyCombo;
		private System.Windows.Forms.CheckBox underlineCheck;
		private System.Windows.Forms.CheckBox italicCheck;
		private System.Windows.Forms.CheckBox boldCheck;
        private System.Windows.Forms.ComboBox backgroundTypeCombo;
		private System.Windows.Forms.ComboBox rotationCombo;
		internal System.Windows.Forms.ComboBox verticalCombo;
		internal System.Windows.Forms.ComboBox horizontalCombo;
		private System.Windows.Forms.GroupBox previewGroup;
		private System.Data.DataSet ComboBoxDataSet;
		private System.Data.DataColumn dataColumn1;
		private System.Data.DataColumn dataColumn2;
		private System.Data.DataTable SizeContextTable;
		private System.Data.DataColumn dataColumn3;
		private System.Data.DataColumn dataColumn4;
		private System.Data.DataTable UnitsTable;
		private System.Data.DataColumn dataColumn5;
		private System.Data.DataColumn dataColumn6;
		private System.Data.DataTable RotationTable;
		private System.Data.DataColumn dataColumn7;
		private System.Data.DataColumn dataColumn8;
		private System.Data.DataTable BackgroundTypeTable;
		private System.Data.DataColumn dataColumn9;
		private System.Data.DataColumn dataColumn10;
		private System.Data.DataTable HorizontalTable;
		private System.Data.DataColumn dataColumn11;
		private System.Data.DataColumn dataColumn12;
		private System.Data.DataTable VerticalTable;
		private System.Data.DataColumn dataColumn13;
		private System.Data.DataColumn dataColumn14;
		private System.Data.DataTable FontTable;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ITextSymbol m_item;
		internal System.Windows.Forms.Label verticalLabel;
		internal System.Windows.Forms.Label horizontalLabel;
        private CheckBox DisplayLabel;
        private ColorComboWithTransparency textColor;
        private Label label12;
        private Label label11;
        private ColorComboWithTransparency backgroundColor;
		private bool m_inUpdate = false;

		public event EventHandler Changed;

        private IEditorService m_editor;
        private ClassDefinition m_schema;
        private string m_featureSource;
        private string m_providername;

        private ILayerElementFactory _factory;

        public FontStyleEditor(IEditorService editor, ClassDefinition schema, string featureSource)
            : this()
        {
            m_editor = editor;
            m_schema = schema;

            var fs = (IFeatureSource)editor.ResourceService.GetResource(featureSource);

            _factory = (ILayerElementFactory)editor.GetEditedResource();

            m_providername = fs.Provider;
            m_featureSource = featureSource;

            propertyCombo.Items.Clear();
            foreach (var col in m_schema.Properties)
            {
                if (col.Type == PropertyDefinitionType.Data)
                    propertyCombo.Items.Add(col.Name);
            }
            propertyCombo.Items.Add(Properties.Resources.ExpressionItem);

            fontCombo.Items.Clear();
            foreach (FontFamily f in new System.Drawing.Text.InstalledFontCollection().Families)
                fontCombo.Items.Add(f.Name);

        }

        private FontStyleEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            using(System.IO.StringReader sr = new System.IO.StringReader(Properties.Resources.GeometryStyleComboDataset))
				ComboBoxDataSet.ReadXml(sr);
        }


		private void UpdateDisplay()
		{
			if (m_inUpdate)
				return;
			try
			{
				m_inUpdate = true;

                if (m_item == null)
                {
                    DisplayLabel.Checked = false;
                    return;
                }
                else
                    DisplayLabel.Checked = true;

				propertyCombo.Text = m_item.Text;
				propertyCombo.SelectedItem = m_item.Text;
				if (m_item.FontName != null)
					fontCombo.Text = m_item.FontName;
				sizeContextCombo.SelectedValue = m_item.SizeContext.ToString();
				unitsCombo.SelectedValue = m_item.Unit.ToString();
				if (m_item.SizeX == null)
					sizeCombo.Text = "";
				else
					sizeCombo.Text = m_item.SizeX.ToString();

				boldCheck.Checked = m_item.Bold == "true";
				italicCheck.Checked = m_item.Italic == "true";
				underlineCheck.Checked = m_item.Underlined == "true";
                textColor.CurrentColor = Utility.ParseHTMLColor(m_item.ForegroundColor);
                backgroundColor.CurrentColor = Utility.ParseHTMLColor(m_item.BackgroundColor);
				backgroundTypeCombo.SelectedValue = m_item.BackgroundStyle.ToString();
                rotationCombo.SelectedIndex = -1;
                rotationCombo.Text = m_item.Rotation.HasValue ? m_item.Rotation.ToString() : "";
                if (m_item.HorizontalAlignment != null)
                {
                    horizontalCombo.SelectedValue = m_item.HorizontalAlignment;
                    if (horizontalCombo.SelectedValue == null)
                    {
                        horizontalCombo.SelectedIndex = -1;
                        horizontalCombo.Text = m_item.HorizontalAlignment;
                    }
                }

                if (m_item.VerticalAlignment != null)
                {
                    verticalCombo.SelectedValue = m_item.VerticalAlignment;
                    if (verticalCombo.SelectedValue == null)
                    {
                        verticalCombo.SelectedIndex = -1;
                        verticalCombo.Text = m_item.VerticalAlignment;
                    }
                }
			}
			finally
			{
				m_inUpdate = false;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FontStyleEditor));
            this.fontGroup = new System.Windows.Forms.GroupBox();
            this.sizeCombo = new System.Windows.Forms.ComboBox();
            this.unitsCombo = new System.Windows.Forms.ComboBox();
            this.UnitsTable = new System.Data.DataTable();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.sizeContextCombo = new System.Windows.Forms.ComboBox();
            this.SizeContextTable = new System.Data.DataTable();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.fontCombo = new System.Windows.Forms.ComboBox();
            this.propertyCombo = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.underlineCheck = new System.Windows.Forms.CheckBox();
            this.italicCheck = new System.Windows.Forms.CheckBox();
            this.boldCheck = new System.Windows.Forms.CheckBox();
            this.FontTable = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.colorGroup = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.backgroundColor = new ColorComboWithTransparency();
            this.textColor = new ColorComboWithTransparency();
            this.backgroundTypeCombo = new System.Windows.Forms.ComboBox();
            this.BackgroundTypeTable = new System.Data.DataTable();
            this.dataColumn9 = new System.Data.DataColumn();
            this.dataColumn10 = new System.Data.DataColumn();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.alignmentGroup = new System.Windows.Forms.GroupBox();
            this.rotationCombo = new System.Windows.Forms.ComboBox();
            this.RotationTable = new System.Data.DataTable();
            this.dataColumn7 = new System.Data.DataColumn();
            this.dataColumn8 = new System.Data.DataColumn();
            this.verticalCombo = new System.Windows.Forms.ComboBox();
            this.VerticalTable = new System.Data.DataTable();
            this.dataColumn13 = new System.Data.DataColumn();
            this.dataColumn14 = new System.Data.DataColumn();
            this.horizontalCombo = new System.Windows.Forms.ComboBox();
            this.HorizontalTable = new System.Data.DataTable();
            this.dataColumn11 = new System.Data.DataColumn();
            this.dataColumn12 = new System.Data.DataColumn();
            this.label10 = new System.Windows.Forms.Label();
            this.verticalLabel = new System.Windows.Forms.Label();
            this.horizontalLabel = new System.Windows.Forms.Label();
            this.previewGroup = new System.Windows.Forms.GroupBox();
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.ComboBoxDataSet = new System.Data.DataSet();
            this.DisplayLabel = new System.Windows.Forms.CheckBox();
            this.fontGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FontTable)).BeginInit();
            this.colorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundTypeTable)).BeginInit();
            this.alignmentGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VerticalTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HorizontalTable)).BeginInit();
            this.previewGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // fontGroup
            // 
            resources.ApplyResources(this.fontGroup, "fontGroup");
            this.fontGroup.Controls.Add(this.sizeCombo);
            this.fontGroup.Controls.Add(this.unitsCombo);
            this.fontGroup.Controls.Add(this.sizeContextCombo);
            this.fontGroup.Controls.Add(this.fontCombo);
            this.fontGroup.Controls.Add(this.propertyCombo);
            this.fontGroup.Controls.Add(this.label6);
            this.fontGroup.Controls.Add(this.label5);
            this.fontGroup.Controls.Add(this.label4);
            this.fontGroup.Controls.Add(this.label3);
            this.fontGroup.Controls.Add(this.label2);
            this.fontGroup.Controls.Add(this.label1);
            this.fontGroup.Controls.Add(this.panel1);
            this.fontGroup.Name = "fontGroup";
            this.fontGroup.TabStop = false;
            this.fontGroup.Enter += new System.EventHandler(this.fontGroup_Enter);
            // 
            // sizeCombo
            // 
            resources.ApplyResources(this.sizeCombo, "sizeCombo");
            this.sizeCombo.Items.AddRange(new object[] {
            resources.GetString("sizeCombo.Items")});
            this.sizeCombo.Name = "sizeCombo";
            this.sizeCombo.SelectedIndexChanged += new System.EventHandler(this.sizeCombo_SelectedIndexChanged);
            this.sizeCombo.TextChanged += new System.EventHandler(this.sizeCombo_TextChanged);
            // 
            // unitsCombo
            // 
            resources.ApplyResources(this.unitsCombo, "unitsCombo");
            this.unitsCombo.DataSource = this.UnitsTable;
            this.unitsCombo.DisplayMember = "Display";
            this.unitsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.unitsCombo.Name = "unitsCombo";
            this.unitsCombo.ValueMember = "Value";
            this.unitsCombo.SelectedIndexChanged += new System.EventHandler(this.unitsCombo_SelectedIndexChanged);
            // 
            // UnitsTable
            // 
            this.UnitsTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn5,
            this.dataColumn6});
            this.UnitsTable.TableName = "Units";
            // 
            // dataColumn5
            // 
            this.dataColumn5.Caption = "Display";
            this.dataColumn5.ColumnName = "Display";
            // 
            // dataColumn6
            // 
            this.dataColumn6.Caption = "Value";
            this.dataColumn6.ColumnName = "Value";
            // 
            // sizeContextCombo
            // 
            resources.ApplyResources(this.sizeContextCombo, "sizeContextCombo");
            this.sizeContextCombo.DataSource = this.SizeContextTable;
            this.sizeContextCombo.DisplayMember = "Display";
            this.sizeContextCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeContextCombo.Name = "sizeContextCombo";
            this.sizeContextCombo.ValueMember = "Value";
            this.sizeContextCombo.SelectedIndexChanged += new System.EventHandler(this.sizeContextCombo_SelectedIndexChanged);
            // 
            // SizeContextTable
            // 
            this.SizeContextTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn3,
            this.dataColumn4});
            this.SizeContextTable.TableName = "SizeContext";
            // 
            // dataColumn3
            // 
            this.dataColumn3.Caption = "Display";
            this.dataColumn3.ColumnName = "Display";
            // 
            // dataColumn4
            // 
            this.dataColumn4.Caption = "Value";
            this.dataColumn4.ColumnName = "Value";
            // 
            // fontCombo
            // 
            resources.ApplyResources(this.fontCombo, "fontCombo");
            this.fontCombo.Name = "fontCombo";
            this.fontCombo.SelectedIndexChanged += new System.EventHandler(this.fontCombo_SelectedIndexChanged);
            this.fontCombo.TextChanged += new System.EventHandler(this.fontCombo_TextChanged);
            // 
            // propertyCombo
            // 
            resources.ApplyResources(this.propertyCombo, "propertyCombo");
            this.propertyCombo.Name = "propertyCombo";
            this.propertyCombo.SelectedIndexChanged += new System.EventHandler(this.propertyCombo_SelectedIndexChanged);
            this.propertyCombo.TextChanged += new System.EventHandler(this.propertyCombo_TextChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.underlineCheck);
            this.panel1.Controls.Add(this.italicCheck);
            this.panel1.Controls.Add(this.boldCheck);
            this.panel1.Name = "panel1";
            // 
            // underlineCheck
            // 
            resources.ApplyResources(this.underlineCheck, "underlineCheck");
            this.underlineCheck.Name = "underlineCheck";
            this.underlineCheck.CheckedChanged += new System.EventHandler(this.underlineCheck_CheckedChanged);
            // 
            // italicCheck
            // 
            resources.ApplyResources(this.italicCheck, "italicCheck");
            this.italicCheck.Name = "italicCheck";
            this.italicCheck.CheckedChanged += new System.EventHandler(this.italicCheck_CheckedChanged);
            // 
            // boldCheck
            // 
            resources.ApplyResources(this.boldCheck, "boldCheck");
            this.boldCheck.Name = "boldCheck";
            this.boldCheck.CheckedChanged += new System.EventHandler(this.boldCheck_CheckedChanged);
            // 
            // FontTable
            // 
            this.FontTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2});
            this.FontTable.TableName = "Font";
            // 
            // dataColumn1
            // 
            this.dataColumn1.Caption = "Display";
            this.dataColumn1.ColumnName = "Display";
            // 
            // dataColumn2
            // 
            this.dataColumn2.Caption = "Value";
            this.dataColumn2.ColumnName = "Value";
            // 
            // colorGroup
            // 
            resources.ApplyResources(this.colorGroup, "colorGroup");
            this.colorGroup.Controls.Add(this.label12);
            this.colorGroup.Controls.Add(this.label11);
            this.colorGroup.Controls.Add(this.backgroundColor);
            this.colorGroup.Controls.Add(this.textColor);
            this.colorGroup.Controls.Add(this.backgroundTypeCombo);
            this.colorGroup.Controls.Add(this.label7);
            this.colorGroup.Controls.Add(this.label8);
            this.colorGroup.Controls.Add(this.label9);
            this.colorGroup.Name = "colorGroup";
            this.colorGroup.TabStop = false;
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // backgroundColor
            // 
            resources.ApplyResources(this.backgroundColor, "backgroundColor");
            this.backgroundColor.CurrentColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.backgroundColor.Name = "backgroundColor";
            this.backgroundColor.CurrentColorChanged += new System.EventHandler(this.backgroundColor_SelectedIndexChanged);
            // 
            // textColor
            // 
            resources.ApplyResources(this.textColor, "textColor");
            this.textColor.CurrentColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textColor.Name = "textColor";
            this.textColor.CurrentColorChanged += new System.EventHandler(this.textColor_SelectedIndexChanged);
            // 
            // backgroundTypeCombo
            // 
            resources.ApplyResources(this.backgroundTypeCombo, "backgroundTypeCombo");
            this.backgroundTypeCombo.DataSource = this.BackgroundTypeTable;
            this.backgroundTypeCombo.DisplayMember = "Display";
            this.backgroundTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.backgroundTypeCombo.Name = "backgroundTypeCombo";
            this.backgroundTypeCombo.ValueMember = "Value";
            this.backgroundTypeCombo.SelectedIndexChanged += new System.EventHandler(this.backgroundTypeCombo_SelectedIndexChanged);
            // 
            // BackgroundTypeTable
            // 
            this.BackgroundTypeTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn9,
            this.dataColumn10});
            this.BackgroundTypeTable.TableName = "BackgroundType";
            // 
            // dataColumn9
            // 
            this.dataColumn9.ColumnName = "Display";
            // 
            // dataColumn10
            // 
            this.dataColumn10.ColumnName = "Value";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // alignmentGroup
            // 
            resources.ApplyResources(this.alignmentGroup, "alignmentGroup");
            this.alignmentGroup.Controls.Add(this.rotationCombo);
            this.alignmentGroup.Controls.Add(this.verticalCombo);
            this.alignmentGroup.Controls.Add(this.horizontalCombo);
            this.alignmentGroup.Controls.Add(this.label10);
            this.alignmentGroup.Controls.Add(this.verticalLabel);
            this.alignmentGroup.Controls.Add(this.horizontalLabel);
            this.alignmentGroup.Name = "alignmentGroup";
            this.alignmentGroup.TabStop = false;
            // 
            // rotationCombo
            // 
            resources.ApplyResources(this.rotationCombo, "rotationCombo");
            this.rotationCombo.DataSource = this.RotationTable;
            this.rotationCombo.DisplayMember = "Display";
            this.rotationCombo.Name = "rotationCombo";
            this.rotationCombo.ValueMember = "Value";
            this.rotationCombo.SelectedIndexChanged += new System.EventHandler(this.rotationCombo_SelectedIndexChanged);
            this.rotationCombo.TextChanged += new System.EventHandler(this.rotationCombo_TextChanged);
            // 
            // RotationTable
            // 
            this.RotationTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn7,
            this.dataColumn8});
            this.RotationTable.TableName = "Rotation";
            // 
            // dataColumn7
            // 
            this.dataColumn7.Caption = "Display";
            this.dataColumn7.ColumnName = "Display";
            // 
            // dataColumn8
            // 
            this.dataColumn8.Caption = "Value";
            this.dataColumn8.ColumnName = "Value";
            // 
            // verticalCombo
            // 
            resources.ApplyResources(this.verticalCombo, "verticalCombo");
            this.verticalCombo.DataSource = this.VerticalTable;
            this.verticalCombo.DisplayMember = "Display";
            this.verticalCombo.Name = "verticalCombo";
            this.verticalCombo.ValueMember = "Value";
            this.verticalCombo.SelectedIndexChanged += new System.EventHandler(this.verticalCombo_SelectedIndexChanged);
            this.verticalCombo.TextChanged += new System.EventHandler(this.verticalCombo_TextChanged);
            // 
            // VerticalTable
            // 
            this.VerticalTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn13,
            this.dataColumn14});
            this.VerticalTable.TableName = "Vertical";
            // 
            // dataColumn13
            // 
            this.dataColumn13.ColumnName = "Display";
            // 
            // dataColumn14
            // 
            this.dataColumn14.ColumnName = "Value";
            // 
            // horizontalCombo
            // 
            resources.ApplyResources(this.horizontalCombo, "horizontalCombo");
            this.horizontalCombo.DataSource = this.HorizontalTable;
            this.horizontalCombo.DisplayMember = "Display";
            this.horizontalCombo.Name = "horizontalCombo";
            this.horizontalCombo.ValueMember = "Value";
            this.horizontalCombo.SelectedIndexChanged += new System.EventHandler(this.horizontalCombo_SelectedIndexChanged);
            this.horizontalCombo.TextChanged += new System.EventHandler(this.horizontalCombo_TextChanged);
            // 
            // HorizontalTable
            // 
            this.HorizontalTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn11,
            this.dataColumn12});
            this.HorizontalTable.TableName = "Horizontal";
            // 
            // dataColumn11
            // 
            this.dataColumn11.ColumnName = "Display";
            // 
            // dataColumn12
            // 
            this.dataColumn12.ColumnName = "Value";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // verticalLabel
            // 
            resources.ApplyResources(this.verticalLabel, "verticalLabel");
            this.verticalLabel.Name = "verticalLabel";
            // 
            // horizontalLabel
            // 
            resources.ApplyResources(this.horizontalLabel, "horizontalLabel");
            this.horizontalLabel.Name = "horizontalLabel";
            // 
            // previewGroup
            // 
            resources.ApplyResources(this.previewGroup, "previewGroup");
            this.previewGroup.Controls.Add(this.previewPicture);
            this.previewGroup.Name = "previewGroup";
            this.previewGroup.TabStop = false;
            // 
            // previewPicture
            // 
            resources.ApplyResources(this.previewPicture, "previewPicture");
            this.previewPicture.BackColor = System.Drawing.Color.White;
            this.previewPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewPicture.Name = "previewPicture";
            this.previewPicture.TabStop = false;
            this.previewPicture.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPicture_Paint);
            // 
            // ComboBoxDataSet
            // 
            this.ComboBoxDataSet.DataSetName = "ComboBoxDataSet";
            this.ComboBoxDataSet.Locale = new System.Globalization.CultureInfo("da-DK");
            this.ComboBoxDataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.FontTable,
            this.SizeContextTable,
            this.UnitsTable,
            this.RotationTable,
            this.BackgroundTypeTable,
            this.HorizontalTable,
            this.VerticalTable});
            // 
            // DisplayLabel
            // 
            resources.ApplyResources(this.DisplayLabel, "DisplayLabel");
            this.DisplayLabel.Checked = true;
            this.DisplayLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayLabel.Name = "DisplayLabel";
            this.DisplayLabel.UseVisualStyleBackColor = true;
            this.DisplayLabel.CheckedChanged += new System.EventHandler(this.DisplayLabel_CheckedChanged);
            // 
            // FontStyleEditor
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.DisplayLabel);
            this.Controls.Add(this.previewGroup);
            this.Controls.Add(this.colorGroup);
            this.Controls.Add(this.fontGroup);
            this.Controls.Add(this.alignmentGroup);
            this.Name = "FontStyleEditor";
            this.fontGroup.ResumeLayout(false);
            this.fontGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FontTable)).EndInit();
            this.colorGroup.ResumeLayout(false);
            this.colorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundTypeTable)).EndInit();
            this.alignmentGroup.ResumeLayout(false);
            this.alignmentGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VerticalTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HorizontalTable)).EndInit();
            this.previewGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		private void propertyCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (propertyCombo.SelectedIndex == propertyCombo.Items.Count - 1)
            {
                string current = m_item.Text;
                string expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource);
                if (!string.IsNullOrEmpty(expr))
                    current = expr;

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), propertyCombo, current, expr != null);
            }
		}

		private void fontCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;
			m_item.FontName = (string)fontCombo.Text;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void sizeContextCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;
			m_item.SizeContext = (SizeContextType)Enum.Parse(typeof(SizeContextType), (string)sizeContextCombo.SelectedValue);
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void unitsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;
			m_item.Unit = (LengthUnitType)Enum.Parse(typeof(LengthUnitType), (string)unitsCombo.SelectedValue);
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void sizeCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (sizeCombo.SelectedIndex == sizeCombo.Items.Count - 1)
            {
                string current = m_item.SizeX.HasValue ? m_item.SizeX.ToString() : string.Empty;
                string expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource);
                if (!string.IsNullOrEmpty(expr))
                    current = expr;

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), sizeCombo, current, expr != null);
            }
        }

		private void boldCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;
			m_item.Bold = boldCheck.Checked ? "true" : null;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void italicCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;
			m_item.Italic = italicCheck.Checked ? "true" : null;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void underlineCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;
			m_item.Underlined = underlineCheck.Checked ? "true" : null;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void textColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;
            m_item.ForegroundColor = Utility.SerializeHTMLColor(textColor.CurrentColor, true);
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void backgroundColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;
            m_item.BackgroundColor = Utility.SerializeHTMLColor(backgroundColor.CurrentColor, true);
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void backgroundTypeCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;
			m_item.BackgroundStyle = (BackgroundStyleType)Enum.Parse(typeof(BackgroundStyleType), (string)backgroundTypeCombo.SelectedValue);
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void horizontalCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (m_inUpdate)
                return;

            if (horizontalCombo.SelectedIndex == horizontalCombo.Items.Count - 1)
            {
                string current = m_item.HorizontalAlignment;
                string expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource);
                if (!string.IsNullOrEmpty(expr))
                    current = expr;

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), horizontalCombo, current, expr != null);
            }
            else if (horizontalCombo.SelectedIndex != -1)
            {
                m_item.HorizontalAlignment = (string)horizontalCombo.SelectedValue;
            }
        }

		private void verticalCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (verticalCombo.SelectedIndex == verticalCombo.Items.Count - 1)
            {
                string current = m_item.VerticalAlignment;
                string expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource);
                if (!string.IsNullOrEmpty(expr))
                    current = expr;

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), verticalCombo, current, expr != null);
            }
            else if (verticalCombo.SelectedIndex != -1)
            {
                m_item.VerticalAlignment = (string)verticalCombo.SelectedValue;
            }
        }

		private void rotationCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (rotationCombo.SelectedIndex == rotationCombo.Items.Count - 1)
            {
                string current = m_item.Rotation.HasValue ? m_item.Rotation.Value.ToString() : string.Empty;
                string expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource);
                if (!string.IsNullOrEmpty(expr))
                    current = expr;

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), rotationCombo, current, expr != null);
            }
            else if (rotationCombo.SelectedIndex != -1)
            {
                m_item.Rotation = StringToDouble((string)rotationCombo.SelectedValue);
            }
        }

        static double? StringToDouble(string value)
        {
            double d;
            if (double.TryParse(value, out d))
                return d;
            return null;
        }

		private void previewPicture_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			FeaturePreviewRender.RenderPreviewFont(e.Graphics, new Rectangle(1, 1, previewPicture.Width - 2, previewPicture.Height - 2), m_item);
		}

		private void fontGroup_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void propertyCombo_TextChanged(object sender, System.EventArgs e)
		{
            if (m_inUpdate || propertyCombo.SelectedIndex == propertyCombo.Items.Count - 1)
                return;

            m_item.Text = propertyCombo.Text;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        public ITextSymbol Item
		{
			get { return m_item; }
			set 
			{
				m_item = value;
				UpdateDisplay();
			}
		}

        private void DisplayLabel_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
                c.Enabled = c == DisplayLabel || DisplayLabel.Checked;

            if (m_inUpdate)
                return;

            if (DisplayLabel.Checked)
            {
                if (DisplayLabel.Tag as ITextSymbol != null)
                    this.Item = DisplayLabel.Tag as ITextSymbol;
                if (m_item == null)
                    this.Item = _factory.CreateDefaultTextSymbol();
            }
            else
            {
                DisplayLabel.Tag = m_item;
                this.Item = null;
            }

        }

        private void sizeCombo_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || sizeCombo.SelectedIndex != -1)
                return;

            //TODO: Validate
            m_item.SizeX = m_item.SizeY = StringToDouble(sizeCombo.Text);
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void fontCombo_TextChanged(object sender, EventArgs e)
        {
            fontCombo_SelectedIndexChanged(sender, e);
        }


        public delegate void UpdateComboTextFromSelectChangedDelegate(ComboBox owner, string text, bool userChange);

        private void UpdateComboTextFromSelectChanged(ComboBox owner, string text, bool userChange)
        {
            try
            {
                if (!userChange)
                    m_inUpdate = true;
                owner.SelectedIndex = -1;

                //HACK: Odd bug, don't remove
                if (owner.SelectedIndex != -1)
                    owner.SelectedIndex = -1;

                owner.Text = text;
            }
            finally
            {
                if (!userChange)
                    m_inUpdate = false;
            }
        }

        private void horizontalCombo_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || horizontalCombo.SelectedIndex != -1)
                return;

            m_item.HorizontalAlignment = (string)horizontalCombo.Text;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void verticalCombo_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || verticalCombo.SelectedIndex != -1)
                return;

            m_item.VerticalAlignment = (string)verticalCombo.Text;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void rotationCombo_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || rotationCombo.SelectedIndex != -1)
                return;

            //TODO: Validate
            m_item.Rotation = StringToDouble(rotationCombo.Text);
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

	}
}
