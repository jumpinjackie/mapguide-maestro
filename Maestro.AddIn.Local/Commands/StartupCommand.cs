#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Local;
using System.IO;
using Maestro.Base.Services;
using Maestro.AddIn.Local.Services;
using OSGeo.MapGuide;
using Maestro.Editors.Preview;

namespace Maestro.AddIn.Local.Commands
{
    public class StartupCommand : AbstractCommand
    {
        public override void Run()
        {
            ResourceService.RegisterNeutralImages(Properties.Resources.ResourceManager);
            ResourceService.RegisterNeutralStrings(Strings.ResourceManager);

            if (!Platform.IsRunningOnMono)
            {
                ResourcePreviewerFactory.RegisterPreviewer(LocalConnection.PROVIDER_NAME, new LocalPreviewer());

                System.Windows.Forms.Application.ApplicationExit += new EventHandler(OnAppExit);
            }
            else
            {
                LoggingService.Info("Skipping local connection provider registration because I am guessing we're running Mono on Linux/Mac"); //LOCALIZEME
            }
        }

        void OnAppExit(object sender, EventArgs e)
        {
            try
            {
                MgdPlatform.Terminate();
            }
            catch (MgException ex)
            {
                ex.Dispose();
            }
        }
    }
}
