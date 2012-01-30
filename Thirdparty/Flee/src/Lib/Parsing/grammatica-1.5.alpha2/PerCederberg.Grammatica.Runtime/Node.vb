' 
' * Node.cs 
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
Imports System.IO 

Namespace PerCederberg.Grammatica.Runtime 
    
    '* 
' * An abstract parse tree node. This class is inherited by all 
' * nodes in the parse tree, i.e. by the token and production 
' * classes. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' 
    
	Friend MustInherit Class Node

		'* 
		' * The parent node. 
		' 

		Private m_parent As Node

		'* 
		' * The computed node values. 
		' 

		Private m_values As ArrayList

		'* 
		' * Checks if this node is hidden, i.e. if it should not be 
		' * visible outside the parser. 
		' * 
		' * @return true if the node should be hidden, or 
		' * false otherwise 
		' 

		Friend Overridable Function IsHidden() As Boolean
			Return False
		End Function

		'* 
		' * The node type id property (read-only). This value is set as 
		' * a unique identifier for each type of node, in order to 
		' * simplify later identification. 
		' * 
		' * @since 1.5 
		' 

		Public MustOverride ReadOnly Property Id() As Integer

		'* 
		' * Returns the node type id. This value is set as a unique 
		' * identifier for each type of node, in order to simplify 
		' * later identification. 
		' * 
		' * @return the node type id 
		' * 
		' * @see #Id 
		' * 
		' * @deprecated Use the Id property instead. 
		' 

		Public Overridable Function GetId() As Integer
			Return Id
		End Function

		'* 
		' * The node name property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public MustOverride ReadOnly Property Name() As String

		'* 
		' * Returns the node name. 
		' * 
		' * @return the node name 
		' * 
		' * @see #Name 
		' * 
		' * @deprecated Use the Name property instead. 
		' 

		Public Overridable Function GetName() As String
			Return Name
		End Function

		'* 
		' * The line number property of the first character in this 
		' * node (read-only). If the node has child elements, this 
		' * value will be fetched from the first child. 
		' * 
		' * @since 1.5 
		' 

		Public Overridable ReadOnly Property StartLine() As Integer
			Get
				Dim line As Integer
				For i As Integer = 0 To Count - 1

					line = Me(i).StartLine
					If line >= 0 Then
						Return line
					End If
				Next
				Return -1
			End Get
		End Property

		'* 
		' * The line number of the first character in this node. If the 
		' * node has child elements, this value will be fetched from 
		' * the first child. 
		' * 
		' * @return the line number of the first character, or 
		' * -1 if not applicable 
		' * 
		' * @see #StartLine 
		' * 
		' * @deprecated Use the StartLine property instead. 
		' 

		Public Overridable Function GetStartLine() As Integer
			Return StartLine
		End Function

		'* 
		' * The column number property of the first character in this 
		' * node (read-only). If the node has child elements, this 
		' * value will be fetched from the first child. 
		' * 
		' * @since 1.5 
		' 

		Public Overridable ReadOnly Property StartColumn() As Integer
			Get
				Dim col As Integer
				For i As Integer = 0 To Count - 1

					col = Me(i).StartColumn
					If col >= 0 Then
						Return col
					End If
				Next
				Return -1
			End Get
		End Property

		'* 
		' * The column number of the first character in this node. If 
		' * the node has child elements, this value will be fetched 
		' * from the first child. 
		' * 
		' * @return the column number of the first token character, or 
		' * -1 if not applicable 
		' * 
		' * @see #StartColumn 
		' * 
		' * @deprecated Use the StartColumn property instead. 
		' 

		Public Overridable Function GetStartColumn() As Integer
			Return StartColumn
		End Function

		'* 
		' * The line number property of the last character in this node 
		' * (read-only). If the node has child elements, this value 
		' * will be fetched from the last child. 
		' * 
		' * @since 1.5 
		' 

		Public Overridable ReadOnly Property EndLine() As Integer
			Get
				Dim line As Integer
				For i As Integer = Count - 1 To 0 Step -1

					line = Me(i).EndLine
					If line >= 0 Then
						Return line
					End If
				Next
				Return -1
			End Get
		End Property

		'* 
		' * The line number of the last character in this node. If the 
		' * node has child elements, this value will be fetched from 
		' * the last child. 
		' * 
		' * @return the line number of the last token character, or 
		' * -1 if not applicable 
		' * 
		' * @see #EndLine 
		' * 
		' * @deprecated Use the EndLine property instead. 
		' 

		Public Overridable Function GetEndLine() As Integer
			Return EndLine
		End Function

		'* 
		' * The column number property of the last character in this 
		' * node (read-only). If the node has child elements, this 
		' * value will be fetched from the last child. 
		' * 
		' * @since 1.5 
		' 

		Public Overridable ReadOnly Property EndColumn() As Integer
			Get
				Dim col As Integer
				For i As Integer = Count - 1 To 0 Step -1

					col = Me(i).EndColumn
					If col >= 0 Then
						Return col
					End If
				Next
				Return -1
			End Get
		End Property

		'* 
		' * The column number of the last character in this node. If 
		' * the node has child elements, this value will be fetched 
		' * from the last child. 
		' * 
		' * @return the column number of the last token character, or 
		' * -1 if not applicable 
		' * 
		' * @see #EndColumn 
		' * 
		' * @deprecated Use the EndColumn property instead. 
		' 

		Public Overridable Function GetEndColumn() As Integer
			Return EndColumn
		End Function

		'* 
		' * The parent node property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Parent() As Node
			Get
				Return m_parent
			End Get
		End Property

		'* 
		' * Returns the parent node. 
		' * 
		' * @return the parent parse tree node 
		' * 
		' * @see #Parent 
		' * 
		' * @deprecated Use the Parent property instead. 
		' 

		Public Function GetParent() As Node
			Return Parent
		End Function

		'* 
		' * Sets the parent node. 
		' * 
		' * @param parent the new parent node 
		' 

		Friend Sub SetParent(ByVal parent As Node)
			Me.m_parent = parent
		End Sub

		'* 
		' * The child node count property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public Overridable ReadOnly Property Count() As Integer
			Get
				Return 0
			End Get
		End Property

		'* 
		' * Returns the number of child nodes. 
		' * 
		' * @return the number of child nodes 
		' * 
		' * @deprecated Use the Count property instead. 
		' 

		Public Overridable Function GetChildCount() As Integer
			Return Count
		End Function

		'* 
		' * Returns the number of descendant nodes. 
		' * 
		' * @return the number of descendant nodes 
		' * 
		' * @since 1.2 
		' 

		Public Function GetDescendantCount() As Integer
			Dim count As Integer = 0
			For i As Integer = 0 To count - 1

				count += 1 + Me(i).GetDescendantCount()
			Next
			Return count
		End Function

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

		Default Public Overridable ReadOnly Property Item(ByVal index As Integer) As Node
			Get
				Return Nothing
			End Get
		End Property

		'* 
		' * Returns the child node with the specified index. 
		' * 
		' * @param index the child index, 0 <= index < count 
		' * 
		' * @return the child node found, or 
		' * null if index out of bounds 
		' * 
		' * @deprecated Use the class indexer instead. 
		' 

		Public Overridable Function GetChildAt(ByVal index As Integer) As Node
			Return Me(index)
		End Function

		'* 
		' * The node values property. This property provides direct 
		' * access to the list of computed values associated with this 
		' * node during analysis. Note that setting this property to 
		' * null will remove all node values. Any operation on the 
		' * value array list is allowed and is immediately reflected 
		' * through the various value reading and manipulation methods. 
		' * 
		' * @since 1.5 
		' 

		Public Property Values() As ArrayList
			Get
				If m_values Is Nothing Then
					m_values = New ArrayList()
				End If
				Return m_values
			End Get
			Set(ByVal value As ArrayList)
				Me.m_values = value
			End Set
		End Property

		'* 
		' * Returns the number of computed values associated with this 
		' * node. Any number of values can be associated with a node 
		' * through calls to AddValue(). 
		' * 
		' * @return the number of values associated with this node 
		' * 
		' * @see #Values 
		' * 
		' * @deprecated Use the Values and Values.Count properties 
		' * instead. 
		' 

		Public Function GetValueCount() As Integer
			If m_values Is Nothing Then
				Return 0
			Else
				Return m_values.Count
			End If
		End Function

		'* 
		' * Returns a computed value of this node, if previously set. A 
		' * value may be used for storing intermediate results in the 
		' * parse tree during analysis. 
		' * 
		' * @param pos the value position, 0 <= pos < count 
		' * 
		' * @return the computed node value, or 
		' * null if not set 
		' * 
		' * @see #Values 
		' * 
		' * @deprecated Use the Values property and it's array indexer 
		' * instead. 
		' 

		Public Function GetValue(ByVal pos As Integer) As Object
			Return Values(pos)
		End Function

		'* 
		' * Returns the list with all the computed values for this 
		' * node. Note that the list is not a copy, so changes will 
		' * affect the values in this node (as it is the same object). 
		' * 
		' * @return a list with all values, or 
		' * null if no values have been set 
		' * 
		' * @see #Values 
		' * 
		' * @deprecated Use the Values property instead. Note that the 
		' * Values property will never be null, but possibly empty. 
		' 

		Public Function GetAllValues() As ArrayList
			Return m_values
		End Function

		'* 
		' * Adds a computed value to this node. The computed value may 
		' * be used for storing intermediate results in the parse tree 
		' * during analysis. 
		' * 
		' * @param value the node value 
		' * 
		' * @see #Values 
		' * 
		' * @deprecated Use the Values property and the Values.Add 
		' * method instead. 
		' 

		Public Sub AddValue(ByVal value As Object)
			If value IsNot Nothing Then
				Values.Add(value)
			End If
		End Sub

		'* 
		' * Adds a set of computed values to this node. 
		' * 
		' * @param values the vector with node values 
		' * 
		' * @see #Values 
		' * 
		' * @deprecated Use the Values property and the Values.AddRange 
		' * method instead. 
		' 

		Public Sub AddValues(ByVal values As ArrayList)
			If values IsNot Nothing Then
				Me.Values.AddRange(values)
			End If
		End Sub

		'* 
		' * Removes all computed values stored in this node. 
		' * 
		' * @see #Values 
		' * 
		' * @deprecated Use the Values property and the Values.Clear 
		' * method instead. Alternatively the Values property can 
		' * be set to null. 
		' 

		Public Sub RemoveAllValues()
			m_values = Nothing
		End Sub

		'* 
		' * Prints this node and all subnodes to the specified output 
		' * stream. 
		' * 
		' * @param output the output stream to use 
		' 

		Public Sub PrintTo(ByVal output As TextWriter)
			PrintTo(output, "")
			output.Flush()
		End Sub

		'* 
		' * Prints this node and all subnodes to the specified output 
		' * stream. 
		' * 
		' * @param output the output stream to use 
		' * @param indent the indentation string 
		' 

		Private Sub PrintTo(ByVal output As TextWriter, ByVal indent As String)
			output.WriteLine(indent + ToString())
			indent = indent + " "
			For i As Integer = 0 To Count - 1
				Me(i).PrintTo(output, indent)
			Next
		End Sub
	End Class
End Namespace 