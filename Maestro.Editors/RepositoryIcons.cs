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
using OSGeo.MapGuide.MaestroAPI;
using System.Windows.Forms;

namespace Maestro.Editors
{
    /// <summary>
    /// Image list helper class for components that display or interact with Repository resources
    /// </summary>
    public class RepositoryIcons
    {
        /// <summary>
        /// Icon for resource types
        /// </summary>
        public const int RES_UNKNOWN = 0;
        /// <summary>
        /// Icon for feature sources
        /// </summary>
        public const int RES_FEATURESOURCE = 1;
        /// <summary>
        /// Icon for layer definitions
        /// </summary>
        public const int RES_LAYERDEFINITION = 2;
        /// <summary>
        /// Icon for map definitions
        /// </summary>
        public const int RES_MAPDEFINITION = 3;
        /// <summary>
        /// Icon for web layouts
        /// </summary>
        public const int RES_WEBLAYOUT = 4;
        /// <summary>
        /// Icon for symbol libraries
        /// </summary>
        public const int RES_SYMBOLLIBRARY = 5;
        /// <summary>
        /// Icon for print layouts
        /// </summary>
        public const int RES_PRINTLAYOUT = 6;
        /// <summary>
        /// Icon for drawing sources
        /// </summary>
        public const int RES_DRAWINGSOURCE = 7;
        /// <summary>
        /// Icon for application definitions
        /// </summary>
        public const int RES_APPLICATIONDEFINITION = 8;
        /// <summary>
        /// Icon for symbol definitions
        /// </summary>
        public const int RES_SYMBOLDEFINITION = 9;
        /// <summary>
        /// Icon for watermark definitions
        /// </summary>
        public const int RES_WATERMARK = 10;
        /// <summary>
        /// Icon for load procedures
        /// </summary>
        public const int RES_LOADPROCEDURE = 11;

        /// <summary>
        /// Icon for the root of the repository
        /// </summary>
        public const int RES_ROOT = 12;
        /// <summary>
        /// Icon for folders
        /// </summary>
        public const int RES_FOLDER = 13;

        /// <summary>
        /// Gets the image icon index for the given resource type
        /// </summary>
        /// <param name="resType">Type of the resource.</param>
        /// <returns></returns>
        public static int GetImageIndexForResourceType(ResourceTypes resType)
        {
            switch (resType)
            {
                case ResourceTypes.ApplicationDefinition:
                    return RES_APPLICATIONDEFINITION;
                case ResourceTypes.DrawingSource:
                    return RES_DRAWINGSOURCE;
                case ResourceTypes.FeatureSource:
                    return RES_FEATURESOURCE;
                case ResourceTypes.Folder:
                    return RES_FOLDER;
                case ResourceTypes.LayerDefinition:
                    return RES_LAYERDEFINITION;
                case ResourceTypes.LoadProcedure:
                    return RES_LOADPROCEDURE;
                case ResourceTypes.MapDefinition:
                    return RES_MAPDEFINITION;
                case ResourceTypes.PrintLayout:
                    return RES_PRINTLAYOUT;
                case ResourceTypes.SymbolDefinition:
                    return RES_SYMBOLDEFINITION;
                case ResourceTypes.SymbolLibrary:
                    return RES_SYMBOLLIBRARY;
                case ResourceTypes.WatermarkDefinition:
                    return RES_WATERMARK;
                case ResourceTypes.WebLayout:
                    return RES_WEBLAYOUT;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Creates the image list.
        /// </summary>
        /// <returns></returns>
        public static ImageList CreateImageList()
        {
            var imgList = new ImageList();

            PopulateImageList(imgList);

            return imgList;
        }

        /// <summary>
        /// Populates the image list.
        /// </summary>
        /// <param name="imgList">The img list.</param>
        public static void PopulateImageList(ImageList imgList)
        {
            imgList.Images.Add(Properties.Resources.document);
            imgList.Images.Add(Properties.Resources.database_share);
            imgList.Images.Add(Properties.Resources.layer);
            imgList.Images.Add(Properties.Resources.map);
            imgList.Images.Add(Properties.Resources.application_browser);
            imgList.Images.Add(Properties.Resources.images_stack);
            imgList.Images.Add(Properties.Resources.printer);
            imgList.Images.Add(Properties.Resources.blueprints);
            imgList.Images.Add(Properties.Resources.applications_stack);
            imgList.Images.Add(Properties.Resources.marker);
            imgList.Images.Add(Properties.Resources.water);
            imgList.Images.Add(Properties.Resources.script__arrow);
            imgList.Images.Add(Properties.Resources.server);
            imgList.Images.Add(Properties.Resources.folder_horizontal);
        }
    }
}
