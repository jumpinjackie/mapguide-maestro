#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
using System.Data;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors
{
	/// <summary>
	/// Summary description for PointFeatureStyleEditor.
	/// </summary>
	public class PointFeatureStyleEditor : System.Windows.Forms.UserControl
	{
		private static byte[] SharedComboDataSet = null;

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.PictureBox previewPicture;
		private System.Data.DataSet ComboBoxDataSet;
		private System.Data.DataTable SymbolMarkTable;
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
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ComboBox HeigthText;
		private System.Windows.Forms.ComboBox WidthText;
		private System.Windows.Forms.ComboBox RotationBox;
		private System.Windows.Forms.TextBox ReferenceX;
		private System.Windows.Forms.CheckBox MaintainAspectRatio;
		private System.Windows.Forms.ComboBox SizeUnits;
		private System.Windows.Forms.ComboBox SizeContext;
		private System.Windows.Forms.ComboBox Symbol;
		private System.Windows.Forms.TextBox ReferenceY;
		private ResourceEditors.GeometryStyleEditors.FillStyleEditor fillStyleEditor;
		private ResourceEditors.GeometryStyleEditors.LineStyleEditor lineStyleEditor;

		private OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType m_item;
		private bool m_inUpdate = false;

		private OSGeo.MapGuide.MaestroAPI.FillType previousFill = null;
        private CheckBox DisplayPoints;
		private OSGeo.MapGuide.MaestroAPI.StrokeType previousEdge = null;
        private Globalizator.Globalizator m_globalizor;

		public event EventHandler Changed;

		public PointFeatureStyleEditor()
		{
			if (SharedComboDataSet == null)
			{
				System.IO.Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "PointStyleComboDataset.xml");
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

			fillStyleEditor.displayFill.CheckedChanged += new EventHandler(displayFill_CheckedChanged);
			fillStyleEditor.fillCombo.SelectedIndexChanged += new EventHandler(fillCombo_SelectedIndexChanged);
			fillStyleEditor.foregroundColor.SelectedIndexChanged += new EventHandler(foregroundColor_SelectedIndexChanged);
			fillStyleEditor.backgroundColor.SelectedIndexChanged +=new EventHandler(backgroundColor_SelectedIndexChanged);

			lineStyleEditor.displayLine.CheckedChanged +=new EventHandler(displayLine_CheckedChanged);
			lineStyleEditor.thicknessUpDown.ValueChanged +=new EventHandler(thicknessCombo_SelectedIndexChanged);
			lineStyleEditor.colorCombo.SelectedIndexChanged +=new EventHandler(colorCombo_SelectedIndexChanged);
			lineStyleEditor.fillCombo.SelectedIndexChanged +=new EventHandler(fillCombo_Line_SelectedIndexChanged);

			m_item = new OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType();
			m_item.Item = new OSGeo.MapGuide.MaestroAPI.MarkSymbolType();

            m_globalizor = new Globalizator.Globalizator(this);
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
                    DisplayPoints.Checked = false;
                    return;
                }

                DisplayPoints.Checked = true;

				if (m_item.Item == null)
					m_item.Item = new OSGeo.MapGuide.MaestroAPI.MarkSymbolType();

				WidthText.Text = m_item.Item.SizeX;
				HeigthText.Text = m_item.Item.SizeY;
				RotationBox.Text = m_item.Item.Rotation;
				ReferenceX.Text = m_item.Item.InsertionPointX.ToString();
				ReferenceY.Text = m_item.Item.InsertionPointY.ToString();
				MaintainAspectRatio.Checked = m_item.Item.MaintainAspect;
				SizeUnits.SelectedValue = m_item.Item.Unit;
				SizeContext.SelectedValue = m_item.Item.SizeContext.ToString();

				if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				{
					OSGeo.MapGuide.MaestroAPI.MarkSymbolType t = (OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item;
					Symbol.SelectedValue = t.Shape.ToString();

					fillStyleEditor.displayFill.Checked = t.Fill != null;
					if (t.Fill != null)
					{
						fillStyleEditor.foregroundColor.CurrentColor = t.Fill.ForegroundColor;
						fillStyleEditor.backgroundColor.CurrentColor = t.Fill.BackgroundColor;
						fillStyleEditor.fillCombo.SelectedValue = t.Fill.FillPattern;
						if (fillStyleEditor.fillCombo.SelectedItem == null && fillStyleEditor.fillCombo.Items.Count > 0)
							fillStyleEditor.fillCombo.SelectedIndex = fillStyleEditor.fillCombo.FindString(t.Fill.FillPattern);
					}

					lineStyleEditor.displayLine.Checked = t.Edge != null;
					if (t.Edge != null)
					{
						lineStyleEditor.fillCombo.SelectedValue = t.Edge.LineStyle;
						if (lineStyleEditor.fillCombo.SelectedItem == null && lineStyleEditor.fillCombo.Items.Count > 0)
							lineStyleEditor.fillCombo.SelectedIndex = lineStyleEditor.fillCombo.FindString(t.Edge.LineStyle);

						lineStyleEditor.colorCombo.CurrentColor = t.Edge.Color;
						double o;
						if (double.TryParse(t.Edge.Thickness, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out o))
							lineStyleEditor.thicknessUpDown.Value = (decimal)o;
						else
							lineStyleEditor.thicknessUpDown.Value = 0;
					}


				}
				else
					//TODO: Fix this
					MessageBox.Show(this, "Only symbols of type \"Mark\" are currently supported", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

				previewPicture.Refresh();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RotationBox = new System.Windows.Forms.ComboBox();
            this.RotationTable = new System.Data.DataTable();
            this.dataColumn7 = new System.Data.DataColumn();
            this.dataColumn8 = new System.Data.DataColumn();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ReferenceY = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ReferenceX = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.MaintainAspectRatio = new System.Windows.Forms.CheckBox();
            this.HeigthText = new System.Windows.Forms.ComboBox();
            this.WidthText = new System.Windows.Forms.ComboBox();
            this.SizeUnits = new System.Windows.Forms.ComboBox();
            this.UnitsTable = new System.Data.DataTable();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.SizeContext = new System.Windows.Forms.ComboBox();
            this.SizeContextTable = new System.Data.DataTable();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.Symbol = new System.Windows.Forms.ComboBox();
            this.SymbolMarkTable = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fillStyleEditor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.FillStyleEditor();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lineStyleEditor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.LineStyleEditor();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.ComboBoxDataSet = new System.Data.DataSet();
            this.DisplayPoints = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SymbolMarkTable)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.RotationBox);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.ReferenceY);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.ReferenceX);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.MaintainAspectRatio);
            this.groupBox1.Controls.Add(this.HeigthText);
            this.groupBox1.Controls.Add(this.WidthText);
            this.groupBox1.Controls.Add(this.SizeUnits);
            this.groupBox1.Controls.Add(this.SizeContext);
            this.groupBox1.Controls.Add(this.Symbol);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 256);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Symbol style";
            // 
            // RotationBox
            // 
            this.RotationBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RotationBox.DataSource = this.RotationTable;
            this.RotationBox.DisplayMember = "Display";
            this.RotationBox.Location = new System.Drawing.Point(128, 224);
            this.RotationBox.Name = "RotationBox";
            this.RotationBox.Size = new System.Drawing.Size(208, 21);
            this.RotationBox.TabIndex = 18;
            this.RotationBox.ValueMember = "Value";
            this.RotationBox.SelectedIndexChanged += new System.EventHandler(this.RotationBox_SelectedIndexChanged);
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
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(16, 232);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 16);
            this.label9.TabIndex = 17;
            this.label9.Text = "Rotation";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(288, 192);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 24);
            this.button1.TabIndex = 16;
            this.button1.Text = "...";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ReferenceY
            // 
            this.ReferenceY.Location = new System.Drawing.Point(232, 192);
            this.ReferenceY.Name = "ReferenceY";
            this.ReferenceY.Size = new System.Drawing.Size(48, 20);
            this.ReferenceY.TabIndex = 15;
            this.ReferenceY.TextChanged += new System.EventHandler(this.ReferenceY_TextChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(216, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 16);
            this.label8.TabIndex = 14;
            this.label8.Text = "Y";
            // 
            // ReferenceX
            // 
            this.ReferenceX.Location = new System.Drawing.Point(144, 192);
            this.ReferenceX.Name = "ReferenceX";
            this.ReferenceX.Size = new System.Drawing.Size(48, 20);
            this.ReferenceX.TabIndex = 13;
            this.ReferenceX.TextChanged += new System.EventHandler(this.ReferenceX_TextChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(128, 192);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(16, 16);
            this.label7.TabIndex = 12;
            this.label7.Text = "X";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 192);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 16);
            this.label6.TabIndex = 11;
            this.label6.Text = "Reference point";
            // 
            // MaintainAspectRatio
            // 
            this.MaintainAspectRatio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MaintainAspectRatio.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.MaintainAspectRatio.Location = new System.Drawing.Point(128, 168);
            this.MaintainAspectRatio.Name = "MaintainAspectRatio";
            this.MaintainAspectRatio.Size = new System.Drawing.Size(208, 16);
            this.MaintainAspectRatio.TabIndex = 10;
            this.MaintainAspectRatio.Text = "Maintain aspect ratio";
            // 
            // HeigthText
            // 
            this.HeigthText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.HeigthText.Location = new System.Drawing.Point(128, 144);
            this.HeigthText.Name = "HeigthText";
            this.HeigthText.Size = new System.Drawing.Size(208, 21);
            this.HeigthText.TabIndex = 9;
            this.HeigthText.SelectedIndexChanged += new System.EventHandler(this.HeigthText_SelectedIndexChanged);
            this.HeigthText.TextChanged += new System.EventHandler(this.HeigthText_TextChanged);
            // 
            // WidthText
            // 
            this.WidthText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.WidthText.Location = new System.Drawing.Point(128, 112);
            this.WidthText.Name = "WidthText";
            this.WidthText.Size = new System.Drawing.Size(208, 21);
            this.WidthText.TabIndex = 8;
            this.WidthText.SelectedIndexChanged += new System.EventHandler(this.WidthText_SelectedIndexChanged);
            this.WidthText.TextChanged += new System.EventHandler(this.WidthText_TextChanged);
            // 
            // SizeUnits
            // 
            this.SizeUnits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SizeUnits.DataSource = this.UnitsTable;
            this.SizeUnits.DisplayMember = "Display";
            this.SizeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SizeUnits.Location = new System.Drawing.Point(128, 80);
            this.SizeUnits.Name = "SizeUnits";
            this.SizeUnits.Size = new System.Drawing.Size(208, 21);
            this.SizeUnits.TabIndex = 7;
            this.SizeUnits.ValueMember = "Value";
            this.SizeUnits.SelectedIndexChanged += new System.EventHandler(this.SizeUnits_SelectedIndexChanged);
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
            // SizeContext
            // 
            this.SizeContext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SizeContext.DataSource = this.SizeContextTable;
            this.SizeContext.DisplayMember = "Display";
            this.SizeContext.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SizeContext.Location = new System.Drawing.Point(128, 48);
            this.SizeContext.Name = "SizeContext";
            this.SizeContext.Size = new System.Drawing.Size(208, 21);
            this.SizeContext.TabIndex = 6;
            this.SizeContext.ValueMember = "Value";
            this.SizeContext.SelectedIndexChanged += new System.EventHandler(this.SizeContext_SelectedIndexChanged);
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
            // Symbol
            // 
            this.Symbol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Symbol.DataSource = this.SymbolMarkTable;
            this.Symbol.DisplayMember = "Display";
            this.Symbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Symbol.Location = new System.Drawing.Point(128, 16);
            this.Symbol.Name = "Symbol";
            this.Symbol.Size = new System.Drawing.Size(208, 21);
            this.Symbol.TabIndex = 5;
            this.Symbol.ValueMember = "Value";
            this.Symbol.SelectedIndexChanged += new System.EventHandler(this.Symbol_SelectedIndexChanged);
            // 
            // SymbolMarkTable
            // 
            this.SymbolMarkTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2});
            this.SymbolMarkTable.TableName = "SymbolMark";
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
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Height";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Width";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Size units";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Size context";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Symbol";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.fillStyleEditor);
            this.groupBox2.Location = new System.Drawing.Point(0, 288);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(344, 128);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Symbol fill";
            // 
            // fillStyleEditor
            // 
            this.fillStyleEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fillStyleEditor.Location = new System.Drawing.Point(8, 16);
            this.fillStyleEditor.Name = "fillStyleEditor";
            this.fillStyleEditor.Size = new System.Drawing.Size(328, 104);
            this.fillStyleEditor.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lineStyleEditor);
            this.groupBox3.Location = new System.Drawing.Point(0, 424);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(344, 128);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Symbol border";
            // 
            // lineStyleEditor
            // 
            this.lineStyleEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lineStyleEditor.Location = new System.Drawing.Point(8, 16);
            this.lineStyleEditor.Name = "lineStyleEditor";
            this.lineStyleEditor.Size = new System.Drawing.Size(328, 104);
            this.lineStyleEditor.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.previewPicture);
            this.groupBox4.Location = new System.Drawing.Point(0, 560);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(344, 48);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Preview";
            // 
            // previewPicture
            // 
            this.previewPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewPicture.BackColor = System.Drawing.Color.White;
            this.previewPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewPicture.Location = new System.Drawing.Point(8, 16);
            this.previewPicture.Name = "previewPicture";
            this.previewPicture.Size = new System.Drawing.Size(328, 24);
            this.previewPicture.TabIndex = 0;
            this.previewPicture.TabStop = false;
            this.previewPicture.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPicture_Paint);
            // 
            // ComboBoxDataSet
            // 
            this.ComboBoxDataSet.DataSetName = "ComboBoxDataSet";
            this.ComboBoxDataSet.Locale = new System.Globalization.CultureInfo("da-DK");
            this.ComboBoxDataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.SymbolMarkTable,
            this.SizeContextTable,
            this.UnitsTable,
            this.RotationTable});
            // 
            // DisplayPoints
            // 
            this.DisplayPoints.AutoSize = true;
            this.DisplayPoints.Checked = true;
            this.DisplayPoints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayPoints.Location = new System.Drawing.Point(0, 0);
            this.DisplayPoints.Name = "DisplayPoints";
            this.DisplayPoints.Size = new System.Drawing.Size(91, 17);
            this.DisplayPoints.TabIndex = 8;
            this.DisplayPoints.Text = "Display points";
            this.DisplayPoints.UseVisualStyleBackColor = true;
            this.DisplayPoints.CheckedChanged += new System.EventHandler(this.DisplayPoints_CheckedChanged);
            // 
            // PointFeatureStyleEditor
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(344, 584);
            this.Controls.Add(this.DisplayPoints);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Name = "PointFeatureStyleEditor";
            this.Size = new System.Drawing.Size(344, 610);
            this.Load += new System.EventHandler(this.PointFeatureStyleEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SymbolMarkTable)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void PointFeatureStyleEditor_Load(object sender, System.EventArgs e)
		{
			UpdateDisplay();

			ComboBoxDataSet.WriteXmlSchema("testschema.xsd");
			ComboBoxDataSet.WriteXml("testcontent.xml");
		}

		private void previewPicture_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            if (m_item != null && m_item.Item as OSGeo.MapGuide.MaestroAPI.MarkSymbolType != null)
                FeaturePreviewRender.RenderPreviewPoint(e.Graphics, new Rectangle(1, 1, previewPicture.Width - 2, previewPicture.Height - 2), (OSGeo.MapGuide.MaestroAPI.MarkSymbolType)m_item.Item);
            else
                FeaturePreviewRender.RenderPreviewPoint(e.Graphics, new Rectangle(1, 1, previewPicture.Width - 2, previewPicture.Height - 2), null);
		}

		private void Symbol_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Shape = (OSGeo.MapGuide.MaestroAPI.ShapeType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.ShapeType), (string)Symbol.SelectedValue);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void SizeContext_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).SizeContext = (OSGeo.MapGuide.MaestroAPI.SizeContextType)Enum.Parse((typeof(OSGeo.MapGuide.MaestroAPI.SizeContextType)), (string)SizeContext.SelectedValue);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void SizeUnits_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Unit = (OSGeo.MapGuide.MaestroAPI.LengthUnitType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.LengthUnitType), (string) SizeUnits.SelectedValue);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void WidthText_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

			//TODO: Validate
			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).SizeX = WidthText.Text;
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void HeigthText_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

			//TODO: Validate
			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).SizeY = HeigthText.Text;
			previewPicture.Refresh();		
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void ReferenceX_TextChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				try { 
                    ((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).InsertionPointX = double.Parse(ReferenceX.Text);
                    ((OSGeo.MapGuide.MaestroAPI.MarkSymbolType)m_item.Item).InsertionPointXSpecified = true;
                }
				catch { } //TODO: Handle better
			previewPicture.Refresh();		
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void ReferenceY_TextChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				try { 
                    ((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).InsertionPointY = double.Parse( ReferenceY.Text );
                    ((OSGeo.MapGuide.MaestroAPI.MarkSymbolType)m_item.Item).InsertionPointYSpecified = true;
                }
				catch {} //TODO: Handle better
			previewPicture.Refresh();		
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void RotationBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

			//TODO: Validate
			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Rotation = (string)RotationBox.SelectedValue;
			previewPicture.Refresh();
		
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void displayFill_CheckedChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				if (fillStyleEditor.displayFill.Checked)
					((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Fill = previousFill == null ? new OSGeo.MapGuide.MaestroAPI.FillType() : previousFill;
				else
				{
					if (((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Fill != null)
						previousFill = ((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Fill;
					((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Fill = null;
				}
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void displayLine_CheckedChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				if (lineStyleEditor.displayLine.Checked)
					((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Edge = previousEdge == null ? new OSGeo.MapGuide.MaestroAPI.StrokeType() : previousEdge;
				else
				{
					if (((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Edge != null)
						previousEdge = ((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Edge;
					((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Edge = null;
				}
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void fillCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Fill.FillPattern = fillStyleEditor.fillCombo.Text;
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void foregroundColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Fill.ForegroundColor = fillStyleEditor.foregroundColor.CurrentColor;
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void backgroundColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Fill.BackgroundColor = fillStyleEditor.backgroundColor.CurrentColor;
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void thicknessCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			//TODO: Validate
			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Edge.Thickness =  lineStyleEditor.thicknessUpDown.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void colorCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Edge.Color = lineStyleEditor.colorCombo.CurrentColor;
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void fillCombo_Line_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			//TODO: Validate
			if (m_item.Item.GetType() == typeof(OSGeo.MapGuide.MaestroAPI.MarkSymbolType))
				((OSGeo.MapGuide.MaestroAPI.MarkSymbolType) m_item.Item).Edge.LineStyle = lineStyleEditor.fillCombo.Text;
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this, "This method is not yet implemented", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType Item
		{
			get { return m_item; }
			set 
			{
				m_item = value;
				UpdateDisplay();
			}
		}

        private void DisplayPoints_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
                c.Enabled = c == DisplayPoints || DisplayPoints.Checked;

            if (m_inUpdate)
                return;

            if (DisplayPoints.Checked)
            {
                if (DisplayPoints.Tag as OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType != null)
                    this.Item = DisplayPoints.Tag as OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType;
                if (m_item == null)
                    this.Item = new OSGeo.MapGuide.MaestroAPI.PointSymbolization2DType();
            }
            else
            {
                DisplayPoints.Tag = m_item;
                this.Item = null;
            }
        }

        private void WidthText_TextChanged(object sender, EventArgs e)
        {
            WidthText_SelectedIndexChanged(sender, e);
        }

        private void HeigthText_TextChanged(object sender, EventArgs e)
        {
            HeigthText_SelectedIndexChanged(sender, e);
        }
    }
}
