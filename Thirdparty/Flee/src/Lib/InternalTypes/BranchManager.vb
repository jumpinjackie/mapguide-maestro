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

Imports System.Reflection.Emit

''' <summary>
''' Manages branch information and allows us to determine if we should emit a short or long branch
''' </summary>
Friend Class BranchManager

	Private MyBranchInfos As IList(Of BranchInfo)
	Private MyKeyLabelMap As IDictionary(Of Object, Label)

	Public Sub New()
		MyBranchInfos = New List(Of BranchInfo)()
		MyKeyLabelMap = New Dictionary(Of Object, Label)()
	End Sub

	''' <summary>
	''' Determine whether to use short or long branches
	''' </summary>
	''' <remarks></remarks>
	Public Sub ComputeBranches()
		Dim betweenBranches As New List(Of BranchInfo)()

		For Each bi As BranchInfo In MyBranchInfos
			betweenBranches.Clear()

			' Find any branches between the start and end locations of this branch
			Me.FindBetweenBranches(bi, betweenBranches)

			' Count the number of long branches in the above set
			Dim longBranchesBetween As Integer = Me.CountLongBranches(betweenBranches)

			' Adjust the branch as necessary
			bi.AdjustForLongBranchesBetween(longBranchesBetween)
		Next

		Dim longBranchCount As Integer = 0

		' Adjust the start location of each branch
		For Each bi As BranchInfo In MyBranchInfos
			' Save the short/long branch type
			bi.BakeIsLongBranch()

			' Adjust the start location as necessary
			bi.AdjustForLongBranches(longBranchCount)

			' Keep a tally of the number of long branches
			longBranchCount += Convert.ToInt32(bi.IsLongBranch)
		Next
	End Sub

	''' <summary>
	''' Count the number of long branches in a set
	''' </summary>
	''' <param name="dest"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Private Function CountLongBranches(ByVal dest As ICollection(Of BranchInfo)) As Integer
		Dim count As Integer = 0

		For Each bi As BranchInfo In dest
			count += Convert.ToInt32(bi.ComputeIsLongBranch())
		Next

		Return count
	End Function

	''' <summary>
	''' Find all the branches between the start and end locations of a target branch
	''' </summary>
	''' <param name="target"></param>
	''' <param name="dest"></param>
	''' <remarks></remarks>
	Private Sub FindBetweenBranches(ByVal target As BranchInfo, ByVal dest As ICollection(Of BranchInfo))
		For Each bi As BranchInfo In MyBranchInfos
			If bi.IsBetween(target) = True Then
				dest.Add(bi)
			End If
		Next
	End Sub

	''' <summary>
	''' Determine if a branch from a point to a label will be long
	''' </summary>
	''' <param name="ilg"></param>
	''' <param name="target"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function IsLongBranch(ByVal ilg As FleeILGenerator, ByVal target As Label) As Boolean
		Dim startLoc As New ILLocation(ilg.Length)
		Dim bi As New BranchInfo(startLoc, target)

		Dim index As Integer = MyBranchInfos.IndexOf(bi)
		bi = MyBranchInfos.Item(index)

		Return bi.IsLongBranch
	End Function

	''' <summary>
	''' Add a branch from a location to a target label
	''' </summary>
	''' <param name="ilg"></param>
	''' <param name="target"></param>
	''' <remarks></remarks>
	Public Sub AddBranch(ByVal ilg As FleeILGenerator, ByVal target As Label)
		Dim startLoc As New ILLocation(ilg.Length)

		Dim bi As New BranchInfo(startLoc, target)
		MyBranchInfos.Add(bi)
	End Sub

	''' <summary>
	''' Get a label by a key
	''' </summary>
	''' <param name="key"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function FindLabel(ByVal key As Object) As Label
		Return MyKeyLabelMap.Item(key)
	End Function

	''' <summary>
	''' Get a label by a key.  Create the label if it is not present.
	''' </summary>
	''' <param name="key"></param>
	''' <param name="ilg"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function GetLabel(ByVal key As Object, ByVal ilg As FleeILGenerator) As Label
		Dim lbl As Label = Nothing

		If MyKeyLabelMap.TryGetValue(key, lbl) = False Then
			lbl = ilg.DefineLabel()
			MyKeyLabelMap.Add(key, lbl)
		End If

		Return lbl
	End Function

	''' <summary>
	''' Determines if we have a label for a key
	''' </summary>
	''' <param name="key"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function HasLabel(ByVal key As Object) As Boolean
		Return MyKeyLabelMap.ContainsKey(key)
	End Function

	''' <summary>
	''' Set the position for a label
	''' </summary>
	''' <param name="ilg"></param>
	''' <param name="target"></param>
	''' <remarks></remarks>
	Public Sub MarkLabel(ByVal ilg As FleeILGenerator, ByVal target As Label)
		Dim pos As Integer = ilg.Length

		For Each bi As BranchInfo In MyBranchInfos
			bi.Mark(target, pos)
		Next
	End Sub

	Public Overrides Function ToString() As String
		Dim arr(MyBranchInfos.Count - 1) As String

		For i As Integer = 0 To MyBranchInfos.Count - 1
			arr(i) = MyBranchInfos(i).ToString()
		Next

		Return String.Join(System.Environment.NewLine, arr)
	End Function
End Class

''' <summary>
''' Represents a location in an IL stream
''' </summary>
Friend Class ILLocation
	Implements IEquatable(Of ILLocation)
	Implements IComparable(Of ILLocation)

	Private MyPosition As Integer

	''' <summary>
	''' ' Long branch is 5 bytes; short branch is 2; so we adjust by the difference
	''' </summary>
	Private Const LongBranchAdjust As Integer = 3

	''' <summary>
	''' Length of the Br_s opcode
	''' </summary>
	Private Const Br_s_Length As Integer = 2

	Public Sub New()

	End Sub

	Public Sub New(ByVal position As Integer)
		MyPosition = position
	End Sub

	Public Sub SetPosition(ByVal position As Integer)
		MyPosition = position
	End Sub

	''' <summary>
	''' Adjust our position by a certain amount of long branches
	''' </summary>
	''' <param name="longBranchCount"></param>
	''' <remarks></remarks>
	Public Sub AdjustForLongBranch(ByVal longBranchCount As Integer)
		MyPosition += longBranchCount * LongBranchAdjust
	End Sub

	''' <summary>
	''' Determine if this branch is long
	''' </summary>
	''' <param name="target"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function IsLongBranch(ByVal target As ILLocation) As Boolean
		' The branch offset is relative to the instruction *after* the branch so we add 2 (length of a br_s) to our position
		Return Utility.IsLongBranch(MyPosition + Br_s_Length, target.MyPosition)
	End Function

	Public Function Equals1(ByVal other As ILLocation) As Boolean Implements System.IEquatable(Of ILLocation).Equals
		Return MyPosition = other.MyPosition
	End Function

	Public Overrides Function ToString() As String
		Return MyPosition.ToString("x")
	End Function

	Public Function CompareTo(ByVal other As ILLocation) As Integer Implements System.IComparable(Of ILLocation).CompareTo
		Return MyPosition.CompareTo(other.MyPosition)
	End Function
End Class

''' <summary>
''' Represents a branch from a start location to an end location
''' </summary>
Friend Class BranchInfo
	Implements IEquatable(Of BranchInfo)

	Private MyStart As ILLocation
	Private MyEnd As ILLocation
	Private MyLabel As Label
	Private MyIsLongBranch As Boolean

	Public Sub New(ByVal startLocation As ILLocation, ByVal endLabel As Label)
		MyStart = startLocation
		MyLabel = endLabel
		MyEnd = New ILLocation()
	End Sub

	Public Sub AdjustForLongBranches(ByVal longBranchCount As Integer)
		MyStart.AdjustForLongBranch(longBranchCount)
	End Sub

	Public Sub BakeIsLongBranch()
		MyIsLongBranch = Me.ComputeIsLongBranch()
	End Sub

	Public Sub AdjustForLongBranchesBetween(ByVal betweenLongBranchCount As Integer)
		MyEnd.AdjustForLongBranch(betweenLongBranchCount)
	End Sub

	Public Function IsBetween(ByVal other As BranchInfo) As Boolean
		Return MyStart.CompareTo(other.MyStart) > 0 AndAlso MyStart.CompareTo(other.MyEnd) < 0
	End Function

	Public Function ComputeIsLongBranch() As Boolean
		Return MyStart.IsLongBranch(MyEnd)
	End Function

	Public Sub Mark(ByVal target As Label, ByVal position As Integer)
		If MyLabel.Equals(target) = True Then
			MyEnd.SetPosition(position)
		End If
	End Sub

	Public Function Equals1(ByVal other As BranchInfo) As Boolean Implements System.IEquatable(Of BranchInfo).Equals
		Return MyStart.Equals1(other.MyStart) AndAlso MyLabel.Equals(other.MyLabel)
	End Function

	Public Overrides Function ToString() As String
		Return String.Format("{0} -> {1} (L={2})", MyStart, MyEnd, MyStart.IsLongBranch(MyEnd))
	End Function

	Public ReadOnly Property IsLongBranch() As Boolean
		Get
			Return MyIsLongBranch
		End Get
	End Property
End Class