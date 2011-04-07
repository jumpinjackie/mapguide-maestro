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
using Maestro.Editors;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Base.UI.Preferences;
using ICSharpCode.Core;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;

namespace Maestro.Base.Editor
{
    internal class ResourcePreviewEngine
    {
        private string _rootUrl;
        private IEditorService _edSvc;

        public ResourcePreviewEngine(string rootUrl, IEditorService edSvc)
        {
            _rootUrl = rootUrl;
            _edSvc = edSvc;
        }

        private string GenerateFeatureSourcePreviewUrl(IResource res)
        {
            string url = _rootUrl;
            if (!url.EndsWith("/"))
                url += "/";

            var resId = res.ResourceID;
            var sessionId = _edSvc.SessionID;
            url += "schemareport/describeschema.php?viewer=basic&schemaName=&className=&resId=" + resId + "&sessionId=" + sessionId;

            return url;
        }

        private string GenerateLayerPreviewUrl(IResource res)
        {
            string url = _rootUrl;
            if (!url.EndsWith("/"))
                url += "/";

            var ldf = (ILayerDefinition)res;
            var sessionId = _edSvc.SessionID;
            var conn = res.CurrentConnection;

            //Create temp map definition to house our current layer
            var mdfId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".MapDefinition";
            var extent = ldf.GetSpatialExtent(true);

            //TODO: Based on the visible scales in this layer, size this extents accordingly
            var mdf = ObjectFactory.CreateMapDefinition(conn, Properties.Resources.PreviewMap, ldf.GetCoordinateSystemWkt(), extent);

            mdf.AddLayer(null, ResourceIdentifier.GetName(ldf.ResourceID), ldf.ResourceID);

            conn.ResourceService.SaveResourceAs(mdf, mdfId);

            if (PropertyService.Get(ConfigProperties.PreviewViewerType, "AJAX").Equals("AJAX"))
            {
                //Create temp web layout to house this map
                var wl = ObjectFactory.CreateWebLayout(_edSvc.GetEditedResource().CurrentConnection, new Version(1, 0, 0), mdfId);

                //Add a custom zoom command (to assist previews of layers that aren't [0, infinity] scale)
                AttachZoomToScaleCommand(wl);

                var resId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".WebLayout";

                conn.ResourceService.SaveResourceAs(wl, resId);
                url += "mapviewerajax/?WEBLAYOUT=" + resId + "&SESSION=" + sessionId;
            }
            else
            {
                throw new NotImplementedException();
                ////Create temp flex layout
                //var appDef = ObjectFactory.CreatePreviewFlexLayout(conn);
                //var resId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".ApplicationDefinition";
                //appDef.AddMapGroup("previewmap", true, mdfId);

                //conn.ResourceService.SaveResourceAs(appDef, resId);
                //url += "fusion/templates/mapguide/preview/index.html?Session=" + sessionId + "&ApplicationDefinition=" + resId;
            }

            return url;
        }

        private static void AttachZoomToScaleCommand(IWebLayout wl)
        {
            var cmd = wl.CreateInvokeScriptCommand();
            cmd.Name = "ZoomScale";
            cmd.Label = "Zoom to Scale"; //LOCALIZEME
            cmd.Tooltip = "Zoom to a specified scale";
            cmd.Script = @"
                var map = parent.parent.GetMapFrame();
                var center = map.GetCenter();
                var scale = parseFloat(prompt('Enter the scale:'));
                map.ZoomToView(center.X, center.Y, scale, true);
                ";

            cmd.TargetViewer = TargetViewerType.Ajax;
            cmd.ImageURL = "../stdicons/icon_zoom.gif";

            wl.CommandSet.AddCommand(cmd);

            var item = wl.CreateCommandItem(cmd.Name);
            wl.ToolBar.AddItem(item);
        }

        private string GenerateMapPreviewUrl(IResource res)
        {
            string url = _rootUrl;
            if (!url.EndsWith("/"))
                url += "/";

            var sessionId = _edSvc.SessionID;
            var mdfId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".MapDefinition";
            var mdf = (IMapDefinition)res;

            var conn = mdf.CurrentConnection;
            conn.ResourceService.SaveResourceAs(mdf, mdfId);

            if (PropertyService.Get(ConfigProperties.PreviewViewerType, "AJAX").Equals("AJAX"))
            {
                //Create temp web layout to house this map
                var wl = ObjectFactory.CreateWebLayout(_edSvc.GetEditedResource().CurrentConnection, new Version(1, 0, 0), mdfId);

                //Add a custom zoom command (to assist previews of layers that aren't [0, infinity] scale)
                AttachZoomToScaleCommand(wl);

                var resId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".WebLayout";

                conn.ResourceService.SaveResourceAs(wl, resId);
                url += "mapviewerajax/?WEBLAYOUT=" + resId + "&SESSION=" + sessionId;
            }
            else
            {
                throw new NotImplementedException();
                ////Create temp flex layout
                //var appDef = ObjectFactory.CreateFlexibleLayout(conn);
                //var resId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".ApplicationDefinition";
                //appDef.AddMapGroup("previewmap", true, mdfId);

                //conn.ResourceService.SaveResourceAs(appDef, resId);
                //url += "fusion/templates/mapguide/preview/index.html?Session=" + sessionId + "&ApplicationDefinition=" + resId;
            }

            return url;
        }

        private string GenerateWebLayoutPreviewUrl(IResource res)
        {
            string url = _rootUrl;
            if (!url.EndsWith("/"))
                url += "/";

            var sessionId = _edSvc.SessionID;
            var resId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".WebLayout";
            var wl = (IWebLayout)res;
            var conn = wl.CurrentConnection;

            conn.ResourceService.SaveResourceAs(wl, resId);
            url += "mapviewerajax/?WEBLAYOUT=" + resId + "&SESSION=" + sessionId;

            return url;
        }

        private string GenerateFlexLayoutPreviewUrl(IResource res)
        {
            string url = _rootUrl;
            if (!url.EndsWith("/"))
                url += "/";

            //Create temp flex layout
            var sessionId = _edSvc.SessionID;
            var appDef = (IApplicationDefinition)res;
            var conn = appDef.CurrentConnection;
            var resId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".ApplicationDefinition";
            
            conn.ResourceService.SaveResourceAs(appDef, resId);
            url += appDef.TemplateUrl + "?Session=" + sessionId + "&ApplicationDefinition=" + resId;
            return url;
        }

        public string GeneratePreviewUrl(IResource res)
        {
            switch (res.ResourceType)
            {
                case ResourceTypes.FeatureSource:
                    return GenerateFeatureSourcePreviewUrl(res);
                case ResourceTypes.ApplicationDefinition:
                    return GenerateFlexLayoutPreviewUrl(res);
                case ResourceTypes.LayerDefinition:
                    return GenerateLayerPreviewUrl(res);
                case ResourceTypes.MapDefinition:
                    return GenerateMapPreviewUrl(res);
                case ResourceTypes.WebLayout:
                    return GenerateWebLayoutPreviewUrl(res);
                default:
                    throw new InvalidOperationException(Properties.Resources.UnpreviewableResourceType);
            }
        }

        private static ResourceTypes[] PREVIEWABLE_RESOURCE_TYPES = new ResourceTypes[] 
        {
            ResourceTypes.FeatureSource,
            ResourceTypes.ApplicationDefinition,
            ResourceTypes.LayerDefinition,
            ResourceTypes.MapDefinition,
            ResourceTypes.WebLayout
        };

        internal static bool IsPreviewableType(OSGeo.MapGuide.MaestroAPI.ResourceTypes rt)
        {
            return Array.IndexOf(PREVIEWABLE_RESOURCE_TYPES, rt) >= 0;
        }
    }
}
