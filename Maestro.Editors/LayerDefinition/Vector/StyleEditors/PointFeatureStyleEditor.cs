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
using System.Data;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System.Globalization;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels;
using System.Threading;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
	/// <summary>
	/// Summary description for PointFeatureStyleEditor.
	/// </summary>
	internal class PointFeatureStyleEditor : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
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
		private System.Windows.Forms.ComboBox SizeUnits;
		private System.Windows.Forms.ComboBox SizeContext;
		private System.Windows.Forms.ComboBox Symbol;
		private LineStyleEditor lineStyleEditor;

		private IPointSymbolization2D m_item;
        private IMarkSymbol m_lastMark = null;
        private IFontSymbol m_lastFont = null;

		private bool m_inUpdate = false;

		private IFill previousFill = null;
        private CheckBox DisplayPoints;
		private IStroke previousEdge = null;
		private GroupBox groupBoxFont;
		private ComboBox fontCombo;
		private Label label10;
		private ComboBox comboBoxCharacter;
		private GroupBox groupBoxSymbolLocation;
		private Button button1;
		private TextBox ReferenceY;
		private Label label8;
		private TextBox ReferenceX;
		private Label label7;
		private Label label6;
		private CheckBox MaintainAspectRatio;
		private ComboBox RotationBox;
		private Label label9;
        private FillStyleEditor fillStyleEditor;
		private Label lblForeground;
        private Panel panel1;
        private ToolStrip toolStrip1;
        private ToolStripButton FontBoldButton;
        private ToolStripButton FontItalicButton;
        private ColorComboWithTransparency colorFontForeground;
        private Label label11;
        private ToolStripButton FontUnderlineButton;

        public event EventHandler Changed;

        private IEditorService m_editor;
        private ClassDefinition m_schema;
        private string m_featureSource;
        private string m_providername;
        private ILayerElementFactory _factory;

        public PointFeatureStyleEditor(IEditorService editor, ClassDefinition schema, string featureSource)
            : this()
        {
            m_editor = editor;
            m_schema = schema;

            _factory = (ILayerElementFactory)editor.GetEditedResource();
            var fs = (IFeatureSource)editor.ResourceService.GetResource(featureSource);

            m_providername = fs.Provider;
            m_featureSource = featureSource;

            m_item = _factory.CreateDefaultPointSymbolization2D();
        }

		private PointFeatureStyleEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            using(System.IO.StringReader sr = new System.IO.StringReader(Properties.Resources.GeometryStyleComboDataset))
				ComboBoxDataSet.ReadXml(sr);

			fontCombo.Items.Clear();
            foreach (FontFamily f in new System.Drawing.Text.InstalledFontCollection().Families)
                fontCombo.Items.Add(f.Name);

			colorFontForeground.CurrentColorChanged += new EventHandler(colourFontForeground_CurrentColorChanged);

			fillStyleEditor.displayFill.CheckedChanged += new EventHandler(displayFill_CheckedChanged);
			fillStyleEditor.fillCombo.SelectedIndexChanged += new EventHandler(fillCombo_SelectedIndexChanged);
			fillStyleEditor.foregroundColor.CurrentColorChanged += new EventHandler(foregroundColor_CurrentColorChanged);
			fillStyleEditor.backgroundColor.CurrentColorChanged +=new EventHandler(backgroundColor_CurrentColorChanged);

			lineStyleEditor.displayLine.CheckedChanged +=new EventHandler(displayLine_CheckedChanged);
			lineStyleEditor.thicknessCombo.SelectedIndexChanged += new EventHandler(thicknessCombo_SelectedIndexChanged);
            lineStyleEditor.thicknessCombo.TextChanged += new EventHandler(thicknessCombo_TextChanged);
			lineStyleEditor.colorCombo.CurrentColorChanged +=new EventHandler(colorCombo_CurrentColorChanged);
			lineStyleEditor.fillCombo.SelectedIndexChanged +=new EventHandler(fillCombo_Line_SelectedIndexChanged);
		}

		private void setUIForMarkSymbol(bool enabled)
		{
            groupBoxSymbolLocation.Enabled = enabled;
            groupBox2.Enabled = enabled;
            groupBox3.Enabled = enabled;

            groupBoxFont.Enabled = !enabled;
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

                if (m_item.Symbol == null)
                    m_item.Symbol = _factory.CreateDefaultMarkSymbol();

				// shared values
                WidthText.Text = DoubleToString(m_item.Symbol.SizeX);
                HeigthText.Text = DoubleToString(m_item.Symbol.SizeY);
                RotationBox.SelectedIndex = -1;
                RotationBox.Text = DoubleToString(m_item.Symbol.Rotation);

                SizeUnits.SelectedValue = m_item.Symbol.Unit;
                SizeContext.SelectedValue = m_item.Symbol.SizeContext.ToString();

				// specifics
				if (m_item.Symbol.Type == PointSymbolType.Mark)
				{
                    MaintainAspectRatio.Checked = m_item.Symbol.MaintainAspect;
					double d;
                    ReferenceX.Text = DoubleToString(m_item.Symbol.InsertionPointX);

                    //if (double.TryParse(m_item.Item.InsertionPointY, NumberStyles.Float, CultureInfo.InvariantCulture, out d))
                    //    ReferenceY.Text = d.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture);
                    //else
                    //    ReferenceY.Text = m_item.Item.InsertionPointY;
                    ReferenceY.Text = DoubleToString(m_item.Symbol.InsertionPointY);

                    IMarkSymbol t = (IMarkSymbol)m_item.Symbol;
					Symbol.SelectedValue = t.Shape.ToString();

					fillStyleEditor.displayFill.Checked = t.Fill != null;
					if (t.Fill != null)
					{
                        fillStyleEditor.foregroundColor.CurrentColor = Utility.ParseHTMLColor(t.Fill.ForegroundColor);
                        fillStyleEditor.backgroundColor.CurrentColor = Utility.ParseHTMLColor(t.Fill.BackgroundColor);
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

                        lineStyleEditor.colorCombo.CurrentColor = Utility.ParseHTMLColor(t.Edge.Color);
						lineStyleEditor.thicknessCombo.Text = t.Edge.Thickness;
					}

					setUIForMarkSymbol(true);
				}
				else if (m_item.Symbol.Type == PointSymbolType.Font)
				{
					IFontSymbol f = (IFontSymbol)m_item.Symbol;

					// TODO: Dislike this hard coding, but with association from 'Shape' the 'Font...' string cannot be found or set from the Symbol combobox
					Symbol.SelectedIndex = 6;

                    fontCombo.SelectedIndex = fontCombo.FindString(f.FontName);
                    if (string.Compare(fontCombo.Text, f.FontName, true) == 0)
                        fontCombo.Text = f.FontName;

                    comboBoxCharacter.SelectedIndex = comboBoxCharacter.FindString(f.Character);
                    if (comboBoxCharacter.Text != f.Character)
                        comboBoxCharacter.Text = f.Character;

                    FontBoldButton.Checked = f.Bold.HasValue && f.Bold.Value;
                    FontItalicButton.Checked = f.Italic.HasValue && f.Italic.Value;
                    FontUnderlineButton.Checked = f.Underlined.HasValue && f.Underlined.Value;

                    if (string.IsNullOrEmpty(f.ForegroundColor))
                        colorFontForeground.CurrentColor = Color.Black;
                    else
                        colorFontForeground.CurrentColor = Utility.ParseHTMLColor(f.ForegroundColor);

					setUIForMarkSymbol(false);
				}
				else
					//TODO: Fix this
					MessageBox.Show(this, Properties.Resources.SymbolTypeNotSupported, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PointFeatureStyleEditor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RotationBox = new System.Windows.Forms.ComboBox();
            this.RotationTable = new System.Data.DataTable();
            this.dataColumn7 = new System.Data.DataColumn();
            this.dataColumn8 = new System.Data.DataColumn();
            this.label9 = new System.Windows.Forms.Label();
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
            this.fillStyleEditor = new FillStyleEditor();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lineStyleEditor = new LineStyleEditor();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.ComboBoxDataSet = new System.Data.DataSet();
            this.DisplayPoints = new System.Windows.Forms.CheckBox();
            this.groupBoxFont = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.colorFontForeground = new ColorComboWithTransparency();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.FontBoldButton = new System.Windows.Forms.ToolStripButton();
            this.FontItalicButton = new System.Windows.Forms.ToolStripButton();
            this.FontUnderlineButton = new System.Windows.Forms.ToolStripButton();
            this.lblForeground = new System.Windows.Forms.Label();
            this.comboBoxCharacter = new System.Windows.Forms.ComboBox();
            this.fontCombo = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBoxSymbolLocation = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ReferenceY = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ReferenceX = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.MaintainAspectRatio = new System.Windows.Forms.CheckBox();
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
            this.groupBoxFont.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBoxSymbolLocation.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.RotationBox);
            this.groupBox1.Controls.Add(this.label9);
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
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // RotationBox
            // 
            resources.ApplyResources(this.RotationBox, "RotationBox");
            this.RotationBox.DataSource = this.RotationTable;
            this.RotationBox.DisplayMember = "Display";
            this.RotationBox.Name = "RotationBox";
            this.RotationBox.ValueMember = "Value";
            this.RotationBox.SelectedIndexChanged += new System.EventHandler(this.RotationBox_SelectedIndexChanged);
            this.RotationBox.TextChanged += new System.EventHandler(this.RotationBox_TextChanged);
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
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // HeigthText
            // 
            resources.ApplyResources(this.HeigthText, "HeigthText");
            this.HeigthText.Items.AddRange(new object[] {
            resources.GetString("HeigthText.Items")});
            this.HeigthText.Name = "HeigthText";
            this.HeigthText.SelectedIndexChanged += new System.EventHandler(this.HeigthText_SelectedIndexChanged);
            this.HeigthText.TextChanged += new System.EventHandler(this.HeigthText_TextChanged);
            // 
            // WidthText
            // 
            resources.ApplyResources(this.WidthText, "WidthText");
            this.WidthText.Items.AddRange(new object[] {
            resources.GetString("WidthText.Items")});
            this.WidthText.Name = "WidthText";
            this.WidthText.SelectedIndexChanged += new System.EventHandler(this.WidthText_SelectedIndexChanged);
            this.WidthText.TextChanged += new System.EventHandler(this.WidthText_TextChanged);
            // 
            // SizeUnits
            // 
            resources.ApplyResources(this.SizeUnits, "SizeUnits");
            this.SizeUnits.DataSource = this.UnitsTable;
            this.SizeUnits.DisplayMember = "Display";
            this.SizeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SizeUnits.Name = "SizeUnits";
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
            resources.ApplyResources(this.SizeContext, "SizeContext");
            this.SizeContext.DataSource = this.SizeContextTable;
            this.SizeContext.DisplayMember = "Display";
            this.SizeContext.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SizeContext.Name = "SizeContext";
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
            resources.ApplyResources(this.Symbol, "Symbol");
            this.Symbol.DataSource = this.SymbolMarkTable;
            this.Symbol.DisplayMember = "Display";
            this.Symbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Symbol.Name = "Symbol";
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
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.fillStyleEditor);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // fillStyleEditor
            // 
            resources.ApplyResources(this.fillStyleEditor, "fillStyleEditor");
            this.fillStyleEditor.Name = "fillStyleEditor";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.lineStyleEditor);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // lineStyleEditor
            // 
            resources.ApplyResources(this.lineStyleEditor, "lineStyleEditor");
            this.lineStyleEditor.Name = "lineStyleEditor";
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.previewPicture);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
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
            this.SymbolMarkTable,
            this.SizeContextTable,
            this.UnitsTable,
            this.RotationTable});
            // 
            // DisplayPoints
            // 
            resources.ApplyResources(this.DisplayPoints, "DisplayPoints");
            this.DisplayPoints.Checked = true;
            this.DisplayPoints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayPoints.Name = "DisplayPoints";
            this.DisplayPoints.UseVisualStyleBackColor = true;
            this.DisplayPoints.CheckedChanged += new System.EventHandler(this.DisplayPoints_CheckedChanged);
            // 
            // groupBoxFont
            // 
            resources.ApplyResources(this.groupBoxFont, "groupBoxFont");
            this.groupBoxFont.Controls.Add(this.label11);
            this.groupBoxFont.Controls.Add(this.colorFontForeground);
            this.groupBoxFont.Controls.Add(this.panel1);
            this.groupBoxFont.Controls.Add(this.lblForeground);
            this.groupBoxFont.Controls.Add(this.comboBoxCharacter);
            this.groupBoxFont.Controls.Add(this.fontCombo);
            this.groupBoxFont.Controls.Add(this.label10);
            this.groupBoxFont.Name = "groupBoxFont";
            this.groupBoxFont.TabStop = false;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // colorFontForeground
            // 
            resources.ApplyResources(this.colorFontForeground, "colorFontForeground");
            this.colorFontForeground.CurrentColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorFontForeground.Name = "colorFontForeground";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FontBoldButton,
            this.FontItalicButton,
            this.FontUnderlineButton});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // FontBoldButton
            // 
            this.FontBoldButton.CheckOnClick = true;
            this.FontBoldButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.FontBoldButton, "FontBoldButton");
            this.FontBoldButton.Name = "FontBoldButton";
            this.FontBoldButton.Click += new System.EventHandler(this.FontBoldButton_Click);
            // 
            // FontItalicButton
            // 
            this.FontItalicButton.CheckOnClick = true;
            this.FontItalicButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.FontItalicButton, "FontItalicButton");
            this.FontItalicButton.Name = "FontItalicButton";
            this.FontItalicButton.Click += new System.EventHandler(this.FontItalicButton_Click);
            // 
            // FontUnderlineButton
            // 
            this.FontUnderlineButton.CheckOnClick = true;
            this.FontUnderlineButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.FontUnderlineButton, "FontUnderlineButton");
            this.FontUnderlineButton.Name = "FontUnderlineButton";
            this.FontUnderlineButton.Click += new System.EventHandler(this.FontUnderlineButton_Click);
            // 
            // lblForeground
            // 
            resources.ApplyResources(this.lblForeground, "lblForeground");
            this.lblForeground.Name = "lblForeground";
            // 
            // comboBoxCharacter
            // 
            resources.ApplyResources(this.comboBoxCharacter, "comboBoxCharacter");
            this.comboBoxCharacter.DisplayMember = "Display";
            this.comboBoxCharacter.Name = "comboBoxCharacter";
            this.comboBoxCharacter.ValueMember = "Value";
            this.comboBoxCharacter.SelectedIndexChanged += new System.EventHandler(this.comboBoxCharacter_SelectedIndexChanged);
            this.comboBoxCharacter.TextChanged += new System.EventHandler(this.comboBoxCharacter_TextChanged);
            // 
            // fontCombo
            // 
            resources.ApplyResources(this.fontCombo, "fontCombo");
            this.fontCombo.DisplayMember = "Display";
            this.fontCombo.Name = "fontCombo";
            this.fontCombo.ValueMember = "Value";
            this.fontCombo.SelectedIndexChanged += new System.EventHandler(this.fontCombo_SelectedIndexChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // groupBoxSymbolLocation
            // 
            resources.ApplyResources(this.groupBoxSymbolLocation, "groupBoxSymbolLocation");
            this.groupBoxSymbolLocation.Controls.Add(this.button1);
            this.groupBoxSymbolLocation.Controls.Add(this.ReferenceY);
            this.groupBoxSymbolLocation.Controls.Add(this.label8);
            this.groupBoxSymbolLocation.Controls.Add(this.ReferenceX);
            this.groupBoxSymbolLocation.Controls.Add(this.label7);
            this.groupBoxSymbolLocation.Controls.Add(this.label6);
            this.groupBoxSymbolLocation.Controls.Add(this.MaintainAspectRatio);
            this.groupBoxSymbolLocation.Name = "groupBoxSymbolLocation";
            this.groupBoxSymbolLocation.TabStop = false;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            // 
            // ReferenceY
            // 
            resources.ApplyResources(this.ReferenceY, "ReferenceY");
            this.ReferenceY.Name = "ReferenceY";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // ReferenceX
            // 
            resources.ApplyResources(this.ReferenceX, "ReferenceX");
            this.ReferenceX.Name = "ReferenceX";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // MaintainAspectRatio
            // 
            resources.ApplyResources(this.MaintainAspectRatio, "MaintainAspectRatio");
            this.MaintainAspectRatio.Name = "MaintainAspectRatio";
            // 
            // PointFeatureStyleEditor
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBoxSymbolLocation);
            this.Controls.Add(this.groupBoxFont);
            this.Controls.Add(this.DisplayPoints);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Name = "PointFeatureStyleEditor";
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
            this.groupBoxFont.ResumeLayout(false);
            this.groupBoxFont.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBoxSymbolLocation.ResumeLayout(false);
            this.groupBoxSymbolLocation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void PointFeatureStyleEditor_Load(object sender, System.EventArgs e)
		{
			UpdateDisplay();
		}

		private void previewPicture_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			if (m_item != null && m_item.Symbol.Type == PointSymbolType.Mark)
				FeaturePreviewRender.RenderPreviewPoint(e.Graphics, new Rectangle(1, 1, previewPicture.Width - 2, previewPicture.Height - 2), (IMarkSymbol)m_item.Symbol);
			else if (m_item != null && m_item.Symbol.Type == PointSymbolType.Font)
                FeaturePreviewRender.RenderPreviewFontSymbol(e.Graphics, new Rectangle(1, 1, previewPicture.Width - 2, previewPicture.Height - 2), (IFontSymbol)m_item.Symbol);
			else
                FeaturePreviewRender.RenderPreviewPoint(e.Graphics, new Rectangle(1, 1, previewPicture.Width - 2, previewPicture.Height - 2), null);
		}

		private void Symbol_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

            bool isSymbol = false;
            ShapeType selectedShape = ShapeType.Circle;

			// see if need to change symbol type
            foreach (string s in Enum.GetNames(typeof(ShapeType)))
                if (string.Compare(s, (string)Symbol.SelectedValue, true) == 0)
                {
                    selectedShape = (ShapeType)Enum.Parse(typeof(ShapeType), s);
                    isSymbol = true;
                    break;
                }

            if (m_item.Symbol.Type == PointSymbolType.Mark)
                m_lastMark = (IMarkSymbol)m_item.Symbol;
            else if (m_item.Symbol.Type == PointSymbolType.Font)
                m_lastFont = (IFontSymbol)m_item.Symbol;

            if (isSymbol)
            {
                bool update = m_item.Symbol != m_lastMark;

                if (m_lastMark == null)
                    m_lastMark = _factory.CreateDefaultMarkSymbol();

                m_lastMark.Shape = selectedShape;
                m_item.Symbol = m_lastMark;
                
                setUIForMarkSymbol(true);
                if (update)
                    UpdateDisplay();
            }
			else if (Symbol.SelectedIndex == 6)
			{
			    // user wants to change away FROM a valid 'Mark' symbol type
			    // if ("Font..." == Symbol.SelectedText)

                bool update = m_item.Symbol != m_lastFont;

                if (m_lastFont == null)
                {
                    m_lastFont = _factory.CreateDefaultFontSymbol();
                    m_lastFont.SizeContext = SizeContextType.DeviceUnits;
                    m_lastFont.Rotation = 0.0;
                    m_lastFont.SizeX = 10;
                    m_lastFont.SizeY = 10;
                    m_lastFont.Unit = LengthUnitType.Points;
                }

                m_item.Symbol = m_lastFont;
                setUIForMarkSymbol(false);
                if (update)
                    UpdateDisplay();
            }
			else
			{
				MessageBox.Show(this, Properties.Resources.SymbolTypeNotSupported, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void SizeContext_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

            if (m_item.Symbol.Type == PointSymbolType.Mark)
                ((IMarkSymbol)m_item.Symbol).SizeContext = (SizeContextType)Enum.Parse((typeof(SizeContextType)), (string)SizeContext.SelectedValue);
            else if (m_item.Symbol.Type == PointSymbolType.Font)
                ((IFontSymbol)m_item.Symbol).SizeContext = (SizeContextType)Enum.Parse((typeof(SizeContextType)), (string)SizeContext.SelectedValue);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void SizeUnits_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Symbol.Type == PointSymbolType.Mark || m_item.Symbol.Type == PointSymbolType.Font)
                m_item.Symbol.Unit = (LengthUnitType)Enum.Parse(typeof(LengthUnitType), (string)SizeUnits.SelectedValue);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
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

		private void WidthText_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

            if (WidthText.SelectedIndex == WidthText.Items.Count - 1)
            {
                string current = null;
                if (m_item.Symbol.Type == PointSymbolType.Mark || m_item.Symbol.Type == PointSymbolType.Font)
                    current = DoubleToString(m_item.Symbol.SizeX);

                string expr = null;
                if (current != null)
                {
                    expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource);
                    if (!string.IsNullOrEmpty(expr))
                        current = expr;
                }

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), WidthText, current, expr != null);
            }
		}

		private void HeigthText_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

            if (HeigthText.SelectedIndex == HeigthText.Items.Count - 1)
            {
                string current = null;
                if (m_item.Symbol.Type == PointSymbolType.Mark || m_item.Symbol.Type == PointSymbolType.Font)
                    current = DoubleToString(m_item.Symbol.SizeY);

                string expr = null;
                if (current != null)
                {
                    expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource);
                    if (!string.IsNullOrEmpty(expr))
                        current = expr;
                }

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), HeigthText, current, expr != null);
            }
		}

		private void ReferenceX_TextChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

            if (m_item.Symbol.Type == PointSymbolType.Mark)
            {
                double d;
                if (ReferenceX.Text.Trim().Length == 0)
                    m_item.Symbol.InsertionPointY = 0.5;
                else if (double.TryParse(ReferenceX.Text, System.Globalization.NumberStyles.Float, System.Threading.Thread.CurrentThread.CurrentUICulture, out d) || double.TryParse(ReferenceX.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out d))
                    m_item.Symbol.InsertionPointX = StringToDouble(Math.Min(Math.Max(0.0, d), 1.0).ToString(System.Globalization.CultureInfo.InvariantCulture));
                else
                    m_item.Symbol.InsertionPointX = StringToDouble(ReferenceX.Text);
            }
			previewPicture.Refresh();		
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void ReferenceY_TextChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

            if (m_item.Symbol.Type == PointSymbolType.Mark)
            {
                double d;
                if (double.TryParse(ReferenceY.Text, System.Globalization.NumberStyles.Float, System.Threading.Thread.CurrentThread.CurrentUICulture, out d) || double.TryParse(ReferenceY.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out d))
                    m_item.Symbol.InsertionPointY = Math.Min(Math.Max(0.0, d), 1.0);
                else
                    m_item.Symbol.InsertionPointY = 0.5;
            }
			previewPicture.Refresh();		
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void RotationBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

            if (RotationBox.SelectedIndex == RotationBox.Items.Count - 1)
            {
                string current = null;
                if (m_item.Symbol.Type == PointSymbolType.Mark || m_item.Symbol.Type == PointSymbolType.Font)
                    current = DoubleToString(m_item.Symbol.Rotation);

                string expr = null;
                if (current != null)
                {
                    expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource);
                    if (!string.IsNullOrEmpty(expr))
                        current = expr;
                }

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), RotationBox, current, expr != null);
            }
            else if (RotationBox.SelectedIndex != -1)
            {
                if (m_item.Symbol.Type == PointSymbolType.Mark || m_item.Symbol.Type == PointSymbolType.Font)
                    m_item.Symbol.Rotation = StringToDouble((string)RotationBox.SelectedValue);

                //RotationBox.SelectedIndex = -1;
            }

		}

		private void displayFill_CheckedChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Symbol.Type == PointSymbolType.Mark)
				if (fillStyleEditor.displayFill.Checked)
					((IMarkSymbol) m_item.Symbol).Fill = previousFill == null ? _factory.CreateDefaultFill() : previousFill;
				else
				{
                    if (((IMarkSymbol)m_item.Symbol).Fill != null)
                        previousFill = ((IMarkSymbol)m_item.Symbol).Fill;
                    ((IMarkSymbol)m_item.Symbol).Fill = null;
				}
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void displayLine_CheckedChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Symbol.Type == PointSymbolType.Mark)
				if (lineStyleEditor.displayLine.Checked)
					((IMarkSymbol) m_item.Symbol).Edge = previousEdge == null ? _factory.CreateDefaultStroke() : previousEdge;
				else
				{
                    if (((IMarkSymbol)m_item.Symbol).Edge != null)
                        previousEdge = ((IMarkSymbol)m_item.Symbol).Edge;
                    ((IMarkSymbol)m_item.Symbol).Edge = null;
				}
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void fillCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Symbol.Type == PointSymbolType.Mark)
				((IMarkSymbol) m_item.Symbol).Fill.FillPattern = fillStyleEditor.fillCombo.Text;
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void foregroundColor_CurrentColorChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

            if (m_item.Symbol.Type == PointSymbolType.Mark)
                ((IMarkSymbol)m_item.Symbol).Fill.ForegroundColor = Utility.SerializeHTMLColor(fillStyleEditor.foregroundColor.CurrentColor, true);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void backgroundColor_CurrentColorChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

            if (m_item.Symbol.Type == PointSymbolType.Mark)
                ((IMarkSymbol)m_item.Symbol).Fill.BackgroundColor = Utility.SerializeHTMLColor(fillStyleEditor.backgroundColor.CurrentColor, true);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}


        private void thicknessCombo_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || lineStyleEditor.thicknessCombo.SelectedIndex != -1)
                return;

            //TODO: Validate
            if (m_item.Symbol.Type == PointSymbolType.Mark)
                ((IMarkSymbol)m_item.Symbol).Edge.Thickness = lineStyleEditor.thicknessCombo.Text;
            previewPicture.Refresh();
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void thicknessCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate || lineStyleEditor.thicknessCombo.SelectedIndex != lineStyleEditor.thicknessCombo.Items.Count - 1)
				return;

                string current = null;
                if (m_item.Symbol.Type == PointSymbolType.Mark)
                    current = ((IMarkSymbol)m_item.Symbol).Edge.Thickness;

                string expr = null;
                if (current != null)
                {
                    expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource);
                    if (!string.IsNullOrEmpty(expr))
                        current = expr;
                }

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), lineStyleEditor.thicknessCombo, current, expr != null);
        }

		private void colorCombo_CurrentColorChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

            if (m_item.Symbol.Type == PointSymbolType.Mark)
                ((IMarkSymbol)m_item.Symbol).Edge.Color = Utility.SerializeHTMLColor(lineStyleEditor.colorCombo.CurrentColor, true);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void fillCombo_Line_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (m_item.Symbol.Type == PointSymbolType.Mark)
                ((IMarkSymbol)m_item.Symbol).Edge.LineStyle = lineStyleEditor.fillCombo.Text;
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void colourFontForeground_CurrentColorChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

            if (m_item.Symbol.Type == PointSymbolType.Font)
                ((IFontSymbol)m_item.Symbol).ForegroundColor = Utility.SerializeHTMLColor(colorFontForeground.CurrentColor, true);

            previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void fontCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			//TODO: Validate
            if (!(m_item.Symbol is IFontSymbol))
				return;
            ((IFontSymbol)m_item.Symbol).FontName = fontCombo.Text;

			comboBoxCharacter.Items.Clear();
			try
			{
				comboBoxCharacter.Font = new Font(fontCombo.SelectedText, (float)8.25);
			}
			catch
			{
				MessageBox.Show(this, string.Format(Properties.Resources.SymbolTypeNotSupported, fontCombo.SelectedText), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			// populate with a basic A-Z
			for (char c = 'A'; c < 'Z'; c++)
				comboBoxCharacter.Items.Add(c);

			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void comboBoxCharacter_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (m_inUpdate)
				return;

			//TODO: Validate
			if (m_item.Symbol.Type != PointSymbolType.Font)
				return;

			((IFontSymbol)m_item.Symbol).Character = comboBoxCharacter.Text;

			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void comboBoxCharacter_TextChanged(object sender, System.EventArgs e)
		{
			comboBoxCharacter_SelectedIndexChanged(sender, e);
		}


		public IPointSymbolization2D Item
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
                if (DisplayPoints.Tag as IPointSymbolization2D != null)
                    this.Item = DisplayPoints.Tag as IPointSymbolization2D;
                if (m_item == null)
                    this.Item = _factory.CreateDefaultPointSymbolization2D();
            }
            else
            {
                DisplayPoints.Tag = m_item;
                this.Item = null;
            }
        }

        static string DoubleToString(double? value)
        {
            return value.HasValue ? value.Value.ToString() : null;
        }

        static double? StringToDouble(string value)
        {
            double d;
            if (double.TryParse(value, out d))
                return d;

            return null;
        }

        private void WidthText_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || WidthText.SelectedIndex != -1)
                return;

            //TODO: Validate
            if (m_item.Symbol.Type == PointSymbolType.Mark || m_item.Symbol.Type == PointSymbolType.Font)
                m_item.Symbol.SizeX = StringToDouble(WidthText.Text);
            previewPicture.Refresh();
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void HeigthText_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || HeigthText.SelectedIndex != -1)
                return;

            //TODO: Validate
            if (m_item.Symbol.Type == PointSymbolType.Mark)
                m_item.Symbol.SizeY = StringToDouble(HeigthText.Text);
            else if (m_item.Symbol.Type == PointSymbolType.Font)
                m_item.Symbol.SizeY = StringToDouble(HeigthText.Text);
            previewPicture.Refresh();
            if (Changed != null)
                Changed(this, new EventArgs());
        }

		private void RotationBox_TextChanged(object sender, EventArgs e)
		{
            if (m_inUpdate || RotationBox.SelectedIndex != -1)
                return;

            //TODO: Validate
            if (m_item.Symbol.Type == PointSymbolType.Mark)
                m_item.Symbol.Rotation = StringToDouble(RotationBox.Text);
            else if (m_item.Symbol.Type == PointSymbolType.Font)
                m_item.Symbol.Rotation = StringToDouble(RotationBox.Text);
            previewPicture.Refresh();
            if (Changed != null)
                Changed(this, new EventArgs());
		}
		
		private void ReferenceY_Leave(object sender, EventArgs e)
        {
            //double d;
            //if (m_item.Item is IMarkSymbol)
            //    if (!double.TryParse(((IMarkSymbol)m_item.Item).InsertionPointY, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out d))
            //        MessageBox.Show(this, Properties.Resources.InsertionPointYError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void FontBoldButton_Click(object sender, EventArgs e)
        {
            if (m_inUpdate)
                return;

            if (m_item.Symbol.Type == PointSymbolType.Font)
            {
                var fs = (IFontSymbol)m_item.Symbol;
                fs.Bold = FontBoldButton.Checked;
            }

            previewPicture.Refresh();
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void FontItalicButton_Click(object sender, EventArgs e)
        {
            if (m_inUpdate)
                return;

            if (m_item.Symbol.Type == PointSymbolType.Font)
            {
                var fs = (IFontSymbol)m_item.Symbol;
                fs.Italic = FontItalicButton.Checked;
            }

            previewPicture.Refresh();
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void FontUnderlineButton_Click(object sender, EventArgs e)
        {
            if (m_inUpdate)
                return;

            if (m_item.Symbol.Type == PointSymbolType.Font)
            {
                var fs = (IFontSymbol)m_item.Symbol;
                fs.Underlined = FontUnderlineButton.Checked;
            }

            previewPicture.Refresh();
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        internal void SetupForTheming()
        {
            fillStyleEditor.foregroundColor.Enabled = 
            lineStyleEditor.lblColor.Enabled = 
            colorFontForeground.Enabled =
            lblForeground.Enabled =
            DisplayPoints.Enabled =
            fillStyleEditor.displayFill.Enabled =
                false;
        }
    }
}
