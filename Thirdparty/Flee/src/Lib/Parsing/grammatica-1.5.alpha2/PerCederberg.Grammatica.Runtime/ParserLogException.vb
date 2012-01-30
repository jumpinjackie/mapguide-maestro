' 
' * ParserLogException.cs 
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
' * A parser log exception. This class contains a list of all the 
' * parse errors encountered while parsing. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' * @since 1.1 
' 
	<Serializable()> _
	Friend Class ParserLogException
		Inherits Exception

		'* 
		' * The list of errors found. 
		' 

		Private errors As New ArrayList()

		'* 
		' * Creates a new empty parser log exception. 
		' 

		Public Sub New()
		End Sub

		Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
			MyBase.New(info, context)
			Me.errors = info.GetValue("Errors", GetType(ArrayList))
		End Sub

		Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
			MyBase.GetObjectData(info, context)
			info.AddValue("Errors", errors, GetType(ArrayList))
		End Sub

		'* 
		' * The message property (read-only). This property contains 
		' * the detailed exception error message. 
		' 

		Public Overloads Overrides ReadOnly Property Message() As String
			Get
				Dim buffer As New StringBuilder()
				For i As Integer = 0 To Count - 1

					If i > 0 Then
						buffer.Append("" & Chr(10) & "")
					End If
					buffer.Append(Me(i).Message)
				Next
				Return buffer.ToString()
			End Get
		End Property

		'* 
		' * The error count property (read-only). 
		' * 
		' * @since 1.5 
		' 

		Public ReadOnly Property Count() As Integer
			Get
				Return errors.Count
			End Get
		End Property

		'* 
		' * Returns the number of errors in this log. 
		' * 
		' * @return the number of errors in this log 
		' * 
		' * @see #Count 
		' * 
		' * @deprecated Use the Count property instead. 
		' 

		Public Function GetErrorCount() As Integer
			Return Count
		End Function

		'* 
		' * The error index (read-only). This index contains all the 
		' * errors in this error log. 
		' * 
		' * @param index the error index, 0 <= index < Count 
		' * 
		' * @return the parse error requested 
		' * 
		' * @since 1.5 
		' 

		Default Public ReadOnly Property Item(ByVal index As Integer) As ParseException
			Get
				Return DirectCast(errors(index), ParseException)
			End Get
		End Property

		'* 
		' * Returns a specific error from the log. 
		' * 
		' * @param index the error index, 0 <= index < count 
		' * 
		' * @return the parse error requested 
		' * 
		' * @deprecated Use the class indexer instead. 
		' 

		Public Function GetError(ByVal index As Integer) As ParseException
			Return Me(index)
		End Function

		'* 
		' * Adds a parse error to the log. 
		' * 
		' * @param e the parse error to add 
		' 

		Public Sub AddError(ByVal e As ParseException)
			errors.Add(e)
		End Sub

		'* 
		' * Returns the detailed error message. This message will contain 
		' * the error messages from all errors in this log, separated by 
		' * a newline. 
		' * 
		' * @return the detailed error message 
		' * 
		' * @see #Message 
		' * 
		' * @deprecated Use the Message property instead. 
		' 

		Public Function GetMessage() As String
			Return Message
		End Function
	End Class
End Namespace 