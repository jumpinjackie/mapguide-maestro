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
	/// Summary description for AreaFeatureStyleEditor.
	/// </summary>
	public class AreaFeatureStyleEditor : System.Windows.Forms.UserControl
	{
		private static byte[] SharedComboDataSet = null;
		private System.Windows.Forms.ComboBox sizeContextCombo;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TrackBar Transparency;
		private System.Windows.Forms.Label percentageLabel;
		private System.Windows.Forms.GroupBox previewGroup;
		private System.Windows.Forms.PictureBox previewPicture;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Data.DataSet ComboBoxDataSet;
		private System.Data.DataTable SizeContextTable;
		private System.Data.DataColumn dataColumn3;
		private System.Data.DataColumn dataColumn4;
		private System.Data.DataTable UnitsTable;
		private System.Data.DataColumn dataColumn5;
		private System.Data.DataColumn dataColumn6;

		private OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType m_item;
		private ResourceEditors.GeometryStyleEditors.FillStyleEditor fillStyleEditor;
		private ResourceEditors.GeometryStyleEditors.LineStyleEditor lineStyleEditor;
		private System.Windows.Forms.ComboBox sizeUnitsCombo;
		private bool m_inUpdate = false;

		private OSGeo.MapGuide.MaestroAPI.FillType previousFill = null;
		private OSGeo.MapGuide.MaestroAPI.StrokeType previousStroke = null;

        private Globalizator.Globalizator m_globalizor;

		public event EventHandler Changed;

		public AreaFeatureStyleEditor()
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

            m_globalizor = new Globalizator.Globalizator(this);
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
            this.fillStyleEditor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.FillStyleEditor();
            this.lineStyleEditor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.LineStyleEditor();
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
            this.percentageLabel = new System.Windows.Forms.Label();
            this.Transparency = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.previewGroup = new System.Windows.Forms.GroupBox();
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.ComboBoxDataSet = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Transparency)).BeginInit();
            this.previewGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // fillStyleEditor
            // 
            this.fillStyleEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fillStyleEditor.Location = new System.Drawing.Point(16, 16);
            this.fillStyleEditor.Name = "fillStyleEditor";
            this.fillStyleEditor.Size = new System.Drawing.Size(218, 104);
            this.fillStyleEditor.TabIndex = 0;
            // 
            // lineStyleEditor
            // 
            this.lineStyleEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lineStyleEditor.Location = new System.Drawing.Point(16, 16);
            this.lineStyleEditor.Name = "lineStyleEditor";
            this.lineStyleEditor.Size = new System.Drawing.Size(218, 112);
            this.lineStyleEditor.TabIndex = 1;
            // 
            // sizeUnitsCombo
            // 
            this.sizeUnitsCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sizeUnitsCombo.DataSource = this.UnitsTable;
            this.sizeUnitsCombo.DisplayMember = "Display";
            this.sizeUnitsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeUnitsCombo.Location = new System.Drawing.Point(136, 152);
            this.sizeUnitsCombo.Name = "sizeUnitsCombo";
            this.sizeUnitsCombo.Size = new System.Drawing.Size(98, 21);
            this.sizeUnitsCombo.TabIndex = 13;
            this.sizeUnitsCombo.ValueMember = "Value";
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
            this.sizeContextCombo.Location = new System.Drawing.Point(136, 120);
            this.sizeContextCombo.Name = "sizeContextCombo";
            this.sizeContextCombo.Size = new System.Drawing.Size(98, 21);
            this.sizeContextCombo.TabIndex = 12;
            this.sizeContextCombo.ValueMember = "Value";
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
            this.label4.Location = new System.Drawing.Point(16, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Units";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Size context";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.sizeUnitsCombo);
            this.groupBox1.Controls.Add(this.sizeContextCombo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lineStyleEditor);
            this.groupBox1.Location = new System.Drawing.Point(0, 176);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 184);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edge style";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.percentageLabel);
            this.groupBox2.Controls.Add(this.Transparency);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.fillStyleEditor);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(242, 168);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fill style";
            // 
            // percentageLabel
            // 
            this.percentageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.percentageLabel.Location = new System.Drawing.Point(194, 128);
            this.percentageLabel.Name = "percentageLabel";
            this.percentageLabel.Size = new System.Drawing.Size(40, 16);
            this.percentageLabel.TabIndex = 3;
            this.percentageLabel.Text = "0%";
            this.percentageLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Transparency
            // 
            this.Transparency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Transparency.Location = new System.Drawing.Point(136, 120);
            this.Transparency.Maximum = 255;
            this.Transparency.Name = "Transparency";
            this.Transparency.Size = new System.Drawing.Size(58, 42);
            this.Transparency.TabIndex = 2;
            this.Transparency.TickFrequency = 25;
            this.Transparency.ValueChanged += new System.EventHandler(this.Transparency_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Transparency";
            // 
            // previewGroup
            // 
            this.previewGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewGroup.Controls.Add(this.previewPicture);
            this.previewGroup.Location = new System.Drawing.Point(0, 368);
            this.previewGroup.Name = "previewGroup";
            this.previewGroup.Size = new System.Drawing.Size(242, 48);
            this.previewGroup.TabIndex = 19;
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
            this.previewPicture.Size = new System.Drawing.Size(226, 24);
            this.previewPicture.TabIndex = 0;
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
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(244, 416);
            this.Controls.Add(this.previewGroup);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "AreaFeatureStyleEditor";
            this.Size = new System.Drawing.Size(244, 416);
            this.Load += new System.EventHandler(this.AreaFeatureStyleEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Transparency)).EndInit();
            this.previewGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void AreaFeatureStyleEditor_Load(object sender, System.EventArgs e)
		{
			//UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			if (m_inUpdate)
				return;

			try
			{
				if (m_item == null)
				{
					m_inUpdate = true;
					fillStyleEditor.displayFill.Checked = false;
					lineStyleEditor.displayLine.Checked = false;
					return;
				}

				fillStyleEditor.displayFill.Checked = m_item.Fill != null;
				if (m_item.Fill != null)
				{
                    int alpha = Transparency.Maximum - m_item.Fill.ForegroundColor.A;
                    fillStyleEditor.foregroundColor.CurrentColor = Color.FromArgb(255, m_item.Fill.ForegroundColor);
                    if (m_item.Fill.BackgroundColor.A == 0)
                        fillStyleEditor.backgroundColor.CurrentColor = Color.Transparent;
                    else
                        fillStyleEditor.backgroundColor.CurrentColor = Color.FromArgb(255, m_item.Fill.BackgroundColor);
                    Transparency.Value = alpha;

					fillStyleEditor.fillCombo.SelectedValue = m_item.Fill.FillPattern;
					if (fillStyleEditor.fillCombo.SelectedItem == null && fillStyleEditor.fillCombo.Items.Count > 0)
						fillStyleEditor.fillCombo.SelectedIndex = fillStyleEditor.fillCombo.FindString(m_item.Fill.FillPattern);

				}
				
				lineStyleEditor.displayLine.Checked = m_item.Stroke != null;
				if (m_item.Stroke != null)
				{
					sizeUnitsCombo.SelectedValue = m_item.Stroke.Unit.ToString();
					//sizeContextCombo.SelectedValue = st.??;
					//TODO: Should probably allow user to select a 'null' color?
					if (m_item.Stroke.ColorAsHTML != null)
						lineStyleEditor.colorCombo.CurrentColor = m_item.Stroke.Color;
					lineStyleEditor.fillCombo.SelectedIndex = lineStyleEditor.fillCombo.FindString(m_item.Stroke.LineStyle);
					double o;
					if (double.TryParse(m_item.Stroke.Thickness, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out o))
						lineStyleEditor.thicknessUpDown.Value = (decimal)o;
					else
						lineStyleEditor.thicknessUpDown.Value = 0;

				}
				m_inUpdate = true;

				previewPicture.Refresh();
			} 
			finally
			{
				m_inUpdate = false;
			}
		}

		private void previewPicture_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			FeaturePreviewRender.RenderPreviewArea(e.Graphics, new Rectangle(1, 1, previewPicture.Width - 4, previewPicture.Height - 4), m_item);
		}

		public OSGeo.MapGuide.MaestroAPI.AreaSymbolizationFillType Item 
		{
			get { return m_item; }
			set
			{
				m_item = value;
				UpdateDisplay();
			}
		}

		private void fillCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			m_item.Fill.FillPattern = fillStyleEditor.fillCombo.Text;
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void displayFill_CheckedChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (fillStyleEditor.displayFill.Checked)
				m_item.Fill = previousFill == null ? new OSGeo.MapGuide.MaestroAPI.FillType() : previousFill;
			else
			{
				if (m_item.Fill != null)
					previousFill = m_item.Fill;
				m_item.Fill = null;
			}
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void foregroundColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			m_item.Fill.ForegroundColor = Color.FromArgb(Transparency.Maximum - Transparency.Value,  fillStyleEditor.foregroundColor.CurrentColor);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void backgroundColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

            int transparency;

            if (fillStyleEditor.backgroundColor.CurrentColor.A == 0)
                transparency = 0;
            else
                transparency = Transparency.Maximum - Transparency.Value;

			m_item.Fill.BackgroundColor = Color.FromArgb(transparency, fillStyleEditor.backgroundColor.CurrentColor);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void displayLine_CheckedChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (lineStyleEditor.displayLine.Checked)
				m_item.Stroke = previousStroke == null ? new OSGeo.MapGuide.MaestroAPI.StrokeType() : previousStroke;
			else
			{
				if (m_item.Stroke != null)
					previousStroke = m_item.Stroke;
				m_item.Stroke = null;
			}
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void thicknessCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			//TODO: Validate
			m_item.Stroke.Thickness = lineStyleEditor.thicknessUpDown.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void colorCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			m_item.Stroke.Color = lineStyleEditor.colorCombo.CurrentColor;
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void fillCombo_Line_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			//TODO: Validate
			m_item.Stroke.LineStyle = lineStyleEditor.fillCombo.Text;
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

        private void Transparency_ValueChanged(object sender, EventArgs e)
        {
            percentageLabel.Text = ((int)((Transparency.Value / 255.0) * 100)).ToString() + "%";

            if (m_inUpdate)
                return;
            if (m_item.Fill.BackgroundColorAsHTML == null)
                m_item.Fill.BackgroundColor = Color.Black;
            if (m_item.Fill.ForegroundColorAsHTML == null)
                m_item.Fill.ForegroundColor = Color.White;

            m_item.Fill.BackgroundColor = Color.FromArgb(Transparency.Maximum - Transparency.Value, m_item.Fill.BackgroundColor);
            m_item.Fill.ForegroundColor = Color.FromArgb(Transparency.Maximum - Transparency.Value, m_item.Fill.ForegroundColor);

            previewPicture.Refresh();
            if (Changed != null)
                Changed(this, new EventArgs());

        }
	}
}
