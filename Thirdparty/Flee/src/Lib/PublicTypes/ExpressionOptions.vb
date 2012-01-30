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
Imports System.Reflection
Imports System.Globalization

''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionOptions/Class/*' />
Public NotInheritable Class ExpressionOptions

	Private MyProperties As PropertyDictionary
	Private MyOwnerType As Type
	Private MyOwner As ExpressionContext

	Friend Event CaseSensitiveChanged As EventHandler

	Friend Sub New(ByVal owner As ExpressionContext)
		MyOwner = owner
		MyProperties = New PropertyDictionary()

		Me.InitializeProperties()
	End Sub

#Region "Methods - Private"

	Private Sub InitializeProperties()
		Me.StringComparison = System.StringComparison.Ordinal
		Me.OwnerMemberAccess = BindingFlags.Public

		MyProperties.SetToDefault(Of Boolean)("CaseSensitive")
		MyProperties.SetToDefault(Of Boolean)("Checked")
		MyProperties.SetToDefault(Of Boolean)("EmitToAssembly")
		MyProperties.SetToDefault(Of Type)("ResultType")
		MyProperties.SetToDefault(Of Boolean)("IsGeneric")
		MyProperties.SetToDefault(Of Boolean)("IntegersAsDoubles")
		MyProperties.SetValue("ParseCulture", CultureInfo.CurrentCulture)
		Me.SetParseCulture(Me.ParseCulture)
		MyProperties.SetValue("RealLiteralDataType", RealLiteralDataType.Double)
	End Sub

	Private Sub SetParseCulture(ByVal ci As CultureInfo)
		Dim po As ExpressionParserOptions = MyOwner.ParserOptions
		po.DecimalSeparator = ci.NumberFormat.NumberDecimalSeparator
		po.FunctionArgumentSeparator = ci.TextInfo.ListSeparator
		po.DateTimeFormat = ci.DateTimeFormat.ShortDatePattern
	End Sub

#End Region

#Region "Methods - Internal"

	Friend Function Clone() As ExpressionOptions
		Dim clonedOptions As ExpressionOptions = Me.MemberwiseClone()
		clonedOptions.MyProperties = MyProperties.Clone()
		Return clonedOptions
	End Function

	Friend Function IsOwnerType(ByVal t As Type) As Boolean
		Return Me.MyOwnerType.IsAssignableFrom(t)
	End Function

	Friend Sub SetOwnerType(ByVal ownerType As Type)
		MyOwnerType = ownerType
	End Sub

#End Region

#Region "Properties - Public"
	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionOptions/ResultType/*' />
	Public Property ResultType() As Type
		Get
			Return MyProperties.GetValue(Of Type)("ResultType")
		End Get
		Set(ByVal value As Type)
			Utility.AssertNotNull(value, "value")
			MyProperties.SetValue("ResultType", value)
		End Set
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionOptions/Checked/*' />
	Public Property Checked() As Boolean
		Get
			Return MyProperties.GetValue(Of Boolean)("Checked")
		End Get
		Set(ByVal value As Boolean)
			MyProperties.SetValue("Checked", value)
		End Set
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionOptions/StringComparison/*' />
	Public Property StringComparison() As StringComparison
		Get
			Return MyProperties.GetValue(Of StringComparison)("StringComparison")
		End Get
		Set(ByVal value As StringComparison)
			MyProperties.SetValue("StringComparison", value)
		End Set
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionOptions/EmitToAssembly/*' />
	Public Property EmitToAssembly() As Boolean
		Get
			Return MyProperties.GetValue(Of Boolean)("EmitToAssembly")
		End Get
		Set(ByVal value As Boolean)
			MyProperties.SetValue("EmitToAssembly", value)
		End Set
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionOptions/OwnerMemberAccess/*' />
	Public Property OwnerMemberAccess() As BindingFlags
		Get
			Return MyProperties.GetValue(Of BindingFlags)("OwnerMemberAccess")
		End Get
		Set(ByVal value As BindingFlags)
			MyProperties.SetValue("OwnerMemberAccess", value)
		End Set
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionOptions/CaseSensitive/*' />
	Public Property CaseSensitive() As Boolean
		Get
			Return MyProperties.GetValue(Of Boolean)("CaseSensitive")
		End Get
		Set(ByVal value As Boolean)
			If Me.CaseSensitive <> value Then
				MyProperties.SetValue("CaseSensitive", value)
				RaiseEvent CaseSensitiveChanged(Me, EventArgs.Empty)
			End If
		End Set
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionOptions/IntegersAsDoubles/*' />
	Public Property IntegersAsDoubles() As Boolean
		Get
			Return MyProperties.GetValue(Of Boolean)("IntegersAsDoubles")
		End Get
		Set(ByVal value As Boolean)
			MyProperties.SetValue("IntegersAsDoubles", value)
		End Set
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionOptions/ParseCulture/*' />
	Public Property ParseCulture() As CultureInfo
		Get
			Return MyProperties.GetValue(Of CultureInfo)("ParseCulture")
		End Get
		Set(ByVal value As CultureInfo)
			Utility.AssertNotNull(value, "ParseCulture")
			If (value.LCID <> Me.ParseCulture.LCID) Then
				MyProperties.SetValue("ParseCulture", value)
				Me.SetParseCulture(value)
				MyOwner.ParserOptions.RecreateParser()
			End If
		End Set
	End Property

	''' <include file='Resources/DocComments.xml' path='DocComments/ExpressionOptions/RealLiteralDataType/*' />
	Public Property RealLiteralDataType() As RealLiteralDataType
		Get
			Return MyProperties.GetValue(Of RealLiteralDataType)("RealLiteralDataType")
		End Get
		Set(ByVal value As RealLiteralDataType)
			MyProperties.SetValue("RealLiteralDataType", value)
		End Set
	End Property
#End Region

#Region "Properties - Non Public"
	Friend ReadOnly Property StringComparer() As IEqualityComparer(Of String)
		Get
			If Me.CaseSensitive = True Then
				Return System.StringComparer.Ordinal
			Else
				Return System.StringComparer.OrdinalIgnoreCase
			End If
		End Get
	End Property

	Friend ReadOnly Property MemberFilter() As MemberFilter
		Get
			If Me.CaseSensitive = True Then
				Return Type.FilterName
			Else
				Return Type.FilterNameIgnoreCase
			End If
		End Get
	End Property

	Friend ReadOnly Property MemberStringComparison() As StringComparison
		Get
			If Me.CaseSensitive = True Then
				Return System.StringComparison.Ordinal
			Else
				Return System.StringComparison.OrdinalIgnoreCase
			End If
		End Get
	End Property

	Friend ReadOnly Property OwnerType() As Type
		Get
			Return MyOwnerType
		End Get
	End Property

	Friend Property IsGeneric() As Boolean
		Get
			Return MyProperties.GetValue(Of Boolean)("IsGeneric")
		End Get
		Set(ByVal value As Boolean)
			MyProperties.SetValue("IsGeneric", value)
		End Set
	End Property
#End Region
End Class