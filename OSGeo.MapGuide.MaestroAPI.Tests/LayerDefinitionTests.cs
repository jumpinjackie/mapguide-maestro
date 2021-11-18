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

using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using System;
using System.Linq;
using Xunit;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    public class LayerDefinitionTests
    {
        [Theory]
        [InlineData("1.0.0")]
        [InlineData("1.1.0")]
        [InlineData("1.2.0")]
        [InlineData("1.3.0")]
        [InlineData("2.3.0")]
        [InlineData("2.4.0")]
        [InlineData("4.0.0")]
        public void TestTextSymbolClone(string version)
        {
            var ldf = ObjectFactory.CreateDefaultLayer(LayerType.Vector, new Version(version));
            var vl = ldf.SubLayer as IVectorLayerDefinition;
            var vsr = vl.VectorScaleRange.First();
            var ar = vsr.AreaStyle.GetRuleAt(0);

            ar.Label = ldf.CreateDefaultTextSymbol();

            // Set advanced placement
            ar.Label.AdvancedPlacement = ldf.CreateDefaultAdvancedPlacement(0.7);
            var clonedLabel = ar.Label.Clone();

            Assert.NotNull(clonedLabel.AdvancedPlacement);
            Assert.Equal(0.7, clonedLabel.AdvancedPlacement.ScaleLimit);

            clonedLabel.AdvancedPlacement.ScaleLimit = 0.8;
            var clonedLabel2 = clonedLabel.Clone();

            Assert.NotNull(clonedLabel2.AdvancedPlacement);
            Assert.Equal(0.8, clonedLabel2.AdvancedPlacement.ScaleLimit);
        }
    }
}
