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
    /// Allows low level access to DWF (Design Web Format) data stored in a resource repository as part of a drawing source. 
    /// </summary>
    /// <example>
    /// This example shows how to obtain a drawing service instance. Note that you should check if this service type is
    /// supported through its capabilities.
    /// <code>
    /// <![CDATA[
    /// IServerConnection conn;
    /// ...
    /// IDrawingService drawingSvc = (IDrawingService)conn.GetService((int)ServiceType.Drawing);
    /// ]]>
    /// </code>
    /// </example>
    public interface IDrawingService : IService
    {
        /// <summary>
        /// Gets the manifest.xml document which describes the supported document interfaces, the document properties, the sections and their contents, and section dependencies. 
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        System.IO.Stream DescribeDrawing(string resourceID);

        /// <summary>
        /// Gets the names of the layers in a DWF section. 
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        string[] EnumerateDrawingLayers(string resourceID, string sectionName);

        /// <summary>
        /// Enumerates the resources of a DWF section (sometimes called a sheet). 
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        DrawingSectionResourceList EnumerateDrawingSectionResources(string resourceID, string sectionName);

        /// <summary>
        /// Enumerates only the ePlot  sections (sheets) in a DWF. 
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        DrawingSectionList EnumerateDrawingSections(string resourceID);

        /// <summary>
        /// Gets the coordinate system assigned to the DWF drawing. 
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        string GetDrawingCoordinateSpace(string resourceID);

        /// <summary>
        /// Returns the DWF stream for a drawing specified by resource identifier. 
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        System.IO.Stream GetDrawing(string resourceID);

        /// <summary>
        /// Gets a layer from a particular section of a DWF. 
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="sectionName"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        System.IO.Stream GetLayer(string resourceID, string sectionName, string layerName);

        /// <summary>
        /// Gets a DWF containing only the requested section (sometimes called a sheet). 
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        System.IO.Stream GetSection(string resourceID, string sectionName);

        /// <summary>
        /// Gets a specific resource from the DWF. 
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        System.IO.Stream GetSectionResource(string resourceID, string resourceName);
    }
}
