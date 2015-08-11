#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Resource;
using NUnit.Framework;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Tests
{
    [TestFixture]
    public class ResourceSchemaChainTests
    {
        [Test]
        public void GetValidatingSchemasTest()
        {
            var schemas = ResourceSchemaChain.GetValidatingSchemas("LayerDefinition-1.0.0.xsd");
            Assert.Contains("PlatformCommon-1.0.0.xsd", schemas);
            schemas = ResourceSchemaChain.GetValidatingSchemas("LayerDefinition-1.1.0.xsd");
            Assert.Contains("PlatformCommon-1.0.0.xsd", schemas);
            Assert.Contains("SymbolDefinition-1.0.0.xsd", schemas);
            schemas = ResourceSchemaChain.GetValidatingSchemas("LayerDefinition-1.2.0.xsd");
            Assert.Contains("PlatformCommon-1.0.0.xsd", schemas);
            Assert.Contains("SymbolDefinition-1.1.0.xsd", schemas);
            schemas = ResourceSchemaChain.GetValidatingSchemas("LayerDefinition-1.3.0.xsd");
            Assert.Contains("PlatformCommon-1.0.0.xsd", schemas);
            Assert.Contains("SymbolDefinition-1.1.0.xsd", schemas);
            schemas = ResourceSchemaChain.GetValidatingSchemas("LayerDefinition-2.4.0.xsd");
            Assert.Contains("PlatformCommon-1.0.0.xsd", schemas);
            Assert.Contains("SymbolDefinition-2.4.0.xsd", schemas);
            Assert.Contains("WatermarkDefinition-2.4.0.xsd", schemas);
            schemas = ResourceSchemaChain.GetValidatingSchemas("WatermarkDefinition-2.3.0.xsd");
            Assert.Contains("SymbolDefinition-1.1.0.xsd", schemas);
            schemas = ResourceSchemaChain.GetValidatingSchemas("WatermarkDefinition-2.4.0.xsd");
            Assert.Contains("SymbolDefinition-2.4.0.xsd", schemas);
            schemas = ResourceSchemaChain.GetValidatingSchemas("SymbolDefinition-1.0.0.xsd");
            Assert.Contains("PlatformCommon-1.0.0.xsd", schemas);
            schemas = ResourceSchemaChain.GetValidatingSchemas("SymbolDefinition-1.1.0.xsd");
            Assert.Contains("PlatformCommon-1.0.0.xsd", schemas);
            schemas = ResourceSchemaChain.GetValidatingSchemas("SymbolDefinition-2.4.0.xsd");
            Assert.Contains("PlatformCommon-1.0.0.xsd", schemas);
            schemas = ResourceSchemaChain.GetValidatingSchemas("MapDefinition-2.3.0.xsd");
            Assert.Contains("WatermarkDefinition-2.3.0.xsd", schemas);
            schemas = ResourceSchemaChain.GetValidatingSchemas("MapDefinition-2.4.0.xsd");
            Assert.Contains("WatermarkDefinition-2.4.0.xsd", schemas);
        }
    }
}
