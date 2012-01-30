' 
' * ProductionPatternElement.cs 
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
Imports System.Text 

Namespace PerCederberg.Grammatica.Runtime 
    
    '* 
' * A production pattern element. This class represents a reference to 
' * either a token or a production. Each element also contains minimum 
' * and maximum occurence counters, controlling the number of 
' * repetitions allowed. A production pattern element is always 
' * contained within a production pattern rule. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
	Friend Class ProductionPatternElement

		'* 
		' * The token flag. This flag is true for token elements, and 
		' * false for production elements. 
		' 

		Private token As Boolean

		'* 
		' * The node identity. 
		' 

		Private m_id As Integer

		'* 
		' * The minimum occurance count. 
		' 

		Private min As Integer

		'* 
		' * The maximum occurance count. 
		' 

		Private max As Integer

		'* 
		' * The look-ahead set associated with this element. 
		' 

		Private m_lookAhead As LookAheadSet

		'* 
		' * Creates a new element. If the maximum value if zero (0) or 
		' * negative, it will be set to Int32.MaxValue. 
		' * 
		' * @param isToken the token flag 
		' * @param id the node identity 
		' * @param min the minimum number of occurancies 
		' * @param max the maximum number of occurancies, or 
		' * negative for infinite 
		' 

		Public Sub New(ByVal isToken As Boolean, ByVal id As Integer, ByVal min As Integer, ByVal max As Integer)

			Me.token = isToken
			Me.m_id = id
			If min < 0 Then
				min = 0
			End If
			Me.min = min
			If max <= 0 Then
				max = Int32.MaxValue
			ElseIf max < min Then
				max = min
			End If
			Me.max = max
		End Sub

		'* 
		' * The node identity property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Id() As Integer
			Get
				Return m_id
			End Get
		End Property

		'* 
		' * Returns the node identity. 
		' * 
		' * @return the node identity 
		' * 
		' * @see #Id 
		' * 
		' * @deprecated Use the Id property instead. 
		' 

		Public Function GetId() As Integer
			Return Id
		End Function

		'* 
		' * The minimum occurence count property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property MinCount() As Integer
			Get
				Return min
			End Get
		End Property

		'* 
		' * Returns the minimum occurence count. 
		' * 
		' * @return the minimum occurence count 
		' * 
		' * @see #MinCount 
		' * 
		' * @deprecated Use the MinCount property instead. 
		' 

		Public Function GetMinCount() As Integer
			Return MinCount
		End Function

		'* 
		' * The maximum occurence count property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property MaxCount() As Integer
			Get
				Return max
			End Get
		End Property

		'* 
		' * Returns the maximum occurence count. 
		' * 
		' * @return the maximum occurence count 
		' * 
		' * @see #MaxCount 
		' * 
		' * @deprecated Use the MaxCount property instead. 
		' 

		Public Function GetMaxCount() As Integer
			Return MaxCount
		End Function

		'* 
		' * The look-ahead set property. This is the look-ahead set 
		' * associated with this alternative. 
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
		' * Returns true if this element represents a token. 
		' * 
		' * @return true if the element is a token, or 
		' * false otherwise 
		' 

		Public Function IsToken() As Boolean
			Return token
		End Function

		'* 
		' * Returns true if this element represents a production. 
		' * 
		' * @return true if the element is a production, or 
		' * false otherwise 
		' 

		Public Function IsProduction() As Boolean
			Return Not token
		End Function

		'* 
		' * Checks if a specific token matches this element. This 
		' * method will only return true if this element is a token 
		' * element, and the token has the same id and this element. 
		' * 
		' * @param token the token to check 
		' * 
		' * @return true if the token matches this element, or 
		' * false otherwise 
		' 

		Public Function IsMatch(ByVal token As Token) As Boolean
			Return IsToken() AndAlso token IsNot Nothing AndAlso token.Id = m_id
		End Function

		'* 
		' * Checks if this object is equal to another. This method only 
		' * returns true for another identical production pattern 
		' * element. 
		' * 
		' * @param obj the object to compare with 
		' * 
		' * @return true if the object is identical to this one, or 
		' * false otherwise 
		' 

		Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
			Dim elem As ProductionPatternElement

			If TypeOf obj Is ProductionPatternElement Then
				elem = DirectCast(obj, ProductionPatternElement)
				Return Me.token = elem.token AndAlso Me.m_id = elem.Id AndAlso Me.min = elem.min AndAlso Me.max = elem.max
			Else
				Return False
			End If
		End Function

		'* 
		' * Returns a string representation of this object. 
		' * 
		' * @return a string representation of this object 
		' 

		Public Overloads Overrides Function ToString() As String
			Dim buffer As New StringBuilder()

			buffer.Append(m_id)
			If token Then
				buffer.Append("(Token)")
			Else
				buffer.Append("(Production)")
			End If
			If min <> 1 OrElse max <> 1 Then
				buffer.Append("{")
				buffer.Append(min)
				buffer.Append(",")
				buffer.Append(max)
				buffer.Append("}")
			End If
			Return buffer.ToString()
		End Function
	End Class
End Namespace 