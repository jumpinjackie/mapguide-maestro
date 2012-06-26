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

        public LocalRuntimeMap(LocalConnection conn, MgdMap map) : base(conn)
        { 
            _impl = map;
            InitializeLayersAndGroups();
        }

        private void InitializeLayersAndGroups()
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
                this.Layers.Add(new LocalRuntimeMapLayer(this, layers.GetItem(i)));
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
                _impl.DisplayDpi = value;
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
                _impl.SetDisplaySize(_impl.DisplayWidth, value);
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
                _impl.SetDisplaySize(value, _impl.DisplayHeight);
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
            protected set
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
            _impl.SetViewCenterXY(x, y);
        }

        public override double ViewScale
        {
            get
            {
                return _impl.ViewScale;
            }
            set
            {
                _impl.SetViewScale(value);
            }
        }

        public override int WatermarkUsage
        {
            get
            {
                return _impl.GetWatermarkUsage();
            }
        }

        public override void Save()
        {
            //Synchronize the ordering of our layers and groups

        }

        public MgdMap GetWrappedInstance() { return _impl; }
    }

    internal class LocalRuntimeMapGroup : RuntimeMapGroup
    {
        private LocalRuntimeMap _parent;
        private MgLayerGroup _impl;

        public LocalRuntimeMapGroup(LocalRuntimeMap parent, MgLayerGroup group) : base(parent, "")
        {
            _parent = parent;
            _impl = group;
        }

        public override bool ExpandInLegend
        {
            get
            {
                return _impl.ExpandInLegend;
            }
            set
            {
                MgdMap.SetGroupExpandInLegend(_impl, value);
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
                var groups = impl.GetLayerGroups();
                if (groups.IndexOf(value) >= 0)
                {
                    var grp = groups.GetItem(value);
                    _impl.Group = grp;
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
                _impl.LegendLabel = value;
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
                _impl.SetDisplayInLegend(value);
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
                _impl.Visible = value;
            }
        }
    }

    internal class LocalRuntimeMapLayer : RuntimeMapLayer
    {
        private LocalRuntimeMap _parent;
        private MgLayerBase _impl;

        public LocalRuntimeMapLayer(LocalRuntimeMap parent, MgLayerBase layer) : base(parent)
        {
            _parent = parent;
            _impl = layer;
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
                MgdMap.SetLayerExpandInLegend(_impl, value);
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
                var groups = impl.GetLayerGroups();
                if (groups.IndexOf(value) >= 0)
                {
                    var grp = groups.GetItem(value);
                    _impl.Group = grp;
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
                _impl.LegendLabel = value;
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
                return _impl.GetClassName();
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
                _impl.Selectable = value;
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
                _impl.DisplayInLegend = value;
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
                _impl.Visible = value;
            }
        }
    }
}
