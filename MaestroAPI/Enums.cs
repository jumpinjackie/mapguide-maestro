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
	public enum ResourceTypes
	{
		MapDefinition,
		LayerDefiniton,
		FeatureSource,
		WebLayout,
		RuntimeMap,
		Folder,
		ApplicationDefinition,
	}

	public enum Operations
	{
		CreateSession,
		DescribeDrawing,
		GetDrawing,
		EnumerateDrawingLayers,
		GetDrawingLayer,
		GetDrawingSection,
		GetDrawingSectionResource,
		EnumerateDrawingSections,
		EnumerateDrawingSectionResources,
		GetDrawingCoordinateSpace,
		GetFeatureProviders,
		GetProviderCapabilities,
		GetConnectionPropertyValues,
		DescribeFeatureSchema,
		SelectFeatures,
		SelectAggregates,
		ExecuteSqlQuery,
		GetSpatialContexts,
		GetLongTransactions,
		EnumerateDataStores,
		GetSchemaMapping,
		GetSchemas,
		GetClasses,
		GetClassDefinition,
		GetIdentityProperties,
		TestConnection,
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

	public enum QueryMapFeaturesLayerAttributes : int
	{
		AllLayers = 0,
		OnlyVisible = 1,
		OnlySelectable = 2,
		Default = 3,
		OnlyWithToolTips = 4,
		VisibleWithToolTips = 5
	}

	public enum UnmanagedDataTypes : int
	{
		Files,
		Folders,
		Both
	}
}
