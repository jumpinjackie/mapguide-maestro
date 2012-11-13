#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Base.Editor;
using Maestro.Editors;
using System.Drawing;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Services;
using Maestro.Editors.SymbolDefinition;
using ICSharpCode.Core;
using Maestro.Base.UI.Preferences;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using Maestro.Base.UI;
using OSGeo.MapGuide.MaestroAPI.Mapping;

namespace Maestro.Base.Services
{
    /// <summary>
    /// Defines an interface for previewing resources
    /// </summary>
    public interface IResourcePreviewer
    {
        /// <summary>
        /// Gets whether the specified resource can be previewed
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        bool IsPreviewable(IResource res);

        /// <summary>
        /// Previews the specified resource
        /// </summary>
        /// <param name="res">The resource to be previewed</param>
        /// <param name="edSvc">The editor service</param>
        void Preview(IResource res, IEditorService edSvc);

        /// <summary>
        /// Previews the specified resource using the given locale
        /// </summary>
        /// <param name="res">The resource to be previewed</param>
        /// <param name="edSvc">The editor service</param>
        /// <param name="locale">The locale</param>
        /// <remarks>
        /// The locale parameter should be treated as a hint. The underlying <see cref="T:OSGeo.MapGuide.MaestroAPI.IServerConnection"/> implementation
        /// may not actually respect this value.
        /// </remarks>
        void Preview(IResource res, IEditorService edSvc, string locale);
    }

    internal class StubPreviewer : IResourcePreviewer
    {
        public bool IsPreviewable(IResource res)
        {
            return false;
        }

        public void Preview(IResource res, IEditorService edSvc)
        {
            
        }

        public void Preview(IResource res, IEditorService edSvc, string locale)
        {
            
        }
    }


    public class LocalMapPreviewer : IResourcePreviewer
    {
        private IResourcePreviewer _inner;

        public LocalMapPreviewer(IResourcePreviewer inner)
        {
            _inner = inner;
        }

        public bool UseLocal
        {
            get { return PropertyService.Get(ConfigProperties.UseLocalPreview, ConfigProperties.DefaultUseLocalPreview); }
        }

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

        public void Preview(IResource res, IEditorService edSvc)
        {
            Preview(res, edSvc, null);
        }

        static bool SupportsMappingService(IServerConnection conn)
        {
            return Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Mapping) >= 0;
        }

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
                    if (previewMdf != null)
                        return mapSvc.CreateMap(previewMdf);
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
                        var launcher = ServiceRegistry.GetService<UrlLauncherService>();
                        var diag = new MapPreviewDialog(rtMap, launcher, (edSvc.IsNew) ? null : edSvc.ResourceID);
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

    /// <summary>
    /// The default resource previewer implementation
    /// </summary>
    public class DefaultResourcePreviewer : IResourcePreviewer
    {
        abstract class PreviewResult
        { 
            
        }

        class UrlPreviewResult : PreviewResult 
        {
            public string Url { get; set; }
        }

        class ImagePreviewResult : PreviewResult
        {
            public Image ImagePreview { get; set; }
        }

        /// <summary>
        /// Previews the specified resource
        /// </summary>
        /// <param name="res">The resource to be previewed</param>
        /// <param name="edSvc">The editor service</param>
        public void Preview(IResource res, IEditorService edSvc)
        {
            Preview(res, edSvc, edSvc.PreviewLocale);
        }

        /// <summary>
        /// Previews the specified resource
        /// </summary>
        /// <param name="res">The resource to be previewed</param>
        /// <param name="edSvc">The editor service</param>
        public void Preview(IResource res, IEditorService edSvc, string locale)
        {
            //TODO: Prompt for symbol parameters if there are any, as these can affect the rendered output
            //and it is a nice way to test symbol parameters wrt to rendering

            IServerConnection conn = res.CurrentConnection;
            BusyWaitDelegate worker = () => {
                string mapguideRootUrl = (string)conn.GetCustomProperty("BaseUrl"); //NOXLATE
                //Save the current resource to another session copy
                string resId = "Session:" + edSvc.SessionID + "//" + res.ResourceType.ToString() + "Preview" + Guid.NewGuid() + "." + res.ResourceType.ToString(); //NOXLATE
                
                edSvc.ResourceService.SaveResourceAs(res, resId);
                edSvc.ResourceService.CopyResource(res.ResourceID, resId, true);
                var previewCopy = edSvc.ResourceService.GetResource(resId);

                if (previewCopy.ResourceType == ResourceTypes.SymbolDefinition && conn.SiteVersion >= new Version(2, 0))
                {
                    //For Symbol Definition previews, we make a placeholder Layer Definition with the 
                    ILayerDefinition layerDef = ObjectFactory.CreateDefaultLayer(conn, LayerType.Vector);
                    IVectorLayerDefinition2 vl = layerDef.SubLayer as IVectorLayerDefinition2;
                    if (vl != null)
                    {
                        //HACK-ish: We are flubbing a completely invalid Layer Definition under normal circumstances, 
                        //but one that has the minimum required content model to generate an appropriate GETLEGENDIMAGE preview for
                        vl.FeatureName = string.Empty;
                        vl.ResourceId = string.Empty;
                        vl.Geometry = string.Empty;
                        vl.ToolTip = string.Empty;
                        var vsr = vl.GetScaleRangeAt(0) as IVectorScaleRange2;
                        if (vsr != null)
                        {
                            vsr.AreaStyle = null;
                            vsr.LineStyle = null;
                            vsr.PointStyle = null;
                            var cs = layerDef.CreateDefaultCompositeStyle();
                            var cr = cs.GetRuleAt(0);
                            var csym = cr.CompositeSymbolization;
                            var si = csym.CreateSymbolReference(previewCopy.ResourceID);
                            csym.AddSymbolInstance(si);
                            vsr.CompositeStyle = new List<ICompositeTypeStyle>() { cs };

                            var ldfId = "Session:" + edSvc.SessionID + "//" + res.ResourceType.ToString() + "Preview" + Guid.NewGuid() + ".LayerDefinition"; //NOXLATE
                            edSvc.ResourceService.SaveResourceAs(layerDef, ldfId);

                            var mappingSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
                            var img = mappingSvc.GetLegendImage(42, ldfId, 0, 4, 100, 100, "PNG"); //NOXLATE
                            return new ImagePreviewResult() { ImagePreview = img };
                        }
                    }

                    return null;
                }
                else
                {
                    //Now feed it to the preview engine
                    var url = new ResourcePreviewEngine(mapguideRootUrl, edSvc).GeneratePreviewUrl(previewCopy, locale);
                    return new UrlPreviewResult() { Url = url };
                }
            };
            Action<object, Exception> onComplete = (result, ex) => {
                var urlResult = result as UrlPreviewResult;
                var imgResult = result as ImagePreviewResult;
                if (urlResult != null)
                {
                    var url = urlResult.Url;
                    var launcher = ServiceRegistry.GetService<UrlLauncherService>();
                    launcher.OpenUrl(url);
                }
                else if (imgResult != null)
                {
                    new SymbolPreviewDialog(imgResult.ImagePreview).Show(null);
                }
            };
            BusyWaitDialog.Run(Strings.PrgPreparingResourcePreview, worker, onComplete);
        }

        /// <summary>
        /// Gets whether the specified resource can be previewed
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public bool IsPreviewable(IResource res)
        {
            var rt = res.ResourceType;
            if (res.CurrentConnection.Capabilities.SupportsResourcePreviews)
            {
                if (rt == ResourceTypes.SymbolDefinition)
                {
                    return res.CurrentConnection.SiteVersion >= new Version(2, 0) && Array.IndexOf(res.CurrentConnection.Capabilities.SupportedServices, (int)ServiceType.Mapping) >= 0;
                }
                else
                {
                    return ResourcePreviewEngine.IsPreviewableType(rt);
                }
            }
            return false;
        }
    }

    /// <summary>
    /// A simple <see cref="T:Maestro.Base.Service.IResourcePreviewer"/> resolution container
    /// </summary>
    public static class ResourcePreviewerFactory
    {
        static Dictionary<string, IResourcePreviewer> _previewers = new Dictionary<string, IResourcePreviewer>();

        /// <summary>
        /// Registers the given <see cref="T:Maestro.Base.Service.IResourcePreviewer"/> for a given connection provider
        /// </summary>
        /// <param name="provider">The name of the connection provider</param>
        /// <param name="previewer">The previewer implementation</param>
        public static void RegisterPreviewer(string provider, IResourcePreviewer previewer)
        {
            _previewers[provider.ToUpper()] = new LocalMapPreviewer(previewer);
        }

        /// <summary>
        /// Gets whether a previewer has been registered for the given connection provider
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static bool HasPreviewer(string provider)
        {
            return _previewers.ContainsKey(provider.ToUpper());
        }
        
        /// <summary>
        /// Gets whether the given resource type is previewable for the given connection provider
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static bool IsPreviewable(string provider, IResource resource)
        {
            var preview = GetPreviewer(provider);
            if (preview != null)
                return preview.IsPreviewable(resource);

            return false;
        }

        /// <summary>
        /// Gets the registered previewer for the specified connection provider
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IResourcePreviewer GetPreviewer(string provider)
        {
            if (HasPreviewer(provider))
                return _previewers[provider.ToUpper()];

            return null;
        }
    }
}
