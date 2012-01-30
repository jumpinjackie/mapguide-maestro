' 
' * LookAheadReader.cs 
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
Imports System.IO 

Namespace PerCederberg.Grammatica.Runtime 
    
    '* 
' * A look-ahead character stream reader. This class provides the 
' * functionalities of a buffered line-number reader, but with the 
' * additional possibility of peeking an unlimited number of 
' * characters ahead. When looking further and further ahead in the 
' * character stream, the buffer is continously enlarged to contain 
' * all the required characters from the current position an 
' * onwards. This means that looking more characters ahead requires 
' * more memory, and thus becomes unviable in the end. 
' * 
' * @author Per Cederberg, <per at percederberg dot net> 
' * @version 1.5 
' * @since 1.5 
' 
    
	Friend Class LookAheadReader
		Inherits TextReader

		'* 
		' * The character stream block size. All reads from the 
		' * underlying character stream will be made in multiples of 
		' * this block size. 
		' 

		Private Const STREAM_BLOCK_SIZE As Integer = 4096

		'* 
		' * The buffer block size. The size of the internal buffer will 
		' * always be a multiple of this block size. 
		' 

		Private Const BUFFER_BLOCK_SIZE As Integer = 1024

		'* 
		' * The character buffer. 
		' 

		Private buffer As Char() = New Char(STREAM_BLOCK_SIZE - 1) {}

		'* 
		' * The current character buffer position. 
		' 

		Private pos As Integer

		'* 
		' * The number of characters in the buffer. 
		' 

		Private length As Integer

		'* 
		' * The underlying character stream reader. 
		' 

		Private input As TextReader = Nothing

		'* 
		' * The line number of the next character to read. This value 
		' * will be incremented when reading past line breaks. 
		' 

		Private line As Integer = 1

		'* 
		' * The column number of the next character to read. This value 
		' * will be updated for every character read. 
		' 

		Private column As Integer = 1

		'* 
		' * Creates a new look-ahead character stream reader. 
		' * 
		' * @param input the character stream reader to wrap 
		' 

		Public Sub New(ByVal input As TextReader)
			MyBase.New()

			Me.input = input
		End Sub

		'* 
		' * The current line number property (read-only). This number 
		' * is the line number of the next character to read. 
		' 

		Public ReadOnly Property LineNumber() As Integer
			Get
				Return line
			End Get
		End Property

		'* 
		' * The current column number property (read-only). This number 
		' * is the column number of the next character to read. 
		' 

		Public ReadOnly Property ColumnNumber() As Integer
			Get
				Return column
			End Get
		End Property

		'* 
		' * Reads a single character. 
		' * 
		' * @return the character in the range 0 to 65535 
		' * (0x00-0xffff), or -1 if the end of the stream was reached 
		' * 
		' * @throws IOException if an I/O error occurred 
		' 

		Public Overloads Overrides Function Read() As Integer
			ReadAhead(1)
			If pos >= length Then
				Return -1
			Else
				UpdateLineColumnNumbers(1)

				Return Convert.ToInt32(buffer(System.Math.Max(System.Threading.Interlocked.Increment(pos), pos - 1)))
			End If
		End Function

		'* 
		' * Reads characters into an array. This method will always 
		' * return any remaining characters to read before returning 
		' * -1. 
		' * 
		' * @param cbuf the destination buffer 
		' * @param off the offset at which to start storing chars 
		' * @param len the maximum number of characters to read 
		' * 
		' * @return the number of characters read, or -1 if the end of 
		' * the stream was reached 
		' * 
		' * @throws IOException if an I/O error occurred 
		' 

		Public Overloads Overrides Function Read(ByVal cbuf As Char(), ByVal off As Integer, ByVal len As Integer) As Integer
			Dim count As Integer

			ReadAhead(len)
			If pos >= length Then
				Return -1
			Else
				count = length - pos
				If count > len Then
					count = len
				End If
				UpdateLineColumnNumbers(count)
				Array.Copy(buffer, pos, cbuf, off, count)
				pos += count
				Return count
			End If
		End Function

		'* 
		' * Reads characters into a string. This method will always 
		' * return any remaining characters to read before returning 
		' * null. 
		' * 
		' * @param len the maximum number of characters to read 
		' * 
		' * @return the string containing the characters read, or null 
		' * if the end of the stream was reached 
		' * 
		' * @throws IOException if an I/O error occurred 
		' 

		Public Function ReadString(ByVal len As Integer) As String
			Dim count As Integer
			Dim result As String

			ReadAhead(len)
			If pos >= length Then
				Return Nothing
			Else
				count = length - pos
				If count > len Then
					count = len
				End If
				UpdateLineColumnNumbers(count)
				result = New String(buffer, pos, count)
				pos += count
				Return result
			End If
		End Function

		'* 
		' * Returns the next character to read. 
		' * 
		' * @return the character found in the range 0 to 65535 
		' * (0x00-0xffff), or -1 if the end of the stream was reached 
		' * 
		' * @throws IOException if an I/O error occurred 
		' 

		Public Overloads Overrides Function Peek() As Integer
			Return Peek(0)
		End Function

		'* 
		' * Returns a character not yet read. This method will read 
		' * characters up until the specified offset and store them for 
		' * future retrieval in an internal buffer. The character 
		' * offset must be positive, but is allowed to span the entire 
		' * size of the input character stream. Note that the internal 
		' * buffer must hold all the intermediate characters, which may 
		' * be wasteful of memory if offset is too large. 
		' * 
		' * @param off the character offset, from 0 and up 
		' * 
		' * @return the character found in the range 0 to 65535 
		' * (0x00-0xffff), or -1 if the end of the stream was reached 
		' * 
		' * @throws IOException if an I/O error occurred 
		' 

		Public Overloads Function Peek(ByVal off As Integer) As Integer
			ReadAhead(off + 1)
			If pos + off >= length Then
				Return -1
			Else
				Return Convert.ToInt32(buffer(pos + off))
			End If
		End Function

		'* 
		' * Returns a string of characters not yet read. This method 
		' * will read characters up until the specified offset (plus 
		' * length) and store them for future retrieval in an internal 
		' * buffer. The character offset must be positive, but is 
		' * allowed to span the entire size of the input character 
		' * stream. Note that the internal buffer must hold all the 
		' * intermediate characters, which may be wasteful of memory if 
		' * offset is too large. 
		' * 
		' * @param off the character offset, from 0 and up 
		' * @param len the maximum number of characters to read 
		' * 
		' * @return the string containing the characters read, or null 
		' * if the end of the stream was reached 
		' * 
		' * @throws IOException if an I/O error occurred 
		' 

		Public Function PeekString(ByVal off As Integer, ByVal len As Integer) As String
			Dim count As Integer

			ReadAhead(off + len + 1)
			If pos + off >= length Then
				Return Nothing
			Else
				count = length - (pos + off)
				If count > len Then
					count = len
				End If
				Return New String(buffer, pos + off, count)
			End If
		End Function

		'* 
		' * Close the stream. Once a stream has been closed, further 
		' * reads will throw an IOException. Closing a 
		' * previously-closed stream, however, has no effect. 
		' 

		Public Overloads Overrides Sub Close()
			buffer = Nothing
			pos = 0
			length = 0
			If input IsNot Nothing Then
				input.Close()
				input = Nothing
			End If
		End Sub

		'* 
		' * Reads characters from the input stream and appends them to 
		' * the input buffer. This method is safe to call even though 
		' * the end of file has been reached. As a side effect, this 
		' * method may also remove characters at the beginning of the 
		' * buffer. It will enlarge the buffer as needed. 
		' * 
		' * @param offset the read offset, from 0 and up 
		' * 
		' * @throws IOException if an error was encountered while 
		' * reading the input stream 
		' 

		Private Sub ReadAhead(ByVal offset As Integer)
			Dim size As Integer
			Dim readSize As Integer

			' Check for end of stream or already read characters 
			If input Is Nothing OrElse pos + offset < length Then
				Return
			End If

			' Remove old characters from buffer 
			If pos > BUFFER_BLOCK_SIZE Then
				Array.Copy(buffer, pos, buffer, 0, length - pos)
				length -= pos
				pos = 0
			End If

			' Calculate number of characters to read 
			size = pos + offset - length + 1
			If size Mod STREAM_BLOCK_SIZE <> 0 Then
				size = (size / STREAM_BLOCK_SIZE) * STREAM_BLOCK_SIZE
				size += STREAM_BLOCK_SIZE
			End If
			EnsureBufferCapacity(length + size)

			' Read characters 
			Try
				readSize = input.Read(buffer, length, size)
			Catch e As IOException
				input = Nothing
				Throw
			End Try

			' Append characters to buffer 
			If readSize > 0 Then
				length += readSize
			End If
			If readSize < size Then
				Try
					input.Close()
				Finally
					input = Nothing
				End Try
			End If
		End Sub

		'* 
		' * Ensures that the buffer has at least the specified 
		' * capacity. 
		' * 
		' * @param size the minimum buffer size 
		' 

		Private Sub EnsureBufferCapacity(ByVal size As Integer)
			Dim newbuf As Char()

			If buffer.Length >= size Then
				Return
			End If
			If size Mod BUFFER_BLOCK_SIZE <> 0 Then
				size = (size / BUFFER_BLOCK_SIZE) * BUFFER_BLOCK_SIZE
				size += BUFFER_BLOCK_SIZE
			End If
			newbuf = New Char(size - 1) {}
			Array.Copy(buffer, 0, newbuf, 0, length)
			buffer = newbuf
		End Sub

		'* 
		' * Updates the line and column numbers counters. This method 
		' * requires all the characters to be processed (i.e. returned 
		' * as read) to be present in the buffer, starting at the 
		' * current buffer position. 
		' * 
		' * @param offset the number of characters to process 
		' 

		Private Sub UpdateLineColumnNumbers(ByVal offset As Integer)
			For i As Integer = 0 To offset - 1
				If buffer(pos + i) = Chr(10) Then
					line += 1
					column = 1
				Else
					column += 1
				End If
			Next
		End Sub
	End Class
End Namespace 