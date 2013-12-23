#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Base.Services;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Generic;
using System.ComponentModel;
using System.IO;
using Maestro.Editors.Common;
using Maestro.Base.UI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using Maestro.Editors.Preview;

namespace Maestro.Base.Editor
{
    internal class ResourceEditorService : ResourceEditorServiceBase
    {
        private IUrlLauncherService _launcher;
        private ISiteExplorer _siteExp;
        private OpenResourceManager _orm;

        internal ResourceEditorService(string resourceID, IServerConnection conn, IUrlLauncherService launcher, ISiteExplorer siteExp, OpenResourceManager orm)
            : base(resourceID, conn)
        {
            _siteExp = siteExp;
            _launcher = launcher;
            _orm = orm;
        }

        internal void ReReadSessionResource()
        {
            _editCopy = _conn.ResourceService.GetResource(this.EditedResourceID);
            _editCopy.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnResourcePropertyChanged);
        }

        public override void OpenUrl(string url)
        {
            _launcher.OpenUrl(url);
        }

        public override string SelectUnmanagedData(string startPath, System.Collections.Specialized.NameValueCollection fileTypes)
        {
            throw new NotImplementedException();
        }

        public override void RequestRefresh()
        {
            _siteExp.RefreshModel(_editCopy.CurrentConnection.DisplayName);
        }

        public override void RequestRefresh(string folderId)
        {
            _siteExp.RefreshModel(_editCopy.CurrentConnection.DisplayName, folderId);
        }

        public override void OpenResource(string resourceId)
        {
            _orm.Open(resourceId, _conn, false, _siteExp);
        }

        public override void RunProcess(string processName, params string[] args)
        {
            //HACK: Yeah yeah
            if (processName.ToLower() == "mgcooker") //NOXLATE
            {
                Maestro.Base.Commands.MgCookerCommand.RunCooker(args);
            }
            else
            {
                throw new ApplicationException(string.Format(Strings.ErrorUnknownExecutable, processName));
            }
        }
    }
}
