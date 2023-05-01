#region Disclaimer / License

// Copyright (C) 2021, Jackie Ng
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

using Moq;
using OSGeo.MapGuide.MaestroAPI.Converters;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests.Resource
{
    public class FsConversionTests
    {
        [Theory]
        [InlineData("OSGeo.OGR", true)]
        [InlineData("OSGeo.SHP", false)]
        [InlineData("OSGeo.SDF", true)]
        [InlineData("OSGeo.SQLite", true)]
        [InlineData("OSGeo.SQLServerSpatial", true)]
        [InlineData("OSGeo.WFS", true)]
        [InlineData("OSGeo.WMS", true)]
        [InlineData("OSGeo.MySQL", true)]
        [InlineData("OSGeo.PostgreSQL", true)]
        public void TestOgrConversion(string provider, bool expectFail)
        {
            var fs = new Mock<IFeatureSource>();
            var resSvc = new Mock<IResourceService>();
            var featSvc = new Mock<IFeatureService>();

            var mockOgrCaps = new FeatureProviderRegistryFeatureProvider
            {
                ConnectionProperties = new System.ComponentModel.BindingList<FeatureProviderRegistryFeatureProviderConnectionProperty>()
                {
                    new FeatureProviderRegistryFeatureProviderConnectionProperty { Name = "DefaultSchemaName" }
                }
            };

            fs.Setup(f => f.Provider).Returns(provider);
            featSvc
                .Setup(f => f.GetFeatureProvider(It.Is<string>(s => s == "OSGeo.OGR")))
                .Returns(mockOgrCaps);

            var newFs = Mock.Of<IFeatureSource>();

            var targetId = "Library://Samples/Data/Converted.FeatureSource";
            resSvc.Setup(r => r.GetResource(It.Is<string>(s => s == targetId))).Returns(newFs);

            var conv = new OgrFeatureSourceConverter(fs.Object, resSvc.Object, featSvc.Object);

            if (expectFail)
                Assert.ThrowsAny<Exception>(() => conv.Convert(targetId));
            else
                conv.Convert(targetId);

        }
    }
}
