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

using Maestro.Editors;
using Maestro.Login;
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MaestroFsPreview
{
    internal static class Program
    {
        private class EditorServiceImpl : ResourceEditorServiceBase
        {
            public EditorServiceImpl(IServerConnection conn)
                : base("Session://", conn)
            {
            }

            public override IPreviewUrl[] GetAlternateFlexibleLayoutPreviewUrls(string resourceID, string locale)
            {
                return Array.Empty<IPreviewUrl>();
            }

            public override IPreviewUrl[] GetAlternateWebLayoutPreviewUrls(string resourceID, string locale)
            {
                return Array.Empty<IPreviewUrl>();
            }

            public override void OpenResource(string resourceId)
            {
                throw new NotImplementedException();
            }

            public override void OpenUrl(string url)
            {
                Process.Start(url);
            }

            public override void RequestRefresh(string folderId)
            {
                throw new NotImplementedException();
            }

            public override void RequestRefresh()
            {
                throw new NotImplementedException();
            }

            public override void RunProcess(string processName, params string[] args)
            {
                throw new NotImplementedException();
            }

            public override string SelectUnmanagedData(string startPath, System.Collections.Specialized.NameValueCollection fileTypes)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var parser = new ArgumentParser(args);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            PreferredSiteList.InitCulture();
            //Init the Maestro connection registry with additional providers from ConnectionProviders.xml
            ConnectionProviderRegistry.InitRegistry();
            IServerConnection conn = null;
            if (!parser.IsDefined(CommandLineArguments.Provider) || !parser.IsDefined(CommandLineArguments.Session))
            {
                var login = new LoginDialog();
                if (login.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    conn = login.Connection;
                }
            }
            else
            {
                string prov = parser.GetValue(CommandLineArguments.Provider);

                conn = ConnectionProviderRegistry.CreateConnection(prov, parser.GetAllArgumentsWithValues());
            }

            if (conn == null)
                return;

            Application.Run(new MainForm(new EditorServiceImpl(conn)));
        }
    }
}