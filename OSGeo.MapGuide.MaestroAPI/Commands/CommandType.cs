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
        GetFdoCacheInfo
    }
}
