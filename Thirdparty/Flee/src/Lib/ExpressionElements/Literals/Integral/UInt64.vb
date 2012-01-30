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

Friend Class UInt64LiteralElement
	Inherits IntegralLiteralElement

	Private MyValue As UInt64

	Public Sub New(ByVal image As String, ByVal ns As System.Globalization.NumberStyles)
		Try
			MyValue = UInt64.Parse(image, ns)
		Catch ex As OverflowException
			MyBase.OnParseOverflow(image)
		End Try
	End Sub

	Public Sub New(ByVal value As UInt64)
		MyValue = value
	End Sub

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		EmitLoad(CLng(MyValue), ilg)
	End Sub

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return GetType(UInt64)
		End Get
	End Property
End Class