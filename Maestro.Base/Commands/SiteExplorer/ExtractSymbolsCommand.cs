#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Editors.SymbolDefinition;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource.Conversion;
using OSGeo.MapGuide.ObjectModels;
using System.Collections.Generic;
using System.ComponentModel;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class ExtractSymbolsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            if (wb != null)
            {
                if (wb.ActiveSiteExplorer != null)
                {
                    var items = wb.ActiveSiteExplorer.SelectedItems;
                    if (items.Length == 1)
                    {
                        var it = items[0];
                        if (it.ResourceType == ResourceTypes.SymbolLibrary.ToString())
                        {
                            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
                            var conn = connMgr.GetConnection(wb.ActiveSiteExplorer.ConnectionName);
                            var diag = new ExtractSymbolLibraryDialog(conn, it.ResourceId);
                            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                string symbolLib = diag.SymbolLibrary;
                                string targetFolder = diag.TargetFolder;
                                IEnumerable<string> symbols = diag.SelectedSymbols;

                                DoSymbolExtraction(conn, symbolLib, symbols, targetFolder);
                            }
                        }
                    }
                }
            }
        }

        private object SymbolExtractionWorker(BackgroundWorker worker, DoWorkEventArgs e, params object[] args)
        {
            int processed = 0;
            IServerConnection conn = (IServerConnection)args[0];
            string symbolLib = (string)args[1];
            List<string> symbols = (List<string>)args[2];
            string targetFolder = (string)args[3];

            ImageSymbolConverter conv = new ImageSymbolConverter(conn, symbolLib);
            conv.ExtractSymbols(targetFolder, symbols, (count, total) =>
            {
                processed = count;
                int pc = (int)((double)count / (double)total);
                worker.ReportProgress(pc);
            });

            return processed;
        }

        private void DoSymbolExtraction(IServerConnection conn, string symbolLib, IEnumerable<string> symbols, string targetFolder)
        {
            var wb = Workbench.Instance;
            var list = new List<string>(symbols);
            var diag = new ProgressDialog();
            var worker = new ProgressDialog.DoBackgroundWork(SymbolExtractionWorker);
            diag.RunOperationAsync(wb, worker, conn, symbolLib, list, targetFolder);
        }
    }
}