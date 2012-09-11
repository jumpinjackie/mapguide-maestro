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
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Base.Commands
{
    internal class CopyCommand : ClipboardCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var clip = ServiceRegistry.GetService<ClipboardService>();
            var omgr = ServiceRegistry.GetService<OpenResourceManager>();
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();

            ResetClipboardedItems(clip, omgr, connMgr);

            if (exp.SelectedItems.Length > 0)
            {
                var items = new List<RepositoryItem>();
                foreach (var item in exp.SelectedItems)
                {
                    item.ClipboardState = RepositoryItem.ClipboardAction.Copy;
                    items.Add(item);
                }
                clip.Put(items.ToArray());
                LoggingService.InfoFormatted(Strings.ItemsCopied, items.Count);
            }
        }
    }

    internal abstract class ClipboardCommand : AbstractMenuCommand
    {
        protected static void ResetClipboardedItems(ClipboardService clip, OpenResourceManager omgr, ServerConnectionManager connMgr)
        {
            //Reset state of clipboarded items
            if (clip.HasContent())
            {
                object o = clip.Get();
                var r = o as RepositoryItem;
                var rs = o as RepositoryItem[];
                if (r != null)
                {
                    var conn = connMgr.GetConnection(r.ConnectionName);
                    ResetItem(omgr, r, conn);
                }
                else if (rs != null)
                {
                    ResetItems(omgr, rs, connMgr);
                }
            }
        }

        protected static void ResetItems(OpenResourceManager omgr, IEnumerable<RepositoryItem> rs, ServerConnectionManager connMgr)
        {
            foreach (var ri in rs)
            {
                var conn = connMgr.GetConnection(ri.ConnectionName);
                ResetItem(omgr, ri, conn);
            }
        }

        protected static void ResetItem(OpenResourceManager omgr, RepositoryItem ri, IServerConnection conn)
        {
            ri.Reset();
            if (omgr.IsOpen(ri.ResourceId, conn))
            {
                ri.IsOpen = true;
                var ed = omgr.GetOpenEditor(ri.ResourceId, conn);
                if (ed.IsDirty)
                    ri.IsDirty = true;
            }
        }
    }
}
