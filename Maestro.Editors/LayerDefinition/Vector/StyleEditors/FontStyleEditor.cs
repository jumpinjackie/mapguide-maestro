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
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    /// <summary>
    /// Summary description for FontStyleEditor.
    /// </summary>
    [ToolboxItem(false)]
    internal partial class FontStyleEditor : System.Windows.Forms.UserControl
    {
        private ITextSymbol m_item;
        
        private bool m_inUpdate = false;

        public event EventHandler Changed;

        private IEditorService m_editor;
        private ClassDefinition m_schema;
        private string m_featureSource;
        private string m_providername;

        private ILayerElementFactory _factory;

        public FontStyleEditor(IEditorService editor, ClassDefinition schema, string featureSource)
            : this()
        {
            m_editor = editor;
            m_schema = schema;

            var fs = (IFeatureSource)editor.ResourceService.GetResource(featureSource);

            _factory = (ILayerElementFactory)editor.GetEditedResource();

            m_providername = fs.Provider;
            m_featureSource = featureSource;

            propertyCombo.Items.Clear();
            foreach (var col in m_schema.Properties)
            {
                if (col.Type == PropertyDefinitionType.Data)
                    propertyCombo.Items.Add(col.Name);
            }
            propertyCombo.Items.Add(Strings.ExpressionItem);

            fontCombo.Items.Clear();
            foreach (FontFamily f in new System.Drawing.Text.InstalledFontCollection().Families)
                fontCombo.Items.Add(f.Name);

        }

        private FontStyleEditor()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            using(System.IO.StringReader sr = new System.IO.StringReader(Strings.GeometryStyleComboDataset))
                ComboBoxDataSet.ReadXml(sr);
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
                    DisplayLabel.Checked = false;
                    return;
                }
                else
                    DisplayLabel.Checked = true;

                propertyCombo.Text = m_item.Text;
                propertyCombo.SelectedItem = m_item.Text;
                if (m_item.FontName != null)
                    fontCombo.Text = m_item.FontName;
                sizeContextCombo.SelectedValue = m_item.SizeContext.ToString();
                unitsCombo.SelectedValue = m_item.Unit.ToString();
                if (m_item.SizeX == null)
                    sizeCombo.Text = "";
                else
                    sizeCombo.Text = m_item.SizeX.ToString();

                boldCheck.Checked = m_item.Bold == "true";
                italicCheck.Checked = m_item.Italic == "true";
                underlineCheck.Checked = m_item.Underlined == "true";
                textColor.ColorExpression = m_item.ForegroundColor;
                backgroundColor.ColorExpression = m_item.BackgroundColor;
                backgroundTypeCombo.SelectedValue = m_item.BackgroundStyle.ToString();
                rotationCombo.SelectedIndex = -1;
                rotationCombo.Text = m_item.Rotation;
                if (m_item.HorizontalAlignment != null)
                {
                    horizontalCombo.SelectedValue = m_item.HorizontalAlignment;
                    if (horizontalCombo.SelectedValue == null)
                    {
                        horizontalCombo.SelectedIndex = -1;
                        horizontalCombo.Text = m_item.HorizontalAlignment;
                    }
                }

                if (m_item.VerticalAlignment != null)
                {
                    verticalCombo.SelectedValue = m_item.VerticalAlignment;
                    if (verticalCombo.SelectedValue == null)
                    {
                        verticalCombo.SelectedIndex = -1;
                        verticalCombo.Text = m_item.VerticalAlignment;
                    }
                }
            }
            finally
            {
                m_inUpdate = false;
            }
        }


        

        private void propertyCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;
            if (propertyCombo.SelectedIndex == propertyCombo.Items.Count - 1)
            {
                string current = m_item.Text;
                string expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource, true);
                if (!string.IsNullOrEmpty(expr))
                    current = expr;

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), propertyCombo, current, expr != null);
            }
        }

        private void fontCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;
            m_item.FontName = (string)fontCombo.Text;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void sizeContextCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;
            m_item.SizeContext = (SizeContextType)Enum.Parse(typeof(SizeContextType), (string)sizeContextCombo.SelectedValue);
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void unitsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;
            m_item.Unit = (LengthUnitType)Enum.Parse(typeof(LengthUnitType), (string)unitsCombo.SelectedValue);
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void sizeCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;
            if (sizeCombo.SelectedIndex == sizeCombo.Items.Count - 1)
            {
                string current = m_item.SizeX;
                string expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource, true);
                if (!string.IsNullOrEmpty(expr))
                    current = expr;

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), sizeCombo, current, expr != null);
            }
        }

        private void boldCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;
            m_item.Bold = boldCheck.Checked ? "true" : null;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void italicCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;
            m_item.Italic = italicCheck.Checked ? "true" : null;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void underlineCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;
            m_item.Underlined = underlineCheck.Checked ? "true" : null;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void textColor_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;
            m_item.ForegroundColor = textColor.ColorExpression;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void backgroundColor_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;
            m_item.BackgroundColor = backgroundColor.ColorExpression;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void backgroundTypeCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;
            m_item.BackgroundStyle = (BackgroundStyleType)Enum.Parse(typeof(BackgroundStyleType), (string)backgroundTypeCombo.SelectedValue);
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void horizontalCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;

            if (horizontalCombo.SelectedIndex == horizontalCombo.Items.Count - 1)
            {
                string current = m_item.HorizontalAlignment;
                string expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource, true);
                if (!string.IsNullOrEmpty(expr))
                    current = expr;

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), horizontalCombo, current, expr != null);
            }
            else if (horizontalCombo.SelectedIndex != -1)
            {
                m_item.HorizontalAlignment = (string)horizontalCombo.SelectedValue;
            }
        }

        private void verticalCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;

            if (verticalCombo.SelectedIndex == verticalCombo.Items.Count - 1)
            {
                string current = m_item.VerticalAlignment;
                string expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource, true);
                if (!string.IsNullOrEmpty(expr))
                    current = expr;

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), verticalCombo, current, expr != null);
            }
            else if (verticalCombo.SelectedIndex != -1)
            {
                m_item.VerticalAlignment = (string)verticalCombo.SelectedValue;
            }
        }

        private void rotationCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_inUpdate || m_item == null)
                return;

            if (rotationCombo.SelectedIndex == rotationCombo.Items.Count - 1)
            {
                string current = m_item.Rotation;
                string expr = m_editor.EditExpression(current, m_schema, m_providername, m_featureSource, true);
                if (!string.IsNullOrEmpty(expr))
                    current = expr;

                //This is required as we cannot update the text from within the SelectedIndexChanged event :(
                BeginInvoke(new UpdateComboTextFromSelectChangedDelegate(UpdateComboTextFromSelectChanged), rotationCombo, current, expr != null);
            }
            else if (rotationCombo.SelectedIndex != -1)
            {
                m_item.Rotation = (string)rotationCombo.SelectedValue;
            }
        }

        static double? StringToDouble(string value)
        {
            double d;
            if (double.TryParse(value, out d))
                return d;
            return null;
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
            if (m_inUpdate || m_item == null || propertyCombo.SelectedIndex == propertyCombo.Items.Count - 1)
                return;

            m_item.Text = propertyCombo.Text;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        public ITextSymbol Item
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

            if (m_inUpdate)
                return;

            if (DisplayLabel.Checked)
            {
                if (DisplayLabel.Tag as ITextSymbol != null)
                    this.Item = DisplayLabel.Tag as ITextSymbol;
                if (m_item == null)
                    this.Item = _factory.CreateDefaultTextSymbol();
            }
            else
            {
                DisplayLabel.Tag = m_item;
                this.Item = null;
            }

        }

        private void sizeCombo_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || m_item == null || sizeCombo.SelectedIndex != -1)
                return;

            //TODO: Validate
            m_item.SizeX = m_item.SizeY = sizeCombo.Text;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void fontCombo_TextChanged(object sender, EventArgs e)
        {
            fontCombo_SelectedIndexChanged(sender, e);
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

        private void horizontalCombo_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || m_item == null || horizontalCombo.SelectedIndex != -1)
                return;

            m_item.HorizontalAlignment = (string)horizontalCombo.Text;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void verticalCombo_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || m_item == null || verticalCombo.SelectedIndex != -1)
                return;

            m_item.VerticalAlignment = (string)verticalCombo.Text;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void rotationCombo_TextChanged(object sender, EventArgs e)
        {
            if (m_inUpdate || rotationCombo.SelectedIndex != -1)
                return;

            //TODO: Validate
            m_item.Rotation = rotationCombo.Text;
            previewPicture.Refresh();

            if (Changed != null)
                Changed(this, new EventArgs());
        }

        
        void TextColor_RequestExpressionEditor(object sender, EventArgs e)
        {
            string expr = m_editor.EditExpression(textColor.ColorExpression, m_schema, m_providername, m_featureSource, true);
            if (expr != null)
                textColor.ColorExpression = expr;
        }
        
        void BackgroundColor_RequestExpressionEditor(object sender, EventArgs e)
        {
            string expr = m_editor.EditExpression(backgroundColor.ColorExpression, m_schema, m_providername, m_featureSource, true);
            if (expr != null)
                backgroundColor.ColorExpression = expr;
        }
    }
}
