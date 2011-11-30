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
        public const string ValidateOnSave = "General.ValidateResourceOnSave";
        public const string XsdSchemaPath = "Editor.XsdSchemaPath";
        public const string ShowTipOfTheDay = "General.ShowTipOfTheDay";

        internal static void ApplyDefaults()
        {
            ApplyGeneralDefaults();
            ApplyEditorDefaults();
        }

        internal static void ApplyEditorDefaults()
        {
            Props.Set(ConfigProperties.ValidateOnSave, DefaultValidateOnSave);
            Props.Set(ConfigProperties.XsdSchemaPath, DefaultXsdSchemaPath);
        }

        internal static void ApplyGeneralDefaults()
        {
            Props.Set(ConfigProperties.PreviewViewerType, DefaultPreviewViewerType);
            Props.Set(ConfigProperties.UserTemplatesDirectory, DefaultUserTemplatesDirectory);
            Props.Set(ConfigProperties.ShowMessages, DefaultShowMessages);
            Props.Set(ConfigProperties.ShowOutboundRequests, DefaultShowOutboundRequests);
            Props.Set(ConfigProperties.OpenColor, DefaultOpenColor);
            Props.Set(ConfigProperties.DirtyColor, DefaultDirtyColor);
            Props.Set(ConfigProperties.MgCookerPath, DefaultMgCookerPath);
            Props.Set(ConfigProperties.LocalFsPreviewPath, DefaultLocalFsPreviewPath);
            Props.Set(ConfigProperties.ShowTipOfTheDay, DefaultShowTipOfTheDay);
        }

        public static bool DefaultShowTipOfTheDay { get { return true; } }

        public static string DefaultMgCookerPath { get { return Path.Combine(FileUtility.ApplicationRootPath, "MgCooker.exe"); } }

        public static string DefaultLocalFsPreviewPath { get { return Path.Combine(FileUtility.ApplicationRootPath, "MaestroFsPreview.exe"); } }

        public static Color DefaultOpenColor { get { return Color.LightGreen; } }

        public static Color DefaultDirtyColor { get { return Color.Pink; } }

        public static bool DefaultShowMessages { get { return true; } }

        public static bool DefaultShowOutboundRequests { get { return true; } }

        public static bool DefaultValidateOnSave { get { return true; } }

        public static string DefaultXsdSchemaPath { get { return Path.Combine(FileUtility.ApplicationRootPath, "Schemas"); } }

        public static string DefaultPreviewViewerType { get { return "AJAX"; } }

        public static string DefaultUserTemplatesDirectory { get { return Path.Combine(FileUtility.ApplicationRootPath, "UserTemplates"); } }
    }
}
