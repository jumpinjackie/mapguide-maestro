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

' Chain of member accesses
Friend Class InvocationListElement
	Inherits ExpressionElement

	Private MyTail As MemberElement

	Public Sub New(ByVal elements As IList, ByVal services As IServiceProvider)
		Me.HandleFirstElement(elements, services)
		LinkElements(elements)
		Resolve(elements, services)
		MyTail = elements.Item(elements.Count - 1)
	End Sub

	' Arrange elements as a linked list
	Private Shared Sub LinkElements(ByVal elements As IList)
		For i As Integer = 0 To elements.Count - 1
			Dim current As MemberElement = elements.Item(i)
			Dim nextElement As MemberElement = Nothing
			If i + 1 < elements.Count Then
				nextElement = elements.Item(i + 1)
			End If
			current.Link(nextElement)
		Next
	End Sub

	Private Sub HandleFirstElement(ByVal elements As IList, ByVal services As IServiceProvider)
		Dim first As ExpressionElement = elements.Item(0)

		' If the first element is not a member element, then we assume it is an expression and replace it with the correct member element
		If Not TypeOf (first) Is MemberElement Then
			Dim actualFirst As New ExpressionMemberElement(first)
			elements.Item(0) = actualFirst
		Else
			Me.ResolveNamespaces(elements, services)
		End If
	End Sub

	Private Sub ResolveNamespaces(ByVal elements As IList, ByVal services As IServiceProvider)
		Dim context As ExpressionContext = services.GetService(GetType(ExpressionContext))
		Dim currentImport As ImportBase = context.Imports.RootImport

		While True
			Dim name As String = GetName(elements)

			If name Is Nothing Then
				Exit While
			End If

			Dim import As ImportBase = currentImport.FindImport(name)

			If import Is Nothing Then
				Exit While
			End If

			currentImport = import
			elements.RemoveAt(0)

			If elements.Count > 0 Then
				Dim newFirst As MemberElement = DirectCast(elements.Item(0), MemberElement)
				newFirst.SetImport(currentImport)
			End If
		End While

		If elements.Count = 0 Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.NamespaceCannotBeUsedAsType, CompileExceptionReason.TypeMismatch, currentImport.Name)
		End If
	End Sub

	Private Shared Function GetName(ByVal elements As IList) As String
		If elements.Count = 0 Then
			Return Nothing
		End If

		' Is the first member a field/property element?
		Dim fpe As IdentifierElement = TryCast(elements.Item(0), IdentifierElement)

		If fpe Is Nothing Then
			Return Nothing
		Else
			Return fpe.MemberName
		End If
	End Function

	Private Shared Sub Resolve(ByVal elements As IList, ByVal services As IServiceProvider)
		For Each element As MemberElement In elements
			element.Resolve(services)
		Next
	End Sub

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		MyTail.Emit(ilg, services)
	End Sub

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return MyTail.ResultType
		End Get
	End Property
End Class