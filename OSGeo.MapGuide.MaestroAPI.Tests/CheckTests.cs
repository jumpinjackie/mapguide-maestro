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

using System;
using Xunit;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    public class CheckTests
    {
        [Fact]
        public void NotNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => Check.ArgumentNotNull<object>(null, "Test"));
            Check.ArgumentNotNull("agdsfd", "arg");
        }

        [Fact]
        public void NotEmptyTest()
        {
            Assert.Throws<ArgumentException>(() => Check.ArgumentNotEmpty(null, "test"));
            Assert.Throws<ArgumentException>(() => Check.ArgumentNotEmpty("", "test"));
            Check.ArgumentNotEmpty("agdsfd", "arg");
        }

        [Fact]
        public void IsFolderTest()
        {
            Assert.Throws<ArgumentException>(() => Check.ThatArgumentIsFolder("Library://Test.FeatureSource", "test"));
            Assert.Throws<ArgumentException>(() => Check.ThatArgumentIsFolder("Library://Test", "test"));
            Check.ThatArgumentIsFolder("Library://Test/", "test");
        }

        [Fact]
        public void PreconditionTest()
        {
            Assert.Throws<ArgumentException>(() => Check.ThatPreconditionIsMet(false, "test"));
            Check.ThatPreconditionIsMet(true, "test");
        }

        [Fact]
        public void RangeTest()
        {
            Assert.Throws<ArgumentException>(() => Check.ThatArgumentIsBetweenRange(1, 2, 3, false, "test"));
            Assert.Throws<ArgumentException>(() => Check.ThatArgumentIsBetweenRange(1, 1, 1, false, "test"));
            Assert.Throws<ArgumentException>(() => Check.ThatArgumentIsBetweenRange(1.0, 1.0, 1.0, false, "test"));
            Assert.Throws<ArgumentException>(() => Check.ThatArgumentIsBetweenRange(1.0, 2.0, 3.0, false, "test"));

            Check.ThatArgumentIsBetweenRange(1, 1, 1, true, "test");
            Check.ThatArgumentIsBetweenRange(2, 1, 3, true, "test");
            Check.ThatArgumentIsBetweenRange(1, 1, 3, true, "test");
            Check.ThatArgumentIsBetweenRange(3, 1, 3, true, "test");
            Check.ThatArgumentIsBetweenRange(1.0, 1.0, 1.0, true, "test");
            Check.ThatArgumentIsBetweenRange(2.0, 1.0, 3.0, true, "test");
            Check.ThatArgumentIsBetweenRange(1.0, 1.0, 3.0, true, "test");
            Check.ThatArgumentIsBetweenRange(3.0, 1.0, 3.0, true, "test");
            Check.ThatArgumentIsBetweenRange(1.5, 1, 2, true, "test");
        }
    }
}
