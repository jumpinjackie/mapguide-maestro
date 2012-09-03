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

namespace Maestro.Editors.LayerDefinition.Vector
{
    internal interface ILayerStylePreviewable
    {
        string LayerDefinition { get; }

        double Scale { get; }

        int Width { get; }

        int Height { get; }

        string ImageFormat { get; }

        int ThemeCategory { get; }
    }

    internal class LayerStylePreviewable : ILayerStylePreviewable
    {
        public LayerStylePreviewable(string layerDefinition, double scale, int width, int height, string imgFormat, int themeCat)
        {
            this.LayerDefinition = layerDefinition;
            this.Scale = scale;
            this.Width = width;
            this.Height = height;
            this.ImageFormat = imgFormat;
            this.ThemeCategory = themeCat;
        }

        public string LayerDefinition
        {
            get;
            private set;
        }

        public double Scale
        {
            get;
            private set;
        }

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public string ImageFormat
        {
            get;
            private set;
        }

        public int ThemeCategory
        {
            get;
            private set;
        }
    }
}
