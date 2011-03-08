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
using OSGeo.MapGuide.ObjectModels.WebLayout;

namespace Maestro.Editors.WebLayout
{
    internal static class CommandIconCache
    {
        public static Bitmap GetStandardCommandIcon(BuiltInCommandType cmdType)
        {
            var resMgr = Properties.Resources.ResourceManager;
            var culture = Properties.Resources.Culture;

            return resMgr.GetObject("icon_" + cmdType.ToString().ToLower(), culture) as Bitmap;
        }

        public static Bitmap GetStandardCommandIcon(string relpath)
        {
            if (relpath == null || !relpath.StartsWith("../stdicons/") && !relpath.EndsWith(".gif"))
                return null;

            var resMgr = Properties.Resources.ResourceManager;
            var culture = Properties.Resources.Culture;

            int stdicons = "../stdicons/".Length;
            int gif = ".gif".Length;
            string iconName = relpath.Substring(stdicons, relpath.Length - gif - stdicons);

            return resMgr.GetObject(iconName, culture) as Bitmap;
        }
    }
}
