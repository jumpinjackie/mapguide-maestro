' This library is free software; you can redistribute it and/or
' modify it under the terms of the GNU Lesser General Public License
' as published by the Free Software Foundation; either version 2.1
' of the License, or (at your option) any later version.
' 
' This library is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
' Lesser General Public License for more details.
' 
' You should have received a copy of the GNU Lesser General Public
' License along with this library; if not, write to the Free
' Software Foundation, Inc., 59 Temple Place, Suite 330, Boston,
' MA 02111-1307, USA.
' 
' Flee - Fast Lightweight Expression Evaluator
' Copyright © 2007 Eugene Ciloci
'

''' <summary>
''' Encapsulates an argument list
''' </summary>
''' <remarks></remarks>
Friend Class ArgumentList

	Private MyElements As IList(Of ExpressionElement)

	Public Sub New(ByVal elements As ICollection)
		Dim arr(elements.Count - 1) As ExpressionElement
		elements.CopyTo(arr, 0)
		MyElements = arr
	End Sub

	Private Function GetArgumentTypeNames() As String()
		Dim l As New List(Of String)()

		For Each e As ExpressionElement In MyElements
			l.Add(e.ResultType.Name)
		Next

		Return l.ToArray()
	End Function

	Public Function GetArgumentTypes() As Type()
		Dim l As New List(Of Type)()

		For Each e As ExpressionElement In MyElements
			l.Add(e.ResultType)
		Next

		Return l.ToArray()
	End Function

	Public Overrides Function ToString() As String
		Dim typeNames As String() = Me.GetArgumentTypeNames()
		Return Utility.FormatList(typeNames)
	End Function

	Public Function ToArray() As ExpressionElement()
		Dim arr(MyElements.Count - 1) As ExpressionElement
		MyElements.CopyTo(arr, 0)
		Return arr
	End Function

	Default Public ReadOnly Property Item(ByVal index As Integer) As ExpressionElement
		Get
			Return MyElements.Item(index)
		End Get
	End Property

	Public ReadOnly Property Count() As Integer
		Get
			Return MyElements.Count
		End Get
	End Property
End Class
