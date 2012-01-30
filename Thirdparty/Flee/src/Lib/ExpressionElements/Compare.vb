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

' Element that compares values and generates a boolean result
Friend Class CompareElement
	Inherits BinaryExpressionElement

	Private MyOperation As LogicalCompareOperation

	Public Sub New()

	End Sub

	Public Sub Initialize(ByVal leftChild As ExpressionElement, ByVal rightChild As ExpressionElement, ByVal op As LogicalCompareOperation)
		MyLeftChild = leftChild
		MyRightChild = rightChild
		MyOperation = op
	End Sub

	Public Sub Validate()
		Me.ValidateInternal(MyOperation)
	End Sub

	Protected Overrides Sub GetOperation(ByVal operation As Object)
		MyOperation = DirectCast(operation, LogicalCompareOperation)
	End Sub

	Protected Overrides Function GetResultType(ByVal leftType As System.Type, ByVal rightType As System.Type) As System.Type
		Dim binaryResultType As Type = ImplicitConverter.GetBinaryResultType(leftType, rightType)
		Dim overloadedOperator As MethodInfo = Me.GetOverloadedCompareOperator()
		Dim isEqualityOp As Boolean = IsOpTypeEqualOrNotEqual(MyOperation)

		' Use our string equality instead of overloaded operator
		If leftType Is GetType(String) And rightType Is GetType(String) And isEqualityOp = True Then
			' String equality
			Return GetType(Boolean)
		ElseIf Not overloadedOperator Is Nothing Then
			Return overloadedOperator.ReturnType
		ElseIf Not binaryResultType Is Nothing Then
			' Comparison of numeric operands
			Return GetType(Boolean)
		ElseIf leftType Is GetType(Boolean) And rightType Is GetType(Boolean) And isEqualityOp = True Then
			' Boolean equality
			Return GetType(Boolean)
		ElseIf Me.AreBothChildrenReferenceTypes() = True And isEqualityOp = True Then
			' Comparison of reference types
			Return GetType(Boolean)
		ElseIf Me.AreBothChildrenSameEnum() = True Then
			Return GetType(Boolean)
		Else
			' Invalid operands
			Return Nothing
		End If
	End Function

	Private Function GetOverloadedCompareOperator() As MethodInfo
		Dim name As String = GetCompareOperatorName(MyOperation)
		Return MyBase.GetOverloadedBinaryOperator(name, MyOperation)
	End Function

	Private Shared Function GetCompareOperatorName(ByVal op As LogicalCompareOperation) As String
		Select Case op
			Case LogicalCompareOperation.Equal
				Return "Equality"
			Case LogicalCompareOperation.NotEqual
				Return "Inequality"
			Case LogicalCompareOperation.GreaterThan
				Return "GreaterThan"
			Case LogicalCompareOperation.LessThan
				Return "LessThan"
			Case LogicalCompareOperation.GreaterThanOrEqual
				Return "GreaterThanOrEqual"
			Case LogicalCompareOperation.LessThanOrEqual
				Return "LessThanOrEqual"
			Case Else
				Debug.Assert(False, "unknown compare type")
				Return Nothing
		End Select
	End Function

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim binaryResultType As Type = ImplicitConverter.GetBinaryResultType(MyLeftChild.ResultType, MyRightChild.ResultType)
		Dim overloadedOperator As MethodInfo = Me.GetOverloadedCompareOperator()

		If Me.AreBothChildrenOfType(GetType(String)) Then
			' String equality
			MyLeftChild.Emit(ilg, services)
			MyRightChild.Emit(ilg, services)
			EmitStringEquality(ilg, MyOperation, services)
		ElseIf Not overloadedOperator Is Nothing Then
			MyBase.EmitOverloadedOperatorCall(overloadedOperator, ilg, services)
		ElseIf Not binaryResultType Is Nothing Then
			' Emit a compare of numeric operands
			EmitChildWithConvert(MyLeftChild, binaryResultType, ilg, services)
			EmitChildWithConvert(MyRightChild, binaryResultType, ilg, services)
			EmitCompareOperation(ilg, MyOperation)
		ElseIf Me.AreBothChildrenOfType(GetType(Boolean)) Then
			' Boolean equality
			Me.EmitRegular(ilg, services)
		ElseIf Me.AreBothChildrenReferenceTypes() = True Then
			' Reference equality
			Me.EmitRegular(ilg, services)
		ElseIf MyLeftChild.ResultType.IsEnum = True And MyRightChild.ResultType.IsEnum = True Then
			Me.EmitRegular(ilg, services)
		Else
			Debug.Fail("unknown operand types")
		End If
	End Sub

	Private Sub EmitRegular(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		MyLeftChild.Emit(ilg, services)
		MyRightChild.Emit(ilg, services)
		Me.EmitCompareOperation(ilg, MyOperation)
	End Sub

	Private Shared Sub EmitStringEquality(ByVal ilg As FleeILGenerator, ByVal op As LogicalCompareOperation, ByVal services As IServiceProvider)
		' Get the StringComparison from the options
		Dim options As ExpressionOptions = services.GetService(GetType(ExpressionOptions))
		Dim ic As New Int32LiteralElement(options.StringComparison)

		ic.Emit(ilg, services)

		' and emit the method call
		Dim mi As System.Reflection.MethodInfo = GetType(String).GetMethod("Equals", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static, Nothing, New Type() {GetType(String), GetType(String), GetType(StringComparison)}, Nothing)
		ilg.Emit(OpCodes.Call, mi)

		If op = LogicalCompareOperation.NotEqual Then
			ilg.Emit(OpCodes.Ldc_I4_0)
			ilg.Emit(OpCodes.Ceq)
		End If
	End Sub

	Private Shared Function IsOpTypeEqualOrNotEqual(ByVal op As LogicalCompareOperation) As Boolean
		Return op = LogicalCompareOperation.Equal Or op = LogicalCompareOperation.NotEqual
	End Function

	Private Function AreBothChildrenReferenceTypes() As Boolean
		Return MyLeftChild.ResultType.IsValueType = False And MyRightChild.ResultType.IsValueType = False
	End Function

	Private Function AreBothChildrenSameEnum() As Boolean
		Return MyLeftChild.ResultType.IsEnum = True AndAlso MyLeftChild.ResultType Is MyRightChild.ResultType
	End Function

	' Emit the actual compare
	Private Sub EmitCompareOperation(ByVal ilg As FleeILGenerator, ByVal op As LogicalCompareOperation)
		Dim ltOpcode As OpCode = Me.GetCompareGTLTOpcode(False)
		Dim gtOpcode As OpCode = Me.GetCompareGTLTOpcode(True)

		Select Case op
			Case LogicalCompareOperation.Equal
				ilg.Emit(OpCodes.Ceq)
			Case LogicalCompareOperation.LessThan
				ilg.Emit(ltOpcode)
			Case LogicalCompareOperation.GreaterThan
				ilg.Emit(gtOpcode)
			Case LogicalCompareOperation.NotEqual
				ilg.Emit(OpCodes.Ceq)
				ilg.Emit(OpCodes.Ldc_I4_0)
				ilg.Emit(OpCodes.Ceq)
			Case LogicalCompareOperation.LessThanOrEqual
				ilg.Emit(gtOpcode)
				ilg.Emit(OpCodes.Ldc_I4_0)
				ilg.Emit(OpCodes.Ceq)
			Case LogicalCompareOperation.GreaterThanOrEqual
				ilg.Emit(ltOpcode)
				ilg.Emit(OpCodes.Ldc_I4_0)
				ilg.Emit(OpCodes.Ceq)
			Case Else
				Debug.Fail("Unknown op type")
		End Select
	End Sub

	' Get the correct greater/less than opcode
	Private Function GetCompareGTLTOpcode(ByVal greaterThan As Boolean) As OpCode
		Dim leftType As Type = MyLeftChild.ResultType

		If leftType Is MyRightChild.ResultType Then
			If leftType Is GetType(UInt32) Or leftType Is GetType(UInt64) Then
				If greaterThan = True Then
					Return OpCodes.Cgt_Un
				Else
					Return OpCodes.Clt_Un
				End If
			Else
				Return GetCompareOpcode(greaterThan)
			End If
		Else
			Return GetCompareOpcode(greaterThan)
		End If
	End Function

	Private Shared Function GetCompareOpcode(ByVal greaterThan As Boolean) As OpCode
		If greaterThan = True Then
			Return OpCodes.Cgt
		Else
			Return OpCodes.Clt
		End If
	End Function
End Class