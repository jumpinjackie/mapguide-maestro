#region Disclaimer / License

// Copyright (C) 2021, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License


using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Commands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace Maestro.Base.UI
{
    public class WfsRootRepositoryItem
    {
        readonly List<WfsLayerRepositoryItem> _layers;

        public WfsRootRepositoryItem(string connName)
        {
            this.ConnectionName = connName;
            _layers = new List<WfsLayerRepositoryItem>();
        }

        /// <summary>
        /// Gets the name of the associated connection
        /// </summary>
        public string ConnectionName
        {
            get;
            private set;
        }

        public string Name => Strings.WfsLayers;

        /// <summary>
        /// Gets the icon for this item
        /// </summary>
        public Image Icon => Properties.Resources.server_cloud;

        public bool IsLoaded { get; private set; }

        public IEnumerable<WfsLayerRepositoryItem> Layers => _layers;

        const string NS_WFS = "http://www.opengis.net/wfs";

        public void Load(IGetWfsCapabilities cmd)
        {
            using (var s = cmd.Execute(WfsVersion.v1_1_0))
            {
                var doc = new XmlDocument();
                doc.Load(s);

                var layers = doc.GetElementsByTagName("FeatureType", NS_WFS); //NOXLATE

                foreach (XmlNode layer in layers)
                {
                    var name = layer["Name", NS_WFS].InnerText; //NOXLATE
                    var title = layer["Title", NS_WFS].InnerText; //NOXLATE
                    if (string.IsNullOrEmpty(title))
                        title = Strings.UntitledWfsLayer;
                    var @abstract = layer["Abstract", NS_WFS]?.InnerText; //NOXLATE
                    var crs = layer["DefaultSRS", NS_WFS]?.InnerText; //NOXLATE

                    _layers.Add(new WfsLayerRepositoryItem(this.ConnectionName, title)
                    {
                        LayerName = name,
                        Abstract = @abstract,
                        Crs = crs
                    });
                }
            }
            _layers.Sort((a, b) => a.Name.CompareTo(b.Name));
            this.IsLoaded = true;
        }
    }

    public class WfsLayerRepositoryItem
    {
        public WfsLayerRepositoryItem(string connName, string name)
        {
            this.ConnectionName = connName;
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the associated connection
        /// </summary>
        public string ConnectionName
        {
            get;
            private set;
        }

        public string Name { get; private set; }

        public Image Icon => Properties.Resources.layer;

        public string LayerName { get; internal set; }
        public string Abstract { get; internal set; }
        public string Crs { get; internal set; }
    }


    public class WmsRootRepositoryItem
    {
        readonly List<WmsLayerRepositoryItem> _layers;

        public WmsRootRepositoryItem(string connName)
        {
            this.ConnectionName = connName;
            _layers = new List<WmsLayerRepositoryItem>();
        }

        /// <summary>
        /// Gets the name of the associated connection
        /// </summary>
        public string ConnectionName
        {
            get;
            private set;
        }

        public string Name => Strings.WmsLayers;

        /// <summary>
        /// Gets the icon for this item
        /// </summary>
        public Image Icon => Properties.Resources.server_cloud;

        public bool IsLoaded { get; private set; }

        public IEnumerable<WmsLayerRepositoryItem> Layers => _layers;

        public void Load(IGetWmsCapabilities cmd)
        {
            using (var s = cmd.Execute(WmsVersion.v1_3_0))
            {
                var doc = new XmlDocument();
                doc.Load(s);

                var layers = doc.GetElementsByTagName("Layer"); //NOXLATE

                foreach (XmlNode layer in layers)
                {
                    if (layer.Attributes["queryable"] != null)
                    {
                        var queryable = layer.Attributes["queryable"].Value == "1"; //NOXLATE
                        var name = layer["Name"].InnerText; //NOXLATE
                        var title = layer["Title"].InnerText; //NOXLATE
                        if (string.IsNullOrEmpty(title))
                            title = Strings.UntitledWmsLayer;
                        var @abstract = layer["Abstract"]?.InnerText; //NOXLATE
                        var crs = layer["CRS"]?.InnerText; //NOXLATE

                        WmsBounds bbox = null;
                        var bboxEl = layer["BoundingBox"];
                        if (bboxEl != null)
                        {
                            var sMinX = bboxEl.Attributes["minx"]?.Value;
                            var sMinY = bboxEl.Attributes["miny"]?.Value;
                            var sMaxX = bboxEl.Attributes["maxx"]?.Value;
                            var sMaxY = bboxEl.Attributes["maxy"]?.Value;
                            if (double.TryParse(sMinX, out var minx) &&
                                double.TryParse(sMinY, out var miny) &&
                                double.TryParse(sMaxX, out var maxx) &&
                                double.TryParse(sMaxY, out var maxy))
                            {
                                //TODO: The bounds of this layer will clearly show nothing in the OL preview
                                //if the Layer Definition in question
                                bbox = new WmsBounds(minx, miny, maxx, maxy);
                            }
                        }

                        _layers.Add(new WmsLayerRepositoryItem(this.ConnectionName, title)
                        {
                            LayerName = name,
                            Abstract = @abstract,
                            Crs = crs,
                            Queryable = queryable,
                            BBOX = bbox
                        });
                    }
                }
            }
            _layers.Sort((a, b) => a.Name.CompareTo(b.Name));
            this.IsLoaded = true;
        }
    }

    public record WmsBounds(double MinX, double MinY, double MaxX, double MaxY)
    { 
        public string ConvertToString(string crs, WmsVersion version)
        {
            switch (version)
            {
                case WmsVersion.v1_0_0:
                case WmsVersion.v1_1_0:
                case WmsVersion.v1_1_1:
                    return $"{MinX},{MinY},{MaxX},{MaxY}";
                case WmsVersion.v1_3_0:
                    if (ShoudlFlipCoordAxes(crs))
                        return $"{MinY},{MinX},{MaxY},{MaxX}";
                    else
                        return $"{MinX},{MinY},{MaxX},{MaxY}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(version));
            }
        }

        static bool ShoudlFlipCoordAxes(string crs)
        {
            //TODO: We are handling the most common scenarios. True solution is to compile an EPSG lookup table
            //of CRSes with flipped axes
            switch (crs)
            {
                case "EPSG:4326":
                case "CRS:84":
                    return true;
            }
            return false;
        }
    }


    public class WmsLayerRepositoryItem : ISiteExplorerNode
    {
        public WmsLayerRepositoryItem(string connName, string name)
        {
            this.ConnectionName = connName;
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the associated connection
        /// </summary>
        public string ConnectionName
        {
            get;
            private set;
        }

        public string Name { get; private set; }

        public Image Icon => Properties.Resources.map;

        public string LayerName { get; internal set; }
        public string Abstract { get; internal set; }
        public string Crs { get; internal set; }
        public WmsBounds BBOX { get; internal set; }
        public bool Queryable { get; internal set; }
    }
}
