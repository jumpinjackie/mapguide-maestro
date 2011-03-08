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
using ICSharpCode.Core;
using Props = ICSharpCode.Core.PropertyService;
using System.IO;
using System.Drawing;

namespace Maestro.Base.UI.Preferences
{
    public static class ConfigProperties
    {
        public const string PreviewViewerType = "General.PreviewViewerType";
        public const string UserTemplatesDirectory = "General.UserTemplatesDirectory";
        public const string ShowMessages = "General.ShowMessages";
        public const string ShowOutboundRequests = "General.ShowOutboundRequests";
        public const string OpenColor = "General.OpenColor";
        public const string DirtyColor = "General.DirtyColor";
        public const string MgCookerPath = "General.MgCookerPath";
        public const string LocalFsPreviewPath = "General.LocalFsPreviewPath";

        internal static void ApplyDefaults()
        {
            Props.Set(ConfigProperties.PreviewViewerType, "AJAX");
            Props.Set(ConfigProperties.UserTemplatesDirectory, Path.Combine(FileUtility.ApplicationRootPath, "UserTemplates"));
            Props.Set(ConfigProperties.ShowMessages, true);
            Props.Set(ConfigProperties.ShowOutboundRequests, true);
            Props.Set(ConfigProperties.OpenColor, Color.LightGreen);
            Props.Set(ConfigProperties.DirtyColor, Color.Pink);
            Props.Set(ConfigProperties.MgCookerPath, Path.Combine(FileUtility.ApplicationRootPath, "MgCooker.exe"));
            Props.Set(ConfigProperties.LocalFsPreviewPath, Path.Combine(FileUtility.ApplicationRootPath, "MaestroFsPreview.exe"));
        }
    }
}
