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

Friend Class UInt32LiteralElement
	Inherits IntegralLiteralElement

	Private MyValue As UInt32

	Public Sub New(ByVal value As UInt32)
		MyValue = value
	End Sub

	Public Shared Function TryCreate(ByVal image As String, ByVal ns As System.Globalization.NumberStyles) As UInt32LiteralElement
		Dim value As UInt32
		If UInt32.TryParse(image, ns, Nothing, value) = True Then
			Return New UInt32LiteralElement(value)
		Else
			Return Nothing
		End If
	End Function

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		EmitLoad(CInt(MyValue), ilg)
	End Sub

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return GetType(UInt32)
		End Get
	End Property
End Class