' 
' * ProductionPattern.cs 
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
' * A production pattern. This class represents a set of production 
' * alternatives that together forms a single production. A 
' * production pattern is identified by an integer id and a name, 
' * both provided upon creation. The pattern id is used for 
' * referencing the production pattern from production pattern 
' * elements. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
	Friend Class ProductionPattern

		'* 
		' * The production pattern identity. 
		' 

		Private m_id As Integer

		'* 
		' * The production pattern name. 
		' 

		Private m_name As String

		'* 
		' * The synthectic production flag. If this flag is set, the 
		' * production identified by this pattern has been artificially 
		' * inserted into the grammar. 
		' 

		Private m_synthetic As Boolean

		'* 
		' * The list of production pattern alternatives. 
		' 

		Private alternatives As ArrayList

		'* 
		' * The default production pattern alternative. This alternative 
		' * is used when no other alternatives match. It may be set to 
		' * -1, meaning that there is no default (or fallback) alternative. 
		' 

		Private defaultAlt As Integer

		'* 
		' * The look-ahead set associated with this pattern. 
		' 

		Private m_lookAhead As LookAheadSet

		'* 
		' * Creates a new production pattern. 
		' * 
		' * @param id the production pattern id 
		' * @param name the production pattern name 
		' 

		Public Sub New(ByVal id As Integer, ByVal name As String)
			Me.m_id = id
			Me.m_name = name
			Me.alternatives = New ArrayList()
			Me.defaultAlt = -1
		End Sub

		'* 
		' * The production pattern identity property (read-only). This 
		' * property contains the unique identity value. 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Id() As Integer
			Get
				Return m_id
			End Get
		End Property

		'* 
		' * Returns the unique production pattern identity value. 
		' * 
		' * @return the production pattern id 
		' * 
		' * @see #Id 
		' * 
		' * @deprecated Use the Id property instead. 
		' 

		Public Function GetId() As Integer
			Return Id
		End Function

		'* 
		' * The production pattern name property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Name() As String
			Get
				Return m_name
			End Get
		End Property

		'* 
		' * Returns the production pattern name. 
		' * 
		' * @return the production pattern name 
		' * 
		' * @see #Name 
		' * 
		' * @deprecated Use the Name property instead. 
		' 

		Public Function GetName() As String
			Return Name
		End Function

		'* 
		' * The synthetic production pattern property. If this property 
		' * is set, the production identified by this pattern has been 
		' * artificially inserted into the grammar. No parse tree nodes 
		' * will be created for such nodes, instead the child nodes 
		' * will be added directly to the parent node. By default this 
		' * property is set to false. 
		' * 
		' * @since 1.5 
		' 

		Public Property Synthetic() As Boolean
			Get
				Return m_synthetic
			End Get
			Set(ByVal value As Boolean)
				m_synthetic = Value
			End Set
		End Property

		'* 
		' * Checks if the synthetic production flag is set. If this 
		' * flag is set, the production identified by this pattern has 
		' * been artificially inserted into the grammar. No parse tree 
		' * nodes will be created for such nodes, instead the child 
		' * nodes will be added directly to the parent node. 
		' * 
		' * @return true if this production pattern is synthetic, or 
		' * false otherwise 
		' * 
		' * @see #Synthetic 
		' * 
		' * @deprecated Use the Synthetic property instead. 
		' 

		Public Function IsSyntetic() As Boolean
			Return Synthetic
		End Function

		'* 
		' * Sets the synthetic production pattern flag. If this flag is set, 
		' * the production identified by this pattern has been artificially 
		' * inserted into the grammar. By default this flag is set to 
		' * false. 
		' * 
		' * @param syntetic the new value of the synthetic flag 
		' * 
		' * @see #Synthetic 
		' * 
		' * @deprecated Use the Synthetic property instead. 
		' 

		Public Sub SetSyntetic(ByVal synthetic As Boolean)
			synthetic = synthetic
		End Sub

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
		' * The default pattern alternative property. The default 
		' * alternative is used when no other alternative matches. The 
		' * default alternative must previously have been added to the 
		' * list of alternatives. This property is set to null if no 
		' * default pattern alternative has been set. 
		' 

		Friend Property DefaultAlternative() As ProductionPatternAlternative
			Get
				If defaultAlt >= 0 Then
					Dim obj As Object = alternatives(defaultAlt)
					Return DirectCast(obj, ProductionPatternAlternative)
				Else
					Return Nothing
				End If
			End Get
			Set(ByVal value As ProductionPatternAlternative)
				defaultAlt = 0
				For i As Integer = 0 To alternatives.Count - 1
					If alternatives(i) Is Value Then
						defaultAlt = i
					End If
				Next
			End Set
		End Property

		'* 
		' * The production pattern alternative count property 
		' * (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Count() As Integer
			Get
				Return alternatives.Count
			End Get
		End Property

		'* 
		' * Returns the number of alternatives in this pattern. 
		' * 
		' * @return the number of alternatives in this pattern 
		' * 
		' * @see #Count 
		' * 
		' * @deprecated Use the Count property instead. 
		' 

		Public Function GetAlternativeCount() As Integer
			Return Count
		End Function

		'* 
		' * The production pattern alternative index (read-only). 
		' * 
		' * @param index the alternative index, 0 <= pos < Count 
		' * 
		' * @return the alternative found 
		' * 
		' * @since 1.5 
		' 

		Default Public ReadOnly Property Item(ByVal index As Integer) As ProductionPatternAlternative
			Get
				Return DirectCast(alternatives(index), ProductionPatternAlternative)
			End Get
		End Property

		'* 
		' * Returns an alternative in this pattern. 
		' * 
		' * @param pos the alternative position, 0 <= pos < count 
		' * 
		' * @return the alternative found 
		' * 
		' * @deprecated Use the class indexer instead. 
		' 

		Public Function GetAlternative(ByVal pos As Integer) As ProductionPatternAlternative
			Return Me(pos)
		End Function

		'* 
		' * Checks if this pattern is recursive on the left-hand side. 
		' * This method checks if any of the production pattern 
		' * alternatives is left-recursive. 
		' * 
		' * @return true if at least one alternative is left recursive, or 
		' * false otherwise 
		' 

		Public Function IsLeftRecursive() As Boolean
			Dim alt As ProductionPatternAlternative
			For i As Integer = 0 To alternatives.Count - 1

				alt = DirectCast(alternatives(i), ProductionPatternAlternative)
				If alt.IsLeftRecursive() Then
					Return True
				End If
			Next
			Return False
		End Function

		'* 
		' * Checks if this pattern is recursive on the right-hand side. 
		' * This method checks if any of the production pattern 
		' * alternatives is right-recursive. 
		' * 
		' * @return true if at least one alternative is right recursive, or 
		' * false otherwise 
		' 

		Public Function IsRightRecursive() As Boolean
			Dim alt As ProductionPatternAlternative
			For i As Integer = 0 To alternatives.Count - 1

				alt = DirectCast(alternatives(i), ProductionPatternAlternative)
				If alt.IsRightRecursive() Then
					Return True
				End If
			Next
			Return False
		End Function

		'* 
		' * Checks if this pattern would match an empty stream of 
		' * tokens. This method checks if any one of the production 
		' * pattern alternatives would match the empty token stream. 
		' * 
		' * @return true if at least one alternative match no tokens, or 
		' * false otherwise 
		' 

		Public Function IsMatchingEmpty() As Boolean
			Dim alt As ProductionPatternAlternative
			For i As Integer = 0 To alternatives.Count - 1

				alt = DirectCast(alternatives(i), ProductionPatternAlternative)
				If alt.IsMatchingEmpty() Then
					Return True
				End If
			Next
			Return False
		End Function

		'* 
		' * Adds a production pattern alternative. 
		' * 
		' * @param alt the production pattern alternative to add 
		' * 
		' * @throws ParserCreationException if an identical alternative has 
		' * already been added 
		' 

		Public Sub AddAlternative(ByVal alt As ProductionPatternAlternative)
			If alternatives.Contains(alt) Then
				Throw New ParserCreationException(ParserCreationException.ErrorType.INVALID_PRODUCTION, m_name, "two identical alternatives exist")
			End If
			alt.SetPattern(Me)
			alternatives.Add(alt)
		End Sub

		'* 
		' * Returns a string representation of this object. 
		' * 
		' * @return a token string representation 
		' 

		Public Overloads Overrides Function ToString() As String
			Dim buffer As New StringBuilder()
			Dim indent As New StringBuilder()
			Dim i As Integer

			buffer.Append(m_name)
			buffer.Append("(")
			buffer.Append(m_id)
			buffer.Append(") ")
			For i = 0 To buffer.Length - 1
				indent.Append(" ")
			Next
			For i = 0 To alternatives.Count - 1
				If i = 0 Then
					buffer.Append("= ")
				Else
					buffer.Append("" & Chr(10) & "")
					buffer.Append(indent)
					buffer.Append("| ")
				End If
				buffer.Append(alternatives(i))
			Next
			Return buffer.ToString()
		End Function
	End Class
End Namespace 