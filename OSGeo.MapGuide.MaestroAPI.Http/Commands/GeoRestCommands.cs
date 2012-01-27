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
using OSGeo.MapGuide.MaestroAPI.Commands;
using System.Xml;
using OSGeo.MapGuide.MaestroAPI.Feature;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace OSGeo.MapGuide.MaestroAPI.Http.Commands
{
    public class GeoRestInsertFeatures : DefaultInsertCommand<HttpServerConnection>
    {
        internal GeoRestInsertFeatures(HttpServerConnection conn) : base(conn) { }

        protected override void ExecuteInternal()
        {
            var gconn = base.ConnImpl.GeoRestConnection;
            if (gconn == null)
                throw new InvalidOperationException("Connection does not have a valid GeoREST url or configuration"); //LOCALIZEME

            if (!gconn.Configuration.IsGeoRestEnabled(this.FeatureSourceId, this.ClassName))
                throw new InvalidOperationException(this.FeatureSourceId + " (" + this.ClassName + ") is not GeoREST configured"); //LOCALIEME

            var fsConf = gconn.Configuration.GetFeatureSource(this.FeatureSourceId, this.ClassName);
            if (!fsConf.AllowInsert)
                throw new InvalidOperationException(this.FeatureSourceId + " (" + this.ClassName + ") is not configured for insert operations"); //LOCALIEME

            //Assemble XML to POST
            XmlDocument doc = new XmlDocument();
            var fs = doc.AppendChild(doc.CreateElement("FeatureSet"));
            var features = fs.AppendChild(doc.CreateElement("Features"));
            var feat = ConvertToFeatureElement(doc);
            features.AppendChild(feat);
            
            //Build GeoREST insert url
            string insertUrl = gconn.Url;
            if (!insertUrl.EndsWith("/"))
                insertUrl += "/";
            insertUrl += "rest/data/" + fsConf.UriPart + "/.xml";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(insertUrl);
            req.Method = "POST";
            req.ContentType = "application/xml; charset=\"UTF-8\"";
            req.Timeout = 10 * 1000;

            using (Stream reqStream = req.GetRequestStream())
            {
                using (StreamWriter sw = new StreamWriter(reqStream))
                {
                    doc.Save(sw);
                }
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
#if DEBUG
            using (var sr = new StreamReader(resp.GetResponseStream()))
            {
                Trace.TraceInformation("GeoRest Insert:\n" + sr.ReadToEnd());
            }
#endif
        }

        private XmlNode ConvertToFeatureElement(XmlDocument doc)
        {
            XmlNode feat = doc.CreateElement("Feature");
            foreach (var propName in this.RecordToInsert.PropertyNames)
            {
                var prop = doc.CreateElement("Property");
                var name = doc.CreateElement("Name");
                name.InnerText = propName;
                prop.AppendChild(name);

                var value = doc.CreateElement("Value");
                prop.AppendChild(value);

                if (!this.RecordToInsert.IsNull(propName))
                {
                    var pv = this.RecordToInsert.GetValue(propName);
                    value.InnerText = pv.ValueAsString();
                }
                feat.AppendChild(prop);
            }
            return feat;
        }
    }

    /*
    public class GeoRestUpdateFeatures : DefaultUpdateCommand<HttpServerConnection>
    {
        public override int ExecuteInternal()
        {
            var gconn = base.ConnImpl.GeoRestConnection;
            if (gconn == null)
                throw new InvalidOperationException("Connection does not have a valid GeoREST url or configuration"); //LOCALIZEME

            if (!gconn.Configuration.IsGeoRestEnabled(this.FeatureSourceId))
                throw new InvalidOperationException(this.FeatureSourceId + " is not GeoREST configured"); //LOCALIEME

            var fsConf = gconn.Configuration.GetFeatureSource(this.FeatureSourceId);
            if (!fsConf.AllowInsert)
                throw new InvalidOperationException(this.FeatureSourceId + " is not configured for insert operations"); //LOCALIEME

            XmlDocument doc = new XmlDocument();
            var fs = doc.AppendChild(doc.CreateElement("FeatureSet"));
            var features = fs.AppendChild(doc.CreateElement("Features"));
            var feat = ConvertToFeatureElement(doc);
            features.AppendChild(feat);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(gconn.Url);
            req.Method = "POST";
            req.ContentType = "text/xml";

            Stream reqStream = req.GetRequestStream();
            doc.Save(reqStream);

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
#if DEBUG
            using (var sr = new StreamReader(resp.GetResponseStream()))
            {
                Trace.TraceInformation("GeoRest Insert:\n" + sr.ReadToEnd());
            }
#endif
            return 1;
        }

        private XmlNode ConvertToFeatureElement(XmlDocument doc)
        {
            XmlNode feat = doc.CreateElement("Feature");
            foreach (var propName in this.ValuesToUpdate.PropertyNames)
            {
                var prop = doc.CreateElement("Property");
                var name = doc.CreateElement("Name");
                name.InnerText = propName;
                prop.AppendChild(name);

                var value = doc.CreateElement("Value");
                prop.AppendChild(value);

                if (!this.ValuesToUpdate.IsNull(propName))
                {
                    var pv = this.ValuesToUpdate.GetValue(propName);
                    value.InnerText = pv.ValueAsString();
                }
                feat.AppendChild(prop);
            }
            return feat;
        }
    }
     
    public class GeoRestDeleteFeatures : DefaultDeleteCommand<HttpServerConnection>
    {
        protected override int ExecuteInternal()
        {
            var gconn = base.ConnImpl;
            if (gconn == null)
                throw new InvalidOperationException("Connection does not have a valid GeoREST url or configuration");
        }
    }
     */
}
