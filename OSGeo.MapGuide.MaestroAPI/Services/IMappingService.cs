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
using System.Drawing;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace OSGeo.MapGuide.MaestroAPI.Services
{
    /// <summary>
    /// Provides services for interaction with the runtime map
    /// </summary>
    /// <example>
    /// This example shows how to obtain a mapping service instance. Note that you should check if this service type is
    /// supported through its capabilities.
    /// <code>
    /// <![CDATA[
    /// IServerConnection conn;
    /// ...
    /// IMappingService mappingSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);
    /// ]]>
    /// </code>
    /// </example>
    public interface IMappingService : IService
    {
        /// <summary>
        /// Creates the map group.
        /// </summary>
        /// <param name="parent">The parent runtime map. The runtime map must have been created or opened from this same service instance</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        RuntimeMapGroup CreateMapGroup(RuntimeMap parent, string name);

        /// <summary>
        /// Creates a new runtime map group
        /// </summary>
        /// <param name="parent">The parent runtime map. The runtime map must have been created or opened from this same service instance</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        RuntimeMapGroup CreateMapGroup(RuntimeMap parent, IBaseMapGroup group);

        /// <summary>
        /// Creates a new runtime map group
        /// </summary>
        /// <param name="parent">The parent runtime map. The runtime map must have been created or opened from this same service instance</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        RuntimeMapGroup CreateMapGroup(RuntimeMap parent, IMapLayerGroup group);

        /// <summary>
        /// Creates a new runtime map layer from the specified Layer Definition
        /// </summary>
        /// <param name="parent">The parent runtime map. The runtime map must have been created or opened from this same service instance</param>
        /// <param name="ldf">The layer definition</param>
        /// <returns></returns>
        RuntimeMapLayer CreateMapLayer(RuntimeMap parent, ILayerDefinition ldf);

        /// <summary>
        /// Creates a new runtime map layer from the specified <see cref="T:OSGeo.MapGuide.ObjectModels.MapDefinition.IBaseMapLayer"/> instance
        /// </summary>
        /// <param name="parent">The parent runtime map. The runtime map must have been created or opened from this same service instance</param>
        /// <param name="source">The map definition layer</param>
        /// <returns></returns>
        RuntimeMapLayer CreateMapLayer(RuntimeMap parent, IBaseMapLayer source);

        /// <summary>
        /// Creates a new runtime map layer from the specified <see cref="T:OSGeo.MapGuide.ObjectModels.MapDefinition.IMapLayer"/> instance
        /// </summary>
        /// <param name="parent">The parent runtime map. The runtime map must have been created or opened from this same service instance</param>
        /// <param name="source">The map definition layer</param>
        /// <returns></returns>
        RuntimeMapLayer CreateMapLayer(RuntimeMap parent, IMapLayer source);

        /// <summary>
        /// Creates a new runtime map instance from an existing map definition.
        /// </summary>
        /// <param name="mapDef"></param>
        /// <returns></returns>
        RuntimeMap CreateMap(IMapDefinition mapDef);

        /// <summary>
        /// Creates a new runtime map instance from an existing map definition. Meters per unit
        /// is calculated from the Coordinate System WKT of the map definition.
        /// </summary>
        /// <param name="mapDef"></param>
        /// <param name="metersPerUnit"></param>
        /// <returns></returns>
        RuntimeMap CreateMap(IMapDefinition mapDef, double metersPerUnit);

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
        [Obsolete("Use the version of RenderDynamicOverlay that is not marked Obsolete")] //NOXLATE
        System.IO.Stream RenderDynamicOverlay(RuntimeMap map, MapSelection selection, string format);

        /// <summary>
        /// Renders a dynamic overlay image of the map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="selection"></param>
        /// <param name="format"></param>
        /// <param name="keepSelection"></param>
        /// <returns></returns>
        [Obsolete("Use the version of RenderDynamicOverlay that is not marked Obsolete")] //NOXLATE
        System.IO.Stream RenderDynamicOverlay(RuntimeMap map, MapSelection selection, string format, bool keepSelection);

        /// <summary>
        /// Renders a dynamic overlay image of the map
        /// </summary>
        /// <param name="map">The runtime map instance</param>
        /// <param name="selection">The map selection</param>
        /// <param name="format">The image format</param>
        /// <param name="selectionColor">The color of the selection</param>
        /// <param name="behaviour">The rendering behaviour</param>
        /// <returns></returns>
        /// <exception cref="T:System.NotSupportedException">Thrown if the service is too old to be able to support this API</exception>
        System.IO.Stream RenderDynamicOverlay(RuntimeMap map, MapSelection selection, string format, Color selectionColor, int behaviour);

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="map">The runtime map instance</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x, double y, double scale, int width, int height, int dpi);
        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="map">The runtime map instance.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x1, double y1, double x2, double y2, int width, int height, int dpi);

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="map">The runtime map instance.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x, double y, double scale, int width, int height, int dpi, string format);
        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="map">The runtime map instance.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format);

        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="map">The runtime map instance.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpi">The dpi.</param>
        /// <param name="format">The format.</param>
        /// <param name="clip">if set to <c>true</c> [clip].</param>
        /// <returns></returns>
        System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x, double y, double scale, int width, int height, int dpi, string format, bool clip);
        /// <summary>
        /// Renders the runtime map.
        /// </summary>
        /// <param name="map">The runtime map</param>
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
        System.IO.Stream RenderRuntimeMap(RuntimeMap map, double x1, double y1, double x2, double y2, int width, int height, int dpi, string format, bool clip);

        /// <summary>
        /// Renders the legend for the specified <see cref="RuntimeMap"/> to the requested size and format
        /// </summary>
        /// <param name="map"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        System.IO.Stream RenderMapLegend(RuntimeMap map, int width, int height, Color backgroundColor, string format);

        /// <summary>
        /// Renders a minature bitmap of the layers style
        /// </summary>
        /// <param name="scale">The scale for the bitmap to match</param>
        /// <param name="layerdefinition">The layer definition resource id</param>
        /// <param name="themeIndex">If the layer is themed, this gives the theme index, otherwise set to 0</param>
        /// <param name="type">The geometry type, 1 for point, 2 for line, 3 for area, 4 for composite</param>
        /// <returns>The minature bitmap</returns>
        System.Drawing.Image GetLegendImage(double scale, string layerdefinition, int themeIndex, int type);

        /// <summary>
        ///Renders a minature bitmap of the layers style
        /// </summary>
        /// <param name="scale">The scale for the bitmap to match</param>
        /// <param name="layerdefinition">The layer definition resource id</param>
        /// <param name="themeIndex">If the layer is themed, this gives the theme index, otherwise set to 0</param>
        /// <param name="type">The geometry type, 1 for point, 2 for line, 3 for area, 4 for composite</param>
        /// <param name="width">The width of the image to request.</param>
        /// <param name="height">The height of the image to request.</param>
        /// <param name="format">The image format (PNG, JPG or GIF).</param>
        /// <returns></returns>
        System.Drawing.Image GetLegendImage(double scale, string layerdefinition, int themeIndex, int type, int width, int height, string format);

        /// <summary>
        /// Identifies features that meet the specified spatial selection criteria. These features can be persisted as selected features in a map. QueryMapFeatures returns an XML document describing the set of selected features. If a single feature is selected, the XML contains the tooltip, hyperlink, and properties of the feature.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="maxFeatures"></param>
        /// <param name="wkt"></param>
        /// <param name="persist"></param>
        /// <param name="selectionVariant"></param>
        /// <param name="extraOptions"></param>
        /// <returns></returns>
        string QueryMapFeatures(RuntimeMap map, int maxFeatures, string wkt, bool persist, string selectionVariant, QueryMapOptions extraOptions);
    }
}
