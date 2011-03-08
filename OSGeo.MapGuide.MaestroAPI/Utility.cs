#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
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
using System.Collections.Generic;
using OSGeo.MapGuide.MaestroAPI.IO;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System.Collections.Specialized;

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
        /// Converts the specified name value collection into a connection string
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToConnectionString(NameValueCollection values)
        {
            List<string> tokens = new List<string>();

            foreach (string name in values.Keys)
            {
                string value = values[name];
                if (value.Contains(";"))
                    value = "\"" + value + "\"";
                tokens.Add(name + "=" + value);
            }

            return string.Join(";", tokens.ToArray());
        }

        /// <summary>
        /// Strips the version component from provider name
        /// </summary>
        /// <param name="providername">The provider name.</param>
        /// <returns></returns>
        public static string StripVersionFromProviderName(string providername)
        {
            double x;
            string[] parts = providername.Split('.');
            for (int i = parts.Length - 1; i >= 0; i--)
            {
                if (!double.TryParse(parts[i], System.Globalization.NumberStyles.Integer, null, out x))
                {
                    if (i != 0)
                        return string.Join(".", parts, 0, i + 1);
                    break;
                }
            }
            return providername;
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
        /// Copies the content of a stream into another stream.
        /// Automatically attempts to rewind the source stream.
        /// </summary>
        /// <param name="source">The source stream</param>
        /// <param name="target">The target stream</param>
        public static void CopyStream(System.IO.Stream source, System.IO.Stream target)
        {
            CopyStream(source, target, true);
        }

		/// <summary>
		/// Copies the content of a stream into another stream.
		/// </summary>
		/// <param name="source">The source stream</param>
		/// <param name="target">The target stream</param>
        /// <param name="rewind">True if the source stream should be rewound before being copied</param>
		public static void CopyStream(System.IO.Stream source, System.IO.Stream target, bool rewind)
		{
			int r;
			byte[] buf = new byte[1024];

            bool rewound = false;
            if (rewind)
            {
                if (source.CanSeek)
                {
                    try 
                    { 
                        source.Position = 0;
                        rewound = true;
                    }
                    catch { }
                }
                else
                {
                    ReadOnlyRewindableStream roSource = source as ReadOnlyRewindableStream;
                    if (roSource != null && roSource.CanRewind)
                    {
                        roSource.Rewind();
                        rewound = true;
                    }
                }

                //if (!rewound)
                //    throw new InvalidOperationException("Could not rewind the source stream. Most likely the source stream does not support seeking or rewinding"); //LOCALIZEME
            }

			do
			{
				r  = source.Read(buf, 0, buf.Length);
				target.Write(buf, 0, r);
			} while (r > 0);
		}

        /// <summary>
        /// A delegate used to update a progress bar while copying a stream.
        /// </summary>
        /// <param name="copied">The number of bytes copied so far</param>
        /// <param name="remaining">The number of bytes left in the stream. -1 if the stream length is not known</param>
        /// <param name="total">The total number of bytes in the source stream. -1 if the stream length is unknown</param>
        public delegate void StreamCopyProgressDelegate(long copied, long remaining, long total);

        /// <summary>
        /// Copies the content of a stream into another stream, with callbacks.
        /// </summary>
        /// <param name="source">The source stream</param>
        /// <param name="target">The target stream</param>
        /// <param name="callback">An optional callback delegate, may be null.</param>
        /// <param name="updateFrequence">The number of bytes to copy before calling the callback delegate, set to 0 to get every update</param>
        public static void CopyStream(System.IO.Stream source, System.IO.Stream target, StreamCopyProgressDelegate callback, long updateFrequence)
        {
            long length = -1;
            try { length = source.Length; }
            catch { }

            long copied = 0;
            long freqCount = 0;

            if (callback != null)
                callback(copied, length > 0 ? (length - copied) : -1 , length);

            int r;
            byte[] buf = new byte[1024];
            do
            {
                r = source.Read(buf, 0, buf.Length);
                target.Write(buf, 0, r);
                
                copied += r;
                freqCount += r;

                if (freqCount > updateFrequence)
                {
                    freqCount = 0;
                    if (callback != null)
                        callback(copied, length > 0 ? (length - copied) : -1, length);
                }

            } while (r > 0);

            if (callback != null)
                callback(copied, 0, copied);
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
			//Skip UTF file header, since the MapGuide XmlParser is broken
			ms.Position = 0;
			byte[] utfheader = new byte[3];
			if (ms.Read(utfheader, 0, utfheader.Length) == utfheader.Length)
				if (utfheader[0] == 0xEF && utfheader[1] == 0xBB && utfheader[2] == 0xBF)
				{
					ms.Position = 3;
					System.IO.MemoryStream mxs = new System.IO.MemoryStream();
					Utility.CopyStream(ms, mxs, false);
					mxs.Position = 0;
					return mxs;
				}

			ms.Position = 0;
			return ms;
		}

        /// <summary>
        /// Returns a type used to define an unknown column type in a feature reader
        /// </summary>
        public static Type UnmappedType
        {
            get { return typeof(UnmappedDataType); }
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
        /// This method tries to extract the html content of a WebException.
        /// If succesfull, it will return an exception with an error message corresponding to the html text.
        /// If not, it will return the original exception object.
        /// </summary>
        /// <param name="ex">The exception object to extract from</param>
        /// <returns>A potentially better exeception</returns>
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

        /// <summary>
        /// Removes the outer &lt;FeatureInformation&gt; tag, and returns a blank string for empty sets.
        /// This eases the use of SelectionXml, because different parts of MapGuide represents it differently.
        /// </summary>
        /// <param name="input">The string to clean</param>
        /// <returns>The cleaned string</returns>
        public static string CleanUpFeatureSet(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            System.Xml.XmlDocument doc1 = new System.Xml.XmlDocument();
            doc1.LoadXml(input);

            System.Xml.XmlDocument doc2 = new System.Xml.XmlDocument();

            System.Xml.XmlNode root1 = doc1["FeatureInformation"];
            if (root1 == null)
                root1 = doc1;

            if (root1["FeatureSet"] != null)
            {
                System.Xml.XmlNode root2 = doc2.AppendChild(doc2.CreateElement("FeatureSet"));
                root2.InnerXml = root1["FeatureSet"].InnerXml;
            }

            return doc2.OuterXml == "<FeatureSet />" ? "" : doc2.OuterXml;
            
        }

        private static void CopyNodeRecursive(System.Xml.XmlNode n1, System.Xml.XmlNode n2)
        {
            foreach (System.Xml.XmlAttribute attr in n1.Attributes)
                n2.Attributes.Append(n2.OwnerDocument.CreateAttribute(attr.Name)).Value = attr.Value;

            foreach (System.Xml.XmlNode n in n1.ChildNodes)
                CopyNodeRecursive(n, n2.AppendChild(n2.OwnerDocument.CreateElement(n.Name)));
        }

        /// <summary>
        /// Formats a number of bytes to a human readable format, ea.: 2.56 Kb
        /// </summary>
        /// <param name="size">The size in bytes</param>
        /// <returns>The human readable string</returns>
        public static string FormatSizeString(long size)
        {
            if (size > 1024 * 1024 * 1024)
                return string.Format("{0:N} GB", (double)size / (1024 * 1024 * 1024));
            else if (size > 1024 * 1024)
                return string.Format("{0:N} MB", (double)size / (1024 * 1024));
            else if (size > 1024)
                return string.Format("{0:N} KB", (double)size / 1024);
            else
                return string.Format("{0} bytes", size);
        }

        private static System.Text.RegularExpressions.Regex EncRegExp = new System.Text.RegularExpressions.Regex(@"(\-x([0-9]|[a-e]|[A-E])([0-9]|[a-e]|[A-E])\-)|(\-dot\-)|(\-colon\-)", System.Text.RegularExpressions.RegexOptions.Compiled);

        /// <summary>
        /// Converts FDO encoded characters into their original character.
        /// Encoded characters have the form -x00-.
        /// </summary>
        /// <param name="name">The FDO encoded string</param>
        /// <returns>The unencoded version of the string</returns>
        public static string DecodeFDOName(string name)
        {
            System.Text.RegularExpressions.Match m = EncRegExp.Match(name);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int previndex = 0;

            while (m != null && m.Success)
            {
                string replaceval;
                if (m.Value == "-dot-")
                    replaceval = ".";
                else if (m.Value == "-colon-")
                    replaceval = ":";
                else
                    replaceval = ((char)int.Parse(m.Value.Substring(2, 2), System.Globalization.NumberStyles.HexNumber)).ToString();

                sb.Append(name.Substring(previndex, m.Index - previndex));
                sb.Append(replaceval);
                previndex = m.Index + m.Value.Length;

                m = m.NextMatch();
            }

            if (sb.Length == 0)
                return name;
            else
            {
                sb.Append(name.Substring(previndex));
                return sb.ToString();
            }
        }


        /// <summary>
        /// Enumerates all xml nodes in the document, and looks for tags or attributes named ResourceId
        /// </summary>
        /// <param name="item">The xml item to examine</param>
        /// <returns>A list with all found elements</returns>
        public static List<KeyValuePair<System.Xml.XmlNode, string>> GetResourceIdPointers(System.Xml.XmlNode item)
        {
            Queue<System.Xml.XmlNode> lst = new Queue<System.Xml.XmlNode>();
            List<KeyValuePair<System.Xml.XmlNode, string>> res = new List<KeyValuePair<System.Xml.XmlNode, string>>();

            lst.Enqueue(item);

            while (lst.Count > 0)
            {
                System.Xml.XmlNode n = lst.Dequeue();

                foreach (System.Xml.XmlNode nx in n.ChildNodes)
                    if (nx.NodeType == System.Xml.XmlNodeType.Element)
                        lst.Enqueue(nx);

                if (n.Name == "ResourceId")
                    res.Add(new KeyValuePair<System.Xml.XmlNode, string>(n, n.InnerXml));

                if (n.Attributes != null)
                    foreach (System.Xml.XmlAttribute a in n.Attributes)
                        if (a.Name == "ResourceId")
                            res.Add(new KeyValuePair<System.Xml.XmlNode, string>(a, a.Value));
            }

            return res;
        }

        /// <summary>
        /// Enumerates all objects by reflection, returns the list of referenced objects
        /// </summary>
        /// <param name="obj">The object to examine</param>
        /// <returns>Te combined list of references</returns>
        public static List<object> EnumerateObjects(object obj)
        {
            EnumerateObjectCollector c = new EnumerateObjectCollector();
            EnumerateObjects(obj, new EnumerateObjectCallback(c.AddItem));
            return c.items;
        }

        /// <summary>
        /// 
        /// </summary>
        public delegate void EnumerateObjectCallback(object obj);

        /// <summary>
        /// Helper class for easy enumeration collection
        /// </summary>
        private class EnumerateObjectCollector
        {
            public List<object> items = new List<object>();
            public void AddItem(object o)
            {
                items.Add(o);
            }
        }

        /// <summary>
        /// Enumerates all objects by reflection, calling the supplied callback method for each object
        /// </summary>
        /// <param name="obj">The object to examine</param>
        /// <param name="c">The callback function</param>
        public static void EnumerateObjects(object obj, EnumerateObjectCallback c)
        {
			if (obj == null || c == null)
				return;

		    Dictionary<object, object> visited = new Dictionary<object,object>();

            Queue<object> items = new Queue<object>();
            items.Enqueue(obj);

            while (items.Count > 0)
            {
                object o = items.Dequeue();
                if (visited.ContainsKey(o))
                    continue;

                //Prevent infinite recursion by circular reference
                visited.Add(o, o);
                c(o);

                //Try to find the object properties
                foreach (System.Reflection.PropertyInfo pi in o.GetType().GetProperties())
                {
                    //Only index free read-write properties are taken into account
                    if (!pi.CanRead || !pi.CanWrite || pi.GetIndexParameters().Length != 0 || pi.GetValue(o, null) == null)
                        continue;

                    if (pi.GetValue(o, null).GetType().GetInterface(typeof(System.Collections.ICollection).FullName) != null)
                    {
                        //Handle collections
                        System.Collections.ICollection srcList = (System.Collections.ICollection)pi.GetValue(o, null);
                        foreach (object ox in srcList)
                            items.Enqueue(ox);
                    }
                    else if (pi.GetValue(o, null).GetType().IsArray)
                    {
                        //Handle arrays
                        System.Array sourceArr = (System.Array)pi.GetValue(o, null);
                        for (int i = 0; i < sourceArr.Length; i++)
                            items.Enqueue(sourceArr.GetValue(i));
                    }
                    else if (pi.PropertyType.IsClass)
                    {
                        //Handle subobjects
                        items.Enqueue(pi.GetValue(o, null));
                    }
                }
            }
        }
    }
}
