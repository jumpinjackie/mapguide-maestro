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
using System.Xml.Serialization;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System.Globalization;

namespace OSGeo.MapGuide.ObjectModels.Common
{
    /// <summary>
    /// Represents a Spatial Context of a Feature Source
    /// </summary>
    public interface IFdoSpatialContext : IFdoSerializable
    {
        string CoordinateSystemName { get; set; }

        string CoordinateSystemWkt { get; set; }

        string Description { get; set; }

        IEnvelope Extent { get; set; }

        FdoSpatialContextListSpatialContextExtentType ExtentType { get; set; }

        string Name { get; set; }

        double XYTolerance { get; set; }

        double ZTolerance { get; set; }
    }

    partial class FdoSpatialContextListSpatialContext : IFdoSpatialContext
    {
        [XmlIgnore]
        IEnvelope IFdoSpatialContext.Extent
        {
            get
            {
                double llx;
                double lly;
                double urx;
                double ury;

                if (double.TryParse(this.Extent.LowerLeftCoordinate.X, out llx) &&
                    double.TryParse(this.Extent.LowerLeftCoordinate.Y, out lly) &&
                    double.TryParse(this.Extent.UpperRightCoordinate.X, out urx) &&
                    double.TryParse(this.Extent.UpperRightCoordinate.Y, out ury))
                    return ObjectFactory.CreateEnvelope(llx, lly, urx, ury);

                return null;
            }
            set
            {
                if (value == null)
                {
                    this.Extent = null;
                    return;
                }

                this.Extent = new FdoSpatialContextListSpatialContextExtent()
                {
                    LowerLeftCoordinate = new FdoSpatialContextListSpatialContextExtentLowerLeftCoordinate()
                    {
                        X = value.MinX.ToString(CultureInfo.InvariantCulture),
                        Y = value.MinY.ToString(CultureInfo.InvariantCulture)
                    },
                    UpperRightCoordinate = new FdoSpatialContextListSpatialContextExtentUpperRightCoordinate()
                    {
                        X = value.MaxX.ToString(CultureInfo.InvariantCulture),
                        Y = value.MaxY.ToString(CultureInfo.InvariantCulture)
                    }
                };
            }
        }

        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            //Can't write dynamic extents
            if (this.ExtentType == FdoSpatialContextListSpatialContextExtentType.Dynamic)
                return;

            var crs = doc.CreateElement("gml", "DerivedCRS", XmlNamespaces.GML);
            {
                crs.SetAttribute("id", this.Name);
                var meta = doc.CreateElement("gml", "metaDataProperty", XmlNamespaces.GML);
                crs.AppendChild(meta);
                {
                    var genMeta = doc.CreateElement("gml", "GenericMetaData", XmlNamespaces.GML);
                    meta.AppendChild(genMeta);
                    {
                        var fdoXY = doc.CreateElement("fdo", "XYTolerance", XmlNamespaces.FDO);
                        var fdoZ = doc.CreateElement("fdo", "ZTolerance", XmlNamespaces.FDO);
                        fdoXY.InnerText = this.XYTolerance.ToString(CultureInfo.InvariantCulture);
                        fdoZ.InnerText = this.ZTolerance.ToString(CultureInfo.InvariantCulture);
                        genMeta.AppendChild(fdoXY);
                        genMeta.AppendChild(fdoZ);
                    }
                }
                
                var remarks = doc.CreateElement("gml", "remarks", XmlNamespaces.GML);
                remarks.InnerText = this.Description;
                crs.AppendChild(remarks);

                var csName = doc.CreateElement("gml", "srsName", XmlNamespaces.GML);
                csName.InnerText = string.IsNullOrEmpty(this.CoordinateSystemName) ? this.Name : this.CoordinateSystemName;
                crs.AppendChild(csName);

                var ext = doc.CreateElement("gml", "validArea", XmlNamespaces.GML);
                {
                    var bbox = doc.CreateElement("gml", "boundingBox", XmlNamespaces.GML);
                    {
                        var ll = doc.CreateElement("gml", "pos", XmlNamespaces.GML);
                        var ur = doc.CreateElement("gml", "pos", XmlNamespaces.GML);
                        ll.InnerText = this.Extent.LowerLeftCoordinate.X + " " + this.Extent.LowerLeftCoordinate.Y;
                        ur.InnerText = this.Extent.UpperRightCoordinate.X + " " + this.Extent.UpperRightCoordinate.Y;
                        bbox.AppendChild(ll);
                        bbox.AppendChild(ur);
                    }
                    ext.AppendChild(bbox);
                }
                crs.AppendChild(ext);

                var baseCrs = doc.CreateElement("gml", "baseCRS", XmlNamespaces.GML);
                var definedBy = doc.CreateElement("gml", "definedByConversion", XmlNamespaces.GML);
                var derivedCrs = doc.CreateElement("gml", "derivedCRSType", XmlNamespaces.GML);
                var userCs = doc.CreateElement("gml", "usesCS", XmlNamespaces.GML);

                if (string.IsNullOrEmpty(this.CoordinateSystemWkt))
                {
                    baseCrs.SetAttribute("href", XmlNamespaces.XLINK, "http://fdo.osgeo.org/schemas/feature/crs/#" + (string.IsNullOrEmpty(this.CoordinateSystemName) ? this.Name : this.CoordinateSystemName));
                }
                else
                {
                    var wktCRS = doc.CreateElement("fdo", "WKTCRS", XmlNamespaces.FDO);
                    wktCRS.SetAttribute("id", XmlNamespaces.GML, this.Name);
                    {
                        var srsName = doc.CreateElement("gml", "srsName", XmlNamespaces.GML);
                        srsName.InnerText = this.nameField;
                        var fdowkt = doc.CreateElement("fdo", "WKT", XmlNamespaces.FDO);
                        fdowkt.InnerText = this.CoordinateSystemWkt;
                        wktCRS.AppendChild(srsName);
                        wktCRS.AppendChild(fdowkt);
                    }
                    baseCrs.AppendChild(wktCRS);
                }

                definedBy.SetAttribute("href", XmlNamespaces.XLINK, "http://fdo.osgeo.org/coord_conversions#identity");
                derivedCrs.SetAttribute("codeSpace", "http://fdo.osgeo.org/crs_types");
                derivedCrs.InnerText = "geographic";
                userCs.SetAttribute("href", XmlNamespaces.XLINK, "http://fdo.osgeo.org/cs#default_cartesian");

                crs.AppendChild(baseCrs);
                crs.AppendChild(definedBy);
                crs.AppendChild(derivedCrs);
                crs.AppendChild(userCs);
            }

            currentNode.AppendChild(crs);
        }

        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (!node.Name.Equals("gml:DerivedCRS"))
                throw new Exception("Bad document. Expected element gml:DerivedCRS"); //LOCALIZEME

            var meta = node["gml:metaDataProperty"];
            var genMeta = meta["gml:GenericMetaData"];

            var scType = genMeta["fdo:SCExtentType"];
            var xyTol = genMeta["fdo:XYTolerance"];
            var zTol = genMeta["fdo:ZTolerance"];

            this.ExtentType = (scType == null || scType.InnerText == "dynamic") ? FdoSpatialContextListSpatialContextExtentType.Dynamic : FdoSpatialContextListSpatialContextExtentType.Static;

            double xy_tol;
            double z_tol;

            if (double.TryParse(xyTol.InnerText, out xy_tol))
                this.XYTolerance = xy_tol;

            if (double.TryParse(zTol.InnerText, out z_tol))
                this.ZTolerance = z_tol;

            var remarks = node["gml:remarks"];
            var srsName = node["gml:srsName"];
            var ext = node["gml:validArea"];
            var baseCrs = node["gml:baseCRS"];

            //Anything we read in *must* be static!
            this.ExtentType = FdoSpatialContextListSpatialContextExtentType.Static;
            this.Name = srsName.InnerText;
            this.Description = remarks.InnerText;

            var bbox = ext["gml:boundingBox"];
            var ll = bbox.FirstChild;
            var ur = bbox.LastChild;

            var llt = ll.InnerText.Split(' ');
            var urt = ur.InnerText.Split(' ');

            if (llt.Length != 2 || urt.Length != 2)
                throw new Exception("Bad document. Invalid bounding box"); //LOCALIZEME

            this.Extent = new FdoSpatialContextListSpatialContextExtent()
            {
                LowerLeftCoordinate = new FdoSpatialContextListSpatialContextExtentLowerLeftCoordinate()
                {
                    X = llt[0],
                    Y = llt[1]
                },
                UpperRightCoordinate = new FdoSpatialContextListSpatialContextExtentUpperRightCoordinate()
                {
                    X = urt[0],
                    Y = urt[1]
                }
            };

            if (baseCrs.HasAttribute("xlink:href"))
            {
                var href = baseCrs.GetAttribute("xlink:href");
                this.CoordinateSystemName = href.Substring(href.LastIndexOf("#") + 1);
            }

            if (baseCrs["fdo:WKTCRS"] != null)
            {
                if (baseCrs["fdo:WKTCRS"]["fdo:WKT"] != null)
                {
                    this.CoordinateSystemWkt = baseCrs["fdo:WKTCRS"]["fdo:WKT"].InnerText;
                }
            }
        }
    }
}
