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
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.MaestroAPI.Serialization;

namespace OSGeo.MapGuide.MaestroAPI.Mapping
{
    /// <summary>
    /// Represents a group of layers in a runtime map
    /// </summary>
    public class RuntimeMapGroup : MapObservable
    {
        //From MgLayerGroupType
        internal const int kBaseMap = 2;
        internal const int kNormal = 1;

        internal RuntimeMapGroup() 
        {
            _disableChangeTracking = true;

            this.Type = kNormal;
            this.ObjectId = Guid.NewGuid().ToString();
        }

        internal RuntimeMapGroup(RuntimeMap map, string name) 
            : this()
        {
            this.Parent = map;
            this.Name = name;

            _disableChangeTracking = false;
        }

        internal RuntimeMapGroup(RuntimeMap map, IMapLayerGroup group)
            : this(map, group.Name)
        {
            _disableChangeTracking = true;

            this.ExpandInLegend = group.ExpandInLegend;
            this.LegendLabel = group.LegendLabel;
            this.ShowInLegend = group.ShowInLegend;
            this.Visible = group.Visible;

            this.Type = kNormal;

            _disableChangeTracking = false;
        }

        internal RuntimeMapGroup(RuntimeMap map, IBaseMapGroup group)
            : this(map, group.Name)
        {
            _disableChangeTracking = true;

            this.ExpandInLegend = group.ExpandInLegend;
            this.LegendLabel = group.LegendLabel;
            this.ShowInLegend = group.ShowInLegend;
            this.Visible = group.Visible;

            this.Type = kBaseMap;

            _disableChangeTracking = false;
        }

        private bool _visible;

        /// <summary>
        /// Gets or sets whether this group is visible
        /// </summary>
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                SetField(ref _visible, value, "Visible");
            }
        }

        private string _group;

        /// <summary>
        /// Gets or sets the name of the parent group
        /// </summary>
        public string Group
        {
            get
            {
                return _group;
            }
            set
            {
                SetField(ref _group, value, "Group");
            }
        }

        /// <summary>
        /// Gets the parent runtime map
        /// </summary>
        public RuntimeMap Parent
        {
            get;
            internal set;
        }

        private string _name;

        /// <summary>
        /// Gets or sets the name of this group
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetField(ref _name, value, "Name");
            }
        }

        private bool _showInLegend;

        /// <summary>
        /// Gets or sets whether this group is visible in the legend
        /// </summary>
        public bool ShowInLegend
        {
            get
            {
                return _showInLegend;
            }
            set
            {
                SetField(ref _showInLegend, value, "ShowInLegend");
            }
        }

        private string _legendLabel;

        /// <summary>
        /// Gets or sets the legend label
        /// </summary>
        public string LegendLabel
        {
            get
            {
                return _legendLabel;
            }
            set
            {
                SetField(ref _legendLabel, value, "LegendLabel");
            }
        }

        private bool _expandInLegend;

        /// <summary>
        /// Gets or sets whether this group is expanded in the legend
        /// </summary>
        public bool ExpandInLegend
        {
            get
            {
                return _expandInLegend;
            }
            set
            {
                SetField(ref _expandInLegend, value, "ExpandInLegend");
            }
        }

        /// <summary>
        /// Gets the group type
        /// </summary>
        public int Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the unique identifier for this group
        /// </summary>
        public string ObjectId
        {
            get;
            private set;
        }

        /// <summary>
        /// Serializes this instance
        /// </summary>
        /// <param name="s"></param>
        public void Serialize(MgBinarySerializer s)
        {
            s.Write(this.Group);
            if (s.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
                s.WriteClassId(12001);
            else
                s.WriteClassId(19001);
            s.Write(this.Name);
            s.Write(this.ObjectId);
            s.Write(this.Type);
            s.Write(this.Visible);
            s.Write(this.ShowInLegend);
            s.Write(this.ExpandInLegend);
            s.Write(this.LegendLabel);
        }

        /// <summary>
        /// Initialize this instance using the specified binary stream
        /// </summary>
        /// <param name="d"></param>
        public void Deserialize(MgBinaryDeserializer d)
        {
            this.Group = d.ReadString();
            int objid = d.ReadClassId();
            if (d.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
            {
                if (objid != 12001)
                    throw new Exception("Group must have object id 12001, but had: " + objid);
            }
            else if (objid != 19001)
                throw new Exception("Group must have object id 19001, but had: " + objid);

            this.Name = d.ReadString();
            this.ObjectId = d.ReadString();
            this.Type = d.ReadInt32();
            this.Visible = d.ReadBool();
            this.ShowInLegend = d.ReadBool();
            this.ExpandInLegend = d.ReadBool();
            this.LegendLabel = d.ReadString();
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged(string propertyName)
        {
            if (_disableChangeTracking)
                return;

            //register change items on map
            switch (propertyName)
            {
                case "Group":
                    this.Parent.OnGroupParentChanged(this, this.ObjectId);
                    break;
                case "Visible":
                    this.Parent.OnGroupVisibilityChanged(this, this.Visible ? "1" : "0");
                    break;
                case "ShowInLegend":
                    this.Parent.OnGroupDisplayInLegendChanged(this, this.ShowInLegend ? "1" : "0");
                    break;
                case "LegendLabel":
                    this.Parent.OnGroupLegendLabelChanged(this, this.LegendLabel);
                    break;
            }

            base.OnPropertyChanged(propertyName);
        }
    }
}
