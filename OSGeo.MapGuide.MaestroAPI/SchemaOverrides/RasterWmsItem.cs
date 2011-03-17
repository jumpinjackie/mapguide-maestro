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
    public class RasterWmsItem : IFdoSerializable
    {
        public class WmsImageFormat
        {
            public const string PNG = "PNG";
            public const string TIF = "TIF";
            public const string JPG = "JPG";
            public const string GIF = "GIF";
        }

        internal RasterWmsItem() { }

        public RasterWmsItem(string className, string rasterPropertyName)
        {
            this.FeatureClass = className;
            this.RasterPropertyName = rasterPropertyName;
        }

        public string FeatureClass { get; set; }

        public string RasterPropertyName { get; set; }

        public string ElevationDimension { get; set; }

        public string ImageFormat { get; set; }

        private List<WmsLayerDefinition> _layers = new List<WmsLayerDefinition>();

        public WmsLayerDefinition[] Layers { get { return _layers.ToArray(); } }

        public void AddLayer(WmsLayerDefinition layer) { _layers.Add(layer); }

        public void RemoveLayer(WmsLayerDefinition layer) { _layers.Remove(layer); }

        public string Time { get; set; }

        public string SpatialContextName { get; set; }

        public bool IsTransparent { get; set; }

        public Color BackgroundColor { get; set; }

        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var rasterDef = doc.CreateElement("RasterDefinition");
            var n = doc.CreateAttribute("name");
            n.Value = this.RasterPropertyName;
            rasterDef.Attributes.Append(n);
            {
                var format = doc.CreateElement("Format");
                format.InnerText = this.ImageFormat;

                var transparent = doc.CreateElement("Transparent");
                transparent.InnerText = this.IsTransparent ? "true" : "false";

                var bgcolor = doc.CreateElement("BackgroundColor");
                bgcolor.InnerText = "0x" + Utility.SerializeHTMLColor(this.BackgroundColor, false);

                var time = doc.CreateElement("Time");
                time.InnerText = this.Time;

                var elevation = doc.CreateElement("Elevation");
                elevation.InnerText = this.ElevationDimension;

                var sc = doc.CreateElement("SpatialContext");
                sc.InnerText = this.SpatialContextName;

                rasterDef.AppendChild(format);
                rasterDef.AppendChild(transparent);
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

        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (node.Name != "RasterDefinition")
                throw new Exception("Bad document. Expected element: RasterDefinition");

            var fc = node.ParentNode.Attributes["name"].Value;
            this.FeatureClass = fc.Substring(0, fc.Length - "Type".Length);

            var format = node["Format"];
            var transparent = node["Transparent"];
            var bgcolor = node["BackgroundColor"];
            var time = node["Time"];
            var elevation = node["Elevation"];
            var sc = node["SpatialContext"];

            if (format != null)
                this.ImageFormat = format.InnerText;

            if (transparent != null)
                this.IsTransparent = (transparent.InnerText.ToLower() == "true");

            if (bgcolor != null)
            {
                if (bgcolor.InnerText.StartsWith("0x"))
                    this.BackgroundColor = ColorTranslator.FromHtml("#" + bgcolor.InnerText.Substring(2));
                else
                    this.BackgroundColor = ColorTranslator.FromHtml("#" + bgcolor.InnerText);
            }

            if (time != null)
                this.Time = time.InnerText;

            if (elevation != null)
                this.ElevationDimension = elevation.InnerText;

            if (sc != null)
                this.SpatialContextName = sc.InnerText;

            foreach (XmlNode ln in node.ChildNodes)
            {
                if (ln.Name == "Layer")
                {
                    var layer = new WmsLayerDefinition();
                    layer.ReadXml(ln, mgr);

                    this.AddLayer(layer);
                }
            }
        }

        public void RemoveAllLayers()
        {
            _layers.Clear();
        }
    }
}
