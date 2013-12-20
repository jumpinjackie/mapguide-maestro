#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.Linq;
using System.Text;
using OSGeo.MapGuide.MaestroAPI.Mapping;
using System.Drawing;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace OSGeo.MapGuide.MaestroAPI.Local
{
    // A general note about this implementation
    //
    // The only thing this shares with RuntimeMap is the ancestry (which is needed if we want to use rendering APIs) because
    // other than that, these implementations are complete wrappers around their respective MgdMap, MgLayerGroup and MgLayerBase
    // classes, barely touching anything from their respective superclasses (and almost overriding everything from their parents)

    internal class LocalRuntimeMap : RuntimeMap
    {
        private MgdMap _impl;
        private LocalConnection _conn;

        public LocalRuntimeMap(LocalConnection conn, MgdMap map, bool suppressErrors) : base(conn)
        { 
            _impl = map;
            _conn = conn;
            InitializeLayersAndGroups(suppressErrors);
            _disableChangeTracking = false;
        }

        public override bool SupportsMutableBackgroundColor
        {
            get
            {
                return false;
            }
        }

        public override bool SupportsMutableExtents
        {
            get
            {
                return false;
            }
        }

        public override bool SupportsMutableCoordinateSystem
        {
            get
            {
                return false;
            }
        }

        public override bool SupportsMutableMetersPerUnit
        {
            get
            {
                return false;
            }
        }

        private void InitializeLayersAndGroups(bool suppressErrors)
        {
            this.Layers.Clear();
            this.Groups.Clear();

            var groups = _impl.GetLayerGroups();
            var layers = _impl.GetLayers();

            //Groups first
            for (int i = 0; i < groups.GetCount(); i++)
            {
                this.Groups.Add(new LocalRuntimeMapGroup(this, groups.GetItem(i)));
            }
            
            //Then layers
            for (int i = 0; i < layers.GetCount(); i++)
            {
                this.Layers.Add(new LocalRuntimeMapLayer(this, layers.GetItem(i), _conn, suppressErrors));
            }
        }

        public override System.Drawing.Color BackgroundColor
        {
            get
            {
                var bgColor = _impl.GetBackgroundColor();
                if (bgColor.Length == 8 || bgColor.Length == 6)
                {
                    return ColorTranslator.FromHtml("#" + bgColor);
                }
                throw new InvalidOperationException("Unsure how to convert color: " + bgColor);
            }
            set
            {
                if (_disableChangeTracking) return; //Still initializing
                throw new NotSupportedException();
            }
        }

        public override string CoordinateSystem
        {
            get
            {
                return _impl.GetMapSRS();
            }
        }

        public override ObjectModels.Common.IEnvelope DataExtent
        {
            get
            {
                var env = _impl.GetDataExtent();
                var envLL = env.GetLowerLeftCoordinate();
                var envUR = env.GetUpperRightCoordinate();

                return ObjectFactory.CreateEnvelope(envLL.X, envLL.Y, envUR.X, envUR.Y);
            }
            set
            {
                if (_disableChangeTracking) return; //Still initializing
                throw new NotSupportedException();
            }
        }

        public override int DisplayDpi
        {
            get
            {
                return _impl.DisplayDpi;
            }
            set
            {
                Action<int> setter = (val) => { _impl.DisplayDpi = val; };
                ObservableSet(_impl.DisplayDpi, value, setter, "DisplayDpi");
            }
        }

        public override int DisplayHeight
        {
            get
            {
                return _impl.DisplayHeight;
            }
            set
            {
                Action<int> setter = (val) => { _impl.SetDisplaySize(_impl.DisplayWidth, val); };
                ObservableSet(_impl.DisplayHeight, value, setter, "DisplayHeight");
            }
        }

        public override int DisplayWidth
        {
            get
            {
                return _impl.DisplayWidth;
            }
            set
            {
                Action<int> setter = (val) => { _impl.SetDisplaySize(val, _impl.DisplayHeight); };
                ObservableSet(_impl.DisplayWidth, value, setter, "DisplayWidth");
            }
        }

        public override int LayerRefreshMode
        {
            get
            {
                return base.LayerRefreshMode;
            }
        }

        public override string MapDefinition
        {
            get
            {
                return _impl.MapDefinition.ToString();
            }
        }

        public override ObjectModels.Common.IEnvelope MapExtent
        {
            get
            {
                var env = _impl.GetMapExtent();
                var envLL = env.GetLowerLeftCoordinate();
                var envUR = env.GetUpperRightCoordinate();

                return ObjectFactory.CreateEnvelope(envLL.X, envLL.Y, envUR.X, envUR.Y);
            }
            set
            {
                if (_disableChangeTracking) return; //Still initializing
                throw new NotSupportedException();
            }
        }

        public override double MetersPerUnit
        {
            get
            {
                return _impl.GetMetersPerUnit();
            }
            set { throw new NotImplementedException(); }
        }

        public override string ObjectId
        {
            get
            {
                return _impl.GetObjectId();
            }
        }

        public override ObjectModels.Common.IPoint2D ViewCenter
        {
            get
            {
                var pt = _impl.ViewCenter;
                var coord = pt.Coordinate;
                return ObjectFactory.CreatePoint2D(coord.X, coord.Y);
            }
        }

        public override void SetViewCenter(double x, double y)
        {
            var center = this.ViewCenter;
            if (center.X != x || center.Y != y)
            {
                _impl.SetViewCenterXY(x, y);
                OnPropertyChanged("ViewCenter");
            }
        }

        public override double ViewScale
        {
            get
            {
                return _impl.ViewScale;
            }
            set
            {
                Action<double> setter = (val) => { _impl.SetViewScale(val); };
                ObservableSet(_impl.ViewScale, value, setter, "ViewScale");
            }
        }

        public override int WatermarkUsage
        {
            get
            {
                return _impl.GetWatermarkUsage();
            }
        }

        public override bool IsDirty
        {
            get
            {
                return base.IsDirty;
            }
            protected set
            {
                if (_disableChangeTracking) return;
                base.IsDirty = value;
            }
        }

        public override void Save()
        {
            //Synchronize the ordering of our layers and groups
            var layers = _impl.GetLayers();
            var groups = _impl.GetLayerGroups();

            layers.Clear();
            groups.Clear();

            foreach (LocalRuntimeMapGroup group in this.Groups)
            {
                groups.Add(group.GetWrappedInstance());
            }

            foreach (LocalRuntimeMapLayer layer in this.Layers)
            {
                layers.Add(layer.GetWrappedInstance());
            }

            this.IsDirty = false;
        }

        public MgdMap GetWrappedInstance() { return _impl; }

        public override void Deserialize(Serialization.MgBinaryDeserializer d)
        {
            
        }

        public override void Serialize(Serialization.MgBinarySerializer s)
        {
            
        }

        internal void ResetDirtyState() { this.IsDirty = false; }
        internal void MakeDirty() { this.IsDirty = true; }
    }

    internal class LocalRuntimeMapGroup : RuntimeMapGroup
    {
        private LocalRuntimeMap _parent;
        private MgLayerGroup _impl;

        public LocalRuntimeMapGroup(LocalRuntimeMap parent, MgLayerGroup group) : base(parent, "")
        {
            _parent = parent;
            _impl = group;
            _disableChangeTracking = false;
        }

        public override bool ExpandInLegend
        {
            get
            {
                return _impl.ExpandInLegend;
            }
            set
            {
                Action<bool> setter = (val) => { MgdMap.SetGroupExpandInLegend(_impl, val); };
                ObservableSet(_impl.ExpandInLegend, value, setter, "ExpandInLegend");
            }
        }

        public override string Group
        {
            get
            {
                var grp = _impl.Group;
                if (grp != null)
                    return grp.Name;
                return string.Empty;
            }
            set
            {
                if (_disableChangeTracking) return; //Still initializing it seems
                var impl = _parent.GetWrappedInstance();
                var groups = impl.GetLayerGroups();
                if (groups.IndexOf(value) >= 0)
                {
                    var grp = groups.GetItem(value);
                    if (grp != _impl.Group)
                    {
                        _impl.Group = grp;
                        OnPropertyChanged("Group");
                    }
                }
                else
                {
                    throw new ArgumentException("Group not found: " + value); //LOCALIZEME
                }
            }
        }

        public override string LegendLabel
        {
            get
            {
                return _impl.LegendLabel;
            }
            set
            {
                if (_disableChangeTracking) return; //Still initializing it seems

                Action<string> setter = (val) => { _impl.LegendLabel = val; };
                ObservableSet(_impl.LegendLabel, value, setter, "LegendLabel");
            }
        }

        public override string Name
        {
            get
            {
                return _impl.Name;
            }
            set
            {
                if (_disableChangeTracking) return; //Still initializing it seems
                throw new NotSupportedException();
            }
        }

        public override string ObjectId
        {
            get
            {
                return _impl.GetObjectId();
            }
        }

        public override bool ShowInLegend
        {
            get
            {
                return _impl.GetDisplayInLegend();
            }
            set
            {
                if (_disableChangeTracking) return; //Still initializing it seems
                Action<bool> setter = (val) => { _impl.SetDisplayInLegend(val); };
                ObservableSet(_impl.GetDisplayInLegend(), value, setter, "ShowInLegend");
            }
        }

        public override int Type
        {
            get
            {
                return _impl.LayerGroupType;
            }
        }

        public override bool Visible
        {
            get
            {
                return _impl.Visible;
            }
            set
            {
                if (_disableChangeTracking) return; //Still initializing it seems
                Action<bool> setter = (val) => { _impl.Visible = val; };
                ObservableSet(_impl.Visible, value, setter, "Visible");
            }
        }

        internal MgLayerGroup GetWrappedInstance() { return _impl; }

        public override void Deserialize(Serialization.MgBinaryDeserializer d)
        {
            
        }

        public override void Serialize(Serialization.MgBinarySerializer s)
        {
            
        }
    }

    internal class LocalRuntimeMapLayer : RuntimeMapLayer
    {
        private LocalRuntimeMap _parent;
        private MgLayerBase _impl;

        internal LocalRuntimeMapLayer(LocalRuntimeMap parent, MgLayerBase layer, IResourceService resSvc, bool suppressErrors) : base(parent)
        {
            _parent = parent;
            _impl = layer;
            var ldfId = layer.GetLayerDefinition();
            var ldf = (ILayerDefinition)resSvc.GetResource(ldfId.ToString());
            Initialize(ldf, suppressErrors);
            _disableChangeTracking = false;
        }

        public override bool ExpandInLegend
        {
            get
            {
                return _impl.ExpandInLegend;
            }
            set
            {
                if (_disableChangeTracking) return; //Still initializing it seems
                Action<bool> setter = (val) => { MgdMap.SetLayerExpandInLegend(_impl, val); };
                if (ObservableSet(_impl.ExpandInLegend, value, setter, "ExpandInLegend"))
                    _parent.MakeDirty();
            }
        }

        public override string FeatureSourceID
        {
            get
            {
                return _impl.FeatureSourceId;
            }
        }

        public override string Filter
        {
            get
            {
                return _impl.Filter;
            }
        }

        public override string GeometryPropertyName
        {
            get
            {
                return _impl.GetFeatureGeometryName();
            }
        }

        public override string Group
        {
            get
            {
                var grp = _impl.Group;
                if (grp != null)
                    return grp.Name;
                return null;
            }
            set
            {
                if (_disableChangeTracking) return; //Still initializing it seems
                var impl = _parent.GetWrappedInstance();
                var grp = _parent.Groups[value] as LocalRuntimeMapGroup;
                if (grp != null)
                {
                    var implGroup = grp.GetWrappedInstance();
                    if (implGroup != _impl.Group)
                    {
                        _impl.Group = implGroup;
                        OnPropertyChanged("Group");
                        _parent.MakeDirty();
                    }
                }
                else
                {
                    throw new ArgumentException("Group not found: " + value); //LOCALIZEME
                }
            }
        }

        public override string LayerDefinitionID
        {
            get
            {
                return _impl.LayerDefinition.ToString();
            }
        }

        public override string LegendLabel
        {
            get
            {
                return _impl.LegendLabel;
            }
            set
            {
                Action<string> setter = (val) => { _impl.LegendLabel = val; };
                if (ObservableSet(_impl.LegendLabel, value, setter, "LegendLabel"))
                    _parent.MakeDirty();
            }
        }

        public override string Name
        {
            get
            {
                return _impl.Name;
            }
            set
            {
                if (_disableChangeTracking) return; //Still initializing
                throw new NotSupportedException();
            }
        }

        public override bool NeedsRefresh
        {
            get
            {
                return _impl.NeedsRefresh();
            }
        }

        public override string ObjectId
        {
            get
            {
                return _impl.GetObjectId();
            }
        }

        public override string QualifiedClassName
        {
            get
            {
                return _impl.GetFeatureClassName();
            }
        }

        public override bool Selectable
        {
            get
            {
                return _impl.Selectable;
            }
            set
            {
                Action<bool> setter = (val) => { _impl.Selectable = val; };
                if (ObservableSet(_impl.Selectable, value, setter, "Selectable"))
                    _parent.MakeDirty();
            }
        }

        public override bool ShowInLegend
        {
            get
            {
                return _impl.DisplayInLegend;
            }
            set
            {
                Action<bool> setter = (val) => { _impl.DisplayInLegend = val; };
                if (ObservableSet(_impl.DisplayInLegend, value, setter, "ShowInLegend"))
                    _parent.MakeDirty();
            }
        }

        public override int Type
        {
            get
            {
                return _impl.GetLayerType();
            }
        }

        public override bool Visible
        {
            get
            {
                return _impl.Visible;
            }
            set
            {
                Action<bool> setter = (val) => { _impl.Visible = val; };
                if (ObservableSet(_impl.Visible, value, setter, "Visible"))
                    _parent.MakeDirty();
            }
        }

        internal MgLayerBase GetWrappedInstance() { return _impl; }

        public override void Serialize(Serialization.MgBinarySerializer s)
        {
            
        }

        public override void Deserialize(Serialization.MgBinaryDeserializer d)
        {
            
        }
    }
}
