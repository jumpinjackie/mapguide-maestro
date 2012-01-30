' 
' * ProductionPatternAlternative.cs 
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
' * A production pattern alternative. This class represents a list of 
' * production pattern elements. In order to provide productions that 
' * cannot be represented with the element occurance counters, multiple 
' * alternatives must be created and added to the same production 
' * pattern. A production pattern alternative is always contained 
' * within a production pattern. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
	Friend Class ProductionPatternAlternative

		'* 
		' * The production pattern. 
		' 

		Private m_pattern As ProductionPattern

		'* 
		' * The element list. 
		' 

		Private elements As New ArrayList()

		'* 
		' * The look-ahead set associated with this alternative. 
		' 

		Private m_lookAhead As LookAheadSet

		'* 
		' * Creates a new production pattern alternative. 
		' 

		Public Sub New()
		End Sub

		'* 
		' * The production pattern property (read-only). This property 
		' * contains the pattern having this alternative. 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Pattern() As ProductionPattern
			Get
				Return m_pattern
			End Get
		End Property

		'* 
		' * Returns the production pattern containing this alternative. 
		' * 
		' * @return the production pattern for this alternative 
		' * 
		' * @see #Pattern 
		' * 
		' * @deprecated Use the Pattern property instead. 
		' 

		Public Function GetPattern() As ProductionPattern
			Return Pattern
		End Function

		'* 
		' * The look-ahead set property. This property contains the 
		' * look-ahead set associated with this alternative. 
		' 

		Friend Property LookAhead() As LookAheadSet
			Get
				Return m_lookAhead
			End Get
			Set(ByVal value As LookAheadSet)
				m_lookAhead = Value
			End Set
		End Property

		'* 
		' * The production pattern element count property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Count() As Integer
			Get
				Return elements.Count
			End Get
		End Property

		'* 
		' * Returns the number of elements in this alternative. 
		' * 
		' * @return the number of elements in this alternative 
		' * 
		' * @see #Count 
		' * 
		' * @deprecated Use the Count property instead. 
		' 

		Public Function GetElementCount() As Integer
			Return Count
		End Function

		'* 
		' * The production pattern element index (read-only). 
		' * 
		' * @param index the element index, 0 <= pos < Count 
		' * 
		' * @return the element found 
		' * 
		' * @since 1.5 
		' 

		Default Public ReadOnly Property Item(ByVal index As Integer) As ProductionPatternElement
			Get
				Return DirectCast(elements(index), ProductionPatternElement)
			End Get
		End Property

		'* 
		' * Returns an element in this alternative. 
		' * 
		' * @param pos the element position, 0 <= pos < count 
		' * 
		' * @return the element found 
		' * 
		' * @deprecated Use the class indexer instead. 
		' 

		Public Function GetElement(ByVal pos As Integer) As ProductionPatternElement
			Return Me(pos)
		End Function

		'* 
		' * Checks if this alternative is recursive on the left-hand 
		' * side. This method checks all the possible left side 
		' * elements and returns true if the pattern itself is among 
		' * them. 
		' * 
		' * @return true if the alternative is left side recursive, or 
		' * false otherwise 
		' 

		Public Function IsLeftRecursive() As Boolean
			Dim elem As ProductionPatternElement
			For i As Integer = 0 To elements.Count - 1

				elem = DirectCast(elements(i), ProductionPatternElement)
				If elem.Id = m_pattern.Id Then
					Return True
				ElseIf elem.MinCount > 0 Then
					Exit For
				End If
			Next
			Return False
		End Function

		'* 
		' * Checks if this alternative is recursive on the right-hand side. 
		' * This method checks all the possible right side elements and 
		' * returns true if the pattern itself is among them. 
		' * 
		' * @return true if the alternative is right side recursive, or 
		' * false otherwise 
		' 

		Public Function IsRightRecursive() As Boolean
			Dim elem As ProductionPatternElement
			For i As Integer = elements.Count - 1 To 0 Step -1

				elem = DirectCast(elements(i), ProductionPatternElement)
				If elem.Id = m_pattern.Id Then
					Return True
				ElseIf elem.MinCount > 0 Then
					Exit For
				End If
			Next
			Return False
		End Function

		'* 
		' * Checks if this alternative would match an empty stream of 
		' * tokens. This check is equivalent of getMinElementCount() 
		' * returning zero (0). 
		' * 
		' * @return true if the rule can match an empty token stream, or 
		' * false otherwise 
		' 

		Public Function IsMatchingEmpty() As Boolean
			Return GetMinElementCount() = 0
		End Function

		'* 
		' * Changes the production pattern containing this alternative. 
		' * This method should only be called by the production pattern 
		' * class. 
		' * 
		' * @param pattern the new production pattern 
		' 

		Friend Sub SetPattern(ByVal pattern As ProductionPattern)
			Me.m_pattern = pattern
		End Sub

		'* 
		' * Returns the minimum number of elements needed to satisfy 
		' * this alternative. The value returned is the sum of all the 
		' * elements minimum count. 
		' * 
		' * @return the minimum number of elements 
		' 

		Public Function GetMinElementCount() As Integer
			Dim elem As ProductionPatternElement
			Dim min As Integer = 0
			For i As Integer = 0 To elements.Count - 1

				elem = DirectCast(elements(i), ProductionPatternElement)
				min += elem.MinCount
			Next
			Return min
		End Function

		'* 
		' * Returns the maximum number of elements needed to satisfy 
		' * this alternative. The value returned is the sum of all the 
		' * elements maximum count. 
		' * 
		' * @return the maximum number of elements 
		' 

		Public Function GetMaxElementCount() As Integer
			Dim elem As ProductionPatternElement
			Dim max As Integer = 0
			For i As Integer = 0 To elements.Count - 1

				elem = DirectCast(elements(i), ProductionPatternElement)
				If elem.MaxCount >= Int32.MaxValue Then
					Return Int32.MaxValue
				Else
					max += elem.MaxCount
				End If
			Next
			Return max
		End Function

		'* 
		' * Adds a token to this alternative. The token is appended to 
		' * the end of the element list. The multiplicity values 
		' * specified define if the token is optional or required, and 
		' * if it can be repeated. 
		' * 
		' * @param id the token (pattern) id 
		' * @param min the minimum number of occurancies 
		' * @param max the maximum number of occurancies, or 
		' * -1 for infinite 
		' 

		Public Sub AddToken(ByVal id As Integer, ByVal min As Integer, ByVal max As Integer)
			AddElement(New ProductionPatternElement(True, id, min, max))
		End Sub

		'* 
		' * Adds a production to this alternative. The production is 
		' * appended to the end of the element list. The multiplicity 
		' * values specified define if the production is optional or 
		' * required, and if it can be repeated. 
		' * 
		' * @param id the production (pattern) id 
		' * @param min the minimum number of occurancies 
		' * @param max the maximum number of occurancies, or 
		' * -1 for infinite 
		' 

		Public Sub AddProduction(ByVal id As Integer, ByVal min As Integer, ByVal max As Integer)
			AddElement(New ProductionPatternElement(False, id, min, max))
		End Sub

		'* 
		' * Adds a production pattern element to this alternative. The 
		' * element is appended to the end of the element list. 
		' * 
		' * @param elem the production pattern element 
		' 

		Public Sub AddElement(ByVal elem As ProductionPatternElement)
			elements.Add(elem)
		End Sub

		'* 
		' * Adds a production pattern element to this alternative. The 
		' * multiplicity values in the element will be overridden with 
		' * the specified values. The element is appended to the end of 
		' * the element list. 
		' * 
		' * @param elem the production pattern element 
		' * @param min the minimum number of occurancies 
		' * @param max the maximum number of occurancies, or 
		' * -1 for infinite 
		' 

		Public Sub AddElement(ByVal elem As ProductionPatternElement, ByVal min As Integer, ByVal max As Integer)

			If elem.IsToken() Then
				AddToken(elem.Id, min, max)
			Else
				AddProduction(elem.Id, min, max)
			End If
		End Sub

		'* 
		' * Checks if this object is equal to another. This method only 
		' * returns true for another production pattern alternative 
		' * with identical elements in the same order. 
		' * 
		' * @param obj the object to compare with 
		' * 
		' * @return true if the object is identical to this one, or 
		' * false otherwise 
		' 

		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			If TypeOf obj Is ProductionPatternAlternative Then
				Return Equals(DirectCast(obj, ProductionPatternAlternative))
			Else
				Return False
			End If
		End Function

		'* 
		' * Checks if this alternative is equal to another. This method 
		' * returns true if the other production pattern alternative 
		' * has identical elements in the same order. 
		' * 
		' * @param alt the alternative to compare with 
		' * 
		' * @return true if the object is identical to this one, or 
		' * false otherwise 
		' 

		Public Overloads Function Equals(ByVal alt As ProductionPatternAlternative) As Boolean
			If elements.Count <> alt.elements.Count Then
				Return False
			End If
			For i As Integer = 0 To elements.Count - 1
				If Not elements(i).Equals(alt.elements(i)) Then
					Return False
				End If
			Next
			Return True
		End Function

		'* 
		' * Returns a string representation of this object. 
		' * 
		' * @return a token string representation 
		' 

		Public Overloads Overrides Function ToString() As String
			Dim buffer As New StringBuilder()
			For i As Integer = 0 To elements.Count - 1

				If i > 0 Then
					buffer.Append(" ")
				End If
				buffer.Append(elements(i))
			Next
			Return buffer.ToString()
		End Function
	End Class
End Namespace 