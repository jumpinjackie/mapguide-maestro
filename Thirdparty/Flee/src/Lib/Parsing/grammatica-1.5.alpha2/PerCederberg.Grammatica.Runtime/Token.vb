' 
' * Token.cs 
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

Imports System.Text 

Namespace PerCederberg.Grammatica.Runtime 
    
    '* 
' * A token node. This class represents a token (i.e. a set of adjacent 
' * characters) in a parse tree. The tokens are created by a tokenizer, 
' * that groups characters together into tokens according to a set of 
' * token patterns. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
	Friend Class Token
		Inherits Node

		'* 
		' * The token pattern used for this token. 
		' 

		Private m_pattern As TokenPattern

		'* 
		' * The characters that constitute this token. This is normally 
		' * referred to as the token image. 
		' 

		Private m_image As String

		'* 
		' * The line number of the first character in the token image. 
		' 

		Private m_startLine As Integer

		'* 
		' * The column number of the first character in the token image. 
		' 

		Private m_startColumn As Integer

		'* 
		' * The line number of the last character in the token image. 
		' 

		Private m_endLine As Integer

		'* 
		' * The column number of the last character in the token image. 
		' 

		Private m_endColumn As Integer

		'* 
		' * The previous token in the list of tokens. 
		' 

		Private m_previous As Token

		'* 
		' * The next token in the list of tokens. 
		' 

		Private m_next As Token

		'* 
		' * Creates a new token. 
		' * 
		' * @param pattern the token pattern 
		' * @param image the token image (i.e. characters) 
		' * @param line the line number of the first character 
		' * @param col the column number of the first character 
		' 

		Public Sub New(ByVal pattern As TokenPattern, ByVal image As String, ByVal line As Integer, ByVal col As Integer)
			Me.m_pattern = pattern
			Me.m_image = image
			Me.m_startLine = line
			Me.m_startColumn = col
			Me.m_endLine = line
			Me.m_endColumn = col + image.Length - 1
			Dim pos As Integer = 0
			While image.IndexOf(Chr(10), pos) >= 0
				pos = image.IndexOf(Chr(10), pos) + 1
				Me.m_endLine += 1
				m_endColumn = image.Length - pos
			End While
		End Sub

		'* 
		' * The node type id property (read-only). This value is set as 
		' * a unique identifier for each type of node, in order to 
		' * simplify later identification. 
		' * 
		' * @since 1.5 
		' 

		Public Overloads Overrides ReadOnly Property Id() As Integer
			Get
				Return m_pattern.Id
			End Get
		End Property

		'* 
		' * The node name property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public Overloads Overrides ReadOnly Property Name() As String
			Get
				Return m_pattern.Name
			End Get
		End Property

		'* 
		' * The line number property of the first character in this 
		' * node (read-only). If the node has child elements, this 
		' * value will be fetched from the first child. 
		' * 
		' * @since 1.5 
		' 

		Public Overloads Overrides ReadOnly Property StartLine() As Integer
			Get
				Return m_startLine
			End Get
		End Property

		'* 
		' * The column number property of the first character in this 
		' * node (read-only). If the node has child elements, this 
		' * value will be fetched from the first child. 
		' * 
		' * @since 1.5 
		' 

		Public Overloads Overrides ReadOnly Property StartColumn() As Integer
			Get
				Return m_startColumn
			End Get
		End Property

		'* 
		' * The line number property of the last character in this node 
		' * (read-only). If the node has child elements, this value 
		' * will be fetched from the last child. 
		' * 
		' * @since 1.5 
		' 

		Public Overloads Overrides ReadOnly Property EndLine() As Integer
			Get
				Return m_endLine
			End Get
		End Property

		'* 
		' * The column number property of the last character in this 
		' * node (read-only). If the node has child elements, this 
		' * value will be fetched from the last child. 
		' * 
		' * @since 1.5 
		' 

		Public Overloads Overrides ReadOnly Property EndColumn() As Integer
			Get
				Return m_endColumn
			End Get
		End Property

		'* 
		' * The token image property (read-only). The token image 
		' * consists of the input characters matched to form this 
		' * token. 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Image() As String
			Get
				Return m_image
			End Get
		End Property

		'* 
		' * Returns the token image. The token image consists of the 
		' * input characters matched to form this token. 
		' * 
		' * @return the token image 
		' * 
		' * @see #Image 
		' * 
		' * @deprecated Use the Image property instead. 
		' 

		Public Function GetImage() As String
			Return Image
		End Function

		'* 
		' * The token pattern property (read-only). 
		' 

		Friend ReadOnly Property Pattern() As TokenPattern
			Get
				Return m_pattern
			End Get
		End Property

		'* 
		' * The previous token property. If the token list feature is 
		' * used in the tokenizer, all tokens found will be chained 
		' * together in a double-linked list. The previous token may be 
		' * a token that was ignored during the parsing, due to it's 
		' * ignore flag being set. If there is no previous token or if 
		' * the token list feature wasn't used in the tokenizer (the 
		' * default), the previous token will always be null. 
		' * 
		' * @see #Next 
		' * @see Tokenizer#UseTokenList 
		' * 
		' * @since 1.5 
		' 

		Public Property Previous() As Token
			Get
				Return m_previous
			End Get
			Set(ByVal value As Token)
				If m_previous IsNot Nothing Then
					m_previous.[Next] = Nothing
				End If
				m_previous = Value
				If m_previous IsNot Nothing Then
					m_previous.[Next] = Me
				End If
			End Set
		End Property

		'* 
		' * Returns the previous token. The previous token may be a token 
		' * that has been ignored in the parsing. Note that if the token 
		' * list feature hasn't been used in the tokenizer, this method 
		' * will always return null. By default the token list feature is 
		' * not used. 
		' * 
		' * @return the previous token, or 
		' * null if no such token is available 
		' * 
		' * @see #Previous 
		' * @see #GetNextToken 
		' * @see Tokenizer#UseTokenList 
		' * 
		' * @since 1.4 
		' * 
		' * @deprecated Use the Previous property instead. 
		' 

		Public Function GetPreviousToken() As Token
			Return Previous
		End Function

		'* 
		' * The next token property. If the token list feature is used 
		' * in the tokenizer, all tokens found will be chained together 
		' * in a double-linked list. The next token may be a token that 
		' * was ignored during the parsing, due to it's ignore flag 
		' * being set. If there is no next token or if the token list 
		' * feature wasn't used in the tokenizer (the default), the 
		' * next token will always be null. 
		' * 
		' * @see #Previous 
		' * @see Tokenizer#UseTokenList 
		' * 
		' * @since 1.5 
		' 

		Public Property [Next]() As Token
			Get
				Return m_next
			End Get
			Set(ByVal value As Token)
				If m_next IsNot Nothing Then
					m_next.Previous = Nothing
				End If
				m_next = Value
				If m_next IsNot Nothing Then
					m_next.Previous = Me
				End If
			End Set
		End Property

		'* 
		' * Returns the next token. The next token may be a token that has 
		' * been ignored in the parsing. Note that if the token list 
		' * feature hasn't been used in the tokenizer, this method will 
		' * always return null. By default the token list feature is not 
		' * used. 
		' * 
		' * @return the next token, or 
		' * null if no such token is available 
		' * 
		' * @see #Next 
		' * @see #GetPreviousToken 
		' * @see Tokenizer#UseTokenList 
		' * 
		' * @since 1.4 
		' * 
		' * @deprecated Use the Next property instead. 
		' 

		Public Function GetNextToken() As Token
			Return [Next]
		End Function

		'* 
		' * Returns a string representation of this token. 
		' * 
		' * @return a string representation of this token 
		' 

		Public Overloads Overrides Function ToString() As String
			Dim buffer As New StringBuilder()
			Dim newline As Integer = m_image.IndexOf(Chr(10))

			buffer.Append(m_pattern.Name)
			buffer.Append("(")
			buffer.Append(m_pattern.Id)
			buffer.Append("): """)
			If newline >= 0 Then
				If newline > 0 AndAlso m_image(newline - 1) = Chr(13) Then
					newline -= 1
				End If
				buffer.Append(m_image.Substring(0, newline))
				buffer.Append("(...)")
			Else
				buffer.Append(m_image)
			End If
			buffer.Append(""", line: ")
			buffer.Append(m_startLine)
			buffer.Append(", col: ")
			buffer.Append(m_startColumn)

			Return buffer.ToString()
		End Function

		'* 
		' * Returns a short string representation of this token. The 
		' * string will only contain the token image and possibly the 
		' * token pattern name. 
		' * 
		' * @return a short string representation of this token 
		' 

		Public Function ToShortString() As String
			Dim buffer As New StringBuilder()
			Dim newline As Integer = m_image.IndexOf(Chr(10))

			buffer.Append(""""c)
			If newline >= 0 Then
				If newline > 0 AndAlso m_image(newline - 1) = Chr(13) Then
					newline -= 1
				End If
				buffer.Append(m_image.Substring(0, newline))
				buffer.Append("(...)")
			Else
				buffer.Append(m_image)
			End If
			buffer.Append(""""c)
			If m_pattern.Type = TokenPattern.PatternType.REGEXP Then
				buffer.Append(" <")
				buffer.Append(m_pattern.Name)
				buffer.Append(">")
			End If

			Return buffer.ToString()
		End Function
	End Class
End Namespace 