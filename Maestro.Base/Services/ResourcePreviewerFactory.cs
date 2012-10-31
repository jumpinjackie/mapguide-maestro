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
            BusyWaitDialog.Run(Strings.PrgPreparingResourcePreview, () => {
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
            }, (result) => {
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
            });
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
            _previewers[provider.ToUpper()] = previewer;
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
