#region Disclaimer / License

// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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

using OSGeo.MapGuide.ObjectModels;
using System;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// Commonly used string constants for MapGuide
    /// </summary>
    public static class StringConstants
    {
        /// <summary>
        /// The root resource identifier
        /// </summary>
        public const string RootIdentifier = "Library://"; //NOXLATE

        /// <summary>
        /// A thumbnail dwf resource role
        /// </summary>
        public const string Thumbnail = nameof(Thumbnail);

        /// <summary>
        /// The username login placeholder token
        /// </summary>
        public const string MgUsernamePlaceholder = "%MG_USERNAME%"; //NOXLATE

        /// <summary>
        /// The password login placeholder token
        /// </summary>
        public const string MgPasswordPlaceholder = "%MG_PASSWORD%"; //NOXLATE

        /// <summary>
        /// The placeholder token that resolves to the path containing resource data
        /// </summary>
        public const string MgDataFilePath = "%MG_DATA_FILE_PATH%"; //NOXLATE

        /// <summary>
        /// The name of the resource data item containing the secured Feature Source credentials
        /// </summary>
        public const string MgUserCredentialsResourceData = "MG_USER_CREDENTIALS"; //NOXLATE

        /// <summary>
        /// The file picker filter for all files
        /// </summary>
        public static string AllFilesFilter
        {
            get
            {
                return string.Format(Strings.GenericFilter, Strings.PickAll, "*");
            }
        }
    }

    /// <summary>
    /// Represents a method involving a resource id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ResourceEventHandler(object sender, ResourceEventArgs e);

    /// <summary>
    /// Contains the resource id
    /// </summary>
    public class ResourceEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="resourceID"></param>
        public ResourceEventArgs(string resourceID)
        {
            this.ResourceID = resourceID;
        }

        /// <summary>
        /// Gets or sets the resource id
        /// </summary>
        public string ResourceID { get; set; }
    }

    /// <summary>
    /// Helper class to filter <see cref="ResourceTypes"/> into the ones that are
    /// editable
    /// </summary>
    public static class SiteResourceType
    {
        private static readonly ResourceTypes[] _resTypes;

        static SiteResourceType()
        {
            _resTypes = (ResourceTypes[])Enum.GetValues(typeof(ResourceTypes));
        }

        /// <summary>
        /// Returns an array of all editable <see cref="ResourceTypes"/> values
        /// </summary>
        /// <returns></returns>
        public static ResourceTypes[] All()
        {
            return _resTypes;
        }
    }

    /// <summary>
    /// Flags that can be used for the QueryMapFeatures operation
    /// </summary>
    [Flags]
    public enum QueryMapFeaturesLayerAttributes : int
    {
        /// <summary>
        /// All layers
        /// </summary>
        AllLayers = 0,

        /// <summary>
        /// Only visible
        /// </summary>
        OnlyVisible = 1,

        /// <summary>
        /// Only selectable
        /// </summary>
        OnlySelectable = 2,

        /// <summary>
        /// Default
        /// </summary>
        Default = 3,

        /// <summary>
        /// Only with tooltips
        /// </summary>
        OnlyWithToolTips = 4,

        /// <summary>
        /// Visible with tooltips
        /// </summary>
        VisibleWithToolTips = 5
    }

    /// <summary>
    /// Defines the types of unmananged data
    /// </summary>
    public enum UnmanagedDataTypes : int
    {
        /// <summary>
        /// Files
        /// </summary>
        Files,

        /// <summary>
        /// Folders
        /// </summary>
        Folders,

        /// <summary>
        /// Files and Folders
        /// </summary>
        Both
    }
}