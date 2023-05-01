﻿#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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
using Maestro.Editors.Common;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using System.IO;
using System.Linq;
using System.Xml;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class RepointCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = connMgr.GetConnection(exp.ConnectionName);
            if (!IsValid(conn))
            {
                MessageService.ShowError(Strings.ConnectionDoesNotSupportRequiredInterfaces);
                return;
            }
            var sel = exp.GetSelectedResources().ToArray();
            if (sel.Length == 1)
            {
                var selected = sel[0];
                var resId = new ResourceIdentifier(selected.ResourceId);
                if (resId.ResourceType != ResourceTypes.WebLayout.ToString() &&
                    resId.ResourceType != ResourceTypes.ApplicationDefinition.ToString() &&
                    resId.ResourceType != ResourceTypes.LoadProcedure.ToString())
                {
                    DoRepointResource(wb, conn, resId);
                }
                else
                {
                    MessageService.ShowMessage(Strings.ResourceNotRepointable);
                }
            }
        }

        private bool IsValid(IServerConnection conn)
        {
            return conn.Capabilities.SupportsResourceReferences;
        }

        private static void DoRepointResource(Workbench wb, IServerConnection conn, ResourceIdentifier resId)
        {
            var diag = new RepointerDialog(resId, conn);
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string srcId = diag.Source;
                string dstId = diag.Target;

                var deps = diag.Dependents;

                ProgressDialog.DoBackgroundWork worker = (wk, e, args) =>
                {
                    int updated = 0;
                    int total = deps.Count;
                    wk.ReportProgress(0, Strings.ProgressUpdatingReferences);
                    foreach (var dep in deps)
                    {
                        using (var stream = conn.ResourceService.GetResourceXmlData(dep))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(stream);
                            bool changed = Utility.ReplaceResourceIds(doc, srcId, dstId);
                            if (changed)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    doc.Save(ms);
                                    ms.Position = 0L; //Rewind
                                    conn.ResourceService.SetResourceXmlData(dep, ms);
                                }
                                updated++;
                                wk.ReportProgress((updated / total) * 100);
                            }
                        }
                    }
                    return updated;
                };
                var prd = new ProgressDialog();
                int result = (int)prd.RunOperationAsync(wb, worker);
                MessageService.ShowMessage(string.Format(Strings.ResourcesRepointed, result, dstId));
            }
        }
    }
}