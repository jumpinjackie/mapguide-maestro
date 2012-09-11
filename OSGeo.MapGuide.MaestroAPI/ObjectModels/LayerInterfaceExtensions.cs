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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System.Drawing;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace OSGeo.MapGuide.ObjectModels.LayerDefinition
{
    /// <summary>
    /// Extension method clas
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Sets the color of the block.
        /// </summary>
        /// <param name="sym">The sym.</param>
        /// <param name="c">The c.</param>
        public static void SetBlockColor(this IBlockSymbol sym, Color c)
        {
            Check.NotNull(sym, "sym");
            sym.BlockColor = Utility.SerializeHTMLColor(c, true);
        }

        /// <summary>
        /// Sets the color of the layer.
        /// </summary>
        /// <param name="sym">The sym.</param>
        /// <param name="c">The c.</param>
        public static void SetLayerColor(this IBlockSymbol sym, Color c)
        {
            Check.NotNull(sym, "sym"); //NOXLATE
            sym.LayerColor = Utility.SerializeHTMLColor(c, true);
        }

        /// <summary>
        /// Applies properties (name, italic, bold, underline) from the characteristics of the specified font
        /// </summary>
        /// <param name="sym"></param>
        /// <param name="f"></param>
        public static void Apply(this IFontSymbol sym, Font f)
        {
            Check.NotNull(sym, "sym"); //NOXLATE
            sym.FontName = f.Name;
            sym.Italic = f.Italic;
            sym.Bold = f.Bold;
            sym.Underlined = f.Underline;
        }

        /// <summary>
        /// Sets the color of the foreground.
        /// </summary>
        /// <param name="sym">The sym.</param>
        /// <param name="c">The c.</param>
        public static void SetForegroundColor(this IFontSymbol sym, Color c)
        {
            Check.NotNull(sym, "sym"); //NOXLATE
            sym.ForegroundColor = Utility.SerializeHTMLColor(c, true);
        }

        /// <summary>
        /// Determines whether the vector layer has scale ranges
        /// </summary>
        /// <param name="vl">The vl.</param>
        /// <returns>
        /// 	<c>true</c> if vector layer has scale ranges; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasVectorScaleRanges(this IVectorLayerDefinition vl)
        {
            Check.NotNull(vl, "vl"); //NOXLATE
            return vl.GetScaleRangeCount() > 0;
        }

        /// <summary>
        /// Gets the number of scale ranges in this vector layer
        /// </summary>
        /// <param name="vl"></param>
        /// <returns></returns>
        public static int GetScaleRangeCount(this IVectorLayerDefinition vl)
        {
            Check.NotNull(vl, "vl"); //NOXLATE
            var list = new List<IVectorScaleRange>(vl.VectorScaleRange);
            return list.Count;
        }

        /// <summary>
        /// Purge the specified scale range of the following styles
        /// </summary>
        /// <param name="range"></param>
        /// <param name="geomTypes">The geometry types to remove</param>
        public static void RemoveStyles(this IVectorScaleRange range, IEnumerable<string> geomTypes)
        {
            Check.NotNull(range, "range"); //NOXLATE
            Check.NotNull(geomTypes, "geomTypes"); //NOXLATE

            List<IVectorStyle> remove = new List<IVectorStyle>();

            foreach (var geomType in geomTypes)
            {
                if (geomType.ToLower().Equals(FeatureGeometricType.Curve.ToString().ToLower()))
                {
                    range.LineStyle = null;
                }
                else if (geomType.ToLower().Equals(FeatureGeometricType.Point.ToString().ToLower()))
                {
                    range.PointStyle = null;
                }
                else if (geomType.ToLower().Equals(FeatureGeometricType.Surface.ToString().ToLower()))
                {
                    range.AreaStyle = null;
                }
            }
        }

        /// <summary>
        /// Removes the styles.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="geomTypes">The geom types.</param>
        public static void RemoveStyles(this IVectorScaleRange range, params string[] geomTypes)
        {
            range.RemoveStyles(geomTypes);
        }

        /// <summary>
        /// Gets the coordinate system WKT.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        public static string GetCoordinateSystemWkt(this ILayerDefinition layer)
        {
            Check.NotNull(layer, "layer"); //NOXLATE
            if (layer.CurrentConnection == null)
                throw new System.Exception(OSGeo.MapGuide.MaestroAPI.Strings.ErrorNoServerConnectionAttached);

            var conn = layer.CurrentConnection;
            switch (layer.SubLayer.LayerType)
            {
                case LayerType.Raster:
                    {
                        var rl = (IRasterLayerDefinition)layer.SubLayer;
                        var fs = (IFeatureSource)conn.ResourceService.GetResource(rl.ResourceId);
                        var scList = fs.GetSpatialInfo(true);
                        if (scList.SpatialContext.Count > 0)
                            return scList.SpatialContext[0].CoordinateSystemWkt;
                    }
                    break;
                case LayerType.Vector:
                    {
                        var vl = (IVectorLayerDefinition)layer.SubLayer;
                        var fs = (IFeatureSource)conn.ResourceService.GetResource(vl.ResourceId);
                        var scList = fs.GetSpatialInfo(true);
                        if (scList.SpatialContext.Count > 0)
                            return scList.SpatialContext[0].CoordinateSystemWkt;
                    }
                    break;
                case LayerType.Drawing:
                    {
                        int[] services = conn.Capabilities.SupportedServices;
                        if (Array.IndexOf(services, (int)ServiceType.Drawing) >= 0)
                        {
                            var sheet = ((IDrawingLayerDefinition)layer.SubLayer).Sheet;
                            var dws = (IDrawingSource)conn.ResourceService.GetResource(((IDrawingLayerDefinition)layer.SubLayer).ResourceId);

                            //This should already be WKT form
                            return dws.CoordinateSpace;
                        }
                    }
                    break;
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns the spatial extent of the data.
        /// This is calculated by asking the underlying featuresource for the minimum rectangle that
        /// contains all the features in the specified table. If the <paramref name="allowFallbackToContextInformation"/>
        /// is set to true, and the query fails, the code will attempt to read this information
        /// from the spatial context information instead.
        /// </summary>
        /// <param name="layer">The layer definition</param>
        /// <param name="allowFallbackToContextInformation">If true, will default to the extents of the active spatial context.</param>
        /// <param name="csWkt">The coordinate system WKT that this extent corresponds to</param>
        /// <returns></returns>
        public static IEnvelope GetSpatialExtent(this ILayerDefinition layer, bool allowFallbackToContextInformation, out string csWkt)
        {
            csWkt = null;
            Check.NotNull(layer, "layer"); //NOXLATE
            if (layer.CurrentConnection == null)
                throw new System.Exception(OSGeo.MapGuide.MaestroAPI.Strings.ErrorNoServerConnectionAttached);

            var conn = layer.CurrentConnection;
            switch (layer.SubLayer.LayerType)
            {
                case LayerType.Vector:
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
                            //TODO: Check if ones like SQL Server will return the WKT, otherwise we'll need to bring in the
                            //CS catalog to do CS code to WKT conversion.
                            csWkt = activeSc.CoordinateSystemWkt;

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
        /// Gets the referenced schema of this vector layer
        /// </summary>
        /// <param name="vl"></param>
        /// <returns></returns>
        public static string GetSchema(this IVectorLayerDefinition vl)
        {
            if (string.IsNullOrEmpty(vl.FeatureName) || !vl.FeatureName.Contains(":")) //NOXLATE
                return string.Empty;
            else
                return vl.FeatureName.Split(':')[0]; //NOXLATE
        }

        /// <summary>
        /// Sets the color of the fill.
        /// </summary>
        /// <param name="sym">The sym.</param>
        /// <param name="c">The c.</param>
        public static void SetFillColor(this IW2DSymbol sym, Color c)
        {
            Check.NotNull(sym, "sym"); //NOXLATE
            sym.FillColor = Utility.SerializeHTMLColor(c, true);
        }

        /// <summary>
        /// Sets the color of the line.
        /// </summary>
        /// <param name="sym">The sym.</param>
        /// <param name="c">The c.</param>
        public static void SetLineColor(this IW2DSymbol sym, Color c)
        {
            Check.NotNull(sym, "sym"); //NOXLATE
            sym.LineColor = Utility.SerializeHTMLColor(c, true);
        }

        /// <summary>
        /// Sets the color of the text.
        /// </summary>
        /// <param name="sym">The sym.</param>
        /// <param name="c">The c.</param>
        public static void SetTextColor(this IW2DSymbol sym, Color c)
        {
            Check.NotNull(sym, "sym"); //NOXLATE
            sym.TextColor = Utility.SerializeHTMLColor(c, true);
        }

        /// <summary>
        /// Sets the color of the foreground.
        /// </summary>
        /// <param name="sym">The sym.</param>
        /// <param name="c">The c.</param>
        public static void SetForegroundColor(this ITextSymbol sym, Color c)
        {
            Check.NotNull(sym, "sym"); //NOXLATE
            sym.ForegroundColor = Utility.SerializeHTMLColor(c, true);
        }

        /// <summary>
        /// Sets the color of the background.
        /// </summary>
        /// <param name="sym">The sym.</param>
        /// <param name="c">The c.</param>
        public static void SetBackgroundColor(this ITextSymbol sym, Color c)
        {
            Check.NotNull(sym, "sym"); //NOXLATE
            sym.BackgroundColor = Utility.SerializeHTMLColor(c, true);
        }

        /// <summary>
        /// Applies properties (name, italic, bold, underline) from the characteristics of the specified font
        /// </summary>
        /// <param name="sym"></param>
        /// <param name="f"></param>
        public static void Apply(this ITextSymbol sym, Font f)
        {
            Check.NotNull(sym, "sym"); //NOXLATE
            sym.FontName = f.Name;
            sym.Italic = f.Italic.ToString();
            sym.Bold = f.Bold.ToString();
            sym.Underlined = f.Underline.ToString();
        }

        /// <summary>
        /// Sets the color of the background.
        /// </summary>
        /// <param name="fil">The fil.</param>
        /// <param name="c">The c.</param>
        public static void SetBackgroundColor(this IFill fil, Color c)
        {
            Check.NotNull(fil, "fil"); //NOXLATE
            fil.BackgroundColor = Utility.SerializeHTMLColor(c, true);
        }

        /// <summary>
        /// Sets the color of the foreground.
        /// </summary>
        /// <param name="fil">The fil.</param>
        /// <param name="c">The c.</param>
        public static void SetForegroundColor(this IFill fil, Color c)
        {
            Check.NotNull(fil, "fil"); //NOXLATE
            fil.ForegroundColor = Utility.SerializeHTMLColor(c, true);
        }

        /// <summary>
        /// Sets the color of the transparency.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="c">The c.</param>
        public static void SetTransparencyColor(this IGridColorStyle style, Color c)
        {
            Check.NotNull(style, "style"); //NOXLATE
            style.TransparencyColor = Utility.SerializeHTMLColor(c, true);
        }

        /// <summary>
        /// Sets the default color.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="c">The c.</param>
        public static void SetDefaultColor(this IGridSurfaceStyle style, Color c)
        {
            Check.NotNull(style, "style"); //NOXLATE
            style.DefaultColor = Utility.SerializeHTMLColor(c, true);
        }
    }
}
