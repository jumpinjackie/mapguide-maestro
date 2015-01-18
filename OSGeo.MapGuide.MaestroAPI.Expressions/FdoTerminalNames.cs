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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Expressions
{
    internal class FdoTerminalNames
    {
        public const string Identifier = "Identifier";
        public const string String = "String";
        public const string Integer = "Integer";
        public const string Double = "Double";

        public const string Expression = "Expression";
        public const string UnaryExpression = "UnaryExpression";
        public const string BinaryExpression = "BinaryExpression";
        public const string ValueExpression = "ValueExpression";
        public const string Function = "Function";

        public const string DataValue = "DataValue";
        public const string LiteralValue = "LiteralValue";
        public const string Parameter = "Parameter";
        public const string ExpressionCollection = "ExpressionCollection";
        public const string GeometryValue = "GeometryValue";

        public const string Boolean = "Boolean";
        public const string DateTime = "DateTime";

        public const string Filter = "Filter";
        public const string LogicalOperator = "LogicalOperator";
        public const string BinaryLogicalOperator = "BinaryLogicalOperator";
        public const string UnaryLogicalOperator = "UnaryLogicalOperator";
        public const string BinaryLogicalOperations = "BinaryLogicalOperations";
        public const string InCondition = "InCondition";
        public const string SearchCondition = "SearchCondition";
        public const string ComparisonCondition = "ComparisonCondition";
        public const string GeometricCondition = "GeometricCondition";
        public const string NullCondition = "NullCondition";
        public const string DistanceCondition = "DistanceCondition";
        public const string SpatialCondition = "SpatialCondition";

        public const string ValueExpressionCollection = "ValueExpressionCollection";
        public const string ComparisonOperations = "ComparisonOperations";
        public const string DistanceOperations = "DistanceOperations";
        public const string Distance = "Distance";
        public const string SpatialOperations = "SpatialOperations";
    }
}
