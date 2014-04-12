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
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Login;
using Maestro.Editors;
using System.Diagnostics;

namespace MaestroFsPreview
{
    static class Program
    {
        class EditorServiceImpl : ResourceEditorServiceBase
        {
            public EditorServiceImpl(IServerConnection conn)
                : base("Session://", conn)
            {

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
        static void Main(string [] args)
        {
            var parser = new ArgumentParser(args);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            PreferredSiteList.InitCulture();
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
