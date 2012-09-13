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
using Maestro.Base.UI.Preferences;
using System.IO;
using Maestro.Shared.UI;
using System.Diagnostics;

namespace Maestro.Base.Commands
{
    internal class LocalFeatureSourcePreviewCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            string exe = PropertyService.Get(ConfigProperties.LocalFsPreviewPath, string.Empty);

            if (!File.Exists(exe))
            {
                using (var dlg = DialogFactory.OpenFile())
                {
                    dlg.Title = string.Format(Strings.LocateExecutable, "MaestroFsPreview.exe"); //NOXLATE
                    dlg.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickExe, "exe"); //NOXLATE
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        exe = dlg.FileName;
                        PropertyService.Set(ConfigProperties.LocalFsPreviewPath, exe);
                    }
                }
            }

            var procInfo = new ProcessStartInfo(exe);
            procInfo.WorkingDirectory = Path.GetDirectoryName(exe);
            var proc = Process.Start(procInfo);
        }
    }
}
