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

Friend Class SingleLiteralElement
	Inherits RealLiteralElement

	Private MyValue As Single

	Private Sub New()

	End Sub

	Public Sub New(ByVal value As Single)
		MyValue = value
	End Sub

	Public Shared Function Parse(ByVal image As String, ByVal services As IServiceProvider) As SingleLiteralElement
		Dim options As ExpressionParserOptions = services.GetService(GetType(ExpressionParserOptions))
		Dim element As New SingleLiteralElement

		Try
			Dim value As Single = options.ParseSingle(image)
			Return New SingleLiteralElement(value)
		Catch ex As OverflowException
			element.OnParseOverflow(image)
			Return Nothing
		End Try
	End Function

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		ilg.Emit(OpCodes.Ldc_R4, MyValue)
	End Sub

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return GetType(Single)
		End Get
	End Property
End Class