#region Disclaimer / License
// Copyright (C) 2013, Jackie Ng
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
using OSGeo.MapGuide.ObjectModels.RuntimeMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.Commands
{
    /// <summary>
    /// Create a runtime map from the 
    /// </summary>
    public interface ICreateRuntimeMap : ICommand
    {
        /// <summary>
        /// The resource id of the Map Definition to create a Runtime Map from
        /// </summary>
        string MapDefinition { get; set; }

        /// <summary>
        /// The name to assign to the created runtime map. If null, then MapGuide will
        /// auto-generate one based on the Map Definition ID
        /// </summary>
        string TargetMapName { get; set; }

        /// <summary>
        /// A bitmask specifying what pieces of information to include in the CREATERUNTIMEMAP response.
        /// Use the <see cref="E:OSGeo.MapGuide.MaestroAPI.Commands.RuntimeMapRequest"/> to determine what
        /// valid values you can use.
        /// </summary>
        int RequestedFeatures { get; set; }

        /// <summary>
        /// The number of icons to render inline (as base64 images) per scale range in each layer of the map
        /// </summary>
        int IconsPerScaleRange { get; set; }

        /// <summary>
        /// The icon image format (default: PNG)
        /// </summary>
        string IconFormat { get; set; }

        /// <summary>
        /// The width of each inline icon that will be rendered (default: 16)
        /// </summary>
        int IconWidth { get; set; }

        /// <summary>
        /// The height of each inline icon that will be rendered (default: 16)
        /// </summary>
        int IconHeight { get; set; }

        /// <summary>
        /// Executes the request and returns the structure of the created runtime map
        /// </summary>
        /// <returns>The structure of the runtime map</returns>
        IRuntimeMapInfo Execute();
    }

    /// <summary>
    /// A bitmask that defines what information to include in a CreateRuntimeMap or DescribeRuntimeMap
    /// response.
    /// </summary>
    public enum RuntimeMapRequestedFeatures : int
    {
        /// <summary>
        /// No features
        /// </summary>
        None = 0,
        /// <summary>
        /// Include layer and group structure
        /// </summary>
        LayersAndGroups = 1,
        /// <summary>
        /// Include inline icons for each layer. Does nothing if <see cref="E:OSGeo.MapGuide.MaestroAPI.Commands.RuntimeMapRequest.LayersAndGroups"/>
        /// is not included in the mask
        /// </summary>
        Icons = 2,
        /// <summary>
        /// Include feature source information for each layer. Does nothing if <see cref="E:OSGeo.MapGuide.MaestroAPI.Commands.RuntimeMapRequest.LayersAndGroups"/>
        /// is not included in the mask
        /// </summary>
        FeatureSourceInformation = 4
    }
}
