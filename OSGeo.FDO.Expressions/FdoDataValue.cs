#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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
    /// <summary>
    /// Data value types
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// Boolean
        /// </summary>
        Boolean,
        /// <summary>
        /// DateTime
        /// </summary>
        DateTime,
        /// <summary>
        /// Double
        /// </summary>
        Double,
        /// <summary>
        /// Int32
        /// </summary>
        Int32,
        /// <summary>
        /// String
        /// </summary>
        String
    }

    /// <summary>
    /// A data value
    /// </summary>
    public abstract class FdoDataValue : FdoLiteralValue
    {
        /// <summary>
        /// The data type
        /// </summary>
        public DataType DataType { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
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
    
    /// <summary>
    /// An FDO string value
    /// </summary>
    public class FdoStringValue : FdoDataValue
    {
        /// <summary>
        /// The expression type
        /// </summary>
        public override ExpressionType ExpressionType => ExpressionType.StringValue;

        /// <summary>
        /// The string value
        /// </summary>
        public string Value { get; }

        internal FdoStringValue(ParseTreeNode node)
        {
            this.DataType = DataType.String;
            this.Value = node.Token.ValueString;
        }
    }

    /// <summary>
    /// An FDO int32 value
    /// </summary>
    public class FdoInt32Value : FdoDataValue
    {
        /// <summary>
        /// The expression type
        /// </summary>
        public override ExpressionType ExpressionType => ExpressionType.Int32Value;

        /// <summary>
        /// The int32 value
        /// </summary>
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

    /// <summary>
    /// An FDO double value
    /// </summary>
    public class FdoDoubleValue : FdoDataValue
    {
        /// <summary>
        /// The expression type
        /// </summary>
        public override ExpressionType ExpressionType => ExpressionType.DoubleValue;

        /// <summary>
        /// The double value
        /// </summary>
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

    /// <summary>
    /// An FDO datetime value
    /// </summary>
    public class FdoDateTimeValue : FdoDataValue
    {
        /// <summary>
        /// The expression type
        /// </summary>
        public override ExpressionType ExpressionType => ExpressionType.DateTimeValue;

        /// <summary>
        /// The datetime value
        /// </summary>
        public DateTime? DateTime { get; }

        /// <summary>
        /// The time span value
        /// </summary>
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
                            throw new FdoParseException(string.Format(Strings.InvalidFdoDateString, value));
                        this.DateTime = new DateTime(Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]), Convert.ToInt32(tokens[2]));
                    }
                    break;
                case "TIME":
                    {
                        string [] tokens = value.Split(':', '.');
                        if (tokens.Length != 3 && tokens.Length != 4)
                            throw new FdoParseException(string.Format(Strings.InvalidFdoTimeString, value));
                        var ts = new TimeSpan(0, Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]), Convert.ToInt32(tokens[2]), (tokens.Length == 4) ? Convert.ToInt32(tokens[3]) : 0);
                        this.Time = ts;
                    }
                    break;
                case "TIMESTAMP":
                    {
                        string[] tokens = value.Split('-', '.', ':', ' ');
                        if (tokens.Length != 6 && tokens.Length != 7)
                            throw new FdoParseException(string.Format(Strings.InvalidFdoTimestampString, value));
                        this.DateTime = new DateTime(Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]), Convert.ToInt32(tokens[2]), Convert.ToInt32(tokens[3]), Convert.ToInt32(tokens[4]), Convert.ToInt32(tokens[5]), (tokens.Length == 7) ? Convert.ToInt32(tokens[6]) : 0);
                    }
                    break;
                default:
                    throw new FdoParseException(string.Format(Strings.UnknownKeyword, keyword));
            }
        } 
    }

    /// <summary>
    /// An FDO boolean value
    /// </summary>
    public class FdoBooleanValue : FdoDataValue
    {
        /// <summary>
        /// The expression type
        /// </summary>
        public override ExpressionType ExpressionType => ExpressionType.BooleanValue;

        /// <summary>
        /// The boolean value
        /// </summary>
        public bool Value { get; }

        internal FdoBooleanValue(ParseTreeNode node)
        {
            this.Value = Convert.ToBoolean(node.ChildNodes[0].Token.ValueString);
        }
    }
}
