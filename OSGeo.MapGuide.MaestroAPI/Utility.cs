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

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using OSGeo.MapGuide.ObjectModels.Capabilities.v1_0_0;
using OSGeo.MapGuide.ObjectModels.IO;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// Various helper functions
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Returns true if the given value is in range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="bInclusive"></param>
        /// <returns></returns>
        public static bool InRange<T>(T val, T min, T max, bool bInclusive = true) where T : IComparable
        {
            Check.ThatPreconditionIsMet(min.CompareTo(max) <= 0, "min < max");
            if (bInclusive)
            {
                return val.CompareTo(min) >= 0
                    && val.CompareTo(max) <= 0;
            }
            else
            {
                return val.CompareTo(min) > 0
                    && val.CompareTo(max) < 0;
            }
        }

        /// <summary>
        /// Returns true if this value is zero for all intents and purposes
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsZero(this float val) => Math.Abs(val) < float.Epsilon;

        /// <summary>
        /// Returns true if this value is zero for all intents and purposes
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsZero(this double val) => Math.Abs(val) < double.Epsilon;

        /// <summary>
        /// Creates the polygon WKT for the given bounding box
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static string MakeWktPolygon(double x1, double y1, double x2, double y2)
            => $"POLYGON(({x1} {y1}, {x2} {y1}, {x2} {y2}, {x1} {y2}, {x1} {y1}))"; //NOXLATE

        /// <summary>
        /// Creates the WKT for the given circle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string MakeWktCircle(double x, double y, double r)
            => $"CURVEPOLYGON (({(x - r)} {y} (CIRCULARARCSEGMENT ({x} {(y - r)}, {(x + r)} {y}), CIRCULARARCSEGMENT ({x} {(y + r)}, {(x - r)} {y}))))"; //NOXLATE

        /// <summary>
        /// The exception data key that hints that an <see cref="T:System.Exception"/> thrown is related to XML content errors
        /// </summary>
        public const string XML_EXCEPTION_KEY = "XmlError"; //NOXLATE

        /// <summary>
        /// Gets whether the thrown exception is related to DBXML
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool IsDbXmlError(Exception ex) => ex.Message.Contains("MgDbXmlException") || ex.Message.Contains("MgXmlParserException"); //NOXLATE

        /// <summary>
        /// Gets whether the given exception has original xml content attached
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool HasOriginalXml(Exception ex) => ex.Data[XML_EXCEPTION_KEY] != null;

        //Americans NEVER obey nationalization when outputting decimal values, so the rest of the world always have to work around their bugs :(
        private static CultureInfo m_enCI = new CultureInfo("en-US"); //NOXLATE

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
                if (value.Contains(";")) //NOXLATE
                    value = "\"" + value + "\""; //NOXLATE
                tokens.Add(name + "=" + value); //NOXLATE
            }

            return string.Join(";", tokens.ToArray()); //NOXLATE
        }

        /// <summary>
        /// Strips the version component from provider name
        /// </summary>
        /// <param name="providername">The provider name.</param>
        /// <returns></returns>
        public static string StripVersionFromProviderName(string providername)
        {
            double x;
            string[] parts = providername.Split('.'); //NOXLATE
            for (int i = parts.Length - 1; i >= 0; i--)
            {
                if (!double.TryParse(parts[i], System.Globalization.NumberStyles.Integer, null, out x))
                {
                    if (i != 0)
                        return string.Join(".", parts, 0, i + 1); //NOXLATE
                    break;
                }
            }
            return providername;
        }

        /// <summary>
        /// Checks if the given result is a successful test connection result
        /// </summary>
        /// <param name="sResult"></param>
        /// <returns></returns>
        public static bool IsSuccessfulConnectionTestResult(string sResult) => (sResult == "No errors" || sResult?.ToLower() == "true"); //NOXLATE

        /// <summary>
        /// Parses a color in HTML notation (ea. #ffaabbff)
        /// </summary>
        /// <param name="color">The HTML representation of the color</param>
        /// <returns>The .Net color structure that matches the color</returns>
        public static Color ParseHTMLColor(string color)
        {
            if (color.Length == 8)
            {
                int a = int.Parse(color.Substring(0, 2), NumberStyles.HexNumber);
                int r = int.Parse(color.Substring(2, 2), NumberStyles.HexNumber);
                int g = int.Parse(color.Substring(4, 2), NumberStyles.HexNumber);
                int b = int.Parse(color.Substring(6, 2), NumberStyles.HexNumber);

                return Color.FromArgb(a, r, g, b);
            }
            else if (color.Length == 6)
            {
                int r = int.Parse(color.Substring(0, 2), NumberStyles.HexNumber);
                int g = int.Parse(color.Substring(2, 2), NumberStyles.HexNumber);
                int b = int.Parse(color.Substring(4, 2), NumberStyles.HexNumber);

                return Color.FromArgb(r, g, b);
            }
            else
                throw new Exception(string.Format(Strings.ErrorBadHtmlColor, color));
        }

        /// <summary>
        /// Parses a color in HTML notation (ea. #ffaabbff)
        /// </summary>
        /// <param name="color">The HTML representation of the color</param>
        /// <returns>The .Net color structure that matches the color</returns>
        public static Color ParseHTMLColorRGBA(string color)
        {
            if (color.Length == 8)
            {
                int r = int.Parse(color.Substring(0, 2), NumberStyles.HexNumber);
                int g = int.Parse(color.Substring(2, 2), NumberStyles.HexNumber);
                int b = int.Parse(color.Substring(4, 2), NumberStyles.HexNumber);
                int a = int.Parse(color.Substring(6, 2), NumberStyles.HexNumber);

                return Color.FromArgb(a, r, g, b);
            }
            else if (color.Length == 6)
            {
                int r = int.Parse(color.Substring(0, 2), NumberStyles.HexNumber);
                int g = int.Parse(color.Substring(2, 2), NumberStyles.HexNumber);
                int b = int.Parse(color.Substring(4, 2), NumberStyles.HexNumber);

                return Color.FromArgb(r, g, b);
            }
            else
                throw new Exception(string.Format(Strings.ErrorBadHtmlColor, color));
        }

        /// <summary>
        /// Parses a color in HTML notation (ea. #ffaabbff)
        /// </summary>
        /// <param name="color">The HTML representation of the color</param>
        /// <returns>The .Net color structure that matches the color</returns>
        public static Color ParseHTMLColorARGB(string color)
        {
            if (color.Length == 8)
            {
                int a = int.Parse(color.Substring(0, 2), NumberStyles.HexNumber);
                int r = int.Parse(color.Substring(2, 2), NumberStyles.HexNumber);
                int g = int.Parse(color.Substring(4, 2), NumberStyles.HexNumber);
                int b = int.Parse(color.Substring(6, 2), NumberStyles.HexNumber);

                return Color.FromArgb(a, r, g, b);
            }
            else if (color.Length == 6)
            {
                int r = int.Parse(color.Substring(0, 2), NumberStyles.HexNumber);
                int g = int.Parse(color.Substring(2, 2), NumberStyles.HexNumber);
                int b = int.Parse(color.Substring(4, 2), NumberStyles.HexNumber);

                return Color.FromArgb(r, g, b);
            }
            else
                throw new Exception(string.Format(Strings.ErrorBadHtmlColor, color));
        }

        /// <summary>
        /// Returns the HTML ARGB representation of an .Net color structure
        /// </summary>
        /// <param name="color">The color to encode</param>
        /// <param name="includeAlpha">A flag indicating if the color structures alpha value should be included</param>
        /// <returns>The HTML representation of the color structure</returns>
        public static string SerializeHTMLColor(Color color, bool includeAlpha)
        {
            string res = string.Empty;
            if (includeAlpha)
                res += color.A.ToString("x02"); //NOXLATE
            res += color.R.ToString("x02"); //NOXLATE
            res += color.G.ToString("x02"); //NOXLATE
            res += color.B.ToString("x02"); //NOXLATE
            return res;
        }

        /// <summary>
        /// Returns the HTML RGBA representation of an .Net color structure
        /// </summary>
        /// <param name="color">The color to encode</param>
        /// <param name="includeAlpha">A flag indicating if the color structures alpha value should be included</param>
        /// <returns>The HTML representation of the color structure</returns>
        public static string SerializeHTMLColorRGBA(Color color, bool includeAlpha)
        {
            string res = string.Empty;
            res += color.R.ToString("x02"); //NOXLATE
            res += color.G.ToString("x02"); //NOXLATE
            res += color.B.ToString("x02"); //NOXLATE
            if (includeAlpha)
                res += color.A.ToString("x02"); //NOXLATE
            return res;
        }

        /// <summary>
        /// Returns the HTML ARGB representation of an .Net color structure
        /// </summary>
        /// <param name="color">The color to encode</param>
        /// <param name="includeAlpha">A flag indicating if the color structures alpha value should be included</param>
        /// <returns>The HTML representation of the color structure</returns>
        public static string SerializeHTMLColorARGB(Color color, bool includeAlpha)
        {
            string res = string.Empty;
            if (includeAlpha)
                res += color.A.ToString("x02"); //NOXLATE
            res += color.R.ToString("x02"); //NOXLATE
            res += color.G.ToString("x02"); //NOXLATE
            res += color.B.ToString("x02"); //NOXLATE
            return res;
        }

        /// <summary>
        /// Parses the given query string to a dictionary
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ParseQueryString(string queryString)
        {
            return queryString
                .Split('&')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Split('='))
                .ToDictionary(s => s[0], s => s.Length > 1 ? s[1] : "");
        }

        /// <summary>
        /// Turns a decimal value into a string representation in EN-US format
        /// </summary>
        /// <param name="digit">The value to encode</param>
        /// <returns>The encoded value</returns>
        public static string SerializeDigit(float digit) => digit.ToString(m_enCI);

        /// <summary>
        /// Turns a decimal value into a string representation in EN-US format
        /// </summary>
        /// <param name="digit">The value to encode</param>
        /// <returns>The encoded value</returns>
        public static string SerializeDigit(double digit) => digit.ToString(m_enCI);

        /// <summary>
        /// Copies the content of a stream into another stream.
        /// Automatically attempts to rewind the source stream.
        /// </summary>
        /// <param name="source">The source stream</param>
        /// <param name="target">The target stream</param>
        public static void CopyStream(Stream source, Stream target) => CopyStream(source, target, true);

        /// <summary>
        /// Copies the content of a stream into another stream.
        /// </summary>
        /// <param name="source">The source stream</param>
        /// <param name="target">The target stream</param>
        /// <param name="rewind">True if the source stream should be rewound before being copied</param>
        public static void CopyStream(Stream source, Stream target, bool rewind)
        {
            //int r;
            byte[] buf = new byte[1024];

            //bool rewound = false;
            if (rewind)
            {
                if (source.CanSeek)
                {
                    try
                    {
                        source.Position = 0;
                        //rewound = true;
                    }
                    catch { }
                }
                else
                {
                    ReadOnlyRewindableStream roSource = source as ReadOnlyRewindableStream;
                    if (roSource != null && roSource.CanRewind)
                    {
                        roSource.Rewind();
                        //rewound = true;
                    }
                }

                //if (!rewound)
                //    throw new InvalidOperationException("Could not rewind the source stream. Most likely the source stream does not support seeking or rewinding"); //LOCALIZEME
            }

            source.CopyTo(target);
            /*
            do
            {
                r = source.Read(buf, 0, buf.Length);
                target.Write(buf, 0, r);
            } while (r > 0);
             */
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
        public static void CopyStream(Stream source, Stream target, StreamCopyProgressDelegate callback, long updateFrequence)
        {
            long length = -1;
            if (source.CanSeek)
                length = source.Length;

            long copied = 0;
            long freqCount = 0;

            callback?.Invoke(copied, length > 0 ? (length - copied) : -1, length);

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
                    callback?.Invoke(copied, length > 0 ? (length - copied) : -1, length);
                }
            } while (r > 0);

            callback?.Invoke(copied, 0, copied);
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

            var ser = new XmlSerializer(source.GetType());
            using (var ms = MemoryStreamPool.GetStream())
            {
                ser.Serialize(ms, source);
                ms.Position = 0;
                return ser.Deserialize(ms);
            }
        }

        /*
        /// <summary>
        /// Makes a deep copy of an object, by copying all the public properties.
        /// This overload tries to maintain object references by assigning properties
        /// </summary>
        /// <param name="source">The object to copy</param>
        /// <param name="target">The object to assign to</param>
        /// <returns>A copied object</returns>
        public static object DeepCopy(object source, object target)
        {
            foreach (System.Reflection.PropertyInfo pi in source.GetType().GetProperties())
            {
                if (!pi.CanRead || !pi.CanWrite)
                    continue;

                if (!pi.PropertyType.IsClass || pi.PropertyType == typeof(string))
                    pi.SetValue(target, pi.GetValue(source, null), null);
                else if (pi.GetValue(source, null) == null)
                    pi.SetValue(target, null, null);
                else if (pi.GetValue(source, null).GetType().GetInterface(typeof(System.Collections.ICollection).FullName) != null)
                {
                    System.Collections.ICollection srcList = (System.Collections.ICollection)pi.GetValue(source, null);
                    System.Collections.ICollection trgList = (System.Collections.ICollection)Activator.CreateInstance(srcList.GetType());
                    foreach (object o in srcList)
                        trgList.GetType().GetMethod("Add").Invoke(trgList, new object[] { DeepCopy(o) }); //NOXLATE
                    pi.SetValue(target, trgList, null);
                }
                else if (pi.GetValue(source, null).GetType().IsArray)
                {
                    Array sourceArr = (Array)pi.GetValue(source, null);
                    Array targetArr = (Array)Activator.CreateInstance(sourceArr.GetType(), new object[] { sourceArr.Length });
                    for (int i = 0; i < targetArr.Length; i++)
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

            foreach (System.Reflection.PropertyInfo pi in source.GetType().GetProperties())
            {
                if (!pi.CanRead || !pi.CanWrite)
                    continue;

                if (!pi.PropertyType.IsClass || pi.PropertyType == typeof(string))
                    pi.SetValue(target, pi.GetValue(source, null), null);
                else if (pi.GetValue(source, null) == null)
                    pi.SetValue(target, null, null);
                else if (pi.GetValue(source, null).GetType().GetInterface(typeof(System.Collections.ICollection).FullName) != null)
                {
                    System.Collections.ICollection srcList = (System.Collections.ICollection)pi.GetValue(source, null);
                    System.Collections.ICollection trgList = (System.Collections.ICollection)Activator.CreateInstance(srcList.GetType());
                    foreach (object o in srcList)
                        trgList.GetType().GetMethod("Add").Invoke(trgList, new object[] { DeepCopy(o) }); //NOXLATE
                    pi.SetValue(target, trgList, null);
                }
                else if (pi.GetValue(source, null).GetType().IsArray)
                {
                    Array sourceArr = (Array)pi.GetValue(source, null);
                    Array targetArr = (Array)Activator.CreateInstance(sourceArr.GetType(), new object[] { sourceArr.Length });
                    for (int i = 0; i < targetArr.Length; i++)
                        targetArr.SetValue(DeepCopy(sourceArr.GetValue(i)), i);
                    pi.SetValue(target, targetArr, null);
                }
                else
                    pi.SetValue(target, DeepCopy(pi.GetValue(source, null)), null);
            }

            return target;
        }
        */

        /// <summary>
        /// Reads all data from a stream, and returns it as a single array.
        /// Note that this is very inefficient if the stream is several megabytes long.
        /// </summary>
        /// <param name="s">The stream to exhaust</param>
        /// <returns>The streams content as an array</returns>
        public static byte[] StreamAsArray(Stream s)
        {
            var mes = s as MemoryStream;
            if (mes != null)
                return mes.ToArray();

            if (!s.CanSeek)
            {
                var ms = MemoryStreamPool.GetStream();
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

        /*
        /// <summary>
        /// Serializes the given object as a UTF-8 encoded XML string. Any BOM is stripped from the XML string
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string NormalizedSerialize(XmlSerializer serializer, object o)
        {
            using (var ms = MemoryStreamPool.GetStream())
            {
                using (var xw = new Utf8XmlWriter(ms))
                {
                    serializer.Serialize(xw, o);
                    using (var ms2 = RemoveUTF8BOM(ms))
                    {
                        using (var sr = new StreamReader(ms2))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
        */

        /// <summary>
        /// Creates a copy of the stream, with removed Utf8 BOM, if any
        /// </summary>
        /// <param name="ms">The stream to fix</param>
        /// <returns>A stream with no Utf8 BOM</returns>
        public static MemoryStream RemoveUTF8BOM(MemoryStream ms)
        {
            //Skip UTF file header, since the MapGuide XmlParser is broken
            ms.Position = 0;
            byte[] utfheader = new byte[3];
            if (ms.Read(utfheader, 0, utfheader.Length) == utfheader.Length)
            {
                if (utfheader[0] == 0xEF && utfheader[1] == 0xBB && utfheader[2] == 0xBF)
                {
                    ms.Position = 3;
                    var mxs = MemoryStreamPool.GetStream();
                    CopyStream(ms, mxs, false);
                    mxs.Position = 0;
                    return mxs;
                }
            }
            ms.Position = 0;
            return ms;
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
            if (ex is System.Net.WebException wex)
            {
                try
                {
                    if (wex.Response != null)
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(wex.Response.GetResponseStream()))
                        {
                            string html = sr.ReadToEnd();
                            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("(\\<body\\>)(.+)\\<\\/body\\>", System.Text.RegularExpressions.RegexOptions.Singleline);
                            System.Text.RegularExpressions.Match m = r.Match(html);
                            if (m.Success && m.Groups.Count == 3)
                            {
                                html = m.Groups[2].Value;
                                int n = html.IndexOf("</h2>"); //NOXLATE
                                if (n > 0)
                                    html = html.Substring(n + "</h2>".Length); //NOXLATE
                            }

                            return new Exception(wex.Message + ": " + html, wex); //NOXLATE
                        }
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

            var doc1 = new XmlDocument();
            doc1.LoadXml(input);

            var doc2 = new XmlDocument();

            XmlNode root1 = doc1["FeatureInformation"]; //NOXLATE
            if (root1 == null)
                root1 = doc1;

            if (root1["FeatureSet"] != null) //NOXLATE
            {
                XmlNode root2 = doc2.AppendChild(doc2.CreateElement("FeatureSet")); //NOXLATE
                root2.InnerXml = root1["FeatureSet"].InnerXml; //NOXLATE
            }

            return doc2.OuterXml == "<FeatureSet />" ? "" : doc2.OuterXml; //NOXLATE
        }

        /// <summary>
        /// Formats a number of bytes to a human readable format, ea.: 2.56 Kb
        /// </summary>
        /// <param name="size">The size in bytes</param>
        /// <returns>The human readable string</returns>
        public static string FormatSizeString(long size)
        {
            if (size > 1024 * 1024 * 1024)
                return string.Format("{0:N} GB", (double)size / (1024 * 1024 * 1024)); //NOXLATE
            else if (size > 1024 * 1024)
                return string.Format("{0:N} MB", (double)size / (1024 * 1024)); //NOXLATE
            else if (size > 1024)
                return string.Format("{0:N} KB", (double)size / 1024); //NOXLATE
            else
                return string.Format("{0} bytes", size); //NOXLATE
        }

        private static Regex EncRegExp = new Regex(@"(\-x[0-2a-fA-F][0-9a-fA-F]\-)|(\-dot\-)|(\-colon\-)", System.Text.RegularExpressions.RegexOptions.Compiled);

        private static Regex TokenRegex = new Regex("^x[0-9a-fA-F][0-9a-fA-F]", RegexOptions.Compiled);

        private static Regex TokenRegex2 = new Regex("^_x[0-9a-fA-F][0-9a-fA-F]", RegexOptions.Compiled);

        /// <summary>
        /// FDO encodes a string
        /// </summary>
        /// <param name="name"></param>
        /// <remarks>
        /// <para>
        /// FDO names must always be encoded when writing back to attributes in XML configuration documents as it may contain reserved characters that would render the final XML attribute content invalid.
        /// </para>
        /// <para>
        /// Consequently, such names must always be decoded when reading from XML configuration documents otherwise these escape characters may still be present after reading
        /// </para>
        /// </remarks>
        /// <returns></returns>
        public static string EncodeFDOName(string name)
        {
            //Decode characters not allowed by FDO
            string lName = name.Replace("-dot-", ".")
                               .Replace("-colon-", ":");

            //Break the name up by '-' delimiters
            string[] tokens = lName.Split(new char[] { '-' }, StringSplitOptions.None);

            StringBuilder outName = new StringBuilder();

            // Encode any characters that are not allowed in XML names.
            // The encoding pattern is "-x%x-" where %x is the character value in hexidecimal.
            // The dash delimeters were an unfortunate choice since dash cannot be the 1st character
            // in an XML name. When the 1st character needs to be encoded, it is encoded as "_x%x-" to
            // resolve this issue.
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                bool bMatchedToken = false;
                if (TokenRegex.Match(token, 0).Success)
                {
                    bMatchedToken = true;
                    // the token happens to match the encoding pattern. We want to avoid
                    // decoding this sub-string on decode. This is done by encoding the leading
                    // dash.
                    if (outName.Length == 0)
                        outName.Append(string.Format("_x{0:X}-", Convert.ToInt32('-')).ToLower());
                    else
                        outName.Append(string.Format("-x{0:X}-", Convert.ToInt32('-')).ToLower());
                }
                else if (TokenRegex2.Match(token, 0).Success && i == 0)
                {
                    bMatchedToken = true;
                    // the token happens to match the encoding pattern for the 1st character.
                    // We want to avoid decoding this sub-string on decode.
                    // This is done by prepending a dummy encoding for character 0. This character is
                    // discarded on decode.
                    outName.Append("_x00-");
                }
                else
                {
                    // The token doesn't match the encoding pattern, just write the dash
                    // that was discarded by the tokenizer.
                    if (i > 0)
                    {
                        if (outName.Length == 0)
                            outName.Append("_x2d-"); // 1st character so lead with '_'
                        else
                            outName.Append("-");
                    }
                }
                outName.Append(bMatchedToken ? token : ReplaceBadChars(token, outName.Length == 0));
            }

            char c = outName[0];

            //Perform actual substitutions of bad characters
            outName = outName.Remove(0, 1);

            //Check if the first character requires a meta-escape character replacement
            string prefix = c + string.Empty;
            switch (c)
            {
                case ' ':
                    prefix = "_x20-";
                    break;

                case '-':
                    prefix = "_x2d-";
                    break;

                case '&':
                    prefix = "_x26-";
                    break;

                default:
                    if (Char.IsDigit(c))
                    {
                        prefix = "_x3" + c + "-";
                    }
                    break;
            }
            string result = prefix + outName.ToString();
            return result;
        }

        private static string ReplaceBadChars(string token, bool bFirstInString)
        {
            StringBuilder sb = new StringBuilder();
            bool bFirstChar = bFirstInString;
            foreach (char c in token)
            {
                if (Char.IsDigit(c) || IsValidXmlChar(c))
                    sb.Append(c);
                else
                    sb.Append(string.Format("{0}x{1:X}-", bFirstChar ? "_" : "-", Convert.ToInt32(c)).ToLower());

                bFirstChar = false;
            }
            return sb.ToString();
        }

        private static bool IsValidXmlChar(char c)
        {
            try
            {
                XmlConvert.VerifyNCName(c + "");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converts FDO encoded characters into their original character.
        /// </summary>
        /// <param name="name">The FDO encoded string</param>
        /// <remarks>
        /// <para>
        /// FDO names must always be encoded when writing back to attributes in XML configuration documents as it may contain reserved characters that would render the final XML attribute content invalid.
        /// </para>
        /// <para>
        /// Consequently, such names must always be decoded when reading from XML configuration documents otherwise these escape characters may still be present after reading
        /// </para>
        /// </remarks>
        /// <returns>The unencoded version of the string</returns>
        public static string DecodeFDOName(string name)
        {
            // The encoding pattern is delimited by '-' so break string up by '-'.
            string[] tokens = name.Split(new char[] { '-' }, StringSplitOptions.None);
            bool prevDecode = true;
            StringBuilder decoded = new StringBuilder();
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                //This is a special character inserted during the encoding process.
                //If we find this at the beginning, discard it
                if (i == 0 && token == "_x00")
                    continue;

                var m = TokenRegex.Match(token, 0);
                var m2 = TokenRegex2.Match(token, 0);
                if (token.Length == 4 && token.StartsWith("_x3") && Char.IsDigit(token[3]))
                {
                    decoded.Append(token[3]);
                    prevDecode = true;
                }
                else
                {
                    if ((!prevDecode) && m.Success)
                    {
                        string replace = ((char)int.Parse(m.Value.Substring(1, 2), System.Globalization.NumberStyles.HexNumber)).ToString();
                        decoded.Append(replace);
                        prevDecode = true;
                    }
                    else if ((i == 0) && m2.Success)
                    {
                        string replace = ((char)int.Parse(m2.Value.Substring(2, 2), System.Globalization.NumberStyles.HexNumber)).ToString();
                        decoded.Append(replace);
                        prevDecode = true;
                    }
                    else
                    {
                        if (i > 0 && !prevDecode)
                            decoded.Append("-");

                        decoded.Append(token);
                        prevDecode = false;
                    }
                }
            }
            return decoded.ToString();
        }

        /// <summary>
        /// Enumerates all xml nodes in the document, and looks for tags or attributes named ResourceId
        /// </summary>
        /// <param name="item">The xml item to examine</param>
        /// <returns>A list with all found elements</returns>
        public static List<KeyValuePair<XmlNode, string>> GetResourceIdPointers(System.Xml.XmlNode item)
        {
            var lst = new Queue<XmlNode>();
            var res = new List<KeyValuePair<XmlNode, string>>();

            lst.Enqueue(item);

            while (lst.Count > 0)
            {
                XmlNode n = lst.Dequeue();

                foreach (XmlNode nx in n.ChildNodes)
                {
                    if (nx.NodeType == XmlNodeType.Element)
                        lst.Enqueue(nx);
                }

                if (n.Name == "ResourceId") //NOXLATE
                {
                    res.Add(new KeyValuePair<XmlNode, string>(n, n.InnerXml));
                }

                if (n.Attributes != null)
                {
                    foreach (XmlAttribute a in n.Attributes)
                    {
                        if (a.Name == "ResourceId") //NOXLATE
                            res.Add(new KeyValuePair<XmlNode, string>(a, a.Value));
                    }
                }
            }

            return res;
        }

        /*
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

            Dictionary<object, object> visited = new Dictionary<object, object>();

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
        */

        /// <summary>
        /// Transforms the envelope from the given source coordinate system to the
        /// given target coordinate system
        /// </summary>
        /// <param name="csCatalog">The connection's associated coordinate system catalog</param>
        /// <param name="env">The envelope.</param>
        /// <param name="srcCsWkt">The source coordinate system WKT.</param>
        /// <param name="dstCsWkt">The destination coordinate system WKT.</param>
        /// <returns></returns>
        public static ObjectModels.Common.IEnvelope TransformEnvelope(ICoordinateSystemCatalog csCatalog, ObjectModels.Common.IEnvelope env, string srcCsWkt, string dstCsWkt)
        {
            try
            {
                ISimpleTransform trans = null;
                trans = CsHelper.DefaultCatalog != null ? CsHelper.DefaultCatalog.CreateTransform(srcCsWkt, dstCsWkt) : csCatalog.CreateTransform(srcCsWkt, dstCsWkt);
                var oldExt = env;

                double llx;
                double lly;
                double urx;
                double ury;

                if (trans.Transform(oldExt.MinX, oldExt.MinY, out llx, out lly) &&
                    trans.Transform(oldExt.MaxX, oldExt.MaxY, out urx, out ury))
                {
                    return ObjectFactory.CreateEnvelope(llx, lly, urx, ury);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets an fdo-related attribute from the specified xml element using the
        /// unqualified name and trying again with the fdo: qualifier if it didn't exist
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static XmlAttribute GetFdoAttribute(XmlNode node, string name)
        {
            var att = node.Attributes[name];
            if (att == null)
            {
                att = node.Attributes.Cast<XmlAttribute>().FirstOrDefault(a =>
                {
                    return a.Name.ToUpper() == $"FDO:{name.ToUpper()}"; //NOXLATE
                });
            }
            return att;
        }

        /// <summary>
        /// Gets an fdo-related element from the specified xml element using the
        /// unqualified name and trying again with the fdo: qualifier if it didn't exist
        /// </summary>
        /// <param name="el"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static XmlElement GetFdoElement(XmlElement el, string name)
        {
            var element = el[name];
            if (element == null)
                return el["fdo:" + name]; //NOXLATE
            return element;
        }

        /// <summary>
        /// Explodes a themed layer into filtered sub-layers where each sub-layer is filtered on the individual theme rule's filter
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="options"></param>
        /// <param name="progress"></param>
        public static void ExplodeThemeIntoFilteredLayers(IServerConnection conn, ExplodeThemeOptions options, LengthyOperationProgressCallBack progress)
        {
            var origLayerId = options.Layer.ResourceID;
            string layerPrefix = options.LayerPrefix;

            var origVl = (IVectorLayerDefinition)options.Layer.SubLayer;
            var origRange = options.Range;
            var origStyle = options.ActiveStyle;
            int processed = 0;
            for (int i = 0; i < origStyle.RuleCount; i++)
            {
                var currentRule = origStyle.GetRuleAt(i);

                var newLayer = ObjectFactory.CreateDefaultLayer(LayerType.Vector, options.Layer.ResourceVersion);
                var vl = (IVectorLayerDefinition)newLayer.SubLayer;
                vl.ResourceId = origVl.ResourceId;
                vl.FeatureName = origVl.FeatureName;
                vl.Geometry = origVl.Geometry;

                //Set this layer's filter to be that of the current rule
                vl.Filter = currentRule.Filter;

                //A newly created Layer Definition will only have one scale range
                var range = vl.GetScaleRangeAt(0);
                range.MinScale = origRange.MinScale;
                range.MaxScale = origRange.MaxScale;

                //Composite styles aren't applicable, so remove them if they exist
                var range2 = range as IVectorScaleRange2;
                if (range2 != null)
                    range2.CompositeStyle = null;

                //Invalidate geometry types not of the original style
                switch (origStyle.StyleType)
                {
                    case StyleType.Area:
                        range.LineStyle = null;
                        range.PointStyle = null;
                        IAreaRule ar = range.AreaStyle.GetRuleAt(0);
                        IAreaRule oar = (IAreaRule)currentRule;
                        if (oar.AreaSymbolization2D != null)
                            ar.AreaSymbolization2D = oar.AreaSymbolization2D.Clone();
                        if (oar.Label != null)
                            ar.Label = oar.Label.Clone();
                        break;

                    case StyleType.Line:
                        range.AreaStyle = null;
                        range.PointStyle = null;
                        ILineRule lr = range.LineStyle.GetRuleAt(0);
                        ILineRule olr = (ILineRule)currentRule;
                        if (olr.StrokeCount > 0)
                        {
                            foreach (var stroke in olr.Strokes)
                            {
                                lr.AddStroke(stroke.Clone());
                            }
                        }
                        if (olr.Label != null)
                            lr.Label = olr.Label.Clone();
                        break;

                    case StyleType.Point:
                        range.AreaStyle = null;
                        range.LineStyle = null;
                        IPointRule pr = range.PointStyle.GetRuleAt(0);
                        IPointRule opr = (IPointRule)currentRule;
                        if (opr.Label != null)
                            pr.Label = opr.Label.Clone();
                        if (opr.PointSymbolization2D != null)
                            pr.PointSymbolization2D = opr.PointSymbolization2D.Clone();
                        break;
                }

                string newResId = options.FolderId +
                                  GenerateLayerName(options.LayerNameFormat, layerPrefix, GetScaleRangeStr(options.Range), i, currentRule) +
                                  ".LayerDefinition";

                conn.ResourceService.SaveResourceAs(newLayer, newResId);
                processed++;
                progress?.Invoke(null, new LengthyOperationProgressArgs(newResId, (processed / origStyle.RuleCount) * 100));
            }
        }

        private const string RESERVED_CHARS = "\\:*?\"<>|&'%=/";

        /// <summary>
        /// Creates a default Layer Definition from the given Class Definition
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="fsId"></param>
        /// <param name="clsDef"></param>
        /// <param name="targetFolder"></param>
        /// <returns></returns>
        public static string CreateDefaultLayer(IServerConnection conn, string fsId, ClassDefinition clsDef, string targetFolder)
        {
            GeometricPropertyDefinition geom = null;
            //Try designated
            if (!string.IsNullOrEmpty(clsDef.DefaultGeometryPropertyName))
            {
                geom = clsDef.FindProperty(clsDef.DefaultGeometryPropertyName) as GeometricPropertyDefinition;
            }
            //Try first geom property
            if (geom == null)
            {
                foreach (PropertyDefinition pd in clsDef.Properties)
                {
                    if (pd.Type == PropertyDefinitionType.Geometry)
                    {
                        geom = (GeometricPropertyDefinition)pd;
                        break;
                    }
                }
            }
            //Can't proceed without a geometry
            if (geom == null)
                return null;

            //Compute target resource id
            var prefix = targetFolder;
            if (!prefix.EndsWith("/")) //NOXLATE
                prefix += "/";
            var lyrId = prefix + clsDef.Name + ".LayerDefinition"; //NOXLATE

            int counter = 0;
            while (conn.ResourceService.ResourceExists(lyrId))
            {
                counter++;
                lyrId = prefix + clsDef.Name + "(" + counter + ").LayerDefinition"; //NOXLATE
            }

            var ld = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(1, 0, 0));

            //Assign default properties
            ld.ResourceID = lyrId;
            var vld = ld.SubLayer as IVectorLayerDefinition;
            vld.ResourceId = fsId;
            vld.FeatureName = clsDef.QualifiedName;
            vld.Geometry = geom.Name;

            //Infer geometry storage support and remove unsupported styles
            var scale = vld.GetScaleRangeAt(0);
            var geomTypes = geom.GetIndividualGeometricTypes();
            var remove = new List<string>();
            if (Array.IndexOf(geomTypes, FeatureGeometricType.Point) < 0)
            {
                remove.Add(FeatureGeometricType.Point.ToString().ToLower());
            }
            if (Array.IndexOf(geomTypes, FeatureGeometricType.Curve) < 0)
            {
                remove.Add(FeatureGeometricType.Curve.ToString().ToLower());
            }
            if (Array.IndexOf(geomTypes, FeatureGeometricType.Surface) < 0)
            {
                remove.Add(FeatureGeometricType.Surface.ToString().ToLower());
            }

            scale.RemoveStyles(remove);
            conn.ResourceService.SaveResource(ld);

            return lyrId;
        }

        /// <summary>
        /// Generates a unique layer name for a given vector rule
        /// </summary>
        /// <param name="layerFormat"></param>
        /// <param name="layerName"></param>
        /// <param name="scaleRange"></param>
        /// <param name="i"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static string GenerateLayerName(string layerFormat, string layerName, string scaleRange, int i, IVectorRule rule)
        {
            StringBuilder sb = new StringBuilder(string.Format(layerFormat, layerName, scaleRange, string.IsNullOrEmpty(rule.LegendLabel) ? ("Rule" + i) : rule.LegendLabel)); //NOXLATE
            foreach (char c in RESERVED_CHARS.ToCharArray())
            {
                sb.Replace(c, '_');
            }
            return sb.ToString().Trim();
        }

        /// <summary>
        /// String-ifies a scale range
        /// </summary>
        /// <param name="parentRange"></param>
        /// <returns></returns>
        public static string GetScaleRangeStr(IVectorScaleRange parentRange)
        {
            return string.Format("{0} to {1}", //NOXLATE
                parentRange.MinScale.HasValue ? parentRange.MinScale.Value.ToString(CultureInfo.InvariantCulture) : "0", //NOXLATE
                parentRange.MaxScale.HasValue ? parentRange.MaxScale.Value.ToString(CultureInfo.InvariantCulture) : "Infinity"); //NOXLATE
        }

        /// <summary>
        /// Surrounds the given string with single-quotes. Mainly used for FDO expressions where string literals are required as un-quoted
        /// strings will trigger the FDO expression engine.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FdoStringifiyLiteral(string str) => $"'{str}'"; //NOXLATE

        /// <summary>
        /// Returns the list of known FDO stylization expression functions
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IFdoFunctionDefintion> GetStylizationFunctions()
        {
            //ARGB
            yield return new FdoProviderCapabilitiesExpressionFunctionDefinition()
            {
                ArgumentDefinitionList = new BindingList<FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition>()
                {
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "aValue", //NOXLATE
                        Description = Strings.Func_ARGB_AValueDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.Int32
                    },
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "rValue", //NOXLATE
                        Description = Strings.Func_ARGB_RValueDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.Int32
                    },
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "gValue", //NOXLATE
                        Description = Strings.Func_ARGB_GValueDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.Int32
                    },
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "bValue", //NOXLATE
                        Description = Strings.Func_ARGB_BValueDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.Int32
                    },
                },
                Description = Strings.Func_ARGB_Description,
                Name = "ARGB", //NOXLATE
                ReturnType = "Int32" //NOXLATE
            };
            //DECAP
            yield return new FdoProviderCapabilitiesExpressionFunctionDefinition()
            {
                ArgumentDefinitionList = new BindingList<FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition>()
                {
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "strValue", //NOXLATE
                        Description = Strings.Func_DECAP_StringValueDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    }
                },
                Description = Strings.Func_DECAP_Description,
                Name = "DECAP", //NOXLATE
                ReturnType = "String" //NOXLATE
            };
            //FEATURECLASS
            yield return new FdoProviderCapabilitiesExpressionFunctionDefinition()
            {
                ArgumentDefinitionList = new BindingList<FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition>(),
                Description = Strings.Func_FEATURECLASS_Description,
                Name = "FEATURECLASS", //NOXLATE
                ReturnType = "String" //NOXLATE
            };
            //FEATUREID
            yield return new FdoProviderCapabilitiesExpressionFunctionDefinition()
            {
                ArgumentDefinitionList = new BindingList<FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition>(),
                Description = Strings.Func_FEATUREID_Description,
                Name = "FEATUREID", //NOXLATE
                ReturnType = "String" //NOXLATE
            };
            //IF
            yield return new FdoProviderCapabilitiesExpressionFunctionDefinition()
            {
                ArgumentDefinitionList = new BindingList<FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition>()
                {
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "condition", //NOXLATE
                        Description = Strings.Func_IF_ConditionDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    },
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "trueValue", //NOXLATE
                        Description = Strings.Func_IF_TrueValueDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    },
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "falseValue", //NOXLATE
                        Description = Strings.Func_IF_FalseValueDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    }
                },
                Description = Strings.Func_IF_Description,
                Name = "IF", //NOXLATE
                ReturnType = "String" //NOXLATE
            };
            //LAYERID
            yield return new FdoProviderCapabilitiesExpressionFunctionDefinition()
            {
                ArgumentDefinitionList = new BindingList<FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition>(),
                Description = Strings.Func_LAYERID_Description,
                Name = "LAYERID", //NOXLATE
                ReturnType = "String" //NOXLATE
            };
            //LOOKUP
            yield return new FdoProviderCapabilitiesExpressionFunctionDefinition()
            {
                ArgumentDefinitionList = new BindingList<FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition>()
                {
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "expression", //NOXLATE
                        Description = Strings.Func_LOOKUP_ExpressionDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    },
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "defaultValue", //NOXLATE
                        Description = Strings.Func_LOOKUP_DefaultValueDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    },
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "index", //NOXLATE
                        Description = Strings.Func_LOOKUP_IndexDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    },
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "value", //NOXLATE
                        Description = Strings.Func_LOOKUP_ValueDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    }
                },
                Description = Strings.Func_LOOKUP_Description,
                Name = "LOOKUP", //NOXLATE
                ReturnType = "String" //NOXLATE
            };
            //MAPNAME
            yield return new FdoProviderCapabilitiesExpressionFunctionDefinition()
            {
                ArgumentDefinitionList = new BindingList<FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition>(),
                Description = Strings.Func_MAPNAME_Description,
                Name = "MAPNAME", //NOXLATE
                ReturnType = "String" //NOXLATE
            };
            //RANGE
            yield return new FdoProviderCapabilitiesExpressionFunctionDefinition()
            {
                ArgumentDefinitionList = new BindingList<FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition>()
                {
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "expression", //NOXLATE
                        Description = Strings.Func_RANGE_ExpressionDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    },
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "rangeMin", //NOXLATE
                        Description = Strings.Func_RANGE_MinDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    },
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "rangeMax", //NOXLATE
                        Description = Strings.Func_RANGE_MaxDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    },
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "defaultValue", //NOXLATE
                        Description = Strings.Func_RANGE_DefaultValueDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    },
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "value", //NOXLATE
                        Description = Strings.Func_RANGE_ValueDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    }
                },
                Description = Strings.Func_RANGE_Description,
                Name = "RANGE", //NOXLATE
                ReturnType = "String" //NOXLATE
            };
            //SESSION
            yield return new FdoProviderCapabilitiesExpressionFunctionDefinition()
            {
                ArgumentDefinitionList = new BindingList<FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition>(),
                Description = Strings.Func_SESSION_Description,
                Name = "SESSION", //NOXLATE
                ReturnType = "String" //NOXLATE
            };
            //URLENCODE
            yield return new FdoProviderCapabilitiesExpressionFunctionDefinition()
            {
                ArgumentDefinitionList = new BindingList<FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition>()
                {
                    new FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinition()
                    {
                        Name = "strValue", //NOXLATE
                        Description = Strings.Func_URLENCODE_StringValueDescription,
                        DataType = FdoProviderCapabilitiesExpressionFunctionDefinitionArgumentDefinitionDataType.String
                    }
                },
                Description = Strings.Func_URLENCODE_Description,
                Name = "URLENCODE", //NOXLATE
                ReturnType = "String" //NOXLATE
            };
        }

        /// <summary>
        /// Creates a preview map definition for the given watermark
        /// </summary>
        /// <param name="wmd"></param>
        /// <returns></returns>
        public static IMapDefinition2 CreateWatermarkPreviewMapDefinition(IWatermarkDefinition wmd)
        {
            IMapDefinition2 map = (IMapDefinition2)ObjectFactory.CreateMapDefinition(wmd.SupportedMapDefinitionVersion, "Watermark Definition Preview"); //NOXLATE
            map.CoordinateSystem = @"LOCAL_CS[""*XY-M*"", LOCAL_DATUM[""*X-Y*"", 10000], UNIT[""Meter"", 1], AXIS[""X"", EAST], AXIS[""Y"", NORTH]]"; //NOXLATE
            map.Extents = ObjectFactory.CreateEnvelope(-1000000, -1000000, 1000000, 1000000);
            map.AddWatermark(wmd);
            return map;
        }

        /// <summary>
        /// Replaces all references of the given resource id.
        /// </summary>
        /// <param name="doc">A resource document. Any replacements will modify the XmlDocument that is passed in</param>
        /// <param name="srcId">The resource id to replace</param>
        /// <param name="dstId">The resource id to replace with</param>
        /// <returns>true if a replacement was made. false if no replacements were made</returns>
        public static bool ReplaceResourceIds(XmlDocument doc, string srcId, string dstId)
        {
            Check.ArgumentNotEmpty(srcId, nameof(srcId));
            Check.ArgumentNotEmpty(dstId, nameof(dstId));
            bool changed = false;
            //There's an unwritten spec that all elements that refer to a Resource ID are named "ResourceId".
            //This is why this method can be relied upon to cover all resource id references.
            var resIdNodes = Utility.GetResourceIdPointers(doc);
            for (int i = 0; i < resIdNodes.Count; i++)
            {
                var kvp = resIdNodes[i];
                if (kvp.Value.Equals(srcId))
                {
                    kvp.Key.InnerXml = dstId;
                    changed = true;
                }
            }
            return changed;
        }

        //From: http://www.pvladov.com/2012/09/make-color-lighter-or-darker.html

        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1.
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="System.Drawing.Color"/> structure.
        /// </returns>
        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        /// <summary>
        /// Creates a simple symbol definition at the highest supported resource version
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static ISimpleSymbolDefinition CreateSimpleSymbol(IServerConnection conn, string name, string description)
            => ObjectFactory.CreateSimpleSymbol(conn.Capabilities.GetMaxSupportedResourceVersion(ResourceTypes.SymbolDefinition.ToString()), name, description);
        
        /// <summary>
        /// Creates a map definition at the highest supported resource version
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IMapDefinition CreateMapDefinition(IServerConnection conn, string name)
            => ObjectFactory.CreateMapDefinition(conn.Capabilities.GetMaxSupportedResourceVersion(ResourceTypes.MapDefinition.ToString()), name);

        /// <summary>
        /// Creates a map definition at the highest supported resource version
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="name"></param>
        /// <param name="csWkt"></param>
        /// <param name="extent"></param>
        /// <returns></returns>
        public static IMapDefinition CreateMapDefinition(IServerConnection conn, string name, string csWkt, ObjectModels.Common.IEnvelope extent)
            => ObjectFactory.CreateMapDefinition(conn.Capabilities.GetMaxSupportedResourceVersion(ResourceTypes.MapDefinition.ToString()),
                                                     name,
                                                     csWkt,
                                                     extent);

        /// <summary>
        /// Creates a layer definition at the highest supported resource version
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="layerType"></param>
        /// <returns></returns>
        public static ILayerDefinition CreateDefaultLayer(IServerConnection conn, LayerType layerType)
            => ObjectFactory.CreateDefaultLayer(layerType, conn.Capabilities.GetMaxSupportedResourceVersion(ResourceTypes.LayerDefinition.ToString()));
        
        /// <summary>
        /// Creates a flexible layout
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="templateName"></param>
        /// <param name="statelessMode">If true, the widget set will only be populated with widgets that can work without a running MapGuide Server</param>
        /// <returns></returns>
        public static IApplicationDefinition CreateFlexibleLayout(IServerConnection conn, string templateName, bool statelessMode = false)
        {
            Check.ThatPreconditionIsMet(Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Fusion) >= 0, "Required Fusion service not supported on this connection");

            IFusionService service = (IFusionService)conn.GetService((int)ServiceType.Fusion);
            var templates = service.GetApplicationTemplates();
            var widgets = service.GetApplicationWidgets();
            var containers = service.GetApplicationContainers();

            return ObjectFactory.CreateFlexibleLayout(conn.SiteVersion, templates, widgets, containers, templateName, statelessMode);
        }

        /// <summary>
        /// Strips known WMS metadata from the given resource document header
        /// </summary>
        /// <param name="header"></param>
        public static void StripWmsMetadata(ObjectModels.Common.ResourceDocumentHeaderType header)
        {
            //Rather than a wholsale nukage of the Metadata, we will only remove known WMS metadata
            //properties (so any custom metadata is preserved)
            header.Metadata.SetProperty("_Title", null); //NOXLATE
            header.Metadata.SetProperty("_Keywords", null); //NOXLATE
            header.Metadata.SetProperty("_Abstract", null); //NOXLATE
            header.Metadata.SetProperty("_ExtendedMetadata", null); //NOXLATE

            header.Metadata.SetProperty("_Queryable", null); //NOXLATE
            header.Metadata.SetProperty("_Opaque", null); //NOXLATE
            header.Metadata.SetProperty("_IsPublished", null); //NOXLATE
            header.Metadata.SetProperty("_EnableGeometry", null); //NOXLATE
            header.Metadata.SetProperty("_Bounds", null); //NOXLATE

            if (header.Metadata.Simple.Property.Count == 0)
                header.Metadata = null;
        }

        /// <summary>
        /// Helper to determine if the given exception is related to session expiry
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool IsSessionExpiredException(Exception ex)
        {
            var wex = ex as System.Net.WebException;
            if (wex != null && (wex.Message.ToLower().IndexOf("session expired") >= 0 || wex.Message.ToLower().IndexOf("session not found") >= 0 || wex.Message.ToLower().IndexOf("mgsessionexpiredexception") >= 0))
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Defines parameter for exploding a theme into filtered layers
    /// </summary>
    public class ExplodeThemeOptions
    {
        /// <summary>
        /// The layer definition
        /// </summary>
        public ILayerDefinition Layer { get; set; }

        /// <summary>
        /// The active scale range
        /// </summary>
        public IVectorScaleRange Range { get; set; }

        /// <summary>
        /// The active style
        /// </summary>
        public IVectorStyle ActiveStyle { get; set; }

        /// <summary>
        /// The folder to create the layers in
        /// </summary>
        public string FolderId { get; set; }

        /// <summary>
        /// The layer name format string
        /// </summary>
        public string LayerNameFormat { get; set; }

        /// <summary>
        /// The layer prefix
        /// </summary>
        public string LayerPrefix { get; set; }
    }
}