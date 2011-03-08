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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
	/// <summary>
	/// Summary description for AreaFeatureStyleEditor.
	/// </summary>
	internal class AreaFeatureStyleEditor : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ComboBox sizeContextCombo;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
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

		private IAreaSymbolizationFill m_item;
		private FillStyleEditor fillStyleEditor;
		private LineStyleEditor lineStyleEditor;
		private System.Windows.Forms.ComboBox sizeUnitsCombo;
		private bool m_inUpdate = false;

		private IFill previousFill = null;
		private IStroke previousStroke = null;

		public event EventHandler Changed;

        private IEditorService m_editor;
        private ClassDefinition m_schema;
        private string m_featureSource;
        private string m_providername;
        private ILayerElementFactory _factory;

        public AreaFeatureStyleEditor(IEditorService editor, ClassDefinition schema, string featureSource)
            : this()
        {
            m_editor = editor;
            m_schema = schema;

            _factory = (ILayerElementFactory)editor.GetEditedResource();
            var fs = (IFeatureSource)m_editor.ResourceService.GetResource(featureSource);

            m_providername = fs.Provider;
            m_featureSource = featureSource;
        }

        private AreaFeatureStyleEditor()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            using (System.IO.StringReader sr = new System.IO.StringReader(Properties.Resources.GeometryStyleComboDataset))
                ComboBoxDataSet.ReadXml(sr);

            fillStyleEditor.displayFill.CheckedChanged += new EventHandler(displayFill_CheckedChanged);
            fillStyleEditor.fillCombo.SelectedIndexChanged += new EventHandler(fillCombo_SelectedIndexChanged);
            fillStyleEditor.foregroundColor.CurrentColorChanged += new EventHandler(foregroundColor_CurrentColorChanged);
            fillStyleEditor.backgroundColor.CurrentColorChanged += new EventHandler(backgroundColor_CurrentColorChanged);

            lineStyleEditor.displayLine.CheckedChanged += new EventHandler(displayLine_CheckedChanged);
            lineStyleEditor.thicknessCombo.SelectedIndexChanged += new EventHandler(thicknessCombo_SelectedIndexChanged);
            lineStyleEditor.thicknessCombo.TextChanged += new EventHandler(thicknessCombo_TextChanged);
            lineStyleEditor.colorCombo.CurrentColorChanged += new EventHandler(colorCombo_CurrentColorChanged);
            lineStyleEditor.fillCombo.SelectedIndexChanged += new EventHandler(fillCombo_Line_SelectedIndexChanged);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AreaFeatureStyleEditor));
            this.fillStyleEditor = new FillStyleEditor();
            this.lineStyleEditor = new LineStyleEditor();
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
            // 
            // lineStyleEditor
            // 
            resources.ApplyResources(this.lineStyleEditor, "lineStyleEditor");
            this.lineStyleEditor.Name = "lineStyleEditor";
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
                m_inUpdate = true;
                
                if (m_item == null)
				{
					fillStyleEditor.displayFill.Checked = false;
					lineStyleEditor.displayLine.Checked = false;
					return;
				}

				fillStyleEditor.displayFill.Checked = m_item.Fill != null;
				if (m_item.Fill != null)
				{
                    fillStyleEditor.foregroundColor.CurrentColor = Utility.ParseHTMLColor(m_item.Fill.ForegroundColor);
                    fillStyleEditor.backgroundColor.CurrentColor = Utility.ParseHTMLColor(m_item.Fill.BackgroundColor);

					fillStyleEditor.fillCombo.SelectedValue = m_item.Fill.FillPattern;
					if (fillStyleEditor.fillCombo.SelectedItem == null && fillStyleEditor.fillCombo.Items.Count > 0)
						fillStyleEditor.fillCombo.SelectedIndex = fillStyleEditor.fillCombo.FindString(m_item.Fill.FillPattern);
				}
				
				lineStyleEditor.displayLine.Checked = m_item.Stroke != null;
				if (m_item.Stroke != null)
				{
					sizeUnitsCombo.SelectedValue = m_item.Stroke.Unit.ToString();
                    //sizeContextCombo.SelectedValue = m_item.Stroke.SizeContext.ToString();
                    if (!string.IsNullOrEmpty(m_item.Stroke.Color))
                        lineStyleEditor.colorCombo.CurrentColor = Utility.ParseHTMLColor(m_item.Stroke.Color);
					lineStyleEditor.fillCombo.SelectedIndex = lineStyleEditor.fillCombo.FindString(m_item.Stroke.LineStyle);
                    lineStyleEditor.thicknessCombo.Text = m_item.Stroke.Thickness;
				}

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

		public IAreaSymbolizationFill Item 
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
				m_item.Fill = previousFill == null ? _factory.CreateDefaultFill() : previousFill;
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

		private void foregroundColor_CurrentColorChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

            m_item.Fill.ForegroundColor = Utility.SerializeHTMLColor(fillStyleEditor.foregroundColor.CurrentColor, true);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void backgroundColor_CurrentColorChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

            m_item.Fill.BackgroundColor = Utility.SerializeHTMLColor(fillStyleEditor.backgroundColor.CurrentColor, true);
			previewPicture.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void displayLine_CheckedChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

			if (lineStyleEditor.displayLine.Checked)
                m_item.Stroke = previousStroke == null ? _factory.CreateDefaultStroke() : previousStroke;
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

		private void colorCombo_CurrentColorChanged(object sender, EventArgs e)
		{
			if (m_inUpdate)
				return;

            m_item.Stroke.Color = Utility.SerializeHTMLColor(lineStyleEditor.colorCombo.CurrentColor, true);
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

        private void thicknessCombo_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || lineStyleEditor.thicknessCombo.SelectedIndex != -1)
                return;

            //TODO: Validate
            m_item.Stroke.Thickness = lineStyleEditor.thicknessCombo.Text;
            previewPicture.Refresh();
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void thicknessCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || lineStyleEditor.thicknessCombo.SelectedIndex != lineStyleEditor.thicknessCombo.Items.Count - 1)
                return;

            string current = null;
            current = m_item.Stroke.Thickness;

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

        internal void SetupForTheming()
        {
            fillStyleEditor.foregroundColor.Enabled =
            fillStyleEditor.lblForeground.Enabled =
            fillStyleEditor.displayFill.Enabled =
                false;
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

        private void sizeContextCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || m_item.Stroke == null)
                return;

            //m_item.Stroke.SizeContext = (SizeContextType)Enum.Parse(typeof(SizeContextType), (string)sizeContextCombo.SelectedValue);
        }

        private void sizeUnitsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || m_item.Stroke == null)
                return;

            m_item.Stroke.Unit = (LengthUnitType)Enum.Parse(typeof(LengthUnitType), (string)sizeUnitsCombo.SelectedValue);
        }
    }
}
