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
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI.Serialization;
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace OSGeo.MapGuide.MaestroAPI.Mapping
{
    /// <summary>
    /// Describes a FDO property
    /// </summary>
    public class PropertyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public PropertyInfo(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; set; }
    }
    /// <summary>
    /// Represents a runtime map layer
    /// </summary>
    public class RuntimeMapLayer : MapObservable
    {
        //From MgLayerType
        internal const int kBaseMap = 2;
        internal const int kDynamic = 1;

        const double InfinityScale = double.MaxValue;

        internal RuntimeMap Parent { get; private set; }

        internal RuntimeMapLayer(RuntimeMap parent) 
        {
            _scaleRanges = new double[] { 0.0, InfinityScale };
            this.Type = kDynamic;
            this.IdentityProperties = new PropertyInfo[0];
            this.ObjectId = Guid.NewGuid().ToString();
            this.Parent = parent;
        }

        internal RuntimeMapLayer(RuntimeMap parent, ILayerDefinition ldf)
            : this(parent)
        {
            Check.NotNull(ldf, "ldf");

            this.LayerDefinitionID = ldf.ResourceID;
            this.ExpandInLegend = false;
            this.Group = null;
            this.Name = ResourceIdentifier.GetName(ldf.ResourceID);
            this.Selectable = true;
            this.ShowInLegend = true;
            this.Visible = true;

            _disableChangeTracking = false;
        }

        internal RuntimeMapLayer(RuntimeMap parent, IMapLayer source, ILayerDefinition ldf)
            : this(parent, (IBaseMapLayer)source, ldf)
        {
            _disableChangeTracking = true;

            this.Group = source.Group;
            this.Visible = source.Visible;

            _disableChangeTracking = false;
        }

        internal RuntimeMapLayer(RuntimeMap parent, IBaseMapLayer source, ILayerDefinition ldf) 
            : this(parent, ldf)
        {
            Check.NotNull(source, "source");
            Check.NotNull(ldf, "ldf");
            Check.Precondition(source.ResourceId == ldf.ResourceID, "source.ResourceId == ldf.ResourceID");

            _disableChangeTracking = true;

            this.LayerDefinitionID = source.ResourceId;
            this.ExpandInLegend = source.ExpandInLegend;
            this.Name = source.Name;
            this.Selectable = source.Selectable;
            this.ShowInLegend = source.ShowInLegend;
            this.LegendLabel = source.LegendLabel;

            this.NeedsRefresh = false;
            this.DisplayOrder = 0;

            switch (ldf.SubLayer.LayerType)
            {
                case LayerType.Drawing:
                    {
                    }
                    break;
                case LayerType.Raster:
                    {
                        IRasterLayerDefinition rdf = (IRasterLayerDefinition)ldf.SubLayer;
                        this.FeatureSourceID = rdf.ResourceId;
                        this.GeometryPropertyName = rdf.Geometry;
                        this.QualifiedClassName = rdf.FeatureName;

                        if (rdf.GridScaleRangeCount > 0)
                        {
                            _scaleRanges = new double[rdf.GridScaleRangeCount * 2];
                            int i = 0;
                            foreach (var gsr in rdf.GridScaleRange)
                            {
                                _scaleRanges[i * 2] = gsr.MinScale.HasValue ? gsr.MinScale.Value : 0;
                                _scaleRanges[i * 2 + 1] = gsr.MaxScale.HasValue ? gsr.MaxScale.Value : InfinityScale;
                                i++;
                            }
                        }
                    }
                    break;
                case LayerType.Vector:
                    {
                        IVectorLayerDefinition vld = (IVectorLayerDefinition)ldf.SubLayer;
                        this.FeatureSourceID = vld.ResourceId;
                        this.GeometryPropertyName = vld.Geometry;
                        this.QualifiedClassName = vld.FeatureName;
                        this.Filter = vld.Filter;

                        if (vld.HasVectorScaleRanges())
                        {
                            int vsrCount = vld.GetScaleRangeCount();
                            _scaleRanges = new double[vsrCount * 2];
                            for (int i = 0; i < vsrCount; i++)
                            {
                                var vsr = vld.GetScaleRangeAt(i);
                                _scaleRanges[i * 2] = vsr.MinScale.HasValue ? vsr.MinScale.Value : 0;
                                _scaleRanges[i * 2 + 1] = vsr.MaxScale.HasValue ? vsr.MaxScale.Value : InfinityScale;
                            }
                        }
                        this.HasTooltips = !string.IsNullOrEmpty(vld.ToolTip);
                        //get identity property information

                    }
                    break;
            }

            _disableChangeTracking = false;
        }

        private bool _visible;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RuntimeMapLayer"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
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
        /// Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
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
        /// Gets the layer definition ID.
        /// </summary>
        /// <value>The layer definition ID.</value>
        public string LayerDefinitionID
        {
            get;
            private set;
        }

        private bool _selectable;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RuntimeMapLayer"/> is selectable.
        /// </summary>
        /// <value><c>true</c> if selectable; otherwise, <c>false</c>.</value>
        public bool Selectable
        {
            get
            {
                return _selectable;
            }
            set
            {
                SetField(ref _selectable, value, "Selectable");
            }
        }

        private string _name;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
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
        /// Gets or sets a value indicating whether [show in legend].
        /// </summary>
        /// <value><c>true</c> if [show in legend]; otherwise, <c>false</c>.</value>
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
        /// Gets or sets the legend label.
        /// </summary>
        /// <value>The legend label.</value>
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
        /// Gets or sets a value indicating whether [expand in legend].
        /// </summary>
        /// <value><c>true</c> if [expand in legend]; otherwise, <c>false</c>.</value>
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
        /// Gets or sets the feature source ID.
        /// </summary>
        /// <value>The feature source ID.</value>
        public string FeatureSourceID
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the name of the qualified class.
        /// </summary>
        /// <value>The name of the qualified class.</value>
        public string QualifiedClassName
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the identity properties.
        /// </summary>
        /// <value>The identity properties.</value>
        public PropertyInfo[] IdentityProperties
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the object id.
        /// </summary>
        /// <value>The object id.</value>
        public string ObjectId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the name of the geometry property.
        /// </summary>
        /// <value>The name of the geometry property.</value>
        public string GeometryPropertyName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public string Filter
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the type
        /// </summary>
        public int Type
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the display order
        /// </summary>
        public double DisplayOrder
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets whether this layer needs to be refreshed
        /// </summary>
        public bool NeedsRefresh
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets whether this layer has tooltips
        /// </summary>
        public bool HasTooltips
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the schema name
        /// </summary>
        public string SchemaName
        {
            get
            {
                var tokens = this.QualifiedClassName.Split(':');
                if (tokens.Length == 2)
                    return tokens[0];
                return string.Empty;
            }
        }

        private double[] _scaleRanges;

        /// <summary>
        /// Serializes this instance to a binary stream
        /// </summary>
        /// <param name="s"></param>
        public void Serialize(MgBinarySerializer s)
        {
            s.Write(this.Group);

            if (s.SiteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1))
                s.WriteClassId(19003);
            else
                s.WriteClassId(30501);

            s.WriteResourceIdentifier(this.LayerDefinitionID);

            if (s.SiteVersion < SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
            {
                s.Write(this.Name);
                s.Write(this.ObjectId);

                s.Write(this.Type);

                s.Write((byte)(this.Visible ? 1 : 0));
                s.Write((byte)(this.Selectable ? 1 : 0));
                s.Write((byte)(this.ShowInLegend ? 1 : 0));
                s.Write((byte)(this.ExpandInLegend ? 1 : 0));

                s.Write(this.LegendLabel);
                s.Write((byte)(this.NeedsRefresh ? 1 : 0));
                s.Write(this.DisplayOrder);

                s.Write(_scaleRanges.Length);
                foreach (double d in _scaleRanges)
                    s.Write(d);

                s.Write(this.FeatureSourceID);
                s.Write(this.QualifiedClassName);
                s.Write(this.GeometryPropertyName);

                s.Write(this.IdentityProperties.Length);
                foreach (var x in this.IdentityProperties)
                {
                    s.Write((short)ConvertNetTypeToMgType(x.Type));
                    s.Write(x.Name);
                }
            }
            else
            {
                s.WriteStringInternal(this.Name);
                s.WriteStringInternal(this.ObjectId);
                s.WriteRaw(BitConverter.GetBytes(this.Type));
                int flags = 0;
                flags |= this.Visible ? 1 : 0;
                flags |= this.Selectable ? 2 : 0;
                flags |= this.ShowInLegend ? 4 : 0;
                flags |= this.ExpandInLegend ? 8 : 0;
                flags |= this.NeedsRefresh ? 16 : 0;
                flags |= this.HasTooltips ? 32 : 0;
                s.WriteRaw(new byte[] { (byte)flags });

                s.WriteStringInternal(this.LegendLabel);
                s.WriteRaw(BitConverter.GetBytes(this.DisplayOrder));

                s.WriteRaw(BitConverter.GetBytes(_scaleRanges.Length));
                foreach (double d in _scaleRanges)
                    s.WriteRaw(BitConverter.GetBytes(d));

                s.WriteStringInternal(this.FeatureSourceID);
                s.WriteStringInternal(this.QualifiedClassName);
                if (s.SiteVersion > SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS2_1))
                    s.WriteStringInternal(this.Filter);
                s.WriteStringInternal(this.SchemaName);
                s.WriteStringInternal(this.GeometryPropertyName);

                s.WriteRaw(BitConverter.GetBytes(this.IdentityProperties.Length));
                foreach (var x in this.IdentityProperties)
                {
                    s.WriteRaw(BitConverter.GetBytes((short)ConvertNetTypeToMgType(x.Type)));
                    s.WriteStringInternal(x.Name);
                }
            }
        }

        /// <summary>
        /// Initializes this instance with the specified binary stream
        /// </summary>
        /// <param name="d"></param>
        public void Deserialize(MgBinaryDeserializer d)
        {
            this.Group = d.ReadString();

            int classid = d.ReadClassId();
            if (d.SiteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classid != 19003)
                throw new Exception("Resource Identifier expected, but got: " + classid.ToString());
            if (d.SiteVersion > SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classid != 30501)
                throw new Exception("Resource Identifier expected, but got: " + classid.ToString());

            this.LayerDefinitionID = d.ReadResourceIdentifier();

            if (d.SiteVersion < SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
            {
                this.Name = d.ReadString();
                this.ObjectId = d.ReadString();
                this.Type = d.ReadInt32();

                this.Visible = d.ReadByte() > 0;
                this.Selectable = d.ReadByte() > 0;
                this.ShowInLegend = d.ReadByte() > 0;
                this.ExpandInLegend = d.ReadByte() > 0;

                this.LegendLabel = d.ReadString();
                this.NeedsRefresh = d.ReadByte() > 0;
                this.DisplayOrder = d.ReadDouble();

                var scaleRanges = new List<double>();
                int scales = d.ReadInt32();
                while (scales-- > 0)
                    scaleRanges.Add(d.ReadDouble());

                _scaleRanges = scaleRanges.ToArray();
                
                this.FeatureSourceID = d.ReadString();
                this.QualifiedClassName = d.ReadString();
                this.GeometryPropertyName = d.ReadString();

                var ids = new List<PropertyInfo>();
                int idCount = d.ReadInt32();

                while (idCount-- > 0)
                {
                    short idType = d.ReadInt16();
                    string idName = d.ReadString();
                    ids.Add(new PropertyInfo(idName, ConvertMgTypeToNetType(idType)));
                }

                this.IdentityProperties = ids.ToArray();
            }
            else
            {
                //AAARGH!!! Now they bypass their own header system ....
                this.Name = d.ReadInternalString();
                this.ObjectId = d.ReadInternalString();
                this.Type = BitConverter.ToInt32(d.ReadStreamRepeat(4), 0);

                int flags = d.ReadStreamRepeat(1)[0];
                this.Visible = (flags & 1) > 0;
                this.Selectable = (flags & 2) > 0;
                this.ShowInLegend = (flags & 4) > 0;
                this.ExpandInLegend = (flags & 8) > 0;
                this.NeedsRefresh = (flags & 16) > 0;
                this.HasTooltips = (flags & 32) > 0;

                this.LegendLabel = d.ReadInternalString();
                this.DisplayOrder = BitConverter.ToDouble(d.ReadStreamRepeat(8), 0);

                var scaleRanges = new List<double>();
                int scales = BitConverter.ToInt32(d.ReadStreamRepeat(4), 0);
                while (scales-- > 0)
                    scaleRanges.Add(BitConverter.ToDouble(d.ReadStreamRepeat(8), 0));

                _scaleRanges = scaleRanges.ToArray();
                
                this.FeatureSourceID = d.ReadInternalString();
                this.QualifiedClassName = d.ReadInternalString();
                if (d.SiteVersion > SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS2_1))
                    this.Filter = d.ReadInternalString();
                //this.SchemaName = d.ReadInternalString();
                d.ReadInternalString();
                this.GeometryPropertyName = d.ReadInternalString();

                var ids = new List<PropertyInfo>();
                int idCount = BitConverter.ToInt32(d.ReadStreamRepeat(4), 0);

                while (idCount-- > 0)
                {
                    short idType = BitConverter.ToInt16(d.ReadStreamRepeat(2), 0);
                    string idName = d.ReadInternalString();
                    ids.Add(new PropertyInfo(idName, ConvertMgTypeToNetType(idType)));
                }

                this.IdentityProperties = ids.ToArray();
            }
        }

        //from MgPropertyType
        const int Blob = 10;
        const int Boolean = 1;
        const int Byte = 2;
        const int Clob = 11;
        const int DateTime = 3;
        const int Double = 5;
        const int Feature = 12;
        const int Geometry = 13;
        const int Int16 = 6;
        const int Int32 = 7;
        const int Int64 = 8;
        const int Null = 0;
        const int Raster = 14;
        const int Single = 4;
        const int String = 9;

        private static short ConvertNetTypeToMgType(Type type)
        {
            if (type == typeof(short))
                return Int16;
            else if (type == typeof(byte))
                return Byte;
            else if (type == typeof(bool))
                return Boolean;
            else if (type == typeof(int))
                return Int32;
            else if (type == typeof(long))
                return Int64;
            else if (type == typeof(float))
                return Single;
            else if (type == typeof(double))
                return Double;
            else if (type == Utility.GeometryType)
                return Geometry;
            else if (type == typeof(string))
                return String;
            else if (type == typeof(DateTime))
                return DateTime;
            else if (type == Utility.RasterType)
                return Raster;
            else if (type == typeof(byte[]))
                return Blob;

            throw new Exception("Failed to find type for: " + type.FullName.ToString());
        }

        private static Type ConvertMgTypeToNetType(short idType)
        {
            switch (idType)
            {
                case Byte:
                    return typeof(byte);
                case Int16:
                    return typeof(short);
                case Int32:
                    return typeof(int);
                case Int64:
                    return typeof(long);
                case Single:
                    return typeof(float);
                case Double:
                    return typeof(double);
                case Boolean:
                    return typeof(bool);
                case Geometry:
                    return Utility.GeometryType;
                case String:
                    return typeof(string);
                case DateTime:
                    return typeof(DateTime);
                case Raster:
                    return Utility.RasterType;
                case Blob:
                    return typeof(byte[]);
                case Clob:
                    return typeof(byte[]);
                default:
                    throw new Exception("Failed to find type for: " + idType.ToString());
            }
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
                    this.Parent.OnLayerParentChanged(this, this.ObjectId);
                    break;
                case "Visible":
                    this.Parent.OnLayerVisibilityChanged(this, this.Visible ? "1" : "0");
                    break;
                case "ShowInLegend":
                    this.Parent.OnLayerDisplayInLegendChanged(this, this.ShowInLegend ? "1" : "0");
                    break;
                case "LegendLabel":
                    this.Parent.OnLayerLegendLabelChanged(this, this.LegendLabel);
                    break;
                case "LayerDefinitionID":
                    this.Parent.OnLayerDefinitionChanged(this);
                    break;
                case "Selectable":
                    this.Parent.OnLayerSelectabilityChanged(this, this.Selectable ? "1" : "0");
                    break;
            }

            base.OnPropertyChanged(propertyName);
        }
    }
}
