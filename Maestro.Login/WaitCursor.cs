#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using System.Windows.Forms;

namespace Maestro.Login
{
	/// <summary>
	/// A class that wraps the wait cursor into a disposable class, for use with deterministic disposal
	/// </summary>
	public class WaitCursor 
		: IDisposable
	{
		private Form m_owner = null;
		private Cursor m_oldcursor;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitCursor"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
		public WaitCursor(Form owner)
		{
			//This ensures that nested WaitCursors behave as expected.
			//If we are the outermost WaitCursor, set the cursor
			if (owner.Cursor != Cursors.WaitCursor)
			{
				m_oldcursor = owner.Cursor;
				m_owner = owner;
				m_owner.Cursor = Cursors.WaitCursor;
			}
		}

		#region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
		public void Dispose()
		{
			//If we are the the outermost WaitCursor, reset the cursor
			if (m_owner != null && m_owner.Cursor == Cursors.WaitCursor)
			{
				m_owner.Cursor = m_oldcursor;
				m_owner = null;
			}
		}

		#endregion
	}
}
