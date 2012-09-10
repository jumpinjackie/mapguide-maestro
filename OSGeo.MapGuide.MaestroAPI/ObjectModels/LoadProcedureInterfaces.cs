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
    /// Defines how to handle duplicate SDF2 keys (not supported by Maestro)
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
        /// A Load Procedure for SDF 3.0 files
        /// </summary>
        Sdf,
        /// <summary>
        /// A Load Procedure for SHP files
        /// </summary>
        Shp,
        /// <summary>
        /// A Load Procedure for DWF files
        /// </summary>
        Dwf,
        /// <summary>
        /// A Load Procedure for Raster files (not supported by Maestro)
        /// </summary>
        Raster,
        /// <summary>
        /// A Load Procedure for DWG files (not supported by Maestro)
        /// </summary>
        Dwg,
        /// <summary>
        /// A Load Procedure for SQLite files
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
    /// Base type of all load procedures. All Load Procedures at the minimum require 
    /// the following information:
    /// 
    /// <list type="number">
    ///     <item>
    ///         <description>A list of source files.</description>
    ///     </item>
    ///     <item>
    ///         <description>The root path to load into</description>
    ///     </item>
    ///     <item>
    ///         <description>The folder where spatial data sources will be created [optional, but useless if not specified]</description>
    ///     </item>
    ///     <item>
    ///         <description>The folder where layers will be created [optional. dependent on #3]</description>
    ///     </item>
    /// </list>
    /// 
    /// Once initialized, load procedures can be executed via <see cref="M:OSGeo.MapGuide.MaestroAPI.MgServerConnectionBase.ExecuteLoadProcedure(OSGeo.MapGuide.ObjectModels.LoadProcedure.ILoadProcedure,OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressCallBack,System.Boolean)"/> method
    /// 
    /// Because Load Procedures are also resources, they can be saved into the library repository via the <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IResourceService.SaveResource(OSGeo.MapGuide.MaestroAPI.Resource.IResource)"/> method
    /// and retrieved from the repository via the <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IResourceService.GetResource(System.String)"/> method
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
        /// Gets or sets a value indicating whether to create a spatial data source for each source
        /// file. The spatial data sources will be created under the <see cref="SpatialDataSourcesFolder"/>
        /// under the <see cref="SpatialDataSourcesPath"/>
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
        /// Gets or sets a value indicating whether to create a layer for each spatial data source that
        /// is created. This will be created in the <see cref="LayersFolder"/> under the <see cref="LayersPath"/>
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
        /// Gets or sets the generate maps. Not supported by Maestro
        /// </summary>
        /// <value>The generate maps.</value>
        bool? GenerateMaps { get; set; }

        /// <summary>
        /// Gets or sets the maps path. Not supported by Maestro
        /// </summary>
        /// <value>The maps path.</value>
        string MapsPath { get; set; }

        /// <summary>
        /// Gets or sets the maps folder. Not supported by Maestro
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
            Check.NotNull(proc, "proc"); //NOXLATE
            Check.NotNull(files, "files"); //NOXLATE
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
    /// <remarks>
    /// The SDF Load Procedure has the following limitations when executed by Maestro
    /// <list type="bullet">
    /// <item><description>The input SDF files must be SDF3 files. Loading of SDF2 files is not supported</description></item>
    /// </list>
    /// </remarks>
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
    /// <remarks>
    /// The SHP Load Procedure has the following limitations when executed by Maestro
    /// <list type="bullet">
    /// <item><description>Generalization is not supported</description></item>
    /// <item><description>Conversion to SDF is not supported</description></item>
    /// </list>
    /// </remarks>
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
    /// <remarks>
    /// <para>SQLite load procedures can only be saved to a server whose site version is 2.2 or higher</para>
    /// <para>The SQLite Load Procedure has the following limitations when executed by Maestro</para>
    /// <list type="bullet">
    /// <item><description>Generalization is not supported</description></item>
    /// </list>
    /// </remarks>
    public interface ISqliteLoadProcedure : IBaseLoadProcedure
    {
        /// <summary>
        /// Not supported by Maestro
        /// </summary>
        double Generalization { get; set; }
    }
}
