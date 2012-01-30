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

' Elements that represent constants in an expression

Imports System.Reflection.Emit

Friend Class Int64LiteralElement
	Inherits IntegralLiteralElement

	Private MyValue As Int64
	Private Const MinValue As String = "9223372036854775808"
	Private MyIsMinValue As Boolean

	Public Sub New(ByVal value As Int64)
		MyValue = value
	End Sub

	Private Sub New()
		MyIsMinValue = True
	End Sub

	Public Shared Function TryCreate(ByVal image As String, ByVal isHex As Boolean, ByVal negated As Boolean) As Int64LiteralElement
		If negated = True And image = MinValue Then
			Return New Int64LiteralElement()
		ElseIf isHex = True Then
			Dim value As Int64

			If Int64.TryParse(image, Globalization.NumberStyles.AllowHexSpecifier, Nothing, value) = False Then
				Return Nothing
			ElseIf value >= 0 And value <= Int64.MaxValue Then
				Return New Int64LiteralElement(value)
			Else
				Return Nothing
			End If
		Else
			Dim value As Int64

			If Int64.TryParse(image, value) = True Then
				Return New Int64LiteralElement(value)
			Else
				Return Nothing
			End If
		End If
	End Function

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		EmitLoad(MyValue, ilg)
	End Sub

	Public Sub Negate()
		If MyIsMinValue = True Then
			MyValue = Int64.MinValue
		Else
			MyValue = -MyValue
		End If
	End Sub

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return GetType(Int64)
		End Get
	End Property
End Class