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

using ICSharpCode.Core;
using Maestro.Base.Services;
using Maestro.Base.UI;
using Maestro.Base.UI.Preferences;
using Maestro.Editors.Preview;
using Maestro.Shared.UI;
using Props = ICSharpCode.Core.PropertyService;

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
                var vcMgr = ServiceRegistry.GetService<ViewContentManager>();
                ResourcePreviewerFactory.RegisterPreviewer("Maestro.Http", new LocalMapPreviewer(new DefaultResourcePreviewer(urlLauncher), urlLauncher, vcMgr)); //NOXLATE
                ResourcePreviewerFactory.RegisterPreviewer("Maestro.Rest", new LocalMapPreviewer(new DefaultResourcePreviewer(urlLauncher), urlLauncher, vcMgr)); //NOXLATE
                //A stub previewer does nothing, but will use local map previews for applicable resources if the configuration
                //property is set
                ResourcePreviewerFactory.RegisterPreviewer("Maestro.LocalNative", new LocalMapPreviewer(new StubPreviewer(), urlLauncher, vcMgr)); //NOXLATE

                ServiceRegistry.GetService<NewItemTemplateService>().InitUserTemplates();
                var wb = Workbench.Instance;

                Themes.CurrentTheme = Props.Get<string>(ConfigProperties.SelectedTheme, ConfigProperties.DefaultSelectedTheme);
                var themeToApply = Themes.Get(Themes.CurrentTheme);
                if (themeToApply != null)
                    wb.ApplyTheme(themeToApply);

                wb.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OnWorkbenchClosing);
                wb.Text = "MapGuide Maestro"; //NOXLATE
                
                if (Props.Get(ConfigProperties.ShowMessages, true))
                    vcMgr.OpenContent<MessageViewer>(ViewRegion.Bottom);

                if (Props.Get(ConfigProperties.ShowOutboundRequests, true))
                    vcMgr.OpenContent<OutboundRequestViewer>(ViewRegion.Bottom);

                LoginCommand.RunInternal(true);
            };
        }

        private void OnWorkbenchClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = Maestro.Base.Commands.SiteExplorer.DisconnectCommand.CancelDisconnect();
        }
    }
}