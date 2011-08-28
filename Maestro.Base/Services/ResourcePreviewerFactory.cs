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
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Base.Editor;
using Maestro.Editors;

namespace Maestro.Base.Services
{
    public interface IResourcePreviewer
    {
        bool IsPreviewable(IResource res);

        void Preview(IResource res, IEditorService edSvc);
    }

    public class DefaultResourcePreviewer : IResourcePreviewer
    {
        public void Preview(IResource res, IEditorService edSvc)
        {
            IServerConnection conn = res.CurrentConnection;
            string mapguideRootUrl = (string)conn.GetCustomProperty("BaseUrl");

            //Save the current resource to another session copy
            string resId = "Session:" + edSvc.SessionID + "//" + Guid.NewGuid() + "." + res.ResourceType.ToString();
            edSvc.ResourceService.SetResourceXmlData(resId, ResourceTypeRegistry.Serialize(res));

            //Copy any resource data
            var previewCopy = edSvc.ResourceService.GetResource(resId);
            res.CopyResourceDataTo(previewCopy);

            //Now feed it to the preview engine
            var url = new ResourcePreviewEngine(mapguideRootUrl, edSvc).GeneratePreviewUrl(previewCopy);
            var launcher = ServiceRegistry.GetService<UrlLauncherService>();

            launcher.OpenUrl(url);
        }

        public bool IsPreviewable(IResource res)
        {
            var rt = res.ResourceType;
            return ResourcePreviewEngine.IsPreviewableType(rt) && res.CurrentConnection.Capabilities.SupportsResourcePreviews;                    
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
