#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.Linq;
using System.Windows.Forms;
using Maestro.Shared.UI;
using OSGeo.MapGuide.ExtendedObjectModels;
using Maestro.Editors.Preview;
using System.Diagnostics;

namespace Maestro.LiveMapEditor
{
    static class Program
    {
        class OurUrlLauncher : IUrlLauncherService
        {
            public void OpenUrl(string url)
            {
                Process.Start(url);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ModelSetup.Initialize();
            Application.EnableVisualStyles();
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetCompatibleTextRenderingDefault(false);

            //Register previewers
            var urlLauncher = new OurUrlLauncher();
            ResourcePreviewerFactory.RegisterPreviewer("Maestro.Http", new LocalMapPreviewer(new DefaultResourcePreviewer(urlLauncher), urlLauncher)); //NOXLATE
            //A stub previewer does nothing, but will use local map previews for applicable resources if the configuration
            //property is set
            ResourcePreviewerFactory.RegisterPreviewer("Maestro.LocalNative", new LocalMapPreviewer(new StubPreviewer(), urlLauncher)); //NOXLATE

            var login = new Login.LoginDialog();
            if (login.ShowDialog() == DialogResult.OK)
            {
                var conn = login.Connection;
                //TODO: Validate connection capabilities
                Application.Run(new MainForm(conn));
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ErrorDialog.Show(e.Exception);
        }
    }
}
