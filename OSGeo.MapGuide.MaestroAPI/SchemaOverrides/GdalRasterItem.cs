#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.IO;

namespace OSGeo.MapGuide.MaestroAPI.SchemaOverrides
{
    public class GdalRasterLocationItem : IFdoSerializable
    {
        private List<GdalRasterItem> _items = new List<GdalRasterItem>();

        public string Location { get; set; }

        public void AddItem(GdalRasterItem item) { _items.Add(item); }

        public void RemoveItem(GdalRasterItem item) { _items.Remove(item); }

        public GdalRasterItem[] Items { get { return _items.ToArray(); } }

        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var loc = doc.CreateElement("Location");
            var locName = doc.CreateAttribute("name");
            locName.Value = this.Location;
            loc.Attributes.Append(locName);

            foreach (var item in _items)
            {
                item.WriteXml(doc, loc);   
            }

            currentNode.AppendChild(loc);
        }

        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (node.Name != "Location")
                throw new Exception("Bad document. Expected element: Location");

            var loc = node.Attributes["name"];
            this.Location = loc.Value;

            foreach (System.Xml.XmlNode item in node.ChildNodes)
            {
                var raster = new GdalRasterItem();
                raster.ReadXml(item, mgr);

                AddItem(raster);
            }
        }

        public void RemoveItem(string fileName)
        {
            GdalRasterItem item = null;
            foreach (var raster in _items)
            {
                if (raster.FileName == fileName)
                {
                    item = raster;
                }
            }

            if (item != null)
                _items.Remove(item);
        }
    }

    public class GdalRasterItem : IFdoSerializable
    {
        public string FileName { get; set; }

        public double MinX { get; set; }

        public double MinY { get; set; }

        public double MaxX { get; set; }

        public double MaxY { get; set; }

        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var feat = doc.CreateElement("Feature");

            var featName = doc.CreateAttribute("name");
            featName.Value = Path.GetFileNameWithoutExtension(this.FileName);

            feat.Attributes.Append(featName);
            {
                var band = doc.CreateElement("Band");
                var bandName = doc.CreateAttribute("name");
                bandName.Value = "RGB";
                var bandNo = doc.CreateAttribute("number");
                bandNo.Value = "1";

                band.Attributes.Append(bandName);
                band.Attributes.Append(bandNo);
                {
                    var img = doc.CreateElement("Image");
                    var imgFrame = doc.CreateAttribute("frame");
                    imgFrame.Value = "1";
                    var imgName = doc.CreateAttribute("name");
                    imgName.Value = this.FileName;

                    img.Attributes.Append(imgFrame);
                    img.Attributes.Append(imgName);
                    {
                        var bounds = doc.CreateElement("Bounds");
                        bounds.InnerXml = string.Format("<MinX>{0}</MinX><MinY>{1}</MinY><MaxX>{2}</MaxX><MaxY>{3}</MaxY>", this.MinX, this.MinY, this.MaxX, this.MaxY);

                        img.AppendChild(bounds);
                    }
                    band.AppendChild(img);
                }
                feat.AppendChild(band);
            }
            currentNode.AppendChild(feat);
        }

        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (node.Name != "Feature")
                throw new Exception("Bad document. Expected element: Feature");

            var band = node.FirstChild;
            var image = band.FirstChild;
            var bounds = image.FirstChild;

            if (band.Name != "Band")
                throw new Exception("Bad document. Expected element: Band");

            if (image.Name != "Image")
                throw new Exception("Bad document. Expected element: Image");

            if (bounds.Name != "Bounds")
                throw new Exception("Bad document. Expected element: Bounds");

            var fileName = image.Attributes["name"];
            this.FileName = fileName.Value;

            var minx = bounds.ChildNodes[0];
            var miny = bounds.ChildNodes[1];
            var maxx = bounds.ChildNodes[2];
            var maxy = bounds.ChildNodes[3];

            if (minx.Name != "MinX")
                throw new Exception("Bad document. Expected element: MinX");

            if (miny.Name != "MinY")
                throw new Exception("Bad document. Expected element: MinY");

            if (maxx.Name != "MaxX")
                throw new Exception("Bad document. Expected element: MaxX");

            if (maxy.Name != "MaxY")
                throw new Exception("Bad document. Expected element: MaxY");

            this.MinX = Convert.ToDouble(minx.InnerText);
            this.MinY = Convert.ToDouble(miny.InnerText);
            this.MaxX = Convert.ToDouble(maxx.InnerText);
            this.MaxY = Convert.ToDouble(maxy.InnerText);
        }
    }
}
