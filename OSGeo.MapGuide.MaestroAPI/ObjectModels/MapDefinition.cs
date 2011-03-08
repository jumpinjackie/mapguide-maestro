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
using System.Xml.Serialization;
using OSGeo.MapGuide.MaestroAPI;
using System.Drawing;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.MapDefinition;

#pragma warning disable 1591, 0114, 0108

namespace OSGeo.MapGuide.ObjectModels.MapDefinition_1_0_0
{
    partial class MapDefinitionType
    {
        internal MapDefinitionType() { }

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
                OnPropertyChanged("BackgroundColor");
            }
        }
    }

    partial class MapDefinition : IMapDefinition
    {
        internal MapDefinition() { } 

        private static readonly Version RES_VERSION = new Version(1, 0, 0);

        [XmlIgnore]
        public OSGeo.MapGuide.MaestroAPI.IServerConnection CurrentConnection
        {
            get;
            set;
        }

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
                    throw new InvalidOperationException("Not a valid resource identifier"); //LOCALIZE

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.MapDefinition.ToString())
                    throw new InvalidOperationException("Invalid resource identifier for this type of object: " + res.Extension); //LOCALIZE

                _resId = value;
                this.OnPropertyChanged("ResourceID");
            }
        }

        [XmlIgnore]
        public virtual ResourceTypes ResourceType
        {
            get
            {
                return ResourceTypes.MapDefinition;
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

        internal void SortGroupList() { }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string ValidatingSchema 
        { 
            get { return "MapDefinition-1.0.0.xsd"; }
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
                OnPropertyChanged("Extents");
            }
        }

        public IMapLayerGroup AddGroup(string groupName)
        {
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
            OnPropertyChanged("MapLayerGroup");
            return group;
        }

        public IMapLayer AddLayer(string groupName, string layerName, string resourceId)
        { 
            var layer = new MapLayerType() { 
                Parent = this,
                ExpandInLegend = true,
                LegendLabel = layerName,
                Name = layerName,
                ResourceId = resourceId,
                ShowInLegend = true,
                Visible = true,
                Selectable = true
            };
            //TODO: Throw exception if adding to non-existent group?
            layer.Group = string.IsNullOrEmpty(groupName) ? string.Empty : groupName;
            
            this.MapLayer.Add(layer);
            OnPropertyChanged("MapLayer");

            if (this.MapLayer.Count == 1) //First one
            {
                var ldf = (ILayerDefinition)this.CurrentConnection.ResourceService.GetResource(layer.ResourceId);
                if (string.IsNullOrEmpty(this.CoordinateSystem))
                {
                    this.CoordinateSystem = ldf.GetCoordinateSystemWkt();
                }
                if (IsEmpty(this.Extents))
                {
                    var env = ldf.GetSpatialExtent(true);
                    ((IMapDefinition)this).SetExtents(env.MinX, env.MinY, env.MaxX, env.MaxY);
                }
            }

            return layer;
        }


        public IMapLayer AddLayer(IMapLayer layerToInsertAbove, string groupName, string layerName, string resourceId)
        {
            var layer = new MapLayerType()
            {
                Parent = this,
                ExpandInLegend = true,
                LegendLabel = layerName,
                Name = layerName,
                ResourceId = resourceId,
                ShowInLegend = true,
                Visible = true,
                Selectable = true
            };
            //TODO: Throw exception if adding to non-existent group?
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
            OnPropertyChanged("MapLayer");

            if (this.MapLayer.Count == 1) //First one
            {
                var ldf = (ILayerDefinition)this.CurrentConnection.ResourceService.GetResource(layer.ResourceId);
                if (string.IsNullOrEmpty(this.CoordinateSystem))
                {
                    this.CoordinateSystem = ldf.GetCoordinateSystemWkt();
                }
                if (IsEmpty(this.Extents))
                {
                    var env = ldf.GetSpatialExtent(true);
                    ((IMapDefinition)this).SetExtents(env.MinX, env.MinY, env.MaxX, env.MaxY);
                }
            }

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
                OnPropertyChanged("MapLayer");
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
            Check.NotNull(group, "group");
            var grp = group as MapLayerGroupType;
            if (grp != null)
            {
                this.MapLayerGroup.Remove(grp);
                OnPropertyChanged("MapLayerGroup");
            }
        }


        int IMapDefinition.GetIndex(IMapLayer layer)
        {
            Check.NotNull(layer, "layer");
            var lyr = layer as MapLayerType;
            if (lyr != null)
                return this.MapLayer.IndexOf(lyr);

            return -1;
        }

        int IMapDefinition.MoveUp(IMapLayer layer)
        {
            Check.NotNull(layer, "layer");
            int isrc = ((IMapDefinition)this).GetIndex(layer);
            if (isrc > 0)
            {
                int idst = isrc - 1;

                var src = this.MapLayer[isrc];
                var dst = this.MapLayer[idst];

                this.MapLayer[isrc] = dst;
                this.MapLayer[idst] = src;

                OnPropertyChanged("MapLayer");

                return idst;
            }

            return -1;
        }

        int IMapDefinition.MoveDown(IMapLayer layer)
        {
            Check.NotNull(layer, "layer");
            int isrc = ((IMapDefinition)this).GetIndex(layer);
            if (isrc < this.MapLayer.Count - 1)
            {
                int idst = isrc + 1;

                var src = this.MapLayer[isrc];
                var dst = this.MapLayer[idst];

                this.MapLayer[isrc] = dst;
                this.MapLayer[idst] = src;

                OnPropertyChanged("MapLayer");

                return idst;
            }

            return -1;
        }

        int IMapDefinition.GetIndex(IMapLayerGroup group)
        {
            Check.NotNull(group, "group");
            var grp = group as MapLayerGroupType;
            if (grp != null)
                return this.MapLayerGroup.IndexOf(grp);

            return -1;
        }

        void IMapDefinition.SetTopDrawOrder(IMapLayer layer)
        {
            Check.NotNull(layer, "layer");
            int isrc = ((IMapDefinition)this).GetIndex(layer);
            if (isrc > 0)
            {
                var src = this.MapLayer[isrc];
             
                //take everything before this and shift them up one position
                for (int i = isrc - 1; i >= 0; i--)
                {
                    this.MapLayer[i+1] = this.MapLayer[i];
                }

                this.MapLayer[0] = src;
                OnPropertyChanged("MapLayer");
            }
        }

        void IMapDefinition.SetBottomDrawOrder(IMapLayer layer)
        {
            Check.NotNull(layer, "layer");
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
                OnPropertyChanged("MapLayer");
            }
        }
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

        void IBaseMapDefinition.RemoveScaleAt(int index)
        {
            this.FiniteDisplayScale.RemoveAt(index);
        }

        double IBaseMapDefinition.GetScaleAt(int index)
        {
            return this.FiniteDisplayScale[index];
        }

        [XmlIgnore]
        int IBaseMapDefinition.GroupCount
        {
            get { return this.BaseMapLayerGroup.Count; }
        }

        IBaseMapGroup IBaseMapDefinition.GetGroupAt(int index)
        {
            return this.BaseMapLayerGroup[index];
        }

        [XmlIgnore]
        int IBaseMapDefinition.ScaleCount
        {
            get { return this.FiniteDisplayScale.Count; }
        }

        [XmlIgnore]
        IEnumerable<double> IBaseMapDefinition.FiniteDisplayScale
        {
            get 
            {
                foreach (var scale in this.FiniteDisplayScale)
                {
                    yield return scale;
                }
            }
        }

        public void AddFiniteDisplayScale(double value)
        {
            this.FiniteDisplayScale.Add(value);
        }

        public void RemoveFiniteDisplayScale(double value)
        {
            this.FiniteDisplayScale.Remove(value);
        }

        void IBaseMapDefinition.RemoveAllScales()
        {
            this.FiniteDisplayScale.Clear();
            OnPropertyChanged("FiniteDisplayScale");
        }

        [XmlIgnore]
        IEnumerable<IBaseMapGroup> IBaseMapDefinition.BaseMapLayerGroup
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
            OnPropertyChanged("BaseMapLayer");
            return layer;
        }

        public void RemoveBaseMapLayer(IBaseMapLayer layer)
        {
            var lyr = layer as BaseMapLayerType;
            if (lyr != null)
            {
                this.BaseMapLayer.Remove(lyr);
                OnPropertyChanged("BaseMapLayer");
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

                    OnPropertyChanged("BaseMapLayer");

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

                    OnPropertyChanged("BaseMapLayer");

                    return idst;
                }
            }

            return -1;
        }
    }
}
