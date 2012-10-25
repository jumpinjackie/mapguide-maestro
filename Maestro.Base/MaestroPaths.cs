#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.Reflection;
using System.IO;

namespace Maestro.Base
{
    /// <summary>
    /// Contains path information
    /// </summary>
    public class MaestroPaths
    {
        /// <summary>
        /// The name of the application
        /// </summary>
        public static string AppName
        {
            get
            {
                var asmVersion = Assembly.GetExecutingAssembly().GetName().Version;
                return string.Format("Maestro-{0}.{1}", asmVersion.Major, asmVersion.Minor); //NOXLATE
            }
        }

        /// <summary>
        /// Gets the root directory for all application configuration and data
        /// </summary>
        public static string BasePath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
            }
        }

        /*
        /// <summary>
        /// Gets the path for user-installable addins
        /// </summary>
        public static string UserAddInLocation
        {
            get
            {
                var asmVersion = Assembly.GetExecutingAssembly().GetName().Version;
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    string.Format("Maestro-{0}.{1}", asmVersion.Major, asmVersion.Minor), //NOXLATE
                                    "AddIns"); //NOXLATE
            }
        }*/
    }
}
