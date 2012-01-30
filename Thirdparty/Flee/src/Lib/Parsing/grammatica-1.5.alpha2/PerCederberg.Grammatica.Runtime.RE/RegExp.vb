' 
' * RegExp.cs 
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
Imports System.IO 
Imports System.Globalization 
Imports System.Text 

Imports Ciloci.Flee.PerCederberg.Grammatica.Runtime

Namespace PerCederberg.Grammatica.Runtime.RE 
    
    '* 
' * A regular expression. This class creates and holds an internal 
' * data structure representing a regular expression. It also 
' * allows creating matchers. This class is thread-safe. Multiple 
' * matchers may operate simultanously on the same regular 
' * expression. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
	Friend Class RegExp

		'* 
		' * The base regular expression element. 
		' 

		Private element As Element

		'* 
		' * The regular expression pattern. 
		' 

		Private pattern As String

		'* 
		' * The character case ignore flag. 
		' 

		Private ignoreCase As Boolean

		'* 
		' * The current position in the pattern. This variable is used by 
		' * the parsing methods. 
		' 

		Private pos As Integer

		'* 
		' * Creates a new case-sensitive regular expression. 
		' * 
		' * @param pattern the regular expression pattern 
		' * 
		' * @throws RegExpException if the regular expression couldn't be 
		' * parsed correctly 
		' 

		Public Sub New(ByVal pattern As String)
			Me.New(pattern, False)
		End Sub

		'* 
		' * Creates a new regular expression. The regular expression 
		' * can be either case-sensitive or case-insensitive. 
		' * 
		' * @param pattern the regular expression pattern 
		' * @param ignoreCase the character case ignore flag 
		' * 
		' * @throws RegExpException if the regular expression couldn't be 
		' * parsed correctly 
		' * 
		' * @since 1.5 
		' 

		Public Sub New(ByVal pattern As String, ByVal ignoreCase As Boolean)
			Me.pattern = pattern
			Me.ignoreCase = ignoreCase
			Me.element = ParseExpr()
			If pos < pattern.Length Then
				Throw New RegExpException(RegExpException.ErrorType.UNEXPECTED_CHARACTER, pos, pattern)
			End If
		End Sub

		'* 
		' * Creates a new matcher for the specified string. 
		' * 
		' * @param str the string to work with 
		' * 
		' * @return the regular expresion matcher 
		' 

		Public Function Matcher(ByVal str As String) As Matcher
			Return Matcher(New StringReader(str))
		End Function

		'* 
		' * Creates a new matcher for the specified character input 
		' * stream. 
		' * 
		' * @param input the character input stream 
		' * 
		' * @return the regular expresion matcher 
		' * 
		' * @since 1.5 
		' 

		Public Function Matcher(ByVal input As TextReader) As Matcher
			If TypeOf input Is LookAheadReader Then
				Return Matcher(DirectCast(input, LookAheadReader))
			Else
				Return Matcher(New LookAheadReader(input))
			End If
		End Function

		'* 
		' * Creates a new matcher for the specified look-ahead 
		' * character input stream. 
		' * 
		' * @param input the character input stream 
		' * 
		' * @return the regular expresion matcher 
		' * 
		' * @since 1.5 
		' 

		Private Function Matcher(ByVal input As LookAheadReader) As Matcher
			Return New Matcher(DirectCast(element.Clone(), Element), input, ignoreCase)
		End Function

		'* 
		' * Returns a string representation of the regular expression. 
		' * 
		' * @return a string representation of the regular expression 
		' 

		Public Overloads Overrides Function ToString() As String
			Dim str As StringWriter

			str = New StringWriter()
			str.WriteLine("Regular Expression")
			str.WriteLine(" Pattern: " + pattern)
			str.Write(" Flags:")
			If ignoreCase Then
				str.Write(" caseignore")
			End If
			str.WriteLine()
			str.WriteLine(" Compiled:")
			element.PrintTo(str, " ")
			Return str.ToString()
		End Function

		'* 
		' * Parses a regular expression. This method handles the Expr 
		' * production in the grammar (see regexp.grammar). 
		' * 
		' * @return the element representing this expression 
		' * 
		' * @throws RegExpException if an error was encountered in the 
		' * pattern string 
		' 

		Private Function ParseExpr() As Element
			Dim first As Element
			Dim second As Element

			first = ParseTerm()
			If PeekChar(0) <> Convert.ToInt32("|"c) Then
				Return first
			Else
				ReadChar("|"c)
				second = ParseExpr()
				Return New AlternativeElement(first, second)
			End If
		End Function

		'* 
		' * Parses a regular expression term. This method handles the 
		' * Term production in the grammar (see regexp.grammar). 
		' * 
		' * @return the element representing this term 
		' * 
		' * @throws RegExpException if an error was encountered in the 
		' * pattern string 
		' 

		Private Function ParseTerm() As Element
			Dim list As New ArrayList()

			list.Add(ParseFact())
			While True
				Dim i As Integer = PeekChar(0)

				If i = -1 Then
					Return CombineElements(list)
				End If

				Dim c As Char = Convert.ToChar(i)

				Select Case c
					Case ")"c, "]"c, "{"c, "}"c, "?"c, "+"c, "|"c
						Return CombineElements(list)
					Case Else
						list.Add(ParseFact())
						Exit Select
				End Select
			End While

			Return Nothing
		End Function

		'* 
		' * Parses a regular expression factor. This method handles the 
		' * Fact production in the grammar (see regexp.grammar). 
		' * 
		' * @return the element representing this factor 
		' * 
		' * @throws RegExpException if an error was encountered in the 
		' * pattern string 
		' 

		Private Function ParseFact() As Element
			Dim elem As Element

			elem = ParseAtom()

			Dim i As Integer = PeekChar(0)

			If i = -1 Then
				Return elem
			End If

			Dim c As Char = Convert.ToChar(i)

			Select Case c
				Case "?"c, "*"c, "+"c, "{"c
					Return ParseAtomModifier(elem)
				Case Else
					Return elem
			End Select
		End Function

		'* 
		' * Parses a regular expression atom. This method handles the 
		' * Atom production in the grammar (see regexp.grammar). 
		' * 
		' * @return the element representing this atom 
		' * 
		' * @throws RegExpException if an error was encountered in the 
		' * pattern string 
		' 

		Private Function ParseAtom() As Element
			Dim elem As Element

			Dim i As Integer = PeekChar(0)

			If i = -1 Then
				Throw New RegExpException(RegExpException.ErrorType.UNEXPECTED_CHARACTER, pos, pattern)
			End If

			Dim c As Char = Convert.ToChar(i)

			Select Case c
				Case "."c
					ReadChar("."c)
					Return CharacterSetElement.DOT
				Case "("c
					ReadChar("("c)
					elem = ParseExpr()
					ReadChar(")"c)
					Return elem
				Case "["c
					ReadChar("["c)
					elem = ParseCharSet()
					ReadChar("]"c)
					Return elem
				Case ")"c, "]"c, "{"c, "}"c, "?"c, "*"c, "+"c, "|"c
					Throw New RegExpException(RegExpException.ErrorType.UNEXPECTED_CHARACTER, pos, pattern)
				Case Else
					Return ParseChar()
			End Select
		End Function

		'* 
		' * Parses a regular expression atom modifier. This method handles 
		' * the AtomModifier production in the grammar (see regexp.grammar). 
		' * 
		' * @param elem the element to modify 
		' * 
		' * @return the modified element 
		' * 
		' * @throws RegExpException if an error was encountered in the 
		' * pattern string 
		' 

		Private Function ParseAtomModifier(ByVal elem As Element) As Element
			Dim min As Integer = 0
			Dim max As Integer = -1
			Dim type As RepeatElement.RepeatType
			Dim firstPos As Integer

			' Read min and max 
			type = RepeatElement.RepeatType.GREEDY
			Select Case ReadChar()
				Case "?"c
					min = 0
					max = 1
					Exit Select
				Case "*"c
					min = 0
					max = -1
					Exit Select
				Case "+"c
					min = 1
					max = -1
					Exit Select
				Case "{"c
					firstPos = pos - 1
					min = ReadNumber()
					max = min
					If PeekChar(0) = Convert.ToInt32(","c) Then
						ReadChar(","c)
						max = -1
						If PeekChar(0) <> Convert.ToInt32("}"c) Then
							max = ReadNumber()
						End If
					End If
					ReadChar("}"c)
					If max = 0 OrElse (max > 0 AndAlso min > max) Then
						Throw New RegExpException(RegExpException.ErrorType.INVALID_REPEAT_COUNT, firstPos, pattern)
					End If
					Exit Select
				Case Else
					Throw New RegExpException(RegExpException.ErrorType.UNEXPECTED_CHARACTER, pos - 1, pattern)
			End Select

			' Read operator mode 
			If PeekChar(0) = Convert.ToInt32("?"c) Then
				ReadChar("?"c)
				type = RepeatElement.RepeatType.RELUCTANT
			ElseIf PeekChar(0) = Convert.ToInt32("+"c) Then
				ReadChar("+"c)
				type = RepeatElement.RepeatType.POSSESSIVE
			End If

			Return New RepeatElement(elem, min, max, type)
		End Function

		'* 
		' * Parses a regular expression character set. This method handles 
		' * the contents of the '[...]' construct in a regular expression. 
		' * 
		' * @return the element representing this character set 
		' * 
		' * @throws RegExpException if an error was encountered in the 
		' * pattern string 
		' 

		Private Function ParseCharSet() As Element
			Dim charset As CharacterSetElement
			Dim elem As Element
			Dim repeat As Boolean = True
			Dim start As Char
			Dim [end] As Char

			If PeekChar(0) = Convert.ToInt32("^"c) Then
				ReadChar("^"c)
				charset = New CharacterSetElement(True)
			Else
				charset = New CharacterSetElement(False)
			End If

			While PeekChar(0) > 0 AndAlso repeat
				start = Convert.ToChar(PeekChar(0))
				Select Case start
					Case "]"c
						repeat = False
						Exit Select
					Case "\"c
						elem = ParseEscapeChar()
						If TypeOf elem Is StringElement Then
							charset.AddCharacters(DirectCast(elem, StringElement))
						Else
							charset.AddCharacterSet(DirectCast(elem, CharacterSetElement))
						End If
						Exit Select
					Case Else
						ReadChar(start)
						If PeekChar(0) = Convert.ToInt32("-"c) AndAlso PeekChar(1) > 0 AndAlso PeekChar(1) <> Convert.ToInt32("]"c) Then

							ReadChar("-"c)
							[end] = ReadChar()
							charset.AddRange(FixChar(start), FixChar([end]))
						Else
							charset.AddCharacter(FixChar(start))
						End If
						Exit Select
				End Select
			End While

			Return charset
		End Function

		'* 
		' * Parses a regular expression character. This method handles 
		' * a single normal character in a regular expression. 
		' * 
		' * @return the element representing this character 
		' * 
		' * @throws RegExpException if an error was encountered in the 
		' * pattern string 
		' 

		Private Function ParseChar() As Element
			Dim c As Char = Convert.ToChar(PeekChar(0))

			Select Case c
				Case "\"c
					Return ParseEscapeChar()
				Case "^"c, "$"c
					Throw New RegExpException(RegExpException.ErrorType.UNSUPPORTED_SPECIAL_CHARACTER, pos, pattern)
				Case Else
					Return New StringElement(FixChar(ReadChar()))
			End Select
		End Function

		'* 
		' * Parses a regular expression character escape. This method 
		' * handles a single character escape in a regular expression. 
		' * 
		' * @return the element representing this character escape 
		' * 
		' * @throws RegExpException if an error was encountered in the 
		' * pattern string 
		' 

		Private Function ParseEscapeChar() As Element
			Dim c As Char
			Dim str As String
			Dim value As Integer

			ReadChar("\"c)
			c = ReadChar()
			Select Case c
				Case "0"c
					c = ReadChar()
					If c < "0"c OrElse c > "3"c Then
						Throw New RegExpException(RegExpException.ErrorType.UNSUPPORTED_ESCAPE_CHARACTER, pos - 3, pattern)
					End If
					value = Convert.ToInt32(c) - Convert.ToInt32("0"c)
					c = Convert.ToChar(PeekChar(0))
					If "0"c <= c AndAlso c <= "7"c Then
						value *= 8
						value += Convert.ToInt32(ReadChar()) - Convert.ToInt32("0"c)
						c = Convert.ToChar(PeekChar(0))
						If "0"c <= c AndAlso c <= "7"c Then
							value *= 8
							value += Convert.ToInt32(ReadChar()) - Convert.ToInt32("0"c)
						End If
					End If
					Return New StringElement(FixChar(Convert.ToChar(value)))
				Case "x"c
					str = ReadChar().ToString() + ReadChar().ToString()
					Try
						value = Int32.Parse(str, NumberStyles.AllowHexSpecifier)
						Return New StringElement(FixChar(Convert.ToChar(value)))
					Catch generatedExceptionName As FormatException
						Throw New RegExpException(RegExpException.ErrorType.UNSUPPORTED_ESCAPE_CHARACTER, pos - str.Length - 2, pattern)
					End Try
				Case "u"c
					str = ReadChar().ToString() + ReadChar().ToString() + ReadChar().ToString() + ReadChar().ToString()
					Try
						value = Int32.Parse(str, NumberStyles.AllowHexSpecifier)
						Return New StringElement(FixChar(Convert.ToChar(value)))
					Catch generatedExceptionName As FormatException
						Throw New RegExpException(RegExpException.ErrorType.UNSUPPORTED_ESCAPE_CHARACTER, pos - str.Length - 2, pattern)
					End Try
				Case "t"c
					Return New StringElement(Chr(9))
				Case "n"c
					Return New StringElement(Chr(10))
				Case "r"c
					Return New StringElement(Chr(13))
				Case "f"c
					Return New StringElement(Chr(12))
				Case "a"c
					Return New StringElement(Chr(7))
				Case "e"c
					Return New StringElement(Chr(27))
				Case "d"c
					Return CharacterSetElement.DIGIT
				Case "D"c
					Return CharacterSetElement.NON_DIGIT
				Case "s"c
					Return CharacterSetElement.WHITESPACE
				Case "S"c
					Return CharacterSetElement.NON_WHITESPACE
				Case "w"c
					Return CharacterSetElement.WORD
				Case "W"c
					Return CharacterSetElement.NON_WORD
				Case Else
					If ("A"c <= c AndAlso c <= "Z"c) OrElse ("a"c <= c AndAlso c <= "z"c) Then
						Throw New RegExpException(RegExpException.ErrorType.UNSUPPORTED_ESCAPE_CHARACTER, pos - 2, pattern)
					End If
					Return New StringElement(FixChar(c))
			End Select
		End Function

		'* 
		' * Adjusts a character for inclusion in a string or character 
		' * set element. For case-insensitive regular expressions, this 
		' * transforms the character to lower-case. 
		' * 
		' * @param c the input character 
		' * 
		' * @return the adjusted character 
		' 

		Private Function FixChar(ByVal c As Char) As Char
			Return IIf(ignoreCase, [Char].ToLower(c), c)
		End Function

		'* 
		' * Reads a number from the pattern. If the next character isn't a 
		' * numeric character, an exception is thrown. This method reads 
		' * several consecutive numeric characters. 
		' * 
		' * @return the numeric value read 
		' * 
		' * @throws RegExpException if an error was encountered in the 
		' * pattern string 
		' 

		Private Function ReadNumber() As Integer
			Dim buf As New StringBuilder()
			Dim c As Integer

			c = PeekChar(0)
			While Convert.ToInt32("0"c) <= c AndAlso c <= Convert.ToInt32("9"c)
				buf.Append(ReadChar())
				c = PeekChar(0)
			End While
			If buf.Length <= 0 Then
				Throw New RegExpException(RegExpException.ErrorType.UNEXPECTED_CHARACTER, pos, pattern)
			End If
			Return Int32.Parse(buf.ToString())
		End Function

		'* 
		' * Reads the next character in the pattern. If no next character 
		' * exists, an exception is thrown. 
		' * 
		' * @return the character read 
		' * 
		' * @throws RegExpException if no next character was available in 
		' * the pattern string 
		' 

		Private Function ReadChar() As Char
			Dim c As Integer = PeekChar(0)

			If c < 0 Then
				Throw New RegExpException(RegExpException.ErrorType.UNTERMINATED_PATTERN, pos, pattern)
			Else
				pos += 1
				Return Convert.ToChar(c)
			End If
		End Function

		'* 
		' * Reads the next character in the pattern. If the character 
		' * wasn't the specified one, an exception is thrown. 
		' * 
		' * @param c the character to read 
		' * 
		' * @return the character read 
		' * 
		' * @throws RegExpException if the character read didn't match the 
		' * specified one, or if no next character was 
		' * available in the pattern string 
		' 

		Private Function ReadChar(ByVal c As Char) As Char
			If c <> ReadChar() Then
				Throw New RegExpException(RegExpException.ErrorType.UNEXPECTED_CHARACTER, pos - 1, pattern)
			End If
			Return c
		End Function

		'* 
		' * Returns a character that has not yet been read from the 
		' * pattern. If the requested position is beyond the end of the 
		' * pattern string, -1 is returned. 
		' * 
		' * @param count the preview position, from zero (0) 
		' * 
		' * @return the character found, or 
		' * -1 if beyond the end of the pattern string 
		' 

		Private Function PeekChar(ByVal count As Integer) As Integer
			If pos + count < pattern.Length Then
				Return Convert.ToInt32(pattern(pos + count))
			Else
				Return -1
			End If
		End Function

		'* 
		' * Combines a list of elements. This method takes care to always 
		' * concatenate adjacent string elements into a single string 
		' * element. 
		' * 
		' * @param list the list with elements 
		' * 
		' * @return the combined element 
		' 

		Private Function CombineElements(ByVal list As ArrayList) As Element
			Dim prev As Element
			Dim elem As Element
			Dim str As String
			Dim i As Integer

			' Concatenate string elements 
			prev = DirectCast(list(0), Element)
			For i = 1 To list.Count - 2
				elem = DirectCast(list(i), Element)
				If TypeOf prev Is StringElement AndAlso TypeOf elem Is StringElement Then

					str = DirectCast(prev, StringElement).GetString() + DirectCast(elem, StringElement).GetString()
					elem = New StringElement(str)
					list.RemoveAt(i)
					list(i - 1) = elem
					i -= 1
				End If
				prev = elem
			Next

			' Combine all remaining elements 
			elem = DirectCast(list(list.Count - 1), Element)
			For i = list.Count - 2 To 0 Step -1
				prev = DirectCast(list(i), Element)
				elem = New CombineElement(prev, elem)
			Next

			Return elem
		End Function
	End Class
End Namespace 