#region Disclaimer / License

// Copyright (C) 2013, Jackie Ng
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

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using System;
using System.IO;
using System.Text;

namespace Maestro.Editors.Preview
{
    /// <summary>
    /// Performs any necessary setup required to preview a given resource in a web browser
    /// </summary>
    internal class ResourcePreviewEngine
    {
        private IEditorService _edSvc;

        /// <summary>
        /// Initializes a new instance of the ResourcePreviewEngine class
        /// </summary>
        /// <param name="edSvc"></param>
        public ResourcePreviewEngine(IEditorService edSvc)
        {
            _edSvc = edSvc;
        }

        /// <summary>
        /// Generates the appropriate viewer URL to preview the given resource under the given locale
        /// </summary>
        /// <param name="res"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public string GeneratePreviewUrl(IResource res, string locale)
        {
            var conn = res.CurrentConnection;
            var previewer = conn.GetPreviewUrlGenerator();
            previewer.AddDebugWatermark = PreviewSettings.AddDebugWatermark;
            previewer.UseAjaxViewer = PreviewSettings.UseAjaxViewer;
            return previewer.GeneratePreviewUrl(res, locale, _edSvc.IsNew, _edSvc.SessionID);
        }

        #region Preview helper code

        private static string CreateDebugWatermark(IMapDefinition2 mdf, IServerConnection conn, string layerSc)
        {
            //Tidy up the CS WKT so that it can display nicely in a watermark
            StringBuilder cleanCs = new StringBuilder(mdf.CoordinateSystem);
            cleanCs.Replace("[", "[\n");
            cleanCs.Replace("],", "],\n");

            string message = null;

            if (!string.IsNullOrEmpty(layerSc))
            {
                message = string.Format(Strings.DebugWatermarkMessageLayer,
                    mdf.Extents.MinX,
                    mdf.Extents.MinY,
                    mdf.Extents.MaxX,
                    mdf.Extents.MaxY,
                    cleanCs.ToString(),
                    layerSc);
            }
            else
            {
                message = string.Format(Strings.DebugWatermarkMessage,
                    mdf.Extents.MinX,
                    mdf.Extents.MinY,
                    mdf.Extents.MaxX,
                    mdf.Extents.MaxY,
                    cleanCs.ToString());
            }
            string watermarkXml = string.Format(Properties.Resources.TextWatermark, message);
            string resId = "Session:" + conn.SessionID + "//Debug.WatermarkDefinition";
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(watermarkXml)))
            {
                conn.ResourceService.SetResourceXmlData(resId, ms);
            }

            //Add watermark to Map Definition
            var wmd = (IWatermarkDefinition)conn.ResourceService.GetResource(resId);
            mdf.AddWatermark(wmd);

            return resId;
        }

        internal static IMapDefinition CreateLayerPreviewMapDefinition(ILayerDefinition ldf, string sessionId, string layerName, IServerConnection conn)
        {
            //Create temp map definition to house our current layer
            var mdfId = "Session:" + sessionId + "//" + Guid.NewGuid() + ".MapDefinition"; //NOXLATE
            string csWkt;
            var extent = ldf.GetSpatialExtent(true, out csWkt);
            if (extent == null)
                throw new ApplicationException(Strings.FailedToCalculateFeatureSourceExtents);

            string layerSc = Utility.GetLayerSpatialContext(ldf);

            //TODO: Based on the visible scales in this layer, size this extents accordingly
            var mdf = ObjectFactory.CreateMapDefinition(conn, Strings.PreviewMap, csWkt, extent);
            IMapDefinition2 mdf2 = mdf as IMapDefinition2;
            if (mdf2 != null && PreviewSettings.AddDebugWatermark)
                CreateDebugWatermark(mdf2, conn, layerSc);

            var layer = mdf.AddLayer(null, layerName, ldf.ResourceID);
            conn.ResourceService.SaveResourceAs(mdf, mdfId);
            mdf.ResourceID = mdfId;
            return mdf;
        }

        #endregion Preview helper code
    }
}