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
    public enum FeatureGeometricType
    {
        Point = 1,
        Curve = 2,
        Surface = 4,
        Solid = 8,
        All = Curve | Point | Solid | Surface
    }

    /// <summary>
    /// Defines the valid specific geometry types for geometric storage
    /// </summary>
    public enum SpecificGeometryType
    {
        Point = 1,
        LineString = 2,
        Polygon = 3,
        MultiPoint = 4,
        MultiLineString = 5,
        MultiPolygon = 6,
        MultiGeometry = 7,
        CurveString = 10,
        CurvePolygon = 11,
        MultiCurveString = 12,
        MultiCurvePolygon = 13
    }

    /// <summary>
    /// A geometric property
    /// </summary>
    public class GeometricPropertyDefinition : PropertyDefinition
    {
        private GeometricPropertyDefinition() { this.SpecificGeometryTypes = new SpecificGeometryType[0]; }

        public GeometricPropertyDefinition(string name, string description)
            : this()
        {
            this.Name = name;
            this.Description = description;
        }

        public bool IsReadOnly { get; set; }

        public bool HasMeasure { get; set; }

        public bool HasElevation { get; set; }

        public string SpatialContextAssociation { get; set; }

        private FeatureGeometricType _geometricTypes;

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

        public override PropertyDefinitionType Type
        {
            get { return PropertyDefinitionType.Geometry; }
        }

        public override void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            var geom = doc.CreateElement("xs", "element", XmlNamespaces.XS);
            geom.SetAttribute("name", this.Name); //TODO: This may have been decoded. Should it be re-encoded?
            geom.SetAttribute("type", "gml:AbstractGeometryType");
            geom.SetAttribute("hasMeasure", XmlNamespaces.FDO, this.HasMeasure.ToString().ToLower());
            geom.SetAttribute("hasElevation", XmlNamespaces.FDO, this.HasElevation.ToString().ToLower());
            geom.SetAttribute("srsName", XmlNamespaces.FDO, this.SpatialContextAssociation);
            geom.SetAttribute("geometricTypes", XmlNamespaces.FDO, GeometricTypesToString());
            geom.SetAttribute("geometryTypes", XmlNamespaces.FDO, GeometryTypesToString());
            geom.SetAttribute("geometryReadOnly", XmlNamespaces.FDO, this.IsReadOnly.ToString().ToLower());

            currentNode.AppendChild(geom);
        }

        public override void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            var gt = node.Attributes["fdo:geometricTypes"];
            var gt2 = node.Attributes["fdo:geometryTypes"];
            var gtro = node.Attributes["fdo:geometryReadOnly"];
            var hms = node.Attributes["fdo:hasMeasure"];
            var hev = node.Attributes["fdo:hasElevation"];
            var srs = node.Attributes["fdo:srsName"];

            this.GeometricTypes = ProcessGeometricTypes(gt.Value);
            if (gt2 != null)
                this.SpecificGeometryTypes = ProcessSpecificGeometryTypes(gt2.Value);

            this.IsReadOnly = (gtro != null ? Convert.ToBoolean(gtro.Value) : false);
            this.HasElevation = (hev != null ? Convert.ToBoolean(hev.Value) : false);
            this.HasMeasure = (hms != null ? Convert.ToBoolean(hms.Value) : false);
            this.SpatialContextAssociation = (srs != null ? srs.Value : string.Empty);
        }

        private string GeometryTypesToString()
        {
            List<string> values = new List<string>();
            var gts = GetIndividualGeometricTypes();
            foreach (var gt in gts)
            {
                values.Add(gt.ToString().ToLower());
            }
            return string.Join(" ", values.ToArray());
        }

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
                    case "curvepolygon":
                        values.Add(SpecificGeometryType.CurvePolygon);
                        break;
                    case "curvestring":
                        values.Add(SpecificGeometryType.CurveString);
                        break;
                    case "linestring":
                        values.Add(SpecificGeometryType.LineString);
                        break;
                    case "multicurvepolygon":
                        values.Add(SpecificGeometryType.MultiCurvePolygon);
                        break;
                    case "multicurvestring":
                        values.Add(SpecificGeometryType.MultiCurveString);
                        break;
                    case "multigeometry":
                        values.Add(SpecificGeometryType.MultiGeometry);
                        break;
                    case "multilinestring":
                        values.Add(SpecificGeometryType.MultiLineString);
                        break;
                    case "multipoint":
                        values.Add(SpecificGeometryType.MultiPoint);
                        break;
                    case "multipolygon":
                        values.Add(SpecificGeometryType.MultiPolygon);
                        break;
                    case "point":
                        values.Add(SpecificGeometryType.Point);
                        break;
                    case "polygon":
                        values.Add(SpecificGeometryType.Polygon);
                        break;
                }
            }
            return values.ToArray();
        }

        private static FeatureGeometricType ProcessGeometricTypes(string p)
        {
            FeatureGeometricType? gt = null;
            string[] tokens = p.ToLower().Split(' ');
            foreach (string str in tokens)
            {
                switch (str)
                {
                    case "point":
                        {
                            if (gt.HasValue)
                                gt = gt.Value | FeatureGeometricType.Point;
                            else
                                gt = FeatureGeometricType.Point;
                        }
                        break;
                    case "curve":
                        {
                            if (gt.HasValue)
                                gt = gt.Value | FeatureGeometricType.Curve;
                            else
                                gt = FeatureGeometricType.Curve;
                        }
                        break;
                    case "surface":
                        {
                            if (gt.HasValue)
                                gt = gt.Value | FeatureGeometricType.Surface;
                            else
                                gt = FeatureGeometricType.Surface;
                        }
                        break;
                    case "solid":
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
    }
}
