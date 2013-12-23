#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using System.Windows.Forms;
using System.ComponentModel;
using ICSharpCode.TextEditor.Document;
using Maestro.Shared.UI;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using Maestro.Editors.Generic.XmlEditor;
using System.Drawing;
using Maestro.Editors.Generic.XmlEditor.AutoCompletion;

namespace Maestro.Editors.Generic
{
    /// <summary>
    /// An extension of TextEditorControl for use by the generic XML editor control
    /// </summary>
    [ToolboxItem(false)]
    public class XmlTextEditorControl : TextEditorControl
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public XmlTextEditorControl()
        {
            XmlFormattingStrategy strategy = new XmlFormattingStrategy();
            Document.FormattingStrategy = (IFormattingStrategy)strategy;
            Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("XML");
            Document.FoldingManager.FoldingStrategy = new XmlFoldingStrategy();
        }

        /// <summary>
        /// Applies the given text editor properties
        /// </summary>
        /// <param name="props"></param>
        public void ApplySettings(ITextEditorProperties props)
        {
            this.TextEditorProperties = props;
        }

        internal void RegisterAction(Keys k, IEditAction action)
        {
            editactions[k] = action;
        }

        /// <summary>
        /// Forces the editor to update its folds.
        /// </summary>
        internal void UpdateFolding()
        {
            this.Document.FoldingManager.UpdateFoldings(String.Empty, null);
            RefreshMargin();
        }

        /// <summary>
        /// Repaints the folds in the margin.
        /// </summary>
        internal void RefreshMargin()
        {
            Action action = () => 
            {
                this.ActiveTextAreaControl.TextArea.Refresh(this.ActiveTextAreaControl.TextArea.FoldMargin);
            };
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else
                action();
        }

        #region XML auto-completion stuff

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newControl"></param>
        protected override void InitializeTextAreaControl(TextAreaControl newControl)
        {
            base.InitializeTextAreaControl(newControl);

            //primaryTextAreaControl = newControl;

            newControl.TextArea.KeyEventHandler += new ICSharpCode.TextEditor.KeyEventHandler(HandleKeyPress);
            /*
            newControl.ContextMenuStrip = contextMenuStrip;
            newControl.SelectionManager.SelectionChanged += new EventHandler(SelectionChanged);
            newControl.Document.DocumentChanged += new DocumentEventHandler(DocumentChanged);
            newControl.TextArea.ClipboardHandler.CopyText += new CopyTextEventHandler(ClipboardHandlerCopyText);

            newControl.MouseWheel += new MouseEventHandler(TextAreaMouseWheel);
            newControl.DoHandleMousewheel = false;
             */
        }

        CodeCompletionWindow codeCompletionWindow;
        XmlSchemaCompletionDataCollection schemaCompletionDataItems = new XmlSchemaCompletionDataCollection();
        XmlSchemaCompletionData defaultSchemaCompletionData = null;

        /// <summary>
        /// Gets the schemas that the xml editor will use.
        /// </summary>
        /// <remarks>
        /// Probably should NOT have a 'set' property, but allowing one
        /// allows us to share the completion data amongst multiple
        /// xml editor controls.
        /// </remarks>
        internal XmlSchemaCompletionDataCollection SchemaCompletionDataItems
        {
            get
            {
                return schemaCompletionDataItems;
            }
            set
            {
                schemaCompletionDataItems = value;
            }
        }

        /// <summary>
        /// Gets or sets the default schema completion data associated with this
        /// view.
        /// </summary>
        internal XmlSchemaCompletionData DefaultSchemaCompletionData
        {
            get
            {
                return defaultSchemaCompletionData;
            }
            set
            {
                defaultSchemaCompletionData = value;
            }
        }

        char GetCharacterBeforeCaret()
        {
            string text = Document.GetText(ActiveTextAreaControl.TextArea.Caret.Offset - 1, 1);
            if (text.Length > 0)
            {
                return text[0];
            }

            return '\0';
        }

        bool IsCaretAtDocumentStart
        {
            get
            {
                return ActiveTextAreaControl.TextArea.Caret.Offset == 0;
            }
        }

        /// <summary>
        /// Called when the user hits Ctrl+Space.
        /// </summary>
        public void ShowCompletionWindow()
        {
            if (!IsCaretAtDocumentStart)
            {
                // Find character before cursor.

                char ch = GetCharacterBeforeCaret();

                HandleKeyPress(ch);
            }
        }

        /// <summary>
        /// Captures the user's key presses.
        /// </summary>
        /// <remarks>
        /// <para>The code completion window ProcessKeyEvent is not perfect
        /// when typing xml.  If enter a space or ':' the text is
        /// autocompleted when it should not be.</para>
        /// <para>The code completion window has one predefined width,
        /// which cuts off any long namespaces that we show.</para>
        /// <para>The above issues have been resolved by duplicating
        /// the code completion window and fixing the problems in the
        /// duplicated class.</para>
        /// </remarks>
        protected bool HandleKeyPress(char ch)
        {
            if (IsCodeCompletionWindowOpen)
            {
                if (codeCompletionWindow.ProcessKeyEvent(ch))
                {
                    return false;
                }
            }

            try
            {
                switch (ch)
                {
                    case '<':
                    case ' ':
                    case '=':
                        ShowCompletionWindow(ch);
                        return false;
                    default:
                        if (XmlParser.IsAttributeValueChar(ch))
                        {
                            if (IsInsideQuotes(ActiveTextAreaControl.TextArea))
                            {
                                // Have to insert the character ourselves since
                                // it is not actually inserted yet.  If it is not
                                // inserted now the code completion will not work
                                // since the completion data provider attempts to
                                // include the key typed as the pre-selected text.
                                InsertCharacter(ch);
                                ShowCompletionWindow(ch);
                                return true;
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                ErrorDialog.Show(e);
            }

            return false;
        }

        bool IsCodeCompletionEnabled
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Checks whether the caret is inside a set of quotes (" or ').
        /// </summary>
        bool IsInsideQuotes(TextArea textArea)
        {
            bool inside = false;

            LineSegment line = textArea.Document.GetLineSegment(textArea.Document.GetLineNumberForOffset(textArea.Caret.Offset));
            if (line != null)
            {
                if ((line.Offset + line.Length > textArea.Caret.Offset) &&
                    (line.Offset < textArea.Caret.Offset))
                {

                    char charAfter = textArea.Document.GetCharAt(textArea.Caret.Offset);
                    char charBefore = textArea.Document.GetCharAt(textArea.Caret.Offset - 1);

                    if (((charBefore == '\'') && (charAfter == '\'')) ||
                        ((charBefore == '\"') && (charAfter == '\"')))
                    {
                        inside = true;
                    }
                }
            }

            return inside;
        }

        /// <summary>
        /// Inserts a character into the text editor at the current offset.
        /// </summary>
        /// <remarks>
        /// This code is copied from the TextArea.SimulateKeyPress method.  This
        /// code is needed to handle an issue with code completion.  What if
        /// we want to include the character just typed as the pre-selected text
        /// for autocompletion?  If we do not insert the character before
        /// displaying the autocompletion list we cannot set the pre-selected text
        /// because it is not actually inserted yet.  The autocompletion window
        /// checks the offset of the pre-selected text and closes the window
        /// if the range is wrong.  The offset check is always wrong since the text
        /// does not actually exist yet.  The check occurs in
        /// CodeCompletionWindow.CaretOffsetChanged:
        /// <code>[[!CDATA[	int offset = control.ActiveTextAreaControl.Caret.Offset;
        ///
        ///	if (offset &lt; startOffset || offset &gt; endOffset) {
        ///		Close();
        ///	} else {
        ///		codeCompletionListView.SelectItemWithStart(control.Document.GetText(startOffset, offset - startOffset));
        ///	}]]
        /// </code>
        /// The Close method is called because the offset is out of the range.
        /// </remarks>
        void InsertCharacter(char ch)
        {
            ActiveTextAreaControl.TextArea.BeginUpdate();
            Document.UndoStack.StartUndoGroup();

            switch (ActiveTextAreaControl.TextArea.Caret.CaretMode)
            {
                case CaretMode.InsertMode:
                    ActiveTextAreaControl.TextArea.InsertChar(ch);
                    break;
                case CaretMode.OverwriteMode:
                    ActiveTextAreaControl.TextArea.ReplaceChar(ch);
                    break;
            }
            int currentLineNr = ActiveTextAreaControl.TextArea.Caret.Line;
            Document.FormattingStrategy.FormatLine(ActiveTextAreaControl.TextArea, currentLineNr, Document.PositionToOffset(ActiveTextAreaControl.TextArea.Caret.Position), ch);

            ActiveTextAreaControl.TextArea.EndUpdate();
            Document.UndoStack.EndUndoGroup();
        }

        void CodeCompletionWindowClosed(object sender, EventArgs e)
        {
            codeCompletionWindow.Closed -= new EventHandler(CodeCompletionWindowClosed);
            codeCompletionWindow.Dispose();
            codeCompletionWindow = null;
        }

        bool IsCodeCompletionWindowOpen
        {
            get
            {
                return ((codeCompletionWindow != null) && (!codeCompletionWindow.IsDisposed));
            }
        }

        void ShowCompletionWindow(char ch)
        {
            if (IsCodeCompletionWindowOpen)
            {
                codeCompletionWindow.Close();
            }

            if (IsCodeCompletionEnabled)
            {
                XmlCompletionDataProvider completionDataProvider = new XmlCompletionDataProvider(schemaCompletionDataItems, defaultSchemaCompletionData, string.Empty /* defaultNamespacePrefix */);
                codeCompletionWindow = CodeCompletionWindow.ShowCompletionWindow(ParentForm, this, FileName, completionDataProvider, ch, true /* showDeclarationWindow */, false);

                if (codeCompletionWindow != null)
                {
                    codeCompletionWindow.Closed += new EventHandler(CodeCompletionWindowClosed);
                }
            }
        }

        #endregion
    }

    internal class TextEditorProperties : ITextEditorProperties
    {
        public bool CaretLine
        {
            get;
            set;
        }

        public bool AutoInsertCurlyBracket
        {
            get;
            set;
        }

        public bool HideMouseCursor
        {
            get;
            set;
        }

        public bool IsIconBarVisible
        {
            get;
            set;
        }

        public bool AllowCaretBeyondEOL
        {
            get;
            set;
        }

        public bool ShowMatchingBracket
        {
            get;
            set;
        }

        public bool CutCopyWholeLine
        {
            get;
            set;
        }

        public System.Drawing.Text.TextRenderingHint TextRenderingHint
        {
            get;
            set;
        }

        public bool MouseWheelScrollDown
        {
            get;
            set;
        }

        public bool MouseWheelTextZoom
        {
            get;
            set;
        }

        public string LineTerminator
        {
            get;
            set;
        }

        public LineViewerStyle LineViewerStyle
        {
            get;
            set;
        }

        public bool ShowInvalidLines
        {
            get;
            set;
        }

        public int VerticalRulerRow
        {
            get;
            set;
        }

        public bool ShowSpaces
        {
            get;
            set;
        }

        public bool ShowTabs
        {
            get;
            set;
        }

        public bool ShowEOLMarker
        {
            get;
            set;
        }

        public bool ConvertTabsToSpaces
        {
            get;
            set;
        }

        public bool ShowHorizontalRuler
        {
            get;
            set;
        }

        public bool ShowVerticalRuler
        {
            get;
            set;
        }

        public Encoding Encoding
        {
            get;
            set;
        }

        public bool EnableFolding
        {
            get;
            set;
        }

        public bool ShowLineNumbers
        {
            get;
            set;
        }

        public int TabIndent
        {
            get;
            set;
        }

        public int IndentationSize
        {
            get;
            set;
        }

        public IndentStyle IndentStyle
        {
            get;
            set;
        }

        public DocumentSelectionMode DocumentSelectionMode
        {
            get;
            set;
        }

        public System.Drawing.Font Font
        {
            get;
            set;
        }

        public FontContainer FontContainer
        {
            get;
            private set;
        }

        public BracketMatchingStyle BracketMatchingStyle
        {
            get;
            set;
        }

        public bool SupportReadOnlySegments
        {
            get;
            set;
        }

        public static ITextEditorProperties CreateDefault(Font font)
        {
            return new TextEditorProperties()
            {
                EnableFolding = true,
                ShowLineNumbers  = true,
                ShowHorizontalRuler = false,
                ShowVerticalRuler = false,
                ShowSpaces = true,
                ShowTabs = true,
                ShowInvalidLines = true,
                ShowMatchingBracket = true,
                IsIconBarVisible = true,
                IndentStyle = ICSharpCode.TextEditor.Document.IndentStyle.Smart,
                IndentationSize = 2,
                DocumentSelectionMode = ICSharpCode.TextEditor.Document.DocumentSelectionMode.Normal,
                LineViewerStyle = ICSharpCode.TextEditor.Document.LineViewerStyle.FullRow,
                ConvertTabsToSpaces = true,
                MouseWheelScrollDown = true,
                MouseWheelTextZoom = false,
                FontContainer = new FontContainer(font),
                Encoding = Encoding.UTF8
            };
        }
    }
}
