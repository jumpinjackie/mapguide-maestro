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

Imports System.Reflection.Emit

Friend Class Int32LiteralElement
	Inherits IntegralLiteralElement

	Private MyValue As Int32
	Private Const MinValue As String = "2147483648"
	Private MyIsMinValue As Boolean

	Public Sub New(ByVal value As Int32)
		MyValue = value
	End Sub

	Private Sub New()
		MyIsMinValue = True
	End Sub

	Public Shared Function TryCreate(ByVal image As String, ByVal isHex As Boolean, ByVal negated As Boolean) As Int32LiteralElement
		If negated = True And image = MinValue Then
			Return New Int32LiteralElement()
		ElseIf isHex = True Then
			Dim value As Int32

			' Since Int32.TryParse will succeed for a string like 0xFFFFFFFF we have to do some special handling
			If Int32.TryParse(image, Globalization.NumberStyles.AllowHexSpecifier, Nothing, value) = False Then
				Return Nothing
			ElseIf value >= 0 And value <= Int32.MaxValue Then
				Return New Int32LiteralElement(value)
			Else
				Return Nothing
			End If
		Else
			Dim value As Int32

			If Int32.TryParse(image, value) = True Then
				Return New Int32LiteralElement(value)
			Else
				Return Nothing
			End If
		End If
	End Function

	Public Sub Negate()
		If MyIsMinValue = True Then
			MyValue = Int32.MinValue
		Else
			MyValue = -MyValue
		End If
	End Sub

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		EmitLoad(MyValue, ilg)
	End Sub

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return GetType(Int32)
		End Get
	End Property

	Public ReadOnly Property Value() As Integer
		Get
			Return MyValue
		End Get
	End Property
End Class