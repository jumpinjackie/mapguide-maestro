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
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maestro.Editors.Generic.XmlEditor.AutoCompletion
{
    /// <summary>
    /// Holds the text for  namespace, child element or attribute 
    /// autocomplete (intellisense).
    /// </summary>
    internal class XmlCompletionData : ICompletionData
    {
        string text;
        DataType dataType = DataType.XmlElement;
        string description = String.Empty;

        /// <summary>
        /// The type of text held in this object.
        /// </summary>
        public enum DataType
        {
            XmlElement = 1,
            XmlAttribute = 2,
            NamespaceUri = 3,
            XmlAttributeValue = 4
        }

        public XmlCompletionData(string text)
            : this(text, String.Empty, DataType.XmlElement)
        {
        }

        public XmlCompletionData(string text, string description)
            : this(text, description, DataType.XmlElement)
        {
        }

        public XmlCompletionData(string text, DataType dataType)
            : this(text, String.Empty, dataType)
        {
        }

        public XmlCompletionData(string text, string description, DataType dataType)
        {
            this.text = text;
            this.description = description;
            this.dataType = dataType;
        }

        public int ImageIndex
        {
            get
            {
                return 0;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        /// <summary>
        /// Returns the xml item's documentation as retrieved from
        /// the xs:annotation/xs:documentation element.
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
        }

        public double Priority
        {
            get
            {
                return 0;
            }
        }

        public bool InsertAction(TextArea textArea, char ch)
        {
            if ((dataType == DataType.XmlElement) || (dataType == DataType.XmlAttributeValue))
            {
                textArea.InsertString(text);
            }
            else if (dataType == DataType.NamespaceUri)
            {
                textArea.InsertString(String.Concat("\"", text, "\""));
            }
            else
            {
                // Insert an attribute.
                Caret caret = textArea.Caret;
                textArea.InsertString(String.Concat(text, "=\"\""));

                // Move caret into the middle of the attribute quotes.
                caret.Position = textArea.Document.OffsetToPosition(caret.Offset - 1);
            }
            return false;
        }
    }
}
