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
using System;
using System.IO;
using System.Reflection;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class Utils
    {
        public static string ResolvePath(string relPath)
        {
            UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string absPath = Path.Combine(Path.GetDirectoryName(path), relPath);
            return absPath;
        }

        public static void WriteAllText(string relPath, string text)
        {
            string absPath = ResolvePath(relPath);
            File.WriteAllText(absPath, text);
        }

        public static string ReadAllText(string relPath)
        {
            string absPath = ResolvePath(relPath);
            return File.ReadAllText(absPath);
        }

        public static Stream OpenFile(string relPath)
        {
            string absPath = ResolvePath(relPath);
            return File.OpenRead(absPath);
        }

        public static Stream OpenTempWrite(string relPath)
        {
            string absPath = Path.Combine(Path.GetTempPath(), relPath);
            return File.OpenWrite(absPath);
        }
    }
}
