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

' Unary negate
Friend Class NegateElement
	Inherits UnaryElement

	Public Sub New()

	End Sub

	Protected Overrides Function GetResultType(ByVal childType As System.Type) As System.Type
		Dim tc As TypeCode = Type.GetTypeCode(childType)

		Dim mi As MethodInfo = Utility.GetSimpleOverloadedOperator("UnaryNegation", childType, childType)
		If Not mi Is Nothing Then
			Return mi.ReturnType
		End If

		Select Case tc
			Case TypeCode.Single, TypeCode.Double, TypeCode.Int32, TypeCode.Int64
				Return childType
			Case TypeCode.UInt32
				Return GetType(Int64)
			Case Else
				Return Nothing
		End Select
	End Function

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim resultType As Type = Me.ResultType
		MyChild.Emit(ilg, services)
		ImplicitConverter.EmitImplicitConvert(MyChild.ResultType, resultType, ilg)

		Dim mi As MethodInfo = Utility.GetSimpleOverloadedOperator("UnaryNegation", resultType, resultType)

		If mi Is Nothing Then
			ilg.Emit(OpCodes.Neg)
		Else
			ilg.Emit(OpCodes.Call, mi)
		End If
	End Sub
End Class