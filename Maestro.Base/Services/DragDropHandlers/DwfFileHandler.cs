#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.IO;
using ICSharpCode.Core;
using OSGeo.MapGuide.ObjectModels;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.DrawingSource;

namespace Maestro.Base.Services.DragDropHandlers
{
    internal class DwfFileHandler : IDragDropHandler
    {
        public string HandlerAction
        {
            get { return Strings.DwfHandlerAction; }
        }

        string[] extensions = { ".dwf" }; //NOXLATE

        public string[] FileExtensions
        {
            get { return extensions; }
        }

        public bool HandleDrop(IServerConnection conn, string file, string folderId)
        {
            try
            {
                var wb = Workbench.Instance;
                var exp = wb.ActiveSiteExplorer;
                var ds = ObjectFactory.CreateDrawingSource(conn);

                string fileName = Path.GetFileName(file);
                string resName = Path.GetFileNameWithoutExtension(file);
                int counter = 0;
                string resId = folderId + resName + ".DrawingSource"; //NOXLATE
                while (conn.ResourceService.ResourceExists(resId))
                {
                    counter++;
                    resId = folderId + resName + " (" + counter + ").DrawingSource"; //NOXLATE
                }
                ds.ResourceID = resId;
                //fs.SetConnectionProperty("File", StringConstants.MgDataFilePath + fileName); //NOXLATE

                ds.SourceName = fileName;
                conn.ResourceService.SaveResource(ds);

                using (var stream = File.Open(file, FileMode.Open))
                {
                    ds.SetResourceData(fileName, OSGeo.MapGuide.ObjectModels.Common.ResourceDataType.File, stream);
                }

                ds.RegenerateSheetList();
                conn.ResourceService.SaveResource(ds); //Need to re-save for the next call to work
                ds.UpdateExtents();

                return true;
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ex);
                return false;
            }
        }
    }
}
