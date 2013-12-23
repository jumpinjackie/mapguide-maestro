#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.DrawingSource;
using OSGeo.MapGuide.ObjectModels.SymbolLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Conversion
{
    /// <summary>
    /// A utility class that extracts image symbols from a Symbol Library and converts them
    /// to image-based Symbol Definition resources
    /// </summary>
    public class ImageSymbolConverter
    {
        private string _symbolLibId;
        private IServerConnection _conn;

        /// <summary>
        /// Initializes a new instance of the ImageSymbolConverter class
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="symbolLibId"></param>
        public ImageSymbolConverter(IServerConnection conn, string symbolLibId)
        {
            Check.NotNull(conn, "conn"); //NOXLATE
            Check.NotEmpty(symbolLibId, "symbolLibId"); //NOXLATE
            Check.Precondition(ResourceIdentifier.GetResourceType(symbolLibId) == ResourceTypes.SymbolLibrary, "ResourceIdentifier.GetResourceType(symbolLibId) == ResourceTypes.SymbolLibrary"); //NOXLATE
            Check.Precondition(Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Drawing) >= 0, "Array.IndexOf(conn.Capabilities.SupportedServices, (int)ServiceType.Drawing) >= 0"); //NOXLATE
            _symbolLibId = symbolLibId;
            _conn = conn;
        }

        /// <summary>
        /// Creates an image-based Symbol Definition in the specified folder for each image symbol in the Symbol Library.
        /// 
        /// Any existing resource names are overwritten
        /// </summary>
        /// <param name="targetFolder"></param>
        public void ExtractSymbols(string targetFolder)
        {
            Check.NotEmpty(targetFolder, "targetFolder"); //NOXLATE
            Check.Precondition(ResourceIdentifier.IsFolderResource(targetFolder), "ResourceIdentifier.IsFolderResource(targetFolder)"); //NOXLATE

            IDrawingService drawSvc = (IDrawingService)_conn.GetService((int)ServiceType.Drawing);
            IDrawingSource ds = PrepareSymbolDrawingSource(_conn, _symbolLibId);

            //Each section in the symbols.dwf represents a symbol
            var sectionList = drawSvc.EnumerateDrawingSections(ds.ResourceID);

            foreach (var sect in sectionList.Section)
            {
                var sectResources = drawSvc.EnumerateDrawingSectionResources(ds.ResourceID, sect.Name);

                foreach (var res in sectResources.SectionResource)
                {
                    if (res.Role.ToUpper() == StringConstants.Thumbnail.ToUpper())
                    {
                        using (var rs = drawSvc.GetSectionResource(ds.ResourceID, res.Href))
                        {
                            using (Image img = Image.FromStream(rs))
                            {

                                string targetId = targetFolder + sect.Title + "." + ResourceTypes.SymbolDefinition.ToString();
                                string dataName = sect.Title + "." + GetImageFormat(img.RawFormat);

                                var symDef = ObjectFactory.CreateSimpleSymbol(_conn, sect.Title, "Image symbol definition extracted from a Symbol Library by Maestro"); //NOXLATE
                                var imgGraphics = symDef.CreateImageGraphics();
                                symDef.AddGraphics(imgGraphics);

                                imgGraphics.Item = symDef.CreateImageReference(string.Empty, Utility.FdoStringifiyLiteral(dataName)); //Empty resource id = self reference

                                imgGraphics.SizeScalable = "True"; //NOXLATE
                                imgGraphics.ResizeControl = Utility.FdoStringifiyLiteral("ResizeNone"); //NOXLATE
                                imgGraphics.Angle = "0.0"; //NOXLATE
                                imgGraphics.PositionX = "0.0"; //NOXLATE
                                imgGraphics.PositionY = "4.0"; //NOXLATE

                                imgGraphics.SizeX = PxToMillimeters(img.Width).ToString(CultureInfo.InvariantCulture);
                                imgGraphics.SizeY = PxToMillimeters(img.Height).ToString(CultureInfo.InvariantCulture);

                                symDef.PointUsage = symDef.CreatePointUsage();
                                symDef.PointUsage.Angle = "%ROTATION_ANGLE%"; //NOXLATE

                                var rotParam = symDef.CreateParameter();
                                rotParam.DataType = "String"; //NOXLATE
                                rotParam.Identifier = "ROTATION_ANGLE"; //NOXLATE
                                rotParam.DisplayName = "Angle to rotate symbol"; //NOXLATE
                                rotParam.DefaultValue = "0.0"; //NOXLATE

                                symDef.ParameterDefinition.AddParameter(rotParam);

                                _conn.ResourceService.SaveResourceAs(symDef, targetId);
                                using (var ms = new MemoryStream())
                                {
                                    img.Save(ms, ImageFormat.Png);
                                    ms.Position = 0L; //Rewind
                                    _conn.ResourceService.SetResourceData(targetId, dataName, ObjectModels.Common.ResourceDataType.File, ms);
                                }

                                Trace.TraceInformation("Extracted symbol: " + targetId);
                            }
                        }
                    }
                }
            }
        }

        private string GetImageFormat(ImageFormat imageFormat)
        {
            if (imageFormat.Guid == ImageFormat.Png.Guid)
                return "png"; //NOXLATE
            else if (imageFormat.Guid == ImageFormat.Jpeg.Guid)
                return "jpg"; //NOXLATE
            else
                throw new NotSupportedException();
        }

        const int DPI = 96;

        private static double PxToMillimeters(int pixels)
        {
            var res = (pixels * 2.54 / DPI) * 10;
            return Math.Round(res, 1);
        }

        /// <summary>
        /// Extracts the DWF symbol data store from the given symbol library to a new session-based drawing source
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="symResId"></param>
        /// <returns></returns>
        public static IDrawingSource PrepareSymbolDrawingSource(IServerConnection conn, string symResId)
        {
            //Extract the symbols.dwf resource data and copy to a session based drawing source
            var dwf = conn.ResourceService.GetResourceData(symResId, "symbols.dwf"); //NOXLATE
            if (!dwf.CanSeek)
            {
                //House in MemoryStream
                var ms = new MemoryStream();
                Utility.CopyStream(dwf, ms);
                ms.Position = 0L;

                //Replace old stream with new
                dwf.Dispose();
                dwf = ms;
            }
            var ds = OSGeo.MapGuide.ObjectModels.ObjectFactory.CreateDrawingSource(conn);
            ds.SourceName = "symbols.dwf"; //NOXLATE
            ds.ResourceID = "Session:" + conn.SessionID + "//" + Guid.NewGuid() + ".DrawingSource"; //NOXLATE
            conn.ResourceService.SaveResource(ds);

            using (dwf)
            {
                conn.ResourceService.SetResourceData(ds.ResourceID, "symbols.dwf", OSGeo.MapGuide.ObjectModels.Common.ResourceDataType.File, dwf); //NOXLATE
            }
            return ds;
        }
    }
}
