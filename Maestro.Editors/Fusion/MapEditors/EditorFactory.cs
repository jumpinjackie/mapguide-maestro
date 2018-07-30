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

using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Maestro.Editors.Fusion.MapEditors
{
    internal static class EditorFactory
    {
        private const string G_NORMAL_MAP = "G_NORMAL_MAP"; //NOXLATE
        private const string G_SATELLITE_MAP = "G_SATELLITE_MAP"; //NOXLATE
        private const string G_HYBRID_MAP = "G_HYBRID_MAP"; //NOXLATE
        private const string G_PHYSICAL_MAP = "G_PHYSICAL_MAP"; //NOXLATE

        private const string BING_ROAD = "Road"; //NOXLATE
        private const string BING_AERIAL = "Aerial"; //NOXLATE
        /// <summary>
        /// Deprecated as of 30th June 2017 with the retirement of the V7 API. Do not use. 
        /// </summary>
        private const string BING_HYBRID = "Hybrid"; //NOXLATE
        private const string BING_AERIAL_WITH_LABELS = "AerialWithLabels"; //NOXLATE

        internal const string Type_Google = "Google"; //NOXLATE
        internal const string Type_Bing = "VirtualEarth"; //NOXLATE
        internal const string Type_OSM = "OpenStreetMap"; //NOXLATE
        internal const string Type_MapGuide = "MapGuide"; //NOXLATE
        internal const string Type_Stamen = "Stamen"; //NOXLATE
        internal const string Type_XYZ = "XYZ"; //NOXLATE
        private const string Type_Generic = "Generic"; //NOXLATE

        private const string OSM_MAP_MAPNIK = "Mapnik"; //NOXLATE
        private const string OSM_MAP_TRANSPORTMAP = "TransportMap"; //NOXLATE
        private const string OSM_MAP_CYCLEMAP = "CycleMap"; //NOXLATE

        private const string STAMEN_TERRAIN = "terrain"; //NOXLATE
        private const string STAMEN_TONER = "toner"; //NOXLATE
        private const string STAMEN_WATERCOLOR = "watercolor"; //NOXLATE

        internal const string GOOGLE_URL = "https://maps.googleapis.com/maps/api/js"; //NOXLATE

        internal static Control GetEditor(IEditorService edSvc, IMapGroup group, IMap map)
        {
            switch (map.Type)
            {
                case Type_Google:
                    return new CommercialMapEditor(edSvc, map, new string[] { G_NORMAL_MAP, G_SATELLITE_MAP, G_HYBRID_MAP, G_PHYSICAL_MAP });

                case Type_Bing:
                    return new CommercialMapEditor(edSvc, map, new string[] { BING_ROAD, BING_AERIAL, BING_AERIAL_WITH_LABELS });

                case Type_OSM:
                    return new CommercialMapEditor(edSvc, map, new string[] { OSM_MAP_MAPNIK, OSM_MAP_CYCLEMAP, OSM_MAP_TRANSPORTMAP });

                case Type_Stamen:
                    return new CommercialMapEditor(edSvc, map, new string[] { STAMEN_TONER, STAMEN_TERRAIN, STAMEN_WATERCOLOR });

                case Type_MapGuide:
                    return new MapGuideEditor(edSvc, group, map);

                case Type_XYZ:
                    return new XYZEditor(edSvc, group, map);

                default:
                    return new GenericEditor(edSvc, map);
            }
        }

        internal static IEnumerable<EditorInvoker> GetAvailableOptions(Version version, IMapGroup group)
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
                Action = group.CreateGenericEntry
            };
            yield return new EditorInvoker()
            {
                Name = Strings.CmsGoogleStreets,
                Action = () =>
                {
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
                Name = Strings.CmsBingAerialsWithLabels,
                Action = () =>
                {
                    return group.CreateCmsMapEntry(Type_Bing, false, Strings.CmsBingAerialsWithLabels, BING_AERIAL_WITH_LABELS);
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
            if (version.Major >= 3) //MGOS 3.0 or higher
            {
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStamenToner,
                    Action = () =>
                    {
                        return group.CreateCmsMapEntry(Type_Stamen, false, Strings.CmsStamenToner, STAMEN_TONER);
                    }
                };
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStamenTerrain,
                    Action = () =>
                    {
                        return group.CreateCmsMapEntry(Type_Stamen, false, Strings.CmsStamenTerrain, STAMEN_TERRAIN);
                    }
                };
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStamenWaterColor,
                    Action = () =>
                    {
                        return group.CreateCmsMapEntry(Type_Stamen, false, Strings.CmsStamenWaterColor, STAMEN_WATERCOLOR);
                    }
                };
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsXYZ,
                    Action = () =>
                    {
                        return group.CreateCmsMapEntry(Type_XYZ, false, Strings.CmsXYZ, "XYZ");
                    }
                };
            }
        }
    }

    internal class EditorInvoker
    {
        public string Name;

        public Func<IMap> Action;
    }
}