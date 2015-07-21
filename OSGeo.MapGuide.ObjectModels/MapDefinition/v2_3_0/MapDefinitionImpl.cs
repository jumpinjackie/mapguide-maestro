#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

#define MDF_230
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

#pragma warning disable 1591, 0114, 0108

#if MDF_240
namespace OSGeo.MapGuide.ObjectModels.MapDefinition.v2_4_0
#elif MDF_230

namespace OSGeo.MapGuide.ObjectModels.MapDefinition.v2_3_0
#else

namespace OSGeo.MapGuide.ObjectModels.MapDefinition.v1_0_0
#endif
{
    partial class MapDefinitionType
    {
        internal MapDefinitionType()
        {
        }

        [XmlIgnore]
        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColorField;
            }
            set
            {
                this.backgroundColorField = value;
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }

        protected void DetachChangeListeners()
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                foreach (var h in handler.GetInvocationList())
                {
                    this.PropertyChanged -= (PropertyChangedEventHandler)h;
                }
                handler = null;
            }
        }
    }

    public static class MdfEntryPoint
    {
        internal static MapDefinition CreateDefault()
        {
            return new MapDefinition()
            {
                Name = string.Empty,
                CoordinateSystem = string.Empty,
                Extents = new Box2DType() { MaxX = 0.0, MaxY = 0.0, MinX = 0.0, MinY = 0.0 },
                BackgroundColor = Color.White,
                MapLayer = new System.ComponentModel.BindingList<MapLayerType>(),
                MapLayerGroup = new System.ComponentModel.BindingList<MapLayerGroupType>(),
#if MDF_240
                Watermarks = new BindingList<OSGeo.MapGuide.ObjectModels.WatermarkDefinition.v2_4_0.WatermarkType>()
#elif MDF_230
                Watermarks = new BindingList<OSGeo.MapGuide.ObjectModels.WatermarkDefinition.v2_3_0.WatermarkType>()
#endif
            };
        }

        public static IResource Deserialize(string xml)
        {
            var mdf = MapDefinition.Deserialize(xml);
            foreach (var group in mdf.MapLayerGroup)
            {
                group.Parent = mdf;
            }
            return mdf;
        }

        public static Stream Serialize(IResource res)
        {
            return res.SerializeToStream();
        }
    }

    partial class MapDefinition : IMapDefinition
#if MDF_230 || MDF_240
, IMapDefinition2
#endif
    {
        internal MapDefinition()
        {
#if MDF_240
            this.versionField = "2.4.0"; //NOXLATE
#elif MDF_230
            this.versionField = "2.3.0"; //NOXLATE
#endif
        }

#if MDF_240
        private static readonly Version RES_VERSION = new Version(2, 4, 0);
#elif MDF_230
        private static readonly Version RES_VERSION = new Version(2, 3, 0);
#else
        private static readonly Version RES_VERSION = new Version(1, 0, 0);
#endif
        private string _resId;

        [XmlIgnore]
        public string ResourceID
        {
            get
            {
                return _resId;
            }
            set
            {
                if (!ResourceIdentifier.Validate(value))
                    throw new InvalidOperationException(Strings.ErrorInvalidResourceIdentifier);

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.MapDefinition.ToString())
                    throw new InvalidOperationException(string.Format(Strings.ErrorUnexpectedResourceType, res.ToString(), ResourceTypes.MapDefinition));

                _resId = value;
                this.OnPropertyChanged(nameof(ResourceID));
            }
        }

        [XmlIgnore]
        public virtual string ResourceType
        {
            get
            {
                return ResourceTypes.MapDefinition.ToString();
            }
        }

        [XmlIgnore]
        public Version ResourceVersion
        {
            get
            {
                return RES_VERSION;
            }
        }

        internal void SortGroupList()
        {
        }

        object ICloneable.Clone()
        {
            var mdf = this.Clone();
            mdf.DetachChangeListeners();
            return mdf;
        }

        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")] //NOXLATE
        public string ValidatingSchema
        {
#if MDF_240
            get { return "MapDefinition-2.4.0.xsd"; } //NOXLATE
#elif MDF_230
            get { return "MapDefinition-2.3.0.xsd"; } //NOXLATE
#else
            get { return "MapDefinition-1.0.0.xsd"; } //NOXLATE
#endif
            set { }
        }

        [XmlIgnore]
        public bool IsStronglyTyped
        {
            get { return true; }
        }

        /// <summary>
        /// Inserts the layer at the specified index
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="layer"></param>
        void IMapDefinition.InsertLayer(int idx, IMapLayer layer)
        {
            Check.ThatArgumentIsBetweenRange(idx, 0, this.MapLayer.Count, true, nameof(idx) + " (" + idx + ") between [" + 0 + "," + this.MapLayer.Count + "]");
            Check.ArgumentNotNull(layer, nameof(layer));
            var li = layer as MapLayerType;
            if (li != null)
            {
                this.MapLayer.Insert(idx, li);
                li.Parent = this;
            }
        }

        void IMapDefinition.SetExtents(double minx, double miny, double maxx, double maxy)
        {
            if (this.Extents == null)
            {
                var ext = new Box2DType()
                {
                    MaxX = maxx,
                    MaxY = maxy,
                    MinX = minx,
                    MinY = miny
                };
                this.Extents = ext;
            }
            else
            {
                this.Extents.MaxX = maxx;
                this.Extents.MaxY = maxy;
                this.Extents.MinX = minx;
                this.Extents.MinY = miny;
                OnPropertyChanged(nameof(Extents));
            }
        }

        public IMapLayerGroup AddGroup(string groupName)
        {
            Check.ArgumentNotEmpty(groupName, nameof(groupName));
            Check.ThatPreconditionIsMet(this.GetGroupByName(groupName) == null, $"<{nameof(groupName)}> does not already exist");
            if (this.MapLayerGroup == null)
                this.MapLayerGroup = new System.ComponentModel.BindingList<MapLayerGroupType>();

            var group = new MapLayerGroupType()
            {
                Parent = this,
                ExpandInLegend = true,
                LegendLabel = groupName,
                Name = groupName,
                ShowInLegend = true,
                Visible = true,
                Group = string.Empty
            };
            this.MapLayerGroup.Add(group);
            OnPropertyChanged(nameof(MapLayerGroup));
            return group;
        }

        public IMapLayer AddLayer(string groupName, string layerName, string layerDefinitionId)
        {
            Check.ArgumentNotEmpty(layerName, nameof(layerName));
            Check.ArgumentNotEmpty(layerDefinitionId, nameof(layerDefinitionId));
            Check.ThatPreconditionIsMet(ResourceIdentifier.Validate(layerDefinitionId), $"ResourceIdentifier.Validate({nameof(layerDefinitionId)})");
            Check.ThatPreconditionIsMet(ResourceIdentifier.GetResourceTypeAsString(layerDefinitionId) == ResourceTypes.LayerDefinition.ToString(), $"ResourceIdentifier.GetResourceTypeAsString({nameof(layerDefinitionId)}) == ResourceTypes.LayerDefinition.ToString()");
            if (!string.IsNullOrEmpty(groupName))
            {
                Check.ThatPreconditionIsMet(this.GetGroupByName(groupName) != null, $"There should be an existing group for <{nameof(groupName)}>");
            }
            var layer = new MapLayerType()
            {
                Parent = this,
                ExpandInLegend = true,
                LegendLabel = layerName,
                Name = layerName,
                ResourceId = layerDefinitionId,
                ShowInLegend = true,
                Visible = true,
                Selectable = true
            };
            layer.Group = string.IsNullOrEmpty(groupName) ? string.Empty : groupName;

            this.MapLayer.Insert(0, layer);
            OnPropertyChanged(nameof(MapLayer));

            this.AutoSetExtentsFromLayer(layer.ResourceId);

            return layer;
        }

        [XmlIgnore]
        public ILayerExtentCalculator ExtentCalculator { get; set; }

        public IMapLayer InsertLayer(int index, string groupName, string layerName, string layerDefinitionId)
        {
            Check.ThatArgumentIsBetweenRange(index, 0, this.MapLayer.Count, true, nameof(index) + " (" + index + ") between [" + 0 + "," + this.MapLayer.Count + "]");
            Check.ArgumentNotEmpty(layerName, nameof(layerName));
            Check.ArgumentNotEmpty(layerDefinitionId, nameof(layerDefinitionId));
            Check.ThatPreconditionIsMet(ResourceIdentifier.Validate(layerDefinitionId), $"ResourceIdentifier.Validate({nameof(layerDefinitionId)})");
            Check.ThatPreconditionIsMet(ResourceIdentifier.GetResourceTypeAsString(layerDefinitionId) == ResourceTypes.LayerDefinition.ToString(), $"ResourceIdentifier.GetResourceTypeAsString({nameof(layerDefinitionId)}) == ResourceTypes.LayerDefinition.ToString()");
            if (!string.IsNullOrEmpty(groupName))
            {
                Check.ArgumentNotNull(this.GetGroupByName(groupName), $"Group for <{nameof(groupName)}>");
            }
            var layer = new MapLayerType()
            {
                Parent = this,
                ExpandInLegend = true,
                LegendLabel = layerName,
                Name = layerName,
                ResourceId = layerDefinitionId,
                ShowInLegend = true,
                Visible = true,
                Selectable = true
            };
            layer.Group = string.IsNullOrEmpty(groupName) ? string.Empty : groupName;
            this.MapLayer.Insert(index, layer);
            return layer;
        }

        public IMapLayer AddLayer(IMapLayer layerToInsertAbove, string groupName, string layerName, string layerDefinitionId)
        {
            Check.ArgumentNotEmpty(layerName, nameof(layerName));
            Check.ArgumentNotEmpty(layerDefinitionId, nameof(layerDefinitionId));
            Check.ThatPreconditionIsMet(ResourceIdentifier.Validate(layerDefinitionId), $"ResourceIdentifier.Validate({nameof(layerDefinitionId)})");
            Check.ThatPreconditionIsMet(ResourceIdentifier.GetResourceTypeAsString(layerDefinitionId) == ResourceTypes.LayerDefinition.ToString(), $"ResourceIdentifier.GetResourceTypeAsString({nameof(layerDefinitionId)}) == ResourceTypes.LayerDefinition.ToString()");
            if (!string.IsNullOrEmpty(groupName))
            {
                Check.ArgumentNotNull(this.GetGroupByName(groupName), $"Group for <{nameof(groupName)}>");
            }
            var layer = new MapLayerType()
            {
                Parent = this,
                ExpandInLegend = true,
                LegendLabel = layerName,
                Name = layerName,
                ResourceId = layerDefinitionId,
                ShowInLegend = true,
                Visible = true,
                Selectable = true
            };
            layer.Group = string.IsNullOrEmpty(groupName) ? string.Empty : groupName;

            if (layerToInsertAbove != null)
            {
                var clayerToInsertAbove = layerToInsertAbove as MapLayerType;
                if (clayerToInsertAbove != null)
                {
                    var idx = this.MapLayer.IndexOf(clayerToInsertAbove);
                    if (idx >= 0)
                    {
                        this.MapLayer.Insert(idx, layer);
                    }
                    else
                    {
                        this.MapLayer.Add(layer);
                    }
                }
                else
                {
                    this.MapLayer.Add(layer);
                }
            }
            else
            {
                this.MapLayer.Add(layer);
            }
            OnPropertyChanged(nameof(MapLayer));

            this.AutoSetExtentsFromLayer(layer.ResourceId);

            return layer;
        }

        private static bool IsEmpty(Box2DType box2DType)
        {
            return box2DType == null ||
                (box2DType.MaxX == 0.0 &&
                box2DType.MaxY == 0.0 &&
                box2DType.MinX == 0.0 &&
                box2DType.MinY == 0.0);
        }

        [XmlIgnore]
        OSGeo.MapGuide.ObjectModels.Common.IEnvelope IMapDefinition.Extents
        {
            get
            {
                return this.Extents;
            }
            set
            {
                if (value == null)
                {
                    this.Extents = null;
                }
                else
                {
                    if (this.Extents == null)
                        this.Extents = new Box2DType();

                    this.Extents.MaxX = value.MaxX;
                    this.Extents.MaxY = value.MaxY;
                    this.Extents.MinX = value.MinX;
                    this.Extents.MinY = value.MinY;
                }
            }
        }

        [XmlIgnore]
        IBaseMapDefinition IMapDefinition.BaseMap
        {
            get
            {
                return (IBaseMapDefinition)this.BaseMapDefinition;
            }
        }

        void IMapDefinition.AttachBaseMap(IBaseMapDefinition baseMap)
        {
            var bmd = baseMap as MapDefinitionTypeBaseMapDefinition;
            if (bmd != null)
            {
                this.BaseMapDefinition = bmd;
            }
        }

        void IMapDefinition.InitBaseMap()
        {
            if (this.BaseMapDefinition == null)
            {
                this.BaseMapDefinition = new MapDefinitionTypeBaseMapDefinition()
                {
                    Parent = (IMapDefinition)this,
                    BaseMapLayerGroup = new System.ComponentModel.BindingList<BaseMapLayerGroupCommonType>(),
                    FiniteDisplayScale = new System.ComponentModel.BindingList<double>()
                };
            }
        }

        void IMapDefinition.RemoveBaseMap()
        {
            this.BaseMapDefinition = null;
        }

        [XmlIgnore]
        IEnumerable<IMapLayer> IMapDefinition.MapLayer
        {
            get
            {
                foreach (var layer in this.MapLayer)
                {
                    yield return layer;
                }
            }
        }

        void IMapDefinition.RemoveLayer(IMapLayer layer)
        {
            var lyr = layer as MapLayerType;
            if (lyr != null)
            {
                this.MapLayer.Remove(lyr);
                OnPropertyChanged(nameof(MapLayer));
            }
        }

        [XmlIgnore]
        IEnumerable<IMapLayerGroup> IMapDefinition.MapLayerGroup
        {
            get
            {
                foreach (var grp in this.MapLayerGroup)
                {
                    yield return grp;
                }
            }
        }

        void IMapDefinition.RemoveGroup(IMapLayerGroup group)
        {
            Check.ArgumentNotNull(group, nameof(group));
            var grp = group as MapLayerGroupType;
            if (grp != null)
            {
                this.MapLayerGroup.Remove(grp);
                OnPropertyChanged(nameof(MapLayerGroup));
            }
        }

        void IMapDefinition.RemoveAllGroups()
        {
            this.MapLayerGroup.Clear();
        }

        void IMapDefinition.RemoveAllLayers()
        {
            this.MapLayer.Clear();
        }

        int IMapDefinition.GetIndex(IMapLayer layer)
        {
            Check.ArgumentNotNull(layer, nameof(layer));
            var lyr = layer as MapLayerType;
            if (lyr != null)
                return this.MapLayer.IndexOf(lyr);

            return -1;
        }

        int IMapDefinition.MoveUp(IMapLayer layer)
        {
            Check.ArgumentNotNull(layer, nameof(layer));
            int isrc = ((IMapDefinition)this).GetIndex(layer);
            if (isrc > 0)
            {
                int idst = isrc - 1;

                var src = this.MapLayer[isrc];
                var dst = this.MapLayer[idst];

                this.MapLayer[isrc] = dst;
                this.MapLayer[idst] = src;

                OnPropertyChanged(nameof(MapLayer));

                return idst;
            }
            else if (isrc == 0)
            {
                return isrc; //Unchanged
            }

            return -1;
        }

        int IMapDefinition.MoveDown(IMapLayer layer)
        {
            Check.ArgumentNotNull(layer, nameof(layer));
            int isrc = ((IMapDefinition)this).GetIndex(layer);
            if (isrc < this.MapLayer.Count - 1)
            {
                int idst = isrc + 1;

                var src = this.MapLayer[isrc];
                var dst = this.MapLayer[idst];

                this.MapLayer[isrc] = dst;
                this.MapLayer[idst] = src;

                OnPropertyChanged(nameof(MapLayer));

                return idst;
            }
            else if (isrc == this.MapLayer.Count - 1)
            {
                return this.MapLayer.Count - 1; //Unchanged
            }

            return -1;
        }

        int IMapDefinition.MoveUpGroup(IMapLayerGroup group)
        {
            Check.ArgumentNotNull(group, nameof(group));
            var grp = group as MapLayerGroupType;
            if (grp != null)
            {
                var idx = this.MapLayerGroup.IndexOf(grp);
                if (idx > 0)
                {
                    int idst = idx - 1;

                    var src = this.MapLayerGroup[idx];
                    var dst = this.MapLayerGroup[idst];

                    this.MapLayerGroup[idx] = dst;
                    this.MapLayerGroup[idst] = src;

                    OnPropertyChanged(nameof(MapLayerGroup));

                    return idst;
                }
                else if (idx == 0)
                {
                    return idx; //Unchanged
                }
            }

            return -1;
        }

        int IMapDefinition.MoveDownGroup(IMapLayerGroup group)
        {
            Check.ArgumentNotNull(group, nameof(group));
            var grp = group as MapLayerGroupType;
            if (grp != null)
            {
                var idx = this.MapLayerGroup.IndexOf(grp);
                if (idx < this.MapLayerGroup.Count - 1)
                {
                    int idst = idx + 1;

                    var src = this.MapLayerGroup[idx];
                    var dst = this.MapLayerGroup[idst];

                    this.MapLayerGroup[idx] = dst;
                    this.MapLayerGroup[idst] = src;

                    OnPropertyChanged(nameof(MapLayerGroup));

                    return idst;
                }
                else if (idx == this.MapLayerGroup.Count - 1)
                {
                    return this.MapLayerGroup.Count - 1; //Unchanged
                }
            }

            return -1;
        }

        int IMapDefinition.GetIndex(IMapLayerGroup group)
        {
            Check.ArgumentNotNull(group, nameof(group));
            var grp = group as MapLayerGroupType;
            if (grp != null)
                return this.MapLayerGroup.IndexOf(grp);

            return -1;
        }

        void IMapDefinition.SetTopDrawOrder(IMapLayer layer)
        {
            Check.ArgumentNotNull(layer, nameof(layer));
            int isrc = ((IMapDefinition)this).GetIndex(layer);
            if (isrc > 0)
            {
                var src = this.MapLayer[isrc];

                //take everything before this and shift them up one position
                for (int i = isrc - 1; i >= 0; i--)
                {
                    this.MapLayer[i + 1] = this.MapLayer[i];
                }

                this.MapLayer[0] = src;
                OnPropertyChanged(nameof(MapLayer));
            }
        }

        void IMapDefinition.SetBottomDrawOrder(IMapLayer layer)
        {
            Check.ArgumentNotNull(layer, nameof(layer));
            int isrc = ((IMapDefinition)this).GetIndex(layer);
            if (isrc >= 0 && isrc < this.MapLayer.Count)
            {
                var src = this.MapLayer[isrc];

                //take everything after this and shift them down one position
                for (int i = isrc + 1; i < this.MapLayer.Count; i++)
                {
                    this.MapLayer[i - 1] = this.MapLayer[i];
                }

                this.MapLayer[this.MapLayer.Count - 1] = src;
                OnPropertyChanged(nameof(MapLayer));
            }
        }

#if MDF_230 || MDF_240

        IEnumerable<IWatermark> IWatermarkCollection.Watermarks
        {
            get
            {
                foreach (var wm in this.Watermarks)
                    yield return wm;
            }
        }

        IWatermark IWatermarkCollection.AddWatermark(IWatermarkDefinition watermark)
        {
            return WatermarkCollectionUtil.AddWatermark(this.Watermarks, watermark);
        }

        void IWatermarkCollection.RemoveWatermark(IWatermark watermark)
        {
            WatermarkCollectionUtil.RemoveWatermark(this.Watermarks, watermark);
        }

        int IWatermarkCollection.WatermarkCount
        {
            get { return this.Watermarks.Count; }
        }

#endif
    }

    partial class Box2DType : IEnvelope
    { }

    partial class MapDefinitionTypeBaseMapDefinition : IBaseMapDefinition
    {
        [XmlIgnore]
        public IMapDefinition Parent
        {
            get;
            set;
        }

        bool ITileSetAbstract.SupportsCustomFiniteDisplayScalesUnconditionally
        {
            get
            {
                return true;
            }
        }

        void ITileSetAbstract.RemoveScaleAt(int index)
        {
            this.FiniteDisplayScale.RemoveAt(index);
        }

        double ITileSetAbstract.GetScaleAt(int index)
        {
            return this.FiniteDisplayScale[index];
        }

        [XmlIgnore]
        int ITileSetAbstract.GroupCount
        {
            get { return this.BaseMapLayerGroup.Count; }
        }

        IBaseMapGroup ITileSetAbstract.GetGroupAt(int index)
        {
            return this.BaseMapLayerGroup[index];
        }

        [XmlIgnore]
        int ITileSetAbstract.ScaleCount
        {
            get { return this.FiniteDisplayScale.Count; }
        }

        public void AddFiniteDisplayScale(double value)
        {
            this.FiniteDisplayScale.Add(value);
        }

        public void RemoveFiniteDisplayScale(double value)
        {
            this.FiniteDisplayScale.Remove(value);
        }

        void ITileSetAbstract.SetFiniteDisplayScales(IEnumerable<double> scales)
        {
            this.FiniteDisplayScale.Clear();
            foreach (double scale in scales.OrderBy(s => s))
            {
                this.FiniteDisplayScale.Add(scale);
            }
            OnPropertyChanged(nameof(FiniteDisplayScale));
        }

        void ITileSetAbstract.RemoveAllScales()
        {
            this.FiniteDisplayScale.Clear();
            OnPropertyChanged(nameof(FiniteDisplayScale));
        }

        [XmlIgnore]
        IEnumerable<double> ITileSetAbstract.FiniteDisplayScale
        {
            get
            {
                foreach (double dbl in this.FiniteDisplayScale)
                {
                    yield return dbl;
                }
            }
        }

        IEnumerable<IBaseMapLayer> ITileSetAbstract.GetLayersForGroup(string groupName)
        {
            return this.GetLayersForGroup(groupName);
        }

        [XmlIgnore]
        bool ITileSetAbstract.SupportsCustomFiniteDisplayScales
        {
            get { return true; }
        }

        [XmlIgnore]
        IEnumerable<IBaseMapGroup> ITileSetAbstract.BaseMapLayerGroups
        {
            get
            {
                foreach (var grp in this.BaseMapLayerGroup)
                {
                    yield return grp;
                }
            }
        }

        public IBaseMapGroup AddBaseLayerGroup(string name)
        {
            var grp = new BaseMapLayerGroupCommonType()
            {
                Parent = this.Parent,
                Name = name,
                BaseMapLayer = new System.ComponentModel.BindingList<BaseMapLayerType>(),
                ExpandInLegend = true,
                ShowInLegend = true,
                LegendLabel = name,
                Visible = true
            };
            this.BaseMapLayerGroup.Add(grp);

            return grp;
        }

        public void RemoveBaseLayerGroup(IBaseMapGroup group)
        {
            var grp = group as BaseMapLayerGroupCommonType;
            if (grp != null)
                this.BaseMapLayerGroup.Remove(grp);
        }
    }

    partial class MapLayerType : IMapLayer
    {
    }

    partial class BaseMapLayerType : IBaseMapLayer
    {
        [XmlIgnore]
        public IMapDefinition Parent
        {
            get;
            set;
        }
    }

    partial class MapLayerGroupType : IMapLayerGroup
    {
        [XmlIgnore]
        public IMapDefinition Parent
        {
            get;
            set;
        }
    }

    partial class BaseMapLayerGroupCommonType : IBaseMapGroup
    {
        [XmlIgnore]
        public IMapDefinition Parent
        {
            get;
            set;
        }

        [XmlIgnore]
        IEnumerable<IBaseMapLayer> IBaseMapGroup.BaseMapLayer
        {
            get
            {
                foreach (var lyr in this.BaseMapLayer)
                {
                    yield return lyr;
                }
            }
        }

        public int GetIndex(IBaseMapLayer layer)
        {
            var bl = layer as BaseMapLayerType;
            if (bl != null)
            {
                return this.BaseMapLayer.IndexOf(bl);
            }
            return -1;
        }

        public void InsertLayer(int index, IBaseMapLayer layer)
        {
            var bl = layer as BaseMapLayerType;
            if (bl != null)
            {
                this.BaseMapLayer.Insert(index, bl);
            }
        }

        public IBaseMapLayer AddLayer(string layerName, string resourceId)
        {
            BaseMapLayerType layer = new BaseMapLayerType()
            {
                Parent = this.Parent,
                ExpandInLegend = true,
                LegendLabel = layerName,
                Name = layerName,
                ResourceId = resourceId,
                ShowInLegend = true,
                Selectable = true
            };
            this.BaseMapLayer.Add(layer);
            OnPropertyChanged(nameof(BaseMapLayer));
            return layer;
        }

        public void RemoveBaseMapLayer(IBaseMapLayer layer)
        {
            var lyr = layer as BaseMapLayerType;
            if (lyr != null)
            {
                this.BaseMapLayer.Remove(lyr);
                OnPropertyChanged(nameof(BaseMapLayer));
            }
        }

        int IBaseMapGroup.MoveUp(IBaseMapLayer layer)
        {
            var lyr = layer as BaseMapLayerType;
            if (lyr != null)
            {
                int isrc = this.BaseMapLayer.IndexOf(lyr);
                if (isrc > 0)
                {
                    int idst = isrc - 1;
                    var src = this.BaseMapLayer[isrc];
                    var dst = this.BaseMapLayer[idst];

                    //swap
                    this.BaseMapLayer[isrc] = dst;
                    this.BaseMapLayer[idst] = src;

                    OnPropertyChanged(nameof(BaseMapLayer));

                    return idst;
                }
            }

            return -1;
        }

        int IBaseMapGroup.MoveDown(IBaseMapLayer layer)
        {
            var lyr = layer as BaseMapLayerType;
            if (lyr != null)
            {
                int isrc = this.BaseMapLayer.IndexOf(lyr);
                if (isrc < this.BaseMapLayer.Count - 1)
                {
                    int idst = isrc + 1;
                    var src = this.BaseMapLayer[isrc];
                    var dst = this.BaseMapLayer[idst];

                    //swap
                    this.BaseMapLayer[isrc] = dst;
                    this.BaseMapLayer[idst] = src;

                    OnPropertyChanged(nameof(BaseMapLayer));

                    return idst;
                }
            }

            return -1;
        }
    }
}