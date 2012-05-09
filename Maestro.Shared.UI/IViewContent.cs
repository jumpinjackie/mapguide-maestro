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
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Maestro.Shared.UI
{
    /// <summary>
    /// Abstract view interface.
    /// </summary>
    public interface IViewContent : ISubView
    {
        /// <summary>
        /// The title of the view
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// The view's description, this is the ToolTip content
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Raised when the description has changed
        /// </summary>
        event EventHandler DescriptionChanged;
        /// <summary>
        /// Fires when the title has been changed
        /// </summary>
        event EventHandler TitleChanged;
        /// <summary>
        /// Detrmines if this view can be closed by the user, note that this does not affect the <see cref="Close"/> method
        /// in any way. All view content can still be programmatically closed if they inherit from <see cref="ViewContentBase"/> and
        /// does not override the default implementation of <see cref="Close"/>
        /// </summary>
        bool AllowUserClose { get; }
        /// <summary>
        /// Makes this content active
        /// </summary>
        void Activate();
        /// <summary>
        /// Hides this view content. Can only be called when 
        /// </summary>
        void Hide();
        /// <summary>
        /// Closes the view. This raises the <see cref="ViewContentClosing"/> event
        /// </summary>
        /// <returns></returns>
        void Close();
        /// <summary>
        /// Sets the parent form this view will reside in. INTERNAL USE ONLY
        /// </summary>
        /// <param name="form"></param>
        void SetParentForm(System.Windows.Forms.Form form);
        /// <summary>
        /// Fired when the view is activating
        /// </summary>
        event EventHandler ViewContentActivating;
        /// <summary>
        /// Fired when the view has been closed internally
        /// </summary>
        event CancelEventHandler ViewContentClosing;
        /// <summary>
        /// Fired when the view has been closed internally
        /// </summary>
        event EventHandler ViewContentClosed;
        /// <summary>
        /// Fired when the view is going to hide
        /// </summary>
        event EventHandler ViewContentHiding;
        /// <summary>
        /// Fired when the view, which was hidden is now being shown 
        /// </summary>
        event EventHandler ViewContentShowing;
        /// <summary>
        /// Displays an exception message
        /// </summary>
        /// <param name="ex">The exception object</param>
        void ShowError(Exception ex);
        /// <summary>
        /// Displays an error message
        /// </summary>
        /// <param name="message">The message</param>
        void ShowError(string message);
        /// <summary>
        /// Displays an alert message
        /// </summary>
        /// <param name="title">The title of this message</param>
        /// <param name="message">The message</param>
        void ShowMessage(string title, string message);
        /// <summary>
        /// Make a request for confirmation
        /// </summary>
        /// <param name="title">The title of the confirmation message</param>
        /// <param name="message">The message</param>
        /// <returns>true if confirmed, false otherwise</returns>
        bool Confirm(string title, string message);
        /// <summary>
        /// Make a request for confirmation
        /// </summary>
        /// <param name="title">The title of the confirmation message</param>
        /// <param name="format">The message template</param>
        /// <param name="args">The template values</param>
        /// <returns>true if confirmed, false otherwise</returns>
        bool ConfirmFormatted(string title, string format, params string[] args);
        /// <summary>
        /// Indicates whether this view is attached to a workbench
        /// </summary>
        bool IsAttached { get; }
        /// <summary>
        /// Gets whether this view resides in a modal window. Only applies if the <see cref="DefaultRegion"/>
        /// is Floating
        /// </summary>
        bool IsModalWindow { get; }
        /// <summary>
        /// Gets whether this view is set exclusively to the document region. If true, this view will always
        /// be placed into the document region regardless of the regions specified in the <see cref="DefaultRegion"/>
        /// property
        /// </summary>
        bool IsExclusiveToDocumentRegion { get; }
        /// <summary>
        /// Indicates the default region this view content will be put in
        /// </summary>
        ViewRegion DefaultRegion { get; }
        /// <summary>
        /// Gets the icon for this view
        /// </summary>
        Icon ViewIcon { get; }
    }

    /// <summary>
    /// Defines the possible regions of the user interface a <see cref="IViewContent"/> can reside in 
    /// </summary>
    public enum ViewRegion
    {
        /// <summary>
        /// The view content will reside in a floating dialog
        /// </summary>
        Floating = 1,
        /// <summary>
        /// The view content will be docked to the left
        /// </summary>
        Left = 2,
        /// <summary>
        /// The view content will be docked to the right
        /// </summary>
        Right = 4,
        /// <summary>
        /// The view content will be docked to the top
        /// </summary>
        Top = 8,
        /// <summary>
        /// The view content will be docked to the bottom
        /// </summary>
        Bottom = 16,
        /// <summary>
        /// The view content will be docked to the center, (in a tabbed document interface)
        /// </summary>
        Document = 32
    }
}
