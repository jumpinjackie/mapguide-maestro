#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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

namespace OSGeo.MapGuide.MaestroAPI
{
    using System;
    using System.Collections;
    using System.Data;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;
    using System.Text;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using OSGeo.MapGuide.MaestroAPI.Resource;
    using OSGeo.MapGuide.MaestroAPI.Exceptions;

    ///<summary>
    /// Class that makes XSD validation
    ///</summary>
    public class XmlValidator
    {
        private List<string> warnings = new List<string>();
        private List<string> errors = new List<string>();

        /// <summary>
        /// Gets the validation warnings.
        /// </summary>
        public ReadOnlyCollection<string> ValidationWarnings
        {
            get { return this.warnings.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        public ReadOnlyCollection<string> ValidationErrors
        {
            get { return this.errors.AsReadOnly(); }
        }

        /// <summary>
        /// Validates the specified XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="xsds">The array of <see cref="T:System.Xml.Schema.XmlSchema"/> objects to validate against.</param>
        public void Validate(System.IO.Stream xml, XmlSchema[] xsds)
        {
            this.warnings.Clear();
            this.errors.Clear();

            var config = new XmlReaderSettings();
            if (xsds != null && xsds.Length > 0)
            {
                foreach(var xsd in xsds)
                    config.Schemas.Add(xsd);
            }
            config.ValidationType = ValidationType.Schema;
            config.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            config.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            config.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            //This will trap all the errors and warnings that are raised
            config.ValidationEventHandler += (s, e) =>
            {
                var ex = e.Exception;
                if (e.Severity == XmlSeverityType.Warning)
                {
                    this.warnings.Add(string.Format(Strings.XmlValidationIssueTemplate, ex.LineNumber, ex.LinePosition, ex.Message));
                }
                else
                {
                    this.errors.Add(string.Format(Strings.XmlValidationIssueTemplate, ex.LineNumber, ex.LinePosition, ex.Message));
                }
            };

            using (var reader = XmlReader.Create(xml, config))
            {
                while (reader.Read()) { } //Trigger the validation
            }
        }

        private static XmlSchema GetXsd(string xsdPath, string xsdFile)
        {
            string path = xsdFile;

            if (!string.IsNullOrEmpty(xsdPath))
                path = Path.Combine(xsdPath, xsdFile);

            if (File.Exists(path))
            {
                ValidationEventHandler handler = (s, e) =>
                {
                };
                return XmlSchema.Read(File.OpenRead(path), handler);
            }
            return null;
        }

        /// <summary>
        /// Validates the content of the resource XML.
        /// </summary>
        /// <param name="xmlContent">Content of the XML.</param>
        /// <param name="xsdPath">The XSD path.</param>
        /// <param name="errors">The errors.</param>
        /// <param name="warnings">The warnings.</param>
        public static void ValidateResourceXmlContent(string xmlContent, string xsdPath, out string[] errors, out string[] warnings)
        {
            errors = new string[0];
            warnings = new string[0];

            List<string> err = new List<string>();
            List<string> warn = new List<string>();

            //Test for well-formedness
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);
            }
            catch (XmlException ex)
            {
                err.Add(ex.Message);
            }

            IResource res = null;
            //Test for serializablility
            try
            {
                //Use original resource type to determine how to deserialize
                res = ResourceTypeRegistry.Deserialize(xmlContent);
            }
            catch (Exception ex)
            {
                if (!(ex is SerializationException))
                    err.Add(ex.Message);
                else
                    res = null;
            }

            if (res != null)
            {
                //Finally verify the content itself
                var xml = xmlContent;
                var xsds = new Dictionary<string, XmlSchema>();
                var xsd = GetXsd(xsdPath, res.ValidatingSchema);
                if (xsd != null)
                    xsds.Add(res.ValidatingSchema, xsd);
                else
                    return; //One or more schemas is not found. Cannot proceed. Let MG figure it out

                //HACK: Yes this is hard-coded because XmlSchema's dependency resolution sucks!

                //Nearly all relevant xsds include this anyway so add it to the set
                var pc = GetXsd(xsdPath, "PlatformCommon-1.0.0.xsd"); //NOXLATE
                if (pc != null)
                    xsds.Add("PlatformCommon-1.0.0.xsd", pc); //NOXLATE

                if (res.ResourceType == ResourceTypes.LayerDefinition)
                {
                    string version = res.ResourceVersion.ToString();
                    if (version.StartsWith("1.1.0")) //NOXLATE
                    {
                        var sym = GetXsd(xsdPath, "SymbolDefinition-1.0.0.xsd"); //NOXLATE
                        if (sym != null)
                            xsds.Add("SymbolDefinition-1.0.0.xsd", sym); //NOXLATE
                    }
                    else if (version.StartsWith("1.2.0") || version.StartsWith("1.3.0")) //NOXLATE
                    {
                        var sym = GetXsd(xsdPath, "SymbolDefinition-1.1.0.xsd"); //NOXLATE
                        if (sym != null)
                            xsds.Add("SymbolDefinition-1.1.0.xsd", sym); //NOXLATE
                    }
                    else if (version.StartsWith("2.3.0")) //NOXLATE
                    {
                        var wmd = GetXsd(xsdPath, "WatermarkDefinition-2.3.0.xsd"); //NOXLATE
                        if (wmd != null)
                            xsds.Add("WatermarkDefinition-2.3.0.xsd", wmd); //NOXLATE
                    }
                    else if (version.StartsWith("2.4.0")) //NOXLATE
                    {
                        var sym = GetXsd(xsdPath, "SymbolDefinition-2.4.0.xsd"); //NOXLATE
                        if (sym != null)
                            xsds.Add("SymbolDefinition-2.4.0.xsd", sym); //NOXLATE

                        var wmd = GetXsd(xsdPath, "WatermarkDefinition-2.4.0.xsd"); //NOXLATE
                        if (wmd != null)
                            xsds.Add("WatermarkDefinition-2.4.0.xsd", wmd); //NOXLATE
                    }
                }

                if (res.ResourceType == ResourceTypes.WatermarkDefinition)
                {
                    string version = res.ResourceVersion.ToString();
                    if (version.StartsWith("2.3.0")) //NOXLATE
                    {
                        var sym = GetXsd(xsdPath, "SymbolDefinition-1.1.0.xsd"); //NOXLATE
                        if (sym != null)
                            xsds.Add("SymbolDefinition-1.1.0.xsd", sym); //NOXLATE
                    }
                    else if (version.StartsWith("2.4.0"))
                    {
                        var sym = GetXsd(xsdPath, "SymbolDefinition-2.4.0.xsd"); //NOXLATE
                        if (sym != null)
                            xsds.Add("SymbolDefinition-2.4.0.xsd", sym); //NOXLATE
                    }
                }

                if (res.ResourceType == ResourceTypes.MapDefinition)
                {
                    string version = res.ResourceVersion.ToString();
                    if (version.StartsWith("2.3.0"))
                    {
                        var sym = GetXsd(xsdPath, "SymbolDefinition-1.1.0.xsd"); //NOXLATE
                        if (sym != null)
                            xsds.Add("SymbolDefinition-1.1.0.xsd", sym); //NOXLATE

                        var wmd = GetXsd(xsdPath, "WatermarkDefinition-2.3.0.xsd"); //NOXLATE
                        if (wmd != null)
                            xsds.Add("WatermarkDefinition-2.3.0.xsd", wmd); //NOXLATE
                    }
                    else if (version.StartsWith("2.4.0"))
                    {
                        var sym = GetXsd(xsdPath, "SymbolDefinition-2.4.0.xsd"); //NOXLATE
                        if (sym != null)
                            xsds.Add("SymbolDefinition-2.4.0.xsd", sym); //NOXLATE

                        var wmd = GetXsd(xsdPath, "WatermarkDefinition-2.4.0.xsd"); //NOXLATE
                        if (wmd != null)
                            xsds.Add("WatermarkDefinition-2.4.0.xsd", wmd); //NOXLATE
                    }
                }

                string xsdName = res.ResourceType.ToString() + "-" + res.ResourceVersion.ToString() + ".xsd"; //NOXLATE
                if (!xsds.ContainsKey(xsdName))
                {
                    var schemaObj = GetXsd(xsdPath, xsdName);
                    if (schemaObj != null)
                        xsds.Add(xsdName, schemaObj);
                }

                var validator = new XmlValidator();
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    validator.Validate(ms, new List<XmlSchema>(xsds.Values).ToArray());
                }

                err.AddRange(validator.ValidationErrors);
                warn.AddRange(validator.ValidationWarnings);
            }

            errors = err.ToArray();
            warnings = warn.ToArray();
        }
    }
}
