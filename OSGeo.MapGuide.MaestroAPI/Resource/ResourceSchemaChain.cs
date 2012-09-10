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

namespace OSGeo.MapGuide.MaestroAPI.Resource
{
    /// <summary>
    /// Helper class to determine the full dependency chain for a given xml schema
    /// </summary>
    public static class ResourceSchemaChain
    {
        /// <summary>
        /// Gets the validating schemas.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <returns></returns>
        public static string[] GetValidatingSchemas(string schema)
        {
            switch (schema)
            {
                case "LayerDefinition-1.0.0.xsd": //NOXLATE
                    return new string[] { schema, "PlatformCommon-1.0.0.xsd" }; //NOXLATE
                case "LayerDefinition-1.1.0.xsd": //NOXLATE
                    return new string[] { schema, "SymbolDefinition-1.0.0.xsd", "PlatformCommon-1.0.0.xsd" }; //NOXLATE
                case "LayerDefinition-1.2.0.xsd": //NOXLATE
                case "LayerDefinition-1.3.0.xsd": //NOXLATE
                    return new string[] { schema, "SymbolDefinition-1.1.0.xsd", "PlatformCommon-1.0.0.xsd" }; //NOXLATE
            }
            return new string[] { schema, "PlatformCommon-1.0.0.xsd" }; //NOXLATE
        }
    }
}
