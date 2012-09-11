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
using OSGeo.MapGuide.MaestroAPI.Services;
using Maestro.Shared.UI;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    /// <summary>
    /// Summary description for AreaFeatureStyleEditor.
    /// </summary>
    [ToolboxItem(false)]
    internal partial class AreaFeatureStyleEditor : System.Windows.Forms.UserControl
    {
        private IAreaSymbolizationFill m_item;
        private FillStyleEditor fillStyleEditor;
        private LineStyleEditor lineStyleEditor;
        private bool m_inUpdate = false;

        private IFill previousFill = null;
        private IStroke previousStroke = null;

        public event EventHandler Changed;

        private IEditorService m_editor;
        private ClassDefinition m_schema;
        private string m_featureSource;
        private string m_providername;
        private ILayerElementFactory _factory;
        private IMappingService _mappingSvc;
        private LinkLabel lnkRefresh;
        private ILayerStylePreviewable _preview;

        internal AreaFeatureStyleEditor(IEditorService editor, ClassDefinition schema, string featureSource, ILayerStylePreviewable prev)
            : this()
        {
            m_editor = editor;
            m_schema = schema;

            _factory = (ILayerElementFactory)editor.GetEditedResource();
            var fs = (IFeatureSource)m_editor.ResourceService.GetResource(featureSource);

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

        private AreaFeatureStyleEditor()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            using (System.IO.StringReader sr = new System.IO.StringReader(Strings.GeometryStyleComboDataset))
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
                    fillStyleEditor.foregroundColor.ColorExpression = m_item.Fill.ForegroundColor;
                    fillStyleEditor.backgroundColor.ColorExpression = m_item.Fill.BackgroundColor;

                    fillStyleEditor.fillCombo.SelectedValue = m_item.Fill.FillPattern;
                    if (fillStyleEditor.fillCombo.SelectedItem == null && fillStyleEditor.fillCombo.Items.Count > 0)
                        fillStyleEditor.fillCombo.SelectedIndex = fillStyleEditor.fillCombo.FindString(m_item.Fill.FillPattern);
                }
                
                lineStyleEditor.displayLine.Checked = m_item.Stroke != null;
                if (m_item.Stroke != null)
                {
                    sizeUnitsCombo.SelectedValue = m_item.Stroke.Unit.ToString();
                    var s2 = m_item.Stroke as IStroke2;
                    if (s2 != null)
                    {
                        sizeContextCombo.Enabled = true;
                        sizeContextCombo.SelectedValue = s2.SizeContext.ToString();
                    }
                    else
                    {
                        sizeContextCombo.Enabled = false;
                    }
                    if (!string.IsNullOrEmpty(m_item.Stroke.Color))
                        lineStyleEditor.colorCombo.ColorExpression = m_item.Stroke.Color;
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

        private void UpdatePreviewImage()
        {
            using (new WaitCursor(this))
            {
                m_editor.SyncSessionCopy();
                _previewImg = _mappingSvc.GetLegendImage(_preview.Scale, _preview.LayerDefinition, _preview.ThemeCategory, 3, previewPicture.Width, previewPicture.Height, _preview.ImageFormat);
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
                FeaturePreviewRender.RenderPreviewArea(e.Graphics, new Rectangle(1, 1, previewPicture.Width - 4, previewPicture.Height - 4), m_item);
            }
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

            m_item.Fill.ForegroundColor = fillStyleEditor.foregroundColor.ColorExpression;
            previewPicture.Refresh();
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void backgroundColor_CurrentColorChanged(object sender, EventArgs e)
        {
            if (m_inUpdate)
                return;

            m_item.Fill.BackgroundColor = fillStyleEditor.backgroundColor.ColorExpression;
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

            m_item.Stroke.Color = lineStyleEditor.colorCombo.ColorExpression;
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
                expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource, true);
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
            var s2 = m_item.Stroke as IStroke2;
            if (m_inUpdate || s2 == null)
                return;

            if (s2 != null)
                s2.SizeContext = (SizeContextType)Enum.Parse(typeof(SizeContextType), (string)sizeContextCombo.SelectedValue);
        }

        private void sizeUnitsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || m_item.Stroke == null)
                return;

            m_item.Stroke.Unit = (LengthUnitType)Enum.Parse(typeof(LengthUnitType), (string)sizeUnitsCombo.SelectedValue);
        }

        private void fillStyleEditor_BackgroundRequiresExpression(object sender, EventArgs e)
        {
            string expr = m_editor.EditExpression(fillStyleEditor.backgroundColor.ColorExpression, m_schema, m_providername, m_featureSource, true);
            if (expr != null)
                fillStyleEditor.backgroundColor.ColorExpression = expr;
        }

        private void fillStyleEditor_ForegroundRequiresExpression(object sender, EventArgs e)
        {
            string expr = m_editor.EditExpression(fillStyleEditor.foregroundColor.ColorExpression, m_schema, m_providername, m_featureSource, true);
            if (expr != null)
                fillStyleEditor.foregroundColor.ColorExpression = expr;
        }
        
        void LineStyleEditor_RequiresExpressionEditor(object sender, EventArgs e)
        {
            string expr = m_editor.EditExpression(lineStyleEditor.colorCombo.ColorExpression, m_schema, m_providername, m_featureSource, true);
            if (expr != null)
                lineStyleEditor.colorCombo.ColorExpression = expr;
        }

        private void lnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_editCommit != null)
                _editCommit.Invoke();
            UpdatePreviewImage();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_editCommit != null)
                _editCommit.Invoke();
            UpdatePreviewImage();
        }

        private Action _editCommit;

        internal void SetEditCommit(Action editCommit)
        {
            _editCommit = editCommit;
        }
    }
}
