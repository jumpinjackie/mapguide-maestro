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
using System.Threading.Tasks;

namespace OSGeo.FDO.Expressions
{
    public enum DistanceOperations
    {
        Beyond,
        WithinDistance
    }

    public class FdoDistanceCondition : FdoGeometricCondition
    {
        public override FilterType FilterType => FilterType.DistanceCondition;

        public FdoIdentifier Identifier { get; }

        public DistanceOperations Operator { get; }

        public FdoExpression Expression { get; }

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
                    throw new FdoParseException($"Unknown operator: {opName}"); //LOCALIZEME
            }
            this.Expression = FdoExpression.ParseNode(node.ChildNodes[2]);
            this.Distance = FdoDataValue.ParseDataNode(node.ChildNodes[3].ChildNodes[0]);
        }
    }
}
