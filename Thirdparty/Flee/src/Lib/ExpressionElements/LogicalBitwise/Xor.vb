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

Friend Class XorElement
	Inherits BinaryExpressionElement

	Protected Overrides Function GetResultType(ByVal leftType As System.Type, ByVal rightType As System.Type) As System.Type
		Dim bitwiseType As Type = Utility.GetBitwiseOpType(leftType, rightType)

		If Not bitwiseType Is Nothing Then
			Return bitwiseType
		ElseIf Me.AreBothChildrenOfType(GetType(Boolean)) = True Then
			Return GetType(Boolean)
		Else
			Return Nothing
		End If
	End Function

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim resultType As Type = Me.ResultType

		MyLeftChild.Emit(ilg, services)
		ImplicitConverter.EmitImplicitConvert(MyLeftChild.ResultType, resultType, ilg)
		MyRightChild.Emit(ilg, services)
		ImplicitConverter.EmitImplicitConvert(MyRightChild.ResultType, resultType, ilg)
		ilg.Emit(OpCodes.Xor)
	End Sub

	Protected Overrides Sub GetOperation(ByVal operation As Object)

	End Sub
End Class