using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.IO;
using OSGeo.MapGuide.MaestroAPI;

#pragma warning disable 1591, 0114, 0108

#if LP110
namespace OSGeo.MapGuide.ObjectModels.LoadProcedure_1_1_0
#elif LP220
namespace OSGeo.MapGuide.ObjectModels.LoadProcedure_2_2_0
#else
namespace OSGeo.MapGuide.ObjectModels.LoadProcedure_1_0_0
#endif
{
    /// <summary>
    /// Helper class for registration with <see cref="OSGeo.MapGuide.ObjectModels.ObjectFactory"/> and <see cref="OSGeo.MapGuide.MaestroAPI.ResourceTypeRegistry"/> classes
    /// </summary>
    public static class LoadProcEntryPoint
    {
        const string ARBITRARY_XYM = "LOCAL_CS[\"Non-Earth (Meter)\", LOCAL_DATUM[\"Local Datum\", 0], UNIT[\"Meter\", 1], AXIS[\"X\", EAST], AXIS[\"Y\", NORTH]]"; //NOXLATE

        private static void ApplyDefaults(LoadProcedureType lt)
        {
            lt.RootPath = StringConstants.RootIdentifier;
            lt.CoordinateSystem = ARBITRARY_XYM;
            lt.SpatialDataSourcesPath = string.Empty;
            lt.SpatialDataSourcesFolder = "Data"; //NOXLATE
            lt.LayersPath = string.Empty;
            lt.LayersFolder = "Layers"; //NOXLATE
            lt.GenerateMaps = false;
            lt.GenerateLayers = true;
            lt.GenerateSpatialDataSources = true;
            lt.SourceFile = new System.ComponentModel.BindingList<string>();
        }

        public static ILoadProcedure CreateDefaultSdf()
        {
            var proc = new LoadProcedure()
            {
                Item = new SdfLoadProcedureType() { Generalization = 100.0 }
            };
            ApplyDefaults(proc.Item);
            return proc;
        }

        public static ILoadProcedure CreateDefaultShp()
        {
            var proc = new LoadProcedure()
            {
                Item = new ShpLoadProcedureType() { Generalization = 100.0, ConvertToSdf = false }
            };
            ApplyDefaults(proc.Item);
            return proc;
        }

        public static ILoadProcedure CreateDefaultDwf()
        {
            var proc = new LoadProcedure()
            {
                Item = new DwfLoadProcedureType()
            };
            ApplyDefaults(proc.Item);
            return proc;
        }

        public static IResource Deserialize(string xml)
        {
            return LoadProcedure.Deserialize(xml);
        }

        public static Stream Serialize(IResource res)
        {
            return res.SerializeToStream();
        }

#if LP220

        public static ILoadProcedure CreateDefaultSqlite()
        {
            var proc = new LoadProcedure()
            {
                Item = new SQLiteLoadProcedureType() { Generalization = 100.0 }
            };
            ApplyDefaults(proc.Item);
            return proc;
        }

#endif
    }
}
