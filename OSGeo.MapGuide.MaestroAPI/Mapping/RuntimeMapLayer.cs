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
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.MaestroAPI.Feature;
using System.Diagnostics;
using OSGeo.MapGuide.MaestroAPI.Schema;

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
    /// Extension methods for <see cref="T:OSGeo.MapGuide.MaestroAPI.Mapping.RuntimeMapLayer"/>
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the parent group
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static RuntimeMapGroup GetParentGroup(this RuntimeMapLayer layer)
        {
            Check.NotNull(layer, "layer"); //NOXLATE

            if (string.IsNullOrEmpty(layer.Group))
                return null;

            return layer.Parent.Groups[layer.Group];
        }
    }

    /// <summary>
    /// Represents a runtime map layer. Use <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IMappingService.CreateMapLayer(OSGeo.MapGuide.MaestroAPI.Mapping.RuntimeMap,OSGeo.MapGuide.ObjectModels.LayerDefinition.ILayerDefinition)"/> to create
    /// instances of this class.
    /// </summary>
    public class RuntimeMapLayer : MapObservable
    {
        /// <summary>
        /// Represents a scale range
        /// </summary>
        public class ScaleRange
        {
            internal ScaleRange(double minVal, double maxVal)
            {
                this.MinScale = minVal;
                this.MaxScale = maxVal;
            }

            /// <summary>
            /// Gets the min scale.
            /// </summary>
            public double MinScale { get; private set; }

            /// <summary>
            /// Gets the max scale.
            /// </summary>
            public double MaxScale { get; private set; }
        }

        //From MgLayerType
        internal const int kBaseMap = 2;
        internal const int kDynamic = 1;

        const double InfinityScale = double.MaxValue;

        /// <summary>
        /// Gets the <see cref="T:OSGeo.MapGuide.MaestroAPI.Mapping.RuntimeMap"/> that this layer belongs to
        /// </summary>
        public RuntimeMap Parent { get; private set; }

        /// <summary>
        /// Initializes this instances from the specified Layer Definition
        /// </summary>
        /// <param name="ldf"></param>
        /// <param name="suppressErrors"></param>
        protected void Initialize(ILayerDefinition ldf, bool suppressErrors)
        {
            Check.NotNull(ldf, "ldf"); //NOXLATE
            this.LayerDefinitionID = ldf.ResourceID;
            if (ldf.SubLayer.LayerType == LayerType.Vector)
            {
                var vl = ((IVectorLayerDefinition)ldf.SubLayer);
                _qualifiedClassName = vl.FeatureName;
                _geometryPropertyName = vl.Geometry;
                _featureSourceId = vl.ResourceId;
                _filter = vl.Filter;
                InitIdentityProperties(vl, suppressErrors);
                InitScaleRanges(vl);
                _hasTooltips = !string.IsNullOrEmpty(vl.ToolTip);
            }
            else if (ldf.SubLayer.LayerType == LayerType.Raster)
            {
                var rl = ((IRasterLayerDefinition)ldf.SubLayer);
                _qualifiedClassName = rl.FeatureName;
                _geometryPropertyName = rl.Geometry;
                _featureSourceId = rl.ResourceId;
                InitScaleRanges(rl);
            }
            else if (ldf.SubLayer.LayerType == LayerType.Drawing)
            {
                _featureSourceId = ldf.SubLayer.ResourceId;
                var dl = ((IDrawingLayerDefinition)ldf.SubLayer);
                _scaleRanges = new double[] 
                {
                    dl.MinScale,
                    dl.MaxScale 
                };
                EnsureOrderedMinMaxScales();
            }

            _expandInLegend = false;
            this.Name = ResourceIdentifier.GetName(ldf.ResourceID);
            _legendLabel = this.Name;
            _selectable = true;
            _showInLegend = true;
            _visible = true;
        }

        /// <summary>
        /// Initializes this instance
        /// </summary>
        /// <param name="parent"></param>
        protected internal RuntimeMapLayer(RuntimeMap parent) 
        {
            _scaleRanges = new double[] { 0.0, InfinityScale };
            _type = kDynamic;
            this.IdentityProperties = new PropertyInfo[0];
            _objectId = Guid.NewGuid().ToString();
            this.Parent = parent;
            _group = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeMapLayer"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="ldf">The Layer Definition.</param>
        /// <param name="suppressErrors">If true, any errors while creating the layer are suppressed. The nature of the error may result in un-selectable layers</param>
        protected internal RuntimeMapLayer(RuntimeMap parent, ILayerDefinition ldf, bool suppressErrors)
            : this(parent)
        {
            _disableChangeTracking = true;
            Initialize(ldf, suppressErrors);
            _disableChangeTracking = false;
        }

        private void InitScaleRanges(IRasterLayerDefinition rl)
        {
            List<double> scales = new List<double>();
            foreach (var gsr in rl.GridScaleRange)
            {
                if (gsr.MinScale.HasValue)
                    scales.Add(gsr.MinScale.Value);
                else
                    scales.Add(0.0);

                if (gsr.MaxScale.HasValue)
                    scales.Add(gsr.MaxScale.Value);
                else
                    scales.Add(InfinityScale);
            }
            _scaleRanges = scales.ToArray();
            EnsureOrderedMinMaxScales();
        }

        private void InitScaleRanges(IVectorLayerDefinition vl)
        {
            List<double> scales = new List<double>();
            foreach (var vsr in vl.VectorScaleRange)
            {
                if (vsr.MinScale.HasValue)
                    scales.Add(vsr.MinScale.Value);
                else
                    scales.Add(0.0);

                if (vsr.MaxScale.HasValue)
                    scales.Add(vsr.MaxScale.Value);
                else
                    scales.Add(InfinityScale);
            }
            _scaleRanges = scales.ToArray();
            EnsureOrderedMinMaxScales();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeMapLayer"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="source">The source.</param>
        /// <param name="suppressErrors"></param>
        protected internal RuntimeMapLayer(RuntimeMap parent, IMapLayer source, bool suppressErrors)
            : this(parent, source, (ILayerDefinition)parent.CurrentConnection.ResourceService.GetResource(source.ResourceId), suppressErrors)
        {
            _disableChangeTracking = false;
        }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="source"></param>
        /// <param name="ldf"></param>
        /// <param name="suppressErrors"></param>
        protected internal RuntimeMapLayer(RuntimeMap parent, IMapLayer source, ILayerDefinition ldf, bool suppressErrors)
            : this(parent, (IBaseMapLayer)source, ldf, suppressErrors)
        {
            _disableChangeTracking = true;

            this.Group = source.Group;
            _visible = source.Visible;

            _disableChangeTracking = false;
        }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="source"></param>
        /// <param name="ldf"></param>
        /// <param name="suppressErrors"></param>
        protected internal RuntimeMapLayer(RuntimeMap parent, IBaseMapLayer source, ILayerDefinition ldf, bool suppressErrors) 
            : this(parent, ldf, suppressErrors)
        {
            Check.NotNull(source, "source"); //NOXLATE
            Check.NotNull(ldf, "ldf"); //NOXLATE
            Check.Precondition(source.ResourceId == ldf.ResourceID, "source.ResourceId == ldf.ResourceID"); //NOXLATE
        }

        private void EnsureOrderedMinMaxScales()
        {
            Debug.Assert(_scaleRanges.Length % 2 == 0);
            int scaleCount = _scaleRanges.Length / 2;
            for (int i = 0; i < scaleCount; i++)
            {
                int minPos = i * 2;
                int maxPos = i * 2 + 1;
                if (_scaleRanges[minPos] > _scaleRanges[maxPos])
                {
                    double temp = _scaleRanges[minPos];
                    _scaleRanges[minPos] = _scaleRanges[maxPos];
                    _scaleRanges[maxPos] = temp;
                }
            }
            List<ScaleRange> ranges = new List<ScaleRange>();
            for (int i = 0; i < scaleCount; i++)
            {
                ranges.Add(new ScaleRange(_scaleRanges[i * 2], _scaleRanges[i * 2 + 1]));
            }
            this.ScaleRanges = ranges.ToArray();
        }

        /// <summary>
        /// Gets the applicable scale ranges for this layer
        /// </summary>
        public ScaleRange[] ScaleRanges { get; private set; }

        private void InitIdentityProperties(IVectorLayerDefinition vl, bool suppressErrors)
        {
            try
            {
                var fs = (IFeatureSource)this.Parent.ResourceService.GetResource(vl.ResourceId);
                var cls = fs.GetClass(vl.FeatureName);
                if (cls == null)
                    throw new Exception(string.Format(Strings.ERR_CLASS_NOT_FOUND, vl.FeatureName));

                var idProps = cls.IdentityProperties;
                var propInfo = new PropertyInfo[idProps.Count];

                int i = 0;
                foreach (var prop in idProps)
                {
                    propInfo[i] = new PropertyInfo(prop.Name, ClrFdoTypeMap.GetClrType(prop.DataType));
                    i++;
                }

                this.IdentityProperties = propInfo;
            }
            catch (Exception ex) //Has to be a bug in MapGuide or in the FDO provider
            {
                //If not suppressing, rethrow with original stack trace
                if (!suppressErrors)
                    throw;

                this.IdentityProperties = new PropertyInfo[0];
                Trace.TraceWarning(string.Format(Strings.ERR_INIT_IDENTITY_PROPS, Environment.NewLine, this.Name, ex.ToString()));
            }
        }

        private bool _visible;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RuntimeMapLayer"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public virtual bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (this.Type == kBaseMap)
                    throw new InvalidOperationException(Strings.ErrorSettingVisibilityOfTiledLayer);

                if (SetField(ref _visible, value, "Visible")) //NOXLATE
                    Parent.IsDirty = true;
            }
        }

        private string _group;

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
        public virtual string Group
        {
            get
            {
                return _group;
            }
            set
            {
                if (SetField(ref _group, value, "Group")) //NOXLATE
                    Parent.IsDirty = true;
            }
        }

        /// <summary>
        /// Gets the layer definition ID.
        /// </summary>
        /// <value>The layer definition ID.</value>
        public virtual string LayerDefinitionID
        {
            get;
            internal set;
        }

        private bool _selectable;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RuntimeMapLayer"/> is selectable.
        /// </summary>
        /// <value><c>true</c> if selectable; otherwise, <c>false</c>.</value>
        public virtual bool Selectable
        {
            get
            {
                return _selectable;
            }
            set
            {
                if (SetField(ref _selectable, value, "Selectable")) //NOXLATE
                    Parent.IsDirty = true;
            }
        }

        private string _name;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (SetField(ref _name, value, "Name")) //NOXLATE
                    Parent.IsDirty = true;
            }
        }

        private bool _showInLegend;

        /// <summary>
        /// Gets or sets a value indicating whether [show in legend].
        /// </summary>
        /// <value><c>true</c> if [show in legend]; otherwise, <c>false</c>.</value>
        public virtual bool ShowInLegend
        {
            get
            {
                return _showInLegend;
            }
            set
            {
                if (SetField(ref _showInLegend, value, "ShowInLegend")) //NOXLATE
                    Parent.IsDirty = true;
            }
        }

        private string _legendLabel;

        /// <summary>
        /// Gets or sets the legend label.
        /// </summary>
        /// <value>The legend label.</value>
        public virtual string LegendLabel
        {
            get
            {
                return _legendLabel;
            }
            set
            {
                if (SetField(ref _legendLabel, value, "LegendLabel")) //NOXLATE
                    Parent.IsDirty = true;
            }
        }

        private bool _expandInLegend;

        /// <summary>
        /// Gets or sets a value indicating whether [expand in legend].
        /// </summary>
        /// <value><c>true</c> if [expand in legend]; otherwise, <c>false</c>.</value>
        public virtual bool ExpandInLegend
        {
            get
            {
                return _expandInLegend;
            }
            set
            {
                if (SetField(ref _expandInLegend, value, "ExpandInLegend")) //NOXLATE
                    Parent.IsDirty = true;
            }
        }

        private string _featureSourceId;

        /// <summary>
        /// Gets or sets the feature source ID.
        /// </summary>
        /// <value>The feature source ID.</value>
        public virtual string FeatureSourceID
        {
            get { return _featureSourceId; }
            internal set { _featureSourceId = value; }
        }

        private string _qualifiedClassName;

        /// <summary>
        /// Gets the name of the qualified name of the feature class.
        /// </summary>
        /// <value>The name of the qualified name of the feature class.</value>
        public virtual string QualifiedClassName
        {
            get { return _qualifiedClassName; }
            internal set { _qualifiedClassName = value; }
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

        private string _objectId;

        /// <summary>
        /// Gets the object id.
        /// </summary>
        /// <value>The object id.</value>
        public virtual string ObjectId
        {
            get { return _objectId; }
            internal set { _objectId = value; }
        }

        private string _geometryPropertyName;

        /// <summary>
        /// Gets the name of the geometry property.
        /// </summary>
        /// <value>The name of the geometry property.</value>
        public virtual string GeometryPropertyName
        {
            get { return _geometryPropertyName; }
            private set { _geometryPropertyName = value; }
        }

        private string _filter;

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public virtual string Filter
        {
            get { return _filter; }
            internal set { _filter = value; }
        }

        private int _type;

        /// <summary>
        /// Gets the type
        /// </summary>
        public virtual int Type
        {
            get { return _type; }
            internal set { _type = value; }
        }

        private double _displayOrder;

        /// <summary>
        /// Gets the display order
        /// </summary>
        public virtual double DisplayOrder
        {
            get { return _displayOrder; }
            internal set { _displayOrder = value; }
        }

        private bool _needsRefresh;

        /// <summary>
        /// Gets whether this layer needs to be refreshed
        /// </summary>
        public virtual bool NeedsRefresh
        {
            get { return _needsRefresh; }
            internal set { _needsRefresh = value; }
        }

        /// <summary>
        /// Sets the refresh flag for this layer
        /// </summary>
        public virtual void ForceRefresh()
        {
            if (!this.IsVisibleAtScale(this.Parent.ViewScale))
                return;

            this.NeedsRefresh = true;
        }

        private bool _hasTooltips;

        /// <summary>
        /// Gets whether this layer has tooltips
        /// </summary>
        public virtual bool HasTooltips
        {
            get { return _hasTooltips; }
            internal set { _hasTooltips = value; }
        }

        /// <summary>
        /// Gets the schema name
        /// </summary>
        /// <remarks>
        /// For drawing layers, the schema name will always be empty
        /// </remarks>
        public string SchemaName
        {
            get
            {
                if (!this.FeatureSourceID.EndsWith("DrawingSource")) //NOXLATE
                {
                    var tokens = this.QualifiedClassName.Split(':'); //NOXLATE
                    if (tokens.Length == 2)
                        return tokens[0];
                }
                return string.Empty;
            }
        }

        private double[] _scaleRanges;

        /// <summary>
        /// Serializes this instance to a binary stream
        /// </summary>
        /// <param name="s"></param>
        public virtual void Serialize(MgBinarySerializer s)
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
        /// Parses encoded id string into an array of values
        /// </summary>
        /// <param name="encodedId">The encoded id string.</param>
        /// <returns></returns>
        public object[] ParseSelectionValues(string encodedId)
        {
            int index = 0;
            byte[] data = Convert.FromBase64String(encodedId);
            object[] tmp = new object[this.IdentityProperties.Length];
            for (int i = 0; i < this.IdentityProperties.Length; i++)
            {
                Type type = this.IdentityProperties[i].Type;

                if (type == typeof(short))
                {
                    tmp[i] = BitConverter.ToInt16(data, index);
                    index += MgBinarySerializer.UInt16Len;
                }
                else if (type == typeof(int))
                {
                    tmp[i] = BitConverter.ToInt32(data, index);
                    index += MgBinarySerializer.UInt32Len;
                }
                else if (type == typeof(long))
                {
                    tmp[i] = BitConverter.ToInt64(data, index);
                    index += MgBinarySerializer.UInt64Len;
                }
                else if (type == typeof(double))
                {
                    tmp[i] = BitConverter.ToDouble(data, index);
                    index += MgBinarySerializer.DoubleLen;
                }
                else if (type == typeof(string))
                {
                    int pos = index;
                    while (pos < data.Length && data[pos] != 0)
                        pos++;

                    if (pos >= data.Length)
                        throw new Exception(Strings.ErrorBadNullEncodedString);

                    tmp[i] = System.Text.Encoding.UTF8.GetString(data, index, pos - index);
                    index = pos + 1;
                }
                else
                    throw new Exception(string.Format(Strings.ErrorUnsupportedPkType, type.ToString()));
            }

            return tmp;
        }

        /// <summary>
        /// Initializes this instance with the specified binary stream
        /// </summary>
        /// <param name="d"></param>
        public virtual void Deserialize(MgBinaryDeserializer d)
        {
            this.Group = d.ReadString();

            int classid = d.ReadClassId();
            if (d.SiteVersion <= SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classid != 19003)
                throw new Exception(string.Format(Strings.ErrorResourceIdentifierClassIdNotFound, classid));
            if (d.SiteVersion > SiteVersions.GetVersion(KnownSiteVersions.MapGuideEP1_1) && classid != 30501)
                throw new Exception(string.Format(Strings.ErrorResourceIdentifierClassIdNotFound, classid));

            this.LayerDefinitionID = d.ReadResourceIdentifier();

            if (d.SiteVersion < SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS1_2))
            {
                this.Name = d.ReadString();
                _objectId = d.ReadString();
                _type = d.ReadInt32();

                _visible = d.ReadByte() > 0;
                _selectable = d.ReadByte() > 0;
                _showInLegend = d.ReadByte() > 0;
                _expandInLegend = d.ReadByte() > 0;

                _legendLabel = d.ReadString();
                _needsRefresh = d.ReadByte() > 0;
                _displayOrder = d.ReadDouble();

                var scaleRanges = new List<double>();
                int scales = d.ReadInt32();
                while (scales-- > 0)
                    scaleRanges.Add(d.ReadDouble());

                _scaleRanges = scaleRanges.ToArray();
                
                _featureSourceId = d.ReadString();
                _qualifiedClassName = d.ReadString();
                _geometryPropertyName = d.ReadString();

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
                _objectId = d.ReadInternalString();
                _type = BitConverter.ToInt32(d.ReadStreamRepeat(4), 0);

                int flags = d.ReadStreamRepeat(1)[0];
                _visible = (flags & 1) > 0;
                _selectable = (flags & 2) > 0;
                _showInLegend = (flags & 4) > 0;
                _expandInLegend = (flags & 8) > 0;
                _needsRefresh = (flags & 16) > 0;
                _hasTooltips = (flags & 32) > 0;

                _legendLabel = d.ReadInternalString();
                _displayOrder = BitConverter.ToDouble(d.ReadStreamRepeat(8), 0);

                var scaleRanges = new List<double>();
                int scales = BitConverter.ToInt32(d.ReadStreamRepeat(4), 0);
                while (scales-- > 0)
                    scaleRanges.Add(BitConverter.ToDouble(d.ReadStreamRepeat(8), 0));

                _scaleRanges = scaleRanges.ToArray();
                
                _featureSourceId = d.ReadInternalString();
                _qualifiedClassName = d.ReadInternalString();
                if (d.SiteVersion > SiteVersions.GetVersion(KnownSiteVersions.MapGuideOS2_1))
                    _filter = d.ReadInternalString();
                //this.SchemaName = d.ReadInternalString();
                d.ReadInternalString();
                _geometryPropertyName = d.ReadInternalString();

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

            EnsureOrderedMinMaxScales();
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
                    throw new Exception(string.Format(Strings.ErrorFailedToFindTypeForClrType, idType));
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.ComponentModel.INotfiyPropertyChanged.PropertyChanged"/> event
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
                        this.Parent.OnLayerParentChanged(this, this.Parent.Groups[name].ObjectId);
                    else
                        this.Parent.OnLayerParentChanged(this, string.Empty);
                    break;
                case "Visible": //NOXLATE
                    this.Parent.OnLayerVisibilityChanged(this, this.Visible ? "1" : "0"); //NOXLATE
                    break;
                case "ShowInLegend": //NOXLATE
                    this.Parent.OnLayerDisplayInLegendChanged(this, this.ShowInLegend ? "1" : "0"); //NOXLATE
                    break;
                case "LegendLabel": //NOXLATE
                    this.Parent.OnLayerLegendLabelChanged(this, this.LegendLabel);
                    break;
                case "LayerDefinitionID": //NOXLATE
                    this.Parent.OnLayerDefinitionChanged(this);
                    break;
                case "Selectable": //NOXLATE
                    this.Parent.OnLayerSelectabilityChanged(this, this.Selectable ? "1" : "0"); //NOXLATE
                    break;
            }

            base.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Determines whether this layer is potentially visible at the specified scale
        /// </summary>
        /// <remarks>
        /// Current layer visibility does not factor into the final result
        /// </remarks>
        /// <param name="scale">The scale to check for potential visibility</param>
        /// <returns></returns>
        public bool IsVisibleAtScale(double scale)
        {
            for (int i = 0; i < _scaleRanges.Length; i += 2) 
            {
                if (scale >= _scaleRanges[i] && scale <= _scaleRanges[i + 1])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Convenience method to get the associated class definition
        /// </summary>
        /// <returns></returns>
        public ClassDefinition GetClassDefinition()
        {
            var tokens = this.QualifiedClassName.Split(':');
            return this.Parent.FeatureService.GetClassDefinition(tokens[0], tokens[1]);
        }

        /// <summary>
        /// Gets a display string for this layer for presentation purposes
        /// </summary>
        public string DisplayString { get { return this.LegendLabel + " (" + this.Name + ")"; } } //NOXLATE
    }
}
