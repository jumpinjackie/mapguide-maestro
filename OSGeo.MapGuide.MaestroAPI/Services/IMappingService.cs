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
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.MaestroAPI.Mapping;

namespace OSGeo.MapGuide.MaestroAPI.Services
{
    /// <summary>
    /// Provides services for interaction with the runtime map
    /// </summary>
    public interface IMappingService : IService
    {
        /// <summary>
        /// Creates a new runtime map instance from an existing map definition. Meters per unit
        /// is calculated from the Coordinate System WKT of the map definition.
        /// </summary>
        /// <remarks>
        /// Calculation of meters-per-unit may differ between implementations. This may have an adverse
        /// effect on things such as rendering and measuring depending on the underlying implementation
        /// 
        /// If you are certain of the meters-per-unit value required, use the overloaded method that 
        /// accepts a metersPerUnit parameter.
        /// </remarks>
        /// <param name="runtimeMapResourceId"></param>
        /// <param name="baseMapDefinitionId"></param>
        /// <returns></returns>
        RuntimeMap CreateMap(string runtimeMapResourceId, string baseMapDefinitionId);

        /// <summary>
        /// Creates a new runtime map instance from an existing map definition
        /// </summary>
        /// <param name="runtimeMapResourceId"></param>
        /// <param name="baseMapDefinitionId"></param>
        /// <param name="metersPerUnit"></param>
        /// <returns></returns>
        RuntimeMap CreateMap(string runtimeMapResourceId, string baseMapDefinitionId, double metersPerUnit);

        /// <summary>
        /// Creates a new runtime map instance from an existing map definition. Meters per unit
        /// is calculated from the Coordinate System WKT of the map definition.
        /// </summary>
        /// <remarks>
        /// Calculation of meters-per-unit may differ between implementations. This may have an adverse
        /// effect on things such as rendering and measuring depending on the underlying implementation
        /// 
        /// If you are certain of the meters-per-unit value required, use the overloaded method that 
        /// accepts a metersPerUnit parameter.
        /// </remarks>
        /// <param name="runtimeMapResourceId"></param>
        /// <param name="mdf"></param>
        /// <returns></returns>
        RuntimeMap CreateMap(string runtimeMapResourceId, IMapDefinition mdf);

        /// <summary>
        /// Creates a new runtime map instance from an existing map definition
        /// </summary>
        /// <param name="runtimeMapResourceId"></param>
        /// <param name="mdf"></param>
        /// <param name="metersPerUnit"></param>
        /// <returns></returns>
        RuntimeMap CreateMap(string runtimeMapResourceId, IMapDefinition mdf, double metersPerUnit);

        /// <summary>
        /// Opens an existing runtime map instance
        /// </summary>
        /// <param name="runtimeMapResourceId"></param>
        /// <returns></returns>
        RuntimeMap OpenMap(string runtimeMapResourceId);

        /// <summary>
        /// Renders a dynamic overlay image of the map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="selection"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        System.IO.Stream RenderDynamicOverlay(RuntimeMap map, MapSelection selection, string format);

        /// <summary>
        /// Renders a dynamic overlay image of the map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="selection"></param>
        /// <param name="format"></param>
        /// <param name="keepSelection"></param>
        /// <returns></returns>
        System.IO.Stream RenderDynamicOverlay(RuntimeMap map, MapSelection selection, string format, bool keepSelection);

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(string resourceId, double x, double y, double scale, int width, int height, int dpi);
        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(string resourceId, double x1, double y1, double x2, double y2, int width, int height, int dpi);

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(string resourceId, double x, double y, double scale, int width, int height, int dpi, string format);
        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(string resourceId, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format);

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <param name="clip">if set to <c>true</c> [clip].</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(string resourceId, double x, double y, double scale, int width, int height, int dpi, string format, bool clip);
        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <param name="clip">if set to <c>true</c> [clip].</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(string resourceId, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format, bool clip);
        /*
        /// <summary>
        /// Creates a runtime map on the server. 
        /// The map name will be the name of the resource, without path information.
        /// This is equivalent to the way the AJAX viewer creates the runtime map.
        /// </summary>
        /// <param name="resourceID">The mapDefinition resource id</param>
        void CreateRuntimeMap(string resourceID);

        /// <summary>
        /// Creates a runtime map on the server
        /// </summary>
        /// <param name="resourceID">The target resource id for the runtime map</param>
        /// <param name="mapdefinition">The mapdefinition to base the map on</param>
        void CreateRuntimeMap(string resourceID, string mapdefinition);

        /// <summary>
        /// Creates a runtime map on the server
        /// </summary>
        /// <param name="resourceID">The target resource id for the runtime map</param>
        /// <param name="map">The mapdefinition to base the map on</param>
        void CreateRuntimeMap(string resourceID, IMapDefinition map);

        /// <summary>
        /// Creates a runtime map on the server
        /// </summary>
        /// <param name="resourceID">The target resource id for the runtime map</param>
        /// <param name="map">The mapdefinition to base the map on</param>
        void CreateRuntimeMap(string resourceID, RuntimeMapBase map);

        /// <summary>
        /// Updates an existing runtime map
        /// </summary>
        /// <param name="resourceID">The target resource id for the runtime map</param>
        /// <param name="map">The runtime map to update with</param>
        void SaveRuntimeMap(string resourceID, RuntimeMapBase map);

        /// <summary>
        /// Gets the runtime map.
        /// </summary>
        /// <param name="resourceID">The resource ID.</param>
        /// <returns></returns>
        RuntimeMapBase GetRuntimeMap(string resourceID);

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(string resourceId, double x, double y, double scale, int width, int height, int dpi);
        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(string resourceId, double x1, double y1, double x2, double y2, int width, int height, int dpi);

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(string resourceId, double x, double y, double scale, int width, int height, int dpi, string format);
        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(string resourceId, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format);

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <param name="clip">if set to <c>true</c> [clip].</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(string resourceId, double x, double y, double scale, int width, int height, int dpi, string format, bool clip);
        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <param name="clip">if set to <c>true</c> [clip].</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(string resourceId, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format, bool clip);

        /// <summary>
        /// Sets the selection of a map
        /// </summary>
        /// <param name="runtimeMap">The resourceID of the runtime map</param>
        /// <param name="selectionXml">The selection xml</param>
        void SetSelectionXml(string runtimeMap, string selectionXml);

        /// <summary>
        /// Gets the selection from a map
        /// </summary>
        /// <param name="runtimeMap">The resourceID of the runtime map</param>
        /// <returns>The selection xml</returns>
        string GetSelectionXml(string runtimeMap);


        /// <summary>
        /// Selects features from a runtime map, returning a selection Xml.
        /// </summary>
        /// <param name="runtimemap">The map to query. NOT a resourceID, only the map name!</param>
        /// <param name="wkt">The WKT of the geometry to query with (always uses intersection)</param>
        /// <param name="persist">True if the selection should be saved in the runtime map, false otherwise.</param>
        /// <param name="attributes">The type of layer to include in the query</param>
        /// <param name="raw">True if the result should contain the tooltip and link info</param>
        /// <returns>The selection Xml, or an empty string if there were no data.</returns>
        string QueryMapFeatures(string runtimemap, string wkt, bool persist, QueryMapFeaturesLayerAttributes attributes, bool raw);

        /// <summary>
        /// Selects features from a runtime map, returning a selection Xml.
        /// </summary>
        /// <param name="runtimemap">The map to query. NOT a resourceID, only the map name!</param>
        /// <param name="wkt">The WKT of the geometry to query with (always uses intersection)</param>
        /// <param name="persist">True if the selection should be saved in the runtime map, false otherwise.</param>
        /// <returns>The selection Xml, or an empty string if there were no data.</returns>
        string QueryMapFeatures(string runtimemap, string wkt, bool persist);

        /// <summary>
        /// Renders a minature bitmap of the layers style
        /// </summary>
        /// <param name="scale">The scale for the bitmap to match</param>
        /// <param name="layerdefinition">The layer the image should represent</param>
        /// <param name="themeIndex">If the layer is themed, this gives the theme index, otherwise set to 0</param>
        /// <param name="type">The geometry type, 1 for point, 2 for line, 3 for area, 4 for composite</param>
        /// <returns>The minature bitmap</returns>
        System.Drawing.Image GetLegendImage(double scale, string layerdefinition, int themeIndex, int type);
        */
    }
}
