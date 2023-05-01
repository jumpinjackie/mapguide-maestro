﻿#region Disclaimer / License

// Copyright (C) 2012, Jackie Ng
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

using Maestro.Editors.Preview;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Maestro.LiveMapEditor
{
    internal static class Program
    {
        private class OurUrlLauncher : IUrlLauncherService
        {
            public void OpenUrl(string url)
            {
                var ps = new ProcessStartInfo(url)
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetCompatibleTextRenderingDefault(false);

            //Init the Maestro connection registry with additional providers from ConnectionProviders.xml
            ConnectionProviderRegistry.InitRegistry();

            //Register previewers
            var urlLauncher = new OurUrlLauncher();
            ResourcePreviewerFactory.RegisterPreviewer("Maestro.Http", new LocalMapPreviewer(new DefaultResourcePreviewer(urlLauncher), urlLauncher, null)); //NOXLATE
            //A stub previewer does nothing, but will use local map previews for applicable resources if the configuration
            //property is set
            ResourcePreviewerFactory.RegisterPreviewer("Maestro.LocalNative", new LocalMapPreviewer(new StubPreviewer(), urlLauncher, null)); //NOXLATE

            //Can't use this code, it requires a call to MgdPlatform.Initialize which we can't call indirectly :(
            /*
            //Try to tap into mg-desktop coordinate system services if possible
            try
            {
                var provEntry = ConnectionProviderRegistry.FindProvider("Maestro.Local"); //NOXLATE
                if (provEntry != null)
                {
                    string path = provEntry.AssemblyPath;
                    Assembly asm = Assembly.LoadFrom(path); //NOXLATE
                    if (asm != null)
                    {
                        Type mpuType = asm.GetType("OSGeo.MapGuide.MaestroAPI.Native.LocalNativeMpuCalculator"); //NOXLATE
                        Type catType = asm.GetType("OSGeo.MapGuide.MaestroAPI.Native.LocalNativeCoordinateSystemCatalog"); //NOXLATE

                        if (mpuType != null && catType != null)
                        {
                            IMpuCalculator calc = (IMpuCalculator)Activator.CreateInstance(mpuType);
                            ICoordinateSystemCatalog csCatalog = (ICoordinateSystemCatalog)Activator.CreateInstance(catType);

                            CsHelper.DefaultCalculator = calc;
                            CsHelper.DefaultCatalog = csCatalog;
                            Debug.WriteLine("Using mg-desktop coordinate system services where possible");
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("No mg-desktop coordinate system services to tap into. Using default services");
                }
            }
            catch (Exception ex)
            {
            }
             */

            var login = new Login.LoginDialog();
            if (login.ShowDialog() == DialogResult.OK)
            {
                var conn = login.Connection;

                //TODO: Validate connection capabilities
                Application.Run(new MainForm(conn));
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ErrorDialog.Show(e.Exception);
        }
    }
}