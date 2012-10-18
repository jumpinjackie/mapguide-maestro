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
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.Schema
{
    /// <summary>
    /// Defines the valid types of geometric storage in a geometric property
    /// </summary>
    [Flags]
    public enum FeatureGeometricType : int
    {
        /// <summary>
        /// Points
        /// </summary>
        Point = 1,
        /// <summary>
        /// Curves (lines)
        /// </summary>
        Curve = 2,
        /// <summary>
        /// Surfaces (polygons)
        /// </summary>
        Surface = 4,
        /// <summary>
        /// Solids
        /// </summary>
        Solid = 8,
        /// <summary>
        /// All types
        /// </summary>
        All = Curve | Point | Solid | Surface
    }

    /// <summary>
    /// Defines the valid specific geometry types for geometric storage
    /// </summary>
    public enum SpecificGeometryType
    {
        /// <summary>
        /// Points
        /// </summary>
        Point = 1,
        /// <summary>
        /// Line Strings
        /// </summary>
        LineString = 2,
        /// <summary>
        /// Polygons
        /// </summary>
        Polygon = 3,
        /// <summary>
        /// Multi Points
        /// </summary>
        MultiPoint = 4,
        /// <summary>
        /// Multi Line Strings
        /// </summary>
        MultiLineString = 5,
        /// <summary>
        /// Multi Polygons
        /// </summary>
        MultiPolygon = 6,
        /// <summary>
        /// Multi Geometries
        /// </summary>
        MultiGeometry = 7,
        /// <summary>
        /// Curve Strings
        /// </summary>
        CurveString = 10,
        /// <summary>
        /// Curve Polygons
        /// </summary>
        CurvePolygon = 11,
        /// <summary>
        /// Multi Curve Strings
        /// </summary>
        MultiCurveString = 12,
        /// <summary>
        /// Multi Curve Polygons
        /// </summary>
        MultiCurvePolygon = 13
    }

    /// <summary>
    /// A geometric property
    /// </summary>
    public class GeometricPropertyDefinition : PropertyDefinition
    {
        private GeometricPropertyDefinition() { this.SpecificGeometryTypes = new SpecificGeometryType[0]; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricPropertyDefinition"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public GeometricPropertyDefinition(string name, string description)
            : this()
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets a Boolean value that indicates if this geometric property is read-only. 
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates if the geometry of this property includes measurement values that can be used for dynamic segmentation. 
        /// </summary>
        public bool HasMeasure { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates if the geometry of this property include elevation values. 
        /// </summary>
        public bool HasElevation { get; set; }

        /// <summary>
        /// Gets or sets the Spatial Context name associated to this geometric property. 
        /// </summary>
        public string SpatialContextAssociation { get; set; }

        private FeatureGeometricType _geometricTypes;

        /// <summary>
        /// Gets or sets the <see cref="T:OSGeo.MapGuide.MaestroAPI.Schema.FeatureGeometricType"/> that can be stored in this geometric 
        /// property. The returned value may be any combination of the values from the <see cref="T:OSGeo.MapGuide.MaestroAPI.Schema.FeatureGeometricType"/> 
        /// enumeration combined via a bit-wise or operation. 
        /// </summary>
        public FeatureGeometricType GeometricTypes
        {
            get { return _geometricTypes; }
            set
            {
                _geometricTypes = value;

                Dictionary<SpecificGeometryType, SpecificGeometryType> sgts = new Dictionary<SpecificGeometryType, SpecificGeometryType>();
                foreach (FeatureGeometricType fgt in Enum.GetValues(typeof(FeatureGeometricType)))
                {
                    if ((_geometricTypes & fgt) > 0)
                    {
                        switch (fgt)
                        {
                            case FeatureGeometricType.Point:
                                sgts.Add(SpecificGeometryType.Point, SpecificGeometryType.Point);
                                sgts.Add(SpecificGeometryType.MultiPoint, SpecificGeometryType.MultiPoint);
                                break;
                            case FeatureGeometricType.Curve:
                                sgts.Add(SpecificGeometryType.LineString, SpecificGeometryType.LineString);
                                sgts.Add(SpecificGeometryType.MultiLineString, SpecificGeometryType.MultiLineString);
                                sgts.Add(SpecificGeometryType.CurveString, SpecificGeometryType.CurveString);
                                sgts.Add(SpecificGeometryType.MultiCurveString, SpecificGeometryType.MultiCurveString);
                                break;
                            case FeatureGeometricType.Surface:
                                sgts.Add(SpecificGeometryType.Polygon, SpecificGeometryType.Polygon);
                                sgts.Add(SpecificGeometryType.MultiPolygon, SpecificGeometryType.MultiPolygon);
                                sgts.Add(SpecificGeometryType.CurvePolygon, SpecificGeometryType.CurvePolygon);
                                sgts.Add(SpecificGeometryType.MultiCurvePolygon, SpecificGeometryType.MultiCurvePolygon);
                                break;
                        }
                    }
                }

                _sgts = new List<SpecificGeometryType>(sgts.Keys).ToArray();
            }
        }

        private SpecificGeometryType[] _sgts;

        /// <summary>
        /// Gets or sets the specific set of geometry types that can be stored in this geometric property. The provided value is a 
        /// list of geometry types that are supported. Usually, one specific type is supported, but there can be more than one. 
        /// </summary>
        public SpecificGeometryType[] SpecificGeometryTypes
        {
            get { return _sgts; }
            set
            {
                _sgts = value;

                bool hasPoint = false;
                bool hasLine = false;
                bool hasPoly = false;

                for (int i = 0; i < _sgts.Length; i++)
                {
                    if (_sgts[i] == SpecificGeometryType.Point ||
                        _sgts[i] == SpecificGeometryType.MultiPoint)
                        hasPoint = true;

                    if (_sgts[i] == SpecificGeometryType.LineString ||
                        _sgts[i] == SpecificGeometryType.MultiLineString ||
                        _sgts[i] == SpecificGeometryType.CurveString ||
                        _sgts[i] == SpecificGeometryType.MultiCurveString)
                        hasLine = true;

                    if (_sgts[i] == SpecificGeometryType.Polygon ||
                        _sgts[i] == SpecificGeometryType.MultiPolygon ||
                        _sgts[i] == SpecificGeometryType.CurvePolygon ||
                        _sgts[i] == SpecificGeometryType.MultiCurvePolygon)
                        hasPoly = true;

                    if (_sgts[i] == SpecificGeometryType.MultiGeometry)
                        hasPoly = hasPoint = hasLine = true;
                }

                FeatureGeometricType? fgt = null;
                if (hasPoint)
                {
                    if (fgt.HasValue)
                        fgt = fgt.Value | FeatureGeometricType.Point;
                    else
                        fgt = FeatureGeometricType.Point;
                }
                if (hasLine)
                {
                    if (fgt.HasValue)
                        fgt = fgt.Value | FeatureGeometricType.Curve;
                    else
                        fgt = FeatureGeometricType.Curve;
                }
                if (hasPoly)
                {
                    if (fgt.HasValue)
                        fgt = fgt.Value | FeatureGeometricType.Surface;
                    else
                        fgt = FeatureGeometricType.Surface;
                }

                if (fgt.HasValue)
                    _geometricTypes = fgt.Value;
            }
        }

        /// <summary>
        /// Gets the type of property definition
        /// </summary>
        public override PropertyDefinitionType Type
        {
            get { return PropertyDefinitionType.Geometry; }
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
            geom.SetAttribute("type", "gml:AbstractGeometryType"); //NOXLATE
            geom.SetAttribute("hasMeasure", XmlNamespaces.FDO, this.HasMeasure.ToString().ToLower()); //NOXLATE
            geom.SetAttribute("hasElevation", XmlNamespaces.FDO, this.HasElevation.ToString().ToLower()); //NOXLATE
            geom.SetAttribute("srsName", XmlNamespaces.FDO, this.SpatialContextAssociation); //NOXLATE
            geom.SetAttribute("geometricTypes", XmlNamespaces.FDO, GeometricTypesToString()); //NOXLATE
            geom.SetAttribute("geometryTypes", XmlNamespaces.FDO, GeometryTypesToString()); //NOXLATE
            geom.SetAttribute("geometryReadOnly", XmlNamespaces.FDO, this.IsReadOnly.ToString().ToLower()); //NOXLATE

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
            var gt = Utility.GetFdoAttribute(node, "geometricTypes"); //NOXLATE
            var gt2 = Utility.GetFdoAttribute(node, "geometryTypes"); //NOXLATE
            var gtro = Utility.GetFdoAttribute(node, "geometryReadOnly"); //NOXLATE
            var hms = Utility.GetFdoAttribute(node, "hasMeasure"); //NOXLATE
            var hev = Utility.GetFdoAttribute(node, "hasElevation"); //NOXLATE
            var srs = Utility.GetFdoAttribute(node, "srsName"); //NOXLATE

            this.GeometricTypes = ProcessGeometricTypes(gt.Value);
            if (gt2 != null)
                this.SpecificGeometryTypes = ProcessSpecificGeometryTypes(gt2.Value);

            this.IsReadOnly = (gtro != null ? Convert.ToBoolean(gtro.Value) : false);
            this.HasElevation = (hev != null ? Convert.ToBoolean(hev.Value) : false);
            this.HasMeasure = (hms != null ? Convert.ToBoolean(hms.Value) : false);
            this.SpatialContextAssociation = (srs != null ? srs.Value : string.Empty);
        }

        /// <summary>
        /// Converts the current specified geometry types to a space-delimited list of types
        /// </summary>
        /// <returns></returns>
        public string GeometryTypesToString()
        {
            List<string> values = new List<string>();
            var gts = GetIndividualGeometricTypes();
            foreach (var gt in gts)
            {
                values.Add(gt.ToString().ToLower());
            }
            return string.Join(" ", values.ToArray()); //NOXLATE
        }

        /// <summary>
        /// Gets an array of the individual <see cref="T:OSGeo.MapGuide.MaestroAPI.Schema.FeatureGeometricType"/> values that
        /// compose the final masked value that is returned by the <see cref="M:OSGeo.MapGuide.MaestroAPI.GeometricPropertyDefinition.GeometricTypes"/>
        /// property
        /// </summary>
        /// <returns></returns>
        public FeatureGeometricType[] GetIndividualGeometricTypes()
        {
            List<FeatureGeometricType> gts = new List<FeatureGeometricType>();
            if (this.GeometricTypes == FeatureGeometricType.All)
            {
                gts.AddRange((FeatureGeometricType[])Enum.GetValues(typeof(FeatureGeometricType)));
            }
            else
            {
                foreach (FeatureGeometricType gt in Enum.GetValues(typeof(FeatureGeometricType)))
                {
                    if (((int)this.GeometricTypes & (int)gt) == (int)gt)
                    {
                        gts.Add(gt);
                    }
                }
            }
            return gts.ToArray();
        }

        private string GeometricTypesToString()
        {
            List<string> values = new List<string>();
            foreach (SpecificGeometryType geom in this.SpecificGeometryTypes)
            {
                values.Add(geom.ToString().ToLower());
            }
            return string.Join(" ", values.ToArray());
        }

        private static SpecificGeometryType[] ProcessSpecificGeometryTypes(string str)
        {
            List<SpecificGeometryType> values = new List<SpecificGeometryType>();
            string[] tokens = str.ToLower().Split(' ');
            foreach (string t in tokens)
            {
                switch (t)
                {
                    case "curvepolygon": //NOXLATE
                        values.Add(SpecificGeometryType.CurvePolygon);
                        break;
                    case "curvestring": //NOXLATE
                        values.Add(SpecificGeometryType.CurveString);
                        break;
                    case "linestring": //NOXLATE
                        values.Add(SpecificGeometryType.LineString);
                        break;
                    case "multicurvepolygon": //NOXLATE
                        values.Add(SpecificGeometryType.MultiCurvePolygon);
                        break;
                    case "multicurvestring": //NOXLATE
                        values.Add(SpecificGeometryType.MultiCurveString);
                        break;
                    case "multigeometry": //NOXLATE
                        values.Add(SpecificGeometryType.MultiGeometry);
                        break;
                    case "multilinestring": //NOXLATE
                        values.Add(SpecificGeometryType.MultiLineString);
                        break;
                    case "multipoint": //NOXLATE
                        values.Add(SpecificGeometryType.MultiPoint);
                        break;
                    case "multipolygon": //NOXLATE
                        values.Add(SpecificGeometryType.MultiPolygon);
                        break;
                    case "point": //NOXLATE
                        values.Add(SpecificGeometryType.Point);
                        break;
                    case "polygon": //NOXLATE
                        values.Add(SpecificGeometryType.Polygon);
                        break;
                }
            }
            return values.ToArray();
        }

        private static FeatureGeometricType ProcessGeometricTypes(string p)
        {
            FeatureGeometricType? gt = null;
            string[] tokens = p.ToLower().Split(' '); //NOXLATE
            foreach (string str in tokens)
            {
                switch (str)
                {
                    case "point": //NOXLATE
                        {
                            if (gt.HasValue)
                                gt = gt.Value | FeatureGeometricType.Point;
                            else
                                gt = FeatureGeometricType.Point;
                        }
                        break;
                    case "curve": //NOXLATE
                        {
                            if (gt.HasValue)
                                gt = gt.Value | FeatureGeometricType.Curve;
                            else
                                gt = FeatureGeometricType.Curve;
                        }
                        break;
                    case "surface": //NOXLATE
                        {
                            if (gt.HasValue)
                                gt = gt.Value | FeatureGeometricType.Surface;
                            else
                                gt = FeatureGeometricType.Surface;
                        }
                        break;
                    case "solid": //NOXLATE
                        {
                            if (gt.HasValue)
                                gt = gt.Value | FeatureGeometricType.Solid;
                            else
                                gt = FeatureGeometricType.Solid;
                        }
                        break;
                }
            }
            if (gt.HasValue)
                return gt.Value;
            else
                throw new ArgumentException();
        }

        /// <summary>
        /// Gets the expression data type
        /// </summary>
        public override OSGeo.MapGuide.ObjectModels.Common.ExpressionDataType ExpressionType
        {
            get { return OSGeo.MapGuide.ObjectModels.Common.ExpressionDataType.Geometry; }
        }
    }
}
