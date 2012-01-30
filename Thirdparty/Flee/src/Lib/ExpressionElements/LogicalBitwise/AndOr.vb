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

' Bitwise and logical And/Or
Friend Class AndOrElement
	Inherits BinaryExpressionElement

	Private MyOperation As AndOrOperation
	Private Shared OurTrueTerminalKey As New Object
	Private Shared OurFalseTerminalKey As New Object
	Private Shared OurEndLabelKey As New Object

	Public Sub New()

	End Sub

	Protected Overrides Sub GetOperation(ByVal operation As Object)
		MyOperation = DirectCast(operation, AndOrOperation)
	End Sub

	Protected Overrides Function GetResultType(ByVal leftType As System.Type, ByVal rightType As System.Type) As System.Type
		Dim bitwiseOpType As Type = Utility.GetBitwiseOpType(leftType, rightType)
		If Not bitwiseOpType Is Nothing Then
			Return bitwiseOpType
		ElseIf Me.AreBothChildrenOfType(GetType(Boolean)) Then
			Return GetType(Boolean)
		Else
			Return Nothing
		End If
	End Function

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim resultType As Type = Me.ResultType

		If resultType Is GetType(Boolean) Then
			Me.DoEmitLogical(ilg, services)
		Else
			MyLeftChild.Emit(ilg, services)
			ImplicitConverter.EmitImplicitConvert(MyLeftChild.ResultType, resultType, ilg)
			MyRightChild.Emit(ilg, services)
			ImplicitConverter.EmitImplicitConvert(MyRightChild.ResultType, resultType, ilg)
			EmitBitwiseOperation(ilg, MyOperation)
		End If
	End Sub

	Private Shared Sub EmitBitwiseOperation(ByVal ilg As FleeILGenerator, ByVal op As AndOrOperation)
		Select Case op
			Case AndOrOperation.And
				ilg.Emit(OpCodes.And)
			Case AndOrOperation.Or
				ilg.Emit(OpCodes.Or)
			Case Else
				Debug.Fail("Unknown op type")
		End Select
	End Sub

	Private Sub DoEmitLogical(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		' We have to do a 'fake' emit so we can get the positions of the labels
		Dim info As New ShortCircuitInfo()
		' Create a temporary IL generator
		Dim ilgTemp As FleeILGenerator = Me.CreateTempFleeILGenerator(ilg)

		' We have to make sure that the label count for the temp FleeILGenerator matches our real FleeILGenerator
		Utility.SyncFleeILGeneratorLabels(ilg, ilgTemp)
		' Do the fake emit
		Me.EmitLogical(ilgTemp, info, services)

		' Clear everything except the label positions
		info.ClearTempState()

		info.Branches.ComputeBranches()

		Utility.SyncFleeILGeneratorLabels(ilgTemp, ilg)

		' Do the real emit
		Me.EmitLogical(ilg, info, services)
	End Sub

	' Emit a short-circuited logical operation sequence
	' The idea: Store all the leaf operands in a stack with the leftmost at the top and rightmost at the bottom.
	' For each operand, emit it and try to find an end point for when it short-circuits.  This means we go up through
	' the stack of operators (ignoring siblings) until we find a different operation (then emit a branch to its right operand)
	' or we reach the root (emit a branch to a true/false).
	' Repeat the process for all operands and then emit the true/false/last operand end cases.
	Private Sub EmitLogical(ByVal ilg As FleeILGenerator, ByVal info As ShortCircuitInfo, ByVal services As IServiceProvider)
		' We always have an end label
		info.Branches.GetLabel(OurEndLabelKey, ilg)

		' Populate our data structures
		Me.PopulateData(info)

		' Emit the sequence
		EmitLogicalShortCircuit(ilg, info, services)

		' Get the last operand
		Dim terminalOperand As ExpressionElement = info.Operands.Pop()
		' Emit it
		EmitOperand(terminalOperand, info, ilg, services)
		' And jump to the end
		Dim endLabel As Label = info.Branches.FindLabel(OurEndLabelKey)
		ilg.Emit(OpCodes.Br_S, endLabel)

		' Emit our true/false terminals
		EmitTerminals(info, ilg, endLabel)

		' Mark the end
		ilg.MarkLabel(endLabel)
	End Sub

	' Emit a sequence of and/or expressions with short-circuiting
	Private Shared Sub EmitLogicalShortCircuit(ByVal ilg As FleeILGenerator, ByVal info As ShortCircuitInfo, ByVal services As IServiceProvider)
		While info.Operators.Count <> 0
			' Get the operator
			Dim op As AndOrElement = info.Operators.Pop()
			' Get the left operand
			Dim leftOperand As ExpressionElement = info.Operands.Pop()

			' Emit the left
			EmitOperand(leftOperand, info, ilg, services)

			' Get the label for the short-circuit case
			Dim l As Label = GetShortCircuitLabel(op, info, ilg)
			' Emit the branch
			EmitBranch(op, ilg, l, info)
		End While
	End Sub

	Private Shared Sub EmitBranch(ByVal op As AndOrElement, ByVal ilg As FleeILGenerator, ByVal target As Label, ByVal info As ShortCircuitInfo)

		If ilg.IsTemp = True Then
			info.Branches.AddBranch(ilg, target)

			' Temp mode; just emit a short branch and return
			Dim shortBranch As OpCode = GetBranchOpcode(op, False)
			ilg.Emit(shortBranch, target)

			Return
		End If

		' Emit the proper branch opcode

		' Determine if it is a long branch
		Dim longBranch As Boolean = info.Branches.IsLongBranch(ilg, target)

		' Get the branch opcode
		Dim brOpcode As OpCode = GetBranchOpcode(op, longBranch)

		' Emit the branch
		ilg.Emit(brOpcode, target)
	End Sub

	' Emit a short/long branch for an And/Or element
	Private Shared Function GetBranchOpcode(ByVal op As AndOrElement, ByVal longBranch As Boolean) As OpCode
		If op.MyOperation = AndOrOperation.And Then
			If longBranch = True Then
				Return OpCodes.Brfalse
			Else
				Return OpCodes.Brfalse_S
			End If
		Else
			If longBranch = True Then
				Return OpCodes.Brtrue
			Else
				Return OpCodes.Brtrue_S
			End If
		End If
	End Function

	' Get the label for a short-circuit
	Private Shared Function GetShortCircuitLabel(ByVal current As AndOrElement, ByVal info As ShortCircuitInfo, ByVal ilg As FleeILGenerator) As Label
		' We modify the given stacks so we need to clone them
		Dim cloneOperands As Stack = info.Operands.Clone()
		Dim cloneOperators As Stack = info.Operators.Clone()

		' Pop all siblings
		current.PopRightChild(cloneOperands, cloneOperators)

		' Go until we run out of operators
		While cloneOperators.Count > 0
			' Get the top operator
			Dim top As AndOrElement = cloneOperators.Pop()

			' Is is a different operation?
			If top.MyOperation <> current.MyOperation Then
				' Yes, so return a label to its right operand
				Dim nextOperand As Object = cloneOperands.Pop()
				Return GetLabel(nextOperand, ilg, info)
			Else
				' No, so keep going up the stack
				top.PopRightChild(cloneOperands, cloneOperators)
			End If
		End While

		' We've reached the end of the stack so return the label for the appropriate true/false terminal
		If current.MyOperation = AndOrOperation.And Then
			Return GetLabel(OurFalseTerminalKey, ilg, info)
		Else
			Return GetLabel(OurTrueTerminalKey, ilg, info)
		End If
	End Function

	Private Sub PopRightChild(ByVal operands As Stack, ByVal operators As Stack)
		Dim andOrChild As AndOrElement = TryCast(MyRightChild, AndOrElement)

		' What kind of child do we have?
		If Not andOrChild Is Nothing Then
			' Another and/or expression so recurse
			andOrChild.Pop(operands, operators)
		Else
			' A terminal so pop it off the operands stack
			operands.Pop()
		End If
	End Sub

	' Recursively pop operators and operands
	Private Sub Pop(ByVal operands As Stack, ByVal operators As Stack)
		operators.Pop()

		Dim andOrChild As AndOrElement = TryCast(MyLeftChild, AndOrElement)
		If andOrChild Is Nothing Then
			operands.Pop()
		Else
			andOrChild.Pop(operands, operators)
		End If

		andOrChild = TryCast(MyRightChild, AndOrElement)

		If andOrChild Is Nothing Then
			operands.Pop()
		Else
			andOrChild.Pop(operands, operators)
		End If
	End Sub

	Private Shared Sub EmitOperand(ByVal operand As ExpressionElement, ByVal info As ShortCircuitInfo, ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		' Is this operand the target of a label?
		If info.Branches.HasLabel(operand) = True Then
			' Yes, so mark it
			Dim leftLabel As Label = info.Branches.FindLabel(operand)
			ilg.MarkLabel(leftLabel)

			' Note the label's position
			MarkBranchTarget(info, leftLabel, ilg)
		End If

		' Emit the operand
		operand.Emit(ilg, services)
	End Sub

	' Emit the end cases for a short-circuit
	Private Shared Sub EmitTerminals(ByVal info As ShortCircuitInfo, ByVal ilg As FleeILGenerator, ByVal endLabel As Label)
		' Emit the false case if it was used
		If info.Branches.HasLabel(OurFalseTerminalKey) = True Then
			Dim falseLabel As Label = info.Branches.FindLabel(OurFalseTerminalKey)

			' Mark the label and note its position
			ilg.MarkLabel(falseLabel)
			MarkBranchTarget(info, falseLabel, ilg)

			ilg.Emit(OpCodes.Ldc_I4_0)

			' If we also have a true terminal, then skip over it
			If info.Branches.HasLabel(OurTrueTerminalKey) = True Then
				ilg.Emit(OpCodes.Br_S, endLabel)
			End If
		End If

		' Emit the true case if it was used
		If info.Branches.HasLabel(OurTrueTerminalKey) = True Then
			Dim trueLabel As Label = info.Branches.FindLabel(OurTrueTerminalKey)

			' Mark the label and note its position
			ilg.MarkLabel(trueLabel)
			MarkBranchTarget(info, trueLabel, ilg)

			ilg.Emit(OpCodes.Ldc_I4_1)
		End If
	End Sub

	' Note a label's position if we are in mark mode
	Private Shared Sub MarkBranchTarget(ByVal info As ShortCircuitInfo, ByVal target As Label, ByVal ilg As FleeILGenerator)
		If ilg.IsTemp = True Then
			info.Branches.MarkLabel(ilg, target)
		End If
	End Sub

	Private Shared Function GetLabel(ByVal key As Object, ByVal ilg As FleeILGenerator, ByVal info As ShortCircuitInfo) As Label
		Return info.Branches.GetLabel(key, ilg)
	End Function

	' Visit the nodes of the tree (right then left) and populate some data structures
	Private Sub PopulateData(ByVal info As ShortCircuitInfo)
		' Is our right child a leaf or another And/Or expression?
		Dim andOrChild As AndOrElement = TryCast(MyRightChild, AndOrElement)
		If andOrChild Is Nothing Then
			' Leaf so push it on the stack
			info.Operands.Push(MyRightChild)
		Else
			' Another And/Or expression so recurse
			andOrChild.PopulateData(info)
		End If

		' Add ourselves as an operator
		info.Operators.Push(Me)

		' Do the same thing for the left child
		andOrChild = TryCast(MyLeftChild, AndOrElement)

		If andOrChild Is Nothing Then
			info.Operands.Push(MyLeftChild)
		Else
			andOrChild.PopulateData(info)
		End If
	End Sub
End Class