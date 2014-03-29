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
using System.Collections.Generic;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Services;
using Maestro.Shared.UI;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    /// <summary>
    /// Summary description for LineFeatureStyleEditor.
    /// </summary>
    [ToolboxItem(false)]
    internal class LineFeatureStyleEditor : System.Windows.Forms.UserControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.GroupBox CompositeGroup;
        private System.Windows.Forms.Panel AdvancedPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox previewPicture;
        private System.Windows.Forms.CheckBox applyLineStyle;
        private System.Windows.Forms.CheckBox compositeLines;
        private System.Windows.Forms.ListBox lineStyles;
        private System.Windows.Forms.Panel propertyPanel;
        private System.Windows.Forms.ComboBox sizeUnitsCombo;
        private System.Windows.Forms.ComboBox sizeContextCombo;
        private LineStyleEditor lineStyleEditor;
        
        private IList<IStroke> m_item = null;
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
        private bool m_inUpdate = false;
        private ToolStrip toolStrip1;
        private ToolStripButton AddStyleButton;
        private ToolStripButton RemoveStyleButton;

        public event EventHandler Changed;

        private IEditorService m_editor;
        private ClassDefinition m_schema;
        private string m_featureSource;
        private string m_providername;
        private ILayerElementFactory _factory;
        private IMappingService _mappingSvc;
        private LinkLabel lnkRefresh;
        private ILayerStylePreviewable _preview;

        internal LineFeatureStyleEditor(IEditorService editor, ClassDefinition schema, string featureSource, ILayerElementFactory factory, ILayerStylePreviewable prev)
            : this()
        {
            m_editor = editor;
            m_schema = schema;

            _factory = (ILayerElementFactory)editor.GetEditedResource();

            var fs = (IFeatureSource)editor.ResourceService.GetResource(featureSource);

            m_providername = fs.Provider;
            m_featureSource = featureSource;

            _preview = prev;
            var conn = editor.GetEditedResource().CurrentConnection;
            if (Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Mapping) >= 0)
            {
                _mappingSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
            }
            lnkRefresh.Visible = this.UseLayerIconPreview;
        }

        public bool UseLayerIconPreview
        {
            get { return _mappingSvc != null && _preview != null; }
        }

        private LineFeatureStyleEditor()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            using (System.IO.StringReader sr = new System.IO.StringReader(Strings.GeometryStyleComboDataset))
                ComboBoxDataSet.ReadXml(sr);

            lineStyleEditor.displayLine.Visible = false;
            lineStyleEditor.thicknessCombo.SelectedIndexChanged += new EventHandler(thicknessCombo_SelectedIndexChanged);
            lineStyleEditor.thicknessCombo.TextChanged += new EventHandler(thicknessCombo_TextChanged);
            lineStyleEditor.colorCombo.CurrentColorChanged += new EventHandler(colorCombo_CurrentValueChanged);
            lineStyleEditor.fillCombo.SelectedIndexChanged += new EventHandler(fillCombo_SelectedIndexChanged);
        }

        public void UpdateDisplay()
        {
            try
            {
                m_inUpdate = true;
                applyLineStyle.Checked = (m_item != null && m_item.Count != 0);

                lineStyles.Items.Clear();
                if (applyLineStyle.Checked)
                    foreach (IStroke st in m_item)
                        lineStyles.Items.Add(st);

                compositeLines.Checked = lineStyles.Items.Count > 1;
                if (lineStyles.Items.Count > 0)
                    lineStyles.SelectedIndex = 0;

                if (!compositeLines.Checked)
                {
                    if (m_item.Count > 0)
                    {
                        var st2 = m_item[0] as IStroke2;
                        if (st2 != null)
                            sizeContextCombo.SelectedValue = st2.SizeContext;
                        else
                            sizeContextCombo.Enabled = false; //Must be a 1.0.0 schema line rule
                    }
                }

                UpdateDisplayForSelected();
            }
            finally
            {
                m_inUpdate = false;
            }

        }

        private void UpdateDisplayForSelected()
        {
            bool prevUpdate = m_inUpdate;
            try
            {
                m_inUpdate = true;
                IStroke st = this.CurrentStrokeType;
                sizeGroup.Enabled = 
                lineGroup.Enabled =
                previewGroup.Enabled =
                    st != null;

                RemoveStyleButton.Enabled = st != null && m_item.Count > 1;

                if (st != null)
                {
                    sizeUnitsCombo.SelectedValue = st.Unit.ToString();
                    
                    //sizeContextCombo.SelectedValue = st.SizeContext.ToString();

                    if (st.Color == null)
                        lineStyleEditor.colorCombo.ColorExpression = Utility.SerializeHTMLColor(Color.Black, true);
                    else
                        lineStyleEditor.colorCombo.ColorExpression = st.Color;

                    foreach(object i in lineStyleEditor.fillCombo.Items)
                        if (i as ImageStylePicker.NamedImage != null && (i as ImageStylePicker.NamedImage).Name == st.LineStyle)
                        {
                            lineStyleEditor.fillCombo.SelectedItem = i;
                            break;
                        }

                    lineStyleEditor.thicknessCombo.Text = st.Thickness;

                    sizeContextCombo.Enabled = true;
                    var st2 = st as IStroke2;
                    if (st2 != null)
                        sizeContextCombo.SelectedValue = st2.SizeContext;
                    else
                        sizeContextCombo.Enabled = false;
                }
                UpdatePreviewResult();
            } 
            finally
            {
                m_inUpdate = prevUpdate;
            }

        }

        private IStroke CurrentStrokeType
        {
            get 
            {
                if (lineStyles.Items.Count == 0)
                    return null;
                else if (lineStyles.Items.Count == 1 || lineStyles.SelectedIndex <= 0)
                    return (IStroke)lineStyles.Items[0];
                else
                    return (IStroke)lineStyles.SelectedItem;
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddStyleButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveStyleButton = new System.Windows.Forms.ToolStripButton();
            this.lineStyles = new System.Windows.Forms.ListBox();
            this.AdvancedPanel = new System.Windows.Forms.Panel();
            this.compositePanel = new System.Windows.Forms.Panel();
            this.propertyPanel = new System.Windows.Forms.Panel();
            this.lineGroup = new System.Windows.Forms.GroupBox();
            this.lineStyleEditor = new Maestro.Editors.LayerDefinition.Vector.StyleEditors.LineStyleEditor();
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
            this.previewGroup = new System.Windows.Forms.GroupBox();
            this.lnkRefresh = new System.Windows.Forms.LinkLabel();
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.ComboBoxDataSet = new System.Data.DataSet();
            this.CompositeGroup.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.AdvancedPanel.SuspendLayout();
            this.compositePanel.SuspendLayout();
            this.propertyPanel.SuspendLayout();
            this.lineGroup.SuspendLayout();
            this.sizeGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).BeginInit();
            this.previewGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // applyLineStyle
            // 
            this.applyLineStyle.Checked = true;
            this.applyLineStyle.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.applyLineStyle, "applyLineStyle");
            this.applyLineStyle.Name = "applyLineStyle";
            this.applyLineStyle.CheckedChanged += new System.EventHandler(this.applyLineStyle_CheckedChanged);
            // 
            // compositeLines
            // 
            this.compositeLines.Checked = true;
            this.compositeLines.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.compositeLines, "compositeLines");
            this.compositeLines.Name = "compositeLines";
            this.compositeLines.CheckedChanged += new System.EventHandler(this.compositeLines_CheckedChanged);
            // 
            // CompositeGroup
            // 
            this.CompositeGroup.Controls.Add(this.toolStrip1);
            this.CompositeGroup.Controls.Add(this.lineStyles);
            resources.ApplyResources(this.CompositeGroup, "CompositeGroup");
            this.CompositeGroup.Name = "CompositeGroup";
            this.CompositeGroup.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddStyleButton,
            this.RemoveStyleButton});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // AddStyleButton
            // 
            this.AddStyleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.AddStyleButton, "AddStyleButton");
            this.AddStyleButton.Name = "AddStyleButton";
            this.AddStyleButton.Click += new System.EventHandler(this.AddStyleButton_Click);
            // 
            // RemoveStyleButton
            // 
            this.RemoveStyleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.RemoveStyleButton, "RemoveStyleButton");
            this.RemoveStyleButton.Name = "RemoveStyleButton";
            this.RemoveStyleButton.Click += new System.EventHandler(this.RemoveStyleButton_Click);
            // 
            // lineStyles
            // 
            resources.ApplyResources(this.lineStyles, "lineStyles");
            this.lineStyles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lineStyles.Name = "lineStyles";
            this.lineStyles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lineStyles_DrawItem);
            this.lineStyles.SelectedIndexChanged += new System.EventHandler(this.lineStyles_SelectedIndexChanged);
            // 
            // AdvancedPanel
            // 
            this.AdvancedPanel.Controls.Add(this.compositeLines);
            this.AdvancedPanel.Controls.Add(this.applyLineStyle);
            resources.ApplyResources(this.AdvancedPanel, "AdvancedPanel");
            this.AdvancedPanel.Name = "AdvancedPanel";
            // 
            // compositePanel
            // 
            this.compositePanel.Controls.Add(this.CompositeGroup);
            resources.ApplyResources(this.compositePanel, "compositePanel");
            this.compositePanel.Name = "compositePanel";
            // 
            // propertyPanel
            // 
            this.propertyPanel.Controls.Add(this.lineGroup);
            this.propertyPanel.Controls.Add(this.sizeGroup);
            resources.ApplyResources(this.propertyPanel, "propertyPanel");
            this.propertyPanel.Name = "propertyPanel";
            // 
            // lineGroup
            // 
            this.lineGroup.Controls.Add(this.lineStyleEditor);
            resources.ApplyResources(this.lineGroup, "lineGroup");
            this.lineGroup.Name = "lineGroup";
            this.lineGroup.TabStop = false;
            // 
            // lineStyleEditor
            // 
            this.lineStyleEditor.ColorExpression = "";
            resources.ApplyResources(this.lineStyleEditor, "lineStyleEditor");
            this.lineStyleEditor.Name = "lineStyleEditor";
            this.lineStyleEditor.RequiresExpressionEditor += new System.EventHandler(this.lineStyleEditor_RequiresExpressionEditor);
            // 
            // sizeGroup
            // 
            this.sizeGroup.Controls.Add(this.sizeUnitsCombo);
            this.sizeGroup.Controls.Add(this.sizeContextCombo);
            this.sizeGroup.Controls.Add(this.label3);
            this.sizeGroup.Controls.Add(this.label2);
            resources.ApplyResources(this.sizeGroup, "sizeGroup");
            this.sizeGroup.Name = "sizeGroup";
            this.sizeGroup.TabStop = false;
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
            // previewGroup
            // 
            this.previewGroup.Controls.Add(this.lnkRefresh);
            this.previewGroup.Controls.Add(this.previewPicture);
            resources.ApplyResources(this.previewGroup, "previewGroup");
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
            // LineFeatureStyleEditor
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.previewGroup);
            this.Controls.Add(this.propertyPanel);
            this.Controls.Add(this.compositePanel);
            this.Controls.Add(this.AdvancedPanel);
            this.Name = "LineFeatureStyleEditor";
            this.CompositeGroup.ResumeLayout(false);
            this.CompositeGroup.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.AdvancedPanel.ResumeLayout(false);
            this.compositePanel.ResumeLayout(false);
            this.propertyPanel.ResumeLayout(false);
            this.lineGroup.ResumeLayout(false);
            this.sizeGroup.ResumeLayout(false);
            this.sizeGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).EndInit();
            this.previewGroup.ResumeLayout(false);
            this.previewGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion


        private void lineStyles_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateDisplayForSelected();
        }

        private void sizeContextCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate)
                return;

            var st2 = this.CurrentStrokeType as IStroke2;
            if (st2 != null)
            {
                st2.SizeContext = (SizeContextType)Enum.Parse(typeof(SizeContextType), (string)sizeContextCombo.SelectedValue);
                UpdatePreviewResult();
                lineStyles.Refresh();
                if (Changed != null)
                    Changed(this, new EventArgs());
            }
        }

        private void sizeUnitsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || this.CurrentStrokeType == null)
                return;
            this.CurrentStrokeType.Unit = (LengthUnitType)Enum.Parse(typeof(LengthUnitType), (string)sizeUnitsCombo.SelectedValue);
            UpdatePreviewResult();
            lineStyles.Refresh();
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        public IList<IStroke> Item
        {
            get { return m_item; }
            set
            {
                m_item = value;
                UpdateDisplay();
            }
        }

        private void thicknessCombo_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || lineStyleEditor.thicknessCombo.SelectedIndex != -1)
                return;

            //TODO: Validate
            this.CurrentStrokeType.Thickness = lineStyleEditor.thicknessCombo.Text;
            UpdatePreviewResult();
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void thicknessCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || lineStyleEditor.thicknessCombo.SelectedIndex != lineStyleEditor.thicknessCombo.Items.Count - 1)
                return;

            string current = this.CurrentStrokeType.Thickness;
            string expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource, true);
            if (!string.IsNullOrEmpty(expr))
                current = expr;

            //This is required as we cannot update the text from within the SelectedIndexChanged event :(
            BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), lineStyleEditor.thicknessCombo, current, expr != null);
        }

        private void colorCombo_CurrentValueChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || this.CurrentStrokeType == null)
                return;
            this.CurrentStrokeType.Color = lineStyleEditor.colorCombo.ColorExpression;
            UpdatePreviewResult();
            lineStyles.Refresh();
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void fillCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || this.CurrentStrokeType == null)
                return;

            if (lineStyleEditor.fillCombo.SelectedItem as ImageStylePicker.NamedImage != null)
                this.CurrentStrokeType.LineStyle = (lineStyleEditor.fillCombo.SelectedItem as ImageStylePicker.NamedImage).Name;
            UpdatePreviewResult();
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

            if (!m_inUpdate)
            {
                if (!applyLineStyle.Checked)
                {
                    applyLineStyle.Tag = m_item;
                    m_item = new BindingList<IStroke>();
                    UpdatePreviewResult();
                }
                else
                {
                    m_item = applyLineStyle.Tag as IList<IStroke>;

                    if (m_item == null)
                        m_item = new BindingList<IStroke>();

                    if (m_item.Count == 0)
                        m_item.Add(_factory.CreateDefaultStroke());

                    UpdateDisplay();
                }
            }
        }

        private void compositeLines_CheckedChanged(object sender, System.EventArgs e)
        {
            compositePanel.Visible = compositeLines.Checked;

            if (m_inUpdate)
                return;
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void UpdatePreviewImage()
        {
            using (new WaitCursor(this))
            {
                m_editor.SyncSessionCopy();
                _previewImg = _mappingSvc.GetLegendImage(_preview.Scale, _preview.LayerDefinition, _preview.ThemeCategory, 2, previewPicture.Width, previewPicture.Height, _preview.ImageFormat);
                previewPicture.Invalidate();
            }
        }

        private Image _previewImg = null;

        private void previewPicture_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (UseLayerIconPreview)
            {
                if (_previewImg != null)
                {
                    e.Graphics.DrawImage(_previewImg, new Point(0, 0));
                }
            }
            else
            {
                FeaturePreviewRender.RenderPreviewLine(e.Graphics, new Rectangle(1, 1, previewPicture.Width - 2, previewPicture.Height - 2), m_item);
            }
        }

        private void lineStyles_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            e.DrawBackground();
            if ((e.State & DrawItemState.Focus) != 0)
                e.DrawFocusRectangle();
            
            if (e.Index >= 0 && e.Index < lineStyles.Items.Count)
            {
                var col = new BindingList<IStroke>();
                col.Add((IStroke) lineStyles.Items[e.Index]);
                FeaturePreviewRender.RenderPreviewLine(e.Graphics, new Rectangle(e.Bounds.Left + 1, e.Bounds.Top + 1, e.Bounds.Width - 2, e.Bounds.Height - 2), col);		
            }
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
            m_item.Add(_factory.CreateDefaultStroke());
            UpdateDisplay();
            lineStyles.SelectedIndex = lineStyles.Items.Count - 1;
        }

        internal void SetupForTheming()
        {
            lineStyleEditor.colorCombo.Enabled =
            lineStyleEditor.lblColor.Enabled =
            AdvancedPanel.Enabled =
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
                UpdatePreviewResult();
            }
            finally
            {
                if (!userChange)
                    m_inUpdate = false;
            }
        }

        private void lineStyleEditor_RequiresExpressionEditor(object sender, EventArgs e)
        {
            string expr = m_editor.EditExpression(lineStyleEditor.ColorExpression, m_schema, m_providername, m_featureSource, true);
            if (expr != null)
            {
                lineStyleEditor.ColorExpression = expr;
                UpdatePreviewResult();
            }
        }

        private void lnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UpdatePreviewResult();
        }

        private void UpdatePreviewResult()
        {
            if (_editCommit != null)
                _editCommit.Invoke();
            UpdatePreviewImage();
        }

        private Action _editCommit;

        internal void SetEditCommit(Action editCommit)
        {
            _editCommit = editCommit;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdatePreviewResult();
        }
    }
}
