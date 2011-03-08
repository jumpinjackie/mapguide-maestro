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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.Core.WinForms;
using ICSharpCode.Core;

namespace Maestro.Base
{
    /// <summary>
    /// The base class of all view content. Provides the default implementation of <see cref="IViewContent"/>
    /// </summary>
    [ToolboxItem(false)]
    public partial class ViewContentBase : UserControl, IViewContent
    {
        public ViewContentBase()
        {
            InitializeComponent();
        }

        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    var handler = this.TitleChanged;
                    if (handler != null)
                        handler(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler TitleChanged;

        public virtual bool AllowUserClose
        {
            get { return true; }
        }

        public virtual void Close()
        {
            CancelEventArgs ce = new CancelEventArgs(false);
            var ceHandler = this.ViewContentClosing;
            if (ceHandler != null)
                ceHandler(this, ce);

            if (ce.Cancel)
                return;

            var handler = this.ViewContentClosed;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event CancelEventHandler ViewContentClosing;

        public void ShowError(Exception ex)
        {
            MessageService.ShowError(ex);
        }

        public void ShowError(string message)
        {
            MessageService.ShowError(message);
        }

        public void ShowMessage(string title, string message)
        {
            MessageService.ShowMessage(message, title);
        }

        public bool Confirm(string title, string message)
        {
            return MessageService.AskQuestion(message, title);
        }

        public bool ConfirmFormatted(string title, string format, params string[] args)
        {
            return MessageService.AskQuestion(string.Format(format, args), title);
        }

        public Control ContentControl
        {
            get { return this; }
        }

        private string _description;

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    var handler = this.DescriptionChanged;
                    if (handler != null)
                        handler(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler DescriptionChanged;

        public void Activate()
        {
            var handler = this.ViewContentActivating;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler ViewContentHiding;

        public event EventHandler ViewContentShowing;

        public event EventHandler ViewContentActivating;

        void IViewContent.Hide()
        {
            var handler = this.ViewContentHiding;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public bool IsAttached
        {
            get;
            internal set;
        }

        public virtual ViewRegion DefaultRegion
        {
            get { return ViewRegion.Document; }
        }

        public event EventHandler ViewContentClosed;
    }
}
