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

' Element representing an array index
Friend Class IndexerElement
	Inherits MemberElement

	Private MyIndexerElement As ExpressionElement
	Private MyIndexerElements As ArgumentList

	Public Sub New(ByVal indexer As ArgumentList)
		MyIndexerElements = indexer
	End Sub

	Protected Overrides Sub ResolveInternal()
		' Are we are indexing on an array?
		Dim target As Type = MyPrevious.TargetType

		' Yes, so setup for an array index
		If target.IsArray = True Then
			Me.SetupArrayIndexer()
			Return
		End If

		' Not an array, so try to find an indexer on the type
		If Me.FindIndexer(target) = False Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.TypeNotArrayAndHasNoIndexerOfType, CompileExceptionReason.TypeMismatch, target.Name, MyIndexerElements)
		End If
	End Sub

	Private Sub SetupArrayIndexer()
		MyIndexerElement = MyIndexerElements.Item(0)

		If MyIndexerElements.Count > 1 Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.MultiArrayIndexNotSupported, CompileExceptionReason.TypeMismatch)
		ElseIf ImplicitConverter.EmitImplicitConvert(MyIndexerElement.ResultType, GetType(Int32), Nothing) = False Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.ArrayIndexersMustBeOfType, CompileExceptionReason.TypeMismatch, GetType(Int32).Name)
		End If
	End Sub

	Private Function FindIndexer(ByVal targetType As Type) As Boolean
		' Get the default members
		Dim members As MemberInfo() = targetType.GetDefaultMembers()

		Dim methods As New List(Of MethodInfo)

		' Use the first one that's valid for our indexer type
		For Each mi As MemberInfo In members
			Dim pi As PropertyInfo = TryCast(mi, PropertyInfo)
			If Not pi Is Nothing Then
				methods.Add(pi.GetGetMethod(True))
			End If
		Next

		dim func as New FunctionCallElement("Indexer", methods.ToArray(), MyIndexerElements)
		func.Resolve(MyServices)
		MyIndexerElement = func

		Return True
	End Function

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		MyBase.Emit(ilg, services)

		If Me.IsArray = True Then
			Me.EmitArrayLoad(ilg, services)
		Else
			Me.EmitIndexer(ilg, services)
		End If
	End Sub

	Private Sub EmitArrayLoad(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		MyIndexerElement.Emit(ilg, services)
		ImplicitConverter.EmitImplicitConvert(MyIndexerElement.ResultType, GetType(Int32), ilg)

		Dim elementType As Type = Me.ResultType

		If elementType.IsValueType = False Then
			' Simple reference load
			ilg.Emit(OpCodes.Ldelem_Ref)
		Else
			Me.EmitValueTypeArrayLoad(ilg, elementType)
		End If
	End Sub

	Private Sub EmitValueTypeArrayLoad(ByVal ilg As FleeILGenerator, ByVal elementType As Type)
		If Me.NextRequiresAddress = True Then
			ilg.Emit(OpCodes.Ldelema, elementType)
		Else
			Utility.EmitArrayLoad(ilg, elementType)
		End If
	End Sub

	Private Sub EmitIndexer(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim func As FunctionCallElement = MyIndexerElement
		func.EmitFunctionCall(Me.NextRequiresAddress, ilg, services)
	End Sub

	Private ReadOnly Property ArrayType() As Type
		Get
			If Me.IsArray = True Then
				Return MyPrevious.TargetType
			Else
				Return Nothing
			End If
		End Get
	End Property

	Private ReadOnly Property IsArray() As Boolean
		Get
			Return MyPrevious.TargetType.IsArray
		End Get
	End Property

	Protected Overrides ReadOnly Property RequiresAddress() As Boolean
		Get
			Return Me.IsArray = False
		End Get
	End Property

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			If Me.IsArray = True Then
				Return Me.ArrayType.GetElementType()
			Else
				Return MyIndexerElement.ResultType
			End If
		End Get
	End Property

	Protected Overrides ReadOnly Property IsPublic() As Boolean
		Get
			If Me.IsArray = True Then
				Return True
			Else
				Return MemberElement.IsElementPublic(MyIndexerElement)
			End If
		End Get
	End Property

	Public Overrides ReadOnly Property IsStatic() As Boolean
		Get
			Return False
		End Get
	End Property
End Class