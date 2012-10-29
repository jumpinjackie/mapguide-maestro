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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Maestro.Editors.Common
{
    public class TextEditor : ITextEditor
    {
        delegate string GetLineInvoker(int index);
        delegate void WriteInvoker(string text, Color color);

        TextEditorControl textEditorControl;
        TextArea textArea;
        Color customLineColour = Color.LightGray;
        TextMarker readOnlyMarker;

        CodeCompletionWindow completionWindow;

        public TextEditor(TextEditorControl textEditorControl)
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

        public event KeyEventHandler KeyPress
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
            Write(text, Color.Empty);
        }

        public void Write(string text, Color backgroundColour)
        {
            if (textEditorControl.InvokeRequired)
            {
                WriteInvoker invoker = new WriteInvoker(Write);
                textEditorControl.Invoke(invoker, new object[] { text, backgroundColour });
            }
            else
            {
                int offset = textEditorControl.Document.PositionToOffset(new TextLocation(Column, Line));
                textEditorControl.ActiveTextAreaControl.TextArea.InsertString(text);

                if (!backgroundColour.IsEmpty)
                {
                    TextMarker marker = new TextMarker(offset, text.Length, TextMarkerType.SolidBlock, backgroundColour);
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

        public void ShowCompletionWindow(ICompletionDataProvider completionDataProvider)
        {
            completionWindow = CodeCompletionWindow.ShowCompletionWindow(textEditorControl.ParentForm, textEditorControl, String.Empty, completionDataProvider, ' ');
            if (completionWindow != null)
            {
                completionWindow.Width = 250;
                completionWindow.Closed += CompletionWindowClosed;
            }
        }

        public bool IsCompletionWindowDisplayed
        {
            get { return completionWindow != null; }
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
