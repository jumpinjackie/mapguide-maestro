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
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.ComponentModel;

namespace Maestro.Editors
{
    /// <summary>
    /// Base class of all resource editors
    /// </summary>
    [ToolboxItem(false)]
    public class EditorBase : UserControl, IEditorBindable
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
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnsubscribeEventHandlers();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Unsubscribes the event handlers.
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
                this.ResourceChanged = null;
            }
        }

        internal void RaiseResourceChanged() { OnResourceChanged(); }

        /// <summary>
        /// Called when [resource changed].
        /// </summary>
        protected virtual void OnResourceChanged()
        {
            var handler = this.ResourceChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when [resource changed].
        /// </summary>
        public event EventHandler ResourceChanged;
    }

    /// <summary>
    /// An interface for flagging dirty state
    /// </summary>
    public interface INotifyResourceChanged
    {
        /// <summary>
        /// Occurs when [resource changed].
        /// </summary>
        event EventHandler ResourceChanged;
    }
}
