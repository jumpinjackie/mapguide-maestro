' 
' * RecursiveDescentParser.cs 
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

Namespace PerCederberg.Grammatica.Runtime 
    
    '* 
' * A recursive descent parser. This parser handles LL(n) grammars, 
' * selecting the appropriate pattern to parse based on the next few 
' * tokens. The parser is more efficient the fewer look-ahead tokens 
' * that is has to consider. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.0 
' 
    
	Friend Class RecursiveDescentParser
		Inherits Parser

		'* 
		' * Creates a new parser. 
		' * 
		' * @param tokenizer the tokenizer to use 
		' 

		Public Sub New(ByVal tokenizer As Tokenizer)
			MyBase.New(tokenizer)
		End Sub

		'* 
		' * Creates a new parser. 
		' * 
		' * @param tokenizer the tokenizer to use 
		' * @param analyzer the analyzer callback to use 
		' 

		Public Sub New(ByVal tokenizer As Tokenizer, ByVal analyzer As Analyzer)
			MyBase.New(tokenizer, analyzer)
		End Sub

		'* 
		' * Adds a new production pattern to the parser. The pattern 
		' * will be added last in the list. The first pattern added is 
		' * assumed to be the starting point in the grammar. The 
		' * pattern will be validated against the grammar type to some 
		' * extent. 
		' * 
		' * @param pattern the pattern to add 
		' * 
		' * @throws ParserCreationException if the pattern couldn't be 
		' * added correctly to the parser 
		' 

		Public Overloads Overrides Sub AddPattern(ByVal pattern As ProductionPattern)

			' Check for empty matches 
			If pattern.IsMatchingEmpty() Then
				Throw New ParserCreationException(ParserCreationException.ErrorType.INVALID_PRODUCTION, pattern.Name, "zero elements can be matched (minimum is one)")
			End If

			' Check for left-recusive patterns 
			If pattern.IsLeftRecursive() Then
				Throw New ParserCreationException(ParserCreationException.ErrorType.INVALID_PRODUCTION, pattern.Name, "left recursive patterns are not allowed")
			End If

			' Add pattern 
			MyBase.AddPattern(pattern)
		End Sub

		'* 
		' * Initializes the parser. All the added production patterns 
		' * will be analyzed for ambiguities and errors. This method 
		' * also initializes the internal data structures used during 
		' * the parsing. 
		' * 
		' * @throws ParserCreationException if the parser couldn't be 
		' * initialized correctly 
		' 

		Public Overloads Overrides Sub Prepare()
			Dim e As IEnumerator

			' Performs production pattern checks 
			MyBase.Prepare()
			SetInitialized(False)

			' Calculate production look-ahead sets 
			e = GetPatterns().GetEnumerator()
			While e.MoveNext()
				CalculateLookAhead(DirectCast(e.Current, ProductionPattern))
			End While

			' Set initialized flag 
			SetInitialized(True)
		End Sub

		'* 
		' * Parses the input stream and creates a parse tree. 
		' * 
		' * @return the parse tree 
		' * 
		' * @throws ParseException if the input couldn't be parsed 
		' * correctly 
		' 

		Protected Overloads Overrides Function ParseStart() As Node
			Dim token As Token
			Dim node As Node
			Dim list As ArrayList

			node = ParsePattern(GetStartPattern())
			token = PeekToken(0)
			If token IsNot Nothing Then
				list = New ArrayList(1)
				list.Add("<EOF>")
				Throw New ParseException(ParseException.ErrorType.UNEXPECTED_TOKEN, token.ToShortString(), list, token.StartLine, token.StartColumn)
			End If
			Return node
		End Function

		'* 
		' * Parses a production pattern. A parse tree node may or may 
		' * not be created depending on the analyzer callbacks. 
		' * 
		' * @param pattern the production pattern to parse 
		' * 
		' * @return the parse tree node created, or null 
		' * 
		' * @throws ParseException if the input couldn't be parsed 
		' * correctly 
		' 

		Private Function ParsePattern(ByVal pattern As ProductionPattern) As Node
			Dim alt As ProductionPatternAlternative
			Dim defaultAlt As ProductionPatternAlternative

			defaultAlt = pattern.DefaultAlternative
			For i As Integer = 0 To pattern.Count - 1
				alt = pattern(i)
				If defaultAlt IsNot alt AndAlso IsNext(alt) Then
					Return ParseAlternative(alt)
				End If
			Next
			If defaultAlt Is Nothing OrElse Not IsNext(defaultAlt) Then
				ThrowParseException(FindUnion(pattern))
			End If
			Return ParseAlternative(defaultAlt)
		End Function

		'* 
		' * Parses a production pattern alternative. A parse tree node 
		' * may or may not be created depending on the analyzer 
		' * callbacks. 
		' * 
		' * @param alt the production pattern alternative 
		' * 
		' * @return the parse tree node created, or null 
		' * 
		' * @throws ParseException if the input couldn't be parsed 
		' * correctly 
		' 

		Private Function ParseAlternative(ByVal alt As ProductionPatternAlternative) As Node
			Dim node As Production

			node = New Production(alt.Pattern)
			EnterNode(node)
			For i As Integer = 0 To alt.Count - 1
				Try
					ParseElement(node, alt(i))
				Catch e As ParseException
					AddError(e, True)
					NextToken()
					i -= 1
				End Try
			Next
			Return ExitNode(node)
		End Function

		'* 
		' * Parses a production pattern element. All nodes parsed may 
		' * or may not be added to the parse tree node specified, 
		' * depending on the analyzer callbacks. 
		' * 
		' * @param node the production parse tree node 
		' * @param elem the production pattern element to parse 
		' * 
		' * @throws ParseException if the input couldn't be parsed 
		' * correctly 
		' 

		Private Sub ParseElement(ByVal node As Production, ByVal elem As ProductionPatternElement)

			Dim child As Node
			For i As Integer = 0 To elem.MaxCount - 1

				If i < elem.MinCount OrElse IsNext(elem) Then
					If elem.IsToken() Then
						child = NextToken(elem.Id)
						EnterNode(child)
						AddNode(node, ExitNode(child))
					Else
						child = ParsePattern(GetPattern(elem.Id))
						AddNode(node, child)
					End If
				Else
					Exit For
				End If
			Next
		End Sub

		'* 
		' * Checks if the next tokens match a production pattern. The 
		' * pattern look-ahead set will be used if existing, otherwise 
		' * this method returns false. 
		' * 
		' * @param pattern the pattern to check 
		' * 
		' * @return true if the next tokens match, or 
		' * false otherwise 
		' 

		Private Function IsNext(ByVal pattern As ProductionPattern) As Boolean
			Dim [set] As LookAheadSet = pattern.LookAhead

			If [set] Is Nothing Then
				Return False
			Else
				Return [set].IsNext(Me)
			End If
		End Function

		'* 
		' * Checks if the next tokens match a production pattern 
		' * alternative. The pattern alternative look-ahead set will be 
		' * used if existing, otherwise this method returns false. 
		' * 
		' * @param alt the pattern alternative to check 
		' * 
		' * @return true if the next tokens match, or 
		' * false otherwise 
		' 

		Private Function IsNext(ByVal alt As ProductionPatternAlternative) As Boolean
			Dim [set] As LookAheadSet = alt.LookAhead

			If [set] Is Nothing Then
				Return False
			Else
				Return [set].IsNext(Me)
			End If
		End Function

		'* 
		' * Checks if the next tokens match a production pattern 
		' * element. If the element has a look-ahead set it will be 
		' * used, otherwise the look-ahead set of the referenced 
		' * production or token will be used. 
		' * 
		' * @param elem the pattern element to check 
		' * 
		' * @return true if the next tokens match, or 
		' * false otherwise 
		' 

		Private Function IsNext(ByVal elem As ProductionPatternElement) As Boolean
			Dim [set] As LookAheadSet = elem.LookAhead

			If [set] IsNot Nothing Then
				Return [set].IsNext(Me)
			ElseIf elem.IsToken() Then
				Return elem.IsMatch(PeekToken(0))
			Else
				Return IsNext(GetPattern(elem.Id))
			End If
		End Function

		'* 
		' * Calculates the look-ahead needed for the specified production 
		' * pattern. This method attempts to resolve any conflicts and 
		' * stores the results in the pattern look-ahead object. 
		' * 
		' * @param pattern the production pattern 
		' * 
		' * @throws ParserCreationException if the look-ahead set couldn't 
		' * be determined due to inherent ambiguities 
		' 

		Private Sub CalculateLookAhead(ByVal pattern As ProductionPattern)
			Dim alt As ProductionPatternAlternative
			Dim result As LookAheadSet
			Dim alternatives As LookAheadSet()
			Dim conflicts As LookAheadSet
			Dim previous As New LookAheadSet(0)
			Dim length As Integer = 1
			Dim i As Integer
			Dim stack As New CallStack()

			' Calculate simple look-ahead 
			stack.Push(pattern.Name, 1)
			result = New LookAheadSet(1)
			alternatives = New LookAheadSet(pattern.Count - 1) {}
			For i = 0 To pattern.Count - 1
				alt = pattern(i)
				alternatives(i) = FindLookAhead(alt, 1, 0, stack, Nothing)
				alt.LookAhead = alternatives(i)
				result.AddAll(alternatives(i))
			Next
			If pattern.LookAhead Is Nothing Then
				pattern.LookAhead = result
			End If
			conflicts = FindConflicts(pattern, 1)

			' Resolve conflicts 
			While conflicts.Size() > 0
				length += 1
				stack.Clear()
				stack.Push(pattern.Name, length)
				conflicts.AddAll(previous)
				For i = 0 To pattern.Count - 1
					alt = pattern(i)
					If alternatives(i).Intersects(conflicts) Then
						alternatives(i) = FindLookAhead(alt, length, 0, stack, conflicts)
						alt.LookAhead = alternatives(i)
					End If
					If alternatives(i).Intersects(conflicts) Then
						If pattern.DefaultAlternative Is Nothing Then
							pattern.DefaultAlternative = alt
						ElseIf pattern.DefaultAlternative IsNot alt Then
							result = alternatives(i).CreateIntersection(conflicts)
							ThrowAmbiguityException(pattern.Name, Nothing, result)
						End If
					End If
				Next
				previous = conflicts
				conflicts = FindConflicts(pattern, length)
			End While
			For i = 0 To pattern.Count - 1

				' Resolve conflicts inside rules 
				CalculateLookAhead(pattern(i), 0)
			Next
		End Sub

		'* 
		' * Calculates the look-aheads needed for the specified pattern 
		' * alternative. This method attempts to resolve any conflicts in 
		' * optional elements by recalculating look-aheads for referenced 
		' * productions. 
		' * 
		' * @param alt the production pattern alternative 
		' * @param pos the pattern element position 
		' * 
		' * @throws ParserCreationException if the look-ahead set couldn't 
		' * be determined due to inherent ambiguities 
		' 

		Private Sub CalculateLookAhead(ByVal alt As ProductionPatternAlternative, ByVal pos As Integer)

			Dim pattern As ProductionPattern
			Dim elem As ProductionPatternElement
			Dim first As LookAheadSet
			Dim follow As LookAheadSet
			Dim conflicts As LookAheadSet
			Dim previous As New LookAheadSet(0)
			Dim location As String
			Dim length As Integer = 1

			' Check trivial cases 
			If pos >= alt.Count Then
				Return
			End If

			' Check for non-optional element 
			pattern = alt.Pattern
			elem = alt(pos)
			If elem.MinCount = elem.MaxCount Then
				CalculateLookAhead(alt, pos + 1)
				Return
			End If

			' Calculate simple look-aheads 
			first = FindLookAhead(elem, 1, New CallStack(), Nothing)
			follow = FindLookAhead(alt, 1, pos + 1, New CallStack(), Nothing)

			' Resolve conflicts 
			location = "at position " & (pos + 1)
			conflicts = FindConflicts(pattern.Name, location, first, follow)
			While conflicts.Size() > 0
				length += 1
				conflicts.AddAll(previous)
				first = FindLookAhead(elem, length, New CallStack(), conflicts)
				follow = FindLookAhead(alt, length, pos + 1, New CallStack(), conflicts)
				first = first.CreateCombination(follow)
				elem.LookAhead = first
				If first.Intersects(conflicts) Then
					first = first.CreateIntersection(conflicts)
					ThrowAmbiguityException(pattern.Name, location, first)
				End If
				previous = conflicts
				conflicts = FindConflicts(pattern.Name, location, first, follow)
			End While

			' Check remaining elements 
			CalculateLookAhead(alt, pos + 1)
		End Sub

		'* 
		' * Finds the look-ahead set for a production pattern. The maximum 
		' * look-ahead length must be specified. It is also possible to 
		' * specify a look-ahead set filter, which will make sure that 
		' * unnecessary token sequences will be avoided. 
		' * 
		' * @param pattern the production pattern 
		' * @param length the maximum look-ahead length 
		' * @param stack the call stack used for loop detection 
		' * @param filter the look-ahead set filter 
		' * 
		' * @return the look-ahead set for the production pattern 
		' * 
		' * @throws ParserCreationException if an infinite loop was found 
		' * in the grammar 
		' 

		Private Function FindLookAhead(ByVal pattern As ProductionPattern, ByVal length As Integer, ByVal stack As CallStack, ByVal filter As LookAheadSet) As LookAheadSet

			Dim result As LookAheadSet
			Dim temp As LookAheadSet

			' Check for infinite loop 
			If stack.Contains(pattern.Name, length) Then
				Throw New ParserCreationException(ParserCreationException.ErrorType.INFINITE_LOOP, pattern.Name, DirectCast(Nothing, String))
			End If

			' Find pattern look-ahead 
			stack.Push(pattern.Name, length)
			result = New LookAheadSet(length)
			For i As Integer = 0 To pattern.Count - 1
				temp = FindLookAhead(pattern(i), length, 0, stack, filter)
				result.AddAll(temp)
			Next
			stack.Pop()

			Return result
		End Function

		'* 
		' * Finds the look-ahead set for a production pattern alternative. 
		' * The pattern position and maximum look-ahead length must be 
		' * specified. It is also possible to specify a look-ahead set 
		' * filter, which will make sure that unnecessary token sequences 
		' * will be avoided. 
		' * 
		' * @param alt the production pattern alternative 
		' * @param length the maximum look-ahead length 
		' * @param pos the pattern element position 
		' * @param stack the call stack used for loop detection 
		' * @param filter the look-ahead set filter 
		' * 
		' * @return the look-ahead set for the pattern alternative 
		' * 
		' * @throws ParserCreationException if an infinite loop was found 
		' * in the grammar 
		' 

		Private Function FindLookAhead(ByVal alt As ProductionPatternAlternative, ByVal length As Integer, ByVal pos As Integer, ByVal stack As CallStack, ByVal filter As LookAheadSet) As LookAheadSet

			Dim first As LookAheadSet
			Dim follow As LookAheadSet
			Dim overlaps As LookAheadSet

			' Check trivial cases 
			If length <= 0 OrElse pos >= alt.Count Then
				Return New LookAheadSet(0)
			End If

			' Find look-ahead for this element 
			first = FindLookAhead(alt(pos), length, stack, filter)
			If alt(pos).MinCount = 0 Then
				first.AddEmpty()
			End If

			' Find remaining look-ahead 
			If filter Is Nothing Then
				length -= first.GetMinLength()
				If length > 0 Then
					follow = FindLookAhead(alt, length, pos + 1, stack, Nothing)
					first = first.CreateCombination(follow)
				End If
			ElseIf filter.IsOverlap(first) Then
				overlaps = first.CreateOverlaps(filter)
				length -= overlaps.GetMinLength()
				filter = filter.CreateFilter(overlaps)
				follow = FindLookAhead(alt, length, pos + 1, stack, filter)
				first.RemoveAll(overlaps)
				first.AddAll(overlaps.CreateCombination(follow))
			End If

			Return first
		End Function

		'* 
		' * Finds the look-ahead set for a production pattern element. The 
		' * maximum look-ahead length must be specified. This method takes 
		' * the element repeats into consideration when creating the 
		' * look-ahead set, but does NOT include an empty sequence even if 
		' * the minimum count is zero (0). It is also possible to specify a 
		' * look-ahead set filter, which will make sure that unnecessary 
		' * token sequences will be avoided. 
		' * 
		' * @param elem the production pattern element 
		' * @param length the maximum look-ahead length 
		' * @param stack the call stack used for loop detection 
		' * @param filter the look-ahead set filter 
		' * 
		' * @return the look-ahead set for the pattern element 
		' * 
		' * @throws ParserCreationException if an infinite loop was found 
		' * in the grammar 
		' 

		Private Function FindLookAhead(ByVal elem As ProductionPatternElement, ByVal length As Integer, ByVal stack As CallStack, ByVal filter As LookAheadSet) As LookAheadSet

			Dim result As LookAheadSet
			Dim first As LookAheadSet
			Dim follow As LookAheadSet
			Dim max As Integer

			' Find initial element look-ahead 
			first = FindLookAhead(elem, length, 0, stack, filter)
			result = New LookAheadSet(length)
			result.AddAll(first)
			If filter Is Nothing OrElse Not filter.IsOverlap(result) Then
				Return result
			End If

			' Handle element repetitions 
			If elem.MaxCount = Int32.MaxValue Then
				first = first.CreateRepetitive()
			End If
			max = elem.MaxCount
			If length < max Then
				max = length
			End If
			For i As Integer = 1 To max - 1
				first = first.CreateOverlaps(filter)
				If first.Size() <= 0 OrElse first.GetMinLength() >= length Then
					Exit For
				End If
				follow = FindLookAhead(elem, length, 0, stack, filter.CreateFilter(first))
				first = first.CreateCombination(follow)
				result.AddAll(first)
			Next

			Return result
		End Function

		'* 
		' * Finds the look-ahead set for a production pattern element. The 
		' * maximum look-ahead length must be specified. This method does 
		' * NOT take the element repeat into consideration when creating 
		' * the look-ahead set. It is also possible to specify a look-ahead 
		' * set filter, which will make sure that unnecessary token 
		' * sequences will be avoided. 
		' * 
		' * @param elem the production pattern element 
		' * @param length the maximum look-ahead length 
		' * @param dummy a parameter to distinguish the method 
		' * @param stack the call stack used for loop detection 
		' * @param filter the look-ahead set filter 
		' * 
		' * @return the look-ahead set for the pattern element 
		' * 
		' * @throws ParserCreationException if an infinite loop was found 
		' * in the grammar 
		' 

		Private Function FindLookAhead(ByVal elem As ProductionPatternElement, ByVal length As Integer, ByVal dummy As Integer, ByVal stack As CallStack, ByVal filter As LookAheadSet) As LookAheadSet

			Dim result As LookAheadSet
			Dim pattern As ProductionPattern

			If elem.IsToken() Then
				result = New LookAheadSet(length)
				result.Add(elem.Id)
			Else
				pattern = GetPattern(elem.Id)
				result = FindLookAhead(pattern, length, stack, filter)
				If stack.Contains(pattern.Name) Then
					result = result.CreateRepetitive()
				End If
			End If

			Return result
		End Function

		'* 
		' * Returns a look-ahead set with all conflics between 
		' * alternatives in a production pattern. 
		' * 
		' * @param pattern the production pattern 
		' * @param maxLength the maximum token sequence length 
		' * 
		' * @return a look-ahead set with the conflicts found 
		' * 
		' * @throws ParserCreationException if an inherent ambiguity was 
		' * found among the look-ahead sets 
		' 

		Private Function FindConflicts(ByVal pattern As ProductionPattern, ByVal maxLength As Integer) As LookAheadSet

			Dim result As New LookAheadSet(maxLength)
			Dim set1 As LookAheadSet
			Dim set2 As LookAheadSet
			For i As Integer = 0 To pattern.Count - 1

				set1 = pattern(i).LookAhead
				For j As Integer = 0 To i - 1
					set2 = pattern(j).LookAhead
					result.AddAll(set1.CreateIntersection(set2))
				Next
			Next
			If result.IsRepetitive() Then
				ThrowAmbiguityException(pattern.Name, Nothing, result)
			End If
			Return result
		End Function

		'* 
		' * Returns a look-ahead set with all conflicts between two 
		' * look-ahead sets. 
		' * 
		' * @param pattern the pattern name being analyzed 
		' * @param location the pattern location 
		' * @param set1 the first look-ahead set 
		' * @param set2 the second look-ahead set 
		' * 
		' * @return a look-ahead set with the conflicts found 
		' * 
		' * @throws ParserCreationException if an inherent ambiguity was 
		' * found among the look-ahead sets 
		' 

		Private Function FindConflicts(ByVal pattern As String, ByVal location As String, ByVal set1 As LookAheadSet, ByVal set2 As LookAheadSet) As LookAheadSet

			Dim result As LookAheadSet

			result = set1.CreateIntersection(set2)
			If result.IsRepetitive() Then
				ThrowAmbiguityException(pattern, location, result)
			End If
			Return result
		End Function

		'* 
		' * Returns the union of all alternative look-ahead sets in a 
		' * production pattern. 
		' * 
		' * @param pattern the production pattern 
		' * 
		' * @return a unified look-ahead set 
		' 

		Private Function FindUnion(ByVal pattern As ProductionPattern) As LookAheadSet
			Dim result As LookAheadSet
			Dim length As Integer = 0
			Dim i As Integer
			For i = 0 To pattern.Count - 1

				result = pattern(i).LookAhead
				If result.GetMaxLength() > length Then
					length = result.GetMaxLength()
				End If
			Next
			result = New LookAheadSet(length)
			For i = 0 To pattern.Count - 1
				result.AddAll(pattern(i).LookAhead)
			Next

			Return result
		End Function

		'* 
		' * Throws a parse exception that matches the specified look-ahead 
		' * set. This method will take into account any initial matching 
		' * tokens in the look-ahead set. 
		' * 
		' * @param set the look-ahead set to match 
		' * 
		' * @throws ParseException always thrown by this method 
		' 

		Private Sub ThrowParseException(ByVal [set] As LookAheadSet)
			Dim token As Token
			Dim list As New ArrayList()
			Dim initials As Integer()

			' Read tokens until mismatch 
			While [set].IsNext(Me, 1)
				[set] = [set].CreateNextSet(NextToken().Id)
			End While

			' Find next token descriptions 
			initials = [set].GetInitialTokens()
			For i As Integer = 0 To initials.Length - 1
				list.Add(GetTokenDescription(initials(i)))
			Next

			' Create exception 
			token = NextToken()
			Throw New ParseException(ParseException.ErrorType.UNEXPECTED_TOKEN, token.ToShortString(), list, token.StartLine, token.StartColumn)
		End Sub

		'* 
		' * Throws a parser creation exception for an ambiguity. The 
		' * specified look-ahead set contains the token conflicts to be 
		' * reported. 
		' * 
		' * @param pattern the production pattern name 
		' * @param location the production pattern location, or null 
		' * @param set the look-ahead set with conflicts 
		' * 
		' * @throws ParserCreationException always thrown by this method 
		' 

		Private Sub ThrowAmbiguityException(ByVal pattern As String, ByVal location As String, ByVal [set] As LookAheadSet)

			Dim list As New ArrayList()
			Dim initials As Integer()

			' Find next token descriptions 
			initials = [set].GetInitialTokens()
			For i As Integer = 0 To initials.Length - 1
				list.Add(GetTokenDescription(initials(i)))
			Next

			' Create exception 
			Throw New ParserCreationException(ParserCreationException.ErrorType.INHERENT_AMBIGUITY, pattern, location, list)
		End Sub


		'* 
		' * A name value stack. This stack is used to detect loops and 
		' * repetitions of the same production during look-ahead analysis. 
		' 

		Private Class CallStack

			'* 
			' * A stack with names. 
			' 

			Private nameStack As New ArrayList()

			'* 
			' * A stack with values. 
			' 

			Private valueStack As New ArrayList()

			'* 
			' * Checks if the specified name is on the stack. 
			' * 
			' * @param name the name to search for 
			' * 
			' * @return true if the name is on the stack, or 
			' * false otherwise 
			' 

			Public Function Contains(ByVal name As String) As Boolean
				Return nameStack.Contains(name)
			End Function

			'* 
			' * Checks if the specified name and value combination is on 
			' * the stack. 
			' * 
			' * @param name the name to search for 
			' * @param value the value to search for 
			' * 
			' * @return true if the combination is on the stack, or 
			' * false otherwise 
			' 

			Public Function Contains(ByVal name As String, ByVal value As Integer) As Boolean
				For i As Integer = 0 To nameStack.Count - 1
					If nameStack(i).Equals(name) AndAlso valueStack(i).Equals(value) Then

						Return True
					End If
				Next
				Return False
			End Function

			'* 
			' * Clears the stack. This method removes all elements on 
			' * the stack. 
			' 

			Public Sub Clear()
				nameStack.Clear()
				valueStack.Clear()
			End Sub

			'* 
			' * Adds a new element to the top of the stack. 
			' * 
			' * @param name the stack name 
			' * @param value the stack value 
			' 

			Public Sub Push(ByVal name As String, ByVal value As Integer)
				nameStack.Add(name)
				valueStack.Add(value)
			End Sub

			'* 
			' * Removes the top element of the stack. 
			' 

			Public Sub Pop()
				If nameStack.Count > 0 Then
					nameStack.RemoveAt(nameStack.Count - 1)
					valueStack.RemoveAt(valueStack.Count - 1)
				End If
			End Sub
		End Class
	End Class
End Namespace 