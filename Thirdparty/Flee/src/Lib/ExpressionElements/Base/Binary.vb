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

' Base class for expression elements that operate on two child elements
Friend MustInherit Class BinaryExpressionElement
	Inherits ExpressionElement

	Protected MyLeftChild, MyRightChild As ExpressionElement
	Private MyResultType As Type

	Protected Sub New()

	End Sub

	' Converts a list of binary elements into a binary tree
	Public Shared Function CreateElement(ByVal childValues As IList, ByVal elementType As Type) As BinaryExpressionElement
		Dim firstElement As BinaryExpressionElement = Activator.CreateInstance(elementType)
		firstElement.Configure(childValues.Item(0), childValues.Item(2), childValues.Item(1))

		Dim lastElement As BinaryExpressionElement = firstElement

		For i As Integer = 3 To childValues.Count - 1 Step 2
			Dim element As BinaryExpressionElement = Activator.CreateInstance(elementType)
			element.Configure(lastElement, childValues.Item(i + 1), childValues.Item(i))
			lastElement = element
		Next

		Return lastElement
	End Function

	Protected MustOverride Sub GetOperation(ByVal operation As Object)

	Protected Sub ValidateInternal(ByVal op As Object)
		MyResultType = Me.GetResultType(MyLeftChild.ResultType, MyRightChild.ResultType)

		If MyResultType Is Nothing Then
			Me.ThrowOperandTypeMismatch(op, MyLeftChild.ResultType, MyRightChild.ResultType)
		End If
	End Sub

	Protected Function GetOverloadedBinaryOperator(ByVal name As String, ByVal operation As Object) As MethodInfo
		Dim leftType As Type = MyLeftChild.ResultType
		Dim rightType As Type = MyRightChild.ResultType
		Dim binder As New BinaryOperatorBinder(leftType, rightType)

		' If both arguments are of the same type, pick either as the owner type
		If leftType Is rightType Then
			Return Utility.GetOverloadedOperator(name, leftType, binder, leftType, rightType)
		End If

		' Get the operator for both types
		Dim leftMethod, rightMethod As MethodInfo
		leftMethod = Utility.GetOverloadedOperator(name, leftType, binder, leftType, rightType)
		rightMethod = Utility.GetOverloadedOperator(name, rightType, binder, leftType, rightType)

		' Pick the right one
		If leftMethod Is Nothing And rightMethod Is Nothing Then
			' No operator defined for either
			Return Nothing
		ElseIf leftMethod Is Nothing Then
			Return rightMethod
		ElseIf rightMethod Is Nothing Then
			Return leftMethod
		Else
			' Ambiguous call
			MyBase.ThrowAmbiguousCallException(leftType, rightType, operation)
			Return Nothing
		End If
	End Function

	Protected Sub EmitOverloadedOperatorCall(ByVal method As MethodInfo, ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim params As ParameterInfo() = method.GetParameters()
		Dim pLeft As ParameterInfo = params(0)
		Dim pRight As ParameterInfo = params(1)

		EmitChildWithConvert(MyLeftChild, pLeft.ParameterType, ilg, services)
		EmitChildWithConvert(MyRightChild, pRight.ParameterType, ilg, services)
		ilg.Emit(OpCodes.Call, method)
	End Sub

	Protected Sub ThrowOperandTypeMismatch(ByVal operation As Object, ByVal leftType As Type, ByVal rightType As Type)
		MyBase.ThrowCompileException(CompileErrorResourceKeys.OperationNotDefinedForTypes, CompileExceptionReason.TypeMismatch, operation, leftType.Name, rightType.Name)
	End Sub

	Protected MustOverride Function GetResultType(ByVal leftType As Type, ByVal rightType As Type) As Type

	Protected Shared Sub EmitChildWithConvert(ByVal child As ExpressionElement, ByVal resultType As Type, ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		child.Emit(ilg, services)
		Dim converted As Boolean = ImplicitConverter.EmitImplicitConvert(child.ResultType, resultType, ilg)
		Debug.Assert(converted, "convert failed")
	End Sub

	Protected Function AreBothChildrenOfType(ByVal target As Type) As Boolean
		Return IsChildOfType(MyLeftChild, target) And IsChildOfType(MyRightChild, target)
	End Function

	Protected Function IsEitherChildOfType(ByVal target As Type) As Boolean
		Return IsChildOfType(MyLeftChild, target) OrElse IsChildOfType(MyRightChild, target)
	End Function

	Protected Shared Function IsChildOfType(ByVal child As ExpressionElement, ByVal t As Type) As Boolean
		Return child.ResultType Is t
	End Function

	' Set the left and right operands, get the operation, and get the result type
	Private Sub Configure(ByVal leftChild As ExpressionElement, ByVal rightChild As ExpressionElement, ByVal op As Object)
		MyLeftChild = leftChild
		MyRightChild = rightChild
		Me.GetOperation(op)

		Me.ValidateInternal(op)
	End Sub

	Public NotOverridable Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return MyResultType
		End Get
	End Property
End Class