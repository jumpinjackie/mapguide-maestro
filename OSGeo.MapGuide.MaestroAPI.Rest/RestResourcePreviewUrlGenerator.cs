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
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Resource.Preview;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.Rest
{
    class RestResourcePreviewUrlGenerator : ResourcePreviewUrlGenerator
    {
        private RestConnection _conn;

        public RestResourcePreviewUrlGenerator(RestConnection conn)
        {
            _conn = conn;
        }

        private string GetSuffix(string resourceID, string sessionID)
        {
            ResourceIdentifier ri = new ResourceIdentifier(resourceID);
            if (ri.IsInLibrary)
            {
                return "/library/" + ri.Path + "." + ri.ResourceType;
            }
            else
            {
                return "/session/" + sessionID + "/" + ri.Path + "." + ri.ResourceType;
            }
        }

        protected override string GenerateWatermarkPreviewUrl(ObjectModels.WatermarkDefinition.IWatermarkDefinition res, string locale, bool isNew, string sessionID)
        {
            var url = _conn.RestRootUrl + GetSuffix(res.ResourceID, sessionID) + "/preview";
            return url;
        }

        protected override string GenerateWebLayoutPreviewUrl(Resource.IResource res, string locale, bool isNew, string sessionID)
        {
            var url = _conn.RestRootUrl + GetSuffix(res.ResourceID, sessionID) + "/preview";
            return url;
        }

        protected override string GenerateMapPreviewUrl(Resource.IResource res, string locale, bool isNew, string sessionID)
        {
            var url = _conn.RestRootUrl + GetSuffix(res.ResourceID, sessionID) + "/preview";
            return url;
        }

        protected override string GenerateLayerPreviewUrl(Resource.IResource res, string locale, bool isNew, string sessionID)
        {
            var url = _conn.RestRootUrl + GetSuffix(res.ResourceID, sessionID) + "/viewer";
            return url;
        }

        protected override string GenerateFlexLayoutPreviewUrl(Resource.IResource res, string locale, bool isNew, string sessionID)
        {
            var flexLayout = (IApplicationDefinition)res;
            var url = _conn.RestRootUrl + GetSuffix(res.ResourceID, sessionID) + "/viewer/" + flexLayout.GetTemplateName();
            return url;
        }

        protected override string GenerateFeatureSourcePreviewUrl(Resource.IResource res, string locale, bool isNew, string sessionID)
        {
            var url = _conn.RestRootUrl + GetSuffix(res.ResourceID, sessionID) + "/preview";
            return url;
        }

        private static string[] PREVIEWABLE_RESOURCE_TYPES = new string[] 
        {
            ResourceTypes.FeatureSource.ToString(),
            ResourceTypes.ApplicationDefinition.ToString(),
            ResourceTypes.LayerDefinition.ToString(),
            ResourceTypes.MapDefinition.ToString(),
            ResourceTypes.WebLayout.ToString(),
            ResourceTypes.WatermarkDefinition.ToString()
        };

        public override bool IsPreviewableType(string resourceType)
        {
            return Array.IndexOf(PREVIEWABLE_RESOURCE_TYPES, resourceType) >= 0;
        }
    }
}
