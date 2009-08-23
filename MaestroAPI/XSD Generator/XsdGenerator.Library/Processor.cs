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
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace XsdGenerator
{
	/// <summary>
	/// Processes WXS files and builds code for them.
	/// </summary>
	public sealed class Processor
	{
		public const string ExtensionNamespace = "http://weblogs.asp.net/cazzu";
		private static XPathExpression Extensions;

		static Processor() 
		{
			XPathNavigator nav = new XmlDocument().CreateNavigator();
			// Select all extension types.
			Extensions = nav.Compile( "/xs:schema/xs:annotation/xs:appinfo/kzu:Code/kzu:Extension/@Type" );
			
			// Create and set namespace resolution context.
			XmlNamespaceManager nsmgr = new XmlNamespaceManager( nav.NameTable );
			nsmgr.AddNamespace( "xs", XmlSchema.Namespace );
			nsmgr.AddNamespace( "kzu", ExtensionNamespace );
			Extensions.SetContext( nsmgr );
		}

		private Processor() {}

		/// <summary>
		/// Processes the schema.
		/// </summary>
		/// <param name="xsdFile">The full path to the WXS file to process.</param>
		/// <param name="targetNamespace">The namespace to put generated classes in.</param>
		/// <returns>The CodeDom tree generated from the schema.</returns>
		public static CodeNamespace Process( string xsdFile, string targetNamespace )
		{
			// Load the XmlSchema and its collection.
			XmlSchema xsd;
			using ( FileStream fs = new FileStream( xsdFile, FileMode.Open ) )
			{
				xsd = XmlSchema.Read( fs, null );
				xsd.Compile( null );
			}

			XmlSchemas schemas = new XmlSchemas();
			schemas.Add( xsd );

			// Create the importer for these schemas.
			XmlSchemaImporter importer = new XmlSchemaImporter( schemas );

			// System.CodeDom namespace for the XmlCodeExporter to put classes in.
			CodeNamespace ns = new CodeNamespace( targetNamespace );
			XmlCodeExporter exporter = new XmlCodeExporter( ns );

			// Iterate schema top-level elements and export code for each.
			foreach ( XmlSchemaElement element in xsd.Elements.Values )
			{
				// Import the mapping first.
				XmlTypeMapping mapping = importer.ImportTypeMapping( 
					element.QualifiedName );

				// Export the code finally.
				exporter.ExportTypeMapping( mapping );
			}

			new XsdGenerator.Extensions.ArraysToCollectionsExtension().Process( ns, xsd );
			new XsdGenerator.Extensions.FieldsToPropertiesExtension().Process( ns, xsd );

			#region Execute extensions
			
			XPathNavigator nav;
			using ( FileStream fs = new FileStream( xsdFile, FileMode.Open ) )
			{ 
				nav = new XPathDocument( fs ).CreateNavigator(); 
			}

			XPathNodeIterator it = nav.Select( Extensions );
			while ( it.MoveNext() )
			{
				Type t = Type.GetType( it.Current.Value, true );
				// Is the type an ICodeExtension?
				Type iface = t.GetInterface( typeof( ICodeExtension ).Name );
				if (iface == null)
					throw new ArgumentException( "Invalid extension type '" + it.Current.Value + "'." );

				ICodeExtension ext = ( ICodeExtension ) Activator.CreateInstance( t );
				// Run it!
				ext.Process( ns, xsd );
			}

			#endregion Execute extensions

			return ns;
		}
	}
}
