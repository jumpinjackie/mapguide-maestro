using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    partial class AreaFeatureStyleEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AreaFeatureStyleEditor));
            this.fillStyleEditor = new Maestro.Editors.LayerDefinition.Vector.StyleEditors.FillStyleEditor();
            this.lineStyleEditor = new Maestro.Editors.LayerDefinition.Vector.StyleEditors.LineStyleEditor();
            this.sizeUnitsCombo = new System.Windows.Forms.ComboBox();
            this.UnitsTable = new System.Data.DataTable();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.sizeContextCombo = new System.Windows.Forms.ComboBox();
            this.SizeContextTable = new System.Data.DataTable();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.previewGroup = new System.Windows.Forms.GroupBox();
            this.lnkRefresh = new System.Windows.Forms.LinkLabel();
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.ComboBoxDataSet = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.previewGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // fillStyleEditor
            // 
            resources.ApplyResources(this.fillStyleEditor, "fillStyleEditor"); 
            this.fillStyleEditor.Name = "fillStyleEditor"; 
            this.fillStyleEditor.ForegroundRequiresExpression += new System.EventHandler(this.fillStyleEditor_ForegroundRequiresExpression);
            this.fillStyleEditor.BackgroundRequiresExpression += new System.EventHandler(this.fillStyleEditor_BackgroundRequiresExpression);
            // 
            // lineStyleEditor
            // 
            resources.ApplyResources(this.lineStyleEditor, "lineStyleEditor"); 
            this.lineStyleEditor.ColorExpression = ""; 
            this.lineStyleEditor.Name = "lineStyleEditor"; 
            this.lineStyleEditor.RequiresExpressionEditor += new System.EventHandler(this.LineStyleEditor_RequiresExpressionEditor);
            // 
            // sizeUnitsCombo
            // 
            resources.ApplyResources(this.sizeUnitsCombo, "sizeUnitsCombo"); 
            this.sizeUnitsCombo.DataSource = this.UnitsTable;
            this.sizeUnitsCombo.DisplayMember = "Display"; 
            this.sizeUnitsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeUnitsCombo.Name = "sizeUnitsCombo"; 
            this.sizeUnitsCombo.ValueMember = "Value"; 
            this.sizeUnitsCombo.SelectedIndexChanged += new System.EventHandler(this.sizeUnitsCombo_SelectedIndexChanged);
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
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1"); 
            this.groupBox1.Controls.Add(this.sizeUnitsCombo);
            this.groupBox1.Controls.Add(this.sizeContextCombo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lineStyleEditor);
            this.groupBox1.Name = "groupBox1"; 
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.fillStyleEditor);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // previewGroup
            // 
            resources.ApplyResources(this.previewGroup, "previewGroup");
            this.previewGroup.Controls.Add(this.lnkRefresh);
            this.previewGroup.Controls.Add(this.previewPicture);
            this.previewGroup.Name = "previewGroup";
            this.previewGroup.TabStop = false;
            // 
            // lnkRefresh
            // 
            resources.ApplyResources(this.lnkRefresh, "lnkRefresh");
            this.lnkRefresh.BackColor = System.Drawing.Color.Transparent;
            this.lnkRefresh.Name = "lnkRefresh";
            this.lnkRefresh.TabStop = true;
            this.lnkRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRefresh_LinkClicked);
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
            this.SizeContextTable,
            this.UnitsTable});
            // 
            // AreaFeatureStyleEditor
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.previewGroup);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "AreaFeatureStyleEditor";
            this.Load += new System.EventHandler(this.AreaFeatureStyleEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.previewGroup.ResumeLayout(false);
            this.previewGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

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

        private System.Windows.Forms.ComboBox sizeContextCombo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox previewGroup;
        private System.Windows.Forms.PictureBox previewPicture;
        private System.Windows.Forms.ComboBox sizeUnitsCombo;

        private System.Data.DataSet ComboBoxDataSet;
        private System.Data.DataTable SizeContextTable;
        private System.Data.DataColumn dataColumn3;
        private System.Data.DataColumn dataColumn4;
        private System.Data.DataTable UnitsTable;
        private System.Data.DataColumn dataColumn5;
        private System.Data.DataColumn dataColumn6;
    }
}
