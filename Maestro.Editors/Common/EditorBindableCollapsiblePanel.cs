#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

using Maestro.Shared.UI;
using System;
using System.ComponentModel;

namespace Maestro.Editors.Common
{
    /// <summary>
    /// An extension of <see cref="CollapsiblePanel"/> with a default implementation of the
    /// <see cref="IEditorBindable"/> interface
    /// </summary>
    [ToolboxItem(false)]
    public class EditorBindableCollapsiblePanel : CollapsiblePanel, IEditorBindable
    {
        /// <summary>
        /// Sets the initial state of this editor and sets up any databinding
        /// within such that user interface changes will propagate back to the
        /// model.
        /// </summary>
        /// <param name="service"></param>
        public virtual void Bind(IEditorService service)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnsubscribeEventHandlers();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Unsubscribes all handlers from events of this instance. If overridden in a dervied class, be
        /// sure to call the base class version
        /// </summary>
        protected virtual void UnsubscribeEventHandlers()
        {
            var handler = this.ResourceChanged;
            if (handler != null)
            {
                foreach (var h in handler.GetInvocationList())
                {
                    this.ResourceChanged -= (EventHandler)h;
                }
                //In case we left out something (shouldn't be)
                this.ResourceChanged = null;
            }
        }

        internal void RaiseResourceChanged()
        {
            OnResourceChanged();
        }

        /// <summary>
        /// Raises the <see cref="ResourceChanged"/> event. If overridden in the derived class, be sure
        /// to call the base class method to ensure the event is properly raised.
        /// </summary>
        protected virtual void OnResourceChanged()
        {
            this.ResourceChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when [resource changed].
        /// </summary>
        public event EventHandler ResourceChanged;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // EditorBindableCollapsiblePanel
            //
            this.Name = nameof(EditorBindableCollapsiblePanel);
            this.ResumeLayout(false);
        }
    }
}