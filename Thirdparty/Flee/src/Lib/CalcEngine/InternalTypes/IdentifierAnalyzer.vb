'
' This library is free software; you can redistribute it and/or
' modify it under the terms of the GNU Lesser General Public License
' as published by the Free Software Foundation; either version 2.1
' of the License, or (at your option) any later version.
'
' This library is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
' Lesser General Public License for more details.
'
' You should have received a copy of the GNU Lesser General Public
' License along with this library; if not, write to the Free
' Software Foundation, Inc., 59 Temple Place, Suite 330, Boston,
' MA 02111-1307, USA.
'
'
' Copyright (c) 2007 Eugene Ciloci

Imports Ciloci.Flee.PerCederberg.Grammatica.Runtime

Friend Class IdentifierAnalyzer
	Inherits Analyzer

	Private MyIdentifiers As IDictionary(Of Integer, String)
	Private MyMemberExpressionCount As Integer
	Private MyInFieldPropertyExpression As Boolean

	Public Sub New()
		MyIdentifiers = New Dictionary(Of Integer, String)()
	End Sub

	Public Overrides Function [Exit](ByVal node As Node) As Node
		Select Case node.Id
			Case ExpressionConstants.IDENTIFIER
				Me.ExitIdentifier(CType(node, Token))
			Case ExpressionConstants.FIELD_PROPERTY_EXPRESSION
				Me.ExitFieldPropertyExpression()
		End Select

		Return node
	End Function

	Public Overrides Sub [Enter](ByVal node As Node)
		Select Case node.Id
			Case ExpressionConstants.MEMBER_EXPRESSION
				Me.EnterMemberExpression()
			Case ExpressionConstants.FIELD_PROPERTY_EXPRESSION
				Me.EnterFieldPropertyExpression()
		End Select
	End Sub

	Private Sub ExitIdentifier(ByVal node As Token)
		If MyInFieldPropertyExpression = False Then
			Return
		End If

		If MyIdentifiers.ContainsKey(MyMemberExpressionCount) = False Then
			MyIdentifiers.Add(MyMemberExpressionCount, node.Image)
		End If
	End Sub

	Private Sub EnterMemberExpression()
		MyMemberExpressionCount += 1
	End Sub

	Private Sub EnterFieldPropertyExpression()
		MyInFieldPropertyExpression = True
	End Sub

	Private Sub ExitFieldPropertyExpression()
		MyInFieldPropertyExpression = False
	End Sub

	Public Sub Reset()
		MyIdentifiers.Clear()
		MyMemberExpressionCount = -1
	End Sub

	Public Function GetIdentifiers(ByVal context As ExpressionContext) As ICollection(Of String)
		Dim dict As New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)
		Dim ei As ExpressionImports = context.Imports

		For Each identifier As String In MyIdentifiers.Values
			' Skip names registered as namespaces
			If ei.HasNamespace(identifier) = True Then
				Continue For
			ElseIf context.Variables.ContainsKey(identifier) = True Then
				' Identifier is a variable
				Continue For
			End If

			' Get only the unique values
			dict.Item(identifier) = Nothing
		Next

		Return dict.Keys
	End Function
End Class
