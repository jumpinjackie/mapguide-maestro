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

#endregion Disclaimer / License

using ICSharpCode.TextEditor.Document;
using Maestro.AddIn.Scripting.Services;
using Maestro.Editors.Common;
using Microsoft.Scripting.Hosting.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Maestro.AddIn.Scripting.Lang.Python
{
    internal class PythonConsole : IConsole, IDisposable, IMemberProvider
    {
        private ITextEditor textEditor;
        private int lineReceivedEventIndex = 0; // The index into the waitHandles array where the lineReceivedEvent is stored.
        private ManualResetEvent inputLineReceivedEvent = new ManualResetEvent(false);
        private ManualResetEvent lineReceivedEvent = new ManualResetEvent(false);
        private ManualResetEvent disposedEvent = new ManualResetEvent(false);
        private WaitHandle[] waitHandles;
        private int promptLength;
        private List<string> previousLines = new List<string>();
        private CommandLine commandLine;
        private CommandLineHistory commandLineHistory = new CommandLineHistory();

        public CommandLine CommandLine { get { return this.commandLine; } }

        private IConsoleLineHook _hook;

        public PythonConsole(ITextEditor textEditor, CommandLine commandLine, IConsoleLineHook hook)
        {
            _hook = hook;
            waitHandles = new WaitHandle[] { lineReceivedEvent, disposedEvent };

            this.commandLine = commandLine;

            this.textEditor = textEditor;
            textEditor.KeyPress += ProcessKeyPress;
            textEditor.DialogKeyPress += ProcessDialogKeyPress;
            textEditor.IndentStyle = IndentStyle.None;
        }

        public void Dispose()
        {
            disposedEvent.Set();
            //TextArea textArea = textEditor.ActiveTextAreaControl.TextArea;
            //textArea.KeyEventHandler -= ProcessKeyPress;
            //textArea.DoProcessDialogKey -= ProcessDialogKey;
        }

        public TextWriter Output
        {
            get
            {
                Debug.WriteLine("PythonConsole.Output get");
                return null;
            }
            set
            {
                Debug.WriteLine("PythonConsole.Output set");
            }
        }

        public TextWriter ErrorOutput
        {
            get
            {
                Debug.WriteLine("PythonConsole.ErrorOutput get");
                return null;
            }
            set
            {
                Debug.WriteLine("PythonConsole.ErrorOutput get");
            }
        }

        /// <summary>
        /// Gets the member names of the specified item.
        /// </summary>
        public IList<string> GetMemberNames(string name)
        {
            return commandLine.GetMemberNames(name);
        }

        public IList<string> GetGlobals(string name)
        {
            return commandLine.GetGlobals(name);
        }

        /// <summary>
        /// Returns the next line typed in by the console user. If no line is available this method
        /// will block.
        /// </summary>
        public string ReadLine(int autoIndentSize)
        {
            Debug.WriteLine("PythonConsole.ReadLine(): autoIndentSize: " + autoIndentSize);
            string indent = String.Empty;
            if (autoIndentSize > 0)
            {
                indent = String.Empty.PadLeft(autoIndentSize);
                Write(indent, Style.Prompt);
            }

            string line = ReadLineFromTextEditor();
            if (line != null)
            {
                Debug.WriteLine("ReadLine: " + indent + line);
                return indent + line;
            }
            return null;
        }

        private bool bHostAppInitialized = false;

        /// <summary>
        /// Writes text to the console.
        /// </summary>
        public void Write(string text, Style style)
        {
            Debug.WriteLine("PythonConsole.Write(text, style): " + text);
            if (style == Style.Error)
                textEditor.Write(text, Color.Red, Color.White);
            else if (style == Style.Warning)
                textEditor.Write(text, Color.Yellow, Color.Black);
            else
                textEditor.Write(text);

            if (style == Style.Prompt)
            {
                promptLength = text.Length;
                textEditor.MakeCurrentContentReadOnly();
            }

            //HACK: This seems to be the safest point which to inject our Host Application
            if (!bHostAppInitialized && this.commandLine.ScriptScope != null)
            {
                this.commandLine.ScriptScope.SetVariable(ScriptGlobals.HostApp, new HostApplication());
                bHostAppInitialized = true;
            }
        }

        /// <summary>
        /// Writes text followed by a newline to the console.
        /// </summary>
        public void WriteLine(string text, Style style)
        {
            Write(text + Environment.NewLine, style);
        }

        /// <summary>
        /// Writes an empty line to the console.
        /// </summary>
        public void WriteLine()
        {
            Write(Environment.NewLine, Style.Out);
        }

        /// <summary>
        /// Indicates whether there is a line already read by the console and waiting to be processed.
        /// </summary>
        public bool IsLineAvailable
        {
            get
            {
                lock (previousLines)
                {
                    return previousLines.Count > 0;
                }
            }
        }

        /// <summary>
        /// Gets the text that is yet to be processed from the console. This is the text that is being
        /// typed in by the user who has not yet pressed the enter key.
        /// </summary>
        public string GetCurrentLine()
        {
            string fullLine = GetLastTextEditorLine();
            return fullLine.Substring(promptLength);
        }

        /// <summary>
        /// Gets the lines that have not been returned by the ReadLine method. This does not
        /// include the current line.
        /// </summary>
        public string[] GetUnreadLines()
        {
            return previousLines.ToArray();
        }

        private string GetLastTextEditorLine()
        {
            return textEditor.GetLine(textEditor.TotalLines - 1);
        }

        private readonly object _syncInput = new object();
        private bool _IsReadingInput;

        internal bool IsReadingInput
        {
            get
            {
                lock (_syncInput)
                    return _IsReadingInput;
            }
            set
            {
                lock (_syncInput)
                {
                    _IsReadingInput = value;
                    Debug.WriteLine("({0}): IsReadingInput: {1}", Thread.CurrentThread.ManagedThreadId, value);
                    if (!value)
                    {
                        Debug.WriteLine("({0}): IsReadingInput - Reset line received event", Thread.CurrentThread.ManagedThreadId);
                        inputLineReceivedEvent.Reset();
                    }
                }
            }
        }

        private string _inputLine;

        internal string GetLineForInput(string lastWrittenLine)
        {
            Debug.WriteLine(string.Format("({0}): GetLineForInput() - BEGIN", Thread.CurrentThread.ManagedThreadId));
            inputLineReceivedEvent.WaitOne();
            Debug.WriteLine("({0}): GetLineForInput() - {1}", Thread.CurrentThread.ManagedThreadId, _inputLine);
            string lineInput = null;
            if (_inputLine != null)
            {
                lineInput = _inputLine.Substring(lastWrittenLine.Substring(promptLength).Length);
                _inputLine = null;
            }
            this.IsReadingInput = false;
            return lineInput;
        }

        

        private string ReadLineFromTextEditor()
        {
            Debug.WriteLine(string.Format("({0}): ReadLineFromTextEditor()", Thread.CurrentThread.ManagedThreadId));
            _hook.OnBeginWaitForNextLine();
            int result = WaitHandle.WaitAny(waitHandles);
            if (result == lineReceivedEventIndex)
            {
                Debug.WriteLine(string.Format("({0}): Received line", Thread.CurrentThread.ManagedThreadId));
                lock (previousLines)
                {
                    string line = previousLines[0];
                    previousLines.RemoveAt(0);
                    if (previousLines.Count == 0)
                    {
                        Debug.WriteLine(string.Format("({0}): ReadLineFromTextEditor - Reset line received event", Thread.CurrentThread.ManagedThreadId));
                        lineReceivedEvent.Reset();
                    }
                    return line;
                }
            }
            return null;
        }

        /// <summary>
        /// Processes characters entered into the text editor by the user.
        /// </summary>
        private bool ProcessKeyPress(char ch)
        {
            if (IsInReadOnlyRegion)
            {
                return true;
            }
            
            if (ch == '\n')
            {
                OnEnterKeyPressed();
            }

            if (ch == '.')
            {
                ShowCompletionWindow();
            }
            return false;
        }

        /// <summary>
        /// Process dialog keys such as the enter key when typed into the editor by the user.
        /// </summary>
        private bool ProcessDialogKeyPress(Keys keyData)
        {
            if (textEditor.ProcessKeyPress(keyData))
                return true;

            if (textEditor.IsCompletionWindowDisplayed)
            {
                return false;
            }

            if (IsInReadOnlyRegion)
            {
                switch (keyData)
                {
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Up:
                    case Keys.Down:
                        return false;

                    default:
                        return true;
                }
            }

            switch (keyData)
            {
                case Keys.Back:
                    return !CanBackspace;

                case Keys.Home:
                    MoveToHomePosition();
                    return true;

                case Keys.Down:
                    MoveToNextCommandLine();
                    return true;

                case Keys.Up:
                    MoveToPreviousCommandLine();
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Move cursor to the end of the line before retrieving the line.
        /// </summary>
        private void OnEnterKeyPressed()
        {
            Debug.WriteLine(string.Format("({0}): OnEnterKeyPressed", Thread.CurrentThread.ManagedThreadId));
            lock (previousLines)
            {
                // Move cursor to the end of the line.
                textEditor.Column = GetLastTextEditorLine().Length;

                // Append line.
                string currentLine = GetCurrentLine();

                if (IsReadingInput)
                {
                    _inputLine = currentLine;
                    Debug.WriteLine(string.Format("({0}): Set input line received event", Thread.CurrentThread.ManagedThreadId));
                    inputLineReceivedEvent.Set();
                }
                else
                {
                    previousLines.Add(currentLine);
                    commandLineHistory.Add(currentLine);

                    Debug.WriteLine(string.Format("({0}): Set line received event", Thread.CurrentThread.ManagedThreadId));
                    lineReceivedEvent.Set();
                }
            }
        }

        /// <summary>
        /// Returns true if the cursor is in a readonly text editor region.
        /// </summary>
        private bool IsInReadOnlyRegion
        {
            get { return IsCurrentLineReadOnly || IsInPrompt; }
        }

        /// <summary>
        /// Only the last line in the text editor is not read only.
        /// </summary>
        private bool IsCurrentLineReadOnly
        {
            get { return textEditor.Line < textEditor.TotalLines - 1; }
        }

        /// <summary>
        /// Determines whether the current cursor position is in a prompt.
        /// </summary>
        private bool IsInPrompt
        {
            get { return textEditor.Column - promptLength < 0; }
        }

        /// <summary>
        /// Returns true if the user can backspace at the current cursor position.
        /// </summary>
        private bool CanBackspace
        {
            get
            {
                int cursorIndex = textEditor.Column - promptLength;
                int selectionStartIndex = textEditor.SelectionStart - promptLength;
                return cursorIndex > 0 && selectionStartIndex > 0;
            }
        }

        private void ShowCompletionWindow()
        {
            PythonConsoleCompletionDataProvider completionProvider = new PythonConsoleCompletionDataProvider(this);
            textEditor.ShowCompletionWindow(completionProvider);
        }

        /// <summary>
        /// The home position is at the start of the line after the prompt.
        /// </summary>
        private void MoveToHomePosition()
        {
            textEditor.Column = promptLength;
        }

        /// <summary>
        /// Shows the previous command line in the command line history.
        /// </summary>
        private void MoveToPreviousCommandLine()
        {
            if (commandLineHistory.MovePrevious())
            {
                ReplaceCurrentLineTextAfterPrompt(commandLineHistory.Current);
            }
        }

        /// <summary>
        /// Shows the next command line in the command line history.
        /// </summary>
        private void MoveToNextCommandLine()
        {
            if (commandLineHistory.MoveNext())
            {
                ReplaceCurrentLineTextAfterPrompt(commandLineHistory.Current);
            }
        }

        /// <summary>
        /// Replaces the current line text after the prompt with the specified text.
        /// </summary>
        private void ReplaceCurrentLineTextAfterPrompt(string text)
        {
            string currentLine = GetCurrentLine();
            textEditor.Replace(promptLength, currentLine.Length, text);

            // Put cursor at end.
            textEditor.Column = promptLength + text.Length;
        }
    }
}