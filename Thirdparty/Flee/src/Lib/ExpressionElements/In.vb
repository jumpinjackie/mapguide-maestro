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

' Operand that searches for a value in a list of values or a collection
Friend Class InElement
	Inherits ExpressionElement

	' Element we will search for
	Private MyOperand As ExpressionElement
	' Elements we will compare against
	Private MyArguments As List(Of ExpressionElement)
	' Collection to look in
	Private MyTargetCollectionElement As ExpressionElement
	' Type of the collection
	Private MyTargetCollectionType As Type

	' Initialize for searching a list of values
	Public Sub New(ByVal operand As ExpressionElement, ByVal listElements As IList)
		MyOperand = operand

		Dim arr(listElements.Count - 1) As ExpressionElement
		listElements.CopyTo(arr, 0)

		MyArguments = New List(Of ExpressionElement)(arr)
		Me.ResolveForListSearch()
	End Sub

	' Initialize for searching a collection
	Public Sub New(ByVal operand As ExpressionElement, ByVal targetCollection As ExpressionElement)
		MyOperand = operand
		MyTargetCollectionElement = targetCollection
		Me.ResolveForCollectionSearch()
	End Sub

	Private Sub ResolveForListSearch()
		Dim ce As New CompareElement()

		' Validate that our operand is comparable to all elements in the list
		For Each argumentElement As ExpressionElement In MyArguments
			ce.Initialize(MyOperand, argumentElement, LogicalCompareOperation.Equal)
			ce.Validate()
		Next
	End Sub

	Private Sub ResolveForCollectionSearch()
		' Try to find a collection type
		MyTargetCollectionType = Me.GetTargetCollectionType()

		If MyTargetCollectionType Is Nothing Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.SearchArgIsNotKnownCollectionType, CompileExceptionReason.TypeMismatch, MyTargetCollectionElement.ResultType.Name)
		End If

		' Validate that the operand type is compatible with the collection
		Dim mi As MethodInfo = Me.GetCollectionContainsMethod()
		Dim p1 As ParameterInfo = mi.GetParameters()(0)

		If ImplicitConverter.EmitImplicitConvert(MyOperand.ResultType, p1.ParameterType, Nothing) = False Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.OperandNotConvertibleToCollectionType, CompileExceptionReason.TypeMismatch, MyOperand.ResultType.Name, p1.ParameterType.Name)
		End If
	End Sub

	Private Function GetTargetCollectionType() As Type
		Dim collType As Type = MyTargetCollectionElement.ResultType

		' Try to see if the collection is a generic ICollection or IDictionary
		Dim interfaces As Type() = collType.GetInterfaces()

		For Each interfaceType As Type In interfaces
			If interfaceType.IsGenericType = False Then
				Continue For
			End If

			Dim genericTypeDef As Type = interfaceType.GetGenericTypeDefinition()

			If genericTypeDef Is GetType(ICollection(Of )) Or genericTypeDef Is GetType(IDictionary(Of ,)) Then
				Return interfaceType
			End If
		Next

		' Try to see if it is a regular IList or IDictionary
		If GetType(IList).IsAssignableFrom(collType) = True Then
			Return GetType(IList)
		ElseIf GetType(IDictionary).IsAssignableFrom(collType) = True Then
			Return GetType(IDictionary)
		End If

		' Not a known collection type
		Return Nothing
	End Function

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		If Not MyTargetCollectionType Is Nothing Then
			Me.EmitCollectionIn(ilg, services)
		Else

			Dim bm As New BranchManager()
			bm.GetLabel("endLabel", ilg)
			bm.GetLabel("trueTerminal", ilg)

			' Do a fake emit to get branch positions
			Dim ilgTemp As FleeILGenerator = Me.CreateTempFleeILGenerator(ilg)
			Utility.SyncFleeILGeneratorLabels(ilg, ilgTemp)

			Me.EmitListIn(ilgTemp, services, bm)

			bm.ComputeBranches()

			' Do the real emit
			Me.EmitListIn(ilg, services, bm)
		End If
	End Sub

	Private Sub EmitCollectionIn(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		' Get the contains method
		Dim mi As MethodInfo = Me.GetCollectionContainsMethod()
		Dim p1 As ParameterInfo = mi.GetParameters()(0)

		' Load the collection
		MyTargetCollectionElement.Emit(ilg, services)
		' Load the argument
		MyOperand.Emit(ilg, services)
		' Do an implicit convert if necessary
		ImplicitConverter.EmitImplicitConvert(MyOperand.ResultType, p1.ParameterType, ilg)
		' Call the contains method
		ilg.Emit(OpCodes.Callvirt, mi)
	End Sub

	Private Function GetCollectionContainsMethod() As MethodInfo
		Dim methodName As String = "Contains"

		If MyTargetCollectionType.IsGenericType = True AndAlso MyTargetCollectionType.GetGenericTypeDefinition() Is GetType(IDictionary(Of ,)) Then
			methodName = "ContainsKey"
		End If

		Return MyTargetCollectionType.GetMethod(methodName, BindingFlags.Public Or BindingFlags.Instance Or BindingFlags.IgnoreCase)
	End Function

	Private Sub EmitListIn(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider, ByVal bm As BranchManager)
		Dim ce As New CompareElement()
		Dim endLabel As Label = bm.FindLabel("endLabel")
		Dim trueTerminal As Label = bm.FindLabel("trueTerminal")

		' Cache the operand since we will be comparing against it a lot
		Dim lb As LocalBuilder = ilg.DeclareLocal(MyOperand.ResultType)
		Dim targetIndex As Integer = lb.LocalIndex

		MyOperand.Emit(ilg, services)
		Utility.EmitStoreLocal(ilg, targetIndex)

		' Wrap our operand in a local shim
		Dim targetShim As New LocalBasedElement(MyOperand, targetIndex)

		' Emit the compares
		For Each argumentElement As ExpressionElement In MyArguments
			ce.Initialize(targetShim, argumentElement, LogicalCompareOperation.Equal)
			ce.Emit(ilg, services)

			EmitBranchToTrueTerminal(ilg, trueTerminal, bm)
		Next

		ilg.Emit(OpCodes.Ldc_I4_0)
		ilg.Emit(OpCodes.Br_S, endLabel)

		bm.MarkLabel(ilg, trueTerminal)
		ilg.MarkLabel(trueTerminal)

		ilg.Emit(OpCodes.Ldc_I4_1)

		bm.MarkLabel(ilg, endLabel)
		ilg.MarkLabel(endLabel)
	End Sub

	Private Shared Sub EmitBranchToTrueTerminal(ByVal ilg As FleeILGenerator, ByVal trueTerminal As Label, ByVal bm As BranchManager)
		If ilg.IsTemp = True Then
			bm.AddBranch(ilg, trueTerminal)
			ilg.Emit(OpCodes.Brtrue_S, trueTerminal)
		ElseIf bm.IsLongBranch(ilg, trueTerminal) = False Then
			ilg.Emit(OpCodes.Brtrue_S, trueTerminal)
		Else
			ilg.Emit(OpCodes.Brtrue, trueTerminal)
		End If
	End Sub

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return GetType(Boolean)
		End Get
	End Property
End Class