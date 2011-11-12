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
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace OSGeo.MapGuide.MaestroAPI.Commands
{
    /// <summary>
    /// Defines a command to get a set of resource contents in a single batch
    /// </summary>
    /// <example>
    /// This example shows how invoke the <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.IGetResourceContents"/>
    /// command. Note that you should check if the connection supports this command through its capabilities
    /// <code>
    /// <![CDATA[
    /// IServerConnection conn;
    /// ...
    /// IGetResourceContents command = (IGetResourceContents)conn.CreateCommand(CommandType.GetResourceContents);
    /// string [] resourceIds = new string[] {
    ///     "Library://Samples/Sheboygan/Layers/Parcels.LayerDefinition",
    ///     "Library://Samples/Sheboygan/Layers/Rail.LayerDefinition",
    ///     "Library://Samples/Sheboygan/Layers/HydrographicPolygons.LayerDefinition",
    ///     "Library://Samples/Sheboygan/Layers/CityLimits.LayerDefinition",
    ///     "Library://Samples/Sheboygan/Layers/Buildings.LayerDefinition",
    ///     "Library://Samples/Sheboygan/Layers/Roads.LayerDefinition"
    /// };
    /// Dictionary<string, IResource> results = command.Execute(resourceIds);
    /// ]]>
    /// </code>
    /// </example>
    public interface IGetResourceContents : ICommand
    {
        /// <summary>
        /// Gets the resource content of the specified resources
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        Dictionary<string, IResource> Execute(IEnumerable<string> resourceIds);
    }
}
