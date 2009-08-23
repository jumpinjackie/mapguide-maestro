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
using System.Collections;
using System.Xml.Schema;

namespace XsdGenerator.Extensions
{
	/// <summary>
	/// Converts array-based properties into collection-based ones, and 
	/// creates a typed <see cref="CollectionBase"/> inherited class for it.
	/// </summary>
	public class ArraysToCollectionsExtension : ICodeExtension
	{
		#region ICodeExtension Members

		public void Process( CodeNamespace code, XmlSchema schema )
		{
			// Copy as we will be adding types.
			CodeTypeDeclaration[] types = 
				new CodeTypeDeclaration[code.Types.Count];
			code.Types.CopyTo( types, 0 );

			foreach ( CodeTypeDeclaration type in types )
			{
				if ( type.IsClass || type.IsStruct )
				{
					foreach ( CodeTypeMember member in type.Members )
					{
						// Process fields only.
						if ( member is CodeMemberField && 
							( ( CodeMemberField )member ).Type.ArrayElementType != null )
						{
							CodeMemberField field = ( CodeMemberField ) member;
							CodeTypeDeclaration col = GetCollection( 
								field.Type.ArrayElementType );
							// Change field type to collection.
							field.Type = new CodeTypeReference( col.Name );
							code.Types.Add( col );
						}
					}
				}
			}
		}

		#endregion

		public CodeTypeDeclaration GetCollection( CodeTypeReference forType )
		{
			CodeTypeDeclaration col = new CodeTypeDeclaration( 
				forType.BaseType + "Collection" );
			col.BaseTypes.Add( typeof( CollectionBase ) );
			col.Attributes = MemberAttributes.Final | MemberAttributes.Public;

			// Add method
			CodeMemberMethod add = new CodeMemberMethod();
			add.Attributes = MemberAttributes.Final | MemberAttributes.Public;
			add.Name = "Add";
			add.ReturnType = new CodeTypeReference( typeof( int ) );
			add.Parameters.Add( new CodeParameterDeclarationExpression (
				forType, "value" ) );
			// Generates: return base.InnerList.Add( value );
			add.Statements.Add( new CodeMethodReturnStatement (
				new CodeMethodInvokeExpression( 
					new CodePropertyReferenceExpression( 
						new CodeBaseReferenceExpression(), "InnerList" ), 
					"Add", 
					new CodeExpression[] 
						{ new CodeArgumentReferenceExpression( "value" ) } 
					 )
				 )
			 );

			// Add to type.
			col.Members.Add( add );

			// Indexer property ( 'this' )
			CodeMemberProperty indexer = new CodeMemberProperty();
			indexer.Attributes = MemberAttributes.Final | MemberAttributes.Public;
			indexer.Name = "Item";
			indexer.Type = forType;
			indexer.Parameters.Add( new CodeParameterDeclarationExpression (
				typeof( int ), "idx" ) );
			indexer.HasGet = true;
			indexer.HasSet = true;
			// Generates: return ( theType ) base.InnerList[idx];
			indexer.GetStatements.Add( 
				new CodeMethodReturnStatement (
					new CodeCastExpression( 
						forType, 
						new CodeIndexerExpression( 
							new CodePropertyReferenceExpression( 
								new CodeBaseReferenceExpression(), 
								"InnerList" ), 
							new CodeExpression[] 
								{ new CodeArgumentReferenceExpression( "idx" ) } ) 
						 )
					 )
				 );
			// Generates: base.InnerList[idx] = value;
			indexer.SetStatements.Add( 
				new CodeAssignStatement( 
					new CodeIndexerExpression( 
						new CodePropertyReferenceExpression( 
							new CodeBaseReferenceExpression(), 
							"InnerList" ), 
						new CodeExpression[] 
							{ new CodeArgumentReferenceExpression( "idx" ) } ), 
					new CodeArgumentReferenceExpression( "value" )
				 )
			 );

			// Add to type.
			col.Members.Add( indexer );

			return col;
		}
	}
}
