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
using System.Collections.Generic;

namespace OSGeo.FDO.Expressions
{
    /// <summary>
    /// An FDO function expression
    /// </summary>
    public class FdoFunction : FdoExpression
    {
        /// <summary>
        /// The expression type
        /// </summary>
        public override ExpressionType ExpressionType => ExpressionType.Function;

        /// <summary>
        /// The name of the function
        /// </summary>
        public FdoIdentifier Identifier { get; }

        /// <summary>
        /// The list of arguments passed in
        /// </summary>
        public List<FdoExpression> Arguments { get; }

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
