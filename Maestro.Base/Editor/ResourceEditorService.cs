#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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

using Maestro.Base.Services;
using Maestro.Base.UI;
using Maestro.Editors;
using Maestro.Editors.Preview;
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.ComponentModel;

namespace Maestro.Base.Editor
{
    internal interface IAlternatePreviewFactory
    {
        IPreviewUrl[] GetAlternateWebLayoutPreviewUrls(string resourceID, string locale);

        IPreviewUrl[] GetAlternateFlexibleLayoutPreviewUrls(string resourceID, string locale);
    }

    internal class ResourceEditorService : ResourceEditorServiceBase
    {
        readonly IUrlLauncherService _launcher;
        readonly ISiteExplorer _siteExp;
        readonly OpenResourceManager _orm;
        readonly IAlternatePreviewFactory _factory;

        internal ResourceEditorService(string resourceID, IServerConnection conn, IUrlLauncherService launcher, ISiteExplorer siteExp, IAlternatePreviewFactory factory, OpenResourceManager orm)
            : base(resourceID, conn)
        {
            _siteExp = siteExp;
            _launcher = launcher;
            _factory = factory;
            _orm = orm;
        }

        internal void ReReadSessionResource()
        {
            _editCopy = _conn.ResourceService.GetResource(this.EditedResourceID);
            _editCopy.PropertyChanged += WeakEventHandler.Wrap<PropertyChangedEventHandler>(OnResourcePropertyChanged, (eh) => _editCopy.PropertyChanged -= eh);
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
            _siteExp.RefreshModel(_conn.DisplayName);
        }

        public override void RequestRefresh(string folderId)
        {
            _siteExp.RefreshModel(_conn.DisplayName, folderId);
        }

        public override void OpenResource(string resourceId)
        {
            _orm.Open(resourceId, _conn, false, _siteExp);
        }

        public override void RunProcess(string processName, params string[] args)
        {
            throw new ApplicationException(string.Format(Strings.ErrorUnknownExecutable, processName));
        }

        public override IPreviewUrl[] GetAlternateWebLayoutPreviewUrls(string resourceID, string locale)
        {
            return _factory.GetAlternateWebLayoutPreviewUrls(resourceID, locale);
        }

        public override IPreviewUrl[] GetAlternateFlexibleLayoutPreviewUrls(string resourceID, string locale)
        {
            return _factory.GetAlternateFlexibleLayoutPreviewUrls(resourceID, locale);
        }
    }
}