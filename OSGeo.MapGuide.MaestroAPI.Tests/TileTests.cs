#region Disclaimer / License

// Copyright (C) 2017, Jackie Ng
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

using OSGeo.MapGuide.MaestroAPI.Tile;
using Xunit;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    public class TileTests
    {
        internal static readonly double[] SCALE_LIST = { 200000, 100000, 50000, 25000, 12500, 6250, 3125, 1562.5, 781.25, 390.625 };

        [Fact]
        public void Test_TileRef_Serialization()
        {
            var tr = new TileRef("abc", 1, 2, 3);
            var str = tr.Serialize();

            Assert.Equal("1/2/3/abc", str);

            var tr2 = TileRef.Parse(str);
            Assert.NotNull(tr2);
            Assert.Equal(1, tr2.Value.Row);
            Assert.Equal(2, tr2.Value.Col);
            Assert.Equal(3, tr2.Value.Scale);
            Assert.Equal("abc", tr2.Value.GroupName);
        }

        [Theory]
        [InlineData("1/2/3/", false, 1, 2, 3, "")]
        [InlineData("1/2/3/4", false, 1, 2, 3, "4")]
        [InlineData("1/2/3/4/5", false, 1, 2, 3, "4/5")]
        [InlineData("1/2/3/4/ 5", false, 1, 2, 3, "4/ 5")]
        [InlineData("1/2/3/4 /5", false, 1, 2, 3, "4 /5")]
        [InlineData("1/2/3", true, null, null, null, null)]
        [InlineData("1/2/a", true, null, null, null, null)]
        [InlineData("1/a/3", true, null, null, null, null)]
        [InlineData("a/2/3", true, null, null, null, null)]
        [InlineData("1/2/a/asdf", true, null, null, null, null)]
        [InlineData("1/a/3/asdf", true, null, null, null, null)]
        [InlineData("a/2/3/asdf", true, null, null, null, null)]
        public void Test_TileRef_Deserialization(string str, bool expectNull, int? row, int? col, int? scale, string groupName)
        {
            var tr = TileRef.Parse(str);
            if (expectNull)
            {
                Assert.Null(tr);
            }
            else
            {
                Assert.NotNull(tr);
                Assert.Equal(row, tr.Value.Row);
                Assert.Equal(col, tr.Value.Col);
                Assert.Equal(scale, tr.Value.Scale);
                Assert.Equal(groupName, tr.Value.GroupName);
            }
        }
    }
}
