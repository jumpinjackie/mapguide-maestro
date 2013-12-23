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
    partial class RuntimeMap : IRuntimeMapInfo
    {
        string IRuntimeMapInfo.SiteVersion
        {
            get { return this.SiteVersion; }
        }

        string IRuntimeMapInfo.Name
        {
            get { return this.Name; }
        }

        string IRuntimeMapInfo.MapDefinition
        {
            get { return this.MapDefinition; }
        }

        string IRuntimeMapInfo.BackgroundColor
        {
            get { return this.BackgroundColor; }
        }

        int IRuntimeMapInfo.DisplayDpi
        {
            get 
            {
                int dpi;
                if (int.TryParse(this.DisplayDpi, out dpi))
                    return dpi;
                return 96;
            }
        }

        string IRuntimeMapInfo.IconMimeType
        {
            get { return this.IconMimeType; }
        }

        ICoordinateSystemInfo IRuntimeMapInfo.CoordinateSystem
        {
            get { return this.CoordinateSystem; }
        }

        IEnvelope IRuntimeMapInfo.Extents
        {
            get 
            {
                double minx;
                double miny;
                double maxx;
                double maxy;
                if (double.TryParse(this.Extents.LowerLeftCoordinate.X, out minx) &&
                    double.TryParse(this.Extents.LowerLeftCoordinate.Y, out miny) &&
                    double.TryParse(this.Extents.UpperRightCoordinate.X, out maxx) &&
                    double.TryParse(this.Extents.UpperRightCoordinate.Y, out maxy))
                {
                    return ObjectFactory.CreateEnvelope(minx, miny, maxx, maxy);
                }
                return null;
            }
        }

        ReadOnlyLayerCollection _roLayers;

        IRuntimeLayerInfoCollection IRuntimeMapInfo.Layers
        {
            get
            {
                if (this.Layer == null)
                    return null;

                if (_roLayers == null)
                    _roLayers = new ReadOnlyLayerCollection(this.Layer);

                return _roLayers;
            }
        }

        ReadOnlyGroupCollection _roGroups;

        IRuntimeLayerGroupInfoCollection IRuntimeMapInfo.Groups
        {
            get 
            {
                if (this.Group == null)
                    return null;

                if (_roGroups == null)
                    _roGroups = new ReadOnlyGroupCollection(this.Group);

                return _roGroups;
            }
        }

        double[] IRuntimeMapInfo.FiniteDisplayScales
        {
            get
            {
                return this.FiniteDisplayScale.ToArray();
            }
        }
    }

    //I love C# generics!
    internal abstract class ReadOnlyCollectionWrapper<TInterface, TImpl> 
        : IReadOnlyCollection<TInterface>
        where TImpl : TInterface
    {
        private IList<TImpl> _list;

        protected ReadOnlyCollectionWrapper(IList<TImpl> list)
        {
            _list = list;
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public TInterface this[int index]
        {
            get { return _list[index]; }
        }

        class Enumerator : IEnumerator<TInterface>
        {
            private IList<TImpl> _innerList;
            private int _pos;

            public Enumerator(IList<TImpl> list)
            {
                _pos = -1;
                _innerList = list;
            }

            public TInterface Current
            {
                get { return _innerList[_pos]; }
            }

            public void Dispose()
            {
                
            }

            object System.Collections.IEnumerator.Current
            {
                get { return _innerList[_pos]; }
            }

            public bool MoveNext()
            {
                _pos++;
                return _pos <= _innerList.Count - 1;
            }

            public void Reset()
            {
                _pos = -1;
            }
        }

        public IEnumerator<TInterface> GetEnumerator()
        {
            return new Enumerator(_list);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }

    partial class RuntimeMapLayer : IRuntimeLayerInfo
    {

        int IRuntimeLayerInfo.LayerType
        {
            get { return Convert.ToInt32(this.Type); }
        }

        IFeatureSourceInfo IRuntimeLayerInfo.FeatureSource
        {
            get { return this.FeatureSource; }
        }

        ScaleRangeCollection _roScales;

        IScaleRangeInfoCollection IRuntimeLayerInfo.ScaleRanges
        {
            get
            {
                if (this.ScaleRange == null)
                    return null;

                if (_roScales == null)
                    _roScales = new ScaleRangeCollection(this.ScaleRange);

                return _roScales;
            }
        }

        string IRuntimeMapLegendElement.ObjectID
        {
            get { return this.ObjectId; }
        }

        string IRuntimeMapLegendElement.ParentID
        {
            get { return this.ParentId; }
        }
    }

    partial class FeatureSourceInfo : IFeatureSourceInfo
    {
        string IFeatureSourceInfo.ResourceID
        {
            get { return this.ResourceId; }
        }
    }

    partial class RuntimeMapGroup : IRuntimeLayerGroupInfo
    {
        int IRuntimeLayerGroupInfo.GroupType
        {
            get { return Convert.ToInt32(this.Type); }
        }

        string IRuntimeMapLegendElement.ObjectID
        {
            get { return this.ObjectId; }
        }

        string IRuntimeMapLegendElement.ParentID
        {
            get { return this.ParentId; }
        }
    }

    internal class ReadOnlyLayerCollection : ReadOnlyCollectionWrapper<IRuntimeLayerInfo, RuntimeMapLayer>,
                                             IRuntimeLayerInfoCollection
    {
        public ReadOnlyLayerCollection(IList<RuntimeMapLayer> list) : base(list) { }
    }

    internal class ReadOnlyGroupCollection : ReadOnlyCollectionWrapper<IRuntimeLayerGroupInfo, RuntimeMapGroup>,
                                             IRuntimeLayerGroupInfoCollection
    {
        public ReadOnlyGroupCollection(IList<RuntimeMapGroup> list) : base(list) { }
    }

    internal class ScaleRangeCollection : ReadOnlyCollectionWrapper<IScaleRangeInfo, ScaleRangeInfo>,
                                          IScaleRangeInfoCollection
    {
        public ScaleRangeCollection(IList<ScaleRangeInfo> list) : base(list) { }
    }

    internal class FeatureStyleCollection : ReadOnlyCollectionWrapper<IFeatureStyleInfo, FeatureStyleInfo>,
                                            IFeatureStyleInfoCollection
    {
        public FeatureStyleCollection(IList<FeatureStyleInfo> list) : base(list) { }
    }

    internal class RuleInfoCollection : ReadOnlyCollectionWrapper<IRuleInfo, RuleInfo>,
                                        IRuleInfoCollection
    {
        public RuleInfoCollection(IList<RuleInfo> list) : base(list) { }
    }

    partial class ScaleRangeInfo : IScaleRangeInfo
    {
        FeatureStyleCollection _roStyles;

        IFeatureStyleInfoCollection IScaleRangeInfo.FeatureStyle
        {
            get 
            {
                if (this.FeatureStyle == null)
                    return null;

                if (_roStyles == null)
                    _roStyles = new FeatureStyleCollection(this.FeatureStyle);

                return _roStyles;
            }
        }
    }

    partial class FeatureStyleInfo : IFeatureStyleInfo
    {
        int IFeatureStyleInfo.Type
        {
            get { return Convert.ToInt32(this.Type); }
        }

        RuleInfoCollection _roRules;

        /// <summary>
        /// Gets the rules in this feature style
        /// </summary>
        public IRuleInfoCollection Rules
        {
            get 
            {
                if (this.Rule == null)
                    return null;

                if (_roRules == null)
                    _roRules = new RuleInfoCollection(this.Rule);

                return _roRules;
            }
        }
    }

    partial class RuleInfo : IRuleInfo
    {
        string IRuleInfo.IconBase64
        {
            get { return this.Icon; }
        }
    }

    partial class CoordinateSystemType : ICoordinateSystemInfo
    {

    }
}
