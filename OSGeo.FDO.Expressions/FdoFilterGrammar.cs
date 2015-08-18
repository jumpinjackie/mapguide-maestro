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

namespace OSGeo.FDO.Expressions
{
    [Language("FDO Filter", "1.0", "FDO Filter Grammar")]
    public class FdoFilterGrammar : Grammar
    {
        public FdoFilterGrammar()
            : base(false)
        {
            NonTerminal Filter = new NonTerminal(FdoTerminalNames.Filter);
            NonTerminal LogicalOperator = new NonTerminal(FdoTerminalNames.LogicalOperator);
            NonTerminal BinaryLogicalOperator = new NonTerminal(FdoTerminalNames.BinaryLogicalOperator);
            NonTerminal UnaryLogicalOperator = new NonTerminal(FdoTerminalNames.UnaryLogicalOperator);
            NonTerminal BinaryLogicalOperations = new NonTerminal(FdoTerminalNames.BinaryLogicalOperations);

            NonTerminal InCondition = new NonTerminal(FdoTerminalNames.InCondition);
            NonTerminal SearchCondition = new NonTerminal(FdoTerminalNames.SearchCondition);
            NonTerminal ComparisonCondition = new NonTerminal(FdoTerminalNames.ComparisonCondition);
            NonTerminal GeometricCondition = new NonTerminal(FdoTerminalNames.GeometricCondition);
            NonTerminal NullCondition = new NonTerminal(FdoTerminalNames.NullCondition);
            NonTerminal DistanceCondition = new NonTerminal(FdoTerminalNames.DistanceCondition);
            NonTerminal SpatialCondition = new NonTerminal(FdoTerminalNames.SpatialCondition);

            NonTerminal ValueExpressionCollection = new NonTerminal(FdoTerminalNames.ValueExpressionCollection);
            NonTerminal ComparisonOperations = new NonTerminal(FdoTerminalNames.ComparisonOperations);
            NonTerminal DistanceOperations = new NonTerminal(FdoTerminalNames.DistanceOperations);
            NonTerminal Distance = new NonTerminal(FdoTerminalNames.Distance);
            NonTerminal SpatialOperations = new NonTerminal(FdoTerminalNames.SpatialOperations);

            //FDO Expression terminals and literals
            IdentifierTerminal Identifier = new IdentifierTerminal(FdoTerminalNames.Identifier);
            StringLiteral QuotedIdentifier = new StringLiteral(FdoTerminalNames.Identifier, "\"");
            StringLiteral String = new StringLiteral(FdoTerminalNames.String, "\'");
            NumberLiteral Integer = new NumberLiteral(FdoTerminalNames.Integer, NumberOptions.IntOnly);
            NumberLiteral Double = new NumberLiteral(FdoTerminalNames.Double);

            //FDO Expression non-terminals
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

            //Filter BNF
            Filter.Rule =
                "(" + Filter + ")" |
                LogicalOperator |
                SearchCondition;
            LogicalOperator.Rule =
                BinaryLogicalOperator |
                UnaryLogicalOperator;
            BinaryLogicalOperator.Rule = Filter + BinaryLogicalOperations + Filter;
            SearchCondition.Rule =
                InCondition |
                ComparisonCondition |
                GeometricCondition |
                NullCondition;
            InCondition.Rule = Identifier + ToTerm("IN") + "(" + ValueExpressionCollection + ")";
            ValueExpressionCollection.Rule =
                ValueExpression |
                ValueExpressionCollection + "," + ValueExpression;
            ComparisonCondition.Rule = Expression + ComparisonOperations + Expression;
            GeometricCondition.Rule =
                DistanceCondition |
                SpatialCondition;
            DistanceCondition.Rule = Identifier + DistanceOperations + Expression + Distance;
            NullCondition.Rule = Identifier + ToTerm("NULL");
            SpatialCondition.Rule = Identifier + SpatialOperations + Expression;
            UnaryLogicalOperator.Rule = ToTerm("NOT") + Filter;
            BinaryLogicalOperations.Rule =
                ToTerm("AND") |
                ToTerm("OR");
            ComparisonOperations.Rule =
                ToTerm("=") |
                ToTerm("<>") |
                ToTerm(">") |
                ToTerm(">=") |
                ToTerm("<") |
                ToTerm("<=") |
                ToTerm("LIKE");
            DistanceOperations.Rule =
                ToTerm("BEYOND") |
                ToTerm("WITHINDISTANCE");
            Distance.Rule =
                Double |
                Integer;
            SpatialOperations.Rule =
                ToTerm("CONTAINS") |
                ToTerm("CROSSES") |
                ToTerm("DISJOINT") |
                ToTerm("EQUALS") |
                ToTerm("INTERSECTS") |
                ToTerm("OVERLAPS") |
                ToTerm("TOUCHES") |
                ToTerm("WITHIN") |
                ToTerm("COVEREDBY") |
                ToTerm("INSIDE");

            //FDO Expression BNF
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
            Function.Rule = Identifier + "(" + ExpressionCollection + ")";
            ExpressionCollection.Rule = Expression | ExpressionCollection + "," + Expression;
            GeometryValue.Rule = ToTerm("GEOMFROMTEXT") + "(" + String + ")";
            ValueExpression.Rule = LiteralValue | Parameter;
            LiteralValue.Rule = GeometryValue | DataValue;
            Parameter.Rule = Parameter | ":" + Identifier;
            UnaryExpression.Rule = "-" + Expression;
            /*
                <Filter> ::= '(' Filter ')'
                | <LogicalOperator>
                | <SearchCondition>
                <LogicalOperator> ::= <BinaryLogicalOperator>
                | <UnaryLogicalOperator>
                <BinaryLogicalOperator> ::= <Filter> <BinaryLogicalOperations> <Filter>
                <SearchCondition> ::= <InCondition>
                | <ComparisonCondition>
                | <GeometricCondition>
                | <NullCondition>
                <InCondition> ::= <Identifier> IN '(' ValueExpressionCollection
                ')'
                <ValueExpressionCollection> ::= <ValueExpression>
                | <ValueExpressionCollection> ',' <ValueExpression>
                <ComparisonCondition> ::= <Expression> <ComparisonOperations> <Expression>
                <GeometricCondition> ::= <DistanceCondition>
                | <SpatialCondition>
                <DistanceCondition> ::= <Identifier> <DistanceOperations> <Expression> <distance>
                <NullCondition> ::= <Identifier> NULL
                <SpatialCondition> ::= <Identifier> <SpatialOperations> <Expression>
                <UnaryLogicalOperator> ::= NOT <Filter>
                <BinaryLogicalOperations> ::= AND | OR
                <ComparisionOperations> ::=
                = // EqualTo (EQ)
                <> // NotEqualTo (NE)
                > // GreaterThan (GT)
                >= // GreaterThanOrEqualTo (GE)
                < // LessThan (LT)
                <= // LessThanOrEqualTo (LE)
                LIKE // Like
                <DistanceOperations> ::= BEYOND | WITHINDISTANCE
                <distance> ::= DOUBLE | INTEGER
                <SpatialOperations> ::= CONTAINS | CROSSES | DISJOINT
                | EQUALS | INTERSECTS | OVERLAPS | TOUCHES | WITHIN | COVEREDBY |
                INSIDE
             */
            this.Root = Filter;
            
            
            RegisterOperators(1, "-", "NOT");
            RegisterOperators(2, "*", "/");
            RegisterOperators(3, "+", "-");
            RegisterOperators(4, "=", "<>", ">", ">=", "<", "<=");
            RegisterOperators(5, "AND");
            RegisterOperators(6, "OR");
            this.MarkPunctuation("(", ")", ",", ":", "-");
        }
    }
}
