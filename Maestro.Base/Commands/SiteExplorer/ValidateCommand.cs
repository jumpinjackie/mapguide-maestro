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

using Maestro.Base.Services;
using Maestro.Base.UI;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.Linq;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class ValidateCommand : BaseValidateCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var items = wb.ActiveSiteExplorer.GetSelectedResources().ToArray();
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            _conn = connMgr.GetConnection(wb.ActiveSiteExplorer.ConnectionName);

            if (items.Length > 0)
            {
                var pdlg = new ProgressDialog();
                pdlg.CancelAbortsThread = true;

                string[] args = new string[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    args[i] = items[i].ResourceId;
                }

                var issues = (ValidationIssue[])pdlg.RunOperationAsync(wb, new ProgressDialog.DoBackgroundWork(BackgroundValidate), args);

                CollectAndDisplayIssues(issues);
            }
        }
    }
}