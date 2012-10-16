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
using Maestro.Editors;
using System.Diagnostics;
using OSGeo.MapGuide.MaestroAPI;

namespace StandaloneWebLayoutEditor
{
    /// <summary>
    /// A simple implementation of <see cref="T:Maestro.Editors.ResourceEditorServiceBase"/>. Pretty much
    /// all required methods here are non-essential and can be safely stubbed out.
    /// </summary>
    public class MyResourceEditorService : ResourceEditorServiceBase
    {
        public MyResourceEditorService(string resourceID, IServerConnection conn)
            : base(resourceID, conn)
        { }

        /// <summary>
        /// Called when an editor requires to open a particular resource. In Maestro, this would
        /// spawn a new resource editor (or activate an existing one) for the selected resource id.
        /// </summary>
        /// <param name="resourceId"></param>
        public override void OpenResource(string resourceId)
        {
            
        }

        /// <summary>
        /// Called when an editor requires opening a url. 
        /// </summary>
        /// <param name="url"></param>
        public override void OpenUrl(string url)
        {
            Process.Start(url);
        }

        /// <summary>
        /// Called when the editor requires a refresh of the site explorer for a specific folder
        /// </summary>
        /// <param name="folderId"></param>
        public override void RequestRefresh(string folderId)
        {
            
        }

        /// <summary>
        /// Called when the editor requires a refresh of the entire site explorer
        /// </summary>
        public override void RequestRefresh()
        {
            
        }

        /// <summary>
        /// Called when the editor needs to prompt the user to select an unmanaged resource. Currently not used
        /// </summary>
        /// <param name="startPath"></param>
        /// <param name="fileTypes"></param>
        /// <returns></returns>
        public override string SelectUnmanagedData(string startPath, System.Collections.Specialized.NameValueCollection fileTypes)
        {
            throw new NotImplementedException();
        }

        public override void RunProcess(string processName, params string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
