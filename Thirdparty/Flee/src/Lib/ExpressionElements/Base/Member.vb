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

' Base class for all member elements
Friend MustInherit Class MemberElement
	Inherits ExpressionElement

	Protected MyName As String
	Protected MyPrevious As MemberElement
	Protected MyNext As MemberElement
	Protected MyServices As IServiceProvider
	Protected MyOptions As ExpressionOptions
	Protected MyContext As ExpressionContext
	Protected MyImport As ImportBase
	Public Const BindFlags As BindingFlags = BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Static

	Protected Sub New()

	End Sub

	Public Sub Link(ByVal nextElement As MemberElement)
		MyNext = nextElement
		If Not nextElement Is Nothing Then
			nextElement.MyPrevious = Me
		End If
	End Sub

	Public Sub Resolve(ByVal services As IServiceProvider)
		MyServices = services
		MyOptions = services.GetService(GetType(ExpressionOptions))
		MyContext = services.GetService(GetType(ExpressionContext))
		Me.ResolveInternal()
		Me.Validate()
	End Sub

	Public Sub SetImport(ByVal import As ImportBase)
		MyImport = import
	End Sub

	Protected MustOverride Sub ResolveInternal()
	Public MustOverride ReadOnly Property IsStatic() As Boolean
	Protected MustOverride ReadOnly Property IsPublic() As Boolean

	Protected Overridable Sub Validate()
		If MyPrevious Is Nothing Then
			Return
		End If

		If Me.IsStatic = True AndAlso Me.SupportsStatic = False Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.StaticMemberCannotBeAccessedWithInstanceReference, CompileExceptionReason.TypeMismatch, MyName)
		ElseIf Me.IsStatic = False AndAlso Me.SupportsInstance = False Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.ReferenceToNonSharedMemberRequiresObjectReference, CompileExceptionReason.TypeMismatch, MyName)
		End If
	End Sub

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		If Not MyPrevious Is Nothing Then
			MyPrevious.Emit(ilg, services)
		End If
	End Sub

	Protected Shared Sub EmitLoadVariables(ByVal ilg As FleeILGenerator)
		ilg.Emit(OpCodes.Ldarg_2)
	End Sub

	' Handles a call emit for static, instance methods of reference/value types
	Protected Sub EmitMethodCall(ByVal mi As MethodInfo, ByVal ilg As FleeILGenerator)
		EmitMethodCall(Me.ResultType, Me.NextRequiresAddress, mi, ilg)
	End Sub

	Protected Shared Sub EmitMethodCall(ByVal resultType As Type, ByVal nextRequiresAddress As Boolean, ByVal mi As MethodInfo, ByVal ilg As FleeILGenerator)
		If mi.ReflectedType.IsValueType = False Then
			EmitReferenceTypeMethodCall(mi, ilg)
		Else
			EmitValueTypeMethodCall(mi, ilg)
		End If

		If resultType.IsValueType = True And nextRequiresAddress = True Then
			EmitValueTypeLoadAddress(ilg, resultType)
		End If
	End Sub

	Protected Shared Function IsGetTypeMethod(ByVal mi As MethodInfo) As Boolean
		Dim miGetType As MethodInfo = GetType(Object).GetMethod("gettype", BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.IgnoreCase)
		Return mi.MethodHandle.Equals(miGetType.MethodHandle)
	End Function

	' Emit a function call for a value type
	Private Shared Sub EmitValueTypeMethodCall(ByVal mi As MethodInfo, ByVal ilg As FleeILGenerator)
		If mi.IsStatic = True Then
			ilg.Emit(OpCodes.Call, mi)
		ElseIf Not mi.DeclaringType Is mi.ReflectedType Then
			' Method is not defined on the value type

			If IsGetTypeMethod(mi) = True Then
				' Special GetType method which requires a box
				ilg.Emit(OpCodes.Box, mi.ReflectedType)
				ilg.Emit(OpCodes.Call, mi)
			Else
				' Equals, GetHashCode, and ToString methods on the base
				ilg.Emit(OpCodes.Constrained, mi.ReflectedType)
				ilg.Emit(OpCodes.Callvirt, mi)
			End If
		Else
			' Call value type's implementation
			ilg.Emit(OpCodes.Call, mi)
		End If
	End Sub

	Private Shared Sub EmitReferenceTypeMethodCall(ByVal mi As MethodInfo, ByVal ilg As FleeILGenerator)
		If mi.IsStatic = True Then
			ilg.Emit(OpCodes.Call, mi)
		Else
			ilg.Emit(OpCodes.Callvirt, mi)
		End If
	End Sub

	Protected Shared Sub EmitValueTypeLoadAddress(ByVal ilg As FleeILGenerator, ByVal targetType As Type)
		Dim index As Integer = ilg.GetTempLocalIndex(targetType)
		Utility.EmitStoreLocal(ilg, index)
		ilg.Emit(OpCodes.Ldloca_S, CByte(index))
	End Sub

	Protected Sub EmitLoadOwner(ByVal ilg As FleeILGenerator)
		ilg.Emit(OpCodes.Ldarg_0)

		Dim ownerType As Type = MyOptions.OwnerType

		If ownerType.IsValueType = False Then
			Return
		End If

		ilg.Emit(OpCodes.Unbox, ownerType)
		ilg.Emit(OpCodes.Ldobj, ownerType)

		' Emit usual stuff for value types but use the owner type as the target
		If Me.RequiresAddress = True Then
			EmitValueTypeLoadAddress(ilg, ownerType)
		End If
	End Sub

	' Determine if a field, property, or method is public
	Private Shared Function IsMemberPublic(ByVal member As MemberInfo) As Boolean
		Dim fi As FieldInfo = TryCast(member, FieldInfo)

		If Not fi Is Nothing Then
			Return fi.IsPublic
		End If

		Dim pi As PropertyInfo = TryCast(member, PropertyInfo)

		If Not pi Is Nothing Then
			Dim pmi As MethodInfo = pi.GetGetMethod(True)
			Return pmi.IsPublic
		End If

		Dim mi As MethodInfo = TryCast(member, MethodInfo)

		If Not mi Is Nothing Then
			Return mi.IsPublic
		End If

		Debug.Assert(False, "unknown member type")
		Return False
	End Function

	Protected Function GetAccessibleMembers(ByVal members As MemberInfo()) As MemberInfo()
		Dim accessible As New List(Of MemberInfo)

		' Keep all members that are accessible
		For Each mi As MemberInfo In members
			If Me.IsMemberAccessible(mi) = True Then
				accessible.Add(mi)
			End If
		Next

		Return accessible.ToArray()
	End Function

	Protected Shared Function IsOwnerMemberAccessible(ByVal member As MemberInfo, ByVal options As ExpressionOptions) As Boolean
		Dim accessAllowed As Boolean

		' Get the allowed access defined in the options
		If IsMemberPublic(member) = True Then
			accessAllowed = (options.OwnerMemberAccess And BindingFlags.Public) <> 0
		Else
			accessAllowed = (options.OwnerMemberAccess And BindingFlags.NonPublic) <> 0
		End If

		' See if the member has our access attribute defined
		Dim attr As ExpressionOwnerMemberAccessAttribute = Attribute.GetCustomAttribute(member, GetType(ExpressionOwnerMemberAccessAttribute))

		If attr Is Nothing Then
			' No, so return the access level
			Return accessAllowed
		Else
			' Member has our access attribute defined; use its access value instead
			Return attr.AllowAccess
		End If
	End Function

	Public Function IsMemberAccessible(ByVal member As MemberInfo) As Boolean
		If MyOptions.IsOwnerType(member.ReflectedType) = True Then
			Return IsOwnerMemberAccessible(member, MyOptions)
		Else
			Return IsMemberPublic(member)
		End If
	End Function

	Protected Function GetMembers(ByVal targets As MemberTypes) As MemberInfo()
		If MyPrevious Is Nothing Then
			' Do we have a namespace?
			If MyImport Is Nothing Then
				' Get all members in the default namespace
				Return Me.GetDefaultNamespaceMembers(MyName, targets)
			Else
				Return MyImport.FindMembers(MyName, targets)
			End If
		Else
			' We are not the first element; find all members with our name on the type of the previous member
			Return MyPrevious.TargetType.FindMembers(targets, BindFlags, MyOptions.MemberFilter, MyName)
		End If
	End Function

	' Find members in the default namespace
	Protected Function GetDefaultNamespaceMembers(ByVal name As String, ByVal memberType As MemberTypes) As MemberInfo()
		' Search the owner first
		Dim members As MemberInfo() = MyContext.Imports.FindOwnerMembers(name, memberType)

		' Keep only the accessible members
		members = Me.GetAccessibleMembers(members)

		' If we have some matches, return them
		If members.Length > 0 Then
			Return members
		End If

		' No matches on owner, so search imports
		Return MyContext.Imports.RootImport.FindMembers(name, memberType)
	End Function

	Protected Shared Function IsElementPublic(ByVal e As MemberElement) As Boolean
		Return e.IsPublic
	End Function

	Public ReadOnly Property MemberName() As String
		Get
			Return MyName
		End Get
	End Property

	Protected ReadOnly Property NextRequiresAddress() As Boolean
		Get
			If MyNext Is Nothing Then
				Return False
			Else
				Return MyNext.RequiresAddress
			End If
		End Get
	End Property

	Protected Overridable ReadOnly Property RequiresAddress() As Boolean
		Get
			Return False
		End Get
	End Property

	Protected Overridable ReadOnly Property SupportsInstance() As Boolean
		Get
			Return True
		End Get
	End Property

	Protected Overridable ReadOnly Property SupportsStatic() As Boolean
		Get
			Return False
		End Get
	End Property

	Public ReadOnly Property TargetType() As System.Type
		Get
			Return Me.ResultType
		End Get
	End Property
End Class