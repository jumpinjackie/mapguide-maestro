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

using Newtonsoft.Json.Linq;
using OSGeo.MapGuide.MaestroAPI.Tests;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace OSGeo.MapGuide.ObjectModels.Tests
{
    public class ApplicationDefinitionTests
    {
        [Fact]
        public void ApplicationDefinitionDeserializationWithFullContentModel()
        {
            IResource res = ObjectFactory.DeserializeXml(Utils.ReadAllText($"Resources{System.IO.Path.DirectorySeparatorChar}AppDef-1.0.txt"));
            Assert.NotNull(res);
            Assert.Equal("ApplicationDefinition", res.ResourceType);
            Assert.Equal(res.ResourceVersion, new Version(1, 0, 0));
            IApplicationDefinition appDef = res as IApplicationDefinition;
            Assert.NotNull(appDef);
        }

        [Fact]
        public void ApplicationDefinition_SubjectLayers()
        {
            var res = CreateFlexLayout();
            //Make our MapGroup
            var mg = res.AddMapGroup("MainMap");
            //Make subject layer
            var sub = mg.CreateSubjectLayerEntry("MG Tileset", "XYZ");
            dynamic props = new ExpandoObject();
            props.layer_name = "Sheboygan XYZ";
            props.source_type = "XYZ";
            props.source_params_urls = new[]
            {
                "http://localhost:8018/mapguide/mapagent/mapagent.fcgi?OPERATION=GETTILEIMAGE&VERSION=1.2.0&CLIENTAGENT=&USERNAME=Anonymous&MAPDEFINITION=Library://Samples/Sheboygan/TileSets/SheboyganXYZ.TileSetDefinition&BASEMAPLAYERGROUPNAME=Base Layer Group&TILECOL={y}&TILEROW={x}&SCALEINDEX={z}"
            };
            props.meta_extents = new[] { -9769953.66131227, 5417808.88017179, -9762220.79944393, 5434161.22418638 };
            props.meta_projection = "EPSG:3857";

            sub.SetSubjectOrExternalLayerProperties((IDictionary<string, object>)props);

            mg.AddMap(sub);

            string json = AppDefJsonSerializer.Serialize(res);
            dynamic appDef = JObject.Parse(json);
        }

        static IApplicationDefinition CreateFlexLayout()
        {
            var res = ObjectFactory.DeserializeEmbeddedFlexLayout(new Version(2, 4, 0));
            //Clear groups
            var toRemove = res.MapSet.MapGroups.ToList();
            foreach (var rem in toRemove)
            {
                res.MapSet.RemoveGroup(rem);
            }
            return res;
        }

        [Fact]
        public void ApplicationDefinition_JsonConversion()
        {
            var res = CreateFlexLayout();
            //Make our MapGroup
            var mg = res.AddMapGroup("MainMap");

            //Add XYZ layer
            var ble = mg.CreateCmsMapEntry("XYZ", false, "Here Maps", "XYZ");
            var hereUrl = "https://1.base.maps.api.here.com/maptile/2.1/maptile/newest/normal.day/${z}/${x}/${y}/256/png8?app_id=YOUR_APP_ID_HERE&amp;app_code=YOUR_APP_CODE_HERE";
            ble.SetXYZUrls(hereUrl);

            //Check
            var xyzUrls = ble.GetXYZUrls();
            Assert.Single(xyzUrls);
            Assert.Equal(hereUrl, xyzUrls.First());

            mg.AddMap(ble);

            Assert.NotNull(res);
            string json = AppDefJsonSerializer.Serialize(res);
            dynamic appDef = JObject.Parse(json);
        }
    }
}