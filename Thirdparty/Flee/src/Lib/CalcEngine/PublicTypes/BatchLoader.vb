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

	''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="BatchLoader"]/*' />
	Public NotInheritable Class BatchLoader

		Private MyNameInfoMap As IDictionary(Of String, BatchLoadInfo)
		Private MyDependencies As DependencyManager(Of String)

		Friend Sub New()
			MyNameInfoMap = New Dictionary(Of String, BatchLoadInfo)(StringComparer.OrdinalIgnoreCase)
			MyDependencies = New DependencyManager(Of String)(StringComparer.OrdinalIgnoreCase)
		End Sub

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="BatchLoader.Add"]/*' />
		Public Sub Add(ByVal atomName As String, ByVal expression As String, ByVal context As ExpressionContext)
			Utility.AssertNotNull(atomName, "atomName")
			Utility.AssertNotNull(expression, "expression")
			Utility.AssertNotNull(context, "context")

			Dim info As New BatchLoadInfo(atomName, expression, context)
			MyNameInfoMap.Add(atomName, info)
			MyDependencies.AddTail(atomName)

			Dim references As ICollection(Of String) = Me.GetReferences(expression, context)

			For Each reference As String In references
				MyDependencies.AddTail(reference)
				MyDependencies.AddDepedency(reference, atomName)
			Next
		End Sub

		''' <include file='Resources/DocComments.xml' path='DocComments/Member[@name="BatchLoader.Contains"]/*' />
		Public Function Contains(ByVal atomName As String) As Boolean
			Return MyNameInfoMap.ContainsKey(atomName)
		End Function

		Friend Function GetBachInfos() As BatchLoadInfo()
			Dim tails As String() = MyDependencies.GetTails()
			Dim sources As Queue(Of String) = MyDependencies.GetSources(tails)

			Dim result As IList(Of String) = MyDependencies.TopologicalSort(sources)

			Dim infos(result.Count - 1) As BatchLoadInfo

			For i As Integer = 0 To result.Count - 1
				infos(i) = MyNameInfoMap.Item(result.Item(i))
			Next

			Return infos
		End Function

		Private Function GetReferences(ByVal expression As String, ByVal context As ExpressionContext) As ICollection(Of String)
			Dim analyzer As IdentifierAnalyzer = context.ParseIdentifiers(expression)

			Return analyzer.GetIdentifiers(context)
		End Function
	End Class

End Namespace