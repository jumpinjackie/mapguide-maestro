#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
// 
// Original code from SharpDevelop 3.2.1 licensed under the same terms (LGPL 2.1)
// Copyright 2002-2010 by
//
//  AlphaSierraPapa, Christoph Wille
//  Vordernberger Strasse 27/8
//  A-8700 Leoben
//  Austria
//
//  email: office@alphasierrapapa.com
//  court of jurisdiction: Landesgericht Leoben
//
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
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.Common
{
    internal abstract class TextEditorBase : ITextEditor
    {
        delegate string GetLineInvoker(int index);
        delegate void WriteInvoker(string text, Color color, Color fore);

        protected TextEditorControl textEditorControl;
        protected TextArea textArea;
        protected Color customLineColour = Color.LightGray;
        protected TextMarker readOnlyMarker;

        protected TextEditorBase(TextEditorControl textEditorControl)
        {
            this.textEditorControl = textEditorControl;
            this.textArea = textEditorControl.ActiveTextAreaControl.TextArea;
            textEditorControl.TextEditorProperties.SupportReadOnlySegments = true;
        }

        public IndentStyle IndentStyle
        {
            get { return textEditorControl.IndentStyle; }
            set { SetIndentStyle(value); }
        }

        public event ICSharpCode.TextEditor.KeyEventHandler KeyPress
        {
            add { textArea.KeyEventHandler += value; }
            remove { textArea.KeyEventHandler -= value; }
        }

        public event DialogKeyProcessor DialogKeyPress
        {
            add { textArea.DoProcessDialogKey += value; }
            remove { textArea.DoProcessDialogKey -= value; }
        }

        public Color CustomLineColour
        {
            get { return customLineColour; }
        }

        public void Write(string text)
        {
            Write(text, Color.Empty, default(Color));
        }

        public void Write(string text, Color backgroundColour)
        {
            Write(text, backgroundColour, default(Color));
        }

        public void Write(string text, Color backgroundColour, Color foregroundColor)
        {
            if (textEditorControl.InvokeRequired)
            {
                WriteInvoker invoker = new WriteInvoker(Write);
                textEditorControl.Invoke(invoker, new object[] { text, backgroundColour, foregroundColor });
            }
            else
            {
                int offset = textEditorControl.Document.PositionToOffset(new TextLocation(Column, Line));
                textEditorControl.ActiveTextAreaControl.TextArea.InsertString(text);

                if (!backgroundColour.IsEmpty)
                {
                    TextMarker marker = new TextMarker(offset, text.Length, TextMarkerType.SolidBlock, backgroundColour, foregroundColor);
                    textEditorControl.Document.MarkerStrategy.AddMarker(marker);
                    textEditorControl.Refresh();
                }
            }
        }

        public int Column
        {
            get { return textEditorControl.ActiveTextAreaControl.Caret.Column; }
            set { textEditorControl.ActiveTextAreaControl.Caret.Column = value; }
        }

        public int SelectionStart
        {
            get
            {
                ColumnRange range = GetSelectionRange();
                if (range != ColumnRange.NoColumn)
                {
                    return range.StartColumn;
                }
                return Column;
            }
        }

        public int SelectionLength
        {
            get
            {
                ColumnRange range = GetSelectionRange();
                return range.EndColumn - range.StartColumn;
            }
        }

        /// <summary>
        /// Gets the current cursor line.
        /// </summary>
        public int Line
        {
            get { return textArea.Caret.Line; }
        }

        /// <summary>
        /// Gets the total number of lines in the text editor.
        /// </summary>
        public int TotalLines
        {
            get { return textEditorControl.Document.TotalNumberOfLines; }
        }

        /// <summary>
        /// Gets the text for the specified line.
        /// </summary>
        public string GetLine(int index)
        {
            if (textEditorControl.InvokeRequired)
            {
                GetLineInvoker invoker = new GetLineInvoker(GetLine);
                return (string)textEditorControl.Invoke(invoker, new object[] { index });
            }
            else
            {
                LineSegment lineSegment = textEditorControl.Document.GetLineSegment(index);
                return textEditorControl.Document.GetText(lineSegment);
            }
        }

        /// <summary>
        /// Replaces the text at the specified index on the current line with the specified text.
        /// </summary>
        public void Replace(int index, int length, string text)
        {
            int currentLine = textEditorControl.ActiveTextAreaControl.Caret.Line;
            LineSegment lineSegment = textEditorControl.Document.GetLineSegment(currentLine);
            textEditorControl.Document.Replace(lineSegment.Offset + index, length, text);
        }

        /// <summary>
        /// Makes the current text read only. Text can still be entered at the end.
        /// </summary>
        public void MakeCurrentContentReadOnly()
        {
            IDocument doc = textEditorControl.Document;
            if (readOnlyMarker == null)
            {
                readOnlyMarker = new TextMarker(0, doc.TextLength, TextMarkerType.Invisible);
                readOnlyMarker.IsReadOnly = true;
                doc.MarkerStrategy.AddMarker(readOnlyMarker);
            }
            readOnlyMarker.Offset = 0;
            readOnlyMarker.Length = doc.TextLength;
            doc.UndoStack.ClearAll();
        }

        public virtual void ShowCompletionWindow(ICompletionDataProvider completionDataProvider)
        {
            ShowCompletionWindow(completionDataProvider, ' ');
        }

        public abstract void ShowCompletionWindow(ICompletionDataProvider completionDataProvider, char firstChar);

        public abstract bool IsCompletionWindowDisplayed
        {
            get;
        }

        /// <summary>
        /// Gets the range of the currently selected text.
        /// </summary>
        ColumnRange GetSelectionRange()
        {
            return textArea.SelectionManager.GetSelectionAtLine(textArea.Caret.Line);
        }

        void SetIndentStyle(IndentStyle style)
        {
            if (textEditorControl.InvokeRequired)
            {
                Action<IndentStyle> action = SetIndentStyle;
                textEditorControl.Invoke(action, new object[] { style });
            }
            else
            {
                textEditorControl.IndentStyle = style;
            }
        }

        public virtual bool ProcessKeyPress(Keys keyData)
        {
            return false;
        }

        public virtual void SetParent(Control frm) { } 
    }

    /// <summary>
    /// Resolves the appropriate ITextEditor instance for the given text editor
    /// </summary>
    public static class TextEditorFactory
    {
        /// <summary>
        /// Creates the appropriate ITextEditor instance for the given text editor
        /// </summary>
        /// <param name="textEditor">The text editor instance</param>
        /// <returns></returns>
        public static ITextEditor CreateEditor(TextEditorControl textEditor)
        {
            if (Platform.IsRunningOnMono)
                return new MonoCompatibleTextEditor(textEditor);
            else
                return new DefaultTextEditor(textEditor);
        }
    }

    /// <summary>
    /// A text editor controller using Mono-friendly auto-completion
    /// </summary>
    internal class MonoCompatibleTextEditor : TextEditorBase
    {
        private AutoCompletionListBox _autoBox;
        private ToolTip _autoCompleteTooltip;

        internal MonoCompatibleTextEditor(TextEditorControl textEditor)
            : base(textEditor)
        {
            _autoBox = new AutoCompletionListBox();
            _autoCompleteTooltip = new ToolTip();
        }

        private Control _parent;

        public override void SetParent(Control ctrl)
        {
            _parent = ctrl;
            _parent.Controls.Add(_autoBox);
        }

        public override bool IsCompletionWindowDisplayed
        {
            get { return _autoBox.IsShown; }
        }

        public override void ShowCompletionWindow(ICompletionDataProvider completionDataProvider, char firstChar)
        {
            //Not ready for beta 5. Remove after release of beta 5.
            return;
            /*
            var context = new AutoCompletionListBox.AutoCompleteContext()
            {
                AutoCompleteTooltip = _autoCompleteTooltip,
                CompletionProvider = completionDataProvider,
                Editor = textEditorControl,
                FirstChar = firstChar,
                GetCaretPoint = GetCaretPoint,
                InsertionOffset = textEditorControl.ActiveTextAreaControl.Caret.Offset
            };
            _autoBox.SetCompletionItems(textEditorControl.ParentForm, context, string.Empty);
             */
        }

        private Point GetCaretPoint()
        {
            var pt = textArea.Caret.ScreenPosition;
            var cpt = textEditorControl.PointToScreen(pt);
            int dx = 15; //Shift a bit 
            int dy = 0;

            //Adjust the postion to accomodate as much space for the auto-complete box as much as possible
            if (_parent != null)
            {
                if (_autoBox.Height > _parent.Height)
                    dy = -pt.Y;
            }

            pt.Offset(dx, dy);
            return pt;
        }

        static bool IsAlphanumeric(Keys key)
        {
            return (key >= Keys.D0 && key <= Keys.Z);
        }

        public override bool ProcessKeyPress(Keys keyData)
        {
            bool bProcessed = false;
            if (IsCompletionWindowDisplayed)
            {
                if (IsAlphanumeric(keyData))
                {
                    _autoBox.AdvanceInsertionOffset();
                    return false;
                }

                switch (keyData)
                {
                    case Keys.Up:
                        _autoBox.MoveAutoCompleteSelectionUp();
                        bProcessed = true;
                        break;
                    case Keys.Down:
                        _autoBox.MoveAutoCompleteSelectionDown();
                        bProcessed = true;
                        break;
                    case Keys.Enter:
                        _autoBox.HandleEnterKey();
                        bProcessed = true;
                        break;
                    case Keys.Escape:
                        _autoBox.HideBox();
                        break;
                }
            }
            return bProcessed;
        }
    }

    /// <summary>
    /// Default text editor, using the ICSharpCode.TextEditor auto-completion facilities
    /// </summary>
    internal class DefaultTextEditor : TextEditorBase
    {
        CodeCompletionWindow completionWindow;

        internal DefaultTextEditor(TextEditorControl textEditor)
            : base(textEditor)
        { }

        public override bool IsCompletionWindowDisplayed
        {
            get { return completionWindow != null; }
        }

        public override void ShowCompletionWindow(ICompletionDataProvider completionDataProvider, char ch)
        {
            completionWindow = CodeCompletionWindow.ShowCompletionWindow(textEditorControl.ParentForm, textEditorControl, String.Empty, completionDataProvider, ch);
            if (completionWindow != null)
            {
                completionWindow.Width = 250;
                completionWindow.Closed += CompletionWindowClosed;
            }
        }

        void CompletionWindowClosed(object source, EventArgs e)
        {
            if (completionWindow != null)
            {
                completionWindow.Closed -= CompletionWindowClosed;
                completionWindow.Dispose();
                completionWindow = null;
            }
        }
    }
}
