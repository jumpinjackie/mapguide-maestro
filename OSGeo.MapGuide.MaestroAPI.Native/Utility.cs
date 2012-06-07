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
using System.Drawing;
using OSGeo.MapGuide.MaestroAPI.Schema;
using GeoAPI.Geometries;

namespace OSGeo.MapGuide.MaestroAPI.Native
{
    public static class Utility
    {
        /// <summary>
        /// Reads all data from a stream, and returns it as a single array.
        /// Note that this is very inefficient if the stream is several megabytes long.
        /// </summary>
        /// <param name="s">The stream to exhaust</param>
        /// <returns>The streams content as an array</returns>
        public static byte[] StreamAsArray(System.IO.Stream s)
        {
            if (s as System.IO.MemoryStream != null)
                return ((System.IO.MemoryStream)s).ToArray();

            if (!s.CanSeek)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                byte[] buf = new byte[1024];
                int c;
                while ((c = s.Read(buf, 0, buf.Length)) > 0)
                    ms.Write(buf, 0, c);
                return ms.ToArray();
            }
            else
            {
                byte[] buf = new byte[s.Length];
                s.Position = 0;
                s.Read(buf, 0, buf.Length);
                return buf;
            }
        }

        /// <summary>
        /// Returns a type used to define a raster column in a feature reader
        /// </summary>
        public static Type RasterType
        {
            get { return typeof(System.Drawing.Bitmap); }
        }

        /// <summary>
        /// Returns the type used to define a geometry column in a feature reader
        /// </summary>
        public static Type GeometryType
        {
            get { return typeof(IGeometry); }
        }

        /// <summary>
        /// Gets the type of an item, given the MapGuide type id
        /// </summary>
        /// <param name="MgType">The MapGuide type id</param>
        /// <returns>The corresponding .Net type</returns>
        public static Type ConvertMgTypeToNetType(int MgType)
        {
            switch (MgType)
            {
                case OSGeo.MapGuide.MgPropertyType.Byte:
                    return typeof(byte);
                case OSGeo.MapGuide.MgPropertyType.Int16:
                    return typeof(short);
                case OSGeo.MapGuide.MgPropertyType.Int32:
                    return typeof(int);
                case OSGeo.MapGuide.MgPropertyType.Int64:
                    return typeof(long);
                case OSGeo.MapGuide.MgPropertyType.Single:
                    return typeof(float);
                case OSGeo.MapGuide.MgPropertyType.Double:
                    return typeof(double);
                case OSGeo.MapGuide.MgPropertyType.Boolean:
                    return typeof(bool);
                case OSGeo.MapGuide.MgPropertyType.Geometry:
                    return Utility.GeometryType;
                case OSGeo.MapGuide.MgPropertyType.String:
                    return typeof(string);
                case OSGeo.MapGuide.MgPropertyType.DateTime:
                    return typeof(DateTime);
                case OSGeo.MapGuide.MgPropertyType.Raster:
                    return Utility.RasterType;
                case OSGeo.MapGuide.MgPropertyType.Blob:
                    return typeof(byte[]);
                case OSGeo.MapGuide.MgPropertyType.Clob:
                    return typeof(byte[]);
                default:
                    throw new Exception("Failed to find type for: " + MgType.ToString());
            }
        }

        /// <summary>
        /// Parses a color in HTML notation (ea. #ffaabbff)
        /// </summary>
        /// <param name="color">The HTML representation of the color</param>
        /// <returns>The .Net color structure that matches the color</returns>
        public static Color ParseHTMLColor(string color)
        {
            if (color.Length == 8)
            {
                int a = int.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int r = int.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int g = int.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                int b = int.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

                return Color.FromArgb(a, r, g, b);
            }
            else if (color.Length == 6)
            {
                int r = int.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int g = int.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int b = int.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                return Color.FromArgb(r, g, b);
            }
            else
                throw new Exception("Bad HTML color: \"" + color + "\"");
        }

        /// <summary>
        /// Returns a type used to define an unknown column type in a feature reader
        /// </summary>
        public static Type UnmappedType
        {
            get { return typeof(UnmappedDataType); }
        }

        /// <summary>
        /// Gets the MapGuide id for a given type
        /// </summary>
        /// <param name="type">The .Net type</param>
        /// <returns>The corresponding MapGuide type id</returns>
        public static int ConvertNetTypeToMgType(Type type)
        {
            if (type == typeof(short))
                return OSGeo.MapGuide.MgPropertyType.Int16;
            else if (type == typeof(byte))
                return OSGeo.MapGuide.MgPropertyType.Byte;
            else if (type == typeof(bool))
                return OSGeo.MapGuide.MgPropertyType.Boolean;
            else if (type == typeof(int))
                return OSGeo.MapGuide.MgPropertyType.Int32;
            else if (type == typeof(long))
                return OSGeo.MapGuide.MgPropertyType.Int64;
            else if (type == typeof(float))
                return OSGeo.MapGuide.MgPropertyType.Single;
            else if (type == typeof(double))
                return OSGeo.MapGuide.MgPropertyType.Double;
            else if (type == Utility.GeometryType)
                return OSGeo.MapGuide.MgPropertyType.Geometry;
            else if (type == typeof(string))
                return OSGeo.MapGuide.MgPropertyType.String;
            else if (type == typeof(DateTime))
                return OSGeo.MapGuide.MgPropertyType.DateTime;
            else if (type == Utility.RasterType)
                return OSGeo.MapGuide.MgPropertyType.Raster;
            else if (type == typeof(byte[]))
                return OSGeo.MapGuide.MgPropertyType.Blob;

            throw new Exception("Failed to find type for: " + type.FullName.ToString());
        }

        public static ClassDefinition ConvertClassDefinition(MgClassDefinition mgClass)
        {
            var cls = new ClassDefinition(mgClass.Name, mgClass.Description);
            //foreach (var prop in mgClass.GetProperties())
            var props = mgClass.GetProperties();
            int pcount = props.GetCount();
            for (int i = 0; i < pcount; i++)
            {
                var prop = props.GetItem(i);
                if (prop.PropertyType == MgFeaturePropertyType.DataProperty)
                {
                    MgDataPropertyDefinition mgData = (MgDataPropertyDefinition)prop;
                    var dp = ConvertDataProperty(mgData);

                    //API Bug? passing object reference gives incorrect result for identity
                    //properties
                    bool identity = (mgClass.GetIdentityProperties().Contains(prop.Name));
                    cls.AddProperty(dp, identity);
                }
                else if (prop.PropertyType == MgFeaturePropertyType.GeometricProperty)
                {
                    MgGeometricPropertyDefinition mgGeom = (MgGeometricPropertyDefinition)prop;
                    var geom = ConvertGeometricProperty(mgGeom);

                    cls.AddProperty(geom);
                }
                else if (prop.PropertyType == MgFeaturePropertyType.RasterProperty)
                {
                    MgRasterPropertyDefinition mgRaster = (MgRasterPropertyDefinition)prop;
                    var raster = ConvertRasterProperty(mgRaster);

                    cls.AddProperty(raster);
                }
                else if (prop.PropertyType == MgFeaturePropertyType.ObjectProperty)
                {
                    
                }
                else if (prop.PropertyType == MgFeaturePropertyType.AssociationProperty)
                {
                }
            }

            cls.DefaultGeometryPropertyName = mgClass.DefaultGeometryPropertyName;

            return cls;
        }

        private static RasterPropertyDefinition ConvertRasterProperty(MgRasterPropertyDefinition mgRaster)
        {
            var rp = new RasterPropertyDefinition(mgRaster.Name, mgRaster.Description);
            rp.DefaultImageXSize = mgRaster.DefaultImageXSize;
            rp.DefaultImageYSize = mgRaster.DefaultImageYSize;
            rp.IsNullable = mgRaster.Nullable;
            rp.IsReadOnly = mgRaster.ReadOnly;
#if LOCAL_API
            rp.SpatialContextAssociation = mgRaster.SpatialContextAssociation;
#endif
            return rp;
        }

        private static GeometricPropertyDefinition ConvertGeometricProperty(MgGeometricPropertyDefinition mgGeom)
        {
            var gp = new GeometricPropertyDefinition(mgGeom.Name, mgGeom.Description);

            gp.GeometricTypes = (FeatureGeometricType)mgGeom.GeometryTypes;

            gp.HasElevation = mgGeom.HasElevation;
            gp.HasMeasure = mgGeom.HasMeasure;
            gp.IsReadOnly = mgGeom.ReadOnly;
            gp.SpatialContextAssociation = mgGeom.SpatialContextAssociation;

            return gp;
        }

        private static DataPropertyDefinition ConvertDataProperty(MgDataPropertyDefinition dataProp)
        {
            var dp = new DataPropertyDefinition(dataProp.Name, dataProp.Description);
            dp.DataType = (DataPropertyType)dataProp.DataType;
            dp.DefaultValue = dataProp.DefaultValue;
            dp.Description = dataProp.Description;
            dp.IsAutoGenerated = dataProp.IsAutoGenerated();
            dp.IsNullable = dataProp.Nullable;
            dp.IsReadOnly = dataProp.ReadOnly;
            dp.Length = dataProp.Length;
            dp.Precision = dataProp.Precision;
            dp.Scale = dataProp.Scale;

            return dp;
        }

        internal static DateTime ConvertMgDateTime(MgDateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Microsecond * 1000);
        }
    }
}
