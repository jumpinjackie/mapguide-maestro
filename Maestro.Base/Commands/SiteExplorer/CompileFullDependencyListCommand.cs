#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Base.UI;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class CompileFullDependencyListCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var siteExp = wb.ActiveSiteExplorer;
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = connMgr.GetConnection(wb.ActiveSiteExplorer.ConnectionName);

            var items = siteExp.SelectedItems;
            var prg = new ProgressDialog();
            var results = (ICollection<string>)prg.RunOperationAsync(wb, DoBackgroundWorker, items, conn);

            var list = new List<string>(results);
            list.Sort();
            new ResourceDependencyListDialog(list).Show(wb);
        }

        private static IEnumerable<string> GetDirectDependents(string resId, IResourceService resSvc)
        {
            using (var s = resSvc.GetResourceXmlData(resId))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(s);

                var matches = Utility.GetResourceIdPointers(doc);
                //Need to weed out any empty results and self-references
                return matches.Select(x => x.Value).Where(x => !string.IsNullOrEmpty(x) && x != resId);
            }
        }

        private static void ProcessDependencies(ICollection<string> results, string resourceId, IResourceService resSvc)
        {
            foreach (var resId in GetDirectDependents(resourceId, resSvc))
            {
                results.Add(resId);
                ProcessDependencies(results, resId, resSvc);
            }
        }

        private static object DoBackgroundWorker(BackgroundWorker wrk, DoWorkEventArgs e, params object[] args)
        {
            var items = (RepositoryItem[])args[0];
            var conn = (IServerConnection)args[1];

            LengthyOperationProgressCallBack cb = (o, pe) =>
            {
                wrk.ReportProgress(pe.Progress, o);
            };
            
            var result = new HashSet<string>();

            foreach (var ri in items)
            {
                result.Add(ri.ResourceId);
                ProcessDependencies(result, ri.ResourceId, conn.ResourceService);
            }

            return result;
        }
    }
}
