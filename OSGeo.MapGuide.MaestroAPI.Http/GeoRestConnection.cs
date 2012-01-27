#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.IO;
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    internal class GeoRestConnection
    {
        private string _geoRestUrl;

        private string _configFile;

        private GeoRestFeatureSourceConfiguration _conf;

        public GeoRestFeatureSourceConfiguration Configuration { get { return _conf; } }

        internal GeoRestConnection(string url, string configFile)
        {
            _geoRestUrl = url;
            _configFile = configFile;
            _conf = GeoRestFeatureSourceConfiguration.LoadFrom(configFile);
        }

        public string Url { get { return _geoRestUrl; } }

        public string ConfigPath { get { return _configFile; } }
    }

    internal class GeoRestFeatureSourceConfiguration
    {
        private Dictionary<string, GeoRestFeatureSource> _configuredFeatureSources;

        private GeoRestFeatureSourceConfiguration()
        {
            _configuredFeatureSources = new Dictionary<string, GeoRestFeatureSource>();
        }

        private void PutConfig(string featureSource, string featureClass, GeoRestFeatureSource fs)
        {
            _configuredFeatureSources[featureSource + "|" + featureClass] = fs;
        }

        public bool IsGeoRestEnabled(string featureSource, string featureClass) { return _configuredFeatureSources.ContainsKey(featureSource + "|" + featureClass); }

        public ICollection<string> FeatureSources { get { return _configuredFeatureSources.Keys; } }

        public GeoRestFeatureSource GetFeatureSource(string featureSource, string featureClass) { return _configuredFeatureSources[featureSource + "|" + featureClass]; }

        internal static GeoRestFeatureSourceConfiguration LoadFrom(string configFile)
        {
            var conf = new GeoRestFeatureSourceConfiguration();

            if (!string.IsNullOrEmpty(configFile) && File.Exists(configFile))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configFile);

                XmlNodeList fsNodes = doc.GetElementsByTagName("GeoRestFeatureSource");
                foreach (XmlNode fsNode in fsNodes)
                {
                    var gfs = new GeoRestFeatureSource()
                    {
                        FeatureSourceId = fsNode["FeatureSource"].InnerText,
                        FeatureClass = fsNode["FeatureClass"].InnerText,
                        UriPart = fsNode["UriPart"].InnerText,
                        AllowInsert = bool.Parse(fsNode["AllowInsert"].InnerText),
                        AllowUpdate = bool.Parse(fsNode["AllowUpdate"].InnerText),
                        AllowDelete = bool.Parse(fsNode["AllowDelete"].InnerText)
                    };

                    conf.PutConfig(gfs.FeatureSourceId, gfs.FeatureClass, gfs);
                }
            }

            return conf;
        }
    }

    public class GeoRestFeatureSource
    {
        public string FeatureSourceId { get; set; }

        public string UriPart { get; set; }

        public string FeatureClass { get; set; }

        public bool AllowInsert { get; set; }

        public bool AllowUpdate { get; set; }

        public bool AllowDelete { get; set; }
    }
}
