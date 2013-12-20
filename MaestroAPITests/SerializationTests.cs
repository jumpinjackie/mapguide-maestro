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
using NUnit.Framework;
using OSGeo.MapGuide.ObjectModels;
using NMock2;
using OSGeo.MapGuide.MaestroAPI;

namespace MaestroAPITests
{
    [TestFixture]
    public class SerializationTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            if (TestControl.IgnoreSerializationTests)
                Assert.Ignore("Skipping SerializationTests because TestControl.IgnoreSerializationTests = true");
        }

        [Test]
        public void TestPreMg22FdoCapabilities()
        {
            //MGOS <= 2.2 returned different xml from a GETCAPABILITIES
            //call even though the schema was the same. Verify capabilities
            //are read properly regardless of version
        }

        [Test]
        public void TestResourceContentVersionInspection()
        {
            //Verify our ResoureContentVersionChecker can correctly read the
            //version numbers of resource content streams
        }
    }
}
