' 
' * RegExpException.cs 
' * 
' * This library is free software; you can redistribute it and/or 
' * modify it under the terms of the GNU Lesser General Public License 
' * as published by the Free Software Foundation; either version 2.1 
' * of the License, or (at your option) any later version. 
' * 
' * This library is distributed in the hope that it will be useful, 
' * but WITHOUT ANY WARRANTY; without even the implied warranty of 
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU 
' * Lesser General Public License for more details. 
' * 
' * You should have received a copy of the GNU Lesser General Public 
' * License along with this library; if not, write to the Free 
' * Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, 
' * MA 02111-1307, USA. 
' * 
' * Copyright (c) 2003-2005 Per Cederberg. All rights reserved. 
' 

' Converted to VB.NET	[Eugene Ciloci; Nov 24, 2007]

Imports System 
Imports System.Text 

Namespace PerCederberg.Grammatica.Runtime.RE

	'* 
	' * A regular expression exception. This exception is thrown if a 
	' * regular expression couldn't be processed (or "compiled") 
	' * properly. 
	' * 
	' * @author Per Cederberg, <per at percederberg dot net> 
	' * @version 1.0 
	' 

	Friend Class RegExpException
		Inherits Exception

		'* 
		' * The error type enumeration. 
		' 

		Public Enum ErrorType

			'* 
			' * The unexpected character error constant. This error is 
			' * used when a character was read that didn't match the 
			' * allowed set of characters at the given position. 
			' 

			UNEXPECTED_CHARACTER

			'* 
			' * The unterminated pattern error constant. This error is 
			' * used when more characters were expected in the pattern. 
			' 

			UNTERMINATED_PATTERN

			'* 
			' * The unsupported special character error constant. This 
			' * error is used when special regular expression 
			' * characters are used in the pattern, but not supported 
			' * in this implementation. 
			' 

			UNSUPPORTED_SPECIAL_CHARACTER

			'* 
			' * The unsupported escape character error constant. This 
			' * error is used when an escape character construct is 
			' * used in the pattern, but not supported in this 
			' * implementation. 
			' 

			UNSUPPORTED_ESCAPE_CHARACTER

			'* 
			' * The invalid repeat count error constant. This error is 
			' * used when a repetition count of zero is specified, or 
			' * when the minimum exceeds the maximum. 
			' 

			INVALID_REPEAT_COUNT
		End Enum

		'* 
		' * The error type constant. 
		' 

		Private type As ErrorType

		'* 
		' * The error position. 
		' 

		Private position As Integer

		'* 
		' * The regular expression pattern. 
		' 

		Private pattern As String

		'* 
		' * Creates a new regular expression exception. 
		' * 
		' * @param type the error type constant 
		' * @param pos the error position 
		' * @param pattern the regular expression pattern 
		' 

		Public Sub New(ByVal type As ErrorType, ByVal pos As Integer, ByVal pattern As String)
			Me.type = type
			Me.position = pos
			Me.pattern = pattern
		End Sub

		'* 
		' * The message property. This property contains the detailed 
		' * exception error message. 
		' 

		Public Overloads Overrides ReadOnly Property Message() As String
			Get
				Return GetMessage()
			End Get
		End Property

		'* 
		' * Returns the exception error message. 
		' * 
		' * @return the exception error message 
		' 

		Public Function GetMessage() As String
			Dim buffer As New StringBuilder()

			' Append error type name 
			Select Case type
				Case ErrorType.UNEXPECTED_CHARACTER
					buffer.Append("unexpected character")
					Exit Select
				Case ErrorType.UNTERMINATED_PATTERN
					buffer.Append("unterminated pattern")
					Exit Select
				Case ErrorType.UNSUPPORTED_SPECIAL_CHARACTER
					buffer.Append("unsupported character")
					Exit Select
				Case ErrorType.UNSUPPORTED_ESCAPE_CHARACTER
					buffer.Append("unsupported escape character")
					Exit Select
				Case ErrorType.INVALID_REPEAT_COUNT
					buffer.Append("invalid repeat count")
					Exit Select
				Case Else
					buffer.Append("internal error")
					Exit Select
			End Select

			' Append erroneous character 
			buffer.Append(": ")
			If position < pattern.Length Then
				buffer.Append("'"c)
				buffer.Append(pattern.Substring(position))
				buffer.Append("'"c)
			Else
				buffer.Append("<end of pattern>")
			End If

			' Append position 
			buffer.Append(" at position ")
			buffer.Append(position)

			Return buffer.ToString()
		End Function
	End Class
End Namespace