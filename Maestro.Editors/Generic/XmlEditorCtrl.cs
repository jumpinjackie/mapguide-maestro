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
using ICSharpCode.TextEditor.Actions;
using Maestro.Editors.Generic.XmlEditor;

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
        class FindAction : AbstractEditAction
        {
            private XmlEditorCtrl _parent;

            public FindAction(XmlEditorCtrl parent)
            {
                _parent = parent;
            }

            public override void Execute(ICSharpCode.TextEditor.TextArea textArea)
            {
                _parent.DoFind();
            }
        }

        class FindAndReplaceAction : AbstractEditAction
        {
            private XmlEditorCtrl _parent;

            public FindAndReplaceAction(XmlEditorCtrl parent) { _parent = parent; }

            public override void Execute(ICSharpCode.TextEditor.TextArea textArea)
            {
                _parent.DoFindReplace();
            }
        }

        private bool _ready = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlEditorCtrl"/> class.
        /// </summary>
        public XmlEditorCtrl()
        {
            InitializeComponent();
            txtXmlContent.RegisterAction(Keys.Control | Keys.F, new FindAction(this));
            txtXmlContent.RegisterAction(Keys.Control | Keys.H, new FindAndReplaceAction(this));

            var props = TextEditorProperties.CreateDefault(txtXmlContent.Font);
            props.LineViewerStyle = LineViewerStyle.FullRow;
            props.ShowInvalidLines = true;

            txtXmlContent.ApplySettings(props);

            txtXmlContent.TextChanged += new EventHandler(OnTextContentChanged);
        }

        /// <summary>
        /// Initializes auto-completion data from the given XML Schema path
        /// </summary>
        /// <param name="xsdPath"></param>
        public void LoadAutoCompletionData(string xsdPath)
        {
            txtXmlContent.SchemaCompletionDataItems = Maestro.Editors.Generic.XmlEditor.AutoCompletion.XmlSchemaManager.Instance.SchemaCompletionDataItems;
            txtXmlContent.DefaultSchemaCompletionData = new XmlEditor.AutoCompletion.XmlSchemaCompletionData(xsdPath);
        }
        
        private string _origText;
        
        private void OnTextContentChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_origText) && !txtXmlContent.Text.Equals(_origText))
            {
                OnResourceChanged();
                EvaluateCommands();
            }
                
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
            cutToolStripMenuItem.Enabled = btnCut.Enabled = txtXmlContent.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableCut;
            copyToolStripMenuItem.Enabled = btnCopy.Enabled = txtXmlContent.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableCopy;
            pasteToolStripMenuItem.Enabled = btnPaste.Enabled = Clipboard.ContainsText();
            btnValidate.Enabled = (this.Validator != null);
        }

        /// <summary>
        /// Raised when XML content changes
        /// </summary>
        public event EventHandler TextChanged
        {
            add { txtXmlContent.TextChanged += value; }
            remove { txtXmlContent.TextChanged -= value; }
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
                txtXmlContent.Text = value;
                FormatText();
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            txtXmlContent.Undo();
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
            lblCursorPos.Text = String.Format(Strings.XmlEditorCursorTemplate, line + 1, col + 1);
        }

        private void txtXmlContent_TextChanged(object sender, EventArgs e)
        {
            UpdateTextPosition();
            EvaluateCommands();
            txtXmlContent.UpdateFolding();
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
                        MessageBox.Show(Strings.XmlDocIsValid);
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
            if (string.IsNullOrEmpty(txtXmlContent.Text))
                return;

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(txtXmlContent.Text);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Xml.XmlWriter xw = System.Xml.XmlTextWriter.Create(sb, new System.Xml.XmlWriterSettings() { Indent = true });
            doc.WriteTo(xw);
            xw.Flush();
            txtXmlContent.Text = sb.ToString();
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

        private XmlEditor.FindAndReplaceForm _findForm = new XmlEditor.FindAndReplaceForm();

        private void btnFind_Click(object sender, EventArgs e)
        {
            DoFind();
        }

        private void DoFind()
        {
            var editor = txtXmlContent;
            if (editor == null) return;
            _findForm.ShowFor(editor, false);
        }

        private void btnFindAndReplace_Click(object sender, EventArgs e)
        {
            DoFindReplace();
        }

        private void DoFindReplace()
        {
            var editor = txtXmlContent;
            if (editor == null) return;
            _findForm.ShowFor(editor, true);
        }

        /// <summary>
        /// Find and replace all instances of the specified token with its replacement token
        /// </summary>
        /// <param name="find"></param>
        /// <param name="replace"></param>
        public void FindAndReplace(string find, string replace)
        {
            _findForm.ShowFor(txtXmlContent, true, false); //This is just to initialize it just in case
            _findForm.FindAndReplace(find, replace);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtXmlContent.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(this, EventArgs.Empty);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtXmlContent.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(this, EventArgs.Empty);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtXmlContent.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(this, EventArgs.Empty);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoFind();
        }

        private void findReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoFindReplace();
        }

        /// <summary>
        /// Gets or sets whether this editor can support reloading XML content
        /// from a source resource. If true, subscribe to the 
        /// <see cref="E:Maestro.Editors.Generic.XmlEditorCtrl.RequestReloadFromSource"/> 
        /// event to handle this particular action
        /// </summary>
        public bool SupportsReReadFromSource
        {
            get { return btnReRead.Enabled; }
            set { btnReRead.Enabled = value; }
        }

        /// <summary>
        /// Raised if the user wishes to reload the XML content from source. To handle this
        /// action, read the XML content from the requested source resource and load it into
        /// this editor
        /// </summary>
        public event EventHandler RequestReloadFromSource;

        private void btnReRead_Click(object sender, EventArgs e)
        {
            var h = this.RequestReloadFromSource;
            if (h != null)
                h(this, EventArgs.Empty);
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
                    return String.Concat(prefix, ":", name); //NOXLATE
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
