' 
' * TokenPattern.cs 
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

Namespace PerCederberg.Grammatica.Runtime 
    
    '* 
' * A token pattern. This class contains the definition of a token 
' * (i.e. it's pattern), and allows testing a string against this 
' * pattern. A token pattern is uniquely identified by an integer id, 
' * that must be provided upon creation. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
	Friend Class TokenPattern

		'* 
		' * The pattern type enumeration. 
		' 

		Public Enum PatternType

			'* 
			' * The string pattern type is used for tokens that only 
			' * match an exact string. 
			' 

			[STRING]

			'* 
			' * The regular expression pattern type is used for tokens 
			' * that match a regular expression. 
			' 

			REGEXP
		End Enum

		'* 
		' * The token pattern identity. 
		' 

		Private m_id As Integer

		'* 
		' * The token pattern name. 
		' 

		Private m_name As String

		'* 
		' * The token pattern type. 
		' 

		Private m_type As PatternType

		'* 
		' * The token pattern. 
		' 

		Private m_pattern As String

		'* 
		' * The token error flag. If this flag is set, it means that an 
		' * error should be reported if the token is found. The error 
		' * message is present in the errorMessage variable. 
		' * 
		' * @see #errorMessage 
		' 

		Private m_error As Boolean

		'* 
		' * The token error message. This message will only be set if the 
		' * token error flag is set. 
		' * 
		' * @see #error 
		' 

		Private m_errorMessage As String

		'* 
		' * The token ignore flag. If this flag is set, it means that the 
		' * token should be ignored if found. If an ignore message is 
		' * present in the ignoreMessage variable, it will also be reported 
		' * as a warning. 
		' * 
		' * @see #ignoreMessage 
		' 

		Private m_ignore As Boolean

		'* 
		' * The token ignore message. If this message is set when the token 
		' * ignore flag is also set, a warning message will be printed if 
		' * the token is found. 
		' * 
		' * @see #ignore 
		' 

		Private m_ignoreMessage As String

		'* 
		' * Creates a new token pattern. 
		' * 
		' * @param id the token pattern id 
		' * @param name the token pattern name 
		' * @param type the token pattern type 
		' * @param pattern the token pattern 
		' 

		Protected Sub New()

		End Sub

		Public Sub New(ByVal id As Integer, ByVal name As String, ByVal type As PatternType, ByVal pattern As String)
			Me.SetData(id, name, type, pattern)
		End Sub

		Protected Sub SetData(ByVal id As Integer, ByVal name As String, ByVal type As PatternType, ByVal pattern As String)
			Me.m_id = id
			Me.m_name = name
			Me.m_type = type
			Me.m_pattern = pattern
		End Sub

		'* 
		' * The token pattern identity property (read-only). This 
		' * property contains the unique token pattern identity value. 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Id() As Integer
			Get
				Return m_id
			End Get
		End Property

		'* 
		' * Returns the unique token pattern identity value. 
		' * 
		' * @return the token pattern id 
		' * 
		' * @see #Id 
		' * 
		' * @deprecated Use the Id property instead. 
		' 

		Public Function GetId() As Integer
			Return m_id
		End Function

		'* 
		' * The token pattern name property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Name() As String
			Get
				Return m_name
			End Get
		End Property

		'* 
		' * Returns the token pattern name. 
		' * 
		' * @return the token pattern name 
		' * 
		' * @see #Name 
		' * 
		' * @deprecated Use the Name property instead. 
		' 

		Public Function GetName() As String
			Return m_name
		End Function

		'* 
		' * The token pattern type property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Type() As PatternType
			Get
				Return m_type
			End Get
		End Property

		'* 
		' * Returns the token pattern type. 
		' * 
		' * @return the token pattern type 
		' * 
		' * @see #Type 
		' * 
		' * @deprecated Use the Type property instead. 
		' 

		Public Function GetPatternType() As PatternType
			Return m_type
		End Function

		'* 
		' * The token pattern property (read-only). This property 
		' * contains the actual pattern (string or regexp) which have 
		' * to be matched. 
		' * 
		' * @since 1.5 
		' 

		Public Property Pattern() As String
			Get
				Return m_pattern
			End Get
			Set(ByVal value As String)
				Me.m_pattern = value
			End Set
		End Property

		'* 
		' * Returns te token pattern. 
		' * 
		' * @return the token pattern 
		' * 
		' * @see #Pattern 
		' * 
		' * @deprecated Use the Pattern property instead. 
		' 

		Public Function GetPattern() As String
			Return m_pattern
		End Function

		'* 
		' * The error flag property. If this property is true, the 
		' * token pattern corresponds to an error token and an error 
		' * should be reported if a match is found. When setting this 
		' * property to true, a default error message is created if 
		' * none was previously set. 
		' * 
		' * @since 1.5 
		' 

		Public Property [Error]() As Boolean
			Get
				Return m_error
			End Get
			Set(ByVal value As Boolean)
				m_error = Value
				If m_error AndAlso m_errorMessage Is Nothing Then
					m_errorMessage = "unrecognized token found"
				End If
			End Set
		End Property

		'* 
		' * The token error message property. The error message is 
		' * printed whenever the token is matched. Setting the error 
		' * message property also sets the error flag to true. 
		' * 
		' * @see #Error 
		' * 
		' * @since 1.5 
		' 

		Public Property ErrorMessage() As String
			Get
				Return m_errorMessage
			End Get
			Set(ByVal value As String)
				m_error = True
				m_errorMessage = Value
			End Set
		End Property

		'* 
		' * Checks if the pattern corresponds to an error token. If this 
		' * is true, it means that an error should be reported if a 
		' * matching token is found. 
		' * 
		' * @return true if the pattern maps to an error token, or 
		' * false otherwise 
		' * 
		' * @see #Error 
		' * 
		' * @deprecated Use the Error property instead. 
		' 

		Public Function IsError() As Boolean
			Return [Error]
		End Function

		'* 
		' * Returns the token error message if the pattern corresponds to 
		' * an error token. 
		' * 
		' * @return the token error message 
		' * 
		' * @see #ErrorMessage 
		' * 
		' * @deprecated Use the ErrorMessage property instead. 
		' 

		Public Function GetErrorMessage() As String
			Return ErrorMessage
		End Function

		'* 
		' * Sets the token error flag and assigns a default error message. 
		' * 
		' * @see #Error 
		' * 
		' * @deprecated Use the Error property instead. 
		' 

		Public Sub SetError()
			[Error] = True
		End Sub

		'* 
		' * Sets the token error flag and assigns the specified error 
		' * message. 
		' * 
		' * @param message the error message to display 
		' * 
		' * @see #ErrorMessage 
		' * 
		' * @deprecated Use the ErrorMessage property instead. 
		' 

		Public Sub SetError(ByVal message As String)
			ErrorMessage = message
		End Sub

		'* 
		' * The ignore flag property. If this property is true, the 
		' * token pattern corresponds to an ignore token and should be 
		' * skipped if a match is found. 
		' * 
		' * @since 1.5 
		' 

		Public Property Ignore() As Boolean
			Get
				Return m_ignore
			End Get
			Set(ByVal value As Boolean)
				m_ignore = Value
			End Set
		End Property

		'* 
		' * The token ignore message property. The ignore message is 
		' * printed whenever the token is matched. Setting the ignore 
		' * message property also sets the ignore flag to true. 
		' * 
		' * @see #Ignore 
		' * 
		' * @since 1.5 
		' 

		Public Property IgnoreMessage() As String
			Get
				Return m_ignoreMessage
			End Get
			Set(ByVal value As String)
				m_ignore = True
				m_ignoreMessage = Value
			End Set
		End Property

		'* 
		' * Checks if the pattern corresponds to an ignored token. If this 
		' * is true, it means that the token should be ignored if found. 
		' * 
		' * @return true if the pattern maps to an ignored token, or 
		' * false otherwise 
		' * 
		' * @see #Ignore 
		' * 
		' * @deprecated Use the Ignore property instead. 
		' 

		Public Function IsIgnore() As Boolean
			Return Ignore
		End Function

		'* 
		' * Returns the token ignore message if the pattern corresponds to 
		' * an ignored token. 
		' * 
		' * @return the token ignore message 
		' * 
		' * @see #IgnoreMessage 
		' * 
		' * @deprecated Use the IgnoreMessage property instead. 
		' 

		Public Function GetIgnoreMessage() As String
			Return IgnoreMessage
		End Function

		'* 
		' * Sets the token ignore flag and clears the ignore message. 
		' * 
		' * @see #Ignore 
		' * 
		' * @deprecated Use the Ignore property instead. 
		' 

		Public Sub SetIgnore()
			Ignore = True
		End Sub

		'* 
		' * Sets the token ignore flag and assigns the specified ignore 
		' * message. 
		' * 
		' * @param message the ignore message to display 
		' * 
		' * @see #IgnoreMessage 
		' * 
		' * @deprecated Use the IgnoreMessage property instead. 
		' 

		Public Sub SetIgnore(ByVal message As String)
			IgnoreMessage = message
		End Sub

		'* 
		' * Returns a string representation of this object. 
		' * 
		' * @return a token pattern string representation 
		' 

		Public Overloads Overrides Function ToString() As String
			Dim buffer As New StringBuilder()

			buffer.Append(m_name)
			buffer.Append(" (")
			buffer.Append(m_id)
			buffer.Append("): ")
			Select Case m_type
				Case PatternType.[STRING]
					buffer.Append("""")
					buffer.Append(m_pattern)
					buffer.Append("""")
					Exit Select
				Case PatternType.REGEXP
					buffer.Append("<<")
					buffer.Append(m_pattern)
					buffer.Append(">>")
					Exit Select
			End Select
			If m_error Then
				buffer.Append(" ERROR: """)
				buffer.Append(m_errorMessage)
				buffer.Append("""")
			End If
			If m_ignore Then
				buffer.Append(" IGNORE")
				If m_ignoreMessage IsNot Nothing Then
					buffer.Append(": """)
					buffer.Append(m_ignoreMessage)
					buffer.Append("""")
				End If
			End If

			Return buffer.ToString()
		End Function

		'* 
		' * Returns a short string representation of this object. 
		' * 
		' * @return a short string representation of this object 
		' 

		Public Function ToShortString() As String
			Dim buffer As New StringBuilder()
			Dim newline As Integer = m_pattern.IndexOf(Chr(10))

			If m_type = PatternType.[STRING] Then
				buffer.Append("""")
				If newline >= 0 Then
					If newline > 0 AndAlso m_pattern(newline - 1) = Chr(13) Then
						newline -= 1
					End If
					buffer.Append(m_pattern.Substring(0, newline))
					buffer.Append("(...)")
				Else
					buffer.Append(m_pattern)
				End If
				buffer.Append("""")
			Else
				buffer.Append("<")
				buffer.Append(m_name)
				buffer.Append(">")
			End If

			Return buffer.ToString()
		End Function
	End Class
End Namespace 