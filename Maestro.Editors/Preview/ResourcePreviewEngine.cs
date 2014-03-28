#region Disclaimer / License
// Copyright (C) 2013, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Maestro.Editors.Preview
{
    /// <summary>
    /// Performs any necessary setup required to preview a given resource in a web browser
    /// </summary>
    public class ResourcePreviewEngine
    {
        private string _rootUrl;
        private IEditorService _edSvc;

        /// <summary>
        /// Initializes a new instance of the ResourcePreviewEngine class
        /// </summary>
        /// <param name="rootUrl"></param>
        /// <param name="edSvc"></param>
        public ResourcePreviewEngine(string rootUrl, IEditorService edSvc)
        {
            _rootUrl = rootUrl;
            _edSvc = edSvc;
        }

        static string GetLocale(string locale)
        {
            return string.IsNullOrEmpty(locale) ? "en" : locale; //NOXLATE
        }

        private string GenerateFeatureSourcePreviewUrl(IResource res, string locale)
        {
            string url = _rootUrl;
            if (!url.EndsWith("/")) //NOXLATE
                url += "/"; //NOXLATE

            var resId = res.ResourceID;
            var sessionId = _edSvc.SessionID;
            url += "schemareport/describeschema.php?viewer=basic&schemaName=&className=&resId=" + resId + "&sessionId=" + sessionId + "&locale=" + GetLocale(locale); //NOXLATE

            return url;
        }

        private string GenerateLayerPreviewUrl(IResource res, string locale)
        {
            string url = _rootUrl;
            if (!url.EndsWith("/")) //NOXLATE
                url += "/"; //NOXLATE

            var ldf = (ILayerDefinition)res;
            var sessionId = _edSvc.SessionID;
            var conn = res.CurrentConnection;

            string layerName = string.Empty;
            //Use feature source as name/label if new and unsaved
            if (_edSvc.IsNew)
                layerName = ResourceIdentifier.GetName(ldf.SubLayer.ResourceId);
            else
                layerName = ResourceIdentifier.GetName(_edSvc.ResourceID);

            var mdf = CreateLayerPreviewMapDefinition(ldf, sessionId, layerName, conn);
            //if (PropertyService.Get(ConfigProperties.PreviewViewerType, "AJAX").Equals("AJAX")) //NOXLATE
            if (PreviewSettings.UseAjaxViewer)
            {
                //Create temp web layout to house this map
                var wl = ObjectFactory.CreateWebLayout(_edSvc.GetEditedResource().CurrentConnection, new Version(1, 0, 0), mdf.ResourceID);

                //Add a custom zoom command (to assist previews of layers that aren't [0, infinity] scale)
                AttachPreviewCommands(wl);

                var resId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".WebLayout"; //NOXLATE

                conn.ResourceService.SaveResourceAs(wl, resId);
                url += "mapviewerajax/?WEBLAYOUT=" + resId + "&SESSION=" + sessionId + "&LOCALE=" + GetLocale(locale); //NOXLATE
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

        static string CreateDebugWatermark(IMapDefinition2 mdf, IServerConnection conn, string layerSc)
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
            string watermarkXml = string.Format(Properties.Resources.TextWatermark, message);
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

        internal static IMapDefinition CreateLayerPreviewMapDefinition(ILayerDefinition ldf, string sessionId, string layerName, IServerConnection conn)
        {
            //Create temp map definition to house our current layer
            var mdfId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".MapDefinition"; //NOXLATE
            string csWkt;
            var extent = ldf.GetSpatialExtent(true, out csWkt);
            if (extent == null)
                throw new ApplicationException(Strings.FailedToCalculateFeatureSourceExtents);

            string layerSc = GetLayerSpatialContext(ldf);

            //TODO: Based on the visible scales in this layer, size this extents accordingly
            var mdf = ObjectFactory.CreateMapDefinition(conn, Strings.PreviewMap, csWkt, extent);
            IMapDefinition2 mdf2 = mdf as IMapDefinition2;
            if (mdf2 != null && PreviewSettings.AddDebugWatermark)
                CreateDebugWatermark(mdf2, conn, layerSc);
            
            var layer = mdf.AddLayer(null, layerName, ldf.ResourceID);
            conn.ResourceService.SaveResourceAs(mdf, mdfId);
            mdf.ResourceID = mdfId;
            return mdf;
        }

        private static string GetLayerSpatialContext(ILayerDefinition ldf)
        {
            var conn = ldf.CurrentConnection;
            var rl = ldf.SubLayer as IRasterLayerDefinition;
            var vl = ldf.SubLayer as IVectorLayerDefinition;
            if (vl != null)
            {
                var cls = conn.FeatureService.GetClassDefinition(vl.ResourceId, vl.FeatureName);
                var gp = cls.FindProperty(vl.Geometry) as GeometricPropertyDefinition;
                if (gp != null)
                    return gp.SpatialContextAssociation;
            }
            else if (rl != null)
            {
                var cls = conn.FeatureService.GetClassDefinition(rl.ResourceId, rl.FeatureName);
                var rp = cls.FindProperty(rl.Geometry) as RasterPropertyDefinition;
                if (rp != null)
                    return rp.SpatialContextAssociation;
            }
            return null;
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

        private string GenerateWatermarkPreviewUrl(IWatermarkDefinition wmd, string locale)
        {
            //We demand a 2.3.0 Map Definition or higher
            if (wmd.CurrentConnection.SiteVersion < new Version(2, 3))
                throw new InvalidOperationException(Strings.SiteVersionDoesntSupportWatermarks);

            IMapDefinition2 map = CreateWatermarkPreviewMapDefinition(wmd);
            return GenerateMapPreviewUrl(map, locale);
        }

        internal static IMapDefinition2 CreateWatermarkPreviewMapDefinition(IWatermarkDefinition wmd)
        {
            IMapDefinition2 map = (IMapDefinition2)ObjectFactory.CreateMapDefinition(wmd.CurrentConnection, wmd.SupportedMapDefinitionVersion, "Watermark Definition Preview"); //NOXLATE
            map.CoordinateSystem = @"LOCAL_CS[""*XY-M*"", LOCAL_DATUM[""*X-Y*"", 10000], UNIT[""Meter"", 1], AXIS[""X"", EAST], AXIS[""Y"", NORTH]]"; //NOXLATE
            map.Extents = ObjectFactory.CreateEnvelope(-1000000, -1000000, 1000000, 1000000);
            map.AddWatermark(wmd);
            return map;
        }

        private string GenerateMapPreviewUrl(IResource res, string locale)
        {
            string url = _rootUrl;
            if (!url.EndsWith("/")) //NOXLATE
                url += "/"; //NOXLATE

            var sessionId = _edSvc.SessionID;
            var mdfId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".MapDefinition"; //NOXLATE
            var mdf = (IMapDefinition)res;

            var conn = mdf.CurrentConnection;
            IMapDefinition2 mdf2 = mdf as IMapDefinition2;
            if (mdf2 != null && PreviewSettings.AddDebugWatermark)
                CreateDebugWatermark(mdf2, conn, null);
            conn.ResourceService.SaveResourceAs(mdf, mdfId);

            //if (PropertyService.Get(ConfigProperties.PreviewViewerType, "AJAX").Equals("AJAX")) //NOXLATE
            if (PreviewSettings.UseAjaxViewer)
            {
                //Create temp web layout to house this map
                var wl = ObjectFactory.CreateWebLayout(_edSvc.GetEditedResource().CurrentConnection, new Version(1, 0, 0), mdfId);

                //Add a custom zoom command (to assist previews of layers that aren't [0, infinity] scale)
                AttachPreviewCommands(wl);

                var resId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".WebLayout"; //NOXLATE

                conn.ResourceService.SaveResourceAs(wl, resId);
                url += "mapviewerajax/?WEBLAYOUT=" + resId + "&SESSION=" + sessionId + "&LOCALE=" + GetLocale(locale); //NOXLATE
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

        private string GenerateWebLayoutPreviewUrl(IResource res, string locale)
        {
            string url = _rootUrl;
            if (!url.EndsWith("/")) //NOXLATE
                url += "/"; //NOXLATE

            var sessionId = _edSvc.SessionID;
            var resId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".WebLayout"; //NOXLATE
            var conn = res.CurrentConnection;

            conn.ResourceService.SaveResourceAs(res, resId);
            url += "mapviewerajax/?WEBLAYOUT=" + resId + "&SESSION=" + sessionId + "&LOCALE=" + GetLocale(locale); //NOXLATE

            return url;
        }

        private string GenerateFlexLayoutPreviewUrl(IResource res, string locale)
        {
            string url = _rootUrl;
            if (!url.EndsWith("/")) //NOXLATE
                url += "/"; //NOXLATE

            //Create temp flex layout
            var sessionId = _edSvc.SessionID;
            var appDef = (IApplicationDefinition)res;
            var conn = appDef.CurrentConnection;
            var resId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".ApplicationDefinition"; //NOXLATE

            conn.ResourceService.SaveResourceAs(appDef, resId);
            url += appDef.TemplateUrl + "?Session=" + sessionId + "&ApplicationDefinition=" + resId + "&locale=" + GetLocale(locale); //NOXLATE
            return url;
        }

        /// <summary>
        /// Generates the appropriate viewer URL to preview the given resource under the given locale
        /// </summary>
        /// <param name="res"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public string GeneratePreviewUrl(IResource res, string locale)
        {
            switch (res.ResourceType)
            {
                case ResourceTypes.FeatureSource:
                    return GenerateFeatureSourcePreviewUrl(res, locale);
                case ResourceTypes.ApplicationDefinition:
                    return GenerateFlexLayoutPreviewUrl(res, locale);
                case ResourceTypes.LayerDefinition:
                    return GenerateLayerPreviewUrl(res, locale);
                case ResourceTypes.MapDefinition:
                    return GenerateMapPreviewUrl(res, locale);
                case ResourceTypes.WebLayout:
                    return GenerateWebLayoutPreviewUrl(res, locale);
                case ResourceTypes.WatermarkDefinition:
                    return GenerateWatermarkPreviewUrl((IWatermarkDefinition)res, locale);
                default:
                    throw new InvalidOperationException(Strings.UnpreviewableResourceType);
            }
        }

        private static ResourceTypes[] PREVIEWABLE_RESOURCE_TYPES = new ResourceTypes[] 
        {
            ResourceTypes.FeatureSource,
            ResourceTypes.ApplicationDefinition,
            ResourceTypes.LayerDefinition,
            ResourceTypes.MapDefinition,
            ResourceTypes.WebLayout,
            ResourceTypes.WatermarkDefinition
        };

        internal static bool IsPreviewableType(OSGeo.MapGuide.MaestroAPI.ResourceTypes rt)
        {
            return Array.IndexOf(PREVIEWABLE_RESOURCE_TYPES, rt) >= 0;
        }
    }
}
