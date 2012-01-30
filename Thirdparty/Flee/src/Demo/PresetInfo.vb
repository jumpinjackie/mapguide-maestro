Imports System.Globalization

Friend Class PresetInfo

	Private MyName As String
	Public MyRed, MyGreen, MyBlue As String

	Public Sub New(ByVal name As String, ByVal red As String, ByVal green As String, ByVal blue As String)
		MyName = name

		Dim ci As CultureInfo = CultureInfo.CurrentCulture
		Dim decimalSeparator As String = ci.NumberFormat.NumberDecimalSeparator

		MyRed = Me.AdjustExpressionForCulture(red, decimalSeparator)
		MyGreen = Me.AdjustExpressionForCulture(green, decimalSeparator)
		MyBlue = Me.AdjustExpressionForCulture(blue, decimalSeparator)
	End Sub

	Private Function AdjustExpressionForCulture(ByVal s As String, ByVal decimalSeparator As String) As String
		Return String.Format(s, decimalSeparator)
	End Function

	Public Overrides Function ToString() As String
		Return MyName
	End Function
End Class
