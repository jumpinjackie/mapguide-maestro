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

namespace Maestro.Shared.UI
{
    /// <summary>
    /// The base class of all view content. Provides the default implementation of <see cref="IViewContent"/>
    /// </summary>
    [ToolboxItem(false)]
    public partial class ViewContentBase : UserControl, IViewContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewContentBase"/> class.
        /// </summary>
        public ViewContentBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets whether this instance is a modal window
        /// </summary>
        public virtual bool IsModalWindow { get { return false; } }

        /// <summary>
        /// Gets whether this instance can only be docked to the document region
        /// </summary>
        public virtual bool IsExclusiveToDocumentRegion { get { return false; } }

        private string _title;

        /// <summary>
        /// The title of the view
        /// </summary>
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

        private Form _parent;

        /// <summary>
        /// Sets the parent form for this instance
        /// </summary>
        /// <param name="form"></param>
        public void SetParentForm(Form form)
        {
            if (_parent != null)
                throw new InvalidOperationException("Parent form already set");

            _parent = form;
            _parent.FormClosing += new FormClosingEventHandler(OnParentFormClosing);
            _parent.FormClosing += new FormClosingEventHandler(OnParentFormClosed);
        }

        void OnParentFormClosed(object sender, FormClosingEventArgs e)
        {
            var h = this.ViewContentClosed;
            if (h != null)
                h(this, EventArgs.Empty);
        }

        void OnParentFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = CheckCancelEvents();
        }

        /// <summary>
        /// Fires when the title has been changed
        /// </summary>
        public event EventHandler TitleChanged;

        /// <summary>
        /// Detrmines if this view can be closed by the user, note that this does not affect the <see cref="Close"/> method
        /// in any way. All view content can still be programmatically closed if they inherit from <see cref="ViewContentBase"/> and
        /// does not override the default implementation of <see cref="Close"/>
        /// </summary>
        public virtual bool AllowUserClose
        {
            get { return true; }
        }

        internal bool CheckCancelEvents()
        {
            CancelEventArgs ce = new CancelEventArgs(false);
            var ceHandler = this.ViewContentClosing;
            if (ceHandler != null)
                ceHandler(this, ce);

            return ce.Cancel;
        }

        /// <summary>
        /// Closes the view. This raises the <see cref="ViewContentClosing"/> event
        /// </summary>
        public virtual void Close()
        {
            if (_parent != null)
                _parent.Close();
        }

        /// <summary>
        /// Fired when the view has been closed internally
        /// </summary>
        public event CancelEventHandler ViewContentClosing;

        /// <summary>
        /// Displays an exception message
        /// </summary>
        /// <param name="ex">The exception object</param>
        public void ShowError(Exception ex)
        {
            ErrorDialog.Show(ex);
        }

        /// <summary>
        /// Displays an error message
        /// </summary>
        /// <param name="message">The message</param>
        public virtual void ShowError(string message)
        {
            //MessageService.ShowError(message);
            ErrorDialog.Show(message, message);
        }

        /// <summary>
        /// Displays an alert message
        /// </summary>
        /// <param name="title">The title of this message</param>
        /// <param name="message">The message</param>
        public virtual void ShowMessage(string title, string message)
        {
            //MessageService.ShowMessage(message, title);
            MessageBox.Show(message, title);
        }

        /// <summary>
        /// Make a request for confirmation
        /// </summary>
        /// <param name="title">The title of the confirmation message</param>
        /// <param name="message">The message</param>
        /// <returns>
        /// true if confirmed, false otherwise
        /// </returns>
        public virtual bool Confirm(string title, string message)
        {
            //return MessageService.AskQuestion(message, title);
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        /// <summary>
        /// Make a request for confirmation
        /// </summary>
        /// <param name="title">The title of the confirmation message</param>
        /// <param name="format">The message template</param>
        /// <param name="args">The template values</param>
        /// <returns>
        /// true if confirmed, false otherwise
        /// </returns>
        public virtual bool ConfirmFormatted(string title, string format, params string[] args)
        {
            //return MessageService.AskQuestion(string.Format(format, args), title);
            return MessageBox.Show(string.Format(format, args), title, MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        /// <summary>
        /// The underlying control
        /// </summary>
        public Control ContentControl
        {
            get { return this; }
        }

        private string _description;

        /// <summary>
        /// The view's description, this is the ToolTip content
        /// </summary>
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

        /// <summary>
        /// Raised when the description has changed
        /// </summary>
        public event EventHandler DescriptionChanged;

        /// <summary>
        /// Makes this content active
        /// </summary>
        public void Activate()
        {
            var handler = this.ViewContentActivating;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fired when the view is going to hide
        /// </summary>
        public event EventHandler ViewContentHiding;

        /// <summary>
        /// Fired when the view is activating
        /// </summary>
        public event EventHandler ViewContentActivating;

        /// <summary>
        /// Conceals the control from the user.
        /// </summary>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        ///   </PermissionSet>
        void IViewContent.Hide()
        {
            var handler = this.ViewContentHiding;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Indicates whether this view is attached to a workbench
        /// </summary>
        public bool IsAttached
        {
            get;
            internal set;
        }

        /// <summary>
        /// Indicates the default region this view content will be put in
        /// </summary>
        public virtual ViewRegion DefaultRegion
        {
            get { return ViewRegion.Document; }
        }
        
        /// <summary>
        /// Gets the icon for this view
        /// </summary>
        public virtual Icon ViewIcon
        {
            get { return null; }
        }

        /// <summary>
        /// Fired when the view has been closed internally
        /// </summary>
        public event EventHandler ViewContentClosed;

        /// <summary>
        /// Fired when the view, which was hidden is now being shown
        /// </summary>
        public event EventHandler ViewContentShowing;
    }
}
