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

using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels.LoadProcedure;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System.Collections.Specialized;

namespace OSGeo.MapGuide.MaestroAPI.Commands
{
    /// <summary>
    /// A command to execute Load Procedures
    /// </summary>
    internal interface IExecuteLoadProcedure : ICommand
    {
        /// <summary>
        /// Executes the specified load proc.
        /// </summary>
        /// <param name="loadProc">The load proc.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        string[] Execute(ILoadProcedure loadProc, OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressCallBack callback);
        /// <summary>
        /// Executes the specified resource ID.
        /// </summary>
        /// <param name="resourceID">The resource ID.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        string[] Execute(string resourceID, OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressCallBack callback);
        /// <summary>
        /// Gets or sets a value indicating whether [ignore unsupported features].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [ignore unsupported features]; otherwise, <c>false</c>.
        /// </value>
        bool IgnoreUnsupportedFeatures { get; set; }
    }

    internal class ExecuteLoadProcedure : IExecuteLoadProcedure
    {
        internal ExecuteLoadProcedure(IServerConnection conn)
        {
            this.Parent = conn;
        }

        public IServerConnection Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the resource ID of the load procedure
        /// </summary>
        public string ResourceID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether to ignore unsupported features. If true, an exception is thrown if unsupported
        /// features are encountered during execution
        /// </summary>
        public bool IgnoreUnsupportedFeatures
        {
            get;
            set;
        }

        /// <summary>
        /// Executes the specified load procedure.
        /// </summary>
        /// <param name="resourceID">The resource ID of the load procedure.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public string[] Execute(string resourceID, OSGeo.MapGuide.MaestroAPI.LengthyOperationProgressCallBack callback)
        {
            if (!ResourceIdentifier.Validate(this.ResourceID))
                throw new ArgumentException("Invalid resource id: " + this.ResourceID);

            if (ResourceIdentifier.GetResourceType(this.ResourceID) != ResourceTypes.LoadProcedure)
                throw new ArgumentException("Not a load procedure resource id: " + this.ResourceID);

            ILoadProcedure proc = (ILoadProcedure)this.Parent.ResourceService.GetResource(resourceID);
            return Execute(proc, callback);
        }

        /// <summary>
        /// Executes the specified load procedure. Only SDF and SHP load procedures are supported.
        /// Also note that the following load procedure features are ignored during execution:
        /// - Generalization of data
        /// - Conversion from SHP to SDF
        /// - SDF2 to SDF3 conversion
        /// - SDF3 duplicate key handling
        /// </summary>
        /// <param name="proc">The proc.</param>
        /// <param name="callback">The callback.</param>
        /// <returns>
        /// A list of resource IDs that were created from the execution of this load procedure
        /// </returns>
        public string[] Execute(ILoadProcedure proc, LengthyOperationProgressCallBack callback)
        {
            //TODO: Localize callback messages
            //TODO: Localize exception messages
            //TODO: This would currently overwrite everything. In reality, the load procedure has
            //a list of resource ids which are overwritable, anything not on the list is untouchable.
            //I presume if this list is empty, then everything is overwritten and the resource list
            //list is then assigned to the load procedure, which is then updated so that on subsequent runs,
            //only resources in the list are overwritten instead of everything.

            string[] resourcesCreatedOrUpdated = null;

            LengthyOperationProgressCallBack cb = callback;

            //Assign dummy callback if none specified
            if (cb == null)
                cb = delegate { };


            //bool loadProcedureUpdated = false;
            //bool updateGeneratedResourceIds = false;

            //TODO: SDF and SHP load procedures share lots of common logic. Merge the two 
            //once everything's all good.

            var type = proc.SubType.Type;
            if (type == LoadType.Dwg || type == LoadType.Raster)
                throw new NotSupportedException(Properties.Resources.UnsupportedLoadProcedureType);

            var sproc = (IBaseLoadProcedure)proc.SubType;

            bool firstExecute = true;
            if (type == LoadType.Shp)
            {
                var shpl = (IShpLoadProcedure)sproc;
                if (!this.IgnoreUnsupportedFeatures)
                {
                    //Anything less than 100% implies use of generalization
                    if (shpl.Generalization < 100.0)
                    {
                        throw new NotSupportedException(Properties.Resources.LPROC_GeneralizationNotSupported);
                    }
                    //Can't do this because we don't have a portable .net FDO/MG Feature Service
                    if (shpl.ConvertToSdf)
                    {
                        throw new NotSupportedException(Properties.Resources.LPROC_ConvertToSdf3NotSupported);
                    }
                }
                resourcesCreatedOrUpdated = ExecuteShpLoadProcedure(cb, shpl, ref firstExecute);
            }
            else
            {
                if (!this.IgnoreUnsupportedFeatures)
                {
                    CheckUnsupportedFeatures(sproc);
                }
                resourcesCreatedOrUpdated = ExecuteBaseProcedure(cb, sproc, ref firstExecute);
            }

            //Update the generated resources list if this is the first execution
            if (firstExecute)
            {
                sproc.ResourceId.Clear();
                foreach (var it in resourcesCreatedOrUpdated)
                {
                    sproc.ResourceId.Add(it);
                }
                this.Parent.ResourceService.SaveResourceAs(proc, this.ResourceID);
            }
            return resourcesCreatedOrUpdated;
        }

        private void CheckUnsupportedFeatures(IBaseLoadProcedure sproc)
        {
            if (sproc.Type == LoadType.Dwf)
            {
                if (Array.IndexOf(this.Parent.Capabilities.SupportedServices, (int)ServiceType.Drawing) < 0)
                {
                    throw new NotSupportedException(Properties.Resources.RequiredServiceNotSupported + ServiceType.Drawing.ToString());
                }
            }
        }

        private string[] ExecuteShpLoadProcedure(LengthyOperationProgressCallBack cb, IShpLoadProcedure shpl, ref bool firstExecution)
        {
            List<string> resCreatedOrUpdated = new List<string>();

            var shpFiles = shpl.SourceFile;
            int pcPerFile = (int)(100 / shpFiles.Count);
            int current = 0;

            string root = shpl.RootPath;
            if (!root.EndsWith("/"))
                root += "/";

            string sdp = shpl.SpatialDataSourcesPath;
            string lp = shpl.LayersPath;

            if (!string.IsNullOrEmpty(sdp))
            {
                if (!sdp.EndsWith("/"))
                    sdp += "/";
            }

            if (!string.IsNullOrEmpty(lp))
            {
                if (!lp.EndsWith("/"))
                    lp += "/";
            }

            string fsRoot = (string.IsNullOrEmpty(sdp) ? root : sdp) + shpl.SpatialDataSourcesFolder;
            string layerRoot = (string.IsNullOrEmpty(lp) ? root : lp) + shpl.LayersFolder;

            if (!fsRoot.EndsWith("/"))
                fsRoot += "/";
            if (!layerRoot.EndsWith("/"))
                layerRoot += "/";

            List<string> resToUpdate = new List<string>();
            if (shpl.ResourceId != null)
            {
                resToUpdate.AddRange(shpl.ResourceId);
                firstExecution = false;
            }
            else
            {
                firstExecution = true;
            }

            Dictionary<string, List<string>> extraFiles = new Dictionary<string, List<string>>();
            //Unlike SDF, a SHP file actually consists of multiple files
            foreach (string shp in shpFiles)
            {
                if (!extraFiles.ContainsKey(shp))
                    extraFiles[shp] = new List<string>();
                //we want to preserve casing for everything before the extension
                string prefix = shp.Substring(0, shp.LastIndexOf(".") + 1);
                extraFiles[shp].Add(prefix + "shx");
                extraFiles[shp].Add(prefix + "dbf");
                extraFiles[shp].Add(prefix + "idx");
                extraFiles[shp].Add(prefix + "prj");
                extraFiles[shp].Add(prefix + "cpg");

                //TODO: Are we missing anything else?
            }

            foreach (string file in shpFiles)
            {
                bool success = false;
                if (System.IO.File.Exists(file))
                {
                    string resName = System.IO.Path.GetFileNameWithoutExtension(file);
                    string dataName = System.IO.Path.GetFileName(file);

                    string fsId = fsRoot + resName + ".FeatureSource";
                    string lyrId = layerRoot + resName + ".LayerDefinition";

                    if (shpl.GenerateSpatialDataSources)
                    {
                        //Skip only if we have an update list and this resource id is not in it
                        bool skip = (resToUpdate.Count > 0 && !resToUpdate.Contains(fsId));
                        if (!skip)
                        {
                            //Process is as follows:
                            //
                            // 1. Create and save feature source document.
                            // 2. Upload sdf file as resource data for this document.
                            // 3. Test the connection, it should check out.
                            // 4. If no spatial contexts are detected, assign a default one from the load procedure and save the modified feature source.

                            //Step 1: Create feature source document
                            var conp = new NameValueCollection();
                            conp["DefaultFileLocation"] = "%MG_DATA_FILE_PATH%" + dataName;
                            var fs = ObjectFactory.CreateFeatureSource(this.Parent, "OSGeo.SHP", conp);
                            fs.ResourceID = fsId;

                            this.Parent.ResourceService.SaveResource(fs);
                            resCreatedOrUpdated.Add(fsId);
                            cb(this, new LengthyOperationProgressArgs("Created: " + fsId, current));

                            //TODO: When the infrastructure is available to us (ie. A portable .net FDO/MG Feature Service API wrapper)
                            //Maybe then we can actually implement the generalization and conversion properties. Until then, we skip
                            //these options

                            //Step 2: Load resource data for document
                            this.Parent.ResourceService.SetResourceData(fsId, dataName, ResourceDataType.File, System.IO.File.OpenRead(file));

                            cb(this, new LengthyOperationProgressArgs("Loaded: " + file, current));

                            //Load supplementary files
                            foreach (string extraFile in extraFiles[file])
                            {
                                string dn = System.IO.Path.GetFileName(extraFile);
                                if (System.IO.File.Exists(extraFile))
                                {
                                    this.Parent.ResourceService.SetResourceData(fsId, dn, ResourceDataType.File, System.IO.File.OpenRead(extraFile));
                                    cb(this, new LengthyOperationProgressArgs("Loaded: " + extraFile, current));
                                }
                            }

                            //Step 3: Test to make sure we're all good so far
                            string result = this.Parent.FeatureService.TestConnection(fsId);

                            //LocalNativeConnection returns this string, so I'm assuming this is the "success" result
                            if (result == "No errors")
                            {
                                //Step 4: Test to see if default cs needs to be specified
                                FdoSpatialContextList spatialContexts = this.Parent.FeatureService.GetSpatialContextInfo(fsId, false);
                                if (spatialContexts.SpatialContext.Count == 0 && !string.IsNullOrEmpty(shpl.CoordinateSystem))
                                {
                                    //Register the default CS from the load procedure
                                    fs.AddSpatialContextOverride(new OSGeo.MapGuide.ObjectModels.FeatureSource_1_0_0.SpatialContextType()
                                    {
                                        Name = "Default",
                                        CoordinateSystem = shpl.CoordinateSystem
                                    });

                                    //Update this feature source
                                    this.Parent.ResourceService.SaveResource(fs);

                                    cb(this, new LengthyOperationProgressArgs("Set default spatial context for: " + fsId, current));
                                }
                            }
                        }
                    }

                    if (shpl.GenerateLayers)
                    {
                        //Skip only if we have an update list and this resource id is not in it
                        bool skip = (resToUpdate.Count > 0 && !resToUpdate.Contains(lyrId));
                        if (!skip)
                        {
                            //NOTE: Because we are working against 1.0.0 object types this will always create 1.0.0 Layer Definition
                            //resources

                            //Process is as follows
                            //
                            // 1. Describe the schema of the feature source
                            // 2. If it contains at least one feature class, create a layer definition
                            // 3. Set the following layer definition properties:
                            //    - Feature Source: the feature source id
                            //    - Feature Class: the first feature class in the schema
                            //    - Geometry: the first geometry property in the first feature class
                            // 4. Infer the supported geometry types for this feature class. Toggle supported styles accordingly.

                            //Step 1: Describe the schema
                            FeatureSourceDescription desc = this.Parent.FeatureService.DescribeFeatureSource(fsId);

                            //Step 2: Find the first feature class with a geometry property
                            ClassDefinition clsDef = null;
                            GeometricPropertyDefinition geom = null;

                            bool done = false;

                            foreach (ClassDefinition cls in desc.AllClasses)
                            {
                                if (done) break;

                                foreach (PropertyDefinition prop in cls.Properties)
                                {
                                    if (done) break;

                                    if (prop.Type == OSGeo.MapGuide.MaestroAPI.Schema.PropertyDefinitionType.Geometry)
                                    {
                                        clsDef = cls;
                                        geom = (GeometricPropertyDefinition)prop;
                                        done = true;
                                    }
                                }
                            }

                            if (clsDef != null && geom != null)
                            {
                                var ld = ObjectFactory.CreateDefaultLayer(this.Parent, LayerType.Vector, new Version(1, 0, 0));

                                //Step 3: Assign default properties
                                ld.ResourceID = lyrId;
                                var vld = ld.SubLayer as IVectorLayerDefinition;
                                vld.ResourceId = fsId;
                                vld.FeatureName = clsDef.QualifiedName;
                                vld.Geometry = geom.Name;

                                //Step 4: Infer geometry storage support and remove unsupported styles
                                var scale = vld.GetScaleRangeAt(0);
                                var geomTypes = geom.GetIndividualGeometricTypes();
                                var remove = new List<string>();
                                if (Array.IndexOf(geomTypes, FeatureGeometricType.Point) < 0)
                                {
                                    remove.Add(FeatureGeometricType.Point.ToString().ToLower());
                                }
                                if (Array.IndexOf(geomTypes, FeatureGeometricType.Curve) < 0)
                                {
                                    remove.Add(FeatureGeometricType.Curve.ToString().ToLower());
                                }
                                if (Array.IndexOf(geomTypes, FeatureGeometricType.Surface) < 0)
                                {
                                    remove.Add(FeatureGeometricType.Surface.ToString().ToLower());
                                }

                                scale.RemoveStyles(remove);

                                this.Parent.ResourceService.SaveResource(ld);
                                resCreatedOrUpdated.Add(lyrId);
                                cb(this, new LengthyOperationProgressArgs("Created: " + lyrId, current));
                            }
                        }
                    }
                    success = true;
                }
                else
                {
                    cb(this, new LengthyOperationProgressArgs("File not found: " + file, current));
                }

                //This file is now fully processed, so increment progress
                current += pcPerFile;

                if (success)
                {
                    cb(this, new LengthyOperationProgressArgs("Success: " + file, current));
                }
            }

            return resCreatedOrUpdated.ToArray();
        }

        private string[] ExecuteBaseProcedure(LengthyOperationProgressCallBack cb, IBaseLoadProcedure proc, ref bool firstExecution)
        {
            List<string> resCreatedOrUpdated = new List<string>();

            var files = proc.SourceFile;
            int pcPerFile = (int)(100 / files.Count);
            int current = 0;

            string root = proc.RootPath;
            if (!root.EndsWith("/"))
                root += "/";

            string sdp = proc.SpatialDataSourcesPath;
            string lp = proc.LayersPath;

            if (!string.IsNullOrEmpty(sdp))
            {
                if (!sdp.EndsWith("/"))
                    sdp += "/";
            }

            if (!string.IsNullOrEmpty(lp))
            {
                if (!lp.EndsWith("/"))
                    lp += "/";
            }

            string fsRoot = (string.IsNullOrEmpty(sdp) ? root : sdp) + proc.SpatialDataSourcesFolder;
            string layerRoot = (string.IsNullOrEmpty(lp) ? root : lp) + proc.LayersFolder;

            if (!fsRoot.EndsWith("/"))
                fsRoot += "/";
            if (!layerRoot.EndsWith("/"))
                layerRoot += "/";

            List<string> resToUpdate = new List<string>();
            if (proc.ResourceId != null && proc.ResourceId.Count > 0)
            {
                resToUpdate.AddRange(proc.ResourceId);
                firstExecution = false;
            }
            else
            {
                firstExecution = true;
            }

            foreach (string file in files)
            {
                bool success = false;
                if (System.IO.File.Exists(file))
                {
                    //GOTCHA: We are assuming these SDF files are not SDF2 files. This is
                    //because there is no multi-platform solution to convert SDF2 files to SDF3

                    string resName = System.IO.Path.GetFileNameWithoutExtension(file);
                    string dataName = System.IO.Path.GetFileName(file);
                    string dsId = fsRoot + resName + ".DrawingSource";
                    string fsId = fsRoot + resName + ".FeatureSource";
                    string lyrId = layerRoot + resName + ".LayerDefinition";

                    if (proc.GenerateSpatialDataSources)
                    {
                        //Skip only if we have an update list and this resource id is not in it
                        bool skip = (resToUpdate.Count > 0 && !resToUpdate.Contains(fsId));
                        if (!skip)
                        {
                            if (proc.Type == LoadType.Dwf)
                            {
                                //Process is as follows:
                                //
                                // 1. Create and save drawing source document.
                                // 2. Upload dwf file as resource data for this document.

                                //Step 1: Create and save drawing source document.
                                var ds = ObjectFactory.CreateDrawingSource(this.Parent);
                                ds.SourceName = dataName;
                                ds.CoordinateSpace = proc.CoordinateSystem;
                                ds.ResourceID = dsId;
                                this.Parent.ResourceService.SaveResource(ds);
                                resCreatedOrUpdated.Add(dsId);
                                cb(this, new LengthyOperationProgressArgs("Created: " + dsId, current));

                                //Step 2: Load resource data for document
                                this.Parent.ResourceService.SetResourceData(dsId, dataName, ResourceDataType.File, System.IO.File.OpenRead(file));
                                cb(this, new LengthyOperationProgressArgs("Loaded: " + file, current));

                                var dwSvc = (IDrawingService)Parent.GetService((int)ServiceType.Drawing);
                                var list = dwSvc.EnumerateDrawingSections(dsId);
                                if (list.Section.Count > 0)
                                {
                                    foreach(var sect in list.Section)
                                    {
                                        //TODO: Extract CS and extent info of each sheet and reg them into the document content
                                        //DWF files are ZIP files, so we have the ability to peek at its internals
                                        var sht = ds.CreateSheet(sect.Name, 0, 0, 0, 0);
                                        ds.AddSheet(sht);
                                    }
                                    this.Parent.ResourceService.SaveResource(ds);
                                }
                            }
                            else
                            {
                                //Process is as follows:
                                //
                                // 1. Create and save feature source document.
                                // 2. Upload sdf file as resource data for this document.
                                // 3. Test the connection, it should check out.
                                // 4. If no spatial contexts are detected, assign a default one from the load procedure and save the modified feature source.

                                //Step 1: Create feature source document
                                string provider = "OSGeo.SDF";

                                switch (proc.Type)
                                {
                                    case LoadType.Sqlite:
                                        provider = "OSGeo.SQLite";
                                        break;
                                }
                                var conp = new NameValueCollection();
                                conp["File"] = "%MG_DATA_FILE_PATH%" + dataName;
                                var fs = ObjectFactory.CreateFeatureSource(this.Parent, provider, conp);
                                fs.ResourceID = fsId;

                                this.Parent.ResourceService.SaveResource(fs);
                                resCreatedOrUpdated.Add(fsId);
                                cb(this, new LengthyOperationProgressArgs("Created: " + fsId, current));

                                //TODO: When the infrastructure is available to us (ie. A portable .net FDO/MG Feature Service API wrapper)
                                //Maybe then we can actually implement the generalization and duplicate record handling properties. Until then, we skip
                                //these options

                                //Step 2: Load resource data for document
                                this.Parent.ResourceService.SetResourceData(fsId, dataName, ResourceDataType.File, System.IO.File.OpenRead(file));

                                cb(this, new LengthyOperationProgressArgs("Loaded: " + file, current));

                                //Step 3: Test to make sure we're all good so far
                                string result = this.Parent.FeatureService.TestConnection(fsId);

                                //LocalNativeConnection returns this string, so I'm assuming this is the "success" result
                                if (result == "No errors")
                                {
                                    //Step 4: Test to see if default cs needs to be specified
                                    FdoSpatialContextList spatialContexts = this.Parent.FeatureService.GetSpatialContextInfo(fsId, false);
                                    if (spatialContexts.SpatialContext.Count == 0 && !string.IsNullOrEmpty(proc.CoordinateSystem))
                                    {
                                        //Register the default CS from the load procedure
                                        fs.AddSpatialContextOverride(new OSGeo.MapGuide.ObjectModels.FeatureSource_1_0_0.SpatialContextType()
                                        {
                                            Name = "Default",
                                            CoordinateSystem = proc.CoordinateSystem
                                        });

                                        //Update this feature source
                                        this.Parent.ResourceService.SaveResource(fs);

                                        cb(this, new LengthyOperationProgressArgs("Set default spatial context for: " + fsId, current));
                                    }
                                }
                            }
                        }
                    }

                    if (proc.GenerateLayers)
                    {
                        //Skip only if we have an update list and this resource id is not in it
                        bool skip = (resToUpdate.Count > 0 && !resToUpdate.Contains(lyrId));
                        if (!skip)
                        {
                            if (proc.Type == LoadType.Dwf)
                            {
                                //Process is as follows
                                //
                                // 1. Enumerate the sheets on the drawing source
                                // 2. Set the referenced sheet to the first known sheet

                                var dwSvc = (IDrawingService)Parent.GetService((int)ServiceType.Drawing);
                                var list = dwSvc.EnumerateDrawingSections(dsId);
                                if (list.Section.Count > 0)
                                {
                                    //Create drawing layer
                                    var ld = ObjectFactory.CreateDefaultLayer(this.Parent, LayerType.Drawing, new Version(1, 0, 0));
                                    var dl = ld.SubLayer as IDrawingLayerDefinition;
                                    dl.ResourceId = dsId;
                                    //Use the first one
                                    dl.Sheet = list.Section[0].Name;

                                    ld.ResourceID = lyrId;

                                    this.Parent.ResourceService.SaveResource(ld);
                                    resCreatedOrUpdated.Add(lyrId);
                                    cb(this, new LengthyOperationProgressArgs("Created: " + lyrId, current));
                                }
                            }
                            else
                            {
                                //NOTE: Because we are working against 1.0.0 object types this will always create 1.0.0 Layer Definition
                                //resources

                                //Process is as follows
                                //
                                // 1. Describe the schema of the feature source
                                // 2. If it contains at least one feature class, create a layer definition
                                // 3. Set the following layer definition properties:
                                //    - Feature Source: the feature source id
                                //    - Feature Class: the first feature class in the schema
                                //    - Geometry: the first geometry property in the first feature class
                                // 4. Infer the supported geometry types for this feature class. Toggle supported styles accordingly.

                                //Step 1: Describe the schema
                                FeatureSourceDescription desc = this.Parent.FeatureService.DescribeFeatureSource(fsId);

                                if (desc.HasClasses())
                                {
                                    //Step 2: Find the first feature class with a geometry property
                                    ClassDefinition clsDef = null;
                                    GeometricPropertyDefinition geom = null;

                                    bool done = false;

                                    foreach (ClassDefinition cls in desc.AllClasses)
                                    {
                                        if (done) break;

                                        foreach (PropertyDefinition prop in cls.Properties)
                                        {
                                            if (done) break;

                                            if (prop.Type == OSGeo.MapGuide.MaestroAPI.Schema.PropertyDefinitionType.Geometry)
                                            {
                                                clsDef = cls;
                                                geom = (GeometricPropertyDefinition)prop;
                                                done = true;
                                            }
                                        }
                                    }

                                    if (clsDef != null && geom != null)
                                    {
                                        var ld = ObjectFactory.CreateDefaultLayer(this.Parent, LayerType.Vector, new Version(1, 0, 0));

                                        //Step 3: Assign default properties
                                        ld.ResourceID = lyrId;
                                        var vld = ld.SubLayer as IVectorLayerDefinition;
                                        vld.ResourceId = fsId;
                                        vld.FeatureName = clsDef.QualifiedName;
                                        vld.Geometry = geom.Name;

                                        //Step 4: Infer geometry storage support and remove unsupported styles
                                        var geomTypes = geom.GetIndividualGeometricTypes();
                                        var scale = vld.GetScaleRangeAt(0);

                                        var remove = new List<string>();
                                        if (Array.IndexOf(geomTypes, FeatureGeometricType.Point) < 0)
                                        {
                                            remove.Add(FeatureGeometricType.Point.ToString().ToLower());
                                        }
                                        if (Array.IndexOf(geomTypes, FeatureGeometricType.Curve) < 0)
                                        {
                                            remove.Add(FeatureGeometricType.Curve.ToString().ToLower());
                                        }
                                        if (Array.IndexOf(geomTypes, FeatureGeometricType.Surface) < 0)
                                        {
                                            remove.Add(FeatureGeometricType.Surface.ToString().ToLower());
                                        }

                                        scale.RemoveStyles(remove);

                                        this.Parent.ResourceService.SaveResource(ld);
                                        resCreatedOrUpdated.Add(lyrId);
                                        cb(this, new LengthyOperationProgressArgs("Created: " + lyrId, current));
                                    }
                                }
                            }
                        }
                    }
                    success = true;
                }
                else
                {
                    cb(this, new LengthyOperationProgressArgs("File not found: " + file, current));
                }

                //This file is now fully processed, so increment progress
                current += pcPerFile;

                if (success)
                {
                    cb(this, new LengthyOperationProgressArgs("File processed: " + file, current));
                }
            }
            return resCreatedOrUpdated.ToArray();
        }
    }
}
