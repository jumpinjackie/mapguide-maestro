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
using OSGeo.MapGuide.MaestroAPI;

namespace MaestroAPITests
{
    /// <summary>
    /// This test fixture is to ensure all objects created by <see cref="ObjectFactory"/> have no
    /// null child object properties where there shouldn't be any.
    /// 
    /// Also tests for various classes (not belonging to any particular group) reside here
    /// </summary>
    [TestFixture]
    public class ObjectTests
    {
        [Test]
        public void TestArgParser()
        {
            string[] args = new string[] { "-foo", "-bar:snafu", "-whatever:" };

            var parser = new ArgumentParser(args);
            Assert.IsFalse(parser.IsDefined("snafu"));
            Assert.IsTrue(parser.IsDefined("foo"));
            Assert.IsTrue(parser.IsDefined("bar"));
            Assert.IsTrue(parser.IsDefined("whatever"));
            Assert.AreEqual(string.Empty, parser.GetValue("whatever"));
            Assert.AreEqual(parser.GetValue("bar"), "snafu");
        }

        [Test]
        public void TestEnvelope()
        {
            var env = ObjectFactory.CreateEnvelope(-.1, -.1, .1, .1);
            Assert.AreEqual(env.MinX, -.1);
            Assert.AreEqual(env.MinY, -.1);
            Assert.AreEqual(env.MaxX, .1);
            Assert.AreEqual(env.MaxY, .1);

            Assert.Catch<ArgumentException>(() => ObjectFactory.CreateEnvelope(.1, -.1, -.1, .1));
            Assert.Catch<ArgumentException>(() => ObjectFactory.CreateEnvelope(-.1, .1, .1, -.1));
            Assert.Catch<ArgumentException>(() => ObjectFactory.CreateEnvelope(.1, .1, -.1, -.1));
        }

        [Test]
        public void TestSecurityUser()
        {
            var user = ObjectFactory.CreateSecurityUser();
            Assert.IsNotNull(user.User);
        }

        [Test]
        public void TestSecurityGroup()
        {
            var group = ObjectFactory.CreateSecurityGroup();
            Assert.IsNotNull(group.Group);
        }

        [Test]
        public void TestResourceMetadata()
        {
            var meta = ObjectFactory.CreateMetadata();
            Assert.IsNotNull(meta.Simple);
            Assert.IsNotNull(meta.Simple.Property);

            Assert.AreEqual(0, meta.GetProperties().Count);
        }
    }
}
