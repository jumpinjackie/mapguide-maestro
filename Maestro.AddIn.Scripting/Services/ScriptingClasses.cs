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
using System.Linq;
using System.Text;
using Maestro.Shared.UI;
using ICSharpCode.Core;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Base;
using Maestro.Base.Services;

namespace Maestro.AddIn.Scripting.Services
{
    /// <summary>
    /// A simplified helper class that is exposed to python scripts
    /// </summary>
    public class HostApplication
    {
        /// <summary>
        /// The main application window
        /// </summary>
        public Workbench MainWindow
        {
            get { return Workbench.Instance; }
        }

        /// <summary>
        /// The connection manager
        /// </summary>
        public ServerConnectionManager ConnectionManager
        {
            get { return ServiceRegistry.GetService<ServerConnectionManager>(); }
        }

        /// <summary>
        /// Launches the specified url
        /// </summary>
        /// <param name="url"></param>
        public void OpenUrl(string url)
        {
            var svc = ServiceRegistry.GetService<UrlLauncherService>();
            svc.OpenUrl(url);
        }

        /// <summary>
        /// Prompts for a question that requires a boolean response
        /// </summary>
        /// <param name="title"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public bool AskQuestion(string title, string question)
        {
            return MessageService.AskQuestion(question, title);
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void ShowMessage(string title, string message)
        {
            MessageService.ShowMessage(message, title);
        }

        /// <summary>
        /// Displays an exception in a dialog
        /// </summary>
        /// <param name="ex"></param>
        public void ShowError(Exception ex)
        {
            ErrorDialog.Show(ex);
        }
    }
}
