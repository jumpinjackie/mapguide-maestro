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
using GeoAPI.Geometries;

namespace OSGeo.MapGuide.MaestroAPI.Feature
{
    /// <summary>
    /// Helper class to map MapGuide/FDO data/property types to CLR data types
    /// </summary>
    public static class ClrFdoTypeMap
    {
        /// <summary>
        /// Gets the CLR type for the specified data type
        /// </summary>
        /// <param name="dataPropertyType"></param>
        /// <returns></returns>
        public static Type GetClrType(DataPropertyType dataPropertyType)
        {
            switch (dataPropertyType)
            {
                case DataPropertyType.Blob:
                    return typeof(byte[]);
                case DataPropertyType.Boolean:
                    return typeof(bool);
                case DataPropertyType.Byte:
                    return typeof(byte);
                case DataPropertyType.Clob:
                    return typeof(char[]);
                case DataPropertyType.DateTime:
                    return typeof(DateTime);
                case DataPropertyType.Double:
                    return typeof(double);
                case DataPropertyType.Int16:
                    return typeof(short);
                case DataPropertyType.Int32:
                    return typeof(int);
                case DataPropertyType.Int64:
                    return typeof(long);
                case DataPropertyType.Single:
                    return typeof(float);
                case DataPropertyType.String:
                    return typeof(string);
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// Gets the CLR type for the specified property value type
        /// </summary>
        /// <param name="propertyValueType"></param>
        /// <returns></returns>
        public static Type GetClrType(PropertyValueType propertyValueType)
        {
            switch (propertyValueType)
            {
                case PropertyValueType.Blob:
                    return typeof(byte[]);
                case PropertyValueType.Boolean:
                    return typeof(bool);
                case PropertyValueType.Byte:
                    return typeof(byte);
                case PropertyValueType.Clob:
                    return typeof(char[]);
                case PropertyValueType.DateTime:
                    return typeof(DateTime);
                case PropertyValueType.Double:
                    return typeof(double);
                case PropertyValueType.Feature:
                    return typeof(IFeature[]);
                case PropertyValueType.Geometry:
                    return typeof(IGeometry);
                case PropertyValueType.Int16:
                    return typeof(short);
                case PropertyValueType.Int32:
                    return typeof(int);
                case PropertyValueType.Int64:
                    return typeof(long);
                //case PropertyValueType.Raster:
                case PropertyValueType.Single:
                    return typeof(float);
                case PropertyValueType.String:
                    return typeof(string);
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// Gets the clr type for the specified property definition
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static Type GetClrType(PropertyDefinition prop)
        {
            switch (prop.Type)
            {
                case PropertyDefinitionType.Data:
                    {
                        DataPropertyDefinition dp = (DataPropertyDefinition)prop;
                        return GetClrType(dp.DataType);
                    };
                case PropertyDefinitionType.Geometry:
                    return typeof(IGeometry);
                case PropertyDefinitionType.Object:
                    return typeof(IFeature[]);
                //case PropertyDefinitionType.Raster:
            }
            throw new ArgumentException();
        }
    }
}
