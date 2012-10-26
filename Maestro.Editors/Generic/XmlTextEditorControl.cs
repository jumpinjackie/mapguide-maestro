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
    }

    public class TextEditorProperties : ITextEditorProperties
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
