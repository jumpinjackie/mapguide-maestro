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
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;

namespace OSGeo.MapGuide.MaestroAPI.Services
{
    /// <summary>
    /// Provides services for the construction of Fusion web applications
    /// </summary>
    /// <example>
    /// This example shows how to obtain a fusion service instance. Note that you should check if this service type is
    /// supported through its capabilities.
    /// <code>
    /// <![CDATA[
    /// IServerConnection conn;
    /// ...
    /// IFusionService fusionSvc = (IFusionService)conn.GetService((int)ServiceType.Fusion);
    /// ]]>
    /// </code>
    /// </example>
    public interface IFusionService : IService
    {
        /// <summary>
        /// Gets the application templates.
        /// </summary>
        /// <returns></returns>
        IApplicationDefinitionTemplateInfoSet GetApplicationTemplates();

        /// <summary>
        /// Returns the avalible application widgets on the server
        /// </summary>
        /// <returns>The avalible application widgets on the server</returns>
        IApplicationDefinitionWidgetInfoSet GetApplicationWidgets();

        /// <summary>
        /// Returns the avalible widget containers on the server
        /// </summary>
        /// <returns>The avalible widget containers on the server</returns>
        IApplicationDefinitionContainerInfoSet GetApplicationContainers();
    }
}
