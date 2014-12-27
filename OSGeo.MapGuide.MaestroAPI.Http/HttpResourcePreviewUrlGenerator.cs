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

using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Resource.Preview;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    internal class HttpResourcePreviewUrlGenerator : ResourcePreviewUrlGenerator
    {
        private string _rootUrl;
        private HttpServerConnection _conn;

        public HttpResourcePreviewUrlGenerator(HttpServerConnection conn, string rootUrl)
        {
            _conn = conn;
            _rootUrl = rootUrl;
        }

        protected override string GenerateWatermarkPreviewUrl(ObjectModels.WatermarkDefinition.IWatermarkDefinition wmd, string locale, bool isNew, string sessionID)
        {
            //We demand a 2.3.0 Map Definition or higher
            if (_conn.SiteVersion < new Version(2, 3))
                throw new InvalidOperationException(Strings.SiteVersionDoesntSupportWatermarks);

            IMapDefinition2 map = Utility.CreateWatermarkPreviewMapDefinition(wmd);
            return _GenerateMapPreviewUrl(map, locale, isNew, sessionID, false);
        }

        protected string GetRootUrl()
        {
            string url = _rootUrl;
            if (url.EndsWith("mapagent.fcgi"))
            {
                string[] tokens = url.Split('/');
                url = string.Join("/", tokens.Take(tokens.Length - 2).ToArray());
            }
            if (!url.EndsWith("/")) //NOXLATE
                url += "/"; //NOXLATE

            return url;
        }

        protected override string GenerateWebLayoutPreviewUrl(IResource res, string locale, bool isNew, string sessionID)
        {
            string url = GetRootUrl();

            var resId = "Session:" + sessionID + "//" + Guid.NewGuid() + ".WebLayout"; //NOXLATE
            
            _conn.ResourceService.SaveResourceAs(res, resId);
            url += "mapviewerajax/?WEBLAYOUT=" + resId + "&SESSION=" + sessionID + "&LOCALE=" + GetLocale(locale); //NOXLATE

            return url;
        }

        private string _GenerateMapPreviewUrl(IResource res, string locale, bool isNew, string sessionID, bool addDebugWatermark)
        {
            string url = GetRootUrl();

            var mdfId = "Session:" + sessionID + "//" + Guid.NewGuid() + ".MapDefinition"; //NOXLATE
            var mdf = res as IMapDefinition;
            if (mdf != null)
            {
                IMapDefinition2 mdf2 = mdf as IMapDefinition2;
                if (mdf2 != null && addDebugWatermark)
                    CreateDebugWatermark(mdf2, _conn, null);
            }
            _conn.ResourceService.SaveResourceAs(res, mdfId);
            //if (PropertyService.Get(ConfigProperties.PreviewViewerType, "AJAX").Equals("AJAX")) //NOXLATE
            if (this.UseAjaxViewer)
            {
                //Create temp web layout to house this map
                var wl = ObjectFactory.CreateWebLayout(new Version(1, 0, 0), mdfId);

                //Add a custom zoom command (to assist previews of layers that aren't [0, infinity] scale)
                AttachPreviewCommands(wl);

                var resId = "Session:" + sessionID + "//" + Guid.NewGuid() + ".WebLayout"; //NOXLATE

                _conn.ResourceService.SaveResourceAs(wl, resId);
                url += "mapviewerajax/?WEBLAYOUT=" + resId + "&SESSION=" + sessionID + "&LOCALE=" + GetLocale(locale); //NOXLATE
            }
            else
            {
                throw new NotImplementedException();
                ////Create temp flex layout
                //var appDef = ObjectFactory.CreateFlexibleLayout(conn);
                //var resId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".ApplicationDefinition"; //NOXLATE
                //appDef.AddMapGroup("previewmap", true, mdfId); //NOXLATE

                //conn.ResourceService.SaveResourceAs(appDef, resId);
                //url += "fusion/templates/mapguide/preview/index.html?Session=" + sessionId + "&ApplicationDefinition=" + resId; //NOXLATE
            }

            return url;
        }

        protected override string GenerateMapPreviewUrl(IResource res, string locale, bool isNew, string sessionID)
        {
            return _GenerateMapPreviewUrl(res, locale, isNew, sessionID, this.AddDebugWatermark);
        }

        private IMapDefinition CreateLayerPreviewMapDefinition(ILayerDefinition ldf, string sessionId, string layerName, IServerConnection conn)
        {
            //Create temp map definition to house our current layer
            var mdfId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".MapDefinition"; //NOXLATE
            string csWkt;
            var extent = ldf.GetSpatialExtent(_conn, true, out csWkt);
            if (extent == null)
                throw new ApplicationException(Strings.FailedToCalculateFeatureSourceExtents);

            string layerSc = ldf.GetLayerSpatialContextName(_conn);

            //TODO: Based on the visible scales in this layer, size this extents accordingly
            var mdf = Utility.CreateMapDefinition(conn, Strings.PreviewMap, csWkt, extent);
            IMapDefinition2 mdf2 = mdf as IMapDefinition2;
            if (mdf2 != null && this.AddDebugWatermark)
                CreateDebugWatermark(mdf2, conn, layerSc);

            var layer = mdf.AddLayer(null, layerName, ldf.ResourceID);
            conn.ResourceService.SaveResourceAs(mdf, mdfId);
            mdf.ResourceID = mdfId;
            return mdf;
        }

        protected override string GenerateLayerPreviewUrl(IResource res, string locale, bool isNew, string sessionID)
        {
            string url = GetRootUrl();

            var ldf = (ILayerDefinition)res;

            //Use feature source as name/label
            string layerName = ResourceIdentifier.GetName(ldf.SubLayer.ResourceId);

            var mdf = CreateLayerPreviewMapDefinition(ldf, sessionID, layerName, _conn);
            //if (PropertyService.Get(ConfigProperties.PreviewViewerType, "AJAX").Equals("AJAX")) //NOXLATE
            if (this.UseAjaxViewer)
            {
                //Create temp web layout to house this map
                var wl = ObjectFactory.CreateWebLayout(new Version(1, 0, 0), mdf.ResourceID);

                //Add a custom zoom command (to assist previews of layers that aren't [0, infinity] scale)
                AttachPreviewCommands(wl);

                var resId = "Session:" + sessionID + "//" + Guid.NewGuid() + ".WebLayout"; //NOXLATE

                _conn.ResourceService.SaveResourceAs(wl, resId);
                url += "mapviewerajax/?WEBLAYOUT=" + resId + "&SESSION=" + sessionID + "&LOCALE=" + GetLocale(locale); //NOXLATE
            }
            else
            {
                throw new NotImplementedException();
                ////Create temp flex layout
                //var appDef = ObjectFactory.CreatePreviewFlexLayout(conn);
                //var resId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".ApplicationDefinition"; //NOXLATE
                //appDef.AddMapGroup("previewmap", true, mdfId); //NOXLATE

                //conn.ResourceService.SaveResourceAs(appDef, resId);
                //url += "fusion/templates/mapguide/preview/index.html?Session=" + sessionId + "&ApplicationDefinition=" + resId; //NOXLATE
            }

            return url;
        }

        protected override string GenerateFlexLayoutPreviewUrl(IResource res, string locale, bool isNew, string sessionID)
        {
            string url = GetRootUrl();

            //Create temp flex layout
            var appDef = (IApplicationDefinition)res;
            var resId = "Session:" + sessionID + "//" + Guid.NewGuid() + ".ApplicationDefinition"; //NOXLATE

            _conn.ResourceService.SaveResourceAs(appDef, resId);
            url += appDef.TemplateUrl + "?Session=" + sessionID + "&ApplicationDefinition=" + resId + "&locale=" + GetLocale(locale); //NOXLATE
            return url;
        }

        protected override string GenerateFeatureSourcePreviewUrl(IResource res, string locale, bool isNew, string sessionID)
        {
            string url = GetRootUrl();

            var resId = res.ResourceID;
            url += "schemareport/describeschema.php?viewer=basic&schemaName=&className=&resId=" + resId + "&sessionId=" + sessionID + "&locale=" + GetLocale(locale); //NOXLATE

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

        private static void AttachPreviewCommands(IWebLayout wl)
        {
            var cmd = wl.CreateInvokeScriptCommand();
            cmd.Name = "ZoomScale"; //NOXLATE
            cmd.Label = Strings.Label_ZoomToScale;
            cmd.Tooltip = Strings.Desc_ZoomToScale;
            cmd.Script = @"
                var map = parent.parent.GetMapFrame();
                var center = map.GetCenter();
                var scale = parseFloat(prompt('Enter the scale:'));
                map.ZoomToView(center.X, center.Y, scale, true);
                "; //NOXLATE

            cmd.TargetViewer = TargetViewerType.Ajax;
            cmd.ImageURL = "../stdicons/icon_zoom.gif"; //NOXLATE

            wl.CommandSet.AddCommand(cmd);

            var cmd2 = wl.CreateInvokeScriptCommand();
            cmd2.Name = "GetMapKml"; //NOXLATE
            cmd2.Label = Strings.Label_GetMapKml;
            cmd2.Description = Strings.Desc_GetMapKml;
            cmd2.Tooltip = Strings.Desc_GetMapKml;

            cmd2.Script = @"
                var map = parent.parent.GetMapFrame();
                var url = ""../mapagent/mapagent.fcgi?OPERATION=GETMAPKML&VERSION=1.0.0&FORMAT=KML&DISPLAYDPI=96&MAPDEFINITION=" + wl.Map.ResourceId + @""";
                url += ""&SESSION="" + map.GetSessionId();
                window.open(url);"; //NOXLATE

            cmd2.TargetViewer = TargetViewerType.Ajax;
            cmd2.ImageURL = "../stdicons/icon_invokescript.gif"; //NOXLATE

            wl.CommandSet.AddCommand(cmd2);

            var cmd3 = wl.CreateInvokeScriptCommand();
            cmd3.Name = "GetExtents"; //NOXLATE
            cmd3.Label = Strings.Label_GetExtents;
            cmd3.Description = Strings.Desc_GetExtents;
            cmd3.Tooltip = Strings.Desc_GetExtents;

            cmd3.Script = @"
                var map = parent.parent.GetMapFrame();
                alert('Map Extents\n\nLower Left: ' + map.extX1 + ', ' + map.extY2 + '\nUpper Right: ' + map.extX2 + ', ' + map.extY1);
                "; //NOXLATE

            cmd3.TargetViewer = TargetViewerType.Ajax;
            cmd3.ImageURL = "../stdicons/icon_invokescript.gif"; //NOXLATE

            wl.CommandSet.AddCommand(cmd3);

            var zoomScale = wl.CreateCommandItem(cmd.Name);
            var menu = wl.CreateFlyout(Strings.Label_Tools, Strings.Label_Tools, Strings.Label_ExtraTools, string.Empty, string.Empty,
                wl.CreateCommandItem("ZoomScale"), //NOXLATE
                wl.CreateCommandItem("GetMapKml"), //NOXLATE
                wl.CreateCommandItem("GetExtents") //NOXLATE
            );
            wl.ToolBar.AddItem(menu);
        }

        private static string CreateDebugWatermark(IMapDefinition2 mdf, IServerConnection conn, string layerSc)
        {
            //Tidy up the CS WKT so that it can display nicely in a watermark
            StringBuilder cleanCs = new StringBuilder(mdf.CoordinateSystem);
            cleanCs.Replace("[", "[\n");
            cleanCs.Replace("],", "],\n");

            string message = null;

            if (!string.IsNullOrEmpty(layerSc))
            {
                message = string.Format(Strings.DebugWatermarkMessageLayer,
                    mdf.Extents.MinX,
                    mdf.Extents.MinY,
                    mdf.Extents.MaxX,
                    mdf.Extents.MaxY,
                    cleanCs.ToString(),
                    layerSc);
            }
            else
            {
                message = string.Format(Strings.DebugWatermarkMessage,
                    mdf.Extents.MinX,
                    mdf.Extents.MinY,
                    mdf.Extents.MaxX,
                    mdf.Extents.MaxY,
                    cleanCs.ToString());
            }
            string watermarkXml = string.Format(Files.TextWatermark, message);
            string resId = "Session:" + conn.SessionID + "//Debug.WatermarkDefinition";
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(watermarkXml)))
            {
                conn.ResourceService.SetResourceXmlData(resId, ms);
            }

            //Add watermark to Map Definition
            var wmd = (IWatermarkDefinition)conn.ResourceService.GetResource(resId);
            mdf.AddWatermark(wmd);

            return resId;
        }
    }
}