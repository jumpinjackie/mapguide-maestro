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
Imports System.ComponentModel
Imports Ciloci.Flee.CalcEngine

' Represents an identifier
Friend Class IdentifierElement
	Inherits MemberElement

	Private MyField As FieldInfo
	Private MyProperty As PropertyInfo
	Private MyPropertyDescriptor As PropertyDescriptor
	Private MyVariableType As Type
	Private MyCalcEngineReferenceType As Type

	Public Sub New(ByVal name As String)
		Me.MyName = name
	End Sub

	Protected Overrides Sub ResolveInternal()
		' Try to bind to a field or property
		If Me.ResolveFieldProperty(MyPrevious) = True Then
			Me.AddReferencedVariable(MyPrevious)
			Return
		End If

		' Try to find a variable with our name
		MyVariableType = MyContext.Variables.GetVariableTypeInternal(MyName)

		' Variables are only usable as the first element
		If MyPrevious Is Nothing AndAlso Not MyVariableType Is Nothing Then
			Me.AddReferencedVariable(MyPrevious)
			Return
		End If

		Dim ce As CalculationEngine = MyContext.CalculationEngine

		If Not ce Is Nothing Then
			ce.AddDependency(MyName, MyContext)
			MyCalcEngineReferenceType = ce.ResolveTailType(MyName)
			Return
		End If

		If MyPrevious Is Nothing Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.NoIdentifierWithName, CompileExceptionReason.UndefinedName, MyName)
		Else
			MyBase.ThrowCompileException(CompileErrorResourceKeys.NoIdentifierWithNameOnType, CompileExceptionReason.UndefinedName, MyName, MyPrevious.TargetType.Name)
		End If
	End Sub

	Private Function ResolveFieldProperty(ByVal previous As MemberElement) As Boolean
		Dim members As MemberInfo() = Me.GetMembers(MemberTypes.Field Or MemberTypes.Property)

		' Keep only the ones which are accessible
		members = Me.GetAccessibleMembers(members)

		If members.Length = 0 Then
			' No accessible members; try to resolve a virtual property
			Return Me.ResolveVirtualProperty(previous)
		ElseIf members.Length > 1 Then
			' More than one accessible member
			If previous Is Nothing Then
				MyBase.ThrowCompileException(CompileErrorResourceKeys.IdentifierIsAmbiguous, CompileExceptionReason.AmbiguousMatch, MyName)
			Else
				MyBase.ThrowCompileException(CompileErrorResourceKeys.IdentifierIsAmbiguousOnType, CompileExceptionReason.AmbiguousMatch, MyName, previous.TargetType.Name)
			End If
		Else
			' Only one member; bind to it
			MyField = TryCast(members(0), FieldInfo)
			If Not MyField Is Nothing Then
				Return True
			End If

			' Assume it must be a property
			MyProperty = DirectCast(members(0), PropertyInfo)
			Return True
		End If
	End Function

	Private Function ResolveVirtualProperty(ByVal previous As MemberElement) As Boolean
		If previous Is Nothing Then
			' We can't use virtual properties if we are the first element
			Return False
		End If

		Dim coll As PropertyDescriptorCollection = TypeDescriptor.GetProperties(previous.ResultType)
		MyPropertyDescriptor = coll.Find(MyName, True)
		Return Not MyPropertyDescriptor Is Nothing
	End Function

	Private Sub AddReferencedVariable(ByVal previous As MemberElement)
		If Not previous Is Nothing Then
			Return
		End If

		If Not MyVariableType Is Nothing OrElse MyOptions.IsOwnerType(Me.MemberOwnerType) = True Then
			Dim info As ExpressionInfo = MyServices.GetService(GetType(ExpressionInfo))
			info.AddReferencedVariable(MyName)
		End If
	End Sub

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		MyBase.Emit(ilg, services)

		Me.EmitFirst(ilg)

		If Not MyCalcEngineReferenceType Is Nothing Then
			Me.EmitReferenceLoad(ilg)
		ElseIf Not MyVariableType Is Nothing Then
			Me.EmitVariableLoad(ilg)
		ElseIf Not MyField Is Nothing Then
			Me.EmitFieldLoad(MyField, ilg, services)
		ElseIf Not MyPropertyDescriptor Is Nothing Then
			Me.EmitVirtualPropertyLoad(ilg)
		Else
			Me.EmitPropertyLoad(MyProperty, ilg)
		End If
	End Sub

	Private Sub EmitReferenceLoad(ByVal ilg As FleeILGenerator)
		ilg.Emit(OpCodes.Ldarg_1)
		MyContext.CalculationEngine.EmitLoad(MyName, ilg)
	End Sub

	Private Sub EmitFirst(ByVal ilg As FleeILGenerator)
		If Not MyPrevious Is Nothing Then
			Return
		End If

		Dim isVariable As Boolean = Not MyVariableType Is Nothing

		If isVariable = True Then
			' Load variables
			EmitLoadVariables(ilg)
		ElseIf MyOptions.IsOwnerType(Me.MemberOwnerType) = True And Me.IsStatic = False Then
			Me.EmitLoadOwner(ilg)
		End If
	End Sub

	Private Sub EmitVariableLoad(ByVal ilg As FleeILGenerator)
		Dim mi As MethodInfo = VariableCollection.GetVariableLoadMethod(MyVariableType)
		ilg.Emit(OpCodes.Ldstr, MyName)
		Me.EmitMethodCall(mi, ilg)
	End Sub

	Private Sub EmitFieldLoad(ByVal fi As System.Reflection.FieldInfo, ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		If fi.IsLiteral = True Then
			EmitLiteral(fi, ilg, services)
		ElseIf Me.ResultType.IsValueType = True And Me.NextRequiresAddress = True Then
			EmitLdfld(fi, True, ilg)
		Else
			EmitLdfld(fi, False, ilg)
		End If
	End Sub

	Private Shared Sub EmitLdfld(ByVal fi As System.Reflection.FieldInfo, ByVal indirect As Boolean, ByVal ilg As FleeILGenerator)
		If fi.IsStatic = True Then
			If indirect = True Then
				ilg.Emit(OpCodes.Ldsflda, fi)
			Else
				ilg.Emit(OpCodes.Ldsfld, fi)
			End If
		Else
			If indirect = True Then
				ilg.Emit(OpCodes.Ldflda, fi)
			Else
				ilg.Emit(OpCodes.Ldfld, fi)
			End If
		End If
	End Sub

	' Emit the load of a constant field.  We can't emit a ldsfld/ldfld of a constant so we have to get its value
	' and then emit a ldc.
	Private Shared Sub EmitLiteral(ByVal fi As System.Reflection.FieldInfo, ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim value As Object = fi.GetValue(Nothing)
		Dim t As Type = value.GetType()
		Dim code As TypeCode = Type.GetTypeCode(t)
		Dim elem As LiteralElement

		Select Case code
			Case TypeCode.Char, TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32
				elem = New Int32LiteralElement(System.Convert.ToInt32(value))
			Case TypeCode.UInt32
				elem = New UInt32LiteralElement(DirectCast(value, UInt32))
			Case TypeCode.Int64
				elem = New Int64LiteralElement(DirectCast(value, Int64))
			Case TypeCode.UInt64
				elem = New UInt64LiteralElement(DirectCast(value, UInt64))
			Case TypeCode.Double
				elem = New DoubleLiteralElement(DirectCast(value, Double))
			Case TypeCode.Single
				elem = New SingleLiteralElement(DirectCast(value, Single))
			Case TypeCode.Boolean
				elem = New BooleanLiteralElement(DirectCast(value, Boolean))
			Case TypeCode.String
				elem = New StringLiteralElement(DirectCast(value, String))
			Case Else
				elem = Nothing
				Debug.Fail("Unsupported constant type")
		End Select

		elem.Emit(ilg, services)
	End Sub

	Private Sub EmitPropertyLoad(ByVal pi As System.Reflection.PropertyInfo, ByVal ilg As FleeILGenerator)
		Dim getter As System.Reflection.MethodInfo = pi.GetGetMethod(True)
		MyBase.EmitMethodCall(getter, ilg)
	End Sub

	' Load a PropertyDescriptor based property
	Private Sub EmitVirtualPropertyLoad(ByVal ilg As FleeILGenerator)
		' The previous value is already on the top of the stack but we need it at the bottom

		' Get a temporary local index
		Dim index As Integer = ilg.GetTempLocalIndex(MyPrevious.ResultType)

		' Store the previous value there
		Utility.EmitStoreLocal(ilg, index)

		' Load the variable collection
		EmitLoadVariables(ilg)
		' Load the property name
		ilg.Emit(OpCodes.Ldstr, MyName)

		' Load the previous value and convert it to object
		Utility.EmitLoadLocal(ilg, index)
		ImplicitConverter.EmitImplicitConvert(MyPrevious.ResultType, GetType(Object), ilg)

		' Call the method to get the actual value
		Dim mi As MethodInfo = VariableCollection.GetVirtualPropertyLoadMethod(Me.ResultType)
		Me.EmitMethodCall(mi, ilg)
	End Sub

	Private ReadOnly Property MemberOwnerType() As Type
		Get
			If Not MyField Is Nothing Then
				Return MyField.ReflectedType
			ElseIf Not MyPropertyDescriptor Is Nothing Then
				Return MyPropertyDescriptor.ComponentType
			ElseIf Not MyProperty Is Nothing Then
				Return MyProperty.ReflectedType
			Else
				Return Nothing
			End If
		End Get
	End Property

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			If Not MyCalcEngineReferenceType Is Nothing Then
				Return MyCalcEngineReferenceType
			ElseIf Not MyVariableType Is Nothing Then
				Return MyVariableType
			ElseIf Not MyPropertyDescriptor Is Nothing Then
				Return MyPropertyDescriptor.PropertyType
			ElseIf Not MyField Is Nothing Then
				Return MyField.FieldType
			Else
				Dim mi As MethodInfo = MyProperty.GetGetMethod(True)
				Return mi.ReturnType
			End If
		End Get
	End Property

	Protected Overrides ReadOnly Property RequiresAddress() As Boolean
		Get
			Return MyPropertyDescriptor Is Nothing
		End Get
	End Property

	Protected Overrides ReadOnly Property IsPublic() As Boolean
		Get
			If Not MyVariableType Is Nothing Or Not MyCalcEngineReferenceType Is Nothing Then
				Return True
			ElseIf Not MyVariableType Is Nothing Then
				Return True
			ElseIf Not MyPropertyDescriptor Is Nothing Then
				Return True
			ElseIf Not MyField Is Nothing Then
				Return MyField.IsPublic
			Else
				Dim mi As MethodInfo = MyProperty.GetGetMethod(True)
				Return mi.IsPublic
			End If
		End Get
	End Property

	Protected Overrides ReadOnly Property SupportsStatic() As Boolean
		Get
			If Not MyVariableType Is Nothing Then
				' Variables never support static
				Return False
			ElseIf Not MyPropertyDescriptor Is Nothing Then
				' Neither do virtual properties
				Return False
			ElseIf MyOptions.IsOwnerType(Me.MemberOwnerType) = True AndAlso MyPrevious Is Nothing Then
				' Owner members support static if we are the first element
				Return True
			Else
				' Support static if we are the first (ie: we are a static import)
				Return MyPrevious Is Nothing
			End If
		End Get
	End Property

	Protected Overrides ReadOnly Property SupportsInstance() As Boolean
		Get
			If Not MyVariableType Is Nothing Then
				' Variables always support instance
				Return True
			ElseIf Not MyPropertyDescriptor Is Nothing Then
				' So do virtual properties
				Return True
			ElseIf MyOptions.IsOwnerType(Me.MemberOwnerType) = True AndAlso MyPrevious Is Nothing Then
				' Owner members support instance if we are the first element
				Return True
			Else
				' We always support instance if we are not the first element
				Return Not MyPrevious Is Nothing
			End If
		End Get
	End Property

	Public Overrides ReadOnly Property IsStatic() As Boolean
		Get
			If Not MyVariableType Is Nothing Or Not MyCalcEngineReferenceType Is Nothing Then
				Return False
			ElseIf Not MyVariableType Is Nothing Then
				Return False
			ElseIf Not MyField Is Nothing Then
				Return MyField.IsStatic
			ElseIf Not MyPropertyDescriptor Is Nothing Then
				Return False
			Else
				Dim mi As MethodInfo = MyProperty.GetGetMethod(True)
				Return mi.IsStatic
			End If
		End Get
	End Property
End Class