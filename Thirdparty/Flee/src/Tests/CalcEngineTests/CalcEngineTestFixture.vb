Imports NUnit.Framework
Imports Ciloci.Flee.CalcEngine

<TestFixture()> _
Public Class CalcEngineTestFixture
	Inherits ExpressionTests

	<Test()> _
	Public Sub TestBasic()
		Dim ce As New CalculationEngine()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		variables.Add("x", 100)
		ce.Add("a", "x * 2", context)

		variables.Add("y", 1)
		ce.Add("b", "a + y", context)

		ce.Add("c", "b * 2", context)

		ce.Recalculate("a")
		Dim result As Integer = ce.GetResult(Of Integer)("c")
		Assert.AreEqual(result, ((100 * 2) + 1) * 2)

		variables("x") = 345
		ce.Recalculate("a")
		result = ce.GetResult(Of Integer)("c")
		Assert.AreEqual(((345 * 2) + 1) * 2, result)
	End Sub

	Private Function CreateExpression(ByVal expression As String, ByVal context As ExpressionContext) As IDynamicExpression
		Return context.CompileDynamic(expression)
	End Function

	<Test()> _
	Public Sub TestMutipleIdenticalReferences()
		Dim ce As New CalculationEngine()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		variables.Add("x", 100)
		ce.Add("a", "x * 2", context)

		ce.Add("b", "a + a + a", context)

		ce.Recalculate("a")
		Dim result As Integer = ce.GetResult(Of Integer)("b")
		Assert.AreEqual((100 * 2) * 3, result)
	End Sub

	<Test()> _
	Public Sub TestComplex()
		Dim ce As New CalculationEngine()

		Dim context As New ExpressionContext()

		Dim variables As VariableCollection = context.Variables

		variables.Add("x", 100)
		ce.Add("a", "x * 2", context)

		variables.Add("y", 24)
		ce.Add("b", "y * 2", context)

		ce.Add("c", "a + b", context)

		ce.Add("d", "80", context)

		ce.Add("e", "a + b + c + d", context)

		ce.Recalculate("d")
		ce.Recalculate(New String() {"a", "b"})

		Dim result As Integer = ce.GetResult(Of Integer)("e")
		Assert.AreEqual((100 * 2) + (24 * 2) + ((100 * 2) + (24 * 2)) + 80, result)
	End Sub

	<Test()> _
	Public Sub TestRecalculateNonSource()
		' Test recalculate with a non-source
		Dim ce As New CalculationEngine()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		variables.Add("x", 100)
		ce.Add("a", "x * 2", context)

		variables.Add("y", 1)
		ce.Add("b", "a + y", context)

		ce.Add("c", "b * 2", context)

		ce.Recalculate("a", "b")
		Dim result As Integer = ce.GetResult(Of Integer)("c")
		Assert.AreEqual(((100) * 2 + 1) * 2, result)
	End Sub

	<Test()> _
	Public Sub TestPartialRecalculate()
		Dim ce As New CalculationEngine()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		variables.Add("x", 100)
		ce.Add("a", "x * 2", context)

		variables.Add("y", 1)
		ce.Add("b", "a + y", context)

		ce.Add("c", "b * 2", context)

		ce.Recalculate("a")

		variables("y") = 222
		ce.Recalculate("b")

		Dim result As Integer = ce.GetResult(Of Integer)("c")
		Assert.AreEqual(((100 * 2) + 222) * 2, result)
	End Sub

	<Test(), ExpectedException(GetType(CircularReferenceException))> _
	Public Sub TestCircularReference1()
		Dim ce As New CalculationEngine()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		variables.Add("x", 100)
		ce.Add("a", "x * 2", context)

		variables.Add("y", 1)
		ce.Add("b", "a + y + b", context)

		ce.Recalculate("a")
	End Sub

	<Test(), ExpectedException(GetType(CircularReferenceException))> _
	Public Sub TestCircularReference2()
		Dim ce As New CalculationEngine()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		variables.Add("x", 100)
		ce.Add("a", "x * 2", context)

		variables.Add("y", 1)
		ce.Add("b", "a + y + b", context)

		ce.Recalculate("b")
	End Sub

	<Test()> _
	Public Sub TestWithReferenceTypes()
		Dim ce As New CalculationEngine()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		variables.Add("x", "string")
		ce.Add("a", "x + "" """, context)

		variables.Add("y", "word")
		ce.Add("b", "y", context)

		ce.Add("c", "a + b", context)

		ce.Recalculate("b", "a")

		Dim result As String = ce.GetResult(Of String)("c")
		Assert.AreEqual("string" & " " & "word", result)
	End Sub

	<Test()> _
	Public Sub TestRemove1()
		Dim ce As New CalculationEngine()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		ce.Add("a", "100", context)
		ce.Add("b", "200", context)
		ce.Add("c", "a + b", context)
		ce.Add("d", "300", context)
		ce.Add("e", "c + d", context)

		ce.Remove("a")
		' Only b and d should remain
		Assert.AreEqual(2, ce.Count)

		ce.Remove("b")
		Assert.AreEqual(1, ce.Count)

		ce.Remove("d")
		Assert.AreEqual(0, ce.Count)

		' b and d should have no dependents or precedents
		Assert.IsFalse(ce.HasDependents("b"))
		Assert.IsFalse(ce.HasDependents("d"))
		Assert.IsFalse(ce.HasPrecedents("b"))
		Assert.IsFalse(ce.HasPrecedents("d"))
	End Sub

	<Test()> _
	Public Sub TestRemove2()
		Dim ce As New CalculationEngine()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		ce.Add("a", "100", context)
		ce.Add("b", "200", context)
		ce.Add("c", "a + b", context)
		ce.Add("d", "300", context)
		ce.Add("e", "c + d + a", context)

		ce.Remove("a")
		' Only b and d should remain
		Assert.AreEqual(2, ce.Count)
		ce.Remove("b")
		Assert.AreEqual(1, ce.Count)
		ce.Remove("d")
		Assert.AreEqual(0, ce.Count)
	End Sub

	<Test()> _
	Public Sub TestRemove3()
		Dim ce As New CalculationEngine()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		ce.Add("a", "100", context)
		ce.Add("b", "200", context)
		ce.Add("c", "a + b", context)
		ce.Add("d", "300 + c", context)
		ce.Add("e", "c + d", context)

		ce.Remove("d")
		Assert.AreEqual(3, ce.Count)

		ce.Recalculate("c")
		ce.Remove("c")
		Assert.AreEqual(2, ce.Count)

		ce.Remove("a")
		Assert.AreEqual(1, ce.Count)

		ce.Remove("b")
		Assert.AreEqual(0, ce.Count)
	End Sub

	<Test()> _
	Public Sub TestRemove4()
		Dim ce As New CalculationEngine()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		ce.Add("a", "100", context)
		ce.Add("b", "200", context)
		ce.Add("c", "a + b", context)
		ce.Add("d", "300 + c", context)
		ce.Add("e", "c + d", context)

		ce.Remove("a")
		Assert.AreEqual(1, ce.Count)

		ce.Remove("b")
		Assert.AreEqual(0, ce.Count)
	End Sub

	<Test()> _
	Public Sub TestBatchLoad()
		' Test that we can add expressions in any order
		Dim engine As New CalculationEngine()
		Dim context As New ExpressionContext()

		Dim interest As Integer = 2
		context.Variables.Add("interest", interest)

		Dim loader As BatchLoader = engine.CreateBatchLoader()

		loader.Add("c", "a + b", context)
		loader.Add("a", "100 + interest", context)
		loader.Add("b", "a + 1 + a", context)
		' Test an expression with a reference in a string
		loader.Add("d", """str \"" str"" + a + ""b""", context)

		engine.BatchLoad(loader)

		Dim result As Integer = engine.GetResult(Of Integer)("b")
		Assert.AreEqual((100 + interest) + 1 + (100 + interest), result)

		interest = 300
		context.Variables("interest") = interest
		engine.Recalculate("a")

		result = engine.GetResult(Of Integer)("b")
		Assert.AreEqual((100 + interest) + 1 + (100 + interest), result)

		result = engine.GetResult(Of Integer)("c")
		Assert.AreEqual((100 + interest) + 1 + (100 + interest) + (100 + interest), result)

		Assert.AreEqual("str "" str400b", engine.GetResult(Of String)("d"))
	End Sub

	<Test()> _
	Public Sub TestCalcEngineAtom()
		' Test that calc engine atom reference work properly
		Dim engine As New CalculationEngine()
		Dim context As New ExpressionContext()

		engine.Add("a", """abc""", context)
		engine.Add("b", "a.length", context)
		engine.Add("c", "a.startswith(""a"")", context)

		Dim result As Integer = engine.GetResult(Of Integer)("b")
		Assert.AreEqual("abc".Length, result)

		Assert.AreEqual(True, engine.GetResult(Of Boolean)("c"))
	End Sub

	<Test()> _
	Public Sub TestDependencyManagement()
		' Test that we are keeping accurate stats on our dependencies

		Dim engine As New CalculationEngine()
		Dim context As New ExpressionContext()

		engine.Add("a", "100", context)
		engine.Add("b", "100", context)

		' Nothing should point to a and b
		Assert.IsFalse(engine.HasPrecedents("a"))
		Assert.IsFalse(engine.HasPrecedents("b"))
		Assert.IsFalse(engine.HasDependents("a"))
		Assert.IsFalse(engine.HasDependents("b"))

		engine.Add("c", "a + b", context)
		engine.Add("d", "a + c", context)

		' a and b still have nothing pointing to them
		Assert.IsFalse(engine.HasPrecedents("a"))
		Assert.IsFalse(engine.HasPrecedents("b"))
		' but they have dependents
		Assert.IsTrue(engine.HasDependents("a"))
		Assert.IsTrue(engine.HasDependents("b"))

		' c and d have precedents
		Assert.IsTrue(engine.HasPrecedents("d"))
		Assert.IsTrue(engine.HasPrecedents("c"))
		' and only c should have dependents
		Assert.IsTrue(engine.HasDependents("c"))
		Assert.IsFalse(engine.HasDependents("d"))

		' test our counts
		Assert.AreEqual(2, engine.GetDependents("a").Length)
		Assert.AreEqual(1, engine.GetDependents("b").Length)
		Assert.AreEqual(1, engine.GetDependents("c").Length)
		Assert.AreEqual(0, engine.GetDependents("d").Length)

		Assert.AreEqual(0, engine.GetPrecedents("a").Length)
		Assert.AreEqual(0, engine.GetPrecedents("b").Length)
		Assert.AreEqual(2, engine.GetPrecedents("c").Length)
		Assert.AreEqual(2, engine.GetPrecedents("d").Length)

		engine.Remove("d")

		Assert.IsFalse(engine.HasDependents("c"))
		Assert.IsFalse(engine.HasDependents("d"))
		Assert.IsFalse(engine.HasPrecedents("d"))
		Assert.IsTrue(engine.HasPrecedents("c"))

		Assert.AreEqual(1, engine.GetDependents("a").Length)
		Assert.AreEqual(1, engine.GetDependents("b").Length)
		Assert.AreEqual(0, engine.GetDependents("c").Length)

		engine.Remove("c")

		Assert.IsFalse(engine.HasPrecedents("c"))
		Assert.IsFalse(engine.HasDependents("c"))
		Assert.IsFalse(engine.HasDependents("a"))
		Assert.IsFalse(engine.HasDependents("b"))

		Assert.AreEqual(0, engine.GetDependents("a").Length)
		Assert.AreEqual(0, engine.GetDependents("b").Length)

		engine.Remove("a")
		engine.Remove("b")

		Assert.IsFalse(engine.HasDependents("a"))
		Assert.IsFalse(engine.HasPrecedents("a"))
		Assert.IsFalse(engine.HasDependents("b"))
		Assert.IsFalse(engine.HasPrecedents("b"))

		Assert.AreEqual(0, engine.GetDependents("a").Length)
		Assert.AreEqual(0, engine.GetDependents("b").Length)
		Assert.AreEqual(0, engine.GetPrecedents("a").Length)
		Assert.AreEqual(0, engine.GetPrecedents("b").Length)
	End Sub

	<Test()> _
	Public Sub TestInfoMethodsWithMissing()
		' Test that our informational methods can be called with a non-existant expression
		Dim engine As New CalculationEngine()

		Assert.IsFalse(engine.HasDependents("zz"))
		Assert.IsFalse(engine.HasPrecedents("zz"))
		Assert.AreEqual(0, engine.GetDependents("zz").Length)
		Assert.AreEqual(0, engine.GetPrecedents("zz").Length)
	End Sub

	<Test()> _
	Public Sub TestClear()
		Dim engine As New CalculationEngine()
		Dim context As New ExpressionContext()

		engine.Add("a", "100", context)
		engine.Add("b", "a + 2", context)

		engine.Clear()

		Assert.IsFalse(engine.HasDependents("a"))
		Assert.IsFalse(engine.HasPrecedents("b"))
		Assert.AreEqual(0, engine.Count)
	End Sub
End Class