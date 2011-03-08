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

namespace OSGeo.MapGuide.MaestroAPI.Schema
{
    public class RasterPropertyDefinition : PropertyDefinition
    {
        public RasterPropertyDefinition(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public int DefaultImageXSize { get; set; }

        public int DefaultImageYSize { get; set; }

        public bool IsNullable { get; set; }

        public bool IsReadOnly { get; set; }

        public string SpatialContextAssociation { get; set; }

        public override PropertyDefinitionType Type
        {
            get { return PropertyDefinitionType.Raster; }
        }

        public override void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var geom = doc.CreateElement("xs", "element", XmlNamespaces.XS);
            geom.SetAttribute("name", this.Name); //TODO: This may have been decoded. Should it be re-encoded?
            geom.SetAttribute("type", "fdo:RasterPropertyType");
            geom.SetAttribute("defaultImageXSize", XmlNamespaces.FDO, this.DefaultImageXSize.ToString());
            geom.SetAttribute("defaultImageYSize", XmlNamespaces.FDO, this.DefaultImageYSize.ToString());
            geom.SetAttribute("srsName", XmlNamespaces.FDO, this.SpatialContextAssociation);

            currentNode.AppendChild(geom);
        }

        public override void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            var dix = node.Attributes["fdo:defaultImageXSize"];
            var diy = node.Attributes["fdo:defaultImageYSize"];
            var srs = node.Attributes["fdo:srsName"];

            this.DefaultImageXSize = Convert.ToInt32(dix.Value);
            this.DefaultImageYSize = Convert.ToInt32(diy.Value);

            //TODO: Just copypasta'd from DataPropertyDefinition assuming the same attributes would be used 
            //to indicate nullability and read-only states. Would be nice to verify with an actual example property
            this.IsNullable = (node.Attributes["minOccurs"] != null && node.Attributes["minOccurs"].Value == "0");
            this.IsReadOnly = (node.Attributes["fdo:readOnly"] != null && node.Attributes["fdo:readOnly"].Value == "true");

            this.SpatialContextAssociation = (srs != null ? srs.Value : string.Empty);
        }
    }
}
