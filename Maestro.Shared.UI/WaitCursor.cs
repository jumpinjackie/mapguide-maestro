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

namespace Maestro.Shared.UI
{
    /// <summary>
    /// Helper class to set a control's cursor to busy during the lifetime of this
    /// object
    /// </summary>
    public class WaitCursor : IDisposable
    {
        private Control _owner;
        private Cursor _orig;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitCursor"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public WaitCursor(Control owner)
        {
            _owner = owner;
            _orig = _owner.Cursor;
            _owner.Cursor = Cursors.WaitCursor;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _owner.Cursor = _orig;
        }
    }
}
