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
using OSGeo.MapGuide.MaestroAPI.CrossConnection;
using NUnit.Framework;
using Moq;
using OSGeo.MapGuide.ObjectModels;
using System.IO;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.CrossConnection.Tests
{
    [TestFixture]
    public class ResourceRebaserTests
    {
        [Test]
        public void ResourceRebaserTest()
        {
            Assert.Throws<ArgumentNullException>(() => new ResourceRebaser(null));

            var res = new Mock<IResource>();
            var rbaser = new ResourceRebaser(res.Object);
        }

        [Test]
        public void RebaseTest()
        {
            var res = (IMapDefinition)ObjectFactory.Deserialize(ResourceTypes.MapDefinition.ToString(), File.OpenRead("UserTestData\\TestTiledMap.xml"));
            res.ResourceID = "Library://Test.MapDefinition";
            var rbaser = new ResourceRebaser(res);
            var rbres = rbaser.Rebase("Library://UnitTests/Layers/", "Library://Rebased/Layers/");
            var xml = ObjectFactory.SerializeAsString(rbres);
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var els = doc.GetElementsByTagName("ResourceId");
            foreach (XmlNode el in els)
            {
                Assert.True(el.InnerText.StartsWith("Library://Rebased/Layers/"));
            }
        }
    }
}
