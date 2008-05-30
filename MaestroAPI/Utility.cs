#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using System.Drawing;
using Topology.Geometries;

namespace OSGeo.MapGuide.MaestroAPI
{
	/// <summary>
	/// Various helper functions
	/// </summary>
	public class Utility
	{
		//Americans NEVER obey nationalization when outputting decimal values, so the rest of the world always have to work around their bugs :(
		private static System.Globalization.CultureInfo m_enCI = new System.Globalization.CultureInfo("en-US");

		/// <summary>
		/// Parses a color in HTML notation (ea. #ffaabbff)
		/// </summary>
		/// <param name="color">The HTML representation of the color</param>
		/// <returns>The .Net color structure that matches the color</returns>
		public static Color ParseHTMLColor(string color)
		{
			if (color.Length == 8)
			{
				int a = int.Parse(color.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
				int r = int.Parse(color.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
				int g = int.Parse(color.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
				int b = int.Parse(color.Substring(6,2), System.Globalization.NumberStyles.HexNumber);

				return Color.FromArgb(a, r,g,b);
			}
			else if (color.Length == 6)
			{
				int r = int.Parse(color.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
				int g = int.Parse(color.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
				int b = int.Parse(color.Substring(4,2), System.Globalization.NumberStyles.HexNumber);

				return Color.FromArgb(r,g,b);
			}
			else
				throw new Exception("Bad HTML color: \"" + color + "\"");
		}

		/// <summary>
		/// Returns the HTML representation of an .Net color structure
		/// </summary>
		/// <param name="color">The color to encode</param>
		/// <param name="includeAlpha">A flag indicating if the color structures alpha value should be included</param>
		/// <returns>The HTML representation of the color structure</returns>
		public static string SerializeHTMLColor(Color color, bool includeAlpha)
		{
			string res = "";
			if (includeAlpha)
				res += color.A.ToString("x02");
			res += color.R.ToString("x02");
			res += color.G.ToString("x02");
			res += color.B.ToString("x02");
			return res;
		}

		/// <summary>
		/// Parses a string with a decimal value in EN-US format
		/// </summary>
		/// <param name="digit">The string value</param>
		/// <returns>The parsed value</returns>
		public static float ParseDigit(string digit)
		{
			return (float)double.Parse(digit, m_enCI);
		}

		/// <summary>
		/// Turns a decimal value into a string representation in EN-US format
		/// </summary>
		/// <param name="digit">The value to encode</param>
		/// <returns>The encoded value</returns>
		public static string SerializeDigit(float digit)
		{
			return digit.ToString(m_enCI);
		}

		/// <summary>
		/// Turns a decimal value into a string representation in EN-US format
		/// </summary>
		/// <param name="digit">The value to encode</param>
		/// <returns>The encoded value</returns>
		public static string SerializeDigit(double digit)
		{
			return digit.ToString(m_enCI);
		}


		/// <summary>
		/// Copies the content of a stream into another stream
		/// </summary>
		/// <param name="source">The source stream</param>
		/// <param name="target">The target stream</param>
		public static void CopyStream(System.IO.Stream source, System.IO.Stream target)
		{
			int r;
			byte[] buf = new byte[1024];
			do
			{
				r  = source.Read(buf, 0, buf.Length);
				target.Write(buf, 0, r);
			} while (r > 0);
		}

        /// <summary>
        /// Builds a copy of the object by serializing it to xml, and then deserializing it.
        /// Please note that this function is has a large overhead.
        /// </summary>
        /// <param name="source">The object to copy</param>
        /// <returns>A copy of the object</returns>
        public static object XmlDeepCopy(object source)
        {
            if (source == null)
                return null;

            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(source.GetType());
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ser.Serialize(ms, source);
            ms.Position = 0;
            return ser.Deserialize(ms);
        }

		/// <summary>
		/// Makes a deep copy of an object, by copying all the public properties.
		/// This overload tries to maintain object references by assigning properties
		/// </summary>
		/// <param name="source">The object to copy</param>
		/// <param name="target">The object to assign to</param>
		/// <returns>A copied object</returns>
		public static object DeepCopy(object source, object target)
		{
			foreach(System.Reflection.PropertyInfo pi in source.GetType().GetProperties())
			{
				if (!pi.CanRead || !pi.CanWrite)
					continue;

				if (!pi.PropertyType.IsClass || pi.PropertyType == typeof(string) )
					pi.SetValue(target, pi.GetValue(source, null) , null);
				else if (pi.GetValue(source, null) == null)
					pi.SetValue(target, null, null);
				else if (pi.GetValue(source, null).GetType().GetInterface(typeof(System.Collections.ICollection).FullName) != null)
				{
					System.Collections.ICollection srcList = (System.Collections.ICollection)pi.GetValue(source, null);
					System.Collections.ICollection trgList = (System.Collections.ICollection)Activator.CreateInstance(srcList.GetType());
					foreach(object o in srcList)
						trgList.GetType().GetMethod("Add").Invoke(trgList, new object[] { DeepCopy(o) } );
					pi.SetValue(target, trgList, null);
				}
				else if (pi.GetValue(source, null).GetType().IsArray)
				{
					System.Array sourceArr = (System.Array)pi.GetValue(source, null);
					System.Array targetArr = (System.Array)Activator.CreateInstance(sourceArr.GetType(), new object[] { sourceArr.Length });
					for(int i = 0; i < targetArr.Length; i++)
						targetArr.SetValue(DeepCopy(sourceArr.GetValue(i)), i);
					pi.SetValue(target, targetArr, null);
				}
				else
				{
					if (pi.GetValue(target, null) == null)
						pi.SetValue(target, Activator.CreateInstance(pi.GetValue(source, null).GetType()), null);
					DeepCopy(pi.GetValue(source, null), pi.GetValue(target, null));
				}
			}

			return target;
		}



		/// <summary>
		/// Makes a deep copy of an object, by copying all the public properties
		/// </summary>
		/// <param name="source">The object to copy</param>
		/// <returns>A copied object</returns>
		public static object DeepCopy(object source)
		{
            if (source == null)
                return null;

			object target = Activator.CreateInstance(source.GetType());

			foreach(System.Reflection.PropertyInfo pi in source.GetType().GetProperties())
			{
				if (!pi.CanRead || !pi.CanWrite)
					continue;

				if (!pi.PropertyType.IsClass || pi.PropertyType == typeof(string) )
					pi.SetValue(target, pi.GetValue(source, null) , null);
				else if (pi.GetValue(source, null) == null)
					pi.SetValue(target, null, null);
				else if (pi.GetValue(source, null).GetType().GetInterface(typeof(System.Collections.ICollection).FullName) != null)
				{
					System.Collections.ICollection srcList = (System.Collections.ICollection)pi.GetValue(source, null);
					System.Collections.ICollection trgList = (System.Collections.ICollection)Activator.CreateInstance(srcList.GetType());
					foreach(object o in srcList)
						trgList.GetType().GetMethod("Add").Invoke(trgList, new object[] { DeepCopy(o) } );
					pi.SetValue(target, trgList, null);
				}
				else if (pi.GetValue(source, null).GetType().IsArray)
				{
					System.Array sourceArr = (System.Array)pi.GetValue(source, null);
					System.Array targetArr = (System.Array)Activator.CreateInstance(sourceArr.GetType(), new object[] { sourceArr.Length });
					for(int i = 0; i < targetArr.Length; i++)
						targetArr.SetValue(DeepCopy(sourceArr.GetValue(i)), i);
					pi.SetValue(target, targetArr, null);
				}
				else
					pi.SetValue(target, DeepCopy(pi.GetValue(source, null)), null);
			}

			return target;
		}

		public static System.IO.MemoryStream MgStreamToNetStream(object source, System.Reflection.MethodInfo mi, object[] args)
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

		public static byte[] StreamAsArray(System.IO.Stream s)
		{
			if (s as System.IO.MemoryStream != null)
				return ((System.IO.MemoryStream)s).ToArray();

			if (!s.CanSeek)
			{
				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				byte[] buf = new byte[1024];
				int c;
				while((c = s.Read(buf, 0, buf.Length)) > 0)
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
		/// Creates a copy of the stream, with removed Utf8 BOM, if any
		/// </summary>
		/// <param name="ms">The stream to fix</param>
		/// <returns>A stream with no Utf8 BOM</returns>
		public static System.IO.MemoryStream RemoveUTF8BOM(System.IO.MemoryStream ms)
		{
			//Skip UTF file header, since the XmlParser is broken
			ms.Position = 0;
			byte[] utfheader = new byte[3];
			if (ms.Read(utfheader, 0, utfheader.Length) == utfheader.Length)
				if (utfheader[0] == 0xEF && utfheader[1] == 0xBB && utfheader[2] == 0xBF)
				{
					ms.Position = 3;
					System.IO.MemoryStream mxs = new System.IO.MemoryStream();
					Utility.CopyStream(ms, mxs);
					mxs.Position = 0;
					return mxs;
				}

			ms.Position = 0;
			return ms;
		}

		/// <summary>
		/// Gets the type of an item, givne the MapGuide type id
		/// </summary>
		/// <param name="MgType">The MapGuide type id</param>
		/// <returns>The corresponding .Net type</returns>
		public static Type ConvertMgTypeToNetType(int MgType)
		{
			switch(MgType)
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

        public static Type RasterType
        {
            get { return typeof(System.Drawing.Bitmap); }
        }

        public static Type GeometryType
        {
            get { return typeof(Topology.Geometries.IGeometry); }
        }

        public static Exception ThrowAsWebException(Exception ex)
        {
            if (ex as System.Net.WebException != null)
            {
                try
                {
                    System.Net.WebException wex = ex as System.Net.WebException;
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(wex.Response.GetResponseStream()))
                    {
                        string html = sr.ReadToEnd();
                        System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("(\\<body\\>)(.+)\\<\\/body\\>", System.Text.RegularExpressions.RegexOptions.Singleline);
                        System.Text.RegularExpressions.Match m = r.Match(html);
                        if (m.Success && m.Groups.Count == 3)
                        {
                            html = m.Groups[2].Value;
                            int n = html.IndexOf("</h2>");
                            if (n > 0)
                                html = html.Substring(n + "</h2>".Length);
                        }

                        return new Exception(wex.Message + ": " + html, wex);
                    }
                }
                catch
                {
                }
            }

            return ex;
        }
	}
}
