Imports System.Globalization.CultureInfo
Imports Ciloci.Flee.PerCederberg.Grammatica.Runtime

Friend MustInherit Class CustomTokenPattern
	Inherits TokenPattern

	Public Sub Initialize(ByVal id As Integer, ByVal name As String, ByVal type As PatternType, ByVal pattern As String, ByVal context As ExpressionContext)
		Me.ComputeToken(id, name, type, pattern, context)
	End Sub

	Protected MustOverride Sub ComputeToken(ByVal id As Integer, ByVal name As String, ByVal type As PatternType, ByVal pattern As String, ByVal context As ExpressionContext)
End Class

Friend Class RealPattern
	Inherits CustomTokenPattern

	Protected Overrides Sub ComputeToken(ByVal id As Integer, ByVal name As String, ByVal type As PerCederberg.Grammatica.Runtime.TokenPattern.PatternType, ByVal pattern As String, ByVal context As ExpressionContext)
		Dim options As ExpressionParserOptions = context.ParserOptions

		Dim digitsBeforePattern As Char = IIf(options.RequireDigitsBeforeDecimalPoint, "+"c, "*"c)

		pattern = String.Format(pattern, digitsBeforePattern, options.DecimalSeparator)

		Me.SetData(id, name, type, pattern)
	End Sub
End Class

Friend Class ArgumentSeparatorPattern
	Inherits CustomTokenPattern

	Protected Overrides Sub ComputeToken(ByVal id As Integer, ByVal name As String, ByVal type As PerCederberg.Grammatica.Runtime.TokenPattern.PatternType, ByVal pattern As String, ByVal context As ExpressionContext)
		Dim options As ExpressionParserOptions = context.ParserOptions
		Me.SetData(id, name, type, options.FunctionArgumentSeparator)
	End Sub
End Class