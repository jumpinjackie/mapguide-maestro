Imports Ciloci.Flee
Imports Ciloci.Flee.CalcEngine
Imports System.Reflection
Imports System.Reflection.Emit

Module Module1

	Sub Main()
		Dim context As New ExpressionContext()
		context.Options.EmitToAssembly = True

		'context.ParserOptions.RequireDigitsBeforeDecimalPoint = True
		'context.ParserOptions.DecimalSeparator = ","c
		'context.ParserOptions.RecreateParser()
		'context.Options.ResultType = GetType(Decimal)

		context.Imports.AddType(GetType(Math))
		context.Variables.Add("a", 100D)
		context.Variables.Add("b", 3.14)

		Dim e As IDynamicExpression = context.CompileDynamic("a + a")
		'Dim e As IGenericExpression(Of Double) = context.CompileGeneric(Of Double)("a")
		Dim result As Object = e.Evaluate()
	End Sub

End Module