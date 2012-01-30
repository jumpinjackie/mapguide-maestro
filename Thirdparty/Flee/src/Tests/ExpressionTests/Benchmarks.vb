Imports NUnit.Framework
Imports Ciloci.Flee
Imports Ciloci.Flee.CalcEngine
Imports System.Globalization

' Tests and methods to evaluate speed

<TestFixture()> _
Public Class Benchmarks
	Inherits ExpressionTests

	<Test(description:="Test that setting variables is fast")> _
	Public Sub TestFastVariables()

		' Test should take 200ms or less
		Const EXPECTED_TIME As Integer = 200
		Const ITERATIONS As Integer = 100000

		Dim context As New ExpressionContext()
		Dim vars As VariableCollection = context.Variables
		vars.DefineVariable("a", GetType(Int32))
		vars.DefineVariable("b", GetType(Int32))
		Dim e As IDynamicExpression = Me.CreateDynamicExpression("a + b * (a ^ 2)", context)

		Dim sw As New Stopwatch()
		sw.Start()

		For i As Integer = 0 To ITERATIONS - 1
			Dim result As Object = e.Evaluate()
			vars.Item("a") = 200
			vars.Item("b") = 300
		Next

		sw.Stop()

		Me.PrintSpeedMessage("Fast variables", ITERATIONS, sw)
		Assert.Less(sw.ElapsedMilliseconds, EXPECTED_TIME, "Test time above expected value")
	End Sub

	<Test(description:="Test the speed of the simple calc engine")> _
	Public Sub TestSimpleCalcEngine()
		Const ITERATIONS As Integer = 2000

		Dim engine As New SimpleCalcEngine
		engine.Context.Imports.AddType(GetType(Math))

		Dim sw As New Stopwatch()

		' Test speed of populating the engine
		Dim prev As String = "1"
		Dim cur As String = Nothing

		sw.Start()

		' Create a chain of expressions each referring to the previous one
		For i As Integer = 0 To ITERATIONS - 1
			cur = String.Format("i_{0}", Guid.NewGuid.ToString("N"))
			Dim expression As String = String.Format("{0} + 1 + cos(3.14)", prev)

			engine.AddGeneric(Of Double)(cur, expression)

			prev = cur
		Next

		sw.Stop()

		Me.PrintSpeedMessage("Simple calc engine (population)", ITERATIONS, sw)

		' Evaluate the last expression (which will evaluate all the previous ones up the chain)
		Dim e As IGenericExpression(Of Double) = engine.Item(cur)

		sw.Reset()
		sw.Start()

		Dim result As Double = e.Evaluate()
		sw.Stop()

		Me.PrintSpeedMessage("Simple calc engine (evaluation)", ITERATIONS, sw)
	End Sub

	<Test(Description:="Test how fast we parse/compile an expression")> _
	Public Sub TestParseCompileSpeed()
		Dim expressionText As String = Me.GetIndividualTest("LongBranch1")
		Dim context As New ExpressionContext

		Dim vc As VariableCollection = context.Variables

		vc.Add("M0100_ASSMT_REASON", "0")
		vc.Add("M0220_PRIOR_NOCHG_14D", "1")
		vc.Add("M0220_PRIOR_UNKNOWN", "1")
		vc.Add("M0220_PRIOR_UR_INCON", "1")
		vc.Add("M0220_PRIOR_CATH", "1")
		vc.Add("M0220_PRIOR_INTRACT_PAIN", "1")
		vc.Add("M0220_PRIOR_IMPR_DECSN", "1")
		vc.Add("M0220_PRIOR_DISRUPTIVE", "1")
		vc.Add("M0220_PRIOR_MEM_LOSS", "1")
		vc.Add("M0220_PRIOR_NONE", "1")

		vc.Add("M0220_PRIOR_UR_INCON_bool", True)
		vc.Add("M0220_PRIOR_CATH_bool", True)
		vc.Add("M0220_PRIOR_INTRACT_PAIN_bool", True)
		vc.Add("M0220_PRIOR_IMPR_DECSN_bool", True)
		vc.Add("M0220_PRIOR_DISRUPTIVE_bool", True)
		vc.Add("M0220_PRIOR_MEM_LOSS_bool", True)
		vc.Add("M0220_PRIOR_NONE_bool", True)
		vc.Add("M0220_PRIOR_NOCHG_14D_bool", True)
		vc.Add("M0220_PRIOR_UNKNOWN_bool", True)

		Dim sw As New Stopwatch()

		' Do one compile to eliminate the cold start effect
		Dim e As IDynamicExpression = Me.CreateDynamicExpression(expressionText, context)

		sw.Start()
		e = Me.CreateDynamicExpression(expressionText, context)
		e = Me.CreateDynamicExpression(Me.GetIndividualTest("LongBranch2"))
		e = Me.CreateDynamicExpression("if(1 > 0, 1.0+1.0+1.0+1.0+1.0+1.0+1.0+1.0+1.0+1.0+1.0+1.0+1.0+1.0+1.0+1.0, 20.0)")
		sw.Stop()

		Const EXPECTED_TIME As Integer = 20

		Dim timePerExpression As Single = sw.ElapsedMilliseconds / 3.0

		Assert.Less(timePerExpression, EXPECTED_TIME)

		Me.WriteMessage("Parse/Compile speed = {0:n0}ms", timePerExpression)
	End Sub

	Private Sub PrintSpeedMessage(ByVal title As String, ByVal iterations As Integer, ByVal sw As Stopwatch)
		Me.WriteMessage("{0}: {1:n0} iterations in {2:n2}ms = {3:n2} iterations/sec", title, iterations, sw.ElapsedMilliseconds, iterations / (sw.ElapsedMilliseconds / 1000))
	End Sub
End Class
