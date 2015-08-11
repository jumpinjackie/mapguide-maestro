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

namespace OSGeo.FDO.Expressions
{
    public enum DataType
    {
        Boolean,
        DateTime,
        Double,
        Int32,
        String
    }

    public abstract class FdoDataValue : FdoLiteralValue
    {
        public DataType DataType { get; protected set; }

        protected FdoDataValue()
        {
            this.LiteralValueType = LiteralValueType.Data;
        }

        internal static FdoDataValue ParseDataNode(ParseTreeNode node)
        {
            if (node.Term.Name == FdoTerminalNames.DataValue)
            {
                return ParseDataNode(node.ChildNodes[0]);
            }
            else
            {
                switch (node.Term.Name)
                {
                    case FdoTerminalNames.Boolean:
                        return new FdoBooleanValue(node);
                    case FdoTerminalNames.String:
                        return new FdoStringValue(node);
                    case FdoTerminalNames.Integer:
                        return new FdoInt32Value(node);
                    case FdoTerminalNames.Double:
                        return new FdoDoubleValue(node);
                    case FdoTerminalNames.DateTime:
                        return new FdoDateTimeValue(node);
                    default:
                        throw new FdoParseException($"Unknown terminal: {node.Term.Name}");
                }
            }
        }
    }

    public class FdoStringValue : FdoDataValue
    {
        public override ExpressionType ExpressionType => ExpressionType.StringValue;

        public string Value { get; }

        internal FdoStringValue(ParseTreeNode node)
        {
            this.DataType = DataType.String;
            this.Value = node.Token.ValueString;
        }
    }

    public class FdoInt32Value : FdoDataValue
    {
        public override ExpressionType ExpressionType => ExpressionType.Int32Value;

        public int Value { get; }

        internal FdoInt32Value(ParseTreeNode node)
        {
            this.DataType = DataType.Int32;
            this.Value = Convert.ToInt32(node.Token.ValueString);
        }

        private FdoInt32Value(int value)
        {
            this.Value = value;
        }

        internal FdoInt32Value Negate()
        {
            return new FdoInt32Value(-this.Value);
        }
    }

    public class FdoDoubleValue : FdoDataValue
    {
        public override ExpressionType ExpressionType => ExpressionType.DoubleValue;

        public double Value { get; }

        internal FdoDoubleValue(ParseTreeNode node)
        {
            this.DataType = DataType.Double;
            this.Value = Convert.ToDouble(node.Token.ValueString);
        }

        private FdoDoubleValue(double value)
        {
            this.Value = value;
        }

        internal FdoDoubleValue Negate()
        {
            return new FdoDoubleValue(-this.Value);
        }
    }

    public class FdoDateTimeValue : FdoDataValue
    {
        public override ExpressionType ExpressionType => ExpressionType.DateTimeValue;

        public DateTime? DateTime { get; }

        public TimeSpan? Time { get; }

        internal FdoDateTimeValue(ParseTreeNode node)
        {
            this.DataType = DataType.DateTime;
            string keyword = node.ChildNodes[0].Token.ValueString;
            string value = node.ChildNodes[1].Token.ValueString;
            switch (keyword)
            {
                case "DATE":
                    {
                        string[] tokens = value.Split('-');
                        if (tokens.Length != 3)
                            throw new FdoParseException($"Value is not a valid FDO date string: {value}"); //LOCALIZEME
                        this.DateTime = new DateTime(Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]), Convert.ToInt32(tokens[2]));
                    }
                    break;
                case "TIME":
                    {
                        string [] tokens = value.Split(':', '.');
                        if (tokens.Length != 3 && tokens.Length != 4)
                            throw new FdoParseException($"Value is not a valid FDO time string: {value}"); //LOCALIZEME
                        var ts = new TimeSpan(0, Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]), Convert.ToInt32(tokens[2]), (tokens.Length == 4) ? Convert.ToInt32(tokens[3]) : 0);
                        this.Time = ts;
                    }
                    break;
                case "TIMESTAMP":
                    {
                        string[] tokens = value.Split('-', '.', ':', ' ');
                        if (tokens.Length != 6 && tokens.Length != 7)
                            throw new FdoParseException($"Value is not a valid FDO timestamp string: {value}"); //LOCALIZEME
                        this.DateTime = new DateTime(Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]), Convert.ToInt32(tokens[2]), Convert.ToInt32(tokens[3]), Convert.ToInt32(tokens[4]), Convert.ToInt32(tokens[5]), (tokens.Length == 7) ? Convert.ToInt32(tokens[6]) : 0);
                    }
                    break;
                default:
                    throw new FdoParseException($"Unknown keyword: {keyword}"); //LOCALIZEME
            }
        } 
    }

    public class FdoBooleanValue : FdoDataValue
    {
        public override ExpressionType ExpressionType => ExpressionType.BooleanValue;

        public bool Value { get; }

        internal FdoBooleanValue(ParseTreeNode node)
        {
            this.Value = Convert.ToBoolean(node.ChildNodes[0].Token.ValueString);
        }
    }
}
