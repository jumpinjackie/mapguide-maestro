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

namespace OSGeo.FDO.Expressions
{
    /// <summary>
    /// The binary logical operator
    /// </summary>
    public enum BinaryLogicalOperations
    {
        /// <summary>
        /// Logical AND
        /// </summary>
        And,
        /// <summary>
        /// Logical OR
        /// </summary>
        Or
    }

    /// <summary>
    /// A binary logical operator
    /// </summary>
    public class FdoBinaryLogicalOperator : FdoLogicalOperator
    {
        /// <summary>
        /// The filter type
        /// </summary>
        public override FilterType FilterType => FilterType.BinaryLogicalOperator;

        /// <summary>
        /// The left-hand side expression
        /// </summary>
        public FdoFilter Left { get; }

        /// <summary>
        /// The logical operator
        /// </summary>
        public BinaryLogicalOperations Operator { get; }

        /// <summary>
        /// The right-hand side expression
        /// </summary>
        public FdoFilter Right { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node"></param>
        internal FdoBinaryLogicalOperator(Irony.Parsing.ParseTreeNode node)
        {
            this.Left = FdoFilter.ParseNode(node.ChildNodes[0]);
            var opName = node.ChildNodes[1].ChildNodes[0].Token.ValueString;
            switch (opName.ToUpper())
            {
                case "AND":
                    this.Operator = BinaryLogicalOperations.And;
                    break;
                case "OR":
                    this.Operator = BinaryLogicalOperations.Or;
                    break;
                default:
                    throw new FdoParseException("Unknown operator: " + opName);
            }
            this.Right = FdoFilter.ParseNode(node.ChildNodes[2]);
        }
    }
}
