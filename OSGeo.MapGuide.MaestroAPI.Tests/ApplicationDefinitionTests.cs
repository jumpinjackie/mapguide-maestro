﻿#region Disclaimer / License

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

using Moq;
using Newtonsoft.Json.Linq;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Tests;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition.v1_0_0;
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
        public void ApplicationDefinition_AvailableTemplates_Deserialization()
        {
            var conn = new Mock<PlatformConnectionBase>();
            conn.CallBase = true;

            using (var s = Utils.OpenFile($"Resources{System.IO.Path.DirectorySeparatorChar}AppDefTemplates.xml"))
            {
                var availTemplates = conn.Object.DeserializeObject<ApplicationDefinitionTemplateInfoSet>(s);
                Assert.Equal(5, availTemplates.TemplateInfo.Count);
                Assert.Contains(availTemplates.TemplateInfo, t => t.Name == "Slate");
                Assert.Contains(availTemplates.TemplateInfo, t => t.Name == "Aqua");
                Assert.Contains(availTemplates.TemplateInfo, t => t.Name == "Maroon");
                Assert.Contains(availTemplates.TemplateInfo, t => t.Name == "LimeGold");
                Assert.Contains(availTemplates.TemplateInfo, t => t.Name == "TurquoiseYellow");
            }
        }

        [Fact]
        public void ApplicationDefinition_AvailableContainers_Deserialization()
        {
            var conn = new Mock<PlatformConnectionBase>();
            conn.CallBase = true;

            using (var s = Utils.OpenFile($"Resources{System.IO.Path.DirectorySeparatorChar}AppDefContainers.xml"))
            {
                var availContainers = conn.Object.DeserializeObject<ApplicationDefinitionContainerInfoSet>(s);
                Assert.Equal(3, availContainers.ContainerInfo.Count);
                Assert.Contains(availContainers.ContainerInfo, t => t.Type == "ContextMenu");
                Assert.Contains(availContainers.ContainerInfo, t => t.Type == "Splitterbar");
                Assert.Contains(availContainers.ContainerInfo, t => t.Type == "Toolbar");
            }
        }

        [Fact]
        public void ApplicationDefinition_AvailableWidgets_Deserialization()
        {
            var conn = new Mock<PlatformConnectionBase>();
            conn.CallBase = true;

            using (var s = Utils.OpenFile($"Resources{System.IO.Path.DirectorySeparatorChar}AppDefWidgets.xml"))
            {
                var availWidgets = conn.Object.DeserializeObject<ApplicationDefinitionWidgetInfoSet>(s);
                Assert.Equal(52, availWidgets.WidgetInfo.Count);
            }
        }

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
        public void ApplicationDefinition_SubjectLayer_NestedParams()
        {
            var res = CreateFlexLayout();
            //Make our MapGroup
            var mg = res.AddMapGroup("MainMap");
            //Make subject layer
            var sub = mg.CreateSubjectLayerEntry("Parcels", "GeoJSON");
            dynamic props = new ExpandoObject();
            props.layer_name = "Sheboygan";
            props.source_type = "GeoJSON";
            props.source_param_url = new ExpandoObject();
            props.source_param_url.var_source = "myParcelFeatures";
            props.meta_extents = new[] { -9769953.66131227, 5417808.88017179, -9762220.79944393, 5434161.22418638 };
            props.meta_projection = "EPSG:3857";

            sub.SetSubjectOrExternalLayerProperties((IDictionary<string, object>)props);

            mg.AddMap(sub);

            string json = AppDefJsonSerializer.Serialize(res);
            var appDef = JObject.Parse(json);

            // Verify types of converted elements
            var mapEl = appDef.SelectToken("MapSet.MapGroup[0].Map[0]");
            Assert.Equal("SubjectLayer", mapEl["Type"].Value<string>());
            var urls = mapEl.SelectToken("Extension.source_param_url.var_source");
            Assert.NotNull(urls);
            Assert.Equal(JTokenType.String, urls.Type);
            var extents = mapEl.SelectToken("Extension.meta_extents");
            Assert.Equal(JTokenType.Array, extents.Type);
            foreach (var ev in (JArray)extents)
            {
                Assert.Equal(JTokenType.Float, ev.Type);
            }
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
            props.source_param_urls = new[]
            {
                "http://localhost:8018/mapguide/mapagent/mapagent.fcgi?OPERATION=GETTILEIMAGE&VERSION=1.2.0&CLIENTAGENT=&USERNAME=Anonymous&MAPDEFINITION=Library://Samples/Sheboygan/TileSets/SheboyganXYZ.TileSetDefinition&BASEMAPLAYERGROUPNAME=Base Layer Group&TILECOL={y}&TILEROW={x}&SCALEINDEX={z}"
            };
            props.meta_extents = new[] { -9769953.66131227, 5417808.88017179, -9762220.79944393, 5434161.22418638 };
            props.meta_projection = "EPSG:3857";

            sub.SetSubjectOrExternalLayerProperties((IDictionary<string, object>)props);

            mg.AddMap(sub);

            string json = AppDefJsonSerializer.Serialize(res);
            var appDef = JObject.Parse(json);

            // Verify types of converted elements
            var mapEl = appDef.SelectToken("MapSet.MapGroup[0].Map[0]");
            Assert.Equal("SubjectLayer", mapEl["Type"].Value<string>());
            var urls = mapEl.SelectToken("Extension.source_param_urls");
            Assert.Equal(JTokenType.Array, urls.Type);
            var extents = mapEl.SelectToken("Extension.meta_extents");
            Assert.Equal(JTokenType.Array, extents.Type);
            foreach (var ev in (JArray)extents)
            {
                Assert.Equal(JTokenType.Float, ev.Type);
            }
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

            // Verify types of converted elements
            var mapEl = appDef.SelectToken("MapSet.MapGroup[0].Map[0]");
            Assert.NotNull(mapEl);
            var urlsEl = mapEl.SelectToken("Extension.Options.urls");
            Assert.Equal(JTokenType.Array, urlsEl.Type);
        }
    }
}