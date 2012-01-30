' 
' * Tokenizer.cs 
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

Imports System.Collections 
Imports System.IO 
Imports System.Text 
Imports Ciloci.Flee.PerCederberg.Grammatica.Runtime.RE

Namespace PerCederberg.Grammatica.Runtime 
    
    '* 
' * A character stream tokenizer. This class groups the characters read 
' * from the stream together into tokens ("words"). The grouping is 
' * controlled by token patterns that contain either a fixed string to 
' * search for, or a regular expression. If the stream of characters 
' * don't match any of the token patterns, a parse exception is thrown. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
	Friend Class Tokenizer

		'* 
		' * The ignore character case flag. 
		' 

		Private ignoreCase As Boolean = False

		'* 
		' * The token list feature flag. 
		' 

		Private m_useTokenList As Boolean

		'* 
		' * The string token matcher. This token matcher is used for all 
		' * string token patterns. This matcher implements a DFA to 
		' * provide maximum performance. 
		' 

		Private stringMatcher As StringTokenMatcher

		'* 
		' * The list of all regular expression token matchers. These 
		' * matchers each test matches for a single regular expression. 
		' 

		Private regexpMatchers As New ArrayList()

		'* 
		' * The look-ahead character stream reader. 
		' 

		Private input As LookAheadReader

		'* 
		' * The previous token in the token list. 
		' 

		Private previousToken As Token

		'* 
		' * Creates a new case-sensitive tokenizer for the specified 
		' * input stream. 
		' * 
		' * @param input the input stream to read 
		' 

		Public Sub New(ByVal input As TextReader)
			Me.New(input, False)
		End Sub

		'* 
		' * Creates a new tokenizer for the specified input stream. The 
		' * tokenizer can be set to process tokens either in 
		' * case-sensitive or case-insensitive mode. 
		' * 
		' * @param input the input stream to read 
		' * @param ignoreCase the character case ignore flag 
		' * 
		' * @since 1.5 
		' 

		Public Sub New(ByVal input As TextReader, ByVal ignoreCase As Boolean)
			Me.stringMatcher = New StringTokenMatcher(ignoreCase)
			Me.input = New LookAheadReader(input)
			Me.ignoreCase = ignoreCase
		End Sub

		'* 
		' * The token list flag property. If the token list flag is 
		' * set, all tokens (including ignored tokens) link to each 
		' * other in a double-linked list. By default the token list 
		' * flag is set to false. 
		' * 
		' * @see Token#Previous 
		' * @see Token#Next 
		' * 
		' * @since 1.5 
		' 

		Public Property UseTokenList() As Boolean
			Get
				Return m_useTokenList
			End Get
			Set(ByVal value As Boolean)
				m_useTokenList = Value
			End Set
		End Property

		'* 
		' * Checks if the token list feature is used. The token list 
		' * feature makes all tokens (including ignored tokens) link to 
		' * each other in a linked list. By default the token list feature 
		' * is not used. 
		' * 
		' * @return true if the token list feature is used, or 
		' * false otherwise 
		' * 
		' * @see #UseTokenList 
		' * @see #SetUseTokenList 
		' * @see Token#GetPreviousToken 
		' * @see Token#GetNextToken 
		' * 
		' * @since 1.4 
		' * 
		' * @deprecated Use the UseTokenList property instead. 
		' 

		Public Function GetUseTokenList() As Boolean
			Return m_useTokenList
		End Function

		'* 
		' * Sets the token list feature flag. The token list feature makes 
		' * all tokens (including ignored tokens) link to each other in a 
		' * linked list when active. By default the token list feature is 
		' * not used. 
		' * 
		' * @param useTokenList the token list feature flag 
		' * 
		' * @see #UseTokenList 
		' * @see #GetUseTokenList 
		' * @see Token#GetPreviousToken 
		' * @see Token#GetNextToken 
		' * 
		' * @since 1.4 
		' * 
		' * @deprecated Use the UseTokenList property instead. 
		' 

		Public Sub SetUseTokenList(ByVal useTokenList As Boolean)
			Me.m_useTokenList = useTokenList
		End Sub

		'* 
		' * Returns a description of the token pattern with the 
		' * specified id. 
		' * 
		' * @param id the token pattern id 
		' * 
		' * @return the token pattern description, or 
		' * null if not present 
		' 

		Public Function GetPatternDescription(ByVal id As Integer) As String
			Dim pattern As TokenPattern
			Dim re As RegExpTokenMatcher

			pattern = stringMatcher.GetPattern(id)
			If pattern IsNot Nothing Then
				Return pattern.ToShortString()
			End If
			For i As Integer = 0 To regexpMatchers.Count - 1
				re = DirectCast(regexpMatchers(i), RegExpTokenMatcher)
				If re.GetPattern().Id = id Then
					Return re.GetPattern().ToShortString()
				End If
			Next
			Return Nothing
		End Function

		'* 
		' * Returns the current line number. This number will be the line 
		' * number of the next token returned. 
		' * 
		' * @return the current line number 
		' 

		Public Function GetCurrentLine() As Integer
			Return input.LineNumber
		End Function

		'* 
		' * Returns the current column number. This number will be the 
		' * column number of the next token returned. 
		' * 
		' * @return the current column number 
		' 

		Public Function GetCurrentColumn() As Integer
			Return input.ColumnNumber
		End Function

		'* 
		' * Adds a new token pattern to the tokenizer. The pattern will be 
		' * added last in the list, choosing a previous token pattern in 
		' * case two matches the same string. 
		' * 
		' * @param pattern the pattern to add 
		' * 
		' * @throws ParserCreationException if the pattern couldn't be 
		' * added to the tokenizer 
		' 

		Public Sub AddPattern(ByVal pattern As TokenPattern)
			Select Case pattern.Type
				Case TokenPattern.PatternType.[STRING]
					stringMatcher.AddPattern(pattern)
					Exit Select
				Case TokenPattern.PatternType.REGEXP
					Try
						regexpMatchers.Add(New RegExpTokenMatcher(pattern, ignoreCase, input))
					Catch e As RegExpException
						Throw New ParserCreationException(ParserCreationException.ErrorType.INVALID_TOKEN, pattern.Name, "regular expression contains error(s): " + e.Message)
					End Try
					Exit Select
				Case Else
					Throw New ParserCreationException(ParserCreationException.ErrorType.INVALID_TOKEN, pattern.Name, "pattern type " + pattern.Type + " is undefined")
			End Select
		End Sub

		'* 
		' * Resets this tokenizer for usage with another input stream. 
		' * This method will clear all the internal state in the 
		' * tokenizer as well as close the previous input stream. It is 
		' * normally called in order to reuse a parser and tokenizer 
		' * pair for parsing another input stream. 
		' * 
		' * @param input the new input stream to read 
		' * 
		' * @since 1.5 
		' 

		Public Sub Reset(ByVal input As TextReader)
			Me.input.Close()
			Me.input = New LookAheadReader(input)
			Me.previousToken = Nothing
			stringMatcher.Reset()
			For i As Integer = 0 To regexpMatchers.Count - 1
				DirectCast(regexpMatchers(i), RegExpTokenMatcher).Reset(Me.input)
			Next
		End Sub

		'* 
		' * Finds the next token on the stream. This method will return 
		' * null when end of file has been reached. It will return a 
		' * parse exception if no token matched the input stream, or if 
		' * a token pattern with the error flag set matched. Any tokens 
		' * matching a token pattern with the ignore flag set will be 
		' * silently ignored and the next token will be returned. 
		' * 
		' * @return the next token found, or 
		' * null if end of file was encountered 
		' * 
		' * @throws ParseException if the input stream couldn't be read or 
		' * parsed correctly 
		' 

		Public Function [Next]() As Token
			Dim token As Token = Nothing

			Do
				token = NextToken()
				If m_useTokenList AndAlso token IsNot Nothing Then
					token.Previous = previousToken
					previousToken = token
				End If
				If token Is Nothing Then
					Return Nothing
				ElseIf token.Pattern.[Error] Then
					Throw New ParseException(ParseException.ErrorType.INVALID_TOKEN, token.Pattern.ErrorMessage, token.StartLine, token.StartColumn)
				ElseIf token.Pattern.Ignore Then
					token = Nothing
				End If
			Loop While token Is Nothing

			Return token
		End Function

		'* 
		' * Finds the next token on the stream. This method will return 
		' * null when end of file has been reached. It will return a 
		' * parse exception if no token matched the input stream. 
		' * 
		' * @return the next token found, or 
		' * null if end of file was encountered 
		' * 
		' * @throws ParseException if the input stream couldn't be read or 
		' * parsed correctly 
		' 

		Private Function NextToken() As Token
			Dim m As TokenMatcher
			Dim str As String
			Dim line As Integer
			Dim column As Integer

			Try
				m = FindMatch()
				If m IsNot Nothing Then
					line = input.LineNumber
					column = input.ColumnNumber
					str = input.ReadString(m.GetMatchedLength())
					Return New Token(m.GetMatchedPattern(), str, line, column)
				ElseIf input.Peek() < 0 Then
					Return Nothing
				Else
					line = input.LineNumber
					column = input.ColumnNumber
					Throw New ParseException(ParseException.ErrorType.UNEXPECTED_CHAR, input.ReadString(1), line, column)
				End If
			Catch e As IOException
				Throw New ParseException(ParseException.ErrorType.IO, e.Message, -1, -1)
			End Try
		End Function

		'* 
		' * Finds the longest token match from the current buffer 
		' * position. This method will return the token matcher for the 
		' * best match, or null if no match was found. As a side 
		' * effect, this method will also set the end of buffer flag. 
		' * 
		' * @return the token mathcher with the longest match, or 
		' * null if no match was found 
		' * 
		' * @throws IOException if an I/O error occurred 
		' 

		Private Function FindMatch() As TokenMatcher
			Dim bestMatch As TokenMatcher = Nothing
			Dim bestLength As Integer = 0
			Dim re As RegExpTokenMatcher

			' Check string matches 
			If stringMatcher.Match(input) Then
				bestMatch = stringMatcher
				bestLength = bestMatch.GetMatchedLength()
			End If
			For i As Integer = 0 To regexpMatchers.Count - 1

				' Check regular expression matches 
				re = DirectCast(regexpMatchers(i), RegExpTokenMatcher)
				If re.Match() AndAlso re.GetMatchedLength() > bestLength Then
					bestMatch = re
					bestLength = re.GetMatchedLength()
				End If
			Next
			Return bestMatch
		End Function

		'* 
		' * Returns a string representation of this object. The returned 
		' * string will contain the details of all the token patterns 
		' * contained in this tokenizer. 
		' * 
		' * @return a detailed string representation 
		' 

		Public Overloads Overrides Function ToString() As String
			Dim buffer As New StringBuilder()

			buffer.Append(stringMatcher)
			For i As Integer = 0 To regexpMatchers.Count - 1
				buffer.Append(regexpMatchers(i))
			Next
			Return buffer.ToString()
		End Function
	End Class
    
    
    '* 
' * A token pattern matcher. This class is the base class for the 
' * two types of token matchers that exist. The token matcher 
' * checks for matches with the tokenizer buffer, and maintains the 
' * state of the last match. 
' 
    
    Friend MustInherit Class TokenMatcher 
        
        '* 
' * Returns the latest matched token pattern. 
' * 
' * @return the latest matched token pattern, or 
' * null if no match found 
' 
        
        Public MustOverride Function GetMatchedPattern() As TokenPattern 
        
        '* 
' * Returns the length of the latest match. 
' * 
' * @return the length of the latest match, or 
' * zero (0) if no match found 
' 
        
        Public MustOverride Function GetMatchedLength() As Integer 
    End Class 
    
    
    '* 
' * A regular expression token pattern matcher. This class is used 
' * to match a single regular expression with an input stream. This 
' * class also maintains the state of the last match. 
' 
    
    Friend Class RegExpTokenMatcher 
        Inherits TokenMatcher 
        
        '* 
' * The token pattern to match with. 
' 
        
        Private pattern As TokenPattern 
        
        '* 
' * The regular expression to use. 
' 
        
        Private regExp As RegExp 
        
        '* 
' * The regular expression matcher to use. 
' 
        
		Private matcher As Matcher
        
        '* 
' * Creates a new regular expression token matcher. 
' * 
' * @param pattern the pattern to match 
' * @param ignoreCase the character case ignore flag 
' * @param input the input stream to check 
' * 
' * @throws RegExpException if the regular expression couldn't 
' * be created properly 
' 
        
        Public Sub New(ByVal pattern As TokenPattern, ByVal ignoreCase As Boolean, ByVal input As LookAheadReader) 
            
            Me.pattern = pattern 
            Me.regExp = New RegExp(pattern.Pattern, ignoreCase) 
            Me.matcher = regExp.Matcher(input) 
        End Sub 
        
        '* 
' * Resets the matcher for another character input stream. This 
' * will clear the results of the last match. 
' * 
' * @param input the new input stream to check 
' 
        
        Public Sub Reset(ByVal input As LookAheadReader) 
            matcher.Reset(input) 
        End Sub 
        
        '* 
' * Returns the token pattern. 
' * 
' * @return the token pattern 
' 
        
        Public Function GetPattern() As TokenPattern 
            Return pattern 
        End Function 
        
        '* 
' * Returns the latest matched token pattern. 
' * 
' * @return the latest matched token pattern, or 
' * null if no match found 
' 
        
        Public Overloads Overrides Function GetMatchedPattern() As TokenPattern 
            If matcher Is Nothing OrElse matcher.Length() <= 0 Then 
                Return Nothing 
            Else 
                Return pattern 
            End If 
        End Function 
        
        '* 
' * Returns the length of the latest match. 
' * 
' * @return the length of the latest match, or 
' * zero (0) if no match found 
' 
        
        Public Overloads Overrides Function GetMatchedLength() As Integer 
            Return IIf((matcher Is Nothing),0,matcher.Length()) 
        End Function 
        
        '* 
' * Checks if the token pattern matches the input stream. This 
' * method will also reset all flags in this matcher. 
' * 
' * @param str the string to match 
' * @param pos the starting position 
' * 
' * @return true if a match was found, or 
' * false otherwise 
' * 
' * @throws IOException if an I/O error occurred 
' 
        
        Public Function Match() As Boolean 
            Return matcher.MatchFromBeginning() 
        End Function 
        
        '* 
' * Returns a string representation of this token matcher. 
' * 
' * @return a detailed string representation of this matcher 
' 
        
        Public Overloads Overrides Function ToString() As String 
            Return pattern.ToString() + "" & Chr(10) & "" + regExp.ToString() + "" & Chr(10) & "" 
        End Function 
    End Class 
    
    
    '* 
' * A string token pattern matcher. This class is used to match a 
' * set of strings with an input stream. This class internally uses 
' * a DFA for maximum performance. It also maintains the state of 
' * the last match. 
' 
    
    Friend Class StringTokenMatcher 
        Inherits TokenMatcher 
        
        '* 
' * The list of string token patterns. 
' 
        
        Private patterns As New ArrayList() 
        
        '* 
' * The finite automaton to use for matching. 
' 
        
        Private start As New Automaton() 
        
        '* 
' * The last token pattern match found. 
' 
        
		Private m_match As TokenPattern

		'* 
		' * The ignore character case flag. 
		' 

		Private ignoreCase As Boolean = False

		'* 
		' * Creates a new string token matcher. 
		' * 
		' * @param ignoreCase the character case ignore flag 
		' 

		Public Sub New(ByVal ignoreCase As Boolean)
			Me.ignoreCase = ignoreCase
		End Sub

		'* 
		' * Resets the matcher state. This will clear the results of 
		' * the last match. 
		' 

		Public Sub Reset()
			m_match = Nothing
		End Sub

		'* 
		' * Returns the latest matched token pattern. 
		' * 
		' * @return the latest matched token pattern, or 
		' * null if no match found 
		' 

		Public Overloads Overrides Function GetMatchedPattern() As TokenPattern
			Return m_match
		End Function

		'* 
		' * Returns the length of the latest match. 
		' * 
		' * @return the length of the latest match, or 
		' * zero (0) if no match found 
		' 

		Public Overloads Overrides Function GetMatchedLength() As Integer
			If m_match Is Nothing Then
				Return 0
			Else
				Return m_match.Pattern.Length
			End If
		End Function
        
        '* 
' * Returns the token pattern with the specified id. Only 
' * token patterns handled by this matcher can be returned. 
' * 
' * @param id the token pattern id 
' * 
' * @return the token pattern found, or 
' * null if not found 
' 
        
        Public Function GetPattern(ByVal id As Integer) As TokenPattern 
            Dim pattern As TokenPattern 
            For i As Integer = 0 To patterns.Count - 1 
                
                pattern = DirectCast(patterns(i), TokenPattern) 
                If pattern.Id = id Then 
                    Return pattern 
                End If 
            Next 
            Return Nothing 
        End Function 
        
        '* 
' * Adds a string token pattern to this matcher. 
' * 
' * @param pattern the pattern to add 
' 
        
        Public Sub AddPattern(ByVal pattern As TokenPattern) 
            patterns.Add(pattern) 
            start.AddMatch(pattern.Pattern, ignoreCase, pattern) 
        End Sub 
        
        '* 
' * Checks if the token pattern matches the input stream. This 
' * method will also reset all flags in this matcher. 
' * 
' * @param input the input stream to match 
' * 
' * @return true if a match was found, or 
' * false otherwise 
' * 
' * @throws IOException if an I/O error occurred 
' 
        
        Public Function Match(ByVal input As LookAheadReader) As Boolean 
            Reset() 
			m_match = DirectCast(start.MatchFrom(input, 0, ignoreCase), TokenPattern)
			Return m_match IsNot Nothing
        End Function 
        
        '* 
' * Returns a string representation of this matcher. This will 
' * contain all the token patterns. 
' * 
' * @return a detailed string representation of this matcher 
' 
        
        Public Overloads Overrides Function ToString() As String 
            Dim buffer As New StringBuilder() 
            For i As Integer = 0 To patterns.Count - 1 
                
                buffer.Append(patterns(i)) 
                buffer.Append("" & Chr(10) & "" & Chr(10) & "") 
            Next 
            Return buffer.ToString() 
        End Function 
    End Class 
End Namespace 