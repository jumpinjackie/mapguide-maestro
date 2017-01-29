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
    /// <summary>
    /// Spatial operators
    /// </summary>
    public enum SpatialOperations
    {
        /// <summary>
        /// Contains
        /// </summary>
        Contains,
        /// <summary>
        /// Crosses
        /// </summary>
        Crosses,
        /// <summary>
        /// Disjoint
        /// </summary>
        Disjoint,
        /// <summary>
        /// Equals
        /// </summary>
        Equals,
        /// <summary>
        /// Intersects
        /// </summary>
        Intersects,
        /// <summary>
        /// Overlaps
        /// </summary>
        Overlaps,
        /// <summary>
        /// Touches
        /// </summary>
        Touches,
        /// <summary>
        /// Within
        /// </summary>
        Within,
        /// <summary>
        /// Covered by
        /// </summary>
        CoveredBy,
        /// <summary>
        /// Inside
        /// </summary>
        Inside
    }

    /// <summary>
    /// An FDO spatial condition
    /// </summary>
    public class FdoSpatialCondition : FdoGeometricCondition
    {
        /// <summary>
        /// The filter type
        /// </summary>
        public override FilterType FilterType => FilterType.SpatialCondition;

        /// <summary>
        /// The geometry identifier
        /// </summary>
        public FdoIdentifier Identifier { get; }

        /// <summary>
        /// The spatial operator
        /// </summary>
        public SpatialOperations Operator { get; }

        /// <summary>
        /// The geometric expression
        /// </summary>
        public FdoExpression Expression { get; }

        internal FdoSpatialCondition(ParseTreeNode node)
        {
            this.Identifier = new FdoIdentifier(node.ChildNodes[0]);
            var opName = node.ChildNodes[1].ChildNodes[0].Token.ValueString;
            switch (opName.ToUpper())
            {
                case "CONTAINS":
                    this.Operator = SpatialOperations.Contains;
                    break;
                case "CROSSES":
                    this.Operator = SpatialOperations.Crosses;
                    break;
                case "DISJOINT":
                    this.Operator = SpatialOperations.Disjoint;
                    break;
                case "EQUALS":
                    this.Operator = SpatialOperations.Equals;
                    break;
                case "INTERSECTS":
                    this.Operator = SpatialOperations.Intersects;
                    break;
                case "OVERLAPS":
                    this.Operator = SpatialOperations.Overlaps;
                    break;
                case "TOUCHES":
                    this.Operator = SpatialOperations.Touches;
                    break;
                case "WITHIN":
                    this.Operator = SpatialOperations.Within;
                    break;
                case "COVEREDBY":
                    this.Operator = SpatialOperations.CoveredBy;
                    break;
                case "INSIDE":
                    this.Operator = SpatialOperations.Inside;
                    break;
                default:
                    throw new FdoParseException(string.Format(Strings.UnknownOperator, opName));
            }
            this.Expression = FdoExpression.ParseNode(node.ChildNodes[2]);
        }
    }
}
