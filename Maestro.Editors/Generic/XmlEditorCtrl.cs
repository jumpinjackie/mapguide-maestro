#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Exceptions;

namespace Maestro.Editors.Generic
{
    /// <summary>
    /// 
    /// </summary>
    public delegate void XmlValidationCallback(out string[] errors, out string[] warnings);

    /// <summary>
    /// A generic XML content editor
    /// </summary>
    public partial class XmlEditorCtrl : EditorBase, INotifyResourceChanged
    {
        private bool _ready = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlEditorCtrl"/> class.
        /// </summary>
        public XmlEditorCtrl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the validator.
        /// </summary>
        /// <value>The validator.</value>
        public XmlValidationCallback Validator { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public Color TextColor
        {
            get { return txtXmlContent.ForeColor; }
            set { txtXmlContent.ForeColor = value; }
        }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public Color BackgroundColor
        {
            get { return txtXmlContent.BackColor; }
            set { txtXmlContent.BackColor = value; }
        }

        /// <summary>
        /// Gets or sets the text font.
        /// </summary>
        /// <value>The text font.</value>
        public Font TextFont
        {
            get { return txtXmlContent.Font; }
            set { txtXmlContent.Font = value; }
        }

        /// <summary>
        /// Readies for editing.
        /// </summary>
        public void ReadyForEditing()
        {
            _ready = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            EvaluateCommands();
        }

        private void EvaluateCommands()
        {
            btnUndo.Enabled = txtXmlContent.CanUndo;
            btnCut.Enabled = txtXmlContent.SelectionLength > 0;
            btnCopy.Enabled = txtXmlContent.SelectionLength > 0;
            btnPaste.Enabled = Clipboard.ContainsText();
            btnValidate.Enabled = (this.Validator != null);
        }

        /// <summary>
        /// Gets or sets the content of the XML.
        /// </summary>
        /// <value>The content of the XML.</value>
        public string XmlContent
        {
            get { return txtXmlContent.Text; }
            set { txtXmlContent.Text = value; FormatText(); }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            txtXmlContent.Undo();
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            String szFind = txtFind.Text;
            if (String.IsNullOrEmpty(szFind))
            {
                MessageBox.Show(this, Properties.Resources.FindEmptyString, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtFind.Focus();
                return;
            }
            FindAndReplace(szFind, null);
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            if (txtFind.Text.Length == 0)
                MessageBox.Show(Properties.Resources.FindReplaceNothing);

            FindAndReplace(txtFind.Text, txtReplace.Text);
        }

        private void FindAndReplace(string szFind, string szReplace)
        {
            var textEditor = txtXmlContent;

            // find start 
            int iStartSearching = textEditor.SelectionStart;
            if (textEditor.SelectionLength > 0)
                iStartSearching++;

            System.Text.RegularExpressions.Regex regexThis = new System.Text.RegularExpressions.Regex(szFind, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Match matchThis = regexThis.Match(textEditor.Text, iStartSearching);

            // look by regex, then simple find
            String szFindInstance = "";
            if (matchThis.Success)
            {
                int iRegExStart = matchThis.Index;
                int iRegExLength = matchThis.Length;

                // TODO: this is a rubbish hack for single occurrance - there is probably a better RegEx way to find/replace
                szFindInstance = matchThis.ToString();
            }
            else
            {
                if (textEditor.Text.IndexOf(szFind, iStartSearching, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    MessageBox.Show(this, Properties.Resources.FindNothing, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                szFindInstance = szFind;
            }

            String szHighlight;
            if (String.IsNullOrEmpty(szReplace))
                szHighlight = szFindInstance;
            else
            {
                textEditor.Text = textEditor.Text.Replace(szFindInstance, szReplace);
                szHighlight = szReplace;
            }

            int iFound = textEditor.Text.IndexOf(szHighlight, iStartSearching);
            if (iFound > -1)
            {
                textEditor.Focus();
                textEditor.Select(iFound, szHighlight.Length);
                UpdateTextPosition();
                textEditor.ScrollToCaret();

                if (!String.IsNullOrEmpty(szReplace) && _ready)
                    OnResourceChanged();
            }
        }

        private void UpdateTextPosition()
        {
            var textEditor = txtXmlContent;
            int line = textEditor.GetLineFromCharIndex(textEditor.SelectionStart + textEditor.SelectionLength);
            int col = (textEditor.SelectionStart + textEditor.SelectionLength) - textEditor.GetFirstCharIndexFromLine(line);

            lblCursorPos.Text = String.Format(Properties.Resources.XmlEditorCursorTemplate, line + 1, col + 1);
        }

        private void txtXmlContent_KeyUp(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyData == (Keys.Control | Keys.A))
            {
                txtXmlContent.SelectAll();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (e.KeyData == (Keys.Control | Keys.C))
            {
                txtXmlContent.Copy();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (e.KeyData == (Keys.Control | Keys.V))
            {
                txtXmlContent.Paste();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (e.KeyData == (Keys.Control | Keys.X))
            {
                txtXmlContent.Cut();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            */
            UpdateTextPosition();
            EvaluateCommands();
        }

        private void txtXmlContent_TextChanged(object sender, EventArgs e)
        {
            UpdateTextPosition();
            EvaluateCommands();

            if (_ready) 
                OnResourceChanged();
        }

        private void txtXmlContent_MouseClick(object sender, MouseEventArgs e)
        {
            UpdateTextPosition();
            EvaluateCommands();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            if (this.Validator != null)
            {
                string[] errors;
                string[] warnings;

                this.Validator(out errors, out warnings);

                if (errors.Length > 0 || warnings.Length > 0)
                    new XmlValidationResult(errors, warnings).Show();
                else
                    MessageBox.Show(Properties.Resources.XmlDocIsValid);
            }
        }

        private void btnFormat_Click(object sender, EventArgs e)
        {
            FormatText();
        }

        private void FormatText()
        {
            string content = null;
            if (string.IsNullOrEmpty(txtXmlContent.Text.Trim()))
                return;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(txtXmlContent.Text);

                using (var ms = new MemoryStream())
                {
                    using (var sw = new StreamWriter(ms))
                    {
                        var writer = XmlWriter.Create(sw, new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true });
                        doc.Save(writer);

                        content = Encoding.UTF8.GetString(ms.GetBuffer());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(NestedExceptionMessageProcessor.GetFullMessage(ex), Properties.Resources.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!string.IsNullOrEmpty(content))
            {
                txtXmlContent.Text = content;
            }
        }

        /// <summary>
        /// Binds the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        public override void Bind(IEditorService service)
        {
            var res = service.GetEditedResource();
            this.XmlContent = res.Serialize();
            InitResourceData(service);
        }

        /// <summary>
        /// Inits the resource data.
        /// </summary>
        /// <param name="service">The service.</param>
        public void InitResourceData(IEditorService service)
        {
            resDataCtrl.Init(service);
        }

        private void resDataCtrl_DataListChanged(object sender, EventArgs e)
        {
            if (_ready)
                OnResourceChanged();
        }

        private void btnCut_Click(object sender, EventArgs e)
        {
            txtXmlContent.Cut();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            txtXmlContent.Copy();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            txtXmlContent.Paste();
        }
    }
}
