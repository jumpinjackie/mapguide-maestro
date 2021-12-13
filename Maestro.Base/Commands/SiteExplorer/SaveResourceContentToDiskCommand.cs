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
using Maestro.Base.Services;
using Maestro.Base.UI;
using Maestro.Shared.UI;
using System.Linq;

namespace Maestro.Base.Commands.SiteExplorer
{
    internal class SaveResourceContentToDiskCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            var wb = Workbench.Instance;
            var exp = wb.ActiveSiteExplorer;
            var connMgr = ServiceRegistry.GetService<ServerConnectionManager>();
            var conn = connMgr.GetConnection(exp.ConnectionName);
            var sel = exp.GetSelectedResources().ToArray();
            if (sel.Length == 1)
            {
                using (var diag = DialogFactory.SaveFile())
                {
                    if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        var res = conn.ResourceService.GetResource(sel[0].ResourceId);
                        System.IO.File.WriteAllText(diag.FileName, res.Serialize());
                        MessageService.ShowMessage(string.Format(Strings.SavedResource, diag.FileName));
                    }
                }
            }
        }
    }
}