' ExpressionParser.vb
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

Imports System.IO

Imports Ciloci.Flee.PerCederberg.Grammatica.Runtime

'''<remarks>A token stream parser.</remarks>
Friend Class ExpressionParser
    Inherits RecursiveDescentParser

    '''<summary>An enumeration with the generated production node
    '''identity constants.</summary>
    Private Enum SynteticPatterns
        [SUBPRODUCTION_1] = 3001
        [SUBPRODUCTION_2] = 3002
        [SUBPRODUCTION_3] = 3003
        [SUBPRODUCTION_4] = 3004
        [SUBPRODUCTION_5] = 3005
        [SUBPRODUCTION_6] = 3006
        [SUBPRODUCTION_7] = 3007
        [SUBPRODUCTION_8] = 3008
        [SUBPRODUCTION_9] = 3009
        [SUBPRODUCTION_10] = 3010
        [SUBPRODUCTION_11] = 3011
        [SUBPRODUCTION_12] = 3012
        [SUBPRODUCTION_13] = 3013
        [SUBPRODUCTION_14] = 3014
        [SUBPRODUCTION_15] = 3015
        [SUBPRODUCTION_16] = 3016
	End Enum

	Public Sub New(ByVal input As TextReader, ByVal analyzer As Analyzer, ByVal context As Ciloci.Flee.ExpressionContext)
		MyBase.New(New ExpressionTokenizer(input, context), analyzer)
		CreatePatterns()
	End Sub

    '''<summary>Creates a new parser.</summary>
    '''
    '''<param name='input'>the input stream to read from</param>
    '''
    '''<exception cref='ParserCreationException'>if the parser
    '''couldn't be initialized correctly</exception>
	Public Sub New(ByVal input As TextReader)
		MyBase.New(New ExpressionTokenizer(input))
		CreatePatterns()
	End Sub

    '''<summary>Creates a new parser.</summary>
    '''
    '''<param name='input'>the input stream to read from</param>
    '''
    '''<param name='analyzer'>the analyzer to parse with</param>
    '''
    '''<exception cref='ParserCreationException'>if the parser
    '''couldn't be initialized correctly</exception>
	Public Sub New(ByVal input As TextReader, ByVal analyzer As Analyzer)
		MyBase.New(New ExpressionTokenizer(input), analyzer)
		CreatePatterns()
	End Sub

    '''<summary>Initializes the parser by creating all the production
    '''patterns.</summary>
    '''
    '''<exception cref='ParserCreationException'>if the parser
    '''couldn't be initialized correctly</exception>
    Private Sub CreatePatterns()
        Dim pattern As ProductionPattern
        Dim alt As ProductionPatternAlternative

        pattern = New ProductionPattern(CInt(ExpressionConstants.EXPRESSION), "Expression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.XOR_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.XOR_EXPRESSION), "XorExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.OR_EXPRESSION), 1, 1)
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_1), 0, -1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.OR_EXPRESSION), "OrExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.AND_EXPRESSION), 1, 1)
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_2), 0, -1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.AND_EXPRESSION), "AndExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.NOT_EXPRESSION), 1, 1)
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_3), 0, -1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.NOT_EXPRESSION), "NotExpression")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.NOT), 0, 1)
        alt.AddProduction(CInt(ExpressionConstants.IN_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.IN_EXPRESSION), "InExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.COMPARE_EXPRESSION), 1, 1)
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_4), 0, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.IN_TARGET_EXPRESSION), "InTargetExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.FIELD_PROPERTY_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.IN_LIST_TARGET_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.IN_LIST_TARGET_EXPRESSION), "InListTargetExpression")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.LEFT_PAREN), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.ARGUMENT_LIST), 1, 1)
        alt.AddToken(CInt(ExpressionConstants.RIGHT_PAREN), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.COMPARE_EXPRESSION), "CompareExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.SHIFT_EXPRESSION), 1, 1)
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_6), 0, -1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.SHIFT_EXPRESSION), "ShiftExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.ADDITIVE_EXPRESSION), 1, 1)
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_8), 0, -1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.ADDITIVE_EXPRESSION), "AdditiveExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.MULTIPLICATIVE_EXPRESSION), 1, 1)
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_10), 0, -1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.MULTIPLICATIVE_EXPRESSION), "MultiplicativeExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.POWER_EXPRESSION), 1, 1)
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_12), 0, -1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.POWER_EXPRESSION), "PowerExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.NEGATE_EXPRESSION), 1, 1)
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_13), 0, -1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.NEGATE_EXPRESSION), "NegateExpression")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.SUB), 0, 1)
        alt.AddProduction(CInt(ExpressionConstants.MEMBER_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.MEMBER_EXPRESSION), "MemberExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.BASIC_EXPRESSION), 1, 1)
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_14), 0, -1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.MEMBER_ACCESS_EXPRESSION), "MemberAccessExpression")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.DOT), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.MEMBER_FUNCTION_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.BASIC_EXPRESSION), "BasicExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.LITERAL_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.EXPRESSION_GROUP), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.MEMBER_FUNCTION_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.SPECIAL_FUNCTION_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.MEMBER_FUNCTION_EXPRESSION), "MemberFunctionExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.FIELD_PROPERTY_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.FUNCTION_CALL_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.FIELD_PROPERTY_EXPRESSION), "FieldPropertyExpression")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.IDENTIFIER), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.SPECIAL_FUNCTION_EXPRESSION), "SpecialFunctionExpression")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.IF_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.CAST_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.IF_EXPRESSION), "IfExpression")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.IF), 1, 1)
        alt.AddToken(CInt(ExpressionConstants.LEFT_PAREN), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.EXPRESSION), 1, 1)
        alt.AddToken(CInt(ExpressionConstants.ARGUMENT_SEPARATOR), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.EXPRESSION), 1, 1)
        alt.AddToken(CInt(ExpressionConstants.ARGUMENT_SEPARATOR), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.EXPRESSION), 1, 1)
        alt.AddToken(CInt(ExpressionConstants.RIGHT_PAREN), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.CAST_EXPRESSION), "CastExpression")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.CAST), 1, 1)
        alt.AddToken(CInt(ExpressionConstants.LEFT_PAREN), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.EXPRESSION), 1, 1)
        alt.AddToken(CInt(ExpressionConstants.ARGUMENT_SEPARATOR), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.CAST_TYPE_EXPRESSION), 1, 1)
        alt.AddToken(CInt(ExpressionConstants.RIGHT_PAREN), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.CAST_TYPE_EXPRESSION), "CastTypeExpression")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.IDENTIFIER), 1, 1)
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_15), 0, -1)
        alt.AddToken(CInt(ExpressionConstants.ARRAY_BRACES), 0, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.INDEX_EXPRESSION), "IndexExpression")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.LEFT_BRACE), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.ARGUMENT_LIST), 1, 1)
        alt.AddToken(CInt(ExpressionConstants.RIGHT_BRACE), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.FUNCTION_CALL_EXPRESSION), "FunctionCallExpression")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.IDENTIFIER), 1, 1)
        alt.AddToken(CInt(ExpressionConstants.LEFT_PAREN), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.ARGUMENT_LIST), 0, 1)
        alt.AddToken(CInt(ExpressionConstants.RIGHT_PAREN), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.ARGUMENT_LIST), "ArgumentList")
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.EXPRESSION), 1, 1)
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_16), 0, -1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.LITERAL_EXPRESSION), "LiteralExpression")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.INTEGER), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.REAL), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.STRING_LITERAL), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.BOOLEAN_LITERAL_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.HEX_LITERAL), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.CHAR_LITERAL), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.NULL_LITERAL), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.DATETIME), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.TIMESPAN), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.BOOLEAN_LITERAL_EXPRESSION), "BooleanLiteralExpression")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.TRUE), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.FALSE), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(ExpressionConstants.EXPRESSION_GROUP), "ExpressionGroup")
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.LEFT_PAREN), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.EXPRESSION), 1, 1)
        alt.AddToken(CInt(ExpressionConstants.RIGHT_PAREN), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_1), "Subproduction1")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.XOR), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.OR_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_2), "Subproduction2")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.OR), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.AND_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_3), "Subproduction3")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.AND), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.NOT_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_4), "Subproduction4")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.IN), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.IN_TARGET_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_5), "Subproduction5")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.EQ), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.GT), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.LT), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.GTE), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.LTE), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.NE), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_6), "Subproduction6")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_5), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.SHIFT_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_7), "Subproduction7")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.LEFT_SHIFT), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.RIGHT_SHIFT), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_8), "Subproduction8")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_7), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.ADDITIVE_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_9), "Subproduction9")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.ADD), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.SUB), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_10), "Subproduction10")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_9), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.MULTIPLICATIVE_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_11), "Subproduction11")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.MUL), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.DIV), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.MOD), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_12), "Subproduction12")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(SynteticPatterns.SUBPRODUCTION_11), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.POWER_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_13), "Subproduction13")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.POWER), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.NEGATE_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_14), "Subproduction14")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.MEMBER_ACCESS_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        alt = New ProductionPatternAlternative()
        alt.AddProduction(CInt(ExpressionConstants.INDEX_EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_15), "Subproduction15")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.DOT), 1, 1)
        alt.AddToken(CInt(ExpressionConstants.IDENTIFIER), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)

        pattern = New ProductionPattern(CInt(SynteticPatterns.SUBPRODUCTION_16), "Subproduction16")
        pattern.Synthetic = True
        alt = New ProductionPatternAlternative()
        alt.AddToken(CInt(ExpressionConstants.ARGUMENT_SEPARATOR), 1, 1)
        alt.AddProduction(CInt(ExpressionConstants.EXPRESSION), 1, 1)
        pattern.AddAlternative(alt)
        AddPattern(pattern)
    End Sub
End Class
