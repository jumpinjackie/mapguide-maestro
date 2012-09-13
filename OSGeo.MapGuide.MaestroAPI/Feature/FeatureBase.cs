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
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Base implementation of the <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeature"/>
    /// interface
    /// </summary>
    public class FeatureBase : RecordBase, IFeature
    {
        private ClassDefinition _clsDef;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureBase"/> class.
        /// </summary>
        /// <param name="clsDef">The class definition.</param>
        public FeatureBase(ClassDefinition clsDef) : base()
        {
            _clsDef = clsDef;
            
            for (int i = 0; i < clsDef.Properties.Count; i++)
            {
                var prop = clsDef[i];
                _ordinalMap[i] = prop.Name;
                switch (prop.Type)
                {
                    case PropertyDefinitionType.Data:
                        {
                            DataPropertyDefinition dp = (DataPropertyDefinition)prop;
                            switch (dp.DataType)
                            {
                                case DataPropertyType.Blob:
                                    _values[prop.Name] = new BlobValue();
                                    break;
                                case DataPropertyType.Boolean:
                                    _values[prop.Name] = new BooleanValue();
                                    break;
                                case DataPropertyType.Byte:
                                    _values[prop.Name] = new ByteValue();
                                    break;
                                case DataPropertyType.Clob:
                                    _values[prop.Name] = new ClobValue();
                                    break;
                                case DataPropertyType.DateTime:
                                    _values[prop.Name] = new DateTimeValue();
                                    break;
                                case DataPropertyType.Double:
                                    _values[prop.Name] = new DoubleValue();
                                    break;
                                case DataPropertyType.Int16:
                                    _values[prop.Name] = new Int16Value();
                                    break;
                                case DataPropertyType.Int32:
                                    _values[prop.Name] = new Int32Value();
                                    break;
                                case DataPropertyType.Int64:
                                    _values[prop.Name] = new Int64Value();
                                    break;
                                case DataPropertyType.Single:
                                    _values[prop.Name] = new SingleValue();
                                    break;
                                case DataPropertyType.String:
                                    _values[prop.Name] = new StringValue();
                                    break;
                            }
                        }
                        break;
                    case PropertyDefinitionType.Geometry:
                        _values[prop.Name] = new GeometryValue();
                        break;
                    case PropertyDefinitionType.Object:
                        _values[prop.Name] = new FeatureValue();
                        break;
                    case PropertyDefinitionType.Raster:
                        _values[prop.Name] = new RasterValue();
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the class definition of the object currently being read. If the user has requested
        /// only a subset of the class properties (as specified in the filter text), the class
        /// definition reflects what the user has requested, rather than the full class definition.
        /// </summary>
        public ClassDefinition ClassDefinition
        {
            get { return _clsDef; }
        }
    }

    /// <summary>
    /// A subclass of <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.FeatureReaderBase"/> that
    /// iterates over an array of <see cref="T:OSGeo.MapGuide.MaestroAPI.Feature.IFeature"/> instances
    /// </summary>
    public class FeatureArrayReader : FeatureReaderBase
    {
        private IFeature[] _features;

        private int _pos;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureArrayReader"/> class.
        /// </summary>
        /// <param name="features">The features.</param>
        public FeatureArrayReader(IFeature[] features)
        {
            _features = features;
            //All class definitions are uniform, so get the first one
            if (_features.Length > 0)
                this.ClassDefinition = _features[0].ClassDefinition;
            _pos = -1;
        }

        /// <summary>
        /// Gets the property value type for the property at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override PropertyValueType GetPropertyType(int index)
        {
            var prop = this.ClassDefinition.Properties[index];
            if (prop.Type == PropertyDefinitionType.Data)
                return (PropertyValueType)((DataPropertyDefinition)prop).DataType;
            else if (prop.Type == PropertyDefinitionType.Geometry)
                return PropertyValueType.Geometry;
            else if (prop.Type == PropertyDefinitionType.Raster)
                return PropertyValueType.Raster;
            else
                throw new ArgumentException();
        }

        /// <summary>
        /// Gets the property value type for the property of the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override PropertyValueType GetPropertyType(string name)
        {
            var prop = this.ClassDefinition.FindProperty(name);
            if (prop != null)
            {
                if (prop.Type == PropertyDefinitionType.Data)
                    return (PropertyValueType)((DataPropertyDefinition)prop).DataType;
                else if (prop.Type == PropertyDefinitionType.Geometry)
                    return PropertyValueType.Geometry;
                else if (prop.Type == PropertyDefinitionType.Raster)
                    return PropertyValueType.Raster;
                else
                    throw new ArgumentException();
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// Reads the next feature.
        /// </summary>
        /// <returns></returns>
        protected override IFeature ReadNextFeature()
        {
            _pos++;
            if (_pos < _features.Length)
                return _features[_pos];

            return null;
        }

        /// <summary>
        /// Closes the object, freeing any resources it may be holding.
        /// </summary>
        public override void Close()
        {
            
        }
    }
}
