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
    /// <summary>
    /// Has the information needed to create or completely describe a raster property. This class encapsulates the information 
    /// necessary to insert a 'new' raster, in the absence of any other information, for the properties defined using this schema element. 
    /// </summary>
    public class RasterPropertyDefinition : PropertyDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RasterPropertyDefinition"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public RasterPropertyDefinition(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the default size of image file in the horizontal direction in pixels (number of columns). 
        /// </summary>
        public int DefaultImageXSize { get; set; }

        /// <summary>
        /// Gets or sets the default size of an image file in the vertical direction in pixels (number of rows). 
        /// </summary>
        public int DefaultImageYSize { get; set; }

        /// <summary>
        /// Gets or sets whether this property's value can be null. 
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets whether this property is read-only. 
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the Spatial Context name associated to this raster property. 
        /// </summary>
        public string SpatialContextAssociation { get; set; }

        /// <summary>
        /// Gets the type of property definition
        /// </summary>
        public override PropertyDefinitionType Type
        {
            get { return PropertyDefinitionType.Raster; }
        }

        /// <summary>
        /// Writes the current element's content
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        public override void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var en = Utility.EncodeFDOName(this.Name);

            var geom = doc.CreateElement("xs", "element", XmlNamespaces.XS); //NOXLATE
            
            geom.SetAttribute("name", en); //NOXLATE
            geom.SetAttribute("type", "fdo:RasterPropertyType"); //NOXLATE
            geom.SetAttribute("defaultImageXSize", XmlNamespaces.FDO, this.DefaultImageXSize.ToString()); //NOXLATE
            geom.SetAttribute("defaultImageYSize", XmlNamespaces.FDO, this.DefaultImageYSize.ToString()); //NOXLATE
            geom.SetAttribute("srsName", XmlNamespaces.FDO, this.SpatialContextAssociation); //NOXLATE

            //Write description node
            var anno = doc.CreateElement("xs", "annotation", XmlNamespaces.XS); //NOXLATE
            var docN = doc.CreateElement("xs", "documentation", XmlNamespaces.XS); //NOXLATE
            docN.InnerText = this.Description;
            geom.AppendChild(anno);
            anno.AppendChild(docN);

            currentNode.AppendChild(geom);
        }

        /// <summary>
        /// Set the current element's content from the current XML node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mgr"></param>
        public override void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            var dix = Utility.GetFdoAttribute(node, "defaultImageXSize"); //NOXLATE
            var diy = Utility.GetFdoAttribute(node, "defaultImageYSize"); //NOXLATE
            var srs = Utility.GetFdoAttribute(node, "srsName"); //NOXLATE
            var ro = Utility.GetFdoAttribute(node, "readOnly"); //NOXLATE

            this.DefaultImageXSize = Convert.ToInt32(dix.Value);
            this.DefaultImageYSize = Convert.ToInt32(diy.Value);

            //TODO: Just copypasta'd from DataPropertyDefinition assuming the same attributes would be used 
            //to indicate nullability and read-only states. Would be nice to verify with an actual example property
            this.IsNullable = (node.Attributes["minOccurs"] != null && node.Attributes["minOccurs"].Value == "0"); //NOXLATE
            this.IsReadOnly = (ro != null && ro.Value == "true"); //NOXLATE

            this.SpatialContextAssociation = (srs != null ? srs.Value : string.Empty);
        }

        /// <summary>
        /// Gets the expression data type
        /// </summary>
        public override OSGeo.MapGuide.ObjectModels.Common.ExpressionDataType ExpressionType
        {
            get { return OSGeo.MapGuide.ObjectModels.Common.ExpressionDataType.Raster; }
        }
    }
}
