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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System.Collections.Generic;
using System.Xml;

namespace OSGeo.MapGuide.ObjectModels.Json
{
    public static class AppDefJsonSerializer
    {
        public static string Serialize(IApplicationDefinition appDef)
        {
            var xml = appDef.Serialize();
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);

            const string XSI_NS = "http://www.w3.org/2001/XMLSchema-instance";
            const string JSON_NS = "http://james.newtonking.com/projects/json";
            doc.DocumentElement.SetAttribute("xmlns:json", JSON_NS);

            //Force arrays on key elements
            var widgetSetEls = doc.GetElementsByTagName("WidgetSet");
            foreach (XmlElement el in widgetSetEls)
            {
                el.SetAttribute("Array", JSON_NS, "true");
            }
            var mapGroupEls = doc.GetElementsByTagName("MapGroup");
            foreach (XmlElement el in mapGroupEls)
            {
                el.SetAttribute("Array", JSON_NS, "true");
                el["Map"].SetAttribute("Array", JSON_NS, "true");
            }
            var containerEls = doc.GetElementsByTagName("Container");
            foreach (XmlElement el in containerEls)
            {
                el.SetAttribute("Array", JSON_NS, "true");
                foreach (XmlElement cel in el.ChildNodes)
                {
                    if (cel.Name == "Item")
                    {
                        cel.SetAttribute("Array", JSON_NS, "true");
                    }
                }
            }

            //Clean out XML schema related crap that has no business being serialized out to JSON
            var removeRootAttrs = new List<XmlAttribute>();
            foreach (XmlAttribute a in doc.DocumentElement.Attributes)
            {
                if (a.Name == "xmlns:xsi" ||
                    a.Name == "xmlns:xsd" ||
                    a.Name == "xsi:noNamespaceSchemaLocation")
                {
                    removeRootAttrs.Add(a);
                }
            }
            foreach (var remove in removeRootAttrs)
            {
                doc.DocumentElement.Attributes.Remove(remove);
            }

            var allNodes = doc.SelectNodes("//*");
            foreach (XmlNode n in allNodes)
            {
                var attr = n.Attributes["type", XSI_NS];
                if (attr != null)
                {
                    n.Attributes.Remove(attr);
                }
            }

            var debugXml = doc.OuterXml;

            var rootEl = doc.GetElementsByTagName("ApplicationDefinition");
            var json = JsonConvert.SerializeXmlNode(rootEl[0], Newtonsoft.Json.Formatting.Indented);
            var o = JObject.Parse(json);
            return o["ApplicationDefinition"].ToString();
        }
    }
}
