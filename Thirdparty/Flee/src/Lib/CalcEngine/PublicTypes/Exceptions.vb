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

Namespace CalcEngine

	''' <include file='Resources/DocComments.xml' path='DocComments/CircularReferenceException/Class/*' />	
	<Serializable()> _
	Public Class CircularReferenceException
		Inherits System.Exception

		Private MyCircularReferenceSource As String

		Friend Sub New()

		End Sub

		Friend Sub New(ByVal circularReferenceSource As String)
			MyCircularReferenceSource = circularReferenceSource
		End Sub

		Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
			MyBase.New(info, context)
		End Sub

		Public Overrides ReadOnly Property Message() As String
			Get
				If MyCircularReferenceSource Is Nothing Then
					Return "Circular reference detected in calculation engine"
				Else
					Return String.Format("Circular reference detected in calculation engine at '{0}'", MyCircularReferenceSource)
				End If
			End Get
		End Property
	End Class

	''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="BatchLoadCompileException"]/*' />	
	<Serializable()> _
	Public Class BatchLoadCompileException
		Inherits Exception

		Private MyAtomName As String
		Private MyExpressionText As String

		Friend Sub New(ByVal atomName As String, ByVal expressionText As String, ByVal innerException As ExpressionCompileException)
			MyBase.New(String.Format("Batch Load: The expression for atom '${0}' could not be compiled", atomName), innerException)
			MyAtomName = atomName
			MyExpressionText = expressionText
		End Sub

		Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
			MyBase.New(info, context)
			MyAtomName = info.GetString("AtomName")
			MyExpressionText = info.GetString("ExpressionText")
		End Sub

		Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
			MyBase.GetObjectData(info, context)
			info.AddValue("AtomName", MyAtomName)
			info.AddValue("ExpressionText", MyExpressionText)
		End Sub

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="BatchLoadCompileException.AtomName"]/*' />	
		Public ReadOnly Property AtomName() As String
			Get
				Return MyAtomName
			End Get
		End Property

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="BatchLoadCompileException.ExpressionText"]/*' />	
		Public ReadOnly Property ExpressionText() As String
			Get
				Return MyExpressionText
			End Get
		End Property
	End Class

End Namespace