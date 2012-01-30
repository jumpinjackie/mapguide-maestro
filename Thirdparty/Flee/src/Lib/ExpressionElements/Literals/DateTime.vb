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

Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Globalization

Friend Class DateTimeLiteralElement
	Inherits LiteralElement

	Private MyValue As DateTime

	Public Sub New(ByVal image As String, ByVal context As ExpressionContext)
		Dim options As ExpressionParserOptions = context.ParserOptions

		If DateTime.TryParseExact(image, options.DateTimeFormat, CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, MyValue) = False Then
			MyBase.ThrowCompileException(CompileErrorResourceKeys.CannotParseType, CompileExceptionReason.InvalidFormat, GetType(DateTime).Name)
		End If
	End Sub

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As System.IServiceProvider)
		Dim index As Integer = ilg.GetTempLocalIndex(GetType(DateTime))

		Utility.EmitLoadLocalAddress(ilg, index)

		LiteralElement.EmitLoad(MyValue.Ticks, ilg)

		Dim ci As ConstructorInfo = GetType(DateTime).GetConstructor(New Type() {GetType(Long)})

		ilg.Emit(OpCodes.Call, ci)

		Utility.EmitLoadLocal(ilg, index)
	End Sub

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return GetType(DateTime)
		End Get
	End Property
End Class