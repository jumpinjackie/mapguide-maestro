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
        /// Converts a MgStream to a .Net Stream object.
        /// Due to some swig issues, it is not possible to pass on an MgStream to a function,
        /// so this function calls a method to retrieve the stream locally.
        /// </summary>
        /// <param name="source">The object which has a stream</param>
        /// <param name="mi">The method to call</param>
        /// <param name="args">Arguments to the method</param>
        /// <returns>A read-only <see cref="System.IO.Stream"/> that wraps the underlying <see cref="OSGeo.MapGuide.MgByteReader"/></returns>
        public static System.IO.Stream MgStreamToNetStream(object source, System.Reflection.MethodInfo mi, object[] args)
        {
            try
            {
                //How can we work around invalidating MgByteReader when it's passed as a parameter?
                //Cheat the system by deferring execution of MgByteReader reference assignment until
                //we're in the ctor of MgReadOnlyStream. Only MgReadOnlyStream has access to the MgByteReader
                //Everything else interacts through the .net Stream interface. Win-win.
                GetByteReaderMethod method = () => { return (OSGeo.MapGuide.MgByteReader)mi.Invoke(source, args); };
                return new MgReadOnlyStream(method);
            }
            catch (System.Reflection.TargetInvocationException tex)
            {
                if (tex.InnerException != null)
                    throw tex.InnerException;
                else
                    throw;
            }
            /* 
            try
            {
                OSGeo.MapGuide.MgByteReader rd = (OSGeo.MapGuide.MgByteReader)mi.Invoke(source, args);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                byte[] buf = new byte[1024];
                int c = 0;
                do
                {
                    c = rd.Read(buf, buf.Length);
                    ms.Write(buf, 0, c);
                } while (c != 0);
                ms.Position = 0;
                return ms;
            }
            catch (System.Reflection.TargetInvocationException tex)
            {
                if (tex.InnerException != null)
                    throw tex.InnerException;
                else
                    throw;
            }*/
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
            get { return typeof(Topology.Geometries.IGeometry); }
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
            foreach (var prop in mgClass.GetProperties())
            {
                if (prop.PropertyType == MgFeaturePropertyType.DataProperty)
                {
                    MgDataPropertyDefinition mgData = (MgDataPropertyDefinition)prop;
                    var dp = ConvertDataProperty(mgData);

                    bool identity = (mgClass.GetIdentityProperties().Contains(mgData));
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
            throw new NotImplementedException();
        }

        private static GeometricPropertyDefinition ConvertGeometricProperty(MgGeometricPropertyDefinition mgGeom)
        {
            throw new NotImplementedException();
        }

        private static DataPropertyDefinition ConvertDataProperty(MgDataPropertyDefinition mgDataPropertyDefinition)
        {
            throw new NotImplementedException();
        }

        internal static DateTime ConvertMgDateTime(MgDateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Microsecond * 1000);
        }
    }
}
