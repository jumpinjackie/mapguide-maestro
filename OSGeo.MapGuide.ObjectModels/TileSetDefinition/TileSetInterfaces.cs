#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OSGeo.MapGuide.ObjectModels.TileSetDefinition
{
    /// <summary>
    /// Describes a Tile Set Definition
    /// </summary>
    public interface ITileSetDefinition : IResource, ITileSetAbstract
    {
        /// <summary>
        /// The configuration parameters
        /// </summary>
        ITileStoreParameters TileStoreParameters { get; }

        /// <summary>
        /// The extents of this tile set
        /// </summary>
        IEnvelope Extents { get; set; }
    }

    /// <summary>
    /// Describes configuration parameters for a Tile Set Definition
    /// </summary>
    public interface ITileStoreParameters
    {
        /// <summary>
        /// The tile provider name
        /// </summary>
        string TileProvider { get; set; }

        /// <summary>
        /// Adds a configuration parameter
        /// </summary>
        /// <param name="name">The parameter name</param>
        /// <param name="value">The parameter value</param>
        void AddParameter(string name, string value);

        /// <summary>
        /// Sets the value for a configuration parameter
        /// </summary>
        /// <param name="name">The parameter name</param>
        /// <param name="value">The parameter value</param>
        void SetParameter(string name, string value);

        /// <summary>
        /// Gets all the configuration parameters
        /// </summary>
        IEnumerable<INameStringPair> Parameters { get; }

        /// <summary>
        /// Clears all configuration parameters
        /// </summary>
        void ClearParameters();

        /// <summary>
        /// Raised when a configuration parameter has changed
        /// </summary>
        event EventHandler ParametersChanged;
    }

    /// <summary>
    /// An abstraction that works with both Tile Set Definitions and the base map section of a Map Definition
    /// </summary>
    public interface ITileSetAbstract
    {
        /// <summary>
        /// Sets the finite display scale list
        /// </summary>
        /// <param name="scales">The scales to set</param>
        void SetFiniteDisplayScales(IEnumerable<double> scales);

        /// <summary>
        /// Adds the finite display scale. The implementation may internally sort after adding the added item
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
        /// <returns>The scale</returns>
        double GetScaleAt(int index);

        /// <summary>
        /// Removes all scales.
        /// </summary>
        void RemoveAllScales();

        /// <summary>
        /// The finite display scale list
        /// </summary>
        IEnumerable<double> FiniteDisplayScale { get; }

        /// <summary>
        /// Gets whether this tile set supports under certain conditions. If false, the caller
        /// should check if <see cref="SupportsCustomFiniteDisplayScales"/> is true in order to
        /// safely invoke any scale based operations
        /// </summary>
        bool SupportsCustomFiniteDisplayScalesUnconditionally { get; }

        /// <summary>
        /// Gets whether this tile set supports custom finite display scales. If false, none
        /// of the scale operations should be used
        /// </summary>
        bool SupportsCustomFiniteDisplayScales { get; }

        /// <summary>
        /// The base map layer groups
        /// </summary>
        IEnumerable<IBaseMapGroup> BaseMapLayerGroups { get; }

        /// <summary>
        /// Gets the layer for the given base layer group name
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        IEnumerable<IBaseMapLayer> GetLayersForGroup(string groupName);

        /// <summary>
        /// Gets the group count.
        /// </summary>
        /// <value>The group count.</value>
        int GroupCount { get; }

        /// <summary>
        /// Gets the group at the specified index
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The group</returns>
        IBaseMapGroup GetGroupAt(int index);

        /// <summary>
        /// Adds the base layer group.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The group</returns>
        IBaseMapGroup AddBaseLayerGroup(string name);

        /// <summary>
        /// Removes the base layer group.
        /// </summary>
        /// <param name="group">The group.</param>
        void RemoveBaseLayerGroup(IBaseMapGroup group);
    }

    /// <summary>
    /// Extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Gets the number of base layers
        /// </summary>
        /// <param name="map">The map</param>
        /// <returns>The count</returns>
        public static int GetBaseLayerCount(this ITileSetAbstract map)
        {
            Check.ArgumentNotNull(map, nameof(map));
            return map.BaseMapLayerGroups.SelectMany(g => g.BaseMapLayer).Count();
        }

        /// <summary>
        /// Gets the minimum finite display scale
        /// </summary>
        /// <param name="map">The map</param>
        /// <returns>The minimum scale</returns>
        public static double GetMinScale(this ITileSetAbstract map)
        {
            Check.ArgumentNotNull(map, nameof(map));
            if (map.ScaleCount == 0)
                return 0.0;

            return map.FiniteDisplayScale.First();
        }

        /// <summary>
        /// Gets the maximum finite display scale
        /// </summary>
        /// <param name="map">The map</param>
        /// <returns>The maximum scale</returns>
        public static double GetMaxScale(this ITileSetAbstract map)
        {
            Check.ArgumentNotNull(map, nameof(map));
            if (map.ScaleCount == 0)
                return 0.0;

            return map.FiniteDisplayScale.Last();
        }

        /// <summary>
        /// Gets the parent group for the specified layer
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="layer">The layer</param>
        /// <returns>The parent group</returns>
        public static IBaseMapGroup GetGroupForLayer(this ITileSetAbstract map, IBaseMapLayer layer)
        {
            Check.ArgumentNotNull(map, nameof(map));
            foreach (var group in map.BaseMapLayerGroups)
            {
                foreach (var tl in group.BaseMapLayer)
                {
                    if (tl == layer)
                        return group;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets whether this base map group has tiled layers
        /// </summary>
        /// <param name="grp">The group</param>
        /// <returns>True if it has tiled layers. False otherwise</returns>
        public static bool HasLayers(this IBaseMapGroup grp)
        {
            Check.ArgumentNotNull(grp, nameof(grp));
            return new List<IBaseMapLayer>(grp.BaseMapLayer).Count > 0;
        }

        /// <summary>
        /// Gets whether this base map has tiled layers
        /// </summary>
        /// <param name="map">The map</param>
        /// <returns>True if it has tiled layers. False otherwise</returns>
        public static bool HasLayers(this ITileSetAbstract map)
        {
            Check.ArgumentNotNull(map, nameof(map));
            if (!map.HasGroups())
                return false;

            foreach (var group in map.BaseMapLayerGroups)
            {
                if (group.HasLayers())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets whether this base map has groups
        /// </summary>
        /// <param name="map">The map</param>
        /// <returns>True if it has groups. False otherwise</returns>
        public static bool HasGroups(this ITileSetAbstract map)
        {
            Check.ArgumentNotNull(map, nameof(map));
            return new List<IBaseMapGroup>(map.BaseMapLayerGroups).Count > 0;
        }

        /// <summary>
        /// Gets the first base map group
        /// </summary>
        /// <param name="map">The map</param>
        /// <returns>The first base map group</returns>
        public static IBaseMapGroup GetFirstGroup(this ITileSetAbstract map)
        {
            Check.ArgumentNotNull(map, nameof(map));
            var list = new List<IBaseMapGroup>(map.BaseMapLayerGroups);
            if (list.Count > 0)
                return list[0];
            return null;
        }

        /// <summary>
        /// Gets the base layer of the specified name
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="layerName">The layer name</param>
        /// <returns>The base layer</returns>
        public static IBaseMapLayer GetBaseLayerByName(this ITileSetAbstract map, string layerName)
        {
            Check.ArgumentNotNull(map, nameof(map));
            Check.ArgumentNotEmpty(layerName, nameof(layerName));

            foreach (var group in map.BaseMapLayerGroups)
            {
                foreach (var layer in group.BaseMapLayer)
                {
                    if (layerName.Equals(layer.Name))
                        return layer;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets whether a base layer of the specified name exists.
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="layerName">The layer name</param>
        /// <returns>True if it exists. False otherwise</returns>
        public static bool LayerExists(this ITileSetAbstract map, string layerName)
        {
            Check.ArgumentNotNull(map, nameof(map));
            Check.ArgumentNotEmpty(layerName, nameof(layerName));

            return map.GetBaseLayerByName(layerName) != null;
        }

        /// <summary>
        /// Gets the base map group of the specified name
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="groupName">The group name</param>
        /// <returns>The base map group</returns>
        public static IBaseMapGroup GetGroup(this ITileSetAbstract map, string groupName)
        {
            Check.ArgumentNotNull(map, nameof(map));
            Check.ArgumentNotEmpty(groupName, nameof(groupName));
            foreach (var group in map.BaseMapLayerGroups)
            {
                if (groupName.Equals(group.Name))
                    return group;
            }
            return null;
        }

        /// <summary>
        /// Gets whether the specified base map group exists
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="groupName">The group name</param>
        /// <returns>True if it exists. False otherwise</returns>
        public static bool GroupExists(this ITileSetAbstract map, string groupName)
        {
            Check.ArgumentNotNull(map, nameof(map));
            Check.ArgumentNotEmpty(groupName, nameof(groupName));
            foreach (var group in map.BaseMapLayerGroups)
            {
                if (groupName.Equals(group.Name))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the tiled layers for the specified base map group
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="groupName">The group name</param>
        /// <returns>The tiled layers</returns>
        public static IEnumerable<IBaseMapLayer> GetLayersForGroup(this ITileSetAbstract map, string groupName)
        {
            Check.ArgumentNotNull(map, nameof(map));
            Check.ArgumentNotEmpty(groupName, nameof(groupName));

            foreach (var group in map.BaseMapLayerGroups)
            {
                if (groupName.Equals(group.Name))
                {
                    return group.BaseMapLayer;
                }
            }

            return new IBaseMapLayer[0];
        }

        /// <summary>
        /// Removes the given base layer group from the Map Definition
        /// </summary>
        /// <param name="map">The map</param>
        /// <param name="group">The group to remove</param>
        public static void RemoveBaseLayerGroup(this ITileSetDefinition map, IBaseMapGroup group)
        {
            Check.ArgumentNotNull(map, nameof(map));
            if (null == group)
                return;

            map.RemoveBaseLayerGroup(group);
        }

        /// <summary>
        /// Gets whether the specified base group exists in the tile set
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <param name="groupName">The group name</param>
        /// <returns>True if the group exists. False otherwise</returns>
        public static bool GroupExists(this ITileSetDefinition tileSet, string groupName)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            Check.ArgumentNotEmpty(groupName, nameof(groupName));
            foreach (var group in tileSet.BaseMapLayerGroups)
            {
                if (groupName.Equals(group.Name))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the tile set parameter of the specified name
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <param name="name">The name</param>
        /// <returns>The parameter</returns>
        public static INameStringPair GetParameter(this ITileSetDefinition tileSet, string name)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));

            return tileSet.TileStoreParameters.Parameters.FirstOrDefault(p => p.Name == name);
        }

        /// <summary>
        /// Gets the coordinate system of this tile set. Must be using the default tile provider
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <returns>The coordinate system</returns>
        public static string GetDefaultCoordinateSystem(this ITileSetDefinition tileSet)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));

            var p = tileSet.GetParameter("CoordinateSystem"); //NOXLATE
            if (p != null)
                return p.Value;
            return null;
        }

        /// <summary>
        /// Sets the coordinate system of this tile set. Must be using the default tile provider.
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <param name="coordinateSystem">The coordinate system</param>
        public static void SetDefaultCoordinateSystem(this ITileSetDefinition tileSet, string coordinateSystem)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            Check.ArgumentNotEmpty(coordinateSystem, nameof(coordinateSystem));

            if (tileSet.TileStoreParameters.TileProvider != "Default") //NOXLATE
                throw new InvalidOperationException(string.Format(Strings.ParameterNotApplicableForTileProvider, "CoordinateSystem", tileSet.TileStoreParameters.TileProvider));

            tileSet.TileStoreParameters.SetParameter("CoordinateSystem", coordinateSystem); //NOXLATE
        }

        /// <summary>
        /// Gets the finite scale list of this tile set. Must be using the default tile provider.
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <returns>The array of finite scales</returns>
        public static double[] GetDefaultFiniteScaleList(this ITileSetDefinition tileSet)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            var p = tileSet.GetParameter("FiniteScaleList"); //NOXLATE
            if (p != null && !string.IsNullOrEmpty(p.Value))
                return p.Value.Split(',').Select(x => x.Trim()).Select(x => Convert.ToDouble(x)).OrderBy(s => s).ToArray();
            return new double[0];
        }

        /// <summary>
        /// Sets the finite scale list of this tile set. Must be using the default tile provider.
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <param name="scales">The finite sclae list</param>
        public static void SetDefaultFiniteScaleList(this ITileSetDefinition tileSet, IEnumerable<double> scales)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            Check.ArgumentNotNull(scales, nameof(scales));

            if (tileSet.TileStoreParameters.TileProvider != "Default") //NOXLATE
                throw new InvalidOperationException(string.Format(Strings.ParameterNotApplicableForTileProvider, "FiniteScaleList", tileSet.TileStoreParameters.TileProvider)); //NOXLATE

            string str = string.Join(",", scales.OrderByDescending(x => x).Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());
            tileSet.TileStoreParameters.SetParameter("FiniteScaleList", str); //NOXLATE
        }

        /// <summary>
        /// Gets the tile path of this tile set
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <returns>The tile path</returns>
        public static string GetTilePath(this ITileSetDefinition tileSet)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            var p = tileSet.GetParameter("TilePath"); //NOXLATE
            if (p != null)
                return p.Value;
            return null;
        }

        /// <summary>
        /// Sets the path of this tile set
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <param name="path">The tile path</param>
        public static void SetTilePath(this ITileSetDefinition tileSet, string path)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            Check.ArgumentNotEmpty(path, nameof(path));
            tileSet.TileStoreParameters.SetParameter("TilePath", path); //NOXLATE
        }

        /// <summary>
        /// Gets the width of this tile set. Must be using the default tile provider.
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <returns>The width of this tile set</returns>
        public static int? GetDefaultTileWidth(this ITileSetDefinition tileSet)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            var p = tileSet.GetParameter("TileWidth"); //NOXLATE
            if (p != null)
                return Convert.ToInt32(p.Value);
            return null;
        }

        /// <summary>
        /// Sets the width of this tile set. Must be using the default tile provider.
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <param name="value">The width of this tile set</param>
        public static void SetDefaultTileWidth(this ITileSetDefinition tileSet, int value)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            if (tileSet.TileStoreParameters.TileProvider == "XYZ") //NOXLATE
                throw new InvalidOperationException(string.Format(Strings.ParameterNotApplicableForTileProvider, "TileWidth", "XYZ")); //NOXLATE
            tileSet.TileStoreParameters.SetParameter("TileWidth", value.ToString(CultureInfo.InvariantCulture)); //NOXLATE
        }

        /// <summary>
        /// Gets the height of this tile set. Must be using the default tile provider.
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <returns>The height of this tile set</returns>
        public static int? GetDefaultTileHeight(this ITileSetDefinition tileSet)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            var p = tileSet.GetParameter("TileHeight"); //NOXLATE
            if (p != null)
                return Convert.ToInt32(p.Value);
            return null;
        }

        /// <summary>
        /// Sets the height of this tile set. Must be using the default tile provider.
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <param name="value">The height</param>
        public static void SetDefaultTileHeight(this ITileSetDefinition tileSet, int value)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            if (tileSet.TileStoreParameters.TileProvider == "XYZ")
                throw new InvalidOperationException(string.Format(Strings.ParameterNotApplicableForTileProvider, "TileHeight", "XYZ")); //NOXLATE
            tileSet.TileStoreParameters.SetParameter("TileHeight", value.ToString(CultureInfo.InvariantCulture)); //NOXLATE
        }

        /// <summary>
        /// Gets the image format of this tile set
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <returns>The image format</returns>
        public static string GetTileFormat(this ITileSetDefinition tileSet)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            var p = tileSet.GetParameter("TileFormat"); //NOXLATE
            if (p != null)
                return p.Value;
            return null;
        }

        /// <summary>
        /// Sets the image format of this tile set
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <param name="format">The image format</param>
        public static void SetTileFormat(this ITileSetDefinition tileSet, string format)
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));
            Check.ArgumentNotEmpty(format, nameof(format));
            tileSet.TileStoreParameters.SetParameter("TileFormat", format); //NOXLATE
        }

        /// <summary>
        /// Sets XYZ provider parameters. Any existing parameters are cleared
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <param name="tileFormat">The image format</param>
        /// <param name="tilePath">The tile path</param>
        public static void SetXYZProviderParameters(this ITileSetDefinition tileSet,
                                                    string tileFormat = "PNG",
                                                    string tilePath = "%MG_TILE_CACHE_PATH%")
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));

            var param = tileSet.TileStoreParameters;
            param.TileProvider = "XYZ"; //NOXLATE
            param.ClearParameters();

            param.AddParameter("TileFormat", tileFormat); //NOXLATE
            param.AddParameter("TilePath", tilePath); //NOXLATE
        }

        /// <summary>
        /// Sets default provider parameters. Any existing parameters are cleared
        /// </summary>
        /// <param name="tileSet">The tile set</param>
        /// <param name="tileWidth">Tile width</param>
        /// <param name="tileHeight">Tile height</param>
        /// <param name="coordinateSystem">Coordinate system</param>
        /// <param name="finiteScaleList">The finite scale list</param>
        /// <param name="tileFormat">Image format</param>
        /// <param name="tilePath">Tile path</param>
        public static void SetDefaultProviderParameters(this ITileSetDefinition tileSet, 
                                                        int tileWidth,
                                                        int tileHeight,
                                                        string coordinateSystem,
                                                        double [] finiteScaleList,
                                                        string tileFormat = "PNG", //NOXLATE
                                                        string tilePath = "%MG_TILE_CACHE_PATH%") //NOXLATE
        {
            Check.ArgumentNotNull(tileSet, nameof(tileSet));

            var param = tileSet.TileStoreParameters;
            param.TileProvider = "Default"; //NOXLATE
            param.ClearParameters();

            param.AddParameter("TileWidth", tileWidth.ToString(CultureInfo.InvariantCulture)); //NOXLATE
            param.AddParameter("TileHeight", tileHeight.ToString(CultureInfo.InvariantCulture)); //NOXLATE
            param.AddParameter("TileFormat", tileFormat); //NOXLATE
            param.AddParameter("CoordinateSystem", coordinateSystem); //NOXLATE
            string str = string.Join(",", finiteScaleList.OrderByDescending(x => x).Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray()); //NOXLATE
            param.AddParameter("FiniteScaleList", str); //NOXLATE
            param.AddParameter("TilePath", tilePath); //NOXLATE
        }
    }
}
