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
using System.IO;
using ICSharpCode.Core;
using Maestro.Packaging;

namespace Maestro.Base.Services.DragDropHandlers
{
    public class PackageFileHandler : IDragDropHandler
    {
        public string HandlerAction
        {
            get { return Properties.Resources.MgpHandlerAction; }
        }

        string[] extensions = { ".mgp" };

        public string[] FileExtensions
        {
            get { return extensions; }
        }

        public bool HandleDrop(string file, string folderId)
        {
            try
            {
                if (!MessageService.AskQuestion(Properties.Resources.ConfirmLoadPackage, Properties.Resources.Confirm))
                    return false;

                var wb = Workbench.Instance;
                var exp = wb.ActiveSiteExplorer;
                var mgr = ServiceRegistry.GetService<ServerConnectionManager>();
                var conn = mgr.GetConnection(exp.ConnectionName);
                var res = PackageProgress.UploadPackage(
                        wb,
                        conn,
                        file);

                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    exp.RefreshModel();
                }
                return false; //Already refreshed if successful
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
                return false;
            }
        }
    }
}
