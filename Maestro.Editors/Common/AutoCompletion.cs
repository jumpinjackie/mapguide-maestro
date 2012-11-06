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
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.Common
{ 
    internal class AutoCompletionListBoxItem
    {
        private string _myText;
        private int _myImageIndex;
        // properties 
        public string Text
        {
            get { return _myText; }
            set { _myText = value; }
        }
        public int ImageIndex
        {
            get { return _myImageIndex; }
            set { _myImageIndex = value; }
        }
        //constructor
        public AutoCompletionListBoxItem(string text, int index)
        {
            _myText = text;
            _myImageIndex = index;
        }
        public AutoCompletionListBoxItem(string text) : this(text, -1) { }
        public AutoCompletionListBoxItem() : this(string.Empty) { }

        private object _tag;

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override string ToString()
        {
            return _myText;
        }
    }

    // AutoCompletionListBox class 
    //
    // Based on GListBox
    //
    // http://www.codeproject.com/KB/combobox/glistbox.aspx

    internal class AutoCompletionListBox : ListBox
    {
        public ImageList ImageList
        {
            get;
            private set;
        }

        public AutoCompletionListBox()
        {
            // Set owner draw mode
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.IsShown = false;
            this.Font = new System.Drawing.Font(FontFamily.GenericMonospace, 10.0f); 
            this.DoubleClick += new EventHandler(OnAutoCompleteDoubleClick);
            this.SelectedIndexChanged += new EventHandler(OnAutoCompleteSelectedIndexChanged);
            this.KeyPress += OnKeyPress;
        }

        void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        internal void HandleEnterKey()
        {
            if (!this.IsShown)
                return;
            PutAutoCompleteSuggestion();
            this.HideBox();
        }

        internal void AdvanceInsertionOffset()
        {
            _context.InsertionOffset++;
        }

        private void PutAutoCompleteSuggestion()
        {
            if (this.SelectedItem != null)
            {
                var data = ((AutoCompletionListBoxItem)this.SelectedItem).Tag as ICompletionData;
                if (data != null)
                {
                    _context.CompletionProvider.InsertAction(data,
                                                             _context.Editor.ActiveTextAreaControl.TextArea,
                                                             _context.InsertionOffset + 1,
                                                             _context.FirstChar);
                }
            }
        }

        void OnAutoCompleteSelectedIndexChanged(object sender, EventArgs e)
        {
            _context.Editor.Focus();
            if (this.Visible && this.SelectedIndex >= 0 && this.Items.Count > 0)
            {
                var data = ((AutoCompletionListBoxItem)this.SelectedItem).Tag as ICompletionData;
                if (data != null)
                {
                    string tt = data.Description;
                    if (!string.IsNullOrEmpty(tt))
                    {
                        Point pt = _context.GetCaretPoint();
                        pt.X += this.Width + 10;
                        pt.Y += 65;

                        _context.AutoCompleteTooltip.Show(tt, this, pt.X, pt.Y);
                    }
                }
            }
        }

        void OnAutoCompleteDoubleClick(object sender, EventArgs e)
        {
            PutAutoCompleteSuggestion();
            HideBox();
        }

        public bool IsShown
        {
            get;
            private set;
        }

        internal void MoveAutoCompleteSelectionDown()
        {
            if (this.SelectedIndex < 0)
            {
                this.SelectedIndex = 0;
            }
            else
            {
                int idx = this.SelectedIndex;
                if ((idx + 1) <= this.Items.Count - 1)
                {
                    this.SelectedIndex = idx + 1;
                }
            }
        }

        internal void MoveAutoCompleteSelectionUp()
        {
            if (this.SelectedIndex < 0)
            {
                this.SelectedIndex = 0;
            }
            else
            {
                int idx = this.SelectedIndex;
                if ((idx - 1) >= 0)
                {
                    this.SelectedIndex = idx - 1;
                }
            }
        }

        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            AutoCompletionListBoxItem item;
            Rectangle bounds = e.Bounds;
            Size imageSize = this.ImageList.ImageSize;
            try
            {
                item = (AutoCompletionListBoxItem)Items[e.Index];
                if (item.ImageIndex != -1)
                {
                    this.ImageList.Draw(e.Graphics, bounds.Left, bounds.Top, item.ImageIndex);
                    e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor),
                        bounds.Left + imageSize.Width, bounds.Top);
                }
                else
                {
                    e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor),
                        bounds.Left, bounds.Top);
                }
            }
            catch
            {
                if (e.Index != -1)
                {
                    e.Graphics.DrawString(Items[e.Index].ToString(), e.Font,
                        new SolidBrush(e.ForeColor), bounds.Left, bounds.Top);
                }
                else
                {
                    e.Graphics.DrawString(Text, e.Font, new SolidBrush(e.ForeColor),
                        bounds.Left, bounds.Top);
                }
            }
            base.OnDrawItem(e);
        }

        internal ICompletionDataProvider CurrentProvider
        {
            get
            {
                if (_context != null)
                    return _context.CompletionProvider;
                return null;
            }
        }

        internal void SetCompletionItems(Form parent, AutoCompleteContext context, string fileName)
        {
            SetCompletionItems(parent, context, fileName, true, true);
        }

        internal class AutoCompleteContext
        {
            public ICompletionDataProvider CompletionProvider;
            public TextEditorControl Editor;
            public Func<Point> GetCaretPoint;
            public ToolTip AutoCompleteTooltip;
            public char FirstChar;
            public int InsertionOffset;
        }

        private AutoCompleteContext _context;
        
        private void SetCompletionItems(Form parent, AutoCompleteContext context, string fileName, bool p1, bool p2)
        {
            _context = context;
            ICompletionData[] completionData = _context.CompletionProvider.GenerateCompletionData(fileName, _context.Editor.ActiveTextAreaControl.TextArea, _context.FirstChar);
            if (completionData == null || completionData.Length == 0)
            {
                _context = null;
                HideBox();
            }
            else
            {
                this.ImageList = _context.CompletionProvider.ImageList;

                int width = 0;
                this.Items.Clear();
                foreach (ICompletionData it in completionData)
                {
                    AutoCompletionListBoxItem litem = new AutoCompletionListBoxItem();
                    litem.Text = it.Text;
                    litem.ImageIndex = it.ImageIndex;
                    litem.Tag = it;

                    this.Items.Add(litem);
                    int length = TextRenderer.MeasureText(it.Text, this.Font).Width + 30; //For icon size
                    if (length > width)
                        width = length;
                }
                this.Width = width + 30;
                this.BringToFront();
                this.Show();
                this.IsShown = true;
                Point pt = _context.GetCaretPoint();
                this.Location = pt;
            }
        }

        internal void HideBox()
        {
            System.Diagnostics.Debug.WriteLine("Contextual buffer cleared");
            this.Hide();
            if (_context != null)
                _context.AutoCompleteTooltip.Hide(this);
            this.IsShown = false;
        }
    }
}
