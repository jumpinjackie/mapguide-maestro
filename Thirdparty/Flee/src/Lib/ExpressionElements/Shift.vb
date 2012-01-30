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

' Left/right shift
Friend Class ShiftElement
	Inherits BinaryExpressionElement

	Private MyOperation As ShiftOperation

	Public Sub New()

	End Sub

	Protected Overrides Function GetResultType(ByVal leftType As System.Type, ByVal rightType As System.Type) As System.Type
		' Right argument (shift count) must be convertible to int32
		If ImplicitConverter.EmitImplicitNumericConvert(rightType, GetType(Int32), Nothing) = False Then
			Return Nothing
		End If

		' Left argument must be an integer type
		If Utility.IsIntegralType(leftType) = False Then
			Return Nothing
		End If

		Dim tc As TypeCode = Type.GetTypeCode(leftType)

		Select Case tc
			Case TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32
				Return GetType(Int32)
			Case TypeCode.UInt32
				Return GetType(UInt32)
			Case TypeCode.Int64
				Return GetType(Int64)
			Case TypeCode.UInt64
				Return GetType(UInt64)
			Case Else
				Debug.Assert(False, "unknown left shift operand")
				Return Nothing
		End Select
	End Function

	Protected Overrides Sub GetOperation(ByVal operation As Object)
		MyOperation = DirectCast(operation, ShiftOperation)
	End Sub

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		MyLeftChild.Emit(ilg, services)
		Me.EmitShiftCount(ilg, services)
		Me.EmitShift(ilg)
	End Sub

	' If the shift count is greater than the number of bits in the number, the result is undefined.
	' So we play it safe and force the shift count to 32/64 bits by ANDing it with the appropriate mask.
	Private Sub EmitShiftCount(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		MyRightChild.Emit(ilg, services)
		Dim tc As TypeCode = Type.GetTypeCode(MyLeftChild.ResultType)
		Select Case tc
			Case TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32
				ilg.Emit(OpCodes.Ldc_I4_S, CSByte(&H1F))
			Case TypeCode.Int64, TypeCode.UInt64
				ilg.Emit(OpCodes.Ldc_I4_S, CSByte(&H3F))
			Case Else
				Debug.Assert(False, "unknown left shift operand")
		End Select

		ilg.Emit(OpCodes.And)
	End Sub

	Private Sub EmitShift(ByVal ilg As FleeILGenerator)
		Dim tc As TypeCode = Type.GetTypeCode(MyLeftChild.ResultType)
		Dim op As OpCode

		Select Case tc
			Case TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.Int64
				' Signed operand, emit a left shift or arithmetic right shift
				If MyOperation = ShiftOperation.LeftShift Then
					op = OpCodes.Shl
				Else
					op = OpCodes.Shr
				End If
			Case TypeCode.UInt32, TypeCode.UInt64
				' Unsigned operand, emit left shift or logical right shift
				If MyOperation = ShiftOperation.LeftShift Then
					op = OpCodes.Shl
				Else
					op = OpCodes.Shr_Un
				End If
			Case Else
				Debug.Assert(False, "unknown left shift operand")
		End Select

		ilg.Emit(op)
	End Sub
End Class