' 
' * Parser.cs 
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
' * A base parser class. This class provides the standard parser 
' * interface, as well as token handling. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
	Friend MustInherit Class Parser

		'* 
		' * The parser initialization flag. 
		' 

		Private initialized As Boolean

		'* 
		' * The tokenizer to use. 
		' 

		Private m_tokenizer As Tokenizer

		'* 
		' * The analyzer to use for callbacks. 
		' 

		Private m_analyzer As Analyzer

		'* 
		' * The list of production patterns. 
		' 

		Private patterns As New ArrayList()

		'* 
		' * The map with production patterns and their id:s. This map 
		' * contains the production patterns indexed by their id:s. 
		' 

		Private patternIds As New Hashtable()

		'* 
		' * The list of buffered tokens. This list will contain tokens that 
		' * have been read from the tokenizer, but not yet consumed. 
		' 

		Private tokens As New ArrayList()

		'* 
		' * The error log. All parse errors will be added to this log as 
		' * the parser attempts to recover from the error. If the error 
		' * count is higher than zero (0), this log will be thrown as the 
		' * result from the parse() method. 
		' 

		Private errorLog As New ParserLogException()

		'* 
		' * The error recovery counter. This counter is initially set to a 
		' * negative value to indicate that no error requiring recovery 
		' * has been encountered. When a parse error is found, the counter 
		' * is set to three (3), and is then decreased by one for each 
		' * correctly read token until it reaches zero (0). 
		' 

		Private errorRecovery As Integer = -1

		'* 
		' * Creates a new parser. 
		' * 
		' * @param tokenizer the tokenizer to use 
		' 

		Friend Sub New(ByVal tokenizer As Tokenizer)
			Me.New(tokenizer, Nothing)
		End Sub

		'* 
		' * Creates a new parser. 
		' * 
		' * @param tokenizer the tokenizer to use 
		' * @param analyzer the analyzer callback to use 
		' 

		Friend Sub New(ByVal tokenizer As Tokenizer, ByVal analyzer As Analyzer)
			Me.m_tokenizer = tokenizer
			If analyzer Is Nothing Then
				Me.m_analyzer = New Analyzer()
			Else
				Me.m_analyzer = analyzer
			End If
		End Sub

		'* 
		' * The tokenizer property (read-only). This property contains 
		' * the tokenizer in use by this parser. 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Tokenizer() As Tokenizer
			Get
				Return m_tokenizer
			End Get
		End Property

		'* 
		' * The analyzer property (read-only). This property contains 
		' * the analyzer in use by this parser. 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Analyzer() As Analyzer
			Get
				Return m_analyzer
			End Get
		End Property

		'* 
		' * Returns the tokenizer in use by this parser. 
		' * 
		' * @return the tokenizer in use by this parser 
		' * 
		' * @since 1.4 
		' * 
		' * @see #Tokenizer 
		' * 
		' * @deprecated Use the Tokenizer property instead. 
		' 

		Public Function GetTokenizer() As Tokenizer
			Return Tokenizer
		End Function

		'* 
		' * Returns the analyzer in use by this parser. 
		' * 
		' * @return the analyzer in use by this parser 
		' * 
		' * @since 1.4 
		' * 
		' * @see #Analyzer 
		' * 
		' * @deprecated Use the Analyzer property instead. 
		' 

		Public Function GetAnalyzer() As Analyzer
			Return Analyzer
		End Function

		'* 
		' * Sets the parser initialized flag. Normally this flag is set by 
		' * the prepare() method, but this method allows further 
		' * modifications to it. 
		' * 
		' * @param initialized the new initialized flag 
		' 

		Friend Sub SetInitialized(ByVal initialized As Boolean)
			Me.initialized = initialized
		End Sub

		'* 
		' * Adds a new production pattern to the parser. The first pattern 
		' * added is assumed to be the starting point in the grammar. The 
		' * patterns added may be validated to some extent. 
		' * 
		' * @param pattern the pattern to add 
		' * 
		' * @throws ParserCreationException if the pattern couldn't be 
		' * added correctly to the parser 
		' 

		Public Overridable Sub AddPattern(ByVal pattern As ProductionPattern)
			If pattern.Count <= 0 Then
				Throw New ParserCreationException(ParserCreationException.ErrorType.INVALID_PRODUCTION, pattern.Name, "no production alternatives are present (must have at " + "least one)")
			End If
			If patternIds.ContainsKey(pattern.Id) Then
				Throw New ParserCreationException(ParserCreationException.ErrorType.INVALID_PRODUCTION, pattern.Name, "another pattern with the same id (" + pattern.Id + ") has already been added")
			End If
			patterns.Add(pattern)
			patternIds.Add(pattern.Id, pattern)
			SetInitialized(False)
		End Sub

		'* 
		' * Initializes the parser. All the added production patterns will 
		' * be analyzed for ambiguities and errors. This method also 
		' * initializes internal data structures used during the parsing. 
		' * 
		' * @throws ParserCreationException if the parser couldn't be 
		' * initialized correctly 
		' 

		Public Overridable Sub Prepare()
			If patterns.Count <= 0 Then
				Throw New ParserCreationException(ParserCreationException.ErrorType.INVALID_PARSER, "no production patterns have been added")
			End If
			For i As Integer = 0 To patterns.Count - 1
				CheckPattern(DirectCast(patterns(i), ProductionPattern))
			Next
			SetInitialized(True)
		End Sub

		'* 
		' * Checks a production pattern for completeness. If some rule 
		' * in the pattern referenced an production pattern not added 
		' * to this parser, a parser creation exception will be thrown. 
		' * 
		' * @param pattern the production pattern to check 
		' * 
		' * @throws ParserCreationException if the pattern referenced a 
		' * pattern not added to this parser 
		' 

		Private Sub CheckPattern(ByVal pattern As ProductionPattern)
			For i As Integer = 0 To pattern.Count - 1
				CheckAlternative(pattern.Name, pattern(i))
			Next
		End Sub

		'* 
		' * Checks a production pattern alternative for completeness. 
		' * If some element in the alternative referenced a production 
		' * pattern not added to this parser, a parser creation 
		' * exception will be thrown. 
		' * 
		' * @param name the name of the pattern being checked 
		' * @param alt the production pattern alternative 
		' * 
		' * @throws ParserCreationException if the alternative 
		' * referenced a pattern not added to this parser 
		' 

		Private Sub CheckAlternative(ByVal name As String, ByVal alt As ProductionPatternAlternative)
			For i As Integer = 0 To alt.Count - 1

				CheckElement(name, alt(i))
			Next
		End Sub

		'* 
		' * Checks a production pattern element for completeness. If 
		' * the element references a production pattern not added to 
		' * this parser, a parser creation exception will be thrown. 
		' * 
		' * @param name the name of the pattern being checked 
		' * @param elem the production pattern element to check 
		' * 
		' * @throws ParserCreationException if the element referenced a 
		' * pattern not added to this parser 
		' 

		Private Sub CheckElement(ByVal name As String, ByVal elem As ProductionPatternElement)

			If elem.IsProduction() AndAlso GetPattern(elem.Id) Is Nothing Then
				Throw New ParserCreationException(ParserCreationException.ErrorType.INVALID_PRODUCTION, name, "an undefined production pattern id (" + elem.Id + ") is referenced")
			End If
		End Sub

		'* 
		' * Resets this parser. This method will clear all the internal 
		' * state and error log in the parser, but will not reset the 
		' * tokenizer. In order to parse multiple input streams with 
		' * the same parser, the Tokenizer.reset() method must also be 
		' * called. 
		' * 
		' * @see Tokenizer#Reset 
		' * 
		' * @since 1.5 
		' 

		Public Sub Reset()
			Me.tokens.Clear()
			Me.errorLog = New ParserLogException()
			Me.errorRecovery = -1
		End Sub

		'* 
		' * Parses the token stream and returns a parse tree. This 
		' * method will call Prepare() if not previously called. It 
		' * will also call the Reset() method, to make sure that only 
		' * the Tokenizer.Reset() method must be explicitly called in 
		' * order to reuse a parser for multiple input streams. In case 
		' * of a parse error, the parser will attempt to recover and 
		' * throw all the errors found in a parser log exception in the 
		' * end. 
		' * 
		' * @return the parse tree 
		' * 
		' * @throws ParserCreationException if the parser couldn't be 
		' * initialized correctly 
		' * @throws ParserLogException if the input couldn't be parsed 
		' * correctly 
		' * 
		' * @see #Prepare 
		' * @see #Reset 
		' * @see Tokenizer#Reset 
		' 

		Public Function Parse() As Node
			Dim root As Node = Nothing

			' Initialize parser 
			If Not initialized Then
				Prepare()
			End If
			Reset()

			' Parse input 
			Try
				root = ParseStart()
			Catch e As ParseException
				AddError(e, True)
			End Try

			' Check for errors 
			If errorLog.Count > 0 Then
				Throw errorLog
			End If

			Return root
		End Function

		'* 
		' * Parses the token stream and returns a parse tree. 
		' * 
		' * @return the parse tree 
		' * 
		' * @throws ParseException if the input couldn't be parsed 
		' * correctly 
		' 

		Protected MustOverride Function ParseStart() As Node

		'* 
		' * Adds an error to the error log. If the parser is in error 
		' * recovery mode, the error will not be added to the log. If the 
		' * recovery flag is set, this method will set the error recovery 
		' * counter thus enter error recovery mode. Only lexical or 
		' * syntactical errors require recovery, so this flag shouldn't be 
		' * set otherwise. 
		' * 
		' * @param e the error to add 
		' * @param recovery the recover flag 
		' 

		Friend Sub AddError(ByVal e As ParseException, ByVal recovery As Boolean)
			If errorRecovery <= 0 Then
				errorLog.AddError(e)
			End If
			If recovery Then
				errorRecovery = 3
			End If
		End Sub

		'* 
		' * Returns the production pattern with the specified id. 
		' * 
		' * @param id the production pattern id 
		' * 
		' * @return the production pattern found, or 
		' * null if non-existent 
		' 

		Friend Function GetPattern(ByVal id As Integer) As ProductionPattern
			Return DirectCast(patternIds(id), ProductionPattern)
		End Function

		'* 
		' * Returns the production pattern for the starting production. 
		' * 
		' * @return the start production pattern, or 
		' * null if no patterns have been added 
		' 

		Friend Function GetStartPattern() As ProductionPattern
			If patterns.Count <= 0 Then
				Return Nothing
			Else
				Return DirectCast(patterns(0), ProductionPattern)
			End If
		End Function

		'* 
		' * Returns the ordered set of production patterns. 
		' * 
		' * @return the ordered set of production patterns 
		' 

		Friend Function GetPatterns() As ICollection
			Return patterns
		End Function

		'* 
		' * Handles the parser entering a production. This method calls the 
		' * appropriate analyzer callback if the node is not hidden. Note 
		' * that this method will not call any callback if an error 
		' * requiring recovery has ocurred. 
		' * 
		' * @param node the parse tree node 
		' 

		Friend Sub EnterNode(ByVal node As Node)
			If Not node.IsHidden() AndAlso errorRecovery < 0 Then
				Try
					m_analyzer.Enter(node)
				Catch e As ParseException
					AddError(e, False)
				End Try
			End If
		End Sub

		'* 
		' * Handles the parser leaving a production. This method calls the 
		' * appropriate analyzer callback if the node is not hidden, and 
		' * returns the result. Note that this method will not call any 
		' * callback if an error requiring recovery has ocurred. 
		' * 
		' * @param node the parse tree node 
		' * 
		' * @return the parse tree node, or 
		' * null if no parse tree should be created 
		' 

		Friend Function ExitNode(ByVal node As Node) As Node
			If Not node.IsHidden() AndAlso errorRecovery < 0 Then
				Try
					Return m_analyzer.[Exit](node)
				Catch e As ParseException
					AddError(e, False)
				End Try
			End If
			Return node
		End Function

		'* 
		' * Handles the parser adding a child node to a production. This 
		' * method calls the appropriate analyzer callback. Note that this 
		' * method will not call any callback if an error requiring 
		' * recovery has ocurred. 
		' * 
		' * @param node the parent parse tree node 
		' * @param child the child parse tree node, or null 
		' 

		Friend Sub AddNode(ByVal node As Production, ByVal child As Node)
			' Do nothing 
			If errorRecovery >= 0 Then
			ElseIf node.IsHidden() Then
				node.AddChild(child)
			ElseIf child IsNot Nothing AndAlso child.IsHidden() Then
				For i As Integer = 0 To child.Count - 1
					AddNode(node, child(i))
				Next
			Else
				Try
					m_analyzer.Child(node, child)
				Catch e As ParseException
					AddError(e, False)
				End Try
			End If
		End Sub

		'* 
		' * Reads and consumes the next token in the queue. If no token 
		' * was available for consumation, a parse error will be 
		' * thrown. 
		' * 
		' * @return the token consumed 
		' * 
		' * @throws ParseException if the input stream couldn't be read or 
		' * parsed correctly 
		' 

		Friend Function NextToken() As Token
			Dim token As Token = PeekToken(0)

			If token IsNot Nothing Then
				tokens.RemoveAt(0)
				Return token
			Else
				Throw New ParseException(ParseException.ErrorType.UNEXPECTED_EOF, Nothing, m_tokenizer.GetCurrentLine(), m_tokenizer.GetCurrentColumn())
			End If
		End Function

		'* 
		' * Reads and consumes the next token in the queue. If no token was 
		' * available for consumation, a parse error will be thrown. A 
		' * parse error will also be thrown if the token id didn't match 
		' * the specified one. 
		' * 
		' * @param id the expected token id 
		' * 
		' * @return the token consumed 
		' * 
		' * @throws ParseException if the input stream couldn't be parsed 
		' * correctly, or if the token wasn't expected 
		' 

		Friend Function NextToken(ByVal id As Integer) As Token
			Dim token As Token = NextToken()
			Dim list As ArrayList

			If token.Id = id Then
				If errorRecovery > 0 Then
					errorRecovery -= 1
				End If
				Return token
			Else
				list = New ArrayList(1)
				list.Add(m_tokenizer.GetPatternDescription(id))
				Throw New ParseException(ParseException.ErrorType.UNEXPECTED_TOKEN, token.ToShortString(), list, token.StartLine, token.StartColumn)
			End If
		End Function

		'* 
		' * Returns a token from the queue. This method is used to check 
		' * coming tokens before they have been consumed. Any number of 
		' * tokens forward can be checked. 
		' * 
		' * @param steps the token queue number, zero (0) for first 
		' * 
		' * @return the token in the queue, or 
		' * null if no more tokens in the queue 
		' 

		Friend Function PeekToken(ByVal steps As Integer) As Token
			Dim token As Token

			While steps >= tokens.Count
				Try
					token = m_tokenizer.[Next]()
					If token Is Nothing Then
						Return Nothing
					Else
						tokens.Add(token)
					End If
				Catch e As ParseException
					AddError(e, True)
				End Try
			End While
			Return DirectCast(tokens(steps), Token)
		End Function

		'* 
		' * Returns a string representation of this parser. The string will 
		' * contain all the production definitions and various additional 
		' * information. 
		' * 
		' * @return a detailed string representation of this parser 
		' 

		Public Overloads Overrides Function ToString() As String
			Dim buffer As New StringBuilder()
			For i As Integer = 0 To patterns.Count - 1

				buffer.Append(ToString(DirectCast(patterns(i), ProductionPattern)))
				buffer.Append("" & Chr(10) & "")
			Next
			Return buffer.ToString()
		End Function

		'* 
		' * Returns a string representation of a production pattern. 
		' * 
		' * @param prod the production pattern 
		' * 
		' * @return a detailed string representation of the pattern 
		' 

		Private Overloads Function ToString(ByVal prod As ProductionPattern) As String
			Dim buffer As New StringBuilder()
			Dim indent As New StringBuilder()
			Dim [set] As LookAheadSet
			Dim i As Integer

			buffer.Append(prod.Name)
			buffer.Append(" (")
			buffer.Append(prod.Id)
			buffer.Append(") ")
			For i = 0 To buffer.Length - 1
				indent.Append(" ")
			Next
			buffer.Append("= ")
			indent.Append("| ")
			For i = 0 To prod.Count - 1
				If i > 0 Then
					buffer.Append(indent)
				End If
				buffer.Append(ToString(prod(i)))
				buffer.Append("" & Chr(10) & "")
			Next
			For i = 0 To prod.Count - 1
				[set] = prod(i).LookAhead
				If [set].GetMaxLength() > 1 Then
					buffer.Append("Using ")
					buffer.Append([set].GetMaxLength())
					buffer.Append(" token look-ahead for alternative ")
					buffer.Append(i + 1)
					buffer.Append(": ")
					buffer.Append([set].ToString(m_tokenizer))
					buffer.Append("" & Chr(10) & "")
				End If
			Next
			Return buffer.ToString()
		End Function

		'* 
		' * Returns a string representation of a production pattern 
		' * alternative. 
		' * 
		' * @param alt the production pattern alternative 
		' * 
		' * @return a detailed string representation of the alternative 
		' 

		Private Overloads Function ToString(ByVal alt As ProductionPatternAlternative) As String
			Dim buffer As New StringBuilder()
			For i As Integer = 0 To alt.Count - 1

				If i > 0 Then
					buffer.Append(" ")
				End If
				buffer.Append(ToString(alt(i)))
			Next
			Return buffer.ToString()
		End Function

		'* 
		' * Returns a string representation of a production pattern 
		' * element. 
		' * 
		' * @param elem the production pattern element 
		' * 
		' * @return a detailed string representation of the element 
		' 

		Private Overloads Function ToString(ByVal elem As ProductionPatternElement) As String
			Dim buffer As New StringBuilder()
			Dim min As Integer = elem.MinCount
			Dim max As Integer = elem.MaxCount

			If min = 0 AndAlso max = 1 Then
				buffer.Append("[")
			End If
			If elem.IsToken() Then
				buffer.Append(GetTokenDescription(elem.Id))
			Else
				buffer.Append(GetPattern(elem.Id).Name)
			End If
			If min = 0 AndAlso max = 1 Then
				buffer.Append("]")
			ElseIf min = 0 AndAlso max = Int32.MaxValue Then
				buffer.Append("*")
			ElseIf min = 1 AndAlso max = Int32.MaxValue Then
				buffer.Append("+")
			ElseIf min <> 1 OrElse max <> 1 Then
				buffer.Append("{")
				buffer.Append(min)
				buffer.Append(",")
				buffer.Append(max)
				buffer.Append("}")
			End If
			Return buffer.ToString()
		End Function

		'* 
		' * Returns a token description for a specified token. 
		' * 
		' * @param token the token to describe 
		' * 
		' * @return the token description 
		' 

		Friend Function GetTokenDescription(ByVal token As Integer) As String
			If m_tokenizer Is Nothing Then
				Return ""
			Else
				Return m_tokenizer.GetPatternDescription(token)
			End If
		End Function
	End Class
End Namespace 