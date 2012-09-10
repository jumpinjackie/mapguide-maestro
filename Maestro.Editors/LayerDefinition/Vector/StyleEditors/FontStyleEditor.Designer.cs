using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    partial class FontStyleEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
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
            this.backgroundColor = new Maestro.Editors.LayerDefinition.Vector.StyleEditors.ColorExpressionField();
            this.textColor = new Maestro.Editors.LayerDefinition.Vector.StyleEditors.ColorExpressionField();
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
            this.colorGroup.Controls.Add(this.backgroundColor);
            this.colorGroup.Controls.Add(this.textColor);
            this.colorGroup.Controls.Add(this.backgroundTypeCombo);
            this.colorGroup.Controls.Add(this.label7);
            this.colorGroup.Controls.Add(this.label8);
            this.colorGroup.Controls.Add(this.label9);
            this.colorGroup.Name = "colorGroup";
            this.colorGroup.TabStop = false;
            // 
            // backgroundColor
            // 
            resources.ApplyResources(this.backgroundColor, "backgroundColor");
            this.backgroundColor.ColorExpression = "";
            this.backgroundColor.Name = "backgroundColor";
            this.backgroundColor.CurrentColorChanged += new System.EventHandler(this.backgroundColor_SelectedIndexChanged);
            this.backgroundColor.RequestExpressionEditor += new System.EventHandler(this.BackgroundColor_RequestExpressionEditor);
            // 
            // textColor
            // 
            resources.ApplyResources(this.textColor, "textColor");
            this.textColor.ColorExpression = "";
            this.textColor.Name = "textColor";
            this.textColor.CurrentColorChanged += new System.EventHandler(this.textColor_SelectedIndexChanged);
            this.textColor.RequestExpressionEditor += new System.EventHandler(this.TextColor_RequestExpressionEditor);
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

        internal System.Windows.Forms.Label verticalLabel;
        internal System.Windows.Forms.Label horizontalLabel;
        private System.Windows.Forms.CheckBox DisplayLabel;
        private ColorExpressionField textColor;
        private ColorExpressionField backgroundColor;
        
    }
}
