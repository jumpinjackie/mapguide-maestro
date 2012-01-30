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

' Conditional operator
Friend Class ConditionalElement
	Inherits ExpressionElement

	Private MyCondition As ExpressionElement
	Private MyWhenTrue, MyWhenFalse As ExpressionElement
	Private MyResultType As Type

	Public Sub New(ByVal condition As ExpressionElement, ByVal whenTrue As ExpressionElement, ByVal whenFalse As ExpressionElement)
		MyCondition = condition
		MyWhenTrue = whenTrue
		MyWhenFalse = whenFalse

		If Not MyCondition.ResultType Is GetType(Boolean) Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.FirstArgNotBoolean, CompileExceptionReason.TypeMismatch)
		End If

		' The result type is the type that is common to the true/false operands
		If ImplicitConverter.EmitImplicitConvert(MyWhenFalse.ResultType, MyWhenTrue.ResultType, Nothing) = True Then
			MyResultType = MyWhenTrue.ResultType
		ElseIf ImplicitConverter.EmitImplicitConvert(MyWhenTrue.ResultType, MyWhenFalse.ResultType, Nothing) = True Then
			MyResultType = MyWhenFalse.ResultType
		Else
			MyBase.ThrowCompileException(CompileErrorResourceKeys.NeitherArgIsConvertibleToTheOther, CompileExceptionReason.TypeMismatch, MyWhenTrue.ResultType.Name, MyWhenFalse.ResultType.Name)
		End If
	End Sub

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim bm As New BranchManager()
		bm.GetLabel("falseLabel", ilg)
		bm.GetLabel("endLabel", ilg)

		If ilg.IsTemp = True Then
			' If this is a fake emit, then do a fake emit and return
			Me.EmitConditional(ilg, services, bm)
			Return
		End If

		Dim ilgTemp As FleeILGenerator = Me.CreateTempFleeILGenerator(ilg)
		Utility.SyncFleeILGeneratorLabels(ilg, ilgTemp)

		' Emit fake conditional to get branch target positions
		Me.EmitConditional(ilgTemp, services, bm)

		bm.ComputeBranches()

		' Emit real conditional now that we have the branch target locations
		Me.EmitConditional(ilg, services, bm)
	End Sub

	Private Sub EmitConditional(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider, ByVal bm As BranchManager)
		Dim falseLabel As Label = bm.FindLabel("falseLabel")
		Dim endLabel As Label = bm.FindLabel("endLabel")

		' Emit the condition
		MyCondition.Emit(ilg, services)

		' On false go to the false operand
		If ilg.IsTemp = True Then
			bm.AddBranch(ilg, falseLabel)
			ilg.Emit(OpCodes.Brfalse_S, falseLabel)
		ElseIf bm.IsLongBranch(ilg, falseLabel) = False Then
			ilg.Emit(OpCodes.Brfalse_S, falseLabel)
		Else
			ilg.Emit(OpCodes.Brfalse, falseLabel)
		End If

		' Emit the true operand
		MyWhenTrue.Emit(ilg, services)
		ImplicitConverter.EmitImplicitConvert(MyWhenTrue.ResultType, MyResultType, ilg)

		' Jump to end
		If ilg.IsTemp = True Then
			bm.AddBranch(ilg, endLabel)
			ilg.Emit(OpCodes.Br_S, endLabel)
		ElseIf bm.IsLongBranch(ilg, endLabel) = False Then
			ilg.Emit(OpCodes.Br_S, endLabel)
		Else
			ilg.Emit(OpCodes.Br, endLabel)
		End If

		bm.MarkLabel(ilg, falseLabel)
		ilg.MarkLabel(falseLabel)
		
		' Emit the false operand
		MyWhenFalse.Emit(ilg, services)
		ImplicitConverter.EmitImplicitConvert(MyWhenFalse.ResultType, MyResultType, ilg)
		' Fall through to end
		bm.MarkLabel(ilg, endLabel)
		ilg.MarkLabel(endLabel)
	End Sub

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return MyResultType
		End Get
	End Property
End Class