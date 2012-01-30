Imports NUnit.Framework
Imports ciloci.Flee
Imports System.Globalization
Imports System.Threading

<TestFixture()> _
Public Class IndividualTests
	Inherits ExpressionTests

	<Test(Description:="Test that we properly handle newline escapes in strings")> _
	Public Sub TestStringNewlineEscape()
		Dim e As IGenericExpression(Of String) = Me.CreateGenericExpression(Of String)("""a\r\nb""")
		Dim s As String = e.Evaluate()
		Dim expected As String = String.Format("a{0}b", ControlChars.CrLf)
		Assert.AreEqual(expected, s)
	End Sub

	<Test(Description:="Test that we can parse from multiple threads")> _
	Public Sub TestMultiTreadedParse()
		Dim t1 As New Thread(AddressOf ThreadRunParse)
		t1.Name = "Thread1"

		Dim t2 As New Thread(AddressOf ThreadRunParse)
		t2.Name = "Thread2"

		Dim context As New ExpressionContext()

		t1.Start(context)
		t2.Start(context)

		t1.Join()
		t2.Join()
	End Sub

	Private Sub ThreadRunParse(ByVal o As Object)
		Dim context As ExpressionContext = o
		' Test parse
		For i As Integer = 0 To 40 - 1
			Dim e As IDynamicExpression = Me.CreateDynamicExpression("1+1*200", context)
		Next
	End Sub

	<Test(Description:="Test that we can parse from multiple threads")> _
	Public Sub TestMultiTreadedEvaluate()
		Dim t1 As New System.Threading.Thread(AddressOf ThreadRunEvaluate)
		t1.Name = "Thread1"

		Dim t2 As New System.Threading.Thread(AddressOf ThreadRunEvaluate)
		t2.Name = "Thread2"

		Dim e As IDynamicExpression = Me.CreateDynamicExpression("1+1*200")

		t1.Start(e)
		t2.Start(e)

		t1.Join()
		t2.Join()
	End Sub

	Private Sub ThreadRunEvaluate(ByVal o As Object)
		' Test evaluate
		Dim e2 As IDynamicExpression = o

		For i As Integer = 0 To 40 - 1
			Dim result As Integer = DirectCast(e2.Evaluate(), Integer)
			Assert.AreEqual(1 + 1 * 200, result)
		Next
	End Sub

	<Test(Description:="Test evaluation of generic expressions")> _
	Public Sub TestGenericEvaluate()
		Dim context As ExpressionContext
		context = New ExpressionContext()

		context.Options.ResultType = GetType(Object)

		Dim e1 As IGenericExpression(Of Integer) = Me.CreateGenericExpression(Of Integer)("1000", context)
		Assert.AreEqual(1000, e1.Evaluate())

		Dim e2 As IGenericExpression(Of Double) = Me.CreateGenericExpression(Of Double)("1000.25", context)
		Assert.AreEqual(1000.25, e2.Evaluate())

		Dim e3 As IGenericExpression(Of Double) = Me.CreateGenericExpression(Of Double)("1000", context)
		Assert.AreEqual(1000.0, e3.Evaluate())

		Dim e4 As IGenericExpression(Of ValueType) = Me.CreateGenericExpression(Of ValueType)("1000", context)
		Dim vt As ValueType = e4.Evaluate()
		Assert.AreEqual(1000, vt)

		Dim e5 As IGenericExpression(Of Object) = Me.CreateGenericExpression(Of Object)("1000 + 2.5", context)
		Dim o As Object = e5.Evaluate()
		Assert.AreEqual(1000 + 2.5, o)
	End Sub

	<Test(Description:="Test expression imports")> _
	Public Sub TestImports()
		Dim e As IGenericExpression(Of Double)
		Dim context As ExpressionContext

		context = New ExpressionContext()
		' Import math type directly
		context.Imports.AddType(GetType(Math))

		' Should be able to see PI without qualification
		e = Me.CreateGenericExpression(Of Double)("pi", context)
		Assert.AreEqual(Math.PI, e.Evaluate())

		context = New ExpressionContext(MyValidExpressionsOwner)
		' Import math type with prefix
		context.Imports.AddType(GetType(Math), "math")

		' Should be able to see pi by qualifying with Math	
		e = Me.CreateGenericExpression(Of Double)("math.pi", context)
		Assert.AreEqual(Math.PI, e.Evaluate())

		' Import nothing
		context = New ExpressionContext()
		' Should not be able to see PI
		Me.AssertCompileException("pi")
		Me.AssertCompileException("math.pi")

		' Test importing of builtin types
		context = New ExpressionContext()
		context.Imports.ImportBuiltinTypes()

		Me.CreateGenericExpression(Of Double)("double.maxvalue", context)
		Me.CreateGenericExpression(Of String)("string.concat(""a"", ""b"")", context)
		Me.CreateGenericExpression(Of Long)("long.maxvalue * 2", context)
	End Sub

	<Test(Description:="Test importing of a method")> _
	Public Sub TestImportMethod()
		Dim context As New ExpressionContext()
		context.Imports.AddMethod("cos", GetType(Math), String.Empty)

		Dim e As IDynamicExpression = Me.CreateDynamicExpression("cos(100)", context)
		Assert.AreEqual(Math.Cos(100), DirectCast(e.Evaluate(), Double))

		Dim mi As System.Reflection.MethodInfo = GetType(Integer).GetMethod("parse", Reflection.BindingFlags.Static Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.IgnoreCase, Nothing, Reflection.CallingConventions.Any, New Type() {GetType(String)}, Nothing)
		context.Imports.AddMethod(mi, "")

		e = Me.CreateDynamicExpression("parse(""123"")", context)
		Assert.AreEqual(123, DirectCast(e.Evaluate(), Integer))
	End Sub

	<Test(Description:="Test that we can import multiple types into a namespace")> _
	Public Sub TestImportsNamespaces()
		Dim context As New ExpressionContext()
		context.Imports.AddType(GetType(Math), "ns1")
		context.Imports.AddType(GetType(String), "ns1")

		Dim e As IDynamicExpression = Me.CreateDynamicExpression("ns1.cos(100)", context)
		Assert.AreEqual(Math.Cos(100), DirectCast(e.Evaluate(), Double))

		e = Me.CreateDynamicExpression("ns1.concat(""a"", ""b"")", context)
		Assert.AreEqual(String.Concat("a", "b"), DirectCast(e.Evaluate(), String))
	End Sub

	<Test(Description:="Test our string equality")> _
	Public Sub TestStringEquality()
		Dim context As New ExpressionContext()
		Dim options As ExpressionOptions = context.Options

		Dim e As IGenericExpression(Of Boolean)

		' Should be equal
		e = Me.CreateGenericExpression(Of Boolean)("""abc"" = ""abc""", context)
		Assert.IsTrue(e.Evaluate())

		' Should not be equal
		e = Me.CreateGenericExpression(Of Boolean)("""ABC"" = ""abc""", context)
		Assert.IsFalse(e.Evaluate())

		' Should be not equal
		e = Me.CreateGenericExpression(Of Boolean)("""ABC"" <> ""abc""", context)
		Assert.IsTrue(e.Evaluate())

		' Change string compare type
		options.StringComparison = StringComparison.OrdinalIgnoreCase

		' Should be equal
		e = Me.CreateGenericExpression(Of Boolean)("""ABC"" = ""abc""", context)
		Assert.IsTrue(e.Evaluate())

		' Should also be equal
		e = Me.CreateGenericExpression(Of Boolean)("""ABC"" <> ""abc""", context)
		Assert.IsFalse(e.Evaluate())

		' Should also be not equal
		e = Me.CreateGenericExpression(Of Boolean)("""A"" <> ""z""", context)
		Assert.IsTrue(e.Evaluate())
	End Sub

	<Test(Description:="Test expression variables")> _
	Public Sub TestVariables()
		Me.TestValueTypeVariables()
		Me.TestReferenceTypeVariables()
	End Sub

	Private Sub TestValueTypeVariables()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		variables.Add("a", 100)
		variables.Add("b", -100)
		variables.Add("c", DateTime.Now)

		Dim e1 As IGenericExpression(Of Integer) = Me.CreateGenericExpression(Of Integer)("a+b", context)
		Dim result As Integer = e1.Evaluate()
		Assert.AreEqual(100 + -100, result)

		variables("B") = 1000
		result = e1.Evaluate()
		Assert.AreEqual(100 + 1000, result)

		Dim e2 As IGenericExpression(Of String) = Me.CreateGenericExpression(Of String)("c.tolongdatestring() + c.Year.tostring()", context)
		Assert.AreEqual(DateTime.Now.ToLongDateString() & DateTime.Now.Year, e2.Evaluate())

		' Test null value
		variables("a") = Nothing
		e1 = Me.CreateGenericExpression(Of Integer)("a", context)
		Assert.AreEqual(0, e1.Evaluate())
	End Sub

	Private Sub TestReferenceTypeVariables()
		Dim context As New ExpressionContext()
		Dim variables As VariableCollection = context.Variables

		variables.Add("a", "string")
		variables.Add("b", 100)

		Dim e As IGenericExpression(Of String) = Me.CreateGenericExpression(Of String)("a + b + a.tostring()", context)
		Dim result As String = e.Evaluate()
		Assert.AreEqual("string" & 100 & "string", result)

		variables("a") = "test"
		variables("b") = 1
		result = e.Evaluate()
		Assert.AreEqual("test" & 1 & "test", result)

		' Test null value
		variables("nullVar") = String.Empty
		variables("nullvar") = Nothing

		Dim e2 As IGenericExpression(Of Boolean) = Me.CreateGenericExpression(Of Boolean)("nullvar = null", context)
		Assert.IsTrue(e2.Evaluate())
	End Sub

	<Test(Description:="Test that we properly enforce member access on the expression owner")> _
	Public Sub TestMemberAccess()
		Dim owner As New AccessTestExpressionOwner()
		Dim context As New ExpressionContext(owner)
		Dim options As ExpressionOptions = context.Options
		Dim e As IDynamicExpression

		' Private field, access should be denied
		Me.AssertCompileException("privateField1", context)

		' Private field but with override allowing access
		e = Me.CreateDynamicExpression("privateField2", context)

		options.OwnerMemberAccess = Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic

		' Private field but with override denying access
		Me.AssertCompileException("privateField3", context)

		options.OwnerMemberAccess = Reflection.BindingFlags.Default

		' Public field, access should be denied
		Me.AssertCompileException("PublicField1", context)

		options.OwnerMemberAccess = Reflection.BindingFlags.Public
		' Public field, access should be allowed
		e = Me.CreateDynamicExpression("publicField1", context)
	End Sub

	<Test(Description:="Test parsing for different culture")> _
	Public Sub TestCultureSensitiveParse()
		Dim context As New ExpressionContext()
		context.Options.ParseCulture = CultureInfo.GetCultureInfo("fr-FR")
		context.Imports.AddType(GetType(String), "String")
		context.Imports.AddType(GetType(Math), "Math")

		Dim e As IGenericExpression(Of Double) = Me.CreateGenericExpression(Of Double)("1,25 + 0,75", context)
		Assert.AreEqual(1.25 + 0.75, e.Evaluate())

		e = Me.CreateGenericExpression(Of Double)("math.pow(1,25 + 0,75; 2)", context)
		Assert.AreEqual(Math.Pow(1.25 + 0.75, 2), e.Evaluate())

		Dim e2 As IGenericExpression(Of String) = Me.CreateGenericExpression(Of String)("string.concat(1;2;3;4)", context)
		Assert.AreEqual("1234", e2.Evaluate())
	End Sub

	<Test(Description:="Test tweaking of parser options")> _
	Public Sub TestParserOptions()
		Dim context As New ExpressionContext()
		context.Imports.AddType(GetType(String), "String")
		context.Imports.AddType(GetType(Math), "Math")

		context.ParserOptions.DecimalSeparator = ","
		context.ParserOptions.RecreateParser()
		Dim e As IGenericExpression(Of Double) = Me.CreateGenericExpression(Of Double)("1,25 + 0,75", context)
		Assert.AreEqual(1.25 + 0.75, e.Evaluate())

		context.ParserOptions.FunctionArgumentSeparator = ";"
		context.ParserOptions.RecreateParser()
		e = Me.CreateGenericExpression(Of Double)("math.pow(1,25 + 0,75; 2)", context)
		Assert.AreEqual(Math.Pow(1.25 + 0.75, 2), e.Evaluate())

		e = Me.CreateGenericExpression(Of Double)("math.max(,25;,75)", context)
		Assert.AreEqual(Math.Max(0.25, 0.75), e.Evaluate())

		context.ParserOptions.FunctionArgumentSeparator = ","
		context.ParserOptions.DecimalSeparator = ","
		context.ParserOptions.RequireDigitsBeforeDecimalPoint = True
		context.ParserOptions.RecreateParser()
		e = Me.CreateGenericExpression(Of Double)("math.max(1,25,0,75)", context)
		Assert.AreEqual(Math.Max(1.25, 0.75), e.Evaluate())

		context.ParserOptions.FunctionArgumentSeparator = ";"
		context.ParserOptions.RecreateParser()
		Dim e2 As IGenericExpression(Of String) = Me.CreateGenericExpression(Of String)("string.concat(1;2;3;4)", context)
		Assert.AreEqual("1234", e2.Evaluate())

		' Ambiguous grammar
		context.ParserOptions.FunctionArgumentSeparator = ","
		context.ParserOptions.DecimalSeparator = ","
		context.ParserOptions.RequireDigitsBeforeDecimalPoint = False
		context.ParserOptions.RecreateParser()
		Me.AssertCompileException("math.max(1,24,4,56)", context, CompileExceptionReason.SyntaxError)
	End Sub

	<Test(Description:="Test getting the variables used in an expression")> _
	Public Sub TestGetReferencedVariables()
		Dim context As New ExpressionContext(MyValidExpressionsOwner)
		context.Imports.AddType(GetType(Math))
		context.Options.OwnerMemberAccess = Reflection.BindingFlags.NonPublic

		context.Variables.Add("s1", "string")

		Dim e As IDynamicExpression = Me.CreateDynamicExpression("s1.length + stringa.length", context)
		Dim referencedVariables As String() = e.Info.GetReferencedVariables()

		Assert.AreEqual(2, referencedVariables.Length)
		Assert.AreNotEqual(-1, System.Array.IndexOf(Of String)(referencedVariables, "s1"))
		Assert.AreNotEqual(-1, System.Array.IndexOf(Of String)(referencedVariables, "stringa"))
	End Sub

	<Test(Description:="Test that we can handle long logical expressions and that we properly adjust for long branches")> _
	Public Sub TestLongBranchLogical1()
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

		Dim e As IDynamicExpression = Me.CreateDynamicExpression(expressionText, context)
		' We only care that the expression is valid and can be evaluated
		Dim result As Object = e.Evaluate()
	End Sub

	<Test(Description:="Test that we can handle long logical expressions and that we properly adjust for long branches")> _
	Public Sub TestLongBranchLogical2()
		Dim expressionText As String = Me.GetIndividualTest("LongBranch2")
		Dim context As New ExpressionContext

		Dim e As IGenericExpression(Of Boolean) = Me.CreateGenericExpression(Of Boolean)(expressionText, context)
		Assert.IsFalse(e.Evaluate())
	End Sub

	<Test(Description:="Test that we can work with base and derived owner classes")> _
	Public Sub TestDerivedOwner()
		Dim mb As System.Reflection.MethodBase = System.Reflection.MethodBase.GetCurrentMethod()
		Dim mi As System.Reflection.MethodInfo = mb

		Dim context As New ExpressionContext(mi)

		' Call a property on the base class
		Dim e As IGenericExpression(Of Boolean) = Me.CreateGenericExpression(Of Boolean)("IsPublic", context)

		Assert.AreEqual(mi.IsPublic, e.Evaluate())

		context = New ExpressionContext(mb)
		' Test that setting the owner to a derived class works
		e = Me.CreateGenericExpression(Of Boolean)("IsPublic", context)
		Assert.AreEqual(mb.IsPublic, e.Evaluate())

		' Pick a non-public method and set it as the new owner
		mi = GetType(Math).GetMethod("InternalTruncate", Reflection.BindingFlags.Static Or Reflection.BindingFlags.NonPublic)
		e.Owner = mi

		Assert.AreEqual(mi.IsPublic, e.Evaluate())
	End Sub

	<Test(Description:="Test we can handle an expression owner that is a value type")> _
	Public Sub TestValueTypeOwner()
		Dim owner As New TestStruct(100)
		Dim context As ExpressionContext = Me.CreateGenericContext(owner)

		Dim e As IGenericExpression(Of Integer) = Me.CreateGenericExpression(Of Integer)("myA", context)
		Dim result As Integer = e.Evaluate()
		Assert.AreEqual(100, result)

		e = Me.CreateGenericExpression(Of Integer)("mya.compareto(100)", context)
		result = e.Evaluate()
		Assert.AreEqual(0, result)

		e = Me.CreateGenericExpression(Of Integer)("DoStuff()", context)
		result = e.Evaluate()
		Assert.AreEqual(100, result)

		Dim dt As DateTime = DateTime.Now
		context = Me.CreateGenericContext(dt)

		e = Me.CreateGenericExpression(Of Integer)("Month", context)
		result = e.Evaluate()
		Assert.AreEqual(dt.Month, result)

		Dim e2 As IGenericExpression(Of String) = Me.CreateGenericExpression(Of String)("tolongdatestring()", context)
		Assert.AreEqual(dt.ToLongDateString(), e2.Evaluate())
	End Sub

	<Test(Description:="We should be able to import non-public types if they are in the same module as our owner")> _
	Public Sub TestNonPublicImports()
		Dim context As New ExpressionContext()

		Try
			' Should not be able to import non-public type
			' make sure type is not public
			Assert.IsFalse(GetType(TestImport).IsPublic)
			context.Imports.AddType(GetType(TestImport))
			Assert.Fail()
		Catch ex As ArgumentException

		End Try

		' ...until we set an owner that is in the same module
		context = New ExpressionContext(New OverloadTestExpressionOwner())
		context.Imports.AddType(GetType(TestImport))

		Dim e As IDynamicExpression = Me.CreateDynamicExpression("DoStuff()", context)
		Assert.AreEqual(100, DirectCast(e.Evaluate(), Integer))

		' Try the same test with an invidual method
		context = New ExpressionContext()

		Try
			context.Imports.AddMethod("Dostuff", GetType(TestImport), "")
			Assert.Fail()
		Catch ex As ArgumentException

		End Try

		context = New ExpressionContext(New OverloadTestExpressionOwner())
		context.Imports.AddMethod("Dostuff", GetType(TestImport), "")
		e = Me.CreateDynamicExpression("DoStuff()", context)
		Assert.AreEqual(100, DirectCast(e.Evaluate(), Integer))
	End Sub

	<Test(Description:="Test import with nested types")> _
	Public Sub TestNestedTypeImport()
		Dim context As New ExpressionContext()

		Try
			' Should not be able to import non-public nested type
			context.Imports.AddType(GetType(NestedA.NestedInternalB))
			Assert.Fail()
		Catch ex As ArgumentException

		End Try

		' Should be able to import public nested type
		context.Imports.AddType(GetType(NestedA.NestedPublicB))
		Dim e As IDynamicExpression = Me.CreateDynamicExpression("DoStuff()", context)
		Assert.AreEqual(100, DirectCast(e.Evaluate(), Integer))

		' Should be able to import nested internal type now
		context = New ExpressionContext(New OverloadTestExpressionOwner())
		context.Imports.AddType(GetType(NestedA.NestedInternalB))
		e = Me.CreateDynamicExpression("DoStuff()", context)
		Assert.AreEqual(100, DirectCast(e.Evaluate(), Integer))
	End Sub

	<Test(Description:="We should not allow access to the non-public members of a variable"), ExpectedException(GetType(ExpressionCompileException))> _
	Public Sub TestNonPublicVariableMemberAccess()
		Dim context As New ExpressionContext()
		context.Variables.Add("a", "abc")

		Me.CreateDynamicExpression("a.m_length", context)
	End Sub

	<Test(Description:="We should not compile an expression that accesses a non-public field with the same name as a variable")> _
	Public Sub TestFieldWithSameNameAsVariable()
		Dim context As New ExpressionContext(New Monitor())
		context.Variables.Add("doubleA", New ExpressionOwner())
		Me.AssertCompileException("doubleA.doubleA", context)

		' But it should work for a public member
		context = New ExpressionContext()
		Dim m As New Monitor()
		context.Variables.Add("I", m)

		Dim e As IDynamicExpression = Me.CreateDynamicExpression("i.i", context)
		Assert.AreEqual(m.I, DirectCast(e.Evaluate(), Integer))
	End Sub

	<Test(Description:="Test we can match members with names that differ only by case")> _
	Public Sub TestMemberCaseSensitivity()
		Dim owner As New FleeTest.CaseSensitiveOwner()
		Dim context As New ExpressionContext(owner)
		context.Options.CaseSensitive = True
		context.Options.OwnerMemberAccess = Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic

		context.Variables.Add("x", 300)
		context.Variables.Add("X", 400)

		Dim e As IDynamicExpression = Me.CreateDynamicExpression("a + A + x + X", context)
		Assert.AreEqual(100 + 200 + 300 + 400, DirectCast(e.Evaluate(), Integer))

		' Should fail since the function is called Cos
		Me.AssertCompileException("cos(1)", context, CompileExceptionReason.UndefinedName)
	End Sub

	<Test(Description:="Test handling of on-demand variables")> _
	Public Sub TestOnDemandVariables()
		Dim context As New ExpressionContext()
		AddHandler context.Variables.ResolveVariableType, AddressOf OnResolveVariableType
		AddHandler context.Variables.ResolveVariableValue, AddressOf OnResolveVariableValue

		Dim e As IDynamicExpression = Me.CreateDynamicExpression("a + b", context)
		Assert.AreEqual(100 + 100, DirectCast(e.Evaluate(), Integer))
	End Sub

	Private Sub OnResolveVariableType(ByVal sender As Object, ByVal e As ResolveVariableTypeEventArgs)
		e.VariableType = GetType(Integer)
	End Sub

	Private Sub OnResolveVariableValue(ByVal sender As Object, ByVal e As ResolveVariableValueEventArgs)
		e.VariableValue = 100
	End Sub

	<Test(Description:="Test on-demand functions")> _
	Public Sub TestOnDemandFunctions()
		Dim context As New ExpressionContext()
		AddHandler context.Variables.ResolveFunction, AddressOf OnResolveFunction
		AddHandler context.Variables.InvokeFunction, AddressOf OnInvokeFunction

		Dim e As IDynamicExpression = Me.CreateDynamicExpression("func1(100) * func2(0.25)", context)
		Assert.AreEqual(100 * 0.25, DirectCast(e.Evaluate(), Double))
	End Sub

	Private Sub OnResolveFunction(ByVal sender As Object, ByVal e As ResolveFunctionEventArgs)
		Select Case e.FunctionName
			Case "func1"
				e.ReturnType = GetType(Integer)
			Case "func2"
				e.ReturnType = GetType(Double)
		End Select
	End Sub

	Private Sub OnInvokeFunction(ByVal sender As Object, ByVal e As InvokeFunctionEventArgs)
		e.Result = e.Arguments(0)
	End Sub

	<Test(Description:="Test that we properly resolve method overloads")> _
	Public Sub TestOverloadResolution()
		Dim owner As New OverloadTestExpressionOwner()
		Dim context As New ExpressionContext(owner)

		' Test value types
		Me.DoTestOverloadResolution("valuetype1(100)", context, 1)
		Me.DoTestOverloadResolution("valuetype1(100.0f)", context, 2)
		Me.DoTestOverloadResolution("valuetype1(100.0)", context, 3)
		Me.DoTestOverloadResolution("valuetype2(100)", context, 1)

		' Test value type -> reference type
		Me.DoTestOverloadResolution("Value_ReferenceType1(100)", context, 1)
		Me.DoTestOverloadResolution("Value_ReferenceType2(100)", context, 1)
		'	with interfaces
		Me.DoTestOverloadResolution("Value_ReferenceType3(100)", context, 1)

		' Test reference types
		Me.DoTestOverloadResolution("ReferenceType1(""abc"")", context, 2)
		Me.DoTestOverloadResolution("ReferenceType1(b)", context, 1)
		Me.DoTestOverloadResolution("ReferenceType2(a)", context, 2)
		'	with interfaces
		Me.DoTestOverloadResolution("ReferenceType3(""abc"")", context, 2)

		' Test nulls
		Me.DoTestOverloadResolution("ReferenceType1(null)", context, 2)
		Me.DoTestOverloadResolution("ReferenceType2(null)", context, 2)

		' Test ambiguous match
		Me.DoTestOverloadResolution("valuetype3(100)", context, -1)
		Me.DoTestOverloadResolution("Value_ReferenceType4(100)", context, -1)
		Me.DoTestOverloadResolution("ReferenceType4(""abc"")", context, -1)
		Me.DoTestOverloadResolution("ReferenceType4(null)", context, -1)

		' Test access control
		Me.DoTestOverloadResolution("Access1(""abc"")", context, 1)
		Me.DoTestOverloadResolution("Access2(""abc"")", context, -1)
		Me.DoTestOverloadResolution("Access2(null)", context, -1)

		' Test with multiple arguments
		Me.DoTestOverloadResolution("Multiple1(1.0f, 2.0)", context, 1)
		Me.DoTestOverloadResolution("Multiple1(100, 2.0)", context, 2)
		Me.DoTestOverloadResolution("Multiple1(100, 2.0f)", context, 2)
	End Sub

	Private Sub DoTestOverloadResolution(ByVal expression As String, ByVal context As ExpressionContext, ByVal expectedResult As Integer)
		Try
			Dim e As IGenericExpression(Of Integer) = Me.CreateGenericExpression(Of Integer)(expression, context)
			Dim result As Integer = e.Evaluate()
			Assert.AreEqual(expectedResult, result)
		Catch ex As Exception
			Assert.AreEqual(-1, expectedResult)
		End Try
	End Sub

	<Test(description:="Test the NumbersAsDoubles option")> _
	Public Sub TestNumbersAsDoubles()
		Dim context As New ExpressionContext()
		context.Options.IntegersAsDoubles = True

		Dim e As IGenericExpression(Of Double) = Me.CreateGenericExpression(Of Double)("1 / 2", context)
		Assert.AreEqual(1 / 2.0, e.Evaluate())

		e = Me.CreateGenericExpression(Of Double)("4 * 4 / 10", context)
		Assert.AreEqual(4 * 4 / 10.0, e.Evaluate())

		context.Variables.Add("a", 1)

		e = Me.CreateGenericExpression(Of Double)("a / 10", context)
		Assert.AreEqual(1 / 10.0, e.Evaluate())

		' Should get a double back
		Dim e2 As IDynamicExpression = Me.CreateDynamicExpression("100", context)
		Assert.IsInstanceOfType(GetType(Double), e2.Evaluate())
	End Sub

	<Test(description:="Test variables that are expressions")> _
	Public Sub TestExpressionVariables()
		Dim context1 As New ExpressionContext()
		context1.Imports.AddType(GetType(Math))
		context1.Variables.Add("a", Math.PI)
		Dim exp1 As IDynamicExpression = Me.CreateDynamicExpression("sin(a)", context1)

		Dim context2 As New ExpressionContext()
		context2.Imports.AddType(GetType(Math))
		context2.Variables.Add("a", Math.PI)
		Dim exp2 As IGenericExpression(Of Double) = Me.CreateGenericExpression(Of Double)("cos(a/2)", context2)

		Dim context3 As New ExpressionContext()
		context3.Variables.Add("a", exp1)
		context3.Variables.Add("b", exp2)
		Dim exp3 As IDynamicExpression = Me.CreateDynamicExpression("a + b", context3)

		Dim a As Double = Math.Sin(Math.PI)
		Dim b As Double = Math.Cos(Math.PI / 2)

		Assert.AreEqual(a + b, exp3.Evaluate())

		Dim context4 As New ExpressionContext()
		context4.Variables.Add("a", exp1)
		context4.Variables.Add("b", exp2)
		Dim exp4 As IGenericExpression(Of Double) = Me.CreateGenericExpression(Of Double)("(a * b) + (b - a)", context4)

		Assert.AreEqual((a * b) + (b - a), exp4.Evaluate())
	End Sub

	<Test(description:="Test that no state is held in the original context between compiles")> _
	Public Sub TestNoStateHeldInContext()
		Dim context As New ExpressionContext()

		Dim e1 As IGenericExpression(Of Integer) = context.CompileGeneric(Of Integer)("300")

		' The result type of the cloned context should be set to integer
		Assert.IsTrue(e1.Context.Options.ResultType Is GetType(Int32))

		' The original context should not be affected
		Assert.IsNull(context.Options.ResultType)

		' This should compile
		Dim e2 As IDynamicExpression = context.CompileDynamic("""abc""")
		Assert.IsTrue(e2.Context.Options.ResultType Is GetType(String))

		' The original context should not be affected
		Assert.IsNull(context.Options.ResultType)
	End Sub

	<Test(description:="Test cloning an expression")> _
	Public Sub TestExpressionClone()
		Dim context As New ExpressionContext()
		context.Variables.Add("a", 100)
		context.Variables.Add("b", 200)
		Dim exp1 As IGenericExpression(Of Integer) = Me.CreateGenericExpression(Of Integer)("(a * b)", context)

		Dim exp2 As IGenericExpression(Of Integer) = exp1.Clone()

		Assert.AreNotSame(exp1.Context.Variables, exp2.Context.Variables)

		exp2.Context.Variables("a") = 10
		exp2.Context.Variables("b") = 20

		Assert.AreEqual(10 * 20, exp2.Evaluate())

		Dim t1 As New Thread(AddressOf ThreadRunClone)
		Dim t2 As New Thread(AddressOf ThreadRunClone)
		t1.Start(exp1)
		t2.Start(exp2)

		Dim exp3 As IDynamicExpression = Me.CreateDynamicExpression("a * b", context)
		Dim exp4 As IDynamicExpression = exp3.Clone()

		Assert.AreEqual(100 * 200, exp4.Evaluate())
	End Sub

	Private Sub ThreadRunClone(ByVal o As Object)
		Dim exp As IGenericExpression(Of Integer) = o

		Dim a As Integer = DirectCast(exp.Context.Variables("a"), Integer)
		Dim b As Integer = DirectCast(exp.Context.Variables("b"), Integer)

		For i As Integer = 0 To 10000 - 1
			Assert.AreEqual(a * b, exp.Evaluate())
		Next
	End Sub

	<Test(description:="Test the RealLiteralDataType option")> _
	Public Sub TestRealLiteralDataTypeOption()
		Dim context As New ExpressionContext()
		context.Options.RealLiteralDataType = RealLiteralDataType.Single

		Dim e As IDynamicExpression = Me.CreateDynamicExpression("100.25", context)

		Assert.IsInstanceOfType(GetType(Single), e.Evaluate())

		context.Options.RealLiteralDataType = RealLiteralDataType.Decimal
		e = Me.CreateDynamicExpression("100.25", context)

		Assert.IsInstanceOfType(GetType(Decimal), e.Evaluate())

		context.Options.RealLiteralDataType = RealLiteralDataType.Double
		e = Me.CreateDynamicExpression("100.25", context)

		Assert.IsInstanceOfType(GetType(Double), e.Evaluate())

		' Override should still work though
		e = Me.CreateDynamicExpression("100.25f", context)
		Assert.IsInstanceOfType(GetType(Single), e.Evaluate())

		e = Me.CreateDynamicExpression("100.25d", context)
		Assert.IsInstanceOfType(GetType(Double), e.Evaluate())

		e = Me.CreateDynamicExpression("100.25M", context)
		Assert.IsInstanceOfType(GetType(Decimal), e.Evaluate())
	End Sub
End Class
