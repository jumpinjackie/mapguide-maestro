#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.Reflection;
using Maestro.Editors.Common;

namespace Maestro.Editors.LayerDefinition.Vector.StyleEditors
{
    internal static class StyleImageCache
    {
        private static Assembly _asm;

        static StyleImageCache()
        {
            _asm = Assembly.GetExecutingAssembly();
        }

        private static ImageStylePicker.NamedImage[] _areas;

        public static ImageStylePicker.NamedImage[] FillImages
        {
            get
            {
                if (_areas == null)
                {
                    List<ImageStylePicker.NamedImage> images = new List<ImageStylePicker.NamedImage>();

                    foreach (string name in _asm.GetManifestResourceNames())
                    {
                        if (name.StartsWith("Maestro.Editors.Resources.area"))
                        {
                            string[] tokens = name.Split('.');
                            string n = tokens[tokens.Length - 2]; // +"." + tokens[tokens.Length - 1];
                            var img = System.Drawing.Image.FromStream(_asm.GetManifestResourceStream(name));
                            images.Add(new ImageStylePicker.NamedImage(n, img));
                        }
                    }
                    images.Sort(CompareImages);
                    _areas = images.ToArray();
                }
                return _areas;
            }
        }

        static int CompareImages(ImageStylePicker.NamedImage x, ImageStylePicker.NamedImage y)
        {
            return x.Name.CompareTo(y.Name);
        }

        private static ImageStylePicker.NamedImage[] _lines;

        public static ImageStylePicker.NamedImage[] LineStyles
        {
            get
            {
                if (_lines == null)
                {
                    List<ImageStylePicker.NamedImage> images = new List<ImageStylePicker.NamedImage>();

                    foreach (string name in _asm.GetManifestResourceNames())
                    {
                        if (name.StartsWith("Maestro.Editors.Resources.line"))
                        {
                            string[] tokens = name.Split('.');
                            string n = tokens[tokens.Length - 2]; // +"." + tokens[tokens.Length - 1];
                            var img = System.Drawing.Image.FromStream(_asm.GetManifestResourceStream(name));
                            images.Add(new ImageStylePicker.NamedImage(n, img));
                        }
                    }
                    images.Sort(CompareImages);
                    _lines = images.ToArray();
                }
                return _lines;
            }
        }
    }
}
