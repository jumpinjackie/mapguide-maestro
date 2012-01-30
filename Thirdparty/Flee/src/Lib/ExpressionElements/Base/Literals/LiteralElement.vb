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

' Elements that represent constants in an expression

Imports System.Reflection.Emit

Friend MustInherit Class LiteralElement
	Inherits ExpressionElement

	Protected Sub OnParseOverflow(ByVal image As String)
		MyBase.ThrowCompileException(CompileErrorResourceKeys.ValueNotRepresentableInType, CompileExceptionReason.ConstantOverflow, image, Me.ResultType.Name)
	End Sub

	Public Shared Sub EmitLoad(ByVal value As Int32, ByVal ilg As FleeILGenerator)
		If value >= -1 And value <= 8 Then
			EmitSuperShort(value, ilg)
		ElseIf value >= SByte.MinValue And value <= SByte.MaxValue Then
			ilg.Emit(OpCodes.Ldc_I4_S, CSByte(value))
		Else
			ilg.Emit(OpCodes.Ldc_I4, value)
		End If
	End Sub

	Protected Shared Sub EmitLoad(ByVal value As Int64, ByVal ilg As FleeILGenerator)
		If value >= Int32.MinValue And value <= Int32.MaxValue Then
			EmitLoad(CInt(value), ilg)
			ilg.Emit(OpCodes.Conv_I8)
		ElseIf value >= 0 And value <= UInt32.MaxValue Then
			EmitLoad(CInt(value), ilg)
			ilg.Emit(OpCodes.Conv_U8)
		Else
			ilg.Emit(OpCodes.Ldc_I8, value)
		End If
	End Sub

	Protected Shared Sub EmitLoad(ByVal value As Boolean, ByVal ilg As FleeILGenerator)
		If value = True Then
			ilg.Emit(OpCodes.Ldc_I4_1)
		Else
			ilg.Emit(OpCodes.Ldc_I4_0)
		End If
	End Sub

	Private Shared Sub EmitSuperShort(ByVal value As Int32, ByVal ilg As FleeILGenerator)
		Dim ldcOpcode As OpCode

		Select Case value
			Case 0
				ldcOpcode = OpCodes.Ldc_I4_0
			Case 1
				ldcOpcode = OpCodes.Ldc_I4_1
			Case 2
				ldcOpcode = OpCodes.Ldc_I4_2
			Case 3
				ldcOpcode = OpCodes.Ldc_I4_3
			Case 4
				ldcOpcode = OpCodes.Ldc_I4_4
			Case 5
				ldcOpcode = OpCodes.Ldc_I4_5
			Case 6
				ldcOpcode = OpCodes.Ldc_I4_6
			Case 7
				ldcOpcode = OpCodes.Ldc_I4_7
			Case 8
				ldcOpcode = OpCodes.Ldc_I4_8
			Case -1
				ldcOpcode = OpCodes.Ldc_I4_M1
			Case Else
				Debug.Assert(False, "value out of range")
		End Select

		ilg.Emit(ldcOpcode)
	End Sub
End Class