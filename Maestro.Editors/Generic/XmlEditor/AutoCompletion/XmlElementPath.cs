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
    /// Represents the path to an xml element starting from the root of the
    /// document.
    /// </summary>
    internal class XmlElementPath
    {
        QualifiedNameCollection elements = new QualifiedNameCollection();

        public XmlElementPath()
        {
        }

        /// <summary>
        /// Gets the elements specifying the path.
        /// </summary>
        /// <remarks>The order of the elements determines the path.</remarks>
        public QualifiedNameCollection Elements
        {
            get { return elements; }
        }

        /// <summary>
        /// Compacts the path so it only contains the elements that are from 
        /// the namespace of the last element in the path. 
        /// </summary>
        /// <remarks>This method is used when we need to know the path for a
        /// particular namespace and do not care about the complete path.
        /// </remarks>
        public void Compact()
        {
            if (elements.Count > 0)
            {
                QualifiedName lastName = Elements[Elements.Count - 1];
                if (lastName != null)
                {
                    int index = FindNonMatchingParentElement(lastName.Namespace);
                    if (index != -1)
                    {
                        RemoveParentElements(index);
                    }
                }
            }
        }

        /// <summary>
        /// An xml element path is considered to be equal if 
        /// each path item has the same name and namespace.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is XmlElementPath)) return false;
            if (this == obj) return true;

            XmlElementPath rhs = (XmlElementPath)obj;
            if (elements.Count == rhs.elements.Count)
            {

                for (int i = 0; i < elements.Count; ++i)
                {
                    if (!elements[i].Equals(rhs.elements[i]))
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return elements.GetHashCode();
        }


        /// <summary>
        /// Gets a string that represents the XmlElementPath.
        /// </summary>
        public override string ToString()
        {
            if (elements.Count > 0)
            {
                StringBuilder toString = new StringBuilder();
                int lastIndex = elements.Count - 1;
                for (int i = 0; i < elements.Count; ++i)
                {
                    string elementToString = GetElementToString(elements[i]);
                    if (i == lastIndex)
                    {
                        toString.Append(elementToString);
                    }
                    else
                    {
                        toString.Append(elementToString);
                        toString.Append(" > ");
                    }
                }
                return toString.ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// Removes elements up to and including the specified index.
        /// </summary>
        void RemoveParentElements(int index)
        {
            while (index >= 0)
            {
                --index;
                elements.RemoveFirst();
            }
        }

        /// <summary>
        /// Finds the first parent that does belong in the specified
        /// namespace.
        /// </summary>
        int FindNonMatchingParentElement(string namespaceUri)
        {
            int index = -1;

            if (elements.Count > 1)
            {
                // Start the check from the the last but one item.
                for (int i = elements.Count - 2; i >= 0; --i)
                {
                    QualifiedName name = elements[i];
                    if (name.Namespace != namespaceUri)
                    {
                        index = i;
                        break;
                    }
                }
            }
            return index;
        }

        /// <summary>
        /// Returns the qualified name as a string. If the name has a 
        /// prefix then it returns "prefix:element" otherwise it returns
        /// just the element name.
        /// </summary>
        static string GetElementToString(QualifiedName name)
        {
            if (name.Prefix.Length > 0)
            {
                return name.Prefix + ":" + name.Name;
            }
            return name.Name;
        }
    }
}
