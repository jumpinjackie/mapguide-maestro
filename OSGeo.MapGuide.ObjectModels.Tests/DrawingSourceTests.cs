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

using OSGeo.MapGuide.MaestroAPI.Tests;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using System;
using Xunit;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    public class DrawingSourceTests
    {
        [Fact]
        public void DrawingSourceDeserializationWithFullContentModel()
        {
            IResource res = ObjectFactory.DeserializeXml(Utils.ReadAllText($"Resources{System.IO.Path.DirectorySeparatorChar}DrawingSource_1_0_0.txt"));
            Assert.NotNull(res);
            Assert.Equal("DrawingSource", res.ResourceType);
            Assert.Equal(res.ResourceVersion, new Version(1, 0, 0));
            IDrawingSource ds = res as IDrawingSource;
            Assert.NotNull(ds);
        }
    }
}