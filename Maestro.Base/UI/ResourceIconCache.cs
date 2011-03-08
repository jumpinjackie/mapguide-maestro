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
using System.Windows.Forms;
using System.Drawing;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace Maestro.Base.UI
{
    public interface IResourceIconCache
    {
        ImageList SmallImageList { get; }

        ImageList LargeImageList { get; }

        string GetImageKeyFromResourceID(string resourceId);

        int GetImageIndexFromResourceID(string resourceId);
    }

    public class ResourceIconCache : IResourceIconCache
    {
        private ImageList _small;
        private ImageList _large;

        private ResourceIconCache() 
        {
            _small = new ImageList();
            _large = new ImageList();
        }

        const string UNKNOWN = "UNKNOWN";

        public static ResourceIconCache CreateDefault()
        {
            var icons = new ResourceIconCache();

            //TODO: Externalize
            icons._small.Images.Add(ResourceTypes.DrawingSource.ToString(), Properties.Resources.blueprints);
            icons._small.Images.Add(ResourceTypes.FeatureSource.ToString(), Properties.Resources.database_share);
            icons._small.Images.Add(ResourceTypes.Folder.ToString(), Properties.Resources.folder_horizontal);
            icons._small.Images.Add(ResourceTypes.LayerDefinition.ToString(), Properties.Resources.layer);
            icons._small.Images.Add(ResourceTypes.MapDefinition.ToString(), Properties.Resources.map);
            icons._small.Images.Add(ResourceTypes.WebLayout.ToString(), Properties.Resources.application_browser);
            icons._small.Images.Add(ResourceTypes.ApplicationDefinition.ToString(), Properties.Resources.applications_stack);
            icons._small.Images.Add(ResourceTypes.SymbolLibrary.ToString(), Properties.Resources.images_stack);
            icons._small.Images.Add(ResourceTypes.PrintLayout.ToString(), Properties.Resources.printer);
            icons._small.Images.Add(Properties.Resources.document);

            icons._large.Images.Add(ResourceTypes.DrawingSource.ToString(), Properties.Resources.blueprints);
            icons._large.Images.Add(ResourceTypes.FeatureSource.ToString(), Properties.Resources.database_share);
            icons._large.Images.Add(ResourceTypes.Folder.ToString(), Properties.Resources.folder_horizontal);
            icons._large.Images.Add(ResourceTypes.LayerDefinition.ToString(), Properties.Resources.layer);
            icons._large.Images.Add(ResourceTypes.MapDefinition.ToString(), Properties.Resources.map);
            icons._large.Images.Add(ResourceTypes.WebLayout.ToString(), Properties.Resources.application_browser);
            icons._large.Images.Add(ResourceTypes.ApplicationDefinition.ToString(), Properties.Resources.applications_stack);
            icons._large.Images.Add(ResourceTypes.SymbolLibrary.ToString(), Properties.Resources.images_stack);
            icons._large.Images.Add(ResourceTypes.PrintLayout.ToString(), Properties.Resources.printer);
            icons._large.Images.Add(Properties.Resources.document);

            return icons;
        }

        public string GetImageKeyFromResourceID(string resourceId)
        {
            var rt = ResourceIdentifier.GetResourceType(resourceId);
            switch (rt)
            {
                case ResourceTypes.DrawingSource:
                case ResourceTypes.FeatureSource:
                case ResourceTypes.Folder:
                case ResourceTypes.LayerDefinition:
                case ResourceTypes.MapDefinition:
                case ResourceTypes.WebLayout:
                case ResourceTypes.ApplicationDefinition:
                case ResourceTypes.SymbolLibrary:
                case ResourceTypes.PrintLayout:
                    return rt.ToString();
                default:
                    return UNKNOWN;
            }
        }

        public int GetImageIndexFromResourceID(string resourceId)
        {
            int idx = _small.Images.IndexOfKey(ResourceIdentifier.GetResourceType(resourceId).ToString());

            if (idx < 0)
                return _small.Images.IndexOfKey(UNKNOWN);

            return idx;
        }

        public ImageList SmallImageList
        {
            get { return _small;  }
        }

        public ImageList LargeImageList
        {
            get { return _large; }
        }
    }
}
