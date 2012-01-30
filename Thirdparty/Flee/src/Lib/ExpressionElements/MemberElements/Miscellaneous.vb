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

' Elements for field, property, array, and function access

Imports System.Reflection
Imports System.Reflection.Emit
Imports System.ComponentModel

Friend Class ExpressionMemberElement
	Inherits MemberElement

	Private MyElement As ExpressionElement

	Public Sub New(ByVal element As ExpressionElement)
		MyElement = element
	End Sub

	Protected Overrides Sub ResolveInternal()

	End Sub

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		MyBase.Emit(ilg, services)
		MyElement.Emit(ilg, services)
		If MyElement.ResultType.IsValueType = True Then
			EmitValueTypeLoadAddress(ilg, Me.ResultType)
		End If
	End Sub

	Protected Overrides ReadOnly Property SupportsInstance() As Boolean
		Get
			Return True
		End Get
	End Property

	Protected Overrides ReadOnly Property IsPublic() As Boolean
		Get
			Return True
		End Get
	End Property

	Public Overrides ReadOnly Property IsStatic() As Boolean
		Get
			Return False
		End Get
	End Property

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return MyElement.ResultType
		End Get
	End Property
End Class