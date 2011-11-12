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
using OSGeo.MapGuide.ObjectModels.Common;

namespace OSGeo.MapGuide.MaestroAPI.Services
{
    /// <summary>
    /// Service interface for site operations
    /// </summary>
    /// <example>
    /// This example shows how to obtain a site service instance. Note that you should check if this service type is
    /// supported through its capabilities.
    /// <code>
    /// <![CDATA[
    /// IServerConnection conn;
    /// ...
    /// ISiteService siteSvc = (ISiteService)conn.GetService((int)ServiceType.Site);
    /// ]]>
    /// </code>
    /// </example>
    public interface ISiteService : IService
    {
        /// <summary>
        /// Enumerates the users.
        /// </summary>
        /// <returns></returns>
        UserList EnumerateUsers();

        /// <summary>
        /// Enumerates the users.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        UserList EnumerateUsers(string group);

        /// <summary>
        /// Enumerates the groups.
        /// </summary>
        /// <returns></returns>
        GroupList EnumerateGroups();

        /// <summary>
        /// Gets the site info.
        /// </summary>
        /// <returns></returns>
        SiteInformation GetSiteInfo();
    }
}
