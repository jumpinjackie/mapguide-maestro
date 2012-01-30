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

Imports System.Text.RegularExpressions

Friend Class FleeExpressionAnalyzer
	Inherits ExpressionAnalyzer

	Private MyServices As IServiceProvider
	Private MyUnicodeEscapeRegex As Regex
	Private MyRegularEscapeRegex As Regex
	Private MyInUnaryNegate As Boolean

	Friend Sub New()
		MyUnicodeEscapeRegex = New Regex("\\u[0-9a-f]{4}", RegexOptions.IgnoreCase)
		MyRegularEscapeRegex = New Regex("\\[\\""'trn]", RegexOptions.IgnoreCase)
	End Sub

	Public Sub SetServices(ByVal services As IServiceProvider)
		MyServices = services
	End Sub

	Public Sub Reset()
		MyServices = Nothing
	End Sub

	Public Overrides Function ExitExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddFirstChildValue(node)
		Return node
	End Function

	Public Overrides Function ExitExpressionGroup(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		node.AddValues(Me.GetChildValues(node))
		Return node
	End Function

	Public Overrides Function ExitXorExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddBinaryOp(node, GetType(XorElement))
		Return node
	End Function

	Public Overrides Function ExitOrExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddBinaryOp(node, GetType(AndOrElement))
		Return node
	End Function

	Public Overrides Function ExitAndExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddBinaryOp(node, GetType(AndOrElement))
		Return node
	End Function

	Public Overrides Function ExitNotExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddUnaryOp(node, GetType(NotElement))
		Return node
	End Function

	Public Overrides Function ExitCompareExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddBinaryOp(node, GetType(CompareElement))
		Return node
	End Function

	Public Overrides Function ExitShiftExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddBinaryOp(node, GetType(ShiftElement))
		Return node
	End Function

	Public Overrides Function ExitAdditiveExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddBinaryOp(node, GetType(ArithmeticElement))
		Return node
	End Function

	Public Overrides Function ExitMultiplicativeExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddBinaryOp(node, GetType(ArithmeticElement))
		Return node
	End Function

	Public Overrides Function ExitPowerExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddBinaryOp(node, GetType(ArithmeticElement))
		Return node
	End Function

	' Try to fold a negated constant int32.  We have to do this so that parsing int32.MinValue will work
	Public Overrides Function ExitNegateExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Dim childValues As IList = Me.GetChildValues(node)

		' Get last child
		Dim childElement As ExpressionElement = childValues.Item(childValues.Count - 1)

		' Is it an signed integer constant?
		If childElement.GetType() Is GetType(Int32LiteralElement) And childValues.Count = 2 Then
			DirectCast(childElement, Int32LiteralElement).Negate()
			' Add it directly instead of the negate element since it will already be negated
			node.AddValue(childElement)
		ElseIf childElement.GetType() Is GetType(Int64LiteralElement) And childValues.Count = 2 Then
			DirectCast(childElement, Int64LiteralElement).Negate()
			' Add it directly instead of the negate element since it will already be negated
			node.AddValue(childElement)
		Else
			' No so just add a regular negate
			Me.AddUnaryOp(node, GetType(NegateElement))
		End If

		Return node
	End Function

	Public Overrides Function ExitMemberExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Dim childValues As IList = Me.GetChildValues(node)
		Dim first As Object = childValues.Item(0)

		If childValues.Count = 1 AndAlso Not TypeOf (first) Is MemberElement Then
			node.AddValue(first)
		Else
			Dim list As New InvocationListElement(childValues, MyServices)
			node.AddValue(list)
		End If

		Return node
	End Function

	Public Overrides Function ExitIndexExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Dim childValues As IList = Me.GetChildValues(node)
		Dim args As New ArgumentList(childValues)
		Dim e As New IndexerElement(args)
		node.AddValue(e)
		Return node
	End Function

	Public Overrides Function ExitMemberAccessExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(node.GetChildAt(1).GetValue(0))
		Return node
	End Function

	Public Overrides Function ExitSpecialFunctionExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddFirstChildValue(node)
		Return node
	End Function

	Public Overrides Function ExitIfExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Dim childValues As IList = Me.GetChildValues(node)
		Dim op As New ConditionalElement(childValues.Item(0), childValues.Item(1), childValues.Item(2))
		node.AddValue(op)
		Return node
	End Function

	Public Overrides Function ExitInExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Dim childValues As IList = Me.GetChildValues(node)

		If childValues.Count = 1 Then
			Me.AddFirstChildValue(node)
			Return node
		End If

		Dim operand As ExpressionElement = childValues.Item(0)
		childValues.RemoveAt(0)

		Dim second As Object = childValues.Item(0)
		Dim op As InElement

		If TypeOf (second) Is IList Then
			op = New InElement(operand, DirectCast(second, IList))
		Else
			Dim il As New InvocationListElement(childValues, MyServices)
			op = New InElement(operand, il)
		End If

		node.AddValue(op)
		Return node
	End Function

	Public Overrides Function ExitInTargetExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddFirstChildValue(node)
		Return node
	End Function

	Public Overrides Function ExitInListTargetExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Dim childValues As IList = Me.GetChildValues(node)
		node.AddValue(childValues)
		Return node
	End Function

	Public Overrides Function ExitCastExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Dim childValues As IList = Me.GetChildValues(node)
		Dim destTypeParts As String() = DirectCast(childValues.Item(1), String())
		Dim isArray As Boolean = DirectCast(childValues.Item(2), Boolean)
		Dim op As New CastElement(childValues.Item(0), destTypeParts, isArray, MyServices)
		node.AddValue(op)
		Return node
	End Function

	Public Overrides Function ExitCastTypeExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Dim childValues As IList = Me.GetChildValues(node)
		Dim parts As New List(Of String)()

		For Each part As String In childValues
			parts.Add(part)
		Next

		Dim isArray As Boolean = False

		If parts.Item(parts.Count - 1) = "[]" Then
			isArray = True
			parts.RemoveAt(parts.Count - 1)
		End If

		node.AddValue(parts.ToArray())
		node.AddValue(isArray)
		Return node
	End Function

	Public Overrides Function ExitMemberFunctionExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddFirstChildValue(node)
		Return node
	End Function

	Public Overrides Function ExitFieldPropertyExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Dim name As String = DirectCast(node.GetChildAt(0).GetValue(0), String)
		Dim elem As New IdentifierElement(name)
		node.AddValue(elem)
		Return node
	End Function

	Public Overrides Function ExitFunctionCallExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Dim childValues As IList = Me.GetChildValues(node)
		Dim name As String = DirectCast(childValues.Item(0), String)
		childValues.RemoveAt(0)
		Dim args As New ArgumentList(childValues)
		Dim funcCall As New FunctionCallElement(name, args)
		node.AddValue(funcCall)
		Return node
	End Function

	Public Overrides Function ExitArgumentList(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Dim childValues As IList = Me.GetChildValues(node)
		node.AddValues(childValues)
		Return node
	End Function

	Public Overrides Function ExitBasicExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddFirstChildValue(node)
		Return node
	End Function

	Public Overrides Function ExitLiteralExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddFirstChildValue(node)
		Return node
	End Function

	Private Sub AddFirstChildValue(ByVal node As PerCederberg.Grammatica.Runtime.Production)
		node.AddValue(Me.GetChildAt(node, 0).Values.Item(0))
	End Sub

	Private Sub AddUnaryOp(ByVal node As PerCederberg.Grammatica.Runtime.Production, ByVal elementType As Type)
		Dim childValues As IList = Me.GetChildValues(node)

		If childValues.Count = 2 Then
			Dim element As UnaryElement = Activator.CreateInstance(elementType)
			element.SetChild(childValues.Item(1))
			node.AddValue(element)
		Else
			node.AddValue(childValues.Item(0))
		End If
	End Sub

	Private Sub AddBinaryOp(ByVal node As PerCederberg.Grammatica.Runtime.Production, ByVal elementType As Type)
		Dim childValues As IList = Me.GetChildValues(node)

		If childValues.Count > 1 Then
			Dim e As BinaryExpressionElement = BinaryExpressionElement.CreateElement(childValues, elementType)
			node.AddValue(e)
		ElseIf childValues.Count = 1 Then
			node.AddValue(childValues.Item(0))
		Else
			Debug.Assert(False, "wrong number of chilren")
		End If
	End Sub

	Public Overrides Function ExitREAL(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		Dim image As String = node.Image
		Dim element As LiteralElement = RealLiteralElement.Create(image, MyServices)

		node.AddValue(element)
		Return node
	End Function

	Public Overrides Function ExitINTEGER(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		Dim element As LiteralElement = IntegralLiteralElement.Create(node.Image, False, MyInUnaryNegate, MyServices)
		node.AddValue(element)
		Return node
	End Function

	Public Overrides Function ExitHEXLITERAL(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		Dim element As LiteralElement = IntegralLiteralElement.Create(node.Image, True, MyInUnaryNegate, MyServices)
		node.AddValue(element)
		Return node
	End Function

	Public Overrides Function ExitBooleanLiteralExpression(ByVal node As PerCederberg.Grammatica.Runtime.Production) As PerCederberg.Grammatica.Runtime.Node
		Me.AddFirstChildValue(node)
		Return node
	End Function

	Public Overrides Function ExitTRUE(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(New BooleanLiteralElement(True))
		Return node
	End Function

	Public Overrides Function ExitFALSE(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(New BooleanLiteralElement(False))
		Return node
	End Function

	Public Overrides Function ExitSTRINGLITERAL(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		Dim s As String = Me.DoEscapes(node.Image)
		Dim element As New StringLiteralElement(s)
		node.AddValue(element)
		Return node
	End Function

	Public Overrides Function ExitCHARLITERAL(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		Dim s As String = Me.DoEscapes(node.Image)
		node.AddValue(New CharLiteralElement(s.Chars(0)))
		Return node
	End Function

	Public Overrides Function ExitDatetime(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		Dim context As ExpressionContext = MyServices.GetService(GetType(ExpressionContext))
		Dim image As String = node.Image.Substring(1, node.Image.Length - 2)
		Dim element As New DateTimeLiteralElement(image, context)
		node.AddValue(element)
		Return node
	End Function

	Public Overrides Function ExitTimeSpan(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		Dim image As String = node.Image.Substring(2, node.Image.Length - 3)
		Dim element As New TimeSpanLiteralElement(image)
		node.AddValue(element)
		Return node
	End Function

	Private Function DoEscapes(ByVal image As String) As String
		' Remove outer quotes
		image = image.Substring(1, image.Length - 2)
		image = MyUnicodeEscapeRegex.Replace(image, AddressOf UnicodeEscapeMatcher)
		image = MyRegularEscapeRegex.Replace(image, AddressOf RegularEscapeMatcher)
		Return image
	End Function

	Private Function RegularEscapeMatcher(ByVal m As Match) As String
		Dim s As String = m.Value
		' Remove leading \
		s = s.Remove(0, 1)

		Select Case s
			Case "\", """", "'"
				Return s
			Case "t", "T"
				Return Convert.ToChar(9).ToString()
			Case "n", "N"
				Return Convert.ToChar(10).ToString()
			Case "r", "R"
				Return Convert.ToChar(13).ToString()
			Case Else
				Debug.Assert(False, "Unrecognized escape sequence")
				Return Nothing
		End Select
	End Function

	Private Function UnicodeEscapeMatcher(ByVal m As Match) As String
		Dim s As String = m.Value
		' Remove \u
		s = s.Remove(0, 2)
		Dim code As Integer = Integer.Parse(s, Globalization.NumberStyles.AllowHexSpecifier)
		Dim c As Char = Convert.ToChar(code)
		Return c.ToString()
	End Function

	Public Overrides Function ExitIDENTIFIER(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(node.Image)
		Return node
	End Function

	Public Overrides Function ExitNullLiteral(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(New NullLiteralElement())
		Return node
	End Function

	Public Overrides Function ExitArrayBraces(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue("[]")
		Return node
	End Function

	Public Overrides Function ExitADD(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(BinaryArithmeticOperation.Add)
		Return node
	End Function

	Public Overrides Function ExitSUB(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(BinaryArithmeticOperation.Subtract)
		Return node
	End Function

	Public Overrides Function ExitMUL(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(BinaryArithmeticOperation.Multiply)
		Return node
	End Function

	Public Overrides Function ExitDIV(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(BinaryArithmeticOperation.Divide)
		Return node
	End Function

	Public Overrides Function ExitMOD(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(BinaryArithmeticOperation.Mod)
		Return node
	End Function

	Public Overrides Function ExitPOWER(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(BinaryArithmeticOperation.Power)
		Return node
	End Function

	Public Overrides Function ExitEQ(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(LogicalCompareOperation.Equal)
		Return node
	End Function

	Public Overrides Function ExitNE(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(LogicalCompareOperation.NotEqual)
		Return node
	End Function

	Public Overrides Function ExitLT(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(LogicalCompareOperation.LessThan)
		Return node
	End Function

	Public Overrides Function ExitGT(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(LogicalCompareOperation.GreaterThan)
		Return node
	End Function

	Public Overrides Function ExitLTE(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(LogicalCompareOperation.LessThanOrEqual)
		Return node
	End Function

	Public Overrides Function ExitGTE(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(LogicalCompareOperation.GreaterThanOrEqual)
		Return node
	End Function

	Public Overrides Function ExitAND(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(AndOrOperation.And)
		Return node
	End Function

	Public Overrides Function ExitOR(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(AndOrOperation.Or)
		Return node
	End Function

	Public Overrides Function ExitXOR(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue("Xor")
		Return node
	End Function

	Public Overrides Function ExitNOT(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(String.Empty)
		Return node
	End Function

	Public Overrides Function ExitLeftShift(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(ShiftOperation.LeftShift)
		Return node
	End Function

	Public Overrides Function ExitRightShift(ByVal node As PerCederberg.Grammatica.Runtime.Token) As PerCederberg.Grammatica.Runtime.Node
		node.AddValue(ShiftOperation.RightShift)
		Return node
	End Function

	Public Overrides Sub Child(ByVal node As PerCederberg.Grammatica.Runtime.Production, ByVal child As PerCederberg.Grammatica.Runtime.Node)
		MyBase.Child(node, child)
		MyInUnaryNegate = node.Id = ExpressionConstants.NEGATE_EXPRESSION And child.Id = ExpressionConstants.SUB
	End Sub
End Class