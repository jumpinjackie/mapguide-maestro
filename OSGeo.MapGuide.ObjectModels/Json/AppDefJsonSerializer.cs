﻿#region Disclaimer / License

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
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using System.Collections.Generic;
using System.Xml;

namespace OSGeo.MapGuide.ObjectModels.Json
{
    /// <summary>
    /// Helper class to convert an Application Definition to JSON
    /// </summary>
    public static class AppDefJsonSerializer
    {
        const string XSI_NS = "http://www.w3.org/2001/XMLSchema-instance";
        const string JSON_NS = "http://james.newtonking.com/projects/json";

        static void ForceArray(XmlDocument doc, string tagName, Action<XmlElement> elProcessor = null)
        {
            var els = doc.GetElementsByTagName(tagName);
            foreach (XmlElement el in els)
            {
                el.SetAttribute("Array", JSON_NS, "true");
                elProcessor?.Invoke(el);
            }
        }

        enum JsonDataType
        {
            Integer,
            Boolean,
            Float,
            Date
        }

        static void ForceDataTypeByTagName(XmlDocument doc, string tagName, JsonDataType type)
        {
            var els = doc.GetElementsByTagName(tagName);
            foreach (XmlElement el in els)
            {
                el.SetAttribute("Type", JSON_NS, type.ToString());
            }
        }

        /// <summary>
        /// Serializes the given Application Definition to JSON
        /// </summary>
        /// <param name="appDef"></param>
        /// <returns></returns>
        public static string Serialize(IApplicationDefinition appDef)
        {
            var xml = appDef.Serialize();
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);

            doc.DocumentElement.SetAttribute("xmlns:json", JSON_NS);

            //Force arrays on key elements
            ForceArray(doc, "WidgetSet");
            ForceArray(doc, "MapGroup", el => el["Map"].SetAttribute("Array", JSON_NS, "true"));
            ForceArray(doc, "Container", el =>
            {
                foreach (XmlElement cel in el.ChildNodes)
                {
                    if (cel.Name == "Item")
                    {
                        cel.SetAttribute("Array", JSON_NS, "true");
                    }
                }
            });
            ForceArray(doc, "source_param_urls");
            ForceArray(doc, "urls"); //MapGroup/Map/Extension/Options/urls

            //Force data types on known map extension elements
            ForceDataTypeByTagName(doc, "meta_extents", JsonDataType.Float);
            ForceDataTypeByTagName(doc, "initially_visible", JsonDataType.Boolean);

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
                    if (n.Name == "Widget")
                    {
                        //Transfer xsi:type attribute value to a <WidgetType> element before
                        //removing the attribute
                        var wtNode = doc.CreateElement("WidgetType");
                        wtNode.InnerText = attr.Value;
                        n.PrependChild(wtNode);
                    }
                    n.Attributes.Remove(attr);
                }
            }

            //var debugXml = doc.OuterXml;

            var rootEl = doc.GetElementsByTagName("ApplicationDefinition");
            var json = SerializeXmlNode(rootEl[0], Newtonsoft.Json.Formatting.Indented);
            var o = JObject.Parse(json);
            return o["ApplicationDefinition"].ToString();
        }

        /// <summary>
        /// Serializes the <see cref="XmlNode"/> to a JSON string using formatting.
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>A JSON string of the <see cref="XmlNode"/>.</returns>
        static string SerializeXmlNode(XmlNode node, Newtonsoft.Json.Formatting formatting)
        {
            var converter = new MyXmlNodeConverter();

            return JsonConvert.SerializeObject(node, formatting, converter);
        }
    }
}
