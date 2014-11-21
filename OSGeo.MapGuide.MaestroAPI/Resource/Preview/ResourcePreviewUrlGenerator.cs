#region Disclaimer / License
// Copyright (C) 2014, Jackie Ng
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
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Preview
{
    public abstract class ResourcePreviewUrlGenerator : IResourcePreviewUrlGenerator
    {
        protected ResourcePreviewUrlGenerator()
        {
            this.AddDebugWatermark = true;
            this.UseAjaxViewer = true;
        }

        protected static string GetLocale(string locale)
        {
            return string.IsNullOrEmpty(locale) ? "en" : locale; //NOXLATE
        }

        /// <summary>
        /// Generates the appropriate viewer URL to preview the given resource under the given locale
        /// </summary>
        /// <param name="res"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public string GeneratePreviewUrl(IResource res, string locale, bool isNew, string sessionID)
        {
            switch (res.ResourceType)
            {
                case "FeatureSource":
                    return GenerateFeatureSourcePreviewUrl(res, locale, isNew, sessionID);
                case "ApplicationDefinition":
                    return GenerateFlexLayoutPreviewUrl(res, locale, isNew, sessionID);
                case "LayerDefinition":
                    return GenerateLayerPreviewUrl(res, locale, isNew, sessionID);
                case "MapDefinition":
                    return GenerateMapPreviewUrl(res, locale, isNew, sessionID);
                case "WebLayout":
                    return GenerateWebLayoutPreviewUrl(res, locale, isNew, sessionID);
                case "WatermarkDefinition":
                    return GenerateWatermarkPreviewUrl((IWatermarkDefinition)res, locale, isNew, sessionID);
                default:
                    throw new InvalidOperationException(Strings.UnpreviewableResourceType);
            }
        }

        protected abstract string GenerateWatermarkPreviewUrl(IWatermarkDefinition watermarkDefinition, string locale, bool isNew, string sessionID);

        protected abstract string GenerateWebLayoutPreviewUrl(IResource res, string locale, bool isNew, string sessionID);

        protected abstract string GenerateMapPreviewUrl(IResource res, string locale, bool isNew, string sessionID);

        protected abstract string GenerateLayerPreviewUrl(IResource res, string locale, bool isNew, string sessionID);

        protected abstract string GenerateFlexLayoutPreviewUrl(IResource res, string locale, bool isNew, string sessionID);

        protected abstract string GenerateFeatureSourcePreviewUrl(IResource res, string locale, bool isNew, string sessionID);

        public abstract bool IsPreviewableType(string resourceType);

        public bool UseAjaxViewer
        {
            get;
            set;
        }

        public bool AddDebugWatermark
        {
            get;
            set;
        }
    }
}
