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

            ResetClipboardedItems(clip, omgr);

            if (exp.SelectedItems.Length > 0)
            {
                var items = new List<RepositoryItem>();
                foreach (var item in exp.SelectedItems)
                {
                    item.ClipboardState = RepositoryItem.ClipboardAction.Copy;
                    items.Add(item);
                }
                clip.Put(items.ToArray());
                LoggingService.InfoFormatted(Properties.Resources.ItemsCopied, items.Count);
            }
        }
    }

    internal abstract class ClipboardCommand : AbstractMenuCommand
    {
        protected static void ResetClipboardedItems(ClipboardService clip, OpenResourceManager omgr)
        {
            //Reset state of clipboarded items
            if (clip.HasContent())
            {
                object o = clip.Get();
                var r = o as RepositoryItem;
                var rs = o as RepositoryItem[];
                if (r != null)
                {
                    ResetItem(omgr, r);
                }
                else if (rs != null)
                {
                    ResetItems(omgr, rs);
                }
            }
        }

        protected static void ResetItems(OpenResourceManager omgr, IEnumerable<RepositoryItem> rs)
        {
            foreach (var ri in rs)
            {
                ResetItem(omgr, ri);
            }
        }

        protected static void ResetItem(OpenResourceManager omgr, RepositoryItem ri)
        {
            ri.Reset();
            if (omgr.IsOpen(ri.ResourceId))
            {
                ri.IsOpen = true;
                var ed = omgr.GetOpenEditor(ri.ResourceId);
                if (ed.IsDirty)
                    ri.IsDirty = true;
            }
        }
    }
}
