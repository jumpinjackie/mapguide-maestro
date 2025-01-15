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

using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Maestro.Editors.Fusion.MapEditors
{
    internal static class EditorFactory
    {
        private const string BING_ROAD = "Road"; //NOXLATE
        private const string BING_AERIAL = "Aerial"; //NOXLATE
        private const string BING_AERIAL_WITH_LABELS = "AerialWithLabels"; //NOXLATE

        internal const string Type_Bing = "VirtualEarth"; //NOXLATE
        internal const string Type_OSM = "OpenStreetMap"; //NOXLATE
        internal const string Type_MapGuide = "MapGuide"; //NOXLATE
        internal const string Type_Stamen = "Stamen"; //NOXLATE
        internal const string Type_StadiaMaps = "StadiaMaps"; //NOXLATE
        internal const string Type_XYZ = "XYZ"; //NOXLATE
        private const string Type_Generic = "Generic"; //NOXLATE

        private const string OSM_MAP_MAPNIK = "Mapnik"; //NOXLATE
        private const string OSM_MAP_TRANSPORTMAP = "TransportMap"; //NOXLATE
        private const string OSM_MAP_CYCLEMAP = "CycleMap"; //NOXLATE

        private const string STAMEN_TERRAIN = "terrain"; //NOXLATE
        private const string STAMEN_TONER = "toner"; //NOXLATE
        private const string STAMEN_WATERCOLOR = "watercolor"; //NOXLATE

        private const string STADIA_ALIDADE_SMOOTH = "alidade_smooth"; //NOXLATE
        private const string STADIA_ALIDADE_SMOOTH_DARK = "alidade_smooth_dark"; //NOXLATE
        private const string STADIA_ALIDADE_SATELLITE = "alidade_satellite"; //NOXLATE
        private const string STADIA_OUTDOORS = "outdoors"; //NOXLATE

        internal static Control GetEditor(IEditorService edSvc, IMapGroup group, IMap map)
        {
            switch (map.Type)
            {
                case Type_Bing:
                    return new CommercialMapEditor(edSvc, map, new string[] { BING_ROAD, BING_AERIAL, BING_AERIAL_WITH_LABELS });

                case Type_OSM:
                    return new CommercialMapEditor(edSvc, map, new string[] { OSM_MAP_MAPNIK, OSM_MAP_CYCLEMAP, OSM_MAP_TRANSPORTMAP });

                case Type_Stamen:
                    return new CommercialMapEditor(edSvc, map, new string[] { STAMEN_TONER, STAMEN_TERRAIN, STAMEN_WATERCOLOR });

                case Type_StadiaMaps:
                    return new CommercialMapEditor(edSvc, map, new string[] { STADIA_ALIDADE_SMOOTH, STADIA_ALIDADE_SMOOTH_DARK, STADIA_ALIDADE_SATELLITE, STADIA_OUTDOORS });

                case Type_MapGuide:
                    return new MapGuideEditor(edSvc, group, map);

                case Type_XYZ:
                    return new XYZEditor(edSvc, group, map);

                default:
                    return new GenericEditor(edSvc, map);
            }
        }

        static IMap StadiaXyzSetup(IApplicationDefinition appDef, IMapGroup group, string label, string tileset)
        {
            var apiKey = appDef.GetValue(CommercialMapEditor.STADIA_MAPS_EXTENSION_NAME);
            apiKey = PromptDialog.Show(Strings.ApiKeyDescStadiaMaps, Strings.StadiaMapsKeyPrompt, apiKey);
            appDef.SetValue(CommercialMapEditor.STADIA_MAPS_EXTENSION_NAME, apiKey);

            var mapLayer = group.CreateCmsMapEntry(Type_XYZ, false, label, tileset);
            switch (tileset)
            {
                case STAMEN_TONER:
                    mapLayer.SetXYZUrls("https://tiles.stadiamaps.com/tiles/stamen_toner/${z}/${x}/${y}.png?api_key=" + apiKey); //NOXLATE
                    break;
                case STAMEN_TERRAIN:
                    mapLayer.SetXYZUrls("https://tiles.stadiamaps.com/tiles/stamen_terrain/${z}/${x}/${y}.png?api_key=" + apiKey); //NOXLATE
                    break;
                case STAMEN_WATERCOLOR: // Only jpg supported
                    mapLayer.SetXYZUrls("https://tiles.stadiamaps.com/tiles/stamen_watercolor/${z}/${x}/${y}.jpg?api_key=" + apiKey); //NOXLATE
                    break;
                case STADIA_ALIDADE_SMOOTH:
                    mapLayer.SetXYZUrls("https://tiles.stadiamaps.com/tiles/alidade_smooth/${z}/${x}/${y}.png?api_key=" + apiKey); //NOXLATE
                    break;
                case STADIA_ALIDADE_SMOOTH_DARK:
                    mapLayer.SetXYZUrls("https://tiles.stadiamaps.com/tiles/alidade_smooth_dark/${z}/${x}/${y}.png?api_key=" + apiKey); //NOXLATE
                    break;
                case STADIA_ALIDADE_SATELLITE:
                    mapLayer.SetXYZUrls("https://tiles.stadiamaps.com/tiles/alidade_satellite/${z}/${x}/${y}.png?api_key=" + apiKey); //NOXLATE
                    break;
                case STADIA_OUTDOORS:
                    mapLayer.SetXYZUrls("https://tiles.stadiamaps.com/tiles/outdoors/${z}/${x}/${y}.png?api_key=" + apiKey); //NOXLATE
                    break;
            }
            return mapLayer;
        }

        internal static IEnumerable<EditorInvoker> GetAvailableOptions(IApplicationDefinition appDef, Version version, IMapGroup group)
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

                //XYZ variants
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStamenTonerXyz,
                    Action = () =>
                    {
                        return StadiaXyzSetup(appDef, group, Strings.CmsStamenTonerXyz, STAMEN_TONER);
                    }
                };
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStamenTerrainXyz,
                    Action = () =>
                    {
                        return StadiaXyzSetup(appDef, group, Strings.CmsStamenTerrainXyz, STAMEN_TERRAIN);
                    }
                };
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStamenWaterColorXyz,
                    Action = () =>
                    {
                        return StadiaXyzSetup(appDef, group, Strings.CmsStamenWaterColorXyz, STAMEN_WATERCOLOR);
                    }
                };

                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStadiaAlidadeSmooth,
                    Action = () =>
                    {
                        return group.CreateCmsMapEntry(Type_StadiaMaps, false, Strings.CmsStadiaAlidadeSmooth, STADIA_ALIDADE_SMOOTH);
                    }
                };
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStadiaAlidadeSmoothDark,
                    Action = () =>
                    {
                        return group.CreateCmsMapEntry(Type_StadiaMaps, false, Strings.CmsStadiaAlidadeSmoothDark, STADIA_ALIDADE_SMOOTH_DARK);
                    }
                };
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStadiaAlidadeSatellite,
                    Action = () =>
                    {
                        return group.CreateCmsMapEntry(Type_StadiaMaps, false, Strings.CmsStadiaAlidadeSatellite, STADIA_ALIDADE_SATELLITE);
                    }
                };
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStadiaOutdoors,
                    Action = () =>
                    {
                        return group.CreateCmsMapEntry(Type_StadiaMaps, false, Strings.CmsStadiaOutdoors, STADIA_OUTDOORS);
                    }
                };

                //XYZ variants
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStadiaAlidadeSmoothXyz,
                    Action = () =>
                    {
                        return StadiaXyzSetup(appDef, group, Strings.CmsStadiaAlidadeSmoothXyz, STADIA_ALIDADE_SMOOTH);
                    }
                };
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStadiaAlidadeSmoothDarkXyz,
                    Action = () =>
                    {
                        return StadiaXyzSetup(appDef, group, Strings.CmsStadiaAlidadeSmoothDarkXyz, STADIA_ALIDADE_SMOOTH_DARK);
                    }
                };
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStadiaAlidadeSatelliteXyz,
                    Action = () =>
                    {
                        return StadiaXyzSetup(appDef, group, Strings.CmsStadiaAlidadeSatelliteXyz, STADIA_ALIDADE_SATELLITE);
                    }
                };
                yield return new EditorInvoker()
                {
                    Name = Strings.CmsStadiaOutdoorsXyz,
                    Action = () =>
                    {
                        return StadiaXyzSetup(appDef, group, Strings.CmsStadiaOutdoorsXyz, STADIA_OUTDOORS);
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