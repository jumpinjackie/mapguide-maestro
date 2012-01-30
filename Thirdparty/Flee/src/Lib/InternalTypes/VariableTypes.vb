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

Friend Interface IVariable
	Function Clone() As IVariable
	ReadOnly Property VariableType() As Type
	Property ValueAsObject() As Object
End Interface

Friend Interface IGenericVariable(Of T)
	Function GetValue() As T
End Interface

Friend Class DynamicExpressionVariable(Of T)
	Implements IVariable
	Implements IGenericVariable(Of T)

	Private MyExpression As IDynamicExpression

	Public Function Clone() As IVariable Implements IVariable.Clone
		Dim copy As New DynamicExpressionVariable(Of T)()
		copy.MyExpression = MyExpression
		Return copy
	End Function

	Public Function GetValue() As T Implements IGenericVariable(Of T).GetValue
		Return DirectCast(MyExpression.Evaluate(), T)
	End Function

	Public Property ValueAsObject() As Object Implements IVariable.ValueAsObject
		Get
			Return MyExpression
		End Get
		Set(ByVal value As Object)
			MyExpression = value
		End Set
	End Property

	Public ReadOnly Property VariableType() As System.Type Implements IVariable.VariableType
		Get
			Return MyExpression.Context.Options.ResultType
		End Get
	End Property
End Class

Friend Class GenericExpressionVariable(Of T)
	Implements IVariable
	Implements IGenericVariable(Of T)

	Private MyExpression As IGenericExpression(Of T)

	Public Function Clone() As IVariable Implements IVariable.Clone
		Dim copy As New GenericExpressionVariable(Of T)()
		copy.MyExpression = MyExpression
		Return copy
	End Function

	Public Function GetValue() As T Implements IGenericVariable(Of T).GetValue
		Return MyExpression.Evaluate()
	End Function

	Public Property ValueAsObject() As Object Implements IVariable.ValueAsObject
		Get
			Return MyExpression
		End Get
		Set(ByVal value As Object)
			MyExpression = value
		End Set
	End Property

	Public ReadOnly Property VariableType() As System.Type Implements IVariable.VariableType
		Get
			Return MyExpression.Context.Options.ResultType
		End Get
	End Property
End Class

Friend Class GenericVariable(Of T)
	Implements IVariable
	Implements IGenericVariable(Of T)

	Public MyValue As T

	Public Function Clone() As IVariable Implements IVariable.Clone
		Dim copy As New GenericVariable(Of T)()
		copy.MyValue = MyValue
		Return copy
	End Function

	Public Function GetValue() As T Implements IGenericVariable(Of T).GetValue
		Return MyValue
	End Function

	Public ReadOnly Property VariableType() As System.Type Implements IVariable.VariableType
		Get
			Return GetType(T)
		End Get
	End Property

	Public Property ValueAsObject() As Object Implements IVariable.ValueAsObject
		Get
			Return MyValue
		End Get
		Set(ByVal value As Object)
			If value Is Nothing Then
				MyValue = Nothing
			Else
				MyValue = DirectCast(value, T)
			End If
		End Set
	End Property
End Class