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
Imports System.Reflection

' The expression element at the top of the expression tree
Friend Class RootExpressionElement
	Inherits ExpressionElement

	Private MyChild As ExpressionElement
	Private MyResultType As Type

	Public Sub New(ByVal child As ExpressionElement, ByVal resultType As Type)
		MyChild = child
		MyResultType = resultType
		Me.Validate()
	End Sub

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		MyChild.Emit(ilg, services)
		ImplicitConverter.EmitImplicitConvert(MyChild.ResultType, MyResultType, ilg)

		Dim options As ExpressionOptions = services.GetService(GetType(ExpressionOptions))

		If options.IsGeneric = False Then
			ImplicitConverter.EmitImplicitConvert(MyResultType, GetType(Object), ilg)
		End If

		ilg.Emit(OpCodes.Ret)
	End Sub

	Private Sub Validate()
		If ImplicitConverter.EmitImplicitConvert(MyChild.ResultType, MyResultType, Nothing) = False Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.CannotConvertTypeToExpressionResult, CompileExceptionReason.TypeMismatch, MyChild.ResultType.Name, MyResultType.Name)
		End If
	End Sub

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return GetType(Object)
		End Get
	End Property
End Class