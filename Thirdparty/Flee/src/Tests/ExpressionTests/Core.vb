Imports NUnit.Framework
Imports ciloci.Flee
Imports System.ComponentModel
Imports System.Globalization
Imports System.Reflection
Imports System.IO
Imports System.Xml.XPath

Public MustInherit Class ExpressionTests

	Private Const COMMENT_CHAR As Char = "'"c
	Private Const SEPARATOR_CHAR As Char = ";"c

	Protected Delegate Sub LineProcessor(ByVal lineParts As String())

	Protected MyValidExpressionsOwner As New ExpressionOwner
	Protected MyGenericContext As ExpressionContext
	Protected MyValidCastsContext As ExpressionContext
	Protected MyCurrentContext As ExpressionContext

	Protected Shared ReadOnly TestCulture As CultureInfo = CultureInfo.GetCultureInfo("en-CA")

#Region "Initialization"
	Public Sub New()
		MyValidExpressionsOwner = New ExpressionOwner()

		MyGenericContext = Me.CreateGenericContext(MyValidExpressionsOwner)

		Dim context As New ExpressionContext(MyValidExpressionsOwner)
		context.Options.OwnerMemberAccess = Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic
		context.Imports.ImportBuiltinTypes()
		context.Imports.AddType(GetType(Convert), "Convert")
		context.Imports.AddType(GetType(Guid))
		context.Imports.AddType(GetType(Version))
		context.Imports.AddType(GetType(DayOfWeek))
		context.Imports.AddType(GetType(DayOfWeek), "DayOfWeek")
		context.Imports.AddType(GetType(ValueType))
		context.Imports.AddType(GetType(icomparable))
		context.Imports.AddType(GetType(ICloneable))
		context.Imports.AddType(GetType(Array))
		context.Imports.AddType(GetType(System.Delegate))
		context.Imports.AddType(GetType(AppDomainInitializer))
		context.Imports.AddType(GetType(System.Text.Encoding))
		context.Imports.AddType(GetType(System.Text.ASCIIEncoding))
		context.Imports.AddType(GetType(ArgumentException))

		MyValidCastsContext = context

		' For testing virtual properties
		TypeDescriptor.AddProvider(New UselessTypeDescriptionProvider(TypeDescriptor.GetProvider(GetType(Integer))), GetType(Integer))
		TypeDescriptor.AddProvider(New UselessTypeDescriptionProvider(TypeDescriptor.GetProvider(GetType(String))), GetType(String))

		Me.Initialize()
	End Sub

	Protected Overridable Sub Initialize()

	End Sub

	Protected Function CreateGenericContext(ByVal owner As Object) As ExpressionContext
		Dim context As ExpressionContext

		If owner Is Nothing Then
			context = New ExpressionContext()
		Else
			context = New ExpressionContext(owner)
		End If

		context.Options.OwnerMemberAccess = Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic
		context.Imports.ImportBuiltinTypes()
		context.Imports.AddType(GetType(Math), "Math")
		context.Imports.AddType(GetType(Uri), "Uri")
		context.Imports.AddType(GetType(Mouse), "Mouse")
		context.Imports.AddType(GetType(Monitor), "Monitor")
		context.Imports.AddType(GetType(DateTime), "DateTime")
		context.Imports.AddType(GetType(Convert), "Convert")
		context.Imports.AddType(GetType(Type), "Type")
		context.Imports.AddType(GetType(DayOfWeek), "DayOfWeek")
		context.Imports.AddType(GetType(ConsoleModifiers), "ConsoleModifiers")

		Dim ns1 As New NamespaceImport("ns1")
		Dim ns2 As New NamespaceImport("ns2")
		ns2.Add(New TypeImport(GetType(Math)))

		ns1.Add(ns2)

		context.Imports.RootImport.Add(ns1)

		context.Variables.Add("varInt32", 100)
		context.Variables.Add("varDecimal", New Decimal(100))
		context.Variables.Add("varString", "string")

		Return context
	End Function
#End Region

#Region "Test framework"

	Protected Function CreateGenericExpression(Of T)(ByVal expression As String) As IGenericExpression(Of T)
		Return Me.CreateGenericExpression(Of T)(expression, New ExpressionContext())
	End Function

	Protected Function CreateGenericExpression(Of T)(ByVal expression As String, ByVal context As ExpressionContext) As IGenericExpression(Of T)
		Dim e As IGenericExpression(Of T) = context.CompileGeneric(Of T)(expression)
		Return e
	End Function

	Protected Function CreateDynamicExpression(ByVal expression As String) As IDynamicExpression
		Return Me.CreateDynamicExpression(expression, New ExpressionContext())
	End Function

	Protected Function CreateDynamicExpression(ByVal expression As String, ByVal context As ExpressionContext) As IDynamicExpression
		Return context.CompileDynamic(expression)
	End Function

	Protected Sub AssertCompileException(ByVal expression As String)
		Try
			Me.CreateDynamicExpression(expression)
			Assert.Fail()
		Catch ex As ExpressionCompileException

		End Try
	End Sub

	Protected Sub AssertCompileException(ByVal expression As String, ByVal context As ExpressionContext, Optional ByVal expectedReason As CompileExceptionReason = -1)
		Try
			Me.CreateDynamicExpression(expression, context)
			Assert.Fail("Compile exception expected")
		Catch ex As ExpressionCompileException
			If expectedReason <> -1 Then
				Assert.AreEqual(expectedReason, ex.Reason, String.Format("Expected reason '{0}' but got '{1}'", expectedReason, ex.Reason))
			End If
		End Try
	End Sub

	Protected Sub DoTest(ByVal e As IDynamicExpression, ByVal result As String, ByVal resultType As Type, ByVal testCulture As CultureInfo)
		If resultType Is GetType(Object) Then
			Dim expectedType As Type = Type.GetType(result, False, True)

			If expectedType Is Nothing Then
				' Try to get the type from the Tests assembly
				result = String.Format("{0}.{1}", Me.GetType().Namespace, result)
				expectedType = Me.GetType().Assembly.GetType(result, True, True)
			End If

			Dim expressionResult As Object = e.Evaluate()

			If expectedType Is GetType(Void) Then
				Assert.IsNull(expressionResult)
			Else
				Assert.IsInstanceOfType(expectedType, expressionResult)
			End If
		Else

			Dim tc As TypeConverter = TypeDescriptor.GetConverter(resultType)

			Dim expectedResult As Object = tc.ConvertFromString(Nothing, testCulture, result)
			Dim actualResult As Object = e.Evaluate()

			expectedResult = RoundIfReal(expectedResult)
			actualResult = RoundIfReal(actualResult)

			Assert.AreEqual(expectedResult, actualResult)
		End If
	End Sub

	Protected Function RoundIfReal(ByVal value As Object) As Object
		If value.GetType() Is GetType(Double) Then
			Dim d As Double = DirectCast(value, Double)
			d = Math.Round(d, 4)
			Return d
		ElseIf value.GetType() Is GetType(Single) Then
			Dim s As Single = DirectCast(value, Single)
			s = Math.Round(s, 4)
			Return s
		Else
			Return value
		End If
	End Function

	Protected Sub ProcessScriptTests(ByVal scriptFileName As String, ByVal processor As LineProcessor)
		Me.WriteMessage("Testing: {0}", scriptFileName)

		Dim instream As System.IO.Stream = Me.GetScriptFile(scriptFileName)
		Dim sr As New System.IO.StreamReader(instream)

		Try
			Me.ProcessLines(sr, processor)
		Finally
			sr.Close()
		End Try
	End Sub

	Private Sub ProcessLines(ByVal sr As System.IO.TextReader, ByVal processor As LineProcessor)
		While sr.Peek() <> -1
			Dim line As String = sr.ReadLine()
			Me.ProcessLine(line, processor)
		End While
	End Sub

	Private Sub ProcessLine(ByVal line As String, ByVal processor As LineProcessor)
		If line.StartsWith(COMMENT_CHAR) = True Then
			Return
		End If

		Try
			Dim arr As String() = line.Split(SEPARATOR_CHAR)
			processor(arr)
		Catch ex As Exception
			Me.WriteMessage("Failed line: {0}", line)
			Throw
		End Try
	End Sub

	Protected Function GetScriptFile(ByVal fileName As String) As System.IO.Stream
		Dim a As Assembly = Assembly.GetExecutingAssembly()
		Return a.GetManifestResourceStream(Me.GetType(), fileName)
	End Function

	Protected Function GetIndividualTest(ByVal testName As String) As String
		Dim a As Assembly = Assembly.GetExecutingAssembly()

		Dim s As Stream = a.GetManifestResourceStream(Me.GetType(), "IndividualTests.xml")

		Dim doc As New XPathDocument(s)

		Dim nav As XPathNavigator = doc.CreateNavigator()

		nav = nav.SelectSingleNode(String.Format("Tests/Test[@Name='{0}']", testName))

		Dim str As String = DirectCast(nav.TypedValue, String)

		s.Close()

		Return str
	End Function

	Protected Sub WriteMessage(ByVal msg As String, ByVal ParamArray args As Object())
		msg = String.Format(msg, args)
		Console.WriteLine(msg)
	End Sub

#End Region

#Region "Utility"

	Protected Shared Function Parse(ByVal s As String) As Object
		Dim b As Boolean

		If Boolean.TryParse(s, b) = True Then
			Return b
		End If

		Dim i As Integer

		If Integer.TryParse(s, NumberStyles.Integer, TestCulture, i) = True Then
			Return i
		End If

		Dim d As Double

		If Double.TryParse(s, NumberStyles.Float, TestCulture, i) = True Then
			Return d
		End If

		Dim dt As DateTime

		If DateTime.TryParse(s, TestCulture, DateTimeStyles.None, dt) = True Then
			Return dt
		End If

		Return s
	End Function

	Protected Shared Function ParseQueryString(ByVal s As String) As IDictionary(Of String, Object)
		Dim arr As String() = s.Split("&")
		Dim dict As New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)

		For Each part As String In arr
			Dim arr2 As String() = part.Split("=")
			dict.Add(arr2(0), Parse(arr2(1)))
		Next

		Return dict
	End Function
#End Region

End Class