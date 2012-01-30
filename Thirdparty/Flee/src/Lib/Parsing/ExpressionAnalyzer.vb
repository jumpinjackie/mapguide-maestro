' ExpressionAnalyzer.vb
'
' THIS FILE HAS BEEN GENERATED AUTOMATICALLY. DO NOT EDIT!
'
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
'
' Copyright (c) 2007 Eugene Ciloci

Imports Ciloci.Flee.PerCederberg.Grammatica.Runtime

'''<remarks>A class providing callback methods for the
'''parser.</remarks>
Friend MustInherit Class ExpressionAnalyzer
    Inherits Analyzer

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overrides Sub Enter(ByVal node As Node)
        Select Case node.Id
        Case ExpressionConstants.ADD
            EnterAdd(CType(node,Token))

        Case ExpressionConstants.SUB
            EnterSub(CType(node,Token))

        Case ExpressionConstants.MUL
            EnterMul(CType(node,Token))

        Case ExpressionConstants.DIV
            EnterDiv(CType(node,Token))

        Case ExpressionConstants.POWER
            EnterPower(CType(node,Token))

        Case ExpressionConstants.MOD
            EnterMod(CType(node,Token))

        Case ExpressionConstants.LEFT_PAREN
            EnterLeftParen(CType(node,Token))

        Case ExpressionConstants.RIGHT_PAREN
            EnterRightParen(CType(node,Token))

        Case ExpressionConstants.LEFT_BRACE
            EnterLeftBrace(CType(node,Token))

        Case ExpressionConstants.RIGHT_BRACE
            EnterRightBrace(CType(node,Token))

        Case ExpressionConstants.EQ
            EnterEq(CType(node,Token))

        Case ExpressionConstants.LT
            EnterLt(CType(node,Token))

        Case ExpressionConstants.GT
            EnterGt(CType(node,Token))

        Case ExpressionConstants.LTE
            EnterLte(CType(node,Token))

        Case ExpressionConstants.GTE
            EnterGte(CType(node,Token))

        Case ExpressionConstants.NE
            EnterNe(CType(node,Token))

        Case ExpressionConstants.AND
            EnterAnd(CType(node,Token))

        Case ExpressionConstants.OR
            EnterOr(CType(node,Token))

        Case ExpressionConstants.XOR
            EnterXor(CType(node,Token))

        Case ExpressionConstants.NOT
            EnterNot(CType(node,Token))

        Case ExpressionConstants.IN
            EnterIn(CType(node,Token))

        Case ExpressionConstants.DOT
            EnterDot(CType(node,Token))

        Case ExpressionConstants.ARGUMENT_SEPARATOR
            EnterArgumentSeparator(CType(node,Token))

        Case ExpressionConstants.ARRAY_BRACES
            EnterArrayBraces(CType(node,Token))

        Case ExpressionConstants.LEFT_SHIFT
            EnterLeftShift(CType(node,Token))

        Case ExpressionConstants.RIGHT_SHIFT
            EnterRightShift(CType(node,Token))

        Case ExpressionConstants.INTEGER
            EnterInteger(CType(node,Token))

        Case ExpressionConstants.REAL
            EnterReal(CType(node,Token))

        Case ExpressionConstants.STRING_LITERAL
            EnterStringLiteral(CType(node,Token))

        Case ExpressionConstants.CHAR_LITERAL
            EnterCharLiteral(CType(node,Token))

        Case ExpressionConstants.TRUE
            EnterTrue(CType(node,Token))

        Case ExpressionConstants.FALSE
            EnterFalse(CType(node,Token))

        Case ExpressionConstants.IDENTIFIER
            EnterIdentifier(CType(node,Token))

        Case ExpressionConstants.HEX_LITERAL
            EnterHexLiteral(CType(node,Token))

        Case ExpressionConstants.NULL_LITERAL
            EnterNullLiteral(CType(node,Token))

        Case ExpressionConstants.TIMESPAN
            EnterTimespan(CType(node,Token))

        Case ExpressionConstants.DATETIME
            EnterDatetime(CType(node,Token))

        Case ExpressionConstants.IF
            EnterIf(CType(node,Token))

        Case ExpressionConstants.CAST
            EnterCast(CType(node,Token))

        Case ExpressionConstants.EXPRESSION
            EnterExpression(CType(node,Production))

        Case ExpressionConstants.XOR_EXPRESSION
            EnterXorExpression(CType(node,Production))

        Case ExpressionConstants.OR_EXPRESSION
            EnterOrExpression(CType(node,Production))

        Case ExpressionConstants.AND_EXPRESSION
            EnterAndExpression(CType(node,Production))

        Case ExpressionConstants.NOT_EXPRESSION
            EnterNotExpression(CType(node,Production))

        Case ExpressionConstants.IN_EXPRESSION
            EnterInExpression(CType(node,Production))

        Case ExpressionConstants.IN_TARGET_EXPRESSION
            EnterInTargetExpression(CType(node,Production))

        Case ExpressionConstants.IN_LIST_TARGET_EXPRESSION
            EnterInListTargetExpression(CType(node,Production))

        Case ExpressionConstants.COMPARE_EXPRESSION
            EnterCompareExpression(CType(node,Production))

        Case ExpressionConstants.SHIFT_EXPRESSION
            EnterShiftExpression(CType(node,Production))

        Case ExpressionConstants.ADDITIVE_EXPRESSION
            EnterAdditiveExpression(CType(node,Production))

        Case ExpressionConstants.MULTIPLICATIVE_EXPRESSION
            EnterMultiplicativeExpression(CType(node,Production))

        Case ExpressionConstants.POWER_EXPRESSION
            EnterPowerExpression(CType(node,Production))

        Case ExpressionConstants.NEGATE_EXPRESSION
            EnterNegateExpression(CType(node,Production))

        Case ExpressionConstants.MEMBER_EXPRESSION
            EnterMemberExpression(CType(node,Production))

        Case ExpressionConstants.MEMBER_ACCESS_EXPRESSION
            EnterMemberAccessExpression(CType(node,Production))

        Case ExpressionConstants.BASIC_EXPRESSION
            EnterBasicExpression(CType(node,Production))

        Case ExpressionConstants.MEMBER_FUNCTION_EXPRESSION
            EnterMemberFunctionExpression(CType(node,Production))

        Case ExpressionConstants.FIELD_PROPERTY_EXPRESSION
            EnterFieldPropertyExpression(CType(node,Production))

        Case ExpressionConstants.SPECIAL_FUNCTION_EXPRESSION
            EnterSpecialFunctionExpression(CType(node,Production))

        Case ExpressionConstants.IF_EXPRESSION
            EnterIfExpression(CType(node,Production))

        Case ExpressionConstants.CAST_EXPRESSION
            EnterCastExpression(CType(node,Production))

        Case ExpressionConstants.CAST_TYPE_EXPRESSION
            EnterCastTypeExpression(CType(node,Production))

        Case ExpressionConstants.INDEX_EXPRESSION
            EnterIndexExpression(CType(node,Production))

        Case ExpressionConstants.FUNCTION_CALL_EXPRESSION
            EnterFunctionCallExpression(CType(node,Production))

        Case ExpressionConstants.ARGUMENT_LIST
            EnterArgumentList(CType(node,Production))

        Case ExpressionConstants.LITERAL_EXPRESSION
            EnterLiteralExpression(CType(node,Production))

        Case ExpressionConstants.BOOLEAN_LITERAL_EXPRESSION
            EnterBooleanLiteralExpression(CType(node,Production))

        Case ExpressionConstants.EXPRESSION_GROUP
            EnterExpressionGroup(CType(node,Production))

        End Select
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overrides Function [Exit](ByVal node As Node) As Node
        Select Case node.Id
        Case ExpressionConstants.ADD
            return ExitAdd(CType(node,Token))

        Case ExpressionConstants.SUB
            return ExitSub(CType(node,Token))

        Case ExpressionConstants.MUL
            return ExitMul(CType(node,Token))

        Case ExpressionConstants.DIV
            return ExitDiv(CType(node,Token))

        Case ExpressionConstants.POWER
            return ExitPower(CType(node,Token))

        Case ExpressionConstants.MOD
            return ExitMod(CType(node,Token))

        Case ExpressionConstants.LEFT_PAREN
            return ExitLeftParen(CType(node,Token))

        Case ExpressionConstants.RIGHT_PAREN
            return ExitRightParen(CType(node,Token))

        Case ExpressionConstants.LEFT_BRACE
            return ExitLeftBrace(CType(node,Token))

        Case ExpressionConstants.RIGHT_BRACE
            return ExitRightBrace(CType(node,Token))

        Case ExpressionConstants.EQ
            return ExitEq(CType(node,Token))

        Case ExpressionConstants.LT
            return ExitLt(CType(node,Token))

        Case ExpressionConstants.GT
            return ExitGt(CType(node,Token))

        Case ExpressionConstants.LTE
            return ExitLte(CType(node,Token))

        Case ExpressionConstants.GTE
            return ExitGte(CType(node,Token))

        Case ExpressionConstants.NE
            return ExitNe(CType(node,Token))

        Case ExpressionConstants.AND
            return ExitAnd(CType(node,Token))

        Case ExpressionConstants.OR
            return ExitOr(CType(node,Token))

        Case ExpressionConstants.XOR
            return ExitXor(CType(node,Token))

        Case ExpressionConstants.NOT
            return ExitNot(CType(node,Token))

        Case ExpressionConstants.IN
            return ExitIn(CType(node,Token))

        Case ExpressionConstants.DOT
            return ExitDot(CType(node,Token))

        Case ExpressionConstants.ARGUMENT_SEPARATOR
            return ExitArgumentSeparator(CType(node,Token))

        Case ExpressionConstants.ARRAY_BRACES
            return ExitArrayBraces(CType(node,Token))

        Case ExpressionConstants.LEFT_SHIFT
            return ExitLeftShift(CType(node,Token))

        Case ExpressionConstants.RIGHT_SHIFT
            return ExitRightShift(CType(node,Token))

        Case ExpressionConstants.INTEGER
            return ExitInteger(CType(node,Token))

        Case ExpressionConstants.REAL
            return ExitReal(CType(node,Token))

        Case ExpressionConstants.STRING_LITERAL
            return ExitStringLiteral(CType(node,Token))

        Case ExpressionConstants.CHAR_LITERAL
            return ExitCharLiteral(CType(node,Token))

        Case ExpressionConstants.TRUE
            return ExitTrue(CType(node,Token))

        Case ExpressionConstants.FALSE
            return ExitFalse(CType(node,Token))

        Case ExpressionConstants.IDENTIFIER
            return ExitIdentifier(CType(node,Token))

        Case ExpressionConstants.HEX_LITERAL
            return ExitHexLiteral(CType(node,Token))

        Case ExpressionConstants.NULL_LITERAL
            return ExitNullLiteral(CType(node,Token))

        Case ExpressionConstants.TIMESPAN
            return ExitTimespan(CType(node,Token))

        Case ExpressionConstants.DATETIME
            return ExitDatetime(CType(node,Token))

        Case ExpressionConstants.IF
            return ExitIf(CType(node,Token))

        Case ExpressionConstants.CAST
            return ExitCast(CType(node,Token))

        Case ExpressionConstants.EXPRESSION
            return ExitExpression(CType(node,Production))

        Case ExpressionConstants.XOR_EXPRESSION
            return ExitXorExpression(CType(node,Production))

        Case ExpressionConstants.OR_EXPRESSION
            return ExitOrExpression(CType(node,Production))

        Case ExpressionConstants.AND_EXPRESSION
            return ExitAndExpression(CType(node,Production))

        Case ExpressionConstants.NOT_EXPRESSION
            return ExitNotExpression(CType(node,Production))

        Case ExpressionConstants.IN_EXPRESSION
            return ExitInExpression(CType(node,Production))

        Case ExpressionConstants.IN_TARGET_EXPRESSION
            return ExitInTargetExpression(CType(node,Production))

        Case ExpressionConstants.IN_LIST_TARGET_EXPRESSION
            return ExitInListTargetExpression(CType(node,Production))

        Case ExpressionConstants.COMPARE_EXPRESSION
            return ExitCompareExpression(CType(node,Production))

        Case ExpressionConstants.SHIFT_EXPRESSION
            return ExitShiftExpression(CType(node,Production))

        Case ExpressionConstants.ADDITIVE_EXPRESSION
            return ExitAdditiveExpression(CType(node,Production))

        Case ExpressionConstants.MULTIPLICATIVE_EXPRESSION
            return ExitMultiplicativeExpression(CType(node,Production))

        Case ExpressionConstants.POWER_EXPRESSION
            return ExitPowerExpression(CType(node,Production))

        Case ExpressionConstants.NEGATE_EXPRESSION
            return ExitNegateExpression(CType(node,Production))

        Case ExpressionConstants.MEMBER_EXPRESSION
            return ExitMemberExpression(CType(node,Production))

        Case ExpressionConstants.MEMBER_ACCESS_EXPRESSION
            return ExitMemberAccessExpression(CType(node,Production))

        Case ExpressionConstants.BASIC_EXPRESSION
            return ExitBasicExpression(CType(node,Production))

        Case ExpressionConstants.MEMBER_FUNCTION_EXPRESSION
            return ExitMemberFunctionExpression(CType(node,Production))

        Case ExpressionConstants.FIELD_PROPERTY_EXPRESSION
            return ExitFieldPropertyExpression(CType(node,Production))

        Case ExpressionConstants.SPECIAL_FUNCTION_EXPRESSION
            return ExitSpecialFunctionExpression(CType(node,Production))

        Case ExpressionConstants.IF_EXPRESSION
            return ExitIfExpression(CType(node,Production))

        Case ExpressionConstants.CAST_EXPRESSION
            return ExitCastExpression(CType(node,Production))

        Case ExpressionConstants.CAST_TYPE_EXPRESSION
            return ExitCastTypeExpression(CType(node,Production))

        Case ExpressionConstants.INDEX_EXPRESSION
            return ExitIndexExpression(CType(node,Production))

        Case ExpressionConstants.FUNCTION_CALL_EXPRESSION
            return ExitFunctionCallExpression(CType(node,Production))

        Case ExpressionConstants.ARGUMENT_LIST
            return ExitArgumentList(CType(node,Production))

        Case ExpressionConstants.LITERAL_EXPRESSION
            return ExitLiteralExpression(CType(node,Production))

        Case ExpressionConstants.BOOLEAN_LITERAL_EXPRESSION
            return ExitBooleanLiteralExpression(CType(node,Production))

        Case ExpressionConstants.EXPRESSION_GROUP
            return ExitExpressionGroup(CType(node,Production))

        End Select
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overrides Sub Child(ByVal node As Production, ByVal child As Node)
        Select Case node.Id
        Case ExpressionConstants.EXPRESSION
            ChildExpression(node, child)

        Case ExpressionConstants.XOR_EXPRESSION
            ChildXorExpression(node, child)

        Case ExpressionConstants.OR_EXPRESSION
            ChildOrExpression(node, child)

        Case ExpressionConstants.AND_EXPRESSION
            ChildAndExpression(node, child)

        Case ExpressionConstants.NOT_EXPRESSION
            ChildNotExpression(node, child)

        Case ExpressionConstants.IN_EXPRESSION
            ChildInExpression(node, child)

        Case ExpressionConstants.IN_TARGET_EXPRESSION
            ChildInTargetExpression(node, child)

        Case ExpressionConstants.IN_LIST_TARGET_EXPRESSION
            ChildInListTargetExpression(node, child)

        Case ExpressionConstants.COMPARE_EXPRESSION
            ChildCompareExpression(node, child)

        Case ExpressionConstants.SHIFT_EXPRESSION
            ChildShiftExpression(node, child)

        Case ExpressionConstants.ADDITIVE_EXPRESSION
            ChildAdditiveExpression(node, child)

        Case ExpressionConstants.MULTIPLICATIVE_EXPRESSION
            ChildMultiplicativeExpression(node, child)

        Case ExpressionConstants.POWER_EXPRESSION
            ChildPowerExpression(node, child)

        Case ExpressionConstants.NEGATE_EXPRESSION
            ChildNegateExpression(node, child)

        Case ExpressionConstants.MEMBER_EXPRESSION
            ChildMemberExpression(node, child)

        Case ExpressionConstants.MEMBER_ACCESS_EXPRESSION
            ChildMemberAccessExpression(node, child)

        Case ExpressionConstants.BASIC_EXPRESSION
            ChildBasicExpression(node, child)

        Case ExpressionConstants.MEMBER_FUNCTION_EXPRESSION
            ChildMemberFunctionExpression(node, child)

        Case ExpressionConstants.FIELD_PROPERTY_EXPRESSION
            ChildFieldPropertyExpression(node, child)

        Case ExpressionConstants.SPECIAL_FUNCTION_EXPRESSION
            ChildSpecialFunctionExpression(node, child)

        Case ExpressionConstants.IF_EXPRESSION
            ChildIfExpression(node, child)

        Case ExpressionConstants.CAST_EXPRESSION
            ChildCastExpression(node, child)

        Case ExpressionConstants.CAST_TYPE_EXPRESSION
            ChildCastTypeExpression(node, child)

        Case ExpressionConstants.INDEX_EXPRESSION
            ChildIndexExpression(node, child)

        Case ExpressionConstants.FUNCTION_CALL_EXPRESSION
            ChildFunctionCallExpression(node, child)

        Case ExpressionConstants.ARGUMENT_LIST
            ChildArgumentList(node, child)

        Case ExpressionConstants.LITERAL_EXPRESSION
            ChildLiteralExpression(node, child)

        Case ExpressionConstants.BOOLEAN_LITERAL_EXPRESSION
            ChildBooleanLiteralExpression(node, child)

        Case ExpressionConstants.EXPRESSION_GROUP
            ChildExpressionGroup(node, child)

        End Select
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterAdd(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitAdd(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterSub(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitSub(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterMul(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitMul(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterDiv(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitDiv(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterPower(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitPower(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterMod(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitMod(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterLeftParen(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitLeftParen(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterRightParen(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitRightParen(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterLeftBrace(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitLeftBrace(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterRightBrace(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitRightBrace(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterEq(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitEq(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterLt(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitLt(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterGt(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitGt(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterLte(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitLte(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterGte(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitGte(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterNe(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitNe(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterAnd(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitAnd(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterOr(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitOr(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterXor(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitXor(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterNot(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitNot(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterIn(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitIn(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterDot(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitDot(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterArgumentSeparator(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitArgumentSeparator(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterArrayBraces(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitArrayBraces(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterLeftShift(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitLeftShift(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterRightShift(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitRightShift(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterInteger(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitInteger(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterReal(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitReal(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterStringLiteral(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitStringLiteral(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterCharLiteral(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitCharLiteral(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterTrue(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitTrue(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterFalse(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitFalse(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterIdentifier(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitIdentifier(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterHexLiteral(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitHexLiteral(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterNullLiteral(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitNullLiteral(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterTimespan(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitTimespan(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterDatetime(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitDatetime(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterIf(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitIf(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterCast(ByVal node As Token)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitCast(ByVal node As Token) As Node
        Return node
    End Function

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterXorExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitXorExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildXorExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterOrExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitOrExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildOrExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterAndExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitAndExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildAndExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterNotExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitNotExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildNotExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterInExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitInExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildInExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterInTargetExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitInTargetExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildInTargetExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterInListTargetExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitInListTargetExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildInListTargetExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterCompareExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitCompareExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildCompareExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterShiftExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitShiftExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildShiftExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterAdditiveExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitAdditiveExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildAdditiveExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterMultiplicativeExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitMultiplicativeExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildMultiplicativeExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterPowerExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitPowerExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildPowerExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterNegateExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitNegateExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildNegateExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterMemberExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitMemberExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildMemberExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterMemberAccessExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitMemberAccessExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildMemberAccessExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterBasicExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitBasicExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildBasicExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterMemberFunctionExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitMemberFunctionExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildMemberFunctionExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterFieldPropertyExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitFieldPropertyExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildFieldPropertyExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterSpecialFunctionExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitSpecialFunctionExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildSpecialFunctionExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterIfExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitIfExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildIfExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterCastExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitCastExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildCastExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterCastTypeExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitCastTypeExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildCastTypeExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterIndexExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitIndexExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildIndexExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterFunctionCallExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitFunctionCallExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildFunctionCallExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterArgumentList(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitArgumentList(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildArgumentList(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterLiteralExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitLiteralExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildLiteralExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterBooleanLiteralExpression(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitBooleanLiteralExpression(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildBooleanLiteralExpression(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub

    '''<summary>Called when entering a parse tree node.</summary>
    '''
    '''<param name='node'>the node being entered</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub EnterExpressionGroup(ByVal node As Production)
    End Sub

    '''<summary>Called when exiting a parse tree node.</summary>
    '''
    '''<param name='node'>the node being exited</param>
    '''
    '''<returns>the node to add to the parse tree, or
    '''         null if no parse tree should be created</returns>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Function ExitExpressionGroup(ByVal node As Production) As Node
        Return node
    End Function

    '''<summary>Called when adding a child to a parse tree
    '''node.</summary>
    '''
    '''<param name='node'>the parent node</param>
    '''<param name='child'>the child node, or null</param>
    '''
    '''<exception cref='ParseException'>if the node analysis
    '''discovered errors</exception>
    Public Overridable Sub ChildExpressionGroup(ByVal node As Production, ByVal child As Node)
        node.AddChild(child)
    End Sub
End Class
