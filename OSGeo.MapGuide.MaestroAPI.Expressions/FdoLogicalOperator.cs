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

namespace OSGeo.MapGuide.MaestroAPI.Expressions
{
    public abstract class FdoLogicalOperator : FdoFilter
    {
        internal static FdoLogicalOperator ParseLogicalOperatorNode(ParseTreeNode node)
        {
            if (node.Term.Name == FdoTerminalNames.LogicalOperator)
            {
                return ParseLogicalOperatorNode(node.ChildNodes[0]);
            }
            else
            {
                switch (node.Term.Name)
                {
                    case FdoTerminalNames.BinaryLogicalOperator:
                        return new FdoBinaryLogicalOperator(node);
                    case FdoTerminalNames.UnaryLogicalOperator:
                        return new FdoUnaryLogicalOperator(node);
                    default:
                        throw new FdoParseException("Unknown terminal: " + node.Term.Name);
                }
            }
        }
    }
}
