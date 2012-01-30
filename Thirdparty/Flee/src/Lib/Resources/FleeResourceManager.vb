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

Imports System.Resources

Friend Class FleeResourceManager

	Private MyResourceManagers As Dictionary(Of String, ResourceManager)
	Private Shared OurInstance As New FleeResourceManager

	Private Sub New()
		MyResourceManagers = New Dictionary(Of String, ResourceManager)(StringComparer.OrdinalIgnoreCase)
	End Sub

	Private Function GetResourceManager(ByVal resourceFile As String) As ResourceManager
		SyncLock Me
			Dim rm As ResourceManager = Nothing
			If MyResourceManagers.TryGetValue(resourceFile, rm) = False Then
				Dim t As Type = GetType(FleeResourceManager)
				rm = New ResourceManager(String.Format("{0}.{1}", t.Namespace, resourceFile), t.Assembly)
				MyResourceManagers.Add(resourceFile, rm)
			End If
			Return rm
		End SyncLock
	End Function

	Private Function GetResourceString(ByVal resourceFile As String, ByVal key As String) As String
		Dim rm As ResourceManager = Me.GetResourceManager(resourceFile)
		Return rm.GetString(key)
	End Function

	Public Function GetCompileErrorString(ByVal key As String) As String
		Return Me.GetResourceString("CompileErrors", key)
	End Function

	Public Function GetElementNameString(ByVal key As String) As String
		Return Me.GetResourceString("ElementNames", key)
	End Function

	Public Function GetGeneralErrorString(ByVal key As String) As String
		Return Me.GetResourceString("GeneralErrors", key)
	End Function

	Public Shared ReadOnly Property Instance() As FleeResourceManager
		Get
			Return OurInstance
		End Get
	End Property
End Class