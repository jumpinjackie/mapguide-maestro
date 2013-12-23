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
using OSGeo.MapGuide.ObjectModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModels.RuntimeMap
{
    /// <summary>
    /// Describes the structure of a Runtime Map
    /// </summary>
    public interface IRuntimeMapInfo
    {
        /// <summary>
        /// Gets the site version of the MapGuide Server
        /// </summary>
        string SiteVersion { get; }

        /// <summary>
        /// Gets the name of the runtime map. This combined with the session ID provides the
        /// means for any code using the MapGuide API to open an existing MgMap instance
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the Map Definition resource ID used to create this runtime map
        /// </summary>
        string MapDefinition { get; }

        /// <summary>
        /// Gets the background color
        /// </summary>
        string BackgroundColor { get; }

        /// <summary>
        /// Gets the display DPI of the map
        /// </summary>
        int DisplayDpi { get; }

        /// <summary>
        /// Gets the mime type of any inline icons.
        /// </summary>
        /// <remarks>
        /// If the application did not request any icons as part of the CreateRuntimeMap 
        /// or DescribeRuntimeMap request, this property is null
        /// </remarks>
        string IconMimeType { get; }

        /// <summary>
        /// Gets the coordinate system of this map
        /// </summary>
        ICoordinateSystemInfo CoordinateSystem { get; }

        /// <summary>
        /// Gets the bounding box of this map
        /// </summary>
        IEnvelope Extents { get; }

        /// <summary>
        /// Gets the layers of this map.
        /// </summary>
        /// <remarks>
        /// If the application did not request layer structure as part of the CreateRuntimeMap
        /// or DescribeRuntimeMap request, this property will be an empty collection
        /// </remarks>
        IRuntimeLayerInfoCollection Layers { get; }

        /// <summary>
        /// Gets the groups of this map.
        /// </summary>
        /// <remarks>
        /// Even if the application did not request layer structure as part of the CreateRuntimeMap
        /// or DescribeRuntimeMap request, this property will still contain any Base Layer Groups
        /// if defined in the Map Definition
        /// </remarks>
        IRuntimeLayerGroupInfoCollection Groups { get; }

        /// <summary>
        /// Gets the finite display scales defined for this runtime map
        /// </summary>
        double[] FiniteDisplayScales { get; }
    }

    /// <summary>
    /// Represents coordinate system information for a Runtime Map
    /// </summary>
    public interface ICoordinateSystemInfo
    {
        /// <summary>
        /// Gets the WKT of this coordinate system
        /// </summary>
        string Wkt { get; }

        /// <summary>
        /// Gets the CS-Map coordinate system code of this coordinate system
        /// </summary>
        string MentorCode { get; }

        /// <summary>
        /// Gets the EPSG code of this coordinate system
        /// </summary>
        string EpsgCode { get; }

        /// <summary>
        /// Gets the meters-per-unit value of this runtime map. This value is essential for
        /// any tile or rendering functionality
        /// </summary>
        double MetersPerUnit { get; }
    }
    
    /// <summary>
    /// Models a legend element
    /// </summary>
    public interface IRuntimeMapLegendElement
    {
        /// <summary>
        /// Gets the name of the element
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the label of the element
        /// </summary>
        string LegendLabel { get; }

        /// <summary>
        /// Gets the unique id of the element
        /// </summary>
        string ObjectID { get; }

        /// <summary>
        /// Gets the unique id of the element's parent
        /// </summary>
        string ParentID { get; }

        /// <summary>
        /// Gets whether the element will be shown in the legend
        /// </summary>
        bool DisplayInLegend { get; }

        /// <summary>
        /// Gets whether the element will be expanded in the legend
        /// </summary>
        bool ExpandInLegend { get; }

        /// <summary>
        /// Gets whether the element is potentially
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// Gets whether the element is actually element
        /// </summary>
        bool ActuallyVisible { get; }
    }

    /// <summary>
    /// Represents a layer of the runtime map
    /// </summary>
    public interface IRuntimeLayerInfo : IRuntimeMapLegendElement
    {
        /// <summary>
        /// Gets the type of layer
        /// </summary>
        int LayerType { get; }

        /// <summary>
        /// Gets the Layer Definition ID
        /// </summary>
        string LayerDefinition { get; }

        /// <summary>
        /// Gets feature source information of the layer
        /// </summary>
        IFeatureSourceInfo FeatureSource { get; }

        /// <summary>
        /// Gets information about the scale ranges in the layer
        /// </summary>
        IScaleRangeInfoCollection ScaleRanges { get; }
    }

    /// <summary>
    /// Represents Feature Source information for a layer
    /// </summary>
    public interface IFeatureSourceInfo
    {
        /// <summary>
        /// Gets the resource id of the Feature Source
        /// </summary>
        string ResourceID { get; }

        /// <summary>
        /// Gets the name of the feature class that the layer is rendered from
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// Gets the name of the geometry property of the feature class that the layer is rendered from
        /// </summary>
        string Geometry { get; }
    }

    /// <summary>
    /// Represents a group of a runtime map
    /// </summary>
    public interface IRuntimeLayerGroupInfo : IRuntimeMapLegendElement
    {
        /// <summary>
        /// Gets the type of the group
        /// </summary>
        int GroupType { get; }
    }

    /// <summary>
    /// Represents a scale range of a layer
    /// </summary>
    public interface IScaleRangeInfo
    {
        /// <summary>
        /// Gets the minimum scale this scale range is applicable for
        /// </summary>
        double MinScale { get; }

        /// <summary>
        /// Gets the maximum scale this scale range is applicable for
        /// </summary>
        double MaxScale { get; }

        /// <summary>
        /// Gets the feature styles for this scale range
        /// </summary>
        IFeatureStyleInfoCollection FeatureStyle { get; }
    }

    /// <summary>
    /// Represents a feature style
    /// </summary>
    public interface IFeatureStyleInfo
    {
        /// <summary>
        /// Gets the type of the feature style
        /// </summary>
        int Type { get; }

        /// <summary>
        /// Gets the style rules for this feature style
        /// </summary>
        IRuleInfoCollection Rules { get; }
    }

    /// <summary>
    /// Represents a style rule
    /// </summary>
    public interface IRuleInfo
    {
        /// <summary>
        /// Gets the legend label for this rule
        /// </summary>
        string LegendLabel { get; }

        /// <summary>
        /// Gets the filter for this rule
        /// </summary>
        string Filter { get; }

        /// <summary>
        /// Gets the icon for this rule
        /// </summary>
        string IconBase64 { get; }
    }

    /// <summary>
    /// A generic read-only collection interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyCollection<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets the number of items in this collection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the item at the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T this[int index]
        {
            get;
        }
    }

    /// <summary>
    /// A collection of layers
    /// </summary>
    public interface IRuntimeLayerInfoCollection : IReadOnlyCollection<IRuntimeLayerInfo>
    {

    }

    /// <summary>
    /// A collection of groups
    /// </summary>
    public interface IRuntimeLayerGroupInfoCollection : IReadOnlyCollection<IRuntimeLayerGroupInfo>
    {

    }

    /// <summary>
    /// A collection of scale ranges
    /// </summary>
    public interface IScaleRangeInfoCollection : IReadOnlyCollection<IScaleRangeInfo>
    {

    }

    /// <summary>
    /// A collection of feature styles
    /// </summary>
    public interface IFeatureStyleInfoCollection : IReadOnlyCollection<IFeatureStyleInfo>
    {

    }

    /// <summary>
    /// A collection of style rules
    /// </summary>
    public interface IRuleInfoCollection : IReadOnlyCollection<IRuleInfo>
    {

    }
}
