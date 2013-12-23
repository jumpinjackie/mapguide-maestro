#region Disclaimer / License
// Copyright (C) 2013, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
// 
// Original code from SharpDevelop 3.2.1 licensed under the same terms (LGPL 2.1)
// Copyright 2002-2010 by
//
//  AlphaSierraPapa, Christoph Wille
//  Vordernberger Strasse 27/8
//  A-8700 Leoben
//  Austria
//
//  email: office@alphasierrapapa.com
//  court of jurisdiction: Landesgericht Leoben
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
using System.Linq;
using System.Text;

namespace Maestro.Editors.Generic.XmlEditor.AutoCompletion
{
    /// <summary>
    /// Represents an association between an xml schema and a file extension.
    /// </summary>
    internal class XmlSchemaAssociation //: IXmlConvertable
    {
        string namespaceUri = String.Empty;
        string extension = String.Empty;
        string namespacePrefix = String.Empty;

        public XmlSchemaAssociation(string extension)
            : this(extension, String.Empty, String.Empty)
        {
        }

        public XmlSchemaAssociation(string extension, string namespaceUri)
            : this(extension, namespaceUri, String.Empty)
        {
        }

        public XmlSchemaAssociation(string extension, string namespaceUri, string namespacePrefix)
        {
            this.extension = extension;
            this.namespaceUri = namespaceUri;
            this.namespacePrefix = namespacePrefix;
        }

        public string NamespaceUri
        {
            get
            {
                return namespaceUri;
            }

            set
            {
                namespaceUri = value;
            }
        }

        /// <summary>
        /// Gets or sets the file extension (e.g. '.xml').
        /// </summary>
        public string Extension
        {
            get
            {
                return extension;
            }

            set
            {
                extension = value;
            }
        }

        /// <summary>
        /// Gets or sets the default namespace prefix that will be added
        /// to the xml elements.
        /// </summary>
        public string NamespacePrefix
        {
            get
            {
                return namespacePrefix;
            }

            set
            {
                namespacePrefix = value;
            }
        }

        /// <summary>
        /// Gets the default schema association for the file extension. 
        /// </summary>
        /// <remarks>
        /// These defaults are hard coded.
        /// </remarks>
        public static XmlSchemaAssociation GetDefaultAssociation(string extension)
        {
            XmlSchemaAssociation association = null;

            switch (extension.ToLowerInvariant())
            {
                case ".wxs":
                    association = new XmlSchemaAssociation(extension, @"http://schemas.microsoft.com/wix/2003/01/wi");
                    break;
                case ".config":
                    association = new XmlSchemaAssociation(extension, @"urn:app-config");
                    break;
                case ".build":
                    association = new XmlSchemaAssociation(extension, @"http://nant.sf.net/release/0.85/nant.xsd");
                    break;
                case ".addin":
                    association = new XmlSchemaAssociation(extension, @"http://www.icsharpcode.net/2005/addin");
                    break;
                case ".xsl":
                case ".xslt":
                    association = new XmlSchemaAssociation(extension, @"http://www.w3.org/1999/XSL/Transform", "xsl");
                    break;
                case ".xsd":
                    association = new XmlSchemaAssociation(extension, @"http://www.w3.org/2001/XMLSchema", "xs");
                    break;
                case ".manifest":
                    association = new XmlSchemaAssociation(extension, @"urn:schemas-microsoft-com:asm.v1");
                    break;
                case ".xaml":
                    association = new XmlSchemaAssociation(extension, @"http://schemas.microsoft.com/winfx/avalon/2005");
                    break;
                default:
                    association = new XmlSchemaAssociation(extension);
                    break;
            }
            return association;
        }

        /// <summary>
        /// Two schema associations are considered equal if their file extension,
        /// prefix and namespaceUri are the same.
        /// </summary>
        public override bool Equals(object obj)
        {
            bool equals = false;

            XmlSchemaAssociation rhs = obj as XmlSchemaAssociation;
            if (rhs != null)
            {
                if ((this.namespacePrefix == rhs.namespacePrefix) &&
                    (this.extension == rhs.extension) &&
                    (this.namespaceUri == rhs.namespaceUri))
                {
                    equals = true;
                }
            }

            return equals;
        }

        public override int GetHashCode()
        {
            return (namespaceUri != null ? namespaceUri.GetHashCode() : 0) ^ (extension != null ? extension.GetHashCode() : 0) ^ (namespacePrefix != null ? namespacePrefix.GetHashCode() : 0);
        }

        /// <summary>
        /// Creates an XmlSchemaAssociation from the saved xml.
        /// </summary>
        public static XmlSchemaAssociation ConvertFromString(string text)
        {
            string[] parts = text.Split(new char[] { '|' }, 3);
            return new XmlSchemaAssociation(parts[0], parts[1], parts[2]);
        }

        public string ConvertToString()
        {
            return extension + "|" + namespaceUri + "|" + namespacePrefix;
        }
    }
}
