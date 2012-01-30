' 
' * LookAheadSet.cs 
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
Imports System.Text 

Namespace PerCederberg.Grammatica.Runtime 
    
    '* 
' * A token look-ahead set. This class contains a set of token id 
' * sequences. All sequences in the set are limited in length, so 
' * that no single sequence is longer than a maximum value. This 
' * class also filters out duplicates. Each token sequence also 
' * contains a repeat flag, allowing the look-ahead set to contain 
' * information about possible infinite repetitions of certain 
' * sequences. That information is important when conflicts arise 
' * between two look-ahead sets, as such a conflict cannot be 
' * resolved if the conflicting sequences can be repeated (would 
' * cause infinite loop). 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.1 
' 
    
    Friend Class LookAheadSet 
        
        '* 
' * The set of token look-ahead sequences. Each sequence in 
' * turn is represented by an ArrayList with Integers for the 
' * token id:s. 
' 
        
        Private elements As New ArrayList() 
        
        '* 
' * The maximum length of any look-ahead sequence. 
' 
        
        Private maxLength As Integer 
        
        '* 
' * Creates a new look-ahead set with the specified maximum 
' * length. 
' * 
' * @param maxLength the maximum token sequence length 
' 
        
        Public Sub New(ByVal maxLength As Integer) 
            Me.maxLength = maxLength 
        End Sub 
        
        '* 
' * Creates a duplicate look-ahead set, possibly with a 
' * different maximum length. 
' * 
' * @param maxLength the maximum token sequence length 
' * @param set the look-ahead set to copy 
' 
        
        Public Sub New(ByVal maxLength As Integer, ByVal [set] As LookAheadSet) 
            Me.New(maxLength) 
            
            AddAll([set]) 
        End Sub 
        
        '* 
' * Returns the size of this look-ahead set. 
' * 
' * @return the number of token sequences in the set 
' 
        
        Public Function Size() As Integer 
            Return elements.Count 
        End Function 
        
        '* 
' * Returns the length of the shortest token sequence in this 
' * set. This method will return zero (0) if the set is empty. 
' * 
' * @return the length of the shortest token sequence 
' 
        
        Public Function GetMinLength() As Integer 
            Dim seq As Sequence 
            Dim min As Integer = -1 
            For i As Integer = 0 To elements.Count - 1 
                
                seq = DirectCast(elements(i), Sequence) 
                If min < 0 OrElse seq.Length() < min Then 
                    min = seq.Length() 
                End If 
            Next 
            Return IIf((min < 0),0,min) 
        End Function 
        
        '* 
' * Returns the length of the longest token sequence in this 
' * set. This method will return zero (0) if the set is empty. 
' * 
' * @return the length of the longest token sequence 
' 
        
        Public Function GetMaxLength() As Integer 
            Dim seq As Sequence 
            Dim max As Integer = 0 
            For i As Integer = 0 To elements.Count - 1 
                
                seq = DirectCast(elements(i), Sequence) 
                If seq.Length() > max Then 
                    max = seq.Length() 
                End If 
            Next 
            Return max 
        End Function 
        
        '* 
' * Returns a list of the initial token id:s in this look-ahead 
' * set. The list returned will not contain any duplicates. 
' * 
' * @return a list of the inital token id:s in this look-ahead set 
' 
        
        Public Function GetInitialTokens() As Integer() 
            Dim list As New ArrayList() 
            Dim result As Integer() 
            Dim token As Object 
            Dim i As Integer 
            For i = 0 To elements.Count - 1 
                
                token = DirectCast(elements(i), Sequence).GetToken(0) 
                If token IsNot Nothing AndAlso Not list.Contains(token) Then 
                    list.Add(token) 
                End If 
            Next 
            result = New Integer(list.Count - 1) {} 
            For i = 0 To list.Count - 1 
                result(i) = CInt(list(i)) 
            Next 
            Return result 
        End Function 
        
        '* 
' * Checks if this look-ahead set contains a repetitive token 
' * sequence. 
' * 
' * @return true if at least one token sequence is repetitive, or 
' * false otherwise 
' 
        
        Public Function IsRepetitive() As Boolean 
            Dim seq As Sequence 
            For i As Integer = 0 To elements.Count - 1 
                
                seq = DirectCast(elements(i), Sequence) 
                If seq.IsRepetitive() Then 
                    Return True 
                End If 
            Next 
            Return False 
        End Function 
        
        '* 
' * Checks if the next token(s) in the parser match any token 
' * sequence in this set. 
' * 
' * @param parser the parser to check 
' * 
' * @return true if the next tokens are in the set, or 
' * false otherwise 
' 
        
        Public Function IsNext(ByVal parser As Parser) As Boolean 
            Dim seq As Sequence 
            For i As Integer = 0 To elements.Count - 1 
                
                seq = DirectCast(elements(i), Sequence) 
                If seq.IsNext(parser) Then 
                    Return True 
                End If 
            Next 
            Return False 
        End Function 
        
        '* 
' * Checks if the next token(s) in the parser match any token 
' * sequence in this set. 
' * 
' * @param parser the parser to check 
' * @param length the maximum number of tokens to check 
' * 
' * @return true if the next tokens are in the set, or 
' * false otherwise 
' 
        
        Public Function IsNext(ByVal parser As Parser, ByVal length As Integer) As Boolean 
            Dim seq As Sequence 
            For i As Integer = 0 To elements.Count - 1 
                
                seq = DirectCast(elements(i), Sequence) 
                If seq.IsNext(parser, length) Then 
                    Return True 
                End If 
            Next 
            Return False 
        End Function 
        
        '* 
' * Checks if another look-ahead set has an overlapping token 
' * sequence. An overlapping token sequence is a token sequence 
' * that is identical to another sequence, but for the length. 
' * I.e. one of the two sequences may be longer than the other. 
' * 
' * @param set the look-ahead set to check 
' * 
' * @return true if there is some token sequence that overlaps, or 
' * false otherwise 
' 
        
        Public Function IsOverlap(ByVal [set] As LookAheadSet) As Boolean 
            For i As Integer = 0 To elements.Count - 1 
                If [set].IsOverlap(DirectCast(elements(i), Sequence)) Then 
                    Return True 
                End If 
            Next 
            Return False 
        End Function 
        
        '* 
' * Checks if a token sequence is overlapping. An overlapping token 
' * sequence is a token sequence that is identical to another 
' * sequence, but for the length. I.e. one of the two sequences may 
' * be longer than the other. 
' * 
' * @param seq the token sequence to check 
' * 
' * @return true if there is some token sequence that overlaps, or 
' * false otherwise 
' 
        
        Private Function IsOverlap(ByVal seq As Sequence) As Boolean 
            Dim elem As Sequence 
            For i As Integer = 0 To elements.Count - 1 
                
                elem = DirectCast(elements(i), Sequence) 
                If seq.StartsWith(elem) OrElse elem.StartsWith(seq) Then 
                    Return True 
                End If 
            Next 
            Return False 
        End Function 
        
        '* 
' * Checks if the specified token sequence is present in the 
' * set. 
' * 
' * @param elem the token sequence to check 
' * 
' * @return true if the sequence is present in this set, or 
' * false otherwise 
' 
        
        Private Function Contains(ByVal elem As Sequence) As Boolean 
            Return FindSequence(elem) IsNot Nothing 
        End Function 
        
        '* 
' * Checks if some token sequence is present in both this set 
' * and a specified one. 
' * 
' * @param set the look-ahead set to compare with 
' * 
' * @return true if the look-ahead sets intersect, or 
' * false otherwise 
' 
        
        Public Function Intersects(ByVal [set] As LookAheadSet) As Boolean 
            For i As Integer = 0 To elements.Count - 1 
                If [set].Contains(DirectCast(elements(i), Sequence)) Then 
                    Return True 
                End If 
            Next 
            Return False 
        End Function 
        
        '* 
' * Finds an identical token sequence if present in the set. 
' * 
' * @param elem the token sequence to search for 
' * 
' * @return an identical the token sequence if found, or 
' * null if not found 
' 
        
        Private Function FindSequence(ByVal elem As Sequence) As Sequence 
            For i As Integer = 0 To elements.Count - 1 
                If elements(i).Equals(elem) Then 
                    Return DirectCast(elements(i), Sequence) 
                End If 
            Next 
            Return Nothing 
        End Function 
        
        '* 
' * Adds a token sequence to this set. The sequence will only 
' * be added if it is not already in the set. Also, if the 
' * sequence is longer than the allowed maximum, a truncated 
' * sequence will be added instead. 
' * 
' * @param seq the token sequence to add 
' 
        
        Private Sub Add(ByVal seq As Sequence) 
            If seq.Length() > maxLength Then 
                seq = New Sequence(maxLength, seq) 
            End If 
            If Not Contains(seq) Then 
                elements.Add(seq) 
            End If 
        End Sub 
        
        '* 
' * Adds a new token sequence with a single token to this set. 
' * The sequence will only be added if it is not already in the 
' * set. 
' * 
' * @param token the token to add 
' 
        
        Public Sub Add(ByVal token As Integer) 
            Add(New Sequence(False, token)) 
        End Sub 
        
        '* 
' * Adds all the token sequences from a specified set. Only 
' * sequences not already in this set will be added. 
' * 
' * @param set the set to add from 
' 
        
        Public Sub AddAll(ByVal [set] As LookAheadSet) 
            For i As Integer = 0 To [set].elements.Count - 1 
                Add(DirectCast([set].elements(i), Sequence)) 
            Next 
        End Sub 
        
        '* 
' * Adds an empty token sequence to this set. The sequence will 
' * only be added if it is not already in the set. 
' 
        
        Public Sub AddEmpty() 
            Add(New Sequence()) 
        End Sub 
        
        '* 
' * Removes a token sequence from this set. 
' * 
' * @param seq the token sequence to remove 
' 
        
        Private Sub Remove(ByVal seq As Sequence) 
            elements.Remove(seq) 
        End Sub 
        
        '* 
' * Removes all the token sequences from a specified set. Only 
' * sequences already in this set will be removed. 
' * 
' * @param set the set to remove from 
' 
        
        Public Sub RemoveAll(ByVal [set] As LookAheadSet) 
            For i As Integer = 0 To [set].elements.Count - 1 
                Remove(DirectCast([set].elements(i), Sequence)) 
            Next 
        End Sub 
        
        '* 
' * Creates a new look-ahead set that is the result of reading 
' * the specified token. The new look-ahead set will contain 
' * the rest of all the token sequences that started with the 
' * specified token. 
' * 
' * @param token the token to read 
' * 
' * @return a new look-ahead set containing the remaining tokens 
' 
        
        Public Function CreateNextSet(ByVal token As Integer) As LookAheadSet 
            Dim result As New LookAheadSet(maxLength - 1) 
            Dim seq As Sequence 
            Dim value As Object 
            For i As Integer = 0 To elements.Count - 1 
                
                seq = DirectCast(elements(i), Sequence) 
                value = seq.GetToken(0) 
                If value IsNot Nothing AndAlso token = CInt(value) Then 
                    result.Add(seq.Subsequence(1)) 
                End If 
            Next 
            Return result 
        End Function 
        
        '* 
' * Creates a new look-ahead set that is the intersection of 
' * this set with another set. The token sequences in the net 
' * set will only have the repeat flag set if it was set in 
' * both the identical token sequences. 
' * 
' * @param set the set to intersect with 
' * 
' * @return a new look-ahead set containing the intersection 
' 
        
        Public Function CreateIntersection(ByVal [set] As LookAheadSet) As LookAheadSet 
            Dim result As New LookAheadSet(maxLength) 
            Dim seq1 As Sequence 
            Dim seq2 As Sequence 
            For i As Integer = 0 To elements.Count - 1 
                
                seq1 = DirectCast(elements(i), Sequence) 
                seq2 = [set].FindSequence(seq1) 
                If seq2 IsNot Nothing AndAlso seq1.IsRepetitive() Then 
                    result.Add(seq2) 
ElseIf seq2 IsNot Nothing Then 
                    result.Add(seq1) 
                End If 
            Next 
            Return result 
        End Function 
        
        '* 
' * Creates a new look-ahead set that is the combination of 
' * this set with another set. The combination is created by 
' * creating new token sequences that consist of appending all 
' * elements from the specified set onto all elements in this 
' * set. This is sometimes referred to as the cartesian 
' * product. 
' * 
' * @param set the set to combine with 
' * 
' * @return a new look-ahead set containing the combination 
' 
        
        Public Function CreateCombination(ByVal [set] As LookAheadSet) As LookAheadSet 
            Dim result As New LookAheadSet(maxLength) 
            Dim first As Sequence 
            Dim second As Sequence 
            
            ' Handle special cases 
            If Me.Size() <= 0 Then 
                Return [set] 
ElseIf [set].Size() <= 0 Then 
                Return Me 
            End If 
            For i As Integer = 0 To elements.Count - 1 
                
                ' Create combinations 
                first = DirectCast(elements(i), Sequence) 
                If first.Length() >= maxLength Then 
                    result.Add(first) 
ElseIf first.Length() <= 0 Then 
                    result.AddAll([set]) 
                Else 
                    For j As Integer = 0 To [set].elements.Count - 1 
                        second = DirectCast([set].elements(j), Sequence) 
                        result.Add(first.Concat(maxLength, second)) 
                    Next 
                End If 
            Next 
            Return result 
        End Function 
        
        '* 
' * Creates a new look-ahead set with overlaps from another. All 
' * token sequences in this set that overlaps with the other set 
' * will be added to the new look-ahead set. 
' * 
' * @param set the look-ahead set to check with 
' * 
' * @return a new look-ahead set containing the overlaps 
' 
        
        Public Function CreateOverlaps(ByVal [set] As LookAheadSet) As LookAheadSet 
            Dim result As New LookAheadSet(maxLength) 
            Dim seq As Sequence 
            For i As Integer = 0 To elements.Count - 1 
                
                seq = DirectCast(elements(i), Sequence) 
                If [set].IsOverlap(seq) Then 
                    result.Add(seq) 
                End If 
            Next 
            Return result 
        End Function 
        
        '* 
' * Creates a new look-ahead set filter. The filter will contain 
' * all sequences from this set, possibly left trimmed by each one 
' * of the sequences in the specified set. 
' * 
' * @param set the look-ahead set to trim with 
' * 
' * @return a new look-ahead set filter 
' 
        
        Public Function CreateFilter(ByVal [set] As LookAheadSet) As LookAheadSet 
            Dim result As New LookAheadSet(maxLength) 
            Dim first As Sequence 
            Dim second As Sequence 
            
            ' Handle special cases 
            If Me.Size() <= 0 OrElse [set].Size() <= 0 Then 
                Return Me 
            End If 
            For i As Integer = 0 To elements.Count - 1 
                
                ' Create combinations 
                first = DirectCast(elements(i), Sequence) 
                For j As Integer = 0 To [set].elements.Count - 1 
                    second = DirectCast([set].elements(j), Sequence) 
                    If first.StartsWith(second) Then 
                        result.Add(first.Subsequence(second.Length())) 
                    End If 
                Next 
            Next 
            Return result 
        End Function 
        
        '* 
' * Creates a new identical look-ahead set, except for the 
' * repeat flag being set in each token sequence. 
' * 
' * @return a new repetitive look-ahead set 
' 
        
        Public Function CreateRepetitive() As LookAheadSet 
            Dim result As New LookAheadSet(maxLength) 
            Dim seq As Sequence 
            For i As Integer = 0 To elements.Count - 1 
                
                seq = DirectCast(elements(i), Sequence) 
                If seq.IsRepetitive() Then 
                    result.Add(seq) 
                Else 
                    result.Add(New Sequence(True, seq)) 
                End If 
            Next 
            Return result 
        End Function 
        
        '* 
' * Returns a string representation of this object. 
' * 
' * @return a string representation of this object 
' 
        
        Public Overloads Overrides Function ToString() As String 
            Return ToString(Nothing) 
        End Function 
        
        '* 
' * Returns a string representation of this object. 
' * 
' * @param tokenizer the tokenizer containing the tokens 
' * 
' * @return a string representation of this object 
' 
        
		Public Overloads Function ToString(ByVal tokenizer As Tokenizer) As String
			Dim buffer As New StringBuilder()
			Dim seq As Sequence

			buffer.Append("{")
			For i As Integer = 0 To elements.Count - 1
				seq = DirectCast(elements(i), Sequence)
				buffer.Append("" & Chr(10) & " ")
				buffer.Append(seq.ToString(tokenizer))
			Next
			buffer.Append("" & Chr(10) & "}")
			Return buffer.ToString()
		End Function
        
        
        '* 
' * A token sequence. This class contains a list of token ids. 
' * It is immutable after creation, meaning that no changes 
' * will be made to an instance after creation. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.0 
' 
        
        Private Class Sequence 
            
            '* 
' * The repeat flag. If this flag is set, the token 
' * sequence or some part of it may be repeated infinitely. 
' 
            
			Private repeat As Boolean
            
            '* 
' * The list of token ids in this sequence. 
' 
            
			Private tokens As ArrayList
            
            '* 
' * Creates a new empty token sequence. The repeat flag 
' * will be set to false. 
' 
            
			Public Sub New()
				Me.tokens = New ArrayList(0)
			End Sub
            
            '* 
' * Creates a new token sequence with a single token. 
' * 
' * @param repeat the repeat flag value 
' * @param token the token to add 
' 
            
			Public Sub New(ByVal repeat As Boolean, ByVal token As Integer)
				Me.tokens = New ArrayList(1)
				Me.tokens.Add(token)
			End Sub
            
            '* 
' * Creates a new token sequence that is a duplicate of 
' * another sequence. Only a limited number of tokens will 
' * be copied however. The repeat flag from the original 
' * will be kept intact. 
' * 
' * @param length the maximum number of tokens to copy 
' * @param seq the sequence to copy 
' 
            
            Public Sub New(ByVal length As Integer, ByVal seq As Sequence) 
                Me.repeat = seq.repeat 
                Me.tokens = New ArrayList(length) 
                If seq.Length() < length Then 
                    length = seq.Length() 
                End If 
                For i As Integer = 0 To length - 1 
                    tokens.Add(seq.tokens(i)) 
                Next 
            End Sub 
            
            '* 
' * Creates a new token sequence that is a duplicate of 
' * another sequence. The new value of the repeat flag will 
' * be used however. 
' * 
' * @param repeat the new repeat flag value 
' * @param seq the sequence to copy 
' 
            
            Public Sub New(ByVal repeat As Boolean, ByVal seq As Sequence) 
                Me.repeat = repeat 
                Me.tokens = seq.tokens 
            End Sub 
            
            '* 
' * Returns the length of the token sequence. 
' * 
' * @return the number of tokens in the sequence 
' 
            
            Public Function Length() As Integer 
                Return tokens.Count 
            End Function 
            
            '* 
' * Returns a token at a specified position in the sequence. 
' * 
' * @param pos the sequence position 
' * 
' * @return the token id found, or null 
' 
            
            Public Function GetToken(ByVal pos As Integer) As Object 
                If pos >= 0 AndAlso pos < tokens.Count Then 
                    Return tokens(pos) 
                Else 
                    Return Nothing 
                End If 
            End Function 
            
            '* 
' * Checks if this sequence is equal to another object. 
' * Only token sequences with the same tokens in the same 
' * order will be considered equal. The repeat flag will be 
' * disregarded. 
' * 
' * @param obj the object to compare with 
' * 
' * @return true if the objects are equal, or 
' * false otherwise 
' 
            
            Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean 
                If TypeOf obj Is Sequence Then 
                    Return Equals(DirectCast(obj, Sequence)) 
                Else 
                    Return False 
                End If 
            End Function 
            
            '* 
' * Checks if this sequence is equal to another sequence. 
' * Only sequences with the same tokens in the same order 
' * will be considered equal. The repeat flag will be 
' * disregarded. 
' * 
' * @param seq the sequence to compare with 
' * 
' * @return true if the sequences are equal, or 
' * false otherwise 
' 
            
			Public Overloads Function Equals(ByVal seq As Sequence) As Boolean
				If tokens.Count <> seq.tokens.Count Then
					Return False
				End If
				For i As Integer = 0 To tokens.Count - 1
					If Not tokens(i).Equals(seq.tokens(i)) Then
						Return False
					End If
				Next
				Return True
			End Function
            
            '* 
' * Checks if this token sequence starts with the tokens from 
' * another sequence. If the other sequence is longer than this 
' * sequence, this method will always return false. 
' * 
' * @param seq the token sequence to check 
' * 
' * @return true if this sequence starts with the other, or 
' * false otherwise 
' 
            
            Public Function StartsWith(ByVal seq As Sequence) As Boolean 
                If Length() < seq.Length() Then 
                    Return False 
                End If 
                For i As Integer = 0 To seq.tokens.Count - 1 
                    If Not tokens(i).Equals(seq.tokens(i)) Then 
                        Return False 
                    End If 
                Next 
                Return True 
            End Function 
            
            '* 
' * Checks if this token sequence is repetitive. A repetitive 
' * token sequence is one with the repeat flag set. 
' * 
' * @return true if this token sequence is repetitive, or 
' * false otherwise 
' 
            
            Public Function IsRepetitive() As Boolean 
                Return repeat 
            End Function 
            
            '* 
' * Checks if the next token(s) in the parser matches this 
' * token sequence. 
' * 
' * @param parser the parser to check 
' * 
' * @return true if the next tokens are in the sequence, or 
' * false otherwise 
' 
            
            Public Function IsNext(ByVal parser As Parser) As Boolean 
                Dim token As Token 
                Dim id As Integer 
                For i As Integer = 0 To tokens.Count - 1 
                    
                    id = CInt(tokens(i)) 
                    token = parser.PeekToken(i) 
                    If token Is Nothing OrElse token.Id <> id Then 
                        Return False 
                    End If 
                Next 
                Return True 
            End Function 
            
            '* 
' * Checks if the next token(s) in the parser matches this 
' * token sequence. 
' * 
' * @param parser the parser to check 
' * @param length the maximum number of tokens to check 
' * 
' * @return true if the next tokens are in the sequence, or 
' * false otherwise 
' 
            
            Public Function IsNext(ByVal parser As Parser, ByVal length As Integer) As Boolean 
                Dim token As Token 
                Dim id As Integer 
                
                If length > tokens.Count Then 
                    length = tokens.Count 
                End If 
                For i As Integer = 0 To length - 1 
                    id = CInt(tokens(i)) 
                    token = parser.PeekToken(i) 
                    If token Is Nothing OrElse token.Id <> id Then 
                        Return False 
                    End If 
                Next 
                Return True 
            End Function 
            
            '* 
' * Returns a string representation of this object. 
' * 
' * @return a string representation of this object 
' 
            
            Public Overloads Overrides Function ToString() As String 
                Return ToString(Nothing) 
            End Function 
            
            '* 
' * Returns a string representation of this object. 
' * 
' * @param tokenizer the tokenizer containing the tokens 
' * 
' * @return a string representation of this object 
' 
            
			Public Overloads Function ToString(ByVal tokenizer As Tokenizer) As String
				Dim buffer As New StringBuilder()
				Dim str As String
				Dim id As Integer

				If tokenizer Is Nothing Then
					buffer.Append(tokens.ToString())
				Else
					buffer.Append("[")
					For i As Integer = 0 To tokens.Count - 1
						id = CInt(tokens(i))
						str = tokenizer.GetPatternDescription(id)
						If i > 0 Then
							buffer.Append(" ")
						End If
						buffer.Append(str)
					Next
					buffer.Append("]")
				End If
				If repeat Then
					buffer.Append(" *")
				End If
				Return buffer.ToString()
			End Function
            
            '* 
' * Creates a new token sequence that is the concatenation 
' * of this sequence and another. A maximum length for the 
' * new sequence is also specified. 
' * 
' * @param length the maximum length of the result 
' * @param seq the other sequence 
' * 
' * @return the concatenated token sequence 
' 
            
            Public Function Concat(ByVal length As Integer, ByVal seq As Sequence) As Sequence 
                Dim res As New Sequence(length, Me) 
                
                If seq.repeat Then 
                    res.repeat = True 
                End If 
                length -= Me.Length() 
                If length > seq.Length() Then 
                    res.tokens.AddRange(seq.tokens) 
                Else 
                    For i As Integer = 0 To length - 1 
                        res.tokens.Add(seq.tokens(i)) 
                    Next 
                End If 
                Return res 
            End Function 
            
            '* 
' * Creates a new token sequence that is a subsequence of 
' * this one. 
' * 
' * @param start the subsequence start position 
' * 
' * @return the new token subsequence 
' 
            
            Public Function Subsequence(ByVal start As Integer) As Sequence 
                Dim res As New Sequence(Length(), Me) 
                
                While start > 0 AndAlso res.tokens.Count > 0 
                    res.tokens.RemoveAt(0) 
                    start -= 1 
                End While 
                Return res 
            End Function 
        End Class 
    End Class 
End Namespace 