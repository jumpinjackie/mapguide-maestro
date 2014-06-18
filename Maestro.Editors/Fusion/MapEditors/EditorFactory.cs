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
#endregion
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Editors.Fusion.MapEditors
{
    class EditorFactory
    {
        const string G_NORMAL_MAP = "G_NORMAL_MAP"; //NOXLATE
        const string G_SATELLITE_MAP = "G_SATELLITE_MAP"; //NOXLATE
        const string G_HYBRID_MAP = "G_HYBRID_MAP"; //NOXLATE
        const string G_PHYSICAL_MAP = "G_PHYSICAL_MAP"; //NOXLATE

        const string BING_ROAD = "Road"; //NOXLATE
        const string BING_AERIAL = "Aerial"; //NOXLATE
        const string BING_HYBRID = "Hybrid"; //NOXLATE

        internal const string Type_Google = "Google"; //NOXLATE
        internal const string Type_Bing = "VirtualEarth"; //NOXLATE
        internal const string Type_OSM = "OpenStreetMap"; //NOXLATE
        internal const string Type_MapGuide = "MapGuide"; //NOXLATE
        const string Type_Generic = "Generic"; //NOXLATE

        const string OSM_MAP_MAPNIK = "Mapnik"; //NOXLATE
        const string OSM_MAP_TRANSPORTMAP = "TransportMap"; //NOXLATE
        const string OSM_MAP_CYCLEMAP = "CycleMap"; //NOXLATE

        internal const string OSM_URL = "http://www.openstreetmap.org/openlayers/OpenStreetMap.js"; //NOXLATE
        internal const string GOOGLE_URL = "http://maps.google.com/maps/api/js?sensor=false"; //NOXLATE
        internal const string BING_URL = "http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.2"; //NOXLATE

        internal static Control GetEditor(IEditorService edSvc, IMapGroup group, IMap map)
        {
            switch (map.Type)
            {
                case Type_Google:
                    return new CommercialMapEditor(edSvc, map, new string[] { G_NORMAL_MAP, G_SATELLITE_MAP, G_HYBRID_MAP, G_PHYSICAL_MAP });
                case Type_Bing:
                    return new CommercialMapEditor(edSvc, map, new string[] { BING_ROAD, BING_AERIAL, BING_HYBRID });
                case Type_OSM:
                    return new CommercialMapEditor(edSvc, map, new string[] { OSM_MAP_MAPNIK, OSM_MAP_CYCLEMAP, OSM_MAP_TRANSPORTMAP });
                case Type_MapGuide:
                    return new MapGuideEditor(edSvc, group, map);
                default:
                    return new GenericEditor(edSvc, map);
            }
        }

        internal static IEnumerable<EditorInvoker> GetAvailableOptions(IMapGroup group)
        {
            yield return new EditorInvoker()
            {
                Name = "MapGuide", //NOXLATE
                Action = () =>
                {
                    return group.CreateMapGuideEntry("");
                }
            };
            yield return new EditorInvoker()
            {
                Name = Strings.CmsGeneric,
                Action = () =>
                {
                    return group.CreateGenericEntry();
                }
            };
            yield return new EditorInvoker() 
            {
                Name = Strings.CmsGoogleStreets, 
                Action = () => {
                    return group.CreateCmsMapEntry(Type_Google, false, Strings.CmsGoogleStreets, G_NORMAL_MAP);
                }
            };
            yield return new EditorInvoker()
            {
                Name = Strings.CmsGoogleSatellite,
                Action = () =>
                {
                    return group.CreateCmsMapEntry(Type_Google, false, Strings.CmsGoogleSatellite, G_SATELLITE_MAP);
                }
            };
            yield return new EditorInvoker()
            {
                Name = Strings.CmsGoogleHybrid,
                Action = () =>
                {
                    return group.CreateCmsMapEntry(Type_Google, false, Strings.CmsGoogleHybrid, G_HYBRID_MAP);
                }
            };
            yield return new EditorInvoker()
            {
                Name = Strings.CmsGooglePhysical,
                Action = () =>
                {
                    return group.CreateCmsMapEntry(Type_Google, false, Strings.CmsGooglePhysical, G_PHYSICAL_MAP);
                }
            };
            yield return new EditorInvoker()
            {
                Name = Strings.CmsBingStreet,
                Action = () =>
                {
                    return group.CreateCmsMapEntry(Type_Bing, false, Strings.CmsBingStreet, BING_ROAD);
                }
            };
            yield return new EditorInvoker()
            {
                Name = Strings.CmsBingSatellite,
                Action = () =>
                {
                    return group.CreateCmsMapEntry(Type_Bing, false, Strings.CmsBingSatellite, BING_AERIAL);
                }
            };
            yield return new EditorInvoker()
            {
                Name = Strings.CmsBingHybrid,
                Action = () =>
                {
                    return group.CreateCmsMapEntry(Type_Bing, false, Strings.CmsBingHybrid, BING_HYBRID);
                }
            };
            yield return new EditorInvoker()
            {
                Name = Strings.CmsOsm,
                Action = () =>
                {
                    return group.CreateCmsMapEntry(Type_OSM, false, Strings.CmsOsm, OSM_MAP_MAPNIK);
                }
            };
            yield return new EditorInvoker()
            {
                Name = Strings.CmsOsmCycle,
                Action = () =>
                {
                    return group.CreateCmsMapEntry(Type_OSM, false, Strings.CmsOsmCycle, OSM_MAP_CYCLEMAP);
                }
            };
            yield return new EditorInvoker()
            {
                Name = Strings.CmsOsmTransport,
                Action = () =>
                {
                    return group.CreateCmsMapEntry(Type_OSM, false, Strings.CmsOsmTransport, OSM_MAP_TRANSPORTMAP);
                }
            };
        }
    }

    class EditorInvoker
    {
        public string Name;

        public Func<IMap> Action;
    }
}
