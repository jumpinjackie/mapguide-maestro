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
' Flee - Fast Lightweight Expression Evaluator
' Copyright © 2007 Eugene Ciloci
'

Imports System.IO

Namespace CalcEngine

	Public Class SimpleCalcEngine

#Region "Fields"

		Private MyExpressions As IDictionary(Of String, IExpression)
		Private MyContext As ExpressionContext

#End Region

#Region "Constructor"

		Public Sub New()
			MyExpressions = New Dictionary(Of String, IExpression)(StringComparer.OrdinalIgnoreCase)
			MyContext = New ExpressionContext()
		End Sub

#End Region

#Region "Methods - Private"

		Private Sub AddCompiledExpression(ByVal expressionName As String, ByVal expression As IExpression)
			If MyExpressions.ContainsKey(expressionName) = True Then
				Throw New InvalidOperationException(String.Format("The calc engine already contains an expression named '{0}'", expressionName))
			Else
				MyExpressions.Add(expressionName, expression)
			End If
		End Sub

		Private Function ParseAndLink(ByVal expressionName As String, ByVal expression As String) As ExpressionContext
			Dim analyzer As IdentifierAnalyzer = Context.ParseIdentifiers(expression)

			Dim context2 As ExpressionContext = MyContext.CloneInternal(True)
			Me.LinkExpression(expressionName, context2, analyzer)

			' Tell the expression not to clone the context since it's already been cloned
			context2.NoClone = True

			' Clear our context's variables
			MyContext.Variables.Clear()

			Return context2
		End Function

		Private Sub LinkExpression(ByVal expressionName As String, ByVal context As ExpressionContext, ByVal analyzer As IdentifierAnalyzer)
			For Each identifier As String In analyzer.GetIdentifiers(context)
				Me.LinkIdentifier(identifier, expressionName, context)
			Next
		End Sub

		Private Sub LinkIdentifier(ByVal identifier As String, ByVal expressionName As String, ByVal context As ExpressionContext)
			Dim child As IExpression = Nothing

			If MyExpressions.TryGetValue(identifier, child) = False Then
				Dim msg As String = String.Format("Expression '{0}' references unknown name '{1}'", expressionName, identifier)
				Throw New InvalidOperationException(msg)
			End If

			context.Variables.Add(identifier, child)
		End Sub

#End Region

#Region "Methods - Public"

		Public Sub AddDynamic(ByVal expressionName As String, ByVal expression As String)
			Dim linkedContext As ExpressionContext = Me.ParseAndLink(expressionName, expression)
			Dim e As IExpression = linkedContext.CompileDynamic(expression)
			Me.AddCompiledExpression(expressionName, e)
		End Sub

		Public Sub AddGeneric(Of T)(ByVal expressionName As String, ByVal expression As String)
			Dim linkedContext As ExpressionContext = Me.ParseAndLink(expressionName, expression)
			Dim e As IExpression = linkedContext.CompileGeneric(Of T)(expression)
			Me.AddCompiledExpression(expressionName, e)
		End Sub

		Public Sub Clear()
			MyExpressions.Clear()
		End Sub

#End Region

#Region "Properties - Public"

		Default Public Overloads ReadOnly Property Item(ByVal name As String) As IExpression
			Get
				Dim e As IExpression = Nothing
				MyExpressions.TryGetValue(name, e)
				Return e
			End Get
		End Property

		Public Property Context() As ExpressionContext
			Get
				Return MyContext
			End Get
			Set(ByVal value As ExpressionContext)
				MyContext = value
			End Set
		End Property

#End Region
	End Class

End Namespace