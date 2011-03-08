#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
#endregion
using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable 1591, 0114, 0108

namespace OSGeo.MapGuide.MaestroAPI.Expression
{
    /// <summary>
    /// Expression class that implements the standard MapGuide FDO expression functions
    /// 
    /// Despite the modifier. This class is for Expression Engine use only.
    /// </summary>
    public static class MgFunctionNamespace
    {
        public static object If(bool condition, object trueValue, object falseValue)
        {
            return condition ? trueValue : falseValue;
        }

        // Lookup()
        // Range()
    }
}
