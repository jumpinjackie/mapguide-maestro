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

using NUnit.Framework;
using OSGeo.MapGuide.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    [TestFixture]
    public class CheckTests
    {
        [Test]
        public void NotNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => Check.ArgumentNotNull<object>(null, "Test"));
            Assert.DoesNotThrow(() => Check.ArgumentNotNull("agdsfd", "arg"));
        }

        [Test]
        public void NotEmptyTest()
        {
            Assert.Throws<ArgumentException>(() => Check.ArgumentNotEmpty(null, "test"));
            Assert.Throws<ArgumentException>(() => Check.ArgumentNotEmpty("", "test"));
            Assert.DoesNotThrow(() => Check.ArgumentNotEmpty("agdsfd", "arg"));
        }

        [Test]
        public void IsFolderTest()
        {
            Assert.Throws<ArgumentException>(() => Check.ThatArgumentIsFolder("Library://Test.FeatureSource", "test"));
            Assert.Throws<ArgumentException>(() => Check.ThatArgumentIsFolder("Library://Test", "test"));
            Assert.DoesNotThrow(() => Check.ThatArgumentIsFolder("Library://Test/", "test"));
        }

        [Test]
        public void PreconditionTest()
        {
            Assert.Throws<ArgumentException>(() => Check.ThatPreconditionIsMet(false, "test"));
            Assert.DoesNotThrow(() => Check.ThatPreconditionIsMet(true, "test"));
        }

        [Test]
        public void RangeTest()
        {
            Assert.Throws<ArgumentException>(() => Check.ThatArgumentIsBetweenRange(1, 2, 3, false, "test"));
            Assert.Throws<ArgumentException>(() => Check.ThatArgumentIsBetweenRange(1, 1, 1, false, "test"));
            Assert.Throws<ArgumentException>(() => Check.ThatArgumentIsBetweenRange(1.0, 1.0, 1.0, false, "test"));
            Assert.Throws<ArgumentException>(() => Check.ThatArgumentIsBetweenRange(1.0, 2.0, 3.0, false, "test"));

            Assert.DoesNotThrow(() => Check.ThatArgumentIsBetweenRange(1, 1, 1, true, "test"));
            Assert.DoesNotThrow(() => Check.ThatArgumentIsBetweenRange(2, 1, 3, true, "test"));
            Assert.DoesNotThrow(() => Check.ThatArgumentIsBetweenRange(1, 1, 3, true, "test"));
            Assert.DoesNotThrow(() => Check.ThatArgumentIsBetweenRange(3, 1, 3, true, "test"));
            Assert.DoesNotThrow(() => Check.ThatArgumentIsBetweenRange(1.0, 1.0, 1.0, true, "test"));
            Assert.DoesNotThrow(() => Check.ThatArgumentIsBetweenRange(2.0, 1.0, 3.0, true, "test"));
            Assert.DoesNotThrow(() => Check.ThatArgumentIsBetweenRange(1.0, 1.0, 3.0, true, "test"));
            Assert.DoesNotThrow(() => Check.ThatArgumentIsBetweenRange(3.0, 1.0, 3.0, true, "test"));
            Assert.DoesNotThrow(() => Check.ThatArgumentIsBetweenRange(1.5, 1, 2, true, "test"));
        }
    }
}
