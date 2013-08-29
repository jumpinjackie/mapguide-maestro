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
#endregion
using System;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// Commonly used string constants for MapGuide
    /// </summary>
    public class StringConstants
    {
        /// <summary>
        /// The root resource identifier
        /// </summary>
        public const string RootIdentifier = "Library://"; //NOXLATE

        /// <summary>
        /// A thumbnail dwf resource role
        /// </summary>
        public const string Thumbnail = "Thumbnail"; //NOXLATE

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
    /// Represents the common resource types in MapGuide
    /// </summary>
	public enum ResourceTypes
	{
        /// <summary>
        /// Map Definition
        /// </summary>
		MapDefinition,
        /// <summary>
        /// Layer Definition
        /// </summary>
		LayerDefinition,
        /// <summary>
        /// Feature Source
        /// </summary>
		FeatureSource,
        /// <summary>
        /// Web Layout
        /// </summary>
		WebLayout,
        /// <summary>
        /// Runtime Map
        /// </summary>
		RuntimeMap,
        /// <summary>
        /// Folder
        /// </summary>
		Folder,
        /// <summary>
        /// Fusion Flexible Layout
        /// </summary>
		ApplicationDefinition,
        /// <summary>
        /// Print Layout
        /// </summary>
        PrintLayout,
        /// <summary>
        /// Symbol Definition
        /// </summary>
        SymbolDefinition,
        /// <summary>
        /// Load Procedure
        /// </summary>
        LoadProcedure,
        /// <summary>
        /// Drawing Source
        /// </summary>
        DrawingSource,
        /// <summary>
        /// DWF-based Symbol Library
        /// </summary>
        SymbolLibrary,
        /// <summary>
        /// A watermark
        /// </summary>
        WatermarkDefinition,
        /// <summary>
        /// A selection for a runtime map
        /// </summary>
        Selection
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
        public ResourceEventArgs(string resourceID) { this.ResourceID = resourceID; }

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
        private static ResourceTypes[] _resTypes;

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

	internal class EnumHelper
	{
		private static string[] ResourceTypeNames = new string[]
		{
			"MapDefinition", //NOXLATE
			"LayerDefinition", //NOXLATE
			"FeatureSource", //NOXLATE
			"WebLayout", //NOXLATE
			"Map", //NOXLATE
			string.Empty, //NOXLATE
			"ApplicationDefinition", //NOXLATE
		};

		internal static string ResourceName(ResourceTypes type)
		{
			return ResourceName(type, false);
		}

		internal static string ResourceName(ResourceTypes type, bool prefixWithDot)
		{
				if (type == ResourceTypes.Folder || !prefixWithDot)
					return ResourceTypeNames[(int)type];
				else
                    return "." + ResourceTypeNames[(int)type]; //NOXLATE
		}

	}

    /// <summary>
    /// Flags that can be used for the QueryMapFeatures operation
    /// </summary>
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
