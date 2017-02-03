#region Disclaimer / License

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

using ICSharpCode.Core;
using Maestro.Packaging;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Windows.Forms;

namespace Maestro.Base.Services.DragDropHandlers
{
    internal class PackageFileHandler : IDragDropHandler
    {
        public string HandlerAction
        {
            get { return Strings.MgpHandlerAction; }
        }

        private readonly string[] extensions = { ".mgp" }; //NOXLATE

        public bool HandleDrop(IServerConnection conn, string file, string folderId)
        {
            try
            {
                if (!MessageService.AskQuestion(Strings.ConfirmLoadPackage, Strings.Confirm))
                    return false;

                var wb = Workbench.Instance;
                var exp = wb.ActiveSiteExplorer;
                var optDiag = new PackageUploadOptionDialog();
                optDiag.ShowDialog();
                DialogResult res;
                if (optDiag.Method == PackageUploadMethod.Transactional)
                {
                    res = PackageProgress.UploadPackage(wb, conn, file);
                }
                else
                {
                    res = PackageProgress.StartNonTransactionalUploadLoop(wb, conn, file);
                }
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    exp.RefreshModel(conn.DisplayName);
                }
                return false; //Already refreshed if successful
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ex);
                return false;
            }
        }

        public override string ToString()
        {
            return Strings.MgpHandlerAction;
        }

        public bool CanHandleFileExtension(string fileExtension)
        {
            return HandlerUtil.ExtensionInList(extensions, fileExtension);
        }
    }
}