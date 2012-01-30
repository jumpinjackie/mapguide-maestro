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

Imports System.Reflection
Imports System.Reflection.Emit

Friend Enum BinaryArithmeticOperation
	Add
	Subtract
	Multiply
	Divide
	[Mod]
	Power
End Enum

Friend Enum LogicalCompareOperation
	LessThan
	GreaterThan
	Equal
	NotEqual
	LessThanOrEqual
	GreaterThanOrEqual
End Enum

Friend Enum AndOrOperation
	[And]
	[Or]
End Enum

Friend Enum ShiftOperation
	LeftShift
	RightShift
End Enum

Friend Delegate Function ExpressionEvaluator(Of T)(ByVal owner As Object, ByVal context As ExpressionContext, ByVal variables As VariableCollection) As T

Friend MustInherit Class CustomBinder
	Inherits Binder

	Public Overrides Function BindToField(ByVal bindingAttr As System.Reflection.BindingFlags, ByVal match() As System.Reflection.FieldInfo, ByVal value As Object, ByVal culture As System.Globalization.CultureInfo) As System.Reflection.FieldInfo
		Return Nothing
	End Function

	Public Overrides Function BindToMethod(ByVal bindingAttr As System.Reflection.BindingFlags, ByVal match() As System.Reflection.MethodBase, ByRef args() As Object, ByVal modifiers() As System.Reflection.ParameterModifier, ByVal culture As System.Globalization.CultureInfo, ByVal names() As String, ByRef state As Object) As System.Reflection.MethodBase
		Return Nothing
	End Function

	Public Overrides Function ChangeType(ByVal value As Object, ByVal type As System.Type, ByVal culture As System.Globalization.CultureInfo) As Object
		Return Nothing
	End Function

	Public Overrides Sub ReorderArgumentArray(ByRef args() As Object, ByVal state As Object)

	End Sub

	Public Overrides Function SelectProperty(ByVal bindingAttr As System.Reflection.BindingFlags, ByVal match() As System.Reflection.PropertyInfo, ByVal returnType As System.Type, ByVal indexes() As System.Type, ByVal modifiers() As System.Reflection.ParameterModifier) As System.Reflection.PropertyInfo
		Return Nothing
	End Function
End Class

Friend Class ExplicitOperatorMethodBinder
	Inherits CustomBinder

	Private MyReturnType As Type
	Private MyArgType As Type

	Public Sub New(ByVal returnType As Type, ByVal argType As Type)
		MyReturnType = returnType
		MyArgType = argType
	End Sub

	Public Overrides Function SelectMethod(ByVal bindingAttr As System.Reflection.BindingFlags, ByVal match() As System.Reflection.MethodBase, ByVal types() As System.Type, ByVal modifiers() As System.Reflection.ParameterModifier) As System.Reflection.MethodBase
		For Each mi As MethodInfo In match
			Dim parameters As ParameterInfo() = mi.GetParameters()
			Dim firstParameter As ParameterInfo = parameters(0)
			If firstParameter.ParameterType Is MyArgType And mi.ReturnType Is MyReturnType Then
				Return mi
			End If
		Next
		Return Nothing
	End Function
End Class

Friend Class BinaryOperatorBinder
	Inherits CustomBinder

	Private MyLeftType As Type
	Private MyRightType As Type

	Public Sub New(ByVal leftType As Type, ByVal rightType As Type)
		MyLeftType = leftType
		MyRightType = rightType
	End Sub

	Public Overrides Function SelectMethod(ByVal bindingAttr As System.Reflection.BindingFlags, ByVal match() As System.Reflection.MethodBase, ByVal types() As System.Type, ByVal modifiers() As System.Reflection.ParameterModifier) As System.Reflection.MethodBase
		For Each mi As MethodInfo In match
			Dim parameters As ParameterInfo() = mi.GetParameters()
			Dim leftValid As Boolean = ImplicitConverter.EmitImplicitConvert(MyLeftType, parameters(0).ParameterType, Nothing)
			Dim rightValid As Boolean = ImplicitConverter.EmitImplicitConvert(MyRightType, parameters(1).ParameterType, Nothing)

			If leftValid = True And rightValid = True Then
				Return mi
			End If
		Next
		Return Nothing
	End Function
End Class

Friend Class Null

End Class

Friend Class DefaultExpressionOwner

	Private Shared OurInstance As New DefaultExpressionOwner()

	Private Sub New()

	End Sub

	Public Shared ReadOnly Property Instance() As Object
		Get
			Return OurInstance
		End Get
	End Property
End Class

' Helper class to resolve overloads
Friend Class CustomMethodInfo
	Implements IComparable(Of CustomMethodInfo)
	Implements IEquatable(Of CustomMethodInfo)

	Private MyTarget As MethodInfo				' Method we are wrapping
	Private MyScore As Single					' The rating of how close the method matches the given arguments (0 is best)
	Public IsParamArray As Boolean
	Public MyFixedArgTypes As Type()
	Public MyParamArrayArgTypes As Type()
	Public ParamArrayElementType As Type

	Public Sub New(ByVal target As MethodInfo)
		MyTarget = target
	End Sub

	Public Sub ComputeScore(ByVal argTypes As Type())
		Dim params As ParameterInfo() = MyTarget.GetParameters()

		If params.Length = 0 Then
			MyScore = 0.0
		ElseIf IsParamArray = True Then
			MyScore = Me.ComputeScoreForParamArray(params, argTypes)
		Else
			MyScore = Me.ComputeScoreInternal(params, argTypes)
		End If
	End Sub

	' Compute a score showing how close our method matches the given argument types
	Private Function ComputeScoreInternal(ByVal parameters As ParameterInfo(), ByVal argTypes As Type()) As Single
		' Our score is the average of the scores of each parameter.  The lower the score, the better the match.
		Dim sum As Integer = ComputeSum(parameters, argTypes)

		Return sum / argTypes.Length
	End Function

	Private Shared Function ComputeSum(ByVal parameters As ParameterInfo(), ByVal argTypes As Type()) As Integer
		Debug.Assert(parameters.Length = argTypes.Length)
		Dim sum As Integer = 0

		For i As Integer = 0 To parameters.Length - 1
			sum += ImplicitConverter.GetImplicitConvertScore(argTypes(i), parameters(i).ParameterType)
		Next

		Return sum
	End Function

	Private Function ComputeScoreForParamArray(ByVal parameters As ParameterInfo(), ByVal argTypes As Type()) As Single
		Dim paramArrayParameter As ParameterInfo = parameters(parameters.Length - 1)
		Dim fixedParameterCount As Integer = paramArrayParameter.Position

		Dim fixedParameters(fixedParameterCount - 1) As ParameterInfo

		System.Array.Copy(parameters, fixedParameters, fixedParameterCount)

		Dim fixedSum As Integer = ComputeSum(fixedParameters, MyFixedArgTypes)

		Dim paramArrayElementType As Type = paramArrayParameter.ParameterType.GetElementType()

		Dim paramArraySum As Integer = 0

		For Each argType As Type In MyParamArrayArgTypes
			paramArraySum += ImplicitConverter.GetImplicitConvertScore(argType, paramArrayElementType)
		Next

		Dim score As Single

		If argTypes.Length > 0 Then
			score = (fixedSum + paramArraySum) / argTypes.Length
		Else
			score = 0
		End If

		' The param array score gets a slight penalty so that it scores worse than direct matches
		Return score + 1
	End Function

	Public Function IsAccessible(ByVal owner As MemberElement) As Boolean
		Return owner.IsMemberAccessible(MyTarget)
	End Function

	' Is the given MethodInfo usable as an overload?
	Public Function IsMatch(ByVal argTypes As Type()) As Boolean
		Dim parameters As ParameterInfo() = MyTarget.GetParameters()

		' If there are no parameters and no arguments were passed, then we are a match.
		If parameters.Length = 0 And argTypes.Length = 0 Then
			Return True
		End If

		' If there are no parameters but there are arguments, we cannot be a match
		If parameters.Length = 0 And argTypes.Length > 0 Then
			Return False
		End If

		' Is the last parameter a paramArray?
		Dim lastParam As ParameterInfo = parameters(parameters.Length - 1)

		If lastParam.IsDefined(GetType(ParamArrayAttribute), False) = False Then
			If (parameters.Length <> argTypes.Length) Then
				' Not a paramArray and parameter and argument counts don't match
				Return False
			Else
				' Regular method call, do the test
				Return AreValidArgumentsForParameters(argTypes, parameters)
			End If
		End If

		' At this point, we are dealing with a paramArray call

		' If the parameter and argument counts are equal and there is an implicit conversion from one to the other, we are a match.
		If parameters.Length = argTypes.Length AndAlso AreValidArgumentsForParameters(argTypes, parameters) = True Then
			Return True
		ElseIf Me.IsParamArrayMatch(argTypes, parameters, lastParam) = True Then
			IsParamArray = True
			Return True
		Else
			Return False
		End If
	End Function

	Private Function IsParamArrayMatch(ByVal argTypes As Type(), ByVal parameters As ParameterInfo(), ByVal paramArrayParameter As ParameterInfo) As Boolean
		' Get the count of arguments before the paramArray parameter
		Dim fixedParameterCount As Integer = paramArrayParameter.Position
		Dim fixedArgTypes(fixedParameterCount - 1) As Type
		Dim fixedParameters(fixedParameterCount - 1) As ParameterInfo

		' Get the argument types and parameters before the paramArray
		System.Array.Copy(argTypes, fixedArgTypes, fixedParameterCount)
		System.Array.Copy(parameters, fixedParameters, fixedParameterCount)

		' If the fixed arguments don't match, we are not a match
		If AreValidArgumentsForParameters(fixedArgTypes, fixedParameters) = False Then
			Return False
		End If

		' Get the type of the paramArray
		ParamArrayElementType = paramArrayParameter.ParameterType.GetElementType()

		' Get the types of the arguments passed to the paramArray
		Dim paramArrayArgTypes(argTypes.Length - fixedParameterCount - 1) As Type
		System.Array.Copy(argTypes, fixedParameterCount, paramArrayArgTypes, 0, paramArrayArgTypes.Length)

		' Check each argument
		For Each argType As Type In paramArrayArgTypes
			If ImplicitConverter.EmitImplicitConvert(argType, ParamArrayElementType, Nothing) = False Then
				Return False
			End If
		Next

		MyFixedArgTypes = fixedArgTypes
		MyParamArrayArgTypes = paramArrayArgTypes

		' They all match, so we are a match
		Return True
	End Function

	Private Shared Function AreValidArgumentsForParameters(ByVal argTypes As Type(), ByVal parameters As ParameterInfo()) As Boolean
		Debug.Assert(argTypes.Length = parameters.Length)
		' Match if every given argument is implicitly convertible to the method's corresponding parameter
		For i As Integer = 0 To argTypes.Length - 1
			If ImplicitConverter.EmitImplicitConvert(argTypes(i), parameters(i).ParameterType, Nothing) = False Then
				Return False
			End If
		Next

		Return True
	End Function

	Private Function CompareTo(ByVal other As CustomMethodInfo) As Integer Implements System.IComparable(Of CustomMethodInfo).CompareTo
		Return MyScore.CompareTo(other.MyScore)
	End Function

	Private Function Equals1(ByVal other As CustomMethodInfo) As Boolean Implements System.IEquatable(Of CustomMethodInfo).Equals
		Return MyScore = other.MyScore
	End Function

	Public ReadOnly Property Target() As MethodInfo
		Get
			Return MyTarget
		End Get
	End Property
End Class

Friend Class ShortCircuitInfo

	Public Operands As Stack
	Public Operators As Stack
	Public Branches As BranchManager

	Public Sub New()
		Me.Operands = New Stack()
		Me.Operators = New Stack()
		Me.Branches = New BranchManager()
	End Sub

	Public Sub ClearTempState()
		Me.Operands.Clear()
		Me.Operators.Clear()
	End Sub
End Class

' Wraps an expression element so that it is loaded from a local slot
Friend Class LocalBasedElement
	Inherits ExpressionElement

	Private MyIndex As Integer
	Private MyTarget As ExpressionElement

	Public Sub New(ByVal target As ExpressionElement, ByVal index As Integer)
		MyTarget = target
		MyIndex = index
	End Sub

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Utility.EmitLoadLocal(ilg, MyIndex)
	End Sub

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return MyTarget.ResultType
		End Get
	End Property
End Class

''' <summary>
''' Helper class for storing strongly-typed properties
''' </summary>
''' <remarks></remarks>
Friend Class PropertyDictionary

	Private MyProperties As Dictionary(Of String, Object)

	Public Sub New()
		MyProperties = New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)
	End Sub

	Public Function Clone() As PropertyDictionary
		Dim copy As New PropertyDictionary()

		For Each pair As KeyValuePair(Of String, Object) In MyProperties
			copy.SetValue(pair.Key, pair.Value)
		Next

		Return copy
	End Function

	Public Function GetValue(Of T)(ByVal name As String) As T
		Dim value As T = Nothing
		If MyProperties.TryGetValue(name, value) = False Then
			Debug.Fail(String.Format("Unknown property '{0}'", name))
		End If
		Return value
	End Function

	Public Sub SetToDefault(Of T)(ByVal name As String)
		Dim value As T = Nothing
		Me.SetValue(name, value)
	End Sub

	Public Sub SetValue(ByVal name As String, ByVal value As Object)
		MyProperties.Item(name) = value
	End Sub

	Public Function Contains(ByVal name As String) As Boolean
		Return MyProperties.ContainsKey(name)
	End Function
End Class