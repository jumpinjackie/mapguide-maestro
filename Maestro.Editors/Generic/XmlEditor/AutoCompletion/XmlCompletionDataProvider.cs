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
using System.Windows.Forms;

namespace Maestro.Editors.Generic.XmlEditor.AutoCompletion
{
    /// <summary>
    /// Provides the autocomplete (intellisense) data for an
    /// xml document that specifies a known schema.
    /// </summary>
    internal class XmlCompletionDataProvider : ICompletionDataProvider
    {
        XmlSchemaCompletionDataCollection schemaCompletionDataItems;
        XmlSchemaCompletionData defaultSchemaCompletionData;
        string defaultNamespacePrefix = String.Empty;

        public XmlCompletionDataProvider(XmlSchemaCompletionDataCollection schemaCompletionDataItems, XmlSchemaCompletionData defaultSchemaCompletionData, string defaultNamespacePrefix)
        {
            this.schemaCompletionDataItems = schemaCompletionDataItems;
            this.defaultSchemaCompletionData = defaultSchemaCompletionData;
            this.defaultNamespacePrefix = defaultNamespacePrefix;
            DefaultIndex = 0;
        }
        
        static ImageList smImageList;

        public ImageList ImageList
        {
            get
            {
                if (smImageList == null)
                {
                    smImageList = new ImageList();
                    smImageList.Images.Add(Properties.Resources.document_code);
                }
                return smImageList;
            }
        }

        /// <summary>
        /// Overrides the default behaviour and allows special xml
        /// characters such as '.' and ':' to be used as completion data.
        /// </summary>
        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            if (key == '\r' || key == '\t')
            {
                return CompletionDataProviderKeyResult.InsertionKey;
            }
            return CompletionDataProviderKeyResult.NormalKey;
        }

        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            this.PreSelection = null;
            string text = String.Concat(textArea.Document.GetText(0, textArea.Caret.Offset), charTyped);

            switch (charTyped)
            {
                case '=':
                    // Namespace intellisense.
                    if (XmlParser.IsNamespaceDeclaration(text, text.Length))
                    {
                        return schemaCompletionDataItems.GetNamespaceCompletionData(); ;
                    }
                    break;
                case '<':
                    // Child element intellisense.
                    XmlElementPath parentPath = XmlParser.GetParentElementPath(text);
                    if (parentPath.Elements.Count > 0)
                    {
                        return GetChildElementCompletionData(parentPath);
                    }
                    else if (defaultSchemaCompletionData != null)
                    {
                        return defaultSchemaCompletionData.GetElementCompletionData(defaultNamespacePrefix);
                    }
                    break;

                case ' ':
                    // Attribute intellisense.
                    if (!XmlParser.IsInsideAttributeValue(text, text.Length))
                    {
                        XmlElementPath path = XmlParser.GetActiveElementStartPath(text, text.Length);
                        if (path.Elements.Count > 0)
                        {
                            return GetAttributeCompletionData(path);
                        }
                    }
                    break;

                default:

                    // Attribute value intellisense.
                    if (XmlParser.IsAttributeValueChar(charTyped))
                    {
                        string attributeName = XmlParser.GetAttributeName(text, text.Length);
                        if (attributeName.Length > 0)
                        {
                            XmlElementPath elementPath = XmlParser.GetActiveElementStartPath(text, text.Length);
                            if (elementPath.Elements.Count > 0)
                            {
                                this.PreSelection = charTyped.ToString();
                                return GetAttributeValueCompletionData(elementPath, attributeName);
                            }
                        }
                    }
                    break;
            }

            return null;
        }

        /// <summary>
        /// Finds the schema given the xml element path.
        /// </summary>
        public XmlSchemaCompletionData FindSchema(XmlElementPath path)
        {
            if (path.Elements.Count > 0)
            {
                string namespaceUri = path.Elements[0].Namespace;
                if (namespaceUri.Length > 0)
                {
                    return schemaCompletionDataItems[namespaceUri];
                }
                else if (defaultSchemaCompletionData != null)
                {

                    // Use the default schema namespace if none
                    // specified in a xml element path, otherwise
                    // we will not find any attribute or element matches
                    // later.
                    foreach (QualifiedName name in path.Elements)
                    {
                        if (name.Namespace.Length == 0)
                        {
                            name.Namespace = defaultSchemaCompletionData.NamespaceUri;
                        }
                    }
                    return defaultSchemaCompletionData;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the schema given a namespace URI.
        /// </summary>
        public XmlSchemaCompletionData FindSchema(string namespaceUri)
        {
            return schemaCompletionDataItems[namespaceUri];
        }

        /// <summary>
        /// Gets the schema completion data that was created from the specified 
        /// schema filename.
        /// </summary>
        public XmlSchemaCompletionData FindSchemaFromFileName(string fileName)
        {
            return schemaCompletionDataItems.GetSchemaFromFileName(fileName);
        }

        ICompletionData[] GetChildElementCompletionData(XmlElementPath path)
        {
            ICompletionData[] completionData = null;

            XmlSchemaCompletionData schema = FindSchema(path);
            if (schema != null)
            {
                completionData = schema.GetChildElementCompletionData(path);
            }

            return completionData;
        }

        ICompletionData[] GetAttributeCompletionData(XmlElementPath path)
        {
            ICompletionData[] completionData = null;

            XmlSchemaCompletionData schema = FindSchema(path);
            if (schema != null)
            {
                completionData = schema.GetAttributeCompletionData(path);
            }

            return completionData;
        }

        ICompletionData[] GetAttributeValueCompletionData(XmlElementPath path, string name)
        {
            ICompletionData[] completionData = null;

            XmlSchemaCompletionData schema = FindSchema(path);
            if (schema != null)
            {
                completionData = schema.GetAttributeValueCompletionData(path, name);
            }

            return completionData;
        }

        public string PreSelection
        {
            get;
            private set;
        }

        public int DefaultIndex
        {
            get;
            private set;
        }

        public bool InsertSpace
        {
            get;
            set;
        }

        public bool InsertAction(ICompletionData data, ICSharpCode.TextEditor.TextArea textArea, int insertionOffset, char key)
        {
            if (InsertSpace)
            {
                textArea.Document.Insert(insertionOffset++, " ");
            }
            textArea.Caret.Position = textArea.Document.OffsetToPosition(insertionOffset);

            return data.InsertAction(textArea, key);
        }
    }
}
