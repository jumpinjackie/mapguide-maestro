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

Imports Ciloci.Flee.CalcEngine
Imports System.IO

''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionContext/Class/*' />
Public NotInheritable Class ExpressionContext

#Region "Fields"

	Private MyProperties As PropertyDictionary
	Private MySyncRoot As New Object

	''' <remarks>Keep variables as a field to make access fast</remarks>
	Private MyVariables As VariableCollection

#End Region

#Region "Constructor"

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionContext/New1/*' />
	Public Sub New()
		Me.New(DefaultExpressionOwner.Instance)
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionContext/New2/*' />
	Public Sub New(ByVal expressionOwner As Object)
		Utility.AssertNotNull(expressionOwner, "expressionOwner")
		MyProperties = New PropertyDictionary()

		MyProperties.SetValue("CalculationEngine", Nothing)
		MyProperties.SetValue("CalcEngineExpressionName", Nothing)
		MyProperties.SetValue("IdentifierParser", Nothing)

		MyProperties.SetValue("ExpressionOwner", expressionOwner)

		MyProperties.SetValue("ParserOptions", New ExpressionParserOptions(Me))

		MyProperties.SetValue("Options", New ExpressionOptions(Me))
		MyProperties.SetValue("Imports", New ExpressionImports())
		Me.Imports.SetContext(Me)
		MyVariables = New VariableCollection(Me)

		MyProperties.SetToDefault(Of Boolean)("NoClone")

		Me.RecreateParser()
	End Sub

#End Region

#Region "Methods - Private"

	Private Sub AssertTypeIsAccessibleInternal(ByVal t As Type)
		Dim isPublic As Boolean = t.IsPublic

		If t.IsNested = True Then
			isPublic = t.IsNestedPublic
		End If

		Dim isSameModuleAsOwner As Boolean = t.Module Is Me.ExpressionOwner.GetType().Module

		' Public types are always accessible.  Otherwise they have to be in the same module as the owner
		Dim isAccessible As Boolean = isPublic Or isSameModuleAsOwner

		If isAccessible = False Then
			Dim msg As String = Utility.GetGeneralErrorMessage(GeneralErrorResourceKeys.TypeNotAccessibleToExpression, t.Name)
			Throw New ArgumentException(msg)
		End If
	End Sub

	Private Sub AssertNestedTypeIsAccessible(ByVal t As Type)
		While Not t Is Nothing
			AssertTypeIsAccessibleInternal(t)
			t = t.DeclaringType
		End While
	End Sub
#End Region

#Region "Methods - Internal"

	Friend Function CloneInternal(ByVal cloneVariables As Boolean) As ExpressionContext
		Dim context As ExpressionContext = Me.MemberwiseClone()
		context.MyProperties = MyProperties.Clone()
		context.MyProperties.SetValue("Options", Me.Options.Clone())
		context.MyProperties.SetValue("ParserOptions", Me.ParserOptions.Clone())
		context.MyProperties.SetValue("Imports", Me.Imports.Clone())
		context.Imports.SetContext(context)

		If cloneVariables = True Then
			context.MyVariables = New VariableCollection(Me)
			Me.Variables.Copy(context.MyVariables)
		End If

		Return context
	End Function

	Friend Sub AssertTypeIsAccessible(ByVal t As Type)
		If t.IsNested = True Then
			AssertNestedTypeIsAccessible(t)
		Else
			AssertTypeIsAccessibleInternal(t)
		End If
	End Sub

	' Does the actual parsing of an expression.  Thead-safe.
	Friend Function Parse(ByVal expression As String, ByVal services As IServiceProvider) As ExpressionElement
		SyncLock MySyncRoot
			Dim sr As New System.IO.StringReader(expression)
			Dim parser As ExpressionParser = Me.Parser
			parser.Reset()
			parser.Tokenizer.Reset(sr)
			Dim analyzer As FleeExpressionAnalyzer = parser.Analyzer

			analyzer.SetServices(services)

			Dim rootNode As PerCederberg.Grammatica.Runtime.Node = DoParse()
			analyzer.Reset()
			Dim topElement As ExpressionElement = rootNode.Values.Item(0)
			Return topElement
		End SyncLock
	End Function

	Friend Sub RecreateParser()
		SyncLock MySyncRoot
			Dim analyzer As New FleeExpressionAnalyzer
			Dim parser As ExpressionParser = New ExpressionParser(System.IO.StringReader.Null, analyzer, Me)
			MyProperties.SetValue("ExpressionParser", parser)
		End SyncLock
	End Sub

	Friend Function DoParse() As PerCederberg.Grammatica.Runtime.Node
		Try
			Return Me.Parser.Parse()
		Catch ex As PerCederberg.Grammatica.Runtime.ParserLogException
			' Syntax error; wrap it in our exception and rethrow
			Throw New ExpressionCompileException(ex)
		End Try
	End Function

	Friend Sub SetCalcEngine(ByVal engine As CalculationEngine, ByVal calcEngineExpressionName As String)
		MyProperties.SetValue("CalculationEngine", engine)
		MyProperties.SetValue("CalcEngineExpressionName", calcEngineExpressionName)
	End Sub

	Friend Function ParseIdentifiers(ByVal expression As String) As IdentifierAnalyzer
		Dim parser As ExpressionParser = Me.IdentifierParser
		Dim sr As New StringReader(expression)
		parser.Reset()
		parser.Tokenizer.Reset(sr)

		Dim analyzer As IdentifierAnalyzer = parser.Analyzer
		analyzer.Reset()

		parser.Parse()

		Return parser.Analyzer
	End Function
#End Region

#Region "Methods - Public"

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionContext/Clone/*' />
	Public Function Clone() As ExpressionContext
		Return Me.CloneInternal(True)
	End Function

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionContext/CompileDynamic/*' />
	Public Function CompileDynamic(ByVal expression As String) As IDynamicExpression
		Return New Expression(Of Object)(expression, Me, False)
	End Function

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionContext/CompileGeneric/*' />
	Public Function CompileGeneric(Of TResultType)(ByVal expression As String) As IGenericExpression(Of TResultType)
		Return New Expression(Of TResultType)(expression, Me, True)
	End Function

#End Region

#Region "Properties - Private"

	Private ReadOnly Property IdentifierParser() As ExpressionParser
		Get
			Dim parser As ExpressionParser = MyProperties.GetValue(Of ExpressionParser)("IdentifierParser")

			If parser Is Nothing Then
				Dim analyzer As New IdentifierAnalyzer()
				parser = New ExpressionParser(System.IO.StringReader.Null, analyzer, Me)
				MyProperties.SetValue("IdentifierParser", parser)
			End If

			Return parser
		End Get
	End Property

#End Region

#Region "Properties - Internal"

	Friend Property NoClone() As Boolean
		Get
			Return MyProperties.GetValue(Of Boolean)("NoClone")
		End Get
		Set(ByVal value As Boolean)
			MyProperties.SetValue("NoClone", value)
		End Set
	End Property

	Friend ReadOnly Property ExpressionOwner() As Object
		Get
			Return MyProperties.GetValue(Of Object)("ExpressionOwner")
		End Get
	End Property

	Friend ReadOnly Property CalcEngineExpressionName() As String
		Get
			Return MyProperties.GetValue(Of String)("CalcEngineExpressionName")
		End Get
	End Property

	Friend ReadOnly Property Parser() As ExpressionParser
		Get
			Return MyProperties.GetValue(Of ExpressionParser)("ExpressionParser")
		End Get
	End Property
#End Region

#Region "Properties - Public"
	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionContext/Options/*' />
	Public ReadOnly Property Options() As ExpressionOptions
		Get
			Return MyProperties.GetValue(Of ExpressionOptions)("Options")
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionContext/Imports/*' />
	Public ReadOnly Property [Imports]() As ExpressionImports
		Get
			Return MyProperties.GetValue(Of ExpressionImports)("Imports")
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionContext/Variables/*' />
	Public ReadOnly Property Variables() As VariableCollection
		Get
			Return MyVariables
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionContext/CalculationEngine/*' />
	Public ReadOnly Property CalculationEngine() As CalculationEngine
		Get
			Return MyProperties.GetValue(Of CalculationEngine)("CalculationEngine")
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionContext/ParserOptions/*' />
	Public ReadOnly Property ParserOptions() As ExpressionParserOptions
		Get
			Return MyProperties.GetValue(Of ExpressionParserOptions)("ParserOptions")
		End Get
	End Property
#End Region
End Class