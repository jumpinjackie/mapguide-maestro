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

#endregion Disclaimer / License

using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using System;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Preview
{
    /// <summary>
    /// Base class of any resource preview URL generator
    /// </summary>
    public abstract class ResourcePreviewUrlGenerator : IResourcePreviewUrlGenerator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ResourcePreviewUrlGenerator()
        {
            this.AddDebugWatermark = true;
            this.UseAjaxViewer = true;
        }

        /// <summary>
        /// Gets the locale
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        protected static string GetLocale(string locale)
        {
            return string.IsNullOrEmpty(locale) ? "en" : locale; //NOXLATE
        }

        /// <summary>
        /// Generates the appropriate viewer URL to preview the given resource under the given locale
        /// </summary>
        /// <param name="res"></param>
        /// <param name="locale"></param>
        /// <param name="isNew"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public string GeneratePreviewUrl(IResource res, string locale, bool isNew, string sessionID)
        {
            switch (res.ResourceType)
            {
                case nameof(ResourceTypes.FeatureSource):
                    return GenerateFeatureSourcePreviewUrl(res, locale, isNew, sessionID);

                case nameof(ResourceTypes.ApplicationDefinition):
                    return GenerateFlexLayoutPreviewUrl(res, locale, isNew, sessionID);

                case nameof(ResourceTypes.LayerDefinition):
                    return GenerateLayerPreviewUrl(res, locale, isNew, sessionID);

                case nameof(ResourceTypes.MapDefinition):
                    return GenerateMapPreviewUrl(res, locale, isNew, sessionID);

                case nameof(ResourceTypes.WebLayout):
                    return GenerateWebLayoutPreviewUrl(res, locale, isNew, sessionID);

                case nameof(ResourceTypes.WatermarkDefinition):
                    return GenerateWatermarkPreviewUrl((IWatermarkDefinition)res, locale, isNew, sessionID);

                default:
                    throw new InvalidOperationException(Strings.UnpreviewableResourceType);
            }
        }

        /// <summary>
        /// Generates a preview URL for watermarks
        /// </summary>
        /// <param name="watermarkDefinition"></param>
        /// <param name="locale"></param>
        /// <param name="isNew"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        protected abstract string GenerateWatermarkPreviewUrl(IWatermarkDefinition watermarkDefinition, string locale, bool isNew, string sessionID);

        /// <summary>
        /// Generates a preview URL for Web Layouts
        /// </summary>
        /// <param name="res"></param>
        /// <param name="locale"></param>
        /// <param name="isNew"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        protected abstract string GenerateWebLayoutPreviewUrl(IResource res, string locale, bool isNew, string sessionID);

        /// <summary>
        /// Generates a preview URL for Map Definitions
        /// </summary>
        /// <param name="res"></param>
        /// <param name="locale"></param>
        /// <param name="isNew"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        protected abstract string GenerateMapPreviewUrl(IResource res, string locale, bool isNew, string sessionID);

        /// <summary>
        /// Generates a preview URL for layer definitions
        /// </summary>
        /// <param name="res"></param>
        /// <param name="locale"></param>
        /// <param name="isNew"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        protected abstract string GenerateLayerPreviewUrl(IResource res, string locale, bool isNew, string sessionID);

        /// <summary>
        /// Generates a preview URL for Flexible Layouts
        /// </summary>
        /// <param name="res"></param>
        /// <param name="locale"></param>
        /// <param name="isNew"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        protected abstract string GenerateFlexLayoutPreviewUrl(IResource res, string locale, bool isNew, string sessionID);

        /// <summary>
        /// Generates a preview URL for Feature Sources
        /// </summary>
        /// <param name="res"></param>
        /// <param name="locale"></param>
        /// <param name="isNew"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        protected abstract string GenerateFeatureSourcePreviewUrl(IResource res, string locale, bool isNew, string sessionID);

        /// <summary>
        /// Indicates if the given resource type is previewable
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public abstract bool IsPreviewableType(string resourceType);

        /// <summary>
        /// Gets or sets whether to use the AJAX viewer for previews
        /// </summary>
        public bool UseAjaxViewer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether to insert a debugging watermark for generated previews
        /// </summary>
        public bool AddDebugWatermark
        {
            get;
            set;
        }
    }
}