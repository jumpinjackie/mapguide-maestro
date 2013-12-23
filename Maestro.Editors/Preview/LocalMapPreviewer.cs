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
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maestro.Editors.Preview
{
    /// <summary>
    /// A resource previewer that uses the built-in map viewer component
    /// </summary>
    public class LocalMapPreviewer : IResourcePreviewer
    {
        private IResourcePreviewer _inner;
        private IUrlLauncherService _launcher;
        
        /// <summary>
        /// Initializes a new instance of the LocalMapPreviewer class
        /// </summary>
        /// <param name="inner">The inner resource previewer</param>
        /// <param name="launcher">The URL launcher service</param>
        public LocalMapPreviewer(IResourcePreviewer inner, IUrlLauncherService launcher)
        {
            _inner = inner;
            _launcher = launcher;
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
        /// <returns></returns>
        public bool IsPreviewable(IResource res)
        {
            if (this.UseLocal)
            {
                if (IsLocalPreviewableType(res))
                {
                    return true;
                }
            }
            return _inner.IsPreviewable(res);
        }

        private static bool IsLocalPreviewableType(IResource res)
        {
            return res.ResourceType == ResourceTypes.LayerDefinition ||
                   res.ResourceType == ResourceTypes.MapDefinition ||
                   res.ResourceType == ResourceTypes.WatermarkDefinition;
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

        static bool SupportsMappingService(IServerConnection conn)
        {
            return Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Mapping) >= 0;
        }

        /// <summary>
        /// Previews the given resource
        /// </summary>
        /// <param name="res"></param>
        /// <param name="edSvc"></param>
        /// <param name="locale"></param>
        public void Preview(IResource res, IEditorService edSvc, string locale)
        {
            IServerConnection conn = res.CurrentConnection;
            if (this.UseLocal && IsLocalPreviewableType(res) && SupportsMappingService(conn))
            {
                BusyWaitDelegate worker = () =>
                {
                    IMappingService mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
                    IMapDefinition previewMdf = null;
                    switch (res.ResourceType)
                    {
                        case ResourceTypes.LayerDefinition:
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
                        case ResourceTypes.WatermarkDefinition:
                            {
                                previewMdf = ResourcePreviewEngine.CreateWatermarkPreviewMapDefinition((IWatermarkDefinition)res);
                            }
                            break;
                        case ResourceTypes.MapDefinition:
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
                        var diag = new MapPreviewDialog(rtMap, _launcher, (edSvc.IsNew) ? null : edSvc.ResourceID);
                        diag.Show(null);
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
