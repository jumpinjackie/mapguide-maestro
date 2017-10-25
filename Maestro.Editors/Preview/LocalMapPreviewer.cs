#region Disclaimer / License

// Copyright (C) 2013, Jackie Ng
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

using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using System;

namespace Maestro.Editors.Preview
{
    /// <summary>
    /// A resource previewer that uses the built-in map viewer component
    /// </summary>
    public class LocalMapPreviewer : IResourcePreviewer
    {
        readonly IResourcePreviewer _inner;
        readonly IUrlLauncherService _launcher;
        readonly IViewContentManager _viewManager;

        /// <summary>
        /// Initializes a new instance of the LocalMapPreviewer class
        /// </summary>
        /// <param name="inner">The inner resource previewer</param>
        /// <param name="launcher">The URL launcher service</param>
        /// <param name="viewManager">The view content manager</param>
        public LocalMapPreviewer(IResourcePreviewer inner, IUrlLauncherService launcher, IViewContentManager viewManager)
        {
            _inner = inner;
            _launcher = launcher;
            _viewManager = viewManager;
        }

        /// <summary>
        /// Gets whether to use the built-in map viewer for previewing
        /// </summary>
        public bool UseLocal
        {
            get { return PreviewSettings.UseLocalPreview; }
        }

        /// <summary>
        /// Gets whether the given resource is previewable with this previewer
        /// </summary>
        /// <param name="res"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public bool IsPreviewable(IResource res, IServerConnection conn)
        {
            if (this.UseLocal)
            {
                if (IsLocalPreviewableType(res))
                {
                    return true;
                }
            }
            return _inner.IsPreviewable(res, conn);
        }

        private static bool IsLocalPreviewableType(IResource res)
        {
            return (res.ResourceType == ResourceTypes.LayerDefinition.ToString() ||
                   res.ResourceType == ResourceTypes.MapDefinition.ToString() ||
                   res.ResourceType == ResourceTypes.WatermarkDefinition.ToString()) &&
                   !(res is OSGeo.MapGuide.ObjectModels.UntypedResource);
        }

        /// <summary>
        /// Previews the given resource
        /// </summary>
        /// <param name="res"></param>
        /// <param name="edSvc"></param>
        public void Preview(IResource res, IEditorService edSvc)
        {
            Preview(res, edSvc, null);
        }

        private static bool SupportsMappingService(IServerConnection conn)
        {
            return conn.SiteVersion >= new Version(2, 1) //Local preview needs APIs introduced in MGOS 2.1
                && Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Mapping) >= 0;
        }

        /// <summary>
        /// Previews the given resource
        /// </summary>
        /// <param name="res"></param>
        /// <param name="edSvc"></param>
        /// <param name="locale"></param>
        public void Preview(IResource res, IEditorService edSvc, string locale)
        {
            IServerConnection conn = edSvc.CurrentConnection;
            if (this.UseLocal && IsLocalPreviewableType(res) && SupportsMappingService(conn))
            {
                BusyWaitDelegate worker = () =>
                {
                    IMappingService mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
                    IMapDefinition previewMdf = null;
                    switch (res.ResourceType)
                    {
                        case "LayerDefinition":
                            {
                                ILayerDefinition ldf = (ILayerDefinition)res;
                                string layerName = string.Empty;
                                if (edSvc.IsNew)
                                    layerName = ResourceIdentifier.GetName(ldf.SubLayer.ResourceId);
                                else
                                    layerName = ResourceIdentifier.GetName(edSvc.ResourceID);
                                previewMdf = ResourcePreviewEngine.CreateLayerPreviewMapDefinition(ldf, edSvc.SessionID, layerName, conn);
                            }
                            break;

                        case "WatermarkDefinition":
                            {
                                previewMdf = Utility.CreateWatermarkPreviewMapDefinition((IWatermarkDefinition)res);
                            }
                            break;

                        case "MapDefinition":
                            {
                                previewMdf = (IMapDefinition)res;
                            }
                            break;
                    }

                    if (string.IsNullOrEmpty(previewMdf.ResourceID))
                    {
                        var sessionId = edSvc.SessionID;
                        var mdfId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".MapDefinition"; //NOXLATE

                        conn.ResourceService.SaveResourceAs(previewMdf, mdfId);
                        previewMdf.ResourceID = mdfId;
                    }

                    if (previewMdf != null)
                        return mapSvc.CreateMap(previewMdf, false);
                    else
                        return null;
                };
                Action<object, Exception> onComplete = (obj, ex) =>
                {
                    if (ex != null)
                        throw ex;

                    if (obj != null)
                    {
                        var rtMap = (RuntimeMap)obj;
                        if (_viewManager != null)
                        {
                            _viewManager.OpenContent(ViewRegion.Document, () => new MapPreviewViewContent(rtMap, _launcher, (edSvc.IsNew) ? null : edSvc.ResourceID));
                        }
                        else
                        {
                            var diag = new MapPreviewDialog(rtMap, _launcher, (edSvc.IsNew) ? null : edSvc.ResourceID);
                            diag.Show(null);
                        }
                    }
                    else //Fallback, shouldn't happen
                    {
                        _inner.Preview(res, edSvc, locale);
                    }
                };
                BusyWaitDialog.Run(Strings.PrgPreparingResourcePreview, worker, onComplete);
            }
            else
            {
                _inner.Preview(res, edSvc, locale);
            }
        }
    }
}