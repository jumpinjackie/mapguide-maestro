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
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.IO;
using ICSharpCode.Core;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Shared.UI;

namespace Maestro.Base.Services.DragDropHandlers
{
    internal class XmlResFileHandler : IDragDropHandler
    {
        public string HandlerAction
        {
            get { return Strings.XmlResHandlerAction; }
        }

        private string[] extensions = { ".xml", //NOXLATE
                                        ".FeatureSource", //NOXLATE 
                                        ".LayerDefinition", //NOXLATE
                                        ".MapDefinition", //NOXLATE
                                        ".WebLayout", //NOXLATE
                                        ".SymbolDefinition", //NOXLATE
                                        ".ApplicationDefinition", //NOXLATE
                                        ".PrintLayout", //NOXLATE
                                      };

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

                //The easiest way to tell if this XML file is legit
                var res = ResourceTypeRegistry.Deserialize(File.ReadAllText(file));

                int counter = 0;
                string name = Path.GetFileNameWithoutExtension(file);
                string resId = folderId + name + "." + res.ResourceType.ToString(); //NOXLATE

                while (conn.ResourceService.ResourceExists(resId))
                {
                    counter++;
                    resId = folderId + name + " (" + counter + ")." + res.ResourceType.ToString(); //NOXLATE
                }

                res.ResourceID = resId;
                conn.ResourceService.SaveResource(res);

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
