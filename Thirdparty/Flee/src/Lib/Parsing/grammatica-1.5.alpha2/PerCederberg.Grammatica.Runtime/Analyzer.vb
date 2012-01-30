' 
' * Analyzer.cs 
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
' * A parse tree analyzer. This class provides callback methods that 
' * may be used either during parsing, or for a parse tree traversal. 
' * This class should be subclassed to provide adequate handling of the 
' * parse tree nodes. 
' * 
' * The general contract for the analyzer class does not guarantee a 
' * strict call order for the callback methods. Depending on the type 
' * of parser, the enter() and exit() methods for production nodes can 
' * be called either in a top-down or a bottom-up fashion. The only 
' * guarantee provided by this API, is that the calls for any given 
' * node will always be in the order enter(), child(), and exit(). If 
' * various child() calls are made, they will be made from left to 
' * right as child nodes are added (to the right). 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.1 
' 
    
	Friend Class Analyzer

		'* 
		' * Creates a new parse tree analyzer. 
		' 

		Public Sub New()
		End Sub

		'* 
		' * Analyzes a parse tree node by traversing all it's child nodes. 
		' * The tree traversal is depth-first, and the appropriate 
		' * callback methods will be called. If the node is a production 
		' * node, a new production node will be created and children will 
		' * be added by recursively processing the children of the 
		' * specified production node. This method is used to process a 
		' * parse tree after creation. 
		' * 
		' * @param node the parse tree node to process 
		' * 
		' * @return the resulting parse tree node 
		' * 
		' * @throws ParserLogException if the node analysis discovered 
		' * errors 
		' 

		Public Function Analyze(ByVal node As Node) As Node
			Dim log As New ParserLogException()

			node = Analyze(node, log)
			If log.Count > 0 Then
				Throw log
			End If
			Return node
		End Function

		'* 
		' * Analyzes a parse tree node by traversing all it's child nodes. 
		' * The tree traversal is depth-first, and the appropriate 
		' * callback methods will be called. If the node is a production 
		' * node, a new production node will be created and children will 
		' * be added by recursively processing the children of the 
		' * specified production node. This method is used to process a 
		' * parse tree after creation. 
		' * 
		' * @param node the parse tree node to process 
		' * @param log the parser error log 
		' * 
		' * @return the resulting parse tree node 
		' 

		Private Function Analyze(ByVal node As Node, ByVal log As ParserLogException) As Node
			Dim prod As Production
			Dim errorCount As Integer

			errorCount = log.Count
			If TypeOf node Is Production Then
				prod = DirectCast(node, Production)
				prod = New Production(prod.Pattern)
				Try
					Enter(prod)
				Catch e As ParseException
					log.AddError(e)
				End Try
				For i As Integer = 0 To node.Count - 1
					Try
						Child(prod, Analyze(node(i), log))
					Catch e As ParseException
						log.AddError(e)
					End Try
				Next
				Try
					Return [Exit](prod)
				Catch e As ParseException
					If errorCount = log.Count Then
						log.AddError(e)
					End If
				End Try
			Else
				node.Values.Clear()
				Try
					Enter(node)
				Catch e As ParseException
					log.AddError(e)
				End Try
				Try
					Return [Exit](node)
				Catch e As ParseException
					If errorCount = log.Count Then
						log.AddError(e)
					End If
				End Try
			End If
			Return Nothing
		End Function

		'* 
		' * Called when entering a parse tree node. By default this method 
		' * does nothing. A subclass can override this method to handle 
		' * each node separately. 
		' * 
		' * @param node the node being entered 
		' * 
		' * @throws ParseException if the node analysis discovered errors 
		' 

		Public Overridable Sub Enter(ByVal node As Node)
		End Sub

		'* 
		' * Called when exiting a parse tree node. By default this method 
		' * returns the node. A subclass can override this method to handle 
		' * each node separately. If no parse tree should be created, this 
		' * method should return null. 
		' * 
		' * @param node the node being exited 
		' * 
		' * @return the node to add to the parse tree, or 
		' * null if no parse tree should be created 
		' * 
		' * @throws ParseException if the node analysis discovered errors 
		' 

		Public Overridable Function [Exit](ByVal node As Node) As Node
			Return node
		End Function

		'* 
		' * Called when adding a child to a parse tree node. By default 
		' * this method adds the child to the production node. A subclass 
		' * can override this method to handle each node separately. Note 
		' * that the child node may be null if the corresponding exit() 
		' * method returned null. 
		' * 
		' * @param node the parent node 
		' * @param child the child node, or null 
		' * 
		' * @throws ParseException if the node analysis discovered errors 
		' 

		Public Overridable Sub Child(ByVal node As Production, ByVal child As Node)
			node.AddChild(child)
		End Sub

		'* 
		' * Returns a child at the specified position. If either the node 
		' * or the child node is null, this method will throw a parse 
		' * exception with the internal error type. 
		' * 
		' * @param node the parent node 
		' * @param pos the child position 
		' * 
		' * @return the child node 
		' * 
		' * @throws ParseException if either the node or the child node 
		' * was null 
		' 

		Protected Function GetChildAt(ByVal node As Node, ByVal pos As Integer) As Node
			Dim child As Node

			If node Is Nothing Then
				Throw New ParseException(ParseException.ErrorType.INTERNAL, "attempt to read 'null' parse tree node", -1, -1)
			End If
			child = node(pos)
			If child Is Nothing Then
				Throw New ParseException(ParseException.ErrorType.INTERNAL, "node '" + node.Name + "' has no child at " + "position " + pos, node.StartLine, node.StartColumn)
			End If
			Return child
		End Function

		'* 
		' * Returns the first child with the specified id. If the node is 
		' * null, or no child with the specified id could be found, this 
		' * method will throw a parse exception with the internal error 
		' * type. 
		' * 
		' * @param node the parent node 
		' * @param id the child node id 
		' * 
		' * @return the child node 
		' * 
		' * @throws ParseException if the node was null, or a child node 
		' * couldn't be found 
		' 

		Protected Function GetChildWithId(ByVal node As Node, ByVal id As Integer) As Node
			Dim child As Node

			If node Is Nothing Then
				Throw New ParseException(ParseException.ErrorType.INTERNAL, "attempt to read 'null' parse tree node", -1, -1)
			End If
			For i As Integer = 0 To node.Count - 1
				child = node(i)
				If child IsNot Nothing AndAlso child.Id = id Then
					Return child
				End If
			Next
			Throw New ParseException(ParseException.ErrorType.INTERNAL, "node '" + node.Name + "' has no child with id " + id, node.StartLine, node.StartColumn)
		End Function

		'* 
		' * Returns the node value at the specified position. If either 
		' * the node or the value is null, this method will throw a parse 
		' * exception with the internal error type. 
		' * 
		' * @param node the parse tree node 
		' * @param pos the child position 
		' * 
		' * @return the value object 
		' * 
		' * @throws ParseException if either the node or the value was null 
		' 

		Protected Function GetValue(ByVal node As Node, ByVal pos As Integer) As Object
			Dim value As Object

			If node Is Nothing Then
				Throw New ParseException(ParseException.ErrorType.INTERNAL, "attempt to read 'null' parse tree node", -1, -1)
			End If
			value = node.Values(pos)
			If value Is Nothing Then
				Throw New ParseException(ParseException.ErrorType.INTERNAL, "node '" + node.Name + "' has no value at " + "position " + pos, node.StartLine, node.StartColumn)
			End If
			Return value
		End Function

		'* 
		' * Returns the node integer value at the specified position. If 
		' * either the node is null, or the value is not an instance of 
		' * the Integer class, this method will throw a parse exception 
		' * with the internal error type. 
		' * 
		' * @param node the parse tree node 
		' * @param pos the child position 
		' * 
		' * @return the value object 
		' * 
		' * @throws ParseException if either the node was null, or the 
		' * value wasn't an integer 
		' 

		Protected Function GetIntValue(ByVal node As Node, ByVal pos As Integer) As Integer
			Dim value As Object

			value = GetValue(node, pos)
			If TypeOf value Is Integer Then
				Return CInt(value)
			Else
				Throw New ParseException(ParseException.ErrorType.INTERNAL, "node '" + node.Name + "' has no integer value " + "at position " + pos, node.StartLine, node.StartColumn)
			End If
		End Function

		'* 
		' * Returns the node string value at the specified position. If 
		' * either the node is null, or the value is not an instance of 
		' * the String class, this method will throw a parse exception 
		' * with the internal error type. 
		' * 
		' * @param node the parse tree node 
		' * @param pos the child position 
		' * 
		' * @return the value object 
		' * 
		' * @throws ParseException if either the node was null, or the 
		' * value wasn't a string 
		' 

		Protected Function GetStringValue(ByVal node As Node, ByVal pos As Integer) As String
			Dim value As Object

			value = GetValue(node, pos)
			If TypeOf value Is String Then
				Return DirectCast(value, String)
			Else
				Throw New ParseException(ParseException.ErrorType.INTERNAL, "node '" + node.Name + "' has no string value " + "at position " + pos, node.StartLine, node.StartColumn)
			End If
		End Function

		'* 
		' * Returns all the node values for all child nodes. 
		' * 
		' * @param node the parse tree node 
		' * 
		' * @return a list with all the child node values 
		' * 
		' * @since 1.3 
		' 

		Protected Function GetChildValues(ByVal node As Node) As ArrayList
			Dim result As New ArrayList()
			Dim child As Node
			Dim values As ArrayList
			For i As Integer = 0 To node.Count - 1

				child = node(i)
				values = child.Values
				If values IsNot Nothing Then
					result.AddRange(values)
				End If
			Next
			Return result
		End Function
	End Class
End Namespace 