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

''' <include file='Resources/DocComments.xml' path='DocComments/CompileExceptionReason/Class/*' />	
Public Enum CompileExceptionReason
	''' <include file='Resources/DocComments.xml' path='DocComments/CompileExceptionReason/SyntaxError/*' />	
	SyntaxError
	''' <include file='Resources/DocComments.xml' path='DocComments/CompileExceptionReason/ConstantOverflow/*' />	
	ConstantOverflow
	''' <include file='Resources/DocComments.xml' path='DocComments/CompileExceptionReason/TypeMismatch/*' />	
	TypeMismatch
	''' <include file='Resources/DocComments.xml' path='DocComments/CompileExceptionReason/UndefinedName/*' />	
	UndefinedName
	''' <include file='Resources/DocComments.xml' path='DocComments/CompileExceptionReason/FunctionHasNoReturnValue/*' />	
	FunctionHasNoReturnValue
	''' <include file='Resources/DocComments.xml' path='DocComments/CompileExceptionReason/InvalidExplicitCast/*' />	
	InvalidExplicitCast
	''' <include file='Resources/DocComments.xml' path='DocComments/CompileExceptionReason/AmbiguousMatch/*' />	
	AmbiguousMatch
	''' <include file='Resources/DocComments.xml' path='DocComments/CompileExceptionReason/AccessDenied/*' />	
	AccessDenied
	''' <include file='Resources/DocComments.xml' path='DocComments/CompileExceptionReason/InvalidFormat/*' />	
	InvalidFormat
End Enum

''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionCompileException/Class/*' />	
<Serializable()> _
Public NotInheritable Class ExpressionCompileException
	Inherits Exception

	Private MyReason As CompileExceptionReason

	Friend Sub New(ByVal message As String, ByVal reason As CompileExceptionReason)
		MyBase.New(message)
		MyReason = reason
	End Sub

	Friend Sub New(ByVal parseException As PerCederberg.Grammatica.Runtime.ParserLogException)
		MyBase.New(String.Empty, parseException)
		MyReason = CompileExceptionReason.SyntaxError
	End Sub

	Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
		MyBase.New(info, context)
		MyReason = info.GetInt32("Reason")
	End Sub

	Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
		MyBase.GetObjectData(info, context)
		info.AddValue("Reason", CInt(MyReason))
	End Sub

	Public Overrides ReadOnly Property Message() As String
		Get
			If MyReason = CompileExceptionReason.SyntaxError Then
				Dim innerEx As Exception = Me.InnerException
				Dim msg As String = String.Format("{0}: {1}", Utility.GetCompileErrorMessage(CompileErrorResourceKeys.SyntaxError), innerEx.Message)
				Return msg
			Else
				Return MyBase.Message
			End If
		End Get
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionCompileException/Reason/*' />	
	Public ReadOnly Property Reason() As CompileExceptionReason
		Get
			Return MyReason
		End Get
	End Property
End Class