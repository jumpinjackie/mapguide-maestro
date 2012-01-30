' 
' * StringElement.cs 
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
Imports System.IO 

Imports Ciloci.Flee.PerCederberg.Grammatica.Runtime

Namespace PerCederberg.Grammatica.Runtime.RE 
    
    '* 
' * A regular expression string element. This element only matches 
' * an exact string. Once created, the string element is immutable. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
    Friend Class StringElement 
        Inherits Element 
        
        '* 
' * The string to match with. 
' 
        
        Private value As String 
        
        '* 
' * Creates a new string element. 
' * 
' * @param c the character to match with 
' 
        
        Public Sub New(ByVal c As Char) 
            Me.New(c.ToString()) 
        End Sub 
        
        '* 
' * Creates a new string element. 
' * 
' * @param str the string to match with 
' 
        
        Public Sub New(ByVal str As String) 
            value = str 
        End Sub 
        
        '* 
' * Returns the string to be matched. 
' * 
' * @return the string to be matched 
' 
        
        Public Function GetString() As String 
            Return value 
        End Function 
        
        '* 
' * Returns this element as it is immutable. 
' * 
' * @return this string element 
' 
        
        Public Overloads Overrides Function Clone() As Object 
            Return Me 
        End Function 
        
        '* 
' * Returns the length of a matching string starting at the 
' * specified position. The number of matches to skip can also 
' * be specified, but numbers higher than zero (0) cause a 
' * failed match for any element that doesn't attempt to 
' * combine other elements. 
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
        
        Public Overloads Overrides Function Match(ByVal m As Matcher, ByVal input As LookAheadReader, ByVal start As Integer, ByVal skip As Integer) As Integer 
            
            Dim c As Integer 
            
            If skip <> 0 Then 
                Return -1 
            End If 
            For i As Integer = 0 To value.Length - 1 
                c = input.Peek(start + i) 
                If c < 0 Then 
                    m.SetReadEndOfString() 
                    Return -1 
                End If 
                If m.IsCaseInsensitive() Then 
					c = Convert.ToInt32([Char].ToLower(Convert.ToChar(c)))
                End If 
				If c <> Convert.ToInt32(value(i)) Then
					Return -1
				End If
            Next 
            Return value.Length 
        End Function 
        
        '* 
' * Prints this element to the specified output stream. 
' * 
' * @param output the output stream to use 
' * @param indent the current indentation 
' 
        
        Public Overloads Overrides Sub PrintTo(ByVal output As TextWriter, ByVal indent As String) 
            output.WriteLine(indent + "'" + value + "'") 
        End Sub 
    End Class 
End Namespace 