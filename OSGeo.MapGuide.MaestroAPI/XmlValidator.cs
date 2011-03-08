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

	///<summary>
	/// Class that makes XSD validation
	///</summary>
	public class XMLValidator
	{
		// Validation Error Count
		private int ErrorsCount = 0;

		// Validation Error Message
		private string ErrorMessage = "";

        /// <summary>
        /// Default validation handler method
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Xml.Schema.ValidationEventArgs"/> instance containing the event data.</param>
		public void ValidationHandler(object sender,
			ValidationEventArgs args)
		{
			ErrorMessage = ErrorMessage + args.Message + "\r\n";
			ErrorsCount ++;
		}

        /// <summary>
        /// Validates the specified XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="xsd">The XSD.</param>
		public void Validate(System.IO.Stream xml, XmlSchema xsd)
		{
			// Declare local objects
			XmlValidatingReader vr = new XmlValidatingReader( xml, XmlNodeType.Document, null);
			vr.Schemas.Add(xsd);

			// Add validation event handler
			vr.ValidationType = ValidationType.Schema;
			vr.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

			// Validate XML data
			while(vr.Read());

			// Raise exception, if XML validation fails
			if (ErrorsCount > 0)
				throw new Exception(ErrorMessage);
		}
	}
}
