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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors
{
	/// <summary>
	/// Summary description for FontStyleEditor.
	/// </summary>
	public class FontStyleEditor : System.Windows.Forms.UserControl
	{
		private static byte[] SharedComboDataSet = null;
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
		private ColorComboBox backgroundColor;
		private ColorComboBox textColor;
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

		private OSGeo.MapGuide.MaestroAPI.TextSymbolType m_item;
		internal System.Windows.Forms.Label verticalLabel;
		internal System.Windows.Forms.Label horizontalLabel;
        private CheckBox DisplayLabel;
		private bool isUpdating = false;

        private Globalizator.Globalizator m_globalizor;

		public event EventHandler Changed;

		public FontStyleEditor()
		{
			if (SharedComboDataSet == null)
			{
				System.IO.Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "FontStyleComboDataset.xml");
				byte[] buf = new byte[s.Length];
				if (s.Read(buf, 0, (int)s.Length) != s.Length)
					throw new Exception("Failed while reading data from assembly");
				SharedComboDataSet = buf;
			}

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			using(System.IO.MemoryStream ms = new System.IO.MemoryStream(SharedComboDataSet))
				ComboBoxDataSet.ReadXml(ms);

            backgroundColor.ResetColors();
            textColor.ResetColors();
            m_globalizor = new Globalizator.Globalizator(this);
		}

		public void SetAvalibleColumns(string[] items)
		{
			propertyCombo.Items.Clear();
			if (items != null)
				propertyCombo.Items.AddRange(items);
			//propertyCombo.Items.Add("Expression...");
		}

		private void UpdateDisplay()
		{
			if (isUpdating)
				return;
			try
			{
				isUpdating = true;

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
					fontCombo.SelectedValue = m_item.FontName;
				sizeContextCombo.SelectedValue = m_item.SizeContext.ToString();
				unitsCombo.SelectedValue = m_item.Unit.ToString();
				if (m_item.SizeX == null)
					sizeCombo.Text = "";
				else
					sizeCombo.Text = m_item.SizeX.ToString();

				boldCheck.Checked = m_item.Bold == "true";
				italicCheck.Checked = m_item.Italic == "true";
				underlineCheck.Checked = m_item.Underlined == "true";
				textColor.CurrentColor = m_item.ForegroundColor;
				backgroundColor.CurrentColor = m_item.BackgroundColor;
				backgroundTypeCombo.SelectedValue = m_item.BackgroundStyle.ToString();
				if (m_item.HorizontalAlignment != null)
					horizontalCombo.SelectedValue = m_item.HorizontalAlignment;
				if (m_item.VerticalAlignment != null)
					verticalCombo.SelectedValue = m_item.VerticalAlignment;
			}
			finally
			{
				isUpdating = false;
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
            this.FontTable = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
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
            this.colorGroup = new System.Windows.Forms.GroupBox();
            this.backgroundTypeCombo = new System.Windows.Forms.ComboBox();
            this.BackgroundTypeTable = new System.Data.DataTable();
            this.dataColumn9 = new System.Data.DataColumn();
            this.dataColumn10 = new System.Data.DataColumn();
            this.backgroundColor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox();
            this.textColor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.ColorComboBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.FontTable)).BeginInit();
            this.panel1.SuspendLayout();
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
            this.fontGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
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
            this.fontGroup.Location = new System.Drawing.Point(0, 24);
            this.fontGroup.Name = "fontGroup";
            this.fontGroup.Size = new System.Drawing.Size(296, 208);
            this.fontGroup.TabIndex = 12;
            this.fontGroup.TabStop = false;
            this.fontGroup.Text = "Font";
            this.fontGroup.Enter += new System.EventHandler(this.fontGroup_Enter);
            // 
            // sizeCombo
            // 
            this.sizeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sizeCombo.Location = new System.Drawing.Point(112, 144);
            this.sizeCombo.Name = "sizeCombo";
            this.sizeCombo.Size = new System.Drawing.Size(176, 21);
            this.sizeCombo.TabIndex = 10;
            this.sizeCombo.Text = "comboBox5";
            this.sizeCombo.SelectedIndexChanged += new System.EventHandler(this.sizeCombo_SelectedIndexChanged);
            this.sizeCombo.TextChanged += new System.EventHandler(this.sizeCombo_TextChanged);
            // 
            // unitsCombo
            // 
            this.unitsCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.unitsCombo.DataSource = this.UnitsTable;
            this.unitsCombo.DisplayMember = "Display";
            this.unitsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.unitsCombo.Location = new System.Drawing.Point(112, 112);
            this.unitsCombo.Name = "unitsCombo";
            this.unitsCombo.Size = new System.Drawing.Size(176, 21);
            this.unitsCombo.TabIndex = 9;
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
            this.sizeContextCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sizeContextCombo.DataSource = this.SizeContextTable;
            this.sizeContextCombo.DisplayMember = "Display";
            this.sizeContextCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeContextCombo.Location = new System.Drawing.Point(112, 80);
            this.sizeContextCombo.Name = "sizeContextCombo";
            this.sizeContextCombo.Size = new System.Drawing.Size(176, 21);
            this.sizeContextCombo.TabIndex = 8;
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
            this.fontCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fontCombo.DataSource = this.FontTable;
            this.fontCombo.DisplayMember = "Display";
            this.fontCombo.Location = new System.Drawing.Point(112, 48);
            this.fontCombo.Name = "fontCombo";
            this.fontCombo.Size = new System.Drawing.Size(176, 21);
            this.fontCombo.TabIndex = 7;
            this.fontCombo.ValueMember = "Value";
            this.fontCombo.SelectedIndexChanged += new System.EventHandler(this.fontCombo_SelectedIndexChanged);
            this.fontCombo.TextChanged += new System.EventHandler(this.fontCombo_TextChanged);
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
            // propertyCombo
            // 
            this.propertyCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyCombo.Location = new System.Drawing.Point(112, 16);
            this.propertyCombo.Name = "propertyCombo";
            this.propertyCombo.Size = new System.Drawing.Size(176, 21);
            this.propertyCombo.TabIndex = 6;
            this.propertyCombo.SelectedIndexChanged += new System.EventHandler(this.propertyCombo_SelectedIndexChanged);
            this.propertyCombo.TextChanged += new System.EventHandler(this.propertyCombo_TextChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Style";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Size";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Units";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Size context";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Font";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Property";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.underlineCheck);
            this.panel1.Controls.Add(this.italicCheck);
            this.panel1.Controls.Add(this.boldCheck);
            this.panel1.Location = new System.Drawing.Point(112, 176);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(176, 24);
            this.panel1.TabIndex = 11;
            // 
            // underlineCheck
            // 
            this.underlineCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.underlineCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.underlineCheck.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.underlineCheck.Location = new System.Drawing.Point(64, 0);
            this.underlineCheck.Name = "underlineCheck";
            this.underlineCheck.Size = new System.Drawing.Size(24, 24);
            this.underlineCheck.TabIndex = 5;
            this.underlineCheck.Text = "U";
            this.underlineCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.underlineCheck.CheckedChanged += new System.EventHandler(this.underlineCheck_CheckedChanged);
            // 
            // italicCheck
            // 
            this.italicCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.italicCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.italicCheck.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.italicCheck.Location = new System.Drawing.Point(32, 0);
            this.italicCheck.Name = "italicCheck";
            this.italicCheck.Size = new System.Drawing.Size(24, 24);
            this.italicCheck.TabIndex = 4;
            this.italicCheck.Text = "I";
            this.italicCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.italicCheck.CheckedChanged += new System.EventHandler(this.italicCheck_CheckedChanged);
            // 
            // boldCheck
            // 
            this.boldCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.boldCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.boldCheck.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boldCheck.Location = new System.Drawing.Point(0, 0);
            this.boldCheck.Name = "boldCheck";
            this.boldCheck.Size = new System.Drawing.Size(24, 24);
            this.boldCheck.TabIndex = 3;
            this.boldCheck.Text = "B";
            this.boldCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.boldCheck.CheckedChanged += new System.EventHandler(this.boldCheck_CheckedChanged);
            // 
            // colorGroup
            // 
            this.colorGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.colorGroup.Controls.Add(this.backgroundTypeCombo);
            this.colorGroup.Controls.Add(this.backgroundColor);
            this.colorGroup.Controls.Add(this.textColor);
            this.colorGroup.Controls.Add(this.label7);
            this.colorGroup.Controls.Add(this.label8);
            this.colorGroup.Controls.Add(this.label9);
            this.colorGroup.Location = new System.Drawing.Point(0, 240);
            this.colorGroup.Name = "colorGroup";
            this.colorGroup.Size = new System.Drawing.Size(296, 112);
            this.colorGroup.TabIndex = 14;
            this.colorGroup.TabStop = false;
            this.colorGroup.Text = "Colors";
            // 
            // backgroundTypeCombo
            // 
            this.backgroundTypeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.backgroundTypeCombo.DataSource = this.BackgroundTypeTable;
            this.backgroundTypeCombo.DisplayMember = "Display";
            this.backgroundTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.backgroundTypeCombo.Location = new System.Drawing.Point(112, 78);
            this.backgroundTypeCombo.Name = "backgroundTypeCombo";
            this.backgroundTypeCombo.Size = new System.Drawing.Size(176, 21);
            this.backgroundTypeCombo.TabIndex = 11;
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
            // backgroundColor
            // 
            this.backgroundColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.backgroundColor.Location = new System.Drawing.Point(112, 46);
            this.backgroundColor.Name = "backgroundColor";
            this.backgroundColor.Size = new System.Drawing.Size(176, 21);
            this.backgroundColor.TabIndex = 10;
            this.backgroundColor.SelectedIndexChanged += new System.EventHandler(this.backgroundColor_SelectedIndexChanged);
            // 
            // textColor
            // 
            this.textColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textColor.Location = new System.Drawing.Point(112, 14);
            this.textColor.Name = "textColor";
            this.textColor.Size = new System.Drawing.Size(176, 21);
            this.textColor.TabIndex = 9;
            this.textColor.SelectedIndexChanged += new System.EventHandler(this.textColor_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "Background type";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 16);
            this.label8.TabIndex = 4;
            this.label8.Text = "Background";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 16);
            this.label9.TabIndex = 3;
            this.label9.Text = "Text";
            // 
            // alignmentGroup
            // 
            this.alignmentGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.alignmentGroup.Controls.Add(this.rotationCombo);
            this.alignmentGroup.Controls.Add(this.verticalCombo);
            this.alignmentGroup.Controls.Add(this.horizontalCombo);
            this.alignmentGroup.Controls.Add(this.label10);
            this.alignmentGroup.Controls.Add(this.verticalLabel);
            this.alignmentGroup.Controls.Add(this.horizontalLabel);
            this.alignmentGroup.Location = new System.Drawing.Point(0, 360);
            this.alignmentGroup.Name = "alignmentGroup";
            this.alignmentGroup.Size = new System.Drawing.Size(296, 112);
            this.alignmentGroup.TabIndex = 15;
            this.alignmentGroup.TabStop = false;
            this.alignmentGroup.Text = "Alignment";
            // 
            // rotationCombo
            // 
            this.rotationCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rotationCombo.DataSource = this.RotationTable;
            this.rotationCombo.DisplayMember = "Display";
            this.rotationCombo.Location = new System.Drawing.Point(112, 78);
            this.rotationCombo.Name = "rotationCombo";
            this.rotationCombo.Size = new System.Drawing.Size(176, 21);
            this.rotationCombo.TabIndex = 11;
            this.rotationCombo.ValueMember = "Value";
            this.rotationCombo.SelectedIndexChanged += new System.EventHandler(this.rotationCombo_SelectedIndexChanged);
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
            this.verticalCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalCombo.DataSource = this.VerticalTable;
            this.verticalCombo.DisplayMember = "Display";
            this.verticalCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.verticalCombo.Location = new System.Drawing.Point(112, 46);
            this.verticalCombo.Name = "verticalCombo";
            this.verticalCombo.Size = new System.Drawing.Size(176, 21);
            this.verticalCombo.TabIndex = 10;
            this.verticalCombo.ValueMember = "Value";
            this.verticalCombo.SelectedIndexChanged += new System.EventHandler(this.verticalCombo_SelectedIndexChanged);
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
            this.horizontalCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalCombo.DataSource = this.HorizontalTable;
            this.horizontalCombo.DisplayMember = "Display";
            this.horizontalCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.horizontalCombo.Location = new System.Drawing.Point(112, 14);
            this.horizontalCombo.Name = "horizontalCombo";
            this.horizontalCombo.Size = new System.Drawing.Size(176, 21);
            this.horizontalCombo.TabIndex = 9;
            this.horizontalCombo.ValueMember = "Value";
            this.horizontalCombo.SelectedIndexChanged += new System.EventHandler(this.horizontalCombo_SelectedIndexChanged);
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
            this.label10.Location = new System.Drawing.Point(8, 80);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 16);
            this.label10.TabIndex = 5;
            this.label10.Text = "Rotation";
            // 
            // verticalLabel
            // 
            this.verticalLabel.Location = new System.Drawing.Point(8, 48);
            this.verticalLabel.Name = "verticalLabel";
            this.verticalLabel.Size = new System.Drawing.Size(80, 16);
            this.verticalLabel.TabIndex = 4;
            this.verticalLabel.Text = "Vertical";
            // 
            // horizontalLabel
            // 
            this.horizontalLabel.Location = new System.Drawing.Point(8, 16);
            this.horizontalLabel.Name = "horizontalLabel";
            this.horizontalLabel.Size = new System.Drawing.Size(64, 16);
            this.horizontalLabel.TabIndex = 3;
            this.horizontalLabel.Text = "Horizontal";
            // 
            // previewGroup
            // 
            this.previewGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewGroup.Controls.Add(this.previewPicture);
            this.previewGroup.Location = new System.Drawing.Point(0, 480);
            this.previewGroup.Name = "previewGroup";
            this.previewGroup.Size = new System.Drawing.Size(296, 48);
            this.previewGroup.TabIndex = 16;
            this.previewGroup.TabStop = false;
            this.previewGroup.Text = "Preview";
            // 
            // previewPicture
            // 
            this.previewPicture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewPicture.BackColor = System.Drawing.Color.White;
            this.previewPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewPicture.Location = new System.Drawing.Point(8, 16);
            this.previewPicture.Name = "previewPicture";
            this.previewPicture.Size = new System.Drawing.Size(280, 24);
            this.previewPicture.TabIndex = 0;
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
            this.DisplayLabel.AutoSize = true;
            this.DisplayLabel.Checked = true;
            this.DisplayLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayLabel.Location = new System.Drawing.Point(0, 0);
            this.DisplayLabel.Name = "DisplayLabel";
            this.DisplayLabel.Size = new System.Drawing.Size(126, 17);
            this.DisplayLabel.TabIndex = 17;
            this.DisplayLabel.Text = "Display feature labels";
            this.DisplayLabel.UseVisualStyleBackColor = true;
            this.DisplayLabel.CheckedChanged += new System.EventHandler(this.DisplayLabel_CheckedChanged);
            // 
            // FontStyleEditor
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(296, 531);
            this.Controls.Add(this.DisplayLabel);
            this.Controls.Add(this.previewGroup);
            this.Controls.Add(this.colorGroup);
            this.Controls.Add(this.fontGroup);
            this.Controls.Add(this.alignmentGroup);
            this.Name = "FontStyleEditor";
            this.Size = new System.Drawing.Size(296, 531);
            this.fontGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontTable)).EndInit();
            this.panel1.ResumeLayout(false);
            this.colorGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundTypeTable)).EndInit();
            this.alignmentGroup.ResumeLayout(false);
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
			if (isUpdating)
				return;
			m_item.Text = propertyCombo.Text; //(string)propertyCombo.SelectedItem;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void fontCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			m_item.FontName = (string)fontCombo.SelectedValue;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void sizeContextCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			m_item.SizeContext = (OSGeo.MapGuide.MaestroAPI.SizeContextType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.SizeContextType), (string)sizeContextCombo.SelectedValue);
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void unitsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			m_item.Unit = (OSGeo.MapGuide.MaestroAPI.LengthUnitType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.LengthUnitType), (string)unitsCombo.SelectedValue);
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void sizeCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			//TODO: Validate
			m_item.SizeX = m_item.SizeY = sizeCombo.Text;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void boldCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			m_item.Bold = boldCheck.Checked ? "true" : null;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void italicCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			m_item.Italic = italicCheck.Checked ? "true" : null;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void underlineCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			m_item.Underlined = underlineCheck.Checked ? "true" : null;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void textColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			m_item.ForegroundColor = textColor.CurrentColor;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void backgroundColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			m_item.BackgroundColor = backgroundColor.CurrentColor;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void backgroundTypeCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			m_item.BackgroundStyle = (OSGeo.MapGuide.MaestroAPI.BackgroundStyleType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.BackgroundStyleType), (string)backgroundTypeCombo.SelectedValue);
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void horizontalCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			m_item.HorizontalAlignment = (string)horizontalCombo.SelectedValue;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void verticalCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			m_item.VerticalAlignment = (string)verticalCombo.SelectedValue;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void rotationCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			//TODO: Validate
			m_item.Rotation = sizeCombo.Text;
			previewPicture.Refresh();

			if (Changed != null)
				Changed(this, new EventArgs());
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
			propertyCombo_SelectedIndexChanged(sender, e);
		}

		public OSGeo.MapGuide.MaestroAPI.TextSymbolType Item
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

            if (isUpdating)
                return;

            if (DisplayLabel.Checked)
            {
                if (DisplayLabel.Tag as OSGeo.MapGuide.MaestroAPI.TextSymbolType != null)
                    this.Item = DisplayLabel.Tag as OSGeo.MapGuide.MaestroAPI.TextSymbolType;
                if (m_item == null)
                    this.Item = DefaultItemGenerator.CreateTextSymbolType();
            }
            else
            {
                DisplayLabel.Tag = m_item;
                this.Item = null;
            }

        }

        private void sizeCombo_TextChanged(object sender, EventArgs e)
        {
            sizeCombo_SelectedIndexChanged(sender, e);
        }

        private void fontCombo_TextChanged(object sender, EventArgs e)
        {
            fontCombo_SelectedIndexChanged(sender, e);
        }


	}
}
