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

namespace OSGeo.FDO.Expressions
{
    /// <summary>
    /// The distance operator
    /// </summary>
    public enum DistanceOperations
    {
        /// <summary>
        /// Beyond
        /// </summary>
        Beyond,
        /// <summary>
        /// Within distance
        /// </summary>
        WithinDistance
    }

    /// <summary>
    /// An FDO distance condition
    /// </summary>
    public class FdoDistanceCondition : FdoGeometricCondition
    {
        /// <summary>
        /// The filter type
        /// </summary>
        public override FilterType FilterType => FilterType.DistanceCondition;

        /// <summary>
        /// The geometry identifier
        /// </summary>
        public FdoIdentifier Identifier { get; }

        /// <summary>
        /// The distance operator
        /// </summary>
        public DistanceOperations Operator { get; }

        /// <summary>
        /// The target expression
        /// </summary>
        public FdoExpression Expression { get; }

        /// <summary>
        /// The distance value
        /// </summary>
        public FdoDataValue Distance { get; }

        internal FdoDistanceCondition(ParseTreeNode node)
        {
            this.Identifier = new FdoIdentifier(node.ChildNodes[0]);
            var opName = node.ChildNodes[1].ChildNodes[0].Token.ValueString;
            switch(opName.ToUpper())
            {
                case "WITHINDISTANCE":
                    this.Operator = DistanceOperations.WithinDistance;
                    break;
                case "BEYOND":
                    this.Operator = DistanceOperations.Beyond;
                    break;
                default:
                    throw new FdoParseException(string.Format(Strings.UnknownOperator, opName));
            }
            this.Expression = FdoExpression.ParseNode(node.ChildNodes[2]);
            this.Distance = FdoDataValue.ParseDataNode(node.ChildNodes[3].ChildNodes[0]);
        }
    }
}
