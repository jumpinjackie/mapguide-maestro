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

''' <include file='Resources/DocComments.xml' path='DocComments/ImportBase/Class/*' />
Public MustInherit Class ImportBase
	Implements IEnumerable(Of ImportBase)
	Implements IEquatable(Of ImportBase)

	Private MyContext As ExpressionContext

	Friend Sub New()

	End Sub

#Region "Methods - Non Public"
	Friend Overridable Sub SetContext(ByVal context As ExpressionContext)
		MyContext = context
		Me.Validate()
	End Sub

	Friend MustOverride Sub Validate()

	Protected MustOverride Sub AddMembers(ByVal memberName As String, ByVal memberType As MemberTypes, ByVal dest As ICollection(Of MemberInfo))
	Protected MustOverride Sub AddMembers(ByVal memberType As MemberTypes, ByVal dest As ICollection(Of MemberInfo))

	Friend Function Clone() As ImportBase
		Return Me.MemberwiseClone
	End Function

	Protected Shared Sub AddImportMembers(ByVal import As ImportBase, ByVal memberName As String, ByVal memberType As MemberTypes, ByVal dest As ICollection(Of MemberInfo))
		import.AddMembers(memberName, memberType, dest)
	End Sub

	Protected Shared Sub AddImportMembers(ByVal import As ImportBase, ByVal memberType As MemberTypes, ByVal dest As ICollection(Of MemberInfo))
		import.AddMembers(memberType, dest)
	End Sub

	Protected Shared Sub AddMemberRange(ByVal members As ICollection(Of MemberInfo), ByVal dest As ICollection(Of MemberInfo))
		For Each mi As MemberInfo In members
			dest.Add(mi)
		Next
	End Sub

	Protected Function AlwaysMemberFilter(ByVal member As MemberInfo, ByVal criteria As Object) As Boolean
		Return True
	End Function

	Friend MustOverride Function IsMatch(ByVal name As String) As Boolean
	Friend MustOverride Function FindType(ByVal typename As String) As Type

	Friend Overridable Function FindImport(ByVal name As String) As ImportBase
		Return Nothing
	End Function

	Friend Function FindMembers(ByVal memberName As String, ByVal memberType As MemberTypes) As MemberInfo()
		Dim found As New List(Of MemberInfo)()
		Me.AddMembers(memberName, memberType, found)
		Return found.ToArray()
	End Function
#End Region

#Region "Methods - Public"
	Public Function GetMembers(ByVal memberType As MemberTypes) As MemberInfo()
		Dim found As New List(Of MemberInfo)()
		Me.AddMembers(memberType, found)
		Return found.ToArray()
	End Function
#End Region

#Region "IEnumerable Implementation"
	Public Overridable Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of ImportBase) Implements System.Collections.Generic.IEnumerable(Of ImportBase).GetEnumerator
		Dim coll As New List(Of ImportBase)()
		Return coll.GetEnumerator()
	End Function

	Private Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
		Return Me.GetEnumerator()
	End Function
#End Region

#Region "IEquatable Implementation"
	Public Overloads Function Equals(ByVal other As ImportBase) As Boolean Implements System.IEquatable(Of ImportBase).Equals
		Return Me.EqualsInternal(other)
	End Function

	Protected MustOverride Function EqualsInternal(ByVal import As ImportBase) As Boolean
#End Region

#Region "Properties - Protected"
	Protected ReadOnly Property Context() As ExpressionContext
		Get
			Return MyContext
		End Get
	End Property
#End Region

#Region "Properties - Public"
	''' <include file='Resources/DocComments.xml' path='DocComments/ImportBase/Name/*' />
	Public MustOverride ReadOnly Property Name() As String

	''' <include file='Resources/DocComments.xml' path='DocComments/ImportBase/IsContainer/*' />
	Public Overridable ReadOnly Property IsContainer() As Boolean
		Get
			Return False
		End Get
	End Property
#End Region
End Class

''' <include file='Resources/DocComments.xml' path='DocComments/TypeImport/Class/*' />
Public NotInheritable Class TypeImport
	Inherits ImportBase

	Private MyType As Type
	Private MyBindFlags As BindingFlags
	Private MyUseTypeNameAsNamespace As Boolean

	''' <include file='Resources/DocComments.xml' path='DocComments/TypeImport/New1/*' />
	Public Sub New(ByVal importType As Type)
		Me.New(importType, False)
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/TypeImport/New2/*' />
	Public Sub New(ByVal importType As Type, ByVal useTypeNameAsNamespace As Boolean)
		Me.New(importType, BindingFlags.Public Or BindingFlags.Static, useTypeNameAsNamespace)
	End Sub

#Region "Methods - Non Public"
	Friend Sub New(ByVal t As Type, ByVal flags As BindingFlags, ByVal useTypeNameAsNamespace As Boolean)
		Utility.AssertNotNull(t, "t")
		MyType = t
		MyBindFlags = flags
		MyUseTypeNameAsNamespace = useTypeNameAsNamespace
	End Sub

	Friend Overrides Sub Validate()
		Me.Context.AssertTypeIsAccessible(MyType)
	End Sub

	Protected Overrides Sub AddMembers(ByVal memberName As String, ByVal memberType As MemberTypes, ByVal dest As ICollection(Of MemberInfo))
		Dim members As MemberInfo() = MyType.FindMembers(memberType, MyBindFlags, Me.Context.Options.MemberFilter, memberName)
		ImportBase.AddMemberRange(members, dest)
	End Sub

	Protected Overrides Sub AddMembers(ByVal memberType As MemberTypes, ByVal dest As ICollection(Of MemberInfo))
		If MyUseTypeNameAsNamespace = False Then
			Dim members As MemberInfo() = MyType.FindMembers(memberType, MyBindFlags, AddressOf Me.AlwaysMemberFilter, Nothing)
			ImportBase.AddMemberRange(members, dest)
		End If
	End Sub

	Friend Overrides Function IsMatch(ByVal name As String) As Boolean
		If MyUseTypeNameAsNamespace = True Then
			Return String.Equals(MyType.Name, name, Me.Context.Options.MemberStringComparison)
		Else
			Return False
		End If
	End Function

	Friend Overrides Function FindType(ByVal typeName As String) As Type
		If String.Equals(typeName, MyType.Name, Me.Context.Options.MemberStringComparison) = True Then
			Return MyType
		Else
			Return Nothing
		End If
	End Function

	Protected Overrides Function EqualsInternal(ByVal import As ImportBase) As Boolean
		Dim otherSameType As TypeImport = TryCast(import, TypeImport)
		Return Not otherSameType Is Nothing AndAlso MyType Is otherSameType.MyType
	End Function
#End Region

#Region "Methods - Public"
	Public Overrides Function GetEnumerator() As IEnumerator(Of ImportBase)
		If MyUseTypeNameAsNamespace = True Then
			Dim coll As New List(Of ImportBase)()
			coll.Add(New TypeImport(MyType, False))
			Return coll.GetEnumerator()
		Else
			Return MyBase.GetEnumerator()
		End If
	End Function
#End Region

#Region "Properties - Public"
	Public Overrides ReadOnly Property IsContainer() As Boolean
		Get
			Return MyUseTypeNameAsNamespace
		End Get
	End Property

	Public Overrides ReadOnly Property Name() As String
		Get
			Return MyType.Name
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/TypeImport/Target/*' />
	Public ReadOnly Property Target() As Type
		Get
			Return MyType
		End Get
	End Property
#End Region
End Class

''' <include file='Resources/DocComments.xml' path='DocComments/MethodImport/Class/*' />
Public NotInheritable Class MethodImport
	Inherits ImportBase

	Private MyMethod As MethodInfo

	''' <include file='Resources/DocComments.xml' path='DocComments/MethodImport/New/*' />
	Public Sub New(ByVal importMethod As MethodInfo)
		Utility.AssertNotNull(importMethod, "importMethod")
		MyMethod = importMethod
	End Sub

	Friend Overrides Sub Validate()
		Me.Context.AssertTypeIsAccessible(MyMethod.ReflectedType)
	End Sub

	Protected Overrides Sub AddMembers(ByVal memberName As String, ByVal memberType As MemberTypes, ByVal dest As ICollection(Of MemberInfo))
		If String.Equals(memberName, MyMethod.Name, Me.Context.Options.MemberStringComparison) = True AndAlso (memberType And MemberTypes.Method) <> 0 Then
			dest.Add(MyMethod)
		End If
	End Sub

	Protected Overrides Sub AddMembers(ByVal memberType As MemberTypes, ByVal dest As ICollection(Of MemberInfo))
		If (memberType And MemberTypes.Method) <> 0 Then
			dest.Add(MyMethod)
		End If
	End Sub

	Friend Overrides Function IsMatch(ByVal name As String) As Boolean
		Return String.Equals(MyMethod.Name, name, Me.Context.Options.MemberStringComparison)
	End Function

	Friend Overrides Function FindType(ByVal typeName As String) As Type
		Return Nothing
	End Function

	Protected Overrides Function EqualsInternal(ByVal import As ImportBase) As Boolean
		Dim otherSameType As MethodImport = TryCast(import, MethodImport)
		Return Not otherSameType Is Nothing AndAlso MyMethod.MethodHandle.Equals(otherSameType.MyMethod.MethodHandle)
	End Function

	Public Overrides ReadOnly Property Name() As String
		Get
			Return MyMethod.Name
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/MethodImport/Target/*' />
	Public ReadOnly Property Target() As MethodInfo
		Get
			Return MyMethod
		End Get
	End Property
End Class

''' <include file='Resources/DocComments.xml' path='DocComments/NamespaceImport/Class/*' />
Public NotInheritable Class NamespaceImport
	Inherits ImportBase
	Implements ICollection(Of ImportBase)

	Private MyNamespace As String
	Private MyImports As List(Of ImportBase)

	''' <include file='Resources/DocComments.xml' path='DocComments/NamespaceImport/New/*' />
	Public Sub New(ByVal importNamespace As String)
		Utility.AssertNotNull(importNamespace, "importNamespace")
		If importNamespace.Length = 0 Then
			Dim msg As String = Utility.GetGeneralErrorMessage(GeneralErrorResourceKeys.InvalidNamespaceName)
			Throw New ArgumentException(msg)
		End If

		MyNamespace = importNamespace
		MyImports = New List(Of ImportBase)()
	End Sub

	Friend Overrides Sub SetContext(ByVal context As ExpressionContext)
		MyBase.SetContext(context)

		For Each import As ImportBase In MyImports
			import.SetContext(context)
		Next
	End Sub

	Friend Overrides Sub Validate()

	End Sub

	Protected Overrides Sub AddMembers(ByVal memberName As String, ByVal memberType As MemberTypes, ByVal dest As ICollection(Of MemberInfo))
		For Each import As ImportBase In Me.NonContainerImports
			AddImportMembers(import, memberName, memberType, dest)
		Next
	End Sub

	Protected Overrides Sub AddMembers(ByVal memberType As MemberTypes, ByVal dest As ICollection(Of MemberInfo))

	End Sub

	Friend Overrides Function FindType(ByVal typeName As String) As Type
		For Each import As ImportBase In Me.NonContainerImports
			Dim t As Type = import.FindType(typeName)

			If Not t Is Nothing Then
				Return t
			End If
		Next

		Return Nothing
	End Function

	Friend Overrides Function FindImport(ByVal name As String) As ImportBase
		For Each import As ImportBase In MyImports
			If import.IsMatch(name) = True Then
				Return import
			End If
		Next
		Return Nothing
	End Function

	Friend Overrides Function IsMatch(ByVal name As String) As Boolean
		Return String.Equals(MyNamespace, name, Me.Context.Options.MemberStringComparison)
	End Function

	Private ReadOnly Property NonContainerImports() As ICollection(Of ImportBase)
		Get
			Dim found As New List(Of ImportBase)()

			For Each import As ImportBase In MyImports
				If import.IsContainer = False Then
					found.Add(import)
				End If
			Next

			Return found
		End Get
	End Property

	Protected Overrides Function EqualsInternal(ByVal import As ImportBase) As Boolean
		Dim otherSameType As NamespaceImport = TryCast(import, NamespaceImport)
		Return Not otherSameType Is Nothing AndAlso MyNamespace.Equals(otherSameType.MyNamespace, Me.Context.Options.MemberStringComparison)
	End Function

	Public Overrides ReadOnly Property IsContainer() As Boolean
		Get
			Return True
		End Get
	End Property

	Public Overrides ReadOnly Property Name() As String
		Get
			Return MyNamespace
		End Get
	End Property

#Region "ICollection implementation"
	Public Sub Add(ByVal item As ImportBase) Implements System.Collections.Generic.ICollection(Of ImportBase).Add
		Utility.AssertNotNull(item, "item")

		If Not Me.Context Is Nothing Then
			item.SetContext(Me.Context)
		End If

		MyImports.Add(item)
	End Sub

	Public Sub Clear() Implements System.Collections.Generic.ICollection(Of ImportBase).Clear
		MyImports.Clear()
	End Sub

	Public Function Contains(ByVal item As ImportBase) As Boolean Implements System.Collections.Generic.ICollection(Of ImportBase).Contains
		Return MyImports.Contains(item)
	End Function

	Public Sub CopyTo(ByVal array() As ImportBase, ByVal arrayIndex As Integer) Implements System.Collections.Generic.ICollection(Of ImportBase).CopyTo
		MyImports.CopyTo(array, arrayIndex)
	End Sub

	Public Function Remove(ByVal item As ImportBase) As Boolean Implements System.Collections.Generic.ICollection(Of ImportBase).Remove
		Return MyImports.Remove(item)
	End Function

	Public Overrides Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of ImportBase)
		Return MyImports.GetEnumerator()
	End Function

	Public ReadOnly Property Count() As Integer Implements System.Collections.Generic.ICollection(Of ImportBase).Count
		Get
			Return MyImports.Count
		End Get
	End Property

	Private ReadOnly Property IsReadOnly() As Boolean Implements System.Collections.Generic.ICollection(Of ImportBase).IsReadOnly
		Get
			Return False
		End Get
	End Property
#End Region
End Class