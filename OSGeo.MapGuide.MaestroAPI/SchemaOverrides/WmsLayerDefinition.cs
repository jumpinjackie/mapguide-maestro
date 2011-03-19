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

namespace OSGeo.MapGuide.MaestroAPI.SchemaOverrides
{
    public class WmsLayerDefinition : IFdoSerializable
    {
        public WmsLayerDefinition() { }

        public WmsLayerDefinition(string layerName) { this.Name = layerName; }

        public string Name { get; set; }

        public string Style { get; set; }

        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var layer = doc.CreateElement("Layer");
            var n = doc.CreateAttribute("name");
            n.Value = this.Name;
            layer.Attributes.Append(n);
            {
                var style = doc.CreateElement("Style");
                var s = doc.CreateAttribute("name");
                s.Value = this.Style;
                style.Attributes.Append(s);
                layer.AppendChild(style);
            }
            currentNode.AppendChild(layer);
        }

        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (node.Name != "Layer")
                throw new Exception("Bad document. Expected element: Layer");

            var n = node.Attributes["name"];
            if (n == null)
                throw new Exception("Bad document. Expected attribute: name");

            var style = node.FirstChild;
            if (style != null)
            {
                var s = style.Attributes["name"];
                if (s != null)
                {
                    this.Style = s.Value;
                }
            }

            this.Name = n.Value;
        }
    }
}
