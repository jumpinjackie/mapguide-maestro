#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.ComponentModel;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.ObjectModels.LoadProcedure
{
    /// <summary>
    /// Defines how to handle duplicate SDF2 keys
    /// </summary>
    [System.SerializableAttribute()]
    public enum SdfKeyTreatmentType
    {
        /// <remarks/>
        AutogenerateAll,

        /// <remarks/>
        DiscardDuplicates,

        /// <remarks/>
        MergeDuplicates,
    }

    /// <summary>
    /// The types of load procedures
    /// </summary>
    public enum LoadType
    {
        /// <summary>
        /// 
        /// </summary>
        Sdf,
        /// <summary>
        /// 
        /// </summary>
        Shp,
        /// <summary>
        /// 
        /// </summary>
        Dwf,
        /// <summary>
        /// 
        /// </summary>
        Raster,
        /// <summary>
        /// 
        /// </summary>
        Dwg,
        /// <summary>
        /// 
        /// </summary>
        Sqlite
    }

    /// <summary>
    /// Represents Load Procedures
    /// </summary>
    public interface ILoadProcedure : IResource
    {
        /// <summary>
        /// Gets the type of the sub.
        /// </summary>
        /// <value>The type of the sub.</value>
        IBaseLoadProcedure SubType { get; }
    }

    /// <summary>
    /// A DWG load procedure. Execution not supported by Maestro
    /// </summary>
    public interface IDwgLoadProcedure : IBaseLoadProcedure
    {

    }

    /// <summary>
    /// A raster load procedure. Execution not supported by Maestro
    /// </summary>
    public interface IRasterLoadProcedure : IBaseLoadProcedure
    {

    }

    /// <summary>
    /// Base type of all load procedures
    /// </summary>
    public interface IBaseLoadProcedure : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        LoadType Type { get; }

        /// <summary>
        /// Gets the source files.
        /// </summary>
        /// <value>The source files.</value>
        BindingList<string> SourceFile { get; }

        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="file">The file.</param>
        void AddFile(string file);

        /// <summary>
        /// Removes the file.
        /// </summary>
        /// <param name="file">The file.</param>
        void RemoveFile(string file);

        /// <summary>
        /// Gets or sets the root path.
        /// </summary>
        /// <value>The root path.</value>
        string RootPath { get; set; }

        /// <summary>
        /// Gets or sets the coordinate system to use if none found in the source file.
        /// </summary>
        /// <value>The coordinate system.</value>
        string CoordinateSystem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [generate spatial data sources].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [generate spatial data sources]; otherwise, <c>false</c>.
        /// </value>
        bool GenerateSpatialDataSources { get; set; }

        /// <summary>
        /// Gets or sets the spatial data sources path.
        /// </summary>
        /// <value>The spatial data sources path.</value>
        string SpatialDataSourcesPath { get; set; }

        /// <summary>
        /// Gets or sets the spatial data sources folder.
        /// </summary>
        /// <value>The spatial data sources folder.</value>
        string SpatialDataSourcesFolder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [generate layers].
        /// </summary>
        /// <value><c>true</c> if [generate layers]; otherwise, <c>false</c>.</value>
        bool GenerateLayers { get; set; }

        /// <summary>
        /// Gets or sets the layers path.
        /// </summary>
        /// <value>The layers path.</value>
        string LayersPath { get; set; }

        /// <summary>
        /// Gets or sets the layers folder.
        /// </summary>
        /// <value>The layers folder.</value>
        string LayersFolder { get; set; }

        /// <summary>
        /// Gets or sets the generate maps.
        /// </summary>
        /// <value>The generate maps.</value>
        bool? GenerateMaps { get; set; }

        /// <summary>
        /// Gets or sets the maps path.
        /// </summary>
        /// <value>The maps path.</value>
        string MapsPath { get; set; }

        /// <summary>
        /// Gets or sets the maps folder.
        /// </summary>
        /// <value>The maps folder.</value>
        string MapsFolder { get; set; }

        /// <summary>
        /// Not supported by Maestro
        /// </summary>
        bool? GenerateSymbolLibraries { get; set; }

        /// <summary>
        /// Not supported by Maestro
        /// </summary>
        string SymbolLibrariesPath { get; set; }

        /// <summary>
        /// Not supported by Maestro
        /// </summary>
        string SymbolLibrariesFolder { get; set; }

        /// <summary>
        /// Gets or sets the resource id that were created as part of executing this load procedure
        /// </summary>
        /// <value>The resource id.</value>
        BindingList<string> ResourceId { get; set; }
    }

    /// <summary>
    /// Extension method class
    /// </summary>
    public static class BaseLoadProcedureExtensions
    {
        /// <summary>
        /// Adds a group of files to this load procedure
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="files"></param>
        public static void AddFiles(this IBaseLoadProcedure proc, IEnumerable<string> files)
        {
            Check.NotNull(proc, "proc");
            Check.NotNull(files, "files");
            foreach (var f in files)
            {
                proc.AddFile(f);
            }
        }
    }

    /// <summary>
    /// A DWF load procedure. Execution is supported with limitations
    /// </summary>
    public interface IDwfLoadProcedure : IBaseLoadProcedure
    {

    }

    /// <summary>
    /// A SDF load procedure. Execution is supported with limitations
    /// </summary>
    public interface ISdfLoadProcedure : IBaseLoadProcedure
    {
        /// <summary>
        /// Not supported by Maestro
        /// </summary>
        double Generalization { get; set; }

        /// <summary>
        /// Not supported by Maestro
        /// </summary>
        SdfKeyTreatmentType SdfKeyTreatment { get; set; }
    }

    /// <summary>
    /// A SHP load procedure. Execution is supported with limitations
    /// </summary>
    public interface IShpLoadProcedure : IBaseLoadProcedure
    {
        /// <summary>
        /// Not supported by Maestro
        /// </summary>
        double Generalization { get; set; }

        /// <summary>
        /// Not supported by Maestro
        /// </summary>
        bool ConvertToSdf { get; set; }
    }

    /// <summary>
    /// A SQLite load procedure. Execution is supported with limitations
    /// </summary>
    public interface ISqliteLoadProcedure : IBaseLoadProcedure
    {
        /// <summary>
        /// Not supported by Maestro
        /// </summary>
        double Generalization { get; set; }
    }
}
