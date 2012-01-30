Imports NUnit.Framework
Imports Ciloci.Flee.CalcEngine

<TestFixture()> _
Public Class SimpleCalcEngineTests
	Inherits ExpressionTests

	Private MyEngine As SimpleCalcEngine

	Public Sub New()
		Dim engine As New SimpleCalcEngine()
		Dim context As New ExpressionContext()
		context.Imports.AddType(GetType(Math))
		context.Imports.AddType(GetType(Math), "math")

		engine.Context = context
		MyEngine = engine
	End Sub

	<Test()> _
	Public Sub TestScripts()
		Me.ProcessScriptTests("SimpleCalcEngineTests.txt", AddressOf SimpleCalcEngineTestsProcessor)
	End Sub

	Private Sub SimpleCalcEngineTestsProcessor(ByVal lineParts As String())
		Dim expressions As String() = lineParts(1).Split("|")

		MyEngine.Clear()

		Me.AddExpressions(expressions)

		Me.Evaluate(lineParts(0))
	End Sub

	Private Sub AddExpressions(ByVal expressions As String())
		For Each expression As String In expressions
			Dim arr As String() = expression.Split(":")

			Dim name As String = arr(0)

			Dim arr2 As String() = arr(1).Split("?")
			Dim text As String = arr2(0)

			If arr2.Length > 1 Then
				Me.AddVariables(arr2(1))
			End If

			MyEngine.AddDynamic(name, text)
		Next
	End Sub

	Private Sub Evaluate(ByVal data As String)
		Dim results As IDictionary(Of String, Object) = ParseQueryString(data)

		For Each entry As KeyValuePair(Of String, Object) In results
			Dim e As IDynamicExpression = MyEngine.Item(entry.Key)
			Dim expectedResult As Object = entry.Value
			Dim result As Object = e.Evaluate()
			Assert.AreEqual(expectedResult, result)
		Next
	End Sub

	Private Sub AddVariables(ByVal variablesText As String)
		Dim variables As IDictionary(Of String, Object) = ParseQueryString(variablesText)

		For Each entry As KeyValuePair(Of String, Object) In variables
			MyEngine.Context.Variables.Add(entry.Key, entry.Value)
		Next
	End Sub
End Class
