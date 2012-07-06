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

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// Defines the capabilities of this connection. This is used to safely access supported
    /// features of the connection.
    /// </summary>
    public interface IConnectionCapabilities
    {
        /// <summary>
        /// Gets the highest supported resource version.
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        Version GetMaxSupportedResourceVersion(ResourceTypes resourceType);

        /// <summary>
        /// Indicates whether the current connection can be used between multiple threads
        /// </summary>
        bool IsMultithreaded { get; }

        /// <summary>
        /// Gets an array of supported commands
        /// </summary>
        int[] SupportedCommands { get; }

        /// <summary>
        /// Gets an array of supported services
        /// </summary>
        int[] SupportedServices { get; }

        /// <summary>
        /// Indicates whether web-based previewing capabilities are possible with this connection
        /// </summary>
        bool SupportsResourcePreviews { get; }

        /// <summary>
        /// Gets whether this connection supports publishing resources for WFS
        /// </summary>
        bool SupportsWfsPublishing { get; }

        /// <summary>
        /// Gets whether this connection supports publishing resources for WMS
        /// </summary>
        bool SupportsWmsPublishing { get; }

        /// <summary>
        /// Gets whether this connection supports the concept of resource headers
        /// </summary>
        bool SupportsResourceHeaders { get; }

        /// <summary>
        /// Gets whether this connection supports resource reference tracking
        /// </summary>
        bool SupportsResourceReferences { get; }

        /// <summary>
        /// Gets whether this connection supports resource security
        /// </summary>
        bool SupportsResourceSecurity { get; }

        /// <summary>
        /// Indicates if this current connection supports the specified resource type
        /// </summary>
        /// <param name="resType"></param>
        /// <returns></returns>
        bool IsSupportedResourceType(string resType);

        /// <summary>
        /// Indicates if this current connection supports the specified resource type
        /// </summary>
        /// <param name="resType"></param>
        /// <returns></returns>
        bool IsSupportedResourceType(ResourceTypes resType);
    }
}
