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
using Maestro.Base.Services;
using Maestro.Base.UI;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class NewFolderCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = connMgr.GetConnection(exp.ConnectionName);
            if (exp.SelectedItems.Length == 1)
            {
                var item = exp.SelectedItems[0];
                if (item.IsFolder)
                {
                    string defaultName = "New Folder"; //NOXLATE
                    string name = defaultName;
                    int counter = -1;
                    while (item.Contains(name))
                    {
                        counter++;
                        name = defaultName + counter;
                    }

                    List<string> folderNames = new List<string>();
                    foreach (var child in item.Children)
                    {
                        if (child.IsFolder)
                            folderNames.Add(child.Name);
                    }

                    var diag = new NewFolderDialog(name, folderNames.ToArray());
                    if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        name = diag.FolderName;
                        //conn.ResourceService.CreateFolder(item.ResourceId + name);
                        conn.ResourceService.SetResourceXmlData(item.ResourceId + name + "/", null); //NOXLATE
                        var path = item.Model.GetPath(item);
                        item.Model.RaiseStructureChanged(new Aga.Controls.Tree.TreeModelEventArgs(path, new object[0]));

                        //Expand so user can see this new folder
                        exp.ExpandNode(conn.DisplayName, item.ResourceId);
                        exp.SelectNode(conn.DisplayName, item.ResourceId + name + "/"); //NOXLATE
                    }
                }
            }
        }
    }
}
