#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using Xunit;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    public class ApplicationDefinitionTests
    {
        [Fact]
        public void ApplicationDefinitionDeserializationWithFullContentModel()
        {
            IResource res = ObjectFactory.DeserializeXml(Properties.Resources.AppDef_1_0_0);
            Assert.NotNull(res);
            Assert.Equal(res.ResourceType, "ApplicationDefinition");
            Assert.Equal(res.ResourceVersion, new Version(1, 0, 0));
            IApplicationDefinition appDef = res as IApplicationDefinition;
            Assert.NotNull(appDef);
        }
    }
}