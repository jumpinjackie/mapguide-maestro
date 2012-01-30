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

Imports System.Globalization

''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionParserOptions/Class/*' />
Public Class ExpressionParserOptions

	Private MyProperties As PropertyDictionary
	Private MyOwner As ExpressionContext
	Private MyParseCulture As CultureInfo
	Private Const NumberStyles As NumberStyles = NumberStyles.AllowDecimalPoint Or NumberStyles.AllowExponent

	Friend Sub New(ByVal owner As ExpressionContext)
		MyOwner = owner
		MyProperties = New PropertyDictionary()
		MyParseCulture = CultureInfo.InvariantCulture.Clone()

		Me.InitializeProperties()
	End Sub

#Region "Methods - Public"

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionParserOptions/RecreateParser/*' />
	Public Sub RecreateParser()
		MyOwner.RecreateParser()
	End Sub

#End Region

#Region "Methods - Internal"

	Friend Function Clone() As ExpressionParserOptions
		Dim copy As ExpressionParserOptions = Me.MemberwiseClone()
		copy.MyProperties = MyProperties.Clone()
		Return copy
	End Function

	Friend Function ParseDouble(ByVal image As String) As Double
		Return Double.Parse(image, NumberStyles, MyParseCulture)
	End Function

	Friend Function ParseSingle(ByVal image As String) As Single
		Return Single.Parse(image, NumberStyles, MyParseCulture)
	End Function

	Friend Function ParseDecimal(ByVal image As String) As Decimal
		Return Decimal.Parse(image, NumberStyles, MyParseCulture)
	End Function
#End Region

#Region "Methods - Private"

	Private Sub InitializeProperties()
		Me.DateTimeFormat = "dd/MM/yyyy"
		Me.RequireDigitsBeforeDecimalPoint = False
		Me.DecimalSeparator = "."c
		Me.FunctionArgumentSeparator = ","c
	End Sub

#End Region

#Region "Properties - Public"

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionParserOptions/DateTimeFormat/*' />
	Public Property DateTimeFormat() As String
		Get
			Return MyProperties.GetValue(Of String)("DateTimeFormat")
		End Get
		Set(ByVal value As String)
			MyProperties.SetValue("DateTimeFormat", value)
		End Set
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionParserOptions/RequireDigitsBeforeDecimalPoint/*' />
	Public Property RequireDigitsBeforeDecimalPoint() As Boolean
		Get
			Return MyProperties.GetValue(Of Boolean)("RequireDigitsBeforeDecimalPoint")
		End Get
		Set(ByVal value As Boolean)
			MyProperties.SetValue("RequireDigitsBeforeDecimalPoint", value)
		End Set
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionParserOptions/DecimalSeparator/*' />
	Public Property DecimalSeparator() As Char
		Get
			Return MyProperties.GetValue(Of Char)("DecimalSeparator")
		End Get
		Set(ByVal value As Char)
			MyProperties.SetValue("DecimalSeparator", value)
			MyParseCulture.NumberFormat.NumberDecimalSeparator = value
		End Set
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionParserOptions/FunctionArgumentSeparator/*' />
	Public Property FunctionArgumentSeparator() As Char
		Get
			Return MyProperties.GetValue(Of Char)("FunctionArgumentSeparator")
		End Get
		Set(ByVal value As Char)
			MyProperties.SetValue("FunctionArgumentSeparator", value)
		End Set
	End Property

#End Region

End Class
