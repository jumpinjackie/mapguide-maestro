#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//

#endregion Disclaimer / License
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.FDO.Expressions
{
    [Language("FDO Expression", "1.0", "FDO Expression Grammar")]
    public class FdoExpressionGrammar : Grammar
    {
        public FdoExpressionGrammar()
            : base(false)
        {
            //1. Terminals
            IdentifierTerminal Identifier = new IdentifierTerminal(FdoTerminalNames.Identifier);
            StringLiteral QuotedIdentifier = new StringLiteral(FdoTerminalNames.Identifier, "\"");
            StringLiteral String = new StringLiteral(FdoTerminalNames.String, "\'");
            NumberLiteral Integer = new NumberLiteral(FdoTerminalNames.Integer, NumberOptions.IntOnly | NumberOptions.AllowSign);
            NumberLiteral Double = new NumberLiteral(FdoTerminalNames.Double);

            //2. Non-Terminals
            NonTerminal Expression = new NonTerminal(FdoTerminalNames.Expression);
            NonTerminal UnaryExpression = new NonTerminal(FdoTerminalNames.UnaryExpression);
            NonTerminal BinaryExpression = new NonTerminal(FdoTerminalNames.BinaryExpression);
            NonTerminal ValueExpression = new NonTerminal(FdoTerminalNames.ValueExpression);
            NonTerminal Function = new NonTerminal(FdoTerminalNames.Function);

            NonTerminal DataValue = new NonTerminal(FdoTerminalNames.DataValue);
            NonTerminal LiteralValue = new NonTerminal(FdoTerminalNames.LiteralValue);
            NonTerminal Parameter = new NonTerminal(FdoTerminalNames.Parameter);
            NonTerminal ExpressionCollection = new NonTerminal(FdoTerminalNames.ExpressionCollection);
            NonTerminal GeometryValue = new NonTerminal(FdoTerminalNames.GeometryValue);

            NonTerminal Boolean = new NonTerminal(FdoTerminalNames.Boolean);
            NonTerminal DateTime = new NonTerminal(FdoTerminalNames.DateTime);

            //3. BNF Rules
            Expression.Rule = 
                "(" + Expression + ")" | 
                UnaryExpression | 
                BinaryExpression | 
                Function | 
                Identifier | 
                QuotedIdentifier |
                ValueExpression;
            BinaryExpression.Rule =
                Expression + "+" + Expression |
                Expression + "-" + Expression |
                Expression + "*" + Expression |
                Expression + "/" + Expression;
            DataValue.Rule = 
                DateTime | 
                Double | 
                Integer | 
                String | 
                //Blob | 
                //Clob | 
                Boolean |
                "NULL";
            Boolean.Rule = 
                ToTerm("TRUE") | 
                ToTerm("FALSE");
            DateTime.Rule = 
                "DATE" + String |
                "TIME" + String |
                "TIMESTAMP" + String;
            Function.Rule =
                Identifier + "(" + ExpressionCollection + ")" |
                Identifier + "()";
            ExpressionCollection.Rule = Expression | ExpressionCollection + "," + Expression;
            GeometryValue.Rule = ToTerm("GEOMFROMTEXT") + "(" + String + ")";
            ValueExpression.Rule = LiteralValue | Parameter;
            LiteralValue.Rule = GeometryValue | DataValue;
            Parameter.Rule = Parameter | ":" + Identifier;
            UnaryExpression.Rule = "-" + Expression;

            this.Root = Expression;

            // 4. Set operators precedence
            RegisterOperators(1, "+", "-");
            RegisterOperators(2, "*", "/");
            this.MarkPunctuation("(", ")", ",", ":", "-");
            /*
             <Expression> ::= '(' Expression ')'
                | <UnaryExpression>
                | <BinaryExpression>
                | <Function>
                | <Identifier>
                | <ValueExpression>
             <BinaryExpression> ::=
                <Expression> '+' <Expression>
                | <Expression> '-' <Expression>
                | <Expression> '*' <Expression>
                | <Expression> '/' <Expression>
             <DataValue> ::=
                TRUE
                | FALSE
                | DATETIME
                | DOUBLE
                | INTEGER
                | STRING
                | BLOB
                | CLOB
                | NULL
             <Function> ::= <Identifier> '(' <ExpressionCollection> ')'
             <ExpressionCollection> ::=
                | <Expression>
                | <ExpressionCollection> ',' <Expression>
             <GeometryValue> ::= GEOMFROMTEXT '(' STRING ')'
             <Identifier> ::= IDENTIFIER
             <ValueExpression> ::= <LiteralValue> | <Parameter>;
             <LiteralValue> ::= <GeometryValue> | <DataValue>
             <Parameter> ::= PARAMETER | ':'STRING
             <UnaryExpression> ::= '-' <Expression>
             
             */
        }
    }
}
