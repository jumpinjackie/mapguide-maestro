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
using OSGeo.MapGuide.MaestroAPI.Resource;
using Maestro.Base.UI;
using System.Diagnostics;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using System.Xml;
using System.IO;

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

            if (exp.SelectedItems.Length == 1)
            {
                var selected = exp.SelectedItems[0];
                var resId = new ResourceIdentifier(selected.ResourceId);
                if (resId.ResourceType != ResourceTypes.WebLayout &&
                    resId.ResourceType != ResourceTypes.ApplicationDefinition &&
                    resId.ResourceType != ResourceTypes.LoadProcedure)
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
            var diag = new RepointerDialog(resId, conn.ResourceService);
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
