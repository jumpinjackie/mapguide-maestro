#region Disclaimer / License

// Copyright (C) 2012, Jackie Ng
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

using Maestro.Editors;
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Diagnostics;

namespace Maestro.LiveMapEditor
{
    public class ResourceEditorService : ResourceEditorServiceBase
    {
        public ResourceEditorService(string resourceID, IServerConnection conn)
            : base(resourceID, conn)
        { }

        public override void OpenUrl(string url)
        {
            var ps = new ProcessStartInfo(url)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);
        }

        public override string SelectUnmanagedData(string startPath, System.Collections.Specialized.NameValueCollection fileTypes)
        {
            throw new NotImplementedException();
        }

        public override void RequestRefresh()
        {
            throw new NotImplementedException();
        }

        public override void RequestRefresh(string folderId)
        {
            //throw new NotImplementedException();
        }

        public override void OpenResource(string resourceId)
        {
            throw new NotImplementedException();
        }

        public override void RunProcess(string processName, params string[] args)
        {
            throw new NotImplementedException();
        }

        public override IPreviewUrl[] GetAlternateWebLayoutPreviewUrls(string resourceID, string locale)
        {
            return Array.Empty<IPreviewUrl>();
        }

        public override IPreviewUrl[] GetAlternateFlexibleLayoutPreviewUrls(string resourceID, string locale)
        {
            return Array.Empty<IPreviewUrl>();
        }
    }
}