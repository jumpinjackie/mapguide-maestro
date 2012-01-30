' ExpressionTokenizer.vb
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

'''<remarks>A character stream tokenizer.</remarks>
Friend Class ExpressionTokenizer
    Inherits Tokenizer

	Private MyContext As Ciloci.Flee.ExpressionContext

	'''<summary>Creates a new tokenizer for the specified input
	'''stream.</summary>
	'''
	'''<param name='input'>the input stream to read</param>
	'''
	'''<exception cref='ParserCreationException'>if the tokenizer
	'''couldn't be initialized correctly</exception>
	Public Sub New(ByVal input As TextReader, ByVal context As Ciloci.Flee.ExpressionContext)
		MyBase.New(input, True)
		MyContext = context
		CreatePatterns()
	End Sub

    '''<summary>Creates a new tokenizer for the specified input
    '''stream.</summary>
    '''
    '''<param name='input'>the input stream to read</param>
    '''
    '''<exception cref='ParserCreationException'>if the tokenizer
    '''couldn't be initialized correctly</exception>
	Public Sub New(ByVal input As TextReader)
		MyBase.New(input, True)
		CreatePatterns()
	End Sub

    '''<summary>Initializes the tokenizer by creating all the token
    '''patterns.</summary>
    '''
    '''<exception cref='ParserCreationException'>if the tokenizer
    '''couldn't be initialized correctly</exception>
    Private Sub CreatePatterns()
		Dim pattern As TokenPattern
		Dim customPattern As CustomTokenPattern

        pattern = New TokenPattern(CInt(ExpressionConstants.ADD), "ADD", TokenPattern.PatternType.STRING, "+")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.SUB), "SUB", TokenPattern.PatternType.STRING, "-")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.MUL), "MUL", TokenPattern.PatternType.STRING, "*")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.DIV), "DIV", TokenPattern.PatternType.STRING, "/")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.POWER), "POWER", TokenPattern.PatternType.STRING, "^")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.MOD), "MOD", TokenPattern.PatternType.STRING, "%")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.LEFT_PAREN), "LEFT_PAREN", TokenPattern.PatternType.STRING, "(")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.RIGHT_PAREN), "RIGHT_PAREN", TokenPattern.PatternType.STRING, ")")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.LEFT_BRACE), "LEFT_BRACE", TokenPattern.PatternType.STRING, "[")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.RIGHT_BRACE), "RIGHT_BRACE", TokenPattern.PatternType.STRING, "]")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.EQ), "EQ", TokenPattern.PatternType.STRING, "=")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.LT), "LT", TokenPattern.PatternType.STRING, "<")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.GT), "GT", TokenPattern.PatternType.STRING, ">")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.LTE), "LTE", TokenPattern.PatternType.STRING, "<=")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.GTE), "GTE", TokenPattern.PatternType.STRING, ">=")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.NE), "NE", TokenPattern.PatternType.STRING, "<>")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.AND), "AND", TokenPattern.PatternType.STRING, "AND")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.OR), "OR", TokenPattern.PatternType.STRING, "OR")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.XOR), "XOR", TokenPattern.PatternType.STRING, "XOR")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.NOT), "NOT", TokenPattern.PatternType.STRING, "NOT")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.IN), "IN", TokenPattern.PatternType.STRING, "in")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.DOT), "DOT", TokenPattern.PatternType.STRING, ".")
        AddPattern(pattern)

		customPattern = New ArgumentSeparatorPattern()
		customPattern.Initialize(CInt(ExpressionConstants.ARGUMENT_SEPARATOR), "ARGUMENT_SEPARATOR", TokenPattern.PatternType.STRING, ",", MyContext)
		AddPattern(customPattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.ARRAY_BRACES), "ARRAY_BRACES", TokenPattern.PatternType.STRING, "[]")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.LEFT_SHIFT), "LEFT_SHIFT", TokenPattern.PatternType.STRING, "<<")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.RIGHT_SHIFT), "RIGHT_SHIFT", TokenPattern.PatternType.STRING, ">>")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.WHITESPACE), "WHITESPACE", TokenPattern.PatternType.REGEXP, "\s+")
        pattern.Ignore = True
        AddPattern(pattern)

		pattern = New TokenPattern(CInt(ExpressionConstants.INTEGER), "INTEGER", TokenPattern.PatternType.REGEXP, "\d+(u|l|ul|lu|f|m)?")
        AddPattern(pattern)

		customPattern = New RealPattern()
		customPattern.Initialize(CInt(ExpressionConstants.REAL), "REAL", TokenPattern.PatternType.REGEXP, "\d{0}\{1}\d+([e][+-]\d{{1,3}})?(d|f|m)?", MyContext)
		AddPattern(customPattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.STRING_LITERAL), "STRING_LITERAL", TokenPattern.PatternType.REGEXP, """([^""\r\n\\]|\\u[0-9a-f]{4}|\\[\\""'trn])*""")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.CHAR_LITERAL), "CHAR_LITERAL", TokenPattern.PatternType.REGEXP, "'([^'\r\n\\]|\\u[0-9a-f]{4}|\\[\\""'trn])'")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.TRUE), "TRUE", TokenPattern.PatternType.STRING, "True")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.FALSE), "FALSE", TokenPattern.PatternType.STRING, "False")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.IDENTIFIER), "IDENTIFIER", TokenPattern.PatternType.REGEXP, "[a-z_]\w*")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.HEX_LITERAL), "HEX_LITERAL", TokenPattern.PatternType.REGEXP, "0x[0-9a-f]+(u|l|ul|lu)?")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.NULL_LITERAL), "NULL_LITERAL", TokenPattern.PatternType.STRING, "null")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.TIMESPAN), "TIMESPAN", TokenPattern.PatternType.REGEXP, "##(\d+\.)?\d{2}:\d{2}(:\d{2}(\.\d{1,7})?)?#")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.DATETIME), "DATETIME", TokenPattern.PatternType.REGEXP, "#[^#]+#")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.IF), "IF", TokenPattern.PatternType.STRING, "if")
        AddPattern(pattern)

        pattern = New TokenPattern(CInt(ExpressionConstants.CAST), "CAST", TokenPattern.PatternType.STRING, "cast")
        AddPattern(pattern)
    End Sub
End Class
