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

using OSGeo.MapGuide.ObjectModels.SelectionModel;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests.Mapping
{
    public class QueryFeaturesResponseTests
    {
        [Fact]
        public void QueryFeaturesResponse_Serialization()
        {
            var xml = Utils.ReadAllText($"Resources{System.IO.Path.DirectorySeparatorChar}QueryMapFeatures.xml");
            var fi = FeatureInformation.ParseFromXml(xml);

            Assert.NotNull(fi);
            Assert.NotNull(fi.FeatureSet);
            Assert.Single(fi.FeatureSet.Layer);
            Assert.Equal(34, fi.FeatureSet.Layer[0].Class.ID.Count);
            Assert.NotNull(fi.SelectedFeatures);
            Assert.Single(fi.SelectedFeatures.SelectedLayer);
            Assert.Equal(34, fi.SelectedFeatures.SelectedLayer[0].Feature.Count);
        }
    }
}
