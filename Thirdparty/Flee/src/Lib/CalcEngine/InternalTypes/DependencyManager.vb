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

Namespace CalcEngine

	' Keeps track of our dependencies
	Friend Class DependencyManager(Of T)

		' Map of a node and the nodes that depend on it
		Private MyDependentsMap As Dictionary(Of T, Dictionary(Of T, Object))
		Private MyEqualityComparer As IEqualityComparer(Of T)
		' Map of a node and the number of nodes that point to it
		Private MyPrecedentsMap As Dictionary(Of T, Integer)

		Public Sub New(ByVal comparer As IEqualityComparer(Of T))
			MyEqualityComparer = comparer
			MyDependentsMap = New Dictionary(Of T, Dictionary(Of T, Object))(MyEqualityComparer)
			MyPrecedentsMap = New Dictionary(Of T, Integer)(MyEqualityComparer)
		End Sub

		Private Function CreateInnerDictionary() As IDictionary(Of T, Object)
			Return New Dictionary(Of T, Object)(MyEqualityComparer)
		End Function

		Private Function GetInnerDictionary(ByVal tail As T) As IDictionary(Of T, Object)
			Dim value As Dictionary(Of T, Object) = Nothing

			If MyDependentsMap.TryGetValue(tail, value) = True Then
				Return value
			Else
				Return Nothing
			End If
		End Function

		' Create a dependency list with only the dependents of the given tails
		Public Function CloneDependents(ByVal tails As T()) As DependencyManager(Of T)
			Dim seenNodes As IDictionary(Of T, Object) = Me.CreateInnerDictionary()
			Dim copy As New DependencyManager(Of T)(MyEqualityComparer)

			For Each tail As T In tails
				Me.CloneDependentsInternal(tail, copy, seenNodes)
			Next

			Return copy
		End Function

		Private Sub CloneDependentsInternal(ByVal tail As T, ByVal target As DependencyManager(Of T), ByVal seenNodes As IDictionary(Of T, Object))
			If seenNodes.ContainsKey(tail) = True Then
				' We've already added this node so just return
				Return
			Else
				' Haven't seen this node yet; mark it as visited
				seenNodes.Add(tail, Nothing)
				target.AddTail(tail)
			End If

			Dim innerDict As IDictionary(Of T, Object) = Me.GetInnerDictionary(tail)

			' Do the recursive add
			For Each head As T In innerDict.Keys
				target.AddDepedency(tail, head)
				Me.CloneDependentsInternal(head, target, seenNodes)
			Next
		End Sub

		Public Function GetTails() As T()
			Dim arr(MyDependentsMap.Keys.Count - 1) As T
			MyDependentsMap.Keys.CopyTo(arr, 0)
			Return arr
		End Function

		Public Sub Clear()
			MyDependentsMap.Clear()
			MyPrecedentsMap.Clear()
		End Sub

		Public Sub ReplaceDependency(ByVal old As T, ByVal replaceWith As T)
			Dim value As Dictionary(Of T, Object) = MyDependentsMap.Item(old)

			MyDependentsMap.Remove(old)
			MyDependentsMap.Add(replaceWith, value)

			For Each innerDict As Dictionary(Of T, Object) In MyDependentsMap.Values
				If innerDict.ContainsKey(old) = True Then
					innerDict.Remove(old)
					innerDict.Add(replaceWith, Nothing)
				End If
			Next
		End Sub

		Public Sub AddTail(ByVal tail As T)
			If MyDependentsMap.ContainsKey(tail) = False Then
				MyDependentsMap.Add(tail, Me.CreateInnerDictionary())
			End If
		End Sub

		Public Sub AddDepedency(ByVal tail As T, ByVal head As T)
			Dim innerDict As IDictionary(Of T, Object) = Me.GetInnerDictionary(tail)

			If innerDict.ContainsKey(head) = False Then
				innerDict.Add(head, head)
				Me.AddPrecedent(head)
			End If
		End Sub

		Public Sub RemoveDependency(ByVal tail As T, ByVal head As T)
			Dim innerDict As IDictionary(Of T, Object) = Me.GetInnerDictionary(tail)
			Me.RemoveHead(head, innerDict)
		End Sub

		Private Sub RemoveHead(ByVal head As T, ByVal dict As IDictionary(Of T, Object))
			If dict.Remove(head) = True Then
				Me.RemovePrecedent(head)
			End If
		End Sub

		Public Sub Remove(ByVal tails As T())
			For Each innerDict As Dictionary(Of T, Object) In MyDependentsMap.Values
				For Each tail As T In tails
					Me.RemoveHead(tail, innerDict)
				Next
			Next

			For Each tail As T In tails
				MyDependentsMap.Remove(tail)
			Next
		End Sub

		Public Sub GetDirectDependents(ByVal tail As T, ByVal dest As List(Of T))
			Dim innerDict As Dictionary(Of T, Object) = Me.GetInnerDictionary(tail)
			dest.AddRange(innerDict.Keys)
		End Sub

		Public Function GetDependents(ByVal tail As T) As T()
			Dim dependents As Dictionary(Of T, Object) = Me.CreateInnerDictionary()
			Me.GetDependentsRecursive(tail, dependents)

			Dim arr(dependents.Count - 1) As T
			dependents.Keys.CopyTo(arr, 0)
			Return arr
		End Function

		Private Sub GetDependentsRecursive(ByVal tail As T, ByVal dependents As Dictionary(Of T, Object))
			dependents.Item(tail) = Nothing
			Dim directDependents As Dictionary(Of T, Object) = Me.GetInnerDictionary(tail)

			For Each pair As T In directDependents.Keys
				Me.GetDependentsRecursive(pair, dependents)
			Next
		End Sub

		Public Sub GetDirectPrecedents(ByVal head As T, ByVal dest As IList(Of T))
			For Each tail As T In MyDependentsMap.Keys
				Dim innerDict As Dictionary(Of T, Object) = Me.GetInnerDictionary(tail)
				If innerDict.ContainsKey(head) = True Then
					dest.Add(tail)
				End If
			Next
		End Sub

		Private Sub AddPrecedent(ByVal head As T)
			Dim count As Integer = 0
			MyPrecedentsMap.TryGetValue(head, count)
			MyPrecedentsMap.Item(head) = count + 1
		End Sub

		Private Sub RemovePrecedent(ByVal head As T)
			Dim count As Integer = MyPrecedentsMap.Item(head) - 1

			If count = 0 Then
				MyPrecedentsMap.Remove(head)
			Else
				MyPrecedentsMap.Item(head) = count
			End If
		End Sub

		Public Function HasPrecedents(ByVal head As T) As Boolean
			Return MyPrecedentsMap.ContainsKey(head)
		End Function

		Public Function HasDependents(ByVal tail As T) As Boolean
			Dim innerDict As Dictionary(Of T, Object) = Me.GetInnerDictionary(tail)
			Return innerDict.Count > 0
		End Function

		Private Function FormatValues(ByVal values As ICollection(Of T)) As String
			Dim strings(values.Count - 1) As String
			Dim keys(values.Count - 1) As T
			values.CopyTo(keys, 0)

			For i As Integer = 0 To keys.Length - 1
				strings(i) = keys(i).ToString()
			Next

			If strings.Length = 0 Then
				Return "<empty>"
			Else
				Return String.Join(",", strings)
			End If
		End Function

		' Add all nodes that don't have any incoming edges into a queue
		Public Function GetSources(ByVal rootTails As T()) As Queue(Of T)
			Dim q As New Queue(Of T)()

			For Each rootTail As T In rootTails
				If Me.HasPrecedents(rootTail) = False Then
					q.Enqueue(rootTail)
				End If
			Next

			Return q
		End Function

		Public Function TopologicalSort(ByVal sources As Queue(Of T)) As IList(Of T)
			Dim output As IList(Of T) = New List(Of T)
			Dim directDependents As IList(Of T) = New List(Of T)

			While sources.Count > 0
				Dim n As T = sources.Dequeue()
				output.Add(n)

				directDependents.Clear()
				Me.GetDirectDependents(n, directDependents)

				For Each m As T In directDependents
					Me.RemoveDependency(n, m)

					If Me.HasPrecedents(m) = False Then
						sources.Enqueue(m)
					End If
				Next
			End While

			If output.Count <> Me.Count Then
				Throw New CircularReferenceException()
			End If

			Return output
		End Function

#If DEBUG Then
		Public ReadOnly Property Precedents() As String
			Get
				Dim list As New List(Of String)()

				For Each pair As KeyValuePair(Of T, Integer) In MyPrecedentsMap
					list.Add(pair.ToString())
				Next

				Return String.Join(System.Environment.NewLine, list.ToArray())
			End Get
		End Property
#End If

		Public ReadOnly Property DependencyGraph() As String
			Get
				Dim lines(MyDependentsMap.Count - 1) As String
				Dim index As Integer

				For Each pair As KeyValuePair(Of T, Dictionary(Of T, Object)) In MyDependentsMap
					Dim key As T = pair.Key
					Dim s As String = Me.FormatValues(pair.Value.Keys)
					lines(index) = String.Format("{0} -> {1}", key, s)
					index += 1
				Next

				Return String.Join(System.Environment.NewLine, lines)
			End Get
		End Property

		Public ReadOnly Property Count() As Integer
			Get
				Return MyDependentsMap.Count
			End Get
		End Property
	End Class

End Namespace