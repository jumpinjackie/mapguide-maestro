#region Disclaimer / License
// Copyright (C) 2013, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class TestFeatureSourceCommand : AbstractMenuCommand
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
                try
                {
                    if (Utility.IsSuccessfulConnectionTestResult(conn.FeatureService.TestConnection(item.ResourceId)))
                        MessageBox.Show(Strings.ConnectionTestOk);
                    else
                        MessageBox.Show(string.Format(Strings.ConnectionTestFailed, false));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(Strings.ConnectionTestFailed, ex.Message));
                }
            }
        }
    }
}
