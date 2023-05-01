﻿#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
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

using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Base.UI;
using OSGeo.MapGuide.ObjectModels;
using System.Collections.Generic;
using System.Linq;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class DuplicateResourceCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = connMgr.GetConnection(exp.ConnectionName);

            if (exp.SelectedItems.Length > 0)
            {
                List<RepositoryItem> toDuplicate = new List<RepositoryItem>();
                var sel = exp.GetSelectedResources().ToArray();
                foreach (var item in sel)
                {
                    if (!item.IsFolder)
                        toDuplicate.Add(item);
                }

                //They all have the same parent
                var folder = sel[0].Parent;

                foreach (var item in toDuplicate)
                {
                    //Keep testing until we find a target resource identifier that
                    //doesn't already exists. Note this would automatically guard against any resources in this folder
                    //that may already be open in an editor
                    var rid = new ResourceIdentifier(item.ResourceId);
                    var name = rid.IsFolder ? $"{rid.Name}/" : $"{rid.Name}.{rid.ResourceType.ToString()}"; //NOXLATE
                    var resId = folder.ResourceId + name;
                    int counter = 0;
                    while (conn.ResourceService.ResourceExists(resId))
                    {
                        counter++;

                        if (rid.IsFolder)
                        {
                            resId = $"{folder.ResourceId + rid.Name} ({counter})/"; //NOXLATE
                        }
                        else
                        {
                            var rname = name.Substring(0, name.IndexOf(".")); //NOXLATE
                            var type = name.Substring(name.IndexOf(".")); //NOXLATE
                            rname += $" ({counter})"; //NOXLATE
                            resId = folder.ResourceId + rname + type;
                        }
                    }

                    conn.ResourceService.CopyResource(item.ResourceId, resId, false);
                    LoggingService.Info(string.Format(Strings.ResourceDuplicated, item.ResourceId, resId));
                }

                exp.RefreshModel(conn.DisplayName, folder.ResourceId);
            }
        }
    }
}