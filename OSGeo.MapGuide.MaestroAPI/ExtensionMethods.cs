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

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Generates the session resource id.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="resType">Type of the res.</param>
        /// <returns></returns>
        public static string GenerateSessionResourceId(this IServerConnection conn, string resType)
        {
            Guid id = Guid.NewGuid();
            return conn.GenerateSessionResourceId(id.ToString(), resType);
        }

        /// <summary>
        /// Generates the session resource id.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="name">The name.</param>
        /// <param name="resType">Type of the res.</param>
        /// <returns></returns>
        public static string GenerateSessionResourceId(this IServerConnection conn, string name, string resType)
        {
            return "Session:" + conn.SessionID + "//" + name + "." + resType; //NOXLATE
        }

        /// <summary>
        /// Gets the total number of features in the feature class
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="featureSourceId"></param>
        /// <param name="featureClass"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static int GetFeatureCount(this IServerConnection conn, string featureSourceId, string featureClass, string filter)
        {
            //Implementation ported from schemareport/displayschemafunctions.php

            int total = -1;
            var fs = (IFeatureSource)conn.ResourceService.GetResource(featureSourceId);
            var caps = conn.FeatureService.GetProviderCapabilities(fs.Provider);
            bool canCount = (caps.Expression.SupportedFunctions.Any(func => func.Name.ToLower() == "count"));
            bool gotCount = false;
            var clsDef = conn.FeatureService.GetClassDefinition(featureSourceId, featureClass);
            if (canCount)
            {
                if (clsDef.IdentityProperties.Count > 0)
                {
                    const string TotalCount = nameof(TotalCount);
                    var aggFuncs = new System.Collections.Specialized.NameValueCollection();
                    aggFuncs.Add(TotalCount, $"COUNT({clsDef.IdentityProperties[0].Name})");

                    try
                    {
                        using (var ar = conn.FeatureService.AggregateQueryFeatureSource(featureSourceId, featureClass, filter, aggFuncs))
                        {
                            if (ar.ReadNext())
                            {
                                if (ar.IsNull(TotalCount))
                                {
                                    total = 0;
                                    gotCount = true;
                                }
                                else
                                {
                                    var val = ar[TotalCount];
                                    total = Convert.ToInt32(val);
                                    gotCount = true;
                                }
                            }
                        }
                    }
                    catch (Exception) //Some providers like OGR can lie
                    {
                        gotCount = false;
                    }
                }
            }

            if (!gotCount)
            {
                string propName = null;
                if (clsDef.IdentityProperties.Count > 0)
                {
                    propName = clsDef.IdentityProperties[0].Name;
                }
                else
                {
                    propName = clsDef.Properties.OfType<DataPropertyDefinition>().First().Name;
                }
                //Raw spin a feature reader. To reduce transmission overhead, only request one output property
                try
                {
                    using (var fr = conn.FeatureService.QueryFeatureSource(featureSourceId, featureClass, filter, new[] { propName }))
                    {
                        total = 0;
                        while (fr.ReadNext())
                            total++;
                        fr.Close();
                    }
                }
                catch (Exception)
                {
                    total = -1; //Can't count or raw spin???
                }
            }
            return total;
        }
    }
}

namespace System.Xml
{
    /// <summary>
    /// XML Document extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Gets the XML string of this document
        /// </summary>
        /// <param name="doc">The XML document</param>
        /// <param name="indent">If true, will indent the XML content, false otherwise</param>
        /// <returns>The XML string</returns>
        public static string ToXmlString(this XmlDocument doc, bool indent = false)
        {
            Check.ArgumentNotNull(doc, nameof(doc));
            var xs = new XmlWriterSettings();
            xs.Indent = indent;
            using (var sw = new StringWriter())
            using (var xtw = XmlWriter.Create(sw, xs))
            {
                doc.WriteTo(xtw);
                xtw.Flush();
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}

namespace OSGeo.MapGuide.ObjectModels
{
    /// <summary>
    /// Extension Methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Copies the resource data to the specified resource. Both resources are assumed to originate from the same given connection
        /// </summary>
        /// <remarks>
        /// Avoid using this method if you are copying a IFeatureSource with MG_USER_CREDENTIALS resource data, as MapGuide will automatically return
        /// the decrypted username for MG_USER_CREDENTIALS, rendering the resource data invalid for the target resource. Instead use the
        /// <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IResourceService.CopyResource"/> method, which will copy the resource and its resource
        /// data and keep any MG_USER_CREDENTIALS items intact
        /// </remarks>
        /// <param name="source">The source.</param>
        /// <param name="conn">The server connection</param>
        /// <param name="target">The target.</param>
        public static void CopyResourceDataTo(this IResource source, IServerConnection conn, IResource target)
        {
            Check.ArgumentNotNull(source, nameof(source));
            Check.ArgumentNotNull(conn, nameof(conn));
            Check.ArgumentNotNull(target, nameof(target));

            var resData = conn.ResourceService.EnumerateResourceData(source.ResourceID);
            foreach (var res in resData.ResourceData)
            {
                var data = conn.ResourceService.GetResourceData(source.ResourceID, res.Name);
                bool bDispose = false;
                if (!data.CanSeek)
                {
                    var ms = MemoryStreamPool.GetStream();
                    Utility.CopyStream(data, ms);
                    data = ms;
                    bDispose = true;
                }
                conn.ResourceService.SetResourceData(target.ResourceID, res.Name, res.Type, data);

                if (bDispose)
                {
                    data.Dispose();
                }
            }
        }

        /// <summary>
        /// Copies the resource data to the specified resource. Both resources are assumed to originate from the same given connection
        /// </summary>
        /// <remarks>
        /// Avoid using this method if you are copying a IFeatureSource with MG_USER_CREDENTIALS resource data, as MapGuide will automatically return
        /// the decrypted username for MG_USER_CREDENTIALS, rendering the resource data invalid for the target resource. Instead use the
        /// <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IResourceService.CopyResource"/> method, which will copy the resource and its resource
        /// data and keep any MG_USER_CREDENTIALS items intact
        /// </remarks>
        /// <param name="source">The source.</param>
        /// <param name="conn">The server connection</param>
        /// <param name="targetID">The target ID.</param>
        public static void CopyResourceDataTo(this IResource source, IServerConnection conn, string targetID)
        {
            Check.ArgumentNotNull(source, nameof(source));
            Check.ArgumentNotEmpty(targetID, nameof(targetID));

            var resData = conn.ResourceService.EnumerateResourceData(source.ResourceID);
            foreach (var res in resData.ResourceData)
            {
                var data = conn.ResourceService.GetResourceData(source.ResourceID, res.Name);
                bool bDispose = false;
                if (!data.CanSeek)
                {
                    var ms = MemoryStreamPool.GetStream();
                    Utility.CopyStream(data, ms);
                    data = ms;
                    bDispose = true;
                }
                conn.ResourceService.SetResourceData(targetID, res.Name, res.Type, data);
                if (bDispose)
                {
                    data.Dispose();
                }
            }
        }
    }

    namespace FeatureSource
    {
        /// <summary>
        /// Extension methods
        /// </summary>
        public static class ExtensionMethods
        {
            /// <summary>
            /// Gets the configuration document content
            /// </summary>
            /// <param name="fs">The Feature Source</param>
            /// <param name="conn">The Server Connection</param>
            /// <returns>The configuration document XML content</returns>
            public static string GetConfigurationContent(this IFeatureSource fs, IServerConnection conn)
            {
                Check.ArgumentNotNull(fs, nameof(fs));
                Check.ArgumentNotNull(conn, nameof(conn));
                if (string.IsNullOrEmpty(fs.ConfigurationDocument))
                    return string.Empty;

                var content = conn.ResourceService.GetResourceData(fs.ResourceID, fs.ConfigurationDocument);
                if (content != null)
                {
                    using (var sr = new StreamReader(content))
                    {
                        return sr.ReadToEnd();
                    }
                }
                return string.Empty;
            }

            /// <summary>
            /// Sets the configuration document content
            /// </summary>
            /// <param name="fs">The feature source</param>
            /// <param name="conn">The server connection</param>
            /// <param name="xmlContent">The confiugration document XML content</param>
            public static void SetConfigurationContent(this IFeatureSource fs, IServerConnection conn, string xmlContent)
            {
                Check.ArgumentNotNull(fs, nameof(fs));
                Check.ArgumentNotNull(conn, nameof(conn));
                if (string.IsNullOrEmpty(fs.ConfigurationDocument))
                    fs.ConfigurationDocument = "config.xml"; //NOXLATE

                if (string.IsNullOrEmpty(xmlContent))
                {
                    bool hasResourceData = false;
                    var resDataList = conn.ResourceService.EnumerateResourceData(fs.ResourceID).ResourceData;
                    foreach (var resData in resDataList)
                    {
                        if (resData.Name == fs.ConfigurationDocument)
                        {
                            hasResourceData = true;
                            break;
                        }
                    }

                    if (hasResourceData)
                        conn.ResourceService.DeleteResourceData(fs.ResourceID, fs.ConfigurationDocument);
                }
                else
                {
                    using (var ms = MemoryStreamPool.GetStream("SetConfigurationContent", Encoding.UTF8.GetBytes(xmlContent))) //NOXLATE
                    {
                        conn.ResourceService.SetResourceData(fs.ResourceID, fs.ConfigurationDocument, ResourceDataType.Stream, ms);
                    }
                }
            }

            /// <summary>
            /// Convenience methods to get the identity properties of a given feature class (name)
            /// </summary>
            /// <param name="fs">The feature source</param>
            /// <param name="conn">The server connection</param>
            /// <param name="className">Name of the class.</param>
            /// <returns>The array of identity properties</returns>
            public static string[] GetIdentityProperties(this IFeatureSource fs, IServerConnection conn, string className)
            {
                Check.ArgumentNotNull(fs, nameof(fs));
                Check.ArgumentNotNull(conn, nameof(conn));
                try
                {
                    return conn.FeatureService.GetIdentityProperties(fs.ResourceID, className);
                }
                catch (Exception ex)
                {
                    //MgClassNotFoundException is thrown for classes w/ no identity properties
                    //when the correct server response should be an empty array
                    if (ex.Message.IndexOf("MgClassNotFoundException") >= 0) //NOXLATE
                    {
                        return new string[0];
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }

    namespace DrawingSource
    {
        /// <summary>
        /// Extension methods
        /// </summary>
        public static class ExtensionMethods
        {
            /// <summary>
            /// Regenerates the sheet list in this drawing source.
            /// </summary>
            /// <param name="source">The drawing source</param>
            /// <param name="conn">The server connection</param>
            /// <returns>True if sheets were regenerated. False otherwise</returns>
            public static bool RegenerateSheetList(this IDrawingSource source, IServerConnection conn)
            {
                Check.ArgumentNotNull(source, nameof(source));
                Check.ArgumentNotNull(conn, nameof(conn));
                Check.ArgumentNotEmpty(source.ResourceID, $"{nameof(source)}.{nameof(source.ResourceID)}");

                IDrawingService dwSvc = (IDrawingService)conn.GetService((int)ServiceType.Drawing);
                var sheets = dwSvc.EnumerateDrawingSections(source.ResourceID);
                bool bRegen = sheets.Section.Count > 0;
                source.RemoveAllSheets();
                if (bRegen)
                {
                    foreach (var sht in sheets.Section)
                    {
                        source.AddSheet(source.CreateSheet(sht.Name, 0, 0, 0, 0));
                    }
                }
                return bRegen;
            }

            /// <summary>
            /// Updates the extents of all sheets based on their respective AutoCAD Viewport Data in the embedded PIA resource
            /// </summary>
            /// <param name="source">The drawing source</param>
            /// <param name="conn">The server connection</param>
            public static void UpdateExtents(this IDrawingSource source, IServerConnection conn)
            {
                Check.ArgumentNotNull(source, nameof(source));
                Check.ArgumentNotNull(conn, nameof(conn));
                Check.ArgumentNotEmpty(source.ResourceID, $"{nameof(source)}.{nameof(source.ResourceID)}");

                //Need drawing service
                if (Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Drawing) < 0)
                    throw new NotSupportedException(string.Format(OSGeo.MapGuide.MaestroAPI.Strings.ERR_SERVICE_NOT_SUPPORTED, ServiceType.Drawing.ToString()));

                var drawSvc = (IDrawingService)conn.GetService((int)ServiceType.Drawing);

                foreach (var sht in source.Sheet)
                {
                    var list = drawSvc.EnumerateDrawingSectionResources(source.ResourceID, sht.Name);
                    foreach (var res in list.SectionResource)
                    {
                        if (res.Role == "AutoCAD Viewport Data") //NOXLATE
                        {
                            using (var stream = drawSvc.GetSectionResource(source.ResourceID, res.Href))
                            {
                                //This is text content
                                using (var sr = new StreamReader(stream))
                                {
                                    try
                                    {
                                        string content = sr.ReadToEnd();

                                        //Viewport parameters are:
                                        //
                                        // llx
                                        // lly
                                        // urx
                                        // ury
                                        //
                                        //A the first space after each number of each parameter marks the end of that value
                                        string sllx, slly, surx, sury = "";
                                        if (content.StartsWith("PIAFILEVERSION_3.0,json"))
                                        {
                                            var  pia3= Newtonsoft.Json.Linq.JObject.Parse(content.Replace("PIAFILEVERSION_3.0,json",""));

                                            sllx = pia3["data"]["viewport"]["llx"].ToString();
                                            slly = pia3["data"]["viewport"]["lly"].ToString();
                                            surx = pia3["data"]["viewport"]["urx"].ToString();
                                            sury = pia3["data"]["viewport"]["ury"].ToString();

                                        }
                                        else {

                                            // 4 - length of "llx="
                                            int idx = content.IndexOf("llx") + 4;  //NOXLATE
                                            sllx = content.Substring(idx, content.IndexOf(" ", idx) - idx); //NOXLATE
                                                                                                                   // 4 - length of "lly="
                                            idx = content.IndexOf("lly") + 4; //NOXLATE
                                            slly = content.Substring(idx, content.IndexOf(" ", idx) - idx); //NOXLATE
                                                                                                                   // 4 - length of "urx="
                                            idx = content.IndexOf("urx") + 4; //NOXLATE
                                            surx = content.Substring(idx, content.IndexOf(" ", idx) - idx); //NOXLATE
                                                                                                                   // 4 - length of "ury="
                                            idx = content.IndexOf("ury") + 4; //NOXLATE
                                            sury = content.Substring(idx, content.IndexOf(" ", idx) - idx); //NOXLATE
                                        }
                                        
                                        //Update extents
                                        sht.Extent = ObjectFactory.CreateEnvelope(
                                            Convert.ToDouble(sllx),
                                            Convert.ToDouble(slly),
                                            Convert.ToDouble(surx),
                                            Convert.ToDouble(sury));
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    namespace LayerDefinition
    {
        /// <summary>
        /// Extension methods
        /// </summary>
        public static class ExtensionMethods
        {
            private static IFdoSpatialContext FindSpatialContext(FdoSpatialContextList spatialContexts, string scName)
            {
                foreach (IFdoSpatialContext sc in spatialContexts.SpatialContext)
                {
                    if (sc.Name == scName)
                        return sc;
                }
                return null;
            }

            /// <summary>
            /// Returns the associated spatial context for this Layer Definition
            /// </summary>
            /// <param name="layer">The layer definition</param>
            /// <param name="conn">The server connection</param>
            /// <returns>The associated spatial context</returns>
            public static IFdoSpatialContext GetSpatialContext(this ILayerDefinition layer, IServerConnection conn)
            {
                Check.ArgumentNotNull(layer, nameof(layer));
                Check.ArgumentNotNull(conn, nameof(conn));
                var ltype = layer.SubLayer.LayerType;
                if (ltype == LayerType.Vector ||
                    ltype == LayerType.Raster)
                {
                    var sContexts = conn.FeatureService.GetSpatialContextInfo(layer.SubLayer.ResourceId, false);
                    if (ltype == LayerType.Vector)
                    {
                        IVectorLayerDefinition vl = (IVectorLayerDefinition)layer.SubLayer;
                        var clsDef = conn.FeatureService.GetClassDefinition(vl.ResourceId, vl.FeatureName);
                        var geom = clsDef.FindProperty(vl.Geometry) as GeometricPropertyDefinition;
                        if (geom != null)
                        {
                            var sc = FindSpatialContext(sContexts, geom.SpatialContextAssociation);
                            return sc;
                        }
                        return null;
                    }
                    else if (ltype == LayerType.Raster)
                    {
                        IRasterLayerDefinition rl = (IRasterLayerDefinition)layer.SubLayer;
                        var clsDef = conn.FeatureService.GetClassDefinition(rl.ResourceId, rl.FeatureName);
                        var geom = clsDef.FindProperty(rl.Geometry) as RasterPropertyDefinition;
                        if (geom != null)
                        {
                            var sc = FindSpatialContext(sContexts, geom.SpatialContextAssociation);
                            return sc;
                        }
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Returns the spatial extent of the data.
            /// This is calculated by asking the underlying featuresource for the minimum rectangle that
            /// contains all the features in the specified table. If the <paramref name="allowFallbackToContextInformation"/>
            /// is set to true, and the query fails, the code will attempt to read this information
            /// from the spatial context information instead.
            /// </summary>
            /// <param name="layer">The layer definition</param>
            /// <param name="conn">The server connection</param>
            /// <param name="allowFallbackToContextInformation">If true, will default to the extents of the active spatial context.</param>
            /// <param name="csWkt">The coordinate system WKT that this extent corresponds to</param>
            /// <returns>The spatial extent</returns>
            public static IEnvelope GetSpatialExtent(this ILayerDefinition layer, IServerConnection conn, bool allowFallbackToContextInformation, out string csWkt)
            {
                csWkt = null;
                Check.ArgumentNotNull(layer, nameof(layer));
                Check.ArgumentNotNull(conn, nameof(conn));

                switch (layer.SubLayer.LayerType)
                {
                    case LayerType.Vector:
                        {
                            IEnvelope env = null;
                            IFdoSpatialContext activeSc = null;
                            try
                            {
                                activeSc = layer.GetSpatialContext(conn);
                                if (activeSc != null)
                                {
                                    //TODO: Check if ones like SQL Server will return the WKT, otherwise we'll need to bring in the
                                    //CS catalog to do CS code to WKT conversion.
                                    csWkt = activeSc.CoordinateSystemWkt;
                                }

                                //This can fail if SpatialExtents() aggregate function is not supported
                                env = conn.FeatureService.GetSpatialExtent(layer.SubLayer.ResourceId, ((IVectorLayerDefinition)layer.SubLayer).FeatureName, ((IVectorLayerDefinition)layer.SubLayer).Geometry);
                                return env;
                            }
                            catch
                            {
                                //Which in that case, default to extents of active spatial context
                                if (activeSc != null && activeSc.Extent != null)
                                    return activeSc.Extent.Clone();
                                else
                                    return null;
                            }
                        }
                    case LayerType.Raster:
                        {
                            IEnvelope env = null;
                            IFdoSpatialContext activeSc = null;
                            try
                            {
                                var scList = conn.FeatureService.GetSpatialContextInfo(layer.SubLayer.ResourceId, true);
                                if (scList.SpatialContext.Count > 0)
                                {
                                    activeSc = scList.SpatialContext[0];
                                }

                                //TODO: Would any raster provider *not* return a WKT?
                                csWkt = activeSc.CoordinateSystemWkt;

                                //Can fail if SpatialExtents() aggregate function is not supported
                                env = conn.FeatureService.GetSpatialExtent(layer.SubLayer.ResourceId, ((IRasterLayerDefinition)layer.SubLayer).FeatureName, ((IRasterLayerDefinition)layer.SubLayer).Geometry);
                                return env;
                            }
                            catch //Default to extents of active spatial context
                            {
                                if (activeSc != null && activeSc.Extent != null)
                                    return activeSc.Extent.Clone();
                                else
                                    return null;
                            }
                        }
                    default:
                        {
                            int[] services = conn.Capabilities.SupportedServices;
                            if (Array.IndexOf(services, (int)ServiceType.Drawing) >= 0)
                            {
                                var sheet = ((IDrawingLayerDefinition)layer.SubLayer).Sheet;
                                var dws = (IDrawingSource)conn.ResourceService.GetResource(((IDrawingLayerDefinition)layer.SubLayer).ResourceId);

                                if (dws.Sheet != null)
                                {
                                    //find matching sheet
                                    foreach (var sht in dws.Sheet)
                                    {
                                        if (sheet.Equals(sht.Name))
                                        {
                                            csWkt = dws.CoordinateSpace;
                                            return ObjectFactory.CreateEnvelope(sht.Extent.MinX, sht.Extent.MinY, sht.Extent.MaxX, sht.Extent.MaxY);
                                        }
                                    }
                                }
                            }
                            return null;
                        }
                }
            }

            /// <summary>
            /// Gets the name of the active spatial context used by the given layer definition
            /// </summary>
            /// <param name="ldf">The layer definition</param>
            /// <param name="conn">The server connection</param>
            /// <returns>The name of the active spatial context</returns>
            public static string GetLayerSpatialContextName(this ILayerDefinition ldf, IServerConnection conn)
            {
                var rl = ldf.SubLayer as IRasterLayerDefinition;
                var vl = ldf.SubLayer as IVectorLayerDefinition;
                if (vl != null)
                {
                    var cls = conn.FeatureService.GetClassDefinition(vl.ResourceId, vl.FeatureName);
                    var gp = cls.FindProperty(vl.Geometry) as GeometricPropertyDefinition;
                    if (gp != null)
                        return gp.SpatialContextAssociation;
                }
                else if (rl != null)
                {
                    var cls = conn.FeatureService.GetClassDefinition(rl.ResourceId, rl.FeatureName);
                    var rp = cls.FindProperty(rl.Geometry) as RasterPropertyDefinition;
                    if (rp != null)
                        return rp.SpatialContextAssociation;
                }
                return null;
            }

            /// <summary>
            /// Creates a default point composite rule
            /// </summary>
            /// <param name="fact">The layer element factory</param>
            /// <returns>The default point composite rule</returns>
            public static ICompositeRule CreateDefaultPointCompositeRule(this ILayerElementFactory fact)
            {
                Check.ArgumentNotNull(fact, nameof(fact));
                var rule = fact.CreateDefaultCompositeRule();
                //Clear out existing instances
                rule.CompositeSymbolization.RemoveAllSymbolInstances();

                var ldf = (ILayerDefinition)fact;
                var vl = (IVectorLayerDefinition)ldf.SubLayer;

                string symbolName = "Square"; //NOXLATE

                var ssym = ObjectFactory.CreateSimpleSymbol(vl.SymbolDefinitionVersion,
                                                            symbolName,
                                                            "Default Point Symbol"); //NOXLATE

                var square = ssym.CreatePathGraphics();
                square.Geometry = "M -1.0,-1.0 L 1.0,-1.0 L 1.0,1.0 L -1.0,1.0 L -1.0,-1.0"; //NOXLATE
                square.FillColor = "%FILLCOLOR%"; //NOXLATE
                square.LineColor = "%LINECOLOR%"; //NOXLATE
                square.LineWeight = "%LINEWEIGHT%"; //NOXLATE
                ssym.AddGraphics(square);

                ssym.PointUsage = ssym.CreatePointUsage();
                ssym.PointUsage.Angle = "%ROTATION%"; //NOXLATE

                ssym.DefineParameter("FILLCOLOR", "0xffffffff", "&amp;Fill Color", "Fill Color", "FillColor"); //NOXLATE
                ssym.DefineParameter("LINECOLOR", "0xff000000", "Line &amp;Color", "Line Color", "LineColor"); //NOXLATE
                ssym.DefineParameter("LINEWEIGHT", "0.0", "Line &amp; Thickness", "Line Thickness", "LineWeight"); //NOXLATE
                ssym.DefineParameter("ROTATION", "0.0", "Line &amp; Thickness", "Line Thickness", "Angle"); //NOXLATE

                var instance = rule.CompositeSymbolization.CreateInlineSimpleSymbol(ssym);
                var overrides = instance.ParameterOverrides;

                overrides.AddOverride(symbolName, "FILLCOLOR", "0xffffffff"); //NOXLATE
                overrides.AddOverride(symbolName, "LINECOLOR", "0xff000000"); //NOXLATE
                overrides.AddOverride(symbolName, "LINEWEIGHT", "0.0"); //NOXLATE
                overrides.AddOverride(symbolName, "ROTATION", "0.0"); //NOXLATE

                instance.AddToExclusionRegion = "true"; //NOXLATE
                var inst2 = instance as ISymbolInstance2;
                if (inst2 != null)
                {
                    inst2.UsageContext = UsageContextType.Point;
                    inst2.GeometryContext = GeometryContextType.Point;
                }

                rule.CompositeSymbolization.AddSymbolInstance(instance);
                return rule;
            }

            /// <summary>
            /// Creates a default line composite rule
            /// </summary>
            /// <param name="fact">The layer element factory</param>
            /// <returns>The default line composite rule</returns>
            public static ICompositeRule CreateDefaultLineCompositeRule(this ILayerElementFactory fact)
            {
                Check.ArgumentNotNull(fact, nameof(fact));
                var rule = fact.CreateDefaultCompositeRule();
                //Clear out existing instances
                rule.CompositeSymbolization.RemoveAllSymbolInstances();

                var ldf = (ILayerDefinition)fact;
                var vl = (IVectorLayerDefinition)ldf.SubLayer;

                string symbolName = "Solid Line"; //NOXLATE

                var ssym = ObjectFactory.CreateSimpleSymbol(vl.SymbolDefinitionVersion,
                                                            symbolName,
                                                            "Default Line Symbol"); //NOXLATE

                var line = ssym.CreatePathGraphics();
                line.Geometry = "M 0.0,0.0 L 1.0,0.0"; //NOXLATE
                line.LineColor = "%LINECOLOR%"; //NOXLATE
                line.LineWeight = "%LINEWEIGHT%"; //NOXLATE
                line.LineWeightScalable = "true"; //NOXLATE
                ssym.AddGraphics(line);

                ssym.LineUsage = ssym.CreateLineUsage();
                ssym.LineUsage.Repeat = "1.0";

                ssym.DefineParameter("LINECOLOR", "0xff000000", "Line &amp;Color", "Line Color", "LineColor"); //NOXLATE
                ssym.DefineParameter("LINEWEIGHT", "0.0", "Line &amp; Thickness", "Line Thickness", "LineWeight"); //NOXLATE

                var instance = rule.CompositeSymbolization.CreateInlineSimpleSymbol(ssym);
                var overrides = instance.ParameterOverrides;

                overrides.AddOverride(symbolName, "LINECOLOR", "0xff000000"); //NOXLATE
                overrides.AddOverride(symbolName, "LINEWEIGHT", "0.0"); //NOXLATE

                var inst2 = instance as ISymbolInstance2;
                if (inst2 != null)
                {
                    inst2.UsageContext = UsageContextType.Line;
                    inst2.GeometryContext = GeometryContextType.LineString;
                }

                rule.CompositeSymbolization.AddSymbolInstance(instance);
                return rule;
            }

            /// <summary>
            /// Creates a default area composite rule
            /// </summary>
            /// <param name="fact">The layer element factory</param>
            /// <returns>The default area composite rule</returns>
            public static ICompositeRule CreateDefaultAreaCompositeRule(this ILayerElementFactory fact)
            {
                Check.ArgumentNotNull(fact, nameof(fact));
                var rule = fact.CreateDefaultCompositeRule();
                //Clear out existing instances
                rule.CompositeSymbolization.RemoveAllSymbolInstances();

                var ldf = (ILayerDefinition)fact;
                var vl = (IVectorLayerDefinition)ldf.SubLayer;

                string fillSymbolName = "Solid Fill"; //NOXLATE
                var fillSym = ObjectFactory.CreateSimpleSymbol(vl.SymbolDefinitionVersion,
                                                               fillSymbolName,
                                                               "Default Area Symbol"); //NOXLATE

                var fill = fillSym.CreatePathGraphics();
                fill.Geometry = "M 0.0,0.0 h 100.0 v 100.0 h -100.0 z";
                fill.FillColor = "%FILLCOLOR%";
                fillSym.AddGraphics(fill);

                fillSym.AreaUsage = fillSym.CreateAreaUsage();
                fillSym.AreaUsage.RepeatX = "100.0"; //NOXLATE
                fillSym.AreaUsage.RepeatY = "100.0"; //NOXLATE

                fillSym.DefineParameter("FILLCOLOR", "0xffbfbfbf", "&amp;Fill Color", "Fill Color", "FillColor"); //NOXLATE

                var fillInstance = rule.CompositeSymbolization.CreateInlineSimpleSymbol(fillSym);
                var fillOverrides = fillInstance.ParameterOverrides;

                var fillInst2 = fillInstance as ISymbolInstance2;
                if (fillInst2 != null)
                {
                    fillInst2.GeometryContext = GeometryContextType.Polygon;
                }

                fillOverrides.AddOverride(fillSymbolName, "FILLCOLOR", "0xffbfbfbf");

                string lineSymbolName = "Solid Line"; //NOXLATE
                var lineSym = ObjectFactory.CreateSimpleSymbol(vl.SymbolDefinitionVersion,
                                                               lineSymbolName,
                                                               "Default Line Symbol"); //NOXLATE

                var line = lineSym.CreatePathGraphics();
                line.Geometry = "M 0.0,0.0 L 1.0,0.0"; //NOXLATE
                line.LineColor = "%LINECOLOR%"; //NOXLATE
                line.LineWeight = "%LINEWEIGHT%"; //NOXLATE
                line.LineWeightScalable = "false"; //NOXLATE
                lineSym.AddGraphics(line);

                lineSym.LineUsage = lineSym.CreateLineUsage();
                lineSym.LineUsage.Repeat = "1.0";

                lineSym.DefineParameter("LINECOLOR", "0xff000000", "Line &amp;Color", "Line Color", "LineColor"); //NOXLATE
                lineSym.DefineParameter("LINEWEIGHT", "0.0", "Line &amp; Thickness", "Line Thickness", "LineWeight"); //NOXLATE

                var lineInstance = rule.CompositeSymbolization.CreateInlineSimpleSymbol(lineSym);
                var lineOverrides = lineInstance.ParameterOverrides;

                lineOverrides.AddOverride(lineSymbolName, "LINECOLOR", "0xff000000"); //NOXLATE
                lineOverrides.AddOverride(lineSymbolName, "LINEWEIGHT", "0.0"); //NOXLATE

                var lineInst2 = lineInstance as ISymbolInstance2;
                if (lineInst2 != null)
                {
                    lineInst2.GeometryContext = GeometryContextType.Polygon;
                }

                rule.CompositeSymbolization.AddSymbolInstance(fillInstance);
                rule.CompositeSymbolization.AddSymbolInstance(lineInstance);
                return rule;
            }

            /// <summary>
            /// Creates a default point composite style
            /// </summary>
            /// <param name="fact">The layer element factory</param>
            /// <returns>The default point composite style</returns>
            public static ICompositeTypeStyle CreateDefaultPointCompositeStyle(this ILayerElementFactory fact)
            {
                Check.ArgumentNotNull(fact, nameof(fact));
                var style = fact.CreateDefaultCompositeStyle();
                style.RemoveAllRules();
                style.AddCompositeRule(fact.CreateDefaultPointCompositeRule());
                return style;
            }

            /// <summary>
            /// Creates a default line composite style
            /// </summary>
            /// <param name="fact">The layer element factory</param>
            /// <returns>The default line composite style</returns>
            public static ICompositeTypeStyle CreateDefaultLineCompositeStyle(this ILayerElementFactory fact)
            {
                Check.ArgumentNotNull(fact, nameof(fact));
                var style = fact.CreateDefaultCompositeStyle();
                style.RemoveAllRules();
                style.AddCompositeRule(fact.CreateDefaultLineCompositeRule());
                return style;
            }

            /// <summary>
            /// Creates a default area composite style
            /// </summary>
            /// <param name="fact">The layer element factory</param>
            /// <returns>The default area composite style</returns>
            public static ICompositeTypeStyle CreateDefaultAreaCompositeStyle(this ILayerElementFactory fact)
            {
                Check.ArgumentNotNull(fact, nameof(fact));
                var style = fact.CreateDefaultCompositeStyle();
                style.RemoveAllRules();
                style.AddCompositeRule(fact.CreateDefaultAreaCompositeRule());
                return style;
            }
        }
    }
}