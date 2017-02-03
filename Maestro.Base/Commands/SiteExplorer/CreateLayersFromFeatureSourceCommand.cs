#region Disclaimer / License

// Copyright (C) 2013, Jackie Ng
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
using Maestro.Editors.FeatureSource;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System.Collections.Generic;
using System.ComponentModel;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class CreateLayersFromFeatureSourceCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            if (exp != null && exp.SelectedItems != null && exp.SelectedItems.Length == 1)
            {
                var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
                var conn = connMgr.GetConnection(wb.ActiveSiteExplorer.ConnectionName);
                var item = exp.SelectedItems[0];

                var diag = new CreateLayersFromFeatureSourceDialog(conn, item.ResourceId);
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    CreateLayers(conn, diag.FeatureSource, diag.TargetFolder, diag.FeatureClasses);
                }
            }
        }

        private static object DoBackgroundWorker(BackgroundWorker wrk, DoWorkEventArgs e, params object[] args)
        {
            var items = (List<ClassDefinition>)args[0];
            var conn = (IServerConnection)args[1];
            string featureSource = (string)args[2];
            string targetFolder = (string)args[3];

            LengthyOperationProgressCallBack cb = (o, pe) =>
            {
                wrk.ReportProgress(pe.Progress, o);
            };

            var result = new HashSet<string>();
            int processed = 0;
            foreach (var cls in items)
            {
                var lyrId = Utility.CreateDefaultLayer(conn, featureSource, cls, targetFolder);
                if (lyrId != null)
                    result.Add(lyrId);

                processed++;
                cb(null, new LengthyOperationProgressArgs(string.Empty, (int)((processed / items.Count) * 100)));
            }

            return result;
        }

        private static void CreateLayers(IServerConnection conn, string featureSource, string targetFolder, string[] featureClasses)
        {
            var wb = Workbench.Instance;
            List<ClassDefinition> classes = new List<ClassDefinition>();
            foreach (var clsName in featureClasses)
            {
                classes.Add(conn.FeatureService.GetClassDefinition(featureSource, clsName));
            }
            var prg = new ProgressDialog();
            var results = (ICollection<string>)prg.RunOperationAsync(wb, DoBackgroundWorker, classes, conn, featureSource, targetFolder);
            var exp = wb.ActiveSiteExplorer;
            if (exp != null)
            {
                exp.RefreshModel(exp.ConnectionName, targetFolder);
                exp.ExpandNode(exp.ConnectionName, targetFolder);
            }
        }
    }
}