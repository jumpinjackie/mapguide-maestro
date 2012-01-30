' 
' * Automaton.cs 
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
' * Copyright (c) 2004-2005 Per Cederberg. All rights reserved. 
' 

' Converted to VB.NET	[Eugene Ciloci; Nov 24, 2007]

Imports System 

Namespace PerCederberg.Grammatica.Runtime 
    
    '* 
' * A deterministic finite state automaton. This is a simple 
' * automaton for character sequences, currently used for string 
' * token patterns. It only handles single character transitions 
' * between states, but supports running in an all case-insensitive 
' * mode. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' * @since 1.5 
' 
    
    Friend Class Automaton 
        
        '* 
' * The state value. 
' 
        
		Private value As Object
        
        '* 
' * The automaton state transition tree. Each transition from 
' * this state to another state is added to this tree with the 
' * corresponding character. 
' 
        
        Private tree As New AutomatonTree() 
        
        '* 
' * Creates a new empty automaton. 
' 
        
        Public Sub New() 
        End Sub 
        
        '* 
' * Adds a string match to this automaton. New states and 
' * transitions will be added to extend this automaton to 
' * support the specified string. 
' * 
' * @param str the string to match 
' * @param caseInsensitive the case-insensitive flag 
' * @param value the match value 
' 
        
        Public Sub AddMatch(ByVal str As String, ByVal caseInsensitive As Boolean, ByVal value As Object) 
            Dim state As Automaton 
            
			If str.Length = 0 Then
				Me.value = value
			Else
				state = tree.Find(str(0), caseInsensitive)
				If state Is Nothing Then
					state = New Automaton()
					state.AddMatch(str.Substring(1), caseInsensitive, value)
					tree.Add(str(0), caseInsensitive, state)
				Else
					state.AddMatch(str.Substring(1), caseInsensitive, value)
				End If
			End If
        End Sub 
        
        '* 
' * Checks if the automaton matches an input stream. The 
' * matching will be performed from a specified position. This 
' * method will not read any characters from the stream, just 
' * peek ahead. The comparison can be done either in 
' * case-sensitive or case-insensitive mode. 
' * 
' * @param input the input stream to check 
' * @param pos the starting position 
' * @param caseInsensitive the case-insensitive flag 
' * 
' * @return the match value, or 
' * null if no match was found 
' * 
' * @throws IOException if an I/O error occurred 
' 
        
        Public Function MatchFrom(ByVal input As LookAheadReader, ByVal pos As Integer, ByVal caseInsensitive As Boolean) As Object 
            
            Dim result As Object = Nothing 
            Dim state As Automaton 
            Dim c As Integer 
            
            c = input.Peek(pos) 
            If tree IsNot Nothing AndAlso c >= 0 Then 
				state = tree.Find(Convert.ToChar(c), caseInsensitive)
                If state IsNot Nothing Then 
                    result = state.MatchFrom(input, pos + 1, caseInsensitive) 
                End If 
            End If 
            Return IIf((result Is Nothing),value,result) 
        End Function 
    End Class 
    
    '* 
' * An automaton state transition tree. This class contains a 
' * binary search tree for the automaton transitions from one state 
' * to another. All transitions are linked to a single character. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' * @since 1.5 
' 
    
    Friend Class AutomatonTree 
        
        '* 
' * The transition character. If this value is set to the zero 
' * character ('\0'), this tree is empty. 
' 
        
		Private value As Char
        
        '* 
' * The transition state. 
' 
        
		Private state As Automaton
        
        '* 
' * The left subtree. 
' 
        
		Private left As AutomatonTree
        
        '* 
' * The right subtree. 
' 
        
		Private right As AutomatonTree
        
        '* 
' * Creates a new empty automaton transition tree. 
' 
        
        Public Sub New() 
        End Sub 
        
        '* 
' * Finds an automaton state from the specified transition 
' * character. This method searches this transition tree for a 
' * matching transition. The comparison can optionally be done 
' * with a lower-case conversion of the character. 
' * 
' * @param c the character to search for 
' * @param lowerCase the lower-case conversion flag 
' * 
' * @return the automaton state found, or 
' * null if no transition exists 
' 
        
        Public Function Find(ByVal c As Char, ByVal lowerCase As Boolean) As Automaton 
            If lowerCase Then 
                c = [Char].ToLower(c) 
            End If 
            If value = Chr(0) OrElse value = c Then 
                Return state 
ElseIf value > c Then 
                Return left.Find(c, False) 
            Else 
                Return right.Find(c, False) 
            End If 
        End Function 
        
        '* 
' * Adds a transition to this tree. If the lower-case flag is 
' * set, the character will be converted to lower-case before 
' * being added. 
' * 
' * @param c the character to transition for 
' * @param lowerCase the lower-case conversion flag 
' * @param state the state to transition to 
' 
        
        Public Sub Add(ByVal c As Char, ByVal lowerCase As Boolean, ByVal state As Automaton) 
            If lowerCase Then 
                c = [Char].ToLower(c) 
            End If 
            If value = Chr(0) Then 
                Me.value = c 
                Me.state = state 
                Me.left = New AutomatonTree() 
                Me.right = New AutomatonTree() 
ElseIf value > c Then 
                left.Add(c, False, state) 
            Else 
                right.Add(c, False, state) 
            End If 
        End Sub 
    End Class 
End Namespace 