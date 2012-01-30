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

Imports System.Reflection.Emit
Imports System.Reflection
Imports System.ComponentModel.Design

Friend Class Expression(Of T)
	Implements IExpression
	Implements IDynamicExpression
	Implements IGenericExpression(Of T)

	Private MyExpression As String
	Private MyContext As ExpressionContext
	Private MyOptions As ExpressionOptions
	Private MyInfo As ExpressionInfo
	Private MyEvaluator As ExpressionEvaluator(Of T)
	Private MyOwner As Object

	Private Const EmitAssemblyName As String = "FleeExpression"
	Private Const DynamicMethodName As String = "Flee Expression"

	Public Sub New(ByVal expression As String, ByVal context As ExpressionContext, ByVal isGeneric As Boolean)
		Utility.AssertNotNull(expression, "expression")
		MyExpression = expression
		MyOwner = context.ExpressionOwner

		MyContext = context

		If context.NoClone = False Then
			MyContext = context.CloneInternal(False)
		End If

		MyInfo = New ExpressionInfo()

		Me.SetupOptions(MyContext.Options, isGeneric)

		MyContext.Imports.ImportOwner(MyOptions.OwnerType)

		Me.ValidateOwner(MyOwner)

		Me.Compile(expression, MyOptions)

		If Not MyContext.CalculationEngine Is Nothing Then
			MyContext.CalculationEngine.FixTemporaryHead(Me, MyContext, MyOptions.ResultType)
		End If
	End Sub

	Private Sub SetupOptions(ByVal options As ExpressionOptions, ByVal isGeneric As Boolean)
		' Make sure we clone the options
		MyOptions = options
		MyOptions.IsGeneric = isGeneric

		If isGeneric Then
			MyOptions.ResultType = GetType(T)
		End If

		MyOptions.SetOwnerType(MyOwner.GetType())
	End Sub

	Private Sub Compile(ByVal expression As String, ByVal options As ExpressionOptions)
		' Add the services that will be used by elements during the compile
		Dim services As IServiceContainer = New ServiceContainer()
		Me.AddServices(services)

		' Parse and get the root element of the parse tree
		Dim topElement As ExpressionElement = MyContext.Parse(expression, services)

		If options.ResultType Is Nothing Then
			options.ResultType = topElement.ResultType
		End If

		Dim rootElement As New RootExpressionElement(topElement, options.ResultType)

		Dim dm As DynamicMethod = Me.CreateDynamicMethod()

		Dim ilg As FleeILGenerator = New FleeILGenerator(dm.GetILGenerator())

		' Emit the IL
		rootElement.Emit(ilg, services)

		ilg.ValidateLength()

		' Emit to an assembly if required
		If options.EmitToAssembly = True Then
			EmitToAssembly(rootElement, services)
		End If

		Dim delegateType As Type = GetType(ExpressionEvaluator(Of )).MakeGenericType(GetType(T))
		MyEvaluator = dm.CreateDelegate(delegateType)
	End Sub

	Private Function CreateDynamicMethod() As DynamicMethod
		' Create the dynamic method
		Dim parameterTypes As Type() = {GetType(Object), GetType(ExpressionContext), GetType(VariableCollection)}
		Dim dm As DynamicMethod

		dm = New DynamicMethod(DynamicMethodName, GetType(T), parameterTypes, MyOptions.OwnerType)

		Return dm
	End Function

	Private Sub AddServices(ByVal dest As IServiceContainer)
		dest.AddService(GetType(ExpressionOptions), MyOptions)
		dest.AddService(GetType(ExpressionParserOptions), MyContext.ParserOptions)
		dest.AddService(GetType(ExpressionContext), MyContext)
		dest.AddService(GetType(IExpression), Me)
		dest.AddService(GetType(ExpressionInfo), MyInfo)
	End Sub

	Private Shared Sub EmitToAssembly(ByVal rootElement As ExpressionElement, ByVal services As IServiceContainer)
		Dim assemblyName As New AssemblyName(EmitAssemblyName)

		Dim assemblyFileName As String = String.Format("{0}.dll", EmitAssemblyName)

		Dim assemblyBuilder As AssemblyBuilder = System.AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save)
		Dim moduleBuilder As ModuleBuilder = assemblyBuilder.DefineDynamicModule(assemblyFileName, assemblyFileName)

		Dim mb As MethodBuilder = moduleBuilder.DefineGlobalMethod("Evaluate", MethodAttributes.Public Or MethodAttributes.Static, GetType(T), New Type() {GetType(Object), GetType(ExpressionContext), GetType(VariableCollection)})
		Dim ilg As FleeILGenerator = New FleeILGenerator(mb.GetILGenerator())

		rootElement.Emit(ilg, services)

		moduleBuilder.CreateGlobalFunctions()
		assemblyBuilder.Save(assemblyFileName)
	End Sub

	Private Sub ValidateOwner(ByVal owner As Object)
		Utility.AssertNotNull(owner, "owner")

		If MyOptions.OwnerType.IsAssignableFrom(owner.GetType()) = False Then
			Dim msg As String = Utility.GetGeneralErrorMessage(GeneralErrorResourceKeys.NewOwnerTypeNotAssignableToCurrentOwner)
			Throw New ArgumentException(msg)
		End If
	End Sub

	Public Function Evaluate() As Object Implements IDynamicExpression.Evaluate
		Return MyEvaluator(MyOwner, MyContext, MyContext.Variables)
	End Function

	Public Function EvaluateGeneric() As T Implements IGenericExpression(Of T).Evaluate
		Return MyEvaluator(MyOwner, MyContext, MyContext.Variables)
	End Function

	Public Function Clone() As IExpression Implements IExpression.Clone
		Dim copy As Expression(Of T) = Me.MemberwiseClone()
		copy.MyContext = MyContext.CloneInternal(True)
		copy.MyOptions = copy.MyContext.Options
		Return copy
	End Function

	Public Overrides Function ToString() As String
		Return MyExpression
	End Function

	Friend ReadOnly Property ResultType() As Type
		Get
			Return MyOptions.ResultType
		End Get
	End Property

	Public ReadOnly Property Text() As String Implements IExpression.Text
		Get
			Return MyExpression
		End Get
	End Property

	Public ReadOnly Property Info1() As ExpressionInfo Implements IExpression.Info
		Get
			Return MyInfo
		End Get
	End Property

	Public Property Owner() As Object Implements IExpression.Owner
		Get
			Return MyOwner
		End Get
		Set(ByVal value As Object)
			Me.ValidateOwner(value)
			MyOwner = value
		End Set
	End Property

	Public ReadOnly Property Context() As ExpressionContext Implements IExpression.Context
		Get
			Return MyContext
		End Get
	End Property
End Class