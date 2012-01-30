' 
' * ParserCreationException.cs 
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
' * A parser creation exception. This exception is used for signalling 
' * an error in the token or production patterns, making it impossible 
' * to create a working parser or tokenizer. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
	Friend Class ParserCreationException
		Inherits Exception

		'* 
		' * The error type enumeration. 
		' 

		Public Enum ErrorType

			'* 
			' * The internal error type is only used to signal an 
			' * error that is a result of a bug in the parser or 
			' * tokenizer code. 
			' 

			INTERNAL

			'* 
			' * The invalid parser error type is used when the parser 
			' * as such is invalid. This error is typically caused by 
			' * using a parser without any patterns. 
			' 

			INVALID_PARSER

			'* 
			' * The invalid token error type is used when a token 
			' * pattern is erroneous. This error is typically caused 
			' * by an invalid pattern type or an erroneous regular 
			' * expression. 
			' 

			INVALID_TOKEN

			'* 
			' * The invalid production error type is used when a 
			' * production pattern is erroneous. This error is 
			' * typically caused by referencing undeclared productions, 
			' * or violating some other production pattern constraint. 
			' 

			INVALID_PRODUCTION

			'* 
			' * The infinite loop error type is used when an infinite 
			' * loop has been detected in the grammar. One of the 
			' * productions in the loop will be reported. 
			' 

			INFINITE_LOOP

			'* 
			' * The inherent ambiguity error type is used when the set 
			' * of production patterns (i.e. the grammar) contains 
			' * ambiguities that cannot be resolved. 
			' 

			INHERENT_AMBIGUITY
		End Enum

		'* 
		' * The error type. 
		' 

		Private m_type As ErrorType

		'* 
		' * The token or production pattern name. This variable is only 
		' * set for some error types. 
		' 

		Private m_name As String

		'* 
		' * The additional error information string. This variable is only 
		' * set for some error types. 
		' 

		Private m_info As String

		'* 
		' * The error details list. This variable is only set for some 
		' * error types. 
		' 

		Private m_details As ArrayList

		'* 
		' * Creates a new parser creation exception. 
		' * 
		' * @param type the parse error type 
		' * @param info the additional error information 
		' 

		Public Sub New(ByVal type As ErrorType, ByVal info As String)
			Me.New(type, Nothing, info)
		End Sub

		'* 
		' * Creates a new parser creation exception. 
		' * 
		' * @param type the parse error type 
		' * @param name the token or production pattern name 
		' * @param info the additional error information 
		' 

		Public Sub New(ByVal type As ErrorType, ByVal name As String, ByVal info As String)
			Me.New(type, name, info, Nothing)
		End Sub

		'* 
		' * Creates a new parser creation exception. 
		' * 
		' * @param type the parse error type 
		' * @param name the token or production pattern name 
		' * @param info the additional error information 
		' * @param details the error details list 
		' 

		Public Sub New(ByVal type As ErrorType, ByVal name As String, ByVal info As String, ByVal details As ArrayList)

			Me.m_type = type
			Me.m_name = name
			Me.m_info = info
			Me.m_details = details
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
		' * The token or production name property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Name() As String
			Get
				Return m_name
			End Get
		End Property

		'* 
		' * Returns the token or production name. 
		' * 
		' * @return the token or production name 
		' * 
		' * @see #Name 
		' * 
		' * @deprecated Use the Name property instead. 
		' 

		Public Function GetName() As String
			Return Name
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
		' * The detailed error information property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Details() As String
			Get
				Dim buffer As New StringBuilder()

				If m_details Is Nothing Then
					Return Nothing
				End If
				For i As Integer = 0 To m_details.Count - 1
					If i > 0 Then
						buffer.Append(", ")
						If i + 1 = m_details.Count Then
							buffer.Append("and ")
						End If
					End If
					buffer.Append(m_details(i))
				Next

				Return buffer.ToString()
			End Get
		End Property

		'* 
		' * Returns the detailed error information as a string 
		' * 
		' * @return the detailed error information 
		' * 
		' * @see #Details 
		' * 
		' * @deprecated Use the Details property instead. 
		' 

		Public Function GetDetails() As String
			Return Details
		End Function

		'* 
		' * The message property (read-only). This property contains 
		' * the detailed exception error message. 
		' 

		Public Overloads Overrides ReadOnly Property Message() As String
			Get
				Dim buffer As New StringBuilder()

				Select Case m_type
					Case ErrorType.INVALID_PARSER
						buffer.Append("parser is invalid, as ")
						buffer.Append(m_info)
						Exit Select
					Case ErrorType.INVALID_TOKEN
						buffer.Append("token '")
						buffer.Append(m_name)
						buffer.Append("' is invalid, as ")
						buffer.Append(m_info)
						Exit Select
					Case ErrorType.INVALID_PRODUCTION
						buffer.Append("production '")
						buffer.Append(m_name)
						buffer.Append("' is invalid, as ")
						buffer.Append(m_info)
						Exit Select
					Case ErrorType.INFINITE_LOOP
						buffer.Append("infinite loop found in production pattern '")
						buffer.Append(m_name)
						buffer.Append("'")
						Exit Select
					Case ErrorType.INHERENT_AMBIGUITY
						buffer.Append("inherent ambiguity in production '")
						buffer.Append(m_name)
						buffer.Append("'")
						If m_info IsNot Nothing Then
							buffer.Append(" ")
							buffer.Append(m_info)
						End If
						If m_details IsNot Nothing Then
							buffer.Append(" starting with ")
							If m_details.Count > 1 Then
								buffer.Append("tokens ")
							Else
								buffer.Append("token ")
							End If
							buffer.Append(Details)
						End If
						Exit Select
					Case Else
						buffer.Append("internal error")
						Exit Select
				End Select
				Return buffer.ToString()
			End Get
		End Property

		'* 
		' * Returns the error message. This message will contain all the 
		' * information available. 
		' * 
		' * @return the error message 
		' * 
		' * @see #Message 
		' * 
		' * @deprecated Use the Message property instead. 
		' 

		Public Function GetMessage() As String
			Return Message
		End Function
	End Class
End Namespace 