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
using System.IO;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace OSGeo.MapGuide.MaestroAPI.SchemaOverrides
{
    public class RasterFileItem : RasterItem
    {
        public RasterFileItem(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public IPoint2D InsertionPoint { get; set; }

        public IPoint2D Resolution { get; set; }

        public IPoint2D Rotation { get; set; }

        public void SetPosition(double insertionX, double insertionY,
                                double resolutionX, double resolutionY,
                                double rotationX, double rotationY)
        {
            this.InsertionPoint = ObjectFactory.CreatePoint2D(insertionX, insertionY);
            this.Resolution = ObjectFactory.CreatePoint2D(resolutionX, resolutionY);
            this.Rotation = ObjectFactory.CreatePoint2D(rotationX, rotationY);
        }

        public bool HasPosition
        {
            get
            {
                return this.InsertionPoint != null &&
                       this.Resolution != null &&
                       this.Rotation != null;
            }
        }

        public override void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var ctype = doc.CreateElement("complexType");
            ctype.SetAttribute("name", this.ClassName + "Type");
            {
                var rdf = doc.CreateElement("RasterDefinition");
                rdf.SetAttribute("name", this.ClassName);
                {
                    var loc = doc.CreateElement("Location");
                    loc.SetAttribute("name", Path.GetDirectoryName(this.Name));
                    {
                        var feat = doc.CreateElement("Feature");
                        feat.SetAttribute("name", Path.GetFileNameWithoutExtension(this.Name));
                        {
                            var band = doc.CreateElement("Band");
                            band.SetAttribute("name", "RGB");
                            band.SetAttribute("number", "1");
                            {
                                var img = doc.CreateElement("Image");
                                img.SetAttribute("name", Path.GetFileName(this.Name));
                                band.AppendChild(img);
                                
                                //Set bounds if spatial context is defined
                                if (this.Parent != null)
                                {
                                    var sc = this.Parent.GetSpatialContext(this.SpatialContextName);
                                    if (sc != null)
                                    {
                                        var bounds = doc.CreateElement("Bounds");
                                        bounds.InnerXml = string.Format("<MinX>{0}</MinX><MinY>{1}</MinY><MaxX>{2}</MaxX><MaxY>{3}</MaxY>",
                                            sc.Extent.MinX,
                                            sc.Extent.MinY,
                                            sc.Extent.MaxX,
                                            sc.Extent.MaxY);

                                        band.AppendChild(bounds);
                                    }
                                }

                                //Set georeference if position is defined
                                if (this.HasPosition)
                                {
                                    var georef = doc.CreateElement("Georeference");
                                    georef.InnerXml = string.Format("<InsertionPointX>{0}</InsertionPointX><InsertionPointY>{1}</InsertionPointY><ResolutionX>{2}</ResolutionX><ResolutionY>{3}</ResolutionY><RotationX>{4}</RotationX><RotationY>{5}</RotationY>",
                                        this.InsertionPoint.X, this.InsertionPoint.Y,
                                        this.Resolution.X, this.Resolution.Y,
                                        this.Rotation.X, this.Rotation.Y);

                                    band.AppendChild(georef);
                                }
                            }
                            feat.AppendChild(band);
                        }
                        loc.AppendChild(feat);
                    }
                    rdf.AppendChild(loc);
                }
                ctype.AppendChild(rdf);
            }
            currentNode.AppendChild(ctype);
        }

        public override void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            var ctype = node.SelectSingleNode("ctype");
            var sn = node.Attributes["name"];
            var cn = ctype.Attributes["name"];

            if (sn == null || cn == null)
                throw new Exception("Bad document. Expected attribute: name"); //LOCALIZEME

            this.FeatureClassName = sn.Value + ":" + cn.Value.Substring(0, cn.Value.Length - "Type".Length);

            var loc = node.SelectSingleNode("complexType/RasterDefinition/Location");
            var img = node.SelectSingleNode("complexType/RasterDefinition/Location/Feature/Band/Image");
            var georef = node.SelectSingleNode("complexType/RasterDefinition/Location/Feature/Band/Georeference");

            if (loc == null)
                throw new Exception("Bad document. Expected element: Location"); //LOCALIZEME

            if (img == null)
                throw new Exception("Bad document. Expected element: Image"); //LOCALIZEME

            //Set spatial context
            var cls = this.Parent.GetClass(this.SchemaName, this.ClassName);
            if (cls != null)
            {
                if (!string.IsNullOrEmpty(cls.DefaultGeometryPropertyName))
                {
                    var geom = cls.FindProperty(cls.DefaultGeometryPropertyName) as GeometricPropertyDefinition;
                    if (geom != null && !string.IsNullOrEmpty(geom.SpatialContextAssociation))
                        this.SpatialContextName = geom.SpatialContextAssociation;
                }
            }

            //Construct image path
            var locName = loc.Attributes["name"];
            var imgName = loc.Attributes["name"];

            if (locName == null || imgName == null)
                throw new Exception("Bad document. Expected attribute: name"); //LOCALIZEME

            this.Name = Path.Combine(locName.Value, imgName.Value);

            //Set georeferencing information
            if (georef != null)
            {
                double insX = 0.0;
                double insY = 0.0;
                double rotX = 0.0;
                double rotY = 0.0;
                double resX = 0.0;
                double resY = 0.0;

                if (georef["InsertionPointX"] != null)
                {
                    if (!double.TryParse(georef["InsertionPointX"].InnerText, out insX))
                        throw new Exception("Invalid value for InsertionPointX"); //LOCALIZEME
                }

                if (georef["InsertionPointY"] != null)
                {
                    if (!double.TryParse(georef["InsertionPointY"].InnerText, out insY))
                        throw new Exception("Invalid value for InsertionPointY"); //LOCALIZEME
                }

                if (georef["ResolutionX"] != null)
                {
                    if (!double.TryParse(georef["ResolutionX"].InnerText, out resX))
                        throw new Exception("Invalid value for ResolutionX"); //LOCALIZEME
                }

                if (georef["ResolutionY"] != null)
                {
                    if (!double.TryParse(georef["ResolutionY"].InnerText, out resY))
                        throw new Exception("Invalid value for ResolutionY"); //LOCALIZEME
                }

                if (georef["RotationX"] != null)
                {
                    if (!double.TryParse(georef["RotationX"].InnerText, out rotX))
                        throw new Exception("Invalid value for RotationX"); //LOCALIZEME
                }

                if (georef["RotationY"] != null)
                {
                    if (!double.TryParse(georef["RotationY"].InnerText, out rotY))
                        throw new Exception("Invalid value for RotationY"); //LOCALIZEME
                }

                this.SetPosition(insX, insY, resX, resY, rotX, rotY);
            }
        }
    }
}
