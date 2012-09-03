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
    public interface IResourcePreviewer
    {
        bool IsPreviewable(IResource res);

        void Preview(IResource res, IEditorService edSvc);
    }

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

        public void Preview(IResource res, IEditorService edSvc)
        {
            //TODO: Prompt for symbol parameters if there are any, as these can affect the rendered output
            //and it is a nice way to test symbol parameters wrt to rendering

            IServerConnection conn = res.CurrentConnection;
            BusyWaitDialog.Run(Properties.Resources.PrgPreparingResourcePreview, () => {
                string mapguideRootUrl = (string)conn.GetCustomProperty("BaseUrl");
                //Save the current resource to another session copy
                string resId = "Session:" + edSvc.SessionID + "//" + res.ResourceType.ToString() + "Preview" + Guid.NewGuid() + "." + res.ResourceType.ToString();
                
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
                        vl.FeatureName = "";
                        vl.ResourceId = "";
                        vl.Geometry = "";
                        vl.ToolTip = "";
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

                            var ldfId = "Session:" + edSvc.SessionID + "//" + res.ResourceType.ToString() + "Preview" + Guid.NewGuid() + ".LayerDefinition";
                            edSvc.ResourceService.SaveResourceAs(layerDef, ldfId);

                            var mappingSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
                            var img = mappingSvc.GetLegendImage(42, ldfId, 0, 4, 100, 100, "PNG");
                            return new ImagePreviewResult() { ImagePreview = img };
                        }
                    }

                    return null;
                }
                else
                {
                    //Now feed it to the preview engine
                    var url = new ResourcePreviewEngine(mapguideRootUrl, edSvc).GeneratePreviewUrl(previewCopy);
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

    public static class ResourcePreviewerFactory
    {
        static Dictionary<string, IResourcePreviewer> _previewers = new Dictionary<string, IResourcePreviewer>();

        public static void RegisterPreviewer(string provider, IResourcePreviewer previewer)
        {
            _previewers[provider.ToUpper()] = previewer;
        }

        public static bool HasPreviewer(string provider)
        {
            return _previewers.ContainsKey(provider.ToUpper());
        }

        public static IResourcePreviewer GetPreviewer(string provider)
        {
            if (HasPreviewer(provider))
                return _previewers[provider.ToUpper()];

            return null;
        }
    }
}
