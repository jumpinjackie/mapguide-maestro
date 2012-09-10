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
	/// <summary>
	/// Overrides the default XmlWriter, to ensure that the Xml is Utf8 and with whitespaces, as the MapGuide server requires Utf8
	/// </summary>
	public class Utf8XmlWriter : System.Xml.XmlTextWriter
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Utf8XmlWriter"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
		public Utf8XmlWriter(System.IO.Stream s) 
            //This creation of the UTF8 encoder removes the BOM
            //Which is required because MapGuide has trouble reading files with a BOM.
			: base(s, new System.Text.UTF8Encoding(false, true)) 
		{
            Initialize();
		}

        private void Initialize()
        {
            //The MapGuide Studio parser is broken, it can't read Xml without whitespace :)
            base.Formatting = System.Xml.Formatting.Indented;
            base.Indentation = 2;
            base.IndentChar = ' ';
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Utf8XmlWriter"/> class.
        /// </summary>
        /// <param name="w">The TextWriter to write to. It is assumed that the TextWriter is already set to the correct encoding.</param>
		public Utf8XmlWriter(System.IO.TextWriter w) : base(w) 
        {
            Initialize();
        }

        /// <summary>
        /// Writes the XML declaration with the version "1.0".
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">
        /// This is not the first write method called after the constructor.
        /// </exception>
		public override void WriteStartDocument() {Utf8WriteHeader();}
        /// <summary>
        /// Writes the XML declaration with the version "1.0" and the standalone attribute.
        /// </summary>
        /// <param name="standalone">If true, it writes "standalone=yes"; if false, it writes "standalone=no".</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// This is not the first write method called after the constructor.
        /// </exception>
		public override void WriteStartDocument(bool standalone) {Utf8WriteHeader();}

		private void Utf8WriteHeader()
		{
            base.WriteRaw("<?xml version=\"1.0\" encoding=\"utf-8\"?>"); //NOXLATE
		}
	}
}
