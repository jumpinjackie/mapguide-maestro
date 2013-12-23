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
    /// <summary>
    /// Configuration property keys for the Maestro base addin
    /// </summary>
    public static class ConfigProperties
    {
        /// <summary>
        /// The locale to preview in
        /// </summary>
        public const string PreviewLocale = "General.PreviewLocale"; //NOXLATE

        /// <summary>
        /// The type of viewer to preview
        /// </summary>
        public const string PreviewViewerType = "General.PreviewViewerType"; //NOXLATE

        /// <summary>
        /// The user template directory
        /// </summary>
        public const string UserTemplatesDirectory = "General.UserTemplatesDirectory"; //NOXLATE

        /// <summary>
        /// Show the messages view
        /// </summary>
        public const string ShowMessages = "General.ShowMessages"; //NOXLATE

        /// <summary>
        /// Show the outbound requests view
        /// </summary>
        public const string ShowOutboundRequests = "General.ShowOutboundRequests"; //NOXLATE

        /// <summary>
        /// The color for open resources in the Site Explorer
        /// </summary>
        public const string OpenColor = "General.OpenColor"; //NOXLATE

        /// <summary>
        /// The color for unsaved resources in the Site Explorer
        /// </summary>
        public const string DirtyColor = "General.DirtyColor"; //NOXLATE

        /// <summary>
        /// The path to MgCooker.exe
        /// </summary>
        public const string MgCookerPath = "General.MgCookerPath"; //NOXLATE

        /// <summary>
        /// The path to RtMapInspector.exe
        /// </summary>
        public const string RtMapInspectorPath = "General.RtMapInspectorPath"; //NOXLATE

        /// <summary>
        /// The path to MaestroFsPreview.exe
        /// </summary>
        public const string LocalFsPreviewPath = "General.LocalFsPreviewPath"; //NOXLATE

        /// <summary>
        /// Validate resources when saving
        /// </summary>
        public const string ValidateOnSave = "General.ValidateResourceOnSave"; //NOXLATE

        /// <summary>
        /// The path to MapGuide XML schemas
        /// </summary>
        public const string XsdSchemaPath = "Editor.XsdSchemaPath"; //NOXLATE

        /// <summary>
        /// Use the local preview using Maestro's Map Viewer component
        /// </summary>
        public const string UseLocalPreview = "Editor.UseLocalPreview"; //NOXLATE

        /// <summary>
        /// Show the tip of the day on startup
        /// </summary>
        public const string ShowTipOfTheDay = "General.ShowTipOfTheDay"; //NOXLATE
        
        /// <summary>
        /// The path to Maestro.LiveMapEditor.exe
        /// </summary>
        public const string LiveMapEditorPath = "General.LiveMapEditorPath"; //NOXLATE

        internal static void ApplyDefaults()
        {
            ApplyGeneralDefaults();
            ApplyEditorDefaults();
        }

        internal static void ApplyEditorDefaults()
        {
            Props.Set(ConfigProperties.PreviewLocale, DefaultPreviewLocale);
            Props.Set(ConfigProperties.ValidateOnSave, DefaultValidateOnSave);
            Props.Set(ConfigProperties.XsdSchemaPath, DefaultXsdSchemaPath);
            Props.Set(ConfigProperties.UseLocalPreview, DefaultUseLocalPreview);
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
            Props.Set(ConfigProperties.RtMapInspectorPath, DefaultRtMapInspectorPath);
            Props.Set(ConfigProperties.LocalFsPreviewPath, DefaultLocalFsPreviewPath);
            Props.Set(ConfigProperties.ShowTipOfTheDay, DefaultShowTipOfTheDay);
            Props.Set(ConfigProperties.LiveMapEditorPath, DefaultLiveMapEditorPath);
        }

        /// <summary>
        /// Default locale setting for web-based resource previews
        /// </summary>
        public static string DefaultPreviewLocale { get { return "en"; } } //NOXLATE

        /// <summary>
        /// Default setting for "Show tip of the day"
        /// </summary>
        public static bool DefaultShowTipOfTheDay { get { return true; } }

        /// <summary>
        /// Default path to MgCooker.exe
        /// </summary>
        public static string DefaultMgCookerPath { get { return Path.Combine(FileUtility.ApplicationRootPath, "MgCooker.exe"); } } //NOXLATE

        /// <summary>
        /// Default path to MaestroFsPreview.exe
        /// </summary>
        public static string DefaultLocalFsPreviewPath { get { return Path.Combine(FileUtility.ApplicationRootPath, "MaestroFsPreview.exe"); } } //NOXLATE

        /// <summary>
        /// Default path to RtMapInspector.exe
        /// </summary>
        public static string DefaultRtMapInspectorPath { get { return Path.Combine(FileUtility.ApplicationRootPath, "RtMapInspector.exe"); } } //NOXLATE

        /// <summary>
        /// Default path to Maestro.LiveMapEditor.exe
        /// </summary>
        public static string DefaultLiveMapEditorPath { get { return Path.Combine(FileUtility.ApplicationRootPath, "Maestro.LiveMapEditor.exe"); } } //NOXLATE

        /// <summary>
        /// Default setting for using local previews
        /// </summary>
        public static bool DefaultUseLocalPreview { get { return true; } }

        /// <summary>
        /// Default color for open resources in the Site Explorer
        /// </summary>
        public static Color DefaultOpenColor { get { return Color.LightGreen; } }

        /// <summary>
        /// Default color for unsaved resources in the Site Explorer
        /// </summary>
        public static Color DefaultDirtyColor { get { return Color.Pink; } }

        /// <summary>
        /// Default setting for "Show Messages"
        /// </summary>
        public static bool DefaultShowMessages { get { return true; } }

        /// <summary>
        /// Default setting for "Show Outbound Requests"
        /// </summary>
        public static bool DefaultShowOutboundRequests { get { return true; } }

        /// <summary>
        /// Default setting for "Validate Resources on Save"
        /// </summary>
        public static bool DefaultValidateOnSave { get { return true; } }

        /// <summary>
        /// Default MapGuide XML schema path
        /// </summary>
        public static string DefaultXsdSchemaPath { get { return Path.Combine(FileUtility.ApplicationRootPath, "Schemas"); } } //NOXLATE

        /// <summary>
        /// Default preview viewer type
        /// </summary>
        public static string DefaultPreviewViewerType { get { return "AJAX"; } } //NOXLATE

        /// <summary>
        /// Default user template directory
        /// </summary>
        public static string DefaultUserTemplatesDirectory { get { return Path.Combine(FileUtility.ApplicationRootPath, "UserTemplates"); } } //NOXLATE
    }
}
