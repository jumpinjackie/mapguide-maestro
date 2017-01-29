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
    /// An FDO IN condition
    /// </summary>
    public class FdoInCondition : FdoSearchCondition
    {
        /// <summary>
        /// The filter type
        /// </summary>
        public override FilterType FilterType => FilterType.InCondition;

        /// <summary>
        /// The identifier
        /// </summary>
        public FdoIdentifier Identifier { get; }

        /// <summary>
        /// The list of values to compare against
        /// </summary>
        public List<FdoValueExpression> ValueList { get; }

        internal FdoInCondition(ParseTreeNode node)
        {
            this.Identifier = new FdoIdentifier(node.ChildNodes[0]);
            this.ValueList = new List<FdoValueExpression>();
            ProcessValueList(node.ChildNodes[2]);
        }

        private void ProcessNodeList(ParseTreeNodeList list)
        {
            foreach (ParseTreeNode child in list)
            {
                if (child.Term.Name == FdoTerminalNames.ValueExpressionCollection)
                {
                    ProcessNodeList(child.ChildNodes);
                }
                else
                {
                    this.ValueList.Add(FdoValueExpression.ParseValueNode(child));
                }
            }
        }

        private void ProcessValueList(ParseTreeNode node)
        {
            ProcessNodeList(node.ChildNodes);
        }
    }
}
