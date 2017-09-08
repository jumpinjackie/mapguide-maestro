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

namespace OSGeo.MapGuide.MaestroAPI.Schema.Tests
{
    public class DataPropertyDefinitionTests
    {
        [Fact]
        public void DataPropertyDefinitionTest()
        {
            var prop = new DataPropertyDefinition("Foo", "Bar");
            Assert.Equal("Foo", prop.Name);
            Assert.Equal("Bar", prop.Description);
            Assert.Equal(DataPropertyType.String, prop.DataType);
        }

        [Fact]
        public void IsNumericTypeTest()
        {
            var prop = new DataPropertyDefinition("Foo", "Bar");
            Assert.Equal("Foo", prop.Name);
            Assert.Equal("Bar", prop.Description);
            Assert.Equal(DataPropertyType.String, prop.DataType);

            foreach (DataPropertyType dt in Enum.GetValues(typeof(DataPropertyType)))
            {
                prop.DataType = dt;
                if (dt == DataPropertyType.Blob ||
                    dt == DataPropertyType.Boolean ||
                    dt == DataPropertyType.Clob ||
                    dt == DataPropertyType.DateTime ||
                    dt == DataPropertyType.String)
                {
                    Assert.False(prop.IsNumericType());
                }
                else
                {
                    Assert.True(prop.IsNumericType());
                }
            }
        }
    }
}
