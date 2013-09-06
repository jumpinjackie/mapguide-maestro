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

    public interface IRuntimeMapLegendElement
    {
        string Name { get; }

        string LegendLabel { get; }

        string ObjectID { get; }

        string ParentID { get; }

        bool DisplayInLegend { get; }

        bool ExpandInLegend { get; }

        bool Visible { get; }

        bool ActuallyVisible { get; }
    }

    public interface IRuntimeLayerInfo : IRuntimeMapLegendElement
    {
        int LayerType { get; }

        string LayerDefinition { get; }

        IFeatureSourceInfo FeatureSource { get; }

        IScaleRangeInfoCollection ScaleRanges { get; }
    }

    public interface IFeatureSourceInfo
    {
        string ResourceID { get; }

        string ClassName { get; }

        string Geometry { get; }
    }

    public interface IRuntimeLayerGroupInfo : IRuntimeMapLegendElement
    {
        int GroupType { get; }
    }

    public interface IScaleRangeInfo
    {
        double MinScale { get; }

        double MaxScale { get; }

        IFeatureStyleInfoCollection FeatureStyle { get; }
    }

    public interface IFeatureStyleInfo
    {
        int Type { get; }

        IRuleInfoCollection Rules { get; }
    }

    public interface IRuleInfo
    {
        string LegendLabel { get; }

        string Filter { get; }

        string IconBase64 { get; }
    }

    public interface IReadOnlyCollection<T> : IEnumerable<T>
    {
        int Count { get; }

        T this[int index]
        {
            get;
        }
    }

    public interface IRuntimeLayerInfoCollection : IReadOnlyCollection<IRuntimeLayerInfo>
    {

    }

    public interface IRuntimeLayerGroupInfoCollection : IReadOnlyCollection<IRuntimeLayerGroupInfo>
    {

    }

    public interface IScaleRangeInfoCollection : IReadOnlyCollection<IScaleRangeInfo>
    {

    }

    public interface IFeatureStyleInfoCollection : IReadOnlyCollection<IFeatureStyleInfo>
    {

    }

    public interface IRuleInfoCollection : IReadOnlyCollection<IRuleInfo>
    {

    }
}
