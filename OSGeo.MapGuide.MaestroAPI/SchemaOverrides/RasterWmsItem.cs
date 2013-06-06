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
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels;
using System.Drawing;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.SchemaOverrides
{
    /// <summary>
    /// A WMS Raster configuration element
    /// </summary>
    public class RasterWmsItem : IFdoSerializable
    {
        /// <summary>
        /// Represents all valid WMS image formats
        /// </summary>
        public class WmsImageFormat
        {
            /// <summary>
            /// Portable Network Graphics (PNG)
            /// </summary>
            public const string PNG = "PNG"; //NOXLATE
            /// <summary>
            /// Tagged Image File (TIF)
            /// </summary>
            public const string TIF = "TIF"; //NOXLATE
            /// <summary>
            /// Joint Photographic Experts Group (JPEG)
            /// </summary>
            public const string JPG = "JPG"; //NOXLATE
            /// <summary>
            /// Graphics Interchange Format (GIF)
            /// </summary>
            public const string GIF = "GIF"; //NOXLATE
        }

        internal RasterWmsItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterWmsItem"/> class.
        /// </summary>
        /// <param name="schemaName">Name of the schema</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="rasterPropertyName">Name of the raster property.</param>
        public RasterWmsItem(string schemaName, string className, string rasterPropertyName)
        {
            this.SchemaName = schemaName;
            this.FeatureClass = className;
            this.RasterPropertyName = rasterPropertyName;
        }

        /// <summary>
        /// Gets the name of the FDO logical schema this particular override applies to
        /// </summary>
        public string SchemaName { get; internal set; }

        /// <summary>
        /// Gets or sets the feature class.
        /// </summary>
        /// <value>
        /// The feature class.
        /// </value>
        public string FeatureClass { get; set; }

        /// <summary>
        /// Gets or sets the name of the raster property.
        /// </summary>
        /// <value>
        /// The name of the raster property.
        /// </value>
        public string RasterPropertyName { get; set; }

        /// <summary>
        /// Gets or sets the elevation dimension.
        /// </summary>
        /// <value>
        /// The elevation dimension.
        /// </value>
        public string ElevationDimension { get; set; }

        /// <summary>
        /// Gets or sets the image format.
        /// </summary>
        /// <value>
        /// The image format.
        /// </value>
        public string ImageFormat { get; set; }

        private List<WmsLayerDefinition> _layers = new List<WmsLayerDefinition>();

        /// <summary>
        /// Gets the array of WMS layer configuration elements
        /// </summary>
        public WmsLayerDefinition[] Layers { get { return _layers.ToArray(); } }

        /// <summary>
        /// Adds a WMS layer configuration element.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public void AddLayer(WmsLayerDefinition layer) { _layers.Add(layer); }

        /// <summary>
        /// Removes the given WMS layer configuration element
        /// </summary>
        /// <param name="layer">The layer.</param>
        public void RemoveLayer(WmsLayerDefinition layer) { _layers.Remove(layer); }

        /// <summary>
        /// Gets or sets a value indicating whether tile caching is used
        /// </summary>
        /// <value>
        ///   <c>true</c> if tile caching is used; otherwise, <c>false</c>.
        /// </value>
        public bool UseTileCache { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the name of the spatial context.
        /// </summary>
        /// <value>
        /// The name of the spatial context.
        /// </value>
        public string SpatialContextName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is transparent.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is transparent; otherwise, <c>false</c>.
        /// </value>
        public bool IsTransparent { get; set; }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>
        /// The color of the background.
        /// </value>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Writes the current element's content
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var rasterDef = doc.CreateElement("RasterDefinition"); //NOXLATE
            var n = doc.CreateAttribute("name"); //NOXLATE
            n.Value = this.RasterPropertyName;
            rasterDef.Attributes.Append(n);
            {
                var format = doc.CreateElement("Format"); //NOXLATE
                format.InnerText = this.ImageFormat;

                var mimeType = doc.CreateElement("FormatType"); //NOXLATE
                if (!string.IsNullOrEmpty(this.ImageFormat))
                {
                    switch (this.ImageFormat)
                    {
                        case WmsImageFormat.GIF:
                            mimeType.InnerText = "image/gif"; //NOXLATE
                            break;
                        case WmsImageFormat.JPG:
                            mimeType.InnerText = "image/jpeg"; //NOXLATE
                            break;
                        case WmsImageFormat.PNG:
                            mimeType.InnerText = "image/png"; //NOXLATE
                            break;
                        case WmsImageFormat.TIF:
                            mimeType.InnerText = "image/tiff"; //NOXLATE
                            break;
                    }
                }

                var transparent = doc.CreateElement("Transparent"); //NOXLATE
                transparent.InnerText = this.IsTransparent ? "true" : "false"; //NOXLATE

                var bgcolor = doc.CreateElement("BackgroundColor"); //NOXLATE
                bgcolor.InnerText = "0x" + Utility.SerializeHTMLColor(this.BackgroundColor, false); //NOXLATE

                var useTileCache = doc.CreateElement("UseTileCache"); //NOXLATE
                useTileCache.InnerText = this.UseTileCache ? "true" : "false"; //NOXLATE

                var time = doc.CreateElement("Time"); //NOXLATE
                time.InnerText = this.Time;

                var elevation = doc.CreateElement("Elevation"); //NOXLATE
                elevation.InnerText = this.ElevationDimension;

                var sc = doc.CreateElement("SpatialContext"); //NOXLATE
                sc.InnerText = this.SpatialContextName;

                rasterDef.AppendChild(format);
                rasterDef.AppendChild(mimeType);
                rasterDef.AppendChild(transparent);
                rasterDef.AppendChild(useTileCache);
                rasterDef.AppendChild(bgcolor);
                rasterDef.AppendChild(time);
                rasterDef.AppendChild(elevation);
                rasterDef.AppendChild(sc);

                foreach (var layer in this.Layers)
                {
                    layer.WriteXml(doc, rasterDef);
                }
            };

            currentNode.AppendChild(rasterDef);
        }

        /// <summary>
        /// Set the current element's content from the current XML node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mgr"></param>
        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (node.Name != "RasterDefinition") //NOXLATE
                throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedElement, "RasterDefinition"));

            var fc = node.ParentNode.Attributes["name"].Value; //NOXLATE
            this.FeatureClass = Utility.DecodeFDOName(fc.Substring(0, fc.Length - "Type".Length)); //NOXLATE
            this.RasterPropertyName = node.Attributes["name"].Value; //NOXLATE

            var format = node["Format"]; //NOXLATE
            var transparent = node["Transparent"]; //NOXLATE
            var useTileCache = node["UseTileCache"]; //NOXLATE
            var bgcolor = node["BackgroundColor"]; //NOXLATE
            var time = node["Time"]; //NOXLATE
            var elevation = node["Elevation"]; //NOXLATE
            var sc = node["SpatialContext"]; //NOXLATE

            if (format != null)
                this.ImageFormat = format.InnerText;

            if (transparent != null)
                this.IsTransparent = (transparent.InnerText.ToLower() == "true"); //NOXLATE

            if (useTileCache != null)
                this.UseTileCache = (useTileCache.InnerText.ToLower() == "true"); //NOXLATE

            if (bgcolor != null)
            {
                if (!string.IsNullOrEmpty(bgcolor.InnerText))
                {
                    if (bgcolor.InnerText.StartsWith("0x")) //NOXLATE
                        this.BackgroundColor = ColorTranslator.FromHtml("#" + bgcolor.InnerText.Substring(2)); //NOXLATE
                    else
                        this.BackgroundColor = ColorTranslator.FromHtml("#" + bgcolor.InnerText); //NOXLATE
                }
                else 
                {
                    this.BackgroundColor = default(Color);
                }
            }

            if (time != null)
                this.Time = time.InnerText;

            if (elevation != null)
                this.ElevationDimension = elevation.InnerText;

            if (sc != null)
                this.SpatialContextName = sc.InnerText;

            foreach (XmlNode ln in node.ChildNodes)
            {
                if (ln.Name == "Layer") //NOXLATE
                {
                    var layer = new WmsLayerDefinition();
                    layer.ReadXml(ln, mgr);

                    this.AddLayer(layer);
                }
            }
        }

        /// <summary>
        /// Removes all WMS layer configuration elements
        /// </summary>
        public void RemoveAllLayers()
        {
            _layers.Clear();
        }
    }
}
