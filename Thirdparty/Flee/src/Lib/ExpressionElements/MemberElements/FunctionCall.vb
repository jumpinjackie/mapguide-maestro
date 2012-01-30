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

''' <summary>
''' Represents a function call
''' </summary>
''' <remarks></remarks>
Friend Class FunctionCallElement
	Inherits MemberElement

	Private MyArguments As ArgumentList
	Private MyMethods As ICollection(Of MethodInfo)
	Private MyTargetMethodInfo As CustomMethodInfo
	Private MyOnDemandFunctionReturnType As Type

	Public Sub New(ByVal name As String, ByVal arguments As ArgumentList)
		Me.MyName = name
		MyArguments = arguments
	End Sub

	Friend Sub New(ByVal name As String, ByVal methods As ICollection(Of MethodInfo), ByVal arguments As ArgumentList)
		MyName = name
		MyArguments = arguments
		MyMethods = methods
	End Sub

	Protected Overrides Sub ResolveInternal()
		' Get the types of our arguments
		Dim argTypes As Type() = MyArguments.GetArgumentTypes()
		' Find all methods with our name on the type
		Dim methods As ICollection(Of MethodInfo) = MyMethods

		If methods Is Nothing Then
			' Convert member info to method info
			Dim arr As MemberInfo() = Me.GetMembers(MemberTypes.Method)
			Dim arr2(arr.Length - 1) As MethodInfo
			Array.Copy(arr, arr2, arr.Length)
			methods = arr2
		End If

		If methods.Count > 0 Then
			' More than one method exists with this name			
			Me.BindToMethod(methods, MyPrevious, argTypes)
			Return
		End If

		' No methods with this name exist; try to bind to an on-demand function
		MyOnDemandFunctionReturnType = MyContext.Variables.ResolveOnDemandFunction(MyName, argTypes)

		If MyOnDemandFunctionReturnType Is Nothing Then
			' Failed to bind to a function
			Me.ThrowFunctionNotFoundException(MyPrevious)
		End If
	End Sub

	Private Sub ThrowFunctionNotFoundException(ByVal previous As MemberElement)
		If previous Is Nothing Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.UndefinedFunction, CompileExceptionReason.UndefinedName, MyName, MyArguments)
		Else
			MyBase.ThrowCompileException(CompileErrorResourceKeys.UndefinedFunctionOnType, CompileExceptionReason.UndefinedName, MyName, MyArguments, previous.TargetType.Name)
		End If
	End Sub

	Private Sub ThrowNoAccessibleMethodsException(ByVal previous As MemberElement)
		If previous Is Nothing Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.NoAccessibleMatches, CompileExceptionReason.AccessDenied, MyName, MyArguments)
		Else
			MyBase.ThrowCompileException(CompileErrorResourceKeys.NoAccessibleMatchesOnType, CompileExceptionReason.AccessDenied, MyName, MyArguments, previous.TargetType.Name)
		End If
	End Sub

	Private Sub ThrowAmbiguousMethodCallException()
		MyBase.ThrowCompileException(CompileErrorResourceKeys.AmbiguousCallOfFunction, CompileExceptionReason.AmbiguousMatch, MyName, MyArguments)
	End Sub

	' Try to find a match from a set of methods
	Private Sub BindToMethod(ByVal methods As ICollection(Of MethodInfo), ByVal previous As MemberElement, ByVal argTypes As Type())
		Dim customInfos As New List(Of CustomMethodInfo)()

		' Wrap the MethodInfos in our custom class
		For Each mi As MethodInfo In methods
			Dim cmi As New CustomMethodInfo(mi)
			customInfos.Add(cmi)
		Next

		' Discard any methods that cannot qualify as overloads
		Dim arr As CustomMethodInfo() = customInfos.ToArray()
		customInfos.Clear()

		For Each cmi As CustomMethodInfo In arr
			If cmi.IsMatch(argTypes) = True Then
				customInfos.Add(cmi)
			End If
		Next

		If customInfos.Count = 0 Then
			' We have no methods that can qualify as overloads; throw exception
			Me.ThrowFunctionNotFoundException(previous)
		Else
			' At least one method matches our criteria; do our custom overload resolution
			Me.ResolveOverloads(customInfos.ToArray(), previous, argTypes)
		End If
	End Sub

	' Find the best match from a set of overloaded methods
	Private Sub ResolveOverloads(ByVal infos As CustomMethodInfo(), ByVal previous As MemberElement, ByVal argTypes As Type())
		' Compute a score for each candidate
		For Each cmi As CustomMethodInfo In infos
			cmi.ComputeScore(argTypes)
		Next

		' Sort array from best to worst matches
		Array.Sort(Of CustomMethodInfo)(infos)

		' Discard any matches that aren't accessible
		infos = Me.GetAccessibleInfos(infos)

		' No accessible methods left
		If infos.Length = 0 Then
			Me.ThrowNoAccessibleMethodsException(previous)
		End If

		' Handle case where we have more than one match with the same score
		Me.DetectAmbiguousMatches(infos)

		' If we get here, then there is only one best match
		MyTargetMethodInfo = infos(0)
	End Sub

	Private Function GetAccessibleInfos(ByVal infos As CustomMethodInfo()) As CustomMethodInfo()
		Dim accessible As New List(Of CustomMethodInfo)()

		For Each cmi As CustomMethodInfo In infos
			If cmi.IsAccessible(Me) = True Then
				accessible.Add(cmi)
			End If
		Next

		Return accessible.ToArray()
	End Function

	' Handle case where we have overloads with the same score
	Private Sub DetectAmbiguousMatches(ByVal infos As CustomMethodInfo())
		Dim sameScores As New List(Of CustomMethodInfo)()
		Dim first As CustomMethodInfo = infos(0)

		' Find all matches with the same score as the best match
		For Each cmi As CustomMethodInfo In infos
			If DirectCast(cmi, IEquatable(Of CustomMethodInfo)).Equals(first) = True Then
				sameScores.Add(cmi)
			End If
		Next

		' More than one accessible match with the same score exists
		If sameScores.Count > 1 Then
			Me.ThrowAmbiguousMethodCallException()
		End If
	End Sub

	Protected Overrides Sub Validate()
		MyBase.Validate()

		If Not MyOnDemandFunctionReturnType Is Nothing Then
			Return
		End If

		' Any function reference in an expression must return a value
		If Me.Method.ReturnType Is GetType(Void) Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.FunctionHasNoReturnValue, CompileExceptionReason.FunctionHasNoReturnValue, MyName)
		End If
	End Sub

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		MyBase.Emit(ilg, services)

		Dim elements As ExpressionElement() = MyArguments.ToArray()

		' If we are an on-demand function, then emit that and exit
		If Not MyOnDemandFunctionReturnType Is Nothing Then
			Me.EmitOnDemandFunction(elements, ilg, services)
			Return
		End If

		Dim isOwnerMember As Boolean = MyOptions.IsOwnerType(Me.Method.ReflectedType)

		' Load the owner if required
		If MyPrevious Is Nothing AndAlso isOwnerMember = True AndAlso Me.IsStatic = False Then
			Me.EmitLoadOwner(ilg)
		End If

		Me.EmitFunctionCall(Me.NextRequiresAddress, ilg, services)
	End Sub

	Private Sub EmitOnDemandFunction(ByVal elements As ExpressionElement(), ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		' Load the variable collection
		EmitLoadVariables(ilg)
		' Load the function name
		ilg.Emit(OpCodes.Ldstr, MyName)
		' Load the arguments array
		EmitElementArrayLoad(elements, GetType(Object), ilg, services)

		' Call the function to get the result
		Dim mi As MethodInfo = VariableCollection.GetFunctionInvokeMethod(MyOnDemandFunctionReturnType)

		Me.EmitMethodCall(mi, ilg)
	End Sub

	' Emit the arguments to a paramArray method call
	Private Sub EmitParamArrayArguments(ByVal parameters As ParameterInfo(), ByVal elements As ExpressionElement(), ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		' Get the fixed parameters
		Dim fixedParameters(MyTargetMethodInfo.MyFixedArgTypes.Length - 1) As ParameterInfo
		Array.Copy(parameters, fixedParameters, fixedParameters.Length)

		' Get the corresponding fixed parameters
		Dim fixedElements(MyTargetMethodInfo.MyFixedArgTypes.Length - 1) As ExpressionElement
		Array.Copy(elements, fixedElements, fixedElements.Length)

		' Emit the fixed arguments
		Me.EmitRegularFunctionInternal(fixedParameters, fixedElements, ilg, services)

		' Get the paramArray arguments
		Dim paramArrayElements(elements.Length - fixedElements.Length - 1) As ExpressionElement
		Array.Copy(elements, fixedElements.Length, paramArrayElements, 0, paramArrayElements.Length)

		' Emit them into an array
		EmitElementArrayLoad(paramArrayElements, MyTargetMethodInfo.ParamArrayElementType, ilg, services)
	End Sub

	' Emit elements into an array
	Private Shared Sub EmitElementArrayLoad(ByVal elements As ExpressionElement(), ByVal arrayElementType As Type, ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		' Load the array length
		LiteralElement.EmitLoad(elements.Length, ilg)

		' Create the array
		ilg.Emit(OpCodes.Newarr, arrayElementType)

		' Store the new array in a unique local and remember the index
		Dim local As LocalBuilder = ilg.DeclareLocal(arrayElementType.MakeArrayType())
		Dim arrayLocalIndex As Integer = local.LocalIndex
		Utility.EmitStoreLocal(ilg, arrayLocalIndex)

		For i As Integer = 0 To elements.Length - 1
			' Load the array
			Utility.EmitLoadLocal(ilg, arrayLocalIndex)
			' Load the index
			LiteralElement.EmitLoad(i, ilg)
			' Emit the element (with any required conversions)
			Dim element As ExpressionElement = elements(i)
			element.Emit(ilg, services)
			ImplicitConverter.EmitImplicitConvert(element.ResultType, arrayElementType, ilg)
			' Store it into the array
			Utility.EmitArrayStore(ilg, arrayElementType)
		Next

		' Load the array
		Utility.EmitLoadLocal(ilg, arrayLocalIndex)
	End Sub

	Public Sub EmitFunctionCall(ByVal nextRequiresAddress As Boolean, ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim parameters As ParameterInfo() = Me.Method.GetParameters()
		Dim elements As ExpressionElement() = MyArguments.ToArray()

		' Emit either a regular or paramArray call
		If MyTargetMethodInfo.IsParamArray = False Then
			Me.EmitRegularFunctionInternal(parameters, elements, ilg, services)
		Else
			Me.EmitParamArrayArguments(parameters, elements, ilg, services)
		End If

		MemberElement.EmitMethodCall(Me.ResultType, nextRequiresAddress, Me.Method, ilg)
	End Sub

	' Emit the arguments to a regular method call
	Private Sub EmitRegularFunctionInternal(ByVal parameters As ParameterInfo(), ByVal elements As ExpressionElement(), ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Debug.Assert(parameters.Length = elements.Length, "argument count mismatch")

		' Emit each element and any required conversions to the actual parameter type
		For i As Integer = 0 To parameters.Length - 1
			Dim element As ExpressionElement = elements(i)
			Dim pi As ParameterInfo = parameters(i)
			element.Emit(ilg, services)
			Dim success As Boolean = ImplicitConverter.EmitImplicitConvert(element.ResultType, pi.ParameterType, ilg)
			Debug.Assert(success, "conversion failed")
		Next
	End Sub

	''' <summary>
	''' The method info we will be calling
	''' </summary>	
	Private ReadOnly Property Method() As MethodInfo
		Get
			Return MyTargetMethodInfo.Target
		End Get
	End Property

	Public Overrides ReadOnly Property ResultType() As Type
		Get
			If Not MyOnDemandFunctionReturnType Is Nothing Then
				Return MyOnDemandFunctionReturnType
			Else
				Return Me.Method.ReturnType
			End If
		End Get
	End Property

	Protected Overrides ReadOnly Property RequiresAddress() As Boolean
		Get
			Return Not IsGetTypeMethod(Me.Method)
		End Get
	End Property

	Protected Overrides ReadOnly Property IsPublic() As Boolean
		Get
			Return Me.Method.IsPublic
		End Get
	End Property

	Public Overrides ReadOnly Property IsStatic() As Boolean
		Get
			Return Me.Method.IsStatic
		End Get
	End Property
End Class