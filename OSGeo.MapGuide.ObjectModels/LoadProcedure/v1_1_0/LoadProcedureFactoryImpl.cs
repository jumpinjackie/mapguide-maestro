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

#define LP110
using System.IO;

#pragma warning disable 1591, 0114, 0108

#if LP110

namespace OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_1_0
#elif LP220
namespace OSGeo.MapGuide.ObjectModels.LoadProcedure.v2_2_0
#else

namespace OSGeo.MapGuide.ObjectModels.LoadProcedure.v1_0_0
#endif
{
    /// <summary>
    /// Helper class for registration with <see cref="ObjectFactory"/> and <see cref="ResourceTypeRegistry"/> classes
    /// </summary>
    public static class LoadProcEntryPoint
    {
        private const string ARBITRARY_XYM = "LOCAL_CS[\"Non-Earth (Meter)\", LOCAL_DATUM[\"Local Datum\", 0], UNIT[\"Meter\", 1], AXIS[\"X\", EAST], AXIS[\"Y\", NORTH]]"; //NOXLATE

        private static void ApplyDefaults(LoadProcedureType lt)
        {
            lt.RootPath = "Library://"; //NOXLATE
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