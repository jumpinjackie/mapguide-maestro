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
#pragma warning disable 1591, 0114, 0108, 0114, 0108
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System.Globalization;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.ObjectModels.Common
{
    /// <summary>
    /// Represents a Spatial Context of a Feature Source
    /// </summary>
    public interface IFdoSpatialContext : IFdoSerializable
    {
        /// <summary>
        /// Gets or sets the name of the coordinate system.
        /// </summary>
        /// <value>
        /// The name of the coordinate system.
        /// </value>
        string CoordinateSystemName { get; set; }

        /// <summary>
        /// Gets or sets the coordinate system WKT.
        /// </summary>
        /// <value>
        /// The coordinate system WKT.
        /// </value>
        string CoordinateSystemWkt { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the extent.
        /// </summary>
        /// <value>
        /// The extent.
        /// </value>
        IEnvelope Extent { get; set; }

        /// <summary>
        /// Gets or sets the type of the extent.
        /// </summary>
        /// <value>
        /// The type of the extent.
        /// </value>
        FdoSpatialContextListSpatialContextExtentType ExtentType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the XY tolerance.
        /// </summary>
        /// <value>
        /// The XY tolerance.
        /// </value>
        double XYTolerance { get; set; }

        /// <summary>
        /// Gets or sets the Z tolerance.
        /// </summary>
        /// <value>
        /// The Z tolerance.
        /// </value>
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

            var crs = doc.CreateElement("gml", "DerivedCRS", XmlNamespaces.GML); //NOXLATE
            {
                crs.SetAttribute("id", XmlNamespaces.GML, this.Name); //NOXLATE
                var meta = doc.CreateElement("gml", "metaDataProperty", XmlNamespaces.GML); //NOXLATE
                crs.AppendChild(meta);
                {
                    var genMeta = doc.CreateElement("gml", "GenericMetaData", XmlNamespaces.GML); //NOXLATE
                    meta.AppendChild(genMeta);
                    {
                        var fdoXY = doc.CreateElement("fdo", "XYTolerance", XmlNamespaces.FDO); //NOXLATE
                        var fdoZ = doc.CreateElement("fdo", "ZTolerance", XmlNamespaces.FDO); //NOXLATE
                        fdoXY.InnerText = this.XYTolerance.ToString(CultureInfo.InvariantCulture);
                        fdoZ.InnerText = this.ZTolerance.ToString(CultureInfo.InvariantCulture);
                        genMeta.AppendChild(fdoXY);
                        genMeta.AppendChild(fdoZ);
                    }
                }

                var remarks = doc.CreateElement("gml", "remarks", XmlNamespaces.GML); //NOXLATE
                remarks.InnerText = this.Description;
                crs.AppendChild(remarks);

                var csName = doc.CreateElement("gml", "srsName", XmlNamespaces.GML); //NOXLATE
                csName.InnerText = string.IsNullOrEmpty(this.CoordinateSystemName) ? this.Name : this.CoordinateSystemName;
                crs.AppendChild(csName);

                var ext = doc.CreateElement("gml", "validArea", XmlNamespaces.GML); //NOXLATE
                {
                    var bbox = doc.CreateElement("gml", "boundingBox", XmlNamespaces.GML); //NOXLATE
                    {
                        var ll = doc.CreateElement("gml", "pos", XmlNamespaces.GML); //NOXLATE
                        var ur = doc.CreateElement("gml", "pos", XmlNamespaces.GML); //NOXLATE
                        ll.InnerText = this.Extent.LowerLeftCoordinate.X + " " + this.Extent.LowerLeftCoordinate.Y; //NOXLATE
                        ur.InnerText = this.Extent.UpperRightCoordinate.X + " " + this.Extent.UpperRightCoordinate.Y; //NOXLATE
                        bbox.AppendChild(ll);
                        bbox.AppendChild(ur);
                    }
                    ext.AppendChild(bbox);
                }
                crs.AppendChild(ext);

                var baseCrs = doc.CreateElement("gml", "baseCRS", XmlNamespaces.GML); //NOXLATE
                var definedBy = doc.CreateElement("gml", "definedByConversion", XmlNamespaces.GML); //NOXLATE
                var derivedCrs = doc.CreateElement("gml", "derivedCRSType", XmlNamespaces.GML); //NOXLATE
                var userCs = doc.CreateElement("gml", "usesCS", XmlNamespaces.GML); //NOXLATE

                if (string.IsNullOrEmpty(this.CoordinateSystemWkt))
                {
                    baseCrs.SetAttribute("href", XmlNamespaces.XLINK, "http://fdo.osgeo.org/schemas/feature/crs/#" + (string.IsNullOrEmpty(this.CoordinateSystemName) ? this.Name : this.CoordinateSystemName)); //NOXLATE
                }
                else
                {
                    var wktCRS = doc.CreateElement("fdo", "WKTCRS", XmlNamespaces.FDO); //NOXLATE
                    wktCRS.SetAttribute("id", XmlNamespaces.GML, this.Name); //NOXLATE
                    {
                        var srsName = doc.CreateElement("gml", "srsName", XmlNamespaces.GML); //NOXLATE
                        srsName.InnerText = this.nameField;
                        var fdowkt = doc.CreateElement("fdo", "WKT", XmlNamespaces.FDO); //NOXLATE
                        fdowkt.InnerText = this.CoordinateSystemWkt;
                        wktCRS.AppendChild(srsName);
                        wktCRS.AppendChild(fdowkt);
                    }
                    baseCrs.AppendChild(wktCRS);
                }

                definedBy.SetAttribute("href", XmlNamespaces.XLINK, "http://fdo.osgeo.org/coord_conversions#identity"); //NOXLATE
                derivedCrs.SetAttribute("codeSpace", "http://fdo.osgeo.org/crs_types"); //NOXLATE
                derivedCrs.InnerText = "geographic"; //NOXLATE
                userCs.SetAttribute("href", XmlNamespaces.XLINK, "http://fdo.osgeo.org/cs#default_cartesian"); //NOXLATE

                crs.AppendChild(baseCrs);
                crs.AppendChild(definedBy);
                crs.AppendChild(derivedCrs);
                crs.AppendChild(userCs);
            }

            currentNode.AppendChild(crs);
        }

        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            if (!node.Name.Equals("gml:DerivedCRS")) //NOXLATE
                throw new Exception(string.Format(OSGeo.MapGuide.MaestroAPI.Strings.ErrorBadDocumentExpectedElement, "gml:DerivedCRS"));

            //Start off as dynamic, until we find a bounding box. Then we set it to static
            this.ExtentType = FdoSpatialContextListSpatialContextExtentType.Dynamic;

            var meta = node["gml:metaDataProperty"]; //NOXLATE
            if (meta != null)
            {
                var genMeta = meta["gml:GenericMetaData"]; //NOXLATE

                var scType = Utility.GetFdoElement(genMeta, "SCExtentType"); //NOXLATE
                var xyTol = Utility.GetFdoElement(genMeta, "XYTolerance"); //NOXLATE
                var zTol = Utility.GetFdoElement(genMeta, "ZTolerance"); //NOXLATE

                //this.ExtentType = (scType == null || scType.InnerText == "dynamic") ? FdoSpatialContextListSpatialContextExtentType.Dynamic : FdoSpatialContextListSpatialContextExtentType.Static;

                double xy_tol;
                double z_tol;

                if (double.TryParse(xyTol.InnerText, out xy_tol))
                    this.XYTolerance = xy_tol;

                if (double.TryParse(zTol.InnerText, out z_tol))
                    this.ZTolerance = z_tol;
            }
            else
            {
                this.XYTolerance = 0.0001;
                this.ZTolerance = 0.0001;
            }

            var remarks = node["gml:remarks"]; //NOXLATE
            var srsName = node["gml:srsName"]; //NOXLATE
            var ext = node["gml:validArea"]; //NOXLATE
            var baseCrs = node["gml:baseCRS"]; //NOXLATE

            this.Name = srsName.InnerText;
            this.Description = (remarks != null) ? remarks.InnerText : string.Empty;

            var bbox = ext["gml:boundingBox"]; //NOXLATE
            if (bbox != null)
            {
                var ll = bbox.FirstChild;
                var ur = bbox.LastChild;

                var llt = ll.InnerText.Split(' '); //NOXLATE
                var urt = ur.InnerText.Split(' '); //NOXLATE

                if (llt.Length != 2 || urt.Length != 2)
                    throw new Exception(OSGeo.MapGuide.MaestroAPI.Strings.ErrorBadDocumentInvalidBbox);

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

                this.ExtentType = FdoSpatialContextListSpatialContextExtentType.Static;
            }

            if (baseCrs.HasAttribute("xlink:href")) //NOXLATE
            {
                var href = baseCrs.GetAttribute("xlink:href"); //NOXLATE
                this.CoordinateSystemName = href.Substring(href.LastIndexOf("#") + 1); //NOXLATE
            }

            var wktCrs = Utility.GetFdoElement(baseCrs, "WKTCRS"); //NOXLATE
            if (wktCrs != null)
            {
                var wkt = Utility.GetFdoElement(wktCrs, "WKT"); //NOXLATE
                if (wkt != null)
                {
                    this.CoordinateSystemWkt = wkt.InnerText;
                }
            }
        }
    }
}
