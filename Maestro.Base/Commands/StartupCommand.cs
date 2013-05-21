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
using System.Reflection;
using Maestro.Base.Services;
using Maestro.Base.Events;
using Maestro.Base.UI;
using Maestro.Base.Editor;
using OSGeo.MapGuide.ObjectModels.WebLayout;
using Props = ICSharpCode.Core.PropertyService;
using Maestro.Base.UI.Preferences;
using System.Windows.Forms;
using Maestro.Shared.UI;
using Maestro.Editors.Preview;

namespace Maestro.Base.Commands
{
    internal class StartupCommand : AbstractCommand
    {
        public override void Run()
        {
            ResourceService.RegisterNeutralImages(Properties.Resources.ResourceManager);
            ResourceService.RegisterNeutralStrings(Strings.ResourceManager);
            
            Workbench.WorkbenchInitialized += (sender, e) =>
            {
                PreviewSettings.UseAjaxViewer = PropertyService.Get(ConfigProperties.PreviewViewerType, "AJAX") == "AJAX"; //NOXLATE
                PreviewSettings.UseLocalPreview = PropertyService.Get(ConfigProperties.UseLocalPreview, ConfigProperties.DefaultUseLocalPreview);

                var urlLauncher = ServiceRegistry.GetService<UrlLauncherService>();
                ResourcePreviewerFactory.RegisterPreviewer("Maestro.Http", new LocalMapPreviewer(new DefaultResourcePreviewer(urlLauncher), urlLauncher)); //NOXLATE
                //A stub previewer does nothing, but will use local map previews for applicable resources if the configuration
                //property is set
                ResourcePreviewerFactory.RegisterPreviewer("Maestro.LocalNative", new LocalMapPreviewer(new StubPreviewer(), urlLauncher)); //NOXLATE

                ServiceRegistry.GetService<NewItemTemplateService>().InitUserTemplates();
                var wb = Workbench.Instance;
                wb.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OnWorkbenchClosing);
                wb.Text = "MapGuide Maestro"; //NOXLATE

                var mgr = ServiceRegistry.GetService<ViewContentManager>();
                
                if (Props.Get(ConfigProperties.ShowMessages, true))
                    mgr.OpenContent<MessageViewer>(ViewRegion.Bottom);

                if (Props.Get(ConfigProperties.ShowOutboundRequests, true))
                    mgr.OpenContent<OutboundRequestViewer>(ViewRegion.Bottom);

                TipOfTheDayDialog.FirstTimeOpen();
                new LoginCommand().Run();
            };
        }

        void OnWorkbenchClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = Maestro.Base.Commands.SiteExplorer.DisconnectCommand.CancelDisconnect();   
        }
    }
}
