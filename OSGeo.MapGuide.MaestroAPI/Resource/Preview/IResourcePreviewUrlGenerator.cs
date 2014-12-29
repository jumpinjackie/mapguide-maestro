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

namespace OSGeo.MapGuide.MaestroAPI.Resource.Preview
{
    /// <summary>
    /// Generates preview URLs for any given resource
    /// </summary>
    public interface IResourcePreviewUrlGenerator
    {
        /// <summary>
        /// Gets or sets whether to use the AJAX viewer for previewing
        /// </summary>
        bool UseAjaxViewer { get; set; }

        /// <summary>
        /// Gets or sets whether to insert a debug watermark in the previewed resource
        /// </summary>
        bool AddDebugWatermark { get; set; }

        /// <summary>
        /// Generates a preview URL for the given resource
        /// </summary>
        /// <param name="res"></param>
        /// <param name="locale"></param>
        /// <param name="isNew"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        string GeneratePreviewUrl(IResource res, string locale, bool isNew, string sessionID);

        /// <summary>
        /// Gets whether the given resource type is previewable
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        bool IsPreviewableType(string resourceType);
    }
}