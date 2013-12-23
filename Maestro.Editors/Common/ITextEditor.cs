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
    /// <summary>
    /// A text editor controller interface that supports auto-completion. Note that
    /// all the methods will be called on another thread not the main UI thread and will therefore need to
    /// be invoked.
    /// </summary>
    public interface ITextEditor
    {
        /// <summary>
        /// Fired when a key is pressed but before any text has been added to the text editor.
        /// </summary>
        /// <remarks>
        /// The KeyPress handler should return true if the text editor should not process the key and not
        /// insert any text.
        /// </remarks>
        event KeyEventHandler KeyPress;

        /// <summary>
        /// Fired when dialog key is pressed but before any text has been added to the text editor.
        /// </summary>
        /// <remarks>
        /// The DialogKeyPress handler should return true if the text editor should not process the
        /// dialog key.
        /// </remarks>
        event DialogKeyProcessor DialogKeyPress;

        /// <summary>
        /// Gets or sets the indentation style.
        /// </summary>
        IndentStyle IndentStyle { get; set; }

        /// <summary>
        /// Inserts text at the current cursor location.
        /// </summary>
        void Write(string text);

        /// <summary>
        /// Inserts text at the current cursor location with the specified colour.
        /// </summary>
        void Write(string text, Color backgroundColor);

        /// <summary>
        /// Inserts text at the current cursor location with the specified colour.
        /// </summary>
        void Write(string text, Color backgroundColor, Color foregroundColor);

        /// <summary>
        /// Replaces the text at the specified index on the current line with the specified text.
        /// </summary>
        void Replace(int index, int length, string text);

        /// <summary>
        /// Gets or sets the current column position of the cursor on the current line.  This is zero based.
        /// </summary>
        int Column { get; set; }

        /// <summary>
        /// Gets the length of the currently selected text.
        /// </summary>
        int SelectionLength { get; }

        /// <summary>
        /// Gets the start position of the currently selected text.
        /// </summary>
        int SelectionStart { get; }

        /// <summary>
        /// Gets the current line the cursor is on. This is zero based.
        /// </summary>
        int Line { get; }

        /// <summary>
        /// Gets the total number of lines in the text editor.
        /// </summary>
        int TotalLines { get; }

        /// <summary>
        /// Gets the text for the specified line.
        /// </summary>
        string GetLine(int index);

        /// <summary>
        /// Shows the code completion window.
        /// </summary>
        void ShowCompletionWindow(ICompletionDataProvider completionDataProvider);

        /// <summary>
        /// Shows the code completion window
        /// </summary>
        /// <param name="completionDataProvider"></param>
        /// <param name="enteredChar">The character just entered</param>
        void ShowCompletionWindow(ICompletionDataProvider completionDataProvider, char enteredChar);

        /// <summary>
        /// Indicates whether the completion window is currently being displayed.
        /// </summary>
        bool IsCompletionWindowDisplayed { get; }

        /// <summary>
        /// Makes the current text content read only. Text can be entered at the end.
        /// </summary>
        void MakeCurrentContentReadOnly();

        /// <summary>
        /// Perform custom key press handling
        /// </summary>
        /// <param name="keyData"></param>
        bool ProcessKeyPress(System.Windows.Forms.Keys keyData);

        /// <summary>
        /// Sets the parent control for this editor
        /// </summary>
        /// <param name="ctrl"></param>
        void SetParent(System.Windows.Forms.Control ctrl); 
    }
}
