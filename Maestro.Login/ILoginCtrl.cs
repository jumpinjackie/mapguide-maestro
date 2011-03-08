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

namespace Maestro.Login
{
    /// <summary>
    /// Login control interface
    /// </summary>
    public interface ILoginCtrl
    {
        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The username.</value>
        string Username { get; }
        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>The password.</value>
        string Password { get; }

        /// <summary>
        /// Updates the login status.
        /// </summary>
        void UpdateLoginStatus();

        /// <summary>
        /// Occurs when [enable ok].
        /// </summary>
        event EventHandler EnableOk;
        /// <summary>
        /// Occurs when [disabled ok].
        /// </summary>
        event EventHandler DisabledOk;
        /// <summary>
        /// Occurs when [check saved password].
        /// </summary>
        event EventHandler CheckSavedPassword;
    }
}
