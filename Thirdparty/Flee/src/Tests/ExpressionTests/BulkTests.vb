Imports NUnit.Framework
Imports ciloci.Flee

<TestFixture()> _
Public Class BulkTests
	Inherits ExpressionTests

	<Test(Description:="Expressions that should be valid")> _
	Public Sub TestValidExpressions()
		MyCurrentContext = MyGenericContext

		AddHandler MyCurrentContext.Variables.ResolveFunction, AddressOf TestValidExpressions_OnResolveFunction
		AddHandler MyCurrentContext.Variables.InvokeFunction, AddressOf TestValidExpressions_OnInvokeFunction

		Me.ProcessScriptTests("ValidExpressions.txt", AddressOf DoTestValidExpressions)

		RemoveHandler MyCurrentContext.Variables.ResolveFunction, AddressOf TestValidExpressions_OnResolveFunction
		RemoveHandler MyCurrentContext.Variables.InvokeFunction, AddressOf TestValidExpressions_OnInvokeFunction
	End Sub

	Private Sub TestValidExpressions_OnResolveFunction(ByVal sender As Object, ByVal e As ResolveFunctionEventArgs)
		e.ReturnType = GetType(Integer)
	End Sub

	Private Sub TestValidExpressions_OnInvokeFunction(ByVal sender As Object, ByVal e As InvokeFunctionEventArgs)
		e.Result = 100
	End Sub

	<Test(Description:="Expressions that should not be valid")> _
	Public Sub TestInvalidExpressions()
		Me.ProcessScriptTests("InvalidExpressions.txt", AddressOf DoTestInvalidExpressions)
	End Sub

	<Test(Description:="Casts that should be valid")> _
	Public Sub TestValidCasts()
		MyCurrentContext = MyValidCastsContext
		Me.ProcessScriptTests("ValidCasts.txt", AddressOf DoTestValidExpressions)
	End Sub

	<Test(Description:="Test our handling of checked expressions")> _
	Public Sub TestCheckedExpressions()
		Me.ProcessScriptTests("CheckedTests.txt", AddressOf DoTestCheckedExpressions)
	End Sub

	Private Sub DoTestValidExpressions(ByVal arr As String())
		Dim typeName As String = String.Concat("System.", arr(0))
		Dim expressionType As Type = Type.GetType(typeName, True, True)

		Dim context As ExpressionContext = MyCurrentContext
		context.Options.ResultType = expressionType

		Dim e As IDynamicExpression = Me.CreateDynamicExpression(arr(1), context)
		Me.DoTest(e, arr(2), expressionType, ExpressionTests.TestCulture)
	End Sub

	Private Sub DoTestInvalidExpressions(ByVal arr As String())
		Dim expressionType As Type = Type.GetType(arr(0), True, True)
		Dim reason As CompileExceptionReason = System.Enum.Parse(GetType(CompileExceptionReason), arr(2), True)

		Dim context As ExpressionContext = MyGenericContext
		Dim options As ExpressionOptions = context.Options
		options.ResultType = expressionType
		context.Imports.AddType(GetType(Math))
		options.OwnerMemberAccess = Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic

		Me.AssertCompileException(arr(1), context)
	End Sub

	Private Sub DoTestCheckedExpressions(ByVal arr As String())
		Dim expression As String = arr(0)
		Dim checked As Boolean = Boolean.Parse(arr(1))
		Dim shouldOverflow As Boolean = Boolean.Parse(arr(2))

		Dim context As New ExpressionContext(MyValidExpressionsOwner)
		Dim options As ExpressionOptions = context.Options
		context.Imports.AddType(GetType(Math))
		context.Imports.ImportBuiltinTypes()
		options.Checked = checked

		Try
			Dim e As IDynamicExpression = Me.CreateDynamicExpression(expression, context)
			e.Evaluate()
			Assert.IsFalse(shouldOverflow)
		Catch ex As OverflowException
			Assert.IsTrue(shouldOverflow)
		End Try
	End Sub
End Class