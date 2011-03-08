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
using OSGeo.MapGuide.ObjectModels.Common;
using System.Drawing;
using System.ComponentModel;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.ObjectModels.MapDefinition
{
    /// <summary>
    /// Represents the base interface of map definitions and their runtime forms
    /// </summary>
    public interface IMapDefinitionBase
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the coordinate system. Layers whose coordinate system does
        /// not match will be re-projecte to this coordinate system when rendering
        /// </summary>
        /// <value>The coordinate system.</value>
        string CoordinateSystem { get; }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        Color BackgroundColor { get; set; }
    }

    /// <summary>
    /// Represents a Map Definition
    /// </summary>
    public interface IMapDefinition : IResource, IMapDefinitionBase, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the coordinate system. Layers whose coordinate system does
        /// not match will be re-projected to this coordinate system when rendering
        /// </summary>
        /// <value>The coordinate system.</value>
        string CoordinateSystem { get; set; }

        /// <summary>
        /// Gets or sets the extents.
        /// </summary>
        /// <value>The extents.</value>
        IEnvelope Extents { get; set; }

        /// <summary>
        /// Sets the extents.
        /// </summary>
        /// <param name="minx">The minx.</param>
        /// <param name="miny">The miny.</param>
        /// <param name="maxx">The maxx.</param>
        /// <param name="maxy">The maxy.</param>
        void SetExtents(double minx, double miny, double maxx, double maxy);

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        string Metadata { get; set; }

        /// <summary>
        /// Returns the base map section of this map definition. Ensure <see cref="InitBaseMap"/>
        /// is called first before accessing this property
        /// </summary>
        IBaseMapDefinition BaseMap { get; }

        /// <summary>
        /// Initializes the base map section of this map definition. Subsequent calls
        /// do nothing, unless you have cleared the section via <see cref="RemoveBaseMap"/>
        /// </summary>
        void InitBaseMap();

        /// <summary>
        /// Clears the base map section of this map definition. If you want to rebuild
        /// this section, ensure <see cref="InitBaseMap"/> is called
        /// </summary>
        void RemoveBaseMap();

        /// <summary>
        /// Gets the map layers.
        /// </summary>
        /// <value>The map layers.</value>
        IEnumerable<IMapLayer> MapLayer { get; }

        /// <summary>
        /// Adds a layer to this map. If this is the first layer to be added, the coordinate system 
        /// of this map and its extents will be set to the coordinate system and extents of this layer
        /// if this has not been set already.
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="layerName"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        IMapLayer AddLayer(string groupName, string layerName, string resourceId);

        /// <summary>
        /// Adds a layer to this map. If this is the first layer to be added, the coordinate system 
        /// of this map and its extents will be set to the coordinate system and extents of this layer
        /// if this has not been set already.
        /// </summary>
        /// <param name="layerToInsertAbove">The layer to insert above in the draw order</param>
        /// <param name="groupName"></param>
        /// <param name="layerName"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        IMapLayer AddLayer(IMapLayer layerToInsertAbove, string groupName, string layerName, string resourceId);

        /// <summary>
        /// Removes the layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        void RemoveLayer(IMapLayer layer);

        /// <summary>
        /// Gets the index of the specified layer
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        int GetIndex(IMapLayer layer);

        /// <summary>
        /// Moves the layer up the draw order
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        int MoveUp(IMapLayer layer);

        /// <summary>
        /// Moves the layer down the draw order.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        int MoveDown(IMapLayer layer);

        /// <summary>
        /// Gets the map layer groups.
        /// </summary>
        /// <value>The map layer groups.</value>
        IEnumerable<IMapLayerGroup> MapLayerGroup { get; }

        /// <summary>
        /// Adds the group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        IMapLayerGroup AddGroup(string groupName);

        /// <summary>
        /// Removes the group
        /// </summary>
        /// <param name="group"></param>
        void RemoveGroup(IMapLayerGroup group);

        /// <summary>
        /// Gets the index of the specified group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        int GetIndex(IMapLayerGroup group);

        /// <summary>
        /// Moves the specified layer to the top of the draw order
        /// </summary>
        /// <param name="layer"></param>
        void SetTopDrawOrder(IMapLayer layer);

        /// <summary>
        /// Moves the specified layer to the bottom of the draw order
        /// </summary>
        /// <param name="layer"></param>
        void SetBottomDrawOrder(IMapLayer layer);

        /// <summary>
        /// Inserts the layer at the specified index
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="layer"></param>
        void InsertLayer(int idx, IMapLayer layer);
    }

    /// <summary>
    /// Extension methdo class
    /// </summary>
    public static class BaseMapDefinitionExtensions
    {
        /// <summary>
        /// Gets the minimum finite display scale
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static double GetMinScale(this IBaseMapDefinition map)
        {
            Check.NotNull(map, "map");
            if (map.ScaleCount == 0)
                return 0.0;

            var scales = new List<double>(map.FiniteDisplayScale);
            scales.Sort();
            return scales[0];
        }

        /// <summary>
        /// Gets the maximum finite display scale
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static double GetMaxScale(this IBaseMapDefinition map)
        {
            Check.NotNull(map, "map");
            if (map.ScaleCount == 0)
                return 0.0;

            var scales = new List<double>(map.FiniteDisplayScale);
            scales.Sort();
            return scales[scales.Count - 1];
        }

        /// <summary>
        /// Gets whether this base map group has tiled layers
        /// </summary>
        /// <param name="grp"></param>
        /// <returns></returns>
        public static bool HasLayers(this IBaseMapGroup grp)
        {
            Check.NotNull(grp, "grp");
            return new List<IBaseMapLayer>(grp.BaseMapLayer).Count > 0;
        }

        /// <summary>
        /// Gets whether this base map has tiled layers
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static bool HasLayers(this IBaseMapDefinition map)
        {
            Check.NotNull(map, "map");
            if (!map.HasGroups())
                return false;

            foreach (var group in map.BaseMapLayerGroup)
            {
                if (group.HasLayers())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets whether this base map has groups
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static bool HasGroups(this IBaseMapDefinition map)
        {
            Check.NotNull(map, "map");
            return new List<IBaseMapGroup>(map.BaseMapLayerGroup).Count > 0;
        }

        /// <summary>
        /// Gets the first base map group
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static IBaseMapGroup GetFirstGroup(this IBaseMapDefinition map)
        {
            Check.NotNull(map, "map");
            var list = new List<IBaseMapGroup>(map.BaseMapLayerGroup);
            if (list.Count > 0)
                return list[0];
            return null;
        }

        /// <summary>
        /// Gets whether a tiled layer of the specified name exists.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static bool LayerExists(this IBaseMapDefinition map, string layerName)
        {
            Check.NotNull(map, "map");
            Check.NotEmpty(layerName, "layerName");

            foreach (var group in map.BaseMapLayerGroup)
            {
                foreach (var layer in group.BaseMapLayer)
                {
                    if (layerName.Equals(layer.Name))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the base map group of the specified name
        /// </summary>
        /// <param name="map"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static IBaseMapGroup GetGroup(this IBaseMapDefinition map, string groupName)
        { 
            Check.NotNull(map, "map");
            Check.NotEmpty(groupName, "groupName");
            foreach (var group in map.BaseMapLayerGroup)
            {
                if (groupName.Equals(group.Name))
                    return group;
            }
            return null;
        }

        /// <summary>
        /// Gets whether the specified base map group exists
        /// </summary>
        /// <param name="map"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static bool GroupExists(this IBaseMapDefinition map, string groupName)
        {
            Check.NotNull(map, "map");
            Check.NotEmpty(groupName, "groupName");
            foreach (var group in map.BaseMapLayerGroup)
            {
                if (groupName.Equals(group.Name))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the tiled layers for the specified base map group
        /// </summary>
        /// <param name="map"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static IEnumerable<IBaseMapLayer> GetLayersForGroup(this IBaseMapDefinition map, string groupName)
        {
            Check.NotNull(map, "map");
            Check.NotEmpty(groupName, "groupName");

            foreach (var group in map.BaseMapLayerGroup)
            {
                if (groupName.Equals(group.Name))
                {
                    return group.BaseMapLayer;
                }
            }

            return new IBaseMapLayer[0];
        }
    }

    /// <summary>
    /// Extension method class
    /// </summary>
    public static class MapDefinitionExtensions
    {
        /// <summary>
        /// Get a layer by its name
        /// </summary>
        /// <param name="map"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IMapLayer GetLayerByName(this IMapDefinition map, string name)
        {
            Check.NotNull(map, "map");
            Check.NotEmpty(name, "name");
            foreach (var layer in map.MapLayer)
            {
                if (name.Equals(layer.Name))
                    return layer;
            }
            return null;
        }

        /// <summary>
        /// Gets a group by its name
        /// </summary>
        /// <param name="map"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IMapLayerGroup GetGroupByName(this IMapDefinition map, string name)
        {
            Check.NotNull(map, "map");
            Check.NotEmpty(name, "name");
            foreach (var group in map.MapLayerGroup)
            {
                if (name.Equals(group.Name))
                    return group;
            }
            return null;
        }

        /// <summary>
        /// Gets the number of layers (non-tiled) on this map
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static int GetLayerCount(this IMapDefinition map)
        {
            Check.NotNull(map, "map");
            return new List<IMapLayer>(map.MapLayer).Count;
        }

        /// <summary>
        /// Gets the number of groups (non-tiled) on this map
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static int GetGroupCount(this IMapDefinition map)
        {
            Check.NotNull(map, "map");
            return new List<IMapLayerGroup>(map.MapLayerGroup).Count;
        }

        /// <summary>
        /// Gets all the layers that belong to the specified group
        /// </summary>
        /// <param name="map"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<IMapLayer> GetLayersForGroup(this IMapDefinition map, string name)
        {
            Check.NotNull(map, "map");
            Check.NotEmpty(name, "name");
            foreach (var layer in map.MapLayer)
            {
                if (name.Equals(layer.Group))
                    yield return layer;
            }
        }

        /// <summary>
        /// Gets all that layers that do not belong to a group
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static IEnumerable<IMapLayer> GetLayersWithoutGroups(this IMapDefinition map)
        {
            Check.NotNull(map, "map");
            foreach (var layer in map.MapLayer)
            {
                if (string.IsNullOrEmpty(layer.Group))
                    yield return layer;
            }
        }
    }

    /// <summary>
    /// Represents the tiled map portion of the Map Definition
    /// </summary>
    public interface IBaseMapDefinition
    {
        /// <summary>
        /// Gets the finite display scales
        /// </summary>
        IEnumerable<double> FiniteDisplayScale { get; }

        /// <summary>
        /// Adds the finite display scale.
        /// </summary>
        /// <param name="value">The value.</param>
        void AddFiniteDisplayScale(double value);

        /// <summary>
        /// Removes the finite display scale.
        /// </summary>
        /// <param name="value">The value.</param>
        void RemoveFiniteDisplayScale(double value);

        /// <summary>
        /// Gets the scale count.
        /// </summary>
        /// <value>The scale count.</value>
        int ScaleCount { get; }

        /// <summary>
        /// Removes the scale at the specified index
        /// </summary>
        /// <param name="index">The index.</param>
        void RemoveScaleAt(int index);

        /// <summary>
        /// Gets the scale at the specified index
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        double GetScaleAt(int index);

        /// <summary>
        /// Gets the base map layer groups.
        /// </summary>
        /// <value>The base map layer groups.</value>
        IEnumerable<IBaseMapGroup> BaseMapLayerGroup { get; }

        /// <summary>
        /// Gets the group count.
        /// </summary>
        /// <value>The group count.</value>
        int GroupCount { get; }

        /// <summary>
        /// Gets the group at the specified index
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        IBaseMapGroup GetGroupAt(int index);

        /// <summary>
        /// Adds the base layer group.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IBaseMapGroup AddBaseLayerGroup(string name);

        /// <summary>
        /// Removes the base layer group.
        /// </summary>
        /// <param name="group">The group.</param>
        void RemoveBaseLayerGroup(IBaseMapGroup group);

        /// <summary>
        /// Removes all scales.
        /// </summary>
        void RemoveAllScales();
    }

    /// <summary>
    /// Base legend element
    /// </summary>
    public interface IMapLegendElementBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show in legend].
        /// </summary>
        /// <value><c>true</c> if [show in legend]; otherwise, <c>false</c>.</value>
        bool ShowInLegend { get; set; }

        /// <summary>
        /// Gets or sets the legend label.
        /// </summary>
        /// <value>The legend label.</value>
        string LegendLabel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [expand in legend].
        /// </summary>
        /// <value><c>true</c> if [expand in legend]; otherwise, <c>false</c>.</value>
        bool ExpandInLegend { get; set; }
    }

    /// <summary>
    /// Base layer interface
    /// </summary>
    public interface IBaseMapLayer : IMapLegendElementBase
    {
        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        /// <value>The resource id.</value>
        string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IBaseMapLayer"/> is selectable.
        /// </summary>
        /// <value><c>true</c> if selectable; otherwise, <c>false</c>.</value>
        bool Selectable { get; set; }
    }

    /// <summary>
    /// Tiled map group
    /// </summary>
    public interface IBaseMapGroup : IMapLegendElementBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IBaseMapGroup"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        bool Visible { get; set; }

        /// <summary>
        /// Gets the base map layers.
        /// </summary>
        /// <value>The base map layers.</value>
        IEnumerable<IBaseMapLayer> BaseMapLayer { get; }

        /// <summary>
        /// Adds the layer.
        /// </summary>
        /// <param name="layerName">Name of the layer.</param>
        /// <param name="resourceId">The resource id.</param>
        /// <returns></returns>
        IBaseMapLayer AddLayer(string layerName, string resourceId);

        /// <summary>
        /// Removes the base map layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        void RemoveBaseMapLayer(IBaseMapLayer layer);

        /// <summary>
        /// Moves the specified layer up.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        int MoveUp(IBaseMapLayer layer);

        /// <summary>
        /// Moves the specified layer down.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        int MoveDown(IBaseMapLayer layer);
    }

    /// <summary>
    /// A dynamic map layer
    /// </summary>
    public interface IMapLayer : IBaseMapLayer
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IMapLayer"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
        string Group { get; set; }
    }

    /// <summary>
    /// A dynamic map layer group
    /// </summary>
    public interface IMapLayerGroup : IMapLegendElementBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IMapLayerGroup"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the group name. If null, it means this layer doesn't belong to any group
        /// </summary>
        /// <value>The group.</value>
        string Group { get; set; }

        /// <summary>
        /// Gets the parent map definition
        /// </summary>
        /// <value>The parent map definition.</value>
        IMapDefinition Parent { get; }
    }
}
