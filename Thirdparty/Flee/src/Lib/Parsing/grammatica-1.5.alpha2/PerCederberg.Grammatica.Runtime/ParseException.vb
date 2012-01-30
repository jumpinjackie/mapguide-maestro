' 
' * ParseException.cs 
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
Imports System.Collections 
Imports System.Text 

Namespace PerCederberg.Grammatica.Runtime 
    
    '* 
' * A parse exception. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
	<Serializable()> _
	Friend Class ParseException
		Inherits Exception

		'* 
		' * The error type enumeration. 
		' 

		Public Enum ErrorType

			'* 
			' * The internal error type is only used to signal an error 
			' * that is a result of a bug in the parser or tokenizer 
			' * code. 
			' 

			INTERNAL

			'* 
			' * The I/O error type is used for stream I/O errors. 
			' 

			IO

			'* 
			' * The unexpected end of file error type is used when end 
			' * of file is encountered instead of a valid token. 
			' 

			UNEXPECTED_EOF

			'* 
			' * The unexpected character error type is used when a 
			' * character is read that isn't handled by one of the 
			' * token patterns. 
			' 

			UNEXPECTED_CHAR

			'* 
			' * The unexpected token error type is used when another 
			' * token than the expected one is encountered. 
			' 

			UNEXPECTED_TOKEN

			'* 
			' * The invalid token error type is used when a token 
			' * pattern with an error message is matched. The 
			' * additional information provided should contain the 
			' * error message. 
			' 

			INVALID_TOKEN

			'* 
			' * The analysis error type is used when an error is 
			' * encountered in the analysis. The additional information 
			' * provided should contain the error message. 
			' 

			ANALYSIS
		End Enum

		'* 
		' * The error type. 
		' 

		Private m_type As ErrorType

		'* 
		' * The additional information string. 
		' 

		Private m_info As String

		'* 
		' * The additional details information. This variable is only 
		' * used for unexpected token errors. 
		' 

		Private m_details As ArrayList

		'* 
		' * The line number. 
		' 

		Private m_line As Integer

		'* 
		' * The column number. 
		' 

		Private m_column As Integer

		'* 
		' * Creates a new parse exception. 
		' * 
		' * @param type the parse error type 
		' * @param info the additional information 
		' * @param line the line number, or -1 for unknown 
		' * @param column the column number, or -1 for unknown 
		' 

		Public Sub New(ByVal type As ErrorType, ByVal info As String, ByVal line As Integer, ByVal column As Integer)
			Me.New(type, info, Nothing, line, column)
		End Sub

		'* 
		' * Creates a new parse exception. This constructor is only 
		' * used to supply the detailed information array, which is 
		' * only used for expected token errors. The list then contains 
		' * descriptions of the expected tokens. 
		' * 
		' * @param type the parse error type 
		' * @param info the additional information 
		' * @param details the additional detailed information 
		' * @param line the line number, or -1 for unknown 
		' * @param column the column number, or -1 for unknown 
		' 

		Public Sub New(ByVal type As ErrorType, ByVal info As String, ByVal details As ArrayList, ByVal line As Integer, ByVal column As Integer)
			Me.m_type = type
			Me.m_info = info
			Me.m_details = details
			Me.m_line = line
			Me.m_column = column
		End Sub

		Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
			MyBase.New(info, context)
			Me.m_type = info.GetInt32("Type")
			Me.m_info = info.GetString("Info")
			Me.m_details = info.GetValue("Details", GetType(ArrayList))
			Me.m_line = info.GetInt32("Line")
			Me.m_column = info.GetInt32("Column")
		End Sub

		Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
			MyBase.GetObjectData(info, context)
			info.AddValue("Type", Me.m_type)
			info.AddValue("Info", Me.m_info)
			info.AddValue("Details", Me.m_details)
			info.AddValue("Line", Me.m_line)
			info.AddValue("Column", Me.m_column)
		End Sub

		'* 
		' * The error type property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Type() As ErrorType
			Get
				Return m_type
			End Get
		End Property

		'* 
		' * Returns the error type. 
		' * 
		' * @return the error type 
		' * 
		' * @see #Type 
		' * 
		' * @deprecated Use the Type property instead. 
		' 

		Public Function GetErrorType() As ErrorType
			Return Type
		End Function

		'* 
		' * The additional error information property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Info() As String
			Get
				Return m_info
			End Get
		End Property

		'* 
		' * Returns the additional error information. 
		' * 
		' * @return the additional error information 
		' * 
		' * @see #Info 
		' * 
		' * @deprecated Use the Info property instead. 
		' 

		Public Function GetInfo() As String
			Return Info
		End Function

		'* 
		' * The additional detailed error information property 
		' * (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Details() As ArrayList
			Get
				Return New ArrayList(m_details)
			End Get
		End Property

		'* 
		' * Returns the additional detailed error information. 
		' * 
		' * @return the additional detailed error information 
		' * 
		' * @see #Details 
		' * 
		' * @deprecated Use the Details property instead. 
		' 

		Public Function GetDetails() As ArrayList
			Return Details
		End Function

		'* 
		' * The line number property (read-only). This is the line 
		' * number where the error occured, or -1 if unknown. 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Line() As Integer
			Get
				Return m_line
			End Get
		End Property

		'* 
		' * Returns the line number where the error occured. 
		' * 
		' * @return the line number of the error, or 
		' * -1 if unknown 
		' * 
		' * @see #Line 
		' * 
		' * @deprecated Use the Line property instead. 
		' 

		Public Function GetLine() As Integer
			Return Line
		End Function

		'* 
		' * The column number property (read-only). This is the column 
		' * number where the error occured, or -1 if unknown. 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Column() As Integer
			Get
				Return m_column
			End Get
		End Property

		'* 
		' * Returns the column number where the error occured. 
		' * 
		' * @return the column number of the error, or 
		' * -1 if unknown 
		' * 
		' * @see #Column 
		' * 
		' * @deprecated Use the Column property instead. 
		' 

		Public Function GetColumn() As Integer
			Return m_column
		End Function

		'* 
		' * The message property (read-only). This property contains 
		' * the detailed exception error message, including line and 
		' * column numbers when available. 
		' * 
		' * @see #ErrorMessage 
		' 

		Public Overloads Overrides ReadOnly Property Message() As String
			Get
				Dim buffer As New StringBuilder()

				' Add error description 
				buffer.AppendLine(ErrorMessage)

				' Add line and column 
				If m_line > 0 AndAlso m_column > 0 Then
					Dim msg As String = FleeResourceManager.Instance.GetCompileErrorString(CompileErrorResourceKeys.LineColumn)
					msg = String.Format(msg, m_line, m_column)
					buffer.AppendLine(msg)
					'buffer.Append(", on line: ")
					'buffer.Append(m_line)
					'buffer.Append(" column: ")
					'buffer.Append(m_column)
				End If

				Return buffer.ToString()
			End Get
		End Property

		'* 
		' * Returns a default error message. 
		' * 
		' * @return a default error message 
		' * 
		' * @see #Message 
		' * 
		' * @deprecated Use the Message property instead. 
		' 

		Public Function GetMessage() As String
			Return Message
		End Function

		'* 
		' * The error message property (read-only). This property 
		' * contains all the information available, except for the line 
		' * and column number information. 
		' * 
		' * @see #Message 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property ErrorMessage() As String
			Get
				Dim args As New List(Of String)()

				Select Case m_type
					Case ErrorType.IO, ErrorType.UNEXPECTED_CHAR, ErrorType.INVALID_TOKEN, ErrorType.ANALYSIS, ErrorType.INTERNAL
						args.Add(m_info)
					Case ErrorType.UNEXPECTED_TOKEN
						args.Add(m_info)
						args.Add(Me.GetMessageDetails())
					Case ErrorType.UNEXPECTED_EOF
				End Select

				Dim msg As String = FleeResourceManager.Instance.GetCompileErrorString(m_type.ToString())
				msg = String.Format(msg, args.ToArray())
				Return msg

				' Add type and info 
				'Select Case m_type
				'	Case ErrorType.IO
				'		buffer.Append("I/O error: ")
				'		buffer.Append(m_info)
				'		Exit Select
				'	Case ErrorType.UNEXPECTED_EOF
				'		buffer.Append("unexpected end of file")
				'		Exit Select
				'	Case ErrorType.UNEXPECTED_CHAR
				'		buffer.Append("unexpected character '")
				'		buffer.Append(m_info)
				'		buffer.Append("'")
				'		Exit Select
				'	Case ErrorType.UNEXPECTED_TOKEN
				'		buffer.Append("unexpected token ")
				'		buffer.Append(m_info)
				'		If m_details IsNot Nothing Then
				'			buffer.Append(", expected ")
				'			If m_details.Count > 1 Then
				'				buffer.Append("one of ")
				'			End If
				'			buffer.Append(GetMessageDetails())
				'		End If
				'		Exit Select
				'	Case ErrorType.INVALID_TOKEN
				'		buffer.Append(m_info)
				'		Exit Select
				'	Case ErrorType.ANALYSIS
				'		buffer.Append(m_info)
				'		Exit Select
				'	Case Else
				'		buffer.Append("internal error")
				'		If m_info IsNot Nothing Then
				'			buffer.Append(": ")
				'			buffer.Append(m_info)
				'		End If
				'		Exit Select
				'End Select

				'Return buffer.ToString()
			End Get
		End Property

		'* 
		' * Returns the error message. This message will contain all the 
		' * information available, except for the line and column number 
		' * information. 
		' * 
		' * @return the error message 
		' * 
		' * @see #ErrorMessage 
		' * 
		' * @deprecated Use the ErrorMessage property instead. 
		' 

		Public Function GetErrorMessage() As String
			Return ErrorMessage
		End Function

		'* 
		' * Returns a string containing all the detailed information in 
		' * a list. The elements are separated with a comma. 
		' * 
		' * @return the detailed information string 
		' 

		Private Function GetMessageDetails() As String
			Dim buffer As New StringBuilder()
			For i As Integer = 0 To m_details.Count - 1

				If i > 0 Then
					buffer.Append(", ")
					If i + 1 = m_details.Count Then
						buffer.Append("or ")
					End If
				End If
				buffer.Append(m_details(i))
			Next

			Return buffer.ToString()
		End Function
	End Class
End Namespace