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
    public class FdoFunction : FdoExpression
    {
        public override ExpressionType ExpressionType
        {
            get { return Expressions.ExpressionType.Function; }
        }

        public FdoIdentifier Identifier { get; private set; }

        public List<FdoExpression> Arguments { get; private set; }

        internal FdoFunction(ParseTreeNode node)
        {
            this.Identifier = new FdoIdentifier(node.ChildNodes[0]);
            this.Arguments = new List<FdoExpression>();
            ProcessArguments(node.ChildNodes[1]);
        }

        private void ProcessNodeList(ParseTreeNodeList list)
        {
            foreach (ParseTreeNode child in list)
            {
                if (child.Term.Name == FdoTerminalNames.ExpressionCollection)
                {
                    ProcessNodeList(child.ChildNodes);
                }
                else
                {
                    this.Arguments.Add(FdoExpression.ParseNode(child));
                }
            }
        }

        private void ProcessArguments(ParseTreeNode node)
        {
            ProcessNodeList(node.ChildNodes);
        }
    }
}
