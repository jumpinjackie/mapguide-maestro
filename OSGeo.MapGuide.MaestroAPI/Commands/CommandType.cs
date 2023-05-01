﻿#region Disclaimer / License

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

namespace OSGeo.MapGuide.MaestroAPI.Commands
{
    /// <summary>
    /// Defines the custom command types
    /// </summary>
    public enum CommandType : int
    {
        /// <summary>
        /// Gets a collection of resource content in a single batch
        /// </summary>
        GetResourceContents,

        /// <summary>
        /// Applies a feature schema to a feature source
        /// </summary>
        ApplySchema,

        /// <summary>
        /// Inserts a feature into a feature source
        /// </summary>
        InsertFeature,

        /// <summary>
        /// Updates features in a feature source
        /// </summary>
        UpdateFeatures,

        /// <summary>
        /// Deletes features from a feature source
        /// </summary>
        DeleteFeatures,

        /// <summary>
        /// Creates a data store on a feature source
        /// </summary>
        CreateDataStore,

        /// <summary>
        /// Retrieves information about cached FDO connections
        /// </summary>
        GetFdoCacheInfo,

        /// <summary>
        /// Creates a new Runtime Map and describe its structure
        /// </summary>
        CreateRuntimeMap,

        /// <summary>
        /// Describes the structure of an existing runtime map
        /// </summary>
        DescribeRuntimeMap,

        /// <summary>
        /// Enumerates registered Tile Set Providers
        /// </summary>
        GetTileProviders,

        /// <summary>
        /// Gets WFS capabilities
        /// </summary>
        GetWfsCapabilities,

        /// <summary>
        /// Gets WMS capabilities
        /// </summary>
        GetWmsCapabilities
    }
}