' 
' * RepeatElement.cs 
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

Imports Ciloci.Flee.PerCederberg.Grammatica.Runtime

Namespace PerCederberg.Grammatica.Runtime.RE 
    
    '* 
' * A regular expression element repeater. The element repeats the 
' * matches from a specified element, attempting to reach the 
' * maximum repetition count. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
    Friend Class RepeatElement 
        Inherits Element 
        
        '* 
' * The repeat type constants. 
' 
        
        Public Enum RepeatType 
            
            '* 
' * The greedy repeat type constant. 
' 
            
            GREEDY = 1 
            
            '* 
' * The reluctant repeat type constant. 
' 
            
            RELUCTANT = 2 
            
            '* 
' * The possesive repeat type constant. 
' 
            
            POSSESSIVE = 3 
        End Enum 
        
        '* 
' * The element to repeat. 
' 
        
        Private elem As Element 
        
        '* 
' * The minimum number of repetitions. 
' 
        
        Private min As Integer 
        
        '* 
' * The maximum number of repetitions. 
' 
        
        Private max As Integer 
        
        '* 
' * The repeat type. 
' 
        
        Private type As RepeatType 
        
        '* 
' * The start position of the last set of matches. 
' 
        
        Private matchStart As Integer 
        
        '* 
' * A set with all matches starting at matchStart. A match with 
' * a specific length is reported by a non-zero bit in the bit 
' * array. 
' 
        
        Private matches As BitArray 
        
        '* 
' * Creats a new element repeater. 
' * 
' * @param elem the element to repeat 
' * @param min the minimum count 
' * @param max the maximum count 
' * @param type the repeat type constant 
' 
        
        Public Sub New(ByVal elem As Element, ByVal min As Integer, ByVal max As Integer, ByVal type As RepeatType) 
            
            Me.elem = elem 
            Me.min = min 
            If max <= 0 Then 
                Me.max = Int32.MaxValue 
            Else 
                Me.max = max 
            End If 
            Me.type = type 
			Me.matchStart = -1
        End Sub 
        
        '* 
' * Creates a copy of this element. The copy will be an 
' * instance of the same class matching the same strings. 
' * Copies of elements are necessary to allow elements to cache 
' * intermediate results while matching strings without 
' * interfering with other threads. 
' * 
' * @return a copy of this element 
' 
        
        Public Overloads Overrides Function Clone() As Object 
            Return New RepeatElement(DirectCast(elem.Clone(), Element), min, max, type) 
        End Function 
        
        '* 
' * Returns the length of a matching string starting at the 
' * specified position. The number of matches to skip can also be 
' * specified. 
' * 
' * @param m the matcher being used 
' * @param input the input character stream to match 
' * @param start the starting position 
' * @param skip the number of matches to skip 
' * 
' * @return the length of the matching string, or 
' * -1 if no match was found 
' * 
' * @throws IOException if an I/O error occurred 
' 
        
        Public Overloads Overrides Function Match(ByVal m As Matcher, ByVal input As LookAheadReader, ByVal start As Integer, ByVal skip As Integer) As Integer 
            
            If skip = 0 Then 
                matchStart = -1 
                matches = Nothing 
            End If 
            Select Case type 
                Case RepeatType.GREEDY 
                    Return MatchGreedy(m, input, start, skip) 
                Case RepeatType.RELUCTANT 
                    Return MatchReluctant(m, input, start, skip) 
                Case RepeatType.POSSESSIVE 
                    If skip = 0 Then 
                        Return MatchPossessive(m, input, start, 0) 
                    End If 
                    Exit Select 
            End Select 
            Return -1 
        End Function 
        
        '* 
' * Returns the length of the longest possible matching string 
' * starting at the specified position. The number of matches 
' * to skip can also be specified. 
' * 
' * @param m the matcher being used 
' * @param input the input character stream to match 
' * @param start the starting position 
' * @param skip the number of matches to skip 
' * 
' * @return the length of the longest matching string, or 
' * -1 if no match was found 
' * 
' * @throws IOException if an I/O error occurred 
' 
        
        Private Function MatchGreedy(ByVal m As Matcher, ByVal input As LookAheadReader, ByVal start As Integer, ByVal skip As Integer) As Integer 
            
            ' Check for simple case 
            If skip = 0 Then 
                Return MatchPossessive(m, input, start, 0) 
            End If 
            
            ' Find all matches 
            If matchStart <> start Then 
                matchStart = start 
                matches = New BitArray(10) 
                FindMatches(m, input, start, 0, 0, 0) 
            End If 
            For i As Integer = matches.Count - 1 To 0 Step -1 
                
                ' Find first non-skipped match 
                If matches(i) Then 
                    If skip = 0 Then 
                        Return i 
                    End If 
                    skip -= 1 
                End If 
            Next 
            Return -1 
        End Function 
        
        '* 
' * Returns the length of the shortest possible matching string 
' * starting at the specified position. The number of matches to 
' * skip can also be specified. 
' * 
' * @param m the matcher being used 
' * @param input the input character stream to match 
' * @param start the starting position 
' * @param skip the number of matches to skip 
' * 
' * @return the length of the shortest matching string, or 
' * -1 if no match was found 
' * 
' * @throws IOException if an I/O error occurred 
' 
        
        Private Function MatchReluctant(ByVal m As Matcher, ByVal input As LookAheadReader, ByVal start As Integer, ByVal skip As Integer) As Integer 
            
            ' Find all matches 
            If matchStart <> start Then 
                matchStart = start 
                matches = New BitArray(10) 
                FindMatches(m, input, start, 0, 0, 0) 
            End If 
            For i As Integer = 0 To matches.Count - 1 
                
                ' Find first non-skipped match 
                If matches(i) Then 
                    If skip = 0 Then 
                        Return i 
                    End If 
                    skip -= 1 
                End If 
            Next 
            Return -1 
        End Function 
        
        '* 
' * Returns the length of the maximum number of elements matching 
' * the string starting at the specified position. This method 
' * allows no backtracking, i.e. no skips.. 
' * 
' * @param m the matcher being used 
' * @param input the input character stream to match 
' * @param start the starting position 
' * @param count the start count, normally zero (0) 
' * 
' * @return the length of the longest matching string, or 
' * -1 if no match was found 
' * 
' * @throws IOException if an I/O error occurred 
' 
        
        Private Function MatchPossessive(ByVal m As Matcher, ByVal input As LookAheadReader, ByVal start As Integer, ByVal count As Integer) As Integer 
            
            Dim length As Integer = 0 
            Dim subLength As Integer = 1 
            
            ' Match as many elements as possible 
            While subLength > 0 AndAlso count < max 
                subLength = elem.Match(m, input, start + length, 0) 
                If subLength >= 0 Then 
                    count += 1 
                    length += subLength 
                End If 
            End While 
            
            ' Return result 
            If min <= count AndAlso count <= max Then 
                Return length 
            Else 
                Return -1 
            End If 
        End Function 
        
        '* 
' * Finds all matches and adds the lengths to the matches set. 
' * 
' * @param m the matcher being used 
' * @param input the input character stream to match 
' * @param start the starting position 
' * @param length the match length at the start position 
' * @param count the number of sub-elements matched 
' * @param attempt the number of match attempts here 
' * 
' * @throws IOException if an I/O error occurred 
' 
        
        Private Sub FindMatches(ByVal m As Matcher, ByVal input As LookAheadReader, ByVal start As Integer, ByVal length As Integer, ByVal count As Integer, ByVal attempt As Integer) 
            
            Dim subLength As Integer 
            
            ' Check match ending here 
            If count > max Then 
                Return 
            End If 
            If min <= count AndAlso attempt = 0 Then 
                If matches.Length <= length Then 
                    matches.Length = length + 10 
                End If 
                matches(length) = True 
            End If 
            
            ' Check element match 
            subLength = elem.Match(m, input, start, attempt) 
            If subLength < 0 Then 
                Return 
ElseIf subLength = 0 Then 
                If min = count + 1 Then 
                    If matches.Length <= length Then 
                        matches.Length = length + 10 
                    End If 
                    matches(length) = True 
                End If 
                Return 
            End If 
            
            ' Find alternative and subsequent matches 
            FindMatches(m, input, start, length, count, attempt + 1) 
            FindMatches(m, input, start + subLength, length + subLength, count + 1, 0) 
        End Sub 
        
        '* 
' * Prints this element to the specified output stream. 
' * 
' * @param output the output stream to use 
' * @param indent the current indentation 
' 
        
        Public Overloads Overrides Sub PrintTo(ByVal output As TextWriter, ByVal indent As String) 
            output.Write(indent + "Repeat (" + min + "," + max + ")") 
            If type = RepeatType.RELUCTANT Then 
                output.Write("?") 
ElseIf type = RepeatType.POSSESSIVE Then 
                output.Write("+") 
            End If 
            output.WriteLine() 
            elem.PrintTo(output, indent + " ") 
        End Sub 
    End Class 
End Namespace 