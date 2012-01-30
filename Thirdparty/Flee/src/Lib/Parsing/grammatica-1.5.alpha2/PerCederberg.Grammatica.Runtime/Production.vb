' 
' * Production.cs 
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

Namespace PerCederberg.Grammatica.Runtime 
    
    '* 
' * A production node. This class represents a grammar production 
' * (i.e. a list of child nodes) in a parse tree. The productions 
' * are created by a parser, that adds children a according to a 
' * set of production patterns (i.e. grammar rules). 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
	Friend Class Production
		Inherits Node

		'* 
		' * The production pattern used for this production. 
		' 

		Private m_pattern As ProductionPattern

		'* 
		' * The child nodes. 
		' 

		Private children As ArrayList

		'* 
		' * Creates a new production node. 
		' * 
		' * @param pattern the production pattern 
		' 

		Public Sub New(ByVal pattern As ProductionPattern)
			Me.m_pattern = pattern
			Me.children = New ArrayList()
		End Sub

		'* 
		' * The node type id property (read-only). This value is set as 
		' * a unique identifier for each type of node, in order to 
		' * simplify later identification. 
		' * 
		' * @since 1.5 
		' 

		Public Overloads Overrides ReadOnly Property Id() As Integer
			Get
				Return m_pattern.Id
			End Get
		End Property

		'* 
		' * The node name property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public Overloads Overrides ReadOnly Property Name() As String
			Get
				Return m_pattern.Name
			End Get
		End Property

		'* 
		' * The child node count property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public Overloads Overrides ReadOnly Property Count() As Integer
			Get
				Return children.Count
			End Get
		End Property

		'* 
		' * The child node index (read-only). 
		' * 
		' * @param index the child index, 0 <= index < Count 
		' * 
		' * @return the child node found, or 
		' * null if index out of bounds 
		' * 
		' * @since 1.5 
		' 

		Default Public Overloads Overrides ReadOnly Property Item(ByVal index As Integer) As Node
			Get
				If index < 0 OrElse index >= children.Count Then
					Return Nothing
				Else
					Return DirectCast(children(index), Node)
				End If
			End Get
		End Property

		'* 
		' * Adds a child node. The node will be added last in the list of 
		' * children. 
		' * 
		' * @param child the child node to add 
		' 

		Public Sub AddChild(ByVal child As Node)
			If child IsNot Nothing Then
				child.SetParent(Me)
				children.Add(child)
			End If
		End Sub

		'* 
		' * The production pattern property (read-only). This property 
		' * contains the production pattern linked to this production. 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Pattern() As ProductionPattern
			Get
				Return m_pattern
			End Get
		End Property

		'* 
		' * Returns the production pattern for this production. 
		' * 
		' * @return the production pattern 
		' * 
		' * @see #Pattern 
		' * 
		' * @deprecated Use the Pattern property instead. 
		' 

		Public Function GetPattern() As ProductionPattern
			Return Pattern
		End Function

		'* 
		' * Checks if this node is hidden, i.e. if it should not be visible 
		' * outside the parser. 
		' * 
		' * @return true if the node should be hidden, or 
		' * false otherwise 
		' 

		Friend Overloads Overrides Function IsHidden() As Boolean
			Return m_pattern.Synthetic
		End Function

		'* 
		' * Returns a string representation of this production. 
		' * 
		' * @return a string representation of this production 
		' 

		Public Overloads Overrides Function ToString() As String
			Return m_pattern.Name & "("c & m_pattern.Id & ")"c
		End Function
	End Class
End Namespace 