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
using ICSharpCode.Core;
using Maestro.Base.UI;
using Maestro.Base.Services;
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace Maestro.Base.Commands
{
    internal class NewItemCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var mgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var orm = ServiceRegistry.GetService<OpenResourceManager>();
            var conn = mgr.GetConnection(exp.ConnectionName);
            var nits = ServiceRegistry.GetService<NewItemTemplateService>();

            NewResourceDialog dlg = null;
            string startPoint = null;
            if (exp.SelectedItems.Length == 1)
            {
                var item = exp.SelectedItems[0];
                if (item.IsFolder)
                    startPoint = item.ResourceId;
            }

            if (dlg == null)
                dlg = new NewResourceDialog(conn, nits);

            if (dlg.ShowDialog(wb) == System.Windows.Forms.DialogResult.OK)
            {
                var tpl = dlg.SelectedTemplate;
                var res = tpl.CreateItem(startPoint, conn);
                if (res != null)
                {
                    res.ResourceID = "Session:" + conn.SessionID + "//" + Guid.NewGuid().ToString() + "." + res.ResourceType.ToString(); //NOXLATE
                    conn.ResourceService.SaveResource(res);
                    var ed = orm.Open(res.ResourceID, conn, false, exp);
                    if (!string.IsNullOrEmpty(startPoint))
                    {
                        ed.EditorService.SuggestedSaveFolder = startPoint;
                        ed.EditorService.MarkDirty();
                    }
                }
            }
        }
    }
}
