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

' Imports of static members into an expression

Imports System.Reflection

''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionImports/Class/*' />	
Public NotInheritable Class ExpressionImports

	Private Shared OurBuiltinTypeMap As Dictionary(Of String, Type) = CreateBuiltinTypeMap()
	Private MyRootImport As NamespaceImport
	Private MyOwnerImport As TypeImport
	Private MyContext As ExpressionContext

	Friend Sub New()
		MyRootImport = New NamespaceImport(True)
	End Sub

	Private Shared Function CreateBuiltinTypeMap() As Dictionary(Of String, Type)
		Dim map As New Dictionary(Of String, Type)(StringComparer.OrdinalIgnoreCase)

		map.Add("boolean", GetType(Boolean))
		map.Add("byte", GetType(Byte))
		map.Add("sbyte", GetType(SByte))
		map.Add("short", GetType(Short))
		map.Add("ushort", GetType(UInt16))
		map.Add("int", GetType(Int32))
		map.Add("uint", GetType(UInt32))
		map.Add("long", GetType(Long))
		map.Add("ulong", GetType(ULong))
		map.Add("single", GetType(Single))
		map.Add("double", GetType(Double))
		map.Add("decimal", GetType(Decimal))
		map.Add("char", GetType(Char))
		map.Add("object", GetType(Object))
		map.Add("string", GetType(String))

		Return map
	End Function

#Region "Methods - Non public"
	Friend Sub SetContext(ByVal context As ExpressionContext)
		MyContext = context
		MyRootImport.SetContext(context)
	End Sub

	Friend Function Clone() As ExpressionImports
		Dim copy As New ExpressionImports()

		copy.MyRootImport = MyRootImport.Clone()
		copy.MyOwnerImport = MyOwnerImport

		Return copy
	End Function

	Friend Sub ImportOwner(ByVal ownerType As Type)
		MyOwnerImport = New TypeImport(ownerType, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Static, False)
		MyOwnerImport.SetContext(MyContext)
	End Sub

	Friend Function HasNamespace(ByVal ns As String) As Boolean
		Dim import As NamespaceImport = TryCast(MyRootImport.FindImport(ns), NamespaceImport)
		Return Not import Is Nothing
	End Function

	Friend Function GetImport(ByVal ns As String) As NamespaceImport
		If ns.Length = 0 Then
			Return MyRootImport
		End If

		Dim import As NamespaceImport = TryCast(MyRootImport.FindImport(ns), NamespaceImport)

		If import Is Nothing Then
			import = New NamespaceImport(ns)
			MyRootImport.Add(import)
		End If

		Return import
	End Function

	Friend Function FindOwnerMembers(ByVal memberName As String, ByVal memberType As System.Reflection.MemberTypes) As MemberInfo()
		Return MyOwnerImport.FindMembers(memberName, memberType)
	End Function

	Friend Function FindType(ByVal typeNameParts As String()) As Type
		Dim namespaces(typeNameParts.Length - 2) As String
		Dim typeName As String = typeNameParts(typeNameParts.Length - 1)

		System.Array.Copy(typeNameParts, namespaces, namespaces.Length)
		Dim currentImport As ImportBase = MyRootImport

		For Each ns As String In namespaces
			currentImport = currentImport.FindImport(ns)
			If currentImport Is Nothing Then
				Exit For
			End If
		Next

		If currentImport Is Nothing Then
			Return Nothing
		Else
			Return currentImport.FindType(typeName)
		End If
	End Function

	Friend Shared Function GetBuiltinType(ByVal name As String) As Type
		Dim t As Type = Nothing

		If OurBuiltinTypeMap.TryGetValue(name, t) = True Then
			Return t
		Else
			Return Nothing
		End If
	End Function
#End Region

#Region "Methods - Public"
	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionImports/AddType1/*' />	
	Public Sub AddType(ByVal t As Type, ByVal ns As String)
		Utility.AssertNotNull(t, "t")
		Utility.AssertNotNull(ns, "namespace")

		MyContext.AssertTypeIsAccessible(t)

		Dim import As NamespaceImport = Me.GetImport(ns)
		import.Add(New TypeImport(t, BindingFlags.Public Or BindingFlags.Static, False))
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionImports/Addtype2/*' />	
	Public Sub AddType(ByVal t As Type)
		Me.AddType(t, String.Empty)
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionImports/AddMethod1/*' />	
	Public Sub AddMethod(ByVal methodName As String, ByVal t As Type, ByVal ns As String)
		Utility.AssertNotNull(methodName, "methodName")
		Utility.AssertNotNull(t, "t")
		Utility.AssertNotNull(ns, "namespace")

		Dim mi As MethodInfo = t.GetMethod(methodName, BindingFlags.Public Or BindingFlags.Static Or BindingFlags.IgnoreCase)

		If mi Is Nothing Then
			Dim msg As String = Utility.GetGeneralErrorMessage(GeneralErrorResourceKeys.CouldNotFindPublicStaticMethodOnType, methodName, t.Name)
			Throw New ArgumentException(msg)
		End If

		Me.AddMethod(mi, ns)
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionImports/AddMethod2/*' />	
	Public Sub AddMethod(ByVal mi As MethodInfo, ByVal ns As String)
		Utility.AssertNotNull(mi, "mi")
		Utility.AssertNotNull(ns, "namespace")

		MyContext.AssertTypeIsAccessible(mi.ReflectedType)

		If mi.IsStatic = False Or mi.IsPublic = False Then
			Dim msg As String = Utility.GetGeneralErrorMessage(GeneralErrorResourceKeys.OnlyPublicStaticMethodsCanBeImported)
			Throw New ArgumentException(msg)
		End If

		Dim import As NamespaceImport = Me.GetImport(ns)
		import.Add(New MethodImport(mi))
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionImports/ImportBuiltinTypes/*' />	
	Public Sub ImportBuiltinTypes()
		For Each pair As KeyValuePair(Of String, Type) In OurBuiltinTypeMap
			Me.AddType(pair.Value, pair.Key)
		Next
	End Sub
#End Region

#Region "Properties - Public"
	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionImports/RootImport/*' />	
	Public ReadOnly Property RootImport() As NamespaceImport
		Get
			Return MyRootImport
		End Get
	End Property
#End Region
End Class