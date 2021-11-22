#region Disclaimer / License

// Copyright (C) 2021, Jackie Ng
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

using ICSharpCode.Core;

namespace Maestro.Scripting.Core
{
    /// <summary>
    /// Scripting AddIn configuration properties
    /// </summary>
    public static class ScriptingConfigProperties
    {
        /// <summary>
        /// Default IronPython module path
        /// </summary>
        public static string DefaultIronPythonModulePath => Path.Combine(FileUtility.ApplicationRootPath, "Lib");

        /// <summary>
        /// Show the IronPython console by default
        /// </summary>
        public static bool DefaultShowIronPythonConsole => true;

        /// <summary>
        /// IronPython module path (property name)
        /// </summary>
        public const string IronPythonModulePath = "Scripting.IronPythonModulePaths"; //NOXLATE

        /// <summary>
        /// Show the IronPython console (property name)
        /// </summary>
        public const string ShowIronPythonConsole = "Scripting.ShowIronPythonConsole"; //NOXLATE
    }
}