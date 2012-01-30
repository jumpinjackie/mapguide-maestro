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

Imports System.Reflection.Emit

Namespace CalcEngine

	Friend Class PairEqualityComparer
		Inherits EqualityComparer(Of ExpressionResultPair)

		Public Overloads Overrides Function Equals(ByVal x As ExpressionResultPair, ByVal y As ExpressionResultPair) As Boolean
			Return String.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase)
		End Function

		Public Overloads Overrides Function GetHashCode(ByVal obj As ExpressionResultPair) As Integer
			Return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name)
		End Function
	End Class

	Friend MustInherit Class ExpressionResultPair

		Private MyName As String
		Protected MyExpression As IDynamicExpression

		Protected Sub New()

		End Sub

		Public MustOverride Sub Recalculate()

		Public Sub SetExpression(ByVal e As IDynamicExpression)
			MyExpression = e
		End Sub

		Public Sub SetName(ByVal name As String)
			MyName = name
		End Sub

		Public Overrides Function ToString() As String
			Return MyName
		End Function

		Public ReadOnly Property Name() As String
			Get
				Return MyName
			End Get
		End Property

		Public MustOverride ReadOnly Property ResultType() As Type
		Public MustOverride ReadOnly Property ResultAsObject() As Object

		Public ReadOnly Property Expression() As IDynamicExpression
			Get
				Return MyExpression
			End Get
		End Property
	End Class

	Friend Class GenericExpressionResultPair(Of T)
		Inherits ExpressionResultPair

		Public MyResult As T

		Public Sub New()

		End Sub

		Public Overrides Sub Recalculate()
			MyResult = DirectCast(MyExpression.Evaluate(), T)
		End Sub

		Public ReadOnly Property Result() As T
			Get
				Return MyResult
			End Get
		End Property

		Public Overrides ReadOnly Property ResultType() As System.Type
			Get
				Return GetType(T)
			End Get
		End Property

		Public Overrides ReadOnly Property ResultAsObject() As Object
			Get
				Return MyResult
			End Get
		End Property
	End Class

	Friend Class BatchLoadInfo
		Public Name As String
		Public ExpressionText As String
		Public Context As ExpressionContext

		Public Sub New(ByVal name As String, ByVal text As String, ByVal context As ExpressionContext)
			Me.Name = name
			Me.ExpressionText = text
			Me.Context = context
		End Sub
	End Class

	''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="NodeEventArgs"]/*' />	
	Public NotInheritable Class NodeEventArgs
		Inherits EventArgs

		Private MyName As String
		Private MyResult As Object

		Friend Sub New()

		End Sub

		Friend Sub SetData(ByVal name As String, ByVal result As Object)
			MyName = name
			MyResult = result
		End Sub

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="NodeEventArgs.Name"]/*' />	
		Public ReadOnly Property Name() As String
			Get
				Return MyName
			End Get
		End Property

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="NodeEventArgs.Result"]/*' />	
		Public ReadOnly Property Result() As Object
			Get
				Return MyResult
			End Get
		End Property
	End Class

End Namespace