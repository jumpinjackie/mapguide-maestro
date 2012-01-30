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
Imports System.Reflection

Friend Class DecimalLiteralElement
	Inherits RealLiteralElement

	Private Shared OurConstructorInfo As ConstructorInfo = GetConstructor()

	Private MyValue As Decimal

	Private Sub New()

	End Sub

	Public Sub New(ByVal value As Decimal)
		MyValue = value
	End Sub

	Private Shared Function GetConstructor() As ConstructorInfo
		Dim types As Type() = {GetType(Int32), GetType(Int32), GetType(Int32), GetType(Boolean), GetType(Byte)}
		Return GetType(Decimal).GetConstructor(BindingFlags.Instance Or BindingFlags.Public, Nothing, CallingConventions.Any, types, Nothing)
	End Function

	Public Shared Function Parse(ByVal image As String, ByVal services As IServiceProvider) As DecimalLiteralElement
		Dim options As ExpressionParserOptions = services.GetService(GetType(ExpressionParserOptions))
		Dim element As New DecimalLiteralElement

		Try
			Dim value As Double = options.ParseDecimal(image)
			Return New DecimalLiteralElement(value)
		Catch ex As OverflowException
			element.OnParseOverflow(image)
			Return Nothing
		End Try
	End Function

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		Dim index As Integer = ilg.GetTempLocalIndex(GetType(Decimal))
		Utility.EmitLoadLocalAddress(ilg, index)

		Dim bits As Integer() = Decimal.GetBits(MyValue)
		EmitLoad(bits(0), ilg)
		EmitLoad(bits(1), ilg)
		EmitLoad(bits(2), ilg)

		Dim flags As Integer = bits(3)

		EmitLoad((flags >> 31) = -1, ilg)

		EmitLoad(flags >> 16, ilg)

		ilg.Emit(OpCodes.Call, OurConstructorInfo)

		Utility.EmitLoadLocal(ilg, index)
	End Sub

	Public Overrides ReadOnly Property ResultType() As System.Type
		Get
			Return GetType(Decimal)
		End Get
	End Property
End Class