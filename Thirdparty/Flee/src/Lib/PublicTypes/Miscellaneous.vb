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

''' <include file='Resources/DocComments.xml' path='DocComments/IExpression/Class/*' />	
Public Interface IExpression

	''' <include file='Resources/DocComments.xml' path='DocComments/IExpression/Clone/*' />	
	Function Clone() As IExpression
	''' <include file='Resources/DocComments.xml' path='DocComments/IExpression/Text/*' />	
	ReadOnly Property Text() As String
	''' <include file='Resources/DocComments.xml' path='DocComments/IExpression/Info/*' />	
	ReadOnly Property Info() As ExpressionInfo
	''' <include file='Resources/DocComments.xml' path='DocComments/IExpression/Context/*' />	
	ReadOnly Property Context() As ExpressionContext
	''' <include file='Resources/DocComments.xml' path='DocComments/IExpression/Owner/*' />	
	Property Owner() As Object
End Interface

''' <include file='Resources/DocComments.xml' path='DocComments/IDynamicExpression/Class/*' />	
Public Interface IDynamicExpression
	Inherits IExpression
	''' <include file='Resources/DocComments.xml' path='DocComments/IDynamicExpression/Evaluate/*' />	
	Function Evaluate() As Object
End Interface

''' <include file='Resources/DocComments.xml' path='DocComments/IGenericExpression/Class/*' />	
Public Interface IGenericExpression(Of T)
	Inherits IExpression
	''' <include file='Resources/DocComments.xml' path='DocComments/IGenericExpression/Evaluate/*' />	
	Function Evaluate() As T
End Interface

''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionInfo/Class/*' />	
Public NotInheritable Class ExpressionInfo

	Private MyData As IDictionary(Of String, Object)

	Friend Sub New()
		MyData = New Dictionary(Of String, Object)
		MyData.Add("ReferencedVariables", New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase))
	End Sub

	Friend Sub AddReferencedVariable(ByVal name As String)
		Dim dict As IDictionary(Of String, String) = MyData.Item("ReferencedVariables")
		dict.Item(name) = name
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionInfo/GetReferencedVariables/*' />	
	Public Function GetReferencedVariables() As String()
		Dim dict As IDictionary(Of String, String) = MyData.Item("ReferencedVariables")
		Dim arr(dict.Count - 1) As String
		dict.Keys.CopyTo(arr, 0)
		Return arr
	End Function
End Class

''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionOwnerMemberAccessAttribute/Class/*' />	
<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Method Or AttributeTargets.Property, AllowMultiple:=False, Inherited:=False)> _
Public NotInheritable Class ExpressionOwnerMemberAccessAttribute
	Inherits Attribute

	Private MyAllowAccess As Boolean

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionOwnerMemberAccessAttribute/New/*' />	
	Public Sub New(ByVal allowAccess As Boolean)
		MyAllowAccess = allowAccess
	End Sub

	Friend ReadOnly Property AllowAccess() As Boolean
		Get
			Return MyAllowAccess
		End Get
	End Property
End Class

''' <include file='Resources/DocComments.xml' path='DocComments/ResolveVariableTypeEventArgs/Class/*' />
Public Class ResolveVariableTypeEventArgs
	Inherits EventArgs

	Private MyName As String
	Private MyType As Type

	Friend Sub New(ByVal name As String)
		Me.MyName = name
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/ResolveVariableTypeEventArgs/VariableName/*' />
	Public ReadOnly Property VariableName() As String
		Get
			Return MyName
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ResolveVariableTypeEventArgs/VariableType/*' />
	Public Property VariableType() As Type
		Get
			Return MyType
		End Get
		Set(ByVal value As Type)
			MyType = value
		End Set
	End Property
End Class

''' <include file='Resources/DocComments.xml' path='DocComments/ResolveVariableValueEventArgs/Class/*' />
Public Class ResolveVariableValueEventArgs
	Inherits EventArgs

	Private MyName As String
	Private MyType As Type
	Private MyValue As Object

	Friend Sub New(ByVal name As String, ByVal t As Type)
		MyName = name
		MyType = t
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/ResolveVariableValueEventArgs/VariableName/*' />
	Public ReadOnly Property VariableName() As String
		Get
			Return MyName
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ResolveVariableValueEventArgs/VariableType/*' />
	Public ReadOnly Property VariableType() As Type
		Get
			Return MyType
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ResolveVariableValueEventArgs/VariableValue/*' />
	Public Property VariableValue() As Object
		Get
			Return MyValue
		End Get
		Set(ByVal value As Object)
			MyValue = value
		End Set
	End Property
End Class

''' <include file='Resources/DocComments.xml' path='DocComments/ResolveFunctionEventArgs/Class/*' />
Public Class ResolveFunctionEventArgs
	Inherits EventArgs

	Private MyName As String
	Private MyArgumentTypes As Type()
	Private MyReturnType As Type

	Friend Sub New(ByVal name As String, ByVal argumentTypes As Type())
		MyName = name
		MyArgumentTypes = argumentTypes
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/ResolveFunctionEventArgs/FunctionName/*' />
	Public ReadOnly Property FunctionName() As String
		Get
			Return MyName
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ResolveFunctionEventArgs/ArgumentTypes/*' />
	Public ReadOnly Property ArgumentTypes() As Type()
		Get
			Return MyArgumentTypes
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ResolveFunctionEventArgs/ReturnType/*' />
	Public Property ReturnType() As Type
		Get
			Return MyReturnType
		End Get
		Set(ByVal value As Type)
			MyReturnType = value
		End Set
	End Property
End Class

''' <include file='Resources/DocComments.xml' path='DocComments/InvokeFunctionEventArgs/Class/*' />
Public Class InvokeFunctionEventArgs
	Inherits EventArgs

	Private MyName As String
	Private MyArguments As Object()
	Private MyFunctionResult As Object

	Friend Sub New(ByVal name As String, ByVal arguments As Object())
		MyName = name
		MyArguments = arguments
	End Sub

	''' <include file='Resources/DocComments.xml' path='DocComments/InvokeFunctionEventArgs/FunctionName/*' />
	Public ReadOnly Property FunctionName() As String
		Get
			Return MyName
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/InvokeFunctionEventArgs/Arguments/*' />
	Public ReadOnly Property Arguments() As Object()
		Get
			Return MyArguments
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/InvokeFunctionEventArgs/Result/*' />
	Public Property Result() As Object
		Get
			Return MyFunctionResult
		End Get
		Set(ByVal value As Object)
			MyFunctionResult = value
		End Set
	End Property
End Class

''' <include file='Resources/DocComments.xml' path='DocComments/RealLiteralDataType/Class/*' />	
Public Enum RealLiteralDataType
	''' <include file='Resources/DocComments.xml' path='DocComments/RealLiteralDataType/Single/*' />	
	[Single]
	''' <include file='Resources/DocComments.xml' path='DocComments/RealLiteralDataType/Double/*' />	
	[Double]
	''' <include file='Resources/DocComments.xml' path='DocComments/RealLiteralDataType/Decimal/*' />	
	[Decimal]
End Enum