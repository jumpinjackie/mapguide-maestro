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

Friend MustInherit Class RealLiteralElement
	Inherits LiteralElement

	Protected Sub New()

	End Sub

	Public Shared Function CreateFromInteger(ByVal image As String, ByVal services As IServiceProvider) As LiteralElement
		Dim element As LiteralElement

		element = CreateSingle(image, services)

		If Not element Is Nothing Then
			Return element
		End If

		element = CreateDecimal(image, services)

		If Not element Is Nothing Then
			Return element
		End If

		Dim options As ExpressionOptions = services.GetService(GetType(ExpressionOptions))

		' Convert to a double if option is set
		If options.IntegersAsDoubles = True Then
			Return DoubleLiteralElement.Parse(image, services)
		End If

		Return Nothing
	End Function

	Public Shared Function Create(ByVal image As String, ByVal services As IServiceProvider) As LiteralElement
		Dim element As LiteralElement

		element = CreateSingle(image, services)

		If Not element Is Nothing Then
			Return element
		End If

		element = CreateDecimal(image, services)

		If Not element Is Nothing Then
			Return element
		End If

		element = CreateDouble(image, services)

		If Not element Is Nothing Then
			Return element
		End If

		element = CreateImplicitReal(image, services)

		Return element
	End Function

	Private Shared Function CreateImplicitReal(ByVal image As String, ByVal services As IServiceProvider) As LiteralElement
		Dim options As ExpressionOptions = services.GetService(GetType(ExpressionOptions))
		Dim realType As RealLiteralDataType = options.RealLiteralDataType

		Select Case realType
			Case RealLiteralDataType.Double
				Return DoubleLiteralElement.Parse(image, services)
			Case RealLiteralDataType.Single
				Return SingleLiteralElement.Parse(image, services)
			Case RealLiteralDataType.Decimal
				Return DecimalLiteralElement.Parse(image, services)
			Case Else
				Debug.Fail("Unknown value")
				Return Nothing
		End Select
	End Function

	Private Shared Function CreateDouble(ByVal image As String, ByVal services As IServiceProvider) As DoubleLiteralElement
		If image.EndsWith("d", StringComparison.OrdinalIgnoreCase) = True Then
			image = image.Remove(image.Length - 1)
			Return DoubleLiteralElement.Parse(image, services)
		Else
			Return Nothing
		End If
	End Function

	Private Shared Function CreateSingle(ByVal image As String, ByVal services As IServiceProvider) As SingleLiteralElement
		If image.EndsWith("f", StringComparison.OrdinalIgnoreCase) = True Then
			image = image.Remove(image.Length - 1)
			Return SingleLiteralElement.Parse(image, services)
		Else
			Return Nothing
		End If
	End Function

	Private Shared Function CreateDecimal(ByVal image As String, ByVal services As IServiceProvider) As DecimalLiteralElement
		If image.EndsWith("m", StringComparison.OrdinalIgnoreCase) = True Then
			image = image.Remove(image.Length - 1)
			Return DecimalLiteralElement.Parse(image, services)
		Else
			Return Nothing
		End If
	End Function
End Class