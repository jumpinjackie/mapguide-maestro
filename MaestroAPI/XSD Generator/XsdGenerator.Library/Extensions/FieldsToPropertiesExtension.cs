#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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

namespace XsdGenerator.Extensions
{
	/// <summary>
	/// Converts the default public fields into properties backed by a private field.
	/// </summary>
	public class FieldsToPropertiesExtension : ICodeExtension
	{
		#region ICodeExtension Members

		public void Process( System.CodeDom.CodeNamespace code, System.Xml.Schema.XmlSchema schema )
		{
			foreach ( CodeTypeDeclaration type in code.Types )
			{
				if ( type.IsClass || type.IsStruct )
				{
					// Copy the colletion to an array for safety. We will be 
					// changing this collection.
					CodeTypeMember[] members = new CodeTypeMember[type.Members.Count];
					type.Members.CopyTo( members, 0 );

					foreach ( CodeTypeMember member in members )
					{
						// Process fields only.
						if ( member is CodeMemberField )
						{
							CodeMemberProperty prop = new CodeMemberProperty();
							prop.Name = member.Name;

							prop.Attributes = member.Attributes;
							prop.Type = ( ( CodeMemberField )member ).Type;

							// Copy attributes from field to the property.
							prop.CustomAttributes.AddRange( member.CustomAttributes );
							member.CustomAttributes.Clear();

							// Copy comments from field to the property.
							prop.Comments.AddRange( member.Comments );
							member.Comments.Clear();

							// Modify the field.
							member.Attributes = MemberAttributes.Private;
							Char[] letters = member.Name.ToCharArray();
							letters[0] = Char.ToLower(letters[0]);
							member.Name = String.Concat( "m_", new string(letters ) );

							prop.HasGet = true;
							prop.HasSet = true;

							// Add get/set statements pointing to field. Generates:
							// return this.m_fieldname;
							prop.GetStatements.Add( 
								new CodeMethodReturnStatement( 
								new CodeFieldReferenceExpression( new CodeThisReferenceExpression(), 
								member.Name ) ) );
							// Generates:
							// this.m_fieldname = value;
							prop.SetStatements.Add(
								new CodeAssignStatement(
								new CodeFieldReferenceExpression( new CodeThisReferenceExpression(),
								member.Name ), 
								new CodeArgumentReferenceExpression( "value" ) ) );

							// Finally add the property to the type
							type.Members.Add( prop );
						}
					}
				}
			}

		}
			#endregion Turn Public Fields into Properties
	}
}
