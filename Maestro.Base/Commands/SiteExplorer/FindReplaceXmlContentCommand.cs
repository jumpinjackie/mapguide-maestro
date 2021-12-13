#region Disclaimer / License

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
using Maestro.Base.Editor;
using Maestro.Base.Services;
using Maestro.Base.UI;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.Common;
using System;

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
                Action<string> DoRun = (string resourceId) =>
                {
                    //To maintain resource integrity, we don't modify any open resources. So we
                    //ask if they want to close down first. If they say no, omit this resource from
                    //the find/replace
                    if (omgr.IsOpen(resourceId, conn))
                    {
                        omgr.CloseEditors(conn, resourceId, false);
                        //Still open. Must've said no
                        if (omgr.IsOpen(resourceId, conn))
                        {
                            LoggingService.Info(string.Format(Strings.SkippingResource, resourceId));
                            return;
                        }
                    }

                    //Re-open in XML editor for user review
                    var ed = (XmlEditor)omgr.Open(resourceId, conn, true, wb.ActiveSiteExplorer);

                    //Do the find/replace. Dirty state would be triggered if any replacements were made
                    //It is then up to the user to review the change and decide whether to save or not
                    ed.FindAndReplace(diag.FindToken, diag.ReplaceToken);

                    replaced++;
                };
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (var item in exp.GetSelectedResources())
                    {
                        if (item.IsFolder)
                        {
                            var resources = conn.ResourceService.GetRepositoryResources(item.ResourceId);
                            foreach (IRepositoryItem resource in resources.Items)
                            {
                                if (resource.ResourceType != "Folder")
                                {
                                    DoRun(resource.ResourceId);
                                }
                            }
                        }
                        else
                        {
                            DoRun(item.ResourceId);
                        }
                    }
                }
            }
        }

    }
}