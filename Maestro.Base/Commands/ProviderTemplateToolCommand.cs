#region Disclaimer / License

// Copyright (C) 2019, Jackie Ng
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
using Maestro.Base.UI.Preferences;
using Maestro.Shared.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestro.Base.Commands
{
    internal class ProviderTemplateToolCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            RunTool();
        }

        internal static void RunTool()
        {
            string exe = PropertyService.Get(ConfigProperties.ProviderToolPath, ConfigProperties.DefaultProviderToolPath); //NOXLATE

            if (!File.Exists(exe))
            {
                using (var dlg = DialogFactory.OpenFile())
                {
                    dlg.Title = string.Format(Strings.LocateExecutable, "ProviderTemplate.exe"); //NOXLATE
                    dlg.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickExe, "exe"); //NOXLATE
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        exe = dlg.FileName;
                        PropertyService.Set(ConfigProperties.ProviderToolPath, exe);
                    }
                }
            }

            var procInfo = new ProcessStartInfo(exe);
            procInfo.WorkingDirectory = Path.GetDirectoryName(exe);
            var proc = Process.Start(procInfo);
        }
    }
}
