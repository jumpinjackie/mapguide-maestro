#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
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

namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// Font information
    /// </summary>
    public struct FontInfo
    {
        /// <summary>
        /// The name of the font
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Enable/disable italics
        /// </summary>
        public bool Italic { get; set; }

        /// <summary>
        /// Enable/disable bold
        /// </summary>
        public bool Bold { get; set; }

        /// <summary>
        /// Enable/disable underline
        /// </summary>
        public bool Underline { get; set; }
    }
}
