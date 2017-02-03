#region Disclaimer / License

// Copyright (C) 2012, Jackie Ng
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
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            var results = (DependencySet)prg.RunOperationAsync(wb, DoBackgroundWorker, items, conn);

            var downlist = new List<string>(results.DownstreamDependencies);
            downlist.Sort();
            var uplist = new List<string>(results.UpstreamDependencies);
            uplist.Sort();
            var selResources = new List<string>(results.SelectedResources);
            new ResourceDependencyListDialog(selResources, downlist, uplist).Show(wb);
        }

        private static IEnumerable<string> GetDirectUpstreamDependents(string resId, IResourceService resSvc)
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

        private static void ProcessUpstreamDependencies(ICollection<string> results, string resourceId, IResourceService resSvc)
        {
            foreach (var resId in GetDirectUpstreamDependents(resourceId, resSvc))
            {
                results.Add(resId);
                ProcessUpstreamDependencies(results, resId, resSvc);
            }
        }

        private static void ProcessDownstreamDependencies(ICollection<string> results, string resourceId, IResourceService resSvc)
        {
            var downRefs = resSvc.EnumerateResourceReferences(resourceId);
            foreach (var resId in downRefs.ResourceId)
            {
                results.Add(resId);
                ProcessDownstreamDependencies(results, resId, resSvc);
            }
        }

        private class DependencySet
        {
            public DependencySet(ICollection<string> selResources, ICollection<string> downRefs, ICollection<string> upRefs)
            {
                this.SelectedResources = selResources;
                this.DownstreamDependencies = downRefs;
                this.UpstreamDependencies = upRefs;
            }

            public ICollection<string> SelectedResources { get; }

            public ICollection<string> DownstreamDependencies { get; }

            public ICollection<string> UpstreamDependencies { get; }
        }

        private static object DoBackgroundWorker(BackgroundWorker wrk, DoWorkEventArgs e, params object[] args)
        {
            var items = (RepositoryItem[])args[0];
            var conn = (IServerConnection)args[1];

            LengthyOperationProgressCallBack cb = (o, pe) =>
            {
                wrk.ReportProgress(pe.Progress, o);
            };

            var upRefs = new HashSet<string>();
            var downRefs = new HashSet<string>();
            var selResources = new HashSet<string>();

            foreach (var ri in items)
            {
                selResources.Add(ri.ResourceId);
                ProcessUpstreamDependencies(upRefs, ri.ResourceId, conn.ResourceService);
                ProcessDownstreamDependencies(downRefs, ri.ResourceId, conn.ResourceService);
            }

            return new DependencySet(selResources, downRefs, upRefs);
        }
    }
}