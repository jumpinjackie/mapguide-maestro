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

' The base class for all elements of an expression.
Friend MustInherit Class ExpressionElement

	Friend Sub New()

	End Sub

	' All expression elements must be able to emit their IL
	Public MustOverride Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
	' All expression elements must expose the Type they evaluate to
	Public MustOverride ReadOnly Property ResultType() As Type

	Public Overrides Function ToString() As String
		Return Me.Name
	End Function

	Protected Sub ThrowCompileException(ByVal messageKey As String, ByVal reason As CompileExceptionReason, ByVal ParamArray arguments As Object())
		Dim messageTemplate As String = FleeResourceManager.Instance.GetCompileErrorString(messageKey)
		Dim message As String = String.Format(messageTemplate, arguments)
		message = String.Concat(Me.Name, ": ", message)
		Throw New ExpressionCompileException(message, reason)
	End Sub

	Protected Sub ThrowAmbiguousCallException(ByVal leftType As Type, ByVal rightType As Type, ByVal operation As Object)
		Me.ThrowCompileException(CompileErrorResourceKeys.AmbiguousOverloadedOperator, CompileExceptionReason.AmbiguousMatch, leftType.Name, rightType.Name, operation)
	End Sub

	Protected Function CreateTempFleeILGenerator(ByVal ilgCurrent As FleeILGenerator) As FleeILGenerator
		Dim dm As New DynamicMethod("temp", GetType(Int32), Nothing, Me.GetType())
		Return New FleeILGenerator(dm.GetILGenerator(), ilgCurrent.Length, True)
	End Function

	Protected ReadOnly Property Name() As String
		Get
			Dim key As String = Me.GetType().Name
			Dim value As String = FleeResourceManager.Instance.GetElementNameString(key)
			Debug.Assert(value IsNot Nothing, String.Format("Element name for '{0}' not in resource file", key))
			Return value
		End Get
	End Property
End Class
