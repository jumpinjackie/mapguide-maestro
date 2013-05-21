#region Disclaimer / License
// Copyright (C) 2013, Jackie Ng
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

namespace Maestro.Editors.Preview
{
    /// <summary>
    /// Settings that control resource preview functionality
    /// </summary>
    public static class PreviewSettings
    {
        static PreviewSettings()
        {
            UseAjaxViewer = true;
            UseLocalPreview = true;
        }

        /// <summary>
        /// Determines if a local map viewer should be used over launching a viewer URL
        /// </summary>
        public static bool UseLocalPreview { get; set; }

        /// <summary>
        /// Determines if the AJAX viewer should be used over the Fusion viewer
        /// </summary>
        public static bool UseAjaxViewer { get; set; }
    }
}
