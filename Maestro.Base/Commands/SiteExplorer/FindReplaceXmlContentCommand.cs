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
                        if (omgr.IsOpen(item.ResourceId))
                        {
                            omgr.CloseEditors(item.ResourceId, false);
                            //Still open. Must've said no
                            if (omgr.IsOpen(item.ResourceId))
                            {
                                LoggingService.Info(string.Format(Properties.Resources.SkippingResource, item.ResourceId));
                                continue;
                            }
                        }
                        
                        //Do the find/replace
                        using (var s = conn.ResourceService.GetResourceXmlData(item.ResourceId))
                        {
                            using (var sr = new StreamReader(s))
                            {
                                string xml = sr.ReadToEnd();
                                xml = xml.Replace(diag.FindToken, diag.ReplaceToken);
                                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                                {
                                    conn.ResourceService.SetResourceXmlData(item.ResourceId, ms);
                                }
                                //Re-open in XML editor for user review
                                omgr.Open(item.ResourceId, conn, true, wb.ActiveSiteExplorer);
                                replaced++;
                            }
                        }
                    }
                }
            }
        }
    }
}
