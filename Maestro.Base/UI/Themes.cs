#region Disclaimer / License

// Copyright (C) 2016, Jackie Ng
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
        public static string CurrentTheme { get; internal set; }

        public const string VS2012Blue = nameof(VS2012Blue);
        //public const string VS2012Dark = nameof(VS2012Dark);
        public const string VS2012Light = nameof(VS2012Light);

        public const string VS2013Blue = nameof(VS2013Blue);
        //public const string VS2013Dark = nameof(VS2013Dark);
        public const string VS2013Light = nameof(VS2013Light);

        public const string VS2015Blue = nameof(VS2015Blue);
        //public const string VS2015Dark = nameof(VS2015Dark);
        public const string VS2015Light = nameof(VS2015Light);

        public static string[] List { get; } = new string[]
        {
            VS2012Blue,
            //VS2012Dark,
            VS2012Light,
            VS2013Blue,
            //VS2013Dark,
            VS2013Light,
            VS2015Blue,
            //VS2015Dark,
            VS2015Light
        };

        public static ThemeBase Get(string key)
        {
            switch (key)
            {
                case VS2012Blue:
                    return new VS2012BlueTheme();
                //case VS2012Dark:
                //    return new VS2012DarkTheme();
                case VS2012Light:
                    return new VS2012LightTheme();
                case VS2013Blue:
                    return new VS2013BlueTheme();
                //case VS2013Dark:
                //    return new VS2013DarkTheme();
                case VS2013Light:
                    return new VS2013LightTheme();
                case VS2015Blue:
                    return new VS2015BlueTheme();
                //case VS2015Dark:
                //    return new VS2015DarkTheme();
                case VS2015Light:
                    return new VS2015LightTheme();
                default:
                    return null;
            }
        }
    }
}
