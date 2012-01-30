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

Friend MustInherit Class IntegralLiteralElement
	Inherits LiteralElement

	Protected Sub New()

	End Sub

	' Attempt to find the first type of integer that a number can fit into
	Public Shared Function Create(ByVal image As String, ByVal isHex As Boolean, ByVal negated As Boolean, ByVal services As IServiceProvider) As LiteralElement
		Dim comparison As StringComparison = StringComparison.OrdinalIgnoreCase

		If isHex = False Then
			' Create a real element if required
			Dim realElement As LiteralElement = RealLiteralElement.CreateFromInteger(image, services)

			If Not realElement Is Nothing Then
				Return realElement
			End If
		End If

		Dim hasUSuffix As Boolean = image.EndsWith("u", comparison) And Not image.EndsWith("lu", comparison)
		Dim hasLSuffix As Boolean = image.EndsWith("l", comparison) And Not image.EndsWith("ul", comparison)
		Dim hasULSuffix As Boolean = image.EndsWith("ul", comparison) Or image.EndsWith("lu", comparison)
		Dim hasSuffix As Boolean = hasUSuffix Or hasLSuffix Or hasULSuffix

		Dim constant As LiteralElement
		Dim numStyles As System.Globalization.NumberStyles = Globalization.NumberStyles.Integer

		If isHex = True Then
			numStyles = Globalization.NumberStyles.AllowHexSpecifier
			image = image.Remove(0, 2)
		End If

		If hasSuffix = False Then
			' If the literal has no suffix, it has the first of these types in which its value can be represented: int, uint, long, ulong.
			constant = Int32LiteralElement.TryCreate(image, isHex, negated)

			If Not constant Is Nothing Then
				Return constant
			End If

			constant = UInt32LiteralElement.TryCreate(image, numStyles)

			If Not constant Is Nothing Then
				Return constant
			End If

			constant = Int64LiteralElement.TryCreate(image, isHex, negated)

			If Not constant Is Nothing Then
				Return constant
			End If

			Return New UInt64LiteralElement(image, numStyles)
		ElseIf hasUSuffix = True Then
			image = image.Remove(image.Length - 1)
			' If the literal is suffixed by U or u, it has the first of these types in which its value can be represented: uint, ulong.

			constant = UInt32LiteralElement.TryCreate(image, numStyles)

			If Not constant Is Nothing Then
				Return constant
			Else
				Return New UInt64LiteralElement(image, numStyles)
			End If
		ElseIf hasLSuffix = True Then
			' If the literal is suffixed by L or l, it has the first of these types in which its value can be represented: long, ulong.
			image = image.Remove(image.Length - 1)

			constant = Int64LiteralElement.TryCreate(image, isHex, negated)

			If Not constant Is Nothing Then
				Return constant
			Else
				Return New UInt64LiteralElement(image, numStyles)
			End If
		Else
			' If the literal is suffixed by UL, Ul, uL, ul, LU, Lu, lU, or lu, it is of type ulong.
			Debug.Assert(hasULSuffix = True, "expecting ul suffix")
			image = image.Remove(image.Length - 2)
			Return New UInt64LiteralElement(image, numStyles)
		End If
	End Function
End Class