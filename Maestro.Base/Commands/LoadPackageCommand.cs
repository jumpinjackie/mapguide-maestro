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
using Maestro.Shared.UI;
using Maestro.Packaging;
using System.Windows.Forms;

namespace Maestro.Base.Commands
{
    internal class LoadPackageCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = connMgr.GetConnection(exp.ConnectionName);

            using (var open = DialogFactory.OpenFile())
            {
                open.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickMgp, "mgp"); //NOXLATE
                if (open.ShowDialog(wb) == System.Windows.Forms.DialogResult.OK)
                {
                    var optDiag = new PackageUploadOptionDialog();
                    optDiag.ShowDialog();
                    DialogResult res;
                    if (optDiag.Method == PackageUploadMethod.Transactional)
                    {
                        res = PackageProgress.UploadPackage(wb,
                                                            conn,
                                                            open.FileName);
                    }
                    else
                    {
                        res = PackageProgress.StartNonTransactionalUploadLoop(wb, conn, open.FileName);
                    }
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        exp.RefreshModel(conn.DisplayName);
                    }
                }
            }
        }
    }
}
