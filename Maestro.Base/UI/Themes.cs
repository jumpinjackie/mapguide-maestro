#region Disclaimer / License

// Copyright (C) 2016, Jackie Ng
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
using WeifenLuo.WinFormsUI.Docking;

namespace Maestro.Base.UI
{
    //NOTE: Dark themes disabled as they don't properly mesh with toolstrips (esp wrt label colors)
    //Possibly related to: https://github.com/dockpanelsuite/dockpanelsuite/issues/415

    /// <summary>
    /// Available themes
    /// </summary>
    public static class Themes
    {
        /// <summary>
        /// Gets the current theme
        /// </summary>
        public static string CurrentTheme { get; internal set; }

        /// <summary>
        /// The VS2012 blue theme
        /// </summary>
        public const string VS2012Blue = nameof(VS2012Blue);

        /// <summary>
        /// The VS2012 dark theme
        /// </summary>
        public const string VS2012Dark = nameof(VS2012Dark);

        /// <summary>
        /// The VS2012 light theme
        /// </summary>
        public const string VS2012Light = nameof(VS2012Light);

        /// <summary>
        /// The VS2013 blue theme
        /// </summary>
        public const string VS2013Blue = nameof(VS2013Blue);

        /// <summary>
        /// The VS2013 dark theme
        /// </summary>
        public const string VS2013Dark = nameof(VS2013Dark);

        /// <summary>
        /// The VS2013 light theme
        /// </summary>
        public const string VS2013Light = nameof(VS2013Light);

        /// <summary>
        /// The VS2015 blue theme
        /// </summary>
        public const string VS2015Blue = nameof(VS2015Blue);

        /// <summary>
        /// The VS2015 dark theme
        /// </summary>
        public const string VS2015Dark = nameof(VS2015Dark);

        /// <summary>
        /// The VS2015 light theme
        /// </summary>
        public const string VS2015Light = nameof(VS2015Light);

        /// <summary>
        /// The list of available themes
        /// </summary>
        public static string[] List { get; } = new string[]
        {
            VS2012Blue,
            VS2012Dark,
            VS2012Light,
            VS2013Blue,
            VS2013Dark,
            VS2013Light,
            VS2015Blue,
            VS2015Dark,
            VS2015Light
        };

        /// <summary>
        /// Gets the theme for the specific key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ThemeBase Get(string key)
        {
            switch (key)
            {
                case VS2012Blue:
                    return new VS2012BlueTheme();
                case VS2012Dark:
                    return new VS2012DarkTheme();
                case VS2012Light:
                    return new VS2012LightTheme();
                case VS2013Blue:
                    return new VS2013BlueTheme();
                case VS2013Dark:
                    return new VS2013DarkTheme();
                case VS2013Light:
                    return new VS2013LightTheme();
                case VS2015Blue:
                    return new VS2015BlueTheme();
                case VS2015Dark:
                    return new VS2015DarkTheme();
                case VS2015Light:
                    return new VS2015LightTheme();
                default:
                    return null;
            }
        }
    }
}
