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

namespace System.Drawing
{
    /// <summary>
    /// System.Drawing DTO conversion helper
    /// </summary>
    public static class FontUtils
    {
        /// <summary>
        /// Converts <see cref="System.Drawing.Font"/> object to <see cref="OSGeo.MapGuide.ObjectModels.FontInfo"/> objects
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static OSGeo.MapGuide.ObjectModels.FontInfo ToFontInfo(this Font font)
        {
            return new OSGeo.MapGuide.ObjectModels.FontInfo
            {
                Name = font.Name,
                Bold = font.Bold,
                Italic = font.Italic,
                Underline = font.Underline
            };
        }
    }
}

namespace OSGeo.MapGuide.MaestroAPI.Services
{
    /// <summary>
    /// <see cref="IMappingService"/> polyfill extension methods for previous public APIs in the Maestro API
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Renders a minature bitmap of the layers style
        /// </summary>
        /// <param name="service"></param>
        /// <param name="scale">The scale for the bitmap to match</param>
        /// <param name="layerdefinition">The layer definition resource id</param>
        /// <param name="themeIndex">If the layer is themed, this gives the theme index, otherwise set to 0</param>
        /// <param name="type">The geometry type, 1 for point, 2 for line, 3 for area, 4 for composite</param>
        /// <returns>The legend image</returns>
        public static System.Drawing.Image GetLegendImage(this IMappingService service, double scale, string layerdefinition, int themeIndex, int type)
        {
            var stream = service.GetLegendImageStream(scale, layerdefinition, themeIndex, type);
            return System.Drawing.Image.FromStream(stream);
        }

        /// <summary>
        /// Renders a minature bitmap of the layers style
        /// </summary>
        /// <param name="service"></param>
        /// <param name="scale">The scale for the bitmap to match</param>
        /// <param name="layerdefinition">The layer definition resource id</param>
        /// <param name="themeIndex">If the layer is themed, this gives the theme index, otherwise set to 0</param>
        /// <param name="type">The geometry type, 1 for point, 2 for line, 3 for area, 4 for composite</param>
        /// <param name="width">The width of the image to request.</param>
        /// <param name="height">The height of the image to request.</param>
        /// <param name="format">The image format (PNG, JPG or GIF).</param>
        /// <returns>The legend image</returns>
        public static System.Drawing.Image GetLegendImage(this IMappingService service, double scale, string layerdefinition, int themeIndex, int type, int width, int height, string format)
        {
            var stream = service.GetLegendImageStream(scale, layerdefinition, themeIndex, type, width, height, format);
            return System.Drawing.Image.FromStream(stream);
        }
    }
}

namespace OSGeo.MapGuide.MaestroAPI.Mapping
{
    /// <summary>
    /// <see cref="RuntimeMap"/> polyfill extension methods for previous public APIs in the Maestro API
    /// </summary>
    public static class RuntimeMapExtensions
    {
        public static System.Drawing.Image GetLegendImage(this RuntimeMap map, string layerDefinitionID, double scale, int width, int height, string format, int geomType, int themeCategory)
        {
            var stream = map.GetLegendImageStream(layerDefinitionID, scale, width, height, format, geomType, themeCategory);
            return System.Drawing.Image.FromStream(stream);
        }
    }
}