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

	///<summary>
	/// Class that makes XSD validation
	///</summary>
	public class XmlValidator
	{
        private List<string> warnings = new List<string>();
        private List<string> errors = new List<string>();

        public ReadOnlyCollection<string> ValidationWarnings
        {
            get { return this.warnings.AsReadOnly(); }
        }

        public ReadOnlyCollection<string> ValidationErrors
        {
            get { return this.errors.AsReadOnly(); }
        }

        /// <summary>
        /// Validates the specified XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="xsd">The XSD.</param>
		public void Validate(System.IO.Stream xml, XmlSchema xsd)
		{
            this.warnings.Clear();
            this.errors.Clear();

            var config = new XmlReaderSettings();
            if (xsd != null)
                config.Schemas.Add(xsd);
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
                    this.warnings.Add(string.Format(Properties.Resources.XmlValidationIssueTemplate, ex.LineNumber, ex.LinePosition, ex.Message));
                }
                else
                {
                    this.errors.Add(string.Format(Properties.Resources.XmlValidationIssueTemplate, ex.LineNumber, ex.LinePosition, ex.Message));
                }
            };

            using (var reader = XmlReader.Create(xml, config))
            {
                while (reader.Read()) { } //Trigger the validation
            }
		}
	}
}
