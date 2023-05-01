﻿#region Disclaimer / License

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
using Maestro.Editors.Preview;
using Maestro.Shared.UI;
using System.Diagnostics;

namespace Maestro.Base.Services
{
    /// <summary>
    /// An application-level service for launching URLs
    /// </summary>
    public class UrlLauncherService : ServiceBase, IUrlLauncherService
    {
        /// <summary>
        /// Initializes this instance
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            LoggingService.Info(Strings.Service_Init_Url_Launcher);
        }

        /// <summary>
        /// Opens the specified url using the system default web browser
        /// </summary>
        /// <param name="url">The url to open</param>
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
}