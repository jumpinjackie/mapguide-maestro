#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using OSGeo.MapGuide.ObjectModels.IO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// Utility methods
    /// </summary>
    public static class Utility
    {
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
                int r = int.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int g = int.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int b = int.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                int a = int.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

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
                return node.Attributes["fdo:" + name]; //NOXLATE

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
        /// Copies the content of a stream into another stream.
        /// Automatically attempts to rewind the source stream.
        /// </summary>
        /// <param name="source">The source stream</param>
        /// <param name="target">The target stream</param>
        internal static void CopyStream(System.IO.Stream source, System.IO.Stream target)
        {
            CopyStream(source, target, true);
        }

        /// <summary>
        /// Copies the content of a stream into another stream.
        /// </summary>
        /// <param name="source">The source stream</param>
        /// <param name="target">The target stream</param>
        /// <param name="rewind">True if the source stream should be rewound before being copied</param>
        internal static void CopyStream(System.IO.Stream source, System.IO.Stream target, bool rewind)
        {
            int r;
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

            do
            {
                r = source.Read(buf, 0, buf.Length);
                target.Write(buf, 0, r);
            } while (r > 0);
        }

        /// <summary>
        /// Creates a copy of the stream, with removed Utf8 BOM, if any
        /// </summary>
        /// <param name="ms">The stream to fix</param>
        /// <returns>A stream with no Utf8 BOM</returns>
        internal static System.IO.MemoryStream RemoveUTF8BOM(System.IO.MemoryStream ms)
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
        /// Serializes the given object as a UTF-8 encoded XML string. Any BOM is stripped from the XML string
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        internal static string NormalizedSerialize(XmlSerializer serializer, object o)
        {
            using (var ms = new MemoryStream())
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
    }
}