' 
' * Matcher.cs 
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

Imports System.IO 

Imports Ciloci.Flee.PerCederberg.Grammatica.Runtime

Namespace PerCederberg.Grammatica.Runtime.RE 
    
    '* 
' * A regular expression string matcher. This class handles the 
' * matching of a specific string with a specific regular 
' * expression. It contains state information about the matching 
' * process, as for example the position of the latest match, and a 
' * number of flags that were set. This class is not thread-safe. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
	Friend Class Matcher

		'* 
		' * The base regular expression element. 
		' 

		Private element As Element

		'* 
		' * The input character stream to work with. 
		' 

		Private input As LookAheadReader

		'* 
		' * The character case ignore flag. 
		' 

		Private ignoreCase As Boolean

		'* 
		' * The start of the latest match found. 
		' 

		Private m_start As Integer

		'* 
		' * The length of the latest match found. 
		' 

		Private m_length As Integer

		'* 
		' * The end of string reached flag. This flag is set if the end 
		' * of the string was encountered during the latest match. 
		' 

		Private endOfString As Boolean

		'* 
		' * Creates a new matcher with the specified element. 
		' * 
		' * @param e the base regular expression element 
		' * @param input the input character stream to work with 
		' * @param ignoreCase the character case ignore flag 
		' 

		Friend Sub New(ByVal e As Element, ByVal input As LookAheadReader, ByVal ignoreCase As Boolean)
			Me.element = e
			Me.input = input
			Me.ignoreCase = ignoreCase
			Reset()
		End Sub

		'* 
		' * Checks if this matcher compares in case-insensitive mode. 
		' * 
		' * @return true if the matching is case-insensitive, or 
		' * false otherwise 
		' * 
		' * @since 1.5 
		' 

		Public Function IsCaseInsensitive() As Boolean
			Return ignoreCase
		End Function

		'* 
		' * Resets the information about the last match. This will 
		' * clear all flags and set the match length to a negative 
		' * value. This method is automatically called before starting 
		' * a new match. 
		' 

		Public Sub Reset()
			m_length = -1
			endOfString = False
		End Sub

		'* 
		' * Resets the matcher for use with a new input string. This 
		' * will clear all flags and set the match length to a negative 
		' * value. 
		' * 
		' * @param str the new string to work with 
		' * 
		' * @since 1.5 
		' 

		Public Sub Reset(ByVal str As String)
			Reset(New StringReader(str))
		End Sub

		'* 
		' * Resets the matcher for use with a new character input 
		' * stream. This will clear all flags and set the match length 
		' * to a negative value. 
		' * 
		' * @param input the character input stream 
		' * 
		' * @since 1.5 
		' 

		Public Sub Reset(ByVal input As TextReader)
			If TypeOf input Is LookAheadReader Then
				Reset(DirectCast(input, LookAheadReader))
			Else
				Reset(New LookAheadReader(input))
			End If
		End Sub

		'* 
		' * Resets the matcher for use with a new look-ahead character 
		' * input stream. This will clear all flags and set the match 
		' * length to a negative value. 
		' * 
		' * @param input the character input stream 
		' * 
		' * @since 1.5 
		' 

		Private Sub Reset(ByVal input As LookAheadReader)
			Me.input = input
			Reset()
		End Sub

		'* 
		' * Returns the start position of the latest match. If no match 
		' * has been encountered, this method returns zero (0). 
		' * 
		' * @return the start position of the latest match 
		' 

		Public Function Start() As Integer
			Return m_start
		End Function

		'* 
		' * Returns the end position of the latest match. This is one 
		' * character after the match end, i.e. the first character 
		' * after the match. If no match has been encountered, this 
		' * method returns the same value as start(). 
		' * 
		' * @return the end position of the latest match 
		' 

		Public Function [End]() As Integer
			If m_length > 0 Then
				Return m_start + m_length
			Else
				Return m_start
			End If
		End Function

		'* 
		' * Returns the length of the latest match. 
		' * 
		' * @return the length of the latest match, or 
		' * -1 if no match was found 
		' 

		Public Function Length() As Integer
			Return m_length
		End Function

		'* 
		' * Checks if the end of the string was encountered during the 
		' * last match attempt. This flag signals that more input may 
		' * be needed in order to get a match (or a longer match). 
		' * 
		' * @return true if the end of string was encountered, or 
		' * false otherwise 
		' 

		Public Function HasReadEndOfString() As Boolean
			Return endOfString
		End Function

		'* 
		' * Attempts to find a match starting at the beginning of the 
		' * string. 
		' * 
		' * @return true if a match was found, or 
		' * false otherwise 
		' * 
		' * @throws IOException if an I/O error occurred while reading 
		' * an input stream 
		' 

		Public Function MatchFromBeginning() As Boolean
			Return MatchFrom(0)
		End Function

		'* 
		' * Attempts to find a match starting at the specified position 
		' * in the string. 
		' * 
		' * @param pos the starting position of the match 
		' * 
		' * @return true if a match was found, or 
		' * false otherwise 
		' * 
		' * @throws IOException if an I/O error occurred while reading 
		' * an input stream 
		' 

		Public Function MatchFrom(ByVal pos As Integer) As Boolean
			Reset()
			m_start = pos
			m_length = element.Match(Me, input, m_start, 0)
			Return m_length >= 0
		End Function

		'* 
		' * Returns the latest matched string. If no string has been 
		' * matched, an empty string will be returned. 
		' * 
		' * @return the latest matched string 
		' 

		Public Overloads Overrides Function ToString() As String
			If m_length <= 0 Then
				Return ""
			Else
				Try
					Return input.PeekString(m_start, m_length)
				Catch ignore As IOException
					Return ""
				End Try
			End If
		End Function

		'* 
		' * Sets the end of string encountered flag. This method is 
		' * called by the various elements analyzing the string. 
		' 

		Friend Sub SetReadEndOfString()
			endOfString = True
		End Sub
	End Class
End Namespace 