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
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using System.IO;

namespace OSGeo.MapGuide.ObjectModels.DrawingSource
{
    /// <summary>
    /// A DWF-based Drawing Source
    /// </summary>
    public interface IDrawingSource : IResource
    {
        /// <summary>
        /// Gets or sets the name of the source (dwf file).
        /// </summary>
        /// <value>The name of the source.</value>
        string SourceName { get; set; }

        /// <summary>
        /// Gets or sets the coordinate space.
        /// </summary>
        /// <value>The coordinate space.</value>
        string CoordinateSpace { get; set; }

        /// <summary>
        /// Removes all sheets.
        /// </summary>
        void RemoveAllSheets();

        /// <summary>
        /// Gets the sheets.
        /// </summary>
        /// <value>The sheets.</value>
        IEnumerable<IDrawingSourceSheet> Sheet { get; }

        /// <summary>
        /// Adds the sheet.
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        void AddSheet(IDrawingSourceSheet sheet);

        /// <summary>
        /// Removes the sheet.
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        void RemoveSheet(IDrawingSourceSheet sheet);

        /// <summary>
        /// Creates the sheet.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="minx">The minx.</param>
        /// <param name="miny">The miny.</param>
        /// <param name="maxx">The maxx.</param>
        /// <param name="maxy">The maxy.</param>
        /// <returns></returns>
        IDrawingSourceSheet CreateSheet(string name, double minx, double miny, double maxx, double maxy);
    }

    /// <summary>
    /// Represents a sheet (DWF section)
    /// </summary>
    public interface IDrawingSourceSheet
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the extent.
        /// </summary>
        /// <value>The extent.</value>
        IEnvelope Extent { get; set; }
    }

    /// <summary>
    /// Extension methods for Drawing Sources
    /// </summary>
    public static class DrawingSourceExtensions
    {
        /// <summary>
        /// Regenerates the sheet list in this drawing source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>True if sheets were regenerated. False otherwise</returns>
        public static bool RegenerateSheetList(this IDrawingSource source)
        {
            Check.NotNull(source, "source");
            Check.NotNull(source.CurrentConnection, "source.CurrentConection"); //NOXLATE
            Check.NotEmpty(source.ResourceID, "source.ResourceID"); //NOXLATE

            IDrawingService dwSvc = (IDrawingService)source.CurrentConnection.GetService((int)ServiceType.Drawing);
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
        /// <param name="source"></param>
        public static void UpdateExtents(this IDrawingSource source)
        {
            Check.NotNull(source, "source"); //NOXLATE
            Check.NotNull(source.CurrentConnection, "source.CurrentConection"); //NOXLATE
            Check.NotEmpty(source.ResourceID, "source.ResourceID"); //NOXLATE

            //Need drawing service
            if (Array.IndexOf(source.CurrentConnection.Capabilities.SupportedServices, (int)ServiceType.Drawing) < 0)
                throw new NotSupportedException(string.Format(OSGeo.MapGuide.MaestroAPI.Strings.ERR_SERVICE_NOT_SUPPORTED, ServiceType.Drawing.ToString()));

            var drawSvc = (IDrawingService)source.CurrentConnection.GetService((int)ServiceType.Drawing);

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

                                    // 4 - length of "llx="
                                    int idx = content.IndexOf("llx") + 4;  //NOXLATE
                                    string sllx = content.Substring(idx, content.IndexOf(" ", idx) - idx); //NOXLATE
                                    // 4 - length of "lly="
                                    idx = content.IndexOf("lly") + 4; //NOXLATE
                                    string slly = content.Substring(idx, content.IndexOf(" ", idx) - idx); //NOXLATE
                                    // 4 - length of "urx="
                                    idx = content.IndexOf("urx") + 4; //NOXLATE
                                    string surx = content.Substring(idx, content.IndexOf(" ", idx) - idx); //NOXLATE
                                    // 4 - length of "ury="
                                    idx = content.IndexOf("ury") + 4; //NOXLATE
                                    string sury = content.Substring(idx, content.IndexOf(" ", idx) - idx); //NOXLATE

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
