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
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

using ICSharpCode.TextEditor.Document;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels;

namespace Maestro.Editors.Generic
{
    /// <summary>
    /// 
    /// </summary>
    public delegate void XmlValidationCallback(out string[] errors, out string[] warnings);

    //TODO: Incorporate all the bells and whistles that ICSharpCode.TextEditor has to offer.
    //Right now this is an obvious shim-job

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
            //txtXmlContent.MaxLength = int.MaxValue;
            txtXmlContent.SetHighlighting("XML");
            txtXmlContent.ShowInvalidLines = true;
            txtXmlContent.ShowSpaces = true;
            txtXmlContent.ShowTabs = true;
            txtXmlContent.TextChanged += new EventHandler(OnTextContentChanged);
        }
        
        private string _origText;
        
        private void OnTextContentChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_origText) && !txtXmlContent.Text.Equals(_origText))
                OnResourceChanged();
                
            if (string.IsNullOrEmpty(_origText))
                _origText = txtXmlContent.Text;
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
            btnUndo.Enabled = txtXmlContent.EnableUndo;
            btnCut.Enabled = txtXmlContent.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableCut;
            btnCopy.Enabled = txtXmlContent.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableCopy;
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
            set 
            {
                _origText = null;            
                txtXmlContent.Text = value; FormatText();
            }
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

        /// <summary>
        /// Finds and replaces the specified search string with the specified replacement string
        /// </summary>
        /// <param name="szFind">The search string.</param>
        /// <param name="szReplace">The replacement string.</param>
        public void FindAndReplace(string szFind, string szReplace)
        {
            var textEditor = txtXmlContent;
            
            var selections = textEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectionCollection;
            // find start 
            int iStartSearching = -1;
            if (selections.Count > 0)
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
                /*
                //textEditor.Select(iFound, szHighlight.Length);
                var doc = GetDocument();
                
                textEditor.ActiveTextAreaControl.SelectionManager.SelectedText 
                UpdateTextPosition();
                //textEditor.ScrollToCaret();
                */

                //if (!String.IsNullOrEmpty(szReplace) && _ready)
                //    OnResourceChanged();
            }
        }
        
        private IDocument GetDocument()
        {
            return txtXmlContent.ActiveTextAreaControl.Document;
        }

        private void UpdateTextPosition()
        {
            var textEditor = txtXmlContent;
            int line = textEditor.ActiveTextAreaControl.Caret.Line;
            int col = textEditor.ActiveTextAreaControl.Caret.Column;
            lblCursorPos.Text = String.Format(Properties.Resources.XmlEditorCursorTemplate, line + 1, col + 1);
        }

        private void txtXmlContent_TextChanged(object sender, EventArgs e)
        {
            UpdateTextPosition();
            EvaluateCommands();

            if (_ready) 
                OnResourceChanged();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            PerformValidation(false, false);
        }

        /// <summary>
        /// Performs validation of the XML content
        /// </summary>
        /// <param name="silentSuccess">If true will not show a success dialog on successful validation</param>
        /// <param name="errorsOnly">if set to <c>true</c> displays only errors in validation, otherwise it shows both errors and warnings.</param>
        /// <returns>
        /// true if validation was successful, false otherwise
        /// </returns>
        public bool PerformValidation(bool silentSuccess, bool errorsOnly)
        {
            if (this.Validator != null)
            {
                string[] errors = new string[0];
                string[] warnings = new string[0];

                try
                {
                    this.Validator(out errors, out warnings);
                }
                catch (XmlException ex)
                {
                    var err = new List<string>(errors);
                    err.Add(ex.Message);
                    errors = err.ToArray();
                }

                if (errors.Length > 0 || warnings.Length > 0)
                {
                    if (errorsOnly)
                    {
                        if (errors.Length > 0)
                        {
                            new XmlValidationResult(errors, new string[0]).Show();
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        new XmlValidationResult(errors, warnings).Show();
                    }
                    return false;
                }
                else
                {
                    if (!silentSuccess)
                        MessageBox.Show(Properties.Resources.XmlDocIsValid);
                    return true;
                }
            }
            return true;
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
            this.XmlContent = ResourceTypeRegistry.SerializeAsString(res);
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
            txtXmlContent.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(this, EventArgs.Empty);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            txtXmlContent.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(this, EventArgs.Empty);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            txtXmlContent.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(this, EventArgs.Empty);
        }
    }
    
	/// <summary>
	/// Holds information about the start of a fold in an xml string.
	/// </summary>
	internal class XmlFoldStart
	{
		int line = 0;
		int col = 0;
		string prefix = String.Empty;
		string name = String.Empty;
		string foldText = String.Empty;
		
		public XmlFoldStart(string prefix, string name, int line, int col)
		{
			this.line = line;
			this.col = col;
			this.prefix = prefix;
			this.name = name;
		}
		
		/// <summary>
		/// The line where the fold should start.  Lines start from 0.
		/// </summary>
		public int Line {
			get {
				return line;
			}
		}
		
		/// <summary>
		/// The column where the fold should start.  Columns start from 0.
		/// </summary>
		public int Column {
			get {
				return col;
			}
		}	
		
		/// <summary>
		/// The name of the xml item with its prefix if it has one.
		/// </summary>
		public string Name {
			get {
				if (prefix.Length > 0) {
					return String.Concat(prefix, ":", name);
				} else {
					return name;
				}
			}
		}
		
		/// <summary>
		/// The text to be displayed when the item is folded.
		/// </summary>
		public string FoldText {
			get {
				return foldText;
			}
			
			set {
				foldText = value;
			}
		}
	}
}
