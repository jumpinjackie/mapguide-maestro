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
    /// <summary>
    /// WMS Layer Definition configuration element
    /// </summary>
    public class WmsLayerDefinition : IFdoSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WmsLayerDefinition"/> class.
        /// </summary>
        public WmsLayerDefinition() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WmsLayerDefinition"/> class.
        /// </summary>
        /// <param name="layerName">Name of the layer.</param>
        public WmsLayerDefinition(string layerName) { this.Name = layerName; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>
        /// The style.
        /// </value>
        public string Style { get; set; }

        /// <summary>
        /// Writes the current element's content
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var layer = doc.CreateElement("Layer"); //NOXLATE
            var n = doc.CreateAttribute("name"); //NOXLATE
            n.Value = this.Name;
            layer.Attributes.Append(n);
            {
                var style = doc.CreateElement("Style"); //NOXLATE
                var s = doc.CreateAttribute("name"); //NOXLATE
                s.Value = this.Style;
                style.Attributes.Append(s);
                layer.AppendChild(style);
            }
            currentNode.AppendChild(layer);
        }

        /// <summary>
        /// Set the current element's content from the current XML node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mgr"></param>
        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (node.Name != "Layer") //NOXLATE
                throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedElement, "Layer")); //NOXLATE

            var n = node.Attributes["name"]; //NOXLATE
            if (n == null)
                throw new Exception(string.Format(Strings.ErrorBadDocumentExpectedAttribute, "name")); //NOXLATE

            var style = node.FirstChild;
            if (style != null)
            {
                var s = style.Attributes["name"]; //NOXLATE
                if (s != null)
                {
                    this.Style = s.Value;
                }
            }

            this.Name = n.Value;
        }
    }
}
