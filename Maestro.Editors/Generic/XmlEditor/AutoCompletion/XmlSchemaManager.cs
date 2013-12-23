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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Maestro.Editors.Generic.XmlEditor.AutoCompletion
{
    /// <summary>
    /// Keeps track of all the schemas that the Xml Editor is aware
    /// of.
    /// </summary>
    internal class XmlSchemaManager
    {
        public const string XmlSchemaNamespace = "http://www.w3.org/2001/XMLSchema";

        static XmlSchemaCompletionDataCollection schemas = null;
        static XmlSchemaManager manager = null;

        public static event EventHandler UserSchemaAdded;

        public static event EventHandler UserSchemaRemoved;

        XmlSchemaManager()
        {
        }

        /// <summary>
        /// Determines whether the specified namespace is actually the W3C namespace for
        /// XSD files.
        /// </summary>
        public static bool IsXmlSchemaNamespace(string schemaNamespace)
        {
            return schemaNamespace == XmlSchemaNamespace;
        }

        public static XmlSchemaManager Instance
        {
            get
            {
                if (manager == null)
                    manager = new XmlSchemaManager();

                if (schemas == null)
                    schemas = new XmlSchemaCompletionDataCollection();

                return manager;
            }
        }

        /// <summary>
        /// Gets the schemas that SharpDevelop knows about.
        /// </summary>
        internal XmlSchemaCompletionDataCollection SchemaCompletionDataItems
        {
            get
            {
                return schemas;
            }
        }

        /// <summary>
        /// Gets the schema completion data that is associated with the
        /// specified file extension.
        /// </summary>
        internal XmlSchemaCompletionData GetSchemaCompletionData(string extension)
        {
            XmlSchemaCompletionData data = null;

            XmlSchemaAssociation association = GetSchemaAssociation(extension);
            if (association != null)
            {
                if (association.NamespaceUri.Length > 0)
                {
                    data = SchemaCompletionDataItems[association.NamespaceUri];
                }
            }
            return data;
        }

        /// <summary>
        /// Gets the namespace prefix that is associated with the
        /// specified file extension.
        /// </summary>
        public static string GetNamespacePrefix(string extension)
        {
            string prefix = String.Empty;

            XmlSchemaAssociation association = GetSchemaAssociation(extension);
            if (association != null)
            {
                prefix = association.NamespacePrefix;
            }

            return prefix;
        }

        /// <summary>
        /// Gets an association between a schema and a file extension.
        /// </summary>
        /// <remarks>
        /// <para>The property will be an xml element when the SharpDevelopProperties.xml
        /// is read on startup.  The property will be a schema association
        /// if the user changes the schema associated with the file
        /// extension in tools->options.</para>
        /// <para>The normal way of doing things is to
        /// pass the GetProperty method a default value which auto-magically
        /// turns the xml element into a schema association so we would not 
        /// have to check for both.  In this case, however, I do not want
        /// a default saved to the SharpDevelopProperties.xml file unless the user
        /// makes a change using Tools->Options.</para>
        /// <para>If we have a file extension that is currently missing a default 
        /// schema then if we  ship the schema at a later date the association will 
        /// be updated by the code if the user has not changed the settings themselves. 
        /// </para>
        /// <para>For example, the initial release of the xml editor add-in had
        /// no default schema for .xsl files, by default it was associated with
        /// no schema and this setting is saved if the user ever viewed the settings
        /// in the tools->options dialog.  Now, after the initial release the
        /// .xsl schema was created and shipped with SharpDevelop, there is
        /// no way to associate this schema to .xsl files by default since 
        /// the property exists in the SharpDevelopProperties.xml file.</para>
        /// <para>An alternative way of doing this might be to have the
        /// config info in the schema itself, which a special SharpDevelop 
        /// namespace.  I believe this is what Visual Studio does.  This
        /// way is not as flexible since it requires the user to locate
        /// the schema and change the association manually.</para>
        /// </remarks>
        static XmlSchemaAssociation GetSchemaAssociation(string extension)
        {
            extension = extension.ToLower();
            return XmlSchemaAssociation.GetDefaultAssociation(extension);
        }

        /// <summary>
        /// Removes the schema with the specified namespace from the
        /// user schemas folder and removes the completion data.
        /// </summary>
        public void RemoveUserSchema(string namespaceUri)
        {
            XmlSchemaCompletionData schemaData = SchemaCompletionDataItems[namespaceUri];
            if (schemaData != null)
            {
                if (File.Exists(schemaData.FileName))
                {
                    File.Delete(schemaData.FileName);
                }
                SchemaCompletionDataItems.Remove(schemaData);
                OnUserSchemaRemoved();
            }
        }

        /*
        /// <summary>
        /// Adds the schema to the user schemas folder and makes the
        /// schema available to the xml editor.
        /// </summary>
        public static void AddUserSchema(XmlSchemaCompletionData schemaData)
        {
            if (SchemaCompletionDataItems[schemaData.NamespaceUri] == null)
            {

                if (!Directory.Exists(UserSchemaFolder))
                {
                    Directory.CreateDirectory(UserSchemaFolder);
                }

                string fileName = Path.GetFileName(schemaData.FileName);
                string destinationFileName = Path.Combine(UserSchemaFolder, fileName);
                File.Copy(schemaData.FileName, destinationFileName);
                schemaData.FileName = destinationFileName;
                SchemaCompletionDataItems.Add(schemaData);
                OnUserSchemaAdded();
            }
            else
            {
                Debug.WriteLine("Trying to add a schema that already exists.  Namespace=" + schemaData.NamespaceUri);
            }
        }
        
        /// <summary>
        /// Reads the system and user added schemas.
        /// </summary>
        void ReadSchemas()
        {
            // MSBuild schemas are in framework directory:
            ReadSchemas(RuntimeEnvironment.GetRuntimeDirectory(), true);
            ReadSchemas(SchemaFolder, true);
            ReadSchemas(UserSchemaFolder, false);
        }
         */

        /// <summary>
        /// Reads all .xsd files in the specified folder.
        /// </summary>
        public void ReadSchemas(string folder, bool readOnly)
        {
            if (Directory.Exists(folder))
            {
                foreach (string fileName in Directory.GetFiles(folder, "*.xsd"))
                {
                    ReadSchema(fileName, readOnly);
                }
            }
        }

        /// <summary>
        /// Reads an individual schema and adds it to the collection.
        /// </summary>
        /// <remarks>
        /// If the schema namespace exists in the collection it is not added.
        /// </remarks>
        public void ReadSchema(string fileName, bool readOnly)
        {
            try
            {
                string baseUri = XmlSchemaCompletionData.GetUri(fileName);
                XmlSchemaCompletionData data = new XmlSchemaCompletionData(baseUri, fileName);
                if (data.NamespaceUri != null)
                {
                    if (schemas[data.NamespaceUri] == null)
                    {
                        data.ReadOnly = readOnly;
                        schemas.Add(data);
                    }
                    else
                    {
                        // Namespace already exists.
                        Debug.WriteLine("Ignoring duplicate schema namespace " + data.NamespaceUri);
                    }
                }
                else
                {
                    Debug.WriteLine("Ignoring schema with no namespace " + data.FileName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to read schema '" + fileName + "'. ", ex);
            }
        }

        /*
        /// <summary>
        /// Gets the folder where the schemas for all users on the
        /// local machine are stored.
        /// </summary>
        static string SchemaFolder
        {
            get
            {
                return Path.Combine(PropertyService.DataDirectory, "schemas");
            }
        }

        /// <summary>
        /// Gets the folder where schemas are stored for an individual user.
        /// </summary>
        static string UserSchemaFolder
        {
            get
            {
                return Path.Combine(PropertyService.ConfigDirectory, "schemas");
            }
        }
        */

        /// <summary>
        /// Should really pass schema info with the event.
        /// </summary>
        static void OnUserSchemaAdded()
        {
            if (UserSchemaAdded != null)
            {
                UserSchemaAdded(manager, new EventArgs());
            }
        }

        /// <summary>
        /// Should really pass schema info with the event.
        /// </summary>
        static void OnUserSchemaRemoved()
        {
            if (UserSchemaRemoved != null)
            {
                UserSchemaRemoved(manager, new EventArgs());
            }
        }
    }
}
