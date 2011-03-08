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
        SymbolLibrary
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
            _resTypes = new ResourceTypes[]
            {
                ResourceTypes.ApplicationDefinition,
                ResourceTypes.DrawingSource,
                ResourceTypes.FeatureSource,
                ResourceTypes.LayerDefinition,
                ResourceTypes.LoadProcedure,
                ResourceTypes.MapDefinition,
                ResourceTypes.PrintLayout,
                ResourceTypes.SymbolDefinition,
                ResourceTypes.SymbolLibrary,
                ResourceTypes.WebLayout
            };
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
			"MapDefinition",
			"LayerDefinition",
			"FeatureSource",
			"WebLayout",
			"Map",
			"",
			"ApplicationDefinition",
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
					return "." + ResourceTypeNames[(int)type];
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
