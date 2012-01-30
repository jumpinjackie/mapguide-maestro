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

' Base class for an expression element that operates on one value
Friend MustInherit Class UnaryElement
	Inherits ExpressionElement

	Protected MyChild As ExpressionElement
	Private MyResultType As Type

	Public Sub SetChild(ByVal child As ExpressionElement)
		MyChild = child
		MyResultType = Me.GetResultType(child.ResultType)

		If MyResultType Is Nothing Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.OperationNotDefinedForType, CompileExceptionReason.TypeMismatch, MyChild.ResultType.Name)
		End If
	End Sub

	Protected MustOverride Function GetResultType(ByVal childType As Type) As Type

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return MyResultType
		End Get
	End Property
End Class