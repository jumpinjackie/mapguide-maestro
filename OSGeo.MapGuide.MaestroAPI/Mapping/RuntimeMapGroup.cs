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
    /// Represents a group of layers in a runtime map. Use <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IMappingService.CreateMapGroup(OSGeo.MapGuide.MaestroAPI.Mapping.RuntimeMap,OSGeo.MapGuide.ObjectModels.MapDefinition.IMapLayerGroup)"/> to create
    /// instances of this class.
    /// </summary>
    public class RuntimeMapGroup : MapObservable
    {
        //From MgLayerGroupType
        internal const int kBaseMap = 2;
        internal const int kNormal = 1;

        internal RuntimeMapGroup() 
        {
            _disableChangeTracking = true;
            this.Group = string.Empty;
            this.LegendLabel = string.Empty;
            this.Type = kNormal;
            this.ObjectId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeMapGroup"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="name">The name.</param>
        protected internal RuntimeMapGroup(RuntimeMap map, string name) 
            : this()
        {
            this.Parent = map;
            this.Name = name;

            _disableChangeTracking = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeMapGroup"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="group">The group.</param>
        protected internal RuntimeMapGroup(RuntimeMap map, IMapLayerGroup group)
            : this(map, group.Name)
        {
            _disableChangeTracking = true;

            this.Group = group.Group;
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
        public virtual bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                SetField(ref _visible, value, "Visible"); //NOXLATE
            }
        }

        private string _group;

        /// <summary>
        /// Gets or sets the name of the parent group
        /// </summary>
        public virtual string Group
        {
            get
            {
                return _group;
            }
            set
            {
                SetField(ref _group, value, "Group"); //NOXLATE
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
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetField(ref _name, value, "Name"); //NOXLATE
            }
        }

        private bool _showInLegend;

        /// <summary>
        /// Gets or sets whether this group is visible in the legend
        /// </summary>
        public virtual bool ShowInLegend
        {
            get
            {
                return _showInLegend;
            }
            set
            {
                SetField(ref _showInLegend, value, "ShowInLegend"); //NOXLATE
            }
        }

        private string _legendLabel;

        /// <summary>
        /// Gets or sets the legend label
        /// </summary>
        public virtual string LegendLabel
        {
            get
            {
                return _legendLabel;
            }
            set
            {
                SetField(ref _legendLabel, value, "LegendLabel"); //NOXLATE
            }
        }

        private bool _expandInLegend;

        /// <summary>
        /// Gets or sets whether this group is expanded in the legend
        /// </summary>
        public virtual bool ExpandInLegend
        {
            get
            {
                return _expandInLegend;
            }
            set
            {
                SetField(ref _expandInLegend, value, "ExpandInLegend"); //NOXLATE
            }
        }

        /// <summary>
        /// Gets the group type
        /// </summary>
        public virtual int Type
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the unique identifier for this group
        /// </summary>
        public virtual string ObjectId
        {
            get;
            private set;
        }

        /// <summary>
        /// Serializes this instance
        /// </summary>
        /// <param name="s"></param>
        public virtual void Serialize(MgBinarySerializer s)
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
        public virtual void Deserialize(MgBinaryDeserializer d)
        {
            this.Group = d.ReadString();
            int objid = d.ReadClassId();
            if (d.SiteVersion >= SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
            {
                if (objid != 12001)
                    throw new Exception(string.Format(Strings.ErrorInvalidGroupObjectId, objid));
            }
            else if (objid != 19001)
                throw new Exception(string.Format(Strings.ErrorInvalidGroupObjectId, objid));

            this.Name = d.ReadString();
            this.ObjectId = d.ReadString();
            this.Type = d.ReadInt32();
            this.Visible = d.ReadBool();
            this.ShowInLegend = d.ReadBool();
            this.ExpandInLegend = d.ReadBool();
            this.LegendLabel = d.ReadString();
        }

        /// <summary>
        /// Raises the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged(string propertyName)
        {
            if (_disableChangeTracking)
                return;

            //register change items on map
            switch (propertyName)
            {
                case "Group": //NOXLATE
                    var name = this.Group;
                    if (this.Parent.Groups[name] != null)
                        this.Parent.OnGroupParentChanged(this, this.Parent.Groups[name].ObjectId);
                    else
                        this.Parent.OnGroupParentChanged(this, string.Empty);
                    break;
                case "Visible": //NOXLATE
                    this.Parent.OnGroupVisibilityChanged(this, this.Visible ? "1" : "0"); //NOXLATE
                    break;
                case "ShowInLegend": //NOXLATE
                    this.Parent.OnGroupDisplayInLegendChanged(this, this.ShowInLegend ? "1" : "0"); //NOXLATE
                    break;
                case "LegendLabel": //NOXLATE
                    this.Parent.OnGroupLegendLabelChanged(this, this.LegendLabel);
                    break;
            }

            base.OnPropertyChanged(propertyName);
        }
    }
}
