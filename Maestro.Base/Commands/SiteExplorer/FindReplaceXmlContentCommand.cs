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
using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Editors.Common;
using System.IO;
using Maestro.Base.Editor;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class FindReplaceXmlContentCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var omgr = ServiceRegistry.GetService<OpenResourceManager>();
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = connMgr.GetConnection(exp.ConnectionName);
            if (exp.SelectedItems.Length > 0)
            {
                int replaced = 0;
                var diag = new FindReplaceDialog();
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (var item in exp.SelectedItems)
                    {
                        if (item.IsFolder)
                            continue;

                        //To maintain resource integrity, we don't modify any open resources. So we
                        //ask if they want to close down first. If they say no, omit this resource from
                        //the find/replace
                        if (omgr.IsOpen(item.ResourceId, conn))
                        {
                            omgr.CloseEditors(conn, item.ResourceId, false);
                            //Still open. Must've said no
                            if (omgr.IsOpen(item.ResourceId, conn))
                            {
                                LoggingService.Info(string.Format(Strings.SkippingResource, item.ResourceId));
                                continue;
                            }
                        }

                        //Re-open in XML editor for user review
                        var ed = (XmlEditor)omgr.Open(item.ResourceId, conn, true, wb.ActiveSiteExplorer);

                        //Do the find/replace. Dirty state would be triggered if any replacements were made
                        //It is then up to the user to review the change and decide whether to save or not
                        ed.FindAndReplace(diag.FindToken, diag.ReplaceToken);

                        replaced++;
                    }
                }
            }
        }
    }
}
