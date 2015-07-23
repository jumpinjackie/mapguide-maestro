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

using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OSGeo.MapGuide.ObjectModels.TileSetDefinition.v3_0_0
{
    partial class TileSetDefinition : ITileSetDefinition
    {
        public static ITileSetDefinition CreateDefault()
        {
            return new TileSetDefinition()
            {
                TileStoreParameters = new TileStoreParametersType()
                {
                    Parameter = new System.ComponentModel.BindingList<NameValuePairType>(),
                    TileProvider = "Default" //NOXLATE
                },
                Extents = new Box2DType() { MaxX = 0.0, MaxY = 0.0, MinX = 0.0, MinY = 0.0 },
                BaseMapLayerGroup = new System.ComponentModel.BindingList<BaseMapLayerGroupCommonType>()
                {
                    new BaseMapLayerGroupCommonType()
                    {
                        Name = "Base Layer Group", //NOXLATE
                        LegendLabel = "Base Layer Group", //NOXLATE
                        Visible = true,
                        ShowInLegend = true,
                        ExpandInLegend = true,
                        BaseMapLayer = new System.ComponentModel.BindingList<BaseMapLayerType>()
                    }
                }
            };
        }

        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")] //NOXLATE
        public string ValidatingSchema
        {
            get { return "TileSetDefinition-3.0.0.xsd"; } //NOXLATE
            set { }
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
                    throw new InvalidOperationException(Strings.ErrorInvalidResourceIdentifier);

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.TileSetDefinition.ToString())
                    throw new InvalidOperationException(string.Format(Strings.ErrorUnexpectedResourceType, res.ToString(), ResourceTypes.SymbolDefinition));

                _resId = value;
                this.OnPropertyChanged(nameof(ResourceID));
            }
        }

        [XmlIgnore]
        public string ResourceType
        {
            get { return ResourceTypes.TileSetDefinition.ToString(); }
        }

        [XmlIgnore]
        public bool IsStronglyTyped
        {
            get { return true; }
        }

        protected static readonly Version RES_VERSION = new Version(3, 0, 0);

        [XmlIgnore]
        public Version ResourceVersion
        {
            get { return RES_VERSION; }
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public static Stream Serialize(IResource res)
        {
            return res.SerializeToStream();
        }

        ITileStoreParameters ITileSetDefinition.TileStoreParameters
        {
            get { return this.TileStoreParameters; }
        }

        Common.IEnvelope ITileSetDefinition.Extents
        {
            get
            {
                return this.Extents;
            }
            set
            {
                if (this.Extents == null)
                {
                    var ext = new Box2DType()
                    {
                        MaxX = value.MaxX,
                        MaxY = value.MaxY,
                        MinX = value.MinX,
                        MinY = value.MinY
                    };
                    this.Extents = ext;
                }
                else
                {
                    this.Extents.MaxX = value.MaxX;
                    this.Extents.MaxY = value.MaxY;
                    this.Extents.MinX = value.MinX;
                    this.Extents.MinY = value.MinY;
                    OnPropertyChanged(nameof(Extents));
                }
            }
        }

        public IEnumerable<IBaseMapLayer> GetLayersForGroup(string groupName)
        {
            foreach (var grp in this.BaseMapLayerGroup)
            {
                if (grp.Name == groupName)
                {
                    foreach (var layer in grp.BaseMapLayer)
                    {
                        yield return layer;
                    }
                }
            }
        }

        public IEnumerable<double> FiniteDisplayScale
        {
            get
            {
                if (!this.SupportsCustomFiniteDisplayScales)
                    throw new InvalidOperationException();

                return this.GetDefaultFiniteScaleList();
            }
        }

        public bool SupportsCustomFiniteDisplayScalesUnconditionally
        {
            get
            {
                return false;
            }
        }

        public bool SupportsCustomFiniteDisplayScales
        {
            get
            {
                return this.TileStoreParameters.TileProvider == "Default";
            }
        }

        public IEnumerable<IBaseMapGroup> BaseMapLayerGroups
        {
            get 
            {
                if (this.BaseMapLayerGroup != null )
                {
                    foreach (var grp in this.BaseMapLayerGroup)
                    {
                        yield return grp;
                    }
                }
            }
        }


        public int GroupCount
        {
            get { return this.BaseMapLayerGroup.Count; }
        }

        public IBaseMapGroup GetGroupAt(int index)
        {
            return this.BaseMapLayerGroup[index];
        }

        public IBaseMapGroup AddBaseLayerGroup(string name)
        {
            var grp = new BaseMapLayerGroupCommonType()
            {
                Name = name,
                LegendLabel = name,
                BaseMapLayer = new System.ComponentModel.BindingList<BaseMapLayerType>()
            };
            this.BaseMapLayerGroup.Add(grp);
            return grp;
        }

        public void RemoveBaseLayerGroup(IBaseMapGroup group)
        {
            var grp = group as BaseMapLayerGroupCommonType;
            if (grp != null)
            {
                this.BaseMapLayerGroup.Remove(grp);
            }
        }

        public void AddFiniteDisplayScale(double value)
        {
            if (!this.SupportsCustomFiniteDisplayScales)
                throw new InvalidOperationException();

            this.SetDefaultFiniteScaleList(this.GetDefaultFiniteScaleList().Concat(new double[] { value }));
        }

        public void RemoveFiniteDisplayScale(double value)
        {
            if (!this.SupportsCustomFiniteDisplayScales)
                throw new InvalidOperationException();

            var list = new List<double>(this.GetDefaultFiniteScaleList());
            if (list.Remove(value))
                this.SetDefaultFiniteScaleList(list);
        }

        public int ScaleCount
        {
            get 
            {
                if (!this.SupportsCustomFiniteDisplayScales)
                    throw new InvalidOperationException();
                return this.FiniteDisplayScale.Count(); 
            }
        }

        public void RemoveScaleAt(int index)
        {
            if (!this.SupportsCustomFiniteDisplayScales)
                throw new InvalidOperationException();

            if (index >= this.ScaleCount)
                throw new ArgumentOutOfRangeException(nameof(index));

            var list = new List<double>(this.GetDefaultFiniteScaleList());
            list.RemoveAt(index);
            this.SetDefaultFiniteScaleList(list);
        }

        public double GetScaleAt(int index)
        {
            if (!this.SupportsCustomFiniteDisplayScales)
                throw new InvalidOperationException();

            return this.GetDefaultFiniteScaleList().ElementAt(index);
        }

        public void SetFiniteDisplayScales(IEnumerable<double> scales)
        {
            this.SetDefaultFiniteScaleList(scales.OrderBy(s => s));
        }

        public void RemoveAllScales()
        {
            if (!this.SupportsCustomFiniteDisplayScales)
                throw new InvalidOperationException();

            this.SetDefaultFiniteScaleList(Enumerable.Empty<double>());
        }
    }

    partial class Box2DType : IEnvelope
    {

    }

    partial class TileStoreParametersType : ITileStoreParameters
    {
        public void AddParameter(string name, string value)
        {
            this.Parameter.Add(new NameValuePairType() { Name = name, Value = value });
            OnParametersChanged();
        }

        public void SetParameter(string name, string value)
        {
            if (this.Parameter != null)
            {
                var param = this.Parameter.Where(x => x.Name == name);
                if (param.Count() == 0)
                {
                    this.AddParameter(name, value);
                }
                else if (param.Count() > 1)
                {
                    //Remove and replace
                    foreach(var p in param)
                    {
                        this.Parameter.Remove(p);
                    }
                    this.AddParameter(name, value);
                }
                else //1
                {
                    var p = param.First();
                    if (p.Value != value)
                    {
                        p.Value = value;
                        OnParametersChanged();
                    }
                }
            }
        }

        public IEnumerable<INameStringPair> Parameters
        {
            get
            {
                if (this.Parameter != null)
                {
                    foreach (var nvp in this.Parameter)
                    {
                        yield return nvp;
                    }
                }
            }
        }

        public void ClearParameters()
        {
            if (this.Parameter != null)
            {
                this.Parameter.Clear();
            }
        }

        private void OnParametersChanged()
        {
            var h = this.ParametersChanged;
            if (h != null)
                h(this, EventArgs.Empty);
        }

        public event EventHandler ParametersChanged;
    }

    partial class NameValuePairType : INameStringPair
    {

    }

    partial class BaseMapLayerType : IBaseMapLayer
    {
        [XmlIgnore]
        public ITileSetDefinition Parent
        {
            get;
            set;
        }
    }

    partial class BaseMapLayerGroupCommonType : IBaseMapGroup
    {
        [XmlIgnore]
        public ITileSetDefinition Parent
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
