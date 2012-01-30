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

' Class that manages an expression's variables

Imports System.Reflection
Imports System.ComponentModel

''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/Class/*' />
Public NotInheritable Class VariableCollection
	Implements IDictionary(Of String, Object)

	Private MyVariables As IDictionary(Of String, IVariable)
	Private MyContext As ExpressionContext

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/ResolveVariableType/*' />
	Public Event ResolveVariableType As EventHandler(Of ResolveVariableTypeEventArgs)
	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/ResolveVariableValue/*' />
	Public Event ResolveVariableValue As EventHandler(Of ResolveVariableValueEventArgs)

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/ResolveFunction/*' />
	Public Event ResolveFunction As EventHandler(Of ResolveFunctionEventArgs)
	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/InvokeFunction/*' />
	Public Event InvokeFunction As EventHandler(Of InvokeFunctionEventArgs)

	Friend Sub New(ByVal context As ExpressionContext)
		MyContext = context
		Me.CreateDictionary()
		Me.HookOptions()
	End Sub

#Region "Methods - Non Public"
	Private Sub HookOptions()
		AddHandler MyContext.Options.CaseSensitiveChanged, AddressOf OnOptionsCaseSensitiveChanged
	End Sub

	Private Sub CreateDictionary()
		MyVariables = New Dictionary(Of String, IVariable)(MyContext.Options.StringComparer)
	End Sub

	Private Sub OnOptionsCaseSensitiveChanged(ByVal sender As Object, ByVal e As EventArgs)
		Me.CreateDictionary()
	End Sub

	Friend Sub Copy(ByVal dest As VariableCollection)
		dest.CreateDictionary()
		dest.HookOptions()

		For Each pair As KeyValuePair(Of String, IVariable) In MyVariables
			Dim copyVariable As IVariable = pair.Value.Clone()
			dest.MyVariables.Add(pair.Key, copyVariable)
		Next
	End Sub

	Friend Sub DefineVariableInternal(ByVal name As String, ByVal variableType As Type, ByVal variableValue As Object)
		Utility.AssertNotNull(variableType, "variableType")

		If MyVariables.ContainsKey(name) = True Then
			Dim msg As String = Utility.GetGeneralErrorMessage(GeneralErrorResourceKeys.VariableWithNameAlreadyDefined, name)
			Throw New ArgumentException(msg)
		End If

		Dim v As IVariable = Me.CreateVariable(variableType, variableValue)
		MyVariables.Add(name, v)
	End Sub

	Friend Function GetVariableTypeInternal(ByVal name As String) As Type
		Dim value As IVariable = Nothing
		Dim success As Boolean = MyVariables.TryGetValue(name, value)

		If success = True Then
			Return value.VariableType
		End If

		Dim args As New ResolveVariableTypeEventArgs(name)
		RaiseEvent ResolveVariableType(Me, args)

		Return args.VariableType
	End Function

	Private Function GetVariable(ByVal name As String, ByVal throwOnNotFound As Boolean) As IVariable
		Dim value As IVariable = Nothing
		Dim success As Boolean = MyVariables.TryGetValue(name, value)

		If success = False And throwOnNotFound = True Then
			Dim msg As String = Utility.GetGeneralErrorMessage(GeneralErrorResourceKeys.UndefinedVariable, name)
			Throw New ArgumentException(msg)
		Else
			Return value
		End If
	End Function

	''' <summary>
	''' Create a variable
	''' </summary>
	''' <param name="variableValueType">The variable's type</param>
	''' <param name="variableValue">The actual value; may be null</param>
	''' <returns>A new variable for the value</returns>
	''' <remarks></remarks>
	Private Function CreateVariable(ByVal variableValueType As Type, ByVal variableValue As Object) As IVariable
		Dim variableType As Type

		' Is the variable value an expression?
		Dim expression As IExpression = TryCast(variableValue, IExpression)
		Dim options As ExpressionOptions = Nothing

		If expression IsNot Nothing Then
			options = expression.Context.Options
			' Get its result type
			variableValueType = options.ResultType
		End If

		If expression IsNot Nothing Then
			' Create a variable that wraps the expression

			If options.IsGeneric = False Then
				variableType = GetType(DynamicExpressionVariable(Of ))
			Else
				variableType = GetType(GenericExpressionVariable(Of ))
			End If
		Else
			' Create a variable for a regular value
			MyContext.AssertTypeIsAccessible(variableValueType)
			variableType = GetType(GenericVariable(Of ))
		End If

		' Create the generic variable instance
		variableType = variableType.MakeGenericType(variableValueType)
		Dim v As IVariable = Activator.CreateInstance(variableType)

		Return v
	End Function

	Friend Function ResolveOnDemandFunction(ByVal name As String, ByVal argumentTypes As Type()) As Type
		Dim args As New ResolveFunctionEventArgs(name, argumentTypes)
		RaiseEvent ResolveFunction(Me, args)
		Return args.ReturnType
	End Function

	Private Shared Function ReturnGenericValue(Of T)(ByVal value As Object) As T
		If value Is Nothing Then
			Return Nothing
		Else
			Return DirectCast(value, T)
		End If
	End Function

	Private Shared Sub ValidateSetValueType(ByVal requiredType As Type, ByVal value As Object)
		If value Is Nothing Then
			' Can always assign null value
			Return
		End If

		Dim valueType As Type = value.GetType()

		If requiredType.IsAssignableFrom(valueType) = False Then
			Dim msg As String = Utility.GetGeneralErrorMessage(GeneralErrorResourceKeys.VariableValueNotAssignableToType, valueType.Name, requiredType.Name)
			Throw New ArgumentException(msg)
		End If
	End Sub

	Friend Shared Function GetVariableLoadMethod(ByVal variableType As Type) As MethodInfo
		Dim mi As MethodInfo = GetType(VariableCollection).GetMethod("GetVariableValueInternal", BindingFlags.Public Or BindingFlags.Instance)
		mi = mi.MakeGenericMethod(variableType)
		Return mi
	End Function

	Friend Shared Function GetFunctionInvokeMethod(ByVal returnType As Type) As MethodInfo
		Dim mi As MethodInfo = GetType(VariableCollection).GetMethod("GetFunctionResultInternal", BindingFlags.Public Or BindingFlags.Instance)
		mi = mi.MakeGenericMethod(returnType)
		Return mi
	End Function

	Friend Shared Function GetVirtualPropertyLoadMethod(ByVal returnType As Type) As MethodInfo
		Dim mi As MethodInfo = GetType(VariableCollection).GetMethod("GetVirtualPropertyValueInternal", BindingFlags.Public Or BindingFlags.Instance)
		mi = mi.MakeGenericMethod(returnType)
		Return mi
	End Function

	Private Function GetNameValueDictionary() As Dictionary(Of String, Object)
		Dim dict As New Dictionary(Of String, Object)()

		For Each pair As KeyValuePair(Of String, IVariable) In MyVariables
			dict.Add(pair.Key, pair.Value.ValueAsObject)
		Next

		Return dict
	End Function
#End Region

#Region "Methods - Public"
	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/GetVariableType/*' />
	Public Function GetVariableType(ByVal name As String) As Type
		Dim v As IVariable = Me.GetVariable(name, True)
		Return v.VariableType
	End Function

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/DefineVariable/*' />
	Public Sub DefineVariable(ByVal name As String, ByVal variableType As Type)
		Me.DefineVariableInternal(name, variableType, Nothing)
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/GetVariableValueInternal/*' />
	Public Function GetVariableValueInternal(Of T)(ByVal name As String) As T
		Dim v As IGenericVariable(Of T) = Nothing

		If MyVariables.TryGetValue(name, v) = True Then
			Return v.GetValue()
		End If

		Dim vTemp As New GenericVariable(Of T)
		Dim args As New ResolveVariableValueEventArgs(name, GetType(T))
		RaiseEvent ResolveVariableValue(Me, args)

		ValidateSetValueType(GetType(T), args.VariableValue)
		vTemp.ValueAsObject = args.VariableValue
		v = vTemp

		Return v.GetValue()
	End Function

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/GetVirtualPropertyValueInternal/*' />
	Public Function GetVirtualPropertyValueInternal(Of T)(ByVal name As String, ByVal component As Object) As T
		Dim coll As PropertyDescriptorCollection = TypeDescriptor.GetProperties(component)
		Dim pd As PropertyDescriptor = coll.Find(name, True)

		Dim value As Object = pd.GetValue(component)
		ValidateSetValueType(GetType(T), value)
		Return ReturnGenericValue(Of T)(value)
	End Function

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/GetFunctionResultInternal/*' />
	Public Function GetFunctionResultInternal(Of T)(ByVal name As String, ByVal arguments As Object()) As T
		Dim args As New InvokeFunctionEventArgs(name, arguments)
		RaiseEvent InvokeFunction(Me, args)

		Dim result As Object = args.Result
		ValidateSetValueType(GetType(T), result)

		Return ReturnGenericValue(Of T)(result)
	End Function
#End Region

#Region "IDictionary Implementation"
	Private Sub Add1(ByVal item As System.Collections.Generic.KeyValuePair(Of String, Object)) Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, Object)).Add
		Me.Add(item.Key, item.Value)
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/Clear/*' />
	Public Sub Clear() Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, Object)).Clear
		MyVariables.Clear()
	End Sub

	Private Function Contains1(ByVal item As System.Collections.Generic.KeyValuePair(Of String, Object)) As Boolean Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, Object)).Contains
		Return Me.ContainsKey(item.Key)
	End Function

	Private Sub CopyTo(ByVal array() As System.Collections.Generic.KeyValuePair(Of String, Object), ByVal arrayIndex As Integer) Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, Object)).CopyTo
		Dim dict As Dictionary(Of String, Object) = Me.GetNameValueDictionary()
		Dim coll As ICollection(Of KeyValuePair(Of String, Object)) = dict
		coll.CopyTo(array, arrayIndex)
	End Sub

	Private Function Remove1(ByVal item As System.Collections.Generic.KeyValuePair(Of String, Object)) As Boolean Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, Object)).Remove
		Me.Remove(item.Key)
	End Function

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/Add/*' />
	Public Sub Add(ByVal name As String, ByVal value As Object) Implements System.Collections.Generic.IDictionary(Of String, Object).Add
		Utility.AssertNotNull(value, "value")
		Me.DefineVariableInternal(name, value.GetType(), value)
		Me.Item(name) = value
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/ContainsKey/*' />
	Public Function ContainsKey(ByVal name As String) As Boolean Implements System.Collections.Generic.IDictionary(Of String, Object).ContainsKey
		Return MyVariables.ContainsKey(name)
	End Function

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/Remove/*' />
	Public Function Remove(ByVal name As String) As Boolean Implements System.Collections.Generic.IDictionary(Of String, Object).Remove
		MyVariables.Remove(name)
	End Function

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/TryGetValue/*' />
	Public Function TryGetValue(ByVal key As String, ByRef value As Object) As Boolean Implements System.Collections.Generic.IDictionary(Of String, Object).TryGetValue
		Dim v As IVariable = Me.GetVariable(key, False)
		If Not v Is Nothing Then
			value = v.ValueAsObject
		End If

		Return Not v Is Nothing
	End Function

	Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of System.Collections.Generic.KeyValuePair(Of String, Object)) Implements System.Collections.Generic.IEnumerable(Of System.Collections.Generic.KeyValuePair(Of String, Object)).GetEnumerator
		Dim dict As Dictionary(Of String, Object) = Me.GetNameValueDictionary()
		Return dict.GetEnumerator()
	End Function

	Private Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
		Return Me.GetEnumerator()
	End Function

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/Count/*' />
	Public ReadOnly Property Count() As Integer Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, Object)).Count
		Get
			Return MyVariables.Count
		End Get
	End Property

	Private ReadOnly Property IsReadOnly() As Boolean Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, Object)).IsReadOnly
		Get
			Return False
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/Item/*' />
	Default Public Overloads Property Item(ByVal name As String) As Object Implements System.Collections.Generic.IDictionary(Of String, Object).Item
		Get
			Dim v As IVariable = Me.GetVariable(name, True)
			Return v.ValueAsObject
		End Get
		Set(ByVal value As Object)
			Dim v As IVariable = Nothing

			If MyVariables.TryGetValue(name, v) = True Then
				v.ValueAsObject = value
			Else
				Me.Add(name, value)
			End If
		End Set
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/Keys/*' />
	Public ReadOnly Property Keys() As System.Collections.Generic.ICollection(Of String) Implements System.Collections.Generic.IDictionary(Of String, Object).Keys
		Get
			Return MyVariables.Keys
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/VariableCollection/Values/*' />
	Public ReadOnly Property Values() As System.Collections.Generic.ICollection(Of Object) Implements System.Collections.Generic.IDictionary(Of String, Object).Values
		Get
			Dim dict As Dictionary(Of String, Object) = Me.GetNameValueDictionary()
			Return dict.Values
		End Get
	End Property
#End Region
End Class