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

' Elements for arithmetic operations

Imports System.Reflection.Emit
Imports System.Reflection

' Element that represents all arithmetic operations
Friend Class ArithmeticElement
	Inherits BinaryExpressionElement

	Private Shared OurPowerMethodInfo As MethodInfo
	Private Shared OurStringConcatMethodInfo As MethodInfo
	Private Shared OurObjectConcatMethodInfo As MethodInfo
	Private MyOperation As BinaryArithmeticOperation

	Shared Sub New()
		OurPowerMethodInfo = GetType(Math).GetMethod("Pow", BindingFlags.Public Or BindingFlags.Static)
		OurStringConcatMethodInfo = GetType(String).GetMethod("Concat", BindingFlags.Public Or BindingFlags.Static, Nothing, New Type() {GetType(String), GetType(String)}, Nothing)
		OurObjectConcatMethodInfo = GetType(String).GetMethod("Concat", BindingFlags.Public Or BindingFlags.Static, Nothing, New Type() {GetType(Object), GetType(Object)}, Nothing)
	End Sub

	Public Sub New()

	End Sub

	Protected Overrides Sub GetOperation(ByVal operation As Object)
		MyOperation = DirectCast(operation, BinaryArithmeticOperation)
	End Sub

	Protected Overrides Function GetResultType(ByVal leftType As System.Type, ByVal rightType As System.Type) As System.Type
		Dim binaryResultType As Type = ImplicitConverter.GetBinaryResultType(leftType, rightType)
		Dim overloadedMethod As MethodInfo = Me.GetOverloadedArithmeticOperator()

		' Is an overloaded operator defined for our left and right children?
		If Not overloadedMethod Is Nothing Then
			' Yes, so use its return type
			Return overloadedMethod.ReturnType
		ElseIf Not binaryResultType Is Nothing Then
			' Operands are primitive types.  Return computed result type unless we are doing a power operation
			If MyOperation = BinaryArithmeticOperation.Power Then
				Return Me.GetPowerResultType(leftType, rightType, binaryResultType)
			Else
				Return binaryResultType
			End If
		ElseIf Me.IsEitherChildOfType(GetType(String)) = True And (MyOperation = BinaryArithmeticOperation.Add) Then
			' String concatenation
			Return GetType(String)
		Else
			' Invalid types
			Return Nothing
		End If
	End Function

	Private Function GetPowerResultType(ByVal leftType As Type, ByVal rightType As Type, ByVal binaryResultType As Type) As Type
		If Me.IsOptimizablePower = True Then
			Return leftType
		Else
			Return GetType(Double)
		End If
	End Function

	Private Function GetOverloadedArithmeticOperator() As MethodInfo
		' Get the name of the operator
		Dim name As String = GetOverloadedOperatorFunctionName(MyOperation)
		Return MyBase.GetOverloadedBinaryOperator(name, MyOperation)
	End Function

	Private Shared Function GetOverloadedOperatorFunctionName(ByVal op As BinaryArithmeticOperation) As String
		Select Case op
			Case BinaryArithmeticOperation.Add
				Return "Addition"
			Case BinaryArithmeticOperation.Subtract
				Return "Subtraction"
			Case BinaryArithmeticOperation.Multiply
				Return "Multiply"
			Case BinaryArithmeticOperation.Divide
				Return "Division"
			Case BinaryArithmeticOperation.Mod
				Return "Modulus"
			Case BinaryArithmeticOperation.Power
				Return "Exponent"
			Case Else
				Debug.Assert(False, "unknown operator type")
				Return Nothing
		End Select
	End Function

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim overloadedMethod As MethodInfo = Me.GetOverloadedArithmeticOperator()

		If Not overloadedMethod Is Nothing Then
			' Emit a call to an overloaded operator
			Me.EmitOverloadedOperatorCall(overloadedMethod, ilg, services)
		ElseIf Me.IsEitherChildOfType(GetType(String)) = True Then
			' One of our operands is a string so emit a concatenation
			Me.EmitStringConcat(ilg, services)
		Else
			' Emit a regular arithmetic operation			
			EmitArithmeticOperation(MyOperation, ilg, services)
		End If
	End Sub

	Private Shared Function IsUnsignedForArithmetic(ByVal t As Type) As Boolean
		Return t Is GetType(UInt32) Or t Is GetType(UInt64)
	End Function

	' Emit an arithmetic operation with handling for unsigned and checked contexts
	Private Sub EmitArithmeticOperation(ByVal op As BinaryArithmeticOperation, ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim options As ExpressionOptions = services.GetService(GetType(ExpressionOptions))
		Dim unsigned As Boolean = IsUnsignedForArithmetic(MyLeftChild.ResultType) And IsUnsignedForArithmetic(MyRightChild.ResultType)
		Dim integral As Boolean = Utility.IsIntegralType(MyLeftChild.ResultType) And Utility.IsIntegralType(MyRightChild.ResultType)
		Dim emitOverflow As Boolean = integral And options.Checked

		EmitChildWithConvert(MyLeftChild, Me.ResultType, ilg, services)

		If Me.IsOptimizablePower = False Then
			EmitChildWithConvert(MyRightChild, Me.ResultType, ilg, services)
		End If

		Select Case op
			Case BinaryArithmeticOperation.Add
				If emitOverflow = True Then
					If unsigned = True Then
						ilg.Emit(OpCodes.Add_Ovf_Un)
					Else
						ilg.Emit(OpCodes.Add_Ovf)
					End If
				Else
					ilg.Emit(OpCodes.Add)
				End If
			Case BinaryArithmeticOperation.Subtract
				If emitOverflow = True Then
					If unsigned = True Then
						ilg.Emit(OpCodes.Sub_Ovf_Un)
					Else
						ilg.Emit(OpCodes.Sub_Ovf)
					End If
				Else
					ilg.Emit(OpCodes.Sub)
				End If
			Case BinaryArithmeticOperation.Multiply
				Me.EmitMultiply(ilg, emitOverflow, unsigned)
			Case BinaryArithmeticOperation.Divide
				If unsigned = True Then
					ilg.Emit(OpCodes.Div_Un)
				Else
					ilg.Emit(OpCodes.Div)
				End If
			Case BinaryArithmeticOperation.Mod
				If unsigned = True Then
					ilg.Emit(OpCodes.Rem_Un)
				Else
					ilg.Emit(OpCodes.[Rem])
				End If
			Case BinaryArithmeticOperation.Power
				Me.EmitPower(ilg, emitOverflow, unsigned)
			Case Else
				Debug.Fail("Unknown op type")
		End Select
	End Sub

	Private Sub EmitPower(ByVal ilg As FleeILGenerator, ByVal emitOverflow As Boolean, ByVal unsigned As Boolean)
		If Me.IsOptimizablePower = True Then
			Me.EmitOptimizedPower(ilg, emitOverflow, unsigned)
		Else
			ilg.Emit(OpCodes.Call, OurPowerMethodInfo)
		End If
	End Sub

	Private Sub EmitOptimizedPower(ByVal ilg As FleeILGenerator, ByVal emitOverflow As Boolean, ByVal unsigned As Boolean)
		Dim right As Int32LiteralElement = MyRightChild

		If right.Value = 0 Then
			ilg.Emit(OpCodes.Pop)
			IntegralLiteralElement.EmitLoad(1, ilg)
			ImplicitConverter.EmitImplicitNumericConvert(GetType(Int32), MyLeftChild.ResultType, ilg)
			Return
		End If

		If right.Value = 1 Then
			Return
		End If

		' Start at 1 since left operand has already been emited once
		For i As Integer = 1 To right.Value - 1
			ilg.Emit(OpCodes.Dup)
		Next

		For i As Integer = 1 To right.Value - 1
			Me.EmitMultiply(ilg, emitOverflow, unsigned)
		Next
	End Sub

	Private Sub EmitMultiply(ByVal ilg As FleeILGenerator, ByVal emitOverflow As Boolean, ByVal unsigned As Boolean)
		If emitOverflow = True Then
			If unsigned = True Then
				ilg.Emit(OpCodes.Mul_Ovf_Un)
			Else
				ilg.Emit(OpCodes.Mul_Ovf)
			End If
		Else
			ilg.Emit(OpCodes.Mul)
		End If
	End Sub

	' Emit a string concatenation
	Private Sub EmitStringConcat(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim argType As Type
		Dim concatMethodInfo As System.Reflection.MethodInfo

		' Pick the most specific concat method
		If Me.AreBothChildrenOfType(GetType(String)) = True Then
			concatMethodInfo = OurStringConcatMethodInfo
			argType = GetType(String)
		Else
			Debug.Assert(Me.IsEitherChildOfType(GetType(String)), "one child must be a string")
			concatMethodInfo = OurObjectConcatMethodInfo
			argType = GetType(Object)
		End If

		' Emit the operands and call the function
		MyLeftChild.Emit(ilg, services)
		ImplicitConverter.EmitImplicitConvert(MyLeftChild.ResultType, argType, ilg)
		MyRightChild.Emit(ilg, services)
		ImplicitConverter.EmitImplicitConvert(MyRightChild.ResultType, argType, ilg)
		ilg.Emit(OpCodes.Call, concatMethodInfo)
	End Sub

	Private ReadOnly Property IsOptimizablePower() As Boolean
		Get
			If MyOperation <> BinaryArithmeticOperation.Power Then
				Return False
			End If

			Dim right As Int32LiteralElement = TryCast(MyRightChild, Int32LiteralElement)

			If right Is Nothing Then
				Return False
			End If

			Return right.Value >= 0
		End Get
	End Property
End Class