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
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Maestro.AddIn.Scripting.Lang.Python
{
    /// <summary>
    /// Provides code completion for the Python Console window.
    /// </summary>
    internal class PythonConsoleCompletionDataProvider : ICompletionDataProvider
    {
        IMemberProvider memberProvider;

        public PythonConsoleCompletionDataProvider(IMemberProvider memberProvider)
        {
            this.memberProvider = memberProvider;
            this.DefaultIndex = 0;
            this.PreSelection = null;
            this.ImageList = new System.Windows.Forms.ImageList();
            this.ImageList.Images.Add(Properties.Resources.block);
        }

        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            return GenerateCompletionData(GetLineText(textArea));
        }

        /// <summary>
        /// Generates completion data for the specified text. The text should be everything before
        /// the dot character that triggered the completion. The text can contain the command line prompt
        /// '>>>' as this will be ignored.
        /// </summary>
        public ICompletionData[] GenerateCompletionData(string line)
        {
            List<DefaultCompletionData> items = new List<DefaultCompletionData>();
            string name = GetName(line);
            if (!String.IsNullOrEmpty(name))
            {
                try
                {
                    //TODO: It would be nice if we could get extra information besides the name. I'm not sure
                    //if the DLR hosting API can give us anymore information
                    foreach (string member in memberProvider.GetMemberNames(name))
                    {
                        items.Add(new DefaultCompletionData(member, String.Empty, 0)); //ClassBrowserIconService.MethodIndex));
                    }
                }
                catch
                {
                    // Do nothing.
                }
            }
            return items.ToArray();
        }

        string GetName(string text)
        {
            //Assume compacted expressions, meaning we need to take into account operators and non-whitespace tokens
            //TODO: Still imperfect. A dot after a function expression brings up nothing. But a dot after a named variable will
            //trigger auto-complete most of the time
            int startIndex = text.LastIndexOfAny(new char[] { ' ', '+', '/', '*', '-', '%', '=', '>', '<', '&', '|', '^', '~', '(', ')' });
            string res = text.Substring(startIndex + 1);
            Debug.WriteLine("Evaluating python auto-complete options for: " + res);
            return res;
        }

        /// <summary>
        /// Gets the line of text up to the cursor position.
        /// </summary>
        string GetLineText(TextArea textArea)
        {
            LineSegment lineSegment = textArea.Document.GetLineSegmentForOffset(textArea.Caret.Offset);
            return textArea.Document.GetText(lineSegment);
        }

        public System.Windows.Forms.ImageList ImageList
        {
            get;
            private set;
        }

        public string PreSelection
        {
            get;
            private set;
        }

        public int DefaultIndex
        {
            get;
            private set;
        }

        public bool InsertSpace
        {
            get;
            set;
        }

        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            CompletionDataProviderKeyResult res;
            if (key == ' ' && this.InsertSpace)
            {
                this.InsertSpace = false; // insert space only once
                res = CompletionDataProviderKeyResult.BeforeStartKey;
            }
            else if (char.IsLetterOrDigit(key) || key == '_')
            {
                this.InsertSpace = false; // don't insert space if user types normally
                res = CompletionDataProviderKeyResult.NormalKey;
            }
            else
            {
                // do not reset insertSpace when doing an insertion!
                res = CompletionDataProviderKeyResult.InsertionKey;
            }
            return res;
        }

        public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        {
            if (this.InsertSpace)
            {
                textArea.Document.Insert(insertionOffset++, " ");
            }
            textArea.Caret.Position = textArea.Document.OffsetToPosition(insertionOffset);

            return data.InsertAction(textArea, key);
        }
    }
}
