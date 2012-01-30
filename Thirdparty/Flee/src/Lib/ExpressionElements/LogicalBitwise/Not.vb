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

Friend Class NotElement
	Inherits UnaryElement

	Public Overrides Sub Emit(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		If MyChild.ResultType Is GetType(Boolean) Then
			Me.EmitLogical(ilg, services)
		Else
			MyChild.Emit(ilg, services)
			ilg.Emit(OpCodes.Not)
		End If
	End Sub

	Private Sub EmitLogical(ByVal ilg As FleeILGenerator, ByVal services As IServiceProvider)
		MyChild.Emit(ilg, services)
		ilg.Emit(OpCodes.Ldc_I4_0)
		ilg.Emit(OpCodes.Ceq)
	End Sub

	Protected Overrides Function GetResultType(ByVal childType As System.Type) As System.Type
		If childType Is GetType(Boolean) Then
			Return GetType(Boolean)
		ElseIf Utility.IsIntegralType(childType) = True Then
			Return childType
		Else
			Return Nothing
		End If
	End Function
End Class