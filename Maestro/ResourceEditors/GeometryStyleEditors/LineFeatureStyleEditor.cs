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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors
{
	/// <summary>
	/// Summary description for LineFeatureStyleEditor.
	/// </summary>
	public class LineFeatureStyleEditor : System.Windows.Forms.UserControl
	{
		private static byte[] SharedComboDataSet = null;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox CompositeGroup;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox previewPicture;
		private System.Windows.Forms.CheckBox applyLineStyle;
		private System.Windows.Forms.CheckBox compositeLines;
		private System.Windows.Forms.ListBox lineStyles;
		private System.Windows.Forms.Panel propertyPanel;
		private System.Windows.Forms.ComboBox sizeUnitsCombo;
		private System.Windows.Forms.ComboBox sizeContextCombo;
		private ResourceEditors.GeometryStyleEditors.LineStyleEditor lineStyleEditor;
		
		private OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection m_item = null;
		private System.Windows.Forms.Panel compositePanel;
		private System.Windows.Forms.GroupBox lineGroup;
		private System.Windows.Forms.GroupBox sizeGroup;
		private System.Windows.Forms.GroupBox previewGroup;
		private System.Data.DataSet ComboBoxDataSet;
		private System.Data.DataTable SizeContextTable;
		private System.Data.DataColumn dataColumn3;
		private System.Data.DataColumn dataColumn4;
		private System.Data.DataTable UnitsTable;
		private System.Data.DataColumn dataColumn5;
		private System.Data.DataColumn dataColumn6;
		private bool isUpdating = false;
        private ToolStrip toolStrip1;
        private ToolStripButton AddStyleButton;
        private ToolStripButton RemoveStyleButton;
        private Globalizator.Globalizator m_globalizor;

		public event EventHandler Changed;

		public LineFeatureStyleEditor()
		{
			if (SharedComboDataSet == null)
			{
				System.IO.Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "LineStyleComboDataset.xml");
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

			lineStyleEditor.displayLine.Visible = false;
			lineStyleEditor.thicknessUpDown.ValueChanged +=new EventHandler(thicknessCombo_SelectedIndexChanged);
			lineStyleEditor.colorCombo.SelectedIndexChanged += new EventHandler(colorCombo_SelectedIndexChanged);
			lineStyleEditor.fillCombo.SelectedIndexChanged += new EventHandler(fillCombo_SelectedIndexChanged);


            m_globalizor = new Globalizator.Globalizator(this);
		}

		public void UpdateDisplay()
		{
			try
			{
				isUpdating = true;
				applyLineStyle.Checked = (m_item != null && m_item.Count != 0);

				lineStyles.Items.Clear();
				if (applyLineStyle.Checked)
					foreach(OSGeo.MapGuide.MaestroAPI.StrokeType st in m_item)
						lineStyles.Items.Add(st);

				compositeLines.Checked = lineStyles.Items.Count > 1;
				if (lineStyles.Items.Count > 0)
					lineStyles.SelectedIndex = 0;

				UpdateDisplayForSelected();

			}
			finally
			{
				isUpdating = false;
			}

		}

		private void UpdateDisplayForSelected()
		{
			bool prevUpdate = isUpdating;
			try
			{
				isUpdating = true;
				OSGeo.MapGuide.MaestroAPI.StrokeType st = this.CurrentStrokeType;
				sizeGroup.Enabled = 
				lineGroup.Enabled =
				previewGroup.Enabled =
					st != null;

                RemoveStyleButton.Enabled = st != null && m_item.Count > 1;

				if (st != null)
				{
				sizeUnitsCombo.SelectedValue = st.Unit.ToString();
					//sizeContextCombo.SelectedValue = st.??;
                    if (st.ColorAsHTML == null)
                        lineStyleEditor.colorCombo.CurrentColor = Color.White;
                    else
                        lineStyleEditor.colorCombo.CurrentColor = st.Color;

                    foreach(object i in lineStyleEditor.fillCombo.Items)
                        if (i as ImageStylePicker.NamedImage != null && (i as ImageStylePicker.NamedImage).Name == st.LineStyle)
                        {
                            lineStyleEditor.fillCombo.SelectedItem = i;
                            break;
                        }
					double o;
					if (double.TryParse(st.Thickness, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out o))
						lineStyleEditor.thicknessUpDown.Value = (decimal)o;
					else
						lineStyleEditor.thicknessUpDown.Value = 0;
				}
				previewPicture.Refresh();
			} 
			finally
			{
				isUpdating = prevUpdate;
			}

		}

		private OSGeo.MapGuide.MaestroAPI.StrokeType CurrentStrokeType
		{
			get 
			{
				if (lineStyles.Items.Count == 0)
					return null;
				else if (lineStyles.Items.Count == 1 || lineStyles.SelectedIndex <= 0)
					return (OSGeo.MapGuide.MaestroAPI.StrokeType)lineStyles.Items[0];
				else
					return (OSGeo.MapGuide.MaestroAPI.StrokeType)lineStyles.SelectedItem;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineFeatureStyleEditor));
            this.applyLineStyle = new System.Windows.Forms.CheckBox();
            this.compositeLines = new System.Windows.Forms.CheckBox();
            this.CompositeGroup = new System.Windows.Forms.GroupBox();
            this.lineStyles = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.compositePanel = new System.Windows.Forms.Panel();
            this.propertyPanel = new System.Windows.Forms.Panel();
            this.previewGroup = new System.Windows.Forms.GroupBox();
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.sizeGroup = new System.Windows.Forms.GroupBox();
            this.sizeUnitsCombo = new System.Windows.Forms.ComboBox();
            this.UnitsTable = new System.Data.DataTable();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.sizeContextCombo = new System.Windows.Forms.ComboBox();
            this.SizeContextTable = new System.Data.DataTable();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lineGroup = new System.Windows.Forms.GroupBox();
            this.lineStyleEditor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.LineStyleEditor();
            this.ComboBoxDataSet = new System.Data.DataSet();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddStyleButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveStyleButton = new System.Windows.Forms.ToolStripButton();
            this.CompositeGroup.SuspendLayout();
            this.panel1.SuspendLayout();
            this.compositePanel.SuspendLayout();
            this.propertyPanel.SuspendLayout();
            this.previewGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
            this.sizeGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).BeginInit();
            this.lineGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // applyLineStyle
            // 
            this.applyLineStyle.Checked = true;
            this.applyLineStyle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.applyLineStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.applyLineStyle.Location = new System.Drawing.Point(0, 0);
            this.applyLineStyle.Name = "applyLineStyle";
            this.applyLineStyle.Size = new System.Drawing.Size(176, 16);
            this.applyLineStyle.TabIndex = 0;
            this.applyLineStyle.Text = "Apply line style";
            this.applyLineStyle.CheckedChanged += new System.EventHandler(this.applyLineStyle_CheckedChanged);
            // 
            // compositeLines
            // 
            this.compositeLines.Checked = true;
            this.compositeLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.compositeLines.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.compositeLines.Location = new System.Drawing.Point(0, 24);
            this.compositeLines.Name = "compositeLines";
            this.compositeLines.Size = new System.Drawing.Size(272, 16);
            this.compositeLines.TabIndex = 1;
            this.compositeLines.Text = "Use composite lines";
            this.compositeLines.CheckedChanged += new System.EventHandler(this.compositeLines_CheckedChanged);
            // 
            // CompositeGroup
            // 
            this.CompositeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CompositeGroup.Controls.Add(this.toolStrip1);
            this.CompositeGroup.Controls.Add(this.lineStyles);
            this.CompositeGroup.Location = new System.Drawing.Point(0, 0);
            this.CompositeGroup.Name = "CompositeGroup";
            this.CompositeGroup.Size = new System.Drawing.Size(288, 160);
            this.CompositeGroup.TabIndex = 2;
            this.CompositeGroup.TabStop = false;
            this.CompositeGroup.Text = "Composite line";
            // 
            // lineStyles
            // 
            this.lineStyles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lineStyles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lineStyles.Location = new System.Drawing.Point(16, 56);
            this.lineStyles.Name = "lineStyles";
            this.lineStyles.Size = new System.Drawing.Size(256, 95);
            this.lineStyles.TabIndex = 0;
            this.lineStyles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lineStyles_DrawItem);
            this.lineStyles.SelectedIndexChanged += new System.EventHandler(this.lineStyles_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.compositeLines);
            this.panel1.Controls.Add(this.applyLineStyle);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 48);
            this.panel1.TabIndex = 3;
            // 
            // compositePanel
            // 
            this.compositePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.compositePanel.Controls.Add(this.CompositeGroup);
            this.compositePanel.Location = new System.Drawing.Point(0, 56);
            this.compositePanel.Name = "compositePanel";
            this.compositePanel.Size = new System.Drawing.Size(290, 160);
            this.compositePanel.TabIndex = 4;
            // 
            // propertyPanel
            // 
            this.propertyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyPanel.Controls.Add(this.previewGroup);
            this.propertyPanel.Controls.Add(this.sizeGroup);
            this.propertyPanel.Controls.Add(this.lineGroup);
            this.propertyPanel.Location = new System.Drawing.Point(0, 224);
            this.propertyPanel.Name = "propertyPanel";
            this.propertyPanel.Size = new System.Drawing.Size(290, 256);
            this.propertyPanel.TabIndex = 5;
            // 
            // previewGroup
            // 
            this.previewGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewGroup.Controls.Add(this.previewPicture);
            this.previewGroup.Location = new System.Drawing.Point(0, 208);
            this.previewGroup.Name = "previewGroup";
            this.previewGroup.Size = new System.Drawing.Size(288, 48);
            this.previewGroup.TabIndex = 10;
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
            this.previewPicture.Size = new System.Drawing.Size(272, 24);
            this.previewPicture.TabIndex = 0;
            this.previewPicture.TabStop = false;
            this.previewPicture.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPicture_Paint);
            // 
            // sizeGroup
            // 
            this.sizeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sizeGroup.Controls.Add(this.sizeUnitsCombo);
            this.sizeGroup.Controls.Add(this.sizeContextCombo);
            this.sizeGroup.Controls.Add(this.label3);
            this.sizeGroup.Controls.Add(this.label2);
            this.sizeGroup.Location = new System.Drawing.Point(0, 0);
            this.sizeGroup.Name = "sizeGroup";
            this.sizeGroup.Size = new System.Drawing.Size(288, 80);
            this.sizeGroup.TabIndex = 1;
            this.sizeGroup.TabStop = false;
            this.sizeGroup.Text = "Size";
            // 
            // sizeUnitsCombo
            // 
            this.sizeUnitsCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sizeUnitsCombo.DataSource = this.UnitsTable;
            this.sizeUnitsCombo.DisplayMember = "Display";
            this.sizeUnitsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeUnitsCombo.Location = new System.Drawing.Point(120, 44);
            this.sizeUnitsCombo.Name = "sizeUnitsCombo";
            this.sizeUnitsCombo.Size = new System.Drawing.Size(160, 21);
            this.sizeUnitsCombo.TabIndex = 11;
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
            this.sizeContextCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sizeContextCombo.DataSource = this.SizeContextTable;
            this.sizeContextCombo.DisplayMember = "Display";
            this.sizeContextCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeContextCombo.Location = new System.Drawing.Point(120, 12);
            this.sizeContextCombo.Name = "sizeContextCombo";
            this.sizeContextCombo.Size = new System.Drawing.Size(160, 21);
            this.sizeContextCombo.TabIndex = 10;
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
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Size units";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Size context";
            // 
            // lineGroup
            // 
            this.lineGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lineGroup.Controls.Add(this.lineStyleEditor);
            this.lineGroup.Location = new System.Drawing.Point(0, 88);
            this.lineGroup.Name = "lineGroup";
            this.lineGroup.Size = new System.Drawing.Size(288, 112);
            this.lineGroup.TabIndex = 0;
            this.lineGroup.TabStop = false;
            this.lineGroup.Text = "Line style";
            // 
            // lineStyleEditor
            // 
            this.lineStyleEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lineStyleEditor.Location = new System.Drawing.Point(8, 16);
            this.lineStyleEditor.Name = "lineStyleEditor";
            this.lineStyleEditor.Size = new System.Drawing.Size(272, 88);
            this.lineStyleEditor.TabIndex = 0;
            // 
            // ComboBoxDataSet
            // 
            this.ComboBoxDataSet.DataSetName = "ComboBoxDataSet";
            this.ComboBoxDataSet.Locale = new System.Globalization.CultureInfo("da-DK");
            this.ComboBoxDataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.SizeContextTable,
            this.UnitsTable});
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddStyleButton,
            this.RemoveStyleButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(282, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // AddStyleButton
            // 
            this.AddStyleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddStyleButton.Image = ((System.Drawing.Image)(resources.GetObject("AddStyleButton.Image")));
            this.AddStyleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddStyleButton.Name = "AddStyleButton";
            this.AddStyleButton.Size = new System.Drawing.Size(23, 22);
            this.AddStyleButton.Click += new System.EventHandler(this.AddStyleButton_Click);
            // 
            // RemoveStyleButton
            // 
            this.RemoveStyleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RemoveStyleButton.Enabled = false;
            this.RemoveStyleButton.Image = ((System.Drawing.Image)(resources.GetObject("RemoveStyleButton.Image")));
            this.RemoveStyleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveStyleButton.Name = "RemoveStyleButton";
            this.RemoveStyleButton.Size = new System.Drawing.Size(23, 22);
            this.RemoveStyleButton.Click += new System.EventHandler(this.RemoveStyleButton_Click);
            // 
            // LineFeatureStyleEditor
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(290, 480);
            this.Controls.Add(this.propertyPanel);
            this.Controls.Add(this.compositePanel);
            this.Controls.Add(this.panel1);
            this.Name = "LineFeatureStyleEditor";
            this.Size = new System.Drawing.Size(290, 480);
            this.CompositeGroup.ResumeLayout(false);
            this.CompositeGroup.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.compositePanel.ResumeLayout(false);
            this.propertyPanel.ResumeLayout(false);
            this.previewGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
            this.sizeGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).EndInit();
            this.lineGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion


		private void lineStyles_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UpdateDisplayForSelected();
		}

		private void sizeContextCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			//TODO: Where does this go?
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void sizeUnitsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating || this.CurrentStrokeType == null)
				return;
			this.CurrentStrokeType.Unit = (OSGeo.MapGuide.MaestroAPI.LengthUnitType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.LengthUnitType), (string)sizeUnitsCombo.SelectedValue);
			previewPicture.Refresh();
			lineStyles.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		public OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection Item
		{
			get { return m_item; }
			set
			{
				m_item = value;
				UpdateDisplay();
			}
		}

		private void thicknessCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (isUpdating || this.CurrentStrokeType == null)
				return;
			this.CurrentStrokeType.Thickness = lineStyleEditor.thicknessUpDown.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);

			previewPicture.Refresh();
			lineStyles.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void colorCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (isUpdating || this.CurrentStrokeType == null)
				return;
			this.CurrentStrokeType.Color = lineStyleEditor.colorCombo.CurrentColor;
			previewPicture.Refresh();
			lineStyles.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void fillCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (isUpdating || this.CurrentStrokeType == null)
				return;

            if (lineStyleEditor.fillCombo.SelectedItem as ImageStylePicker.NamedImage != null)
                this.CurrentStrokeType.LineStyle = (lineStyleEditor.fillCombo.SelectedItem as ImageStylePicker.NamedImage).Name;
			previewPicture.Refresh();
			lineStyles.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void applyLineStyle_CheckedChanged(object sender, System.EventArgs e)
		{
			compositePanel.Enabled = 
			compositeLines.Enabled = 
			sizeGroup.Enabled = 
			lineGroup.Enabled =
			previewGroup.Enabled =
				applyLineStyle.Checked;

            if (!isUpdating)
            {
                if (!applyLineStyle.Checked)
                {
                    applyLineStyle.Tag = m_item;
                    m_item = new OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection();
                }
                else
                {
                    m_item = applyLineStyle.Tag as OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection;

                    if (m_item == null)
                        m_item = new OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection();

                    if (m_item.Count == 0)
                        m_item.Add(new OSGeo.MapGuide.MaestroAPI.StrokeType());

                    UpdateDisplay();
                }
            }
		}

		private void compositeLines_CheckedChanged(object sender, System.EventArgs e)
		{
			if (compositePanel.Visible && !compositeLines.Checked)
			{
				this.Height -= compositePanel.Height;
				//this.MinScrollSize 
			}
			else if (!compositePanel.Visible && compositeLines.Checked)
				this.Height += compositePanel.Height;

			compositePanel.Visible = compositeLines.Checked;

			if (isUpdating)
				return;
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void previewPicture_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			FeaturePreviewRender.RenderPreviewLine(e.Graphics, new Rectangle(1, 1, previewPicture.Width - 2, previewPicture.Height - 2), m_item);		
		}

		private void lineStyles_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			e.DrawBackground();
			if (e.Index >= 0 && e.Index < lineStyles.Items.Count)
			{
				OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection col = new OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection();
				col.Add((OSGeo.MapGuide.MaestroAPI.StrokeType) lineStyles.Items[e.Index]);
				FeaturePreviewRender.RenderPreviewLine(e.Graphics, new Rectangle(1, 1, e.Bounds.Width - 2, e.Bounds.Height - 2), col);		
			}
			if ((e.State & DrawItemState.Focus) != 0)
				e.DrawFocusRectangle();
		}

        private void RemoveStyleButton_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < m_item.Count; i++)
                if (m_item[i] == this.CurrentStrokeType)
                {
                    m_item.RemoveAt(i);
                    UpdateDisplay();
                    break;
                }

        }

        private void AddStyleButton_Click(object sender, EventArgs e)
        {
            m_item.Add(new OSGeo.MapGuide.MaestroAPI.StrokeType());
            UpdateDisplay();
            lineStyles.SelectedIndex = lineStyles.Items.Count - 1;
        }

	}
}
