#region Disclaimer / License
// Copyright (C) 2014, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.Rest
{
    public class RestCoordinateSystemCatalog : CoordinateSystemCatalog
    {
        private string _libraryName;
        private CoordinateSystemCategory[] _categories;
        private RestConnection _conn;

        internal RestCoordinateSystemCatalog(RestConnection conn)
        {
            _conn = conn;
        }

        public override CoordinateSystemDefinitionBase CreateEmptyCoordinateSystem()
        {
            return new RestCoordinateSystemDefinition();
        }

        public override CoordinateSystemCategory[] Categories
        {
            get
            {
                if (_categories == null)
                {
                    var client = _conn.MakeClient();
                    var req = new RestRequest("coordsys/categories", Method.GET);
                    var resp = client.Execute(req);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(resp.Content);
                    XmlNodeList lst = doc.SelectNodes("StringCollection/Item");
                    CoordinateSystemCategory[] data = new CoordinateSystemCategory[lst.Count];
                    for (int i = 0; i < lst.Count; i++)
                        data[i] = new RestCoordinateSystemCategory(this, lst[i].InnerText);
                    _categories = data;
                }

                return _categories;
            }
        }

        public override string LibraryName
        {
            get
            {
                if (_libraryName == null)
                {
                    var client = _conn.MakeClient();
                    var req = new RestRequest("coordsys/libraryName", Method.GET);
                    var resp = client.Execute(req);

                    _libraryName = resp.Content;
                }
                return _libraryName;
            }
        }

        public override string ConvertCoordinateSystemCodeToWkt(string coordcode)
        {
            var client = _conn.MakeClient();
            var req = new RestRequest("coordsys/mentor/{csCode}/wkt", Method.GET);
            req.AddUrlSegment("csCode", coordcode);
            var resp = client.Execute(req);
            return resp.Content;
        }

        public override string ConvertEpsgCodeToWkt(string epsg)
        {
            var client = _conn.MakeClient();
            var req = new RestRequest("coordsys/epsg/{epsg}/wkt", Method.GET);
            req.AddUrlSegment("epsg", epsg);
            var resp = client.Execute(req);
            return resp.Content;
        }

        public override string ConvertWktToCoordinateSystemCode(string wkt)
        {
            var client = _conn.MakeClient();
            var req = new RestRequest("coordsys/wkttomentor", Method.POST);
            req.AddParameter("wkt", wkt);
            var resp = client.Execute(req);
            return resp.Content;
        }

        public override string ConvertWktToEpsgCode(string wkt)
        {
            var client = _conn.MakeClient();
            var req = new RestRequest("coordsys/wkttoepsg", Method.POST);
            req.AddParameter("wkt", wkt);
            var resp = client.Execute(req);
            return resp.Content;
        }

        public override CoordinateSystemDefinitionBase[] EnumerateCoordinateSystems(string category)
        {
            CoordinateSystemCategory cat = null;
            foreach (CoordinateSystemCategory csc in this.Categories)
            {
                if (csc.Name == category)
                {
                    cat = csc;
                    break;
                }
            }

            if (cat == null)
                return new CoordinateSystemDefinitionBase[0];


            var client = _conn.MakeClient();
            var req = new RestRequest("coordsys/category/{category}.xml", Method.POST);
            req.AddUrlSegment("category", category);
            var resp = client.Execute(req);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(resp.Content);
            XmlNodeList lst = doc.SelectNodes("BatchPropertyCollection/PropertyCollection");
            CoordinateSystemDefinitionBase[] data = new CoordinateSystemDefinitionBase[lst.Count];
            for (int i = 0; i < lst.Count; i++)
                data[i] = new RestCoordinateSystemDefinition(cat, lst[i]);

            return data;
        }

        public override bool IsValid(string wkt)
        {
            var client = _conn.MakeClient();
            var req = new RestRequest("coordsys/validatewkt", Method.POST);
            req.AddParameter("wkt", wkt);
            var resp = client.Execute(req);
            _conn.ValidateResponse(resp);
            return bool.Parse(resp.Content);
        }

        public override bool IsLoaded
        {
            get { return _categories != null; }
        }
    }
}
