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
using System.Linq;
using ICSharpCode.Core;
using Maestro.Editors;
using Maestro.Base.Services;
using Maestro.Base.UI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Shared.UI;

namespace Maestro.Base.Commands
{
    internal class PasteCommand : ClipboardCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var clip = ServiceRegistry.GetService<ClipboardService>();
            var omgr = ServiceRegistry.GetService<OpenResourceManager>();
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var targetConnName = exp.ConnectionName;
            var conn = connMgr.GetConnection(targetConnName);

            if (!clip.HasContent())
                return;

            if (exp.SelectedItems.Length != 1)
                return;

            if (!exp.SelectedItems[0].IsFolder)
                return;

            var itemsToPaste = GetItems(clip);
            if (itemsToPaste.Length == 0)
                return;

            var folder = exp.SelectedItems[0];
            var sourceItemsNotMoved = new List<string>();
            
            //.net FX 2.0 hack to compensate for lack of set collection
            Dictionary<string, string> folders = new Dictionary<string, string>();
            var notPasted = new List<RepositoryItem>();

            //From same connection
            if (itemsToPaste.First().ConnectionName == targetConnName)
            {
                foreach (var item in itemsToPaste)
                {
                    LoggingService.InfoFormatted(Strings.ClipboardAction, item.ClipboardState, item.ResourceId, folder.ResourceId);

                    //Keep testing until we find a target resource identifier that 
                    //doesn't already exists. Note this would automatically guard against any resources in this folder
                    //that may already be open in an editor
                    var rid = new ResourceIdentifier(item.ResourceId);
                    var name = rid.IsFolder ? (rid.Name + "/") : (rid.Name + "." + rid.ResourceType.ToString()); //NOXLATE
                    var resId = folder.ResourceId + name;
                    int counter = 0;
                    while (conn.ResourceService.ResourceExists(resId))
                    {
                        counter++;

                        if (rid.IsFolder)
                        {
                            resId = folder.ResourceId + rid.Name + " (" + counter + ")/"; //NOXLATE
                        }
                        else
                        {
                            var rname = name.Substring(0, name.IndexOf(".")); //NOXLATE
                            var type = name.Substring(name.IndexOf(".")); //NOXLATE
                            rname += " (" + counter + ")"; //NOXLATE
                            resId = folder.ResourceId + rname + type;
                        }
                    }

                    if (item.ClipboardState == RepositoryItem.ClipboardAction.Copy)
                    {
                        if (item.IsFolder)
                        {
                            conn.ResourceService.CopyFolderWithReferences(item.ResourceId, resId, null, null);
                        }
                        else
                        {
                            conn.ResourceService.CopyResource(item.ResourceId, resId, false);
                        }
                    }
                    else if (item.ClipboardState == RepositoryItem.ClipboardAction.Cut)
                    {
                        if (!item.IsFolder)
                        {
                            if (omgr.IsOpen(item.ResourceId, conn))
                            {
                                sourceItemsNotMoved.Add(item.ResourceId);
                                continue;
                            }
                        }

                        //TODO: Should we prompt? That may be equivalent to saying
                        //"Shall I break your resources because you're moving" isn't it?
                        var res = conn.ResourceService.MoveResourceWithReferences(item.ResourceId, resId, null, null);
                        if (!res)
                            LoggingService.InfoFormatted(Strings.MoveFailure, item.ResourceId, resId);
                        else
                            folders[item.Parent.ResourceId] = item.Parent.ResourceId;
                    }
                }
            }
            else
            {
                var copied = ((UI.SiteExplorer)exp).CopyResourcesToFolder(
                    itemsToPaste.Select(x => new RepositoryHandle(new ResourceIdentifier(x.ResourceId), connMgr.GetConnection(x.ConnectionName))).ToArray(),
                    targetConnName,
                    folder.ResourceId);

                //Delete any originating items that were successfully cut/pasted
                foreach (var item in itemsToPaste)
                {
                    if (item.ClipboardState == RepositoryItem.ClipboardAction.Cut)
                    {
                        var origConn = connMgr.GetConnection(item.ConnectionName);
                        if (origConn.ResourceService.ResourceExists(item.ResourceId))
                        {
                            origConn.ResourceService.DeleteResource(item.ResourceId);
                        }
                    }
                }
                var refreshFolder = UI.SiteExplorer.GetCommonParent(itemsToPaste);
                exp.RefreshModel(itemsToPaste.First().ConnectionName, refreshFolder);
                exp.ExpandNode(itemsToPaste.First().ConnectionName, refreshFolder);
            }
            if (sourceItemsNotMoved.Count > 0)
                MessageService.ShowMessage(string.Format(Strings.ItemsNotMovedDueToBeingOpen, Environment.NewLine + string.Join(Environment.NewLine, sourceItemsNotMoved.ToArray())));

            ResetItems(omgr, itemsToPaste, connMgr);
            exp.RefreshModel(conn.DisplayName, folder.ResourceId);
            foreach (var f in folders.Keys)
            {
                exp.RefreshModel(conn.DisplayName, f);
            }
        }

        private static RepositoryItem[] GetItems(ClipboardService clip)
        {
            object o = clip.Get();
            if (o == null)
                return new RepositoryItem[0];
            else if (o is RepositoryItem[])
                return (RepositoryItem[])o;
            else if (o is RepositoryItem)
                return new RepositoryItem[] { (RepositoryItem)o };
            return new RepositoryItem[0];
        }
    }
}
